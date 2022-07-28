using System.Diagnostics;
using System.Runtime.InteropServices;
using Microsoft.Win32;

namespace SnapAssistForCentering
{
    public partial class Indicator : Form
    {
        public Indicator()
        {
            InitializeComponent();

            hwndDragging = IntPtr.Zero;
            status = Status.IDLE;
            procDelegate = new WinEventDelegate(WinEventProc);
            IntPtr hook = SetWinEventHook(EVENT_SYSTEM_MOVESIZESTART, EVENT_SYSTEM_MOVESIZEEND, IntPtr.Zero, procDelegate, 0, 0, WINEVENT_OUTOFCONTEXT);
            controller = new SizeController(this);
            chkRunAtStartup_GetState();
            GetRestartIntervalState();
        }

        private void Indicator_Load(object sender, EventArgs e)
        {
            Acrylic.Enable(Handle);
        }

        private void Indicator_Shown(object? sender, EventArgs e)
        {
            Shown -= Indicator_Shown;
            controller.To(Screen.PrimaryScreen.WorkingArea, SensorSize(), SIZE_ANIMATION_TIME);
            Hide();
        }

        protected override bool ShowWithoutActivation => true;

        private void chkRunAtStartup_GetState()
        {
            RegistryKey? key = Registry.CurrentUser.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", true);
            if (key == null)
            {
                chkRunAtStartup.Enabled = false;
                chkRunAtStartup.Checked = false;
            }
            else
            {
                chkRunAtStartup.Enabled = true;
                var value = key.GetValue(Application.ProductName);
                if (value == null)
                {
                    chkRunAtStartup.Checked = false;
                }
                else if (value.ToString() == Application.ExecutablePath)
                {
                    chkRunAtStartup.Checked = true;
                }
                else
                {
                    chkRunAtStartup.Checked = false;
                }
            }
        }

        private static Screen? CursorScreen()
        {
            foreach (Screen screen in Screen.AllScreens)
            {
                if (screen.Bounds.Contains(Cursor.Position))
                {
                    return screen;
                }
            }
            return null;
        }

        private static Rectangle Centering(Rectangle area, Size size)
        {
            return new Rectangle(
                area.X + area.Width / 2 - size.Width / 2,
                area.Y + area.Height / 2 - size.Height / 2,
                size.Width, size.Height);
        }

        private static Point LeftTop(Rectangle rectangle)
        {
            return new Point(rectangle.Left, rectangle.Top);
        }

        private static Size RectangleSize(Rectangle rectangle)
        {
            return new Size(rectangle.Width, rectangle.Height);
        }

        readonly WinEventDelegate procDelegate;
        private IntPtr hwndDragging;

        private Status status;
        enum Status
        {
            IDLE = 0, // Not currently
            NON_ACTIVATED = 1, // Dragging with cursor never leaving sensor area, Indicator is non-activated and centering is disabled
            ACTIVATED = 2 // Dragging with cursor once left sensor area, Indicator is activated and centering is enabled
        }

        private bool sensing = false; // If the dragging window is onto activated Indicator

        const uint WINEVENT_OUTOFCONTEXT = 0;
        const uint EVENT_SYSTEM_MOVESIZESTART = 0x000A;
        const uint EVENT_SYSTEM_MOVESIZEEND = 0x000B;
        const int SW_SHOW = 5;
        const int SENSOR_WIDTH = 100;
        const int SENSOR_HEIGHT = 100;
        const int SENSOR_WIDTH_SENSING = 300;
        const int SENSOR_HEIGHT_SENSING = 300;
        const int SIZE_ANIMATION_TIME = 100;

        private readonly SizeController controller;

        private Size SensorSize()
        {
            if (sensing)
            {
                return new Size(SENSOR_WIDTH_SENSING, SENSOR_HEIGHT_SENSING);
            }
            return new Size(SENSOR_WIDTH, SENSOR_HEIGHT);
        }

        void WinEventProc(IntPtr hWinEventHook, uint eventType, IntPtr hwnd, int idObject, int idChild, uint dwEventThread, uint dwmsEventTime)
        {
            if (eventType == EVENT_SYSTEM_MOVESIZESTART)
            {
                hwndDragging = hwnd;
                status = Status.NON_ACTIVATED;
                tmrCursor.Start();

                Screen? screen = CursorScreen();
                if (screen == null) return;

                controller.To(screen.WorkingArea, SensorSize(), SIZE_ANIMATION_TIME);
            }
            else if (eventType == EVENT_SYSTEM_MOVESIZEEND)
            {
                tmrCursor.Stop();
                Hide();

                Screen? screen = CursorScreen();
                if (screen == null) return;

                Rectangle sensor = Centering(screen.WorkingArea, SensorSize());

                if (status == Status.ACTIVATED && sensor.Contains(Cursor.Position))
                {
                    Rectangle rect = GetWindowRectangle(hwndDragging);
                    Rectangle destination = Centering(screen.WorkingArea, RectangleSize(rect));
                    SetWindowPosition(hwndDragging, destination, SW_SHOW);
                }

                hwndDragging = IntPtr.Zero;
                status = Status.IDLE;
            }
        }

