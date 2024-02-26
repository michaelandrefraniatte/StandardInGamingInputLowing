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
using Networks;
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
        private static double statex = 0f, statey = 0f, mousex = 0f, mousey = 0f, mousestatex = 0f, mousestatey = 0f, dzx = 0.0f, dzy = 0.0f, viewpower1x = 0f, viewpower2x = 1f, viewpower3x = 0f, viewpower1y = 0.25f, viewpower2y = 0.75f, viewpower3y = 0f, viewpower05x = 0f, viewpower05y = 0f;
        private static bool getstate;
        private static int sleeptime = 1;
        private XInput xi = new XInput();
        public static Valuechange ValueChange = new Valuechange();
        public static string localip = "192.168.1.10";
        public static string port = "62000";
        public void Close()
        {
            try
            {
                running = false;
                Thread.Sleep(100);
                Network.Disconnect();
                xi.Close();
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
            Network.Connect(localip, port);
            Task.Run(() => task());
        }
        private void task()
        {
            for (; ; )
            {
                if (!running)
                    break;
                string control           = xi.ControllerThumbRightX + "," + xi.ControllerThumbRightY + "," + xi.ControllerThumbLeftX + "," + xi.ControllerThumbLeftY + "," + xi.ControllerButtonUpPressed + "," + xi.ControllerButtonLeftPressed + "," + xi.ControllerButtonDownPressed + "," + xi.ControllerButtonRightPressed + "," + xi.ControllerButtonBackPressed + "," + xi.ControllerButtonStartPressed + "," + xi.ControllerThumbpadLeftPressed + "," + xi.ControllerButtonShoulderLeftPressed + "," + xi.ControllerButtonShoulderRightPressed + "," + xi.ControllerButtonAPressed + "," + xi.ControllerButtonBPressed + "," + xi.ControllerButtonXPressed + "," + xi.ControllerButtonYPressed + "," + xi.ControllerThumbpadRightPressed + "," + xi.ControllerTriggerLeftPosition + "," + xi.ControllerTriggerRightPosition + ",end";
                Network.rawdataavailable = controlToByteArray(control);
                /*xi.ViewData();*/
                Thread.Sleep(sleeptime);
            }
        }
        public static byte[] controlToByteArray(string control)
        {
            byte[] data = Encoding.ASCII.GetBytes(control);
            return data;
        }
        private double Scale(double value, double min, double max, double minScale, double maxScale)
        {
            double scaled = minScale + (double)(value - min) / (max - min) * (maxScale - minScale);
            return scaled;
        }
    }
}