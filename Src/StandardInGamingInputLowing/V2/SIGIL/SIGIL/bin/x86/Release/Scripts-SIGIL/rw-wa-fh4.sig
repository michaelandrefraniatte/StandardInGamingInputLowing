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
using WiiMoteAPI;
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
        private static bool controller1_send_back, controller1_send_start, controller1_send_A, controller1_send_B, controller1_send_X, controller1_send_Y, controller1_send_up, controller1_send_left, controller1_send_down, controller1_send_right, controller1_send_leftstick, controller1_send_rightstick, controller1_send_leftbumper, controller1_send_rightbumper, controller1_send_lefttrigger, controller1_send_righttrigger, controller1_send_xbox;
        private static double controller1_send_leftstickx, controller1_send_leftsticky, controller1_send_rightstickx, controller1_send_rightsticky, controller1_send_lefttriggerposition, controller1_send_righttriggerposition;
        private static double statex = 0f, statey = 0f, mousex = 0f, mousey = 0f, mousestatex = 0f, mousestatey = 0f, dzx = 0.0f, dzy = 0.0f, viewpower1x = 0f, viewpower2x = 1f, viewpower3x = 0f, viewpower1y = 0.25f, viewpower2y = 0.75f, viewpower3y = 0f, viewpower05x = 0f, viewpower05y = 0f;
        private static bool getstate;
        private static int sleeptime = 1;
        private ScpBus scp = new ScpBus();
        private WiiMote wm = new WiiMote();
        public static Valuechange ValueChange = new Valuechange();
        public void Close()
        {
            try
            {
                running = false;
                Thread.Sleep(100);
                wm.Close();
                scp.UnLoadController();
            }
            catch { }
        }
        public void Load()
        {
            TimeBeginPeriod(1);
            NtSetTimerResolution(1, true, ref CurrentResolution);
            Task.Run(() => Start());
        }
        private void Start()
        {
            running = true;
            wm.ScanWiimote();
            wm.BeginPolling();
            Thread.Sleep(1000);
            wm.Init();
            scp.LoadController();
            Task.Run(() => taskX());
        }
        private void taskX()
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
                controller1_send_leftstickx = Math.Abs(-mousex * 32767f / 1024f) <= 32767f ? -mousex * 32767f / 1024f : Math.Sign(-mousex) * 32767f;
                controller1_send_leftsticky = Math.Abs(-mousey * 32767f / 1024f) <= 32767f ? -mousey * 32767f / 1024f : Math.Sign(-mousey) * 32767f;
                controller1_send_down = wm.WiimoteButtonStateLeft;
                controller1_send_left = wm.WiimoteButtonStateUp;
                controller1_send_right = wm.WiimoteButtonStateDown;
                controller1_send_up = wm.WiimoteButtonStateRight;
                controller1_send_A = wm.WiimoteButtonStateB;
                controller1_send_B = wm.WiimoteButtonStateOne & wm.WiimoteButtonStateTwo;
                controller1_send_Y = wm.WiimoteButtonStateA;
                controller1_send_X = wm.WiimoteButtonStateHome;
                controller1_send_rightbumper = wm.WiimoteButtonStatePlus;
                controller1_send_leftbumper = wm.WiimoteButtonStateMinus;
                controller1_send_lefttriggerposition = wm.WiimoteButtonStateOne ? 255 : 0;
                controller1_send_righttriggerposition = wm.WiimoteButtonStateTwo ? 255 : 0;
                scp.SetController(controller1_send_back, controller1_send_start, controller1_send_A, controller1_send_B, controller1_send_X, controller1_send_Y, controller1_send_up, controller1_send_left, controller1_send_down, controller1_send_right, controller1_send_leftstick, controller1_send_rightstick, controller1_send_leftbumper, controller1_send_rightbumper, controller1_send_leftstickx, controller1_send_leftsticky, controller1_send_rightstickx, controller1_send_rightsticky, controller1_send_lefttriggerposition, controller1_send_righttriggerposition, controller1_send_xbox);
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