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
        private static double statex = 0f, statey = 0f, mousex = 0f, mousey = 0f, mousestatex = 0f, mousestatey = 0f, dzx = 20.0f, dzy = 0.0f, viewpower1x = 0f, viewpower2x = 1f, viewpower3x = 0f, viewpower1y = 0.25f, viewpower2y = 0.75f, viewpower3y = 0f, viewpower05x = 0f, viewpower05y = 0f;
        private static bool getstate;
        private static int sleeptime = 1;
        private XBoxController XBC = new XBoxController();
        private JoyconLeft jl = new JoyconLeft();
        public static Valuechange ValueChange = new Valuechange();
        public void Close()
        {
            try
            {
                running = false;
                Thread.Sleep(100);
                jl.Close();
                XBC.Disconnect();
                jl.Dispose();
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
            jl.Scan();
            jl.BeginPolling();
            Thread.Sleep(1000);
            jl.Init();
            XBC.Connect();
            Task.Run(() => task());
        }
        private void task()
        {
            for (; ; )
            {
                if (!running)
                    break;
                if (jl.JoyconLeftButtonMINUS)
                {
                    jl.Init();
                }
                if (jl.JoyconLeftAccelX >= 1024f)
                    jl.JoyconLeftAccelX = 1024f;
                if (jl.JoyconLeftAccelX <= -1024f)
                    jl.JoyconLeftAccelX = -1024f;
                if (jl.JoyconLeftAccelY >= 1024f)
                    jl.JoyconLeftAccelY = 1024f;
                if (jl.JoyconLeftAccelY <= -1024f)
                    jl.JoyconLeftAccelY = -1024f;
                if (jl.JoyconLeftAccelX > 0f & jl.JoyconLeftAccelX <= 1024f)
                    mousex = Scale(jl.JoyconLeftAccelX, 0f, 1024f, (dzx / 100f) * 1024f, 1024f);
                if (jl.JoyconLeftAccelX < 0f & jl.JoyconLeftAccelX >= -1024f)
                    mousex = Scale(jl.JoyconLeftAccelX, -1024f, 0f, -1024f, -(dzx / 100f) * 1024f);
                if (jl.JoyconLeftAccelY > 0f & jl.JoyconLeftAccelY <= 1024f)
                    mousey = Scale(jl.JoyconLeftAccelY, 0f, 1024f, (dzy / 100f) * 1024f, 1024f);
                if (jl.JoyconLeftAccelY < 0f & jl.JoyconLeftAccelY >= -1024f)
                    mousey = Scale(jl.JoyconLeftAccelY, -1024f, 0f, -1024f, -(dzy / 100f) * 1024f);
                Controller_Send_leftstickx           = mousex * 32767f / 1024f;
                Controller_Send_leftsticky           = -mousey * 32767f / 1024f;
                Controller_Send_A                    = jl.JoyconLeftButtonCAPTURE;
                Controller_Send_lefttriggerposition  = jl.JoyconLeftButtonDPAD_LEFT ? 255 : 0;
                Controller_Send_righttriggerposition = jl.JoyconLeftButtonDPAD_DOWN ? 255 : 0;
                Controller_Send_Y                    = jl.JoyconLeftButtonDPAD_RIGHT;
                Controller_Send_back                 = jl.JoyconLeftButtonDPAD_UP;
                Controller_Send_start                = jl.JoyconLeftButtonMINUS;
                Controller_Send_rightstick           = jl.JoyconLeftButtonSTICK;
                Controller_Send_leftbumper           = jl.JoyconLeftButtonSHOULDER_1;
                Controller_Send_rightbumper          = jl.JoyconLeftButtonSHOULDER_2;
                Controller_Send_B                    = jl.JoyconLeftButtonSR;
                Controller_Send_X                    = jl.JoyconLeftButtonSL;
                if (jl.JoyconLeftStickY > 0.35f) 
                    Controller_Send_rightstickx = -32767;
                if (jl.JoyconLeftStickY < -0.35f) 
                    Controller_Send_rightstickx = 32767;
                if (jl.JoyconLeftStickY <= 0.35f & jl.JoyconLeftStickY >= -0.35f) 
                    Controller_Send_rightstickx = 0;
                if (jl.JoyconLeftStickX > 0.35f) 
                    Controller_Send_rightsticky = 32767;
                if (jl.JoyconLeftStickX < -0.35f) 
                    Controller_Send_rightsticky = -32767;
                if (jl.JoyconLeftStickX <= 0.35f & jl.JoyconLeftStickX >= -0.35f) 
                    Controller_Send_rightsticky = 0;
                XBC.Set(Controller_Send_back, Controller_Send_start, Controller_Send_A, Controller_Send_B, Controller_Send_X, Controller_Send_Y, Controller_Send_up, Controller_Send_left, Controller_Send_down, Controller_Send_right, Controller_Send_leftstick, Controller_Send_rightstick, Controller_Send_leftbumper, Controller_Send_rightbumper, Controller_Send_leftstickx, Controller_Send_leftsticky, Controller_Send_rightstickx, Controller_Send_rightsticky, Controller_Send_lefttriggerposition, Controller_Send_righttriggerposition, Controller_Send_xbox);
                /*XBC.ViewData();*/
                /*jl.ViewData();*/
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