        class SizeController
        {
            public Form form;
            public System.Windows.Forms.Timer timer;

            private Rectangle area;
            private Size target;
            private Size delta;
            private int countdown;

            public SizeController(Form form)
            {
                this.form = form;
                timer = new System.Windows.Forms.Timer();
                timer.Tick += new System.EventHandler(Animate);
                timer.Interval = 10;
                target = form.Size;
                delta = new Size();
                area = new Rectangle();
            }

            public void To(Rectangle area, Size size, int milliseconds)
            {
                this.area = area;
                delta = (size - form.Size) * timer.Interval / milliseconds;
                target = size;
                countdown = milliseconds;
                timer.Start();
            }

            private void Animate(object? sender, EventArgs e)
            {
                Console.WriteLine(delta);
                form.Size += delta;
                form.Location = LeftTop(Centering(area, form.Size));
                countdown -= 1;
                if (countdown <= 0)
                {
                    form.Size = target;
                    countdown = 0;
                    timer.Stop();
                }
            }
        }

        delegate void WinEventDelegate(IntPtr hWinEventHook, uint eventType, IntPtr hwnd, int idObject, int idChild, uint dwEventThread, uint dwmsEventTime);

        [DllImport("user32.dll")]
        static extern IntPtr SetWinEventHook(uint eventMin, uint eventMax, IntPtr hmodWinEventProc, WinEventDelegate lpfnWinEventProc,
            uint idProcess, uint idThread, uint dwFlags);

        [DllImport("user32.dll")]
        static extern bool UnhookWinEvent(IntPtr hWinEventHook);

        [DllImport("user32.dll")]
        static extern int GetWindowRect(IntPtr hWnd, out Rectangle rect);
        static Rectangle GetWindowRectangle(IntPtr hwnd)
        {
            Rectangle rectangle = new();
            GetWindowRect(hwnd, out rectangle);
            // The returned Width and Height are actually Right and Bottom
            rectangle.Width = rectangle.Width - rectangle.Left;
            rectangle.Height = rectangle.Height - rectangle.Top;
            return rectangle;
        }

        [DllImport("user32.dll")]
        static extern bool SetWindowPos(IntPtr hWnd, IntPtr hwndInsertAfter, int x, int y, int cx, int cy, uint flags);
        static private bool SetWindowPosition(IntPtr hwnd, Rectangle location, uint flags, IntPtr? hwndInsertAfter = null)
        {
            if (hwndInsertAfter == null)
            {
                hwndInsertAfter = IntPtr.Zero;
            }
            return SetWindowPos(hwnd, (IntPtr)hwndInsertAfter, location.X, location.Y, location.Width, location.Height, flags);
        }
        static private bool SetWindowPosition(IntPtr hwnd, Point location, Size size, uint flags, IntPtr? hwndInsertAfter = null)
        {
            if (hwndInsertAfter == null)
            {
                hwndInsertAfter = IntPtr.Zero;
            }
            return SetWindowPos(hwnd, (IntPtr)hwndInsertAfter, location.X, location.Y, size.Width, size.Height, flags);
        }

        public static class Acrylic
        {

            public const int WS_SYSMENU = 0x80000;
            public const int WS_EX_LAYERED = 0x00080000;

            [DllImport("user32.dll")]
            internal static extern IntPtr SetWindowCompositionAttribute(IntPtr hwnd, ref WindowCompositionAttributeData data);

            internal enum AccentState
            {
                ACCENT_DISABLED = 0,
                ACCENT_GRADIENT = 1,
                ACCENT_TRANSPARENT_GRADIENT = 2,
                ACCENT_BLUR_BEHIND = 3,
                ACCENT_ACRYLIC_BLUR_BEHIND = 4,
                ACCENT_INVALID_STATE = 5
            }

            [StructLayout(LayoutKind.Sequential)]
            internal struct AccentPolicy
            {
                public AccentState AccentState;
                public uint AccentFlags;
                public uint GradientColor;
                public uint AnimationId;
            }

            internal enum WindowCompositionAttribute
            {
                WCA_ACCENT_POLICY = 19
            }

