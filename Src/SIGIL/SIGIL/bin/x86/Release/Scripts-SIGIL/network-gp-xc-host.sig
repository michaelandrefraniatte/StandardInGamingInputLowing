using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Globalization;
using System.IO;
using System.Numerics;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows;
using System.Reflection;
using controllers;
using System.Diagnostics;
using Valuechanges;
using Networkshost;

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
        private static double statex = 0f, statey = 0f, mousex = 0f, mousey = 0f, mousestatex = 0f, mousestatey = 0f, dzx = 0.0f, dzy = 0.0f, viewpower1x = 0f, viewpower2x = 1f, viewpower3x = 0f, viewpower1y = 0.25f, viewpower2y = 0.75f, viewpower3y = 0f, viewpower05x = 0f, viewpower05y = 0f;
        private static bool getstate;
        private static int sleeptime = 1;
        private NetworkHost networkhost = new NetworkHost();
        private static XBoxController XBC = new XBoxController();
        public static Valuechange ValueChange = new Valuechange();
        public static string localip = "192.168.1.14";
        public static string port = "62000";
        public static string[] control = new string[] { "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "" };
        public void Close()
        {
            try
            {
                running = false;
                Thread.Sleep(100);
                networkhost.Disconnect();
                XBC.Disconnect();
                networkhost.Dispose();
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
            networkhost.Connect(localip, port);
            XBC.Connect();
            Task.Run(() => task());
        }
        private void task()
        {
            for (; ; )
            {
                if (!running)
                    break;
                if (networkhost.rawdataavailable != "")
                {
                    control = stringToControl(networkhost.rawdataavailable);
                    mousex  = Convert.ToSingle(control[0]);
                    mousey  = Convert.ToSingle(control[1]);
                    statex = Math.Abs(mousex) <= 32767f ? mousex : Math.Sign(mousex) * 32767f;
                    statey = Math.Abs(mousey) <= 32767f ? mousey : Math.Sign(mousey) * 32767f;
                    Controller_Send_rightstickx          = statex;
                    Controller_Send_rightsticky          = statey;
                    mousex                               = Convert.ToSingle(control[2]);
                    mousey                               = Convert.ToSingle(control[3]);
                    Controller_Send_leftstickx           = Math.Abs(mousex) <= 32767f ? mousex : Math.Sign(mousex) * 32767f;
                    Controller_Send_leftsticky           = Math.Abs(mousey) <= 32767f ? mousey : Math.Sign(mousey) * 32767f;
                    Controller_Send_up                   = bool.Parse(control[4]);
                    Controller_Send_left                 = bool.Parse(control[5]);
                    Controller_Send_down                 = bool.Parse(control[6]);
                    Controller_Send_right                = bool.Parse(control[7]);
                    Controller_Send_back                 = bool.Parse(control[8]);
                    Controller_Send_start                = bool.Parse(control[9]);
                    Controller_Send_leftstick            = bool.Parse(control[10]);
                    Controller_Send_leftbumper           = bool.Parse(control[11]);
                    Controller_Send_rightbumper          = bool.Parse(control[12]);
                    Controller_Send_A                    = bool.Parse(control[13]);
                    Controller_Send_B                    = bool.Parse(control[14]);
                    Controller_Send_X                    = bool.Parse(control[15]);
                    Controller_Send_Y                    = bool.Parse(control[16]);
                    Controller_Send_rightstick           = bool.Parse(control[17]);
                    Controller_Send_lefttriggerposition  = Convert.ToSingle(control[18]);
                    Controller_Send_righttriggerposition = Convert.ToSingle(control[19]);
                    XBC.Set(Controller_Send_back, Controller_Send_start, Controller_Send_A, Controller_Send_B, Controller_Send_X, Controller_Send_Y, Controller_Send_up, Controller_Send_left, Controller_Send_down, Controller_Send_right, Controller_Send_leftstick, Controller_Send_rightstick, Controller_Send_leftbumper, Controller_Send_rightbumper, Controller_Send_leftstickx, Controller_Send_leftsticky, Controller_Send_rightstickx, Controller_Send_rightsticky, Controller_Send_lefttriggerposition, Controller_Send_righttriggerposition, Controller_Send_xbox);
                    /*NetworkHost.ViewData();*/
                    /*XBC.ViewData();*/
                }
                Thread.Sleep(sleeptime);
            }
        }
        public static string[] stringToControl(string unsplitstring)
        {
            string[] splitstring = unsplitstring.Split(',');
            List<string> newsplitstring = new List<string>();
            foreach (string valuestring in splitstring)
            {
                newsplitstring.Add(valuestring);
            }
            return newsplitstring.ToArray();
        }
    }
}