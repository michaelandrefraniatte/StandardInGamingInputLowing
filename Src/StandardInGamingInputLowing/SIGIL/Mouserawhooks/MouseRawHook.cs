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
using Valuechanges;

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
        private string inputdelaybutton = "", inputdelay = "";
        public Valuechange ValueChange;
        private double delay, elapseddown, elapsedup, elapsed;
        private bool getstate = false;
        private int[] wd = { 2 };
        private int[] wu = { 2 };
        public void valchanged(int n, bool val)
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
        public MouseRawHook()
        {
            TimeBeginPeriod(1);
            NtSetTimerResolution(1, true, ref CurrentResolution);
            _rawinput = new RawInput(this.form1.Handle, CaptureOnlyInForeground);
            _rawinput.ButtonPressed += OnButtonPressed;
            running = true;
        }
        public void ViewData(string inputdelaybutton = "")
        {
            if (!form1.Visible)
            {
                PollingRate = new Stopwatch();
                PollingRate.Start();
                ValueChange = new Valuechange();
                this.inputdelaybutton = inputdelaybutton;
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
                    pollingrateperm = (double)PollingRate.ElapsedTicks / (Stopwatch.Frequency / 1000L);
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
                    string txt = str;
                    string[] lines = txt.Split(new string[] { Environment.NewLine }, StringSplitOptions.None);
                    foreach (string line in lines)
                        if (line.Contains(inputdelaybutton + " : "))
                            inputdelay = line;
                    valchanged(0, inputdelay.Contains("True"));
                    if (wd[0] == 1)
                    {
                        getstate = true;
                    }
                    if (inputdelay.Contains("False"))
                        getstate = false;
                    if (getstate)
                    {
                        elapseddown = (double)PollingRate.ElapsedTicks / (Stopwatch.Frequency / 1000L);
                        elapsed = 0;
                    }
                    if (wu[0] == 1)
                    {
                        elapsedup = (double)PollingRate.ElapsedTicks / (Stopwatch.Frequency / 1000L);
                        elapsed = elapsedup - elapseddown;
                    }
                    ValueChange[0] = inputdelay.Contains("False") ? elapsed : 0;
                    if (ValueChange._ValueChange[0] > 0)
                    {
                        delay = ValueChange._ValueChange[0];
                    }
                    str += "InputDelay : " + delay + " ms" + Environment.NewLine;
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