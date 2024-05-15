using System;
using System.Globalization;
using System.IO;
using System.Numerics;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;
using System.Reflection;
using controllers;
using System.Diagnostics;
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
        private static bool Controller_Send_back, Controller_Send_start, Controller_Send_A, Controller_Send_B, Controller_Send_X, Controller_Send_Y, Controller_Send_up, Controller_Send_left, Controller_Send_down, Controller_Send_right, Controller_Send_leftstick, Controller_Send_rightstick, Controller_Send_leftbumper, Controller_Send_rightbumper, Controller_Send_lefttrigger, Controller_Send_righttrigger, Controller_Send_xbox;
        private static double Controller_Send_leftstickx, Controller_Send_leftsticky, Controller_Send_rightstickx, Controller_Send_rightsticky, Controller_Send_lefttriggerposition, Controller_Send_righttriggerposition;
        private static double statex = 0f, statey = 0f, mousex = 0f, mousey = 0f, mousestatex = 0f, mousestatey = 0f, dzx = 15.0f, dzy = 0.0f, viewpower1x = 1f, viewpower2x = 0f, viewpower3x = 0f, viewpower1y = 1f, viewpower2y = 0f, viewpower3y = 0f, viewpower05x = 0f, viewpower05y = 0f;
        private static bool getstate;
        private static int sleeptime = 1;
        private static int irmode = 0;
        private static double centery = 0;
        private XBoxController XBC = new XBoxController();
        private WiiMote wm = new WiiMote();
        public static Valuechange ValueChange = new Valuechange();
        public void Close()
        {
            try
            {
                running = false;
                Thread.Sleep(100);
                wm.Close();
                XBC.Disconnect();
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
            wm.BeginPolling();
            Thread.Sleep(1000);
            wm.Init();
            XBC.Connect();
            Task.Run(() => task());
        }
        private void task()
        {
            for (; ; )
            {
                if (!running)
                    break;
                if (wm.WiimoteRawValuesY > 0f)
                    mousex = Scale(wm.WiimoteRawValuesY * 45f, 0f, 1024f, (dzx / 100f) * 1024f, 1024f);
                if (wm.WiimoteRawValuesY < 0f)
                    mousex = Scale(wm.WiimoteRawValuesY * 45f, -1024f, 0f, -1024f, -(dzx / 100f) * 1024f);
                if (wm.WiimoteRawValuesX > 0f)
                    mousey = Scale(wm.WiimoteRawValuesX * 90f, 0f, 1024f, (dzy / 100f) * 1024f, 1024f);
                if (wm.WiimoteRawValuesX < 0f)
                    mousey = Scale(wm.WiimoteRawValuesX * 90f, -1024f, 0f, -1024f, -(dzy / 100f) * 1024f);
                Controller_Send_leftstickx = Math.Abs(-mousex * 32767f / 1024f) <= 32767f ? -mousex * 32767f / 1024f : Math.Sign(-mousex) * 32767f;
                Controller_Send_leftsticky = Math.Abs(-mousey * 32767f / 1024f) <= 32767f ? -mousey * 32767f / 1024f : Math.Sign(-mousey) * 32767f;
                Controller_Send_down = wm.WiimoteButtonStateLeft;
                Controller_Send_left = wm.WiimoteButtonStateUp;
                Controller_Send_right = wm.WiimoteButtonStateDown;
                Controller_Send_up = wm.WiimoteButtonStateRight;
                Controller_Send_A = wm.WiimoteButtonStateB;
                Controller_Send_B = wm.WiimoteButtonStateOne & wm.WiimoteButtonStateTwo;
                Controller_Send_Y = wm.WiimoteButtonStateA;
                Controller_Send_X = wm.WiimoteButtonStateHome;
                Controller_Send_rightbumper = wm.WiimoteButtonStatePlus;
                Controller_Send_leftbumper = wm.WiimoteButtonStateMinus;
                Controller_Send_lefttriggerposition = wm.WiimoteButtonStateOne ? 255 : 0;
                Controller_Send_righttriggerposition = wm.WiimoteButtonStateTwo ? 255 : 0;
                XBC.Set(Controller_Send_back, Controller_Send_start, Controller_Send_A, Controller_Send_B, Controller_Send_X, Controller_Send_Y, Controller_Send_up, Controller_Send_left, Controller_Send_down, Controller_Send_right, Controller_Send_leftstick, Controller_Send_rightstick, Controller_Send_leftbumper, Controller_Send_rightbumper, Controller_Send_leftstickx, Controller_Send_leftsticky, Controller_Send_rightstickx, Controller_Send_rightsticky, Controller_Send_lefttriggerposition, Controller_Send_righttriggerposition, Controller_Send_xbox);
                /*XBC.ViewData();*/
                /*wm.ViewData();*/
                Thread.Sleep(sleeptime);
            }
        }
        private double Scale(double value, double min, double max, double minScale, double maxScale)
        {
            double scaled = minScale + (double)(value - min) / (max - min) * (maxScale - minScale);
            return scaled;
        }
    }
}