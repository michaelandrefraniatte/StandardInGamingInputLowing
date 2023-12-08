using System;
using vJoy.Wrapper;

namespace controllersvjoy
{
    public class VJoyController
    {
        public static VirtualJoystick joystick1;
        public Form1 form1 = new Form1();
        public void ViewData()
        {
            if (!form1.Visible)
            {
                form1.SetVisible();
            }
        }
        public void Connect()
        {
            joystick1 = new VirtualJoystick(1);
            joystick1.Aquire();
        }
        public void Disconnect()
        {
            joystick1.Dispose();
        }
        public void SubmitReport1(bool Controller1VJoy_Send_1, bool Controller1VJoy_Send_2, bool Controller1VJoy_Send_3, bool Controller1VJoy_Send_4, bool Controller1VJoy_Send_5, bool Controller1VJoy_Send_6, bool Controller1VJoy_Send_7, bool Controller1VJoy_Send_8, double Controller1VJoy_Send_X, double Controller1VJoy_Send_Y, double Controller1VJoy_Send_Z, double Controller1VJoy_Send_WHL, double Controller1VJoy_Send_SL0, double Controller1VJoy_Send_SL1, double Controller1VJoy_Send_RX, double Controller1VJoy_Send_RY, double Controller1VJoy_Send_RZ, double Controller1VJoy_Send_POV, double Controller1VJoy_Send_Hat, double Controller1VJoy_Send_HatExt1, double Controller1VJoy_Send_HatExt2, double Controller1VJoy_Send_HatExt3)
        {
            joystick1.SetJoystickButton(Controller1VJoy_Send_1, 1);
            joystick1.SetJoystickButton(Controller1VJoy_Send_2, 2);
            joystick1.SetJoystickButton(Controller1VJoy_Send_3, 3);
            joystick1.SetJoystickButton(Controller1VJoy_Send_4, 4);
            joystick1.SetJoystickButton(Controller1VJoy_Send_5, 5);
            joystick1.SetJoystickButton(Controller1VJoy_Send_6, 6);
            joystick1.SetJoystickButton(Controller1VJoy_Send_7, 7);
            joystick1.SetJoystickButton(Controller1VJoy_Send_8, 8);
            joystick1.SetJoystickAxis((int)Controller1VJoy_Send_X, Axis.HID_USAGE_X);
            joystick1.SetJoystickAxis((int)Controller1VJoy_Send_Y, Axis.HID_USAGE_Y);
            joystick1.SetJoystickAxis((int)Controller1VJoy_Send_Z, Axis.HID_USAGE_Z);
            joystick1.SetJoystickAxis((int)Controller1VJoy_Send_WHL, Axis.HID_USAGE_WHL);
            joystick1.SetJoystickAxis((int)Controller1VJoy_Send_SL0, Axis.HID_USAGE_SL0);
            joystick1.SetJoystickAxis((int)Controller1VJoy_Send_SL1, Axis.HID_USAGE_SL1);
            joystick1.SetJoystickAxis((int)Controller1VJoy_Send_RX, Axis.HID_USAGE_RX);
            joystick1.SetJoystickAxis((int)Controller1VJoy_Send_RY, Axis.HID_USAGE_RY);
            joystick1.SetJoystickAxis((int)Controller1VJoy_Send_RZ, Axis.HID_USAGE_RZ);
            joystick1.SetJoystickAxis((int)Controller1VJoy_Send_POV, Axis.HID_USAGE_POV);
            joystick1.SetJoystickHat((int)Controller1VJoy_Send_Hat, Hats.Hat);
            joystick1.SetJoystickHat((int)Controller1VJoy_Send_HatExt1, Hats.HatExt1);
            joystick1.SetJoystickHat((int)Controller1VJoy_Send_HatExt2, Hats.HatExt2);
            joystick1.SetJoystickHat((int)Controller1VJoy_Send_HatExt3, Hats.HatExt3);
            joystick1.Update();
            if (form1.Visible)
            {
                string str = "Controller1VJoy_Send_1 : " + Controller1VJoy_Send_1 + Environment.NewLine;
                str += "Controller1VJoy_Send_2 : " + Controller1VJoy_Send_2 + Environment.NewLine;
                str += "Controller1VJoy_Send_3 : " + Controller1VJoy_Send_3 + Environment.NewLine;
                str += "Controller1VJoy_Send_4 : " + Controller1VJoy_Send_4 + Environment.NewLine;
                str += "Controller1VJoy_Send_5 : " + Controller1VJoy_Send_5 + Environment.NewLine;
                str += "Controller1VJoy_Send_6 : " + Controller1VJoy_Send_6 + Environment.NewLine;
                str += "Controller1VJoy_Send_7 : " + Controller1VJoy_Send_7 + Environment.NewLine;
                str += "Controller1VJoy_Send_8 : " + Controller1VJoy_Send_8 + Environment.NewLine;
                str += "Controller1VJoy_Send_X : " + Controller1VJoy_Send_X + Environment.NewLine;
                str += "Controller1VJoy_Send_Y : " + Controller1VJoy_Send_Y + Environment.NewLine;
                str += "Controller1VJoy_Send_Z : " + Controller1VJoy_Send_Z + Environment.NewLine;
                str += "Controller1VJoy_Send_WHL : " + Controller1VJoy_Send_WHL + Environment.NewLine;
                str += "Controller1VJoy_Send_SL0 : " + Controller1VJoy_Send_SL0 + Environment.NewLine;
                str += "Controller1VJoy_Send_SL1 : " + Controller1VJoy_Send_SL1 + Environment.NewLine;
                str += "Controller1VJoy_Send_RX : " + Controller1VJoy_Send_RX + Environment.NewLine;
                str += "Controller1VJoy_Send_RY : " + Controller1VJoy_Send_RY + Environment.NewLine;
                str += "Controller1VJoy_Send_RZ : " + Controller1VJoy_Send_RZ + Environment.NewLine;
                str += "Controller1VJoy_Send_POV : " + Controller1VJoy_Send_POV + Environment.NewLine;
                str += "Controller1VJoy_Send_Hat : " + Controller1VJoy_Send_Hat + Environment.NewLine;
                str += "Controller1VJoy_Send_HatExt1 : " + Controller1VJoy_Send_HatExt1 + Environment.NewLine;
                str += "Controller1VJoy_Send_HatExt2 : " + Controller1VJoy_Send_HatExt2 + Environment.NewLine;
                str += "Controller1VJoy_Send_HatExt3 : " + Controller1VJoy_Send_HatExt3 + Environment.NewLine;
                str += Environment.NewLine;
                form1.SetLabel1(str);
            }
        }
    }
}