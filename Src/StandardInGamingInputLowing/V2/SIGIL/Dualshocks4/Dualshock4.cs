using System;
using System.Linq;
using Device.Net;
using Hid.Net.Windows;
using System.Reactive.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using System.Numerics;
using Dualshocks4;

namespace DualShocks4API
{
    public class DualShock4
    {
        [DllImport("winmm.dll", EntryPoint = "timeBeginPeriod")]
        private static extern uint TimeBeginPeriod(uint ms);
        [DllImport("winmm.dll", EntryPoint = "timeEndPeriod")]
        private static extern uint TimeEndPeriod(uint ms);
        [DllImport("ntdll.dll", EntryPoint = "NtSetTimerResolution")]
        private static extern void NtSetTimerResolution(uint DesiredResolution, bool SetResolution, ref uint CurrentResolution);
        private static uint CurrentResolution = 0;
        private byte[] ds4data1 = new byte[37], ds4data2 = new byte[37];
        public IDevice trezorDevice1, trezorDevice2;
        private byte miscByte1;
        private byte btnBlock11, btnBlock21, btnBlock31;
        public bool PS4Controller1ButtonCrossPressed;
        public bool PS4Controller1ButtonCirclePressed;
        public bool PS4Controller1ButtonSquarePressed;
        public bool PS4Controller1ButtonTrianglePressed;
        public bool PS4Controller1ButtonDPadUpPressed;
        public bool PS4Controller1ButtonDPadRightPressed;
        public bool PS4Controller1ButtonDPadDownPressed;
        public bool PS4Controller1ButtonDPadLeftPressed;
        public bool PS4Controller1ButtonL1Pressed;
        public bool PS4Controller1ButtonR1Pressed;
        public bool PS4Controller1ButtonL2Pressed;
        public bool PS4Controller1ButtonR2Pressed;
        public bool PS4Controller1ButtonL3Pressed;
        public bool PS4Controller1ButtonR3Pressed;
        public bool PS4Controller1ButtonCreatePressed;
        public bool PS4Controller1ButtonMenuPressed;
        public bool PS4Controller1ButtonLogoPressed;
        public bool PS4Controller1ButtonTouchpadPressed;
        public bool PS4Controller1ButtonMicPressed;
        public bool PS4Controller1TouchOn;
        public double PS4Controller1LeftStickX, PS4Controller1LeftStickY, PS4Controller1RightStickX, PS4Controller1RightStickY, PS4Controller1RightTriggerPosition, PS4Controller1LeftTriggerPosition, PS4Controller1TouchX, PS4Controller1TouchY;
        public bool PS4Controller1AccelCenter;
        public double PS4Controller1AccelX, PS4Controller1AccelY, PS4Controller1GyroX, PS4Controller1GyroY;
        public Vector3 gyr1_gPS4 = new Vector3();
        public Vector3 acc1_gPS4 = new Vector3();
        public Vector3 InitDirectAngles1PS4, DirectAngles1PS4;
        private Task<TransferResult> readBuffer1;
        private byte miscByte2;
        private byte btnBlock12, btnBlock22, btnBlock32;
        public bool PS4Controller2ButtonCrossPressed;
        public bool PS4Controller2ButtonCirclePressed;
        public bool PS4Controller2ButtonSquarePressed;
        public bool PS4Controller2ButtonTrianglePressed;
        public bool PS4Controller2ButtonDPadUpPressed;
        public bool PS4Controller2ButtonDPadRightPressed;
        public bool PS4Controller2ButtonDPadDownPressed;
        public bool PS4Controller2ButtonDPadLeftPressed;
        public bool PS4Controller2ButtonL1Pressed;
        public bool PS4Controller2ButtonR1Pressed;
        public bool PS4Controller2ButtonL2Pressed;
        public bool PS4Controller2ButtonR2Pressed;
        public bool PS4Controller2ButtonL3Pressed;
        public bool PS4Controller2ButtonR3Pressed;
        public bool PS4Controller2ButtonCreatePressed;
        public bool PS4Controller2ButtonMenuPressed;
        public bool PS4Controller2ButtonLogoPressed;
        public bool PS4Controller2ButtonTouchpadPressed;
        public bool PS4Controller2ButtonMicPressed;
        public bool PS4Controller2TouchOn;
        public double PS4Controller2LeftStickX, PS4Controller2LeftStickY, PS4Controller2RightStickX, PS4Controller2RightStickY, PS4Controller2RightTriggerPosition, PS4Controller2LeftTriggerPosition, PS4Controller2TouchX, PS4Controller2TouchY;
        public bool PS4Controller2AccelCenter;
        public double PS4Controller2AccelX, PS4Controller2AccelY, PS4Controller2GyroX, PS4Controller2GyroY;
        public Vector3 gyr2_gPS4 = new Vector3();
        public Vector3 acc2_gPS4 = new Vector3();
        public Vector3 InitDirectAngles2PS4, DirectAngles2PS4;
        private Task<TransferResult> readBuffer2;
        public bool running, formvisible;
        public Form1 form1 = new Form1();
        public DualShock4()
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
        public async void ScanDualshock4(string vendor_id, string product_id, string label_id)
        {
            var hidFactory = new FilterDeviceDefinition((uint)int.Parse(vendor_id, System.Globalization.NumberStyles.HexNumber), (uint)int.Parse(product_id, System.Globalization.NumberStyles.HexNumber), label: label_id).CreateWindowsHidDeviceFactory();
            var factories = hidFactory;
            var deviceDefinitions = (await factories.GetConnectedDeviceDefinitionsAsync().ConfigureAwait(false)).ToList();
            if (deviceDefinitions.Count == 0)
            {
                return;
            }
            trezorDevice1 = await hidFactory.GetDeviceAsync(deviceDefinitions.First()).ConfigureAwait(false);
            await trezorDevice1.InitializeAsync().ConfigureAwait(false);
            trezorDevice2 = await hidFactory.GetDeviceAsync(deviceDefinitions.Skip(1).First()).ConfigureAwait(false);
            await trezorDevice2.InitializeAsync().ConfigureAwait(false);
        }
        public void ProcessStateLogic1()
        {
            LeftAnalogStick1 = ReadAnalogStick1(ds4data1[0], ds4data1[1]);
            RightAnalogStick1 = ReadAnalogStick1(ds4data1[2], ds4data1[3]);
            L21 = GetModeSwitch1(ds4data1, 7).ToUnsignedFloat();
            R21 = GetModeSwitch1(ds4data1, 8).ToUnsignedFloat();
            btnBlock11 = GetModeSwitch1(ds4data1, 4);
            btnBlock21 = GetModeSwitch1(ds4data1, 5);
            btnBlock31 = GetModeSwitch1(ds4data1, 6);
            SquareButton1 = btnBlock11.HasFlag(0x10);
            CrossButton1 = btnBlock11.HasFlag(0x20);
            CircleButton1 = btnBlock11.HasFlag(0x40);
            TriangleButton1 = btnBlock11.HasFlag(0x80);
            DPadUpButton1 = ReadDPadButton1(btnBlock11, 0, 1, 7);
            DPadRightButton1 = ReadDPadButton1(btnBlock11, 1, 2, 3);
            DPadDownButton1 = ReadDPadButton1(btnBlock11, 3, 4, 5);
            DPadLeftButton1 = ReadDPadButton1(btnBlock11, 5, 6, 7);
            L1Button1 = btnBlock21.HasFlag(0x01);
            R1Button1 = btnBlock21.HasFlag(0x02);
            L2Button1 = btnBlock21.HasFlag(0x04);
            R2Button1 = btnBlock21.HasFlag(0x08);
            CreateButton1 = btnBlock21.HasFlag(0x10);
            MenuButton1 = btnBlock21.HasFlag(0x20);
            L3Button1 = btnBlock21.HasFlag(0x40);
            R3Button1 = btnBlock21.HasFlag(0x80);
            LogoButton1 = btnBlock31.HasFlag(0x01);
            TouchpadButton1 = btnBlock31.HasFlag(0x02);
            MicButton1 = GetModeSwitch1(ds4data1, 9).HasFlag(0x04);
            Touchpad11 = ReadTouchpad1(GetModeSwitch1(ds4data1, 34, 4));
            Touchpad21 = ReadTouchpad1(GetModeSwitch1(ds4data1, 36, 4));
            Gyro1 = -ReadAccelAxes1(
                GetModeSwitch1(ds4data1, 12, 2),
                GetModeSwitch1(ds4data1, 14, 2),
                GetModeSwitch1(ds4data1, 16, 2)
            );
            Accelerometer1 = ReadAccelAxes1(
                GetModeSwitch1(ds4data1, 18, 2),
                GetModeSwitch1(ds4data1, 20, 2),
                GetModeSwitch1(ds4data1, 22, 2)
            );
            miscByte1 = GetModeSwitch1(ds4data1, 29);
            IsHeadphoneConnected1 = miscByte1.HasFlag(0x01);
            PS4Controller1LeftStickX = LeftAnalogStick1.X;
            PS4Controller1LeftStickY = LeftAnalogStick1.Y;
            PS4Controller1RightStickX = -RightAnalogStick1.X;
            PS4Controller1RightStickY = -RightAnalogStick1.Y;
            PS4Controller1LeftTriggerPosition = L21;
            PS4Controller1RightTriggerPosition = R21;
            PS4Controller1TouchX = Touchpad11.X;
            PS4Controller1TouchY = Touchpad11.Y;
            PS4Controller1TouchOn = Touchpad11.IsDown;
            gyr1_gPS4.X = Gyro1.Z;
            gyr1_gPS4.Y = -Gyro1.X;
            gyr1_gPS4.Z = -Gyro1.Y;
            PS4Controller1GyroX = gyr1_gPS4.Z;
            PS4Controller1GyroY = gyr1_gPS4.Y;
            acc1_gPS4 = new Vector3(Accelerometer1.X, Accelerometer1.Z, Accelerometer1.Y);
            PS4Controller1AccelCenter = MenuButton1;
            DirectAngles1PS4 = acc1_gPS4 - InitDirectAngles1PS4;
            PS4Controller1AccelX = -(DirectAngles1PS4.Y + DirectAngles1PS4.Z) / 6f;
            PS4Controller1AccelY = DirectAngles1PS4.X / 6f;
            PS4Controller1ButtonCrossPressed = CrossButton1;
            PS4Controller1ButtonCirclePressed = CircleButton1;
            PS4Controller1ButtonSquarePressed = SquareButton1;
            PS4Controller1ButtonTrianglePressed = TriangleButton1;
            PS4Controller1ButtonDPadUpPressed = DPadUpButton1;
            PS4Controller1ButtonDPadRightPressed = DPadRightButton1;
            PS4Controller1ButtonDPadDownPressed = DPadDownButton1;
            PS4Controller1ButtonDPadLeftPressed = DPadLeftButton1;
            PS4Controller1ButtonL1Pressed = L1Button1;
            PS4Controller1ButtonR1Pressed = R1Button1;
            PS4Controller1ButtonL2Pressed = L2Button1;
            PS4Controller1ButtonR2Pressed = R2Button1;
            PS4Controller1ButtonL3Pressed = L3Button1;
            PS4Controller1ButtonR3Pressed = R3Button1;
            PS4Controller1ButtonCreatePressed = CreateButton1;
            PS4Controller1ButtonMenuPressed = MenuButton1;
            PS4Controller1ButtonLogoPressed = LogoButton1;
            PS4Controller1ButtonTouchpadPressed = TouchpadButton1;
            PS4Controller1ButtonMicPressed = MicButton1;
        }
        public void ProcessStateLogic2()
        {
            LeftAnalogStick2 = ReadAnalogStick2(ds4data2[0], ds4data2[1]);
            RightAnalogStick2 = ReadAnalogStick2(ds4data2[2], ds4data2[3]);
            L22 = GetModeSwitch2(ds4data2, 7).ToUnsignedFloat();
            R22 = GetModeSwitch2(ds4data2, 8).ToUnsignedFloat();
            btnBlock12 = GetModeSwitch2(ds4data2, 4);
            btnBlock22 = GetModeSwitch2(ds4data2, 5);
            btnBlock32 = GetModeSwitch2(ds4data2, 6);
            SquareButton2 = btnBlock12.HasFlag(0x10);
            CrossButton2 = btnBlock12.HasFlag(0x20);
            CircleButton2 = btnBlock12.HasFlag(0x40);
            TriangleButton2 = btnBlock12.HasFlag(0x80);
            DPadUpButton2 = ReadDPadButton2(btnBlock12, 0, 1, 7);
            DPadRightButton2 = ReadDPadButton2(btnBlock12, 1, 2, 3);
            DPadDownButton2 = ReadDPadButton2(btnBlock12, 3, 4, 5);
            DPadLeftButton2 = ReadDPadButton2(btnBlock12, 5, 6, 7);
            L1Button2 = btnBlock22.HasFlag(0x01);
            R1Button2 = btnBlock22.HasFlag(0x02);
            L2Button2 = btnBlock22.HasFlag(0x04);
            R2Button2 = btnBlock22.HasFlag(0x08);
            CreateButton2 = btnBlock22.HasFlag(0x10);
            MenuButton2 = btnBlock22.HasFlag(0x20);
            L3Button2 = btnBlock22.HasFlag(0x40);
            R3Button2 = btnBlock22.HasFlag(0x80);
            LogoButton2 = btnBlock32.HasFlag(0x01);
            TouchpadButton2 = btnBlock32.HasFlag(0x02);
            MicButton2 = GetModeSwitch2(ds4data2, 9).HasFlag(0x04);
            Touchpad12 = ReadTouchpad2(GetModeSwitch2(ds4data2, 34, 4));
            Touchpad22 = ReadTouchpad2(GetModeSwitch2(ds4data2, 36, 4));
            Gyro2 = -ReadAccelAxes2(
                GetModeSwitch2(ds4data2, 12, 2),
                GetModeSwitch2(ds4data2, 14, 2),
                GetModeSwitch2(ds4data2, 16, 2)
            );
            Accelerometer2 = ReadAccelAxes2(
                GetModeSwitch2(ds4data2, 18, 2),
                GetModeSwitch2(ds4data2, 20, 2),
                GetModeSwitch2(ds4data2, 22, 2)
            );
            miscByte2 = GetModeSwitch2(ds4data2, 29);
            IsHeadphoneConnected2 = miscByte2.HasFlag(0x01);
            PS4Controller2LeftStickX = LeftAnalogStick2.X;
            PS4Controller2LeftStickY = LeftAnalogStick2.Y;
            PS4Controller2RightStickX = -RightAnalogStick2.X;
            PS4Controller2RightStickY = -RightAnalogStick2.Y;
            PS4Controller2LeftTriggerPosition = L22;
            PS4Controller2RightTriggerPosition = R22;
            PS4Controller2TouchX = Touchpad12.X;
            PS4Controller2TouchY = Touchpad12.Y;
            PS4Controller2TouchOn = Touchpad12.IsDown;
            gyr2_gPS4.X = Gyro2.Z;
            gyr2_gPS4.Y = -Gyro2.X;
            gyr2_gPS4.Z = -Gyro2.Y;
            PS4Controller2GyroX = gyr2_gPS4.Z;
            PS4Controller2GyroY = gyr2_gPS4.Y;
            acc2_gPS4 = new Vector3(Accelerometer2.X, Accelerometer2.Z, Accelerometer2.Y);
            PS4Controller2AccelCenter = MenuButton2;
            DirectAngles2PS4 = acc2_gPS4 - InitDirectAngles2PS4;
            PS4Controller2AccelX = -(DirectAngles2PS4.Y + DirectAngles2PS4.Z) / 6f;
            PS4Controller2AccelY = DirectAngles2PS4.X / 6f;
            PS4Controller2ButtonCrossPressed = CrossButton2;
            PS4Controller2ButtonCirclePressed = CircleButton2;
            PS4Controller2ButtonSquarePressed = SquareButton2;
            PS4Controller2ButtonTrianglePressed = TriangleButton2;
            PS4Controller2ButtonDPadUpPressed = DPadUpButton2;
            PS4Controller2ButtonDPadRightPressed = DPadRightButton2;
            PS4Controller2ButtonDPadDownPressed = DPadDownButton2;
            PS4Controller2ButtonDPadLeftPressed = DPadLeftButton2;
            PS4Controller2ButtonL1Pressed = L1Button2;
            PS4Controller2ButtonR1Pressed = R1Button2;
            PS4Controller2ButtonL2Pressed = L2Button2;
            PS4Controller2ButtonR2Pressed = R2Button2;
            PS4Controller2ButtonL3Pressed = L3Button2;
            PS4Controller2ButtonR3Pressed = R3Button2;
            PS4Controller2ButtonCreatePressed = CreateButton2;
            PS4Controller2ButtonMenuPressed = MenuButton2;
            PS4Controller2ButtonLogoPressed = LogoButton2;
            PS4Controller2ButtonTouchpadPressed = TouchpadButton2;
            PS4Controller2ButtonMicPressed = MicButton2;
        }
        public void InitDualShock4Accel1()
        {
            InitDirectAngles1PS4 = acc1_gPS4;
        }
        public void InitDualShock4Accel2()
        {
            InitDirectAngles2PS4 = acc2_gPS4;
        }
        private async void taskD1()
        {
            for (; ; )
            {
                if (!running)
                    break;
                readBuffer1 = trezorDevice1.WriteAndReadAsync(GetOutputDataBytes1());
                readBuffer1.Wait();
                ds4data1 = (await readBuffer1).Data.Skip(1).ToArray();
                ProcessStateLogic1();
                if (formvisible)
                {
                    string str = "PS4Controller1LeftStickX : " + PS4Controller1LeftStickX + Environment.NewLine;
                    str += "PS4Controller1LeftStickY : " + PS4Controller1LeftStickY + Environment.NewLine;
                    str += "PS4Controller1RightStickX : " + PS4Controller1RightStickX + Environment.NewLine;
                    str += "PS4Controller1RightStickY : " + PS4Controller1RightStickY + Environment.NewLine;
                    str += "PS4Controller1LeftTriggerPosition : " + PS4Controller1LeftTriggerPosition + Environment.NewLine;
                    str += "PS4Controller1RightTriggerPosition : " + PS4Controller1RightTriggerPosition + Environment.NewLine;
                    str += "PS4Controller1TouchX : " + PS4Controller1TouchX + Environment.NewLine;
                    str += "PS4Controller1TouchY : " + PS4Controller1TouchY + Environment.NewLine;
                    str += "PS4Controller1TouchOn : " + PS4Controller1TouchOn + Environment.NewLine;
                    str += "PS4Controller1GyroX : " + PS4Controller1GyroX + Environment.NewLine;
                    str += "PS4Controller1GyroY : " + PS4Controller1GyroY + Environment.NewLine;
                    str += "PS4Controller1AccelX : " + PS4Controller1AccelX + Environment.NewLine;
                    str += "PS4Controller1AccelY : " + PS4Controller1AccelY + Environment.NewLine;
                    str += "PS4Controller1ButtonCrossPressed : " + PS4Controller1ButtonCrossPressed + Environment.NewLine;
                    str += "PS4Controller1ButtonCirclePressed : " + PS4Controller1ButtonCirclePressed + Environment.NewLine;
                    str += "PS4Controller1ButtonSquarePressed : " + PS4Controller1ButtonSquarePressed + Environment.NewLine;
                    str += "PS4Controller1ButtonTrianglePressed : " + PS4Controller1ButtonTrianglePressed + Environment.NewLine;
                    str += "PS4Controller1ButtonDPadUpPressed : " + PS4Controller1ButtonDPadUpPressed + Environment.NewLine;
                    str += "PS4Controller1ButtonDPadRightPressed : " + PS4Controller1ButtonDPadRightPressed + Environment.NewLine;
                    str += "PS4Controller1ButtonDPadDownPressed : " + PS4Controller1ButtonDPadDownPressed + Environment.NewLine;
                    str += "PS4Controller1ButtonDPadLeftPressed : " + PS4Controller1ButtonDPadLeftPressed + Environment.NewLine;
                    str += "PS4Controller1ButtonL1Pressed : " + PS4Controller1ButtonL1Pressed + Environment.NewLine;
                    str += "PS4Controller1ButtonR1Pressed : " + PS4Controller1ButtonR1Pressed + Environment.NewLine;
                    str += "PS4Controller1ButtonL2Pressed : " + PS4Controller1ButtonL2Pressed + Environment.NewLine;
                    str += "PS4Controller1ButtonR2Pressed : " + PS4Controller1ButtonR2Pressed + Environment.NewLine;
                    str += "PS4Controller1ButtonL3Pressed : " + PS4Controller1ButtonL3Pressed + Environment.NewLine;
                    str += "PS4Controller1ButtonR3Pressed : " + PS4Controller1ButtonR3Pressed + Environment.NewLine;
                    str += "PS4Controller1ButtonCreatePressed : " + PS4Controller1ButtonCreatePressed + Environment.NewLine;
                    str += "PS4Controller1ButtonMenuPressed : " + PS4Controller1ButtonMenuPressed + Environment.NewLine;
                    str += "PS4Controller1ButtonLogoPressed : " + PS4Controller1ButtonLogoPressed + Environment.NewLine;
                    str += "PS4Controller1ButtonTouchpadPressed : " + PS4Controller1ButtonTouchpadPressed + Environment.NewLine;
                    str += "PS4Controller1ButtonMicPressed : " + PS4Controller1ButtonMicPressed + Environment.NewLine;
                    str += Environment.NewLine;
                    form1.SetLabel1(str);
                }
            }
        }
        private async void taskD2()
        {
            for (; ; )
            {
                if (!running)
                    break;
                readBuffer2 = trezorDevice2.WriteAndReadAsync(GetOutputDataBytes2());
                readBuffer2.Wait();
                ds4data2 = (await readBuffer2).Data.Skip(1).ToArray();
                ProcessStateLogic2();
                if (formvisible)
                {
                    string str = "PS4Controller2LeftStickX : " + PS4Controller2LeftStickX + Environment.NewLine;
                    str += "PS4Controller2LeftStickY : " + PS4Controller2LeftStickY + Environment.NewLine;
                    str += "PS4Controller2RightStickX : " + PS4Controller2RightStickX + Environment.NewLine;
                    str += "PS4Controller2RightStickY : " + PS4Controller2RightStickY + Environment.NewLine;
                    str += "PS4Controller2LeftTriggerPosition : " + PS4Controller2LeftTriggerPosition + Environment.NewLine;
                    str += "PS4Controller2RightTriggerPosition : " + PS4Controller2RightTriggerPosition + Environment.NewLine;
                    str += "PS4Controller2TouchX : " + PS4Controller2TouchX + Environment.NewLine;
                    str += "PS4Controller2TouchY : " + PS4Controller2TouchY + Environment.NewLine;
                    str += "PS4Controller2TouchOn : " + PS4Controller2TouchOn + Environment.NewLine;
                    str += "PS4Controller2GyroX : " + PS4Controller2GyroX + Environment.NewLine;
                    str += "PS4Controller2GyroY : " + PS4Controller2GyroY + Environment.NewLine;
                    str += "PS4Controller2AccelX : " + PS4Controller2AccelX + Environment.NewLine;
                    str += "PS4Controller2AccelY : " + PS4Controller2AccelY + Environment.NewLine;
                    str += "PS4Controller2ButtonCrossPressed : " + PS4Controller2ButtonCrossPressed + Environment.NewLine;
                    str += "PS4Controller2ButtonCirclePressed : " + PS4Controller2ButtonCirclePressed + Environment.NewLine;
                    str += "PS4Controller2ButtonSquarePressed : " + PS4Controller2ButtonSquarePressed + Environment.NewLine;
                    str += "PS4Controller2ButtonTrianglePressed : " + PS4Controller2ButtonTrianglePressed + Environment.NewLine;
                    str += "PS4Controller2ButtonDPadUpPressed : " + PS4Controller2ButtonDPadUpPressed + Environment.NewLine;
                    str += "PS4Controller2ButtonDPadRightPressed : " + PS4Controller2ButtonDPadRightPressed + Environment.NewLine;
                    str += "PS4Controller2ButtonDPadDownPressed : " + PS4Controller2ButtonDPadDownPressed + Environment.NewLine;
                    str += "PS4Controller2ButtonDPadLeftPressed : " + PS4Controller2ButtonDPadLeftPressed + Environment.NewLine;
                    str += "PS4Controller2ButtonL1Pressed : " + PS4Controller2ButtonL1Pressed + Environment.NewLine;
                    str += "PS4Controller2ButtonR1Pressed : " + PS4Controller2ButtonR1Pressed + Environment.NewLine;
                    str += "PS4Controller2ButtonL2Pressed : " + PS4Controller2ButtonL2Pressed + Environment.NewLine;
                    str += "PS4Controller2ButtonR2Pressed : " + PS4Controller2ButtonR2Pressed + Environment.NewLine;
                    str += "PS4Controller2ButtonL3Pressed : " + PS4Controller2ButtonL3Pressed + Environment.NewLine;
                    str += "PS4Controller2ButtonR3Pressed : " + PS4Controller2ButtonR3Pressed + Environment.NewLine;
                    str += "PS4Controller2ButtonCreatePressed : " + PS4Controller2ButtonCreatePressed + Environment.NewLine;
                    str += "PS4Controller2ButtonMenuPressed : " + PS4Controller2ButtonMenuPressed + Environment.NewLine;
                    str += "PS4Controller2ButtonLogoPressed : " + PS4Controller2ButtonLogoPressed + Environment.NewLine;
                    str += "PS4Controller2ButtonTouchpadPressed : " + PS4Controller2ButtonTouchpadPressed + Environment.NewLine;
                    str += "PS4Controller2ButtonMicPressed : " + PS4Controller2ButtonMicPressed + Environment.NewLine;
                    str += Environment.NewLine;
                    form1.SetLabel2(str);
                }
            }
        }
        public void BeginPolling1()
        {
            Task.Run(() => taskD1());
        }
        public void BeginPolling2()
        {
            Task.Run(() => taskD2());
        }
        private static byte[] GetOutputDataBytes1()
        {
            byte[] bytes = new byte[32];
            bytes[0] = 0x05;
            return bytes;
        }
        private static byte GetModeSwitch1(byte[] ds4data, int indexIfUsb)
        {
            return indexIfUsb >= 0 ? ds4data[indexIfUsb] : (byte)0;
        }
        private static byte[] GetModeSwitch1(byte[] data, int startIndexIfUsb, int size)
        {
            return startIndexIfUsb >= 0 ? data.Skip(startIndexIfUsb).Take(size).ToArray() : new byte[size];
        }
        private static Vec2 ReadAnalogStick1(byte x, byte y)
        {
            float x1 = x.ToSignedFloat();
            float y1 = -y.ToSignedFloat();
            return new Vec2
            {
                X = Math.Abs(x1) >= 0f ? x1 : 0,
                Y = Math.Abs(y1) >= 0f ? y1 : 0
            };
        }
        private static bool ReadDPadButton1(byte b, int v1, int v2, int v3)
        {
            int val = b & 0x0F;
            return val == v1 || val == v2 || val == v3;
        }
        private static DualShock4Touch ReadTouchpad1(byte[] bytes)
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
        private static Vec3 ReadAccelAxes1(byte[] x, byte[] y, byte[] z)
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
        public static Vec2 LeftAnalogStick1 { get; private set; }
        public static Vec2 RightAnalogStick1 { get; private set; }
        public static float L21 { get; private set; }
        public static float R21 { get; private set; }
        public static bool SquareButton1 { get; private set; }
        public static bool CrossButton1 { get; private set; }
        public static bool CircleButton1 { get; private set; }
        public static bool TriangleButton1 { get; private set; }
        public static bool DPadUpButton1 { get; private set; }
        public static bool DPadRightButton1 { get; private set; }
        public static bool DPadDownButton1 { get; private set; }
        public static bool DPadLeftButton1 { get; private set; }
        public static bool L1Button1 { get; private set; }
        public static bool R1Button1 { get; private set; }
        public static bool L2Button1 { get; private set; }
        public static bool R2Button1 { get; private set; }
        public static bool CreateButton1 { get; private set; }
        public static bool MenuButton1 { get; private set; }
        public static bool L3Button1 { get; private set; }
        public static bool R3Button1 { get; private set; }
        public static bool LogoButton1 { get; private set; }
        public static bool TouchpadButton1 { get; private set; }
        public static bool MicButton1 { get; private set; }
        public static DualShock4Touch Touchpad11 { get; private set; }
        public static DualShock4Touch Touchpad21 { get; private set; }
        public static Vec3 Gyro1 { get; private set; }
        public static Vec3 Accelerometer1 { get; private set; }
        public static bool IsHeadphoneConnected1 { get; private set; }
        private static byte[] GetOutputDataBytes2()
        {
            byte[] bytes = new byte[32];
            bytes[0] = 0x05;
            return bytes;
        }
        private static byte GetModeSwitch2(byte[] ds4data, int indexIfUsb)
        {
            return indexIfUsb >= 0 ? ds4data[indexIfUsb] : (byte)0;
        }
        private static byte[] GetModeSwitch2(byte[] data, int startIndexIfUsb, int size)
        {
            return startIndexIfUsb >= 0 ? data.Skip(startIndexIfUsb).Take(size).ToArray() : new byte[size];
        }
        private static Vec2 ReadAnalogStick2(byte x, byte y)
        {
            float x1 = x.ToSignedFloat();
            float y1 = -y.ToSignedFloat();
            return new Vec2
            {
                X = Math.Abs(x1) >= 0f ? x1 : 0,
                Y = Math.Abs(y1) >= 0f ? y1 : 0
            };
        }
        private static bool ReadDPadButton2(byte b, int v1, int v2, int v3)
        {
            int val = b & 0x0F;
            return val == v1 || val == v2 || val == v3;
        }
        private static DualShock4Touch ReadTouchpad2(byte[] bytes)
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
        private static Vec3 ReadAccelAxes2(byte[] x, byte[] y, byte[] z)
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
        public static Vec2 LeftAnalogStick2 { get; private set; }
        public static Vec2 RightAnalogStick2 { get; private set; }
        public static float L22 { get; private set; }
        public static float R22 { get; private set; }
        public static bool SquareButton2 { get; private set; }
        public static bool CrossButton2 { get; private set; }
        public static bool CircleButton2 { get; private set; }
        public static bool TriangleButton2 { get; private set; }
        public static bool DPadUpButton2 { get; private set; }
        public static bool DPadRightButton2 { get; private set; }
        public static bool DPadDownButton2 { get; private set; }
        public static bool DPadLeftButton2 { get; private set; }
        public static bool L1Button2 { get; private set; }
        public static bool R1Button2 { get; private set; }
        public static bool L2Button2 { get; private set; }
        public static bool R2Button2 { get; private set; }
        public static bool CreateButton2 { get; private set; }
        public static bool MenuButton2 { get; private set; }
        public static bool L3Button2 { get; private set; }
        public static bool R3Button2 { get; private set; }
        public static bool LogoButton2 { get; private set; }
        public static bool TouchpadButton2 { get; private set; }
        public static bool MicButton2 { get; private set; }
        public static DualShock4Touch Touchpad12 { get; private set; }
        public static DualShock4Touch Touchpad22 { get; private set; }
        public static Vec3 Gyro2 { get; private set; }
        public static Vec3 Accelerometer2 { get; private set; }
        public static bool IsHeadphoneConnected2 { get; private set; }
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