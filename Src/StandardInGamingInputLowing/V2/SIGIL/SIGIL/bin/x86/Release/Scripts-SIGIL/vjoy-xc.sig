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
using controllersvjoy;
using System.Diagnostics;
using Valuechanges;
using XInputAPI;
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
        private static bool Controller1VJoy_Send_1, Controller1VJoy_Send_2, Controller1VJoy_Send_3, Controller1VJoy_Send_4, Controller1VJoy_Send_5, Controller1VJoy_Send_6, Controller1VJoy_Send_7, Controller1VJoy_Send_8;
        private static double Controller1VJoy_Send_X, Controller1VJoy_Send_Y, Controller1VJoy_Send_Z, Controller1VJoy_Send_WHL, Controller1VJoy_Send_SL0, Controller1VJoy_Send_SL1, Controller1VJoy_Send_RX, Controller1VJoy_Send_RY, Controller1VJoy_Send_RZ, Controller1VJoy_Send_POV, Controller1VJoy_Send_Hat, Controller1VJoy_Send_HatExt1, Controller1VJoy_Send_HatExt2, Controller1VJoy_Send_HatExt3;
        private static double statex = 0f, statey = 0f, mousex = 0f, mousey = 0f, mousestatex = 0f, mousestatey = 0f, dzx = 0.0f, dzy = 0.0f, viewpower1x = 0f, viewpower2x = 1f, viewpower3x = 0f, viewpower1y = 0.25f, viewpower2y = 0.75f, viewpower3y = 0f, viewpower05x = 0f, viewpower05y = 0f;
        private static bool getstate;
        private static int sleeptime = 1;
        private VJoyController VJoy = new VJoyController();
        private XInput xi = new XInput();
        public static Valuechange ValueChange = new Valuechange();
        public void Close()
        {
            try
            {
                running = false;
                Thread.Sleep(100);
                xi.Close();
                VJoy.Disconnect();
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
            xi.XInputHookConnect();
            xi.BeginPolling();
            VJoy.Connect();
            Task.Run(() => task());
        }
        private void task()
        {
            for (; ; )
            {
                if (!running)
                    break;
                mousex                   = xi.Controller1ThumbRightX;
                mousey                   = xi.Controller1ThumbRightY;
                statex                   = Math.Abs(mousex) <= 32767f ? mousex : Math.Sign(mousex) * 32767f;
                statey                   = Math.Abs(mousey) <= 32767f ? mousey : Math.Sign(mousey) * 32767f;
                Controller1VJoy_Send_X   = statex;
                Controller1VJoy_Send_Y   = statey;
                mousex                   = xi.Controller1ThumbLeftX;
                mousey                   = xi.Controller1ThumbLeftY;
                Controller1VJoy_Send_RX  = Math.Abs(mousex) <= 32767f ? mousex : Math.Sign(mousex) * 32767f;
                Controller1VJoy_Send_RY  = Math.Abs(mousey) <= 32767f ? mousey : Math.Sign(mousey) * 32767f;
                Controller1VJoy_Send_1   = xi.Controller1ButtonUpPressed;
                Controller1VJoy_Send_2   = xi.Controller1ButtonLeftPressed;
                Controller1VJoy_Send_3   = xi.Controller1ButtonDownPressed;
                Controller1VJoy_Send_4   = xi.Controller1ButtonRightPressed;
                Controller1VJoy_Send_5   = xi.Controller1ThumbpadLeftPressed;
                Controller1VJoy_Send_6   = xi.Controller1ButtonShoulderLeftPressed;
                Controller1VJoy_Send_7   = xi.Controller1ButtonShoulderRightPressed;
                Controller1VJoy_Send_8   = xi.Controller1ButtonAPressed;
                if (xi.Controller1ButtonBPressed)
                    Controller1VJoy_Send_POV = 0;
                if (xi.Controller1ButtonXPressed)
                    Controller1VJoy_Send_POV = 9000;
                if (xi.Controller1ButtonYPressed)
                    Controller1VJoy_Send_POV = 18000;
                if (xi.Controller1ThumbpadRightPressed)
                    Controller1VJoy_Send_POV = 27000;
                Controller1VJoy_Send_Z   = xi.Controller1TriggerLeftPosition;
                Controller1VJoy_Send_WHL = xi.Controller1TriggerRightPosition;
                VJoy.SubmitReport1(Controller1VJoy_Send_1, Controller1VJoy_Send_2, Controller1VJoy_Send_3, Controller1VJoy_Send_4, Controller1VJoy_Send_5, Controller1VJoy_Send_6, Controller1VJoy_Send_7, Controller1VJoy_Send_8, Controller1VJoy_Send_X, Controller1VJoy_Send_Y, Controller1VJoy_Send_Z, Controller1VJoy_Send_WHL, Controller1VJoy_Send_SL0, Controller1VJoy_Send_SL1, Controller1VJoy_Send_RX, Controller1VJoy_Send_RY, Controller1VJoy_Send_RZ, Controller1VJoy_Send_POV, Controller1VJoy_Send_Hat, Controller1VJoy_Send_HatExt1, Controller1VJoy_Send_HatExt2, Controller1VJoy_Send_HatExt3);
                /*xi.ViewData();*/
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