using System;
using System.Linq;
using Device.Net;
using Hid.Net.Windows;
using System.Reactive.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using System.Numerics;
using Dualsenses;
using System.Xml.Linq;

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
        private byte[] dsdata1 = new byte[54], dsdata2 = new byte[54];
        public IDevice trezorDevice1, trezorDevice2;
        private byte miscByte1;
        private byte btnBlock11, btnBlock21, btnBlock31;
        public bool PS5Controller1ButtonCrossPressed;
        public bool PS5Controller1ButtonCirclePressed;
        public bool PS5Controller1ButtonSquarePressed;
        public bool PS5Controller1ButtonTrianglePressed;
        public bool PS5Controller1ButtonDPadUpPressed;
        public bool PS5Controller1ButtonDPadRightPressed;
        public bool PS5Controller1ButtonDPadDownPressed;
        public bool PS5Controller1ButtonDPadLeftPressed;
        public bool PS5Controller1ButtonL1Pressed;
        public bool PS5Controller1ButtonR1Pressed;
        public bool PS5Controller1ButtonL2Pressed;
        public bool PS5Controller1ButtonR2Pressed;
        public bool PS5Controller1ButtonL3Pressed;
        public bool PS5Controller1ButtonR3Pressed;
        public bool PS5Controller1ButtonCreatePressed;
        public bool PS5Controller1ButtonMenuPressed;
        public bool PS5Controller1ButtonLogoPressed;
        public bool PS5Controller1ButtonTouchpadPressed;
        public bool PS5Controller1ButtonFnLPressed;
        public bool PS5Controller1ButtonFnRPressed;
        public bool PS5Controller1ButtonBLPPressed;
        public bool PS5Controller1ButtonBRPPressed;
        public bool PS5Controller1ButtonMicPressed;
        public bool PS5Controller1TouchOn;
        public double PS5Controller1LeftStickX, PS5Controller1LeftStickY, PS5Controller1RightStickX, PS5Controller1RightStickY, PS5Controller1RightTriggerPosition, PS5Controller1LeftTriggerPosition, PS5Controller1TouchX, PS5Controller1TouchY;
        public bool PS5Controller1AccelCenter;
        public double PS5Controller1AccelX, PS5Controller1AccelY, PS5Controller1GyroX, PS5Controller1GyroY;
        public Vector3 gyr1_gPS5 = new Vector3();
        public Vector3 acc1_gPS5 = new Vector3();
        public Vector3 InitDirectAngles1PS5, DirectAngles1PS5;
        private Task<TransferResult> readBuffer1;
        private byte miscByte2;
        private byte btnBlock12, btnBlock22, btnBlock32;
        public bool PS5Controller2ButtonCrossPressed;
        public bool PS5Controller2ButtonCirclePressed;
        public bool PS5Controller2ButtonSquarePressed;
        public bool PS5Controller2ButtonTrianglePressed;
        public bool PS5Controller2ButtonDPadUpPressed;
        public bool PS5Controller2ButtonDPadRightPressed;
        public bool PS5Controller2ButtonDPadDownPressed;
        public bool PS5Controller2ButtonDPadLeftPressed;
        public bool PS5Controller2ButtonL1Pressed;
        public bool PS5Controller2ButtonR1Pressed;
        public bool PS5Controller2ButtonL2Pressed;
        public bool PS5Controller2ButtonR2Pressed;
        public bool PS5Controller2ButtonL3Pressed;
        public bool PS5Controller2ButtonR3Pressed;
        public bool PS5Controller2ButtonCreatePressed;
        public bool PS5Controller2ButtonMenuPressed;
        public bool PS5Controller2ButtonLogoPressed;
        public bool PS5Controller2ButtonTouchpadPressed;
        public bool PS5Controller2ButtonFnLPressed;
        public bool PS5Controller2ButtonFnRPressed;
        public bool PS5Controller2ButtonBLPPressed;
        public bool PS5Controller2ButtonBRPPressed;
        public bool PS5Controller2ButtonMicPressed;
        public bool PS5Controller2TouchOn;
        public double PS5Controller2LeftStickX, PS5Controller2LeftStickY, PS5Controller2RightStickX, PS5Controller2RightStickY, PS5Controller2RightTriggerPosition, PS5Controller2LeftTriggerPosition, PS5Controller2TouchX, PS5Controller2TouchY;
        public bool PS5Controller2AccelCenter;
        public double PS5Controller2AccelX, PS5Controller2AccelY, PS5Controller2GyroX, PS5Controller2GyroY;
        public Vector3 gyr2_gPS5 = new Vector3();
        public Vector3 acc2_gPS5 = new Vector3();
        public Vector3 InitDirectAngles2PS5, DirectAngles2PS5;
        private Task<TransferResult> readBuffer2;
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
        public async void ScanDualsense(string vendor_id, string product_id, string label_id)
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
            LeftAnalogStick1 = ReadAnalogStick1(dsdata1[0], dsdata1[1]);
            RightAnalogStick1 = ReadAnalogStick1(dsdata1[2], dsdata1[3]);
            L21 = GetModeSwitch1(dsdata1, 4).ToUnsignedFloat();
            R21 = GetModeSwitch1(dsdata1, 5).ToUnsignedFloat();
            btnBlock11 = GetModeSwitch1(dsdata1, 7);
            btnBlock21 = GetModeSwitch1(dsdata1, 8);
            btnBlock31 = GetModeSwitch1(dsdata1, 9);
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
            FnL1 = btnBlock31.HasFlag(1 << 4);
            FnR1 = btnBlock31.HasFlag(1 << 5);
            BLP1 = btnBlock31.HasFlag(1 << 6);
            BRP1 = btnBlock31.HasFlag(1 << 7);
            MicButton1 = GetModeSwitch1(dsdata1, 9).HasFlag(0x04);
            Touchpad11 = ReadTouchpad1(GetModeSwitch1(dsdata1, 32, 4));
            Touchpad21 = ReadTouchpad1(GetModeSwitch1(dsdata1, 36, 4));
            Gyro1 = -ReadAccelAxes1(
                GetModeSwitch1(dsdata1, 15, 2),
                GetModeSwitch1(dsdata1, 17, 2),
                GetModeSwitch1(dsdata1, 19, 2)
            );
            Accelerometer1 = ReadAccelAxes1(
                GetModeSwitch1(dsdata1, 21, 2),
                GetModeSwitch1(dsdata1, 23, 2),
                GetModeSwitch1(dsdata1, 25, 2)
            );
            miscByte1 = GetModeSwitch1(dsdata1, 53);
            IsHeadphoneConnected1 = miscByte1.HasFlag(0x01);
            PS5Controller1LeftStickX = LeftAnalogStick1.X;
            PS5Controller1LeftStickY = LeftAnalogStick1.Y;
            PS5Controller1RightStickX = -RightAnalogStick1.X;
            PS5Controller1RightStickY = -RightAnalogStick1.Y;
            PS5Controller1LeftTriggerPosition = L21;
            PS5Controller1RightTriggerPosition = R21;
            PS5Controller1TouchX = Touchpad11.X;
            PS5Controller1TouchY = Touchpad11.Y;
            PS5Controller1TouchOn = Touchpad11.IsDown;
            gyr1_gPS5.X = Gyro1.Z;
            gyr1_gPS5.Y = -Gyro1.X;
            gyr1_gPS5.Z = -Gyro1.Y;
            PS5Controller1GyroX = gyr1_gPS5.Z;
            PS5Controller1GyroY = gyr1_gPS5.Y;
            acc1_gPS5 = new Vector3(Accelerometer1.X, Accelerometer1.Z, Accelerometer1.Y);
            PS5Controller1AccelCenter = MenuButton1;
            DirectAngles1PS5 = acc1_gPS5 - InitDirectAngles1PS5;
            PS5Controller1AccelX = -(DirectAngles1PS5.Y + DirectAngles1PS5.Z) / 6f;
            PS5Controller1AccelY = DirectAngles1PS5.X / 6f;
            PS5Controller1ButtonCrossPressed = CrossButton1;
            PS5Controller1ButtonCirclePressed = CircleButton1;
            PS5Controller1ButtonSquarePressed = SquareButton1;
            PS5Controller1ButtonTrianglePressed = TriangleButton1;
            PS5Controller1ButtonDPadUpPressed = DPadUpButton1;
            PS5Controller1ButtonDPadRightPressed = DPadRightButton1;
            PS5Controller1ButtonDPadDownPressed = DPadDownButton1;
            PS5Controller1ButtonDPadLeftPressed = DPadLeftButton1;
            PS5Controller1ButtonL1Pressed = L1Button1;
            PS5Controller1ButtonR1Pressed = R1Button1;
            PS5Controller1ButtonL2Pressed = L2Button1;
            PS5Controller1ButtonR2Pressed = R2Button1;
            PS5Controller1ButtonL3Pressed = L3Button1;
            PS5Controller1ButtonR3Pressed = R3Button1;
            PS5Controller1ButtonCreatePressed = CreateButton1;
            PS5Controller1ButtonMenuPressed = MenuButton1;
            PS5Controller1ButtonLogoPressed = LogoButton1;
            PS5Controller1ButtonTouchpadPressed = TouchpadButton1;
            PS5Controller1ButtonFnLPressed = FnL1;
            PS5Controller1ButtonFnRPressed = FnR1;
            PS5Controller1ButtonBLPPressed = BLP1;
            PS5Controller1ButtonBRPPressed = BRP1;
            PS5Controller1ButtonMicPressed = MicButton1;
        }
        public void ProcessStateLogic2()
        {
            LeftAnalogStick2 = ReadAnalogStick2(dsdata2[0], dsdata2[1]);
            RightAnalogStick2 = ReadAnalogStick2(dsdata2[2], dsdata2[3]);
            L22 = GetModeSwitch2(dsdata2, 4).ToUnsignedFloat();
            R22 = GetModeSwitch2(dsdata2, 5).ToUnsignedFloat();
            btnBlock12 = GetModeSwitch2(dsdata2, 7);
            btnBlock22 = GetModeSwitch2(dsdata2, 8);
            btnBlock32 = GetModeSwitch2(dsdata2, 9);
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
            FnL2 = btnBlock32.HasFlag(1 << 4);
            FnR2 = btnBlock32.HasFlag(1 << 5);
            BLP2 = btnBlock32.HasFlag(1 << 6);
            BRP2 = btnBlock32.HasFlag(1 << 7);
            MicButton2 = GetModeSwitch2(dsdata2, 9).HasFlag(0x04);
            Touchpad12 = ReadTouchpad2(GetModeSwitch2(dsdata2, 32, 4));
            Touchpad22 = ReadTouchpad2(GetModeSwitch2(dsdata2, 36, 4));
            Gyro2 = -ReadAccelAxes2(
                GetModeSwitch2(dsdata2, 15, 2),
                GetModeSwitch2(dsdata2, 17, 2),
                GetModeSwitch2(dsdata2, 19, 2)
            );
            Accelerometer2 = ReadAccelAxes2(
                GetModeSwitch2(dsdata2, 21, 2),
                GetModeSwitch2(dsdata2, 23, 2),
                GetModeSwitch2(dsdata2, 25, 2)
            );
            miscByte2 = GetModeSwitch2(dsdata2, 53);
            IsHeadphoneConnected2 = miscByte2.HasFlag(0x01);
            PS5Controller2LeftStickX = LeftAnalogStick2.X;
            PS5Controller2LeftStickY = LeftAnalogStick2.Y;
            PS5Controller2RightStickX = -RightAnalogStick2.X;
            PS5Controller2RightStickY = -RightAnalogStick2.Y;
            PS5Controller2LeftTriggerPosition = L22;
            PS5Controller2RightTriggerPosition = R22;
            PS5Controller2TouchX = Touchpad12.X;
            PS5Controller2TouchY = Touchpad12.Y;
            PS5Controller2TouchOn = Touchpad12.IsDown;
            gyr2_gPS5.X = Gyro2.Z;
            gyr2_gPS5.Y = -Gyro2.X;
            gyr2_gPS5.Z = -Gyro2.Y;
            PS5Controller2GyroX = gyr2_gPS5.Z;
            PS5Controller2GyroY = gyr2_gPS5.Y;
            acc2_gPS5 = new Vector3(Accelerometer2.X, Accelerometer2.Z, Accelerometer2.Y);
            PS5Controller2AccelCenter = MenuButton2;
            DirectAngles2PS5 = acc2_gPS5 - InitDirectAngles2PS5;
            PS5Controller2AccelX = -(DirectAngles2PS5.Y + DirectAngles2PS5.Z) / 6f;
            PS5Controller2AccelY = DirectAngles2PS5.X / 6f;
            PS5Controller2ButtonCrossPressed = CrossButton2;
            PS5Controller2ButtonCirclePressed = CircleButton2;
            PS5Controller2ButtonSquarePressed = SquareButton2;
            PS5Controller2ButtonTrianglePressed = TriangleButton2;
            PS5Controller2ButtonDPadUpPressed = DPadUpButton2;
            PS5Controller2ButtonDPadRightPressed = DPadRightButton2;
            PS5Controller2ButtonDPadDownPressed = DPadDownButton2;
            PS5Controller2ButtonDPadLeftPressed = DPadLeftButton2;
            PS5Controller2ButtonL1Pressed = L1Button2;
            PS5Controller2ButtonR1Pressed = R1Button2;
            PS5Controller2ButtonL2Pressed = L2Button2;
            PS5Controller2ButtonR2Pressed = R2Button2;
            PS5Controller2ButtonL3Pressed = L3Button2;
            PS5Controller2ButtonR3Pressed = R3Button2;
            PS5Controller2ButtonCreatePressed = CreateButton2;
            PS5Controller2ButtonMenuPressed = MenuButton2;
            PS5Controller2ButtonLogoPressed = LogoButton2;
            PS5Controller2ButtonTouchpadPressed = TouchpadButton2;
            PS5Controller2ButtonFnLPressed = FnL2;
            PS5Controller2ButtonFnRPressed = FnR2;
            PS5Controller2ButtonBLPPressed = BLP2;
            PS5Controller2ButtonBRPPressed = BRP2;
            PS5Controller2ButtonMicPressed = MicButton2;
        }
        public void InitDualSenseAccel1()
        {
            InitDirectAngles1PS5 = acc1_gPS5;
        }
        public void InitDualSenseAccel2()
        {
            InitDirectAngles2PS5 = acc2_gPS5;
        }
        private async void taskD1()
        {
            for (; ; )
            {
                if (!running)
                    break;
                readBuffer1 = trezorDevice1.WriteAndReadAsync(GetOutputDataBytes1());
                readBuffer1.Wait();
                dsdata1 = (await readBuffer1).Data.Skip(1).ToArray();
                ProcessStateLogic1();
                if (formvisible)
                {
                    string str = "PS5Controller1LeftStickX : " + PS5Controller1LeftStickX + Environment.NewLine;
                    str += "PS5Controller1LeftStickY : " + PS5Controller1LeftStickY + Environment.NewLine;
                    str += "PS5Controller1RightStickX : " + PS5Controller1RightStickX + Environment.NewLine;
                    str += "PS5Controller1RightStickY : " + PS5Controller1RightStickY + Environment.NewLine;
                    str += "PS5Controller1LeftTriggerPosition : " + PS5Controller1LeftTriggerPosition + Environment.NewLine;
                    str += "PS5Controller1RightTriggerPosition : " + PS5Controller1RightTriggerPosition + Environment.NewLine;
                    str += "PS5Controller1TouchX : " + PS5Controller1TouchX + Environment.NewLine;
                    str += "PS5Controller1TouchY : " + PS5Controller1TouchY + Environment.NewLine;
                    str += "PS5Controller1TouchOn : " + PS5Controller1TouchOn + Environment.NewLine;
                    str += "PS5Controller1GyroX : " + PS5Controller1GyroX + Environment.NewLine;
                    str += "PS5Controller1GyroY : " + PS5Controller1GyroY + Environment.NewLine;
                    str += "PS5Controller1AccelX : " + PS5Controller1AccelX + Environment.NewLine;
                    str += "PS5Controller1AccelY : " + PS5Controller1AccelY + Environment.NewLine;
                    str += "PS5Controller1ButtonCrossPressed : " + PS5Controller1ButtonCrossPressed + Environment.NewLine;
                    str += "PS5Controller1ButtonCirclePressed : " + PS5Controller1ButtonCirclePressed + Environment.NewLine;
                    str += "PS5Controller1ButtonSquarePressed : " + PS5Controller1ButtonSquarePressed + Environment.NewLine;
                    str += "PS5Controller1ButtonTrianglePressed : " + PS5Controller1ButtonTrianglePressed + Environment.NewLine;
                    str += "PS5Controller1ButtonDPadUpPressed : " + PS5Controller1ButtonDPadUpPressed + Environment.NewLine;
                    str += "PS5Controller1ButtonDPadRightPressed : " + PS5Controller1ButtonDPadRightPressed + Environment.NewLine;
                    str += "PS5Controller1ButtonDPadDownPressed : " + PS5Controller1ButtonDPadDownPressed + Environment.NewLine;
                    str += "PS5Controller1ButtonDPadLeftPressed : " + PS5Controller1ButtonDPadLeftPressed + Environment.NewLine;
                    str += "PS5Controller1ButtonL1Pressed : " + PS5Controller1ButtonL1Pressed + Environment.NewLine;
                    str += "PS5Controller1ButtonR1Pressed : " + PS5Controller1ButtonR1Pressed + Environment.NewLine;
                    str += "PS5Controller1ButtonL2Pressed : " + PS5Controller1ButtonL2Pressed + Environment.NewLine;
                    str += "PS5Controller1ButtonR2Pressed : " + PS5Controller1ButtonR2Pressed + Environment.NewLine;
                    str += "PS5Controller1ButtonL3Pressed : " + PS5Controller1ButtonL3Pressed + Environment.NewLine;
                    str += "PS5Controller1ButtonR3Pressed : " + PS5Controller1ButtonR3Pressed + Environment.NewLine;
                    str += "PS5Controller1ButtonCreatePressed : " + PS5Controller1ButtonCreatePressed + Environment.NewLine;
                    str += "PS5Controller1ButtonMenuPressed : " + PS5Controller1ButtonMenuPressed + Environment.NewLine;
                    str += "PS5Controller1ButtonLogoPressed : " + PS5Controller1ButtonLogoPressed + Environment.NewLine;
                    str += "PS5Controller1ButtonTouchpadPressed : " + PS5Controller1ButtonTouchpadPressed + Environment.NewLine;
                    str += "PS5Controller1ButtonFnLPressed : " + PS5Controller1ButtonFnLPressed + Environment.NewLine;
                    str += "PS5Controller1ButtonFnRPressed : " + PS5Controller1ButtonFnRPressed + Environment.NewLine;
                    str += "PS5Controller1ButtonBLPPressed : " + PS5Controller1ButtonBLPPressed + Environment.NewLine;
                    str += "PS5Controller1ButtonBRPPressed : " + PS5Controller1ButtonBRPPressed + Environment.NewLine;
                    str += "PS5Controller1ButtonMicPressed : " + PS5Controller1ButtonMicPressed + Environment.NewLine;
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
                dsdata2 = (await readBuffer2).Data.Skip(1).ToArray();
                ProcessStateLogic2();
                if (formvisible)
                {
                    string str = "PS5Controller2LeftStickX : " + PS5Controller2LeftStickX + Environment.NewLine;
                    str += "PS5Controller2LeftStickY : " + PS5Controller2LeftStickY + Environment.NewLine;
                    str += "PS5Controller2RightStickX : " + PS5Controller2RightStickX + Environment.NewLine;
                    str += "PS5Controller2RightStickY : " + PS5Controller2RightStickY + Environment.NewLine;
                    str += "PS5Controller2LeftTriggerPosition : " + PS5Controller2LeftTriggerPosition + Environment.NewLine;
                    str += "PS5Controller2RightTriggerPosition : " + PS5Controller2RightTriggerPosition + Environment.NewLine;
                    str += "PS5Controller2TouchX : " + PS5Controller2TouchX + Environment.NewLine;
                    str += "PS5Controller2TouchY : " + PS5Controller2TouchY + Environment.NewLine;
                    str += "PS5Controller2TouchOn : " + PS5Controller2TouchOn + Environment.NewLine;
                    str += "PS5Controller2GyroX : " + PS5Controller2GyroX + Environment.NewLine;
                    str += "PS5Controller2GyroY : " + PS5Controller2GyroY + Environment.NewLine;
                    str += "PS5Controller2AccelX : " + PS5Controller2AccelX + Environment.NewLine;
                    str += "PS5Controller2AccelY : " + PS5Controller2AccelY + Environment.NewLine;
                    str += "PS5Controller2ButtonCrossPressed : " + PS5Controller2ButtonCrossPressed + Environment.NewLine;
                    str += "PS5Controller2ButtonCirclePressed : " + PS5Controller2ButtonCirclePressed + Environment.NewLine;
                    str += "PS5Controller2ButtonSquarePressed : " + PS5Controller2ButtonSquarePressed + Environment.NewLine;
                    str += "PS5Controller2ButtonTrianglePressed : " + PS5Controller2ButtonTrianglePressed + Environment.NewLine;
                    str += "PS5Controller2ButtonDPadUpPressed : " + PS5Controller2ButtonDPadUpPressed + Environment.NewLine;
                    str += "PS5Controller2ButtonDPadRightPressed : " + PS5Controller2ButtonDPadRightPressed + Environment.NewLine;
                    str += "PS5Controller2ButtonDPadDownPressed : " + PS5Controller2ButtonDPadDownPressed + Environment.NewLine;
                    str += "PS5Controller2ButtonDPadLeftPressed : " + PS5Controller2ButtonDPadLeftPressed + Environment.NewLine;
                    str += "PS5Controller2ButtonL1Pressed : " + PS5Controller2ButtonL1Pressed + Environment.NewLine;
                    str += "PS5Controller2ButtonR1Pressed : " + PS5Controller2ButtonR1Pressed + Environment.NewLine;
                    str += "PS5Controller2ButtonL2Pressed : " + PS5Controller2ButtonL2Pressed + Environment.NewLine;
                    str += "PS5Controller2ButtonR2Pressed : " + PS5Controller2ButtonR2Pressed + Environment.NewLine;
                    str += "PS5Controller2ButtonL3Pressed : " + PS5Controller2ButtonL3Pressed + Environment.NewLine;
                    str += "PS5Controller2ButtonR3Pressed : " + PS5Controller2ButtonR3Pressed + Environment.NewLine;
                    str += "PS5Controller2ButtonCreatePressed : " + PS5Controller2ButtonCreatePressed + Environment.NewLine;
                    str += "PS5Controller2ButtonMenuPressed : " + PS5Controller2ButtonMenuPressed + Environment.NewLine;
                    str += "PS5Controller2ButtonLogoPressed : " + PS5Controller2ButtonLogoPressed + Environment.NewLine;
                    str += "PS5Controller2ButtonTouchpadPressed : " + PS5Controller2ButtonTouchpadPressed + Environment.NewLine;
                    str += "PS5Controller2ButtonFnLPressed : " + PS5Controller2ButtonFnLPressed + Environment.NewLine;
                    str += "PS5Controller2ButtonFnRPressed : " + PS5Controller2ButtonFnRPressed + Environment.NewLine;
                    str += "PS5Controller2ButtonBLPPressed : " + PS5Controller2ButtonBLPPressed + Environment.NewLine;
                    str += "PS5Controller2ButtonBRPPressed : " + PS5Controller2ButtonBRPPressed + Environment.NewLine;
                    str += "PS5Controller2ButtonMicPressed : " + PS5Controller2ButtonMicPressed + Environment.NewLine;
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
            byte[] bytes = new byte[48];
            bytes[0] = 0x02;
            return bytes;
        }
        private static byte GetModeSwitch1(byte[] dsdata, int indexIfUsb)
        {
            return indexIfUsb >= 0 ? dsdata[indexIfUsb] : (byte)0;
        }
        private static byte[] GetModeSwitch1(byte[] dsdata, int startIndexIfUsb, int size)
        {
            return startIndexIfUsb >= 0 ? dsdata.Skip(startIndexIfUsb).Take(size).ToArray() : new byte[size];
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
        public static bool FnL1 { get; private set; }
        public static bool FnR1 { get; private set; }
        public static bool BLP1 { get; private set; }
        public static bool BRP1 { get; private set; }
        public static bool MicButton1 { get; private set; }
        public static DualShock4Touch Touchpad11 { get; private set; }
        public static DualShock4Touch Touchpad21 { get; private set; }
        public static Vec3 Gyro1 { get; private set; }
        public static Vec3 Accelerometer1 { get; private set; }
        public static bool IsHeadphoneConnected1 { get; private set; }
        private static byte[] GetOutputDataBytes2()
        {
            byte[] bytes = new byte[48];
            bytes[0] = 0x02;
            return bytes;
        }
        private static byte GetModeSwitch2(byte[] dsdata, int indexIfUsb)
        {
            return indexIfUsb >= 0 ? dsdata[indexIfUsb] : (byte)0;
        }
        private static byte[] GetModeSwitch2(byte[] dsdata, int startIndexIfUsb, int size)
        {
            return startIndexIfUsb >= 0 ? dsdata.Skip(startIndexIfUsb).Take(size).ToArray() : new byte[size];
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
        public static bool FnL2 { get; private set; }
        public static bool FnR2 { get; private set; }
        public static bool BLP2 { get; private set; }
        public static bool BRP2 { get; private set; }
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