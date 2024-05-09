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
using KeyboardInputsAPI;
using Valuechanges;
using TimersAPI;
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
        private static int sleeptime = 1;
        private static double delay, elapseddown, elapsedup;
        private KeyboardInput ki = new KeyboardInput();
        public static Valuechange ValueChange = new Valuechange();
        private TimersAPI.Timer timer = new TimersAPI.Timer();
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
                ki.Close();
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
            timer.Scan();
            ki.Scan();
            timer.BeginPolling();
            ki.BeginPolling();
            Task.Run(() => task());
        }
        private void task()
        {
            for (; ; )
            {
                if (!running)
                    break;
                if (ki.KeyboardKeyA)
                {
                    elapseddown = timer.timeelapsed;
                }
                if (!ki.KeyboardKeyA)
                {
                    elapsedup = timer.timeelapsed;
                }
                ValueChange[0] = ki.KeyboardKeyA ? elapseddown : elapsedup;
                if (ValueChange._ValueChange[0] > elapsedup - elapseddown)
                {
                    delay = ValueChange._ValueChange[0];
                    MessageBox.Show("Input delay pressing the key A or Q on French keyboard: " + delay.ToString() + " ms.");
                }
                /*ki.ViewData();*/
                /*Thread.Sleep(sleeptime);*/
            }
        }
    }
}