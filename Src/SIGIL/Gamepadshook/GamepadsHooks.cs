using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using Gamepadshook;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Valuechanges;

namespace GamepadsHooksAPI
{
    public class GamepadsHooks
    {
        [DllImport("winmm.dll", EntryPoint = "timeBeginPeriod")]
        private static extern uint TimeBeginPeriod(uint ms);
        [DllImport("winmm.dll", EntryPoint = "timeEndPeriod")]
        private static extern uint TimeEndPeriod(uint ms);
        [DllImport("ntdll.dll", EntryPoint = "NtSetTimerResolution")]
        private static extern void NtSetTimerResolution(uint DesiredResolution, bool SetResolution, ref uint CurrentResolution);
        private static uint CurrentResolution = 0;
        private bool running, formvisible;
        private int number, inc;
        private GamePadState gamepadstate;
        private Form1 form1 = new Form1();
        private Stopwatch PollingRate;
        private double pollingrateperm = 0, pollingratetemp = 0, pollingratedisplay = 0, pollingrate;
        private string inputdelaybutton = "", inputdelay = "";
        public Valuechange ValueChange;
        private double delay, elapseddown, elapsedup, elapsed;
        private bool getstate = false;
        private int[] wd = { 2 };
        private int[] wu = { 2 };
        public void valchanged(int n, bool val)
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
        public GamepadsHooks()
        {
            TimeBeginPeriod(1);
            NtSetTimerResolution(1, true, ref CurrentResolution);
            running = true;
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
        public void Close()
        {
            running = false;
        }
        private void taskD()
        {
            for (; ; )
            {
                if (!running)
                    break;
                ProcessStateLogic();
                System.Threading.Thread.Sleep(1);
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
                    string str = "ControllerButtonAPressed : " + ControllerButtonAPressed + Environment.NewLine;
                    str += "ControllerButtonBPressed : " + ControllerButtonBPressed + Environment.NewLine;
                    str += "ControllerButtonXPressed : " + ControllerButtonXPressed + Environment.NewLine;
                    str += "ControllerButtonYPressed : " + ControllerButtonYPressed + Environment.NewLine;
                    str += "ControllerButtonStartPressed : " + ControllerButtonStartPressed + Environment.NewLine;
                    str += "ControllerButtonBackPressed : " + ControllerButtonBackPressed + Environment.NewLine;
                    str += "ControllerButtonDownPressed : " + ControllerButtonDownPressed + Environment.NewLine;
                    str += "ControllerButtonUpPressed : " + ControllerButtonUpPressed + Environment.NewLine;
                    str += "ControllerButtonLeftPressed : " + ControllerButtonLeftPressed + Environment.NewLine;
                    str += "ControllerButtonRightPressed : " + ControllerButtonRightPressed + Environment.NewLine;
                    str += "ControllerButtonShoulderLeftPressed : " + ControllerButtonShoulderLeftPressed + Environment.NewLine;
                    str += "ControllerButtonShoulderRightPressed : " + ControllerButtonShoulderRightPressed + Environment.NewLine;
                    str += "ControllerThumbpadLeftPressed : " + ControllerThumbpadLeftPressed + Environment.NewLine;
                    str += "ControllerThumbpadRightPressed : " + ControllerThumbpadRightPressed + Environment.NewLine;
                    str += "ControllerTriggerLeftPosition : " + ControllerTriggerLeftPosition + Environment.NewLine;
                    str += "ControllerTriggerRightPosition : " + ControllerTriggerRightPosition + Environment.NewLine;
                    str += "ControllerThumbLeftX : " + ControllerThumbLeftX + Environment.NewLine;
                    str += "ControllerThumbLeftY : " + ControllerThumbLeftY + Environment.NewLine;
                    str += "ControllerThumbRightX : " + ControllerThumbRightX + Environment.NewLine;
                    str += "ControllerThumbRightY : " + ControllerThumbRightY + Environment.NewLine;
                    str += "PollingRate : " + pollingrate + " ms" + Environment.NewLine;
                    string txt = str;
                    string[] lines = txt.Split(new string[] { Environment.NewLine }, StringSplitOptions.None);
                    foreach (string line in lines)
                        if (line.Contains(inputdelaybutton + " : "))
                            inputdelay = line;
                    valchanged(0, inputdelay.Contains("True"));
                    if (wd[0] == 1)
                    {
                        getstate = true;
                    }
                    if (inputdelay.Contains("False"))
                        getstate = false;
                    if (getstate)
                    {
                        elapseddown = (double)PollingRate.ElapsedTicks / (Stopwatch.Frequency / 1000L);
                        elapsed = 0;
                    }
                    if (wu[0] == 1)
                    {
                        elapsedup = (double)PollingRate.ElapsedTicks / (Stopwatch.Frequency / 1000L);
                        elapsed = elapsedup - elapseddown;
                    }
                    ValueChange[0] = inputdelay.Contains("False") ? elapsed : 0;
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
        public void BeginPolling()
        {
            Task.Run(() => taskD());
        }
        public bool ControllerButtonAPressed;
        public bool ControllerButtonBPressed;
        public bool ControllerButtonXPressed;
        public bool ControllerButtonYPressed;
        public bool ControllerButtonStartPressed;
        public bool ControllerButtonBackPressed;
        public bool ControllerButtonDownPressed;
        public bool ControllerButtonUpPressed;
        public bool ControllerButtonLeftPressed;
        public bool ControllerButtonRightPressed;
        public bool ControllerButtonShoulderLeftPressed;
        public bool ControllerButtonShoulderRightPressed;
        public bool ControllerThumbpadLeftPressed;
        public bool ControllerThumbpadRightPressed;
        public double ControllerTriggerLeftPosition;
        public double ControllerTriggerRightPosition;
        public double ControllerThumbLeftX;
        public double ControllerThumbLeftY;
        public double ControllerThumbRightX;
        public double ControllerThumbRightY;
        public bool Scan(int number = 0)
        {
            this.number = number;
            inc = number < 2 ? 0 : number - 1;
            gamepadstate = GamePad.GetState((PlayerIndex)inc, 0);
            if (!gamepadstate.IsConnected)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
        public void Init()
        {
        }
        private void ProcessStateLogic()
        {
            gamepadstate = GamePad.GetState((PlayerIndex)inc, 0);
            ControllerButtonAPressed = gamepadstate.Buttons.A == ButtonState.Pressed;
            ControllerButtonBPressed = gamepadstate.Buttons.B == ButtonState.Pressed;
            ControllerButtonXPressed = gamepadstate.Buttons.X == ButtonState.Pressed;
            ControllerButtonYPressed = gamepadstate.Buttons.Y == ButtonState.Pressed;
            ControllerButtonShoulderLeftPressed = gamepadstate.Buttons.LeftShoulder == ButtonState.Pressed;
            ControllerButtonShoulderRightPressed = gamepadstate.Buttons.RightShoulder == ButtonState.Pressed;
            ControllerButtonStartPressed = gamepadstate.Buttons.Start == ButtonState.Pressed;
            ControllerButtonBackPressed = gamepadstate.Buttons.Back == ButtonState.Pressed;
            ControllerThumbpadLeftPressed = gamepadstate.Buttons.LeftStick == ButtonState.Pressed;
            ControllerThumbpadRightPressed = gamepadstate.Buttons.RightStick == ButtonState.Pressed;
            ControllerButtonUpPressed = gamepadstate.DPad.Up == ButtonState.Pressed;
            ControllerButtonDownPressed = gamepadstate.DPad.Down == ButtonState.Pressed;
            ControllerButtonLeftPressed = gamepadstate.DPad.Left == ButtonState.Pressed;
            ControllerButtonRightPressed = gamepadstate.DPad.Right == ButtonState.Pressed;
            ControllerThumbLeftX = gamepadstate.ThumbSticks.Left.X * 32767f;
            ControllerThumbLeftY = gamepadstate.ThumbSticks.Left.Y * 32767f;
            ControllerThumbRightX = gamepadstate.ThumbSticks.Right.X * 32767f;
            ControllerThumbRightY = gamepadstate.ThumbSticks.Right.Y * 32767f;
            ControllerTriggerLeftPosition = gamepadstate.Triggers.Left * 255f;
            ControllerTriggerRightPosition = gamepadstate.Triggers.Right * 255f;
        }
    }
}