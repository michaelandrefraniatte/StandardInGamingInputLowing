using System;
using System.Threading.Tasks;
using Mousexnahook;
using Microsoft.Xna.Framework.Input;
using System.Runtime.InteropServices;

namespace MouseXnaHookAPI
{
    public class MouseXnaHook
    {
        [DllImport("winmm.dll", EntryPoint = "timeBeginPeriod")]
        private static extern uint TimeBeginPeriod(uint ms);
        [DllImport("winmm.dll", EntryPoint = "timeEndPeriod")]
        private static extern uint TimeEndPeriod(uint ms);
        [DllImport("ntdll.dll", EntryPoint = "NtSetTimerResolution")]
        private static extern void NtSetTimerResolution(uint DesiredResolution, bool SetResolution, ref uint CurrentResolution);
        private static uint CurrentResolution = 0;
        private bool running, formvisible;
        private int number;
        private MouseState mousestate;
        private Form1 form1 = new Form1();
        public MouseXnaHook()
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
        private void taskM()
        {
            for (; ; )
            {
                if (!running)
                    break;
                ProcessStateLogic();
                System.Threading.Thread.Sleep(1);
                if (MouseAxisZ != 0)
                    Task.Run(() => Init());
                if (formvisible)
                {
                    string str = "MouseAxisX : " + MouseAxisX + Environment.NewLine;
                    str += "MouseAxisY : " + MouseAxisY + Environment.NewLine;
                    str += "MouseAxisZ : " + MouseAxisZ + Environment.NewLine;
                    str += "MouseButtons0 : " + MouseButtons0 + Environment.NewLine;
                    str += "MouseButtons1 : " + MouseButtons1 + Environment.NewLine;
                    str += "MouseButtons2 : " + MouseButtons2 + Environment.NewLine;
                    str += "MouseButtons3 : " + MouseButtons3 + Environment.NewLine;
                    str += "MouseButtons4 : " + MouseButtons4 + Environment.NewLine;
                    str += "MouseButtons5 : " + MouseButtons5 + Environment.NewLine;
                    str += "MouseButtons6 : " + MouseButtons6 + Environment.NewLine;
                    str += "MouseButtons7 : " + MouseButtons7 + Environment.NewLine;
                    str += Environment.NewLine;
                    form1.SetLabel1(str);
                }
            }
        }
        public void BeginPolling()
        {
            Task.Run(() => taskM());
        }
        public void Init()
        {
            System.Threading.Thread.Sleep(100);
            MouseAxisZ = 0;
        }
        public bool MouseButtons0;
        public bool MouseButtons1;
        public bool MouseButtons2;
        public bool MouseButtons3;
        public bool MouseButtons4;
        public bool MouseButtons5;
        public bool MouseButtons6;
        public bool MouseButtons7;
        public int MouseAxisX;
        public int MouseAxisY;
        public int MouseAxisZ;
        public bool Scan(int number = 0)
        {
            this.number = number;
            return true;
        }
        private void ProcessStateLogic()
        {
            mousestate = Mouse.GetState();
            MouseButtons0 = mousestate.LeftButton == Microsoft.Xna.Framework.Input.ButtonState.Pressed;
            MouseButtons1 = mousestate.RightButton == Microsoft.Xna.Framework.Input.ButtonState.Pressed;
            MouseButtons2 = mousestate.MiddleButton == Microsoft.Xna.Framework.Input.ButtonState.Pressed;
            MouseButtons3 = mousestate.XButton1 == Microsoft.Xna.Framework.Input.ButtonState.Pressed;
            MouseButtons4 = mousestate.XButton2 == Microsoft.Xna.Framework.Input.ButtonState.Pressed;
            MouseAxisX = mousestate.X;
            MouseAxisY = mousestate.Y;
            MouseAxisZ = mousestate.ScrollWheelValue;
        }
    }
}