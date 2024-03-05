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
using DualSensesAPI;
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
        private static double statex = 0f, statey = 0f, mousex = 0f, mousey = 0f, mousestatex = 0f, mousestatey = 0f, dzx = 22.0f, dzy = 0.0f, viewpower1x = 1f, viewpower2x = 0f, viewpower3x = 0f, viewpower1y = 1f, viewpower2y = 0f, viewpower3y = 0f, viewpower05x = 0f, viewpower05y = 0f;
        private static bool getstate;
        private static int sleeptime = 1;
        private static string vendor_ds_id = "54C", product_ds_id = "CE6", product_ds_label = "DualSense";
        private XBoxController XBC = new XBoxController();
        public DualSense ds = new DualSense();
        public static Valuechange ValueChange = new Valuechange();
        public void Close()
        {
            try
            {
                running = false;
                Thread.Sleep(100);
                ds.Close();
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
            ds.Scan(vendor_ds_id, product_ds_id, product_ds_label);
            ds.BeginPolling();
            XBC.Connect();
            Task.Run(() => task());
        }
        private void task()
        {
            for (; ; )
            {
                if (!running)
                    break;
                if (ds.PS5ControllerButtonMenuPressed)
                    ds.Init();
                mousex      = ds.PS5ControllerAccelY * 35f; 
                mousey      = ds.PS5ControllerLeftStickY * 32767f;
                statex      = Math.Abs(mousex) <= 32767f ? mousex : Math.Sign(mousex) * 32767f;
                statey      = Math.Abs(mousey) <= 32767f ? mousey : Math.Sign(mousey) * 32767f;
                if (statex > 0f)
                    mousestatex = Scale(statex * statex * statex / 32767f / 32767f * viewpower3x + statex * statex / 32767f * viewpower2x + statex * viewpower1x, 0f, 32767f, dzx / 100f * 32767f, 32767f);
                if (statex < 0f)
                    mousestatex = Scale(-(-statex * -statex * -statex) / 32767f / 32767f * viewpower3x - (-statex * -statex) / 32767f * viewpower2x - (-statex) * viewpower1x, -32767f, 0f, -32767f, -(dzx / 100f) * 32767f);
                if (statey > 0f)
                    mousestatey = Scale(statey * statey * statey / 32767f / 32767f * viewpower3y + statey * statey / 32767f * viewpower2y + statey * viewpower1y, 0f, 32767f, dzy / 100f * 32767f, 32767f);
                if (statey < 0f)
                    mousestatey = Scale(-(-statey * -statey * -statey) / 32767f / 32767f * viewpower3y - (-statey * -statey) / 32767f * viewpower2y - (-statey) * viewpower1y, -32767f, 0f, -32767f, -(dzy / 100f) * 32767f);
                mousex                                = mousestatex + ds.PS5ControllerLeftStickX * 32767f;
                mousey                                = mousestatey;
                statex                                = Math.Abs(mousex) <= 32767f ? mousex : Math.Sign(mousex) * 32767f;
                statey                                = Math.Abs(mousey) <= 32767f ? mousey : Math.Sign(mousey) * 32767f;
                Controller_Send_leftstickx           = statex;
                Controller_Send_leftsticky           = statey;
                mousex                                = ds.PS5ControllerRightStickX * 1024f;
                mousey                                = ds.PS5ControllerRightStickY * 1024f;
                Controller_Send_rightstickx          = Math.Abs(-mousex * 32767f / 1024f) <= 32767f ? -mousex * 32767f / 1024f : Math.Sign(-mousex) * 32767f;
                Controller_Send_rightsticky          = Math.Abs(-mousey * 32767f / 1024f) <= 32767f ? -mousey * 32767f / 1024f : Math.Sign(-mousey) * 32767f;
                Controller_Send_down                 = ds.PS5ControllerButtonDPadDownPressed;
                Controller_Send_left                 = ds.PS5ControllerButtonDPadLeftPressed;
                Controller_Send_right                = ds.PS5ControllerButtonDPadRightPressed;
                Controller_Send_up                   = ds.PS5ControllerButtonDPadUpPressed;
                Controller_Send_leftstick            = ds.PS5ControllerButtonL3Pressed;
                Controller_Send_rightstick           = ds.PS5ControllerButtonR3Pressed;
                Controller_Send_A                    = ds.PS5ControllerButtonCrossPressed;
                Controller_Send_B                    = ds.PS5ControllerButtonCirclePressed;
                Controller_Send_X                    = ds.PS5ControllerButtonSquarePressed;
                Controller_Send_Y                    = ds.PS5ControllerButtonTrianglePressed;
                Controller_Send_lefttriggerposition  = ds.PS5ControllerLeftTriggerPosition * 255;
                Controller_Send_righttriggerposition = ds.PS5ControllerRightTriggerPosition * 255;
                Controller_Send_leftbumper           = ds.PS5ControllerButtonL1Pressed;
                Controller_Send_rightbumper          = ds.PS5ControllerButtonR1Pressed;
                Controller_Send_back                 = ds.PS5ControllerButtonLogoPressed;
                Controller_Send_start                = ds.PS5ControllerButtonTouchpadPressed;
                Controller_Send_xbox                 = ds.PS5ControllerButtonMicPressed;
                XBC.Set(Controller_Send_back, Controller_Send_start, Controller_Send_A, Controller_Send_B, Controller_Send_X, Controller_Send_Y, Controller_Send_up, Controller_Send_left, Controller_Send_down, Controller_Send_right, Controller_Send_leftstick, Controller_Send_rightstick, Controller_Send_leftbumper, Controller_Send_rightbumper, Controller_Send_leftstickx, Controller_Send_leftsticky, Controller_Send_rightstickx, Controller_Send_rightsticky, Controller_Send_lefttriggerposition, Controller_Send_righttriggerposition, Controller_Send_xbox);
                /*ds.ViewData();*/
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