using System;
using Nefarius.ViGEm.Client;
using Nefarius.ViGEm.Client.Targets;
using Nefarius.ViGEm.Client.Targets.DualShock4;

namespace controllersds4
{
    public class DS4Controller
    {
        public Form1 form1 = new Form1();
        public static IDualShock4Controller Controller1 { get; set; }
        private static ViGEmClient client1 = new ViGEmClient();
        public void ViewData()
        {
            if (!form1.Visible)
            {
                form1.SetVisible();
            }
        }
        public void Connect()
        {
            Controller1 = client1.CreateDualShock4Controller();
            Controller1.Connect();
        }
        public void Disconnect()
        {
            Controller1.Disconnect();
            client1.Dispose();
        }
        public void SubmitReport1(bool Controller1DS4_Send_Options, bool Controller1DS4_Send_Option, bool Controller1DS4_Send_ThumbLeft, bool Controller1DS4_Send_ThumbRight, bool Controller1DS4_Send_ShoulderLeft, bool Controller1DS4_Send_ShoulderRight, bool Controller1DS4_Send_Cross, bool Controller1DS4_Send_Circle, bool Controller1DS4_Send_Square, bool Controller1DS4_Send_Triangle, bool Controller1DS4_Send_Ps, bool Controller1DS4_Send_Touchpad, bool Controller1DS4_Send_Share, bool Controller1DS4_Send_DPadUp, bool Controller1DS4_Send_DPadDown, bool Controller1DS4_Send_DPadLeft, bool Controller1DS4_Send_DPadRight, double Controller1DS4_Send_LeftThumbX, double Controller1DS4_Send_RightThumbX, double Controller1DS4_Send_LeftThumbY, double Controller1DS4_Send_RightThumbY, bool Controller1DS4_Send_LeftTrigger, bool Controller1DS4_Send_RightTrigger, double Controller1DS4_Send_LeftTriggerPosition, double Controller1DS4_Send_RightTriggerPosition)
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
            if (form1.Visible)
            {
                string str = "Controller1DS4_Send_Options : " + Controller1DS4_Send_Options + Environment.NewLine;
                str += "Controller1DS4_Send_Option : " + Controller1DS4_Send_Option + Environment.NewLine;
                str += "Controller1DS4_Send_ThumbLeft : " + Controller1DS4_Send_ThumbLeft + Environment.NewLine;
                str += "Controller1DS4_Send_ThumbRight : " + Controller1DS4_Send_ThumbRight + Environment.NewLine;
                str += "Controller1DS4_Send_ShoulderLeft : " + Controller1DS4_Send_ShoulderLeft + Environment.NewLine;
                str += "Controller1DS4_Send_ShoulderRight : " + Controller1DS4_Send_ShoulderRight + Environment.NewLine;
                str += "Controller1DS4_Send_Cross : " + Controller1DS4_Send_Cross + Environment.NewLine;
                str += "Controller1DS4_Send_Circle : " + Controller1DS4_Send_Circle + Environment.NewLine;
                str += "Controller1DS4_Send_Square : " + Controller1DS4_Send_Square + Environment.NewLine;
                str += "Controller1DS4_Send_Triangle : " + Controller1DS4_Send_Triangle + Environment.NewLine;
                str += "Controller1DS4_Send_Ps : " + Controller1DS4_Send_Ps + Environment.NewLine;
                str += "Controller1DS4_Send_Touchpad : " + Controller1DS4_Send_Touchpad + Environment.NewLine;
                str += "Controller1DS4_Send_Share : " + Controller1DS4_Send_Share + Environment.NewLine;
                str += "Controller1DS4_Send_DPadUp : " + Controller1DS4_Send_DPadUp + Environment.NewLine;
                str += "Controller1DS4_Send_DPadDown : " + Controller1DS4_Send_DPadDown + Environment.NewLine;
                str += "Controller1DS4_Send_DPadLeft : " + Controller1DS4_Send_DPadLeft + Environment.NewLine;
                str += "Controller1DS4_Send_DPadRight : " + Controller1DS4_Send_DPadRight + Environment.NewLine;
                str += "Controller1DS4_Send_LeftTrigger : " + Controller1DS4_Send_LeftTrigger + Environment.NewLine;
                str += "Controller1DS4_Send_RightTrigger : " + Controller1DS4_Send_RightTrigger + Environment.NewLine;
                str += "Controller1DS4_Send_LeftTriggerPosition : " + Controller1DS4_Send_LeftTriggerPosition + Environment.NewLine;
                str += "Controller1DS4_Send_RightTriggerPosition : " + Controller1DS4_Send_RightTriggerPosition + Environment.NewLine;
                str += "Controller1DS4_Send_LeftThumbX : " + Controller1DS4_Send_LeftThumbX + Environment.NewLine;
                str += "Controller1DS4_Send_RightThumbX : " + Controller1DS4_Send_RightThumbX + Environment.NewLine;
                str += "Controller1DS4_Send_LeftThumbY : " + Controller1DS4_Send_LeftThumbY + Environment.NewLine;
                str += "Controller1DS4_Send_RightThumbY : " + Controller1DS4_Send_RightThumbY + Environment.NewLine;
                str += Environment.NewLine;
                form1.SetLabel1(str);
            }
        }
    }
}