using System;
using vJoy.Wrapper;

namespace controllersvjoy
{
    public class VJoyController
    {
        public static VirtualJoystick joystick1, joystick2;
        public void Connect()
        {
            joystick1 = new VirtualJoystick(1);
            joystick1.Aquire();
            joystick2 = new VirtualJoystick(2);
            joystick2.Aquire();
        }
        public void Disconnect()
        {
            joystick1.Dispose();
            joystick2.Dispose();
        }
        public void SetController1(bool Controller1VJoy_Send_1, bool Controller1VJoy_Send_2, bool Controller1VJoy_Send_3, bool Controller1VJoy_Send_4, bool Controller1VJoy_Send_5, bool Controller1VJoy_Send_6, bool Controller1VJoy_Send_7, bool Controller1VJoy_Send_8, double Controller1VJoy_Send_X, double Controller1VJoy_Send_Y, double Controller1VJoy_Send_Z, double Controller1VJoy_Send_WHL, double Controller1VJoy_Send_SL0, double Controller1VJoy_Send_SL1, double Controller1VJoy_Send_RX, double Controller1VJoy_Send_RY, double Controller1VJoy_Send_RZ, double Controller1VJoy_Send_POV, double Controller1VJoy_Send_Hat, double Controller1VJoy_Send_HatExt1, double Controller1VJoy_Send_HatExt2, double Controller1VJoy_Send_HatExt3)
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
        }
        public void SetController2(bool Controller2VJoy_Send_1, bool Controller2VJoy_Send_2, bool Controller2VJoy_Send_3, bool Controller2VJoy_Send_4, bool Controller2VJoy_Send_5, bool Controller2VJoy_Send_6, bool Controller2VJoy_Send_7, bool Controller2VJoy_Send_8, double Controller2VJoy_Send_X, double Controller2VJoy_Send_Y, double Controller2VJoy_Send_Z, double Controller2VJoy_Send_WHL, double Controller2VJoy_Send_SL0, double Controller2VJoy_Send_SL1, double Controller2VJoy_Send_RX, double Controller2VJoy_Send_RY, double Controller2VJoy_Send_RZ, double Controller2VJoy_Send_POV, double Controller2VJoy_Send_Hat, double Controller2VJoy_Send_HatExt1, double Controller2VJoy_Send_HatExt2, double Controller2VJoy_Send_HatExt3)
        {
            joystick2.SetJoystickButton(Controller2VJoy_Send_1, 1);
            joystick2.SetJoystickButton(Controller2VJoy_Send_2, 2);
            joystick2.SetJoystickButton(Controller2VJoy_Send_3, 3);
            joystick2.SetJoystickButton(Controller2VJoy_Send_4, 4);
            joystick2.SetJoystickButton(Controller2VJoy_Send_5, 5);
            joystick2.SetJoystickButton(Controller2VJoy_Send_6, 6);
            joystick2.SetJoystickButton(Controller2VJoy_Send_7, 7);
            joystick2.SetJoystickButton(Controller2VJoy_Send_8, 8);
            joystick2.SetJoystickAxis((int)Controller2VJoy_Send_X, Axis.HID_USAGE_X);
            joystick2.SetJoystickAxis((int)Controller2VJoy_Send_Y, Axis.HID_USAGE_Y);
            joystick2.SetJoystickAxis((int)Controller2VJoy_Send_Z, Axis.HID_USAGE_Z);
            joystick2.SetJoystickAxis((int)Controller2VJoy_Send_WHL, Axis.HID_USAGE_WHL);
            joystick2.SetJoystickAxis((int)Controller2VJoy_Send_SL0, Axis.HID_USAGE_SL0);
            joystick2.SetJoystickAxis((int)Controller2VJoy_Send_SL1, Axis.HID_USAGE_SL1);
            joystick2.SetJoystickAxis((int)Controller2VJoy_Send_RX, Axis.HID_USAGE_RX);
            joystick2.SetJoystickAxis((int)Controller2VJoy_Send_RY, Axis.HID_USAGE_RY);
            joystick2.SetJoystickAxis((int)Controller2VJoy_Send_RZ, Axis.HID_USAGE_RZ);
            joystick2.SetJoystickAxis((int)Controller2VJoy_Send_POV, Axis.HID_USAGE_POV);
            joystick2.SetJoystickHat((int)Controller2VJoy_Send_Hat, Hats.Hat);
            joystick2.SetJoystickHat((int)Controller2VJoy_Send_HatExt1, Hats.HatExt1);
            joystick2.SetJoystickHat((int)Controller2VJoy_Send_HatExt2, Hats.HatExt2);
            joystick2.SetJoystickHat((int)Controller2VJoy_Send_HatExt3, Hats.HatExt3);
            joystick2.Update();
        }
    }
}