using System.Runtime.InteropServices;

namespace controllers
{
    public class Valuechanges
    {
        [DllImport("winmm.dll", EntryPoint = "timeBeginPeriod")]
        private static extern uint TimeBeginPeriod(uint ms);
        [DllImport("winmm.dll", EntryPoint = "timeEndPeriod")]
        private static extern uint TimeEndPeriod(uint ms);
        [DllImport("ntdll.dll", EntryPoint = "NtSetTimerResolution")]
        private static extern void NtSetTimerResolution(uint DesiredResolution, bool SetResolution, ref uint CurrentResolution);
        private static uint CurrentResolution = 0;
        public static double[] _valuechange = { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
        public static double[] _ValueChange = { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
        public Valuechanges()
        {
            TimeBeginPeriod(1);
            NtSetTimerResolution(1, true, ref CurrentResolution);
        }
        public double this[int index]
        {
            get { return _ValueChange[index]; }
            set
            {
                if (_valuechange[index] != value)
                    _ValueChange[index] = value - _valuechange[index];
                else
                    _ValueChange[index] = 0;
                _valuechange[index] = value;
            }
        }
    }
    public class XBoxController
    {
        [DllImport("winmm.dll", EntryPoint = "timeBeginPeriod")]
        private static extern uint TimeBeginPeriod(uint ms);
        [DllImport("winmm.dll", EntryPoint = "timeEndPeriod")]
        private static extern uint TimeEndPeriod(uint ms);
        [DllImport("ntdll.dll", EntryPoint = "NtSetTimerResolution")]
        private static extern void NtSetTimerResolution(uint DesiredResolution, bool SetResolution, ref uint CurrentResolution);
        private static uint CurrentResolution = 0;
        private ScpBus scpBus;
        private X360Controller controller;
        private int number;
        public static Valuechanges ValueChange = new Valuechanges();
        public XBoxController()
        {
            TimeBeginPeriod(1);
            NtSetTimerResolution(1, true, ref CurrentResolution);
        }
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
            Set(false, false, false, false, false, false, false, false, false, false, false, false, false, false, 0, 0, 0, 0, 0, 0, false);
            if (number == 0 | number == 1)
                scpBus.Unplug(1);
            else if (number == 2)
                scpBus.Unplug(2);
        }
        public void Set(bool back, bool start, bool A, bool B, bool X, bool Y, bool up, bool left, bool down, bool right, bool leftstick, bool rightstick, bool leftbumper, bool rightbumper, double leftstickx, double leftsticky, double rightstickx, double rightsticky, double lefttriggerposition, double righttriggerposition, bool xbox)
        {
            ValueChange[0] = back ? 1 : 0;
            if (Valuechanges._ValueChange[0] > 0f)
                controller.Buttons ^= X360Buttons.Back;
            if (Valuechanges._ValueChange[0] < 0f)
                controller.Buttons &= ~X360Buttons.Back;
            ValueChange[1] = start ? 1 : 0;
            if (Valuechanges._ValueChange[1] > 0f)
                controller.Buttons ^= X360Buttons.Start;
            if (Valuechanges._ValueChange[1] < 0f)
                controller.Buttons &= ~X360Buttons.Start;
            ValueChange[2] = A ? 1 : 0;
            if (Valuechanges._ValueChange[2] > 0f)
                controller.Buttons ^= X360Buttons.A;
            if (Valuechanges._ValueChange[2] < 0f)
                controller.Buttons &= ~X360Buttons.A;
            ValueChange[3] = B ? 1 : 0;
            if (Valuechanges._ValueChange[3] > 0f)
                controller.Buttons ^= X360Buttons.B;
            if (Valuechanges._ValueChange[3] < 0f)
                controller.Buttons &= ~X360Buttons.B;
            ValueChange[4] = X ? 1 : 0;
            if (Valuechanges._ValueChange[4] > 0f)
                controller.Buttons ^= X360Buttons.X;
            if (Valuechanges._ValueChange[4] < 0f)
                controller.Buttons &= ~X360Buttons.X;
            ValueChange[5] = Y ? 1 : 0;
            if (Valuechanges._ValueChange[5] > 0f)
                controller.Buttons ^= X360Buttons.Y;
            if (Valuechanges._ValueChange[5] < 0f)
                controller.Buttons &= ~X360Buttons.Y;
            ValueChange[6] = up ? 1 : 0;
            if (Valuechanges._ValueChange[6] > 0f)
                controller.Buttons ^= X360Buttons.Up;
            if (Valuechanges._ValueChange[6] < 0f)
                controller.Buttons &= ~X360Buttons.Up;
            ValueChange[7] = left ? 1 : 0;
            if (Valuechanges._ValueChange[7] > 0f)
                controller.Buttons ^= X360Buttons.Left;
            if (Valuechanges._ValueChange[7] < 0f)
                controller.Buttons &= ~X360Buttons.Left;
            ValueChange[8] = down ? 1 : 0;
            if (Valuechanges._ValueChange[8] > 0f)
                controller.Buttons ^= X360Buttons.Down;
            if (Valuechanges._ValueChange[8] < 0f)
                controller.Buttons &= ~X360Buttons.Down;
            ValueChange[9] = right ? 1 : 0;
            if (Valuechanges._ValueChange[9] > 0f)
                controller.Buttons ^= X360Buttons.Right;
            if (Valuechanges._ValueChange[9] < 0f)
                controller.Buttons &= ~X360Buttons.Right;
            ValueChange[10] = leftstick ? 1 : 0;
            if (Valuechanges._ValueChange[10] > 0f)
                controller.Buttons ^= X360Buttons.LeftStick;
            if (Valuechanges._ValueChange[10] < 0f)
                controller.Buttons &= ~X360Buttons.LeftStick;
            ValueChange[11] = rightstick ? 1 : 0;
            if (Valuechanges._ValueChange[11] > 0f)
                controller.Buttons ^= X360Buttons.RightStick;
            if (Valuechanges._ValueChange[11] < 0f)
                controller.Buttons &= ~X360Buttons.RightStick;
            ValueChange[12] = leftbumper ? 1 : 0;
            if (Valuechanges._ValueChange[12] > 0f)
                controller.Buttons ^= X360Buttons.LeftBumper;
            if (Valuechanges._ValueChange[12] < 0f)
                controller.Buttons &= ~X360Buttons.LeftBumper;
            ValueChange[13] = rightbumper ? 1 : 0;
            if (Valuechanges._ValueChange[13] > 0f)
                controller.Buttons ^= X360Buttons.RightBumper;
            if (Valuechanges._ValueChange[13] < 0f)
                controller.Buttons &= ~X360Buttons.RightBumper;
            ValueChange[14] = xbox ? 1 : 0;
            if (Valuechanges._ValueChange[14] > 0f)
                controller.Buttons ^= X360Buttons.Logo;
            if (Valuechanges._ValueChange[14] < 0f)
                controller.Buttons &= ~X360Buttons.Logo;
            controller.LeftStickX = (short)leftstickx;
            controller.LeftStickY = (short)leftsticky;
            controller.RightStickX = (short)rightstickx;
            controller.RightStickY = (short)rightsticky;
            controller.LeftTrigger = (byte)lefttriggerposition;
            controller.RightTrigger = (byte)righttriggerposition;
            scpBus.Report(number < 2 ? 1 : 2, controller.GetReport());
        }
    }
}