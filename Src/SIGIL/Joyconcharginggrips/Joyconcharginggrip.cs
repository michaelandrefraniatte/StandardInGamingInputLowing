﻿using Microsoft.Win32.SafeHandles;
using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using Vector3 = System.Numerics.Vector3;
using Joyconcharginggrips;
using System.Collections.Generic;
using System.Linq;
using System.Diagnostics;
using Valuechanges;

namespace JoyconChargingGripsAPI
{
    public class JoyconChargingGrip : IDisposable
    {
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
        [DllImport("lhidread.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "Lhid_read_timeout")]
        private static extern int Lhid_read_timeout(SafeFileHandle dev, byte[] data, UIntPtr length);
        [DllImport("lhidread.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "Lhid_write")]
        private static extern int Lhid_write(SafeFileHandle device, byte[] data, UIntPtr length);
        [DllImport("lhidread.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "Lhid_open_path")]
        private static extern SafeFileHandle Lhid_open_path(IntPtr handle);
        [DllImport("lhidread.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "Lhid_close")]
        private static extern void Lhid_close(SafeFileHandle device);
        [DllImport("rhidread.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "Rhid_read_timeout")]
        private static extern int Rhid_read_timeout(SafeFileHandle dev, byte[] data, UIntPtr length);
        [DllImport("rhidread.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "Rhid_write")]
        private static extern int Rhid_write(SafeFileHandle device, byte[] data, UIntPtr length);
        [DllImport("rhidread.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "Rhid_open_path")]
        private static extern SafeFileHandle Rhid_open_path(IntPtr handle);
        [DllImport("rhidread.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "Rhid_close")]
        private static extern void Rhid_close(SafeFileHandle device);
        [DllImport("winmm.dll", EntryPoint = "timeBeginPeriod")]
        private static extern uint TimeBeginPeriod(uint ms);
        [DllImport("winmm.dll", EntryPoint = "timeEndPeriod")]
        private static extern uint TimeEndPeriod(uint ms);
        [DllImport("ntdll.dll", EntryPoint = "NtSetTimerResolution")]
        private static extern void NtSetTimerResolution(uint DesiredResolution, bool SetResolution, ref uint CurrentResolution);
        private static uint CurrentResolution = 0;
        private const uint report_lenLeft = 49;
        private byte[] report_bufLeft = new byte[report_lenLeft];
        private const uint report_lenRight = 49;
        private byte[] report_bufRight = new byte[report_lenRight];
        private SafeFileHandle handleRight;
        private SafeFileHandle handleLeft;
        private IntPtr handleptrRight, handleptrunsharedRight;
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
        public bool JoyconRightButtonSPA, JoyconRightButtonACC, JoyconRightRollLeft, JoyconRightRollRight;
        public double JoyconRightStickX, JoyconRightStickY;
        private System.Collections.Generic.List<double> RightValListX = new System.Collections.Generic.List<double>(), RightValListY = new System.Collections.Generic.List<double>();
        public bool JoyconRightAccelCenter, JoyconRightStickCenter;
        public double JoyconRightAccelX, JoyconRightAccelY, JoyconRightGyroX, JoyconRightGyroY;
        private double[] stickRight = { 0, 0 };
        private double[] stickCenterRight = { 0, 0 };
        private byte[] stick_rawRight = { 0, 0, 0 };
        private Vector3 acc_gRight = new Vector3();
        private Vector3 gyr_gRight = new Vector3();
        private Vector3 InitDirectAnglesRight, DirectAnglesRight;
        public bool JoyconRightButtonSHOULDER_1, JoyconRightButtonSHOULDER_2, JoyconRightButtonSR, JoyconRightButtonSL, JoyconRightButtonDPAD_DOWN, JoyconRightButtonDPAD_RIGHT, JoyconRightButtonDPAD_UP, JoyconRightButtonDPAD_LEFT, JoyconRightButtonPLUS, JoyconRightButtonSTICK, JoyconRightButtonHOME;
        private float acc_gcalibrationRightX, acc_gcalibrationRightY, acc_gcalibrationRightZ;
        private bool running, formvisible;
        private bool isvalidhandle = false;
        private int number;
        private bool reconnectingboolleft, reconnectingboolright;
        private double reconnectingcountleft, reconnectingcountright;
        private string pathleft, pathright;
        private static List<string> pathsleft = new List<string>(), pathsright = new List<string>();
        private static List<SafeFileHandle> handlesLeft = new List<SafeFileHandle>(), handlesRight = new List<SafeFileHandle>();
        private byte[] default_bufLeft = { 0x0, 0x1, 0x40, 0x40, 0x0, 0x1, 0x40, 0x40 };
        private byte[] default_bufRight = { 0x0, 0x1, 0x40, 0x40, 0x0, 0x1, 0x40, 0x40 };
        private Form1 form1;
        private Stopwatch LeftPollingRate, RightPollingRate;
        private double leftpollingrateperm = 0, leftpollingratetemp = 0, leftpollingratedisplay = 0, leftpollingrate, rightpollingrateperm = 0, rightpollingratetemp = 0, rightpollingratedisplay = 0, rightpollingrate;
        private string leftinputdelaybutton = "", leftinputdelay = "", leftinputdelaytemp = "", rightinputdelaybutton = "", rightinputdelay = "", rightinputdelaytemp = "";
        public Valuechange LeftValueChange, RightValueChange;
        private double leftdelay, leftelapseddown, leftelapsedup, leftelapsed, rightdelay, rightelapseddown, rightelapsedup, rightelapsed;
        private bool leftgetstate = false, rightgetstate = false;
        private bool[] wd = { false, false };
        private bool[] wu = { false, false };
        private bool[] ws = { false, false };
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
        public JoyconChargingGrip()
        {
            TimeBeginPeriod(1);
            NtSetTimerResolution(1, true, ref CurrentResolution);
            running = true;
        }
        public void ViewData(string leftinputdelaybutton = "", string rightinputdelaybutton = "")
        {
            if (!formvisible)
            {
                form1 = new Form1();
                LeftPollingRate = new Stopwatch();
                LeftPollingRate.Start();
                LeftValueChange = new Valuechange();
                this.leftinputdelaybutton = leftinputdelaybutton;
                RightPollingRate = new Stopwatch();
                RightPollingRate.Start();
                RightValueChange = new Valuechange();
                this.rightinputdelaybutton = rightinputdelaybutton;
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
            Thread.Sleep(100);
            SubcommandGripLeftController(0x06, new byte[] { 0x80, 0x05 });
            SubcommandGripLeftController(0x06, new byte[] { 0x80, 0x06 });
            SubcommandGripRightController(0x06, new byte[] { 0x80, 0x05 });
            SubcommandGripRightController(0x06, new byte[] { 0x80, 0x06 });
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
                    leftpollingratedisplay++;
                    leftpollingratetemp = leftpollingrateperm;
                    leftpollingrateperm = (double)LeftPollingRate.ElapsedTicks / (Stopwatch.Frequency / 1000L);
                    if (leftpollingratedisplay > 300)
                    {
                        leftpollingrate = leftpollingrateperm - leftpollingratetemp;
                        leftpollingratedisplay = 0;
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
                    str += "PollingRate : " + leftpollingrate + " ms" + Environment.NewLine;
                    string txt = str;
                    string[] lines = txt.Split(new string[] { Environment.NewLine }, StringSplitOptions.None);
                    foreach (string line in lines)
                        if (line.Contains(leftinputdelaybutton + " : "))
                        {
                            leftinputdelaytemp = leftinputdelay;
                            leftinputdelay = line;
                        }
                    valchanged(0, leftinputdelay.Contains("True") | (!leftinputdelay.Contains("True") & !leftinputdelay.Contains("False") & leftinputdelay != leftinputdelaytemp));
                    if (wd[0])
                    {
                        leftgetstate = true;
                    }
                    if (leftinputdelay.Contains("False") | (!leftinputdelay.Contains("True") & !leftinputdelay.Contains("False") & leftinputdelay == leftinputdelaytemp))
                        leftgetstate = false;
                    if (leftgetstate)
                    {
                        leftelapseddown = (double)LeftPollingRate.ElapsedTicks / (Stopwatch.Frequency / 1000L);
                        leftelapsed = 0;
                    }
                    if (wu[0])
                    {
                        leftelapsedup = (double)LeftPollingRate.ElapsedTicks / (Stopwatch.Frequency / 1000L);
                        leftelapsed = leftelapsedup - leftelapseddown;
                    }
                    LeftValueChange[0] = leftinputdelay.Contains("False") | (!leftinputdelay.Contains("True") & !leftinputdelay.Contains("False") & leftinputdelay == leftinputdelaytemp) ? leftelapsed : 0;
                    if (LeftValueChange._ValueChange[0] > 0)
                    {
                        leftdelay = LeftValueChange._ValueChange[0];
                    }
                    str += "InputDelay : " + leftdelay + " ms" + Environment.NewLine;
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
                    rightpollingratedisplay++;
                    rightpollingratetemp = rightpollingrateperm;
                    rightpollingrateperm = (double)RightPollingRate.ElapsedTicks / (Stopwatch.Frequency / 1000L);
                    if (rightpollingratedisplay > 300)
                    {
                        rightpollingrate = rightpollingrateperm - rightpollingratetemp;
                        rightpollingratedisplay = 0;
                    }
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
                    str += "PollingRate : " + rightpollingrate + " ms" + Environment.NewLine;
                    string txt = str;
                    string[] lines = txt.Split(new string[] { Environment.NewLine }, StringSplitOptions.None);
                    foreach (string line in lines)
                        if (line.Contains(rightinputdelaybutton + " : "))
                        {
                            rightinputdelaytemp = rightinputdelay;
                            rightinputdelay = line;
                        }
                    valchanged(1, rightinputdelay.Contains("True") | (!rightinputdelay.Contains("True") & !rightinputdelay.Contains("False") & rightinputdelay != rightinputdelaytemp));
                    if (wd[1])
                    {
                        rightgetstate = true;
                    }
                    if (rightinputdelay.Contains("False") | (!rightinputdelay.Contains("True") & !rightinputdelay.Contains("False") & rightinputdelay == rightinputdelaytemp))
                        rightgetstate = false;
                    if (rightgetstate)
                    {
                        rightelapseddown = (double)RightPollingRate.ElapsedTicks / (Stopwatch.Frequency / 1000L);
                        rightelapsed = 0;
                    }
                    if (wu[1])
                    {
                        rightelapsedup = (double)RightPollingRate.ElapsedTicks / (Stopwatch.Frequency / 1000L);
                        rightelapsed = rightelapsedup - rightelapseddown;
                    }
                    RightValueChange[0] = rightinputdelay.Contains("False") | (!rightinputdelay.Contains("True") & !rightinputdelay.Contains("False") & rightinputdelay == rightinputdelaytemp) ? rightelapsed : 0;
                    if (RightValueChange._ValueChange[0] > 0)
                    {
                        rightdelay = RightValueChange._ValueChange[0];
                    }
                    str += "InputDelay : " + rightdelay + " ms" + Environment.NewLine;
                    str += Environment.NewLine;
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
                ProcessStateLogicLeft();
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
                ProcessStateLogicRight();
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
                stick_rawLeft[0] = report_bufLeft[6];
                stick_rawLeft[1] = report_bufLeft[7];
                stick_rawLeft[2] = report_bufLeft[8];
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
        private void ProcessStateLogicLeft()
        {
            try
            {
                stick_rawLeft[0] = report_bufLeft[6];
                stick_rawLeft[1] = report_bufLeft[7];
                stick_rawLeft[2] = report_bufLeft[8];
                stickLeft[0] = ((UInt16)(stick_rawLeft[0] | ((stick_rawLeft[1] & 0xf) << 8)) - stickCenterLeft[0]) / 1440f;
                stickLeft[1] = ((UInt16)((stick_rawLeft[1] >> 4) | (stick_rawLeft[2] << 4)) - stickCenterLeft[1]) / 1440f;
                JoyconLeftStickX = stickLeft[0];
                JoyconLeftStickY = stickLeft[1];
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
                acc_gLeft.X = ((Int16)(report_bufLeft[13 + 0 * 12] | ((report_bufLeft[14 + 0 * 12] << 8) & 0xff00)) + (Int16)(report_bufLeft[13 + 1 * 12] | ((report_bufLeft[14 + 1 * 12] << 8) & 0xff00)) + (Int16)(report_bufLeft[13 + 2 * 12] | ((report_bufLeft[14 + 2 * 12] << 8) & 0xff00)) - acc_gcalibrationLeftX) * (1.0f / 12000f);
                acc_gLeft.Y = -((Int16)(report_bufLeft[15 + 0 * 12] | ((report_bufLeft[16 + 0 * 12] << 8) & 0xff00)) + (Int16)(report_bufLeft[15 + 1 * 12] | ((report_bufLeft[16 + 1 * 12] << 8) & 0xff00)) + (Int16)(report_bufLeft[15 + 2 * 12] | ((report_bufLeft[16 + 2 * 12] << 8) & 0xff00)) - acc_gcalibrationLeftY) * (1.0f / 12000f);
                acc_gLeft.Z = -((Int16)(report_bufLeft[17 + 0 * 12] | ((report_bufLeft[18 + 0 * 12] << 8) & 0xff00)) + (Int16)(report_bufLeft[17 + 1 * 12] | ((report_bufLeft[18 + 1 * 12] << 8) & 0xff00)) + (Int16)(report_bufLeft[17 + 2 * 12] | ((report_bufLeft[18 + 2 * 12] << 8) & 0xff00)) - acc_gcalibrationLeftZ) * (1.0f / 12000f);
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
        private void ProcessStateLogicRight()
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
                JoyconRightButtonSHOULDER_1 = (report_bufRight[3] & 0x40) != 0;
                JoyconRightButtonSHOULDER_2 = (report_bufRight[3] & 0x80) != 0;
                JoyconRightButtonSR = (report_bufRight[3] & 0x10) != 0;
                JoyconRightButtonSL = (report_bufRight[3] & 0x20) != 0;
                JoyconRightButtonDPAD_DOWN = (report_bufRight[3] & (0x04)) != 0;
                JoyconRightButtonDPAD_RIGHT = (report_bufRight[3] & (0x08)) != 0;
                JoyconRightButtonDPAD_UP = (report_bufRight[3] & (0x02)) != 0;
                JoyconRightButtonDPAD_LEFT = (report_bufRight[3] & (0x01)) != 0;
                JoyconRightButtonPLUS = (report_bufRight[4] & 0x02) != 0;
                JoyconRightButtonHOME = (report_bufRight[4] & 0x10) != 0;
                JoyconRightButtonSTICK = (report_bufRight[4] & (0x04)) != 0;
                JoyconRightButtonACC = acc_gRight.X <= -1.13;
                JoyconRightButtonSPA = JoyconRightButtonSL | JoyconRightButtonSR | JoyconRightButtonPLUS | JoyconRightButtonACC;
                acc_gRight.X = ((Int16)(report_bufRight[13 + 0 * 12] | ((report_bufRight[14 + 0 * 12] << 8) & 0xff00)) + (Int16)(report_bufRight[13 + 1 * 12] | ((report_bufRight[14 + 1 * 12] << 8) & 0xff00)) + (Int16)(report_bufRight[13 + 2 * 12] | ((report_bufRight[14 + 2 * 12] << 8) & 0xff00)) - acc_gcalibrationRightX) * (1.0f / 12000f);
                acc_gRight.Y = -((Int16)(report_bufRight[15 + 0 * 12] | ((report_bufRight[16 + 0 * 12] << 8) & 0xff00)) + (Int16)(report_bufRight[15 + 1 * 12] | ((report_bufRight[16 + 1 * 12] << 8) & 0xff00)) + (Int16)(report_bufRight[15 + 2 * 12] | ((report_bufRight[16 + 2 * 12] << 8) & 0xff00)) - acc_gcalibrationRightY) * (1.0f / 12000f);
                acc_gRight.Z = -((Int16)(report_bufRight[17 + 0 * 12] | ((report_bufRight[18 + 0 * 12] << 8) & 0xff00)) + (Int16)(report_bufRight[17 + 1 * 12] | ((report_bufRight[18 + 1 * 12] << 8) & 0xff00)) + (Int16)(report_bufRight[17 + 2 * 12] | ((report_bufRight[18 + 2 * 12] << 8) & 0xff00)) - acc_gcalibrationRightZ) * (1.0f / 12000f);
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
        private void ReconnectionLeft()
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
        private void ReconnectionRight()
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
        private const string vendor_id = "57e", vendor_id_ = "057e", product_grip = "200e";
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
                            if (ISRIGHT)
                            {
                                pathleft = diDetail.DevicePath;
                                isvalidhandle = AttachGripLeftController(diDetail.DevicePath);
                                handleptrunsharedLeft = CreateFile(pathleft, System.IO.FileAccess.ReadWrite, System.IO.FileShare.None, new System.IntPtr(), System.IO.FileMode.Open, EFileAttributes.Normal, new System.IntPtr());
                                if (isvalidhandle)
                                {
                                    pathsleft.Add(pathleft);
                                    handlesLeft.Add(handleLeft);
                                    ISLEFT = true;
                                }
                            }
                            if (!ISRIGHT)
                            {
                                pathright = diDetail.DevicePath;
                                isvalidhandle = AttachGripRightController(diDetail.DevicePath);
                                handleptrunsharedRight = CreateFile(pathright, System.IO.FileAccess.ReadWrite, System.IO.FileShare.None, new System.IntPtr(), System.IO.FileMode.Open, EFileAttributes.Normal, new System.IntPtr());
                                if (isvalidhandle)
                                {
                                    pathsright.Add(pathright);
                                    handlesRight.Add(handleRight);
                                    ISRIGHT = true;
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
                handleptrLeft = CreateFile(path, System.IO.FileAccess.ReadWrite, System.IO.FileShare.ReadWrite, new System.IntPtr(), System.IO.FileMode.Open, EFileAttributes.Normal, new System.IntPtr());
                handleLeft = Lhid_open_path(handleptrLeft);
                ResetingGripLeftController();
                SubcommandGripLeftController(0x06, new byte[] { 0x80, 0x02 });
                SubcommandGripLeftController(0x06, new byte[] { 0x80, 0x03 });
                SubcommandGripLeftController(0x06, new byte[] { 0x80, 0x02 });
                SubcommandGripLeftController(0x06, new byte[] { 0x80, 0x04 });
                SubcommandGripLeftController(0x40, new byte[] { 0x01, 0x01 });
                SubcommandGripLeftController(0x03, new byte[] { 0x01, 0x30 });
                return true;
            }
            catch { return false; }
        }
        private void ResetingGripLeftController()
        {
            byte[] a = { 0x0 };
            a = Enumerable.Repeat((byte)0, (int)report_lenLeft).ToArray();
            a[0] = 0x80;
            a[1] = 0x1;
            Lhid_write(handleLeft, a, new UIntPtr(2));
            Lhid_read_timeout(handleLeft, a, (UIntPtr)report_lenLeft);
            if (a[0] != 0x81)
            {
                SubcommandGripLeftController(0x06, new byte[] { 0x80, 0x01 });
            }
        }
        private void SubcommandGripLeftController(byte sc, byte[] buf)
        {
            byte[] buf_ = new byte[report_lenLeft];
            byte[] response = new byte[report_lenLeft];
            Array.Copy(default_bufLeft, 0, buf_, 2, 8);
            Array.Copy(buf, 0, buf_, 11, 1);
            buf_[10] = sc;
            buf_[1] = buf[1];
            buf_[0] = buf[0];
            Lhid_write(handleLeft, buf_, new UIntPtr(12));
            Lhid_read_timeout(handleLeft, response, (UIntPtr)report_lenLeft);
        }
        private bool AttachGripRightController(string path)
        {
            try
            {
                handleptrRight = CreateFile(path, System.IO.FileAccess.ReadWrite, System.IO.FileShare.ReadWrite, new System.IntPtr(), System.IO.FileMode.Open, EFileAttributes.Normal, new System.IntPtr());
                handleRight = Rhid_open_path(handleptrRight);
                ResetingGripRightController();
                SubcommandGripRightController(0x06, new byte[] { 0x80, 0x02 });
                SubcommandGripRightController(0x06, new byte[] { 0x80, 0x03 });
                SubcommandGripRightController(0x06, new byte[] { 0x80, 0x02 });
                SubcommandGripRightController(0x06, new byte[] { 0x80, 0x04 });
                SubcommandGripRightController(0x40, new byte[] { 0x01, 0x01 });
                SubcommandGripRightController(0x03, new byte[] { 0x01, 0x30 });
                return true;
            }
            catch { return false; }
        }
        private void ResetingGripRightController()
        {
            byte[] a = { 0x0 };
            a = Enumerable.Repeat((byte)0, (int)report_lenRight).ToArray();
            a[0] = 0x80;
            a[1] = 0x1;
            Rhid_write(handleRight, a, new UIntPtr(2));
            Rhid_read_timeout(handleRight, a, (UIntPtr)report_lenRight);
            if (a[0] != 0x81)
            {
                SubcommandGripRightController(0x06, new byte[] { 0x80, 0x01 });
            }
        }
        private void SubcommandGripRightController(byte sc, byte[] buf)
        {
            byte[] buf_ = new byte[report_lenRight];
            byte[] response = new byte[report_lenRight];
            Array.Copy(default_bufRight, 0, buf_, 2, 8);
            Array.Copy(buf, 0, buf_, 11, 1);
            buf_[10] = sc;
            buf_[1] = buf[1];
            buf_[0] = buf[0];
            Rhid_write(handleRight, buf_, new UIntPtr(12));
            Rhid_read_timeout(handleRight, response, (UIntPtr)report_lenRight);
        }
        public void Dispose()
        {
            GC.Collect();
            GC.WaitForPendingFinalizers();
            GC.SuppressFinalize(this);
        }
    }
}