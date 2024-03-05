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
        private static double mousex = 0f, mousey = 0f, viewpower1x = 0.33f, viewpower2x = 0f, viewpower3x = 0.67f, viewpower1y = 0.25f, viewpower2y = 0.75f, viewpower3y = 0f, dzx = 3.0f, dzy = 3.0f;
        private static bool getstate;
        private static int sleeptime = 1;
        private static int irmode = 2;
        private static double centery = 80f;
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
                Controller_Send_rightstick           = wm.WiimoteButtonStateDown;
                Controller_Send_leftstick            = wm.WiimoteNunchuckStateZ;
                Controller_Send_A                    = wm.WiimoteNunchuckStateC;
                Controller_Send_back                 = wm.WiimoteButtonStateOne;
                Controller_Send_start                = wm.WiimoteButtonStateTwo;
                Controller_Send_X                    = wm.WiimoteButtonStateHome | ((wm.WiimoteRawValuesZ > 0 ? wm.WiimoteRawValuesZ : -wm.WiimoteRawValuesZ) >= 30f & (wm.WiimoteRawValuesY > 0 ? wm.WiimoteRawValuesY : -wm.WiimoteRawValuesY) >= 30f & (wm.WiimoteRawValuesX > 0 ? wm.WiimoteRawValuesX : -wm.WiimoteRawValuesX) >= 30f);
                Controller_Send_leftbumper           = wm.WiimoteButtonStateMinus;
                Controller_Send_rightbumper          = wm.WiimoteButtonStatePlus;
                Controller_Send_B                    = wm.WiimoteButtonStateRight;
                Controller_Send_Y                    = wm.WiimoteNunchuckStateRawValuesY > 90f | wm.WiimoteButtonStateUp;
                Controller_Send_righttriggerposition = wm.WiimoteButtonStateB ? 255 : 0;
                Controller_Send_lefttriggerposition  = wm.WiimoteButtonStateA ? 255 : 0;
                if (wm.irx >= 0f & wm.irx <= 1024f)
                    mousex = Scale(wm.irx * wm.irx * wm.irx / 1024f / 1024f * viewpower3x + wm.irx * wm.irx / 1024f * viewpower2x + wm.irx * viewpower1x, 0f, 1024f, dzx / 100f * 1024f, 1024f);
                if (wm.irx <= 0f & wm.irx >= -1024f)
                    mousex = Scale(-(-wm.irx * -wm.irx * -wm.irx) / 1024f / 1024f * viewpower3x - (-wm.irx * -wm.irx) / 1024f * viewpower2x - (-wm.irx) * viewpower1x, -1024f, 0f, -1024f, -(dzx / 100f) * 1024f);
                if (wm.iry >= 0f & wm.iry <= 1024f)
                    mousey = Scale(wm.iry * wm.iry * wm.iry / 1024f / 1024f * viewpower3y + wm.iry * wm.iry / 1024f * viewpower2y + wm.iry * viewpower1y, 0f, 1024f, dzy / 100f * 1024f, 1024f);
                if (wm.iry <= 0f & wm.iry >= -1024f)
                    mousey = Scale(-(-wm.iry * -wm.iry * -wm.iry) / 1024f / 1024f * viewpower3y - (-wm.iry * -wm.iry) / 1024f * viewpower2y - (-wm.iry) * viewpower1y, -1024f, 0f, -1024f, -(dzy / 100f) * 1024f);
                Controller_Send_rightstickx = (short)(-mousex / 1024f * 32767f);
                Controller_Send_rightsticky = (short)(-mousey / 1024f * 32767f);
                if (!wm.WiimoteButtonStateOne)
                {
                    if (!wm.WiimoteButtonStateLeft)
                    {
                        if (wm.WiimoteNunchuckStateRawJoystickX > 42f)
                            Controller_Send_leftstickx = 32767;
                        if (wm.WiimoteNunchuckStateRawJoystickX < -42f)
                            Controller_Send_leftstickx = -32767;
                        if (wm.WiimoteNunchuckStateRawJoystickX <= 42f & wm.WiimoteNunchuckStateRawJoystickX >= -42f)
                            Controller_Send_leftstickx = 0;
                        if (wm.WiimoteNunchuckStateRawJoystickY > 42f)
                            Controller_Send_leftsticky = 32767;
                        if (wm.WiimoteNunchuckStateRawJoystickY < -42f)
                            Controller_Send_leftsticky = -32767;
                        if (wm.WiimoteNunchuckStateRawJoystickY <= 42f & wm.WiimoteNunchuckStateRawJoystickY >= -42f)
                            Controller_Send_leftsticky = 0;
                        Controller_Send_right = false;
                        Controller_Send_left  = false;
                        Controller_Send_up    = false;
                        Controller_Send_down  = false;
                    }
                    else
                    {
                        Controller_Send_leftstickx = 0;
                        Controller_Send_leftsticky = 0;
                        Controller_Send_right      = wm.WiimoteNunchuckStateRawJoystickX >= 42f;
                        Controller_Send_left       = wm.WiimoteNunchuckStateRawJoystickX <= -42f;
                        Controller_Send_up         = wm.WiimoteNunchuckStateRawJoystickY >= 42f;
                        Controller_Send_down       = wm.WiimoteNunchuckStateRawJoystickY <= -42f;
                    }
                }
                XBC.Set(Controller_Send_back, Controller_Send_start, Controller_Send_A, Controller_Send_B, Controller_Send_X, Controller_Send_Y, Controller_Send_up, Controller_Send_left, Controller_Send_down, Controller_Send_right, Controller_Send_leftstick, Controller_Send_rightstick, Controller_Send_leftbumper, Controller_Send_rightbumper, Controller_Send_leftstickx, Controller_Send_leftsticky, Controller_Send_rightstickx, Controller_Send_rightsticky, Controller_Send_lefttriggerposition, Controller_Send_righttriggerposition, Controller_Send_xbox);
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