using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using Timers;

namespace TimersAPI
{
    public class Timer
    {
        [DllImport("winmm.dll", EntryPoint = "timeBeginPeriod")]
        private static extern uint TimeBeginPeriod(uint ms);
        [DllImport("winmm.dll", EntryPoint = "timeEndPeriod")]
        private static extern uint TimeEndPeriod(uint ms);
        [DllImport("ntdll.dll", EntryPoint = "NtSetTimerResolution")]
        private static extern void NtSetTimerResolution(uint DesiredResolution, bool SetResolution, ref uint CurrentResolution);
        private static uint CurrentResolution = 0;
        private static Stopwatch watch = new Stopwatch();
        private bool running, formvisible;
        private int number;
        public double timeelapsed;
        private Form1 form1 = new Form1();
        public Timer()
        {
            TimeBeginPeriod(1);
            NtSetTimerResolution(1, true, ref CurrentResolution);
            running = true;
        }
        public void ViewData()
        {
            if (!form1.Visible)
            {
                formvisible = true;
                form1.SetVisible();
            }
        }
        public void Close()
        {
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
                    string str = "timeelapsed : " + timeelapsed + Environment.NewLine;
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
    }
}