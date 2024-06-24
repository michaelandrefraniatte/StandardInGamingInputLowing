using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using Timers;
using Valuechanges;

namespace TimersAPI
{
    public class Timer : IDisposable
    {
        [DllImport("winmm.dll", EntryPoint = "timeBeginPeriod")]
        private static extern uint TimeBeginPeriod(uint ms);
        [DllImport("winmm.dll", EntryPoint = "timeEndPeriod")]
        private static extern uint TimeEndPeriod(uint ms);
        [DllImport("ntdll.dll", EntryPoint = "NtSetTimerResolution")]
        private static extern void NtSetTimerResolution(uint DesiredResolution, bool SetResolution, ref uint CurrentResolution);
        private static uint CurrentResolution = 0;
        private Stopwatch watch = new Stopwatch();
        private bool running, formvisible;
        private int number;
        public double timeelapsed;
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
        public Timer()
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
            Thread.Sleep(100);
        }
        public void BeginPolling()
        {
            Task.Run(() => taskD());
        }
        private void taskD()
        {
            for (; ; )
            {
                if (!running)
                    break;
                try
                {
                    timeelapsed = (double)watch.ElapsedTicks / (Stopwatch.Frequency / 1000L);
                }
                catch { Thread.Sleep(10); }
                Thread.Sleep(1);
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
                    string str = "timeelapsed : " + timeelapsed + Environment.NewLine;
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
        public void Init()
        {
            watch.Stop();
            watch = new Stopwatch();
            watch.Start();
            timeelapsed = 0;
        }
        public void Scan(int number = 0)
        {
            this.number = number;
            watch = new Stopwatch();
            watch.Start();
        }
        public void Dispose()
        {
            GC.Collect();
            GC.WaitForPendingFinalizers();
            GC.SuppressFinalize(this);
        }
    }
}