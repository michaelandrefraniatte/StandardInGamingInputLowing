using System;
using System.Globalization;
using System.IO;
using System.Drawing;
using System.Numerics;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;
using System.Reflection;
using System.Diagnostics;
using Valuechanges;
using DirectInputsAPI;
using controllers;
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
        private static double statex = 0f, statey = 0f, mousexp = 0f, mouseyp = 0f, mousex = 0f, mousey = 0f, viewpower1x = 1f, viewpower2x = 0f, viewpower3x = 0f, viewpower1y = 1f, viewpower2y = 0f, viewpower3y = 0f, dzx = 20.0f, dzy = 20.0f;
        private static double irx = 0f, iry = 0f;        
        private static bool getstate;
        private static int sleeptime = 1;
        public static Valuechange ValueChange = new Valuechange();
        private DirectInput di = new DirectInput();
        private XBoxController XBC = new XBoxController();
        private static int[] wd = { 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2 };
        private static int[] wu = { 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2 };
        public static void valchanged(int n, bool val)
        {
            if (val)
            {
                if (wd[n] <= 1)
                {
                    wd[n] = wd[n] + 1;
                }
                wu[n] = 0;
            }
            else
            {
                if (wu[n] <= 1)
                {
                    wu[n] = wu[n] + 1;
                }
                wd[n] = 0;
            }
        }
        public void Close()
        {
            try
            {
                running = false;
                Thread.Sleep(100);
                di.Close();
                XBC.Disconnect();
                di.Dispose();
                XBC.Dispose();
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
            di.Scan();
            di.BeginPolling();
            XBC.Connect();
            Task.Run(() => task());
        }
        private void task()
        {
            for (; ; )
            {
                if (!running)
                    break;
                mousex     = -(di.JoystickAxisX - 32767f) / 20f; 
                mousey     = (di.JoystickAxisY - 32767f) / 20f;
                Controller_Send_leftstickx = Math.Abs(-mousex * 32767f / 1024f) <= 32767f ? -mousex * 32767f / 1024f : Math.Sign(-mousex) * 32767f;
                Controller_Send_leftsticky = Math.Abs(-mousey * 32767f / 1024f) <= 32767f ? -mousey * 32767f / 1024f : Math.Sign(-mousey) * 32767f;
                if (di.JoystickButtons12 | di.JoystickButtons15)
                {
                    mousexp = 0f;
                    mouseyp = 0f;
                }
                mousexp += Math.Round((di.JoystickAxisZ - 65535f / 2f) / 10000f, 0f) * 2.0;
                mouseyp += Math.Round((di.JoystickRotationZ - 65535f / 2f) / 10000f, 0f) * 2.0f;
                if (mousexp >= 1024f) 
                    mousexp = 1024f;
                if (mousexp <= -1024f) 
                    mousexp = -1024f;
                if (mouseyp >= 1024f) 
                    mouseyp = 1024f;
                if (mouseyp <= -1024f) 
                    mouseyp = -1024f;
                mousex                       = -mousexp - Math.Round((di.JoystickAxisZ - 65535f / 2f) / 10000f, 0f) * 2.0f;
                mousey                       = mouseyp + Math.Round((di.JoystickRotationZ - 65535f / 2f) / 10000f, 0f) * 2.0f;
                Controller_Send_rightstickx = Math.Abs(-mousex * 32767f / 1024f) <= 32767f ? -mousex * 32767f / 1024f : Math.Sign(-mousex) * 32767f;
                Controller_Send_rightsticky = Math.Abs(-mousey * 32767f / 1024f) <= 32767f ? -mousey * 32767f / 1024f : Math.Sign(-mousey) * 32767f;
                Controller_Send_up          = di.JoystickPointOfViewControllers0 == 4500 | di.JoystickPointOfViewControllers0 == 0 | di.JoystickPointOfViewControllers0 == 31500;
                Controller_Send_left        = di.JoystickPointOfViewControllers0 == 22500 | di.JoystickPointOfViewControllers0 == 27000 | di.JoystickPointOfViewControllers0 == 31500;
                Controller_Send_down        = di.JoystickPointOfViewControllers0 == 22500 | di.JoystickPointOfViewControllers0 == 18000 | di.JoystickPointOfViewControllers0 == 13500;
                Controller_Send_right       = di.JoystickPointOfViewControllers0 == 4500 | di.JoystickPointOfViewControllers0 == 9000 | di.JoystickPointOfViewControllers0 == 13500;
                Controller_Send_back        = di.JoystickButtons11;
                Controller_Send_start       = di.JoystickButtons12;
                Controller_Send_leftstick   = di.JoystickButtons13;
                Controller_Send_rightstick  = di.JoystickButtons14;
                Controller_Send_leftbumper  = di.JoystickButtons4;
                Controller_Send_rightbumper = di.JoystickButtons5;
                Controller_Send_A           = di.JoystickButtons1;
                Controller_Send_B           = di.JoystickButtons2;
                Controller_Send_X           = di.JoystickButtons0;
                Controller_Send_Y           = di.JoystickButtons3;
                Controller_Send_lefttriggerposition  = di.JoystickButtons9 ? 255 : 0;
                Controller_Send_righttriggerposition = di.JoystickButtons10 ? 255 : 0;
                XBC.Set(Controller_Send_back, Controller_Send_start, Controller_Send_A, Controller_Send_B, Controller_Send_X, Controller_Send_Y, Controller_Send_up, Controller_Send_left, Controller_Send_down, Controller_Send_right, Controller_Send_leftstick, Controller_Send_rightstick, Controller_Send_leftbumper, Controller_Send_rightbumper, Controller_Send_leftstickx, Controller_Send_leftsticky, Controller_Send_rightstickx, Controller_Send_rightsticky, Controller_Send_lefttriggerposition, Controller_Send_righttriggerposition, Controller_Send_xbox);
                /*XBC.ViewData();*/
                /*di.ViewData();*/
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