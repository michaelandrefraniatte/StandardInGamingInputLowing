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
using controllersds4;
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
        private static bool Controller1DS4_Send_Options, Controller1DS4_Send_Option, Controller1DS4_Send_ThumbLeft, Controller1DS4_Send_ThumbRight, Controller1DS4_Send_ShoulderLeft, Controller1DS4_Send_ShoulderRight, Controller1DS4_Send_Cross, Controller1DS4_Send_Circle, Controller1DS4_Send_Square, Controller1DS4_Send_Triangle, Controller1DS4_Send_Ps, Controller1DS4_Send_Touchpad, Controller1DS4_Send_Share, Controller1DS4_Send_DPadUp, Controller1DS4_Send_DPadDown, Controller1DS4_Send_DPadLeft, Controller1DS4_Send_DPadRight, Controller1DS4_Send_LeftTrigger, Controller1DS4_Send_RightTrigger;
        private static double Controller1DS4_Send_LeftTriggerPosition, Controller1DS4_Send_RightTriggerPosition;
        private static double Controller1DS4_Send_LeftThumbX, Controller1DS4_Send_RightThumbX, Controller1DS4_Send_LeftThumbY, Controller1DS4_Send_RightThumbY;
        private static double statex = 0f, statey = 0f, mousex = 0f, mousey = 0f, mousestatex = 0f, mousestatey = 0f, dzx = 0.0f, dzy = 0.0f, viewpower1x = 0f, viewpower2x = 1f, viewpower3x = 0f, viewpower1y = 0.25f, viewpower2y = 0.75f, viewpower3y = 0f, viewpower05x = 0f, viewpower05y = 0f;
        private static bool getstate;
        private static int sleeptime = 1;
        private DS4Controller ds4 = new DS4Controller();
        private XInput xi = new XInput();
        public static Valuechange ValueChange = new Valuechange();
        public void Close()
        {
            try
            {
                running = false;
                Thread.Sleep(100);
                xi.Close();
                ds4.Disconnect();
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
            ds4.Connect();
            Task.Run(() => taskX());
        }
        private void taskX()
        {
            for (; ; )
            {
                if (!running)
                    break;
                mousex                                   = xi.Controller1ThumbRightX;
                mousey                                   = xi.Controller1ThumbRightY;
                statex                                   = Math.Abs(mousex) <= 32767f ? mousex : Math.Sign(mousex) * 32767f;
                statey                                   = Math.Abs(mousey) <= 32767f ? mousey : Math.Sign(mousey) * 32767f;
                Controller1DS4_Send_RightThumbX          = statex;
                Controller1DS4_Send_RightThumbY          = statey;
                mousex                                   = xi.Controller1ThumbLeftX;
                mousey                                   = xi.Controller1ThumbLeftY;
                Controller1DS4_Send_LeftThumbX           = Math.Abs(mousex) <= 32767f ? mousex : Math.Sign(mousex) * 32767f;
                Controller1DS4_Send_LeftThumbY           = Math.Abs(mousey) <= 32767f ? mousey : Math.Sign(mousey) * 32767f;
                Controller1DS4_Send_DPadUp               = xi.Controller1ButtonUpPressed;
                Controller1DS4_Send_DPadLeft             = xi.Controller1ButtonLeftPressed;
                Controller1DS4_Send_DPadDown             = xi.Controller1ButtonDownPressed;
                Controller1DS4_Send_DPadRight            = xi.Controller1ButtonRightPressed;
                Controller1DS4_Send_Option               = xi.Controller1ButtonBackPressed;
                Controller1DS4_Send_Share                = xi.Controller1ButtonStartPressed;
                Controller1DS4_Send_ThumbRight           = xi.Controller1ThumbpadLeftPressed;
                Controller1DS4_Send_ShoulderLeft         = xi.Controller1ButtonShoulderLeftPressed;
                Controller1DS4_Send_ShoulderRight        = xi.Controller1ButtonShoulderRightPressed;
                Controller1DS4_Send_Cross                = xi.Controller1ButtonAPressed;
                Controller1DS4_Send_Circle               = xi.Controller1ButtonBPressed;
                Controller1DS4_Send_Square               = xi.Controller1ButtonXPressed;
                Controller1DS4_Send_Triangle             = xi.Controller1ButtonYPressed;
                Controller1DS4_Send_ThumbLeft            = xi.Controller1ThumbpadRightPressed;
                Controller1DS4_Send_LeftTriggerPosition  = xi.Controller1TriggerLeftPosition;
                Controller1DS4_Send_RightTriggerPosition = xi.Controller1TriggerRightPosition;
                ds4.SubmitReport1(Controller1DS4_Send_Options, Controller1DS4_Send_Option, Controller1DS4_Send_ThumbLeft, Controller1DS4_Send_ThumbRight, Controller1DS4_Send_ShoulderLeft, Controller1DS4_Send_ShoulderRight, Controller1DS4_Send_Cross, Controller1DS4_Send_Circle, Controller1DS4_Send_Square, Controller1DS4_Send_Triangle, Controller1DS4_Send_Ps, Controller1DS4_Send_Touchpad, Controller1DS4_Send_Share, Controller1DS4_Send_DPadUp, Controller1DS4_Send_DPadDown, Controller1DS4_Send_DPadLeft, Controller1DS4_Send_DPadRight, Controller1DS4_Send_LeftThumbX, Controller1DS4_Send_RightThumbX, Controller1DS4_Send_LeftThumbY, Controller1DS4_Send_RightThumbY, Controller1DS4_Send_LeftTrigger, Controller1DS4_Send_RightTrigger, Controller1DS4_Send_LeftTriggerPosition, Controller1DS4_Send_RightTriggerPosition);
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