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
        private static bool controller1_send_back, controller1_send_start, controller1_send_A, controller1_send_B, controller1_send_X, controller1_send_Y, controller1_send_up, controller1_send_left, controller1_send_down, controller1_send_right, controller1_send_leftstick, controller1_send_rightstick, controller1_send_leftbumper, controller1_send_rightbumper, controller1_send_lefttrigger, controller1_send_righttrigger, controller1_send_xbox;
        private static double controller1_send_leftstickx, controller1_send_leftsticky, controller1_send_rightstickx, controller1_send_rightsticky, controller1_send_lefttriggerposition, controller1_send_righttriggerposition;
        private static double mousestatex = 0f, mousestatey = 0f, statex = 0f, statey = 0f, mousexp = 0f, mouseyp = 0f, mousex = 0f, mousey = 0f, viewpower1x = 1f, viewpower2x = 0f, viewpower3x = 0f, viewpower1y = 1f, viewpower2y = 0f, viewpower3y = 0f, dzx = 8.0f, dzy = 8.0f;
        private static double irx = 0f, iry = 0f;        
        private static bool getstate;
        private static int sleeptime = 8;
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
                if (di.JoystickButtons12 | di.JoystickButtons15)
                {
                    mousexp = 0f;
                    mouseyp = 0f;
                }
                mousexp += Math.Round(-(di.JoystickAxisZ - 65535f / 2f) / 3000f, 0) * 2f;
                mouseyp += Math.Round((di.JoystickRotationZ - 65535f / 2f) / 3000f, 0) * 2f;
                if (mousexp >= 1024f) 
                    mousexp = 1024f;
                if (mousexp <= -1024f) 
                    mousexp = -1024f;
                if (mouseyp >= 1024f) 
                    mouseyp = 1024f;
                if (mouseyp <= -1024f) 
                    mouseyp = -1024f;
                ValueChange[0] = -mousexp;
                ValueChange[1] = mouseyp;
                mousex = mousexp + Math.Round(-(di.JoystickAxisZ - 65535f / 2f) / 3000f, 0) * 2f;
                if ((-mousexp > 0 & ValueChange._ValueChange[0] < 0) | (-mousexp < 0 & ValueChange._ValueChange[0] > 0))
                {
                    mousexp = 0f;
                }
                mousey = mouseyp + Math.Round((di.JoystickRotationZ - 65535f / 2f) / 3000f, 0) * 2f;
                if ((mouseyp > 0 & ValueChange._ValueChange[1] < 0) | (mouseyp < 0 & ValueChange._ValueChange[1] > 0))
                {
                    mouseyp = 0f;
                }
                statex = Math.Abs(-mousex * 32767f / 1024f) <= 32767f ? -mousex * 32767f / 1024f : Math.Sign(-mousex) * 32767f;
                statey = Math.Abs(-mousey * 32767f / 1024f) <= 32767f ? -mousey * 32767f / 1024f : Math.Sign(-mousey) * 32767f;
                if (statex > 0f)
                    mousestatex = Scale(statex, 0f, 32767f, (dzx / 100f) * 32767f, 32767f);
                if (statex < 0f)
                    mousestatex = Scale(statex, -32767f, 0f, -32767f, -(dzx / 100f) * 32767f);
                if (statey > 0f)
                    mousestatey = Scale(statey, 0f, 32767f, (dzy / 100f) * 32767f, 32767f);
                if (statey < 0f)
                    mousestatey = Scale(statey, -32767f, 0f, -32767f, -(dzy / 100f) * 32767f);
                mousex                       = mousestatex;
                mousey                       = mousestatey;
                statex                       = Math.Abs(mousex) <= 32767f ? mousex : Math.Sign(mousex) * 32767f;
                statey                       = Math.Abs(mousey) <= 32767f ? mousey : Math.Sign(mousey) * 32767f;
                controller1_send_rightstickx = statex;
                controller1_send_rightsticky = statey;
                mousex                       = (di.JoystickAxisX - 65535f / 2f); 
                mousey                       = -(di.JoystickAxisY - 65535f / 2f);
                statex                       = Math.Abs(mousex) <= 32767f ? mousex : Math.Sign(mousex) * 32767f;
                statey                       = Math.Abs(mousey) <= 32767f ? mousey : Math.Sign(mousey) * 32767f;
                controller1_send_leftstickx  = statex;
                controller1_send_leftsticky  = statey;
                controller1_send_up          = di.JoystickPointOfViewControllers0 == 4500 | di.JoystickPointOfViewControllers0 == 0 | di.JoystickPointOfViewControllers0 == 31500;
                controller1_send_left        = di.JoystickPointOfViewControllers0 == 22500 | di.JoystickPointOfViewControllers0 == 27000 | di.JoystickPointOfViewControllers0 == 31500;
                controller1_send_down        = di.JoystickPointOfViewControllers0 == 22500 | di.JoystickPointOfViewControllers0 == 18000 | di.JoystickPointOfViewControllers0 == 13500;
                controller1_send_right       = di.JoystickPointOfViewControllers0 == 4500 | di.JoystickPointOfViewControllers0 == 9000 | di.JoystickPointOfViewControllers0 == 13500;
                controller1_send_back        = di.JoystickButtons11;
                controller1_send_start       = di.JoystickButtons12;
                controller1_send_leftstick   = di.JoystickButtons13;
                controller1_send_rightstick  = di.JoystickButtons14;
                controller1_send_leftbumper  = di.JoystickButtons4;
                controller1_send_rightbumper = di.JoystickButtons5;
                controller1_send_A           = di.JoystickButtons1;
                controller1_send_B           = di.JoystickButtons2;
                controller1_send_X           = di.JoystickButtons0;
                controller1_send_Y           = di.JoystickButtons3;
                controller1_send_lefttriggerposition  = di.JoystickButtons9 ? 255 : 0;
                controller1_send_righttriggerposition = di.JoystickButtons10 ? 255 : 0;
                XBC.Set(controller1_send_back, controller1_send_start, controller1_send_A, controller1_send_B, controller1_send_X, controller1_send_Y, controller1_send_up, controller1_send_left, controller1_send_down, controller1_send_right, controller1_send_leftstick, controller1_send_rightstick, controller1_send_leftbumper, controller1_send_rightbumper, controller1_send_leftstickx, controller1_send_leftsticky, controller1_send_rightstickx, controller1_send_rightsticky, controller1_send_lefttriggerposition, controller1_send_righttriggerposition, controller1_send_xbox);
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