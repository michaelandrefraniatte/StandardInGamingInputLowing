using SharpDX.DirectInput;
using System;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using Directinputs;
using System.Collections.Generic;
using System.Diagnostics;
using Valuechanges;

namespace DirectInputsAPI
{
    public class DirectInput
    {
        [DllImport("winmm.dll", EntryPoint = "timeBeginPeriod")]
        private static extern uint TimeBeginPeriod(uint ms);
        [DllImport("winmm.dll", EntryPoint = "timeEndPeriod")]
        private static extern uint TimeEndPeriod(uint ms);
        [DllImport("ntdll.dll", EntryPoint = "NtSetTimerResolution")]
        private static extern void NtSetTimerResolution(uint DesiredResolution, bool SetResolution, ref uint CurrentResolution);
        private static uint CurrentResolution = 0;
        private bool running, formvisible;
        private SharpDX.DirectInput.DirectInput directInput = new SharpDX.DirectInput.DirectInput();
        private int number, inc;
        private static List<Joystick> joysticks = new List<Joystick>();
        private Joystick js;
        private Form1 form1;
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
        public DirectInput()
        {
            TimeBeginPeriod(1);
            NtSetTimerResolution(1, true, ref CurrentResolution);
            running = true;
        }
        public void ViewData(string inputdelaybutton = "")
        {
            if (!formvisible)
            {
                form1 = new Form1();
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
                    string str = "JoystickAxisX : " + JoystickAxisX + Environment.NewLine;
                    str += "JoystickAxisY : " + JoystickAxisY + Environment.NewLine;
                    str += "JoystickAxisZ : " + JoystickAxisZ + Environment.NewLine;
                    str += "JoystickRotationX : " + JoystickRotationX + Environment.NewLine;
                    str += "JoystickRotationY : " + JoystickRotationY + Environment.NewLine;
                    str += "JoystickRotationZ : " + JoystickRotationZ + Environment.NewLine;
                    str += "JoystickSliders0 : " + JoystickSliders0 + Environment.NewLine;
                    str += "JoystickSliders1 : " + JoystickSliders1 + Environment.NewLine;
                    str += "JoystickPointOfViewControllers0 : " + JoystickPointOfViewControllers0 + Environment.NewLine;
                    str += "JoystickPointOfViewControllers1 : " + JoystickPointOfViewControllers1 + Environment.NewLine;
                    str += "JoystickPointOfViewControllers2 : " + JoystickPointOfViewControllers2 + Environment.NewLine;
                    str += "JoystickPointOfViewControllers3 : " + JoystickPointOfViewControllers3 + Environment.NewLine;
                    str += "JoystickVelocityX : " + JoystickVelocityX + Environment.NewLine;
                    str += "JoystickVelocityY : " + JoystickVelocityY + Environment.NewLine;
                    str += "JoystickVelocityZ : " + JoystickVelocityZ + Environment.NewLine;
                    str += "JoystickAngularVelocityX : " + JoystickAngularVelocityX + Environment.NewLine;
                    str += "JoystickAngularVelocityY : " + JoystickAngularVelocityY + Environment.NewLine;
                    str += "JoystickAngularVelocityZ : " + JoystickAngularVelocityZ + Environment.NewLine;
                    str += "JoystickVelocitySliders0 : " + JoystickVelocitySliders0 + Environment.NewLine;
                    str += "JoystickVelocitySliders1 : " + JoystickVelocitySliders1 + Environment.NewLine;
                    str += "JoystickAccelerationX : " + JoystickAccelerationX + Environment.NewLine;
                    str += "JoystickAccelerationY : " + JoystickAccelerationY + Environment.NewLine;
                    str += "JoystickAccelerationZ : " + JoystickAccelerationZ + Environment.NewLine;
                    str += "JoystickAngularAccelerationX : " + JoystickAngularAccelerationX + Environment.NewLine;
                    str += "JoystickAngularAccelerationY : " + JoystickAngularAccelerationY + Environment.NewLine;
                    str += "JoystickAngularAccelerationZ : " + JoystickAngularAccelerationZ + Environment.NewLine;
                    str += "JoystickAccelerationSliders0 : " + JoystickAccelerationSliders0 + Environment.NewLine;
                    str += "JoystickAccelerationSliders1 : " + JoystickAccelerationSliders1 + Environment.NewLine;
                    str += "JoystickForceX : " + JoystickForceX + Environment.NewLine;
                    str += "JoystickForceY : " + JoystickForceY + Environment.NewLine;
                    str += "JoystickForceZ : " + JoystickForceZ + Environment.NewLine;
                    str += "JoystickTorqueX : " + JoystickTorqueX + Environment.NewLine;
                    str += "JoystickTorqueY : " + JoystickTorqueY + Environment.NewLine;
                    str += "JoystickTorqueZ : " + JoystickTorqueZ + Environment.NewLine;
                    str += "JoystickForceSliders0 : " + JoystickForceSliders0 + Environment.NewLine;
                    str += "JoystickForceSliders1 : " + JoystickForceSliders1 + Environment.NewLine;
                    str += "JoystickButtons0 : " + JoystickButtons0 + Environment.NewLine;
                    str += "JoystickButtons1 : " + JoystickButtons1 + Environment.NewLine;
                    str += "JoystickButtons2 : " + JoystickButtons2 + Environment.NewLine;
                    str += "JoystickButtons3 : " + JoystickButtons3 + Environment.NewLine;
                    str += "JoystickButtons4 : " + JoystickButtons4 + Environment.NewLine;
                    str += "JoystickButtons5 : " + JoystickButtons5 + Environment.NewLine;
                    str += "JoystickButtons6 : " + JoystickButtons6 + Environment.NewLine;
                    str += "JoystickButtons7 : " + JoystickButtons7 + Environment.NewLine;
                    str += "JoystickButtons8 : " + JoystickButtons8 + Environment.NewLine;
                    str += "JoystickButtons9 : " + JoystickButtons9 + Environment.NewLine;
                    str += "JoystickButtons10 : " + JoystickButtons10 + Environment.NewLine;
                    str += "JoystickButtons11 : " + JoystickButtons11 + Environment.NewLine;
                    str += "JoystickButtons12 : " + JoystickButtons12 + Environment.NewLine;
                    str += "JoystickButtons13 : " + JoystickButtons13 + Environment.NewLine;
                    str += "JoystickButtons14 : " + JoystickButtons14 + Environment.NewLine;
                    str += "JoystickButtons15 : " + JoystickButtons15 + Environment.NewLine;
                    str += "JoystickButtons16 : " + JoystickButtons16 + Environment.NewLine;
                    str += "JoystickButtons17 : " + JoystickButtons17 + Environment.NewLine;
                    str += "JoystickButtons18 : " + JoystickButtons18 + Environment.NewLine;
                    str += "JoystickButtons19 : " + JoystickButtons19 + Environment.NewLine;
                    str += "JoystickButtons20 : " + JoystickButtons20 + Environment.NewLine;
                    str += "JoystickButtons21 : " + JoystickButtons21 + Environment.NewLine;
                    str += "JoystickButtons22 : " + JoystickButtons22 + Environment.NewLine;
                    str += "JoystickButtons23 : " + JoystickButtons23 + Environment.NewLine;
                    str += "JoystickButtons24 : " + JoystickButtons24 + Environment.NewLine;
                    str += "JoystickButtons25 : " + JoystickButtons25 + Environment.NewLine;
                    str += "JoystickButtons26 : " + JoystickButtons26 + Environment.NewLine;
                    str += "JoystickButtons27 : " + JoystickButtons27 + Environment.NewLine;
                    str += "JoystickButtons28 : " + JoystickButtons28 + Environment.NewLine;
                    str += "JoystickButtons29 : " + JoystickButtons29 + Environment.NewLine;
                    str += "JoystickButtons30 : " + JoystickButtons30 + Environment.NewLine;
                    str += "JoystickButtons31 : " + JoystickButtons31 + Environment.NewLine;
                    str += "JoystickButtons32 : " + JoystickButtons32 + Environment.NewLine;
                    str += "JoystickButtons33 : " + JoystickButtons33 + Environment.NewLine;
                    str += "JoystickButtons34 : " + JoystickButtons34 + Environment.NewLine;
                    str += "JoystickButtons35 : " + JoystickButtons35 + Environment.NewLine;
                    str += "JoystickButtons36 : " + JoystickButtons36 + Environment.NewLine;
                    str += "JoystickButtons37 : " + JoystickButtons37 + Environment.NewLine;
                    str += "JoystickButtons38 : " + JoystickButtons38 + Environment.NewLine;
                    str += "JoystickButtons39 : " + JoystickButtons39 + Environment.NewLine;
                    str += "JoystickButtons40 : " + JoystickButtons40 + Environment.NewLine;
                    str += "JoystickButtons41 : " + JoystickButtons41 + Environment.NewLine;
                    str += "JoystickButtons42 : " + JoystickButtons42 + Environment.NewLine;
                    str += "JoystickButtons43 : " + JoystickButtons43 + Environment.NewLine;
                    str += "JoystickButtons44 : " + JoystickButtons44 + Environment.NewLine;
                    str += "JoystickButtons45 : " + JoystickButtons45 + Environment.NewLine;
                    str += "JoystickButtons46 : " + JoystickButtons46 + Environment.NewLine;
                    str += "JoystickButtons47 : " + JoystickButtons47 + Environment.NewLine;
                    str += "JoystickButtons48 : " + JoystickButtons48 + Environment.NewLine;
                    str += "JoystickButtons49 : " + JoystickButtons49 + Environment.NewLine;
                    str += "JoystickButtons50 : " + JoystickButtons50 + Environment.NewLine;
                    str += "JoystickButtons51 : " + JoystickButtons51 + Environment.NewLine;
                    str += "JoystickButtons52 : " + JoystickButtons52 + Environment.NewLine;
                    str += "JoystickButtons53 : " + JoystickButtons53 + Environment.NewLine;
                    str += "JoystickButtons54 : " + JoystickButtons54 + Environment.NewLine;
                    str += "JoystickButtons55 : " + JoystickButtons55 + Environment.NewLine;
                    str += "JoystickButtons56 : " + JoystickButtons56 + Environment.NewLine;
                    str += "JoystickButtons57 : " + JoystickButtons57 + Environment.NewLine;
                    str += "JoystickButtons58 : " + JoystickButtons58 + Environment.NewLine;
                    str += "JoystickButtons59 : " + JoystickButtons59 + Environment.NewLine;
                    str += "JoystickButtons60 : " + JoystickButtons60 + Environment.NewLine;
                    str += "JoystickButtons61 : " + JoystickButtons61 + Environment.NewLine;
                    str += "JoystickButtons62 : " + JoystickButtons62 + Environment.NewLine;
                    str += "JoystickButtons63 : " + JoystickButtons63 + Environment.NewLine;
                    str += "JoystickButtons64 : " + JoystickButtons64 + Environment.NewLine;
                    str += "JoystickButtons65 : " + JoystickButtons65 + Environment.NewLine;
                    str += "JoystickButtons66 : " + JoystickButtons66 + Environment.NewLine;
                    str += "JoystickButtons67 : " + JoystickButtons67 + Environment.NewLine;
                    str += "JoystickButtons68 : " + JoystickButtons68 + Environment.NewLine;
                    str += "JoystickButtons69 : " + JoystickButtons69 + Environment.NewLine;
                    str += "JoystickButtons70 : " + JoystickButtons70 + Environment.NewLine;
                    str += "JoystickButtons71 : " + JoystickButtons71 + Environment.NewLine;
                    str += "JoystickButtons72 : " + JoystickButtons72 + Environment.NewLine;
                    str += "JoystickButtons73 : " + JoystickButtons73 + Environment.NewLine;
                    str += "JoystickButtons74 : " + JoystickButtons74 + Environment.NewLine;
                    str += "JoystickButtons75 : " + JoystickButtons75 + Environment.NewLine;
                    str += "JoystickButtons76 : " + JoystickButtons76 + Environment.NewLine;
                    str += "JoystickButtons77 : " + JoystickButtons77 + Environment.NewLine;
                    str += "JoystickButtons78 : " + JoystickButtons78 + Environment.NewLine;
                    str += "JoystickButtons79 : " + JoystickButtons79 + Environment.NewLine;
                    str += "JoystickButtons80 : " + JoystickButtons80 + Environment.NewLine;
                    str += "JoystickButtons81 : " + JoystickButtons81 + Environment.NewLine;
                    str += "JoystickButtons82 : " + JoystickButtons82 + Environment.NewLine;
                    str += "JoystickButtons83 : " + JoystickButtons83 + Environment.NewLine;
                    str += "JoystickButtons84 : " + JoystickButtons84 + Environment.NewLine;
                    str += "JoystickButtons85 : " + JoystickButtons85 + Environment.NewLine;
                    str += "JoystickButtons86 : " + JoystickButtons86 + Environment.NewLine;
                    str += "JoystickButtons87 : " + JoystickButtons87 + Environment.NewLine;
                    str += "JoystickButtons88 : " + JoystickButtons88 + Environment.NewLine;
                    str += "JoystickButtons89 : " + JoystickButtons89 + Environment.NewLine;
                    str += "JoystickButtons90 : " + JoystickButtons90 + Environment.NewLine;
                    str += "JoystickButtons91 : " + JoystickButtons91 + Environment.NewLine;
                    str += "JoystickButtons92 : " + JoystickButtons92 + Environment.NewLine;
                    str += "JoystickButtons93 : " + JoystickButtons93 + Environment.NewLine;
                    str += "JoystickButtons94 : " + JoystickButtons94 + Environment.NewLine;
                    str += "JoystickButtons95 : " + JoystickButtons95 + Environment.NewLine;
                    str += "JoystickButtons96 : " + JoystickButtons96 + Environment.NewLine;
                    str += "JoystickButtons97 : " + JoystickButtons97 + Environment.NewLine;
                    str += "JoystickButtons98 : " + JoystickButtons98 + Environment.NewLine;
                    str += "JoystickButtons99 : " + JoystickButtons99 + Environment.NewLine;
                    str += "JoystickButtons100 : " + JoystickButtons100 + Environment.NewLine;
                    str += "JoystickButtons101 : " + JoystickButtons101 + Environment.NewLine;
                    str += "JoystickButtons102 : " + JoystickButtons102 + Environment.NewLine;
                    str += "JoystickButtons103 : " + JoystickButtons103 + Environment.NewLine;
                    str += "JoystickButtons104 : " + JoystickButtons104 + Environment.NewLine;
                    str += "JoystickButtons105 : " + JoystickButtons105 + Environment.NewLine;
                    str += "JoystickButtons106 : " + JoystickButtons106 + Environment.NewLine;
                    str += "JoystickButtons107 : " + JoystickButtons107 + Environment.NewLine;
                    str += "JoystickButtons108 : " + JoystickButtons108 + Environment.NewLine;
                    str += "JoystickButtons109 : " + JoystickButtons109 + Environment.NewLine;
                    str += "JoystickButtons110 : " + JoystickButtons110 + Environment.NewLine;
                    str += "JoystickButtons111 : " + JoystickButtons111 + Environment.NewLine;
                    str += "JoystickButtons112 : " + JoystickButtons112 + Environment.NewLine;
                    str += "JoystickButtons113 : " + JoystickButtons113 + Environment.NewLine;
                    str += "JoystickButtons114 : " + JoystickButtons114 + Environment.NewLine;
                    str += "JoystickButtons115 : " + JoystickButtons115 + Environment.NewLine;
                    str += "JoystickButtons116 : " + JoystickButtons116 + Environment.NewLine;
                    str += "JoystickButtons117 : " + JoystickButtons117 + Environment.NewLine;
                    str += "JoystickButtons118 : " + JoystickButtons118 + Environment.NewLine;
                    str += "JoystickButtons119 : " + JoystickButtons119 + Environment.NewLine;
                    str += "JoystickButtons120 : " + JoystickButtons120 + Environment.NewLine;
                    str += "JoystickButtons121 : " + JoystickButtons121 + Environment.NewLine;
                    str += "JoystickButtons122 : " + JoystickButtons122 + Environment.NewLine;
                    str += "JoystickButtons123 : " + JoystickButtons123 + Environment.NewLine;
                    str += "JoystickButtons124 : " + JoystickButtons124 + Environment.NewLine;
                    str += "JoystickButtons125 : " + JoystickButtons125 + Environment.NewLine;
                    str += "JoystickButtons126 : " + JoystickButtons126 + Environment.NewLine;
                    str += "JoystickButtons127 : " + JoystickButtons127 + Environment.NewLine;
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
        public void BeginPolling()
        {
            Task.Run(() => taskD());
        }
        private Joystick[] joystick = new Joystick[] { null, null, null, null };
        private Guid[] joystickGuid = new Guid[] { Guid.Empty, Guid.Empty, Guid.Empty, Guid.Empty };
        private int dinum = 0;
        public int JoystickAxisX;
        public int JoystickAxisY;
        public int JoystickAxisZ;
        public int JoystickRotationX;
        public int JoystickRotationY;
        public int JoystickRotationZ;
        public int JoystickSliders0;
        public int JoystickSliders1;
        public int JoystickPointOfViewControllers0;
        public int JoystickPointOfViewControllers1;
        public int JoystickPointOfViewControllers2;
        public int JoystickPointOfViewControllers3;
        public int JoystickVelocityX;
        public int JoystickVelocityY;
        public int JoystickVelocityZ;
        public int JoystickAngularVelocityX;
        public int JoystickAngularVelocityY;
        public int JoystickAngularVelocityZ;
        public int JoystickVelocitySliders0;
        public int JoystickVelocitySliders1;
        public int JoystickAccelerationX;
        public int JoystickAccelerationY;
        public int JoystickAccelerationZ;
        public int JoystickAngularAccelerationX;
        public int JoystickAngularAccelerationY;
        public int JoystickAngularAccelerationZ;
        public int JoystickAccelerationSliders0;
        public int JoystickAccelerationSliders1;
        public int JoystickForceX;
        public int JoystickForceY;
        public int JoystickForceZ;
        public int JoystickTorqueX;
        public int JoystickTorqueY;
        public int JoystickTorqueZ;
        public int JoystickForceSliders0;
        public int JoystickForceSliders1;
        public bool JoystickButtons0, JoystickButtons1, JoystickButtons2, JoystickButtons3, JoystickButtons4, JoystickButtons5, JoystickButtons6, JoystickButtons7, JoystickButtons8, JoystickButtons9, JoystickButtons10, JoystickButtons11, JoystickButtons12, JoystickButtons13, JoystickButtons14, JoystickButtons15, JoystickButtons16, JoystickButtons17, JoystickButtons18, JoystickButtons19, JoystickButtons20, JoystickButtons21, JoystickButtons22, JoystickButtons23, JoystickButtons24, JoystickButtons25, JoystickButtons26, JoystickButtons27, JoystickButtons28, JoystickButtons29, JoystickButtons30, JoystickButtons31, JoystickButtons32, JoystickButtons33, JoystickButtons34, JoystickButtons35, JoystickButtons36, JoystickButtons37, JoystickButtons38, JoystickButtons39, JoystickButtons40, JoystickButtons41, JoystickButtons42, JoystickButtons43, JoystickButtons44, JoystickButtons45, JoystickButtons46, JoystickButtons47, JoystickButtons48, JoystickButtons49, JoystickButtons50, JoystickButtons51, JoystickButtons52, JoystickButtons53, JoystickButtons54, JoystickButtons55, JoystickButtons56, JoystickButtons57, JoystickButtons58, JoystickButtons59, JoystickButtons60, JoystickButtons61, JoystickButtons62, JoystickButtons63, JoystickButtons64, JoystickButtons65, JoystickButtons66, JoystickButtons67, JoystickButtons68, JoystickButtons69, JoystickButtons70, JoystickButtons71, JoystickButtons72, JoystickButtons73, JoystickButtons74, JoystickButtons75, JoystickButtons76, JoystickButtons77, JoystickButtons78, JoystickButtons79, JoystickButtons80, JoystickButtons81, JoystickButtons82, JoystickButtons83, JoystickButtons84, JoystickButtons85, JoystickButtons86, JoystickButtons87, JoystickButtons88, JoystickButtons89, JoystickButtons90, JoystickButtons91, JoystickButtons92, JoystickButtons93, JoystickButtons94, JoystickButtons95, JoystickButtons96, JoystickButtons97, JoystickButtons98, JoystickButtons99, JoystickButtons100, JoystickButtons101, JoystickButtons102, JoystickButtons103, JoystickButtons104, JoystickButtons105, JoystickButtons106, JoystickButtons107, JoystickButtons108, JoystickButtons109, JoystickButtons110, JoystickButtons111, JoystickButtons112, JoystickButtons113, JoystickButtons114, JoystickButtons115, JoystickButtons116, JoystickButtons117, JoystickButtons118, JoystickButtons119, JoystickButtons120, JoystickButtons121, JoystickButtons122, JoystickButtons123, JoystickButtons124, JoystickButtons125, JoystickButtons126, JoystickButtons127;
        public bool Scan(int number = 0)
        {
            this.number = number;
            inc = number < 2 ? 0 : number - 1;
            if (number <= 1)
            {
                directInput = new SharpDX.DirectInput.DirectInput();
                joystick = new Joystick[] { null, null, null, null };
                joystickGuid = new Guid[] { Guid.Empty, Guid.Empty, Guid.Empty, Guid.Empty };
                dinum = 0;
                foreach (var deviceInstance in directInput.GetDevices(SharpDX.DirectInput.DeviceType.Gamepad, DeviceEnumerationFlags.AllDevices))
                {
                    joystickGuid[dinum] = deviceInstance.InstanceGuid;
                    joystick[dinum] = new Joystick(directInput, joystickGuid[dinum]);
                    joystick[dinum].Properties.BufferSize = 128;
                    joysticks.Add(joystick[dinum]);
                    dinum++;
                }
                foreach (var deviceInstance in directInput.GetDevices(SharpDX.DirectInput.DeviceType.Joystick, DeviceEnumerationFlags.AllDevices))
                {
                    joystickGuid[dinum] = deviceInstance.InstanceGuid;
                    joystick[dinum] = new Joystick(directInput, joystickGuid[dinum]);
                    joystick[dinum].Properties.BufferSize = 128;
                    joysticks.Add(joystick[dinum]);
                    dinum++;
                }
                foreach (var deviceInstance in directInput.GetDevices(SharpDX.DirectInput.DeviceType.Flight, DeviceEnumerationFlags.AllDevices))
                {
                    joystickGuid[dinum] = deviceInstance.InstanceGuid;
                    joystick[dinum] = new Joystick(directInput, joystickGuid[dinum]);
                    joystick[dinum].Properties.BufferSize = 128;
                    joysticks.Add(joystick[dinum]);
                    dinum++;
                }
                foreach (var deviceInstance in directInput.GetDevices(SharpDX.DirectInput.DeviceType.FirstPerson, DeviceEnumerationFlags.AllDevices))
                {
                    joystickGuid[dinum] = deviceInstance.InstanceGuid;
                    joystick[dinum] = new Joystick(directInput, joystickGuid[dinum]);
                    joystick[dinum].Properties.BufferSize = 128;
                    joysticks.Add(joystick[dinum]);
                    dinum++;
                }
                foreach (var deviceInstance in directInput.GetDevices(SharpDX.DirectInput.DeviceType.Driving, DeviceEnumerationFlags.AllDevices))
                {
                    joystickGuid[dinum] = deviceInstance.InstanceGuid;
                    joystick[dinum] = new Joystick(directInput, joystickGuid[dinum]);
                    joystick[dinum].Properties.BufferSize = 128;
                    joysticks.Add(joystick[dinum]);
                    dinum++;
                }
            }
            if (joysticks.Count == 0)
            {
                return false;
            }
            else
            {
                js = joysticks[inc];
                js.Acquire();
                return true;
            }
        }
        public void Init()
        {
        }
        private void ProcessStateLogic()
        {
            js.Poll();
            var datas = js.GetBufferedData();
            foreach (var state in datas)
            {
                if (state.Offset == JoystickOffset.X)
                    JoystickAxisX = state.Value;
                if (state.Offset == JoystickOffset.Y)
                    JoystickAxisY = state.Value;
                if (state.Offset == JoystickOffset.Z)
                    JoystickAxisZ = state.Value;
                if (state.Offset == JoystickOffset.RotationX)
                    JoystickRotationX = state.Value;
                if (state.Offset == JoystickOffset.RotationY)
                    JoystickRotationY = state.Value;
                if (state.Offset == JoystickOffset.RotationZ)
                    JoystickRotationZ = state.Value;
                if (state.Offset == JoystickOffset.Sliders0)
                    JoystickSliders0 = state.Value;
                if (state.Offset == JoystickOffset.Sliders1)
                    JoystickSliders1 = state.Value;
                if (state.Offset == JoystickOffset.PointOfViewControllers0)
                    JoystickPointOfViewControllers0 = state.Value;
                if (state.Offset == JoystickOffset.PointOfViewControllers1)
                    JoystickPointOfViewControllers1 = state.Value;
                if (state.Offset == JoystickOffset.PointOfViewControllers2)
                    JoystickPointOfViewControllers2 = state.Value;
                if (state.Offset == JoystickOffset.PointOfViewControllers3)
                    JoystickPointOfViewControllers3 = state.Value;
                if (state.Offset == JoystickOffset.VelocityX)
                    JoystickVelocityX = state.Value;
                if (state.Offset == JoystickOffset.VelocityY)
                    JoystickVelocityY = state.Value;
                if (state.Offset == JoystickOffset.VelocityZ)
                    JoystickVelocityZ = state.Value;
                if (state.Offset == JoystickOffset.AngularVelocityX)
                    JoystickAngularVelocityX = state.Value;
                if (state.Offset == JoystickOffset.AngularVelocityY)
                    JoystickAngularVelocityY = state.Value;
                if (state.Offset == JoystickOffset.AngularVelocityZ)
                    JoystickAngularVelocityZ = state.Value;
                if (state.Offset == JoystickOffset.VelocitySliders0)
                    JoystickVelocitySliders0 = state.Value;
                if (state.Offset == JoystickOffset.VelocitySliders1)
                    JoystickVelocitySliders1 = state.Value;
                if (state.Offset == JoystickOffset.AccelerationX)
                    JoystickAccelerationX = state.Value;
                if (state.Offset == JoystickOffset.AccelerationY)
                    JoystickAccelerationY = state.Value;
                if (state.Offset == JoystickOffset.AccelerationZ)
                    JoystickAccelerationZ = state.Value;
                if (state.Offset == JoystickOffset.AngularAccelerationX)
                    JoystickAngularAccelerationX = state.Value;
                if (state.Offset == JoystickOffset.AngularAccelerationY)
                    JoystickAngularAccelerationY = state.Value;
                if (state.Offset == JoystickOffset.AngularAccelerationZ)
                    JoystickAngularAccelerationZ = state.Value;
                if (state.Offset == JoystickOffset.AccelerationSliders0)
                    JoystickAccelerationSliders0 = state.Value;
                if (state.Offset == JoystickOffset.AccelerationSliders1)
                    JoystickAccelerationSliders1 = state.Value;
                if (state.Offset == JoystickOffset.ForceX)
                    JoystickForceX = state.Value;
                if (state.Offset == JoystickOffset.ForceY)
                    JoystickForceY = state.Value;
                if (state.Offset == JoystickOffset.ForceZ)
                    JoystickForceZ = state.Value;
                if (state.Offset == JoystickOffset.TorqueX)
                    JoystickTorqueX = state.Value;
                if (state.Offset == JoystickOffset.TorqueY)
                    JoystickTorqueY = state.Value;
                if (state.Offset == JoystickOffset.TorqueZ)
                    JoystickTorqueZ = state.Value;
                if (state.Offset == JoystickOffset.ForceSliders0)
                    JoystickForceSliders0 = state.Value;
                if (state.Offset == JoystickOffset.ForceSliders1)
                    JoystickForceSliders1 = state.Value;
                if (state.Offset == JoystickOffset.Buttons0 & state.Value == 128)
                    JoystickButtons0 = true;
                if (state.Offset == JoystickOffset.Buttons0 & state.Value == 0)
                    JoystickButtons0 = false;
                if (state.Offset == JoystickOffset.Buttons1 & state.Value == 128)
                    JoystickButtons1 = true;
                if (state.Offset == JoystickOffset.Buttons1 & state.Value == 0)
                    JoystickButtons1 = false;
                if (state.Offset == JoystickOffset.Buttons2 & state.Value == 128)
                    JoystickButtons2 = true;
                if (state.Offset == JoystickOffset.Buttons2 & state.Value == 0)
                    JoystickButtons2 = false;
                if (state.Offset == JoystickOffset.Buttons3 & state.Value == 128)
                    JoystickButtons3 = true;
                if (state.Offset == JoystickOffset.Buttons3 & state.Value == 0)
                    JoystickButtons3 = false;
                if (state.Offset == JoystickOffset.Buttons4 & state.Value == 128)
                    JoystickButtons4 = true;
                if (state.Offset == JoystickOffset.Buttons4 & state.Value == 0)
                    JoystickButtons4 = false;
                if (state.Offset == JoystickOffset.Buttons5 & state.Value == 128)
                    JoystickButtons5 = true;
                if (state.Offset == JoystickOffset.Buttons5 & state.Value == 0)
                    JoystickButtons5 = false;
                if (state.Offset == JoystickOffset.Buttons6 & state.Value == 128)
                    JoystickButtons6 = true;
                if (state.Offset == JoystickOffset.Buttons6 & state.Value == 0)
                    JoystickButtons6 = false;
                if (state.Offset == JoystickOffset.Buttons7 & state.Value == 128)
                    JoystickButtons7 = true;
                if (state.Offset == JoystickOffset.Buttons7 & state.Value == 0)
                    JoystickButtons7 = false;
                if (state.Offset == JoystickOffset.Buttons8 & state.Value == 128)
                    JoystickButtons8 = true;
                if (state.Offset == JoystickOffset.Buttons8 & state.Value == 0)
                    JoystickButtons8 = false;
                if (state.Offset == JoystickOffset.Buttons9 & state.Value == 128)
                    JoystickButtons9 = true;
                if (state.Offset == JoystickOffset.Buttons9 & state.Value == 0)
                    JoystickButtons9 = false;
                if (state.Offset == JoystickOffset.Buttons10 & state.Value == 128)
                    JoystickButtons10 = true;
                if (state.Offset == JoystickOffset.Buttons10 & state.Value == 0)
                    JoystickButtons10 = false;
                if (state.Offset == JoystickOffset.Buttons11 & state.Value == 128)
                    JoystickButtons11 = true;
                if (state.Offset == JoystickOffset.Buttons11 & state.Value == 0)
                    JoystickButtons11 = false;
                if (state.Offset == JoystickOffset.Buttons12 & state.Value == 128)
                    JoystickButtons12 = true;
                if (state.Offset == JoystickOffset.Buttons12 & state.Value == 0)
                    JoystickButtons12 = false;
                if (state.Offset == JoystickOffset.Buttons13 & state.Value == 128)
                    JoystickButtons13 = true;
                if (state.Offset == JoystickOffset.Buttons13 & state.Value == 0)
                    JoystickButtons13 = false;
                if (state.Offset == JoystickOffset.Buttons14 & state.Value == 128)
                    JoystickButtons14 = true;
                if (state.Offset == JoystickOffset.Buttons14 & state.Value == 0)
                    JoystickButtons14 = false;
                if (state.Offset == JoystickOffset.Buttons15 & state.Value == 128)
                    JoystickButtons15 = true;
                if (state.Offset == JoystickOffset.Buttons15 & state.Value == 0)
                    JoystickButtons15 = false;
                if (state.Offset == JoystickOffset.Buttons16 & state.Value == 128)
                    JoystickButtons16 = true;
                if (state.Offset == JoystickOffset.Buttons16 & state.Value == 0)
                    JoystickButtons16 = false;
                if (state.Offset == JoystickOffset.Buttons17 & state.Value == 128)
                    JoystickButtons17 = true;
                if (state.Offset == JoystickOffset.Buttons17 & state.Value == 0)
                    JoystickButtons17 = false;
                if (state.Offset == JoystickOffset.Buttons18 & state.Value == 128)
                    JoystickButtons18 = true;
                if (state.Offset == JoystickOffset.Buttons18 & state.Value == 0)
                    JoystickButtons18 = false;
                if (state.Offset == JoystickOffset.Buttons19 & state.Value == 128)
                    JoystickButtons19 = true;
                if (state.Offset == JoystickOffset.Buttons19 & state.Value == 0)
                    JoystickButtons19 = false;
                if (state.Offset == JoystickOffset.Buttons20 & state.Value == 128)
                    JoystickButtons20 = true;
                if (state.Offset == JoystickOffset.Buttons20 & state.Value == 0)
                    JoystickButtons20 = false;
                if (state.Offset == JoystickOffset.Buttons21 & state.Value == 128)
                    JoystickButtons21 = true;
                if (state.Offset == JoystickOffset.Buttons21 & state.Value == 0)
                    JoystickButtons21 = false;
                if (state.Offset == JoystickOffset.Buttons22 & state.Value == 128)
                    JoystickButtons22 = true;
                if (state.Offset == JoystickOffset.Buttons22 & state.Value == 0)
                    JoystickButtons22 = false;
                if (state.Offset == JoystickOffset.Buttons23 & state.Value == 128)
                    JoystickButtons23 = true;
                if (state.Offset == JoystickOffset.Buttons23 & state.Value == 0)
                    JoystickButtons23 = false;
                if (state.Offset == JoystickOffset.Buttons24 & state.Value == 128)
                    JoystickButtons24 = true;
                if (state.Offset == JoystickOffset.Buttons24 & state.Value == 0)
                    JoystickButtons24 = false;
                if (state.Offset == JoystickOffset.Buttons25 & state.Value == 128)
                    JoystickButtons25 = true;
                if (state.Offset == JoystickOffset.Buttons25 & state.Value == 0)
                    JoystickButtons25 = false;
                if (state.Offset == JoystickOffset.Buttons26 & state.Value == 128)
                    JoystickButtons26 = true;
                if (state.Offset == JoystickOffset.Buttons26 & state.Value == 0)
                    JoystickButtons26 = false;
                if (state.Offset == JoystickOffset.Buttons27 & state.Value == 128)
                    JoystickButtons27 = true;
                if (state.Offset == JoystickOffset.Buttons27 & state.Value == 0)
                    JoystickButtons27 = false;
                if (state.Offset == JoystickOffset.Buttons28 & state.Value == 128)
                    JoystickButtons28 = true;
                if (state.Offset == JoystickOffset.Buttons28 & state.Value == 0)
                    JoystickButtons28 = false;
                if (state.Offset == JoystickOffset.Buttons29 & state.Value == 128)
                    JoystickButtons29 = true;
                if (state.Offset == JoystickOffset.Buttons29 & state.Value == 0)
                    JoystickButtons29 = false;
                if (state.Offset == JoystickOffset.Buttons30 & state.Value == 128)
                    JoystickButtons30 = true;
                if (state.Offset == JoystickOffset.Buttons30 & state.Value == 0)
                    JoystickButtons30 = false;
                if (state.Offset == JoystickOffset.Buttons31 & state.Value == 128)
                    JoystickButtons31 = true;
                if (state.Offset == JoystickOffset.Buttons31 & state.Value == 0)
                    JoystickButtons31 = false;
                if (state.Offset == JoystickOffset.Buttons32 & state.Value == 128)
                    JoystickButtons32 = true;
                if (state.Offset == JoystickOffset.Buttons32 & state.Value == 0)
                    JoystickButtons32 = false;
                if (state.Offset == JoystickOffset.Buttons33 & state.Value == 128)
                    JoystickButtons33 = true;
                if (state.Offset == JoystickOffset.Buttons33 & state.Value == 0)
                    JoystickButtons33 = false;
                if (state.Offset == JoystickOffset.Buttons34 & state.Value == 128)
                    JoystickButtons34 = true;
                if (state.Offset == JoystickOffset.Buttons34 & state.Value == 0)
                    JoystickButtons34 = false;
                if (state.Offset == JoystickOffset.Buttons35 & state.Value == 128)
                    JoystickButtons35 = true;
                if (state.Offset == JoystickOffset.Buttons35 & state.Value == 0)
                    JoystickButtons35 = false;
                if (state.Offset == JoystickOffset.Buttons36 & state.Value == 128)
                    JoystickButtons36 = true;
                if (state.Offset == JoystickOffset.Buttons36 & state.Value == 0)
                    JoystickButtons36 = false;
                if (state.Offset == JoystickOffset.Buttons37 & state.Value == 128)
                    JoystickButtons37 = true;
                if (state.Offset == JoystickOffset.Buttons37 & state.Value == 0)
                    JoystickButtons37 = false;
                if (state.Offset == JoystickOffset.Buttons38 & state.Value == 128)
                    JoystickButtons38 = true;
                if (state.Offset == JoystickOffset.Buttons38 & state.Value == 0)
                    JoystickButtons38 = false;
                if (state.Offset == JoystickOffset.Buttons39 & state.Value == 128)
                    JoystickButtons39 = true;
                if (state.Offset == JoystickOffset.Buttons39 & state.Value == 0)
                    JoystickButtons39 = false;
                if (state.Offset == JoystickOffset.Buttons40 & state.Value == 128)
                    JoystickButtons40 = true;
                if (state.Offset == JoystickOffset.Buttons40 & state.Value == 0)
                    JoystickButtons40 = false;
                if (state.Offset == JoystickOffset.Buttons41 & state.Value == 128)
                    JoystickButtons41 = true;
                if (state.Offset == JoystickOffset.Buttons41 & state.Value == 0)
                    JoystickButtons41 = false;
                if (state.Offset == JoystickOffset.Buttons42 & state.Value == 128)
                    JoystickButtons42 = true;
                if (state.Offset == JoystickOffset.Buttons42 & state.Value == 0)
                    JoystickButtons42 = false;
                if (state.Offset == JoystickOffset.Buttons43 & state.Value == 128)
                    JoystickButtons43 = true;
                if (state.Offset == JoystickOffset.Buttons43 & state.Value == 0)
                    JoystickButtons43 = false;
                if (state.Offset == JoystickOffset.Buttons44 & state.Value == 128)
                    JoystickButtons44 = true;
                if (state.Offset == JoystickOffset.Buttons44 & state.Value == 0)
                    JoystickButtons44 = false;
                if (state.Offset == JoystickOffset.Buttons45 & state.Value == 128)
                    JoystickButtons45 = true;
                if (state.Offset == JoystickOffset.Buttons45 & state.Value == 0)
                    JoystickButtons45 = false;
                if (state.Offset == JoystickOffset.Buttons46 & state.Value == 128)
                    JoystickButtons46 = true;
                if (state.Offset == JoystickOffset.Buttons46 & state.Value == 0)
                    JoystickButtons46 = false;
                if (state.Offset == JoystickOffset.Buttons47 & state.Value == 128)
                    JoystickButtons47 = true;
                if (state.Offset == JoystickOffset.Buttons47 & state.Value == 0)
                    JoystickButtons47 = false;
                if (state.Offset == JoystickOffset.Buttons48 & state.Value == 128)
                    JoystickButtons48 = true;
                if (state.Offset == JoystickOffset.Buttons48 & state.Value == 0)
                    JoystickButtons48 = false;
                if (state.Offset == JoystickOffset.Buttons49 & state.Value == 128)
                    JoystickButtons49 = true;
                if (state.Offset == JoystickOffset.Buttons49 & state.Value == 0)
                    JoystickButtons49 = false;
                if (state.Offset == JoystickOffset.Buttons50 & state.Value == 128)
                    JoystickButtons50 = true;
                if (state.Offset == JoystickOffset.Buttons50 & state.Value == 0)
                    JoystickButtons50 = false;
                if (state.Offset == JoystickOffset.Buttons51 & state.Value == 128)
                    JoystickButtons51 = true;
                if (state.Offset == JoystickOffset.Buttons51 & state.Value == 0)
                    JoystickButtons51 = false;
                if (state.Offset == JoystickOffset.Buttons52 & state.Value == 128)
                    JoystickButtons52 = true;
                if (state.Offset == JoystickOffset.Buttons52 & state.Value == 0)
                    JoystickButtons52 = false;
                if (state.Offset == JoystickOffset.Buttons53 & state.Value == 128)
                    JoystickButtons53 = true;
                if (state.Offset == JoystickOffset.Buttons53 & state.Value == 0)
                    JoystickButtons53 = false;
                if (state.Offset == JoystickOffset.Buttons54 & state.Value == 128)
                    JoystickButtons54 = true;
                if (state.Offset == JoystickOffset.Buttons54 & state.Value == 0)
                    JoystickButtons54 = false;
                if (state.Offset == JoystickOffset.Buttons55 & state.Value == 128)
                    JoystickButtons55 = true;
                if (state.Offset == JoystickOffset.Buttons55 & state.Value == 0)
                    JoystickButtons55 = false;
                if (state.Offset == JoystickOffset.Buttons56 & state.Value == 128)
                    JoystickButtons56 = true;
                if (state.Offset == JoystickOffset.Buttons56 & state.Value == 0)
                    JoystickButtons56 = false;
                if (state.Offset == JoystickOffset.Buttons57 & state.Value == 128)
                    JoystickButtons57 = true;
                if (state.Offset == JoystickOffset.Buttons57 & state.Value == 0)
                    JoystickButtons57 = false;
                if (state.Offset == JoystickOffset.Buttons58 & state.Value == 128)
                    JoystickButtons58 = true;
                if (state.Offset == JoystickOffset.Buttons58 & state.Value == 0)
                    JoystickButtons58 = false;
                if (state.Offset == JoystickOffset.Buttons59 & state.Value == 128)
                    JoystickButtons59 = true;
                if (state.Offset == JoystickOffset.Buttons59 & state.Value == 0)
                    JoystickButtons59 = false;
                if (state.Offset == JoystickOffset.Buttons60 & state.Value == 128)
                    JoystickButtons60 = true;
                if (state.Offset == JoystickOffset.Buttons60 & state.Value == 0)
                    JoystickButtons60 = false;
                if (state.Offset == JoystickOffset.Buttons61 & state.Value == 128)
                    JoystickButtons61 = true;
                if (state.Offset == JoystickOffset.Buttons61 & state.Value == 0)
                    JoystickButtons61 = false;
                if (state.Offset == JoystickOffset.Buttons62 & state.Value == 128)
                    JoystickButtons62 = true;
                if (state.Offset == JoystickOffset.Buttons62 & state.Value == 0)
                    JoystickButtons62 = false;
                if (state.Offset == JoystickOffset.Buttons63 & state.Value == 128)
                    JoystickButtons63 = true;
                if (state.Offset == JoystickOffset.Buttons63 & state.Value == 0)
                    JoystickButtons63 = false;
                if (state.Offset == JoystickOffset.Buttons64 & state.Value == 128)
                    JoystickButtons64 = true;
                if (state.Offset == JoystickOffset.Buttons64 & state.Value == 0)
                    JoystickButtons64 = false;
                if (state.Offset == JoystickOffset.Buttons65 & state.Value == 128)
                    JoystickButtons65 = true;
                if (state.Offset == JoystickOffset.Buttons65 & state.Value == 0)
                    JoystickButtons65 = false;
                if (state.Offset == JoystickOffset.Buttons66 & state.Value == 128)
                    JoystickButtons66 = true;
                if (state.Offset == JoystickOffset.Buttons66 & state.Value == 0)
                    JoystickButtons66 = false;
                if (state.Offset == JoystickOffset.Buttons67 & state.Value == 128)
                    JoystickButtons67 = true;
                if (state.Offset == JoystickOffset.Buttons67 & state.Value == 0)
                    JoystickButtons67 = false;
                if (state.Offset == JoystickOffset.Buttons68 & state.Value == 128)
                    JoystickButtons68 = true;
                if (state.Offset == JoystickOffset.Buttons68 & state.Value == 0)
                    JoystickButtons68 = false;
                if (state.Offset == JoystickOffset.Buttons69 & state.Value == 128)
                    JoystickButtons69 = true;
                if (state.Offset == JoystickOffset.Buttons69 & state.Value == 0)
                    JoystickButtons69 = false;
                if (state.Offset == JoystickOffset.Buttons70 & state.Value == 128)
                    JoystickButtons70 = true;
                if (state.Offset == JoystickOffset.Buttons70 & state.Value == 0)
                    JoystickButtons70 = false;
                if (state.Offset == JoystickOffset.Buttons71 & state.Value == 128)
                    JoystickButtons71 = true;
                if (state.Offset == JoystickOffset.Buttons71 & state.Value == 0)
                    JoystickButtons71 = false;
                if (state.Offset == JoystickOffset.Buttons72 & state.Value == 128)
                    JoystickButtons72 = true;
                if (state.Offset == JoystickOffset.Buttons72 & state.Value == 0)
                    JoystickButtons72 = false;
                if (state.Offset == JoystickOffset.Buttons73 & state.Value == 128)
                    JoystickButtons73 = true;
                if (state.Offset == JoystickOffset.Buttons73 & state.Value == 0)
                    JoystickButtons73 = false;
                if (state.Offset == JoystickOffset.Buttons74 & state.Value == 128)
                    JoystickButtons74 = true;
                if (state.Offset == JoystickOffset.Buttons74 & state.Value == 0)
                    JoystickButtons74 = false;
                if (state.Offset == JoystickOffset.Buttons75 & state.Value == 128)
                    JoystickButtons75 = true;
                if (state.Offset == JoystickOffset.Buttons75 & state.Value == 0)
                    JoystickButtons75 = false;
                if (state.Offset == JoystickOffset.Buttons76 & state.Value == 128)
                    JoystickButtons76 = true;
                if (state.Offset == JoystickOffset.Buttons76 & state.Value == 0)
                    JoystickButtons76 = false;
                if (state.Offset == JoystickOffset.Buttons77 & state.Value == 128)
                    JoystickButtons77 = true;
                if (state.Offset == JoystickOffset.Buttons77 & state.Value == 0)
                    JoystickButtons77 = false;
                if (state.Offset == JoystickOffset.Buttons78 & state.Value == 128)
                    JoystickButtons78 = true;
                if (state.Offset == JoystickOffset.Buttons78 & state.Value == 0)
                    JoystickButtons78 = false;
                if (state.Offset == JoystickOffset.Buttons79 & state.Value == 128)
                    JoystickButtons79 = true;
                if (state.Offset == JoystickOffset.Buttons79 & state.Value == 0)
                    JoystickButtons79 = false;
                if (state.Offset == JoystickOffset.Buttons80 & state.Value == 128)
                    JoystickButtons80 = true;
                if (state.Offset == JoystickOffset.Buttons80 & state.Value == 0)
                    JoystickButtons80 = false;
                if (state.Offset == JoystickOffset.Buttons81 & state.Value == 128)
                    JoystickButtons81 = true;
                if (state.Offset == JoystickOffset.Buttons81 & state.Value == 0)
                    JoystickButtons81 = false;
                if (state.Offset == JoystickOffset.Buttons82 & state.Value == 128)
                    JoystickButtons82 = true;
                if (state.Offset == JoystickOffset.Buttons82 & state.Value == 0)
                    JoystickButtons82 = false;
                if (state.Offset == JoystickOffset.Buttons83 & state.Value == 128)
                    JoystickButtons83 = true;
                if (state.Offset == JoystickOffset.Buttons83 & state.Value == 0)
                    JoystickButtons83 = false;
                if (state.Offset == JoystickOffset.Buttons84 & state.Value == 128)
                    JoystickButtons84 = true;
                if (state.Offset == JoystickOffset.Buttons84 & state.Value == 0)
                    JoystickButtons84 = false;
                if (state.Offset == JoystickOffset.Buttons85 & state.Value == 128)
                    JoystickButtons85 = true;
                if (state.Offset == JoystickOffset.Buttons85 & state.Value == 0)
                    JoystickButtons85 = false;
                if (state.Offset == JoystickOffset.Buttons86 & state.Value == 128)
                    JoystickButtons86 = true;
                if (state.Offset == JoystickOffset.Buttons86 & state.Value == 0)
                    JoystickButtons86 = false;
                if (state.Offset == JoystickOffset.Buttons87 & state.Value == 128)
                    JoystickButtons87 = true;
                if (state.Offset == JoystickOffset.Buttons87 & state.Value == 0)
                    JoystickButtons87 = false;
                if (state.Offset == JoystickOffset.Buttons88 & state.Value == 128)
                    JoystickButtons88 = true;
                if (state.Offset == JoystickOffset.Buttons88 & state.Value == 0)
                    JoystickButtons88 = false;
                if (state.Offset == JoystickOffset.Buttons89 & state.Value == 128)
                    JoystickButtons89 = true;
                if (state.Offset == JoystickOffset.Buttons89 & state.Value == 0)
                    JoystickButtons89 = false;
                if (state.Offset == JoystickOffset.Buttons90 & state.Value == 128)
                    JoystickButtons90 = true;
                if (state.Offset == JoystickOffset.Buttons90 & state.Value == 0)
                    JoystickButtons90 = false;
                if (state.Offset == JoystickOffset.Buttons91 & state.Value == 128)
                    JoystickButtons91 = true;
                if (state.Offset == JoystickOffset.Buttons91 & state.Value == 0)
                    JoystickButtons91 = false;
                if (state.Offset == JoystickOffset.Buttons92 & state.Value == 128)
                    JoystickButtons92 = true;
                if (state.Offset == JoystickOffset.Buttons92 & state.Value == 0)
                    JoystickButtons92 = false;
                if (state.Offset == JoystickOffset.Buttons93 & state.Value == 128)
                    JoystickButtons93 = true;
                if (state.Offset == JoystickOffset.Buttons93 & state.Value == 0)
                    JoystickButtons93 = false;
                if (state.Offset == JoystickOffset.Buttons94 & state.Value == 128)
                    JoystickButtons94 = true;
                if (state.Offset == JoystickOffset.Buttons94 & state.Value == 0)
                    JoystickButtons94 = false;
                if (state.Offset == JoystickOffset.Buttons95 & state.Value == 128)
                    JoystickButtons95 = true;
                if (state.Offset == JoystickOffset.Buttons95 & state.Value == 0)
                    JoystickButtons95 = false;
                if (state.Offset == JoystickOffset.Buttons96 & state.Value == 128)
                    JoystickButtons96 = true;
                if (state.Offset == JoystickOffset.Buttons96 & state.Value == 0)
                    JoystickButtons96 = false;
                if (state.Offset == JoystickOffset.Buttons97 & state.Value == 128)
                    JoystickButtons97 = true;
                if (state.Offset == JoystickOffset.Buttons97 & state.Value == 0)
                    JoystickButtons97 = false;
                if (state.Offset == JoystickOffset.Buttons98 & state.Value == 128)
                    JoystickButtons98 = true;
                if (state.Offset == JoystickOffset.Buttons98 & state.Value == 0)
                    JoystickButtons98 = false;
                if (state.Offset == JoystickOffset.Buttons99 & state.Value == 128)
                    JoystickButtons99 = true;
                if (state.Offset == JoystickOffset.Buttons99 & state.Value == 0)
                    JoystickButtons99 = false;
                if (state.Offset == JoystickOffset.Buttons100 & state.Value == 128)
                    JoystickButtons100 = true;
                if (state.Offset == JoystickOffset.Buttons100 & state.Value == 0)
                    JoystickButtons100 = false;
                if (state.Offset == JoystickOffset.Buttons101 & state.Value == 128)
                    JoystickButtons101 = true;
                if (state.Offset == JoystickOffset.Buttons101 & state.Value == 0)
                    JoystickButtons101 = false;
                if (state.Offset == JoystickOffset.Buttons102 & state.Value == 128)
                    JoystickButtons102 = true;
                if (state.Offset == JoystickOffset.Buttons102 & state.Value == 0)
                    JoystickButtons102 = false;
                if (state.Offset == JoystickOffset.Buttons103 & state.Value == 128)
                    JoystickButtons103 = true;
                if (state.Offset == JoystickOffset.Buttons103 & state.Value == 0)
                    JoystickButtons103 = false;
                if (state.Offset == JoystickOffset.Buttons104 & state.Value == 128)
                    JoystickButtons104 = true;
                if (state.Offset == JoystickOffset.Buttons104 & state.Value == 0)
                    JoystickButtons104 = false;
                if (state.Offset == JoystickOffset.Buttons105 & state.Value == 128)
                    JoystickButtons105 = true;
                if (state.Offset == JoystickOffset.Buttons105 & state.Value == 0)
                    JoystickButtons105 = false;
                if (state.Offset == JoystickOffset.Buttons106 & state.Value == 128)
                    JoystickButtons106 = true;
                if (state.Offset == JoystickOffset.Buttons106 & state.Value == 0)
                    JoystickButtons106 = false;
                if (state.Offset == JoystickOffset.Buttons107 & state.Value == 128)
                    JoystickButtons107 = true;
                if (state.Offset == JoystickOffset.Buttons107 & state.Value == 0)
                    JoystickButtons107 = false;
                if (state.Offset == JoystickOffset.Buttons108 & state.Value == 128)
                    JoystickButtons108 = true;
                if (state.Offset == JoystickOffset.Buttons108 & state.Value == 0)
                    JoystickButtons108 = false;
                if (state.Offset == JoystickOffset.Buttons109 & state.Value == 128)
                    JoystickButtons109 = true;
                if (state.Offset == JoystickOffset.Buttons109 & state.Value == 0)
                    JoystickButtons109 = false;
                if (state.Offset == JoystickOffset.Buttons110 & state.Value == 128)
                    JoystickButtons110 = true;
                if (state.Offset == JoystickOffset.Buttons110 & state.Value == 0)
                    JoystickButtons110 = false;
                if (state.Offset == JoystickOffset.Buttons111 & state.Value == 128)
                    JoystickButtons111 = true;
                if (state.Offset == JoystickOffset.Buttons111 & state.Value == 0)
                    JoystickButtons111 = false;
                if (state.Offset == JoystickOffset.Buttons112 & state.Value == 128)
                    JoystickButtons112 = true;
                if (state.Offset == JoystickOffset.Buttons112 & state.Value == 0)
                    JoystickButtons112 = false;
                if (state.Offset == JoystickOffset.Buttons113 & state.Value == 128)
                    JoystickButtons113 = true;
                if (state.Offset == JoystickOffset.Buttons113 & state.Value == 0)
                    JoystickButtons113 = false;
                if (state.Offset == JoystickOffset.Buttons114 & state.Value == 128)
                    JoystickButtons114 = true;
                if (state.Offset == JoystickOffset.Buttons114 & state.Value == 0)
                    JoystickButtons114 = false;
                if (state.Offset == JoystickOffset.Buttons115 & state.Value == 128)
                    JoystickButtons115 = true;
                if (state.Offset == JoystickOffset.Buttons115 & state.Value == 0)
                    JoystickButtons115 = false;
                if (state.Offset == JoystickOffset.Buttons116 & state.Value == 128)
                    JoystickButtons116 = true;
                if (state.Offset == JoystickOffset.Buttons116 & state.Value == 0)
                    JoystickButtons116 = false;
                if (state.Offset == JoystickOffset.Buttons117 & state.Value == 128)
                    JoystickButtons117 = true;
                if (state.Offset == JoystickOffset.Buttons117 & state.Value == 0)
                    JoystickButtons117 = false;
                if (state.Offset == JoystickOffset.Buttons118 & state.Value == 128)
                    JoystickButtons118 = true;
                if (state.Offset == JoystickOffset.Buttons118 & state.Value == 0)
                    JoystickButtons118 = false;
                if (state.Offset == JoystickOffset.Buttons119 & state.Value == 128)
                    JoystickButtons119 = true;
                if (state.Offset == JoystickOffset.Buttons119 & state.Value == 0)
                    JoystickButtons119 = false;
                if (state.Offset == JoystickOffset.Buttons120 & state.Value == 128)
                    JoystickButtons120 = true;
                if (state.Offset == JoystickOffset.Buttons120 & state.Value == 0)
                    JoystickButtons120 = false;
                if (state.Offset == JoystickOffset.Buttons121 & state.Value == 128)
                    JoystickButtons121 = true;
                if (state.Offset == JoystickOffset.Buttons121 & state.Value == 0)
                    JoystickButtons121 = false;
                if (state.Offset == JoystickOffset.Buttons122 & state.Value == 128)
                    JoystickButtons122 = true;
                if (state.Offset == JoystickOffset.Buttons122 & state.Value == 0)
                    JoystickButtons122 = false;
                if (state.Offset == JoystickOffset.Buttons123 & state.Value == 128)
                    JoystickButtons123 = true;
                if (state.Offset == JoystickOffset.Buttons123 & state.Value == 0)
                    JoystickButtons123 = false;
                if (state.Offset == JoystickOffset.Buttons124 & state.Value == 128)
                    JoystickButtons124 = true;
                if (state.Offset == JoystickOffset.Buttons124 & state.Value == 0)
                    JoystickButtons124 = false;
                if (state.Offset == JoystickOffset.Buttons125 & state.Value == 128)
                    JoystickButtons125 = true;
                if (state.Offset == JoystickOffset.Buttons125 & state.Value == 0)
                    JoystickButtons125 = false;
                if (state.Offset == JoystickOffset.Buttons126 & state.Value == 128)
                    JoystickButtons126 = true;
                if (state.Offset == JoystickOffset.Buttons126 & state.Value == 0)
                    JoystickButtons126 = false;
                if (state.Offset == JoystickOffset.Buttons127 & state.Value == 128)
                    JoystickButtons127 = true;
                if (state.Offset == JoystickOffset.Buttons127 & state.Value == 0)
                    JoystickButtons127 = false;
            }
        }
    }
}