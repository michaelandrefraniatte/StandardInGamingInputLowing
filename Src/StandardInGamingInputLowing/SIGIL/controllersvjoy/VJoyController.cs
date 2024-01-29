using System.Runtime.InteropServices;
using vJoy.Wrapper;

namespace controllersvjoy
{
    public class VJoyController
    {
        [DllImport("winmm.dll", EntryPoint = "timeBeginPeriod")]
        private static extern uint TimeBeginPeriod(uint ms);
        [DllImport("winmm.dll", EntryPoint = "timeEndPeriod")]
        private static extern uint TimeEndPeriod(uint ms);
        [DllImport("ntdll.dll", EntryPoint = "NtSetTimerResolution")]
        private static extern void NtSetTimerResolution(uint DesiredResolution, bool SetResolution, ref uint CurrentResolution);
        private static uint CurrentResolution = 0;
        public VirtualJoystick joystick;
        public VJoyController()
        {
            TimeBeginPeriod(1);
            NtSetTimerResolution(1, true, ref CurrentResolution);
        }
        public void Connect(int number = 0)
        {
            uint id = (uint)(number < 2 ? 1 : number);
            joystick = new VirtualJoystick(id);
            joystick.Aquire();
        }
        public void Disconnect()
        {
            Set(false, false, false, false, false, false, false, false, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
            joystick.Dispose();
        }
        public void Set(bool ControllerVJoy_Send_1, bool ControllerVJoy_Send_2, bool ControllerVJoy_Send_3, bool ControllerVJoy_Send_4, bool ControllerVJoy_Send_5, bool ControllerVJoy_Send_6, bool ControllerVJoy_Send_7, bool ControllerVJoy_Send_8, double ControllerVJoy_Send_X, double ControllerVJoy_Send_Y, double ControllerVJoy_Send_Z, double ControllerVJoy_Send_WHL, double ControllerVJoy_Send_SL0, double ControllerVJoy_Send_SL1, double ControllerVJoy_Send_RX, double ControllerVJoy_Send_RY, double ControllerVJoy_Send_RZ, double ControllerVJoy_Send_POV, double ControllerVJoy_Send_Hat, double ControllerVJoy_Send_HatExt1, double ControllerVJoy_Send_HatExt2, double ControllerVJoy_Send_HatExt3)
        {
            joystick.SetJoystickButton(ControllerVJoy_Send_1, 1);
            joystick.SetJoystickButton(ControllerVJoy_Send_2, 2);
            joystick.SetJoystickButton(ControllerVJoy_Send_3, 3);
            joystick.SetJoystickButton(ControllerVJoy_Send_4, 4);
            joystick.SetJoystickButton(ControllerVJoy_Send_5, 5);
            joystick.SetJoystickButton(ControllerVJoy_Send_6, 6);
            joystick.SetJoystickButton(ControllerVJoy_Send_7, 7);
            joystick.SetJoystickButton(ControllerVJoy_Send_8, 8);
            joystick.SetJoystickAxis((int)ControllerVJoy_Send_X, Axis.HID_USAGE_X);
            joystick.SetJoystickAxis((int)ControllerVJoy_Send_Y, Axis.HID_USAGE_Y);
            joystick.SetJoystickAxis((int)ControllerVJoy_Send_Z, Axis.HID_USAGE_Z);
            joystick.SetJoystickAxis((int)ControllerVJoy_Send_WHL, Axis.HID_USAGE_WHL);
            joystick.SetJoystickAxis((int)ControllerVJoy_Send_SL0, Axis.HID_USAGE_SL0);
            joystick.SetJoystickAxis((int)ControllerVJoy_Send_SL1, Axis.HID_USAGE_SL1);
            joystick.SetJoystickAxis((int)ControllerVJoy_Send_RX, Axis.HID_USAGE_RX);
            joystick.SetJoystickAxis((int)ControllerVJoy_Send_RY, Axis.HID_USAGE_RY);
            joystick.SetJoystickAxis((int)ControllerVJoy_Send_RZ, Axis.HID_USAGE_RZ);
            joystick.SetJoystickAxis((int)ControllerVJoy_Send_POV, Axis.HID_USAGE_POV);
            joystick.SetJoystickHat((int)ControllerVJoy_Send_Hat, Hats.Hat);
            joystick.SetJoystickHat((int)ControllerVJoy_Send_HatExt1, Hats.HatExt1);
            joystick.SetJoystickHat((int)ControllerVJoy_Send_HatExt2, Hats.HatExt2);
            joystick.SetJoystickHat((int)ControllerVJoy_Send_HatExt3, Hats.HatExt3);
            joystick.Update();
        }
    }
}