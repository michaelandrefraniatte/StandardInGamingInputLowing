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
using controllers;
using System.Diagnostics;
using ScalingFactorAPI;
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
        private ScalingFactor sf = new ScalingFactor();
        public void Close()
        {
            try
            {
                running = false;
                Thread.Sleep(100);
                sf.Close();
                sf.Dispose();
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
            sf.Scan();
            sf.BeginPolling();
            Task.Run(() => task());
        }
        private void task()
        {
            for (; ; )
            {
                if (!running)
                    break;
                sf.ViewData();
                Thread.Sleep(sleeptime);
            }
        }
        private double Scale(double value, double mxhn, double max, double mxhnScale, double maxScale)
        {
            double scaled = mxhnScale + (double)(value - mxhn) / (max - mxhn) * (maxScale - mxhnScale);
            return scaled;
        }
    }
}