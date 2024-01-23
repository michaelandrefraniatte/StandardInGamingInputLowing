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
        private static JoyconRight jr = new JoyconRight();
        public static Valuechange ValueChange = new Valuechange();
        public void Close()
        {
            try
            {
                running = false;
                Thread.Sleep(100);
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
            jr.Scan();
            jr.BeginPolling();
            Thread.Sleep(1000);
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
                    jr.Init();
                }
                if (jr.JoyconRightAccelX >= 1024f)
                    jr.JoyconRightAccelX = 1024f;
                if (jr.JoyconRightAccelX <= -1024f)
                    jr.JoyconRightAccelX = -1024f;
                if (jr.JoyconRightAccelY >= 1024f)
                    jr.JoyconRightAccelY = 1024f;
                if (jr.JoyconRightAccelY <= -1024f)
                    jr.JoyconRightAccelY = -1024f;
                if (jr.JoyconRightAccelX > 0f & jr.JoyconRightAccelX <= 1024f)
                    mousex = Scale(jr.JoyconRightAccelX, 0f, 1024f, (dzx / 100f) * 1024f, 1024f);
                if (jr.JoyconRightAccelX < 0f & jr.JoyconRightAccelX >= -1024f)
                    mousex = Scale(jr.JoyconRightAccelX, -1024f, 0f, -1024f, -(dzx / 100f) * 1024f);
                if (jr.JoyconRightAccelY > 0f & jr.JoyconRightAccelY <= 1024f)
                    mousey = Scale(jr.JoyconRightAccelY, 0f, 1024f, (dzy / 100f) * 1024f, 1024f);
                if (jr.JoyconRightAccelY < 0f & jr.JoyconRightAccelY >= -1024f)
                    mousey = Scale(jr.JoyconRightAccelY, -1024f, 0f, -1024f, -(dzy / 100f) * 1024f);
                controller1_send_leftstickx           = -mousex * 32767f / 1024f;
                controller1_send_leftsticky           = -mousey * 32767f / 1024f;
                controller1_send_A                    = jr.JoyconRightButtonHOME;
                controller1_send_lefttriggerposition  = jr.JoyconRightButtonDPAD_RIGHT ? 255 : 0;
                controller1_send_righttriggerposition = jr.JoyconRightButtonDPAD_UP ? 255 : 0;
                controller1_send_Y                    = jr.JoyconRightButtonDPAD_LEFT;
                controller1_send_back                 = jr.JoyconRightButtonDPAD_DOWN;
                controller1_send_start                = jr.JoyconRightButtonPLUS;
                controller1_send_rightstick           = jr.JoyconRightButtonSTICK;
                controller1_send_leftbumper           = jr.JoyconRightButtonSHOULDER_1;
                controller1_send_rightbumper          = jr.JoyconRightButtonSHOULDER_2;
                controller1_send_B                    = jr.JoyconRightButtonSL;
                controller1_send_X                    = jr.JoyconRightButtonSR;
                if (jr.JoyconRightStickY > 0.35f) 
                    controller1_send_rightstickx = 32767;
                if (jr.JoyconRightStickY < -0.35f) 
                    controller1_send_rightstickx = -32767;
                if (jr.JoyconRightStickY <= 0.35f & jr.JoyconRightStickY >= -0.35f) 
                    controller1_send_rightstickx = 0;
                if (jr.JoyconRightStickX > 0.35f) 
                    controller1_send_rightsticky = -32767;
                if (jr.JoyconRightStickX < -0.35f) 
                    controller1_send_rightsticky = 32767;
                if (jr.JoyconRightStickX <= 0.35f & jr.JoyconRightStickX >= -0.35f) 
                    controller1_send_rightsticky = 0;
                XBC.Set(controller1_send_back, controller1_send_start, controller1_send_A, controller1_send_B, controller1_send_X, controller1_send_Y, controller1_send_up, controller1_send_left, controller1_send_down, controller1_send_right, controller1_send_leftstick, controller1_send_rightstick, controller1_send_leftbumper, controller1_send_rightbumper, controller1_send_leftstickx, controller1_send_leftsticky, controller1_send_rightstickx, controller1_send_rightsticky, controller1_send_lefttriggerposition, controller1_send_righttriggerposition, controller1_send_xbox);
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