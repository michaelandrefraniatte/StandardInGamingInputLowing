using System.Threading;

namespace controllers
{
    public class Scp
    {
        public static int[] wd = { 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2 };
        public static int[] wu = { 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2 };
        public static void valchanged(int n, bool val)
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
        private static XBoxController scpBus;
        private static X360Controller controller1, controller2;
        public static void Connect()
        {
            scpBus = new XBoxController();
            scpBus.PlugIn(1);
            controller1 = new X360Controller();
            scpBus.PlugIn(2);
            controller2 = new X360Controller();
        }
        public static void Disconnect()
        {
            UnLoadXC();
            scpBus.Unplug(1);
            scpBus.Unplug(2);
        }
        public static void UnLoadXC()
        {
            SetController1(false, false, false, false, false, false, false, false, false, false, false, false, false, false, 0, 0, 0, 0, 0, 0, false);
            SetController2(false, false, false, false, false, false, false, false, false, false, false, false, false, false, 0, 0, 0, 0, 0, 0, false);
        }
        public static void SetController1(bool controller1_send_back, bool controller1_send_start, bool controller1_send_A, bool controller1_send_B, bool controller1_send_X, bool controller1_send_Y, bool controller1_send_up, bool controller1_send_left, bool controller1_send_down, bool controller1_send_right, bool controller1_send_leftstick, bool controller1_send_rightstick, bool controller1_send_leftbumper, bool controller1_send_rightbumper, double controller1_send_leftstickx, double controller1_send_leftsticky, double controller1_send_rightstickx, double controller1_send_rightsticky, double controller1_send_lefttriggerposition, double controller1_send_righttriggerposition, bool controller1_send_xbox)
        {
            valchanged(1, controller1_send_back);
            if (wd[1] == 1)
                controller1.Buttons ^= X360Buttons.Back;
            if (wu[1] == 1)
                controller1.Buttons &= ~X360Buttons.Back;
            valchanged(2, controller1_send_start);
            if (wd[2] == 1)
                controller1.Buttons ^= X360Buttons.Start;
            if (wu[2] == 1)
                controller1.Buttons &= ~X360Buttons.Start;
            valchanged(3, controller1_send_A);
            if (wd[3] == 1)
                controller1.Buttons ^= X360Buttons.A;
            if (wu[3] == 1)
                controller1.Buttons &= ~X360Buttons.A;
            valchanged(4, controller1_send_B);
            if (wd[4] == 1)
                controller1.Buttons ^= X360Buttons.B;
            if (wu[4] == 1)
                controller1.Buttons &= ~X360Buttons.B;
            valchanged(5, controller1_send_X);
            if (wd[5] == 1)
                controller1.Buttons ^= X360Buttons.X;
            if (wu[5] == 1)
                controller1.Buttons &= ~X360Buttons.X;
            valchanged(6, controller1_send_Y);
            if (wd[6] == 1)
                controller1.Buttons ^= X360Buttons.Y;
            if (wu[6] == 1)
                controller1.Buttons &= ~X360Buttons.Y;
            valchanged(7, controller1_send_up);
            if (wd[7] == 1)
                controller1.Buttons ^= X360Buttons.Up;
            if (wu[7] == 1)
                controller1.Buttons &= ~X360Buttons.Up;
            valchanged(8, controller1_send_left);
            if (wd[8] == 1)
                controller1.Buttons ^= X360Buttons.Left;
            if (wu[8] == 1)
                controller1.Buttons &= ~X360Buttons.Left;
            valchanged(9, controller1_send_down);
            if (wd[9] == 1)
                controller1.Buttons ^= X360Buttons.Down;
            if (wu[9] == 1)
                controller1.Buttons &= ~X360Buttons.Down;
            valchanged(10, controller1_send_right);
            if (wd[10] == 1)
                controller1.Buttons ^= X360Buttons.Right;
            if (wu[10] == 1)
                controller1.Buttons &= ~X360Buttons.Right;
            valchanged(11, controller1_send_leftstick);
            if (wd[11] == 1)
                controller1.Buttons ^= X360Buttons.LeftStick;
            if (wu[11] == 1)
                controller1.Buttons &= ~X360Buttons.LeftStick;
            valchanged(12, controller1_send_rightstick);
            if (wd[12] == 1)
                controller1.Buttons ^= X360Buttons.RightStick;
            if (wu[12] == 1)
                controller1.Buttons &= ~X360Buttons.RightStick;
            valchanged(13, controller1_send_leftbumper);
            if (wd[13] == 1)
                controller1.Buttons ^= X360Buttons.LeftBumper;
            if (wu[13] == 1)
                controller1.Buttons &= ~X360Buttons.LeftBumper;
            valchanged(14, controller1_send_rightbumper);
            if (wd[14] == 1)
                controller1.Buttons ^= X360Buttons.RightBumper;
            if (wu[14] == 1)
                controller1.Buttons &= ~X360Buttons.RightBumper;
            controller1.LeftStickX = (short)controller1_send_leftstickx;
            controller1.LeftStickY = (short)controller1_send_leftsticky;
            controller1.RightStickX = (short)controller1_send_rightstickx;
            controller1.RightStickY = (short)controller1_send_rightsticky;
            controller1.LeftTrigger = (byte)controller1_send_lefttriggerposition;
            controller1.RightTrigger = (byte)controller1_send_righttriggerposition;
            valchanged(33, controller1_send_xbox);
            if (wd[33] == 1)
                controller1.Buttons ^= X360Buttons.Logo;
            if (wu[33] == 1)
                controller1.Buttons &= ~X360Buttons.Logo;
            scpBus.Report(1, controller1.GetReport());
        }
        public static void SetController2(bool controller2_send_back, bool controller2_send_start, bool controller2_send_A, bool controller2_send_B, bool controller2_send_X, bool controller2_send_Y, bool controller2_send_up, bool controller2_send_left, bool controller2_send_down, bool controller2_send_right, bool controller2_send_leftstick, bool controller2_send_rightstick, bool controller2_send_leftbumper, bool controller2_send_rightbumper, double controller2_send_leftstickx, double controller2_send_leftsticky, double controller2_send_rightstickx, double controller2_send_rightsticky, double controller2_send_lefttriggerposition, double controller2_send_righttriggerposition, bool controller2_send_xbox)
        {
            valchanged(15, controller2_send_back);
            if (wd[15] == 1)
                controller2.Buttons ^= X360Buttons.Back;
            if (wu[15] == 1)
                controller2.Buttons &= ~X360Buttons.Back;
            valchanged(16, controller2_send_start);
            if (wd[16] == 1)
                controller2.Buttons ^= X360Buttons.Start;
            if (wu[16] == 1)
                controller2.Buttons &= ~X360Buttons.Start;
            valchanged(17, controller2_send_A);
            if (wd[17] == 1)
                controller2.Buttons ^= X360Buttons.A;
            if (wu[17] == 1)
                controller2.Buttons &= ~X360Buttons.A;
            valchanged(18, controller2_send_B);
            if (wd[18] == 1)
                controller2.Buttons ^= X360Buttons.B;
            if (wu[18] == 1)
                controller2.Buttons &= ~X360Buttons.B;
            valchanged(19, controller2_send_X);
            if (wd[19] == 1)
                controller2.Buttons ^= X360Buttons.X;
            if (wu[19] == 1)
                controller2.Buttons &= ~X360Buttons.X;
            valchanged(20, controller2_send_Y);
            if (wd[20] == 1)
                controller2.Buttons ^= X360Buttons.Y;
            if (wu[20] == 1)
                controller2.Buttons &= ~X360Buttons.Y;
            valchanged(21, controller2_send_up);
            if (wd[21] == 1)
                controller2.Buttons ^= X360Buttons.Up;
            if (wu[21] == 1)
                controller2.Buttons &= ~X360Buttons.Up;
            valchanged(22, controller2_send_left);
            if (wd[22] == 1)
                controller2.Buttons ^= X360Buttons.Left;
            if (wu[22] == 1)
                controller2.Buttons &= ~X360Buttons.Left;
            valchanged(23, controller2_send_down);
            if (wd[23] == 1)
                controller2.Buttons ^= X360Buttons.Down;
            if (wu[23] == 1)
                controller2.Buttons &= ~X360Buttons.Down;
            valchanged(24, controller2_send_right);
            if (wd[24] == 1)
                controller2.Buttons ^= X360Buttons.Right;
            if (wu[24] == 1)
                controller2.Buttons &= ~X360Buttons.Right;
            valchanged(25, controller2_send_leftstick);
            if (wd[25] == 1)
                controller2.Buttons ^= X360Buttons.LeftStick;
            if (wu[25] == 1)
                controller2.Buttons &= ~X360Buttons.LeftStick;
            valchanged(26, controller2_send_rightstick);
            if (wd[26] == 1)
                controller2.Buttons ^= X360Buttons.RightStick;
            if (wu[26] == 1)
                controller2.Buttons &= ~X360Buttons.RightStick;
            valchanged(27, controller2_send_leftbumper);
            if (wd[27] == 1)
                controller2.Buttons ^= X360Buttons.LeftBumper;
            if (wu[27] == 1)
                controller2.Buttons &= ~X360Buttons.LeftBumper;
            valchanged(28, controller2_send_rightbumper);
            if (wd[28] == 1)
                controller2.Buttons ^= X360Buttons.RightBumper;
            if (wu[28] == 1)
                controller2.Buttons &= ~X360Buttons.RightBumper;
            controller2.LeftStickX = (short)controller2_send_leftstickx;
            controller2.LeftStickY = (short)controller2_send_leftsticky;
            controller2.RightStickX = (short)controller2_send_rightstickx;
            controller2.RightStickY = (short)controller2_send_rightsticky;
            controller2.LeftTrigger = (byte)controller2_send_lefttriggerposition;
            controller2.RightTrigger = (byte)controller2_send_righttriggerposition;
            valchanged(34, controller2_send_xbox);
            if (wd[34] == 1)
                controller2.Buttons ^= X360Buttons.Logo;
            if (wu[34] == 1)
                controller2.Buttons &= ~X360Buttons.Logo;
            scpBus.Report(2, controller2.GetReport());
        }
    }
}