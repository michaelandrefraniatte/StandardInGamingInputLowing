﻿using Microsoft.Win32.SafeHandles;
using System.Globalization;
using System;
using System.Runtime.InteropServices;
using System.Diagnostics;
using Valuechanges;
using System.Threading.Tasks;

namespace controllers
{
    public class Valuechanges
    {
        [DllImport("winmm.dll", EntryPoint = "timeBeginPeriod")]
        private static extern uint TimeBeginPeriod(uint ms);
        [DllImport("winmm.dll", EntryPoint = "timeEndPeriod")]
        private static extern uint TimeEndPeriod(uint ms);
        [DllImport("ntdll.dll", EntryPoint = "NtSetTimerResolution")]
        private static extern void NtSetTimerResolution(uint DesiredResolution, bool SetResolution, ref uint CurrentResolution);
        private static uint CurrentResolution = 0;
        public bool[] _valuechange = { false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false };
        public bool[] _ValueChange = { false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false };
        public Valuechanges()
        {
            TimeBeginPeriod(1);
            NtSetTimerResolution(1, true, ref CurrentResolution);
        }
        public bool this[int index]
        {
            get { return _ValueChange[index]; }
            set
            {
                if (_valuechange[index] != value)
                    _ValueChange[index] = true;
                else
                    _ValueChange[index] = false;
                _valuechange[index] = value;
            }
        }
    }
    public class XBoxController
    {
        [DllImport("winmm.dll", EntryPoint = "timeBeginPeriod")]
        private static extern uint TimeBeginPeriod(uint ms);
        [DllImport("winmm.dll", EntryPoint = "timeEndPeriod")]
        private static extern uint TimeEndPeriod(uint ms);
        [DllImport("ntdll.dll", EntryPoint = "NtSetTimerResolution")]
        private static extern void NtSetTimerResolution(uint DesiredResolution, bool SetResolution, ref uint CurrentResolution);
        private static uint CurrentResolution = 0;
        private int number, inc;
        private Valuechanges ValueChanges = new Valuechanges();
        private const string SCP_BUS_CLASS_GUID = "{F679F562-3164-42CE-A4DB-E7DDBE723909}";
        private SafeFileHandle _deviceHandle;
        private int transferred = 0;
        private byte[] outputBuffer = null;
        private bool formvisible;
        private Form1 form1 = new Form1();
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
        public XBoxController()
        {
            TimeBeginPeriod(1);
            NtSetTimerResolution(1, true, ref CurrentResolution);
        }
        public void ViewData(string inputdelaybutton = "")
        {
            if (!formvisible)
            {
                PollingRate = new Stopwatch();
                PollingRate.Start();
                ValueChange = new Valuechange();
                this.inputdelaybutton = inputdelaybutton;
                formvisible = true;
                Task.Run(() => form1.SetVisible());
            }
        }
        public void Connect(int number = 0)
        {
            this.number = number;
            string devicePath = "";
            if (Find(new Guid(SCP_BUS_CLASS_GUID), ref devicePath, 0))
            {
                _deviceHandle = GetHandle(devicePath);
            }
            inc = number < 2 ? 1 : number;
            PlugIn(inc);
        }
        public void Disconnect()
        {
            Set(false, false, false, false, false, false, false, false, false, false, false, false, false, false, 0, 0, 0, 0, 0, 0, false);
            Unplug(inc);
        }
        public void Set(bool back, bool start, bool A, bool B, bool X, bool Y, bool up, bool left, bool down, bool right, bool leftstick, bool rightstick, bool leftbumper, bool rightbumper, double leftstickx, double leftsticky, double rightstickx, double rightsticky, double lefttriggerposition, double righttriggerposition, bool xbox)
        {
            ValueChanges[0] = back;
            if (ValueChanges._ValueChange[0] & ValueChanges._valuechange[0])
                Buttons ^= X360Buttons.Back;
            if (!back)
                Buttons &= ~X360Buttons.Back;
            ValueChanges[1] = start;
            if (ValueChanges._ValueChange[1] & ValueChanges._valuechange[1])
                Buttons ^= X360Buttons.Start;
            if (!start)
                Buttons &= ~X360Buttons.Start;
            ValueChanges[2] = A;
            if (ValueChanges._ValueChange[2] & ValueChanges._valuechange[2])
                Buttons ^= X360Buttons.A;
            if (!A)
                Buttons &= ~X360Buttons.A;
            ValueChanges[3] = B;
            if (ValueChanges._ValueChange[3] & ValueChanges._valuechange[3])
                Buttons ^= X360Buttons.B;
            if (!B)
                Buttons &= ~X360Buttons.B;
            ValueChanges[4] = X;
            if (ValueChanges._ValueChange[4] & ValueChanges._valuechange[4])
                Buttons ^= X360Buttons.X;
            if (!X)
                Buttons &= ~X360Buttons.X;
            ValueChanges[5] = Y;
            if (ValueChanges._ValueChange[5] & ValueChanges._valuechange[5])
                Buttons ^= X360Buttons.Y;
            if (!Y)
                Buttons &= ~X360Buttons.Y;
            ValueChanges[6] = up;
            if (ValueChanges._ValueChange[6] & ValueChanges._valuechange[6])
                Buttons ^= X360Buttons.Up;
            if (!up)
                Buttons &= ~X360Buttons.Up;
            ValueChanges[7] = left;
            if (ValueChanges._ValueChange[7] & ValueChanges._valuechange[7])
                Buttons ^= X360Buttons.Left;
            if (!left)
                Buttons &= ~X360Buttons.Left;
            ValueChanges[8] = down;
            if (ValueChanges._ValueChange[8] & ValueChanges._valuechange[8])
                Buttons ^= X360Buttons.Down;
            if (!down)
                Buttons &= ~X360Buttons.Down;
            ValueChanges[9] = right;
            if (ValueChanges._ValueChange[9] & ValueChanges._valuechange[9])
                Buttons ^= X360Buttons.Right;
            if (!right)
                Buttons &= ~X360Buttons.Right;
            ValueChanges[10] = leftstick;
            if (ValueChanges._ValueChange[10] & ValueChanges._valuechange[10])
                Buttons ^= X360Buttons.LeftStick;
            if (!leftstick)
                Buttons &= ~X360Buttons.LeftStick;
            ValueChanges[11] = rightstick;
            if (ValueChanges._ValueChange[11] & ValueChanges._valuechange[11])
                Buttons ^= X360Buttons.RightStick;
            if (!rightstick)
                Buttons &= ~X360Buttons.RightStick;
            ValueChanges[12] = leftbumper;
            if (ValueChanges._ValueChange[12] & ValueChanges._valuechange[12])
                Buttons ^= X360Buttons.LeftBumper;
            if (!leftbumper)
                Buttons &= ~X360Buttons.LeftBumper;
            ValueChanges[13] = rightbumper;
            if (ValueChanges._ValueChange[13] & ValueChanges._valuechange[13])
                Buttons ^= X360Buttons.RightBumper;
            if (!rightbumper)
                Buttons &= ~X360Buttons.RightBumper;
            ValueChanges[14] = xbox;
            if (ValueChanges._ValueChange[14] & ValueChanges._valuechange[14])
                Buttons ^= X360Buttons.Logo;
            if (!xbox)
                Buttons &= ~X360Buttons.Logo;
            LeftStickX = (short)leftstickx;
            LeftStickY = (short)leftsticky;
            RightStickX = (short)rightstickx;
            RightStickY = (short)rightsticky;
            LeftTrigger = (byte)lefttriggerposition;
            RightTrigger = (byte)righttriggerposition;
            Report(GetReport(inc));
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
                string str = "back : " + back + Environment.NewLine;
                str += "start : " + start + Environment.NewLine;
                str += "A : " + A + Environment.NewLine;
                str += "B : " + B + Environment.NewLine;
                str += "X : " + X + Environment.NewLine;
                str += "Y : " + Y + Environment.NewLine;
                str += "up : " + up + Environment.NewLine;
                str += "left : " + left + Environment.NewLine;
                str += "down : " + down + Environment.NewLine;
                str += "right : " + right + Environment.NewLine;
                str += "leftstick : " + leftstick + Environment.NewLine;
                str += "rightstick : " + rightstick + Environment.NewLine;
                str += "leftbumper : " + leftbumper + Environment.NewLine;
                str += "rightbumper : " + rightbumper + Environment.NewLine;
                str += "leftstickx : " + leftstickx + Environment.NewLine;
                str += "leftsticky : " + leftsticky + Environment.NewLine;
                str += "rightstickx : " + rightstickx + Environment.NewLine;
                str += "rightsticky : " + rightsticky + Environment.NewLine;
                str += "lefttriggerposition : " + lefttriggerposition + Environment.NewLine;
                str += "righttriggerposition : " + righttriggerposition + Environment.NewLine;
                str += "xbox : " + xbox + Environment.NewLine;
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
        private bool PlugIn(int controllerNumber)
        {
            int transfered = 0;
            byte[] buffer = new byte[16];
            buffer[0] = 0x10;
            buffer[1] = 0x00;
            buffer[2] = 0x00;
            buffer[3] = 0x00;
            buffer[4] = (byte)((controllerNumber) & 0xFF);
            buffer[5] = (byte)((controllerNumber >> 8) & 0xFF);
            buffer[6] = (byte)((controllerNumber >> 16) & 0xFF);
            buffer[7] = (byte)((controllerNumber >> 24) & 0xFF);
            return DeviceIoControl(_deviceHandle, 0x2A4000, buffer, buffer.Length, null, 0, ref transfered, IntPtr.Zero);
        }
        private bool Unplug(int controllerNumber)
        {
            int transfered = 0;
            byte[] buffer = new byte[16];
            buffer[0] = 0x10;
            buffer[1] = 0x00;
            buffer[2] = 0x00;
            buffer[3] = 0x00;
            buffer[4] = (byte)((controllerNumber) & 0xFF);
            buffer[5] = (byte)((controllerNumber >> 8) & 0xFF);
            buffer[6] = (byte)((controllerNumber >> 16) & 0xFF);
            buffer[7] = (byte)((controllerNumber >> 24) & 0xFF);
            return DeviceIoControl(_deviceHandle, 0x2A4004, buffer, buffer.Length, null, 0, ref transfered, IntPtr.Zero);
        }
        private bool Report(byte[] controllerReport)
        {
            return DeviceIoControl(_deviceHandle, 0x2A400C, controllerReport, controllerReport.Length, outputBuffer, outputBuffer?.Length ?? 0, ref transferred, IntPtr.Zero) && transferred > 0;
        }
        private bool Find(Guid target, ref string path, int instance = 0)
        {
            IntPtr detailDataBuffer = IntPtr.Zero;
            IntPtr deviceInfoSet = IntPtr.Zero;
            try
            {
                SP_DEVICE_INTERFACE_DATA DeviceInterfaceData = new SP_DEVICE_INTERFACE_DATA(), da = new SP_DEVICE_INTERFACE_DATA();
                int bufferSize = 0, memberIndex = 0;
                deviceInfoSet = SetupDiGetClassDevs(ref target, IntPtr.Zero, IntPtr.Zero, DIGCF_PRESENT | DIGCF_DEVICEINTERFACE);
                DeviceInterfaceData.cbSize = da.cbSize = Marshal.SizeOf(DeviceInterfaceData);
                while (SetupDiEnumDeviceInterfaces(deviceInfoSet, IntPtr.Zero, ref target, memberIndex, ref DeviceInterfaceData))
                {
                    SetupDiGetDeviceInterfaceDetail(deviceInfoSet, ref DeviceInterfaceData, IntPtr.Zero, 0, ref bufferSize, ref da);
                    detailDataBuffer = Marshal.AllocHGlobal(bufferSize);
                    Marshal.WriteInt32(detailDataBuffer, (IntPtr.Size == 4) ? (4 + Marshal.SystemDefaultCharSize) : 8);
                    if (SetupDiGetDeviceInterfaceDetail(deviceInfoSet, ref DeviceInterfaceData, detailDataBuffer, bufferSize, ref bufferSize, ref da))
                    {
                        IntPtr pDevicePathName = detailDataBuffer + 4;
                        path = Marshal.PtrToStringAuto(pDevicePathName).ToUpper(CultureInfo.InvariantCulture);
                        Marshal.FreeHGlobal(detailDataBuffer);
                        if (memberIndex == instance)
                            return true;
                    }
                    else
                        Marshal.FreeHGlobal(detailDataBuffer);
                    memberIndex++;
                }
            }
            finally
            {
                if (deviceInfoSet != IntPtr.Zero)
                {
                    SetupDiDestroyDeviceInfoList(deviceInfoSet);
                }
            }
            return false;
        }
        private SafeFileHandle GetHandle(string devicePath)
        {
            devicePath = devicePath.ToUpper(CultureInfo.InvariantCulture);
            SafeFileHandle handle = CreateFile(devicePath, GENERIC_WRITE | GENERIC_READ, FILE_SHARE_READ | FILE_SHARE_WRITE, IntPtr.Zero, OPEN_EXISTING, FILE_ATTRIBUTE_NORMAL | FILE_FLAG_OVERLAPPED, UIntPtr.Zero);
            return handle;
        }
        private X360Buttons Buttons { get; set; }
        private byte LeftTrigger { get; set; }
        private byte RightTrigger { get; set; }
        private short LeftStickX { get; set; }
        private short LeftStickY { get; set; }
        private short RightStickX { get; set; }
        private short RightStickY { get; set; }
        private byte[] fullReport = { 0x1C, 0x00, 0x00, 0x00, 0, 0, 0, 0, 0x00, 0x14, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
        private byte[] GetReport(int controllerNumber)
        {
            fullReport[4] = (byte)((controllerNumber) & 0xFF);
            fullReport[5] = (byte)((controllerNumber >> 8) & 0xFF);
            fullReport[6] = (byte)((controllerNumber >> 16) & 0xFF);
            fullReport[7] = (byte)((controllerNumber >> 24) & 0xFF);
            fullReport[10] = (byte)((ushort)Buttons & 0xFF);
            fullReport[11] = (byte)((ushort)Buttons >> 8 & 0xFF);
            fullReport[12] = LeftTrigger;
            fullReport[13] = RightTrigger;
            fullReport[14] = (byte)(LeftStickX & 0xFF);
            fullReport[15] = (byte)(LeftStickX >> 8 & 0xFF);
            fullReport[16] = (byte)(LeftStickY & 0xFF);
            fullReport[17] = (byte)(LeftStickY >> 8 & 0xFF);
            fullReport[18] = (byte)(RightStickX & 0xFF);
            fullReport[19] = (byte)(RightStickX >> 8 & 0xFF);
            fullReport[20] = (byte)(RightStickY & 0xFF);
            fullReport[21] = (byte)(RightStickY >> 8 & 0xFF);
            return fullReport;
        }
        [StructLayout(LayoutKind.Sequential)]
        internal struct SP_DEVICE_INTERFACE_DATA
        {
            internal int cbSize;
            internal Guid InterfaceClassGuid;
            internal int Flags;
            internal IntPtr Reserved;
        }
        internal const uint FILE_ATTRIBUTE_NORMAL = 0x80;
        internal const uint FILE_FLAG_OVERLAPPED = 0x40000000;
        internal const uint FILE_SHARE_READ = 1;
        internal const uint FILE_SHARE_WRITE = 2;
        internal const uint GENERIC_READ = 0x80000000;
        internal const uint GENERIC_WRITE = 0x40000000;
        internal const uint OPEN_EXISTING = 3;
        internal const int DIGCF_PRESENT = 0x0002;
        internal const int DIGCF_DEVICEINTERFACE = 0x0010;
        [DllImport("kernel32.dll", SetLastError = true, CharSet = CharSet.Unicode)]
        internal static extern SafeFileHandle CreateFile(string lpFileName, uint dwDesiredAccess, uint dwShareMode, IntPtr lpSecurityAttributes, uint dwCreationDisposition, uint dwFlagsAndAttributes, UIntPtr hTemplateFile);
        [DllImport("kernel32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        internal static extern bool DeviceIoControl(SafeFileHandle hDevice, int dwIoControlCode, byte[] lpInBuffer, int nInBufferSize, byte[] lpOutBuffer, int nOutBufferSize, ref int lpBytesReturned, IntPtr lpOverlapped);
        [DllImport("setupapi.dll", SetLastError = true)]
        internal static extern int SetupDiDestroyDeviceInfoList(IntPtr deviceInfoSet);
        [DllImport("setupapi.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        internal static extern bool SetupDiEnumDeviceInterfaces(IntPtr hDevInfo, IntPtr devInfo, ref Guid interfaceClassGuid, int memberIndex, ref SP_DEVICE_INTERFACE_DATA deviceInterfaceData);
        [DllImport("setupapi.dll", SetLastError = true, CharSet = CharSet.Auto)]
        internal static extern IntPtr SetupDiGetClassDevs(ref Guid classGuid, IntPtr enumerator, IntPtr hwndParent, int flags);
        [DllImport("setupapi.dll", SetLastError = true, CharSet = CharSet.Auto)]
        [return: MarshalAs(UnmanagedType.Bool)]
        internal static extern bool SetupDiGetDeviceInterfaceDetail(IntPtr hDevInfo, ref SP_DEVICE_INTERFACE_DATA deviceInterfaceData, IntPtr deviceInterfaceDetailData, int deviceInterfaceDetailDataSize, ref int requiredSize, ref SP_DEVICE_INTERFACE_DATA deviceInfoData);
        [Flags]
        private enum X360Buttons
        {
            None = 0,
            Up = 1 << 0,
            Down = 1 << 1,
            Left = 1 << 2,
            Right = 1 << 3,
            Start = 1 << 4,
            Back = 1 << 5,
            LeftStick = 1 << 6,
            RightStick = 1 << 7,
            LeftBumper = 1 << 8,
            RightBumper = 1 << 9,
            Logo = 1 << 10,
            A = 1 << 12,
            B = 1 << 13,
            X = 1 << 14,
            Y = 1 << 15,
        }
    }
}