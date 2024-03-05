﻿using System;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using Mouserawinputs;
using SharpDX.Multimedia;
using SharpDX.RawInput;

namespace MouseRawInputsAPI
{
    public class MouseRawInputs
    {
        [DllImport("winmm.dll", EntryPoint = "timeBeginPeriod")]
        private static extern uint TimeBeginPeriod(uint ms);
        [DllImport("winmm.dll", EntryPoint = "timeEndPeriod")]
        private static extern uint TimeEndPeriod(uint ms);
        [DllImport("ntdll.dll", EntryPoint = "NtSetTimerResolution")]
        private static extern void NtSetTimerResolution(uint DesiredResolution, bool SetResolution, ref uint CurrentResolution);
        private static uint CurrentResolution = 0;
        private bool running, formvisible;
        private int number;
        private Form1 form1 = new Form1();
        public MouseRawInputs()
        {
            TimeBeginPeriod(1);
            NtSetTimerResolution(1, true, ref CurrentResolution);
            running = true;
        }
        public void ViewData()
        {
            if (!form1.Visible)
            {
                formvisible = true;
                form1.SetVisible();
            }
        }
        public void Close()
        {
            running = false;
        }
        private void taskM()
        {
            for (; ; )
            {
                if (!running)
                    break;
                System.Threading.Thread.Sleep(1);
                if (MouseAxisZ != 0)
                    Task.Run(() => Init());
                if (formvisible)
                {
                    string str = "MouseAxisX : " + MouseAxisX + Environment.NewLine;
                    str += "MouseAxisY : " + MouseAxisY + Environment.NewLine;
                    str += "MouseAxisZ : " + MouseAxisZ + Environment.NewLine;
                    str += "MouseButtons0 : " + MouseButtons0 + Environment.NewLine;
                    str += "MouseButtons1 : " + MouseButtons1 + Environment.NewLine;
                    str += "MouseButtons2 : " + MouseButtons2 + Environment.NewLine;
                    str += "MouseButtons3 : " + MouseButtons3 + Environment.NewLine;
                    str += "MouseButtons4 : " + MouseButtons4 + Environment.NewLine;
                    str += Environment.NewLine;
                    form1.SetLabel1(str);
                }
            }
        }
        public void BeginPolling()
        {
            Task.Run(() => taskM());
        }
        public void Init()
        {
            System.Threading.Thread.Sleep(100);
            MouseAxisZ = 0;
        }
        public bool MouseButtons0;
        public bool MouseButtons1;
        public bool MouseButtons2;
        public bool MouseButtons3;
        public bool MouseButtons4;
        public int MouseAxisX;
        public int MouseAxisY;
        public int MouseAxisZ;
        public bool Scan(int number = 0)
        {
            this.number = number;
            Device.RegisterDevice(UsagePage.Generic, UsageId.GenericMouse, DeviceFlags.None);
            Device.MouseInput += Device_MouseInput;
            return true;
        }
        private void Device_MouseInput(object sender, MouseInputEventArgs e)
        {
            MouseAxisX = e.X;
            MouseAxisY = e.Y;
            MouseAxisZ = e.WheelDelta;
            if (e.ButtonFlags == MouseButtonFlags.Button1Down)
                MouseButtons0 = true;
            if (e.ButtonFlags == MouseButtonFlags.Button1Up)
                MouseButtons0 = false;
            if (e.ButtonFlags == MouseButtonFlags.Button2Down)
                MouseButtons1 = true;
            if (e.ButtonFlags == MouseButtonFlags.Button2Up)
                MouseButtons1 = false;
            if (e.ButtonFlags == MouseButtonFlags.Button3Down)
                MouseButtons2 = true;
            if (e.ButtonFlags == MouseButtonFlags.Button3Up)
                MouseButtons2 = false;
            if (e.ButtonFlags == MouseButtonFlags.Button4Down)
                MouseButtons3 = true;
            if (e.ButtonFlags == MouseButtonFlags.Button4Up)
                MouseButtons3 = false;
            if (e.ButtonFlags == MouseButtonFlags.Button5Down)
                MouseButtons4 = true;
            if (e.ButtonFlags == MouseButtonFlags.Button5Up)
                MouseButtons4 = false;
        }
    }
}