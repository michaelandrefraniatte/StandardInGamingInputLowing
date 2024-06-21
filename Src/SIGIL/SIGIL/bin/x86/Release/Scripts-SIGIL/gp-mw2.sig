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
using WiiMotesLibAPI;
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
        private static double mousex = 0f, mousey = 0f, viewpower1x = 0f, viewpower2x = 1f, viewpower3x = 0f, viewpower1y = 0.25f, viewpower2y = 0.75f, viewpower3y = 0f, dzx = 2.0f, dzy = 2.0f, countup = 0, countupup = 0, countxy = 0, county = 0;
        private static bool getstate;
        private static int sleeptime = 1;
        private static int irmode = 2;
        private static double centery = 80f;
        private XBoxController XBC = new XBoxController();
        private WiiMoteLib wm = new WiiMoteLib();
        public static Valuechange ValueChange = new Valuechange();
        public void Close()
        {
            try
            {
                running = false;
                Thread.Sleep(100);
                wm.Close();
                XBC.Disconnect();
                wm.Dispose();
                XBC.Dispose();
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
                Controller_Send_righttriggerposition = wm.WiimoteButtonStateB ? 255 : 0;
                ValueChange[0] = wm.WiimoteButtonStateA ? 1 : 0;
                if (ValueChange._ValueChange[0] > 0f & !getstate)
                {
                    getstate = true;
                }
                else
                {
                    if (ValueChange._ValueChange[0] > 0f & getstate)
                    {
                        getstate = false;
                    }
                }
                if (Controller_Send_X | Controller_Send_Y | Controller_Send_rightbumper | Controller_Send_leftbumper | Controller_Send_rightstick | Controller_Send_leftstick | Controller_Send_back | Controller_Send_start)
                {
                    getstate = false;
                }
                Controller_Send_lefttriggerposition = getstate ? 255 : 0;
                Controller_Send_rightstick = wm.WiimoteNunchuckStateRawValuesY >= 90f;
                Controller_Send_leftstick = wm.WiimoteNunchuckStateZ;
                Controller_Send_A = wm.WiimoteNunchuckStateC;
                Controller_Send_back = wm.WiimoteButtonStateOne;
                Controller_Send_start = wm.WiimoteButtonStateTwo;
                ValueChange[1] = wm.WiimoteButtonStateRight ? 1 : 0;
                if ((countxy > 0 & countxy < 300 & ValueChange._ValueChange[1] < 0f) | county > 0)
                    county++;
                if (county > 100)
                    county = 0;
                countxy = wm.WiimoteButtonStateRight ? countxy + 1 : 0;
                Controller_Send_Y = county > 0;
                Controller_Send_X = countxy > 300 | ((wm.WiimoteRawValuesZ > 0 ? wm.WiimoteRawValuesZ : -wm.WiimoteRawValuesZ) >= 30f & (wm.WiimoteRawValuesY > 0 ? wm.WiimoteRawValuesY : -wm.WiimoteRawValuesY) >= 30f & (wm.WiimoteRawValuesX > 0 ? wm.WiimoteRawValuesX : -wm.WiimoteRawValuesX) >= 30f);
                Controller_Send_leftbumper = wm.WiimoteButtonStateMinus | wm.WiimoteButtonStateUp;
                Controller_Send_rightbumper = wm.WiimoteButtonStatePlus | wm.WiimoteButtonStateUp;
                Controller_Send_B = wm.WiimoteButtonStateDown;
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
                countup = wm.WiimoteButtonStateHome ? countup + 1 : 0;
                if ((countup > 0 & countup < 300) | countupup > 0)
                    countupup++;
                if (countupup > 300)
                    countupup = 0;
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
                        Controller_Send_left = false;
                        Controller_Send_up = (countupup > 0 & countupup < 100) | (countupup > 200 & countupup < 300) | countup > 300;
                        Controller_Send_down = false;
                    }
                    else
                    {
                        Controller_Send_leftstickx = 0;
                        Controller_Send_leftsticky = 0;
                        Controller_Send_right = wm.WiimoteNunchuckStateRawJoystickX >= 42f;
                        Controller_Send_left = wm.WiimoteNunchuckStateRawJoystickX <= -42f;
                        Controller_Send_up = wm.WiimoteNunchuckStateRawJoystickY >= 42f;
                        Controller_Send_down = wm.WiimoteNunchuckStateRawJoystickY <= -42f;
                    }
                }
                XBC.Set(Controller_Send_back, Controller_Send_start, Controller_Send_A, Controller_Send_B, Controller_Send_X, Controller_Send_Y, Controller_Send_up, Controller_Send_left, Controller_Send_down, Controller_Send_right, Controller_Send_leftstick, Controller_Send_rightstick, Controller_Send_leftbumper, Controller_Send_rightbumper, Controller_Send_leftstickx, Controller_Send_leftsticky, Controller_Send_rightstickx, Controller_Send_rightsticky, Controller_Send_lefttriggerposition, Controller_Send_righttriggerposition, Controller_Send_xbox);
                XBC.ViewData();
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