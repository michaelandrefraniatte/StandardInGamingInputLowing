using System;
using System.Globalization;
using System.IO;
using System.Drawing;
using System.Numerics;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;
using System.Reflection;
using System.Diagnostics;
using Valuechanges;
using DualShocks4API;
using keyboardsmouses;
namespace StringToCode
{
    public class FooClass 
    { 
        [DllImport("winmm.dll", EntryPoint = "timeBeginPeriod")]
        private static extern uint TimeBeginPeriod(uint ms);
        [DllImport("winmm.dll", EntryPoint = "timeEndPeriod")]
        private static extern uint TimeEndPeriod(uint ms);
        [DllImport("ntdll.dll", EntryPoint = "NtSetTimerResolution")]
        private static extern void NtSetTimerResolution(uint DesiredResolution, bool SetResolution, ref uint CurrentResolution);
        private static uint CurrentResolution = 0;
        private static bool running;
        private static int width, height;
        static string KeyboardMouseDriverType = ""; static double MouseMoveX; static double MouseMoveY; static double MouseAbsX; static double MouseAbsY; static double MouseDesktopX; static double MouseDesktopY; static bool SendLeftClick; static bool SendRightClick; static bool SendMiddleClick; static bool SendWheelUp; static bool SendWheelDown; static bool SendLeft; static bool SendRight; static bool SendUp; static bool SendDown; static bool SendLButton; static bool SendRButton; static bool SendCancel; static bool SendMBUTTON; static bool SendXBUTTON1; static bool SendXBUTTON2; static bool SendBack; static bool SendTab; static bool SendClear; static bool SendReturn; static bool SendSHIFT; static bool SendCONTROL; static bool SendMENU; static bool SendPAUSE; static bool SendCAPITAL; static bool SendKANA; static bool SendHANGEUL; static bool SendHANGUL; static bool SendJUNJA; static bool SendFINAL; static bool SendHANJA; static bool SendKANJI; static bool SendEscape; static bool SendCONVERT; static bool SendNONCONVERT; static bool SendACCEPT; static bool SendMODECHANGE; static bool SendSpace; static bool SendPRIOR; static bool SendNEXT; static bool SendEND; static bool SendHOME; static bool SendLEFT; static bool SendUP; static bool SendRIGHT; static bool SendDOWN; static bool SendSELECT; static bool SendPRINT; static bool SendEXECUTE; static bool SendSNAPSHOT; static bool SendINSERT; static bool SendDELETE; static bool SendHELP; static bool SendAPOSTROPHE; static bool Send0; static bool Send1; static bool Send2; static bool Send3; static bool Send4; static bool Send5; static bool Send6; static bool Send7; static bool Send8; static bool Send9; static bool SendA; static bool SendB; static bool SendC; static bool SendD; static bool SendE; static bool SendF; static bool SendG; static bool SendH; static bool SendI; static bool SendJ; static bool SendK; static bool SendL; static bool SendM; static bool SendN; static bool SendO; static bool SendP; static bool SendQ; static bool SendR; static bool SendS; static bool SendT; static bool SendU; static bool SendV; static bool SendW; static bool SendX; static bool SendY; static bool SendZ; static bool SendLWIN; static bool SendRWIN; static bool SendAPPS; static bool SendSLEEP; static bool SendNUMPAD0; static bool SendNUMPAD1; static bool SendNUMPAD2; static bool SendNUMPAD3; static bool SendNUMPAD4; static bool SendNUMPAD5; static bool SendNUMPAD6; static bool SendNUMPAD7; static bool SendNUMPAD8; static bool SendNUMPAD9; static bool SendMULTIPLY; static bool SendADD; static bool SendSEPARATOR; static bool SendSUBTRACT; static bool SendDECIMAL; static bool SendDIVIDE; static bool SendF1; static bool SendF2; static bool SendF3; static bool SendF4; static bool SendF5; static bool SendF6; static bool SendF7; static bool SendF8; static bool SendF9; static bool SendF10; static bool SendF11; static bool SendF12; static bool SendF13; static bool SendF14; static bool SendF15; static bool SendF16; static bool SendF17; static bool SendF18; static bool SendF19; static bool SendF20; static bool SendF21; static bool SendF22; static bool SendF23; static bool SendF24; static bool SendNUMLOCK; static bool SendSCROLL; static bool SendLeftShift; static bool SendRightShift; static bool SendLeftControl; static bool SendRightControl; static bool SendLMENU; static bool SendRMENU;
        private static double statex = 0f, statey = 0f, mousexp = 0f, mouseyp = 0f, mousex = 0f, mousey = 0f, mousestatex = 0f, mousestatey = 0f, dzx = 0.0f, dzy = 0.0f, viewpower1x = 0f, viewpower2x = 1f, viewpower3x = 0f, viewpower1y = 0.25f, viewpower2y = 0.75f, viewpower3y = 0f, viewpower05x = 0f, viewpower05y = 0f;
        private static bool[] getstate = new bool[12];
        private static int sleeptime = 8;
        public static Valuechange ValueChange = new Valuechange();
        private static string vendor_ds4_id = "54C", product_ds4_id = "9CC", product_ds4_label = "Wireless Controller";
        public DualShock4 ds4 = new DualShock4();
        public static SendKeyboardMouse SKM = new SendKeyboardMouse();
        private static int[] wd = { 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2 };
        private static int[] wu = { 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2 };
        public static void valchanged(int n, bool val)
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
        public void Close()
        {
            try
            {
                running = false;
                Thread.Sleep(100);
                ds4.Close();
                SKM.Disconnect();
                ds4.Dispose();
                SKM.Dispose();
            }
            catch { }
        }
        public static void Main() {}
        public void Load()
        {
            TimeBeginPeriod(1);
            NtSetTimerResolution(1, true, ref CurrentResolution);
            Task.Run(() => Start());
        }
        private void Start()
        {
            running = true;
            ds4.Scan(vendor_ds4_id, product_ds4_id, product_ds4_label);
            ds4.BeginPolling();
            SKM.Connect();
            Task.Run(() => task());
        }
        private void task()
        {
            for (; ; )
            {
                if (!running)
                    break;
                valchanged(0, ds4.PS4ControllerButtonCreatePressed);
                if (wd[0] == 1 & !getstate[0]) 
                {
                    KeyboardMouseDriverType = "kmevent";
                    width                   = System.Windows.Forms.Screen.PrimaryScreen.Bounds.Width;
                    height                  = System.Windows.Forms.Screen.PrimaryScreen.Bounds.Height;
                    getstate[0] = true;
                }
                else 
                { 
                    if (wd[0] == 1 & getstate[0]) 
                    {
                        MouseMoveX              = 0;
                        MouseMoveY              = 0;
                        MouseDesktopX           = 0;
                        MouseDesktopY           = 0;
                        MouseAbsX               = 0;
                        MouseAbsY               = 0;
                        SendD                   = false;
                        SendQ                   = false;
                        SendZ                   = false;
                        SendS                   = false;
                        Send8                   = false;
                        Send7                   = false;
                        Send9                   = false;
                        Send6                   = false;
                        SendB                   = false;  
                        Send1                   = false;
                        Send2                   = false;
                        Send3                   = false;
                        Send4                   = false;
                        SendSpace               = false;
                        SendLeftShift           = false;
                        SendE                   = false;
                        SendA                   = false;
                        SendV                   = false;
                        SendEscape              = false;
                        SendTab                 = false;
                        SendR                   = false;
                        SendF                   = false;
                        SendT                   = false;
                        SendG                   = false;
                        SendY                   = false; 
                        SendU                   = false;
                        SendX                   = false;
                        SendC                   = false;
                        SendRightClick          = false;
                        SendLeftClick           = false;
                        getstate[0]    = false;
                    }
                }
                if (getstate[0]) 
                {
                    if (ds4.PS4ControllerButtonMenuPressed)
                    {
                        mousexp = 0f;
                        mouseyp = 0f;
                        ds4.Init();
                    }
                    mousexp += Math.Round(ds4.PS4ControllerGyroX * 65535f / 2f / 1024f * 2f / 5000f, 0) * 1.5f;
                    mouseyp += Math.Round(ds4.PS4ControllerGyroY * 65535f / 2f / 1024f * 2f / 5000f, 0) * 1.5f;
                    if (mousexp >= width / 2f) 
                        mousexp = width / 2f;
                    if (mousexp <= -width / 2f) 
                        mousexp = -width / 2f;
                    if (mouseyp >= height / 2f) 
                        mouseyp = height / 2f;
                    if (mouseyp <= -height / 2f) 
                        mouseyp = -height / 2f;
                    MouseDesktopX  = width / 2f - mousexp - Math.Round(ds4.PS4ControllerGyroX * 65535f / 2f / 1024f * 2f / 5000f, 0) * 1.5f;
                    MouseDesktopY  = height / 2f + mouseyp + Math.Round(ds4.PS4ControllerGyroY * 65535f / 2f / 1024f * 2f / 5000f, 0) * 1.5f;
                    SendD          = ds4.PS4ControllerLeftStickX > 0.35f;
                    SendQ          = ds4.PS4ControllerLeftStickX < -0.35f;
                    SendZ          = ds4.PS4ControllerLeftStickY > 0.35f;
                    SendS          = ds4.PS4ControllerLeftStickY < -0.35f;
                    Send8          = ds4.PS4ControllerButtonDPadDownPressed;
                    Send7          = ds4.PS4ControllerButtonDPadLeftPressed;
                    Send9          = ds4.PS4ControllerButtonDPadRightPressed;
                    Send6          = ds4.PS4ControllerButtonDPadUpPressed;
                    SendSpace      = ds4.PS4ControllerButtonL1Pressed;
                    SendLeftShift  = ds4.PS4ControllerButtonR1Pressed;
                    SendE          = ds4.PS4ControllerButtonL3Pressed;
                    SendB          = ds4.PS4ControllerButtonR3Pressed;
                    SendR          = ds4.PS4ControllerButtonTrianglePressed;
                    SendF          = ds4.PS4ControllerButtonSquarePressed;
                    SendX          = ds4.PS4ControllerButtonCirclePressed;
                    SendC          = ds4.PS4ControllerButtonCrossPressed;
                    SendRightClick = ds4.PS4ControllerButtonL2Pressed;
                    SendLeftClick  = ds4.PS4ControllerButtonR2Pressed;
                    SendTab        = ds4.PS4ControllerButtonLogoPressed;
                    SendEscape     = ds4.PS4ControllerButtonTouchpadPressed;
                }
                SKM.Set(KeyboardMouseDriverType, MouseMoveX, MouseMoveY, MouseAbsX, MouseAbsY, MouseDesktopX, MouseDesktopY, SendLeftClick, SendRightClick, SendMiddleClick, SendWheelUp, SendWheelDown, SendLeft, SendRight, SendUp, SendDown, SendLButton, SendRButton, SendCancel, SendMBUTTON, SendXBUTTON1, SendXBUTTON2, SendBack, SendTab, SendClear, SendReturn, SendSHIFT, SendCONTROL, SendMENU, SendPAUSE, SendCAPITAL, SendKANA, SendHANGEUL, SendHANGUL, SendJUNJA, SendFINAL, SendHANJA, SendKANJI, SendEscape, SendCONVERT, SendNONCONVERT, SendACCEPT, SendMODECHANGE, SendSpace, SendPRIOR, SendNEXT, SendEND, SendHOME, SendLEFT, SendUP, SendRIGHT, SendDOWN, SendSELECT, SendPRINT, SendEXECUTE, SendSNAPSHOT, SendINSERT, SendDELETE, SendHELP, SendAPOSTROPHE, Send0, Send1, Send2, Send3, Send4, Send5, Send6, Send7, Send8, Send9, SendA, SendB, SendC, SendD, SendE, SendF, SendG, SendH, SendI, SendJ, SendK, SendL, SendM, SendN, SendO, SendP, SendQ, SendR, SendS, SendT, SendU, SendV, SendW, SendX, SendY, SendZ, SendLWIN, SendRWIN, SendAPPS, SendSLEEP, SendNUMPAD0, SendNUMPAD1, SendNUMPAD2, SendNUMPAD3, SendNUMPAD4, SendNUMPAD5, SendNUMPAD6, SendNUMPAD7, SendNUMPAD8, SendNUMPAD9, SendMULTIPLY, SendADD, SendSEPARATOR, SendSUBTRACT, SendDECIMAL, SendDIVIDE, SendF1, SendF2, SendF3, SendF4, SendF5, SendF6, SendF7, SendF8, SendF9, SendF10, SendF11, SendF12, SendF13, SendF14, SendF15, SendF16, SendF17, SendF18, SendF19, SendF20, SendF21, SendF22, SendF23, SendF24, SendNUMLOCK, SendSCROLL, SendLeftShift, SendRightShift, SendLeftControl, SendRightControl, SendLMENU, SendRMENU);
                /*SKM.ViewData();*/
                /*ds4.ViewData();*/
                Thread.Sleep(sleeptime);
            }
        }
        private double Scale(double value, double min, double max, double minScale, double maxScale)
        {
            double scaled = minScale + (double)(value - min) / (max - min) * (maxScale - minScale);
            return scaled;
        }
    }
}