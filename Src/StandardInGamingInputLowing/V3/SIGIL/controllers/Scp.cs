namespace controllers
{
    public class XBoxController
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
        private ScpBus scpBus;
        private X360Controller controller;
        private int number;
        public void Connect(int number = 0)
        {
            this.number = number;
            controller = new X360Controller();
            scpBus = new ScpBus();
            if (number == 0 | number == 1)
                scpBus.PlugIn(1);
            else if (number == 2)
                scpBus.PlugIn(2);
        }
        public void Disconnect()
        {
            SetController(false, false, false, false, false, false, false, false, false, false, false, false, false, false, 0, 0, 0, 0, 0, 0, false);
            if (number == 0 | number == 1)
                scpBus.Unplug(1);
            else if (number == 2)
                scpBus.Unplug(2);
        }
        public void SetController(bool controller_send_back, bool controller_send_start, bool controller_send_A, bool controller_send_B, bool controller_send_X, bool controller_send_Y, bool controller_send_up, bool controller_send_left, bool controller_send_down, bool controller_send_right, bool controller_send_leftstick, bool controller_send_rightstick, bool controller_send_leftbumper, bool controller_send_rightbumper, double controller_send_leftstickx, double controller_send_leftsticky, double controller_send_rightstickx, double controller_send_rightsticky, double controller_send_lefttriggerposition, double controller_send_righttriggerposition, bool controller_send_xbox)
        {
            valchanged(1, controller_send_back);
            if (wd[1] == 1)
                controller.Buttons ^= X360Buttons.Back;
            if (wu[1] == 1)
                controller.Buttons &= ~X360Buttons.Back;
            valchanged(2, controller_send_start);
            if (wd[2] == 1)
                controller.Buttons ^= X360Buttons.Start;
            if (wu[2] == 1)
                controller.Buttons &= ~X360Buttons.Start;
            valchanged(3, controller_send_A);
            if (wd[3] == 1)
                controller.Buttons ^= X360Buttons.A;
            if (wu[3] == 1)
                controller.Buttons &= ~X360Buttons.A;
            valchanged(4, controller_send_B);
            if (wd[4] == 1)
                controller.Buttons ^= X360Buttons.B;
            if (wu[4] == 1)
                controller.Buttons &= ~X360Buttons.B;
            valchanged(5, controller_send_X);
            if (wd[5] == 1)
                controller.Buttons ^= X360Buttons.X;
            if (wu[5] == 1)
                controller.Buttons &= ~X360Buttons.X;
            valchanged(6, controller_send_Y);
            if (wd[6] == 1)
                controller.Buttons ^= X360Buttons.Y;
            if (wu[6] == 1)
                controller.Buttons &= ~X360Buttons.Y;
            valchanged(7, controller_send_up);
            if (wd[7] == 1)
                controller.Buttons ^= X360Buttons.Up;
            if (wu[7] == 1)
                controller.Buttons &= ~X360Buttons.Up;
            valchanged(8, controller_send_left);
            if (wd[8] == 1)
                controller.Buttons ^= X360Buttons.Left;
            if (wu[8] == 1)
                controller.Buttons &= ~X360Buttons.Left;
            valchanged(9, controller_send_down);
            if (wd[9] == 1)
                controller.Buttons ^= X360Buttons.Down;
            if (wu[9] == 1)
                controller.Buttons &= ~X360Buttons.Down;
            valchanged(10, controller_send_right);
            if (wd[10] == 1)
                controller.Buttons ^= X360Buttons.Right;
            if (wu[10] == 1)
                controller.Buttons &= ~X360Buttons.Right;
            valchanged(11, controller_send_leftstick);
            if (wd[11] == 1)
                controller.Buttons ^= X360Buttons.LeftStick;
            if (wu[11] == 1)
                controller.Buttons &= ~X360Buttons.LeftStick;
            valchanged(12, controller_send_rightstick);
            if (wd[12] == 1)
                controller.Buttons ^= X360Buttons.RightStick;
            if (wu[12] == 1)
                controller.Buttons &= ~X360Buttons.RightStick;
            valchanged(13, controller_send_leftbumper);
            if (wd[13] == 1)
                controller.Buttons ^= X360Buttons.LeftBumper;
            if (wu[13] == 1)
                controller.Buttons &= ~X360Buttons.LeftBumper;
            valchanged(14, controller_send_rightbumper);
            if (wd[14] == 1)
                controller.Buttons ^= X360Buttons.RightBumper;
            if (wu[14] == 1)
                controller.Buttons &= ~X360Buttons.RightBumper;
            controller.LeftStickX = (short)controller_send_leftstickx;
            controller.LeftStickY = (short)controller_send_leftsticky;
            controller.RightStickX = (short)controller_send_rightstickx;
            controller.RightStickY = (short)controller_send_rightsticky;
            controller.LeftTrigger = (byte)controller_send_lefttriggerposition;
            controller.RightTrigger = (byte)controller_send_righttriggerposition;
            valchanged(33, controller_send_xbox);
            if (wd[33] == 1)
                controller.Buttons ^= X360Buttons.Logo;
            if (wu[33] == 1)
                controller.Buttons &= ~X360Buttons.Logo;
            scpBus.Report(number < 2 ? 1 : 2, controller.GetReport());
        }
    }
}