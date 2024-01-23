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
using SwitchProControllersAPI;
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
        private static double statex = 0f, statey = 0f, mousexp = 0f, mouseyp = 0f, mousex = 0f, mousey = 0f, mousestatex = 0f, mousestatey = 0f, dzx = 8.0f, dzy = 8.0f, viewpower1x = 0f, viewpower2x = 1f, viewpower3x = 0f, viewpower1y = 0.25f, viewpower2y = 0.75f, viewpower3y = 0f, viewpower05x = 0f, viewpower05y = 0f;
        private static bool getstate;
        private static int sleeptime = 8;
        private XBoxController XBC = new XBoxController();
        private SwitchProController spc = new SwitchProController();
        public static Valuechange ValueChange = new Valuechange();
        public void Close()
        {
            try
            {
                running = false;
                Thread.Sleep(100);
                spc.Close();
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
            spc.Scan();
            spc.BeginPolling();
            Thread.Sleep(1000);
            spc.Init();
            XBC.Connect();
            Task.Run(() => task());
        }
        private void task()
        {
            for (; ; )
            {
                if (!running)
                    break;
                if (spc.ProControllerButtonPLUS)
                {
                    mousexp = 0f;
                    mouseyp = 0f;
                    spc.Init();
                }
                mousexp += Math.Round(spc.ProControllerGyroX / 200f, 0) * 2f;
                mouseyp += Math.Round(spc.ProControllerGyroY / 200f, 0) * 2f;
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
                mousex = mousexp + Math.Round(spc.ProControllerGyroX / 200f, 0) * 2f;
                if ((-mousexp > 0 & ValueChange._ValueChange[0] < 0) | (-mousexp < 0 & ValueChange._ValueChange[0] > 0))
                {
                    mousexp = 0f;
                }
                mousey = mouseyp + Math.Round(spc.ProControllerGyroY / 200f, 0) * 2f;
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
                mousex                                = mousestatex - spc.ProControllerRightStickX * 32767f;
                mousey                                = mousestatey - spc.ProControllerRightStickY * 32767f;
                statex                                = Math.Abs(mousex) <= 32767f ? mousex : Math.Sign(mousex) * 32767f;
                statey                                = Math.Abs(mousey) <= 32767f ? mousey : Math.Sign(mousey) * 32767f;
                controller1_send_rightstickx          = statex;
                controller1_send_rightsticky          = statey;
                mousex                                = -spc.ProControllerLeftStickX * 1024f;
                mousey                                = -spc.ProControllerLeftStickY * 1024f;
                controller1_send_leftstickx           = Math.Abs(-mousex * 32767f / 1024f) <= 32767f ? -mousex * 32767f / 1024f : Math.Sign(-mousex) * 32767f;
                controller1_send_leftsticky           = Math.Abs(-mousey * 32767f / 1024f) <= 32767f ? -mousey * 32767f / 1024f : Math.Sign(-mousey) * 32767f;
                controller1_send_down                 = spc.ProControllerButtonDPAD_DOWN;
                controller1_send_left                 = spc.ProControllerButtonDPAD_LEFT;
                controller1_send_right                = spc.ProControllerButtonDPAD_RIGHT;
                controller1_send_up                   = spc.ProControllerButtonDPAD_UP;
                controller1_send_A                    = spc.ProControllerButtonB;
                controller1_send_B                    = spc.ProControllerButtonA;  
                controller1_send_Y                    = spc.ProControllerButtonX;
                controller1_send_X                    = spc.ProControllerButtonY;
                controller1_send_lefttriggerposition  = spc.ProControllerButtonSHOULDER_Left_2 ? 255 : 0;
                controller1_send_righttriggerposition = spc.ProControllerButtonSHOULDER_Right_2 ? 255 : 0;
                controller1_send_leftbumper           = spc.ProControllerButtonSHOULDER_Left_1;
                controller1_send_rightbumper          = spc.ProControllerButtonSHOULDER_Right_1;
                controller1_send_leftstick            = spc.ProControllerButtonSTICK_Left;
                controller1_send_rightstick           = spc.ProControllerButtonSTICK_Right;
                controller1_send_start                = spc.ProControllerButtonHOME;
                controller1_send_back                 = spc.ProControllerButtonCAPTURE;
                XBC.Set(controller1_send_back, controller1_send_start, controller1_send_A, controller1_send_B, controller1_send_X, controller1_send_Y, controller1_send_up, controller1_send_left, controller1_send_down, controller1_send_right, controller1_send_leftstick, controller1_send_rightstick, controller1_send_leftbumper, controller1_send_rightbumper, controller1_send_leftstickx, controller1_send_leftsticky, controller1_send_rightstickx, controller1_send_rightsticky, controller1_send_lefttriggerposition, controller1_send_righttriggerposition, controller1_send_xbox);
                /*spc.ViewData();*/
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