using System.Diagnostics;
using System.Runtime.InteropServices;

namespace SnapAssistForCentering
{
    public partial class Indicator : Form
    {
        public Indicator()
        {
            InitializeComponent();

            // Initialize for Acrylic
            //SetStyle(ControlStyles.SupportsTransparentBackColor, true);

            hwndDragging = IntPtr.Zero;
            procDelegate = new WinEventDelegate(WinEventProc);
            IntPtr hook = SetWinEventHook(EVENT_SYSTEM_MOVESIZESTART, EVENT_SYSTEM_MOVESIZEEND, IntPtr.Zero, procDelegate, 0, 0, WINEVENT_OUTOFCONTEXT);
        }

        private void Indicator_Load(object sender, EventArgs e)
        {
            Acrylic.Enable(this.Handle);
        }

        private void Indicator_Shown(object sender, EventArgs e)
        {
            if (startup)
            {
                startup = false;
                Hide();
            }
        }

        protected override bool ShowWithoutActivation => true;

        private void tmrCursor_Tick(object sender, EventArgs e)
        {
            if (hwndDragging == IntPtr.Zero) return;

            Rectangle rect = new Rectangle();
            GetWindowRectangle(hwndDragging, out rect);
            Rectangle sensor = new Rectangle(-SENSOR_WIDTH / 2, -SENSOR_HEIGHT / 2, SENSOR_WIDTH, SENSOR_HEIGHT);
            sensor.Offset(new Point(Screen.PrimaryScreen.Bounds.Width / 2, Screen.PrimaryScreen.Bounds.Height / 2));

            if (sensor.Contains(Cursor.Position))
            {
                Size = new Size(rect.Right - rect.Left, rect.Bottom - rect.Top);
            }
            else
            {
                Size = new Size(100, 100);
            }
            Location = new Point(Screen.PrimaryScreen.Bounds.Width / 2, Screen.PrimaryScreen.Bounds.Height / 2) - Size / 2;
        }

        bool startup = true;
        const uint WINEVENT_OUTOFCONTEXT = 0;
        const uint EVENT_SYSTEM_MOVESIZESTART = 0x000A;
        const uint EVENT_SYSTEM_MOVESIZEEND = 0x000B;
        const int SW_SHOW = 5;
        const int SENSOR_WIDTH = 100;
        const int SENSOR_HEIGHT = 100;
        WinEventDelegate procDelegate;
        IntPtr hwndDragging;

        void WinEventProc(IntPtr hWinEventHook, uint eventType, IntPtr hwnd, int idObject, int idChild, uint dwEventThread, uint dwmsEventTime)
        {
            if (eventType == EVENT_SYSTEM_MOVESIZESTART)
            {
                hwndDragging = hwnd;
                tmrCursor.Start();

                IntPtr hwndIndicator = Process.GetCurrentProcess().Handle;
                Size = new Size(100, 100);
                Location = new Point(Screen.PrimaryScreen.Bounds.Width / 2, Screen.PrimaryScreen.Bounds.Height / 2) - Size / 2;
                Show();
                SetWindowPos(hwndIndicator, hwndDragging, Location.X, Location.Y, Size.Width, Size.Height, SW_SHOW);
            }
            else if (eventType == EVENT_SYSTEM_MOVESIZEEND)
            {
                tmrCursor.Stop();
                Hide();

                Rectangle sensor = new Rectangle(-SENSOR_WIDTH / 2, -SENSOR_HEIGHT / 2, SENSOR_WIDTH, SENSOR_HEIGHT);
                sensor.Offset(new Point(Screen.PrimaryScreen.Bounds.Width / 2, Screen.PrimaryScreen.Bounds.Height / 2));

                if (sensor.Contains(Cursor.Position))
                {
                    Rectangle rect = new Rectangle();
                    GetWindowRectangle(hwndDragging, out rect);
                    Point destination = new Point((Screen.PrimaryScreen.Bounds.Width - rect.Width) / 2, (Screen.PrimaryScreen.Bounds.Height - rect.Height) / 2);
                    SetWindowPos(hwndDragging, IntPtr.Zero, destination.X, destination.Y, rect.Width, rect.Height, SW_SHOW);
                }

                hwndDragging = IntPtr.Zero;
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
        static int GetWindowRectangle(IntPtr hwnd, out Rectangle rect)
        {
            int returned = GetWindowRect(hwnd, out rect);
            // The returned Width and Height are actually Right and Bottom
            rect.Width = rect.Width - rect.Left;
            rect.Height = rect.Height - rect.Top;
            return returned;
        }

        [DllImport("user32.dll")]
        static extern bool SetWindowPos(IntPtr hWnd, IntPtr hwndInsertAfter, int x, int y, int cx, int cy, uint flags);



        //internal void EnableBlur()
        //{
        //    AccentPolicy accent = new AccentPolicy();
        //    accent.AccentState = AccentState.ACCENT_ACRYLIC_BLUR_BEHIND;
        //    accent.AccentFlags = 0x20 | 0x40 | 0x80 | 0x100;
        //    //accent.GradientColor = (intBlurOpacity << 24) | (intBlurOpacity & 0xFFFFFF);
        //    var accentStructSize = Marshal.SizeOf(accent);
        //    var accentPtr = Marshal.AllocHGlobal(accentStructSize);
        //    Marshal.StructureToPtr(accent, accentPtr, false);
        //    WindowCompositionAttributeData data = new WindowCompositionAttributeData();
        //    data.Attribute = WindowCompositionAttribute.WCA_ACCENT_POLICY;
        //    data.SizeOfData = accentStructSize;
        //    data.Data = accentPtr;
        //    SetWindowCompositionAttribute(this.Handle, ref data);
        //    Marshal.FreeHGlobal(accentPtr);
        //}

        public static class Acrylic
        {
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

        public const int WS_SYSMENU = 0x80000;
        public const int WS_EX_LAYERED = 0x00080000;

        protected override CreateParams CreateParams
        {
            get
            {
                CreateParams cParms = base.CreateParams;
                cParms.Style |= WS_SYSMENU;
                cParms.ExStyle |= WS_EX_LAYERED;
                return cParms;
            }
        }
    }
}