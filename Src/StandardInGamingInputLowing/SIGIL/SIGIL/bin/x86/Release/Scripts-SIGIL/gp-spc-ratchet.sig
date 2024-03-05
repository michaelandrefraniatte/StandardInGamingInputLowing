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
        private static bool Controller_Send_back, Controller_Send_start, Controller_Send_A, Controller_Send_B, Controller_Send_X, Controller_Send_Y, Controller_Send_up, Controller_Send_left, Controller_Send_down, Controller_Send_right, Controller_Send_leftstick, Controller_Send_rightstick, Controller_Send_leftbumper, Controller_Send_rightbumper, Controller_Send_lefttrigger, Controller_Send_righttrigger, Controller_Send_xbox;
        private static double Controller_Send_leftstickx, Controller_Send_leftsticky, Controller_Send_rightstickx, Controller_Send_rightsticky, Controller_Send_lefttriggerposition, Controller_Send_righttriggerposition;
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
                Controller_Send_rightstickx          = statex;
                Controller_Send_rightsticky          = statey;
                mousex                                = -spc.ProControllerLeftStickX * 1024f;
                mousey                                = -spc.ProControllerLeftStickY * 1024f;
                Controller_Send_leftstickx           = Math.Abs(-mousex * 32767f / 1024f) <= 32767f ? -mousex * 32767f / 1024f : Math.Sign(-mousex) * 32767f;
                Controller_Send_leftsticky           = Math.Abs(-mousey * 32767f / 1024f) <= 32767f ? -mousey * 32767f / 1024f : Math.Sign(-mousey) * 32767f;
                Controller_Send_down                 = spc.ProControllerButtonDPAD_DOWN;
                Controller_Send_left                 = spc.ProControllerButtonDPAD_LEFT;
                Controller_Send_right                = spc.ProControllerButtonDPAD_RIGHT;
                Controller_Send_up                   = spc.ProControllerButtonDPAD_UP;
                Controller_Send_A                    = spc.ProControllerButtonB;
                Controller_Send_B                    = spc.ProControllerButtonA;  
                Controller_Send_Y                    = spc.ProControllerButtonX;
                Controller_Send_X                    = spc.ProControllerButtonY;
                Controller_Send_lefttriggerposition  = spc.ProControllerButtonSHOULDER_Left_2 ? 255 : 0;
                Controller_Send_righttriggerposition = spc.ProControllerButtonSHOULDER_Right_2 ? 255 : 0;
                Controller_Send_leftbumper           = spc.ProControllerButtonSHOULDER_Left_1;
                Controller_Send_rightbumper          = spc.ProControllerButtonSHOULDER_Right_1;
                Controller_Send_leftstick            = spc.ProControllerButtonSTICK_Left;
                Controller_Send_rightstick           = spc.ProControllerButtonSTICK_Right;
                Controller_Send_start                = spc.ProControllerButtonHOME;
                Controller_Send_back                 = spc.ProControllerButtonCAPTURE;
                XBC.Set(Controller_Send_back, Controller_Send_start, Controller_Send_A, Controller_Send_B, Controller_Send_X, Controller_Send_Y, Controller_Send_up, Controller_Send_left, Controller_Send_down, Controller_Send_right, Controller_Send_leftstick, Controller_Send_rightstick, Controller_Send_leftbumper, Controller_Send_rightbumper, Controller_Send_leftstickx, Controller_Send_leftsticky, Controller_Send_rightstickx, Controller_Send_rightsticky, Controller_Send_lefttriggerposition, Controller_Send_righttriggerposition, Controller_Send_xbox);
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