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
using XInputsAPI;
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
        private static bool ControllerVJoy_Send_1, ControllerVJoy_Send_2, ControllerVJoy_Send_3, ControllerVJoy_Send_4, ControllerVJoy_Send_5, ControllerVJoy_Send_6, ControllerVJoy_Send_7, ControllerVJoy_Send_8;
        private static double ControllerVJoy_Send_X, ControllerVJoy_Send_Y, ControllerVJoy_Send_Z, ControllerVJoy_Send_WHL, ControllerVJoy_Send_SL0, ControllerVJoy_Send_SL1, ControllerVJoy_Send_RX, ControllerVJoy_Send_RY, ControllerVJoy_Send_RZ, ControllerVJoy_Send_POV, ControllerVJoy_Send_Hat, ControllerVJoy_Send_HatExt1, ControllerVJoy_Send_HatExt2, ControllerVJoy_Send_HatExt3;
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
                xi.Dispose();
                VJoy.Dispose();
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
            xi.Scan();
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
                mousex                  = xi.ControllerThumbRightX;
                mousey                  = xi.ControllerThumbRightY;
                statex                  = Math.Abs(mousex) <= 32767f ? mousex : Math.Sign(mousex) * 32767f;
                statey                  = Math.Abs(mousey) <= 32767f ? mousey : Math.Sign(mousey) * 32767f;
                ControllerVJoy_Send_X  = statex;
                ControllerVJoy_Send_Y  = statey;
                mousex                  = xi.ControllerThumbLeftX;
                mousey                  = xi.ControllerThumbLeftY;
                ControllerVJoy_Send_RX = Math.Abs(mousex) <= 32767f ? mousex : Math.Sign(mousex) * 32767f;
                ControllerVJoy_Send_RY = Math.Abs(mousey) <= 32767f ? mousey : Math.Sign(mousey) * 32767f;
                ControllerVJoy_Send_1  = xi.ControllerButtonUpPressed;
                ControllerVJoy_Send_2  = xi.ControllerButtonLeftPressed;
                ControllerVJoy_Send_3  = xi.ControllerButtonDownPressed;
                ControllerVJoy_Send_4  = xi.ControllerButtonRightPressed;
                ControllerVJoy_Send_5  = xi.ControllerThumbpadLeftPressed;
                ControllerVJoy_Send_6  = xi.ControllerButtonShoulderLeftPressed;
                ControllerVJoy_Send_7  = xi.ControllerButtonShoulderRightPressed;
                ControllerVJoy_Send_8  = xi.ControllerButtonAPressed;
                if (xi.ControllerButtonBPressed)
                    ControllerVJoy_Send_POV = 0;
                if (xi.ControllerButtonXPressed)
                    ControllerVJoy_Send_POV = 9000;
                if (xi.ControllerButtonYPressed)
                    ControllerVJoy_Send_POV = 18000;
                if (xi.ControllerThumbpadRightPressed)
                    ControllerVJoy_Send_POV = 27000;
                ControllerVJoy_Send_Z   = xi.ControllerTriggerLeftPosition;
                ControllerVJoy_Send_WHL = xi.ControllerTriggerRightPosition;
                VJoy.Set(ControllerVJoy_Send_1, ControllerVJoy_Send_2, ControllerVJoy_Send_3, ControllerVJoy_Send_4, ControllerVJoy_Send_5, ControllerVJoy_Send_6, ControllerVJoy_Send_7, ControllerVJoy_Send_8, ControllerVJoy_Send_X, ControllerVJoy_Send_Y, ControllerVJoy_Send_Z, ControllerVJoy_Send_WHL, ControllerVJoy_Send_SL0, ControllerVJoy_Send_SL1, ControllerVJoy_Send_RX, ControllerVJoy_Send_RY, ControllerVJoy_Send_RZ, ControllerVJoy_Send_POV, ControllerVJoy_Send_Hat, ControllerVJoy_Send_HatExt1, ControllerVJoy_Send_HatExt2, ControllerVJoy_Send_HatExt3);
                /*VJoy.ViewData();*/
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