using System.Runtime.InteropServices;

namespace SnapAssistForCentering
{
    internal static class Program
    {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            IntPtr hook = SetWinEventHook(EVENT_SYSTEM_MOVESIZESTART, EVENT_SYSTEM_MOVESIZEEND, IntPtr.Zero, procDelegate, 0, 0, WINEVENT_OUTOFCONTEXT);
            // To customize application configuration such as set high DPI settings or default font,
            // see https://aka.ms/applicationconfiguration.
            ApplicationConfiguration.Initialize();
            Application.Run(new Indicator());
            MessageBox.Show("Close");
            UnhookWinEvent(hook);
        }

        const uint WINEVENT_OUTOFCONTEXT = 0;
        const uint EVENT_SYSTEM_MOVESIZESTART = 0x000A;
        const uint EVENT_SYSTEM_MOVESIZEEND = 0x000B;
        static WinEventDelegate procDelegate = new WinEventDelegate(WinEventProc);

        static void WinEventProc(IntPtr hWinEventHook, uint eventType, IntPtr hwnd, int idObject, int idChild, uint dwEventThread, uint dwmsEventTime)
        {
            //MessageBox.Show("kkk");
        }

        delegate void WinEventDelegate(IntPtr hWinEventHook, uint eventType, IntPtr hwnd, int idObject, int idChild, uint dwEventThread, uint dwmsEventTime);

        [DllImport("user32.dll")]
        static extern IntPtr SetWinEventHook(uint eventMin, uint eventMax, IntPtr hmodWinEventProc, WinEventDelegate lpfnWinEventProc,
            uint idProcess, uint idThread, uint dwFlags);

        [DllImport("user32.dll")]
        static extern bool UnhookWinEvent(IntPtr hWinEventHook);

        [DllImport("kernel32", CharSet = CharSet.Ansi)]
        public static extern void Beep(int frequency, int duration);
    }
}