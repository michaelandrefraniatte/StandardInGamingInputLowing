using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Mouserawhooks;
using RawInput_dll;

namespace MouseRawHooksAPI
{
    public class MouseRawHook
    {
        [DllImport("winmm.dll", EntryPoint = "timeBeginPeriod")]
        private static extern uint TimeBeginPeriod(uint ms);
        [DllImport("winmm.dll", EntryPoint = "timeEndPeriod")]
        private static extern uint TimeEndPeriod(uint ms);
        [DllImport("ntdll.dll", EntryPoint = "NtSetTimerResolution")]
        private static extern void NtSetTimerResolution(uint DesiredResolution, bool SetResolution, ref uint CurrentResolution);
        private static uint CurrentResolution = 0;
        private bool running, formvisible;
        private int number, inc;
        private RawInput _rawinput;
        private const bool CaptureOnlyInForeground = false;
        private static List<string> devicehandle = new List<string>();
        private Form1 form1 = new Form1();
        private Stopwatch PollingRate;
        private double pollingrateperm = 0, pollingratetemp = 0, pollingratedisplay = 0, pollingrate;
        public MouseRawHook()
        {
            TimeBeginPeriod(1);
            NtSetTimerResolution(1, true, ref CurrentResolution);
            _rawinput = new RawInput(this.form1.Handle, CaptureOnlyInForeground);
            _rawinput.ButtonPressed += OnButtonPressed;
            running = true;
        }
        public void ViewData()
        {
            if (!form1.Visible)
            {
                PollingRate = new Stopwatch();
                PollingRate.Start();
                formvisible = true;
                form1.SetVisible();
            }
        }
        public void Close()
        {
            running = false;
            Thread.Sleep(100);
            _rawinput.ButtonPressed -= OnButtonPressed;
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
                    pollingratedisplay++;
                    pollingratetemp = pollingrateperm;
                    pollingrateperm = PollingRate.ElapsedMilliseconds;
                    if (pollingratedisplay > 300)
                    {
                        pollingrate = pollingrateperm - pollingratetemp;
                        pollingratedisplay = 0;
                    }
                    string str = "MouseAxisX : " + MouseAxisX + Environment.NewLine;
                    str += "MouseAxisY : " + MouseAxisY + Environment.NewLine;
                    str += "MouseAxisZ : " + MouseAxisZ + Environment.NewLine;
                    str += "MouseButtons0 : " + MouseButtons0 + Environment.NewLine;
                    str += "MouseButtons1 : " + MouseButtons1 + Environment.NewLine;
                    str += "MouseButtons2 : " + MouseButtons2 + Environment.NewLine;
                    str += "MouseButtons3 : " + MouseButtons3 + Environment.NewLine;
                    str += "MouseButtons4 : " + MouseButtons4 + Environment.NewLine;
                    str += "MouseButtons5 : " + MouseButtons5 + Environment.NewLine;
                    str += "MouseButtons6 : " + MouseButtons6 + Environment.NewLine;
                    str += "MouseButtons7 : " + MouseButtons7 + Environment.NewLine;
                    str += "PollingRate : " + pollingrate + " ms" + Environment.NewLine;
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
        public bool MouseButtons5;
        public bool MouseButtons6;
        public bool MouseButtons7;
        public int MouseAxisX;
        public int MouseAxisY;
        public int MouseAxisZ;
        public bool Scan(int number = 0)
        {
            this.number = number;
            inc = number < 2 ? 0 : number - 1;
            return true;
        }
        private void OnButtonPressed(object sender, RawInputEventArg e)
        {
            devicehandle.Add(e.ButtonPressEvent.DeviceHandle.ToString());
            devicehandle = devicehandle.Distinct().ToList();
            if (devicehandle[inc] == e.ButtonPressEvent.DeviceHandle.ToString())
            {
                MouseAxisX = e.ButtonPressEvent.lLastX;
                MouseAxisY = e.ButtonPressEvent.lLastY;
                MouseAxisZ = e.ButtonPressEvent.usButtonData;
                if (e.ButtonPressEvent.ulButtons == 1)
                    MouseButtons0 = true;
                if (e.ButtonPressEvent.ulButtons == 2)
                    MouseButtons0 = false;
                if (e.ButtonPressEvent.ulButtons == 4)
                    MouseButtons1 = true;
                if (e.ButtonPressEvent.ulButtons == 8)
                    MouseButtons1 = false;
                if (e.ButtonPressEvent.ulButtons == 16)
                    MouseButtons2 = true;
                if (e.ButtonPressEvent.ulButtons == 32)
                    MouseButtons2 = false;
                if (e.ButtonPressEvent.ulButtons == 256)
                    MouseButtons3 = true;
                if (e.ButtonPressEvent.ulButtons == 512)
                    MouseButtons3 = false;
                if (e.ButtonPressEvent.ulButtons == 64)
                    MouseButtons4 = true;
                if (e.ButtonPressEvent.ulButtons == 128)
                    MouseButtons4 = false;
            }
        }
    }
}