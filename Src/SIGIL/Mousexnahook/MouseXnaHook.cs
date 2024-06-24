using System;
using System.Threading.Tasks;
using Mousexnahook;
using Microsoft.Xna.Framework.Input;
using System.Runtime.InteropServices;
using System.Diagnostics;
using Valuechanges;

namespace MouseXnaHookAPI
{
    public class MouseXnaHook : IDisposable
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
        private MouseState mousestate;
        private Form1 form1;
        private Stopwatch PollingRate;
        private double pollingrateperm = 0, pollingratetemp = 0, pollingratedisplay = 0, pollingrate;
        private string inputdelaybutton = "", inputdelay = "", inputdelaytemp = "";
        public Valuechange ValueChange;
        private double delay, elapseddown, elapsedup, elapsed;
        private bool getstate = false;
        private bool[] wd = { false };
        private bool[] wu = { false };
        private bool[] ws = { false };
        private void valchanged(int n, bool val)
        {
            if (val)
            {
                if (!wd[n] & !ws[n])
                {
                    wd[n] = true;
                    ws[n] = true;
                    return;
                }
                if (wd[n] & ws[n])
                {
                    wd[n] = false;
                }
                ws[n] = true;
                wu[n] = false;
            }
            if (!val)
            {
                if (!wu[n] & ws[n])
                {
                    wu[n] = true;
                    ws[n] = false;
                    return;
                }
                if (wu[n] & !ws[n])
                {
                    wu[n] = false;
                }
                ws[n] = false;
                wd[n] = false;
            }
        }
        public MouseXnaHook()
        {
            TimeBeginPeriod(1);
            NtSetTimerResolution(1, true, ref CurrentResolution);
            running = true;
        }
        public void ViewData(string inputdelaybutton = "")
        {
            if (!formvisible)
            {
                form1 = new Form1();
                PollingRate = new Stopwatch();
                PollingRate.Start();
                ValueChange = new Valuechange();
                this.inputdelaybutton = inputdelaybutton;
                formvisible = true;
                Task.Run(() => form1.SetVisible());
            }
        }
        public void Close()
        {
            if (formvisible)
                if (form1.Visible)
                    form1.Close();
            running = false;
        }
        private void taskM()
        {
            for (; ; )
            {
                if (!running)
                    break;
                ProcessStateLogic();
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
                        {
                            inputdelaytemp = inputdelay;
                            inputdelay = line;
                        }
                    valchanged(0, inputdelay.Contains("True") | (!inputdelay.Contains("True") & !inputdelay.Contains("False") & inputdelay != inputdelaytemp));
                    if (wd[0])
                    {
                        getstate = true;
                    }
                    if (inputdelay.Contains("False") | (!inputdelay.Contains("True") & !inputdelay.Contains("False") & inputdelay == inputdelaytemp))
                        getstate = false;
                    if (getstate)
                    {
                        elapseddown = (double)PollingRate.ElapsedTicks / (Stopwatch.Frequency / 1000L);
                        elapsed = 0;
                    }
                    if (wu[0])
                    {
                        elapsedup = (double)PollingRate.ElapsedTicks / (Stopwatch.Frequency / 1000L);
                        elapsed = elapsedup - elapseddown;
                    }
                    ValueChange[0] = inputdelay.Contains("False") | (!inputdelay.Contains("True") & !inputdelay.Contains("False") & inputdelay == inputdelaytemp) ? elapsed : 0;
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
            return true;
        }
        private void ProcessStateLogic()
        {
            mousestate = Mouse.GetState();
            MouseButtons0 = mousestate.LeftButton == Microsoft.Xna.Framework.Input.ButtonState.Pressed;
            MouseButtons1 = mousestate.RightButton == Microsoft.Xna.Framework.Input.ButtonState.Pressed;
            MouseButtons2 = mousestate.MiddleButton == Microsoft.Xna.Framework.Input.ButtonState.Pressed;
            MouseButtons3 = mousestate.XButton1 == Microsoft.Xna.Framework.Input.ButtonState.Pressed;
            MouseButtons4 = mousestate.XButton2 == Microsoft.Xna.Framework.Input.ButtonState.Pressed;
            MouseAxisX = mousestate.X;
            MouseAxisY = mousestate.Y;
            MouseAxisZ = mousestate.ScrollWheelValue;
        }
        public void Dispose()
        {
            GC.Collect();
            GC.WaitForPendingFinalizers();
            GC.SuppressFinalize(this);
        }
    }
}