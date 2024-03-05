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
using JoyconsLeftAPI;
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
        private static double statex = 0f, statey = 0f, mousex = 0f, mousey = 0f, mousestatex = 0f, mousestatey = 0f, dzx = 0.0f, dzy = 0.0f, viewpower1x = 0f, viewpower2x = 1f, viewpower3x = 0f, viewpower1y = 0.25f, viewpower2y = 0.75f, viewpower3y = 0f, viewpower05x = 0f, viewpower05y = 0f;
        private static bool[] getstate = new bool[12];
        private static int sleeptime = 1;
        private static int irmode = 2;
        private static double centery = 80f;
        private XBoxController XBC = new XBoxController();
        private static JoyconLeft jl = new JoyconLeft();
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
                jl.Close();
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
            jl.Scan();
            wm.Scan(irmode, centery);
            jl.BeginPolling();
            wm.BeginPolling();
            Thread.Sleep(1000);
            jl.Init();
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
                Controller_Send_down                 = jl.JoyconLeftButtonDPAD_DOWN;
                Controller_Send_left                 = jl.JoyconLeftButtonDPAD_LEFT;
                Controller_Send_right                = jl.JoyconLeftButtonDPAD_RIGHT;
                Controller_Send_up                   = jl.JoyconLeftButtonDPAD_UP;
                Controller_Send_rightstick           = jl.JoyconLeftAccelY <= -1.13;
                Controller_Send_leftstick            = jl.JoyconLeftButtonSHOULDER_2;
                Controller_Send_A                    = jl.JoyconLeftButtonSHOULDER_1;
                Controller_Send_back                 = wm.WiimoteButtonStateOne;
                Controller_Send_start                = wm.WiimoteButtonStateTwo;
                Controller_Send_X                    = wm.WiimoteButtonStateHome | ((wm.WiimoteRawValuesZ > 0 ? wm.WiimoteRawValuesZ : -wm.WiimoteRawValuesZ) >= 30f & (wm.WiimoteRawValuesY > 0 ? wm.WiimoteRawValuesY : -wm.WiimoteRawValuesY) >= 30f & (wm.WiimoteRawValuesX > 0 ? wm.WiimoteRawValuesX : -wm.WiimoteRawValuesX) >= 30f);
                Controller_Send_rightbumper          = wm.WiimoteButtonStatePlus | wm.WiimoteButtonStateUp;
                Controller_Send_leftbumper           = wm.WiimoteButtonStateMinus | wm.WiimoteButtonStateUp;
                Controller_Send_B                    = wm.WiimoteButtonStateDown;
                Controller_Send_Y                    = wm.WiimoteButtonStateLeft | wm.WiimoteButtonStateRight;
                Controller_Send_righttriggerposition = wm.WiimoteButtonStateB ? 255 : 0;
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
                if (Controller_Send_X | Controller_Send_Y | Controller_Send_rightbumper | Controller_Send_leftbumper | Controller_Send_rightstick | Controller_Send_leftstick | Controller_Send_back | Controller_Send_start)
                {
                    getstate[0] = false;
                }
                Controller_Send_lefttriggerposition = getstate[0] ? 255 : 0;
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
                if (jl.JoyconLeftStickX > 0.35f)
                    Controller_Send_leftstickx = 32767;
                if (jl.JoyconLeftStickX < -0.35f)
                    Controller_Send_leftstickx = -32767;
                if (jl.JoyconLeftStickX <= 0.35f & jl.JoyconLeftStickX >= -0.35f)
                    Controller_Send_leftstickx = 0;
                if (jl.JoyconLeftStickY > 0.35f)
                    Controller_Send_leftsticky = 32767;
                if (jl.JoyconLeftStickY < -0.35f)
                    Controller_Send_leftsticky = -32767;
                if (jl.JoyconLeftStickY <= 0.35f & jl.JoyconLeftStickY >= -0.35f)
                    Controller_Send_leftsticky = 0;
                XBC.Set(Controller_Send_back, Controller_Send_start, Controller_Send_A, Controller_Send_B, Controller_Send_X, Controller_Send_Y, Controller_Send_up, Controller_Send_left, Controller_Send_down, Controller_Send_right, Controller_Send_leftstick, Controller_Send_rightstick, Controller_Send_leftbumper, Controller_Send_rightbumper, Controller_Send_leftstickx, Controller_Send_leftsticky, Controller_Send_rightstickx, Controller_Send_rightsticky, Controller_Send_lefttriggerposition, Controller_Send_righttriggerposition, Controller_Send_xbox);
                /*jl.ViewData();*/
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