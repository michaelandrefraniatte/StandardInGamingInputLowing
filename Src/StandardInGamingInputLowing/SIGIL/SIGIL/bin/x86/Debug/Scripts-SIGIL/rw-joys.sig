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
using JoyconsRightAPI;
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
        private static bool getstate;
        private static int sleeptime = 1;
        private XBoxController XBC = new XBoxController();
        private static JoyconLeft jl = new JoyconLeft();
        private static JoyconRight jr = new JoyconRight();
        public static Valuechange ValueChange = new Valuechange();
        public void Close()
        {
            try
            {
                running = false;
                Thread.Sleep(100);
                jl.Close();
                jr.Close();
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
            jr.Scan();
            jl.BeginPolling();
            jr.BeginPolling();
            Thread.Sleep(1000);
            jl.Init();
            jr.Init();
            XBC.Connect();
            Task.Run(() => task());
        }
        private void task()
        {
            for (; ; )
            {
                if (!running)
                    break;
                if (jr.JoyconRightButtonPLUS)
                {
                    jl.Init();
                    jr.Init();
                }
                mousex                 = (jl.JoyconLeftAccelY - jr.JoyconRightAccelY) * 13.5f;
                mousey                 = jl.JoyconLeftStickY * 32767f * 1.2f;
                statex                 = Math.Abs(mousex) <= 32767f ? mousex : Math.Sign(mousex) * 32767f;
                statey                 = Math.Abs(mousey) <= 32767f ? mousey : Math.Sign(mousey) * 32767f;
                if (statex > 0f)
                    mousestatex = Scale(statex, 0f, 32767f, dzx / 100f * 32767f, 32767f);
                if (statex < 0f)
                    mousestatex = Scale(statex, -32767f, 0f, -32767f, -(dzx / 100f) * 32767f);
                if (statey > 0f)
                    mousestatey = Scale(statey, 0f, 32767f, dzy / 100f * 32767f, 32767f);
                if (statey < 0f)
                    mousestatey = Scale(statey, -32767f, 0f, -32767f, -(dzy / 100f) * 32767f);
                mousex                                = mousestatex + jl.JoyconLeftStickX * 32767f * 1.2f;
                mousey                                = mousestatey;
                statex                                = Math.Abs(mousex) <= 32767f ? mousex : Math.Sign(mousex) * 32767f;
                statey                                = Math.Abs(mousey) <= 32767f ? mousey : Math.Sign(mousey) * 32767f;
                Controller_Send_leftstickx           = statex;
                Controller_Send_leftsticky           = statey;
                mousex                                = jr.JoyconRightStickX * 1400f;
                mousey                                = jr.JoyconRightStickY * 1400f;
                Controller_Send_rightstickx          = Math.Abs(-mousex * 32767f / 1024f) <= 32767f ? -mousex * 32767f / 1024f : Math.Sign(-mousex) * 32767f;
                Controller_Send_rightsticky          = Math.Abs(-mousey * 32767f / 1024f) <= 32767f ? -mousey * 32767f / 1024f : Math.Sign(-mousey) * 32767f;
                Controller_Send_up                   = jl.JoyconLeftButtonDPAD_UP;
                Controller_Send_left                 = jl.JoyconLeftButtonDPAD_LEFT;
                Controller_Send_down                 = jl.JoyconLeftButtonDPAD_DOWN;
                Controller_Send_right                = jl.JoyconLeftButtonDPAD_RIGHT;
                Controller_Send_back                 = jl.JoyconLeftButtonMINUS | jr.JoyconRightButtonHOME;
                Controller_Send_start                = jl.JoyconLeftButtonCAPTURE | jr.JoyconRightButtonPLUS;
                Controller_Send_leftstick            = jl.JoyconLeftButtonSTICK;
                Controller_Send_leftbumper           = jl.JoyconLeftButtonSL | jl.JoyconLeftButtonSHOULDER_1 | jr.JoyconRightButtonSL;
                Controller_Send_rightbumper          = jl.JoyconLeftButtonSR | jr.JoyconRightButtonSHOULDER_1 | jr.JoyconRightButtonSR;
                Controller_Send_A                    = jr.JoyconRightButtonDPAD_DOWN;
                Controller_Send_B                    = jr.JoyconRightButtonDPAD_RIGHT;
                Controller_Send_X                    = jr.JoyconRightButtonDPAD_LEFT;
                Controller_Send_Y                    = jr.JoyconRightButtonDPAD_UP;
                Controller_Send_rightstick           = jr.JoyconRightButtonSTICK;
                Controller_Send_lefttriggerposition  = jl.JoyconLeftButtonSHOULDER_2 ? 255 : 0;
                Controller_Send_righttriggerposition = jr.JoyconRightButtonSHOULDER_2 ? 255 : 0;
                XBC.Set(Controller_Send_back, Controller_Send_start, Controller_Send_A, Controller_Send_B, Controller_Send_X, Controller_Send_Y, Controller_Send_up, Controller_Send_left, Controller_Send_down, Controller_Send_right, Controller_Send_leftstick, Controller_Send_rightstick, Controller_Send_leftbumper, Controller_Send_rightbumper, Controller_Send_leftstickx, Controller_Send_leftsticky, Controller_Send_rightstickx, Controller_Send_rightsticky, Controller_Send_lefttriggerposition, Controller_Send_righttriggerposition, Controller_Send_xbox);
                /*jl.ViewData();*/
                /*jr.ViewData();*/
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