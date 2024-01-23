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
        private static bool controller1_send_back, controller1_send_start, controller1_send_A, controller1_send_B, controller1_send_X, controller1_send_Y, controller1_send_up, controller1_send_left, controller1_send_down, controller1_send_right, controller1_send_leftstick, controller1_send_rightstick, controller1_send_leftbumper, controller1_send_rightbumper, controller1_send_lefttrigger, controller1_send_righttrigger, controller1_send_xbox;
        private static double controller1_send_leftstickx, controller1_send_leftsticky, controller1_send_rightstickx, controller1_send_rightsticky, controller1_send_lefttriggerposition, controller1_send_righttriggerposition;
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
                controller1_send_leftstickx           = mousex * 32767f / 1024f;
                controller1_send_leftsticky           = -mousey * 32767f / 1024f;
                controller1_send_A                    = jl.JoyconLeftButtonCAPTURE;
                controller1_send_lefttriggerposition  = jl.JoyconLeftButtonDPAD_LEFT ? 255 : 0;
                controller1_send_righttriggerposition = jl.JoyconLeftButtonDPAD_DOWN ? 255 : 0;
                controller1_send_Y                    = jl.JoyconLeftButtonDPAD_RIGHT;
                controller1_send_back                 = jl.JoyconLeftButtonDPAD_UP;
                controller1_send_start                = jl.JoyconLeftButtonMINUS;
                controller1_send_rightstick           = jl.JoyconLeftButtonSTICK;
                controller1_send_leftbumper           = jl.JoyconLeftButtonSHOULDER_1;
                controller1_send_rightbumper          = jl.JoyconLeftButtonSHOULDER_2;
                controller1_send_B                    = jl.JoyconLeftButtonSR;
                controller1_send_X                    = jl.JoyconLeftButtonSL;
                if (jl.JoyconLeftStickY > 0.35f) 
                    controller1_send_rightstickx = -32767;
                if (jl.JoyconLeftStickY < -0.35f) 
                    controller1_send_rightstickx = 32767;
                if (jl.JoyconLeftStickY <= 0.35f & jl.JoyconLeftStickY >= -0.35f) 
                    controller1_send_rightstickx = 0;
                if (jl.JoyconLeftStickX > 0.35f) 
                    controller1_send_rightsticky = 32767;
                if (jl.JoyconLeftStickX < -0.35f) 
                    controller1_send_rightsticky = -32767;
                if (jl.JoyconLeftStickX <= 0.35f & jl.JoyconLeftStickX >= -0.35f) 
                    controller1_send_rightsticky = 0;
                XBC.Set(controller1_send_back, controller1_send_start, controller1_send_A, controller1_send_B, controller1_send_X, controller1_send_Y, controller1_send_up, controller1_send_left, controller1_send_down, controller1_send_right, controller1_send_leftstick, controller1_send_rightstick, controller1_send_leftbumper, controller1_send_rightbumper, controller1_send_leftstickx, controller1_send_leftsticky, controller1_send_rightstickx, controller1_send_rightsticky, controller1_send_lefttriggerposition, controller1_send_righttriggerposition, controller1_send_xbox);
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