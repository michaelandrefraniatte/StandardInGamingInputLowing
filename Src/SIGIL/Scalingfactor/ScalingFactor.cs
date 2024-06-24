using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using System.Windows.Forms;
using Scalingfactor;
using Valuechanges;

namespace ScalingFactorAPI
{
    public class ScalingFactor : IDisposable
    {
        [DllImport("user32.dll")]
        public static extern bool EnumDisplaySettings(string lpszDeviceName, int iModeNum, ref DEVMODE lpDevMode);
        [DllImport("winmm.dll", EntryPoint = "timeBeginPeriod")]
        private static extern uint TimeBeginPeriod(uint ms);
        [DllImport("winmm.dll", EntryPoint = "timeEndPeriod")]
        private static extern uint TimeEndPeriod(uint ms);
        [DllImport("ntdll.dll", EntryPoint = "NtSetTimerResolution")]
        private static extern void NtSetTimerResolution(uint DesiredResolution, bool SetResolution, ref uint CurrentResolution);
        private static uint CurrentResolution = 0;
        private bool running, formvisible;
        private int number;
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
        public ScalingFactor()
        {
            TimeBeginPeriod(1);
            NtSetTimerResolution(1, true, ref CurrentResolution);
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
        }
        private void taskM()
        {
            for (; ; )
            {
                if (!running)
                    break;
                System.Threading.Thread.Sleep(1000);
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
                    string str = "scalingfactorx : " + scalingfactorx + Environment.NewLine;
                    str += "scalingfactory : " + scalingfactory + Environment.NewLine;
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
                    if (inputdelay == inputdelaytemp)
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
            Task.Run(() => taskM());
        }
        public void Init()
        {
        }
        public double scalingfactorx, scalingfactory;
        public bool Scan(int number = 0)
        {
            this.number = number;
            ProcessStateLogic();
            return true;
        }
        private void ProcessStateLogic()
        {
            Screen[] screenList = Screen.AllScreens;
            foreach (Screen screen in screenList)
            {
                DEVMODE dm = new DEVMODE();
                dm.dmSize = (short)Marshal.SizeOf(typeof(DEVMODE));
                EnumDisplaySettings(screen.DeviceName, -1, ref dm);
                scalingfactorx = (double)dm.dmPelsWidth / (double)screen.Bounds.Width;
                scalingfactory = (double)dm.dmPelsHeight / (double)screen.Bounds.Height;
                break;
            }
        }
        [StructLayout(LayoutKind.Sequential)]
        public struct DEVMODE
        {
            private const int CCHDEVICENAME = 0x20;
            private const int CCHFORMNAME = 0x20;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 0x20)]
            public string dmDeviceName;
            public short dmSpecVersion;
            public short dmDriverVersion;
            public short dmSize;
            public short dmDriverExtra;
            public int dmFields;
            public int dmPositionX;
            public int dmPositionY;
            public ScreenOrientation dmDisplayOrientation;
            public int dmDisplayFixedOutput;
            public short dmColor;
            public short dmDuplex;
            public short dmYResolution;
            public short dmTTOption;
            public short dmCollate;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 0x20)]
            public string dmFormName;
            public short dmLogPixels;
            public int dmBitsPerPel;
            public int dmPelsWidth;
            public int dmPelsHeight;
            public int dmDisplayFlags;
            public int dmDisplayFrequency;
            public int dmICMMethod;
            public int dmICMIntent;
            public int dmMediaType;
            public int dmDitherType;
            public int dmReserved1;
            public int dmReserved2;
            public int dmPanningWidth;
            public int dmPanningHeight;
        }
        public void Dispose()
        {
            GC.Collect();
            GC.WaitForPendingFinalizers();
            GC.SuppressFinalize(this);
        }
    }
}