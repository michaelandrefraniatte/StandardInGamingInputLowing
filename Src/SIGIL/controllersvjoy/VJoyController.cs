using System.Runtime.InteropServices;
using System.Diagnostics;
using vJoy.Wrapper;
using Valuechanges;
using System.Threading.Tasks;
using System;

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
        private int number;
        private VirtualJoystick joystick;
        private bool formvisible;
        private Form1 form1 = new Form1();
        private Stopwatch PollingRate;
        private double pollingrateperm = 0, pollingratetemp = 0, pollingratedisplay = 0, pollingrate;
        private string inputdelaybutton = "", inputdelay = "", inputdelaytemp = "";
        public Valuechange ValueChange;
        private double delay, elapseddown, elapsedup, elapsed;
        private bool getstate = false;
        private bool[] wd = { false };
        private bool[] wu = { false };
        private bool[] ws = { false };
        private void valchanged(int n, bool val)
        {
            if (val)
            {
                if (!wd[n] & !ws[n])
                {
                    wd[n] = true;
                    ws[n] = true;
                    return;
                }
                if (wd[n] & ws[n])
                {
                    wd[n] = false;
                }
                ws[n] = true;
                wu[n] = false;
            }
            if (!val)
            {
                if (!wu[n] & ws[n])
                {
                    wu[n] = true;
                    ws[n] = false;
                    return;
                }
                if (wu[n] & !ws[n])
                {
                    wu[n] = false;
                }
                ws[n] = false;
                wd[n] = false;
            }
        }
        public VJoyController()
        {
            TimeBeginPeriod(1);
            NtSetTimerResolution(1, true, ref CurrentResolution);
        }
        public void ViewData(string inputdelaybutton = "")
        {
            if (!formvisible)
            {
                PollingRate = new Stopwatch();
                PollingRate.Start();
                ValueChange = new Valuechange();
                this.inputdelaybutton = inputdelaybutton;
                formvisible = true;
                Task.Run(() => form1.SetVisible());
            }
        }
        public void Connect(int number = 0)
        {
            this.number = number;
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
            if (formvisible)
            {
                pollingratedisplay++;
                pollingratetemp = pollingrateperm;
                pollingrateperm = (double)PollingRate.ElapsedTicks / (Stopwatch.Frequency / 1000L);
                if (pollingratedisplay > 300)
                {
                    pollingrate = pollingrateperm - pollingratetemp;
                    pollingratedisplay = 0;
                }
                string str = "ControllerVJoy_Send_1 : " + ControllerVJoy_Send_1 + Environment.NewLine;
                str += "ControllerVJoy_Send_2 : " + ControllerVJoy_Send_2 + Environment.NewLine;
                str += "ControllerVJoy_Send_3 : " + ControllerVJoy_Send_3 + Environment.NewLine;
                str += "ControllerVJoy_Send_4 : " + ControllerVJoy_Send_4 + Environment.NewLine;
                str += "ControllerVJoy_Send_5 : " + ControllerVJoy_Send_5 + Environment.NewLine;
                str += "ControllerVJoy_Send_6 : " + ControllerVJoy_Send_6 + Environment.NewLine;
                str += "ControllerVJoy_Send_7 : " + ControllerVJoy_Send_7 + Environment.NewLine;
                str += "ControllerVJoy_Send_8 : " + ControllerVJoy_Send_8 + Environment.NewLine;
                str += "ControllerVJoy_Send_X : " + ControllerVJoy_Send_X + Environment.NewLine;
                str += "ControllerVJoy_Send_Y : " + ControllerVJoy_Send_Y + Environment.NewLine;
                str += "ControllerVJoy_Send_Z : " + ControllerVJoy_Send_Z + Environment.NewLine;
                str += "ControllerVJoy_Send_WHL : " + ControllerVJoy_Send_WHL + Environment.NewLine;
                str += "ControllerVJoy_Send_SL0 : " + ControllerVJoy_Send_SL0 + Environment.NewLine;
                str += "ControllerVJoy_Send_SL1 : " + ControllerVJoy_Send_SL1 + Environment.NewLine;
                str += "ControllerVJoy_Send_RX : " + ControllerVJoy_Send_RX + Environment.NewLine;
                str += "ControllerVJoy_Send_RY : " + ControllerVJoy_Send_RY + Environment.NewLine;
                str += "ControllerVJoy_Send_RZ : " + ControllerVJoy_Send_RZ + Environment.NewLine;
                str += "ControllerVJoy_Send_POV : " + ControllerVJoy_Send_POV + Environment.NewLine;
                str += "ControllerVJoy_Send_Hat : " + ControllerVJoy_Send_Hat + Environment.NewLine;
                str += "ControllerVJoy_Send_HatExt1 : " + ControllerVJoy_Send_HatExt1 + Environment.NewLine;
                str += "ControllerVJoy_Send_HatExt2 : " + ControllerVJoy_Send_HatExt2 + Environment.NewLine;
                str += "ControllerVJoy_Send_HatExt3 : " + ControllerVJoy_Send_HatExt3 + Environment.NewLine;
                str += "PollingRate : " + pollingrate + " ms" + Environment.NewLine;
                string txt = str;
                string[] lines = txt.Split(new string[] { Environment.NewLine }, StringSplitOptions.None);
                foreach (string line in lines)
                    if (line.Contains(inputdelaybutton + " : "))
                    {
                        inputdelaytemp = inputdelay;
                        inputdelay = line;
                    }
                valchanged(0, inputdelay.Contains("True") | (!inputdelay.Contains("True") & !inputdelay.Contains("False") & inputdelay != inputdelaytemp));
                if (wd[0])
                {
                    getstate = true;
                }
                if (inputdelay.Contains("False") | (!inputdelay.Contains("True") & !inputdelay.Contains("False") & inputdelay == inputdelaytemp))
                    getstate = false;
                if (getstate)
                {
                    elapseddown = (double)PollingRate.ElapsedTicks / (Stopwatch.Frequency / 1000L);
                    elapsed = 0;
                }
                if (wu[0])
                {
                    elapsedup = (double)PollingRate.ElapsedTicks / (Stopwatch.Frequency / 1000L);
                    elapsed = elapsedup - elapseddown;
                }
                ValueChange[0] = inputdelay.Contains("False") | (!inputdelay.Contains("True") & !inputdelay.Contains("False") & inputdelay == inputdelaytemp) ? elapsed : 0;
                if (ValueChange._ValueChange[0] > 0)
                {
                    delay = ValueChange._ValueChange[0];
                }
                str += "InputDelay : " + delay + " ms" + Environment.NewLine;
                str += Environment.NewLine;
                form1.SetLabel1(str);
            }
        }
    }
}