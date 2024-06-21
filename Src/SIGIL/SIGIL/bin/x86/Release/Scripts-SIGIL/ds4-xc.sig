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
        private static bool ControllerDS4_Send_Options, ControllerDS4_Send_ThumbLeft, ControllerDS4_Send_ThumbRight, ControllerDS4_Send_ShoulderLeft, ControllerDS4_Send_ShoulderRight, ControllerDS4_Send_Cross, ControllerDS4_Send_Circle, ControllerDS4_Send_Square, ControllerDS4_Send_Triangle, ControllerDS4_Send_Ps, ControllerDS4_Send_Touchpad, ControllerDS4_Send_Share, ControllerDS4_Send_DPadUp, ControllerDS4_Send_DPadDown, ControllerDS4_Send_DPadLeft, ControllerDS4_Send_DPadRight, ControllerDS4_Send_LeftTrigger, ControllerDS4_Send_RightTrigger;
        private static double ControllerDS4_Send_LeftTriggerPosition, ControllerDS4_Send_RightTriggerPosition;
        private static double ControllerDS4_Send_LeftThumbX, ControllerDS4_Send_RightThumbX, ControllerDS4_Send_LeftThumbY, ControllerDS4_Send_RightThumbY;
        private static double statex = 0f, statey = 0f, mousex = 0f, mousey = 0f, mousestatex = 0f, mousestatey = 0f, dzx = 0.0f, dzy = 0.0f, viewpower1x = 0f, viewpower2x = 1f, viewpower3x = 0f, viewpower1y = 0.25f, viewpower2y = 0.75f, viewpower3y = 0f, viewpower05x = 0f, viewpower05y = 0f;
        private static bool getstate;
        private static int sleeptime = 1;
        private DS4Controller DS4 = new DS4Controller();
        private XInput xi = new XInput();
        public static Valuechange ValueChange = new Valuechange();
        public void Close()
        {
            try
            {
                running = false;
                Thread.Sleep(100);
                xi.Close();
                DS4.Disconnect();
                xi.Dispose();
                DS4.Dispose();
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
            DS4.Connect();
            Task.Run(() => task());
        }
        private void task()
        {
            for (; ; )
            {
                if (!running)
                    break;
                mousex                                   = xi.ControllerThumbRightX;
                mousey                                   = xi.ControllerThumbRightY;
                statex                                   = Math.Abs(mousex) <= 32767f ? mousex : Math.Sign(mousex) * 32767f;
                statey                                   = Math.Abs(mousey) <= 32767f ? mousey : Math.Sign(mousey) * 32767f;
                ControllerDS4_Send_RightThumbX          = statex;
                ControllerDS4_Send_RightThumbY          = statey;
                mousex                                   = xi.ControllerThumbLeftX;
                mousey                                   = xi.ControllerThumbLeftY;
                ControllerDS4_Send_LeftThumbX           = Math.Abs(mousex) <= 32767f ? mousex : Math.Sign(mousex) * 32767f;
                ControllerDS4_Send_LeftThumbY           = Math.Abs(mousey) <= 32767f ? mousey : Math.Sign(mousey) * 32767f;
                ControllerDS4_Send_DPadUp               = xi.ControllerButtonUpPressed;
                ControllerDS4_Send_DPadLeft             = xi.ControllerButtonLeftPressed;
                ControllerDS4_Send_DPadDown             = xi.ControllerButtonDownPressed;
                ControllerDS4_Send_DPadRight            = xi.ControllerButtonRightPressed;
                ControllerDS4_Send_Options              = xi.ControllerButtonBackPressed;
                ControllerDS4_Send_Share                = xi.ControllerButtonStartPressed;
                ControllerDS4_Send_ThumbRight           = xi.ControllerThumbpadLeftPressed;
                ControllerDS4_Send_ShoulderLeft         = xi.ControllerButtonShoulderLeftPressed;
                ControllerDS4_Send_ShoulderRight        = xi.ControllerButtonShoulderRightPressed;
                ControllerDS4_Send_Cross                = xi.ControllerButtonAPressed;
                ControllerDS4_Send_Circle               = xi.ControllerButtonBPressed;
                ControllerDS4_Send_Square               = xi.ControllerButtonXPressed;
                ControllerDS4_Send_Triangle             = xi.ControllerButtonYPressed;
                ControllerDS4_Send_ThumbLeft            = xi.ControllerThumbpadRightPressed;
                ControllerDS4_Send_LeftTriggerPosition  = xi.ControllerTriggerLeftPosition;
                ControllerDS4_Send_RightTriggerPosition = xi.ControllerTriggerRightPosition;
                DS4.Set(ControllerDS4_Send_Options, ControllerDS4_Send_ThumbLeft, ControllerDS4_Send_ThumbRight, ControllerDS4_Send_ShoulderLeft, ControllerDS4_Send_ShoulderRight, ControllerDS4_Send_Cross, ControllerDS4_Send_Circle, ControllerDS4_Send_Square, ControllerDS4_Send_Triangle, ControllerDS4_Send_Ps, ControllerDS4_Send_Touchpad, ControllerDS4_Send_Share, ControllerDS4_Send_DPadUp, ControllerDS4_Send_DPadDown, ControllerDS4_Send_DPadLeft, ControllerDS4_Send_DPadRight, ControllerDS4_Send_LeftThumbX, ControllerDS4_Send_RightThumbX, ControllerDS4_Send_LeftThumbY, ControllerDS4_Send_RightThumbY, ControllerDS4_Send_LeftTrigger, ControllerDS4_Send_RightTrigger, ControllerDS4_Send_LeftTriggerPosition, ControllerDS4_Send_RightTriggerPosition);
                /*DS4.ViewData();*/
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