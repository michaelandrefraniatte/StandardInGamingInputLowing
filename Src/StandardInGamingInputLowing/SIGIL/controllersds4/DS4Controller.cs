using System.Runtime.InteropServices;

namespace controllersds4
{
    public class DS4Controller
    {
        [DllImport("winmm.dll", EntryPoint = "timeBeginPeriod")]
        private static extern uint TimeBeginPeriod(uint ms);
        [DllImport("winmm.dll", EntryPoint = "timeEndPeriod")]
        private static extern uint TimeEndPeriod(uint ms);
        [DllImport("ntdll.dll", EntryPoint = "NtSetTimerResolution")]
        private static extern void NtSetTimerResolution(uint DesiredResolution, bool SetResolution, ref uint CurrentResolution);
        private static uint CurrentResolution = 0;
        public IDualShock4Controller Controller { get; set; }
        private ViGEmClient client = new ViGEmClient();
        private string vendorid = "54C", productid = "9CC";
        public DS4Controller()
        {
            TimeBeginPeriod(1);
            NtSetTimerResolution(1, true, ref CurrentResolution);
        }
        public void Connect(int number = 0)
        {
            Controller = client.CreateDualShock4Controller(ushort.Parse(vendorid, System.Globalization.NumberStyles.HexNumber), ushort.Parse(productid, System.Globalization.NumberStyles.HexNumber));
            Controller.Connect();
        }
        public void Disconnect()
        {
            Set(false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, 0, 0, 0, 0, false, false, 0, 0);
            Controller.Disconnect();
            client.Dispose();
        }
        public void Set(bool ControllerDS4_Send_Options, bool ControllerDS4_Send_ThumbLeft, bool ControllerDS4_Send_ThumbRight, bool ControllerDS4_Send_ShoulderLeft, bool ControllerDS4_Send_ShoulderRight, bool ControllerDS4_Send_Cross, bool ControllerDS4_Send_Circle, bool ControllerDS4_Send_Square, bool ControllerDS4_Send_Triangle, bool ControllerDS4_Send_Ps, bool ControllerDS4_Send_Touchpad, bool ControllerDS4_Send_Share, bool ControllerDS4_Send_DPadUp, bool ControllerDS4_Send_DPadDown, bool ControllerDS4_Send_DPadLeft, bool ControllerDS4_Send_DPadRight, double ControllerDS4_Send_LeftThumbX, double ControllerDS4_Send_RightThumbX, double ControllerDS4_Send_LeftThumbY, double ControllerDS4_Send_RightThumbY, bool ControllerDS4_Send_LeftTrigger, bool ControllerDS4_Send_RightTrigger, double ControllerDS4_Send_LeftTriggerPosition, double ControllerDS4_Send_RightTriggerPosition)
        {
            Controller.SetButtonState(DualShock4Button.Options, ControllerDS4_Send_Options);
            Controller.SetButtonState(DualShock4Button.ThumbLeft, ControllerDS4_Send_ThumbLeft);
            Controller.SetButtonState(DualShock4Button.ThumbRight, ControllerDS4_Send_ThumbRight);
            Controller.SetButtonState(DualShock4Button.ShoulderLeft, ControllerDS4_Send_ShoulderLeft);
            Controller.SetButtonState(DualShock4Button.ShoulderRight, ControllerDS4_Send_ShoulderRight);
            Controller.SetButtonState(DualShock4Button.Cross, ControllerDS4_Send_Cross);
            Controller.SetButtonState(DualShock4Button.Circle, ControllerDS4_Send_Circle);
            Controller.SetButtonState(DualShock4Button.Square, ControllerDS4_Send_Square);
            Controller.SetButtonState(DualShock4Button.Triangle, ControllerDS4_Send_Triangle);
            Controller.SetButtonState(DualShock4SpecialButton.Ps, ControllerDS4_Send_Ps);
            Controller.SetButtonState(DualShock4SpecialButton.Touchpad, ControllerDS4_Send_Touchpad);
            Controller.SetButtonState(DualShock4Button.Share, ControllerDS4_Send_Share);
            Controller.SetDPadDirection(DualShock4DPadDirection.None);
            if (ControllerDS4_Send_DPadUp)
            {
                Controller.SetDPadDirection(DualShock4DPadDirection.North);
            }
            if (ControllerDS4_Send_DPadDown)
            {
                Controller.SetDPadDirection(DualShock4DPadDirection.South);
            }
            if (ControllerDS4_Send_DPadLeft)
            {
                Controller.SetDPadDirection(DualShock4DPadDirection.West);
            }
            if (ControllerDS4_Send_DPadRight)
            {
                Controller.SetDPadDirection(DualShock4DPadDirection.East);
            }
            if (ControllerDS4_Send_DPadUp & ControllerDS4_Send_DPadLeft)
            {
                Controller.SetDPadDirection(DualShock4DPadDirection.Northwest);
            }
            else if (ControllerDS4_Send_DPadUp & ControllerDS4_Send_DPadRight)
            {
                Controller.SetDPadDirection(DualShock4DPadDirection.Northeast);
            }
            else if (ControllerDS4_Send_DPadDown & ControllerDS4_Send_DPadLeft)
            {
                Controller.SetDPadDirection(DualShock4DPadDirection.Southwest);
            }
            else if (ControllerDS4_Send_DPadDown & ControllerDS4_Send_DPadRight)
            {
                Controller.SetDPadDirection(DualShock4DPadDirection.Southeast);
            }
            Controller.SetAxisValue(DualShock4Axis.LeftThumbX, (byte)(((float)ControllerDS4_Send_LeftThumbX / short.MaxValue * 127) + 0x7f));
            Controller.SetAxisValue(DualShock4Axis.LeftThumbY, (byte)(((float)ControllerDS4_Send_LeftThumbY / short.MaxValue * -127) + 0x7f));
            Controller.SetAxisValue(DualShock4Axis.RightThumbX, (byte)(((float)ControllerDS4_Send_RightThumbX / short.MaxValue * 127) + 0x7f));
            Controller.SetAxisValue(DualShock4Axis.RightThumbY, (byte)(((float)ControllerDS4_Send_RightThumbY / short.MaxValue * -127) + 0x7f));
            Controller.SetButtonState(DualShock4Button.TriggerLeft, ControllerDS4_Send_LeftTrigger);
            Controller.SetSliderValue(DualShock4Slider.LeftTrigger, (byte)ControllerDS4_Send_LeftTriggerPosition);
            Controller.SetButtonState(DualShock4Button.TriggerRight, ControllerDS4_Send_RightTrigger);
            Controller.SetSliderValue(DualShock4Slider.RightTrigger, (byte)ControllerDS4_Send_RightTriggerPosition);
            Controller.SubmitReport();
        }
    }
}