using System;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using System.Numerics;
using Dualsenses;
using System.Threading;
using System.IO;
using Microsoft.Win32.SafeHandles;
using System.Collections.Generic;
using System.Diagnostics;
using Valuechanges;

namespace DualSensesAPI
{
    public class DualSense : IDisposable
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
        private byte[] dsdata = new byte[64];
        public bool PS5ControllerButtonCrossPressed;
        public bool PS5ControllerButtonCirclePressed;
        public bool PS5ControllerButtonSquarePressed;
        public bool PS5ControllerButtonTrianglePressed;
        public bool PS5ControllerButtonDPadUpPressed;
        public bool PS5ControllerButtonDPadRightPressed;
        public bool PS5ControllerButtonDPadDownPressed;
        public bool PS5ControllerButtonDPadLeftPressed;
        public bool PS5ControllerButtonL1Pressed;
        public bool PS5ControllerButtonR1Pressed;
        public bool PS5ControllerButtonL2Pressed;
        public bool PS5ControllerButtonR2Pressed;
        public bool PS5ControllerButtonL3Pressed;
        public bool PS5ControllerButtonR3Pressed;
        public bool PS5ControllerButtonCreatePressed;
        public bool PS5ControllerButtonMenuPressed;
        public bool PS5ControllerButtonLogoPressed;
        public bool PS5ControllerButtonTouchpadPressed;
        public bool PS5ControllerButtonFnLPressed;
        public bool PS5ControllerButtonFnRPressed;
        public bool PS5ControllerButtonBLPPressed;
        public bool PS5ControllerButtonBRPPressed;
        public bool PS5ControllerButtonMicPressed;
        public bool PS5ControllerTouchOn;
        public double PS5ControllerLeftStickX, PS5ControllerLeftStickY, PS5ControllerRightStickX, PS5ControllerRightStickY, PS5ControllerRightTriggerPosition, PS5ControllerLeftTriggerPosition, PS5ControllerTouchX, PS5ControllerTouchY;
        public bool PS5ControllerAccelCenter;
        public double PS5ControllerAccelX, PS5ControllerAccelY, PS5ControllerGyroX, PS5ControllerGyroY;
        private Vector3 gyr_gPS5 = new Vector3();
        private Vector3 acc_gPS5 = new Vector3();
        private Vector3 InitDirectAnglesPS5, DirectAnglesPS5;
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
        public DualSense()
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
            LeftAnalogStick = ReadAnalogStick(dsdata[1], dsdata[2]);
            RightAnalogStick = ReadAnalogStick(dsdata[3], dsdata[4]);
            L2 = ToUnsignedFloat(dsdata[5]);
            R2 = ToUnsignedFloat(dsdata[6]);
            btnBlock1 = dsdata[8];
            btnBlock2 = dsdata[9];
            btnBlock3 = dsdata[10];
            SquareButton = HasFlag(btnBlock1, 0x10);
            CrossButton = HasFlag(btnBlock1, 0x20);
            CircleButton = HasFlag(btnBlock1, 0x40);
            TriangleButton = HasFlag(btnBlock1, 0x80);
            DPadUpButton = ReadDPadButton(btnBlock1, 0, 1, 7);
            DPadRightButton = ReadDPadButton(btnBlock1, 1, 2, 3);
            DPadDownButton = ReadDPadButton(btnBlock1, 3, 4, 5);
            DPadLeftButton = ReadDPadButton(btnBlock1, 5, 6, 7);
            L1Button = HasFlag(btnBlock2, 0x01);
            R1Button = HasFlag(btnBlock2, 0x02);
            L2Button = HasFlag(btnBlock2, 0x04);
            R2Button = HasFlag(btnBlock2, 0x08);
            CreateButton = HasFlag(btnBlock2, 0x10);
            MenuButton = HasFlag(btnBlock2, 0x20);
            L3Button = HasFlag(btnBlock2, 0x40);
            R3Button = HasFlag(btnBlock2, 0x80);
            LogoButton = HasFlag(btnBlock3, 0x01);
            TouchpadButton = HasFlag(btnBlock3, 0x02);
            FnL = HasFlag(btnBlock3, 1 << 4);
            FnR = HasFlag(btnBlock3, 1 << 5);
            BLP = HasFlag(btnBlock3, 1 << 6);
            BRP = HasFlag(btnBlock3, 1 << 7);
            MicButton = HasFlag(dsdata[10], 0x04);
            Touchpad1 = ReadTouchpad(new byte[] { dsdata[33], dsdata[34], dsdata[35], dsdata[36] });
            Touchpad2 = ReadTouchpad(new byte[] { dsdata[37], dsdata[38], dsdata[39], dsdata[40] });
            Gyro = -ReadAccelAxes(
                new byte[] { dsdata[16], dsdata[17] },
                new byte[] { dsdata[18], dsdata[19] },
                new byte[] { dsdata[20], dsdata[21] }
            );
            Accelerometer = ReadAccelAxes(
                new byte[] { dsdata[22], dsdata[23] },
                new byte[] { dsdata[24], dsdata[25] },
                new byte[] { dsdata[26], dsdata[27] }
            );
            miscByte = dsdata[54];
            IsHeadphoneConnected = HasFlag(miscByte, 0x01);
            PS5ControllerLeftStickX = LeftAnalogStick.X;
            PS5ControllerLeftStickY = LeftAnalogStick.Y;
            PS5ControllerRightStickX = -RightAnalogStick.X;
            PS5ControllerRightStickY = -RightAnalogStick.Y;
            PS5ControllerLeftTriggerPosition = L2;
            PS5ControllerRightTriggerPosition = R2;
            PS5ControllerTouchX = Touchpad1.X;
            PS5ControllerTouchY = Touchpad1.Y;
            PS5ControllerTouchOn = Touchpad1.IsDown;
            gyr_gPS5.X = Gyro.Z;
            gyr_gPS5.Y = -Gyro.X;
            gyr_gPS5.Z = -Gyro.Y;
            PS5ControllerGyroX = gyr_gPS5.Z;
            PS5ControllerGyroY = gyr_gPS5.Y;
            acc_gPS5 = new Vector3(Accelerometer.X, Accelerometer.Z, Accelerometer.Y);
            PS5ControllerAccelCenter = MenuButton;
            DirectAnglesPS5 = acc_gPS5 - InitDirectAnglesPS5;
            PS5ControllerAccelX = -(DirectAnglesPS5.Y + DirectAnglesPS5.Z) / 6f;
            PS5ControllerAccelY = DirectAnglesPS5.X / 6f;
            PS5ControllerButtonCrossPressed = CrossButton;
            PS5ControllerButtonCirclePressed = CircleButton;
            PS5ControllerButtonSquarePressed = SquareButton;
            PS5ControllerButtonTrianglePressed = TriangleButton;
            PS5ControllerButtonDPadUpPressed = DPadUpButton;
            PS5ControllerButtonDPadRightPressed = DPadRightButton;
            PS5ControllerButtonDPadDownPressed = DPadDownButton;
            PS5ControllerButtonDPadLeftPressed = DPadLeftButton;
            PS5ControllerButtonL1Pressed = L1Button;
            PS5ControllerButtonR1Pressed = R1Button;
            PS5ControllerButtonL2Pressed = L2Button;
            PS5ControllerButtonR2Pressed = R2Button;
            PS5ControllerButtonL3Pressed = L3Button;
            PS5ControllerButtonR3Pressed = R3Button;
            PS5ControllerButtonCreatePressed = CreateButton;
            PS5ControllerButtonMenuPressed = MenuButton;
            PS5ControllerButtonLogoPressed = LogoButton;
            PS5ControllerButtonTouchpadPressed = TouchpadButton;
            PS5ControllerButtonFnLPressed = FnL;
            PS5ControllerButtonFnRPressed = FnR;
            PS5ControllerButtonBLPPressed = BLP;
            PS5ControllerButtonBRPPressed = BRP;
            PS5ControllerButtonMicPressed = MicButton;
        }
        public void Init()
        {
            InitDirectAnglesPS5 = acc_gPS5;
        }
        private void taskD()
        {
            for (; ; )
            {
                if (!running)
                    break;
                try
                {
                    mStream.Read(dsdata, 0, dsdata.Length);
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
                    string str = "PS5ControllerLeftStickX : " + PS5ControllerLeftStickX + Environment.NewLine;
                    str += "PS5ControllerLeftStickY : " + PS5ControllerLeftStickY + Environment.NewLine;
                    str += "PS5ControllerRightStickX : " + PS5ControllerRightStickX + Environment.NewLine;
                    str += "PS5ControllerRightStickY : " + PS5ControllerRightStickY + Environment.NewLine;
                    str += "PS5ControllerLeftTriggerPosition : " + PS5ControllerLeftTriggerPosition + Environment.NewLine;
                    str += "PS5ControllerRightTriggerPosition : " + PS5ControllerRightTriggerPosition + Environment.NewLine;
                    str += "PS5ControllerTouchX : " + PS5ControllerTouchX + Environment.NewLine;
                    str += "PS5ControllerTouchY : " + PS5ControllerTouchY + Environment.NewLine;
                    str += "PS5ControllerTouchOn : " + PS5ControllerTouchOn + Environment.NewLine;
                    str += "PS5ControllerGyroX : " + PS5ControllerGyroX + Environment.NewLine;
                    str += "PS5ControllerGyroY : " + PS5ControllerGyroY + Environment.NewLine;
                    str += "PS5ControllerAccelX : " + PS5ControllerAccelX + Environment.NewLine;
                    str += "PS5ControllerAccelY : " + PS5ControllerAccelY + Environment.NewLine;
                    str += "PS5ControllerButtonCrossPressed : " + PS5ControllerButtonCrossPressed + Environment.NewLine;
                    str += "PS5ControllerButtonCirclePressed : " + PS5ControllerButtonCirclePressed + Environment.NewLine;
                    str += "PS5ControllerButtonSquarePressed : " + PS5ControllerButtonSquarePressed + Environment.NewLine;
                    str += "PS5ControllerButtonTrianglePressed : " + PS5ControllerButtonTrianglePressed + Environment.NewLine;
                    str += "PS5ControllerButtonDPadUpPressed : " + PS5ControllerButtonDPadUpPressed + Environment.NewLine;
                    str += "PS5ControllerButtonDPadRightPressed : " + PS5ControllerButtonDPadRightPressed + Environment.NewLine;
                    str += "PS5ControllerButtonDPadDownPressed : " + PS5ControllerButtonDPadDownPressed + Environment.NewLine;
                    str += "PS5ControllerButtonDPadLeftPressed : " + PS5ControllerButtonDPadLeftPressed + Environment.NewLine;
                    str += "PS5ControllerButtonL1Pressed : " + PS5ControllerButtonL1Pressed + Environment.NewLine;
                    str += "PS5ControllerButtonR1Pressed : " + PS5ControllerButtonR1Pressed + Environment.NewLine;
                    str += "PS5ControllerButtonL2Pressed : " + PS5ControllerButtonL2Pressed + Environment.NewLine;
                    str += "PS5ControllerButtonR2Pressed : " + PS5ControllerButtonR2Pressed + Environment.NewLine;
                    str += "PS5ControllerButtonL3Pressed : " + PS5ControllerButtonL3Pressed + Environment.NewLine;
                    str += "PS5ControllerButtonR3Pressed : " + PS5ControllerButtonR3Pressed + Environment.NewLine;
                    str += "PS5ControllerButtonCreatePressed : " + PS5ControllerButtonCreatePressed + Environment.NewLine;
                    str += "PS5ControllerButtonMenuPressed : " + PS5ControllerButtonMenuPressed + Environment.NewLine;
                    str += "PS5ControllerButtonLogoPressed : " + PS5ControllerButtonLogoPressed + Environment.NewLine;
                    str += "PS5ControllerButtonTouchpadPressed : " + PS5ControllerButtonTouchpadPressed + Environment.NewLine;
                    str += "PS5ControllerButtonFnLPressed : " + PS5ControllerButtonFnLPressed + Environment.NewLine;
                    str += "PS5ControllerButtonFnRPressed : " + PS5ControllerButtonFnRPressed + Environment.NewLine;
                    str += "PS5ControllerButtonBLPPressed : " + PS5ControllerButtonBLPPressed + Environment.NewLine;
                    str += "PS5ControllerButtonBRPPressed : " + PS5ControllerButtonBRPPressed + Environment.NewLine;
                    str += "PS5ControllerButtonMicPressed : " + PS5ControllerButtonMicPressed + Environment.NewLine;
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
                    str += "InputDelay : " + delay / 2f + " ms" + Environment.NewLine;
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
        private Vector2 ReadAnalogStick(byte x, byte y)
        {
            float x1 = ToSignedFloat(x);
            float y1 = -ToSignedFloat(y);
            return new Vector2
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
        private Vector3 ReadAccelAxes(byte[] x, byte[] y, byte[] z)
        {
            if (!littleendian)
            {
                x = x.Reverse().ToArray();
                y = y.Reverse().ToArray();
                z = z.Reverse().ToArray();
            }
            return new Vector3
            {
                X = -BitConverter.ToInt16(x, 0),
                Y = BitConverter.ToInt16(y, 0),
                Z = BitConverter.ToInt16(z, 0)
            };
        }
        private Vector2 LeftAnalogStick { get; set; }
        private Vector2 RightAnalogStick { get; set; }
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
        private bool FnL { get; set; }
        private bool FnR { get; set; }
        private bool BLP { get; set; }
        private bool BRP { get; set; }
        private bool MicButton { get; set; }
        private DualShock4Touch Touchpad1 { get; set; }
        private DualShock4Touch Touchpad2 { get; set; }
        private Vector3 Gyro { get; set; }
        private Vector3 Accelerometer { get; set; }
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
            PS5ControllerLeftStickX = 0;
            PS5ControllerLeftStickY = 0;
            PS5ControllerRightStickX = 0;
            PS5ControllerRightStickY = 0;
            PS5ControllerLeftTriggerPosition = 0;
            PS5ControllerRightTriggerPosition = 0;
            PS5ControllerTouchX = 0;
            PS5ControllerTouchY = 0;
            PS5ControllerTouchOn = false;
            PS5ControllerGyroX = 0;
            PS5ControllerGyroY = 0;
            PS5ControllerAccelX = 0;
            PS5ControllerAccelY = 0;
            PS5ControllerButtonCrossPressed = false;
            PS5ControllerButtonCirclePressed = false;
            PS5ControllerButtonSquarePressed = false;
            PS5ControllerButtonTrianglePressed = false;
            PS5ControllerButtonDPadUpPressed = false;
            PS5ControllerButtonDPadRightPressed = false;
            PS5ControllerButtonDPadDownPressed = false;
            PS5ControllerButtonDPadLeftPressed = false;
            PS5ControllerButtonL1Pressed = false;
            PS5ControllerButtonR1Pressed = false;
            PS5ControllerButtonL2Pressed = false;
            PS5ControllerButtonR2Pressed = false;
            PS5ControllerButtonL3Pressed = false;
            PS5ControllerButtonR3Pressed = false;
            PS5ControllerButtonCreatePressed = false;
            PS5ControllerButtonMenuPressed = false;
            PS5ControllerButtonLogoPressed = false;
            PS5ControllerButtonTouchpadPressed = false;
            PS5ControllerButtonFnLPressed = false;
            PS5ControllerButtonFnRPressed = false;
            PS5ControllerButtonBLPPressed = false;
            PS5ControllerButtonBRPPressed = false;
            PS5ControllerButtonMicPressed = false;
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
        private bool Found(string path)
        {
            try
            {
                handle = CreateFile(path, FileAccess.ReadWrite, FileShare.ReadWrite, IntPtr.Zero, FileMode.Open, (uint)EFileAttributes.Overlapped, IntPtr.Zero);
                mStream = new FileStream(handle, FileAccess.Read, 64, true);
                return true;
            }
            catch { return false; }
        }
        public void Dispose()
        {
            GC.Collect();
            GC.WaitForPendingFinalizers();
            GC.SuppressFinalize(this);
        }
        private float ToSignedFloat(byte b)
        {
            return (b / 255.0f - 0.5f) * 2.0f;
        }
        private float ToUnsignedFloat(byte b)
        {
            return b / 255.0f;
        }
        private bool HasFlag(byte b, byte flag)
        {
            return (b & flag) == flag;
        }
        private struct DualShock4Touch
        {
            public uint X;
            public uint Y;
            public bool IsDown;
            public byte Id;
        }
    }
}