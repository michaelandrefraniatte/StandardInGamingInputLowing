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
        private static double MouseDesktopX; static double MouseDesktopY; static double int_deltaX = 0; static double int_deltaY = 0; static double int_x = 0; static double int_y = 0; static bool int_SendLeftClick; static bool int_SendRightClick; static bool int_SendMiddleClick; static bool int_SendWheelUp; static bool int_SendWheelDown; static bool int_SendCANCEL; static bool int_SendBACK; static bool int_SendTAB; static bool int_SendCLEAR; static bool int_SendRETURN; static bool int_SendSHIFT; static bool int_SendCONTROL; static bool int_SendMENU; static bool int_SendCAPITAL; static bool int_SendESCAPE; static bool int_SendSPACE; static bool int_SendPRIOR; static bool int_SendNEXT; static bool int_SendEND; static bool int_SendHOME; static bool int_SendLEFT; static bool int_SendUP; static bool int_SendRIGHT; static bool int_SendDOWN; static bool int_SendSNAPSHOT; static bool int_SendINSERT; static bool int_SendNUMPADDEL; static bool int_SendNUMPADINSERT; static bool int_SendHELP; static bool int_SendAPOSTROPHE; static bool int_SendBACKSPACE; static bool int_SendPAGEDOWN; static bool int_SendPAGEUP; static bool int_SendFIN; static bool int_SendMOUSE; static bool int_SendA; static bool int_SendB; static bool int_SendC; static bool int_SendD; static bool int_SendE; static bool int_SendF; static bool int_SendG; static bool int_SendH; static bool int_SendI; static bool int_SendJ; static bool int_SendK; static bool int_SendL; static bool int_SendM; static bool int_SendN; static bool int_SendO; static bool int_SendP; static bool int_SendQ; static bool int_SendR; static bool int_SendS; static bool int_SendT; static bool int_SendU; static bool int_SendV; static bool int_SendW; static bool int_SendX; static bool int_SendY; static bool int_SendZ; static bool int_SendLWIN; static bool int_SendRWIN; static bool int_SendAPPS; static bool int_SendDELETE; static bool int_SendNUMPAD0; static bool int_SendNUMPAD1; static bool int_SendNUMPAD2; static bool int_SendNUMPAD3; static bool int_SendNUMPAD4; static bool int_SendNUMPAD5; static bool int_SendNUMPAD6; static bool int_SendNUMPAD7; static bool int_SendNUMPAD8; static bool int_SendNUMPAD9; static bool int_SendMULTIPLY; static bool int_SendADD; static bool int_SendSUBTRACT; static bool int_SendDECIMAL; static bool int_SendPRINTSCREEN; static bool int_SendDIVIDE; static bool int_SendF1; static bool int_SendF2; static bool int_SendF3; static bool int_SendF4; static bool int_SendF5; static bool int_SendF6; static bool int_SendF7; static bool int_SendF8; static bool int_SendF9; static bool int_SendF10; static bool int_SendF11; static bool int_SendF12; static bool int_SendNUMLOCK; static bool int_SendSCROLLLOCK; static bool int_SendLEFTSHIFT; static bool int_SendRIGHTSHIFT; static bool int_SendLEFTCONTROL; static bool int_SendRIGHTCONTROL; static bool int_SendLEFTALT; static bool int_SendRIGHTALT; static bool int_SendBROWSER_BACK; static bool int_SendBROWSER_FORWARD; static bool int_SendBROWSER_REFRESH; static bool int_SendBROWSER_STOP; static bool int_SendBROWSER_SEARCH; static bool int_SendBROWSER_FAVORITES; static bool int_SendBROWSER_HOME; static bool int_SendVOLUME_MUTE; static bool int_SendVOLUME_DOWN; static bool int_SendVOLUME_UP; static bool int_SendMEDIA_NEXT_TRACK; static bool int_SendMEDIA_PREV_TRACK; static bool int_SendMEDIA_STOP; static bool int_SendMEDIA_PLAY_PAUSE; static bool int_SendLAUNCH_MAIL; static bool int_SendLAUNCH_MEDIA_SELECT; static bool int_SendLAUNCH_APP1; static bool int_SendLAUNCH_APP2; static bool int_SendOEM_1; static bool int_SendOEM_PLUS; static bool int_SendOEM_COMMA; static bool int_SendOEM_MINUS; static bool int_SendOEM_PERIOD; static bool int_SendOEM_2; static bool int_SendOEM_3; static bool int_SendOEM_4; static bool int_SendOEM_5; static bool int_SendOEM_6; static bool int_SendOEM_7; static bool int_SendOEM_8; static bool int_SendOEM_102; static bool int_SendEREOF; static bool int_SendZOOM; static bool int_SendEscape; static bool int_SendOne; static bool int_SendTwo; static bool int_SendThree; static bool int_SendFour; static bool int_SendFive; static bool int_SendSix; static bool int_SendSeven; static bool int_SendEight; static bool int_SendNine; static bool int_SendZero; static bool int_SendDashUnderscore; static bool int_SendPlusEquals; static bool int_SendBackspace; static bool int_SendTab; static bool int_SendOpenBracketBrace; static bool int_SendCloseBracketBrace; static bool int_SendEnter; static bool int_SendControl; static bool int_SendSemicolonColon; static bool int_SendSingleDoubleQuote; static bool int_SendTilde; static bool int_SendLeftShift; static bool int_SendBackslashPipe; static bool int_SendCommaLeftArrow; static bool int_SendPeriodRightArrow; static bool int_SendForwardSlashQuestionMark; static bool int_SendRightShift; static bool int_SendRightAlt; static bool int_SendSpace; static bool int_SendCapsLock; static bool int_SendUp; static bool int_SendDown; static bool int_SendRight; static bool int_SendLeft; static bool int_SendHome; static bool int_SendEnd; static bool int_SendDelete; static bool int_SendPageUp; static bool int_SendPageDown; static bool int_SendInsert; static bool int_SendPrintScreen; static bool int_SendNumLock; static bool int_SendScrollLock; static bool int_SendMenu; static bool int_SendWindowsKey; static bool int_SendNumpadDivide; static bool int_SendNumpadAsterisk; static bool int_SendNumpad7; static bool int_SendNumpad8; static bool int_SendNumpad9; static bool int_SendNumpad4; static bool int_SendNumpad5; static bool int_SendNumpad6; static bool int_SendNumpad1; static bool int_SendNumpad2; static bool int_SendNumpad3; static bool int_SendNumpad0; static bool int_SendNumpadDelete; static bool int_SendNumpadEnter; static bool int_SendNumpadPlus; static bool int_SendNumpadMinus;
        private static int keyboard_1_id = 1, mouse_1_id = 11;
        private static double statex = 0f, statey = 0f, mousex = 0f, mousey = 0f, mousexp = 0f, mouseyp = 0f, mousestatex = 0f, mousestatey = 0f, dzx = 0.2f, dzy = 0.2f, viewpower1x = 0.08f, viewpower2x = 0f, viewpower3x = 0.92f, viewpower1y = 0.08f, viewpower2y = 0f, viewpower3y = 0.92f, viewpower05x = 0f, viewpower05y = 0f;
        private static bool[] getstate = new bool[12];
        private static int sleeptime = 6;
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
                        int_deltaX         = 0;
                        int_deltaY         = 0;
                        MouseDesktopX        = 0;
                        MouseDesktopY        = 0;
                        int_x              = 0;
                        int_y              = 0;
                        int_SendD          = false;
                        int_SendQ          = false;
                        int_SendZ          = false;
                        int_SendS          = false;
                        int_SendEight      = false;
                        int_SendSeven      = false;
                        int_SendNine       = false;
                        int_SendSix        = false;
                        int_SendB          = false;  
                        int_SendOne        = false;
                        int_SendTwo        = false;
                        int_SendThree      = false;
                        int_SendFour       = false;
                        int_SendSpace      = false;
                        int_SendLeftShift  = false;
                        int_SendE          = false;
                        int_SendA          = false;
                        int_SendV          = false;
                        int_SendEscape     = false;
                        int_SendTab        = false;
                        int_SendR          = false;
                        int_SendF          = false;
                        int_SendT          = false;
                        int_SendG          = false;
                        int_SendY          = false; 
                        int_SendU          = false;
                        int_SendX          = false;
                        int_SendC          = false;
                        int_SendRightClick = false;
                        int_SendLeftClick  = false;
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
                    int_deltaX        = -mousex * 600 / 1024f;
                    int_deltaY        = mousey * 400 / 1024f;
                    MouseDesktopX       = width / 2f - mousex * width / 2f / 1024f;
                    MouseDesktopY       = height / 2f + mousey * height / 2f / 1024f;
                    int_SendD         = wm.WiimoteNunchuckStateRawJoystickX >= 60f; /* Droite */
                    int_SendA         = wm.WiimoteNunchuckStateRawJoystickX <= -60f; /* Gauche */
                    int_SendW         = wm.WiimoteNunchuckStateRawJoystickY >= 60f; /* Avancer */
                    int_SendS         = wm.WiimoteNunchuckStateRawJoystickY <= -60f; /* Reculer */
                    int_SendSpace     = wm.WiimoteNunchuckStateC; /* Frapper fort */
                    int_SendLeftShift = wm.WiimoteNunchuckStateZ; /* Courir */
                    int_SendV         = wm.WiimoteNunchuckStateRawValuesY > 33f; /* Coup de pieds */
                    int_SendEscape    = wm.WiimoteButtonStateTwo; /* Passer */
                    int_SendTab       = wm.WiimoteButtonStateOne; /* Map */
                    int_SendR         = ((wm.WiimoteRawValuesZ > 0 ? wm.WiimoteRawValuesZ : -wm.WiimoteRawValuesZ) >= 30f & (wm.WiimoteRawValuesY > 0 ? wm.WiimoteRawValuesY : -wm.WiimoteRawValuesY) >= 30f & (wm.WiimoteRawValuesX > 0 ? wm.WiimoteRawValuesX : -wm.WiimoteRawValuesX) >= 30f); /* Recharger */
                    int_SendF         = wm.WiimoteButtonStateHome; /* Action */
                    int_SendT         = wm.WiimoteButtonStateMinus; /* Soigner */
                    int_SendG         = wm.WiimoteButtonStatePlus; /* Utiliser objet */
                    int_SendY         = wm.WiimoteButtonStateLeft; /* Objet suivant */
                    int_SendU         = wm.WiimoteButtonStateRight; /* Arme suivante */
                    int_SendX         = wm.WiimoteButtonStateUp; /* Zoomer */
                    int_SendC         = wm.WiimoteButtonStateDown; /* A genoux */
                    int_SendLeftClick = wm.WiimoteButtonStateB; /* Tirer */
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
                    if (int_SendSpace | int_SendLeftShift | int_SendV | int_SendEscape | int_SendTab | int_SendR | int_SendF | int_SendT | int_SendG | int_SendY | int_SendU | int_SendX | int_SendC)
                    {
                        getstate[1] = false;
                    }
                    int_SendRightClick = getstate[1]; /* Viser */
                }
                SI.Set(keyboard_1_id, mouse_1_id, MouseDesktopX, MouseDesktopY, int_deltaX, int_deltaY, int_x, int_y, int_SendLeftClick, int_SendRightClick, int_SendMiddleClick, int_SendWheelUp, int_SendWheelDown, int_SendCANCEL, int_SendBACK, int_SendTAB, int_SendCLEAR, int_SendRETURN, int_SendSHIFT, int_SendCONTROL, int_SendMENU, int_SendCAPITAL, int_SendESCAPE, int_SendSPACE, int_SendPRIOR, int_SendNEXT, int_SendEND, int_SendHOME, int_SendLEFT, int_SendUP, int_SendRIGHT, int_SendDOWN, int_SendSNAPSHOT, int_SendINSERT, int_SendNUMPADDEL, int_SendNUMPADINSERT, int_SendHELP, int_SendAPOSTROPHE, int_SendBACKSPACE, int_SendPAGEDOWN, int_SendPAGEUP, int_SendFIN, int_SendMOUSE, int_SendA, int_SendB, int_SendC, int_SendD, int_SendE, int_SendF, int_SendG, int_SendH, int_SendI, int_SendJ, int_SendK, int_SendL, int_SendM, int_SendN, int_SendO, int_SendP, int_SendQ, int_SendR, int_SendS, int_SendT, int_SendU, int_SendV, int_SendW, int_SendX, int_SendY, int_SendZ, int_SendLWIN, int_SendRWIN, int_SendAPPS, int_SendDELETE, int_SendNUMPAD0, int_SendNUMPAD1, int_SendNUMPAD2, int_SendNUMPAD3, int_SendNUMPAD4, int_SendNUMPAD5, int_SendNUMPAD6, int_SendNUMPAD7, int_SendNUMPAD8, int_SendNUMPAD9, int_SendMULTIPLY, int_SendADD, int_SendSUBTRACT, int_SendDECIMAL, int_SendPRINTSCREEN, int_SendDIVIDE, int_SendF1, int_SendF2, int_SendF3, int_SendF4, int_SendF5, int_SendF6, int_SendF7, int_SendF8, int_SendF9, int_SendF10, int_SendF11, int_SendF12, int_SendNUMLOCK, int_SendSCROLLLOCK, int_SendLEFTSHIFT, int_SendRIGHTSHIFT, int_SendLEFTCONTROL, int_SendRIGHTCONTROL, int_SendLEFTALT, int_SendRIGHTALT, int_SendBROWSER_BACK, int_SendBROWSER_FORWARD, int_SendBROWSER_REFRESH, int_SendBROWSER_STOP, int_SendBROWSER_SEARCH, int_SendBROWSER_FAVORITES, int_SendBROWSER_HOME, int_SendVOLUME_MUTE, int_SendVOLUME_DOWN, int_SendVOLUME_UP, int_SendMEDIA_NEXT_TRACK, int_SendMEDIA_PREV_TRACK, int_SendMEDIA_STOP, int_SendMEDIA_PLAY_PAUSE, int_SendLAUNCH_MAIL, int_SendLAUNCH_MEDIA_SELECT, int_SendLAUNCH_APP1, int_SendLAUNCH_APP2, int_SendOEM_1, int_SendOEM_PLUS, int_SendOEM_COMMA, int_SendOEM_MINUS, int_SendOEM_PERIOD, int_SendOEM_2, int_SendOEM_3, int_SendOEM_4, int_SendOEM_5, int_SendOEM_6, int_SendOEM_7, int_SendOEM_8, int_SendOEM_102, int_SendEREOF, int_SendZOOM, int_SendEscape, int_SendOne, int_SendTwo, int_SendThree, int_SendFour, int_SendFive, int_SendSix, int_SendSeven, int_SendEight, int_SendNine, int_SendZero, int_SendDashUnderscore, int_SendPlusEquals, int_SendBackspace, int_SendTab, int_SendOpenBracketBrace, int_SendCloseBracketBrace, int_SendEnter, int_SendControl, int_SendSemicolonColon, int_SendSingleDoubleQuote, int_SendTilde, int_SendLeftShift, int_SendBackslashPipe, int_SendCommaLeftArrow, int_SendPeriodRightArrow, int_SendForwardSlashQuestionMark, int_SendRightShift, int_SendRightAlt, int_SendSpace, int_SendCapsLock, int_SendUp, int_SendDown, int_SendRight, int_SendLeft, int_SendHome, int_SendEnd, int_SendDelete, int_SendPageUp, int_SendPageDown, int_SendInsert, int_SendPrintScreen, int_SendNumLock, int_SendScrollLock, int_SendMenu, int_SendWindowsKey, int_SendNumpadDivide, int_SendNumpadAsterisk, int_SendNumpad7, int_SendNumpad8, int_SendNumpad9, int_SendNumpad4, int_SendNumpad5, int_SendNumpad6, int_SendNumpad1, int_SendNumpad2, int_SendNumpad3, int_SendNumpad0, int_SendNumpadDelete, int_SendNumpadEnter, int_SendNumpadPlus, int_SendNumpadMinus);
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