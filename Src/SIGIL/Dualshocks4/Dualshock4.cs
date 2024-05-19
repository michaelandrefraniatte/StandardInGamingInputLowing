using System;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using System.Numerics;
using Dualshocks4;
using System.Threading;
using System.IO;
using Microsoft.Win32.SafeHandles;
using System.Collections.Generic;
using System.Diagnostics;
using Valuechanges;

namespace DualShocks4API
{
    public class DualShock4
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
        [DllImport("winmm.dll", EntryPoint = "timeBeginPeriod")]
        private static extern uint TimeBeginPeriod(uint ms);
        [DllImport("winmm.dll", EntryPoint = "timeEndPeriod")]
        private static extern uint TimeEndPeriod(uint ms);
        [DllImport("ntdll.dll", EntryPoint = "NtSetTimerResolution")]
        private static extern void NtSetTimerResolution(uint DesiredResolution, bool SetResolution, ref uint CurrentResolution);
        private static uint CurrentResolution = 0;
        private byte miscByte;
        private byte btnBlock1, btnBlock2, btnBlock3;
        private byte[] ds4data = new byte[64];
        public bool PS4ControllerButtonCrossPressed;
        public bool PS4ControllerButtonCirclePressed;
        public bool PS4ControllerButtonSquarePressed;
        public bool PS4ControllerButtonTrianglePressed;
        public bool PS4ControllerButtonDPadUpPressed;
        public bool PS4ControllerButtonDPadRightPressed;
        public bool PS4ControllerButtonDPadDownPressed;
        public bool PS4ControllerButtonDPadLeftPressed;
        public bool PS4ControllerButtonL1Pressed;
        public bool PS4ControllerButtonR1Pressed;
        public bool PS4ControllerButtonL2Pressed;
        public bool PS4ControllerButtonR2Pressed;
        public bool PS4ControllerButtonL3Pressed;
        public bool PS4ControllerButtonR3Pressed;
        public bool PS4ControllerButtonCreatePressed;
        public bool PS4ControllerButtonMenuPressed;
        public bool PS4ControllerButtonLogoPressed;
        public bool PS4ControllerButtonTouchpadPressed;
        public bool PS4ControllerButtonMicPressed;
        public bool PS4ControllerTouchOn;
        public double PS4ControllerLeftStickX, PS4ControllerLeftStickY, PS4ControllerRightStickX, PS4ControllerRightStickY, PS4ControllerRightTriggerPosition, PS4ControllerLeftTriggerPosition, PS4ControllerTouchX, PS4ControllerTouchY;
        public bool PS4ControllerAccelCenter;
        public double PS4ControllerAccelX, PS4ControllerAccelY, PS4ControllerGyroX, PS4ControllerGyroY;
        private Vector3 gyr_gPS4 = new Vector3();
        private Vector3 acc_gPS4 = new Vector3();
        private Vector3 InitDirectAnglesPS4, DirectAnglesPS4;
        private SafeFileHandle handle = null, handleunshared = null;
        private FileStream mStream;
        private int number = 0;
        private bool reconnectingbool;
        private double reconnectingcount;
        private bool isvalidhandle = false;
        private string path;
        private bool running, formvisible, littleendian;
        private static List<string> paths = new List<string>();
        private static List<FileStream> mStreams = new List<FileStream>();
        private Form1 form1 = new Form1();
        private Stopwatch PollingRate;
        private double pollingrateperm = 0, pollingratetemp = 0, pollingratedisplay = 0, pollingrate;
        private string inputdelaybutton = "", inputdelay = "", inputdelaytemp = "";
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
        public DualShock4()
        {
            TimeBeginPeriod(1);
            NtSetTimerResolution(1, true, ref CurrentResolution);
            littleendian = BitConverter.IsLittleEndian;
            running = true;
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
        public void Close()
        {
            running = false;
            Thread.Sleep(100);
            handleunshared.Close();
            handleunshared.Dispose();
            mStream.Close();
            mStream.Dispose();
            handle.Close();
            handle.Dispose();
        }
        private void ProcessStateLogic()
        {
            LeftAnalogStick = ReadAnalogStick(ds4data[1], ds4data[2]);
            RightAnalogStick = ReadAnalogStick(ds4data[3], ds4data[4]);
            L2 = ds4data[8].ToUnsignedFloat();
            R2 = ds4data[9].ToUnsignedFloat();
            btnBlock1 = ds4data[5];
            btnBlock2 = ds4data[6];
            btnBlock3 = ds4data[7];
            SquareButton = btnBlock1.HasFlag(0x10);
            CrossButton = btnBlock1.HasFlag(0x20);
            CircleButton = btnBlock1.HasFlag(0x40);
            TriangleButton = btnBlock1.HasFlag(0x80);
            DPadUpButton = ReadDPadButton(btnBlock1, 0, 1, 7);
            DPadRightButton = ReadDPadButton(btnBlock1, 1, 2, 3);
            DPadDownButton = ReadDPadButton(btnBlock1, 3, 4, 5);
            DPadLeftButton = ReadDPadButton(btnBlock1, 5, 6, 7);
            L1Button = btnBlock2.HasFlag(0x01);
            R1Button = btnBlock2.HasFlag(0x02);
            L2Button = btnBlock2.HasFlag(0x04);
            R2Button = btnBlock2.HasFlag(0x08);
            CreateButton = btnBlock2.HasFlag(0x10);
            MenuButton = btnBlock2.HasFlag(0x20);
            L3Button = btnBlock2.HasFlag(0x40);
            R3Button = btnBlock2.HasFlag(0x80);
            LogoButton = btnBlock3.HasFlag(0x01);
            TouchpadButton = btnBlock3.HasFlag(0x02);
            MicButton = ds4data[10].HasFlag(0x04);
            Touchpad1 = ReadTouchpad(new byte[] { ds4data[35], ds4data[36], ds4data[37], ds4data[38] });
            Touchpad2 = ReadTouchpad(new byte[] { ds4data[39], ds4data[40], ds4data[41], ds4data[42] });
            Gyro = -ReadAccelAxes(
                new byte[] { ds4data[13], ds4data[14] },
                new byte[] { ds4data[15], ds4data[16] },
                new byte[] { ds4data[17], ds4data[18] }
            );
            Accelerometer = ReadAccelAxes(
                new byte[] { ds4data[19], ds4data[20] },
                new byte[] { ds4data[21], ds4data[22] },
                new byte[] { ds4data[23], ds4data[24] }
            );
            miscByte = ds4data[30];
            IsHeadphoneConnected = miscByte.HasFlag(0x01);
            PS4ControllerLeftStickX = LeftAnalogStick.X;
            PS4ControllerLeftStickY = LeftAnalogStick.Y;
            PS4ControllerRightStickX = -RightAnalogStick.X;
            PS4ControllerRightStickY = -RightAnalogStick.Y;
            PS4ControllerLeftTriggerPosition = L2;
            PS4ControllerRightTriggerPosition = R2;
            PS4ControllerTouchX = Touchpad1.X;
            PS4ControllerTouchY = Touchpad1.Y;
            PS4ControllerTouchOn = Touchpad1.IsDown;
            gyr_gPS4.X = Gyro.Z;
            gyr_gPS4.Y = -Gyro.X;
            gyr_gPS4.Z = -Gyro.Y;
            PS4ControllerGyroX = gyr_gPS4.Z;
            PS4ControllerGyroY = gyr_gPS4.Y;
            acc_gPS4 = new Vector3(Accelerometer.X, Accelerometer.Z, Accelerometer.Y);
            PS4ControllerAccelCenter = MenuButton;
            DirectAnglesPS4 = acc_gPS4 - InitDirectAnglesPS4;
            PS4ControllerAccelX = -(DirectAnglesPS4.Y + DirectAnglesPS4.Z) / 6f;
            PS4ControllerAccelY = DirectAnglesPS4.X / 6f;
            PS4ControllerButtonCrossPressed = CrossButton;
            PS4ControllerButtonCirclePressed = CircleButton;
            PS4ControllerButtonSquarePressed = SquareButton;
            PS4ControllerButtonTrianglePressed = TriangleButton;
            PS4ControllerButtonDPadUpPressed = DPadUpButton;
            PS4ControllerButtonDPadRightPressed = DPadRightButton;
            PS4ControllerButtonDPadDownPressed = DPadDownButton;
            PS4ControllerButtonDPadLeftPressed = DPadLeftButton;
            PS4ControllerButtonL1Pressed = L1Button;
            PS4ControllerButtonR1Pressed = R1Button;
            PS4ControllerButtonL2Pressed = L2Button;
            PS4ControllerButtonR2Pressed = R2Button;
            PS4ControllerButtonL3Pressed = L3Button;
            PS4ControllerButtonR3Pressed = R3Button;
            PS4ControllerButtonCreatePressed = CreateButton;
            PS4ControllerButtonMenuPressed = MenuButton;
            PS4ControllerButtonLogoPressed = LogoButton;
            PS4ControllerButtonTouchpadPressed = TouchpadButton;
            PS4ControllerButtonMicPressed = MicButton;
        }
        public void Init()
        {
            InitDirectAnglesPS4 = acc_gPS4;
        }
        private void taskD()
        {
            for (; ; )
            {
                if (!running)
                    break;
                try
                {
                    mStream.Read(ds4data, 0, ds4data.Length);
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
                    string str = "PS4ControllerLeftStickX : " + PS4ControllerLeftStickX + Environment.NewLine;
                    str += "PS4ControllerLeftStickY : " + PS4ControllerLeftStickY + Environment.NewLine;
                    str += "PS4ControllerRightStickX : " + PS4ControllerRightStickX + Environment.NewLine;
                    str += "PS4ControllerRightStickY : " + PS4ControllerRightStickY + Environment.NewLine;
                    str += "PS4ControllerLeftTriggerPosition : " + PS4ControllerLeftTriggerPosition + Environment.NewLine;
                    str += "PS4ControllerRightTriggerPosition : " + PS4ControllerRightTriggerPosition + Environment.NewLine;
                    str += "PS4ControllerTouchX : " + PS4ControllerTouchX + Environment.NewLine;
                    str += "PS4ControllerTouchY : " + PS4ControllerTouchY + Environment.NewLine;
                    str += "PS4ControllerTouchOn : " + PS4ControllerTouchOn + Environment.NewLine;
                    str += "PS4ControllerGyroX : " + PS4ControllerGyroX + Environment.NewLine;
                    str += "PS4ControllerGyroY : " + PS4ControllerGyroY + Environment.NewLine;
                    str += "PS4ControllerAccelX : " + PS4ControllerAccelX + Environment.NewLine;
                    str += "PS4ControllerAccelY : " + PS4ControllerAccelY + Environment.NewLine;
                    str += "PS4ControllerButtonCrossPressed : " + PS4ControllerButtonCrossPressed + Environment.NewLine;
                    str += "PS4ControllerButtonCirclePressed : " + PS4ControllerButtonCirclePressed + Environment.NewLine;
                    str += "PS4ControllerButtonSquarePressed : " + PS4ControllerButtonSquarePressed + Environment.NewLine;
                    str += "PS4ControllerButtonTrianglePressed : " + PS4ControllerButtonTrianglePressed + Environment.NewLine;
                    str += "PS4ControllerButtonDPadUpPressed : " + PS4ControllerButtonDPadUpPressed + Environment.NewLine;
                    str += "PS4ControllerButtonDPadRightPressed : " + PS4ControllerButtonDPadRightPressed + Environment.NewLine;
                    str += "PS4ControllerButtonDPadDownPressed : " + PS4ControllerButtonDPadDownPressed + Environment.NewLine;
                    str += "PS4ControllerButtonDPadLeftPressed : " + PS4ControllerButtonDPadLeftPressed + Environment.NewLine;
                    str += "PS4ControllerButtonL1Pressed : " + PS4ControllerButtonL1Pressed + Environment.NewLine;
                    str += "PS4ControllerButtonR1Pressed : " + PS4ControllerButtonR1Pressed + Environment.NewLine;
                    str += "PS4ControllerButtonL2Pressed : " + PS4ControllerButtonL2Pressed + Environment.NewLine;
                    str += "PS4ControllerButtonR2Pressed : " + PS4ControllerButtonR2Pressed + Environment.NewLine;
                    str += "PS4ControllerButtonL3Pressed : " + PS4ControllerButtonL3Pressed + Environment.NewLine;
                    str += "PS4ControllerButtonR3Pressed : " + PS4ControllerButtonR3Pressed + Environment.NewLine;
                    str += "PS4ControllerButtonCreatePressed : " + PS4ControllerButtonCreatePressed + Environment.NewLine;
                    str += "PS4ControllerButtonMenuPressed : " + PS4ControllerButtonMenuPressed + Environment.NewLine;
                    str += "PS4ControllerButtonLogoPressed : " + PS4ControllerButtonLogoPressed + Environment.NewLine;
                    str += "PS4ControllerButtonTouchpadPressed : " + PS4ControllerButtonTouchpadPressed + Environment.NewLine;
                    str += "PS4ControllerButtonMicPressed : " + PS4ControllerButtonMicPressed + Environment.NewLine;
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
                    if (wd[0] == 1)
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
                    if (wu[0] == 1)
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
        private void taskP()
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
            Task.Run(() => taskD());
            Task.Run(() => taskP());
        }
        private Vec2 ReadAnalogStick(byte x, byte y)
        {
            float x1 = x.ToSignedFloat();
            float y1 = -y.ToSignedFloat();
            return new Vec2
            {
                X = Math.Abs(x1) >= 0f ? x1 : 0,
                Y = Math.Abs(y1) >= 0f ? y1 : 0
            };
        }
        private bool ReadDPadButton(byte b, int v1, int v2, int v3)
        {
            int val = b & 0x0F;
            return val == v1 || val == v2 || val == v3;
        }
        private DualShock4Touch ReadTouchpad(byte[] bytes)
        {
            if (!littleendian)
            {
                bytes = bytes.Reverse().ToArray();
            }
            uint raw = BitConverter.ToUInt32(bytes, 0);
            return new DualShock4Touch
            {
                X = (raw & 0x000FFF00) >> 8,
                Y = (raw & 0xFFF00000) >> 20,
                IsDown = (raw & 128) == 0,
                Id = bytes[0]
            };
        }
        private Vec3 ReadAccelAxes(byte[] x, byte[] y, byte[] z)
        {
            if (!littleendian)
            {
                x = x.Reverse().ToArray();
                y = y.Reverse().ToArray();
                z = z.Reverse().ToArray();
            }
            return new Vec3
            {
                X = -BitConverter.ToInt16(x, 0),
                Y = BitConverter.ToInt16(y, 0),
                Z = BitConverter.ToInt16(z, 0)
            };
        }
        private Vec2 LeftAnalogStick { get; set; }
        private Vec2 RightAnalogStick { get; set; }
        private float L2 { get; set; }
        private float R2 { get; set; }
        private bool SquareButton { get; set; }
        private bool CrossButton { get; set; }
        private bool CircleButton { get; set; }
        private bool TriangleButton { get; set; }
        private bool DPadUpButton { get; set; }
        private bool DPadRightButton { get; set; }
        private bool DPadDownButton { get; set; }
        private bool DPadLeftButton { get; set; }
        private bool L1Button { get; set; }
        private bool R1Button { get; set; }
        private bool L2Button { get; set; }
        private bool R2Button { get; set; }
        private bool CreateButton { get; set; }
        private bool MenuButton { get; set; }
        private bool L3Button { get; set; }
        private bool R3Button { get; set; }
        private bool LogoButton { get; set; }
        private bool TouchpadButton { get; set; }
        private bool MicButton { get; set; }
        private DualShock4Touch Touchpad1 { get; set; }
        private DualShock4Touch Touchpad2 { get; set; }
        private Vec3 Gyro { get; set; }
        private Vec3 Accelerometer { get; set; }
        private bool IsHeadphoneConnected { get; set; }
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
                    Found(path);
                    reconnectingcount = -150f;
                }
                else
                    reconnectingcount = 0;
            }
        }
        private void ReconnectionInit()
        {
            PS4ControllerLeftStickX = 0;
            PS4ControllerLeftStickY = 0;
            PS4ControllerRightStickX = 0;
            PS4ControllerRightStickY = 0;
            PS4ControllerLeftTriggerPosition = 0;
            PS4ControllerRightTriggerPosition = 0;
            PS4ControllerTouchX = 0;
            PS4ControllerTouchY = 0;
            PS4ControllerTouchOn = false;
            PS4ControllerGyroX = 0;
            PS4ControllerGyroY = 0;
            PS4ControllerAccelX = 0;
            PS4ControllerAccelY = 0;
            PS4ControllerButtonCrossPressed = false;
            PS4ControllerButtonCirclePressed = false;
            PS4ControllerButtonSquarePressed = false;
            PS4ControllerButtonTrianglePressed = false;
            PS4ControllerButtonDPadUpPressed = false;
            PS4ControllerButtonDPadRightPressed = false;
            PS4ControllerButtonDPadDownPressed = false;
            PS4ControllerButtonDPadLeftPressed = false;
            PS4ControllerButtonL1Pressed = false;
            PS4ControllerButtonR1Pressed = false;
            PS4ControllerButtonL2Pressed = false;
            PS4ControllerButtonR2Pressed = false;
            PS4ControllerButtonL3Pressed = false;
            PS4ControllerButtonR3Pressed = false;
            PS4ControllerButtonCreatePressed = false;
            PS4ControllerButtonMenuPressed = false;
            PS4ControllerButtonLogoPressed = false;
            PS4ControllerButtonTouchpadPressed = false;
            PS4ControllerButtonMicPressed = false;
        }
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
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 256)]
            public string DevicePath;
        }
        public void Scan(string vendor_id, string product_id, string label_id, int number = 0)
        {
            this.number = number;
            if (number <= 1)
            {
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
                        if (diDetail.DevicePath.ToLower().Contains(vendor_id.ToLower()) & diDetail.DevicePath.ToLower().Contains(product_id.ToLower()))
                        {
                            if (handleunshared != null)
                            {
                                handleunshared.Close();
                                handleunshared.Dispose();
                                handleunshared = null;
                            }
                            path = diDetail.DevicePath;
                            isvalidhandle = Found(path);
                            handleunshared = CreateFile(path, FileAccess.ReadWrite, FileShare.None, IntPtr.Zero, FileMode.Open, (uint)EFileAttributes.Overlapped, IntPtr.Zero);
                            if (isvalidhandle)
                            {
                                paths.Add(path);
                                mStreams.Add(mStream);
                            }
                        }
                    }
                    index++;
                }
            }
            path = paths[number < 2 ? 0 : number - 1];
            mStream = mStreams[number < 2 ? 0 : number - 1];
        }
        public bool Found(string path)
        {
            try
            {
                handle = CreateFile(path, FileAccess.ReadWrite, FileShare.ReadWrite, IntPtr.Zero, FileMode.Open, (uint)EFileAttributes.Overlapped, IntPtr.Zero);
                mStream = new FileStream(handle, FileAccess.Read, 64, true);
                return true;
            }
            catch { return false; }
        }
    }
    internal static class DualShock4ByteConverterExtensions
    {
        public static float ToSignedFloat(this byte b)
        {
            return (b / 255.0f - 0.5f) * 2.0f;
        }
        public static float ToUnsignedFloat(this byte b)
        {
            return b / 255.0f;
        }
        public static bool HasFlag(this byte b, byte flag)
        {
            return (b & flag) == flag;
        }
    }
    public struct DualShock4Touch
    {
        public uint X;
        public uint Y;
        public bool IsDown;
        public byte Id;
    }
    public struct Vec2
    {
        public float X, Y;

        public float Magnitude()
        {
            return (float)Math.Sqrt(X * X + Y * Y);
        }

        public Vec2 Normalize()
        {
            float m = Magnitude();
            return new Vec2 { X = X / m, Y = Y / m };
        }

        public static Vec2 operator -(Vec2 v)
        {
            return new Vec2 { X = -v.X, Y = -v.Y };
        }
    }
    public struct Vec3
    {
        public float X, Y, Z;
        public float Magnitude()
        {
            return (float)Math.Sqrt(X * X + Y * Y + Z * Z);
        }
        public Vec3 Normalize()
        {
            float m = Magnitude();
            return new Vec3 { X = X / m, Y = Y / m, Z = Z / m };
        }
        public static Vec3 operator -(Vec3 v)
        {
            return new Vec3 { X = -v.X, Y = -v.Y, Z = -v.Z };
        }
    }
}