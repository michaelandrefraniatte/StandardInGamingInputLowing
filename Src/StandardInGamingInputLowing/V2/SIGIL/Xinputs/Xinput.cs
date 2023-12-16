using SharpDX.XInput;
using SharpDX;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using Xinputs;

namespace XInputsAPI
{
    public class XInput
    {
        [DllImport("winmm.dll", EntryPoint = "timeBeginPeriod")]
        private static extern uint TimeBeginPeriod(uint ms);
        [DllImport("winmm.dll", EntryPoint = "timeEndPeriod")]
        private static extern uint TimeEndPeriod(uint ms);
        [DllImport("ntdll.dll", EntryPoint = "NtSetTimerResolution")]
        private static extern void NtSetTimerResolution(uint DesiredResolution, bool SetResolution, ref uint CurrentResolution);
        private static uint CurrentResolution = 0;
        private static bool running, formvisible;
        public Form1 form1 = new Form1();
        public XInput()
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
        public void taskD()
        {
            for (; ; )
            {
                if (!running)
                    break;
                ControllerProcess();
                System.Threading.Thread.Sleep(1);
                if (formvisible)
                {
                    string str = "Controller1ButtonAPressed : " + Controller1ButtonAPressed + Environment.NewLine;
                    str += "Controller1ButtonBPressed : " + Controller1ButtonBPressed + Environment.NewLine;
                    str += "Controller1ButtonXPressed : " + Controller1ButtonXPressed + Environment.NewLine;
                    str += "Controller1ButtonYPressed : " + Controller1ButtonYPressed + Environment.NewLine;
                    str += "Controller1ButtonStartPressed : " + Controller1ButtonStartPressed + Environment.NewLine;
                    str += "Controller1ButtonBackPressed : " + Controller1ButtonBackPressed + Environment.NewLine;
                    str += "Controller1ButtonDownPressed : " + Controller1ButtonDownPressed + Environment.NewLine;
                    str += "Controller1ButtonUpPressed : " + Controller1ButtonUpPressed + Environment.NewLine;
                    str += "Controller1ButtonLeftPressed : " + Controller1ButtonLeftPressed + Environment.NewLine;
                    str += "Controller1ButtonRightPressed : " + Controller1ButtonRightPressed + Environment.NewLine;
                    str += "Controller1ButtonShoulderLeftPressed : " + Controller1ButtonShoulderLeftPressed + Environment.NewLine;
                    str += "Controller1ButtonShoulderRightPressed : " + Controller1ButtonShoulderRightPressed + Environment.NewLine;
                    str += "Controller1ThumbpadLeftPressed : " + Controller1ThumbpadLeftPressed + Environment.NewLine;
                    str += "Controller1ThumbpadRightPressed : " + Controller1ThumbpadRightPressed + Environment.NewLine;
                    str += "Controller1TriggerLeftPosition : " + Controller1TriggerLeftPosition + Environment.NewLine;
                    str += "Controller1TriggerRightPosition : " + Controller1TriggerRightPosition + Environment.NewLine;
                    str += "Controller1ThumbLeftX : " + Controller1ThumbLeftX + Environment.NewLine;
                    str += "Controller1ThumbLeftY : " + Controller1ThumbLeftY + Environment.NewLine;
                    str += "Controller1ThumbRightX : " + Controller1ThumbRightX + Environment.NewLine;
                    str += "Controller1ThumbRightY : " + Controller1ThumbRightY + Environment.NewLine;
                    str += "Controller2ButtonAPressed : " + Controller2ButtonAPressed + Environment.NewLine;
                    str += "Controller2ButtonBPressed : " + Controller2ButtonBPressed + Environment.NewLine;
                    str += "Controller2ButtonXPressed : " + Controller2ButtonXPressed + Environment.NewLine;
                    str += "Controller2ButtonYPressed : " + Controller2ButtonYPressed + Environment.NewLine;
                    str += "Controller2ButtonStartPressed : " + Controller2ButtonStartPressed + Environment.NewLine;
                    str += "Controller2ButtonBackPressed : " + Controller2ButtonBackPressed + Environment.NewLine;
                    str += "Controller2ButtonDownPressed : " + Controller2ButtonDownPressed + Environment.NewLine;
                    str += "Controller2ButtonUpPressed : " + Controller2ButtonUpPressed + Environment.NewLine;
                    str += "Controller2ButtonLeftPressed : " + Controller2ButtonLeftPressed + Environment.NewLine;
                    str += "Controller2ButtonRightPressed : " + Controller2ButtonRightPressed + Environment.NewLine;
                    str += "Controller2ButtonShoulderLeftPressed : " + Controller2ButtonShoulderLeftPressed + Environment.NewLine;
                    str += "Controller2ButtonShoulderRightPressed : " + Controller2ButtonShoulderRightPressed + Environment.NewLine;
                    str += "Controller2ThumbpadLeftPressed : " + Controller2ThumbpadLeftPressed + Environment.NewLine;
                    str += "Controller2ThumbpadRightPressed : " + Controller2ThumbpadRightPressed + Environment.NewLine;
                    str += "Controller2TriggerLeftPosition : " + Controller2TriggerLeftPosition + Environment.NewLine;
                    str += "Controller2TriggerRightPosition : " + Controller2TriggerRightPosition + Environment.NewLine;
                    str += "Controller2ThumbLeftX : " + Controller2ThumbLeftX + Environment.NewLine;
                    str += "Controller2ThumbLeftY : " + Controller2ThumbLeftY + Environment.NewLine;
                    str += "Controller2ThumbRightX : " + Controller2ThumbRightX + Environment.NewLine;
                    str += "Controller2ThumbRightY : " + Controller2ThumbRightY + Environment.NewLine;
                    str += Environment.NewLine;
                    form1.SetLabel1(str);
                }
            }
        }
        public void BeginPolling()
        {
            Task.Run(() => taskD());
        }
        private static Controller[] controller = new Controller[] { null };
        private static SharpDX.XInput.State xistate;
        private static int xinum = 0;
        public static bool Controller1ButtonAPressed, Controller2ButtonAPressed;
        public static bool Controller1ButtonBPressed, Controller2ButtonBPressed;
        public static bool Controller1ButtonXPressed, Controller2ButtonXPressed;
        public static bool Controller1ButtonYPressed, Controller2ButtonYPressed;
        public static bool Controller1ButtonStartPressed, Controller2ButtonStartPressed;
        public static bool Controller1ButtonBackPressed, Controller2ButtonBackPressed;
        public static bool Controller1ButtonDownPressed, Controller2ButtonDownPressed;
        public static bool Controller1ButtonUpPressed, Controller2ButtonUpPressed;
        public static bool Controller1ButtonLeftPressed, Controller2ButtonLeftPressed;
        public static bool Controller1ButtonRightPressed, Controller2ButtonRightPressed;
        public static bool Controller1ButtonShoulderLeftPressed, Controller2ButtonShoulderLeftPressed;
        public static bool Controller1ButtonShoulderRightPressed, Controller2ButtonShoulderRightPressed;
        public static bool Controller1ThumbpadLeftPressed, Controller2ThumbpadLeftPressed;
        public static bool Controller1ThumbpadRightPressed, Controller2ThumbpadRightPressed;
        public static double Controller1TriggerLeftPosition, Controller2TriggerLeftPosition;
        public static double Controller1TriggerRightPosition, Controller2TriggerRightPosition;
        public static double Controller1ThumbLeftX, Controller2ThumbLeftX;
        public static double Controller1ThumbLeftY, Controller2ThumbLeftY;
        public static double Controller1ThumbRightX, Controller2ThumbRightX;
        public static double Controller1ThumbRightY, Controller2ThumbRightY;
        public bool ScanXInput()
        {
            try
            {
                controller = new Controller[] { null, null };
                xinum = 0;
                var controllers = new[] { new Controller(UserIndex.One), new Controller(UserIndex.Two) };
                foreach (var selectControler in controllers)
                {
                    if (selectControler.IsConnected)
                    {
                        controller[xinum] = selectControler;
                        xinum++;
                        if (xinum >= 2)
                        {
                            break;
                        }
                    }
                }
            }
            catch { }
            if (controller[0] == null)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
        private void ControllerProcess()
        {
            for (int inc = 0; inc < xinum; inc++)
            {
                xistate = controller[inc].GetState();
                if (inc == 0)
                {
                    if (xistate.Gamepad.Buttons.HasFlag(GamepadButtonFlags.A))
                        Controller1ButtonAPressed = true;
                    else
                        Controller1ButtonAPressed = false;
                    if (xistate.Gamepad.Buttons.HasFlag(GamepadButtonFlags.B))
                        Controller1ButtonBPressed = true;
                    else
                        Controller1ButtonBPressed = false;
                    if (xistate.Gamepad.Buttons.HasFlag(GamepadButtonFlags.X))
                        Controller1ButtonXPressed = true;
                    else
                        Controller1ButtonXPressed = false;
                    if (xistate.Gamepad.Buttons.HasFlag(GamepadButtonFlags.Y))
                        Controller1ButtonYPressed = true;
                    else
                        Controller1ButtonYPressed = false;
                    if (xistate.Gamepad.Buttons.HasFlag(GamepadButtonFlags.Start))
                        Controller1ButtonStartPressed = true;
                    else
                        Controller1ButtonStartPressed = false;
                    if (xistate.Gamepad.Buttons.HasFlag(GamepadButtonFlags.Back))
                        Controller1ButtonBackPressed = true;
                    else
                        Controller1ButtonBackPressed = false;
                    if (xistate.Gamepad.Buttons.HasFlag(GamepadButtonFlags.DPadDown))
                        Controller1ButtonDownPressed = true;
                    else
                        Controller1ButtonDownPressed = false;
                    if (xistate.Gamepad.Buttons.HasFlag(GamepadButtonFlags.DPadUp))
                        Controller1ButtonUpPressed = true;
                    else
                        Controller1ButtonUpPressed = false;
                    if (xistate.Gamepad.Buttons.HasFlag(GamepadButtonFlags.DPadLeft))
                        Controller1ButtonLeftPressed = true;
                    else
                        Controller1ButtonLeftPressed = false;
                    if (xistate.Gamepad.Buttons.HasFlag(GamepadButtonFlags.DPadRight))
                        Controller1ButtonRightPressed = true;
                    else
                        Controller1ButtonRightPressed = false;
                    if (xistate.Gamepad.Buttons.HasFlag(GamepadButtonFlags.LeftShoulder))
                        Controller1ButtonShoulderLeftPressed = true;
                    else
                        Controller1ButtonShoulderLeftPressed = false;
                    if (xistate.Gamepad.Buttons.HasFlag(GamepadButtonFlags.RightShoulder))
                        Controller1ButtonShoulderRightPressed = true;
                    else
                        Controller1ButtonShoulderRightPressed = false;
                    if (xistate.Gamepad.Buttons.HasFlag(GamepadButtonFlags.LeftThumb))
                        Controller1ThumbpadLeftPressed = true;
                    else
                        Controller1ThumbpadLeftPressed = false;
                    if (xistate.Gamepad.Buttons.HasFlag(GamepadButtonFlags.RightThumb))
                        Controller1ThumbpadRightPressed = true;
                    else
                        Controller1ThumbpadRightPressed = false;
                    Controller1TriggerLeftPosition = xistate.Gamepad.LeftTrigger;
                    Controller1TriggerRightPosition = xistate.Gamepad.RightTrigger;
                    Controller1ThumbLeftX = xistate.Gamepad.LeftThumbX;
                    Controller1ThumbLeftY = xistate.Gamepad.LeftThumbY;
                    Controller1ThumbRightX = xistate.Gamepad.RightThumbX;
                    Controller1ThumbRightY = xistate.Gamepad.RightThumbY;
                }
                if (inc == 1)
                {
                    if (xistate.Gamepad.Buttons.HasFlag(GamepadButtonFlags.A))
                        Controller2ButtonAPressed = true;
                    else
                        Controller2ButtonAPressed = false;
                    if (xistate.Gamepad.Buttons.HasFlag(GamepadButtonFlags.B))
                        Controller2ButtonBPressed = true;
                    else
                        Controller2ButtonBPressed = false;
                    if (xistate.Gamepad.Buttons.HasFlag(GamepadButtonFlags.X))
                        Controller2ButtonXPressed = true;
                    else
                        Controller2ButtonXPressed = false;
                    if (xistate.Gamepad.Buttons.HasFlag(GamepadButtonFlags.Y))
                        Controller2ButtonYPressed = true;
                    else
                        Controller2ButtonYPressed = false;
                    if (xistate.Gamepad.Buttons.HasFlag(GamepadButtonFlags.Start))
                        Controller2ButtonStartPressed = true;
                    else
                        Controller2ButtonStartPressed = false;
                    if (xistate.Gamepad.Buttons.HasFlag(GamepadButtonFlags.Back))
                        Controller2ButtonBackPressed = true;
                    else
                        Controller2ButtonBackPressed = false;
                    if (xistate.Gamepad.Buttons.HasFlag(GamepadButtonFlags.DPadDown))
                        Controller2ButtonDownPressed = true;
                    else
                        Controller2ButtonDownPressed = false;
                    if (xistate.Gamepad.Buttons.HasFlag(GamepadButtonFlags.DPadUp))
                        Controller2ButtonUpPressed = true;
                    else
                        Controller2ButtonUpPressed = false;
                    if (xistate.Gamepad.Buttons.HasFlag(GamepadButtonFlags.DPadLeft))
                        Controller2ButtonLeftPressed = true;
                    else
                        Controller2ButtonLeftPressed = false;
                    if (xistate.Gamepad.Buttons.HasFlag(GamepadButtonFlags.DPadRight))
                        Controller2ButtonRightPressed = true;
                    else
                        Controller2ButtonRightPressed = false;
                    if (xistate.Gamepad.Buttons.HasFlag(GamepadButtonFlags.LeftShoulder))
                        Controller2ButtonShoulderLeftPressed = true;
                    else
                        Controller2ButtonShoulderLeftPressed = false;
                    if (xistate.Gamepad.Buttons.HasFlag(GamepadButtonFlags.RightShoulder))
                        Controller2ButtonShoulderRightPressed = true;
                    else
                        Controller2ButtonShoulderRightPressed = false;
                    if (xistate.Gamepad.Buttons.HasFlag(GamepadButtonFlags.LeftThumb))
                        Controller2ThumbpadLeftPressed = true;
                    else
                        Controller2ThumbpadLeftPressed = false;
                    if (xistate.Gamepad.Buttons.HasFlag(GamepadButtonFlags.RightThumb))
                        Controller2ThumbpadRightPressed = true;
                    else
                        Controller2ThumbpadRightPressed = false;
                    Controller2TriggerLeftPosition = xistate.Gamepad.LeftTrigger;
                    Controller2TriggerRightPosition = xistate.Gamepad.RightTrigger;
                    Controller2ThumbLeftX = xistate.Gamepad.LeftThumbX;
                    Controller2ThumbLeftY = xistate.Gamepad.LeftThumbY;
                    Controller2ThumbRightX = xistate.Gamepad.RightThumbX;
                    Controller2ThumbRightY = xistate.Gamepad.RightThumbY;
                }
            }
        }
    }
}