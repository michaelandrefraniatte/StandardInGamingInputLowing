using System;
using System.Globalization;
using System.IO;
using System.Drawing;
using System.Numerics;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;
using System.Reflection;
using System.Diagnostics;
using TimersAPI;
using Valuechanges;
using WiiMotesAPI;

namespace StringToCode
{
    public class FooClass 
    { 
        [DllImport("winmm.dll", EntryPoint = "timeBeginPeriod")]
        private static extern uint TimeBeginPeriod(uint ms);
        [DllImport("winmm.dll", EntryPoint = "timeEndPeriod")]
        private static extern uint TimeEndPeriod(uint ms);
        [DllImport("ntdll.dll", EntryPoint = "NtSetTimerResolution")]
        private static extern void NtSetTimerResolution(uint DesiredResolution, bool SetResolution, ref uint CurrentResolution);
        private static uint CurrentResolution = 0;
        private static bool running;
        private static double delay, elapseddown, elapsedup, elapsed;
        private static bool getstate = false;
        private static int sleeptime = 1;
        private static int irmode = 2;
        private static double centery = 80f;
        public static Valuechange ValueChange = new Valuechange();
        private TimersAPI.Timer timer = new TimersAPI.Timer();
        private WiiMote wm = new WiiMote();
        private static int[] wd = { 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2 };
        private static int[] wu = { 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2 };
        public static void valchanged(int n, bool val)
        {
            if (val)
            {
                if (wd[n] <= 1)
                {
                    wd[n] = wd[n] + 1;
                }
                wu[n] = 0;
            }
            else
            {
                if (wu[n] <= 1)
                {
                    wu[n] = wu[n] + 1;
                }
                wd[n] = 0;
            }
        }
        public void Close()
        {
            try
            {
                running = false;
                Thread.Sleep(100);
                timer.Close();
                wm.Close();
            }
            catch { }
        }
        public static void Main() {}
        public void Load()
        {
            TimeBeginPeriod(1);
            NtSetTimerResolution(1, true, ref CurrentResolution);
            Task.Run(() => Start());
        }
        private void Start()
        {
            running = true;
            wm.Scan(irmode, centery);
            timer.Scan();
            wm.BeginPolling();
            timer.BeginPolling();
            Task.Run(() => task());
        }
        private void task()
        {
            for (; ; )
            {
                if (!running)
                    break;
                valchanged(0, wm.WiimoteButtonStateA);
                if (wd[0] == 1)
                {
                    getstate = true;
                }
                if (!wm.WiimoteButtonStateA)
                    getstate = false;
                if (getstate)
                {
                    elapseddown = timer.timeelapsed;
                    elapsed     = 0;
                }
                if (wu[0] == 1)
                {
                    elapsedup = timer.timeelapsed;
                    elapsed   = elapsedup - elapseddown;
                }
                ValueChange[0] = !wm.WiimoteButtonStateA ? elapsed : 0;
                if (ValueChange._ValueChange[0] > 0)
                {
                    delay = ValueChange._ValueChange[0];
                    MessageBox.Show("Input delay pressing the Wiimote A button: " + delay.ToString() + " ms.");
                }
                /*wm.ViewData();*/
                /*timer.ViewData();*/
                Thread.Sleep(sleeptime);
            }
        }
    }
}