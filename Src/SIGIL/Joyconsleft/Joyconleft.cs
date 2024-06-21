using Microsoft.Win32.SafeHandles;
using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using Vector3 = System.Numerics.Vector3;
using Joyconsleft;
using System.Collections.Generic;
using System.Diagnostics;
using Valuechanges;

namespace JoyconsLeftAPI
{
    public class JoyconLeft : IDisposable
    {
        [DllImport("MotionInputPairing.dll", EntryPoint = "joyconleftconnect")]
        private static extern bool joyconleftconnect();
        [DllImport("MotionInputPairing.dll", EntryPoint = "joyconleftdisconnect")]
        private static extern bool joyconleftdisconnect();
        [DllImport("MotionInputPairing.dll", EntryPoint = "joyconsleftconnect")]
        private static extern bool joyconsleftconnect();
        [DllImport("MotionInputPairing.dll", EntryPoint = "joyconsleftdisconnect")]
        private static extern bool joyconsleftdisconnect();
        [DllImport("lhidread.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "Lhid_read_timeout")]
        private static extern int Lhid_read_timeout(SafeFileHandle dev, byte[] data, UIntPtr length);
        [DllImport("lhidread.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "Lhid_write")]
        private static extern int Lhid_write(SafeFileHandle device, byte[] data, UIntPtr length);
        [DllImport("lhidread.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "Lhid_open_path")]
        private static extern SafeFileHandle Lhid_open_path(IntPtr handle);
        [DllImport("lhidread.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "Lhid_close")]
        private static extern void Lhid_close(SafeFileHandle device);
        [DllImport("hid.dll")]
        private static extern void HidD_GetHidGuid(out Guid gHid);
        [DllImport("hid.dll")]
        private extern static bool HidD_SetOutputReport(IntPtr HidDeviceObject, byte[] lpReportBuffer, uint ReportBufferLength);
        [DllImport("setupapi.dll")]
        private static extern IntPtr SetupDiGetClassDevs(ref Guid ClassGuid, string Enumerator, IntPtr hwndParent, UInt32 Flags);
        [DllImport("setupapi.dll")]
        private static extern Boolean SetupDiEnumDeviceInterfaces(IntPtr hDevInfo, IntPtr devInvo, ref Guid interfaceClassGuid, Int32 memberIndex, ref SP_DEVICE_INTERFACE_DATA deviceInterfaceData);
        [DllImport("setupapi.dll")]
        private static extern Boolean SetupDiGetDeviceInterfaceDetail(IntPtr hDevInfo, ref SP_DEVICE_INTERFACE_DATA deviceInterfaceData, IntPtr deviceInterfaceDetailData, UInt32 deviceInterfaceDetailDataSize, out UInt32 requiredSize, IntPtr deviceInfoData);
        [DllImport("setupapi.dll")]
        private static extern Boolean SetupDiGetDeviceInterfaceDetail(IntPtr hDevInfo, ref SP_DEVICE_INTERFACE_DATA deviceInterfaceData, ref SP_DEVICE_INTERFACE_DETAIL_DATA deviceInterfaceDetailData, UInt32 deviceInterfaceDetailDataSize, out UInt32 requiredSize, IntPtr deviceInfoData);
        [DllImport("Kernel32.dll")]
        private static extern SafeFileHandle CreateFile(string fileName, [MarshalAs(UnmanagedType.U4)] FileAccess fileAccess, [MarshalAs(UnmanagedType.U4)] FileShare fileShare, IntPtr securityAttributes, [MarshalAs(UnmanagedType.U4)] FileMode creationDisposition, [MarshalAs(UnmanagedType.U4)] uint flags, IntPtr template);
        [DllImport("Kernel32.dll")]
        private static extern IntPtr CreateFile(string fileName, System.IO.FileAccess fileAccess, System.IO.FileShare fileShare, IntPtr securityAttributes, System.IO.FileMode creationDisposition, EFileAttributes flags, IntPtr template);
        [DllImport("winmm.dll", EntryPoint = "timeBeginPeriod")]
        private static extern uint TimeBeginPeriod(uint ms);
        [DllImport("winmm.dll", EntryPoint = "timeEndPeriod")]
        private static extern uint TimeEndPeriod(uint ms);
        [DllImport("ntdll.dll", EntryPoint = "NtSetTimerResolution")]
        private static extern void NtSetTimerResolution(uint DesiredResolution, bool SetResolution, ref uint CurrentResolution);
        private static uint CurrentResolution = 0;
        private const uint report_lenLeft = 49;
        private byte[] report_bufLeft = new byte[report_lenLeft];
        private SafeFileHandle handleLeft;
        private IntPtr handleptrLeft, handleptrunsharedLeft;
        public bool JoyconLeftButtonSMA, JoyconLeftButtonACC, JoyconLeftRollLeft, JoyconLeftRollRight;
        public double JoyconLeftStickX, JoyconLeftStickY;
        private System.Collections.Generic.List<double> LeftValListX = new System.Collections.Generic.List<double>(), LeftValListY = new System.Collections.Generic.List<double>();
        public bool JoyconLeftAccelCenter, JoyconLeftStickCenter;
        public double JoyconLeftAccelX, JoyconLeftAccelY, JoyconLeftGyroX, JoyconLeftGyroY;
        private double[] stickLeft = { 0, 0 };
        private double[] stickCenterLeft = { 0, 0 };
        private byte[] stick_rawLeft = { 0, 0, 0 };
        private Vector3 acc_gLeft = new Vector3();
        private Vector3 gyr_gLeft = new Vector3();
        private Vector3 InitDirectAnglesLeft, DirectAnglesLeft;
        public bool JoyconLeftButtonSHOULDER_1, JoyconLeftButtonSHOULDER_2, JoyconLeftButtonSR, JoyconLeftButtonSL, JoyconLeftButtonDPAD_DOWN, JoyconLeftButtonDPAD_RIGHT, JoyconLeftButtonDPAD_UP, JoyconLeftButtonDPAD_LEFT, JoyconLeftButtonMINUS, JoyconLeftButtonSTICK, JoyconLeftButtonCAPTURE;
        private float acc_gcalibrationLeftX, acc_gcalibrationLeftY, acc_gcalibrationLeftZ;
        private bool running, formvisible;
        private bool isvalidhandle = false;
        private int number;
        private bool reconnectingbool;
        private double reconnectingcount;
        private string path;
        private static List<string> paths = new List<string>();
        private static List<SafeFileHandle> handles = new List<SafeFileHandle>();
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
        public JoyconLeft()
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
            running = false;
            Thread.Sleep(100);
            Lhid_close(handleLeft);
            handleLeft.Close();
            handleLeft.Dispose();
        }
        private void taskDLeft()
        {
            for (; ; )
            {
                if (!running)
                    break;
                try
                {
                    Lhid_read_timeout(handleLeft, report_bufLeft, (UIntPtr)report_lenLeft);
                    reconnectingbool = false;
                }
                catch { Thread.Sleep(10); }
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
                    string str = "JoyconLeftStickX : " + JoyconLeftStickX + Environment.NewLine;
                    str += "JoyconLeftStickY : " + JoyconLeftStickY + Environment.NewLine;
                    str += "JoyconLeftButtonSHOULDER_1 : " + JoyconLeftButtonSHOULDER_1 + Environment.NewLine;
                    str += "JoyconLeftButtonSHOULDER_2 : " + JoyconLeftButtonSHOULDER_2 + Environment.NewLine;
                    str += "JoyconLeftButtonSR : " + JoyconLeftButtonSR + Environment.NewLine;
                    str += "JoyconLeftButtonSL : " + JoyconLeftButtonSL + Environment.NewLine;
                    str += "JoyconLeftButtonDPAD_DOWN : " + JoyconLeftButtonDPAD_DOWN + Environment.NewLine;
                    str += "JoyconLeftButtonDPAD_RIGHT : " + JoyconLeftButtonDPAD_RIGHT + Environment.NewLine;
                    str += "JoyconLeftButtonDPAD_UP : " + JoyconLeftButtonDPAD_UP + Environment.NewLine;
                    str += "JoyconLeftButtonDPAD_LEFT : " + JoyconLeftButtonDPAD_LEFT + Environment.NewLine;
                    str += "JoyconLeftButtonMINUS : " + JoyconLeftButtonMINUS + Environment.NewLine;
                    str += "JoyconLeftButtonCAPTURE : " + JoyconLeftButtonCAPTURE + Environment.NewLine;
                    str += "JoyconLeftButtonSTICK : " + JoyconLeftButtonSTICK + Environment.NewLine;
                    str += "JoyconLeftButtonACC : " + JoyconLeftButtonACC + Environment.NewLine;
                    str += "JoyconLeftButtonSMA : " + JoyconLeftButtonSMA + Environment.NewLine;
                    str += "JoyconLeftRollLeft : " + JoyconLeftRollLeft + Environment.NewLine;
                    str += "JoyconLeftRollRight : " + JoyconLeftRollRight + Environment.NewLine;
                    str += "JoyconLeftAccelX : " + JoyconLeftAccelX + Environment.NewLine;
                    str += "JoyconLeftAccelY : " + JoyconLeftAccelY + Environment.NewLine;
                    str += "JoyconLeftGyroX : " + JoyconLeftGyroX + Environment.NewLine;
                    str += "JoyconLeftGyroY : " + JoyconLeftGyroY + Environment.NewLine;
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
        private void taskPLeft()
        {
            for (; ; )
            {
                if (!running)
                    break;
                Reconnection();
                ProcessStateLogic();
                Thread.Sleep(1);
            }
        }
        public void BeginPolling()
        {
            Task.Run(() => taskDLeft());
            Task.Run(() => taskPLeft());
        }
        public void Init()
        {
            try
            {
                stick_rawLeft[0] = report_bufLeft[6 + 0];
                stick_rawLeft[1] = report_bufLeft[7 + 0];
                stick_rawLeft[2] = report_bufLeft[8 + 0];
                stickCenterLeft[0] = (UInt16)(stick_rawLeft[0] | ((stick_rawLeft[1] & 0xf) << 8));
                stickCenterLeft[1] = (UInt16)((stick_rawLeft[1] >> 4) | (stick_rawLeft[2] << 4));
                acc_gcalibrationLeftX = (Int16)(report_bufLeft[13 + 0 * 12] | ((report_bufLeft[14 + 0 * 12] << 8) & 0xff00)) + (Int16)(report_bufLeft[13 + 1 * 12] | ((report_bufLeft[14 + 1 * 12] << 8) & 0xff00)) + (Int16)(report_bufLeft[13 + 2 * 12] | ((report_bufLeft[14 + 2 * 12] << 8) & 0xff00));
                acc_gcalibrationLeftY = (Int16)(report_bufLeft[15 + 0 * 12] | ((report_bufLeft[16 + 0 * 12] << 8) & 0xff00)) + (Int16)(report_bufLeft[15 + 1 * 12] | ((report_bufLeft[16 + 1 * 12] << 8) & 0xff00)) + (Int16)(report_bufLeft[15 + 2 * 12] | ((report_bufLeft[16 + 2 * 12] << 8) & 0xff00));
                acc_gcalibrationLeftZ = (Int16)(report_bufLeft[17 + 0 * 12] | ((report_bufLeft[18 + 0 * 12] << 8) & 0xff00)) + (Int16)(report_bufLeft[17 + 1 * 12] | ((report_bufLeft[18 + 1 * 12] << 8) & 0xff00)) + (Int16)(report_bufLeft[17 + 2 * 12] | ((report_bufLeft[18 + 2 * 12] << 8) & 0xff00));
                InitDirectAnglesLeft = acc_gLeft;
            }
            catch { }
        }
        private void ProcessStateLogic()
        {
            try
            {
                stick_rawLeft[0] = report_bufLeft[6 + 0];
                stick_rawLeft[1] = report_bufLeft[7 + 0];
                stick_rawLeft[2] = report_bufLeft[8 + 0];
                stickLeft[0] = ((UInt16)(stick_rawLeft[0] | ((stick_rawLeft[1] & 0xf) << 8)) - stickCenterLeft[0]) / 1440f;
                stickLeft[1] = ((UInt16)((stick_rawLeft[1] >> 4) | (stick_rawLeft[2] << 4)) - stickCenterLeft[1]) / 1440f;
                JoyconLeftStickX = stickLeft[0];
                JoyconLeftStickY = stickLeft[1];
                acc_gLeft.X = ((Int16)(report_bufLeft[13 + 0 * 12] | ((report_bufLeft[14 + 0 * 12] << 8) & 0xff00)) + (Int16)(report_bufLeft[13 + 1 * 12] | ((report_bufLeft[14 + 1 * 12] << 8) & 0xff00)) + (Int16)(report_bufLeft[13 + 2 * 12] | ((report_bufLeft[14 + 2 * 12] << 8) & 0xff00)) - acc_gcalibrationLeftX) * (1.0f / 12000f);
                acc_gLeft.Y = -((Int16)(report_bufLeft[15 + 0 * 12] | ((report_bufLeft[16 + 0 * 12] << 8) & 0xff00)) + (Int16)(report_bufLeft[15 + 1 * 12] | ((report_bufLeft[16 + 1 * 12] << 8) & 0xff00)) + (Int16)(report_bufLeft[15 + 2 * 12] | ((report_bufLeft[16 + 2 * 12] << 8) & 0xff00)) - acc_gcalibrationLeftY) * (1.0f / 12000f);
                acc_gLeft.Z = -((Int16)(report_bufLeft[17 + 0 * 12] | ((report_bufLeft[18 + 0 * 12] << 8) & 0xff00)) + (Int16)(report_bufLeft[17 + 1 * 12] | ((report_bufLeft[18 + 1 * 12] << 8) & 0xff00)) + (Int16)(report_bufLeft[17 + 2 * 12] | ((report_bufLeft[18 + 2 * 12] << 8) & 0xff00)) - acc_gcalibrationLeftZ) * (1.0f / 12000f);
                JoyconLeftButtonSHOULDER_1 = (report_bufLeft[3 + 2] & 0x40) != 0;
                JoyconLeftButtonSHOULDER_2 = (report_bufLeft[3 + 2] & 0x80) != 0;
                JoyconLeftButtonSR = (report_bufLeft[3 + 2] & 0x10) != 0;
                JoyconLeftButtonSL = (report_bufLeft[3 + 2] & 0x20) != 0;
                JoyconLeftButtonDPAD_DOWN = (report_bufLeft[3 + 2] & (0x01)) != 0;
                JoyconLeftButtonDPAD_RIGHT = (report_bufLeft[3 + 2] & (0x04)) != 0;
                JoyconLeftButtonDPAD_UP = (report_bufLeft[3 + 2] & (0x02)) != 0;
                JoyconLeftButtonDPAD_LEFT = (report_bufLeft[3 + 2] & (0x08)) != 0;
                JoyconLeftButtonMINUS = (report_bufLeft[4] & 0x01) != 0;
                JoyconLeftButtonCAPTURE = (report_bufLeft[4] & 0x20) != 0;
                JoyconLeftButtonSTICK = (report_bufLeft[4] & (0x08)) != 0;
                JoyconLeftButtonACC = acc_gLeft.X <= -1.13;
                JoyconLeftButtonSMA = JoyconLeftButtonSL | JoyconLeftButtonSR | JoyconLeftButtonMINUS | JoyconLeftButtonACC;
                DirectAnglesLeft = acc_gLeft - InitDirectAnglesLeft;
                JoyconLeftAccelX = DirectAnglesLeft.X * 1350f;
                JoyconLeftAccelY = -DirectAnglesLeft.Y * 1350f;
                gyr_gLeft.X = (Int16)(report_bufLeft[19 + 0 * 12] | ((report_bufLeft[20 + 0 * 12] << 8) & 0xff00)) + (Int16)(report_bufLeft[19 + 1 * 12] | ((report_bufLeft[20 + 1 * 12] << 8) & 0xff00)) + (Int16)(report_bufLeft[19 + 2 * 12] | ((report_bufLeft[20 + 2 * 12] << 8) & 0xff00));
                gyr_gLeft.Y = (Int16)(report_bufLeft[21 + 0 * 12] | ((report_bufLeft[22 + 0 * 12] << 8) & 0xff00)) + (Int16)(report_bufLeft[21 + 1 * 12] | ((report_bufLeft[22 + 1 * 12] << 8) & 0xff00)) + (Int16)(report_bufLeft[21 + 2 * 12] | ((report_bufLeft[22 + 2 * 12] << 8) & 0xff00));
                gyr_gLeft.Z = (Int16)(report_bufLeft[23 + 0 * 12] | ((report_bufLeft[24 + 0 * 12] << 8) & 0xff00)) + (Int16)(report_bufLeft[23 + 1 * 12] | ((report_bufLeft[24 + 1 * 12] << 8) & 0xff00)) + (Int16)(report_bufLeft[23 + 2 * 12] | ((report_bufLeft[24 + 2 * 12] << 8) & 0xff00));
                JoyconLeftGyroX = gyr_gLeft.Z;
                JoyconLeftGyroY = gyr_gLeft.Y;
            }
            catch { }
        }
        private void Reconnection()
        {
            if (reconnectingcount == 0)
                reconnectingbool = true;
            reconnectingcount++;
            if (reconnectingcount >= 150f)
            {
                if (reconnectingbool)
                {
                    ReconnectionInit();
                    AttachJoyLeft(path);
                    reconnectingcount = -150f;
                }
                else
                    reconnectingcount = 0;
            }
        }
        private void ReconnectionInit()
        {
            JoyconLeftStickX = 0;
            JoyconLeftStickY = 0;
            JoyconLeftButtonSHOULDER_1 = false;
            JoyconLeftButtonSHOULDER_2 = false;
            JoyconLeftButtonSR = false;
            JoyconLeftButtonSL = false;
            JoyconLeftButtonDPAD_DOWN = false;
            JoyconLeftButtonDPAD_RIGHT = false;
            JoyconLeftButtonDPAD_UP = false;
            JoyconLeftButtonDPAD_LEFT = false;
            JoyconLeftButtonMINUS = false;
            JoyconLeftButtonCAPTURE = false;
            JoyconLeftButtonSTICK = false;
            JoyconLeftButtonACC = false;
            JoyconLeftButtonSMA = false;
            JoyconLeftRollLeft = false;
            JoyconLeftRollRight = false;
            JoyconLeftAccelX = 0;
            JoyconLeftAccelY = 0;
            JoyconLeftGyroX = 0;
            JoyconLeftGyroY = 0;
        }
        private const string vendor_id = "57e", vendor_id_ = "057e", product_l = "2006";
        private enum EFileAttributes : uint
        {
            Overlapped = 0x40000000,
            Normal = 0x80
        };
        private struct SP_DEVICE_INTERFACE_DATA
        {
            public int cbSize;
            public Guid InterfaceClassGuid;
            public int Flags;
            public IntPtr RESERVED;
        }
        private struct SP_DEVICE_INTERFACE_DETAIL_DATA
        {
            public UInt32 cbSize;
            [System.Runtime.InteropServices.MarshalAs(System.Runtime.InteropServices.UnmanagedType.ByValTStr, SizeConst = 256)]
            public string DevicePath;
        }
        public void Scan(int number = 0)
        {
            this.number = number;
            if (number == 0)
                do
                    Thread.Sleep(1);
                while (!joyconleftconnect());
            else if (number == 1)
                do
                    Thread.Sleep(1);
                while (!joyconsleftconnect());
            if (number <= 1)
            {
                int index = 0;
                System.Guid guid;
                HidD_GetHidGuid(out guid);
                System.IntPtr hDevInfo = SetupDiGetClassDevs(ref guid, null, new System.IntPtr(), 0x00000010);
                SP_DEVICE_INTERFACE_DATA diData = new SP_DEVICE_INTERFACE_DATA();
                diData.cbSize = System.Runtime.InteropServices.Marshal.SizeOf(diData);
                while (SetupDiEnumDeviceInterfaces(hDevInfo, new System.IntPtr(), ref guid, index, ref diData))
                {
                    System.UInt32 size;
                    SetupDiGetDeviceInterfaceDetail(hDevInfo, ref diData, new System.IntPtr(), 0, out size, new System.IntPtr());
                    SP_DEVICE_INTERFACE_DETAIL_DATA diDetail = new SP_DEVICE_INTERFACE_DETAIL_DATA();
                    diDetail.cbSize = 5;
                    if (SetupDiGetDeviceInterfaceDetail(hDevInfo, ref diData, ref diDetail, size, out size, new System.IntPtr()))
                    {
                        if ((diDetail.DevicePath.Contains(vendor_id) | diDetail.DevicePath.Contains(vendor_id_)) & diDetail.DevicePath.Contains(product_l))
                        {
                            path = diDetail.DevicePath;
                            isvalidhandle = AttachJoyLeft(diDetail.DevicePath);
                            handleptrunsharedLeft = CreateFile(path, System.IO.FileAccess.ReadWrite, System.IO.FileShare.None, new System.IntPtr(), System.IO.FileMode.Open, EFileAttributes.Normal, new System.IntPtr());
                            if (isvalidhandle)
                            {
                                paths.Add(path);
                                handles.Add(handleLeft);
                            }
                        }
                    }
                    index++;
                }
            }
            path = paths[number < 2 ? 0 : number - 1];
            handleLeft = handles[number < 2 ? 0 : number - 1];
        }
        private bool AttachJoyLeft(string path)
        {
            try
            {
                handleptrLeft = CreateFile(path, System.IO.FileAccess.ReadWrite, System.IO.FileShare.ReadWrite, new System.IntPtr(), System.IO.FileMode.Open, EFileAttributes.Normal, new System.IntPtr());
                handleLeft = Lhid_open_path(handleptrLeft);
                SubcommandLeft(0x40, new byte[] { 0x1 }, 1);
                SubcommandLeft(0x3, new byte[] { 0x30 }, 1);
                return true;
            }
            catch { return false; }
        }
        private void SubcommandLeft(byte sc, byte[] buf, uint len)
        {
            byte[] buf_Left = new byte[report_lenLeft];
            System.Array.Copy(buf, 0, buf_Left, 11, len);
            buf_Left[10] = sc;
            buf_Left[1] = 0;
            buf_Left[0] = 0x1;
            Lhid_write(handleLeft, buf_Left, (UIntPtr)(len + 11));
            Lhid_read_timeout(handleLeft, buf_Left, (UIntPtr)report_lenLeft);
        }
        public void Dispose()
        {
            GC.Collect();
            GC.SuppressFinalize(this);
        }
    }
}