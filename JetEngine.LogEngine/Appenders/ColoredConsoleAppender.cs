using System;
using System.Linq;
using System.Runtime.InteropServices;

namespace JetEngine.LogEngine.Appenders
{
    public class ColoredConsoleAppender : log4net.Appender.ColoredConsoleAppender
    {
        public bool AllocateConsole { get; set; }

        public override void ActivateOptions()
        {
            if (AllocateConsole)
            {
                ConsoleWindow.Open();
            }
            base.ActivateOptions();
        }

        protected override void OnClose()
        {
            base.OnClose();
            if (AllocateConsole)
            {
                ConsoleWindow.Close();
            }
        }

        private static class ConsoleWindow
        {
            private static bool _isAllocated;


            public static void Open()
            {
                //AttachConsole(ATTACH_PARENT_PROCESS);
                var handle = GetConsoleWindow();
                if (handle == IntPtr.Zero)
                {
                    if (AllocConsole())
                    {
                        handle = GetConsoleWindow();
                    }
                    _isAllocated = true;
                }
                ShowWindow(handle, SW_SHOW);

                var newStdHandle = new IntPtr(7);
                SetStdHandle(STD_INPUT_HANDLE, newStdHandle);
                SetStdHandle(STD_OUTPUT_HANDLE, newStdHandle);
                SetStdHandle(STD_ERROR_HANDLE, newStdHandle);
            }

            public static void Close()
            {
                if (!_isAllocated)
                {
                    return;
                }
                _isAllocated = false;
                FreeConsole();
            }

            [DllImport("kernel32.dll", SetLastError = true)]
            [return: MarshalAs(UnmanagedType.Bool)]
            private static extern bool SetStdHandle(UInt32 nStdHandle, IntPtr hHandle);

            [DllImport("kernel32.dll", SetLastError = true)]
            [return: MarshalAs(UnmanagedType.Bool)]
            private static extern bool AllocConsole();

            [DllImport("kernel32.dll", SetLastError = true)]
            [return: MarshalAs(UnmanagedType.Bool)]
            private static extern bool FreeConsole();

            //[DllImport("kernel32", SetLastError = true)]
            //[return: MarshalAs(UnmanagedType.Bool)]
            //private static extern bool AttachConsole(int dwProcessId);

            [DllImport("kernel32.dll")]
            private static extern IntPtr GetConsoleWindow();

            [DllImport("user32.dll")]
            [return: MarshalAs(UnmanagedType.Bool)]
            private static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);

            private const int SW_HIDE = 0;
            private const int SW_SHOW = 5;

            private const UInt32 ATTACH_PARENT_PROCESS = 0xFFFFFFFF;

            private const UInt32 STD_INPUT_HANDLE = 0xFFFFFFF6;
            private const UInt32 STD_OUTPUT_HANDLE = 0xFFFFFFF5;
            private const UInt32 STD_ERROR_HANDLE = 0xFFFFFFF4;
        }
    }
}
