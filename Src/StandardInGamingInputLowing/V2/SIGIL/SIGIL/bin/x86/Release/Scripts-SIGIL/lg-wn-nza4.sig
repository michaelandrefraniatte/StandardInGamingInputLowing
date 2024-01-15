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
using WiiMotesAPI;
using Interceptions;
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
        private static double MouseDesktopX; static double MouseDesktopY; static double int_1_deltaX = 0; static double int_1_deltaY = 0; static double int_1_x = 0; static double int_1_y = 0; static bool int_1_SendLeftClick; static bool int_1_SendRightClick; static bool int_1_SendMiddleClick; static bool int_1_SendWheelUp; static bool int_1_SendWheelDown; static bool int_1_SendCANCEL; static bool int_1_SendBACK; static bool int_1_SendTAB; static bool int_1_SendCLEAR; static bool int_1_SendRETURN; static bool int_1_SendSHIFT; static bool int_1_SendCONTROL; static bool int_1_SendMENU; static bool int_1_SendCAPITAL; static bool int_1_SendESCAPE; static bool int_1_SendSPACE; static bool int_1_SendPRIOR; static bool int_1_SendNEXT; static bool int_1_SendEND; static bool int_1_SendHOME; static bool int_1_SendLEFT; static bool int_1_SendUP; static bool int_1_SendRIGHT; static bool int_1_SendDOWN; static bool int_1_SendSNAPSHOT; static bool int_1_SendINSERT; static bool int_1_SendNUMPADDEL; static bool int_1_SendNUMPADINSERT; static bool int_1_SendHELP; static bool int_1_SendAPOSTROPHE; static bool int_1_SendBACKSPACE; static bool int_1_SendPAGEDOWN; static bool int_1_SendPAGEUP; static bool int_1_SendFIN; static bool int_1_SendMOUSE; static bool int_1_SendA; static bool int_1_SendB; static bool int_1_SendC; static bool int_1_SendD; static bool int_1_SendE; static bool int_1_SendF; static bool int_1_SendG; static bool int_1_SendH; static bool int_1_SendI; static bool int_1_SendJ; static bool int_1_SendK; static bool int_1_SendL; static bool int_1_SendM; static bool int_1_SendN; static bool int_1_SendO; static bool int_1_SendP; static bool int_1_SendQ; static bool int_1_SendR; static bool int_1_SendS; static bool int_1_SendT; static bool int_1_SendU; static bool int_1_SendV; static bool int_1_SendW; static bool int_1_SendX; static bool int_1_SendY; static bool int_1_SendZ; static bool int_1_SendLWIN; static bool int_1_SendRWIN; static bool int_1_SendAPPS; static bool int_1_SendDELETE; static bool int_1_SendNUMPAD0; static bool int_1_SendNUMPAD1; static bool int_1_SendNUMPAD2; static bool int_1_SendNUMPAD3; static bool int_1_SendNUMPAD4; static bool int_1_SendNUMPAD5; static bool int_1_SendNUMPAD6; static bool int_1_SendNUMPAD7; static bool int_1_SendNUMPAD8; static bool int_1_SendNUMPAD9; static bool int_1_SendMULTIPLY; static bool int_1_SendADD; static bool int_1_SendSUBTRACT; static bool int_1_SendDECIMAL; static bool int_1_SendPRINTSCREEN; static bool int_1_SendDIVIDE; static bool int_1_SendF1; static bool int_1_SendF2; static bool int_1_SendF3; static bool int_1_SendF4; static bool int_1_SendF5; static bool int_1_SendF6; static bool int_1_SendF7; static bool int_1_SendF8; static bool int_1_SendF9; static bool int_1_SendF10; static bool int_1_SendF11; static bool int_1_SendF12; static bool int_1_SendNUMLOCK; static bool int_1_SendSCROLLLOCK; static bool int_1_SendLEFTSHIFT; static bool int_1_SendRIGHTSHIFT; static bool int_1_SendLEFTCONTROL; static bool int_1_SendRIGHTCONTROL; static bool int_1_SendLEFTALT; static bool int_1_SendRIGHTALT; static bool int_1_SendBROWSER_BACK; static bool int_1_SendBROWSER_FORWARD; static bool int_1_SendBROWSER_REFRESH; static bool int_1_SendBROWSER_STOP; static bool int_1_SendBROWSER_SEARCH; static bool int_1_SendBROWSER_FAVORITES; static bool int_1_SendBROWSER_HOME; static bool int_1_SendVOLUME_MUTE; static bool int_1_SendVOLUME_DOWN; static bool int_1_SendVOLUME_UP; static bool int_1_SendMEDIA_NEXT_TRACK; static bool int_1_SendMEDIA_PREV_TRACK; static bool int_1_SendMEDIA_STOP; static bool int_1_SendMEDIA_PLAY_PAUSE; static bool int_1_SendLAUNCH_MAIL; static bool int_1_SendLAUNCH_MEDIA_SELECT; static bool int_1_SendLAUNCH_APP1; static bool int_1_SendLAUNCH_APP2; static bool int_1_SendOEM_1; static bool int_1_SendOEM_PLUS; static bool int_1_SendOEM_COMMA; static bool int_1_SendOEM_MINUS; static bool int_1_SendOEM_PERIOD; static bool int_1_SendOEM_2; static bool int_1_SendOEM_3; static bool int_1_SendOEM_4; static bool int_1_SendOEM_5; static bool int_1_SendOEM_6; static bool int_1_SendOEM_7; static bool int_1_SendOEM_8; static bool int_1_SendOEM_102; static bool int_1_SendEREOF; static bool int_1_SendZOOM; static bool int_1_SendEscape; static bool int_1_SendOne; static bool int_1_SendTwo; static bool int_1_SendThree; static bool int_1_SendFour; static bool int_1_SendFive; static bool int_1_SendSix; static bool int_1_SendSeven; static bool int_1_SendEight; static bool int_1_SendNine; static bool int_1_SendZero; static bool int_1_SendDashUnderscore; static bool int_1_SendPlusEquals; static bool int_1_SendBackspace; static bool int_1_SendTab; static bool int_1_SendOpenBracketBrace; static bool int_1_SendCloseBracketBrace; static bool int_1_SendEnter; static bool int_1_SendControl; static bool int_1_SendSemicolonColon; static bool int_1_SendSingleDoubleQuote; static bool int_1_SendTilde; static bool int_1_SendLeftShift; static bool int_1_SendBackslashPipe; static bool int_1_SendCommaLeftArrow; static bool int_1_SendPeriodRightArrow; static bool int_1_SendForwardSlashQuestionMark; static bool int_1_SendRightShift; static bool int_1_SendRightAlt; static bool int_1_SendSpace; static bool int_1_SendCapsLock; static bool int_1_SendUp; static bool int_1_SendDown; static bool int_1_SendRight; static bool int_1_SendLeft; static bool int_1_SendHome; static bool int_1_SendEnd; static bool int_1_SendDelete; static bool int_1_SendPageUp; static bool int_1_SendPageDown; static bool int_1_SendInsert; static bool int_1_SendPrintScreen; static bool int_1_SendNumLock; static bool int_1_SendScrollLock; static bool int_1_SendMenu; static bool int_1_SendWindowsKey; static bool int_1_SendNumpadDivide; static bool int_1_SendNumpadAsterisk; static bool int_1_SendNumpad7; static bool int_1_SendNumpad8; static bool int_1_SendNumpad9; static bool int_1_SendNumpad4; static bool int_1_SendNumpad5; static bool int_1_SendNumpad6; static bool int_1_SendNumpad1; static bool int_1_SendNumpad2; static bool int_1_SendNumpad3; static bool int_1_SendNumpad0; static bool int_1_SendNumpadDelete; static bool int_1_SendNumpadEnter; static bool int_1_SendNumpadPlus; static bool int_1_SendNumpadMinus;
        private static int keyboard_1_id = 1, mouse_1_id = 12;
        private static double statex = 0f, statey = 0f, mousex = 0f, mousey = 0f, mousexp = 0f, mouseyp = 0f, mousestatex = 0f, mousestatey = 0f, dzx = 0.0f, dzy = 0.0f, viewpower1x = 0f, viewpower2x = 1f, viewpower3x = 0f, viewpower1y = 0.25f, viewpower2y = 0.75f, viewpower3y = 0f, viewpower05x = 0f, viewpower05y = 0f;
        private static bool[] getstate = new bool[12];
        private static int sleeptime = 1;
        private static int irmode = 2;
        private static double centery = 80f;
        public static Valuechange ValueChange = new Valuechange();
        private WiiMote wm = new WiiMote();
        public static SendInterception SI = new SendInterception();
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
                wm.Close();
                SI.Disconnect();
            }
            catch { }
        }
        public void Load()
        {
            TimeBeginPeriod(1);
            NtSetTimerResolution(1, true, ref CurrentResolution);
            Task.Run(() => Start());
        }
        private void Start()
        {
            running = true;
            wm.Scan(irmode, centery);
            wm.BeginPolling();
            Thread.Sleep(1000);
            wm.Init();
            SI.Connect();
            Task.Run(() => task());
        }
        private void task()
        {
            for (; ; )
            {
                if (!running)
                    break;
                valchanged(0, wm.WiimoteButtonStateOne);
                if (wd[0] == 1 & !getstate[0]) 
                {
                    width  = System.Windows.Forms.Screen.PrimaryScreen.Bounds.Width;
                    height = System.Windows.Forms.Screen.PrimaryScreen.Bounds.Height;
                    getstate[0] = true;
                }
                else 
                { 
                    if (wd[0] == 1 & getstate[0]) 
                    {
                        int_1_deltaX         = 0;
                        int_1_deltaY         = 0;
                        MouseDesktopX        = 0;
                        MouseDesktopY        = 0;
                        int_1_x              = 0;
                        int_1_y              = 0;
                        int_1_SendD          = false;
                        int_1_SendQ          = false;
                        int_1_SendZ          = false;
                        int_1_SendS          = false;
                        int_1_SendEight      = false;
                        int_1_SendSeven      = false;
                        int_1_SendNine       = false;
                        int_1_SendSix        = false;
                        int_1_SendB          = false;  
                        int_1_SendOne        = false;
                        int_1_SendTwo        = false;
                        int_1_SendThree      = false;
                        int_1_SendFour       = false;
                        int_1_SendSpace      = false;
                        int_1_SendLeftShift  = false;
                        int_1_SendE          = false;
                        int_1_SendA          = false;
                        int_1_SendV          = false;
                        int_1_SendEscape     = false;
                        int_1_SendTab        = false;
                        int_1_SendR          = false;
                        int_1_SendF          = false;
                        int_1_SendT          = false;
                        int_1_SendG          = false;
                        int_1_SendY          = false; 
                        int_1_SendU          = false;
                        int_1_SendX          = false;
                        int_1_SendC          = false;
                        int_1_SendRightClick = false;
                        int_1_SendLeftClick  = false;
                        getstate[0]       = false;
                    }
                }
                if (getstate[0]) 
                {
                    if (wm.irx >= 0f)
                        mousex = Scale(wm.irx * wm.irx * wm.irx / 1024f / 1024f * viewpower3x + wm.irx * wm.irx / 1024f * viewpower2x + wm.irx * viewpower1x, 0f, 1024f, (dzx / 100f) * 1024f, 1024f);
                    if (wm.irx <= 0f)
                        mousex = Scale(-(-wm.irx * -wm.irx * -wm.irx) / 1024f / 1024f * viewpower3x - (-wm.irx * -wm.irx) / 1024f * viewpower2x - (-wm.irx) * viewpower1x, -1024f, 0f, -1024f, -(dzx / 100f) * 1024f);
                    if (wm.iry >= 0f)
                        mousey = Scale(wm.iry * wm.iry * wm.iry / 1024f / 1024f * viewpower3y + wm.iry * wm.iry / 1024f * viewpower2y + wm.iry * viewpower1y, 0f, 1024f, (dzy / 100f) * 1024f, 1024f);
                    if (wm.iry <= 0f)
                        mousey = Scale(-(-wm.iry * -wm.iry * -wm.iry) / 1024f / 1024f * viewpower3y - (-wm.iry * -wm.iry) / 1024f * viewpower2y - (-wm.iry) * viewpower1y, -1024f, 0f, -1024f, -(dzy / 100f) * 1024f);  
                    int_1_deltaX        = -mousex * 600 / 1024f;
                    int_1_deltaY        = mousey * 400 / 1024f;
                    MouseDesktopX       = width / 2f - mousex * width / 2f / 1024f;
                    MouseDesktopY       = height / 2f + mousey * height / 2f / 1024f;
                    int_1_SendD         = wm.WiimoteNunchuckStateRawJoystickX >= 60f; /* Droite */
                    int_1_SendA         = wm.WiimoteNunchuckStateRawJoystickX <= -60f; /* Gauche */
                    int_1_SendW         = wm.WiimoteNunchuckStateRawJoystickY >= 60f; /* Avancer */
                    int_1_SendS         = wm.WiimoteNunchuckStateRawJoystickY <= -60f; /* Reculer */
                    int_1_SendSpace     = wm.WiimoteNunchuckStateC; /* Frapper fort */
                    int_1_SendLeftShift = wm.WiimoteNunchuckStateZ; /* Courir */
                    int_1_SendV         = wm.WiimoteNunchuckStateRawValuesY > 33f; /* Coup de pieds */
                    int_1_SendEscape    = wm.WiimoteButtonStateTwo; /* Passer */
                    int_1_SendTab       = wm.WiimoteButtonStateOne; /* Map */
                    int_1_SendR         = ((wm.WiimoteRawValuesZ > 0 ? wm.WiimoteRawValuesZ : -wm.WiimoteRawValuesZ) >= 30f & (wm.WiimoteRawValuesY > 0 ? wm.WiimoteRawValuesY : -wm.WiimoteRawValuesY) >= 30f & (wm.WiimoteRawValuesX > 0 ? wm.WiimoteRawValuesX : -wm.WiimoteRawValuesX) >= 30f); /* Recharger */
                    int_1_SendF         = wm.WiimoteButtonStateHome; /* Action */
                    int_1_SendT         = wm.WiimoteButtonStateMinus; /* Soigner */
                    int_1_SendG         = wm.WiimoteButtonStatePlus; /* Utiliser objet */
                    int_1_SendY         = wm.WiimoteButtonStateLeft; /* Objet suivant */
                    int_1_SendU         = wm.WiimoteButtonStateRight; /* Arme suivante */
                    int_1_SendX         = wm.WiimoteButtonStateUp; /* Zoomer */
                    int_1_SendC         = wm.WiimoteButtonStateDown; /* A genoux */
                    int_1_SendLeftClick = wm.WiimoteButtonStateB; /* Tirer */
                    valchanged(1, wm.WiimoteButtonStateA);
                    if (wd[1] == 1 & !getstate[1])
                    {
                        getstate[1] = true;
                    }
                    else
                    {
                        if (wd[1] == 1 & getstate[1])
                        {
                            getstate[1] = false;
                        }
                    }
                    if (int_1_SendSpace | int_1_SendLeftShift | int_1_SendV | int_1_SendEscape | int_1_SendTab | int_1_SendR | int_1_SendF | int_1_SendT | int_1_SendG | int_1_SendY | int_1_SendU | int_1_SendX | int_1_SendC)
                    {
                        getstate[1] = false;
                    }
                    int_1_SendRightClick = getstate[1]; /* Viser */
                }
                SI.Set(keyboard_1_id, mouse_1_id, MouseDesktopX, MouseDesktopY, int_1_deltaX, int_1_deltaY, int_1_x, int_1_y, int_1_SendLeftClick, int_1_SendRightClick, int_1_SendMiddleClick, int_1_SendWheelUp, int_1_SendWheelDown, int_1_SendCANCEL, int_1_SendBACK, int_1_SendTAB, int_1_SendCLEAR, int_1_SendRETURN, int_1_SendSHIFT, int_1_SendCONTROL, int_1_SendMENU, int_1_SendCAPITAL, int_1_SendESCAPE, int_1_SendSPACE, int_1_SendPRIOR, int_1_SendNEXT, int_1_SendEND, int_1_SendHOME, int_1_SendLEFT, int_1_SendUP, int_1_SendRIGHT, int_1_SendDOWN, int_1_SendSNAPSHOT, int_1_SendINSERT, int_1_SendNUMPADDEL, int_1_SendNUMPADINSERT, int_1_SendHELP, int_1_SendAPOSTROPHE, int_1_SendBACKSPACE, int_1_SendPAGEDOWN, int_1_SendPAGEUP, int_1_SendFIN, int_1_SendMOUSE, int_1_SendA, int_1_SendB, int_1_SendC, int_1_SendD, int_1_SendE, int_1_SendF, int_1_SendG, int_1_SendH, int_1_SendI, int_1_SendJ, int_1_SendK, int_1_SendL, int_1_SendM, int_1_SendN, int_1_SendO, int_1_SendP, int_1_SendQ, int_1_SendR, int_1_SendS, int_1_SendT, int_1_SendU, int_1_SendV, int_1_SendW, int_1_SendX, int_1_SendY, int_1_SendZ, int_1_SendLWIN, int_1_SendRWIN, int_1_SendAPPS, int_1_SendDELETE, int_1_SendNUMPAD0, int_1_SendNUMPAD1, int_1_SendNUMPAD2, int_1_SendNUMPAD3, int_1_SendNUMPAD4, int_1_SendNUMPAD5, int_1_SendNUMPAD6, int_1_SendNUMPAD7, int_1_SendNUMPAD8, int_1_SendNUMPAD9, int_1_SendMULTIPLY, int_1_SendADD, int_1_SendSUBTRACT, int_1_SendDECIMAL, int_1_SendPRINTSCREEN, int_1_SendDIVIDE, int_1_SendF1, int_1_SendF2, int_1_SendF3, int_1_SendF4, int_1_SendF5, int_1_SendF6, int_1_SendF7, int_1_SendF8, int_1_SendF9, int_1_SendF10, int_1_SendF11, int_1_SendF12, int_1_SendNUMLOCK, int_1_SendSCROLLLOCK, int_1_SendLEFTSHIFT, int_1_SendRIGHTSHIFT, int_1_SendLEFTCONTROL, int_1_SendRIGHTCONTROL, int_1_SendLEFTALT, int_1_SendRIGHTALT, int_1_SendBROWSER_BACK, int_1_SendBROWSER_FORWARD, int_1_SendBROWSER_REFRESH, int_1_SendBROWSER_STOP, int_1_SendBROWSER_SEARCH, int_1_SendBROWSER_FAVORITES, int_1_SendBROWSER_HOME, int_1_SendVOLUME_MUTE, int_1_SendVOLUME_DOWN, int_1_SendVOLUME_UP, int_1_SendMEDIA_NEXT_TRACK, int_1_SendMEDIA_PREV_TRACK, int_1_SendMEDIA_STOP, int_1_SendMEDIA_PLAY_PAUSE, int_1_SendLAUNCH_MAIL, int_1_SendLAUNCH_MEDIA_SELECT, int_1_SendLAUNCH_APP1, int_1_SendLAUNCH_APP2, int_1_SendOEM_1, int_1_SendOEM_PLUS, int_1_SendOEM_COMMA, int_1_SendOEM_MINUS, int_1_SendOEM_PERIOD, int_1_SendOEM_2, int_1_SendOEM_3, int_1_SendOEM_4, int_1_SendOEM_5, int_1_SendOEM_6, int_1_SendOEM_7, int_1_SendOEM_8, int_1_SendOEM_102, int_1_SendEREOF, int_1_SendZOOM, int_1_SendEscape, int_1_SendOne, int_1_SendTwo, int_1_SendThree, int_1_SendFour, int_1_SendFive, int_1_SendSix, int_1_SendSeven, int_1_SendEight, int_1_SendNine, int_1_SendZero, int_1_SendDashUnderscore, int_1_SendPlusEquals, int_1_SendBackspace, int_1_SendTab, int_1_SendOpenBracketBrace, int_1_SendCloseBracketBrace, int_1_SendEnter, int_1_SendControl, int_1_SendSemicolonColon, int_1_SendSingleDoubleQuote, int_1_SendTilde, int_1_SendLeftShift, int_1_SendBackslashPipe, int_1_SendCommaLeftArrow, int_1_SendPeriodRightArrow, int_1_SendForwardSlashQuestionMark, int_1_SendRightShift, int_1_SendRightAlt, int_1_SendSpace, int_1_SendCapsLock, int_1_SendUp, int_1_SendDown, int_1_SendRight, int_1_SendLeft, int_1_SendHome, int_1_SendEnd, int_1_SendDelete, int_1_SendPageUp, int_1_SendPageDown, int_1_SendInsert, int_1_SendPrintScreen, int_1_SendNumLock, int_1_SendScrollLock, int_1_SendMenu, int_1_SendWindowsKey, int_1_SendNumpadDivide, int_1_SendNumpadAsterisk, int_1_SendNumpad7, int_1_SendNumpad8, int_1_SendNumpad9, int_1_SendNumpad4, int_1_SendNumpad5, int_1_SendNumpad6, int_1_SendNumpad1, int_1_SendNumpad2, int_1_SendNumpad3, int_1_SendNumpad0, int_1_SendNumpadDelete, int_1_SendNumpadEnter, int_1_SendNumpadPlus, int_1_SendNumpadMinus);
                /*wm.ViewData();*/
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