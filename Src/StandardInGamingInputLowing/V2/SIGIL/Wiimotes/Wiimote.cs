using Microsoft.Win32.SafeHandles;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using Wiimotes;

namespace WiiMotesAPI
{
    public class WiiMote
    {
        [DllImport("MotionInputPairing.dll", EntryPoint = "wiimotesconnect")]
        public static extern bool wiimotesconnect();
        [DllImport("MotionInputPairing.dll", EntryPoint = "wiimotesdisconnect")]
        public static extern bool wiimotesdisconnect();
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
        private const double REGISTER_IR = 0x04b00030, REGISTER_EXTENSION_INIT_1 = 0x04a400f0, REGISTER_EXTENSION_INIT_2 = 0x04a400fb, REGISTER_EXTENSION_TYPE = 0x04a400fa, REGISTER_EXTENSION_CALIBRATION = 0x04a40020, REGISTER_MOTIONPLUS_INIT = 0x04a600fe;
        public string path1, path2;
        private const byte Type = 0x12, IR = 0x13, WriteMemory = 0x16, ReadMemory = 0x16, IRExtensionAccel = 0x37;
        public bool running, formvisible;
        private int irmode;
        private double centery;
        public List<double> vallistir1x = new List<double>(), vallistir1y = new List<double>(), vallistir2x = new List<double>(), vallistir2y = new List<double>();
        public static bool reconnectingwiimote1bool, Wiimote1IRswitch;
        public static double reconnectingwiimote1count, Wiimote1IR0notfound = 0;
        public static bool reconnectingwiimote2bool, Wiimote2IRswitch;
        public static double reconnectingwiimote2count, Wiimote2IR0notfound = 0;
        public static double ir1x, ir1y, ir1x0, ir1y0, ir1x1, ir1y1, ir1x2, ir1y2, ir1x3, ir1y3, ir1xc, ir1yc, tempir1x, tempir1y, Wiimote1RawValuesX, Wiimote1RawValuesY, Wiimote1RawValuesZ, calibration1xinit, calibration1yinit, calibration1zinit, stickview1xinit, stickview1yinit, Wiimote1NunchuckStateRawValuesX, Wiimote1NunchuckStateRawValuesY, Wiimote1NunchuckStateRawValuesZ, Wiimote1NunchuckStateRawJoystickX, Wiimote1NunchuckStateRawJoystickY, Wiimote1IRSensors0X, Wiimote1IRSensors0Y, Wiimote1IRSensors1X, Wiimote1IRSensors1Y, Wiimote1IRSensors0Xcam, Wiimote1IRSensors1Xcam, Wiimote1IRSensors0Ycam, Wiimote1IRSensors1Ycam, Wiimote1IRSensorsXcam, Wiimote1IRSensorsYcam;
        public static double ir2x, ir2y, ir2x0, ir2y0, ir2x1, ir2y1, ir2x2, ir2y2, ir2x3, ir2y3, ir2xc, ir2yc, tempir2x, tempir2y, Wiimote2RawValuesX, Wiimote2RawValuesY, Wiimote2RawValuesZ, calibration2xinit, calibration2yinit, calibration2zinit, stickview2xinit, stickview2yinit, Wiimote2NunchuckStateRawValuesX, Wiimote2NunchuckStateRawValuesY, Wiimote2NunchuckStateRawValuesZ, Wiimote2NunchuckStateRawJoystickX, Wiimote2NunchuckStateRawJoystickY, Wiimote2IRSensors0X, Wiimote2IRSensors0Y, Wiimote2IRSensors1X, Wiimote2IRSensors1Y, Wiimote2IRSensors0Xcam, Wiimote2IRSensors1Xcam, Wiimote2IRSensors0Ycam, Wiimote2IRSensors1Ycam, Wiimote2IRSensorsXcam, Wiimote2IRSensorsYcam;
        public static bool Wiimote1ButtonStateA, Wiimote1ButtonStateB, Wiimote1ButtonStateMinus, Wiimote1ButtonStateHome, Wiimote1ButtonStatePlus, Wiimote1ButtonStateOne, Wiimote1ButtonStateTwo, Wiimote1ButtonStateUp, Wiimote1ButtonStateDown, Wiimote1ButtonStateLeft, Wiimote1ButtonStateRight, Wiimote1NunchuckStateC, Wiimote1NunchuckStateZ, ISWIIMOTE1, Wiimote1IR0found, Wiimote1IR1found, Wiimote1IR0foundcam, Wiimote1IR1foundcam;
        public static bool Wiimote2ButtonStateA, Wiimote2ButtonStateB, Wiimote2ButtonStateMinus, Wiimote2ButtonStateHome, Wiimote2ButtonStatePlus, Wiimote2ButtonStateOne, Wiimote2ButtonStateTwo, Wiimote2ButtonStateUp, Wiimote2ButtonStateDown, Wiimote2ButtonStateLeft, Wiimote2ButtonStateRight, Wiimote2NunchuckStateC, Wiimote2NunchuckStateZ, ISWIIMOTE2, Wiimote2IR0found, Wiimote2IR1found, Wiimote2IR0foundcam, Wiimote2IR1foundcam;
        public static byte[] buff1 = new byte[] { 0x55 }, mBuff1 = new byte[22], aBuffer1 = new byte[22];
        public static byte[] buff2 = new byte[] { 0x55 }, mBuff2 = new byte[22], aBuffer2 = new byte[22];
        public static FileStream mStream1;
        public static SafeFileHandle handle1 = null;
        public static FileStream mStream2;
        public static SafeFileHandle handle2 = null; 
        public Form1 form1 = new Form1();
        public WiiMote()
        {
            TimeBeginPeriod(1);
            NtSetTimerResolution(1, true, ref CurrentResolution);
            running = true;
            while (vallistir1x.Count <= 2)
            {
                vallistir1x.Add(0);
            }
            while (vallistir1y.Count <= 2)
            {
                vallistir1y.Add(0);
            }
            while (vallistir2x.Count <= 2)
            {
                vallistir2x.Add(0);
            }
            while (vallistir2y.Count <= 2)
            {
                vallistir2y.Add(0);
            }
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
            mStream1.Close();
            mStream1.Dispose();
            handle1.Close();
            handle1.Dispose();
            mStream2.Close();
            mStream2.Dispose();
            handle2.Close();
            handle2.Dispose();
        }
        public void BeginPolling()
        {
            Task.Run(() => taskD1());
            Task.Run(() => taskD2());
        }
        public void taskD1()
        {
            for (; ; )
            {
                if (!running)
                    break;
                Reconnection1();
                try
                {
                    mStream1.Read(aBuffer1, 0, 22);
                    reconnectingwiimote1bool = false;
                }
                catch { }
                ProcessStateLogic1();
                if (formvisible)
                {
                    string str = "ir1x : " + ir1x + Environment.NewLine;
                    str += "ir1y : " + ir1y + Environment.NewLine;
                    str += "Wiimote1ButtonStateA : " + Wiimote1ButtonStateA + Environment.NewLine;
                    str += "Wiimote1ButtonStateB : " + Wiimote1ButtonStateB + Environment.NewLine;
                    str += "Wiimote1ButtonStateMinus : " + Wiimote1ButtonStateMinus + Environment.NewLine;
                    str += "Wiimote1ButtonStateHome : " + Wiimote1ButtonStateHome + Environment.NewLine;
                    str += "Wiimote1ButtonStatePlus : " + Wiimote1ButtonStatePlus + Environment.NewLine;
                    str += "Wiimote1ButtonStateOne : " + Wiimote1ButtonStateOne + Environment.NewLine;
                    str += "Wiimote1ButtonStateTwo : " + Wiimote1ButtonStateTwo + Environment.NewLine;
                    str += "Wiimote1ButtonStateUp : " + Wiimote1ButtonStateUp + Environment.NewLine;
                    str += "Wiimote1ButtonStateDown : " + Wiimote1ButtonStateDown + Environment.NewLine;
                    str += "Wiimote1ButtonStateLeft : " + Wiimote1ButtonStateLeft + Environment.NewLine;
                    str += "Wiimote1ButtonStateRight : " + Wiimote1ButtonStateRight + Environment.NewLine;
                    str += "Wiimote1RawValuesX : " + Wiimote1RawValuesX + Environment.NewLine;
                    str += "Wiimote1RawValuesY : " + Wiimote1RawValuesY + Environment.NewLine;
                    str += "Wiimote1RawValuesZ : " + Wiimote1RawValuesZ + Environment.NewLine;
                    str += "Wiimote1NunchuckStateRawJoystickX : " + Wiimote1NunchuckStateRawJoystickX + Environment.NewLine;
                    str += "Wiimote1NunchuckStateRawJoystickY : " + Wiimote1NunchuckStateRawJoystickY + Environment.NewLine;
                    str += "Wiimote1NunchuckStateRawValuesX : " + Wiimote1NunchuckStateRawValuesX + Environment.NewLine;
                    str += "Wiimote1NunchuckStateRawValuesY : " + Wiimote1NunchuckStateRawValuesY + Environment.NewLine;
                    str += "Wiimote1NunchuckStateRawValuesZ : " + Wiimote1NunchuckStateRawValuesZ + Environment.NewLine;
                    str += "Wiimote1NunchuckStateC : " + Wiimote1NunchuckStateC + Environment.NewLine;
                    str += "Wiimote1NunchuckStateZ : " + Wiimote1NunchuckStateZ + Environment.NewLine;
                    form1.SetLabel1(str);
                }
            }
        }
        public void taskD2()
        {
            for (; ; )
            {
                if (!running)
                    break;
                Reconnection2();
                try
                {
                    mStream2.Read(aBuffer2, 0, 22);
                    reconnectingwiimote2bool = false;
                }
                catch { }
                ProcessStateLogic2();
                if (formvisible)
                {
                    string str = "ir2x : " + ir2x + Environment.NewLine;
                    str += "ir2y : " + ir2y + Environment.NewLine;
                    str += "Wiimote2ButtonStateA : " + Wiimote2ButtonStateA + Environment.NewLine;
                    str += "Wiimote2ButtonStateB : " + Wiimote2ButtonStateB + Environment.NewLine;
                    str += "Wiimote2ButtonStateMinus : " + Wiimote2ButtonStateMinus + Environment.NewLine;
                    str += "Wiimote2ButtonStateHome : " + Wiimote2ButtonStateHome + Environment.NewLine;
                    str += "Wiimote2ButtonStatePlus : " + Wiimote2ButtonStatePlus + Environment.NewLine;
                    str += "Wiimote2ButtonStateOne : " + Wiimote2ButtonStateOne + Environment.NewLine;
                    str += "Wiimote2ButtonStateTwo : " + Wiimote2ButtonStateTwo + Environment.NewLine;
                    str += "Wiimote2ButtonStateUp : " + Wiimote2ButtonStateUp + Environment.NewLine;
                    str += "Wiimote2ButtonStateDown : " + Wiimote2ButtonStateDown + Environment.NewLine;
                    str += "Wiimote2ButtonStateLeft : " + Wiimote2ButtonStateLeft + Environment.NewLine;
                    str += "Wiimote2ButtonStateRight : " + Wiimote2ButtonStateRight + Environment.NewLine;
                    str += "Wiimote2RawValuesX : " + Wiimote2RawValuesX + Environment.NewLine;
                    str += "Wiimote2RawValuesY : " + Wiimote2RawValuesY + Environment.NewLine;
                    str += "Wiimote2RawValuesZ : " + Wiimote2RawValuesZ + Environment.NewLine;
                    str += "Wiimote2NunchuckStateRawJoystickX : " + Wiimote2NunchuckStateRawJoystickX + Environment.NewLine;
                    str += "Wiimote2NunchuckStateRawJoystickY : " + Wiimote2NunchuckStateRawJoystickY + Environment.NewLine;
                    str += "Wiimote2NunchuckStateRawValuesX : " + Wiimote2NunchuckStateRawValuesX + Environment.NewLine;
                    str += "Wiimote2NunchuckStateRawValuesY : " + Wiimote2NunchuckStateRawValuesY + Environment.NewLine;
                    str += "Wiimote2NunchuckStateRawValuesZ : " + Wiimote2NunchuckStateRawValuesZ + Environment.NewLine;
                    str += "Wiimote2NunchuckStateC : " + Wiimote2NunchuckStateC + Environment.NewLine;
                    str += "Wiimote2NunchuckStateZ : " + Wiimote2NunchuckStateZ + Environment.NewLine;
                    form1.SetLabel1(str);
                }
            }
        }
        public void InitWiimote()
        {
            calibration1xinit = -aBuffer1[3] + 135f;
            calibration1yinit = -aBuffer1[4] + 135f;
            calibration1zinit = -aBuffer1[5] + 135f;
            stickview1xinit = -aBuffer1[16] + 125f;
            stickview1yinit = -aBuffer1[17] + 125f;
            calibration2xinit = -aBuffer2[3] + 135f;
            calibration2yinit = -aBuffer2[4] + 135f;
            calibration2zinit = -aBuffer2[5] + 135f;
            stickview2xinit = -aBuffer2[16] + 125f;
            stickview2yinit = -aBuffer2[17] + 125f;
        }
        public void ProcessStateLogic1()
        {
            if (irmode == 1)
            {
                Wiimote1IRSensors0X = aBuffer1[6] | ((aBuffer1[8] >> 4) & 0x03) << 8;
                Wiimote1IRSensors0Y = aBuffer1[7] | ((aBuffer1[8] >> 6) & 0x03) << 8;
                Wiimote1IRSensors1X = aBuffer1[9] | ((aBuffer1[8] >> 0) & 0x03) << 8;
                Wiimote1IRSensors1Y = aBuffer1[10] | ((aBuffer1[8] >> 2) & 0x03) << 8;
                Wiimote1IR0found = Wiimote1IRSensors0X > 0f & Wiimote1IRSensors0X <= 1024f & Wiimote1IRSensors0Y > 0f & Wiimote1IRSensors0Y <= 768f;
                Wiimote1IR1found = Wiimote1IRSensors1X > 0f & Wiimote1IRSensors1X <= 1024f & Wiimote1IRSensors1Y > 0f & Wiimote1IRSensors1Y <= 768f;
                if (Wiimote1IR0found)
                {
                    Wiimote1IRSensors0Xcam = Wiimote1IRSensors0X - 512f;
                    Wiimote1IRSensors0Ycam = Wiimote1IRSensors0Y - 384f;
                }
                if (Wiimote1IR1found)
                {
                    Wiimote1IRSensors1Xcam = Wiimote1IRSensors1X - 512f;
                    Wiimote1IRSensors1Ycam = Wiimote1IRSensors1Y - 384f;
                }
                if (Wiimote1IR0found & Wiimote1IR1found)
                {
                    Wiimote1IRSensorsXcam = (Wiimote1IRSensors0Xcam + Wiimote1IRSensors1Xcam) / 2f;
                    Wiimote1IRSensorsYcam = (Wiimote1IRSensors0Ycam + Wiimote1IRSensors1Ycam) / 2f;
                }
                if (Wiimote1IR0found)
                {
                    ir1x0 = 2 * Wiimote1IRSensors0Xcam - Wiimote1IRSensorsXcam;
                    ir1y0 = 2 * Wiimote1IRSensors0Ycam - Wiimote1IRSensorsYcam;
                }
                if (Wiimote1IR1found)
                {
                    ir1x1 = 2 * Wiimote1IRSensors1Xcam - Wiimote1IRSensorsXcam;
                    ir1y1 = 2 * Wiimote1IRSensors1Ycam - Wiimote1IRSensorsYcam;
                }
                ir1xc = ir1x0 + ir1x1;
                ir1yc = ir1y0 + ir1y1;
            }
            else if (irmode == 2)
            {
                Wiimote1IR0found = (aBuffer1[6] | ((aBuffer1[8] >> 4) & 0x03) << 8) > 1 & (aBuffer1[6] | ((aBuffer1[8] >> 4) & 0x03) << 8) < 1023;
                Wiimote1IR1found = (aBuffer1[9] | ((aBuffer1[8] >> 0) & 0x03) << 8) > 1 & (aBuffer1[9] | ((aBuffer1[8] >> 0) & 0x03) << 8) < 1023;
                if (Wiimote1IR0notfound == 0 & Wiimote1IR1found)
                    Wiimote1IR0notfound = 1;
                if (Wiimote1IR0notfound == 1 & !Wiimote1IR0found & !Wiimote1IR1found)
                    Wiimote1IR0notfound = 2;
                if (Wiimote1IR0notfound == 2 & Wiimote1IR0found)
                {
                    Wiimote1IR0notfound = 0;
                    if (!Wiimote1IRswitch)
                        Wiimote1IRswitch = true;
                    else
                        Wiimote1IRswitch = false;
                }
                if (Wiimote1IR0notfound == 0 & Wiimote1IR0found)
                    Wiimote1IR0notfound = 0;
                if (Wiimote1IR0notfound == 0 & !Wiimote1IR0found & !Wiimote1IR1found)
                    Wiimote1IR0notfound = 0;
                if (Wiimote1IR0notfound == 1 & Wiimote1IR0found)
                    Wiimote1IR0notfound = 0;
                if (Wiimote1IR0found)
                {
                    Wiimote1IRSensors0X = (aBuffer1[6] | ((aBuffer1[8] >> 4) & 0x03) << 8);
                    Wiimote1IRSensors0Y = (aBuffer1[7] | ((aBuffer1[8] >> 6) & 0x03) << 8);
                }
                if (Wiimote1IR1found)
                {
                    Wiimote1IRSensors1X = (aBuffer1[9] | ((aBuffer1[8] >> 0) & 0x03) << 8);
                    Wiimote1IRSensors1Y = (aBuffer1[10] | ((aBuffer1[8] >> 2) & 0x03) << 8);
                }
                if (Wiimote1IRswitch)
                {
                    Wiimote1IR0foundcam = Wiimote1IR0found;
                    Wiimote1IR1foundcam = Wiimote1IR1found;
                    Wiimote1IRSensors0Xcam = Wiimote1IRSensors0X - 512f;
                    Wiimote1IRSensors0Ycam = Wiimote1IRSensors0Y - 384f;
                    Wiimote1IRSensors1Xcam = Wiimote1IRSensors1X - 512f;
                    Wiimote1IRSensors1Ycam = Wiimote1IRSensors1Y - 384f;
                }
                else
                {
                    Wiimote1IR1foundcam = Wiimote1IR0found;
                    Wiimote1IR0foundcam = Wiimote1IR1found;
                    Wiimote1IRSensors1Xcam = Wiimote1IRSensors0X - 512f;
                    Wiimote1IRSensors1Ycam = Wiimote1IRSensors0Y - 384f;
                    Wiimote1IRSensors0Xcam = Wiimote1IRSensors1X - 512f;
                    Wiimote1IRSensors0Ycam = Wiimote1IRSensors1Y - 384f;
                }
                if (Wiimote1IR0foundcam & Wiimote1IR1foundcam)
                {
                    ir1x2 = Wiimote1IRSensors0Xcam;
                    ir1y2 = Wiimote1IRSensors0Ycam;
                    ir1x3 = Wiimote1IRSensors1Xcam;
                    ir1y3 = Wiimote1IRSensors1Ycam;
                    Wiimote1IRSensorsXcam = Wiimote1IRSensors0Xcam - Wiimote1IRSensors1Xcam;
                    Wiimote1IRSensorsYcam = Wiimote1IRSensors0Ycam - Wiimote1IRSensors1Ycam;
                }
                if (Wiimote1IR0foundcam & !Wiimote1IR1foundcam)
                {
                    ir1x2 = Wiimote1IRSensors0Xcam;
                    ir1y2 = Wiimote1IRSensors0Ycam;
                    ir1x3 = Wiimote1IRSensors0Xcam - Wiimote1IRSensorsXcam;
                    ir1y3 = Wiimote1IRSensors0Ycam - Wiimote1IRSensorsYcam;
                }
                if (Wiimote1IR1foundcam & !Wiimote1IR0foundcam)
                {
                    ir1x3 = Wiimote1IRSensors1Xcam;
                    ir1y3 = Wiimote1IRSensors1Ycam;
                    ir1x2 = Wiimote1IRSensors1Xcam + Wiimote1IRSensorsXcam;
                    ir1y2 = Wiimote1IRSensors1Ycam + Wiimote1IRSensorsYcam;
                }
                ir1xc = ir1x2 + ir1x3;
                ir1yc = ir1y2 + ir1y3;
            }
            else
            {
                Wiimote1IR0found = (aBuffer1[6] | ((aBuffer1[8] >> 4) & 0x03) << 8) > 1 & (aBuffer1[6] | ((aBuffer1[8] >> 4) & 0x03) << 8) < 1023;
                Wiimote1IR1found = (aBuffer1[9] | ((aBuffer1[8] >> 0) & 0x03) << 8) > 1 & (aBuffer1[9] | ((aBuffer1[8] >> 0) & 0x03) << 8) < 1023;
                if (Wiimote1IR0found & Wiimote1IR1found)
                {
                    Wiimote1IRSensors0X = (aBuffer1[6] | ((aBuffer1[8] >> 4) & 0x03) << 8);
                    Wiimote1IRSensors0Y = (aBuffer1[7] | ((aBuffer1[8] >> 6) & 0x03) << 8);
                    ir1x2 = Wiimote1IRSensors0X - 512f;
                    ir1y2 = Wiimote1IRSensors0Y - 384f;
                    Wiimote1IRSensors1X = (aBuffer1[9] | ((aBuffer1[8] >> 0) & 0x03) << 8);
                    Wiimote1IRSensors1Y = (aBuffer1[10] | ((aBuffer1[8] >> 2) & 0x03) << 8);
                    ir1x3 = Wiimote1IRSensors1X - 512f;
                    ir1y3 = Wiimote1IRSensors1Y - 384f;
                }
                ir1xc = (ir1x2 + ir1x3) / 512f * 1346f;
                ir1yc = (ir1y2 + ir1y3) / 768f * 782f;
            }
            if (Wiimote1IR0found | Wiimote1IR1found)
            {
                vallistir1x.Add(ir1x);
                vallistir1x.RemoveAt(0);
                vallistir1y.Add(ir1y);
                vallistir1y.RemoveAt(0);
                ir1x = ir1xc * (1024f / 1346f);
                ir1y = ir1yc + this.centery >= 0 ? Scale(ir1yc + this.centery, 0f, 782f + this.centery, 0f, 1024f) : Scale(ir1yc + this.centery, -782f + this.centery, 0f, -1024f, 0f);
            }
            else
            {
                if (ir1x - vallistir1x.Average() >= 600f)
                    ir1x = 1024f;
                if (ir1x - vallistir1x.Average() <= -600f)
                    ir1x = -1024f;
                if (ir1y - vallistir1y.Average() >= 200f)
                    ir1y = 1024f;
                if (ir1y - vallistir1y.Average() <= -200f)
                    ir1y = -1024f;
            }
            Wiimote1ButtonStateA = (aBuffer1[2] & 0x08) != 0;
            Wiimote1ButtonStateB = (aBuffer1[2] & 0x04) != 0;
            Wiimote1ButtonStateMinus = (aBuffer1[2] & 0x10) != 0;
            Wiimote1ButtonStateHome = (aBuffer1[2] & 0x80) != 0;
            Wiimote1ButtonStatePlus = (aBuffer1[1] & 0x10) != 0;
            Wiimote1ButtonStateOne = (aBuffer1[2] & 0x02) != 0;
            Wiimote1ButtonStateTwo = (aBuffer1[2] & 0x01) != 0;
            Wiimote1ButtonStateUp = (aBuffer1[1] & 0x08) != 0;
            Wiimote1ButtonStateDown = (aBuffer1[1] & 0x04) != 0;
            Wiimote1ButtonStateLeft = (aBuffer1[1] & 0x01) != 0;
            Wiimote1ButtonStateRight = (aBuffer1[1] & 0x02) != 0;
            Wiimote1NunchuckStateRawJoystickX = aBuffer1[16] - 125f + stickview1xinit;
            Wiimote1NunchuckStateRawJoystickY = aBuffer1[17] - 125f + stickview1yinit;
            Wiimote1NunchuckStateC = (aBuffer1[21] & 0x02) == 0;
            Wiimote1NunchuckStateZ = (aBuffer1[21] & 0x01) == 0;
            Wiimote1RawValuesX = aBuffer1[3] - 135f + calibration1xinit;
            Wiimote1RawValuesY = aBuffer1[4] - 135f + calibration1yinit;
            Wiimote1RawValuesZ = aBuffer1[5] - 135f + calibration1zinit;
            Wiimote1NunchuckStateRawValuesX = aBuffer1[18] - 125f;
            Wiimote1NunchuckStateRawValuesY = aBuffer1[19] - 125f;
            Wiimote1NunchuckStateRawValuesZ = aBuffer1[20] - 125f;
        }
        public void ProcessStateLogic2()
        {
            if (irmode == 1)
            {
                Wiimote2IRSensors0X = aBuffer2[6] | ((aBuffer2[8] >> 4) & 0x03) << 8;
                Wiimote2IRSensors0Y = aBuffer2[7] | ((aBuffer2[8] >> 6) & 0x03) << 8;
                Wiimote2IRSensors1X = aBuffer2[9] | ((aBuffer2[8] >> 0) & 0x03) << 8;
                Wiimote2IRSensors1Y = aBuffer2[10] | ((aBuffer2[8] >> 2) & 0x03) << 8;
                Wiimote2IR0found = Wiimote2IRSensors0X > 0f & Wiimote2IRSensors0X <= 1024f & Wiimote2IRSensors0Y > 0f & Wiimote2IRSensors0Y <= 768f;
                Wiimote2IR1found = Wiimote2IRSensors1X > 0f & Wiimote2IRSensors1X <= 1024f & Wiimote2IRSensors1Y > 0f & Wiimote2IRSensors1Y <= 768f;
                if (Wiimote2IR0found)
                {
                    Wiimote2IRSensors0Xcam = Wiimote2IRSensors0X - 512f;
                    Wiimote2IRSensors0Ycam = Wiimote2IRSensors0Y - 384f;
                }
                if (Wiimote2IR1found)
                {
                    Wiimote2IRSensors1Xcam = Wiimote2IRSensors1X - 512f;
                    Wiimote2IRSensors1Ycam = Wiimote2IRSensors1Y - 384f;
                }
                if (Wiimote2IR0found & Wiimote2IR1found)
                {
                    Wiimote2IRSensorsXcam = (Wiimote2IRSensors0Xcam + Wiimote2IRSensors1Xcam) / 2f;
                    Wiimote2IRSensorsYcam = (Wiimote2IRSensors0Ycam + Wiimote2IRSensors1Ycam) / 2f;
                }
                if (Wiimote2IR0found)
                {
                    ir2x0 = 2 * Wiimote2IRSensors0Xcam - Wiimote2IRSensorsXcam;
                    ir2y0 = 2 * Wiimote2IRSensors0Ycam - Wiimote2IRSensorsYcam;
                }
                if (Wiimote2IR1found)
                {
                    ir2x1 = 2 * Wiimote2IRSensors1Xcam - Wiimote2IRSensorsXcam;
                    ir2y1 = 2 * Wiimote2IRSensors1Ycam - Wiimote2IRSensorsYcam;
                }
                ir2xc = ir2x0 + ir2x1;
                ir2yc = ir2y0 + ir2y1;
            }
            else if (irmode == 2)
            {
                Wiimote2IR0found = (aBuffer2[6] | ((aBuffer2[8] >> 4) & 0x03) << 8) > 1 & (aBuffer2[6] | ((aBuffer2[8] >> 4) & 0x03) << 8) < 1023;
                Wiimote2IR1found = (aBuffer2[9] | ((aBuffer2[8] >> 0) & 0x03) << 8) > 1 & (aBuffer2[9] | ((aBuffer2[8] >> 0) & 0x03) << 8) < 1023;
                if (Wiimote2IR0notfound == 0 & Wiimote2IR1found)
                    Wiimote2IR0notfound = 1;
                if (Wiimote2IR0notfound == 1 & !Wiimote2IR0found & !Wiimote2IR1found)
                    Wiimote2IR0notfound = 2;
                if (Wiimote2IR0notfound == 2 & Wiimote2IR0found)
                {
                    Wiimote2IR0notfound = 0;
                    if (!Wiimote2IRswitch)
                        Wiimote2IRswitch = true;
                    else
                        Wiimote2IRswitch = false;
                }
                if (Wiimote2IR0notfound == 0 & Wiimote2IR0found)
                    Wiimote2IR0notfound = 0;
                if (Wiimote2IR0notfound == 0 & !Wiimote2IR0found & !Wiimote2IR1found)
                    Wiimote2IR0notfound = 0;
                if (Wiimote2IR0notfound == 1 & Wiimote2IR0found)
                    Wiimote2IR0notfound = 0;
                if (Wiimote2IR0found)
                {
                    Wiimote2IRSensors0X = (aBuffer2[6] | ((aBuffer2[8] >> 4) & 0x03) << 8);
                    Wiimote2IRSensors0Y = (aBuffer2[7] | ((aBuffer2[8] >> 6) & 0x03) << 8);
                }
                if (Wiimote2IR1found)
                {
                    Wiimote2IRSensors1X = (aBuffer2[9] | ((aBuffer2[8] >> 0) & 0x03) << 8);
                    Wiimote2IRSensors1Y = (aBuffer2[10] | ((aBuffer2[8] >> 2) & 0x03) << 8);
                }
                if (Wiimote2IRswitch)
                {
                    Wiimote2IR0foundcam = Wiimote2IR0found;
                    Wiimote2IR1foundcam = Wiimote2IR1found;
                    Wiimote2IRSensors0Xcam = Wiimote2IRSensors0X - 512f;
                    Wiimote2IRSensors0Ycam = Wiimote2IRSensors0Y - 384f;
                    Wiimote2IRSensors1Xcam = Wiimote2IRSensors1X - 512f;
                    Wiimote2IRSensors1Ycam = Wiimote2IRSensors1Y - 384f;
                }
                else
                {
                    Wiimote2IR1foundcam = Wiimote2IR0found;
                    Wiimote2IR0foundcam = Wiimote2IR1found;
                    Wiimote2IRSensors1Xcam = Wiimote2IRSensors0X - 512f;
                    Wiimote2IRSensors1Ycam = Wiimote2IRSensors0Y - 384f;
                    Wiimote2IRSensors0Xcam = Wiimote2IRSensors1X - 512f;
                    Wiimote2IRSensors0Ycam = Wiimote2IRSensors1Y - 384f;
                }
                if (Wiimote2IR0foundcam & Wiimote2IR1foundcam)
                {
                    ir2x2 = Wiimote2IRSensors0Xcam;
                    ir2y2 = Wiimote2IRSensors0Ycam;
                    ir2x3 = Wiimote2IRSensors1Xcam;
                    ir2y3 = Wiimote2IRSensors1Ycam;
                    Wiimote2IRSensorsXcam = Wiimote2IRSensors0Xcam - Wiimote2IRSensors1Xcam;
                    Wiimote2IRSensorsYcam = Wiimote2IRSensors0Ycam - Wiimote2IRSensors1Ycam;
                }
                if (Wiimote2IR0foundcam & !Wiimote2IR1foundcam)
                {
                    ir2x2 = Wiimote2IRSensors0Xcam;
                    ir2y2 = Wiimote2IRSensors0Ycam;
                    ir2x3 = Wiimote2IRSensors0Xcam - Wiimote2IRSensorsXcam;
                    ir2y3 = Wiimote2IRSensors0Ycam - Wiimote2IRSensorsYcam;
                }
                if (Wiimote2IR1foundcam & !Wiimote2IR0foundcam)
                {
                    ir2x3 = Wiimote2IRSensors1Xcam;
                    ir2y3 = Wiimote2IRSensors1Ycam;
                    ir2x2 = Wiimote2IRSensors1Xcam + Wiimote2IRSensorsXcam;
                    ir2y2 = Wiimote2IRSensors1Ycam + Wiimote2IRSensorsYcam;
                }
                ir2xc = ir2x2 + ir2x3;
                ir2yc = ir2y2 + ir2y3;
            }
            else
            {
                Wiimote2IR0found = (aBuffer2[6] | ((aBuffer2[8] >> 4) & 0x03) << 8) > 1 & (aBuffer2[6] | ((aBuffer2[8] >> 4) & 0x03) << 8) < 1023;
                Wiimote2IR1found = (aBuffer2[9] | ((aBuffer2[8] >> 0) & 0x03) << 8) > 1 & (aBuffer2[9] | ((aBuffer2[8] >> 0) & 0x03) << 8) < 1023;
                if (Wiimote2IR0found & Wiimote2IR1found)
                {
                    Wiimote2IRSensors0X = (aBuffer2[6] | ((aBuffer2[8] >> 4) & 0x03) << 8);
                    Wiimote2IRSensors0Y = (aBuffer2[7] | ((aBuffer2[8] >> 6) & 0x03) << 8);
                    ir2x2 = Wiimote2IRSensors0X - 512f;
                    ir2y2 = Wiimote2IRSensors0Y - 384f;
                    Wiimote2IRSensors1X = (aBuffer2[9] | ((aBuffer2[8] >> 0) & 0x03) << 8);
                    Wiimote2IRSensors1Y = (aBuffer2[10] | ((aBuffer2[8] >> 2) & 0x03) << 8);
                    ir2x3 = Wiimote2IRSensors1X - 512f;
                    ir2y3 = Wiimote2IRSensors1Y - 384f;
                }
                ir2xc = (ir2x2 + ir2x3) / 512f * 1346f;
                ir2yc = (ir2y2 + ir2y3) / 768f * 782f;
            }
            if (Wiimote2IR0found | Wiimote2IR1found)
            {
                vallistir2x.Add(ir2x);
                vallistir2x.RemoveAt(0);
                vallistir2y.Add(ir2y);
                vallistir2y.RemoveAt(0);
                ir2x = ir2xc * (1024f / 1346f);
                ir2y = ir2yc + this.centery >= 0 ? Scale(ir2yc + this.centery, 0f, 782f + this.centery, 0f, 1024f) : Scale(ir2yc + this.centery, -782f + this.centery, 0f, -1024f, 0f);
            }
            else
            {
                if (ir2x - vallistir2x.Average() >= 600f)
                    ir2x = 1024f;
                if (ir2x - vallistir2x.Average() <= -600f)
                    ir2x = -1024f;
                if (ir2y - vallistir2y.Average() >= 200f)
                    ir2y = 1024f;
                if (ir2y - vallistir2y.Average() <= -200f)
                    ir2y = -1024f;
            }
            Wiimote2ButtonStateA = (aBuffer2[2] & 0x08) != 0;
            Wiimote2ButtonStateB = (aBuffer2[2] & 0x04) != 0;
            Wiimote2ButtonStateMinus = (aBuffer2[2] & 0x10) != 0;
            Wiimote2ButtonStateHome = (aBuffer2[2] & 0x80) != 0;
            Wiimote2ButtonStatePlus = (aBuffer2[1] & 0x10) != 0;
            Wiimote2ButtonStateOne = (aBuffer2[2] & 0x02) != 0;
            Wiimote2ButtonStateTwo = (aBuffer2[2] & 0x01) != 0;
            Wiimote2ButtonStateUp = (aBuffer2[1] & 0x08) != 0;
            Wiimote2ButtonStateDown = (aBuffer2[1] & 0x04) != 0;
            Wiimote2ButtonStateLeft = (aBuffer2[1] & 0x01) != 0;
            Wiimote2ButtonStateRight = (aBuffer2[1] & 0x02) != 0;
            Wiimote2NunchuckStateRawJoystickX = aBuffer2[16] - 125f + stickview2xinit;
            Wiimote2NunchuckStateRawJoystickY = aBuffer2[17] - 125f + stickview2yinit;
            Wiimote2NunchuckStateC = (aBuffer2[21] & 0x02) == 0;
            Wiimote2NunchuckStateZ = (aBuffer2[21] & 0x01) == 0;
            Wiimote2RawValuesX = aBuffer2[3] - 135f + calibration2xinit;
            Wiimote2RawValuesY = aBuffer2[4] - 135f + calibration2yinit;
            Wiimote2RawValuesZ = aBuffer2[5] - 135f + calibration2zinit;
            Wiimote2NunchuckStateRawValuesX = aBuffer2[18] - 125f;
            Wiimote2NunchuckStateRawValuesY = aBuffer2[19] - 125f;
            Wiimote2NunchuckStateRawValuesZ = aBuffer2[20] - 125f;
        }
        private double Scale(double value, double min, double max, double minScale, double maxScale)
        {
            double scaled = minScale + (double)(value - min) / (max - min) * (maxScale - minScale);
            return scaled;
        }
        public void Reconnection1()
        {
            if (reconnectingwiimote1count == 0)
                reconnectingwiimote1bool = true;
            reconnectingwiimote1count++;
            if (reconnectingwiimote1count >= 15f)
            {
                if (reconnectingwiimote1bool)
                {
                    Wiimote1Found(path1);
                    reconnectingwiimote1count = -15f;
                }
                else
                    reconnectingwiimote1count = 0;
            }
        }
        public void Reconnection2()
        {
            if (reconnectingwiimote2count == 0)
                reconnectingwiimote2bool = true;
            reconnectingwiimote2count++;
            if (reconnectingwiimote2count >= 15f)
            {
                if (reconnectingwiimote2bool)
                {
                    Wiimote2Found(path2);
                    reconnectingwiimote2count = -15f;
                }
                else
                    reconnectingwiimote2count = 0;
            }
        }
        private const string vendor_id = "57e", vendor_id_ = "057e", product_r1 = "0330", product_r2 = "0306", product_l = "2006";
        private enum EFileAttributes : uint
        {
            Overlapped = 0x40000000,
            Normal = 0x80
        };
        struct SP_DEVICE_INTERFACE_DATA
        {
            public int cbSize;
            public Guid InterfaceClassGuid;
            public int Flags;
            public IntPtr RESERVED;
        }
        struct SP_DEVICE_INTERFACE_DETAIL_DATA
        {
            public UInt32 cbSize;
            [System.Runtime.InteropServices.MarshalAs(System.Runtime.InteropServices.UnmanagedType.ByValTStr, SizeConst = 256)]
            public string DevicePath;
        }
        public bool ScanWiimote(int irmode, double centery)
        {
            this.irmode = irmode;
            this.centery = centery;
            do
                Thread.Sleep(1);
            while (!wiimotesconnect());
            ISWIIMOTE1 = false;
            ISWIIMOTE2 = false;
            int index = 0;
            Guid guid;
            HidD_GetHidGuid(out guid);
            IntPtr hDevInfo = SetupDiGetClassDevs(ref guid, null, new IntPtr(), 0x00000010);
            SP_DEVICE_INTERFACE_DATA diData = new SP_DEVICE_INTERFACE_DATA();
            diData.cbSize = Marshal.SizeOf(diData);
            while (SetupDiEnumDeviceInterfaces(hDevInfo, new IntPtr(), ref guid, index, ref diData))
            {
                UInt32 size;
                SetupDiGetDeviceInterfaceDetail(hDevInfo, ref diData, new IntPtr(), 0, out size, new IntPtr());
                SP_DEVICE_INTERFACE_DETAIL_DATA diDetail = new SP_DEVICE_INTERFACE_DETAIL_DATA();
                diDetail.cbSize = 5;
                if (SetupDiGetDeviceInterfaceDetail(hDevInfo, ref diData, ref diDetail, size, out size, new IntPtr()))
                {
                    if ((diDetail.DevicePath.Contains(vendor_id) | diDetail.DevicePath.Contains(vendor_id_)) & (diDetail.DevicePath.Contains(product_r1) | diDetail.DevicePath.Contains(product_r2)))
                    {
                        if (ISWIIMOTE1)
                        {
                            path2 = diDetail.DevicePath;
                            Wiimote2Found(path2);
                            Wiimote2Found(path2);
                            Wiimote2Found(path2);
                            ISWIIMOTE2 = true;
                        }
                        if (!ISWIIMOTE1)
                        {
                            path1 = diDetail.DevicePath;
                            Wiimote1Found(path1);
                            Wiimote1Found(path1);
                            Wiimote1Found(path1);
                            ISWIIMOTE1 = true;
                        }
                        if (ISWIIMOTE1 & ISWIIMOTE2)
                            return true;
                    }
                }
                index++;
            }
            return false;
        }
        public void Wiimote1Found(string path)
        {
            do
            {
                handle1 = CreateFile(path, FileAccess.ReadWrite, FileShare.ReadWrite, IntPtr.Zero, FileMode.Open, (uint)EFileAttributes.Overlapped, IntPtr.Zero);
                Write1Data(handle1, IR, (int)REGISTER_IR, new byte[] { 0x08 }, 1);
                Write1Data(handle1, Type, (int)REGISTER_EXTENSION_INIT_1, new byte[] { 0x55 }, 1);
                Write1Data(handle1, Type, (int)REGISTER_EXTENSION_INIT_2, new byte[] { 0x00 }, 1);
                Write1Data(handle1, Type, (int)REGISTER_MOTIONPLUS_INIT, new byte[] { 0x04 }, 1);
                Read1Data(handle1, 0x0016, 7);
                Read1Data(handle1, (int)REGISTER_EXTENSION_TYPE, 6);
                Read1Data(handle1, (int)REGISTER_EXTENSION_CALIBRATION, 16);
                Read1Data(handle1, (int)REGISTER_EXTENSION_CALIBRATION, 32);
            }
            while (handle1.IsInvalid);
            mStream1 = new FileStream(handle1, FileAccess.Read, 22, true);
        }
        public void Read1Data(SafeFileHandle _hFile, int address, short size)
        {
            mBuff1[0] = (byte)ReadMemory;
            mBuff1[1] = (byte)((address & 0xff000000) >> 24);
            mBuff1[2] = (byte)((address & 0x00ff0000) >> 16);
            mBuff1[3] = (byte)((address & 0x0000ff00) >> 8);
            mBuff1[4] = (byte)(address & 0x000000ff);
            mBuff1[5] = (byte)((size & 0xff00) >> 8);
            mBuff1[6] = (byte)(size & 0xff);
            HidD_SetOutputReport(_hFile.DangerousGetHandle(), mBuff1, 22);
        }
        public void Write1Data(SafeFileHandle _hFile, byte mbuff1, int address, byte[] buff1, short size)
        {
            mBuff1[0] = (byte)mbuff1;
            mBuff1[1] = (byte)(0x04);
            mBuff1[2] = (byte)IRExtensionAccel;
            System.Array.Copy(buff1, 0, mBuff1, 3, 1);
            HidD_SetOutputReport(_hFile.DangerousGetHandle(), mBuff1, 22);
            mBuff1[0] = (byte)WriteMemory;
            mBuff1[1] = (byte)(((address & 0xff000000) >> 24));
            mBuff1[2] = (byte)((address & 0x00ff0000) >> 16);
            mBuff1[3] = (byte)((address & 0x0000ff00) >> 8);
            mBuff1[4] = (byte)((address & 0x000000ff) >> 0);
            mBuff1[5] = (byte)size;
            System.Array.Copy(buff1, 0, mBuff1, 6, 1);
            HidD_SetOutputReport(_hFile.DangerousGetHandle(), mBuff1, 22);
        }
        public void Wiimote2Found(string path)
        {
            do
            {
                handle2 = CreateFile(path, FileAccess.ReadWrite, FileShare.ReadWrite, IntPtr.Zero, FileMode.Open, (uint)EFileAttributes.Overlapped, IntPtr.Zero);
                Write2Data(handle2, IR, (int)REGISTER_IR, new byte[] { 0x08 }, 1);
                Write2Data(handle2, Type, (int)REGISTER_EXTENSION_INIT_1, new byte[] { 0x55 }, 1);
                Write2Data(handle2, Type, (int)REGISTER_EXTENSION_INIT_2, new byte[] { 0x00 }, 1);
                Write2Data(handle2, Type, (int)REGISTER_MOTIONPLUS_INIT, new byte[] { 0x04 }, 1);
                Read2Data(handle2, 0x0016, 7);
                Read2Data(handle2, (int)REGISTER_EXTENSION_TYPE, 6);
                Read2Data(handle2, (int)REGISTER_EXTENSION_CALIBRATION, 16);
                Read2Data(handle2, (int)REGISTER_EXTENSION_CALIBRATION, 32);
            }
            while (handle2.IsInvalid);
            mStream2 = new FileStream(handle2, FileAccess.Read, 22, true);
        }
        public void Read2Data(SafeFileHandle _hFile, int address, short size)
        {
            mBuff2[0] = (byte)ReadMemory;
            mBuff2[1] = (byte)((address & 0xff000000) >> 24);
            mBuff2[2] = (byte)((address & 0x00ff0000) >> 16);
            mBuff2[3] = (byte)((address & 0x0000ff00) >> 8);
            mBuff2[4] = (byte)(address & 0x000000ff);
            mBuff2[5] = (byte)((size & 0xff00) >> 8);
            mBuff2[6] = (byte)(size & 0xff);
            HidD_SetOutputReport(_hFile.DangerousGetHandle(), mBuff2, 22);
        }
        public void Write2Data(SafeFileHandle _hFile, byte mbuff2, int address, byte[] buff2, short size)
        {
            mBuff2[0] = (byte)mbuff2;
            mBuff2[1] = (byte)(0x04);
            mBuff2[2] = (byte)IRExtensionAccel;
            System.Array.Copy(buff2, 0, mBuff2, 3, 1);
            HidD_SetOutputReport(_hFile.DangerousGetHandle(), mBuff2, 22);
            mBuff2[0] = (byte)WriteMemory;
            mBuff2[1] = (byte)(((address & 0xff000000) >> 24));
            mBuff2[2] = (byte)((address & 0x00ff0000) >> 16);
            mBuff2[3] = (byte)((address & 0x0000ff00) >> 8);
            mBuff2[4] = (byte)((address & 0x000000ff) >> 0);
            mBuff2[5] = (byte)size;
            System.Array.Copy(buff2, 0, mBuff2, 6, 1);
            HidD_SetOutputReport(_hFile.DangerousGetHandle(), mBuff2, 22);
        }
    }
}