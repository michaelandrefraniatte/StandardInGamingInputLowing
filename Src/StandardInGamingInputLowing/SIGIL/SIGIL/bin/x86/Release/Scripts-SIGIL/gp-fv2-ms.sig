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
using JoysticksHooksAPI;
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
        private JoysticksHooks jh = new JoysticksHooks();
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
                jh.Close();
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
            jh.Scan();
            jh.BeginPolling();
            XBC.Connect();
            Task.Run(() => task());
        }
        private void task()
        {
            for (; ; )
            {
                if (!running)
                    break;
                mousex     = -(jh.JoystickAxisX - 32767f) / 20f; 
                mousey     = (jh.JoystickAxisY - 32767f) / 20f;
                Controller_Send_leftstickx = Math.Abs(-mousex * 32767f / 1024f) <= 32767f ? -mousex * 32767f / 1024f : Math.Sign(-mousex) * 32767f;
                Controller_Send_leftsticky = Math.Abs(-mousey * 32767f / 1024f) <= 32767f ? -mousey * 32767f / 1024f : Math.Sign(-mousey) * 32767f;
                if (jh.JoystickButtons12 | jh.JoystickButtons15)
                {
                    mousexp = 0f;
                    mouseyp = 0f;
                }
                mousexp += Math.Round((jh.JoystickAxisZ - 65535f / 2f) / 10000f, 0f) * 2.0;
                mouseyp += Math.Round((jh.JoystickRotationZ - 65535f / 2f) / 10000f, 0f) * 2.0f;
                if (mousexp >= 1024f) 
                    mousexp = 1024f;
                if (mousexp <= -1024f) 
                    mousexp = -1024f;
                if (mouseyp >= 1024f) 
                    mouseyp = 1024f;
                if (mouseyp <= -1024f) 
                    mouseyp = -1024f;
                mousex                       = -mousexp - Math.Round((jh.JoystickAxisZ - 65535f / 2f) / 10000f, 0f) * 2.0f;
                mousey                       = mouseyp + Math.Round((jh.JoystickRotationZ - 65535f / 2f) / 10000f, 0f) * 2.0f;
                Controller_Send_rightstickx = Math.Abs(-mousex * 32767f / 1024f) <= 32767f ? -mousex * 32767f / 1024f : Math.Sign(-mousex) * 32767f;
                Controller_Send_rightsticky = Math.Abs(-mousey * 32767f / 1024f) <= 32767f ? -mousey * 32767f / 1024f : Math.Sign(-mousey) * 32767f;
                Controller_Send_up          = jh.JoystickPointOfViewControllers0 == 4500 | jh.JoystickPointOfViewControllers0 == 0 | jh.JoystickPointOfViewControllers0 == 31500;
                Controller_Send_left        = jh.JoystickPointOfViewControllers0 == 22500 | jh.JoystickPointOfViewControllers0 == 27000 | jh.JoystickPointOfViewControllers0 == 31500;
                Controller_Send_down        = jh.JoystickPointOfViewControllers0 == 22500 | jh.JoystickPointOfViewControllers0 == 18000 | jh.JoystickPointOfViewControllers0 == 13500;
                Controller_Send_right       = jh.JoystickPointOfViewControllers0 == 4500 | jh.JoystickPointOfViewControllers0 == 9000 | jh.JoystickPointOfViewControllers0 == 13500;
                Controller_Send_back        = jh.JoystickButtons11;
                Controller_Send_start       = jh.JoystickButtons12;
                Controller_Send_leftstick   = jh.JoystickButtons13;
                Controller_Send_rightstick  = jh.JoystickButtons14;
                Controller_Send_leftbumper  = jh.JoystickButtons4;
                Controller_Send_rightbumper = jh.JoystickButtons5;
                Controller_Send_A           = jh.JoystickButtons1;
                Controller_Send_B           = jh.JoystickButtons2;
                Controller_Send_X           = jh.JoystickButtons0;
                Controller_Send_Y           = jh.JoystickButtons3;
                Controller_Send_lefttriggerposition  = jh.JoystickButtons9 ? 255 : 0;
                Controller_Send_righttriggerposition = jh.JoystickButtons10 ? 255 : 0;
                XBC.Set(Controller_Send_back, Controller_Send_start, Controller_Send_A, Controller_Send_B, Controller_Send_X, Controller_Send_Y, Controller_Send_up, Controller_Send_left, Controller_Send_down, Controller_Send_right, Controller_Send_leftstick, Controller_Send_rightstick, Controller_Send_leftbumper, Controller_Send_rightbumper, Controller_Send_leftstickx, Controller_Send_leftsticky, Controller_Send_rightstickx, Controller_Send_rightsticky, Controller_Send_lefttriggerposition, Controller_Send_righttriggerposition, Controller_Send_xbox);
                /*jh.ViewData();*/
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