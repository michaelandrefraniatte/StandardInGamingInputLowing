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
        private static bool controller1_send_back, controller1_send_start, controller1_send_A, controller1_send_B, controller1_send_X, controller1_send_Y, controller1_send_up, controller1_send_left, controller1_send_down, controller1_send_right, controller1_send_leftstick, controller1_send_rightstick, controller1_send_leftbumper, controller1_send_rightbumper, controller1_send_lefttrigger, controller1_send_righttrigger, controller1_send_xbox;
        private static double controller1_send_leftstickx, controller1_send_leftsticky, controller1_send_rightstickx, controller1_send_rightsticky, controller1_send_lefttriggerposition, controller1_send_righttriggerposition;
        private static double statex = 0f, statey = 0f, mousex = 0f, mousey = 0f, mousestatex = 0f, mousestatey = 0f, dzx = 0.0f, dzy = 0.0f, viewpower1x = 0f, viewpower2x = 1f, viewpower3x = 0f, viewpower1y = 0.25f, viewpower2y = 0.75f, viewpower3y = 0f, viewpower05x = 0f, viewpower05y = 0f;
        private static bool getstate;
        private static int sleeptime = 1;
        private XBoxController XBC = new XBoxController();
        private JoyconLeft jl = new JoyconLeft();
        private JoyconRight jr = new JoyconRight();
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
                mousex = (jl.JoyconLeftAccelY - jr.JoyconRightAccelY) * 13.5f;
                mousey = jl.JoyconLeftStickY * 32767f * 1.2f;
                statex = Math.Abs(mousex) <= 32767f ? mousex : Math.Sign(mousex) * 32767f;
                statey = Math.Abs(mousey) <= 32767f ? mousey : Math.Sign(mousey) * 32767f;
                if (statex > 0f)
                    mousestatex = Scale(statex, 0f, 32767f, dzx / 100f * 32767f, 32767f);
                if (statex < 0f)
                    mousestatex = Scale(statex, -32767f, 0f, -32767f, -(dzx / 100f) * 32767f);
                if (statey > 0f)
                    mousestatey = Scale(statey, 0f, 32767f, dzy / 100f * 32767f, 32767f);
                if (statey < 0f)
                    mousestatey = Scale(statey, -32767f, 0f, -32767f, -(dzy / 100f) * 32767f);
                mousex = mousestatex + jl.JoyconLeftStickX * 32767f * 1.2f;
                mousey = mousestatey;
                statex = Math.Abs(mousex) <= 32767f ? mousex : Math.Sign(mousex) * 32767f;
                statey = Math.Abs(mousey) <= 32767f ? mousey : Math.Sign(mousey) * 32767f;
                controller1_send_leftstickx = statex;
                controller1_send_leftsticky = statey;
                mousex = jr.JoyconRightStickX * 1400f;
                mousey = jr.JoyconRightStickY * 1400f;
                controller1_send_rightstickx = Math.Abs(-mousex * 32767f / 1024f) <= 32767f ? -mousex * 32767f / 1024f : Math.Sign(-mousex) * 32767f;
                controller1_send_rightsticky = Math.Abs(-mousey * 32767f / 1024f) <= 32767f ? -mousey * 32767f / 1024f : Math.Sign(-mousey) * 32767f;
                controller1_send_up = jl.JoyconLeftButtonDPAD_UP;
                controller1_send_left = jl.JoyconLeftButtonDPAD_LEFT;
                controller1_send_down = jl.JoyconLeftButtonDPAD_DOWN;
                controller1_send_right = jl.JoyconLeftButtonDPAD_RIGHT;
                controller1_send_back = jl.JoyconLeftButtonMINUS | jr.JoyconRightButtonHOME;
                controller1_send_start = jl.JoyconLeftButtonCAPTURE | jr.JoyconRightButtonPLUS;
                controller1_send_leftstick = jl.JoyconLeftButtonSTICK;
                controller1_send_leftbumper = jl.JoyconLeftButtonSL | jl.JoyconLeftButtonSHOULDER_1 | jr.JoyconRightButtonSL;
                controller1_send_rightbumper = jl.JoyconLeftButtonSR | jr.JoyconRightButtonSHOULDER_1 | jr.JoyconRightButtonSR;
                controller1_send_A = jr.JoyconRightButtonDPAD_DOWN;
                controller1_send_B = jr.JoyconRightButtonDPAD_RIGHT;
                controller1_send_X = jr.JoyconRightButtonDPAD_LEFT;
                controller1_send_Y = jr.JoyconRightButtonDPAD_UP;
                controller1_send_rightstick = jr.JoyconRightButtonSTICK;
                controller1_send_lefttriggerposition = jl.JoyconLeftButtonSHOULDER_2 ? 255 : 0;
                controller1_send_righttriggerposition = jr.JoyconRightButtonSHOULDER_2 ? 255 : 0;
                XBC.SetController(controller1_send_back, controller1_send_start, controller1_send_A, controller1_send_B, controller1_send_X, controller1_send_Y, controller1_send_up, controller1_send_left, controller1_send_down, controller1_send_right, controller1_send_leftstick, controller1_send_rightstick, controller1_send_leftbumper, controller1_send_rightbumper, controller1_send_leftstickx, controller1_send_leftsticky, controller1_send_rightstickx, controller1_send_rightsticky, controller1_send_lefttriggerposition, controller1_send_righttriggerposition, controller1_send_xbox);
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