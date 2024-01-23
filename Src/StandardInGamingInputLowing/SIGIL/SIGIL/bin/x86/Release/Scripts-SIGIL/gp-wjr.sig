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
using JoyconsRightAPI;
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
        private static bool controller1_send_back, controller1_send_start, controller1_send_A, controller1_send_B, controller1_send_X, controller1_send_Y, controller1_send_up, controller1_send_left, controller1_send_down, controller1_send_right, controller1_send_leftstick, controller1_send_rightstick, controller1_send_leftbumper, controller1_send_rightbumper, controller1_send_xbox;
        private static double controller1_send_leftstickx, controller1_send_leftsticky, controller1_send_rightstickx, controller1_send_rightsticky, controller1_send_lefttriggerposition, controller1_send_righttriggerposition;
        private static double statex = 0f, statey = 0f, mousex = 0f, mousey = 0f, mousestatex = 0f, mousestatey = 0f, dzx = 0.0f, dzy = 0.0f, viewpower1x = 0f, viewpower2x = 1f, viewpower3x = 0f, viewpower1y = 0.25f, viewpower2y = 0.75f, viewpower3y = 0f, viewpower05x = 0f, viewpower05y = 0f;
        private static bool[] getstate = new bool[12];
        private static int sleeptime = 1;
        private static int irmode = 2;
        private static double centery = 80f;
        private XBoxController XBC = new XBoxController();
        private static JoyconRight jr = new JoyconRight();
        private static WiiMote wm = new WiiMote();
        public static Valuechange ValueChange = new Valuechange();
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
                jr.Close();
                wm.Close();
                XBC.Disconnect();
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
            jr.Scan();
            wm.Scan(irmode, centery);
            jr.BeginPolling();
            wm.BeginPolling();
            Thread.Sleep(1000);
            jr.Init();
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
                controller1_send_down                 = jr.JoyconRightButtonDPAD_DOWN;
                controller1_send_left                 = jr.JoyconRightButtonDPAD_LEFT;
                controller1_send_right                = jr.JoyconRightButtonDPAD_RIGHT;
                controller1_send_up                   = jr.JoyconRightButtonDPAD_UP;
                controller1_send_rightstick           = jr.JoyconRightAccelY <= -1.13;
                controller1_send_leftstick            = jr.JoyconRightButtonSHOULDER_2;
                controller1_send_A                    = jr.JoyconRightButtonSHOULDER_1;
                controller1_send_back                 = wm.WiimoteButtonStateOne;
                controller1_send_start                = wm.WiimoteButtonStateTwo;
                controller1_send_X                    = wm.WiimoteButtonStateHome | ((wm.WiimoteRawValuesZ > 0 ? wm.WiimoteRawValuesZ : -wm.WiimoteRawValuesZ) >= 30f & (wm.WiimoteRawValuesY > 0 ? wm.WiimoteRawValuesY : -wm.WiimoteRawValuesY) >= 30f & (wm.WiimoteRawValuesX > 0 ? wm.WiimoteRawValuesX : -wm.WiimoteRawValuesX) >= 30f);
                controller1_send_rightbumper          = wm.WiimoteButtonStatePlus | wm.WiimoteButtonStateUp;
                controller1_send_leftbumper           = wm.WiimoteButtonStateMinus | wm.WiimoteButtonStateUp;
                controller1_send_B                    = wm.WiimoteButtonStateDown;
                controller1_send_Y                    = wm.WiimoteButtonStateLeft | wm.WiimoteButtonStateRight;
                controller1_send_righttriggerposition = wm.WiimoteButtonStateB ? 255 : 0;
                valchanged(0, wm.WiimoteButtonStateA);
                if (wd[0] == 1 & !getstate[0])
                {
                    getstate[0] = true;
                }
                else
                {
                    if (wd[0] == 1 & getstate[0])
                    {
                        getstate[0] = false;
                    }
                }
                if (controller1_send_X | controller1_send_Y | controller1_send_rightbumper | controller1_send_leftbumper | controller1_send_rightstick | controller1_send_leftstick | controller1_send_back | controller1_send_start)
                {
                    getstate[0] = false;
                }
                controller1_send_lefttriggerposition = getstate[0] ? 255 : 0;
                if (wm.irx >= 0f & wm.irx <= 1024f)
                    mousex = Scale(wm.irx * wm.irx * wm.irx / 1024f / 1024f * viewpower3x + wm.irx * wm.irx / 1024f * viewpower2x + wm.irx * viewpower1x, 0f, 1024f, dzx / 100f * 1024f, 1024f);
                if (wm.irx <= 0f & wm.irx >= -1024f)
                    mousex = Scale(-(-wm.irx * -wm.irx * -wm.irx) / 1024f / 1024f * viewpower3x - (-wm.irx * -wm.irx) / 1024f * viewpower2x - (-wm.irx) * viewpower1x, -1024f, 0f, -1024f, -(dzx / 100f) * 1024f);
                if (wm.iry >= 0f & wm.iry <= 1024f)
                    mousey = Scale(wm.iry * wm.iry * wm.iry / 1024f / 1024f * viewpower3y + wm.iry * wm.iry / 1024f * viewpower2y + wm.iry * viewpower1y, 0f, 1024f, dzy / 100f * 1024f, 1024f);
                if (wm.iry <= 0f & wm.iry >= -1024f)
                    mousey = Scale(-(-wm.iry * -wm.iry * -wm.iry) / 1024f / 1024f * viewpower3y - (-wm.iry * -wm.iry) / 1024f * viewpower2y - (-wm.iry) * viewpower1y, -1024f, 0f, -1024f, -(dzy / 100f) * 1024f);
                controller1_send_rightstickx = (short)(-mousex / 1024f * 32767f);
                controller1_send_rightsticky = (short)(-mousey / 1024f * 32767f);
                if (jr.JoyconRightStickX > 0.35f)
                    controller1_send_leftstickx = 32767;
                if (jr.JoyconRightStickX < -0.35f)
                    controller1_send_leftstickx = -32767;
                if (jr.JoyconRightStickX <= 0.35f & jr.JoyconRightStickX >= -0.35f)
                    controller1_send_leftstickx = 0;
                if (jr.JoyconRightStickY > 0.35f)
                    controller1_send_leftsticky = 32767;
                if (jr.JoyconRightStickY < -0.35f)
                    controller1_send_leftsticky = -32767;
                if (jr.JoyconRightStickY <= 0.35f & jr.JoyconRightStickY >= -0.35f)
                    controller1_send_leftsticky = 0;
                XBC.Set(controller1_send_back, controller1_send_start, controller1_send_A, controller1_send_B, controller1_send_X, controller1_send_Y, controller1_send_up, controller1_send_left, controller1_send_down, controller1_send_right, controller1_send_leftstick, controller1_send_rightstick, controller1_send_leftbumper, controller1_send_rightbumper, controller1_send_leftstickx, controller1_send_leftsticky, controller1_send_rightstickx, controller1_send_rightsticky, controller1_send_lefttriggerposition, controller1_send_righttriggerposition, controller1_send_xbox);
                /*jr.ViewData();*/
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