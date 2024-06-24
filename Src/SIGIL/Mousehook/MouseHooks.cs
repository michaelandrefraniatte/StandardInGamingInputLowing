using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using Mousehook;
using Valuechanges;

namespace MouseHooksAPI
{
    public class MouseHooks : IDisposable
    {
        [DllImport("User32.dll")]
        private static extern bool GetCursorPos(out int x, out int y);
        [DllImport("winmm.dll", EntryPoint = "timeBeginPeriod")]
        private static extern uint TimeBeginPeriod(uint ms);
        [DllImport("winmm.dll", EntryPoint = "timeEndPeriod")]
        private static extern uint TimeEndPeriod(uint ms);
        [DllImport("ntdll.dll", EntryPoint = "NtSetTimerResolution")]
        private static extern void NtSetTimerResolution(uint DesiredResolution, bool SetResolution, ref uint CurrentResolution);
        private static uint CurrentResolution = 0;
        private MouseHook mouseHook = new MouseHook();
        public static int MouseHookX, MouseHookY, MouseHookZ, MouseHookButtonX;
        public static bool MouseHookLeftButton, MouseHookRightButton, MouseHookMiddleButton, MouseHookXButton;
        public int CursorX, CursorY, MouseX, MouseY, MouseZ, MouseButtonX;
        public bool MouseLeftButton, MouseRightButton, MouseMiddleButton, MouseXButton;
        private int number = 0;
        private bool running, formvisible;
        private Form1 form1;
        private Stopwatch PollingRate;
        private double pollingrateperm = 0, pollingratetemp = 0, pollingratedisplay = 0, pollingrate;
        private string inputdelaybutton = "", inputdelay = "", inputdelaytemp = "";
        public Valuechange ValueChange;
        private double delay, elapseddown, elapsedup, elapsed;
        private bool getstate = false;
        private bool[] wd = { false };
        private bool[] wu = { false };
        private bool[] ws = { false };
        private void valchanged(int n, bool val)
        {
            if (val)
            {
                if (!wd[n] & !ws[n])
                {
                    wd[n] = true;
                    ws[n] = true;
                    return;
                }
                if (wd[n] & ws[n])
                {
                    wd[n] = false;
                }
                ws[n] = true;
                wu[n] = false;
            }
            if (!val)
            {
                if (!wu[n] & ws[n])
                {
                    wu[n] = true;
                    ws[n] = false;
                    return;
                }
                if (wu[n] & !ws[n])
                {
                    wu[n] = false;
                }
                ws[n] = false;
                wd[n] = false;
            }
        }
        public MouseHooks()
        {
            TimeBeginPeriod(1);
            NtSetTimerResolution(1, true, ref CurrentResolution);
            mouseHook.Hook += new MouseHook.MouseHookCallback(MouseHook_Hook);
            mouseHook.Install();
            running = true;
        }
        public void ViewData(string inputdelaybutton = "")
        {
            if (!formvisible)
            {
                form1 = new Form1();
                PollingRate = new Stopwatch();
                PollingRate.Start();
                ValueChange = new Valuechange();
                this.inputdelaybutton = inputdelaybutton;
                formvisible = true;
                Task.Run(() => form1.SetVisible());
            }
        }
        public void Close()
        {
            if (formvisible)
                if (form1.Visible)
                    form1.Close();
            running = false;
            Thread.Sleep(100);
            mouseHook.Hook -= new MouseHook.MouseHookCallback(MouseHook_Hook);
            mouseHook.Uninstall();
        }
        private void MouseHook_Hook(MouseHook.MSLLHOOKSTRUCT mouseStruct) { }
        public void Init()
        {
            Thread.Sleep(100);
            MouseHookZ = 0;
        }
        private void taskD()
        {
            for (; ; )
            {
                if (!running)
                    break;
                ProcessStateLogic();
                Thread.Sleep(1);
                if (MouseHookZ != 0)
                    Task.Run(() => Init());
                if (formvisible)
                {
                    pollingratedisplay++;
                    pollingratetemp = pollingrateperm;
                    pollingrateperm = (double)PollingRate.ElapsedTicks / (Stopwatch.Frequency / 1000L);
                    if (pollingratedisplay > 300)
                    {
                        pollingrate = pollingrateperm - pollingratetemp;
                        pollingratedisplay = 0;
                    }
                    string str = "CursorX : " + CursorX + Environment.NewLine;
                    str += "CursorY : " + CursorY + Environment.NewLine;
                    str += "MouseX : " + MouseX + Environment.NewLine;
                    str += "MouseY : " + MouseY + Environment.NewLine;
                    str += "MouseZ : " + MouseZ + Environment.NewLine;
                    str += "MouseRightButton : " + MouseRightButton + Environment.NewLine;
                    str += "MouseLeftButton : " + MouseLeftButton + Environment.NewLine;
                    str += "MouseMiddleButton : " + MouseMiddleButton + Environment.NewLine;
                    str += "MouseXButton : " + MouseXButton + Environment.NewLine;
                    str += "MouseButtonX : " + MouseButtonX + Environment.NewLine;
                    str += "PollingRate : " + pollingrate + " ms" + Environment.NewLine;
                    string txt = str;
                    string[] lines = txt.Split(new string[] { Environment.NewLine }, StringSplitOptions.None);
                    foreach (string line in lines)
                        if (line.Contains(inputdelaybutton + " : "))
                        {
                            inputdelaytemp = inputdelay;
                            inputdelay = line;
                        }
                    valchanged(0, inputdelay != inputdelaytemp);
                    if (wd[0])
                    {
                        getstate = true;
                    }
                    
                        getstate = false;
                    if (getstate)
                    {
                        elapseddown = (double)PollingRate.ElapsedTicks / (Stopwatch.Frequency / 1000L);
                        elapsed = 0;
                    }
                    if (wu[0])
                    {
                        elapsedup = (double)PollingRate.ElapsedTicks / (Stopwatch.Frequency / 1000L);
                        elapsed = elapsedup - elapseddown;
                    }
                    ValueChange[0] = inputdelay == inputdelaytemp ? elapsed : 0;
                    if (ValueChange._ValueChange[0] > 0)
                    {
                        delay = ValueChange._ValueChange[0];
                    }
                    str += "InputDelay : " + delay + " ms" + Environment.NewLine;
                    str += Environment.NewLine;
                    form1.SetLabel1(str);
                }
            }
        }
        public void BeginPolling()
        {
            Task.Run(() => taskD());
        }
        public void Scan(int number = 0)
        {
            this.number = number;
        }
        private void ProcessStateLogic() 
        {
            GetCursorPos(out CursorX, out CursorY);
            MouseX = MouseHookX;
            MouseY = MouseHookY;
            MouseZ = MouseHookZ;
            MouseRightButton = MouseHookRightButton;
            MouseLeftButton = MouseHookLeftButton;
            MouseMiddleButton = MouseHookMiddleButton;
            MouseXButton = MouseHookXButton;
            MouseButtonX = MouseHookButtonX;
        }
        public void Dispose()
        {
            GC.Collect();
            GC.WaitForPendingFinalizers();
            GC.SuppressFinalize(this);
        }
    }
    public class MouseHook
    {
        private static int MouseHookX, MouseHookY, MouseHookZ, MouseHookButtonX, MouseDesktopHookX, MouseDesktopHookY, MouseHookTime;
        private static bool MouseHookLeftButton, MouseHookRightButton, MouseHookMiddleButton, MouseHookXButton;
        public delegate IntPtr MouseHookHandler(int nCode, IntPtr wParam, IntPtr lParam);
        private MouseHookHandler hookHandler;
        private MSLLHOOKSTRUCT mouseStruct;
        public delegate void MouseHookCallback(MSLLHOOKSTRUCT mouseStruct);
        public event MouseHookCallback Hook;
        private IntPtr hookID = IntPtr.Zero;
        public void Install()
        {
            hookHandler = HookFunc;
            hookID = SetHook(hookHandler);
        }
        public void Uninstall()
        {
            if (hookID == IntPtr.Zero)
                return;
            UnhookWindowsHookEx(hookID);
            hookID = IntPtr.Zero;
        }
        ~MouseHook()
        {
            Uninstall();
        }
        private IntPtr SetHook(MouseHookHandler proc)
        {
            using (ProcessModule module = Process.GetCurrentProcess().MainModule)
                return SetWindowsHookEx(WH_MOUSE_LL, proc, GetModuleHandle(module.ModuleName), 0);
        }
        private IntPtr HookFunc(int nCode, IntPtr wParam, IntPtr lParam)
        {
            mouseStruct = (MSLLHOOKSTRUCT)Marshal.PtrToStructure(lParam, typeof(MSLLHOOKSTRUCT));
            if (MouseHook.MouseMessages.WM_RBUTTONDOWN == (MouseHook.MouseMessages)wParam)
                MouseHookRightButton = true;
            if (MouseHook.MouseMessages.WM_RBUTTONUP == (MouseHook.MouseMessages)wParam)
                MouseHookRightButton = false;
            if (MouseHook.MouseMessages.WM_LBUTTONDOWN == (MouseHook.MouseMessages)wParam)
                MouseHookLeftButton = true;
            if (MouseHook.MouseMessages.WM_LBUTTONUP == (MouseHook.MouseMessages)wParam)
                MouseHookLeftButton = false;
            if (MouseHook.MouseMessages.WM_MBUTTONDOWN == (MouseHook.MouseMessages)wParam)
                MouseHookMiddleButton = true;
            if (MouseHook.MouseMessages.WM_MBUTTONUP == (MouseHook.MouseMessages)wParam)
                MouseHookMiddleButton = false;
            if (MouseHook.MouseMessages.WM_XBUTTONDOWN == (MouseHook.MouseMessages)wParam)
                MouseHookXButton = true;
            if (MouseHook.MouseMessages.WM_XBUTTONUP == (MouseHook.MouseMessages)wParam)
                MouseHookXButton = false;
            if (MouseHook.MouseMessages.WM_MOUSEWHEEL == (MouseHook.MouseMessages)wParam)
                MouseHookZ = (int)mouseStruct.mouseData; // 7864320, -7864320
            else
                MouseHookZ = 0;
            if (MouseHook.MouseMessages.WM_XBUTTONDOWN == (MouseHook.MouseMessages)wParam)
                MouseHookButtonX = (int)mouseStruct.mouseData; //131072, 65536
            else
                MouseHookButtonX = 0;
            MouseHookX = mouseStruct.pt.x;
            MouseHookY = mouseStruct.pt.y;
            MouseHookTime = (int)mouseStruct.time;
            MouseHooks.MouseHookRightButton = MouseHookRightButton;
            MouseHooks.MouseHookLeftButton = MouseHookLeftButton;
            MouseHooks.MouseHookMiddleButton = MouseHookMiddleButton;
            MouseHooks.MouseHookXButton = MouseHookXButton;
            MouseHooks.MouseHookButtonX = MouseHookButtonX;
            MouseHooks.MouseHookX = MouseHookX;
            MouseHooks.MouseHookY = MouseHookY;
            MouseHooks.MouseHookZ = MouseHookZ;
            Hook((MSLLHOOKSTRUCT)Marshal.PtrToStructure(lParam, typeof(MSLLHOOKSTRUCT)));
            return CallNextHookEx(hookID, nCode, wParam, lParam);
        }
        private const int WH_MOUSE_LL = 14;
        private enum MouseMessages
        {
            WM_LBUTTONDOWN = 0x0201,
            WM_LBUTTONUP = 0x0202,
            WM_MOUSEMOVE = 0x0200,
            WM_MOUSEWHEEL = 0x020A,
            WM_RBUTTONDOWN = 0x0204,
            WM_RBUTTONUP = 0x0205,
            WM_LBUTTONDBLCLK = 0x0203,
            WM_RBUTTONDBLCLK = 0x0206,
            WM_MBUTTONDOWN = 0x0207,
            WM_MBUTTONUP = 0x0208,
            WM_XBUTTONDOWN = 0x020B,
            WM_XBUTTONUP = 0x020C
        }
        [StructLayout(LayoutKind.Sequential)]
        public struct POINT
        {
            public int x;
            public int y;
        }
        [StructLayout(LayoutKind.Sequential)]
        public struct MSLLHOOKSTRUCT
        {
            public POINT pt;
            public uint mouseData;
            public uint flags;
            public uint time;
            public IntPtr dwExtraInfo;
        }
        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr SetWindowsHookEx(int idHook, MouseHookHandler lpfn, IntPtr hMod, uint dwThreadId);
        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool UnhookWindowsHookEx(IntPtr hhk);
        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr CallNextHookEx(IntPtr hhk, int nCode, IntPtr wParam, IntPtr lParam);
        [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr GetModuleHandle(string lpModuleName);
        [DllImport("User32.dll")]
        private static extern bool GetCursorPos(out int x, out int y);
        [DllImport("user32.dll")]
        private static extern void SetCursorPos(int X, int Y);
    }
}