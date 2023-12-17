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
                if (Mouse1AxisZ != 0)
                    Task.Run(() => InitMouse());
                if (formvisible)
                {
                    string str = "Mouse1AxisX : " + Mouse1AxisX + Environment.NewLine;
                    str += "Mouse1AxisY : " + Mouse1AxisY + Environment.NewLine;
                    str += "Mouse1AxisZ : " + Mouse1AxisZ + Environment.NewLine;
                    str += "Mouse1Buttons0 : " + Mouse1Buttons0 + Environment.NewLine;
                    str += "Mouse1Buttons1 : " + Mouse1Buttons1 + Environment.NewLine;
                    str += "Mouse1Buttons2 : " + Mouse1Buttons2 + Environment.NewLine;
                    str += "Mouse1Buttons3 : " + Mouse1Buttons3 + Environment.NewLine;
                    str += "Mouse1Buttons4 : " + Mouse1Buttons4 + Environment.NewLine;
                    str += "Mouse1Buttons5 : " + Mouse1Buttons5 + Environment.NewLine;
                    str += "Mouse1Buttons6 : " + Mouse1Buttons6 + Environment.NewLine;
                    str += "Mouse1Buttons7 : " + Mouse1Buttons7 + Environment.NewLine;
                    str += "Mouse2AxisX : " + Mouse2AxisX + Environment.NewLine;
                    str += "Mouse2AxisY : " + Mouse2AxisY + Environment.NewLine;
                    str += "Mouse2AxisZ : " + Mouse2AxisZ + Environment.NewLine;
                    str += "Mouse2Buttons0 : " + Mouse2Buttons0 + Environment.NewLine;
                    str += "Mouse2Buttons1 : " + Mouse2Buttons1 + Environment.NewLine;
                    str += "Mouse2Buttons2 : " + Mouse2Buttons2 + Environment.NewLine;
                    str += "Mouse2Buttons3 : " + Mouse2Buttons3 + Environment.NewLine;
                    str += "Mouse2Buttons4 : " + Mouse2Buttons4 + Environment.NewLine;
                    str += "Mouse2Buttons5 : " + Mouse2Buttons5 + Environment.NewLine;
                    str += "Mouse2Buttons6 : " + Mouse2Buttons6 + Environment.NewLine;
                    str += "Mouse2Buttons7 : " + Mouse2Buttons7 + Environment.NewLine;
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
            Mouse1AxisZ = 0;
        }
        private static Mouse[] mouse = new Mouse[] { null };
        private static Guid[] mouseGuid = new Guid[] { Guid.Empty };
        private static int mnum = 0;
        public bool Mouse1Buttons0;
        public bool Mouse1Buttons1;
        public bool Mouse1Buttons2;
        public bool Mouse1Buttons3;
        public bool Mouse1Buttons4;
        public bool Mouse1Buttons5;
        public bool Mouse1Buttons6;
        public bool Mouse1Buttons7;
        public int Mouse1AxisX;
        public int Mouse1AxisY;
        public int Mouse1AxisZ;
        public static bool Mouse2Buttons0;
        public static bool Mouse2Buttons1;
        public static bool Mouse2Buttons2;
        public static bool Mouse2Buttons3;
        public static bool Mouse2Buttons4;
        public static bool Mouse2Buttons5;
        public static bool Mouse2Buttons6;
        public static bool Mouse2Buttons7;
        public static int Mouse2AxisX;
        public static int Mouse2AxisY;
        public static int Mouse2AxisZ;
        public bool ScanMouse()
        {
            try
            {
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
            for (int inc = 0; inc < mnum; inc++)
            {
                mouse[inc].Poll();
                var datas = mouse[inc].GetBufferedData();
                foreach (var state in datas)
                {
                    if (inc == 0 & state.Offset == MouseOffset.X)
                        Mouse1AxisX = state.Value;
                    if (inc == 0 & state.Offset == MouseOffset.Y)
                        Mouse1AxisY = state.Value;
                    if (inc == 0 & state.Offset == MouseOffset.Z)
                        Mouse1AxisZ = state.Value;
                    if (inc == 0 & state.Offset == MouseOffset.Buttons0 & state.Value == 128)
                        Mouse1Buttons0 = true;
                    if (inc == 0 & state.Offset == MouseOffset.Buttons0 & state.Value == 0)
                        Mouse1Buttons0 = false;
                    if (inc == 0 & state.Offset == MouseOffset.Buttons1 & state.Value == 128)
                        Mouse1Buttons1 = true;
                    if (inc == 0 & state.Offset == MouseOffset.Buttons1 & state.Value == 0)
                        Mouse1Buttons1 = false;
                    if (inc == 0 & state.Offset == MouseOffset.Buttons2 & state.Value == 128)
                        Mouse1Buttons2 = true;
                    if (inc == 0 & state.Offset == MouseOffset.Buttons2 & state.Value == 0)
                        Mouse1Buttons2 = false;
                    if (inc == 0 & state.Offset == MouseOffset.Buttons3 & state.Value == 128)
                        Mouse1Buttons3 = true;
                    if (inc == 0 & state.Offset == MouseOffset.Buttons3 & state.Value == 0)
                        Mouse1Buttons3 = false;
                    if (inc == 0 & state.Offset == MouseOffset.Buttons4 & state.Value == 128)
                        Mouse1Buttons4 = true;
                    if (inc == 0 & state.Offset == MouseOffset.Buttons4 & state.Value == 0)
                        Mouse1Buttons4 = false;
                    if (inc == 0 & state.Offset == MouseOffset.Buttons5 & state.Value == 128)
                        Mouse1Buttons5 = true;
                    if (inc == 0 & state.Offset == MouseOffset.Buttons5 & state.Value == 0)
                        Mouse1Buttons5 = false;
                    if (inc == 0 & state.Offset == MouseOffset.Buttons6 & state.Value == 128)
                        Mouse1Buttons6 = true;
                    if (inc == 0 & state.Offset == MouseOffset.Buttons6 & state.Value == 0)
                        Mouse1Buttons6 = false;
                    if (inc == 0 & state.Offset == MouseOffset.Buttons7 & state.Value == 128)
                        Mouse1Buttons7 = true;
                    if (inc == 0 & state.Offset == MouseOffset.Buttons7 & state.Value == 0)
                        Mouse1Buttons7 = false;
                    if (inc == 1 & state.Offset == MouseOffset.X)
                        Mouse2AxisX = state.Value;
                    if (inc == 1 & state.Offset == MouseOffset.Y)
                        Mouse2AxisY = state.Value;
                    if (inc == 1 & state.Offset == MouseOffset.Z)
                        Mouse2AxisZ = state.Value;
                    if (inc == 1 & state.Offset == MouseOffset.Buttons0 & state.Value == 128)
                        Mouse2Buttons0 = true;
                    if (inc == 1 & state.Offset == MouseOffset.Buttons0 & state.Value == 0)
                        Mouse2Buttons0 = false;
                    if (inc == 1 & state.Offset == MouseOffset.Buttons1 & state.Value == 128)
                        Mouse2Buttons1 = true;
                    if (inc == 1 & state.Offset == MouseOffset.Buttons1 & state.Value == 0)
                        Mouse2Buttons1 = false;
                    if (inc == 1 & state.Offset == MouseOffset.Buttons2 & state.Value == 128)
                        Mouse2Buttons2 = true;
                    if (inc == 1 & state.Offset == MouseOffset.Buttons2 & state.Value == 0)
                        Mouse2Buttons2 = false;
                    if (inc == 1 & state.Offset == MouseOffset.Buttons3 & state.Value == 128)
                        Mouse2Buttons3 = true;
                    if (inc == 1 & state.Offset == MouseOffset.Buttons3 & state.Value == 0)
                        Mouse2Buttons3 = false;
                    if (inc == 1 & state.Offset == MouseOffset.Buttons4 & state.Value == 128)
                        Mouse2Buttons4 = true;
                    if (inc == 1 & state.Offset == MouseOffset.Buttons4 & state.Value == 0)
                        Mouse2Buttons4 = false;
                    if (inc == 1 & state.Offset == MouseOffset.Buttons5 & state.Value == 128)
                        Mouse2Buttons5 = true;
                    if (inc == 1 & state.Offset == MouseOffset.Buttons5 & state.Value == 0)
                        Mouse2Buttons5 = false;
                    if (inc == 1 & state.Offset == MouseOffset.Buttons6 & state.Value == 128)
                        Mouse2Buttons6 = true;
                    if (inc == 1 & state.Offset == MouseOffset.Buttons6 & state.Value == 0)
                        Mouse2Buttons6 = false;
                    if (inc == 1 & state.Offset == MouseOffset.Buttons7 & state.Value == 128)
                        Mouse2Buttons7 = true;
                    if (inc == 1 & state.Offset == MouseOffset.Buttons7 & state.Value == 0)
                        Mouse2Buttons7 = false;
                }
            }
        }
    }
}