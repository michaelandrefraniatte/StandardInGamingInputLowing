using System;
using Nefarius.ViGEm.Client;
using Nefarius.ViGEm.Client.Targets;
using Nefarius.ViGEm.Client.Targets.DualShock4;

namespace controllersds4
{
    public class DS4Controller
    {
        public static IDualShock4Controller Controller1 { get; set; }
        public static IDualShock4Controller Controller2 { get; set; }
        private static ViGEmClient client1 = new ViGEmClient(), client2 = new ViGEmClient();
        public void Connect()
        {
            Controller1 = client1.CreateDualShock4Controller();
            Controller1.Connect();
            Controller2 = client2.CreateDualShock4Controller();
            Controller2.Connect();
        }
        public void Disconnect()
        {
            Controller1.Disconnect();
            client1.Dispose();
            Controller2.Disconnect();
            client2.Dispose();
        }
        public void SetController1(bool Controller1DS4_Send_Options, bool Controller1DS4_Send_Option, bool Controller1DS4_Send_ThumbLeft, bool Controller1DS4_Send_ThumbRight, bool Controller1DS4_Send_ShoulderLeft, bool Controller1DS4_Send_ShoulderRight, bool Controller1DS4_Send_Cross, bool Controller1DS4_Send_Circle, bool Controller1DS4_Send_Square, bool Controller1DS4_Send_Triangle, bool Controller1DS4_Send_Ps, bool Controller1DS4_Send_Touchpad, bool Controller1DS4_Send_Share, bool Controller1DS4_Send_DPadUp, bool Controller1DS4_Send_DPadDown, bool Controller1DS4_Send_DPadLeft, bool Controller1DS4_Send_DPadRight, double Controller1DS4_Send_LeftThumbX, double Controller1DS4_Send_RightThumbX, double Controller1DS4_Send_LeftThumbY, double Controller1DS4_Send_RightThumbY, bool Controller1DS4_Send_LeftTrigger, bool Controller1DS4_Send_RightTrigger, double Controller1DS4_Send_LeftTriggerPosition, double Controller1DS4_Send_RightTriggerPosition)
        {
            Controller1.SetButtonState(DualShock4Button.Options, Controller1DS4_Send_Options);
            Controller1.SetButtonState(DualShock4SpecialButton.Options, Controller1DS4_Send_Option);
            Controller1.SetButtonState(DualShock4Button.ThumbLeft, Controller1DS4_Send_ThumbLeft);
            Controller1.SetButtonState(DualShock4Button.ThumbRight, Controller1DS4_Send_ThumbRight);
            Controller1.SetButtonState(DualShock4Button.ShoulderLeft, Controller1DS4_Send_ShoulderLeft);
            Controller1.SetButtonState(DualShock4Button.ShoulderRight, Controller1DS4_Send_ShoulderRight);
            Controller1.SetButtonState(DualShock4Button.Cross, Controller1DS4_Send_Cross);
            Controller1.SetButtonState(DualShock4Button.Circle, Controller1DS4_Send_Circle);
            Controller1.SetButtonState(DualShock4Button.Square, Controller1DS4_Send_Square);
            Controller1.SetButtonState(DualShock4Button.Triangle, Controller1DS4_Send_Triangle);
            Controller1.SetButtonState(DualShock4SpecialButton.Ps, Controller1DS4_Send_Ps);
            Controller1.SetButtonState(DualShock4SpecialButton.Touchpad, Controller1DS4_Send_Touchpad);
            Controller1.SetButtonState(DualShock4SpecialButton.Share, Controller1DS4_Send_Share);
            Controller1.SetDPadDirection(DualShock4DPadDirection.None);
            if (Controller1DS4_Send_DPadUp)
            {
                Controller1.SetDPadDirection(DualShock4DPadDirection.North);
            }
            if (Controller1DS4_Send_DPadDown)
            {
                Controller1.SetDPadDirection(DualShock4DPadDirection.South);
            }
            if (Controller1DS4_Send_DPadLeft)
            {
                Controller1.SetDPadDirection(DualShock4DPadDirection.West);
            }
            if (Controller1DS4_Send_DPadRight)
            {
                Controller1.SetDPadDirection(DualShock4DPadDirection.East);
            }
            if (Controller1DS4_Send_DPadUp & Controller1DS4_Send_DPadLeft)
            {
                Controller1.SetDPadDirection(DualShock4DPadDirection.Northwest);
            }
            else if (Controller1DS4_Send_DPadUp & Controller1DS4_Send_DPadRight)
            {
                Controller1.SetDPadDirection(DualShock4DPadDirection.Northeast);
            }
            else if (Controller1DS4_Send_DPadDown & Controller1DS4_Send_DPadLeft)
            {
                Controller1.SetDPadDirection(DualShock4DPadDirection.Southwest);
            }
            else if (Controller1DS4_Send_DPadDown & Controller1DS4_Send_DPadRight)
            {
                Controller1.SetDPadDirection(DualShock4DPadDirection.Southeast);
            }
            Int16 ltx = (Int16)((float)Controller1DS4_Send_LeftThumbX / short.MaxValue * 127);
            Int16 lty = (Int16)((float)Controller1DS4_Send_LeftThumbY / short.MaxValue * -127);
            Int16 rtx = (Int16)((float)Controller1DS4_Send_RightThumbX / short.MaxValue * 127);
            Int16 rty = (Int16)((float)Controller1DS4_Send_RightThumbY / short.MaxValue * -127);
            Controller1.SetAxisValue(DualShock4Axis.LeftThumbX, (byte)(ltx + 0x7f));
            Controller1.SetAxisValue(DualShock4Axis.LeftThumbY, (byte)(lty + 0x7f));
            Controller1.SetAxisValue(DualShock4Axis.RightThumbX, (byte)(rtx + 0x7f));
            Controller1.SetAxisValue(DualShock4Axis.RightThumbY, (byte)(rty + 0x7f));
            Controller1.SetButtonState(DualShock4Button.TriggerLeft, Controller1DS4_Send_LeftTrigger);
            Controller1.SetSliderValue(DualShock4Slider.LeftTrigger, (byte)Controller1DS4_Send_LeftTriggerPosition);
            Controller1.SetButtonState(DualShock4Button.TriggerRight, Controller1DS4_Send_RightTrigger);
            Controller1.SetSliderValue(DualShock4Slider.RightTrigger, (byte)Controller1DS4_Send_RightTriggerPosition);
            Controller1.SubmitReport();
        }
        public void SetController2(bool Controller2DS4_Send_Options, bool Controller2DS4_Send_Option, bool Controller2DS4_Send_ThumbLeft, bool Controller2DS4_Send_ThumbRight, bool Controller2DS4_Send_ShoulderLeft, bool Controller2DS4_Send_ShoulderRight, bool Controller2DS4_Send_Cross, bool Controller2DS4_Send_Circle, bool Controller2DS4_Send_Square, bool Controller2DS4_Send_Triangle, bool Controller2DS4_Send_Ps, bool Controller2DS4_Send_Touchpad, bool Controller2DS4_Send_Share, bool Controller2DS4_Send_DPadUp, bool Controller2DS4_Send_DPadDown, bool Controller2DS4_Send_DPadLeft, bool Controller2DS4_Send_DPadRight, double Controller2DS4_Send_LeftThumbX, double Controller2DS4_Send_RightThumbX, double Controller2DS4_Send_LeftThumbY, double Controller2DS4_Send_RightThumbY, bool Controller2DS4_Send_LeftTrigger, bool Controller2DS4_Send_RightTrigger, double Controller2DS4_Send_LeftTriggerPosition, double Controller2DS4_Send_RightTriggerPosition)
        {
            Controller2.SetButtonState(DualShock4Button.Options, Controller2DS4_Send_Options);
            Controller2.SetButtonState(DualShock4SpecialButton.Options, Controller2DS4_Send_Option);
            Controller2.SetButtonState(DualShock4Button.ThumbLeft, Controller2DS4_Send_ThumbLeft);
            Controller2.SetButtonState(DualShock4Button.ThumbRight, Controller2DS4_Send_ThumbRight);
            Controller2.SetButtonState(DualShock4Button.ShoulderLeft, Controller2DS4_Send_ShoulderLeft);
            Controller2.SetButtonState(DualShock4Button.ShoulderRight, Controller2DS4_Send_ShoulderRight);
            Controller2.SetButtonState(DualShock4Button.Cross, Controller2DS4_Send_Cross);
            Controller2.SetButtonState(DualShock4Button.Circle, Controller2DS4_Send_Circle);
            Controller2.SetButtonState(DualShock4Button.Square, Controller2DS4_Send_Square);
            Controller2.SetButtonState(DualShock4Button.Triangle, Controller2DS4_Send_Triangle);
            Controller2.SetButtonState(DualShock4SpecialButton.Ps, Controller2DS4_Send_Ps);
            Controller2.SetButtonState(DualShock4SpecialButton.Touchpad, Controller2DS4_Send_Touchpad);
            Controller2.SetButtonState(DualShock4SpecialButton.Share, Controller2DS4_Send_Share);
            Controller2.SetDPadDirection(DualShock4DPadDirection.None);
            if (Controller2DS4_Send_DPadUp)
            {
                Controller2.SetDPadDirection(DualShock4DPadDirection.North);
            }
            if (Controller2DS4_Send_DPadDown)
            {
                Controller2.SetDPadDirection(DualShock4DPadDirection.South);
            }
            if (Controller2DS4_Send_DPadLeft)
            {
                Controller2.SetDPadDirection(DualShock4DPadDirection.West);
            }
            if (Controller2DS4_Send_DPadRight)
            {
                Controller2.SetDPadDirection(DualShock4DPadDirection.East);
            }
            if (Controller2DS4_Send_DPadUp & Controller2DS4_Send_DPadLeft)
            {
                Controller2.SetDPadDirection(DualShock4DPadDirection.Northwest);
            }
            else if (Controller2DS4_Send_DPadUp & Controller2DS4_Send_DPadRight)
            {
                Controller2.SetDPadDirection(DualShock4DPadDirection.Northeast);
            }
            else if (Controller2DS4_Send_DPadDown & Controller2DS4_Send_DPadLeft)
            {
                Controller2.SetDPadDirection(DualShock4DPadDirection.Southwest);
            }
            else if (Controller2DS4_Send_DPadDown & Controller2DS4_Send_DPadRight)
            {
                Controller2.SetDPadDirection(DualShock4DPadDirection.Southeast);
            }
            Int16 ltx = (Int16)((float)Controller2DS4_Send_LeftThumbX / short.MaxValue * 127);
            Int16 lty = (Int16)((float)Controller2DS4_Send_LeftThumbY / short.MaxValue * -127);
            Int16 rtx = (Int16)((float)Controller2DS4_Send_RightThumbX / short.MaxValue * 127);
            Int16 rty = (Int16)((float)Controller2DS4_Send_RightThumbY / short.MaxValue * -127);
            Controller2.SetAxisValue(DualShock4Axis.LeftThumbX, (byte)(ltx + 0x7f));
            Controller2.SetAxisValue(DualShock4Axis.LeftThumbY, (byte)(lty + 0x7f));
            Controller2.SetAxisValue(DualShock4Axis.RightThumbX, (byte)(rtx + 0x7f));
            Controller2.SetAxisValue(DualShock4Axis.RightThumbY, (byte)(rty + 0x7f));
            Controller2.SetButtonState(DualShock4Button.TriggerLeft, Controller2DS4_Send_LeftTrigger);
            Controller2.SetSliderValue(DualShock4Slider.LeftTrigger, (byte)Controller2DS4_Send_LeftTriggerPosition);
            Controller2.SetButtonState(DualShock4Button.TriggerRight, Controller2DS4_Send_RightTrigger);
            Controller2.SetSliderValue(DualShock4Slider.RightTrigger, (byte)Controller2DS4_Send_RightTriggerPosition);
            Controller2.SubmitReport();
        }
    }
}