            [StructLayout(LayoutKind.Sequential)]
            internal struct WindowCompositionAttributeData
            {
                public WindowCompositionAttribute Attribute;
                public int SizeOfData;
                public IntPtr Data;
            }
            public static void Enable(IntPtr HWnd, bool hasFrame = true)
            {
                AccentPolicy accent = new AccentPolicy();
                accent.AccentState = AccentState.ACCENT_BLUR_BEHIND;
                if (hasFrame)
                {
                    accent.AccentFlags = 0x20 | 0x40 | 0x80 | 0x100;
                }

                int accentStructSize = Marshal.SizeOf(accent);
                IntPtr accentPtr = Marshal.AllocHGlobal(accentStructSize);
                Marshal.StructureToPtr(accent, accentPtr, false);
                WindowCompositionAttributeData data = new WindowCompositionAttributeData();
                data.Attribute = WindowCompositionAttribute.WCA_ACCENT_POLICY;
                data.SizeOfData = accentStructSize;
                data.Data = accentPtr;
                SetWindowCompositionAttribute(HWnd, ref data);
                Marshal.FreeHGlobal(accentPtr);
            }
        }

        protected override CreateParams CreateParams
        {
            get
            {
                CreateParams cParms = base.CreateParams;
                cParms.Style |= Acrylic.WS_SYSMENU;
                cParms.ExStyle |= Acrylic.WS_EX_LAYERED;
                cParms.ExStyle |= 0x00000008; // WS_EX_TOPMOST = 0x00000008
                return cParms;
            }
        }

        private void tmrCursor_Tick(object sender, EventArgs e)
        {
            if (hwndDragging == IntPtr.Zero) return;

            Screen? screen = CursorScreen();
            if (screen == null) return;

            Rectangle rect = GetWindowRectangle(hwndDragging);
            Rectangle sensor = Centering(screen.WorkingArea, SensorSize());

            if (status == Status.NON_ACTIVATED && !sensor.Contains(Cursor.Position))
            {
                status = Status.ACTIVATED;
                Show();
            }
            else if (status == Status.ACTIVATED)
            {
                if (sensor.Contains(Cursor.Position))
                {
                    sensing = true;
                    controller.To(screen.WorkingArea, new Size(rect.Width, rect.Height), SIZE_ANIMATION_TIME);
                }
                else
                {
                    sensing = false;
                    controller.To(screen.WorkingArea, new Size(100, 100), SIZE_ANIMATION_TIME);
                }
            }
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void chkRunAtStartup_Click(object sender, EventArgs e)
        {
            RegistryKey? key = Registry.CurrentUser.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", true);
            if (key == null) return;
            if (chkRunAtStartup.Checked)
            {
                key.DeleteValue(Application.ProductName, false);
                chkRunAtStartup.Checked = false;
            }
            else
            {
                key.SetValue(Application.ProductName, Application.ExecutablePath);
                chkRunAtStartup.Checked = true;
            }
        }

        private static void Restart()
        {
            Application.Restart();
            Environment.Exit(0);
        }

        private void btnRestart_Click(object sender, EventArgs e)
        {
            Restart();
        }

        private void btnRestartNow_Click(object sender, EventArgs e)
        {
            Restart();
        }

        private static int? GetRestartInterval()
        {
            RegistryKey? key = Registry.CurrentUser.OpenSubKey($"SOFTWARE\\{Application.ProductName}", true);
            if (key == null) return null;
            return (int?)key.GetValue("RestartInterval");
        }

        private void GetRestartIntervalState()
        {
            int? interval = GetRestartInterval();
            chkRestartEvery5Mins.Checked = false;
            chkRestartEveryHours.Checked = false;
            if (interval == RESTART_INTERVAL_5MINS)
            {
                chkRestartEvery5Mins.Checked = true;
                tmrRestart.Interval = RESTART_INTERVAL_5MINS;
                tmrRestart.Start();
            }
            else if (interval == RESTART_INTERVAL_HOURS)
            {
                chkRestartEveryHours.Checked = true;
                tmrRestart.Interval = RESTART_INTERVAL_HOURS;
                tmrRestart.Start();
            }
        }

        private static void SetRestartInterval(int interval)
        {
            RegistryKey? key = Registry.CurrentUser.OpenSubKey($"SOFTWARE\\{Application.ProductName}", true);
            if (key == null) key = Registry.CurrentUser.CreateSubKey($"SOFTWARE\\{Application.ProductName}");
            key.SetValue("RestartInterval", interval);
        }

        private const int RESTART_INTERVAL_5MINS = 5 * 60 * 1000;
        private const int RESTART_INTERVAL_HOURS = 60 * 60 * 1000;

        private void chkRestartEvery5Mins_Click(object sender, EventArgs e)
        {
            if (chkRestartEvery5Mins.Checked)
            {
                SetRestartInterval(-1);
            }
            else
            {
                SetRestartInterval(RESTART_INTERVAL_5MINS);
            }
            GetRestartIntervalState();
        }

        private void chkRestartEveryHours_Click(object sender, EventArgs e)
        {
            if (chkRestartEveryHours.Checked)
            {
                SetRestartInterval(-1);
            }
            else
            {
                SetRestartInterval(RESTART_INTERVAL_HOURS);
            }
            GetRestartIntervalState();
        }

        private void tmrRestart_Tick(object sender, EventArgs e)
        {
            Restart();
        }
    }
}