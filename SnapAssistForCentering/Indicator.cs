using System.Windows.Forms;
using System.Runtime.InteropServices;

namespace SnapAssistForCentering
{
    public partial class Indicator : Form
    {
        public Indicator()
        {
            InitializeComponent();
            hwndDragging = IntPtr.Zero;
            procDelegate = new WinEventDelegate(WinEventProc);
            IntPtr hook = SetWinEventHook(EVENT_SYSTEM_MOVESIZESTART, EVENT_SYSTEM_MOVESIZEEND, IntPtr.Zero, procDelegate, 0, 0, WINEVENT_OUTOFCONTEXT);
        }

        private void Indicator_Load(object sender, EventArgs e)
        {
            //var path = new System.Drawing.Drawing2D.GraphicsPath();

            //path.AddEllipse(0, 0, 100, 100);
            //Region = new Region(path);
            Hide();
        }

        private void tmrCursor_Tick(object sender, EventArgs e)
        {
            if (hwndDragging == IntPtr.Zero) return;

            Rectangle rect = new Rectangle();
            GetWindowRectangle(hwndDragging, out rect);
            //GetWindowRect(hwndDragging, out rect);
            //// The returned Width and Height are actually Right and Bottom
            //rect.Width = rect.Width - rect.Left;
            //rect.Height = rect.Height - rect.Top;

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
                Show();
                Point sensor = new Point(Screen.PrimaryScreen.Bounds.Width / 2, Screen.PrimaryScreen.Bounds.Height / 2) - Size / 2;
                Location = sensor;
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
                    //GetWindowRect(hwndDragging, out rect);
                    //// The returned Width and Height are actually Right and Bottom
                    //rect.Width = rect.Width - rect.Left;
                    //rect.Height = rect.Height - rect.Top;

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
    }
}