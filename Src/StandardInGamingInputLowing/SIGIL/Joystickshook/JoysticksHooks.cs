using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using Joystickshook;
using Microsoft.DirectX.DirectInput;

namespace JoysticksHooksAPI
{
    public class JoysticksHooks
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
        private Device joystickDevice;
        private JoystickState state;
        private string[] systemJoysticks;
        private bool[] buttons;
        private byte[] jsButtons;
        private int[] sliders;
        private int[] asliders;
        private int[] fsliders;
        private int[] vsliders;
        private int[] pointofview;
        private string[] sticks;
        private static List<string> joysticks = new List<string>();
        private string joystick;
        private Form1 form1 = new Form1();
        public void ViewData()
        {
            if (!form1.Visible)
            {
                formvisible = true;
                form1.SetVisible();
            }
        }
        public JoysticksHooks()
        {
            TimeBeginPeriod(1);
            NtSetTimerResolution(1, true, ref CurrentResolution);
            running = true;
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
                    str += Environment.NewLine;
                    form1.SetLabel1(str);
                }
            }
        }
        public void BeginPolling()
        {
            Task.Run(() => taskD());
        }
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
            if (number <= 1)
            {
                sticks = FindJoysticks();
                foreach (string stick in sticks)
                {
                    joysticks.Add(stick);
                }
            }
            if (joysticks.Count == 0)
            {
                return false;
            }
            else
            {
                joystick = joysticks[number < 2 ? 0 : number - 1];
                AcquireJoystick(joystick);
                return true;
            }
        }
        public string[] FindJoysticks()
        {
            systemJoysticks = null;
            try
            {
                DeviceList gameControllerList = Manager.GetDevices(DeviceClass.GameControl, EnumDevicesFlags.AttachedOnly);
                if (gameControllerList.Count > 0)
                {
                    systemJoysticks = new string[gameControllerList.Count];
                    int i = 0;
                    foreach (DeviceInstance deviceInstance in gameControllerList)
                    {
                        joystickDevice = new Device(deviceInstance.InstanceGuid);
                        joystickDevice.SetCooperativeLevel(this.form1, CooperativeLevelFlags.Background | CooperativeLevelFlags.NonExclusive);
                        systemJoysticks[i] = joystickDevice.DeviceInformation.InstanceName;
                        i++;
                    }
                }
            }
            catch { }
            return systemJoysticks;
        }
        public bool AcquireJoystick(string name)
        {
            try
            {
                DeviceList gameControllerList = Manager.GetDevices(DeviceClass.GameControl, EnumDevicesFlags.AttachedOnly);
                int i = 0;
                bool found = false;
                foreach (DeviceInstance deviceInstance in gameControllerList)
                {
                    if (deviceInstance.InstanceName == name)
                    {
                        found = true;
                        joystickDevice = new Device(deviceInstance.InstanceGuid);
                        joystickDevice.SetCooperativeLevel(this.form1, CooperativeLevelFlags.Background | CooperativeLevelFlags.NonExclusive);
                        break;
                    }
                    i++;
                }
                if (!found)
                    return false;
                joystickDevice.SetDataFormat(DeviceDataFormat.Joystick);
                joystickDevice.Acquire();
            }
            catch { }
            return true;
        }
        public void ReleaseJoystick()
        {
            joystickDevice.Unacquire();
        }
        private void ProcessStateLogic()
        {
            joystickDevice.Poll();
            state = joystickDevice.CurrentJoystickState;
            pointofview = state.GetPointOfView();
            sliders = state.GetSlider();
            asliders = state.GetASlider();
            fsliders = state.GetFSlider();
            vsliders = state.GetVSlider();
            jsButtons = state.GetButtons();
            buttons = new bool[jsButtons.Length];
            int i = 0;
            foreach (byte button in jsButtons)
            {
                buttons[i] = button >= 128;
                i++;
            }
            JoystickAxisX = state.X;
            JoystickAxisY = state.Y;
            JoystickAxisZ = state.Z;
            JoystickRotationX = state.Rx;
            JoystickRotationY = state.Ry;
            JoystickRotationZ = state.Rz;
            JoystickSliders0 = sliders[0];
            JoystickSliders1 = sliders[1];
            JoystickPointOfViewControllers0 = pointofview[0];
            JoystickPointOfViewControllers1 = pointofview[1];
            JoystickPointOfViewControllers2 = pointofview[2];
            JoystickPointOfViewControllers3 = pointofview[3];
            JoystickVelocityX = state.VX;
            JoystickVelocityY = state.VY;
            JoystickVelocityZ = state.VZ;
            JoystickAngularVelocityX = state.ARx;
            JoystickAngularVelocityY = state.ARy;
            JoystickAngularVelocityZ = state.ARz;
            JoystickVelocitySliders0 = vsliders[0];
            JoystickVelocitySliders1 = vsliders[1];
            JoystickAccelerationX = state.AX;
            JoystickAccelerationY = state.AY;
            JoystickAccelerationZ = state.AZ;
            JoystickAngularAccelerationX = state.VRx;
            JoystickAngularAccelerationY = state.VRy;
            JoystickAngularAccelerationZ = state.VRz;
            JoystickAccelerationSliders0 = asliders[0];
            JoystickAccelerationSliders1 = asliders[1];
            JoystickForceX = state.FX;
            JoystickForceY = state.FY;
            JoystickForceZ = state.FZ;
            JoystickTorqueX = state.FRx;
            JoystickTorqueY = state.FRy;
            JoystickTorqueZ = state.FRz;
            JoystickForceSliders0 = fsliders[0];
            JoystickForceSliders1 = fsliders[1];
            JoystickButtons0 = buttons[0];
            JoystickButtons1 = buttons[1];
            JoystickButtons2 = buttons[2];
            JoystickButtons3 = buttons[3];
            JoystickButtons4 = buttons[4];
            JoystickButtons5 = buttons[5];
            JoystickButtons6 = buttons[6];
            JoystickButtons7 = buttons[7];
            JoystickButtons8 = buttons[8];
            JoystickButtons9 = buttons[9];
            JoystickButtons10 = buttons[10];
            JoystickButtons11 = buttons[11];
            JoystickButtons12 = buttons[12];
            JoystickButtons13 = buttons[13];
            JoystickButtons14 = buttons[14];
            JoystickButtons15 = buttons[15];
            JoystickButtons16 = buttons[16];
            JoystickButtons17 = buttons[17];
            JoystickButtons18 = buttons[18];
            JoystickButtons19 = buttons[19];
            JoystickButtons20 = buttons[20];
            JoystickButtons21 = buttons[21];
            JoystickButtons22 = buttons[22];
            JoystickButtons23 = buttons[23];
            JoystickButtons24 = buttons[24];
            JoystickButtons25 = buttons[25];
            JoystickButtons26 = buttons[26];
            JoystickButtons27 = buttons[27];
            JoystickButtons28 = buttons[28];
            JoystickButtons29 = buttons[29];
            JoystickButtons30 = buttons[30];
            JoystickButtons31 = buttons[31];
            JoystickButtons32 = buttons[32];
            JoystickButtons33 = buttons[33];
            JoystickButtons34 = buttons[34];
            JoystickButtons35 = buttons[35];
            JoystickButtons36 = buttons[36];
            JoystickButtons37 = buttons[37];
            JoystickButtons38 = buttons[38];
            JoystickButtons39 = buttons[39];
            JoystickButtons40 = buttons[40];
            JoystickButtons41 = buttons[41];
            JoystickButtons42 = buttons[42];
            JoystickButtons43 = buttons[43];
            JoystickButtons44 = buttons[44];
            JoystickButtons45 = buttons[45];
            JoystickButtons46 = buttons[46];
            JoystickButtons47 = buttons[47];
            JoystickButtons48 = buttons[48];
            JoystickButtons49 = buttons[49];
            JoystickButtons50 = buttons[50];
            JoystickButtons51 = buttons[51];
            JoystickButtons52 = buttons[52];
            JoystickButtons53 = buttons[53];
            JoystickButtons54 = buttons[54];
            JoystickButtons55 = buttons[55];
            JoystickButtons56 = buttons[56];
            JoystickButtons57 = buttons[57];
            JoystickButtons58 = buttons[58];
            JoystickButtons59 = buttons[59];
            JoystickButtons60 = buttons[60];
            JoystickButtons61 = buttons[61];
            JoystickButtons62 = buttons[62];
            JoystickButtons63 = buttons[63];
            JoystickButtons64 = buttons[64];
            JoystickButtons65 = buttons[65];
            JoystickButtons66 = buttons[66];
            JoystickButtons67 = buttons[67];
            JoystickButtons68 = buttons[68];
            JoystickButtons69 = buttons[69];
            JoystickButtons70 = buttons[70];
            JoystickButtons71 = buttons[71];
            JoystickButtons72 = buttons[72];
            JoystickButtons73 = buttons[73];
            JoystickButtons74 = buttons[74];
            JoystickButtons75 = buttons[75];
            JoystickButtons76 = buttons[76];
            JoystickButtons77 = buttons[77];
            JoystickButtons78 = buttons[78];
            JoystickButtons79 = buttons[79];
            JoystickButtons80 = buttons[80];
            JoystickButtons81 = buttons[81];
            JoystickButtons82 = buttons[82];
            JoystickButtons83 = buttons[83];
            JoystickButtons84 = buttons[84];
            JoystickButtons85 = buttons[85];
            JoystickButtons86 = buttons[86];
            JoystickButtons87 = buttons[87];
            JoystickButtons88 = buttons[88];
            JoystickButtons89 = buttons[89];
            JoystickButtons90 = buttons[90];
            JoystickButtons91 = buttons[91];
            JoystickButtons92 = buttons[92];
            JoystickButtons93 = buttons[93];
            JoystickButtons94 = buttons[94];
            JoystickButtons95 = buttons[95];
            JoystickButtons96 = buttons[96];
            JoystickButtons97 = buttons[97];
            JoystickButtons98 = buttons[98];
            JoystickButtons99 = buttons[99];
            JoystickButtons100 = buttons[100];
            JoystickButtons101 = buttons[101];
            JoystickButtons102 = buttons[102];
            JoystickButtons103 = buttons[103];
            JoystickButtons104 = buttons[104];
            JoystickButtons105 = buttons[105];
            JoystickButtons106 = buttons[106];
            JoystickButtons107 = buttons[107];
            JoystickButtons108 = buttons[108];
            JoystickButtons109 = buttons[109];
            JoystickButtons110 = buttons[110];
            JoystickButtons111 = buttons[111];
            JoystickButtons112 = buttons[112];
            JoystickButtons113 = buttons[113];
            JoystickButtons114 = buttons[114];
            JoystickButtons115 = buttons[115];
            JoystickButtons116 = buttons[116];
            JoystickButtons117 = buttons[117];
            JoystickButtons118 = buttons[118];
            JoystickButtons119 = buttons[119];
            JoystickButtons120 = buttons[120];
            JoystickButtons121 = buttons[121];
            JoystickButtons122 = buttons[122];
            JoystickButtons123 = buttons[123];
            JoystickButtons124 = buttons[124];
            JoystickButtons125 = buttons[125];
            JoystickButtons126 = buttons[126];
            JoystickButtons127 = buttons[127];
        }
    }
}