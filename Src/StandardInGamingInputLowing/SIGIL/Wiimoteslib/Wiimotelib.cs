using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using Wiimoteslib;
using WiimoteLib;

namespace WiiMotesLibAPI
{
    public class WiiMoteLib
    {
        [DllImport("MotionInputPairing.dll", EntryPoint = "wiimoteconnect")]
        private static extern bool wiimoteconnect();
        [DllImport("MotionInputPairing.dll", EntryPoint = "wiimotedisconnect")]
        private static extern bool wiimotedisconnect();
        [DllImport("MotionInputPairing.dll", EntryPoint = "wiimotesconnect")]
        private static extern bool wiimotesconnect();
        [DllImport("MotionInputPairing.dll", EntryPoint = "wiimotesdisconnect")]
        private static extern bool wiimotesdisconnect();
        [DllImport("winmm.dll", EntryPoint = "timeBeginPeriod")]
        private static extern uint TimeBeginPeriod(uint ms);
        [DllImport("winmm.dll", EntryPoint = "timeEndPeriod")]
        private static extern uint TimeEndPeriod(uint ms);
        [DllImport("ntdll.dll", EntryPoint = "NtSetTimerResolution")]
        private static extern void NtSetTimerResolution(uint DesiredResolution, bool SetResolution, ref uint CurrentResolution);
        private static uint CurrentResolution = 0;
        private bool running, formvisible;
        private int irmode;
        private List<double> vallistirx = new List<double>(), vallistiry = new List<double>();
        private double irxc, iryc, irx0, iry0, irx1, iry1, irx2, iry2, irx3, iry3, WiimoteIRSensors0X, WiimoteIRSensors0Y, WiimoteIRSensors1X, WiimoteIRSensors1Y, calibrationinit, WiimoteIRSensors0Xcam, WiimoteIRSensors0Ycam, WiimoteIRSensors1Xcam, WiimoteIRSensors1Ycam, WiimoteIRSensorsXcam, WiimoteIRSensorsYcam;
        public double irx, iry, WiimoteRawValuesX, WiimoteRawValuesY, WiimoteRawValuesZ;
        public bool WiimoteButtonStateA, WiimoteButtonStateB, WiimoteButtonStateMinus, WiimoteButtonStateHome, WiimoteButtonStatePlus, WiimoteButtonStateOne, WiimoteButtonStateTwo, WiimoteButtonStateUp, WiimoteButtonStateDown, WiimoteButtonStateLeft, WiimoteButtonStateRight, WiimoteNunchuckStateC, WiimoteNunchuckStateZ;
        private bool WiimoteIR0foundcam, WiimoteIR1foundcam, WiimoteIRswitch, WiimoteIR1found, WiimoteIR0found;
        public double WiimoteNunchuckStateRawValuesX, WiimoteNunchuckStateRawValuesY, WiimoteNunchuckStateRawValuesZ, WiimoteNunchuckStateRawJoystickX, WiimoteNunchuckStateRawJoystickY;
        private double WiimoteIR0notfound, stickviewxinit, stickviewyinit, centery;
        private bool WiimoteGuitarStatePlus, WiimoteGuitarStateMinus, WiimoteGuitarStateStrumDown, WiimoteGuitarStateStrumUp, WiimoteGuitarStateFretBlue, WiimoteGuitarStateFretGreen, WiimoteGuitarStateFretOrange, WiimoteGuitarStateFretRed, WiimoteGuitarStateFretYellow, WiimoteClassicControllerStateA, WiimoteClassicControllerStateB, WiimoteClassicControllerStateDown, WiimoteClassicControllerStateHome, WiimoteClassicControllerStateLeft, WiimoteClassicControllerStateMinus, WiimoteClassicControllerStatePlus, WiimoteClassicControllerStateRight, WiimoteClassicControllerStateTriggerL, WiimoteClassicControllerStateTriggerR, WiimoteClassicControllerStateUp, WiimoteClassicControllerStateX, WiimoteClassicControllerStateY, WiimoteClassicControllerStateZL, WiimoteClassicControllerStateZR, WiimoteDrumsStateBlue, WiimoteDrumsStateGreen, WiimoteDrumsStateOrange, WiimoteDrumsStateRed, WiimoteDrumsStateYellow, WiimoteDrumsStateMinus, WiimoteDrumsStatePlus, WiimoteDrumsStatePedal;
        private double WiimoteGuitarStateRawJoystickX, WiimoteGuitarStateRawJoystickY, WiimoteGuitarStateRawWhammyBar, WiimoteClassicControllerStateRawJoystickLeftX, WiimoteClassicControllerStateRawJoystickLeftY, WiimoteClassicControllerStateRawJoystickRightX, WiimoteClassicControllerStateRawJoystickRightY, WiimoteClassicControllerStateRawTriggerL, WiimoteClassicControllerStateRawTriggerR, WiimoteBalanceBoardStateCenterOfGravityX, WiimoteBalanceBoardStateCenterOfGravityY, WiimoteBalanceBoardStateSensorValuesKgBottomLeft, WiimoteBalanceBoardStateSensorValuesKgBottomRight, WiimoteBalanceBoardStateSensorValuesKgTopLeft, WiimoteBalanceBoardStateSensorValuesKgTopRight, WiimoteBalanceBoardStateWeightKg, WiimoteDrumsStateBlueVelocity, WiimoteDrumsStateGreenVelocity, WiimoteDrumsStateOrangeVelocity, WiimoteDrumsStateRedVelocity, WiimoteDrumsStateYellowVelocity, WiimoteDrumsStatePedalVelocity, WiimoteDrumsStateRawJoystickX, WiimoteDrumsStateRawJoystickY;
        private int number;
        private WiimoteCollection mWC; 
        private Wiimote wiimote;
        private static List<Wiimote> wiimotes = new List<Wiimote>();
        private Form1 form1 = new Form1();
        public WiiMoteLib()
        {
            TimeBeginPeriod(1);
            NtSetTimerResolution(1, true, ref CurrentResolution);
            running = true;
            while (vallistirx.Count <= 20)
            {
                vallistirx.Add(0);
            }
            while (vallistiry.Count <= 20)
            {
                vallistiry.Add(0);
            }
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
            Thread.Sleep(100);
            wiimote.Disconnect();
            wiimote.Dispose();
        }
        public void BeginPolling()
        {
            Task.Run(() => taskD());
        }
        private void taskD()
        {
            for (; ; )
            {
                if (!running)
                    break; 
                Thread.Sleep(10);
                if (formvisible)
                {
                    string str = "irx : " + irx + Environment.NewLine;
                    str += "iry : " + iry + Environment.NewLine;
                    str += "WiimoteButtonStateA : " + WiimoteButtonStateA + Environment.NewLine;
                    str += "WiimoteButtonStateB : " + WiimoteButtonStateB + Environment.NewLine;
                    str += "WiimoteButtonStateMinus : " + WiimoteButtonStateMinus + Environment.NewLine;
                    str += "WiimoteButtonStateHome : " + WiimoteButtonStateHome + Environment.NewLine;
                    str += "WiimoteButtonStatePlus : " + WiimoteButtonStatePlus + Environment.NewLine;
                    str += "WiimoteButtonStateOne : " + WiimoteButtonStateOne + Environment.NewLine;
                    str += "WiimoteButtonStateTwo : " + WiimoteButtonStateTwo + Environment.NewLine;
                    str += "WiimoteButtonStateUp : " + WiimoteButtonStateUp + Environment.NewLine;
                    str += "WiimoteButtonStateDown : " + WiimoteButtonStateDown + Environment.NewLine;
                    str += "WiimoteButtonStateLeft : " + WiimoteButtonStateLeft + Environment.NewLine;
                    str += "WiimoteButtonStateRight : " + WiimoteButtonStateRight + Environment.NewLine;
                    str += "WiimoteRawValuesX : " + WiimoteRawValuesX + Environment.NewLine;
                    str += "WiimoteRawValuesY : " + WiimoteRawValuesY + Environment.NewLine;
                    str += "WiimoteRawValuesZ : " + WiimoteRawValuesZ + Environment.NewLine;
                    str += "WiimoteNunchuckStateRawJoystickX : " + WiimoteNunchuckStateRawJoystickX + Environment.NewLine;
                    str += "WiimoteNunchuckStateRawJoystickY : " + WiimoteNunchuckStateRawJoystickY + Environment.NewLine;
                    str += "WiimoteNunchuckStateRawValuesX : " + WiimoteNunchuckStateRawValuesX + Environment.NewLine;
                    str += "WiimoteNunchuckStateRawValuesY : " + WiimoteNunchuckStateRawValuesY + Environment.NewLine;
                    str += "WiimoteNunchuckStateRawValuesZ : " + WiimoteNunchuckStateRawValuesZ + Environment.NewLine;
                    str += "WiimoteNunchuckStateC : " + WiimoteNunchuckStateC + Environment.NewLine;
                    str += "WiimoteNunchuckStateZ : " + WiimoteNunchuckStateZ + Environment.NewLine; 
                    str += "WiimoteGuitarStatePlus : " + WiimoteGuitarStatePlus + Environment.NewLine; 
                    str += "WiimoteGuitarStateMinus : " + WiimoteGuitarStateMinus + Environment.NewLine; 
                    str += "WiimoteGuitarStateStrumDown : " + WiimoteGuitarStateStrumDown + Environment.NewLine; 
                    str += "WiimoteGuitarStateStrumUp : " + WiimoteGuitarStateStrumUp + Environment.NewLine; 
                    str += "WiimoteGuitarStateFretBlue : " + WiimoteGuitarStateFretBlue + Environment.NewLine; 
                    str += "WiimoteGuitarStateFretGreen : " + WiimoteGuitarStateFretGreen + Environment.NewLine; 
                    str += "WiimoteGuitarStateFretOrange : " + WiimoteGuitarStateFretOrange + Environment.NewLine; 
                    str += "WiimoteGuitarStateFretRed : " + WiimoteGuitarStateFretRed + Environment.NewLine; 
                    str += "WiimoteGuitarStateFretYellow : " + WiimoteGuitarStateFretYellow + Environment.NewLine; 
                    str += "WiimoteGuitarStateRawJoystickX : " + WiimoteGuitarStateRawJoystickX + Environment.NewLine; 
                    str += "WiimoteGuitarStateRawJoystickY : " + WiimoteGuitarStateRawJoystickY + Environment.NewLine; 
                    str += "WiimoteGuitarStateRawWhammyBar : " + WiimoteGuitarStateRawWhammyBar + Environment.NewLine; 
                    str += "WiimoteClassicControllerStateA : " + WiimoteClassicControllerStateA + Environment.NewLine; 
                    str += "WiimoteClassicControllerStateB : " + WiimoteClassicControllerStateB + Environment.NewLine; 
                    str += "WiimoteClassicControllerStateDown : " + WiimoteClassicControllerStateDown + Environment.NewLine; 
                    str += "WiimoteClassicControllerStateHome : " + WiimoteClassicControllerStateHome + Environment.NewLine; 
                    str += "WiimoteClassicControllerStateLeft : " + WiimoteClassicControllerStateLeft + Environment.NewLine; 
                    str += "WiimoteClassicControllerStateMinus : " + WiimoteClassicControllerStateMinus + Environment.NewLine; 
                    str += "WiimoteClassicControllerStatePlus : " + WiimoteClassicControllerStatePlus + Environment.NewLine; 
                    str += "WiimoteClassicControllerStateRight : " + WiimoteClassicControllerStateRight + Environment.NewLine; 
                    str += "WiimoteClassicControllerStateTriggerL : " + WiimoteClassicControllerStateTriggerL + Environment.NewLine; 
                    str += "WiimoteClassicControllerStateTriggerR : " + WiimoteClassicControllerStateTriggerR + Environment.NewLine; 
                    str += "WiimoteClassicControllerStateUp : " + WiimoteClassicControllerStateUp + Environment.NewLine; 
                    str += "WiimoteClassicControllerStateX : " + WiimoteClassicControllerStateX + Environment.NewLine; 
                    str += "WiimoteClassicControllerStateY : " + WiimoteClassicControllerStateY + Environment.NewLine; 
                    str += "WiimoteClassicControllerStateZL : " + WiimoteClassicControllerStateZL + Environment.NewLine; 
                    str += "WiimoteClassicControllerStateZR : " + WiimoteClassicControllerStateZR + Environment.NewLine; 
                    str += "WiimoteClassicControllerStateRawJoystickLeftX : " + WiimoteClassicControllerStateRawJoystickLeftX + Environment.NewLine; 
                    str += "WiimoteClassicControllerStateRawJoystickLeftY : " + WiimoteClassicControllerStateRawJoystickLeftY + Environment.NewLine; 
                    str += "WiimoteClassicControllerStateRawJoystickRightX : " + WiimoteClassicControllerStateRawJoystickRightX + Environment.NewLine; 
                    str += "WiimoteClassicControllerStateRawJoystickRightY : " + WiimoteClassicControllerStateRawJoystickRightY + Environment.NewLine; 
                    str += "WiimoteClassicControllerStateRawTriggerL : " + WiimoteClassicControllerStateRawTriggerL + Environment.NewLine; 
                    str += "WiimoteClassicControllerStateRawTriggerR : " + WiimoteClassicControllerStateRawTriggerR + Environment.NewLine; 
                    str += "WiimoteBalanceBoardStateCenterOfGravityX : " + WiimoteBalanceBoardStateCenterOfGravityX + Environment.NewLine; 
                    str += "WiimoteBalanceBoardStateCenterOfGravityY : " + WiimoteBalanceBoardStateCenterOfGravityY + Environment.NewLine; 
                    str += "WiimoteBalanceBoardStateSensorValuesKgBottomLeft : " + WiimoteBalanceBoardStateSensorValuesKgBottomLeft + Environment.NewLine; 
                    str += "WiimoteBalanceBoardStateSensorValuesKgBottomRight : " + WiimoteBalanceBoardStateSensorValuesKgBottomRight + Environment.NewLine; 
                    str += "WiimoteBalanceBoardStateSensorValuesKgTopLeft : " + WiimoteBalanceBoardStateSensorValuesKgTopLeft + Environment.NewLine; 
                    str += "WiimoteBalanceBoardStateSensorValuesKgTopRight : " + WiimoteBalanceBoardStateSensorValuesKgTopRight + Environment.NewLine; 
                    str += "WiimoteBalanceBoardStateWeightKg : " + WiimoteBalanceBoardStateWeightKg + Environment.NewLine; 
                    str += "WiimoteDrumsStateBlue : " + WiimoteDrumsStateBlue + Environment.NewLine; 
                    str += "WiimoteDrumsStateBlueVelocity : " + WiimoteDrumsStateBlueVelocity + Environment.NewLine; 
                    str += "WiimoteDrumsStateGreen : " + WiimoteDrumsStateGreen + Environment.NewLine; 
                    str += "WiimoteDrumsStateGreenVelocity : " + WiimoteDrumsStateGreenVelocity + Environment.NewLine; 
                    str += "WiimoteDrumsStateOrange : " + WiimoteDrumsStateOrange + Environment.NewLine; 
                    str += "WiimoteDrumsStateOrangeVelocity : " + WiimoteDrumsStateOrangeVelocity + Environment.NewLine; 
                    str += "WiimoteDrumsStateRed : " + WiimoteDrumsStateRed + Environment.NewLine; 
                    str += "WiimoteDrumsStateRedVelocity : " + WiimoteDrumsStateRedVelocity + Environment.NewLine; 
                    str += "WiimoteDrumsStateYellow : " + WiimoteDrumsStateYellow + Environment.NewLine; 
                    str += "WiimoteDrumsStateYellowVelocity : " + WiimoteDrumsStateYellowVelocity + Environment.NewLine; 
                    str += "WiimoteDrumsStateMinus : " + WiimoteDrumsStateMinus + Environment.NewLine; 
                    str += "WiimoteDrumsStatePlus : " + WiimoteDrumsStatePlus + Environment.NewLine; 
                    str += "WiimoteDrumsStatePedal : " + WiimoteDrumsStatePedal + Environment.NewLine; 
                    str += "WiimoteDrumsStatePedalVelocity : " + WiimoteDrumsStatePedalVelocity + Environment.NewLine; 
                    str += "WiimoteDrumsStateRawJoystickX : " + WiimoteDrumsStateRawJoystickX + Environment.NewLine; 
                    str += "WiimoteDrumsStateRawJoystickY : " + WiimoteDrumsStateRawJoystickY + Environment.NewLine;
                    str += Environment.NewLine;
                    form1.SetLabel1(str);
                }
            }
        }
        public void Init()
        {
            calibrationinit = -WiimoteRawValuesY;
            stickviewxinit = -WiimoteNunchuckStateRawJoystickX;
            stickviewyinit = -WiimoteNunchuckStateRawJoystickY;
        }
        private double Scale(double value, double min, double max, double minScale, double maxScale)
        {
            double scaled = minScale + (double)(value - min) / (max - min) * (maxScale - minScale);
            return scaled;
        }
        public void Scan(int irmode, double centery, int number = 0)
        {
            this.irmode = irmode;
            this.centery = centery;
            this.number = number;
            if (number == 0)
                do
                    Thread.Sleep(1);
                while (!wiimoteconnect());
            else if (number == 1)
                do
                    Thread.Sleep(1);
                while (!wiimotesconnect());
            if (number <= 1)
            {
                mWC = new WiimoteCollection();
                mWC.FindAllWiimotes();
                foreach (Wiimote wm in mWC)
                {
                    wm.Connect();
                    if (wm.WiimoteState.ExtensionType != ExtensionType.BalanceBoard)
                        wm.SetReportType(InputReport.IRExtensionAccel, IRSensitivity.Maximum, true);
                    wiimotes.Add(wm);
                }
            }
            wiimote = wiimotes[number < 2 ? 0 : number - 1];
            wiimote.WiimoteChanged += new EventHandler<WiimoteChangedEventArgs>(wm_WiimoteChanged);
            wiimote.WiimoteExtensionChanged += new EventHandler<WiimoteExtensionChangedEventArgs>(wm_WiimoteExtensionChanged);
        }
        private void wm_WiimoteChanged(object sender, WiimoteChangedEventArgs e)
        {
            WiimoteState ws = e.WiimoteState;
            if (irmode == 1)
            {
                WiimoteIRSensors0X = ws.IRState.IRSensors[0].RawPosition.X;
                WiimoteIRSensors0Y = ws.IRState.IRSensors[0].RawPosition.Y;
                WiimoteIRSensors1X = ws.IRState.IRSensors[1].RawPosition.X;
                WiimoteIRSensors1Y = ws.IRState.IRSensors[1].RawPosition.Y;
                WiimoteIR0found = WiimoteIRSensors0X > 0f & WiimoteIRSensors0X <= 1024f & WiimoteIRSensors0Y > 0f & WiimoteIRSensors0Y <= 768f;
                WiimoteIR1found = WiimoteIRSensors1X > 0f & WiimoteIRSensors1X <= 1024f & WiimoteIRSensors1Y > 0f & WiimoteIRSensors1Y <= 768f;
                if (WiimoteIR0found)
                {
                    WiimoteIRSensors0Xcam = WiimoteIRSensors0X - 512f;
                    WiimoteIRSensors0Ycam = WiimoteIRSensors0Y - 384f;
                }
                if (WiimoteIR1found)
                {
                    WiimoteIRSensors1Xcam = WiimoteIRSensors1X - 512f;
                    WiimoteIRSensors1Ycam = WiimoteIRSensors1Y - 384f;
                }
                if (WiimoteIR0found & WiimoteIR1found)
                {
                    WiimoteIRSensorsXcam = (WiimoteIRSensors0Xcam + WiimoteIRSensors1Xcam) / 2f;
                    WiimoteIRSensorsYcam = (WiimoteIRSensors0Ycam + WiimoteIRSensors1Ycam) / 2f;
                }
                if (WiimoteIR0found)
                {
                    irx0 = 2 * WiimoteIRSensors0Xcam - WiimoteIRSensorsXcam;
                    iry0 = 2 * WiimoteIRSensors0Ycam - WiimoteIRSensorsYcam;
                }
                if (WiimoteIR1found)
                {
                    irx1 = 2 * WiimoteIRSensors1Xcam - WiimoteIRSensorsXcam;
                    iry1 = 2 * WiimoteIRSensors1Ycam - WiimoteIRSensorsYcam;
                }
                irxc = irx0 + irx1;
                iryc = iry0 + iry1;
            }
            else if (irmode == 2)
            {
                WiimoteIR0found = ws.IRState.IRSensors[0].Found;
                WiimoteIR1found = ws.IRState.IRSensors[1].Found;
                if (WiimoteIR0notfound == 0 & WiimoteIR1found)
                    WiimoteIR0notfound = 1;
                if (WiimoteIR0notfound == 1 & !WiimoteIR0found & !WiimoteIR1found)
                    WiimoteIR0notfound = 2;
                if (WiimoteIR0notfound == 2 & WiimoteIR0found)
                {
                    WiimoteIR0notfound = 0;
                    if (!WiimoteIRswitch)
                        WiimoteIRswitch = true;
                    else
                        WiimoteIRswitch = false;
                }
                if (WiimoteIR0notfound == 0 & WiimoteIR0found)
                    WiimoteIR0notfound = 0;
                if (WiimoteIR0notfound == 0 & !WiimoteIR0found & !WiimoteIR1found)
                    WiimoteIR0notfound = 0;
                if (WiimoteIR0notfound == 1 & WiimoteIR0found)
                    WiimoteIR0notfound = 0;
                if (WiimoteIR0found)
                {
                    WiimoteIRSensors0X = ws.IRState.IRSensors[0].RawPosition.X;
                    WiimoteIRSensors0Y = ws.IRState.IRSensors[0].RawPosition.Y;
                }
                if (WiimoteIR1found)
                {
                    WiimoteIRSensors1X = ws.IRState.IRSensors[1].RawPosition.X;
                    WiimoteIRSensors1Y = ws.IRState.IRSensors[1].RawPosition.Y;
                }
                if (WiimoteIRswitch)
                {
                    WiimoteIR0foundcam = WiimoteIR0found;
                    WiimoteIR1foundcam = WiimoteIR1found;
                    WiimoteIRSensors0Xcam = WiimoteIRSensors0X - 512f;
                    WiimoteIRSensors0Ycam = WiimoteIRSensors0Y - 384f;
                    WiimoteIRSensors1Xcam = WiimoteIRSensors1X - 512f;
                    WiimoteIRSensors1Ycam = WiimoteIRSensors1Y - 384f;
                }
                else
                {
                    WiimoteIR1foundcam = WiimoteIR0found;
                    WiimoteIR0foundcam = WiimoteIR1found;
                    WiimoteIRSensors1Xcam = WiimoteIRSensors0X - 512f;
                    WiimoteIRSensors1Ycam = WiimoteIRSensors0Y - 384f;
                    WiimoteIRSensors0Xcam = WiimoteIRSensors1X - 512f;
                    WiimoteIRSensors0Ycam = WiimoteIRSensors1Y - 384f;
                }
                if (WiimoteIR0foundcam & WiimoteIR1foundcam)
                {
                    irx2 = WiimoteIRSensors0Xcam;
                    iry2 = WiimoteIRSensors0Ycam;
                    irx3 = WiimoteIRSensors1Xcam;
                    iry3 = WiimoteIRSensors1Ycam;
                    WiimoteIRSensorsXcam = WiimoteIRSensors0Xcam - WiimoteIRSensors1Xcam;
                    WiimoteIRSensorsYcam = WiimoteIRSensors0Ycam - WiimoteIRSensors1Ycam;
                }
                if (WiimoteIR0foundcam & !WiimoteIR1foundcam)
                {
                    irx2 = WiimoteIRSensors0Xcam;
                    iry2 = WiimoteIRSensors0Ycam;
                    irx3 = WiimoteIRSensors0Xcam - WiimoteIRSensorsXcam;
                    iry3 = WiimoteIRSensors0Ycam - WiimoteIRSensorsYcam;
                }
                if (WiimoteIR1foundcam & !WiimoteIR0foundcam)
                {
                    irx3 = WiimoteIRSensors1Xcam;
                    iry3 = WiimoteIRSensors1Ycam;
                    irx2 = WiimoteIRSensors1Xcam + WiimoteIRSensorsXcam;
                    iry2 = WiimoteIRSensors1Ycam + WiimoteIRSensorsYcam;
                }
                irxc = irx2 + irx3;
                iryc = iry2 + iry3;
            }
            else if (irmode == 3)
            {
                WiimoteIR0found = ws.IRState.IRSensors[0].Found;
                WiimoteIR1found = ws.IRState.IRSensors[1].Found;
                if (WiimoteIR0found & WiimoteIR1found)
                {
                    WiimoteIRSensors0X = ws.IRState.IRSensors[0].RawPosition.X;
                    WiimoteIRSensors0Y = ws.IRState.IRSensors[0].RawPosition.Y;
                    irx2 = WiimoteIRSensors0X - 512f;
                    iry2 = WiimoteIRSensors0Y - 384f;
                    WiimoteIRSensors1X = ws.IRState.IRSensors[1].RawPosition.X;
                    WiimoteIRSensors1Y = ws.IRState.IRSensors[1].RawPosition.Y;
                    irx3 = WiimoteIRSensors1X - 512f;
                    iry3 = WiimoteIRSensors1Y - 384f;
                }
                irxc = (irx2 + irx3) / 512f * 1346f;
                iryc = (iry2 + iry3) / 768f * 782f;
            }
            if (WiimoteIR0found | WiimoteIR1found)
            {
                vallistirx.Add(irx);
                vallistirx.RemoveAt(0);
                vallistiry.Add(iry);
                vallistiry.RemoveAt(0);
                irx = irxc * (1024f / 1346f);
                iry = iryc + centery >= 0 ? Scale(iryc + centery, 0f, 782f + centery, 0f, 1024f) : Scale(iryc + centery, -782f + centery, 0f, -1024f, 0f);
            }
            else
            {
                if (irx - vallistirx.Average() >= 600f)
                    irx = 1024f;
                if (irx - vallistirx.Average() <= -600f)
                    irx = -1024f;
                if (iry - vallistiry.Average() >= 200f)
                    iry = 1024f;
                if (iry - vallistiry.Average() <= -200f)
                    iry = -1024f;
            }
            WiimoteButtonStateA = ws.ButtonState.A;
            WiimoteButtonStateB = ws.ButtonState.B;
            WiimoteButtonStateMinus = ws.ButtonState.Minus;
            WiimoteButtonStateHome = ws.ButtonState.Home;
            WiimoteButtonStatePlus = ws.ButtonState.Plus;
            WiimoteButtonStateOne = ws.ButtonState.One;
            WiimoteButtonStateTwo = ws.ButtonState.Two;
            WiimoteButtonStateUp = ws.ButtonState.Up;
            WiimoteButtonStateDown = ws.ButtonState.Down;
            WiimoteButtonStateLeft = ws.ButtonState.Left;
            WiimoteButtonStateRight = ws.ButtonState.Right;
            WiimoteRawValuesX = ws.AccelState.RawValues.X + calibrationinit;
            WiimoteRawValuesY = ws.AccelState.RawValues.Y + calibrationinit;
            WiimoteRawValuesZ = ws.AccelState.RawValues.Z + calibrationinit;
            if (ws.ExtensionType == ExtensionType.Nunchuk | ws.ExtensionType == ExtensionType.NewNunchuk)
            {
                WiimoteNunchuckStateRawJoystickX = ws.NunchukState.RawJoystick.X + stickviewxinit;
                WiimoteNunchuckStateRawJoystickY = ws.NunchukState.RawJoystick.Y + stickviewyinit;
                WiimoteNunchuckStateRawValuesX = ws.NunchukState.AccelState.RawValues.X;
                WiimoteNunchuckStateRawValuesY = ws.NunchukState.AccelState.RawValues.Y;
                WiimoteNunchuckStateRawValuesZ = ws.NunchukState.AccelState.RawValues.Z;
                WiimoteNunchuckStateC = ws.NunchukState.C;
                WiimoteNunchuckStateZ = ws.NunchukState.Z;
            }
            if (ws.ExtensionType == ExtensionType.Guitar)
            {
                WiimoteGuitarStatePlus = ws.GuitarState.ButtonState.Plus;
                WiimoteGuitarStateMinus = ws.GuitarState.ButtonState.Minus;
                WiimoteGuitarStateStrumDown = ws.GuitarState.ButtonState.StrumDown;
                WiimoteGuitarStateStrumUp = ws.GuitarState.ButtonState.StrumUp;
                WiimoteGuitarStateFretBlue = ws.GuitarState.FretButtonState.Blue;
                WiimoteGuitarStateFretGreen = ws.GuitarState.FretButtonState.Green;
                WiimoteGuitarStateFretOrange = ws.GuitarState.FretButtonState.Orange;
                WiimoteGuitarStateFretRed = ws.GuitarState.FretButtonState.Red;
                WiimoteGuitarStateFretYellow = ws.GuitarState.FretButtonState.Yellow;
                WiimoteGuitarStateRawJoystickX = ws.GuitarState.RawJoystick.X;
                WiimoteGuitarStateRawJoystickY = ws.GuitarState.RawJoystick.Y;
                WiimoteGuitarStateRawWhammyBar = ws.GuitarState.RawWhammyBar;
            }
            if (ws.ExtensionType == ExtensionType.ClassicController | ws.ExtensionType == ExtensionType.ClassicControllerPro)
            {
                WiimoteClassicControllerStateA = ws.ClassicControllerState.ButtonState.A;
                WiimoteClassicControllerStateB = ws.ClassicControllerState.ButtonState.B;
                WiimoteClassicControllerStateDown = ws.ClassicControllerState.ButtonState.Down;
                WiimoteClassicControllerStateHome = ws.ClassicControllerState.ButtonState.Home;
                WiimoteClassicControllerStateLeft = ws.ClassicControllerState.ButtonState.Left;
                WiimoteClassicControllerStateMinus = ws.ClassicControllerState.ButtonState.Minus;
                WiimoteClassicControllerStatePlus = ws.ClassicControllerState.ButtonState.Plus;
                WiimoteClassicControllerStateRight = ws.ClassicControllerState.ButtonState.Right;
                WiimoteClassicControllerStateTriggerL = ws.ClassicControllerState.ButtonState.TriggerL;
                WiimoteClassicControllerStateTriggerR = ws.ClassicControllerState.ButtonState.TriggerR;
                WiimoteClassicControllerStateUp = ws.ClassicControllerState.ButtonState.Up;
                WiimoteClassicControllerStateX = ws.ClassicControllerState.ButtonState.X;
                WiimoteClassicControllerStateY = ws.ClassicControllerState.ButtonState.Y;
                WiimoteClassicControllerStateZL = ws.ClassicControllerState.ButtonState.ZL;
                WiimoteClassicControllerStateZR = ws.ClassicControllerState.ButtonState.ZR;
                WiimoteClassicControllerStateRawJoystickLeftX = ws.ClassicControllerState.RawJoystickL.X;
                WiimoteClassicControllerStateRawJoystickLeftY = ws.ClassicControllerState.RawJoystickL.Y;
                WiimoteClassicControllerStateRawJoystickRightX = ws.ClassicControllerState.RawJoystickR.X;
                WiimoteClassicControllerStateRawJoystickRightY = ws.ClassicControllerState.RawJoystickR.Y;
                WiimoteClassicControllerStateRawTriggerL = ws.ClassicControllerState.RawTriggerL;
                WiimoteClassicControllerStateRawTriggerR = ws.ClassicControllerState.RawTriggerR;
            }
            if (ws.ExtensionType == ExtensionType.BalanceBoard)
            {
                WiimoteBalanceBoardStateCenterOfGravityX = ws.BalanceBoardState.CenterOfGravity.X;
                WiimoteBalanceBoardStateCenterOfGravityY = ws.BalanceBoardState.CenterOfGravity.Y;
                WiimoteBalanceBoardStateSensorValuesKgBottomLeft = ws.BalanceBoardState.SensorValuesKg.BottomLeft;
                WiimoteBalanceBoardStateSensorValuesKgBottomRight = ws.BalanceBoardState.SensorValuesKg.BottomRight;
                WiimoteBalanceBoardStateSensorValuesKgTopLeft = ws.BalanceBoardState.SensorValuesKg.TopLeft;
                WiimoteBalanceBoardStateSensorValuesKgTopRight = ws.BalanceBoardState.SensorValuesKg.TopRight;
                WiimoteBalanceBoardStateWeightKg = ws.BalanceBoardState.WeightKg;
            }
            if (ws.ExtensionType == ExtensionType.Drums)
            {
                WiimoteDrumsStateBlue = ws.DrumsState.Blue;
                WiimoteDrumsStateBlueVelocity = ws.DrumsState.BlueVelocity;
                WiimoteDrumsStateGreen = ws.DrumsState.Green;
                WiimoteDrumsStateGreenVelocity = ws.DrumsState.GreenVelocity;
                WiimoteDrumsStateOrange = ws.DrumsState.Orange;
                WiimoteDrumsStateOrangeVelocity = ws.DrumsState.OrangeVelocity;
                WiimoteDrumsStateRed = ws.DrumsState.Red;
                WiimoteDrumsStateRedVelocity = ws.DrumsState.RedVelocity;
                WiimoteDrumsStateYellow = ws.DrumsState.Yellow;
                WiimoteDrumsStateYellowVelocity = ws.DrumsState.YellowVelocity;
                WiimoteDrumsStateMinus = ws.DrumsState.Minus;
                WiimoteDrumsStatePlus = ws.DrumsState.Plus;
                WiimoteDrumsStatePedal = ws.DrumsState.Pedal;
                WiimoteDrumsStatePedalVelocity = ws.DrumsState.PedalVelocity;
                WiimoteDrumsStateRawJoystickX = ws.DrumsState.RawJoystick.X;
                WiimoteDrumsStateRawJoystickY = ws.DrumsState.RawJoystick.Y;
            }
        }
        private void wm_WiimoteExtensionChanged(object sender, WiimoteExtensionChangedEventArgs e)
        {
            if (e.Inserted)
                ((Wiimote)sender).SetReportType(InputReport.IRExtensionAccel, true);
            else
                ((Wiimote)sender).SetReportType(InputReport.IRAccel, true);
        }
    }
}