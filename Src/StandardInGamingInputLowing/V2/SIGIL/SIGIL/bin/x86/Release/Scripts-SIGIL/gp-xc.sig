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
        private static bool controller1_send_back, controller1_send_start, controller1_send_A, controller1_send_B, controller1_send_X, controller1_send_Y, controller1_send_up, controller1_send_left, controller1_send_down, controller1_send_right, controller1_send_leftstick, controller1_send_rightstick, controller1_send_leftbumper, controller1_send_rightbumper, controller1_send_lefttrigger, controller1_send_righttrigger, controller1_send_xbox;
        private static double controller1_send_leftstickx, controller1_send_leftsticky, controller1_send_rightstickx, controller1_send_rightsticky, controller1_send_lefttriggerposition, controller1_send_righttriggerposition;
        private static double statex = 0f, statey = 0f, mousex = 0f, mousey = 0f, mousestatex = 0f, mousestatey = 0f, dzx = 0.0f, dzy = 0.0f, viewpower1x = 0f, viewpower2x = 1f, viewpower3x = 0f, viewpower1y = 0.25f, viewpower2y = 0.75f, viewpower3y = 0f, viewpower05x = 0f, viewpower05y = 0f;
        private static bool getstate;
        private static int sleeptime = 1;
        private XBoxController XBC = new XBoxController();
        private XInput xi = new XInput();
        public static Valuechange ValueChange = new Valuechange();
        public void Close()
        {
            try
            {
                running = false;
                Thread.Sleep(100);
                xi.Close();
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
            xi.Scan();
            xi.BeginPolling();
            XBC.Connect();
            Task.Run(() => task());
        }
        private void task()
        {
            for (; ; )
            {
                if (!running)
                    break;
                mousex                                = xi.ControllerThumbRightX;
                mousey                                = xi.ControllerThumbRightY;
                statex                                = Math.Abs(mousex) <= 32767f ? mousex : Math.Sign(mousex) * 32767f;
                statey                                = Math.Abs(mousey) <= 32767f ? mousey : Math.Sign(mousey) * 32767f;
                controller1_send_rightstickx          = statex;
                controller1_send_rightsticky          = statey;
                mousex                                = xi.ControllerThumbLeftX;
                mousey                                = xi.ControllerThumbLeftY;
                controller1_send_leftstickx           = Math.Abs(mousex) <= 32767f ? mousex : Math.Sign(mousex) * 32767f;
                controller1_send_leftsticky           = Math.Abs(mousey) <= 32767f ? mousey : Math.Sign(mousey) * 32767f;
                controller1_send_up                   = xi.ControllerButtonUpPressed;
                controller1_send_left                 = xi.ControllerButtonLeftPressed;
                controller1_send_down                 = xi.ControllerButtonDownPressed;
                controller1_send_right                = xi.ControllerButtonRightPressed;
                controller1_send_back                 = xi.ControllerButtonBackPressed;
                controller1_send_start                = xi.ControllerButtonStartPressed;
                controller1_send_leftstick            = xi.ControllerThumbpadLeftPressed;
                controller1_send_leftbumper           = xi.ControllerButtonShoulderLeftPressed;
                controller1_send_rightbumper          = xi.ControllerButtonShoulderRightPressed;
                controller1_send_A                    = xi.ControllerButtonAPressed;
                controller1_send_B                    = xi.ControllerButtonBPressed;
                controller1_send_X                    = xi.ControllerButtonXPressed;
                controller1_send_Y                    = xi.ControllerButtonYPressed;
                controller1_send_rightstick           = xi.ControllerThumbpadRightPressed;
                controller1_send_lefttriggerposition  = xi.ControllerTriggerLeftPosition;
                controller1_send_righttriggerposition = xi.ControllerTriggerRightPosition;
                XBC.Set(controller1_send_back, controller1_send_start, controller1_send_A, controller1_send_B, controller1_send_X, controller1_send_Y, controller1_send_up, controller1_send_left, controller1_send_down, controller1_send_right, controller1_send_leftstick, controller1_send_rightstick, controller1_send_leftbumper, controller1_send_rightbumper, controller1_send_leftstickx, controller1_send_leftsticky, controller1_send_rightstickx, controller1_send_rightsticky, controller1_send_lefttriggerposition, controller1_send_righttriggerposition, controller1_send_xbox);
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