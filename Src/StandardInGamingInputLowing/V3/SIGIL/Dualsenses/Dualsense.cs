using System;
using System.Linq;
using Device.Net;
using Hid.Net.Windows;
using System.Reactive.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using System.Numerics;
using Dualsenses;

namespace DualSensesAPI
{
    public class DualSense
    {
        [DllImport("winmm.dll", EntryPoint = "timeBeginPeriod")]
        private static extern uint TimeBeginPeriod(uint ms);
        [DllImport("winmm.dll", EntryPoint = "timeEndPeriod")]
        private static extern uint TimeEndPeriod(uint ms);
        [DllImport("ntdll.dll", EntryPoint = "NtSetTimerResolution")]
        private static extern void NtSetTimerResolution(uint DesiredResolution, bool SetResolution, ref uint CurrentResolution);
        private static uint CurrentResolution = 0;
        private byte miscByte;
        private byte btnBlock1, btnBlock2, btnBlock3;
        private byte[] dsdata = new byte[54];
        public IDevice trezorDevice;
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
        public Vector3 gyr_gPS5 = new Vector3();
        public Vector3 acc_gPS5 = new Vector3();
        public Vector3 InitDirectAnglesPS5, DirectAnglesPS5;
        private Task<TransferResult> readBuffer;
        public bool running, formvisible;
        public Form1 form1 = new Form1();
        public DualSense()
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
        public async void Scan(string vendor_id, string product_id, string label_id, int number = 0)
        {
            var hidFactory = new FilterDeviceDefinition((uint)int.Parse(vendor_id, System.Globalization.NumberStyles.HexNumber), (uint)int.Parse(product_id, System.Globalization.NumberStyles.HexNumber), label: label_id).CreateWindowsHidDeviceFactory();
            var factories = hidFactory;
            var deviceDefinitions = (await factories.GetConnectedDeviceDefinitionsAsync().ConfigureAwait(false)).ToList();
            if (deviceDefinitions.Count == 0)
            {
                return;
            }
            if (number == 0 | number == 1)
            {
                trezorDevice = await hidFactory.GetDeviceAsync(deviceDefinitions.First()).ConfigureAwait(false);
                await trezorDevice.InitializeAsync().ConfigureAwait(false);
            }
            else if (number == 2)
            {
                trezorDevice = await hidFactory.GetDeviceAsync(deviceDefinitions.Skip(1).First()).ConfigureAwait(false);
                await trezorDevice.InitializeAsync().ConfigureAwait(false);
            }
        }
        public void ProcessStateLogic()
        {
            LeftAnalogStick = ReadAnalogStick(dsdata[0], dsdata[1]);
            RightAnalogStick = ReadAnalogStick(dsdata[2], dsdata[3]);
            L2 = GetModeSwitch(dsdata, 4).ToUnsignedFloat();
            R2 = GetModeSwitch(dsdata, 5).ToUnsignedFloat();
            btnBlock1 = GetModeSwitch(dsdata, 7);
            btnBlock2 = GetModeSwitch(dsdata, 8);
            btnBlock3 = GetModeSwitch(dsdata, 9);
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
            FnL = btnBlock3.HasFlag(1 << 4);
            FnR = btnBlock3.HasFlag(1 << 5);
            BLP = btnBlock3.HasFlag(1 << 6);
            BRP = btnBlock3.HasFlag(1 << 7);
            MicButton = GetModeSwitch(dsdata, 9).HasFlag(0x04);
            Touchpad1 = ReadTouchpad(GetModeSwitch(dsdata, 32, 4));
            Touchpad2 = ReadTouchpad(GetModeSwitch(dsdata, 36, 4));
            Gyro = -ReadAccelAxes(
                GetModeSwitch(dsdata, 15, 2),
                GetModeSwitch(dsdata, 17, 2),
                GetModeSwitch(dsdata, 19, 2)
            );
            Accelerometer = ReadAccelAxes(
                GetModeSwitch(dsdata, 21, 2),
                GetModeSwitch(dsdata, 23, 2),
                GetModeSwitch(dsdata, 25, 2)
            );
            miscByte = GetModeSwitch(dsdata, 53);
            IsHeadphoneConnected = miscByte.HasFlag(0x01);
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
        private async void taskD()
        {
            for (; ; )
            {
                if (!running)
                    break;
                readBuffer = trezorDevice.WriteAndReadAsync(GetOutputDataBytes());
                readBuffer.Wait();
                dsdata = (await readBuffer).Data.Skip(1).ToArray();
                ProcessStateLogic();
                if (formvisible)
                {
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
                    str += Environment.NewLine;
                    form1.SetLabel1(str);
                }
            }
        }
        public void BeginPolling()
        {
            Task.Run(() => taskD());
        }
        private byte[] GetOutputDataBytes()
        {
            byte[] bytes = new byte[48];
            bytes[0] = 0x02;
            return bytes;
        }
        private byte GetModeSwitch(byte[] dsdata, int indexIfUsb)
        {
            return indexIfUsb >= 0 ? dsdata[indexIfUsb] : (byte)0;
        }
        private byte[] GetModeSwitch(byte[] dsdata, int startIndexIfUsb, int size)
        {
            return startIndexIfUsb >= 0 ? dsdata.Skip(startIndexIfUsb).Take(size).ToArray() : new byte[size];
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
            if (!BitConverter.IsLittleEndian)
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
            if (!BitConverter.IsLittleEndian)
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
        public Vec2 LeftAnalogStick { get; private set; }
        public Vec2 RightAnalogStick { get; private set; }
        public float L2 { get; private set; }
        public float R2 { get; private set; }
        public bool SquareButton { get; private set; }
        public bool CrossButton { get; private set; }
        public bool CircleButton { get; private set; }
        public bool TriangleButton { get; private set; }
        public bool DPadUpButton { get; private set; }
        public bool DPadRightButton { get; private set; }
        public bool DPadDownButton { get; private set; }
        public bool DPadLeftButton { get; private set; }
        public bool L1Button { get; private set; }
        public bool R1Button { get; private set; }
        public bool L2Button { get; private set; }
        public bool R2Button { get; private set; }
        public bool CreateButton { get; private set; }
        public bool MenuButton { get; private set; }
        public bool L3Button { get; private set; }
        public bool R3Button { get; private set; }
        public bool LogoButton { get; private set; }
        public bool TouchpadButton { get; private set; }
        public bool FnL { get; private set; }
        public bool FnR { get; private set; }
        public bool BLP { get; private set; }
        public bool BRP { get; private set; }
        public bool MicButton { get; private set; }
        public DualShock4Touch Touchpad1 { get; private set; }
        public DualShock4Touch Touchpad2 { get; private set; }
        public Vec3 Gyro { get; private set; }
        public Vec3 Accelerometer { get; private set; }
        public bool IsHeadphoneConnected { get; private set; }
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