using SharpDX.DirectInput;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using Mouseinputs;

namespace MouseInputsAPI
{
    public class MouseInput
    {
        [DllImport("winmm.dll", EntryPoint = "timeBeginPeriod")]
        private static extern uint TimeBeginPeriod(uint ms);
        [DllImport("winmm.dll", EntryPoint = "timeEndPeriod")]
        private static extern uint TimeEndPeriod(uint ms);
        [DllImport("ntdll.dll", EntryPoint = "NtSetTimerResolution")]
        private static extern void NtSetTimerResolution(uint DesiredResolution, bool SetResolution, ref uint CurrentResolution);
        private static uint CurrentResolution = 0;
        private static bool running, formvisible;
        static DirectInput directInput = new DirectInput();
        private int number;
        public Form1 form1 = new Form1();
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
        public MouseInput()
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
        public void taskM()
        {
            for (; ; )
            {
                if (!running)
                    break;
                MouseInputProcess();
                System.Threading.Thread.Sleep(1);
                if (MouseAxisZ != 0)
                    Task.Run(() => InitMouse());
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
                    str += "MouseButtons5 : " + MouseButtons5 + Environment.NewLine;
                    str += "MouseButtons6 : " + MouseButtons6 + Environment.NewLine;
                    str += "MouseButtons7 : " + MouseButtons7 + Environment.NewLine;
                    str += Environment.NewLine;
                    form1.SetLabel1(str);
                }
            }
        }
        public void BeginPolling()
        {
            Task.Run(() => taskM());
        }
        public void InitMouse()
        {
            System.Threading.Thread.Sleep(100);
            MouseAxisZ = 0;
        }
        private static Mouse[] mouse = new Mouse[] { null };
        private static Guid[] mouseGuid = new Guid[] { Guid.Empty };
        private static int mnum = 0;
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
            try
            {
                this.number = number;
                directInput = new DirectInput();
                mouse = new Mouse[] { null, null };
                mouseGuid = new Guid[] { Guid.Empty, Guid.Empty };
                mnum = 0;
                foreach (var deviceInstance in directInput.GetDevices(SharpDX.DirectInput.DeviceType.Mouse, DeviceEnumerationFlags.AllDevices))
                {
                    mouseGuid[mnum] = deviceInstance.InstanceGuid;
                    mnum++;
                    if (mnum >= 2)
                        break;
                }
            }
            catch { }
            if (mouseGuid[0] == Guid.Empty)
            {
                return false;
            }
            else
            {
                for (int inc = 0; inc < mnum; inc++)
                {
                    mouse[inc] = new Mouse(directInput);
                    mouse[inc].Properties.BufferSize = 128;
                    mouse[inc].Acquire();
                }
                return true;
            }
        }
        public void MouseInputProcess()
        {
            int inc = number < 2 ? 0 : 1;
            mouse[inc].Poll();
            var datas = mouse[inc].GetBufferedData();
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
    }
}