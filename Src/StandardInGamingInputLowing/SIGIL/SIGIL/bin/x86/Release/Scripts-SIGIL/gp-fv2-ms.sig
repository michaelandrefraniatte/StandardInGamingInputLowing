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
        private static bool controller1_send_back, controller1_send_start, controller1_send_A, controller1_send_B, controller1_send_X, controller1_send_Y, controller1_send_up, controller1_send_left, controller1_send_down, controller1_send_right, controller1_send_leftstick, controller1_send_rightstick, controller1_send_leftbumper, controller1_send_rightbumper, controller1_send_lefttrigger, controller1_send_righttrigger, controller1_send_xbox;
        private static double controller1_send_leftstickx, controller1_send_leftsticky, controller1_send_rightstickx, controller1_send_rightsticky, controller1_send_lefttriggerposition, controller1_send_righttriggerposition;
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
                controller1_send_leftstickx = Math.Abs(-mousex * 32767f / 1024f) <= 32767f ? -mousex * 32767f / 1024f : Math.Sign(-mousex) * 32767f;
                controller1_send_leftsticky = Math.Abs(-mousey * 32767f / 1024f) <= 32767f ? -mousey * 32767f / 1024f : Math.Sign(-mousey) * 32767f;
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
                controller1_send_rightstickx = Math.Abs(-mousex * 32767f / 1024f) <= 32767f ? -mousex * 32767f / 1024f : Math.Sign(-mousex) * 32767f;
                controller1_send_rightsticky = Math.Abs(-mousey * 32767f / 1024f) <= 32767f ? -mousey * 32767f / 1024f : Math.Sign(-mousey) * 32767f;
                controller1_send_up          = jh.JoystickPointOfViewControllers0 == 4500 | jh.JoystickPointOfViewControllers0 == 0 | jh.JoystickPointOfViewControllers0 == 31500;
                controller1_send_left        = jh.JoystickPointOfViewControllers0 == 22500 | jh.JoystickPointOfViewControllers0 == 27000 | jh.JoystickPointOfViewControllers0 == 31500;
                controller1_send_down        = jh.JoystickPointOfViewControllers0 == 22500 | jh.JoystickPointOfViewControllers0 == 18000 | jh.JoystickPointOfViewControllers0 == 13500;
                controller1_send_right       = jh.JoystickPointOfViewControllers0 == 4500 | jh.JoystickPointOfViewControllers0 == 9000 | jh.JoystickPointOfViewControllers0 == 13500;
                controller1_send_back        = jh.JoystickButtons11;
                controller1_send_start       = jh.JoystickButtons12;
                controller1_send_leftstick   = jh.JoystickButtons13;
                controller1_send_rightstick  = jh.JoystickButtons14;
                controller1_send_leftbumper  = jh.JoystickButtons4;
                controller1_send_rightbumper = jh.JoystickButtons5;
                controller1_send_A           = jh.JoystickButtons1;
                controller1_send_B           = jh.JoystickButtons2;
                controller1_send_X           = jh.JoystickButtons0;
                controller1_send_Y           = jh.JoystickButtons3;
                controller1_send_lefttriggerposition  = jh.JoystickButtons9 ? 255 : 0;
                controller1_send_righttriggerposition = jh.JoystickButtons10 ? 255 : 0;
                XBC.Set(controller1_send_back, controller1_send_start, controller1_send_A, controller1_send_B, controller1_send_X, controller1_send_Y, controller1_send_up, controller1_send_left, controller1_send_down, controller1_send_right, controller1_send_leftstick, controller1_send_rightstick, controller1_send_leftbumper, controller1_send_rightbumper, controller1_send_leftstickx, controller1_send_leftsticky, controller1_send_rightstickx, controller1_send_rightsticky, controller1_send_lefttriggerposition, controller1_send_righttriggerposition, controller1_send_xbox);
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