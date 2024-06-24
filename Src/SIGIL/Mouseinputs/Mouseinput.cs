using SharpDX.DirectInput;
using System;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using Mouseinputs;
using System.Collections.Generic;
using System.Diagnostics;
using Valuechanges;

namespace MouseInputsAPI
{
    public class MouseInput : IDisposable
    {
        [DllImport("winmm.dll", EntryPoint = "timeBeginPeriod")]
        private static extern uint TimeBeginPeriod(uint ms);
        [DllImport("winmm.dll", EntryPoint = "timeEndPeriod")]
        private static extern uint TimeEndPeriod(uint ms);
        [DllImport("ntdll.dll", EntryPoint = "NtSetTimerResolution")]
        private static extern void NtSetTimerResolution(uint DesiredResolution, bool SetResolution, ref uint CurrentResolution);
        private static uint CurrentResolution = 0;
        private bool running, formvisible;
        private DirectInput directInput = new DirectInput();
        private int number, inc;
        private static List<Mouse> mouses = new List<Mouse>();
        private Mouse ms;
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
        public MouseInput()
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
                    valchanged(0, inputdelay != inputdelaytemp);
                    if (wd[0])
                    {
                        getstate = true;
                    }
                    if (inputdelay == inputdelaytemp)
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
                    ValueChange[0] = inputdelay == inputdelaytemp ? elapsed : 0;
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
        private Mouse[] mouse = new Mouse[] { null, null, null, null };
        private Guid[] mouseGuid = new Guid[] { Guid.Empty, Guid.Empty, Guid.Empty, Guid.Empty };
        private int mnum = 0;
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
            if (number <= 1)
            {
                directInput = new DirectInput();
                mouse = new Mouse[] { null, null, null, null };
                mouseGuid = new Guid[] { Guid.Empty, Guid.Empty, Guid.Empty, Guid.Empty };
                mnum = 0;
                foreach (var deviceInstance in directInput.GetDevices(SharpDX.DirectInput.DeviceType.Mouse, DeviceEnumerationFlags.AllDevices))
                {
                    mouseGuid[mnum] = deviceInstance.InstanceGuid;
                    mouse[mnum] = new Mouse(directInput);
                    mouse[mnum].Properties.BufferSize = 128;
                    mouses.Add(mouse[mnum]);
                    mnum++;
                }
            }
            if (mouses.Count == 0)
            {
                return false;
            }
            else
            {
                ms = mouses[inc];
                ms.Acquire();
                return true;
            }
        }
        private void ProcessStateLogic()
        {
            ms.Poll();
            var datas = ms.GetBufferedData();
            foreach (var state in datas)
            {
                if (state.Offset == MouseOffset.X)
                    MouseAxisX = state.Value;
                if (state.Offset == MouseOffset.Y)
                    MouseAxisY = state.Value;
                if (state.Offset == MouseOffset.Z)
                    MouseAxisZ = state.Value;
                if (state.Offset == MouseOffset.Buttons0 & state.Value == 128)
                    MouseButtons0 = true;
                if (state.Offset == MouseOffset.Buttons0 & state.Value == 0)
                    MouseButtons0 = false;
                if (state.Offset == MouseOffset.Buttons1 & state.Value == 128)
                    MouseButtons1 = true;
                if (state.Offset == MouseOffset.Buttons1 & state.Value == 0)
                    MouseButtons1 = false;
                if (state.Offset == MouseOffset.Buttons2 & state.Value == 128)
                    MouseButtons2 = true;
                if (state.Offset == MouseOffset.Buttons2 & state.Value == 0)
                    MouseButtons2 = false;
                if (state.Offset == MouseOffset.Buttons3 & state.Value == 128)
                    MouseButtons3 = true;
                if (state.Offset == MouseOffset.Buttons3 & state.Value == 0)
                    MouseButtons3 = false;
                if (state.Offset == MouseOffset.Buttons4 & state.Value == 128)
                    MouseButtons4 = true;
                if (state.Offset == MouseOffset.Buttons4 & state.Value == 0)
                    MouseButtons4 = false;
                if (state.Offset == MouseOffset.Buttons5 & state.Value == 128)
                    MouseButtons5 = true;
                if (state.Offset == MouseOffset.Buttons5 & state.Value == 0)
                    MouseButtons5 = false;
                if (state.Offset == MouseOffset.Buttons6 & state.Value == 128)
                    MouseButtons6 = true;
                if (state.Offset == MouseOffset.Buttons6 & state.Value == 0)
                    MouseButtons6 = false;
                if (state.Offset == MouseOffset.Buttons7 & state.Value == 128)
                    MouseButtons7 = true;
                if (state.Offset == MouseOffset.Buttons7 & state.Value == 0)
                    MouseButtons7 = false;
            }
        }
        public void Dispose()
        {
            GC.Collect();
            GC.WaitForPendingFinalizers();
            GC.SuppressFinalize(this);
        }
    }
}