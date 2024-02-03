using Microsoft.Win32.SafeHandles;
using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using Vector3 = System.Numerics.Vector3;
using Joyconcharginggrips;
using System.Collections.Generic;

namespace JoyconChargingGripsAPI
{
    public class JoyconChargingGrip
    {
        [DllImport("hid.dll")]
        public static extern void HidD_GetHidGuid(out Guid gHid);
        [DllImport("hid.dll")]
        public extern static bool HidD_SetOutputReport(IntPtr HidDeviceObject, byte[] lpReportBuffer, uint ReportBufferLength);
        [DllImport("setupapi.dll")]
        public static extern IntPtr SetupDiGetClassDevs(ref Guid ClassGuid, string Enumerator, IntPtr hwndParent, UInt32 Flags);
        [DllImport("setupapi.dll")]
        public static extern Boolean SetupDiEnumDeviceInterfaces(IntPtr hDevInfo, IntPtr devInvo, ref Guid interfaceClassGuid, Int32 memberIndex, ref SP_DEVICE_INTERFACE_DATA deviceInterfaceData);
        [DllImport("setupapi.dll")]
        public static extern Boolean SetupDiGetDeviceInterfaceDetail(IntPtr hDevInfo, ref SP_DEVICE_INTERFACE_DATA deviceInterfaceData, IntPtr deviceInterfaceDetailData, UInt32 deviceInterfaceDetailDataSize, out UInt32 requiredSize, IntPtr deviceInfoData);
        [DllImport("setupapi.dll")]
        public static extern Boolean SetupDiGetDeviceInterfaceDetail(IntPtr hDevInfo, ref SP_DEVICE_INTERFACE_DATA deviceInterfaceData, ref SP_DEVICE_INTERFACE_DETAIL_DATA deviceInterfaceDetailData, UInt32 deviceInterfaceDetailDataSize, out UInt32 requiredSize, IntPtr deviceInfoData);
        [DllImport("Kernel32.dll")]
        public static extern SafeFileHandle CreateFile(string fileName, [MarshalAs(UnmanagedType.U4)] FileAccess fileAccess, [MarshalAs(UnmanagedType.U4)] FileShare fileShare, IntPtr securityAttributes, [MarshalAs(UnmanagedType.U4)] FileMode creationDisposition, [MarshalAs(UnmanagedType.U4)] uint flags, IntPtr template);
        [DllImport("Kernel32.dll")]
        public static extern IntPtr CreateFile(string fileName, System.IO.FileAccess fileAccess, System.IO.FileShare fileShare, IntPtr securityAttributes, System.IO.FileMode creationDisposition, EFileAttributes flags, IntPtr template);
        [DllImport("lhidread.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "Lhid_read_timeout")]
        public static extern int Lhid_read_timeout(SafeFileHandle dev, byte[] data, UIntPtr length);
        [DllImport("lhidread.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "Lhid_write")]
        public static extern int Lhid_write(SafeFileHandle device, byte[] data, UIntPtr length);
        [DllImport("lhidread.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "Lhid_open_path")]
        public static extern SafeFileHandle Lhid_open_path(IntPtr handle);
        [DllImport("lhidread.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "Lhid_close")]
        public static extern void Lhid_close(SafeFileHandle device);
        [DllImport("rhidread.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "Rhid_read_timeout")]
        public static extern int Rhid_read_timeout(SafeFileHandle dev, byte[] data, UIntPtr length);
        [DllImport("rhidread.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "Rhid_write")]
        public static extern int Rhid_write(SafeFileHandle device, byte[] data, UIntPtr length);
        [DllImport("rhidread.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "Rhid_open_path")]
        public static extern SafeFileHandle Rhid_open_path(IntPtr handle);
        [DllImport("rhidread.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "Rhid_close")]
        public static extern void Rhid_close(SafeFileHandle device);
        [DllImport("winmm.dll", EntryPoint = "timeBeginPeriod")]
        private static extern uint TimeBeginPeriod(uint ms);
        [DllImport("winmm.dll", EntryPoint = "timeEndPeriod")]
        private static extern uint TimeEndPeriod(uint ms);
        [DllImport("ntdll.dll", EntryPoint = "NtSetTimerResolution")]
        private static extern void NtSetTimerResolution(uint DesiredResolution, bool SetResolution, ref uint CurrentResolution);
        private static uint CurrentResolution = 0;
        public const uint report_lenLeft = 49;
        public byte[] report_bufLeft = new byte[report_lenLeft];
        public const uint report_lenRight = 49;
        public byte[] report_bufRight = new byte[report_lenRight];
        public SafeFileHandle handleRight;
        public SafeFileHandle handleLeft;
        public bool JoyconLeftButtonSMA, JoyconLeftButtonACC, JoyconLeftRollLeft, JoyconLeftRollRight;
        public double JoyconLeftStickX, JoyconLeftStickY;
        public System.Collections.Generic.List<double> LeftValListX = new System.Collections.Generic.List<double>(), LeftValListY = new System.Collections.Generic.List<double>();
        public bool JoyconLeftAccelCenter, JoyconLeftStickCenter;
        public double JoyconLeftAccelX, JoyconLeftAccelY, JoyconLeftGyroX, JoyconLeftGyroY;
        private double[] stickLeft = { 0, 0 };
        private double[] stickCenterLeft = { 0, 0 };
        private byte[] stick_rawLeft = { 0, 0, 0 };
        public Vector3 acc_gLeft = new Vector3();
        public Vector3 gyr_gLeft = new Vector3();
        public Vector3 InitDirectAnglesLeft, DirectAnglesLeft;
        public bool JoyconLeftButtonSHOULDER_1, JoyconLeftButtonSHOULDER_2, JoyconLeftButtonSR, JoyconLeftButtonSL, JoyconLeftButtonDPAD_DOWN, JoyconLeftButtonDPAD_RIGHT, JoyconLeftButtonDPAD_UP, JoyconLeftButtonDPAD_LEFT, JoyconLeftButtonMINUS, JoyconLeftButtonSTICK, JoyconLeftButtonCAPTURE;
        public float acc_gcalibrationLeftX, acc_gcalibrationLeftY, acc_gcalibrationLeftZ;
        public bool JoyconRightButtonSPA, JoyconRightButtonACC, JoyconRightRollLeft, JoyconRightRollRight;
        public double JoyconRightStickX, JoyconRightStickY;
        public System.Collections.Generic.List<double> RightValListX = new System.Collections.Generic.List<double>(), RightValListY = new System.Collections.Generic.List<double>();
        public bool JoyconRightAccelCenter, JoyconRightStickCenter;
        public double JoyconRightAccelX, JoyconRightAccelY, JoyconRightGyroX, JoyconRightGyroY;
        private double[] stickRight = { 0, 0 };
        private double[] stickCenterRight = { 0, 0 };
        private byte[] stick_rawRight = { 0, 0, 0 };
        public Vector3 acc_gRight = new Vector3();
        public Vector3 gyr_gRight = new Vector3();
        public Vector3 InitDirectAnglesRight, DirectAnglesRight;
        public bool JoyconRightButtonSHOULDER_1, JoyconRightButtonSHOULDER_2, JoyconRightButtonSR, JoyconRightButtonSL, JoyconRightButtonDPAD_DOWN, JoyconRightButtonDPAD_RIGHT, JoyconRightButtonDPAD_UP, JoyconRightButtonDPAD_LEFT, JoyconRightButtonPLUS, JoyconRightButtonSTICK, JoyconRightButtonHOME;
        public float acc_gcalibrationRightX, acc_gcalibrationRightY, acc_gcalibrationRightZ;
        public bool running, formvisible;
        private bool isvalidhandle = false;
        private int number;
        public bool reconnectingboolleft, reconnectingboolright;
        public double reconnectingcountleft, reconnectingcountright;
        public string pathleft, pathright;
        private static List<string> pathsleft = new List<string>(), pathsright = new List<string>();
        private static List<SafeFileHandle> handlesLeft = new List<SafeFileHandle>(), handlesRight = new List<SafeFileHandle>();
        public Form1 form1 = new Form1();
        public JoyconChargingGrip()
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
            Thread.Sleep(100);
            Subcommand3GripLeftController(0x06, new byte[] { 0x01 }, 1);
            Subcommand3GripRightController(0x06, new byte[] { 0x01 }, 1);
            Lhid_close(handleLeft);
            handleLeft.Close();
            handleLeft.Dispose();
            Rhid_close(handleRight);
            handleRight.Close();
            handleRight.Dispose();
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
                    reconnectingboolleft = false;
                }
                catch { Thread.Sleep(10); }
                if (formvisible)
                {
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
                    str += Environment.NewLine;
                    form1.SetLabel1(str);
                }
            }
        }
        private void taskDRight()
        {
            for (; ; )
            {
                if (!running)
                    break;
                try
                {
                    Rhid_read_timeout(handleRight, report_bufRight, (UIntPtr)report_lenRight);
                    reconnectingboolright = false;
                }
                catch { Thread.Sleep(10); }
                if (formvisible)
                {
                    string str = "JoyconRightStickX : " + JoyconRightStickX + Environment.NewLine;
                    str += "JoyconRightStickY : " + JoyconRightStickY + Environment.NewLine;
                    str += "JoyconRightButtonSHOULDER_1 : " + JoyconRightButtonSHOULDER_1 + Environment.NewLine;
                    str += "JoyconRightButtonSHOULDER_2 : " + JoyconRightButtonSHOULDER_2 + Environment.NewLine;
                    str += "JoyconRightButtonSR : " + JoyconRightButtonSR + Environment.NewLine;
                    str += "JoyconRightButtonSL : " + JoyconRightButtonSL + Environment.NewLine;
                    str += "JoyconRightButtonDPAD_DOWN : " + JoyconRightButtonDPAD_DOWN + Environment.NewLine;
                    str += "JoyconRightButtonDPAD_RIGHT : " + JoyconRightButtonDPAD_RIGHT + Environment.NewLine;
                    str += "JoyconRightButtonDPAD_UP : " + JoyconRightButtonDPAD_UP + Environment.NewLine;
                    str += "JoyconRightButtonDPAD_LEFT : " + JoyconRightButtonDPAD_LEFT + Environment.NewLine;
                    str += "JoyconRightButtonPLUS : " + JoyconRightButtonPLUS + Environment.NewLine;
                    str += "JoyconRightButtonHOME : " + JoyconRightButtonHOME + Environment.NewLine;
                    str += "JoyconRightButtonSTICK : " + JoyconRightButtonSTICK + Environment.NewLine;
                    str += "JoyconRightButtonACC : " + JoyconRightButtonACC + Environment.NewLine;
                    str += "JoyconRightButtonSPA : " + JoyconRightButtonSPA + Environment.NewLine;
                    str += "JoyconRightRollLeft : " + JoyconRightRollLeft + Environment.NewLine;
                    str += "JoyconRightRollRight : " + JoyconRightRollRight + Environment.NewLine;
                    str += "JoyconRightAccelX : " + JoyconRightAccelX + Environment.NewLine;
                    str += "JoyconRightAccelY : " + JoyconRightAccelY + Environment.NewLine;
                    str += "JoyconRightGyroX : " + JoyconRightGyroX + Environment.NewLine;
                    str += "JoyconRightGyroY : " + JoyconRightGyroY + Environment.NewLine;
                    form1.SetLabel2(str);
                }
            }
        }
        private void taskPLeft()
        {
            for (; ; )
            {
                if (!running)
                    break;
                ReconnectionLeft();
                ProcessButtonsLeftJoycon();
                Thread.Sleep(1);
            }
        }
        private void taskPRight()
        {
            for (; ; )
            {
                if (!running)
                    break;
                ReconnectionRight();
                ProcessButtonsRightJoycon();
                Thread.Sleep(1);
            }
        }
        public void BeginPolling()
        {
            Task.Run(() => taskDLeft());
            Task.Run(() => taskDRight());
            Task.Run(() => taskPLeft());
            Task.Run(() => taskPRight());
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
                stick_rawRight[0] = report_bufRight[6 + 3];
                stick_rawRight[1] = report_bufRight[7 + 3];
                stick_rawRight[2] = report_bufRight[8 + 3];
                stickCenterRight[0] = (UInt16)(stick_rawRight[0] | ((stick_rawRight[1] & 0xf) << 8));
                stickCenterRight[1] = (UInt16)((stick_rawRight[1] >> 4) | (stick_rawRight[2] << 4));
                acc_gcalibrationRightX = (Int16)(report_bufRight[13 + 0 * 12] | ((report_bufRight[14 + 0 * 12] << 8) & 0xff00)) + (Int16)(report_bufRight[13 + 1 * 12] | ((report_bufRight[14 + 1 * 12] << 8) & 0xff00)) + (Int16)(report_bufRight[13 + 2 * 12] | ((report_bufRight[14 + 2 * 12] << 8) & 0xff00));
                acc_gcalibrationRightY = (Int16)(report_bufRight[15 + 0 * 12] | ((report_bufRight[16 + 0 * 12] << 8) & 0xff00)) + (Int16)(report_bufRight[15 + 1 * 12] | ((report_bufRight[16 + 1 * 12] << 8) & 0xff00)) + (Int16)(report_bufRight[15 + 2 * 12] | ((report_bufRight[16 + 2 * 12] << 8) & 0xff00));
                acc_gcalibrationRightZ = (Int16)(report_bufRight[17 + 0 * 12] | ((report_bufRight[18 + 0 * 12] << 8) & 0xff00)) + (Int16)(report_bufRight[17 + 1 * 12] | ((report_bufRight[18 + 1 * 12] << 8) & 0xff00)) + (Int16)(report_bufRight[17 + 2 * 12] | ((report_bufRight[18 + 2 * 12] << 8) & 0xff00));
                InitDirectAnglesLeft = acc_gLeft;
                InitDirectAnglesRight = acc_gRight;
            }
            catch { }
        }
        public void ProcessButtonsLeftJoycon()
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
        public void ProcessButtonsRightJoycon()
        {
            try
            {
                stick_rawRight[0] = report_bufRight[6 + 3];
                stick_rawRight[1] = report_bufRight[7 + 3];
                stick_rawRight[2] = report_bufRight[8 + 3];
                stickRight[0] = ((UInt16)(stick_rawRight[0] | ((stick_rawRight[1] & 0xf) << 8)) - stickCenterRight[0]) / 1440f;
                stickRight[1] = ((UInt16)((stick_rawRight[1] >> 4) | (stick_rawRight[2] << 4)) - stickCenterRight[1]) / 1440f;
                JoyconRightStickX = -stickRight[0];
                JoyconRightStickY = -stickRight[1];
                acc_gRight.X = ((Int16)(report_bufRight[13 + 0 * 12] | ((report_bufRight[14 + 0 * 12] << 8) & 0xff00)) + (Int16)(report_bufRight[13 + 1 * 12] | ((report_bufRight[14 + 1 * 12] << 8) & 0xff00)) + (Int16)(report_bufRight[13 + 2 * 12] | ((report_bufRight[14 + 2 * 12] << 8) & 0xff00)) - acc_gcalibrationRightX) * (1.0f / 12000f);
                acc_gRight.Y = -((Int16)(report_bufRight[15 + 0 * 12] | ((report_bufRight[16 + 0 * 12] << 8) & 0xff00)) + (Int16)(report_bufRight[15 + 1 * 12] | ((report_bufRight[16 + 1 * 12] << 8) & 0xff00)) + (Int16)(report_bufRight[15 + 2 * 12] | ((report_bufRight[16 + 2 * 12] << 8) & 0xff00)) - acc_gcalibrationRightY) * (1.0f / 12000f);
                acc_gRight.Z = -((Int16)(report_bufRight[17 + 0 * 12] | ((report_bufRight[18 + 0 * 12] << 8) & 0xff00)) + (Int16)(report_bufRight[17 + 1 * 12] | ((report_bufRight[18 + 1 * 12] << 8) & 0xff00)) + (Int16)(report_bufRight[17 + 2 * 12] | ((report_bufRight[18 + 2 * 12] << 8) & 0xff00)) - acc_gcalibrationRightZ) * (1.0f / 12000f);
                JoyconRightButtonSHOULDER_1 = (report_bufRight[3 + 0] & 0x40) != 0;
                JoyconRightButtonSHOULDER_2 = (report_bufRight[3 + 0] & 0x80) != 0;
                JoyconRightButtonSR = (report_bufRight[3 + 0] & 0x10) != 0;
                JoyconRightButtonSL = (report_bufRight[3 + 0] & 0x20) != 0;
                JoyconRightButtonDPAD_DOWN = (report_bufRight[3 + 0] & (0x04)) != 0;
                JoyconRightButtonDPAD_RIGHT = (report_bufRight[3 + 0] & (0x08)) != 0;
                JoyconRightButtonDPAD_UP = (report_bufRight[3 + 0] & (0x02)) != 0;
                JoyconRightButtonDPAD_LEFT = (report_bufRight[3 + 0] & (0x01)) != 0;
                JoyconRightButtonPLUS = (report_bufRight[4] & 0x02) != 0;
                JoyconRightButtonHOME = (report_bufRight[4] & 0x10) != 0;
                JoyconRightButtonSTICK = (report_bufRight[4] & (0x04)) != 0;
                JoyconRightButtonACC = acc_gRight.X <= -1.13;
                JoyconRightButtonSPA = JoyconRightButtonSL | JoyconRightButtonSR | JoyconRightButtonPLUS | JoyconRightButtonACC;
                DirectAnglesRight = acc_gRight - InitDirectAnglesRight;
                JoyconRightAccelX = DirectAnglesRight.X * 1350f;
                JoyconRightAccelY = -DirectAnglesRight.Y * 1350f;
                gyr_gRight.X = (Int16)(report_bufRight[19 + 0 * 12] | ((report_bufRight[20 + 0 * 12] << 8) & 0xff00)) + (Int16)(report_bufRight[19 + 1 * 12] | ((report_bufRight[20 + 1 * 12] << 8) & 0xff00)) + (Int16)(report_bufRight[19 + 2 * 12] | ((report_bufRight[20 + 2 * 12] << 8) & 0xff00));
                gyr_gRight.Y = (Int16)(report_bufRight[21 + 0 * 12] | ((report_bufRight[22 + 0 * 12] << 8) & 0xff00)) + (Int16)(report_bufRight[21 + 1 * 12] | ((report_bufRight[22 + 1 * 12] << 8) & 0xff00)) + (Int16)(report_bufRight[21 + 2 * 12] | ((report_bufRight[22 + 2 * 12] << 8) & 0xff00));
                gyr_gRight.Z = (Int16)(report_bufRight[23 + 0 * 12] | ((report_bufRight[24 + 0 * 12] << 8) & 0xff00)) + (Int16)(report_bufRight[23 + 1 * 12] | ((report_bufRight[24 + 1 * 12] << 8) & 0xff00)) + (Int16)(report_bufRight[23 + 2 * 12] | ((report_bufRight[24 + 2 * 12] << 8) & 0xff00));
                JoyconRightGyroX = gyr_gRight.Z;
                JoyconRightGyroY = gyr_gRight.Y;
            }
            catch { }
        }
        public void ReconnectionLeft()
        {
            if (reconnectingcountleft == 0)
                reconnectingboolleft = true;
            reconnectingcountleft++;
            if (reconnectingcountleft >= 150f)
            {
                if (reconnectingboolleft)
                {
                    ReconnectionInitLeft();
                    AttachGripLeftController(pathleft);
                    reconnectingcountleft = -150f;
                }
                else
                    reconnectingcountleft = 0;
            }
        }
        private void ReconnectionInitLeft()
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
        public void ReconnectionRight()
        {
            if (reconnectingcountright == 0)
                reconnectingboolright = true;
            reconnectingcountright++;
            if (reconnectingcountright >= 150f)
            {
                if (reconnectingboolright)
                {
                    ReconnectionInitRight();
                    AttachGripRightController(pathright);
                    reconnectingcountright = -150f;
                }
                else
                    reconnectingcountright = 0;
            }
        }
        private void ReconnectionInitRight()
        {
            JoyconRightStickX = 0;
            JoyconRightStickY = 0;
            JoyconRightButtonSHOULDER_1 = false;
            JoyconRightButtonSHOULDER_2 = false;
            JoyconRightButtonSR = false;
            JoyconRightButtonSL = false;
            JoyconRightButtonDPAD_DOWN = false;
            JoyconRightButtonDPAD_RIGHT = false;
            JoyconRightButtonDPAD_UP = false;
            JoyconRightButtonDPAD_LEFT = false;
            JoyconRightButtonPLUS = false;
            JoyconRightButtonHOME = false;
            JoyconRightButtonSTICK = false;
            JoyconRightButtonACC = false;
            JoyconRightButtonSPA = false;
            JoyconRightRollLeft = false;
            JoyconRightRollRight = false;
            JoyconRightAccelX = 0;
            JoyconRightAccelY = 0;
            JoyconRightGyroX = 0;
            JoyconRightGyroY = 0;
        }
        public const string vendor_id = "57e", vendor_id_ = "057e", product_grip = "200e";
        public enum EFileAttributes : uint
        {
            Overlapped = 0x40000000,
            Normal = 0x80
        };
        public struct SP_DEVICE_INTERFACE_DATA
        {
            public int cbSize;
            public Guid InterfaceClassGuid;
            public int Flags;
            public IntPtr RESERVED;
        }
        public struct SP_DEVICE_INTERFACE_DETAIL_DATA
        {
            public UInt32 cbSize;
            [System.Runtime.InteropServices.MarshalAs(System.Runtime.InteropServices.UnmanagedType.ByValTStr, SizeConst = 256)]
            public string DevicePath;
        }
        public void Scan(int number = 0)
        {
            this.number = number;
            bool ISLEFT = false;
            bool ISRIGHT = false;
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
                        if ((diDetail.DevicePath.Contains(vendor_id) | diDetail.DevicePath.Contains(vendor_id_)) & diDetail.DevicePath.Contains(product_grip))
                        {
                            if (ISLEFT)
                            {
                                pathright = diDetail.DevicePath;
                                isvalidhandle = AttachGripRightController(diDetail.DevicePath);
                                if (isvalidhandle)
                                {
                                    pathsright.Add(pathright);
                                    handlesRight.Add(handleRight);
                                    ISRIGHT = true;
                                }
                            }
                            if (!ISLEFT)
                            {
                                pathleft = diDetail.DevicePath;
                                isvalidhandle = AttachGripLeftController(diDetail.DevicePath);
                                if (isvalidhandle)
                                {
                                    pathsleft.Add(pathleft);
                                    handlesLeft.Add(handleLeft);
                                    ISLEFT = true;
                                }
                            }
                            if (ISLEFT & ISRIGHT)
                            {
                                ISLEFT = false;
                                ISRIGHT = false;
                            }
                        }
                    }
                    index++;
                }
            }
            pathleft = pathsleft[number < 2 ? 0 : number - 1];
            handleLeft = handlesLeft[number < 2 ? 0 : number - 1];
            pathright = pathsright[number < 2 ? 0 : number - 1];
            handleRight = handlesRight[number < 2 ? 0 : number - 1];
        }
        private bool AttachGripLeftController(string path)
        {
            try
            {
                IntPtr handle = CreateFile(path, System.IO.FileAccess.ReadWrite, System.IO.FileShare.ReadWrite, new System.IntPtr(), System.IO.FileMode.Open, EFileAttributes.Normal, new System.IntPtr());
                handleLeft = Lhid_open_path(handle);
                Subcommand1GripLeftController(0x06, new byte[] { 0x01 }, 1);
                Subcommand2GripLeftController(0x40, new byte[] { 0x1 }, 1);
                Subcommand2GripLeftController(0x3, new byte[] { 0x30 }, 1);
                return true;
            }
            catch { return false; }
        }
        private void Subcommand1GripLeftController(byte sc, byte[] buf, uint len)
        {
            byte[] buf_Left = new byte[report_lenLeft];
            System.Array.Copy(buf, 0, buf_Left, 11, len);
            buf_Left[10] = sc;
            buf_Left[1] = 0x1;
            buf_Left[0] = 0x80;
            Lhid_write(handleLeft, buf_Left, new UIntPtr(2));
            Lhid_read_timeout(handleLeft, buf_Left, (UIntPtr)report_lenLeft);
            buf_Left[1] = 0x2;
            buf_Left[0] = 0x80;
            Lhid_write(handleLeft, buf_Left, new UIntPtr(2));
            Lhid_read_timeout(handleLeft, buf_Left, (UIntPtr)report_lenLeft);
            buf_Left[1] = 0x3;
            buf_Left[0] = 0x80;
            Lhid_write(handleLeft, buf_Left, new UIntPtr(2));
            Lhid_read_timeout(handleLeft, buf_Left, (UIntPtr)report_lenLeft);
            buf_Left[1] = 0x2;
            buf_Left[0] = 0x80;
            Lhid_write(handleLeft, buf_Left, new UIntPtr(2));
            Lhid_read_timeout(handleLeft, buf_Left, (UIntPtr)report_lenLeft);
            buf_Left[1] = 0x4;
            buf_Left[0] = 0x80;
            Lhid_write(handleLeft, buf_Left, new UIntPtr(2));
            Lhid_read_timeout(handleLeft, buf_Left, (UIntPtr)report_lenLeft);
        }
        private void Subcommand2GripLeftController(byte sc, byte[] buf, uint len)
        {
            byte[] buf_Left = new byte[report_lenLeft];
            System.Array.Copy(buf, 0, buf_Left, 11, len);
            buf_Left[10] = sc;
            buf_Left[1] = 0;
            buf_Left[0] = 0x1;
            Lhid_write(handleLeft, buf_Left, (UIntPtr)(len + 11));
            Lhid_read_timeout(handleLeft, buf_Left, (UIntPtr)report_lenLeft);
        }
        private void Subcommand3GripLeftController(byte sc, byte[] buf, uint len)
        {
            byte[] buf_Left = new byte[report_lenLeft];
            System.Array.Copy(buf, 0, buf_Left, 11, len);
            buf_Left[10] = sc;
            buf_Left[1] = 0x5;
            buf_Left[0] = 0x80;
            Lhid_write(handleLeft, buf_Left, new UIntPtr(2));
            buf_Left[1] = 0x6;
            buf_Left[0] = 0x80;
            Lhid_write(handleLeft, buf_Left, new UIntPtr(2));
        }
        private bool AttachGripRightController(string path)
        {
            try
            {
                IntPtr handle = CreateFile(path, System.IO.FileAccess.ReadWrite, System.IO.FileShare.ReadWrite, new System.IntPtr(), System.IO.FileMode.Open, EFileAttributes.Normal, new System.IntPtr());
                handleRight = Rhid_open_path(handle);
                Subcommand1GripRightController(0x06, new byte[] { 0x01 }, 1);
                Subcommand2GripRightController(0x40, new byte[] { 0x1 }, 1);
                Subcommand2GripRightController(0x3, new byte[] { 0x30 }, 1);
                return true;
            }
            catch { return false; }
        }
        private void Subcommand1GripRightController(byte sc, byte[] buf, uint len)
        {
            byte[] buf_Right = new byte[report_lenRight];
            System.Array.Copy(buf, 0, buf_Right, 11, len);
            buf_Right[10] = sc;
            buf_Right[1] = 0x1;
            buf_Right[0] = 0x80;
            Rhid_write(handleRight, buf_Right, new UIntPtr(2));
            Rhid_read_timeout(handleRight, buf_Right, (UIntPtr)report_lenRight);
            buf_Right[1] = 0x2;
            buf_Right[0] = 0x80;
            Rhid_write(handleRight, buf_Right, new UIntPtr(2));
            Rhid_read_timeout(handleRight, buf_Right, (UIntPtr)report_lenRight);
            buf_Right[1] = 0x3;
            buf_Right[0] = 0x80;
            Rhid_write(handleRight, buf_Right, new UIntPtr(2));
            Rhid_read_timeout(handleRight, buf_Right, (UIntPtr)report_lenRight);
            buf_Right[1] = 0x2;
            buf_Right[0] = 0x80;
            Rhid_write(handleRight, buf_Right, new UIntPtr(2));
            Rhid_read_timeout(handleRight, buf_Right, (UIntPtr)report_lenRight);
            buf_Right[1] = 0x4;
            buf_Right[0] = 0x80;
            Rhid_write(handleRight, buf_Right, new UIntPtr(2));
            Rhid_read_timeout(handleRight, buf_Right, (UIntPtr)report_lenRight);
        }
        private void Subcommand2GripRightController(byte sc, byte[] buf, uint len)
        {
            byte[] buf_Right = new byte[report_lenRight];
            System.Array.Copy(buf, 0, buf_Right, 11, len);
            buf_Right[10] = sc;
            buf_Right[1] = 0;
            buf_Right[0] = 0x1;
            Rhid_write(handleRight, buf_Right, (UIntPtr)(len + 11));
            Rhid_read_timeout(handleRight, buf_Right, (UIntPtr)report_lenRight);
        }
        private void Subcommand3GripRightController(byte sc, byte[] buf, uint len)
        {
            byte[] buf_Right = new byte[report_lenRight];
            System.Array.Copy(buf, 0, buf_Right, 11, len);
            buf_Right[10] = sc;
            buf_Right[1] = 0x5;
            buf_Right[0] = 0x80;
            Rhid_write(handleRight, buf_Right, new UIntPtr(2));
            buf_Right[1] = 0x6;
            buf_Right[0] = 0x80;
            Rhid_write(handleRight, buf_Right, new UIntPtr(2));
        }
    }
}