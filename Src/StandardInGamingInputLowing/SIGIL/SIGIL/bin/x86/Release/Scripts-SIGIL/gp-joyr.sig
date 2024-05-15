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
        private static bool Controller_Send_back, Controller_Send_start, Controller_Send_A, Controller_Send_B, Controller_Send_X, Controller_Send_Y, Controller_Send_up, Controller_Send_left, Controller_Send_down, Controller_Send_right, Controller_Send_leftstick, Controller_Send_rightstick, Controller_Send_leftbumper, Controller_Send_rightbumper, Controller_Send_xbox;
        private static double Controller_Send_leftstickx, Controller_Send_leftsticky, Controller_Send_rightstickx, Controller_Send_rightsticky, Controller_Send_lefttriggerposition, Controller_Send_righttriggerposition;
        private static double statex = 0f, statey = 0f, mousex = 0f, mousey = 0f, mousestatex = 0f, mousestatey = 0f, dzx = 20.0f, dzy = 0.0f, viewpower1x = 0f, viewpower2x = 1f, viewpower3x = 0f, viewpower1y = 0.25f, viewpower2y = 0.75f, viewpower3y = 0f, viewpower05x = 0f, viewpower05y = 0f;
        private static bool getstate;
        private static int sleeptime = 1;
        private XBoxController XBC = new XBoxController();
        private JoyconRight jr = new JoyconRight();
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
                Controller_Send_leftstickx           = -mousex * 32767f / 1024f;
                Controller_Send_leftsticky           = -mousey * 32767f / 1024f;
                Controller_Send_A                    = jr.JoyconRightButtonHOME;
                Controller_Send_lefttriggerposition  = jr.JoyconRightButtonDPAD_RIGHT ? 255 : 0;
                Controller_Send_righttriggerposition = jr.JoyconRightButtonDPAD_UP ? 255 : 0;
                Controller_Send_Y                    = jr.JoyconRightButtonDPAD_LEFT;
                Controller_Send_back                 = jr.JoyconRightButtonDPAD_DOWN;
                Controller_Send_start                = jr.JoyconRightButtonPLUS;
                Controller_Send_rightstick           = jr.JoyconRightButtonSTICK;
                Controller_Send_leftbumper           = jr.JoyconRightButtonSHOULDER_1;
                Controller_Send_rightbumper          = jr.JoyconRightButtonSHOULDER_2;
                Controller_Send_B                    = jr.JoyconRightButtonSL;
                Controller_Send_X                    = jr.JoyconRightButtonSR;
                if (jr.JoyconRightStickY > 0.35f) 
                    Controller_Send_rightstickx = 32767;
                if (jr.JoyconRightStickY < -0.35f) 
                    Controller_Send_rightstickx = -32767;
                if (jr.JoyconRightStickY <= 0.35f & jr.JoyconRightStickY >= -0.35f) 
                    Controller_Send_rightstickx = 0;
                if (jr.JoyconRightStickX > 0.35f) 
                    Controller_Send_rightsticky = -32767;
                if (jr.JoyconRightStickX < -0.35f) 
                    Controller_Send_rightsticky = 32767;
                if (jr.JoyconRightStickX <= 0.35f & jr.JoyconRightStickX >= -0.35f) 
                    Controller_Send_rightsticky = 0;
                XBC.Set(Controller_Send_back, Controller_Send_start, Controller_Send_A, Controller_Send_B, Controller_Send_X, Controller_Send_Y, Controller_Send_up, Controller_Send_left, Controller_Send_down, Controller_Send_right, Controller_Send_leftstick, Controller_Send_rightstick, Controller_Send_leftbumper, Controller_Send_rightbumper, Controller_Send_leftstickx, Controller_Send_leftsticky, Controller_Send_rightstickx, Controller_Send_rightsticky, Controller_Send_lefttriggerposition, Controller_Send_righttriggerposition, Controller_Send_xbox);
                /*XBC.ViewData();*/
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