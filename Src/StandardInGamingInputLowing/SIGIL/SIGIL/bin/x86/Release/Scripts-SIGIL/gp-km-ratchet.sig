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
using MouseHooksAPI;
using KeyboardHooksAPI;
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
        private static bool Controller_Send_back, Controller_Send_start, Controller_Send_A, Controller_Send_B, Controller_Send_X, Controller_Send_Y, Controller_Send_up, Controller_Send_left, Controller_Send_down, Controller_Send_right, Controller_Send_leftstick, Controller_Send_rightstick, Controller_Send_leftbumper, Controller_Send_rightbumper, Controller_Send_lefttrigger, Controller_Send_righttrigger, Controller_Send_xbox;
        private static double Controller_Send_leftstickx, Controller_Send_leftsticky, Controller_Send_rightstickx, Controller_Send_rightsticky, Controller_Send_lefttriggerposition, Controller_Send_righttriggerposition;
        private static int width = System.Windows.Forms.Screen.PrimaryScreen.Bounds.Width, height = System.Windows.Forms.Screen.PrimaryScreen.Bounds.Height;
        private static double statex = 0f, statey = 0f, mousex = 0f, mousey = 0f, mousestatex = 0f, mousestatey = 0f, dzx = 0.0f, dzy = 0.0f, viewpower1x = 0f, viewpower2x = 1f, viewpower3x = 0f, viewpower1y = 0.25f, viewpower2y = 0.75f, viewpower3y = 0f, viewpower05x = 0f, viewpower05y = 0f;
        private static bool[] getstate = new bool[12];
        private static int sleeptime = 1;
        private XBoxController XBC = new XBoxController();
        private MouseHooks mh = new MouseHooks();
        private KeyboardHooks kh = new KeyboardHooks();
        private ScalingFactor sf = new ScalingFactor();
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
                mh.Close();
                kh.Close();
                XBC.Disconnect();
                sf.Close();
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
            mh.Scan();
            kh.Scan();
            mh.BeginPolling();
            kh.BeginPolling();
            XBC.Connect();
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
                valchanged(0, kh.Key_ADD);
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
                if (getstate[0])
                {
                    statex = (width / 2f - mh.MouseX / sf.scalingfactorx) * 1024f * 2f / width;
                    statey = -(height / 2f - mh.MouseY / sf.scalingfactory) * 1024f * 2f / height;
                    if (statex >= 1024f)
                        statex = 1024f;
                    if (statex <= -1024f)
                        statex = -1024f;
                    if (statey >= 1024f)
                        statey = 1024f;
                    if (statey <= -1024f)
                        statey = -1024f;
                    if (statex >= 0f)
                        mousex = Scale(Math.Pow(statex, 3f) / Math.Pow(1024f, 2f) * viewpower3x + Math.Pow(statex, 2f) / Math.Pow(1024f, 1f) * viewpower2x + Math.Pow(statex, 1f) / Math.Pow(1024f, 0f) * viewpower1x + Math.Pow(statex, 0.5f) * Math.Pow(1024f, 0.5f) * viewpower05x, 0f, 1024f, (dzx / 100f) * 1024f, 1024f);
                    if (statex <= 0f)
                        mousex = Scale(-Math.Pow(-statex, 3f) / Math.Pow(1024f, 2f) * viewpower3x - Math.Pow(-statex, 2f) / Math.Pow(1024f, 1f) * viewpower2x - Math.Pow(-statex, 1f) / Math.Pow(1024f, 0f) * viewpower1x - Math.Pow(-statex, 0.5f) * Math.Pow(1024f, 0.5f) * viewpower05x, -1024f, 0f, -1024f, -(dzx / 100f) * 1024f);
                    if (statey >= 0f)
                        mousey = Scale(Math.Pow(statey, 3f) / Math.Pow(1024f, 2f) * viewpower3y + Math.Pow(statey, 2f) / Math.Pow(1024f, 1f) * viewpower2y + Math.Pow(statey, 1f) / Math.Pow(1024f, 0f) * viewpower1y + Math.Pow(statey, 0.5f) * Math.Pow(1024f, 0.5f) * viewpower05y, 0f, 1024f, (dzy / 100f) * 1024f, 1024f);
                    if (statey <= 0f)
                        mousey = Scale(-Math.Pow(-statey, 3f) / Math.Pow(1024f, 2f) * viewpower3y - Math.Pow(-statey, 2f) / Math.Pow(1024f, 1f) * viewpower2y - Math.Pow(-statey, 1f) / Math.Pow(1024f, 0f) * viewpower1y - Math.Pow(-statey, 0.5f) * Math.Pow(1024f, 0.5f) * viewpower05y, -1024f, 0f, -1024f, -(dzy / 100f) * 1024f);
                    Controller_Send_rightstickx          = Math.Abs(-mousex * 32767f / 1024f) <= 32767f ? -mousex * 32767f / 1024f : Math.Sign(-mousex) * 32767f;
                    Controller_Send_rightsticky          = Math.Abs(-mousey * 32767f / 1024f) <= 32767f ? -mousey * 32767f / 1024f : Math.Sign(-mousey) * 32767f;
                    Controller_Send_left                 = kh.Key_Z;
                    Controller_Send_right                = kh.Key_V;
                    Controller_Send_down                 = kh.Key_C;
                    Controller_Send_up                   = kh.Key_X;
                    Controller_Send_rightstick           = kh.Key_E;
                    Controller_Send_leftstick            = kh.Key_LeftShift;
                    Controller_Send_A                    = kh.Key_Space;
                    Controller_Send_back                 = kh.Key_Tab;
                    Controller_Send_start                = kh.Key_Escape;
                    Controller_Send_X                    = kh.Key_R | mh.MouseMiddleButton;
                    Controller_Send_rightbumper          = kh.Key_G | mh.MouseXButton;
                    Controller_Send_leftbumper           = kh.Key_T;
                    Controller_Send_B                    = kh.Key_LeftControl | kh.Key_Q;
                    Controller_Send_Y                    = mh.MouseZ > 0 | mh.MouseZ < 0;
                    Controller_Send_righttriggerposition = mh.MouseLeftButton ? 255 : 0;
                    if (kh.Key_W)
                        Controller_Send_leftsticky = 32767;
                    if (kh.Key_S)
                        Controller_Send_leftsticky = -32767;
                    if ((!kh.Key_W & !kh.Key_S) | (kh.Key_W & kh.Key_S))
                        Controller_Send_leftsticky = 0;
                    if (kh.Key_D)
                        Controller_Send_leftstickx = 32767;
                    if (kh.Key_A)
                        Controller_Send_leftstickx = -32767;
                    if ((!kh.Key_D & !kh.Key_A) | (kh.Key_D & kh.Key_A))
                        Controller_Send_leftstickx = 0;
                    valchanged(1, mh.MouseRightButton);
                    if (wd[1] == 1 & !getstate[1])
                    {
                        getstate[1] = true;
                    }
                    else
                    {
                        if (wd[1] == 1 & getstate[1])
                        {
                            getstate[1] = false;
                        }
                    }
                    if (Controller_Send_X | Controller_Send_Y | Controller_Send_rightbumper | Controller_Send_leftbumper | Controller_Send_rightstick | Controller_Send_leftstick | Controller_Send_back | Controller_Send_start)
                    {
                        getstate[1] = false;
                    }
                    Controller_Send_lefttriggerposition = getstate[1] ? 255 : 0;
                }
                else
                {
                    Controller_Send_rightstickx = 0;
                    Controller_Send_rightsticky = 0;
                    Controller_Send_leftstickx = 0;
                    Controller_Send_leftsticky = 0;
                    Controller_Send_left = false;
                    Controller_Send_right = false;
                    Controller_Send_down = false;
                    Controller_Send_up = false;
                    Controller_Send_rightstick = false;
                    Controller_Send_leftstick = false;
                    Controller_Send_A = false;
                    Controller_Send_back = false;
                    Controller_Send_start = false;
                    Controller_Send_X = false;
                    Controller_Send_rightbumper = false;
                    Controller_Send_leftbumper = false;
                    Controller_Send_B = false;
                    Controller_Send_Y = false;
                    Controller_Send_lefttriggerposition = 0;
                    Controller_Send_righttriggerposition = 0;
                }
                XBC.Set(Controller_Send_back, Controller_Send_start, Controller_Send_A, Controller_Send_B, Controller_Send_X, Controller_Send_Y, Controller_Send_up, Controller_Send_left, Controller_Send_down, Controller_Send_right, Controller_Send_leftstick, Controller_Send_rightstick, Controller_Send_leftbumper, Controller_Send_rightbumper, Controller_Send_leftstickx, Controller_Send_leftsticky, Controller_Send_rightstickx, Controller_Send_rightsticky, Controller_Send_lefttriggerposition, Controller_Send_righttriggerposition, Controller_Send_xbox);
                /*mh.ViewData();*/
                /*kh.ViewData();*/
                /*sf.ViewData();*/
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