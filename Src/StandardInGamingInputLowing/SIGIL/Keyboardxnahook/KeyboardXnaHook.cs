using System;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using Keyboardxnahook;
using Microsoft.Xna.Framework.Input;

namespace KeyboardXnaHookAPI
{
    public class KeyboardXnaHook
    {
        [DllImport("user32.dll")]
        public static extern bool GetAsyncKeyState(Keys vKey);
        [DllImport("winmm.dll", EntryPoint = "timeBeginPeriod")]
        private static extern uint TimeBeginPeriod(uint ms);
        [DllImport("winmm.dll", EntryPoint = "timeEndPeriod")]
        private static extern uint TimeEndPeriod(uint ms);
        [DllImport("ntdll.dll", EntryPoint = "NtSetTimerResolution")]
        private static extern void NtSetTimerResolution(uint DesiredResolution, bool SetResolution, ref uint CurrentResolution);
        private static uint CurrentResolution = 0;
        private bool running, formvisible;
        private int number;
        private Form1 form1 = new Form1();
        public KeyboardXnaHook()
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
        private void taskK()
        {
            for (; ; )
            {
                if (!running)
                    break;
                ProcessStateLogic();
                System.Threading.Thread.Sleep(1);
                if (formvisible)
                {
                    string str = "KeyboardKeyA : " + KeyboardKeyA + Environment.NewLine;
                    str += "KeyboardKeyB : " + KeyboardKeyB + Environment.NewLine;
                    str += "KeyboardKeyC : " + KeyboardKeyC + Environment.NewLine;
                    str += "KeyboardKeyD : " + KeyboardKeyD + Environment.NewLine;
                    str += "KeyboardKeyE : " + KeyboardKeyE + Environment.NewLine;
                    str += "KeyboardKeyF : " + KeyboardKeyF + Environment.NewLine;
                    str += "KeyboardKeyG : " + KeyboardKeyG + Environment.NewLine;
                    str += "KeyboardKeyH : " + KeyboardKeyH + Environment.NewLine;
                    str += "KeyboardKeyI : " + KeyboardKeyI + Environment.NewLine;
                    str += "KeyboardKeyJ : " + KeyboardKeyJ + Environment.NewLine;
                    str += "KeyboardKeyK : " + KeyboardKeyK + Environment.NewLine;
                    str += "KeyboardKeyL : " + KeyboardKeyL + Environment.NewLine;
                    str += "KeyboardKeyM : " + KeyboardKeyM + Environment.NewLine;
                    str += "KeyboardKeyN : " + KeyboardKeyN + Environment.NewLine;
                    str += "KeyboardKeyO : " + KeyboardKeyO + Environment.NewLine;
                    str += "KeyboardKeyP : " + KeyboardKeyP + Environment.NewLine;
                    str += "KeyboardKeyQ : " + KeyboardKeyQ + Environment.NewLine;
                    str += "KeyboardKeyR : " + KeyboardKeyR + Environment.NewLine;
                    str += "KeyboardKeyS : " + KeyboardKeyS + Environment.NewLine;
                    str += "KeyboardKeyT : " + KeyboardKeyT + Environment.NewLine;
                    str += "KeyboardKeyU : " + KeyboardKeyU + Environment.NewLine;
                    str += "KeyboardKeyV : " + KeyboardKeyV + Environment.NewLine;
                    str += "KeyboardKeyW : " + KeyboardKeyW + Environment.NewLine;
                    str += "KeyboardKeyX : " + KeyboardKeyX + Environment.NewLine;
                    str += "KeyboardKeyY : " + KeyboardKeyY + Environment.NewLine;
                    str += "KeyboardKeyZ : " + KeyboardKeyZ + Environment.NewLine;
                    str += "KeyboardKeyEscape : " + KeyboardKeyEscape + Environment.NewLine;
                    str += "KeyboardKeyD1 : " + KeyboardKeyD1 + Environment.NewLine;
                    str += "KeyboardKeyD2 : " + KeyboardKeyD2 + Environment.NewLine;
                    str += "KeyboardKeyD3 : " + KeyboardKeyD3 + Environment.NewLine;
                    str += "KeyboardKeyD4 : " + KeyboardKeyD4 + Environment.NewLine;
                    str += "KeyboardKeyD5 : " + KeyboardKeyD5 + Environment.NewLine;
                    str += "KeyboardKeyD6 : " + KeyboardKeyD6 + Environment.NewLine;
                    str += "KeyboardKeyD7 : " + KeyboardKeyD7 + Environment.NewLine;
                    str += "KeyboardKeyD8 : " + KeyboardKeyD8 + Environment.NewLine;
                    str += "KeyboardKeyD9 : " + KeyboardKeyD9 + Environment.NewLine;
                    str += "KeyboardKeyD0 : " + KeyboardKeyD0 + Environment.NewLine;
                    str += "KeyboardKeyMinus : " + KeyboardKeyMinus + Environment.NewLine;
                    str += "KeyboardKeyEquals : " + KeyboardKeyEquals + Environment.NewLine;
                    str += "KeyboardKeyBack : " + KeyboardKeyBack + Environment.NewLine;
                    str += "KeyboardKeyTab : " + KeyboardKeyTab + Environment.NewLine;
                    str += "KeyboardKeyLeftBracket : " + KeyboardKeyLeftBracket + Environment.NewLine;
                    str += "KeyboardKeyRightBracket : " + KeyboardKeyRightBracket + Environment.NewLine;
                    str += "KeyboardKeyReturn : " + KeyboardKeyReturn + Environment.NewLine;
                    str += "KeyboardKeyLeftControl : " + KeyboardKeyLeftControl + Environment.NewLine;
                    str += "KeyboardKeySemicolon : " + KeyboardKeySemicolon + Environment.NewLine;
                    str += "KeyboardKeyApostrophe : " + KeyboardKeyApostrophe + Environment.NewLine;
                    str += "KeyboardKeyGrave : " + KeyboardKeyGrave + Environment.NewLine;
                    str += "KeyboardKeyLeftShift : " + KeyboardKeyLeftShift + Environment.NewLine;
                    str += "KeyboardKeyBackslash : " + KeyboardKeyBackslash + Environment.NewLine;
                    str += "KeyboardKeyComma : " + KeyboardKeyComma + Environment.NewLine;
                    str += "KeyboardKeyPeriod : " + KeyboardKeyPeriod + Environment.NewLine;
                    str += "KeyboardKeySlash : " + KeyboardKeySlash + Environment.NewLine;
                    str += "KeyboardKeyRightShift : " + KeyboardKeyRightShift + Environment.NewLine;
                    str += "KeyboardKeyMultiply : " + KeyboardKeyMultiply + Environment.NewLine;
                    str += "KeyboardKeyLeftAlt : " + KeyboardKeyLeftAlt + Environment.NewLine;
                    str += "KeyboardKeySpace : " + KeyboardKeySpace + Environment.NewLine;
                    str += "KeyboardKeyCapital : " + KeyboardKeyCapital + Environment.NewLine;
                    str += "KeyboardKeyF1 : " + KeyboardKeyF1 + Environment.NewLine;
                    str += "KeyboardKeyF2 : " + KeyboardKeyF2 + Environment.NewLine;
                    str += "KeyboardKeyF3 : " + KeyboardKeyF3 + Environment.NewLine;
                    str += "KeyboardKeyF4 : " + KeyboardKeyF4 + Environment.NewLine;
                    str += "KeyboardKeyF5 : " + KeyboardKeyF5 + Environment.NewLine;
                    str += "KeyboardKeyF6 : " + KeyboardKeyF6 + Environment.NewLine;
                    str += "KeyboardKeyF7 : " + KeyboardKeyF7 + Environment.NewLine;
                    str += "KeyboardKeyF8 : " + KeyboardKeyF8 + Environment.NewLine;
                    str += "KeyboardKeyF9 : " + KeyboardKeyF9 + Environment.NewLine;
                    str += "KeyboardKeyF10 : " + KeyboardKeyF10 + Environment.NewLine;
                    str += "KeyboardKeyF11 : " + KeyboardKeyF11 + Environment.NewLine;
                    str += "KeyboardKeyF12 : " + KeyboardKeyF12 + Environment.NewLine;
                    str += "KeyboardKeyF13 : " + KeyboardKeyF13 + Environment.NewLine;
                    str += "KeyboardKeyF14 : " + KeyboardKeyF14 + Environment.NewLine;
                    str += "KeyboardKeyF15 : " + KeyboardKeyF15 + Environment.NewLine;
                    str += "KeyboardKeyNumberLock : " + KeyboardKeyNumberLock + Environment.NewLine;
                    str += "KeyboardKeyScrollLock : " + KeyboardKeyScrollLock + Environment.NewLine;
                    str += "KeyboardKeyNumberPad0 : " + KeyboardKeyNumberPad0 + Environment.NewLine;
                    str += "KeyboardKeyNumberPad1 : " + KeyboardKeyNumberPad1 + Environment.NewLine;
                    str += "KeyboardKeyNumberPad2 : " + KeyboardKeyNumberPad2 + Environment.NewLine;
                    str += "KeyboardKeyNumberPad3 : " + KeyboardKeyNumberPad3 + Environment.NewLine;
                    str += "KeyboardKeyNumberPad4 : " + KeyboardKeyNumberPad4 + Environment.NewLine;
                    str += "KeyboardKeyNumberPad5 : " + KeyboardKeyNumberPad5 + Environment.NewLine;
                    str += "KeyboardKeyNumberPad6 : " + KeyboardKeyNumberPad6 + Environment.NewLine;
                    str += "KeyboardKeyNumberPad7 : " + KeyboardKeyNumberPad7 + Environment.NewLine;
                    str += "KeyboardKeyNumberPad8 : " + KeyboardKeyNumberPad8 + Environment.NewLine;
                    str += "KeyboardKeyNumberPad9 : " + KeyboardKeyNumberPad9 + Environment.NewLine;
                    str += "KeyboardKeySubtract : " + KeyboardKeySubtract + Environment.NewLine;
                    str += "KeyboardKeyAdd : " + KeyboardKeyAdd + Environment.NewLine;
                    str += "KeyboardKeyDecimal : " + KeyboardKeyDecimal + Environment.NewLine;
                    str += "KeyboardKeyOem102 : " + KeyboardKeyOem102 + Environment.NewLine;
                    str += "KeyboardKeyKana : " + KeyboardKeyKana + Environment.NewLine;
                    str += "KeyboardKeyAbntC1 : " + KeyboardKeyAbntC1 + Environment.NewLine;
                    str += "KeyboardKeyConvert : " + KeyboardKeyConvert + Environment.NewLine;
                    str += "KeyboardKeyNoConvert : " + KeyboardKeyNoConvert + Environment.NewLine;
                    str += "KeyboardKeyYen : " + KeyboardKeyYen + Environment.NewLine;
                    str += "KeyboardKeyAbntC2 : " + KeyboardKeyAbntC2 + Environment.NewLine;
                    str += "KeyboardKeyNumberPadEquals : " + KeyboardKeyNumberPadEquals + Environment.NewLine;
                    str += "KeyboardKeyPreviousTrack : " + KeyboardKeyPreviousTrack + Environment.NewLine;
                    str += "KeyboardKeyAT : " + KeyboardKeyAT + Environment.NewLine;
                    str += "KeyboardKeyColon : " + KeyboardKeyColon + Environment.NewLine;
                    str += "KeyboardKeyUnderline : " + KeyboardKeyUnderline + Environment.NewLine;
                    str += "KeyboardKeyKanji : " + KeyboardKeyKanji + Environment.NewLine;
                    str += "KeyboardKeyStop : " + KeyboardKeyStop + Environment.NewLine;
                    str += "KeyboardKeyAX : " + KeyboardKeyAX + Environment.NewLine;
                    str += "KeyboardKeyUnlabeled : " + KeyboardKeyUnlabeled + Environment.NewLine;
                    str += "KeyboardKeyNextTrack : " + KeyboardKeyNextTrack + Environment.NewLine;
                    str += "KeyboardKeyNumberPadEnter : " + KeyboardKeyNumberPadEnter + Environment.NewLine;
                    str += "KeyboardKeyRightControl : " + KeyboardKeyRightControl + Environment.NewLine;
                    str += "KeyboardKeyMute : " + KeyboardKeyMute + Environment.NewLine;
                    str += "KeyboardKeyCalculator : " + KeyboardKeyCalculator + Environment.NewLine;
                    str += "KeyboardKeyPlayPause : " + KeyboardKeyPlayPause + Environment.NewLine;
                    str += "KeyboardKeyMediaStop : " + KeyboardKeyMediaStop + Environment.NewLine;
                    str += "KeyboardKeyVolumeDown : " + KeyboardKeyVolumeDown + Environment.NewLine;
                    str += "KeyboardKeyVolumeUp : " + KeyboardKeyVolumeUp + Environment.NewLine;
                    str += "KeyboardKeyWebHome : " + KeyboardKeyWebHome + Environment.NewLine;
                    str += "KeyboardKeyNumberPadComma : " + KeyboardKeyNumberPadComma + Environment.NewLine;
                    str += "KeyboardKeyDivide : " + KeyboardKeyDivide + Environment.NewLine;
                    str += "KeyboardKeyPrintScreen : " + KeyboardKeyPrintScreen + Environment.NewLine;
                    str += "KeyboardKeyRightAlt : " + KeyboardKeyRightAlt + Environment.NewLine;
                    str += "KeyboardKeyPause : " + KeyboardKeyPause + Environment.NewLine;
                    str += "KeyboardKeyHome : " + KeyboardKeyHome + Environment.NewLine;
                    str += "KeyboardKeyUp : " + KeyboardKeyUp + Environment.NewLine;
                    str += "KeyboardKeyPageUp : " + KeyboardKeyPageUp + Environment.NewLine;
                    str += "KeyboardKeyLeft : " + KeyboardKeyLeft + Environment.NewLine;
                    str += "KeyboardKeyRight : " + KeyboardKeyRight + Environment.NewLine;
                    str += "KeyboardKeyEnd : " + KeyboardKeyEnd + Environment.NewLine;
                    str += "KeyboardKeyDown : " + KeyboardKeyDown + Environment.NewLine;
                    str += "KeyboardKeyPageDown : " + KeyboardKeyPageDown + Environment.NewLine;
                    str += "KeyboardKeyInsert : " + KeyboardKeyInsert + Environment.NewLine;
                    str += "KeyboardKeyDelete : " + KeyboardKeyDelete + Environment.NewLine;
                    str += "KeyboardKeyLeftWindowsKey : " + KeyboardKeyLeftWindowsKey + Environment.NewLine;
                    str += "KeyboardKeyRightWindowsKey : " + KeyboardKeyRightWindowsKey + Environment.NewLine;
                    str += "KeyboardKeyApplications : " + KeyboardKeyApplications + Environment.NewLine;
                    str += "KeyboardKeyPower : " + KeyboardKeyPower + Environment.NewLine;
                    str += "KeyboardKeySleep : " + KeyboardKeySleep + Environment.NewLine;
                    str += "KeyboardKeyWake : " + KeyboardKeyWake + Environment.NewLine;
                    str += "KeyboardKeyWebSearch : " + KeyboardKeyWebSearch + Environment.NewLine;
                    str += "KeyboardKeyWebFavorites : " + KeyboardKeyWebFavorites + Environment.NewLine;
                    str += "KeyboardKeyWebRefresh : " + KeyboardKeyWebRefresh + Environment.NewLine;
                    str += "KeyboardKeyWebStop : " + KeyboardKeyWebStop + Environment.NewLine;
                    str += "KeyboardKeyWebForward : " + KeyboardKeyWebForward + Environment.NewLine;
                    str += "KeyboardKeyWebBack : " + KeyboardKeyWebBack + Environment.NewLine;
                    str += "KeyboardKeyMyComputer : " + KeyboardKeyMyComputer + Environment.NewLine;
                    str += "KeyboardKeyMail : " + KeyboardKeyMail + Environment.NewLine;
                    str += "KeyboardKeyMediaSelect : " + KeyboardKeyMediaSelect + Environment.NewLine;
                    str += "KeyboardKeyUnknown : " + KeyboardKeyUnknown + Environment.NewLine;
                    str += Environment.NewLine;
                    form1.SetLabel1(str);
                }
            }
        }
        public void BeginPolling()
        {
            Task.Run(() => taskK());
        }
        private Keys[] keys;
        public bool KeyboardKeyEscape;
        public bool KeyboardKeyD1;
        public bool KeyboardKeyD2;
        public bool KeyboardKeyD3;
        public bool KeyboardKeyD4;
        public bool KeyboardKeyD5;
        public bool KeyboardKeyD6;
        public bool KeyboardKeyD7;
        public bool KeyboardKeyD8;
        public bool KeyboardKeyD9;
        public bool KeyboardKeyD0;
        public bool KeyboardKeyMinus;
        public bool KeyboardKeyEquals;
        public bool KeyboardKeyBack;
        public bool KeyboardKeyTab;
        public bool KeyboardKeyQ;
        public bool KeyboardKeyW;
        public bool KeyboardKeyE;
        public bool KeyboardKeyR;
        public bool KeyboardKeyT;
        public bool KeyboardKeyY;
        public bool KeyboardKeyU;
        public bool KeyboardKeyI;
        public bool KeyboardKeyO;
        public bool KeyboardKeyP;
        public bool KeyboardKeyLeftBracket;
        public bool KeyboardKeyRightBracket;
        public bool KeyboardKeyReturn;
        public bool KeyboardKeyLeftControl;
        public bool KeyboardKeyA;
        public bool KeyboardKeyS;
        public bool KeyboardKeyD;
        public bool KeyboardKeyF;
        public bool KeyboardKeyG;
        public bool KeyboardKeyH;
        public bool KeyboardKeyJ;
        public bool KeyboardKeyK;
        public bool KeyboardKeyL;
        public bool KeyboardKeySemicolon;
        public bool KeyboardKeyApostrophe;
        public bool KeyboardKeyGrave;
        public bool KeyboardKeyLeftShift;
        public bool KeyboardKeyBackslash;
        public bool KeyboardKeyZ;
        public bool KeyboardKeyX;
        public bool KeyboardKeyC;
        public bool KeyboardKeyV;
        public bool KeyboardKeyB;
        public bool KeyboardKeyN;
        public bool KeyboardKeyM;
        public bool KeyboardKeyComma;
        public bool KeyboardKeyPeriod;
        public bool KeyboardKeySlash;
        public bool KeyboardKeyRightShift;
        public bool KeyboardKeyMultiply;
        public bool KeyboardKeyLeftAlt;
        public bool KeyboardKeySpace;
        public bool KeyboardKeyCapital;
        public bool KeyboardKeyF1;
        public bool KeyboardKeyF2;
        public bool KeyboardKeyF3;
        public bool KeyboardKeyF4;
        public bool KeyboardKeyF5;
        public bool KeyboardKeyF6;
        public bool KeyboardKeyF7;
        public bool KeyboardKeyF8;
        public bool KeyboardKeyF9;
        public bool KeyboardKeyF10;
        public bool KeyboardKeyNumberLock;
        public bool KeyboardKeyScrollLock;
        public bool KeyboardKeyNumberPad7;
        public bool KeyboardKeyNumberPad8;
        public bool KeyboardKeyNumberPad9;
        public bool KeyboardKeySubtract;
        public bool KeyboardKeyNumberPad4;
        public bool KeyboardKeyNumberPad5;
        public bool KeyboardKeyNumberPad6;
        public bool KeyboardKeyAdd;
        public bool KeyboardKeyNumberPad1;
        public bool KeyboardKeyNumberPad2;
        public bool KeyboardKeyNumberPad3;
        public bool KeyboardKeyNumberPad0;
        public bool KeyboardKeyDecimal;
        public bool KeyboardKeyOem102;
        public bool KeyboardKeyF11;
        public bool KeyboardKeyF12;
        public bool KeyboardKeyF13;
        public bool KeyboardKeyF14;
        public bool KeyboardKeyF15;
        public bool KeyboardKeyKana;
        public bool KeyboardKeyAbntC1;
        public bool KeyboardKeyConvert;
        public bool KeyboardKeyNoConvert;
        public bool KeyboardKeyYen;
        public bool KeyboardKeyAbntC2;
        public bool KeyboardKeyNumberPadEquals;
        public bool KeyboardKeyPreviousTrack;
        public bool KeyboardKeyAT;
        public bool KeyboardKeyColon;
        public bool KeyboardKeyUnderline;
        public bool KeyboardKeyKanji;
        public bool KeyboardKeyStop;
        public bool KeyboardKeyAX;
        public bool KeyboardKeyUnlabeled;
        public bool KeyboardKeyNextTrack;
        public bool KeyboardKeyNumberPadEnter;
        public bool KeyboardKeyRightControl;
        public bool KeyboardKeyMute;
        public bool KeyboardKeyCalculator;
        public bool KeyboardKeyPlayPause;
        public bool KeyboardKeyMediaStop;
        public bool KeyboardKeyVolumeDown;
        public bool KeyboardKeyVolumeUp;
        public bool KeyboardKeyWebHome;
        public bool KeyboardKeyNumberPadComma;
        public bool KeyboardKeyDivide;
        public bool KeyboardKeyPrintScreen;
        public bool KeyboardKeyRightAlt;
        public bool KeyboardKeyPause;
        public bool KeyboardKeyHome;
        public bool KeyboardKeyUp;
        public bool KeyboardKeyPageUp;
        public bool KeyboardKeyLeft;
        public bool KeyboardKeyRight;
        public bool KeyboardKeyEnd;
        public bool KeyboardKeyDown;
        public bool KeyboardKeyPageDown;
        public bool KeyboardKeyInsert;
        public bool KeyboardKeyDelete;
        public bool KeyboardKeyLeftWindowsKey;
        public bool KeyboardKeyRightWindowsKey;
        public bool KeyboardKeyApplications;
        public bool KeyboardKeyPower;
        public bool KeyboardKeySleep;
        public bool KeyboardKeyWake;
        public bool KeyboardKeyWebSearch;
        public bool KeyboardKeyWebFavorites;
        public bool KeyboardKeyWebRefresh;
        public bool KeyboardKeyWebStop;
        public bool KeyboardKeyWebForward;
        public bool KeyboardKeyWebBack;
        public bool KeyboardKeyMyComputer;
        public bool KeyboardKeyMail;
        public bool KeyboardKeyMediaSelect;
        public bool KeyboardKeyUnknown;
        public bool Scan(int number = 0)
        {
            this.number = number;
            return true;
        }
        public void Init()
        {
        }
        private void ProcessStateLogic()
        {
            if (GetAsyncKeyState(Keys.Escape))
                KeyboardKeyEscape = true;
            else
                KeyboardKeyEscape = false;
            if (GetAsyncKeyState(Keys.D1))
                KeyboardKeyD1 = true;
            else
                KeyboardKeyD1 = false;
            if (GetAsyncKeyState(Keys.D2))
                KeyboardKeyD2 = true;
            else
                KeyboardKeyD2 = false;
            if (GetAsyncKeyState(Keys.D3))
                KeyboardKeyD3 = true;
            else
                KeyboardKeyD3 = false;
            if (GetAsyncKeyState(Keys.D4))
                KeyboardKeyD4 = true;
            else
                KeyboardKeyD4 = false;
            if (GetAsyncKeyState(Keys.D5))
                KeyboardKeyD5 = true;
            else
                KeyboardKeyD5 = false;
            if (GetAsyncKeyState(Keys.D6))
                KeyboardKeyD6 = true;
            else
                KeyboardKeyD6 = false;
            if (GetAsyncKeyState(Keys.D7))
                KeyboardKeyD7 = true;
            else
                KeyboardKeyD7 = false;
            if (GetAsyncKeyState(Keys.D8))
                KeyboardKeyD8 = true;
            else
                KeyboardKeyD8 = false;
            if (GetAsyncKeyState(Keys.D9))
                KeyboardKeyD9 = true;
            else
                KeyboardKeyD9 = false;
            if (GetAsyncKeyState(Keys.D0))
                KeyboardKeyD0 = true;
            else
                KeyboardKeyD0 = false;
            if (GetAsyncKeyState(Keys.OemMinus))
                KeyboardKeyMinus = true;
            else
                KeyboardKeyMinus = false;
            if (GetAsyncKeyState(Keys.OemCloseBrackets))
                KeyboardKeyEquals = true;
            else
                KeyboardKeyEquals = false;
            if (GetAsyncKeyState(Keys.Back))
                KeyboardKeyBack = true;
            else
                KeyboardKeyBack = false;
            if (GetAsyncKeyState(Keys.Tab))
                KeyboardKeyTab = true;
            else
                KeyboardKeyTab = false;
            if (GetAsyncKeyState(Keys.Q))
                KeyboardKeyQ = true;
            else
                KeyboardKeyQ = false;
            if (GetAsyncKeyState(Keys.W))
                KeyboardKeyW = true;
            else
                KeyboardKeyW = false;
            if (GetAsyncKeyState(Keys.E))
                KeyboardKeyE = true;
            else
                KeyboardKeyE = false;
            if (GetAsyncKeyState(Keys.R))
                KeyboardKeyR = true;
            else
                KeyboardKeyR = false;
            if (GetAsyncKeyState(Keys.T))
                KeyboardKeyT = true;
            else
                KeyboardKeyT = false;
            if (GetAsyncKeyState(Keys.Y))
                KeyboardKeyY = true;
            else
                KeyboardKeyY = false;
            if (GetAsyncKeyState(Keys.U))
                KeyboardKeyU = true;
            else
                KeyboardKeyU = false;
            if (GetAsyncKeyState(Keys.I))
                KeyboardKeyI = true;
            else
                KeyboardKeyI = false;
            if (GetAsyncKeyState(Keys.O))
                KeyboardKeyO = true;
            else
                KeyboardKeyO = false;
            if (GetAsyncKeyState(Keys.P))
                KeyboardKeyP = true;
            else
                KeyboardKeyP = false;
            if (GetAsyncKeyState(Keys.OemOpenBrackets))
                KeyboardKeyLeftBracket = true;
            else
                KeyboardKeyLeftBracket = false;
            if (GetAsyncKeyState(Keys.OemCloseBrackets))
                KeyboardKeyRightBracket = true;
            else
                KeyboardKeyRightBracket = false;
            if (GetAsyncKeyState(Keys.Enter))
                KeyboardKeyReturn = true;
            else
                KeyboardKeyReturn = false;
            if (GetAsyncKeyState(Keys.LeftControl))
                KeyboardKeyLeftControl = true;
            else
                KeyboardKeyLeftControl = false;
            if (GetAsyncKeyState(Keys.A))
                KeyboardKeyA = true;
            else
                KeyboardKeyA = false;
            if (GetAsyncKeyState(Keys.S))
                KeyboardKeyS = true;
            else
                KeyboardKeyS = false;
            if (GetAsyncKeyState(Keys.D))
                KeyboardKeyD = true;
            else
                KeyboardKeyD = false;
            if (GetAsyncKeyState(Keys.F))
                KeyboardKeyF = true;
            else
                KeyboardKeyF = false;
            if (GetAsyncKeyState(Keys.G))
                KeyboardKeyG = true;
            else
                KeyboardKeyG = false;
            if (GetAsyncKeyState(Keys.H))
                KeyboardKeyH = true;
            else
                KeyboardKeyH = false;
            if (GetAsyncKeyState(Keys.J))
                KeyboardKeyJ = true;
            else
                KeyboardKeyJ = false;
            if (GetAsyncKeyState(Keys.K))
                KeyboardKeyK = true;
            else
                KeyboardKeyK = false;
            if (GetAsyncKeyState(Keys.L))
                KeyboardKeyL = true;
            else
                KeyboardKeyL = false;
            if (GetAsyncKeyState(Keys.OemSemicolon))
                KeyboardKeySemicolon = true;
            else
                KeyboardKeySemicolon = false;
            if (GetAsyncKeyState(Keys.Apps))
                KeyboardKeyApostrophe = true;
            else
                KeyboardKeyApostrophe = false;
            if (GetAsyncKeyState(Keys.ChatPadGreen))
                KeyboardKeyGrave = true;
            else
                KeyboardKeyGrave = false;
            if (GetAsyncKeyState(Keys.LeftShift))
                KeyboardKeyLeftShift = true;
            else
                KeyboardKeyLeftShift = false;
            if (GetAsyncKeyState(Keys.OemBackslash))
                KeyboardKeyBackslash = true;
            else
                KeyboardKeyBackslash = false;
            if (GetAsyncKeyState(Keys.Z))
                KeyboardKeyZ = true;
            else
                KeyboardKeyZ = false;
            if (GetAsyncKeyState(Keys.X))
                KeyboardKeyX = true;
            else
                KeyboardKeyX = false;
            if (GetAsyncKeyState(Keys.C))
                KeyboardKeyC = true;
            else
                KeyboardKeyC = false;
            if (GetAsyncKeyState(Keys.V))
                KeyboardKeyV = true;
            else
                KeyboardKeyV = false;
            if (GetAsyncKeyState(Keys.B))
                KeyboardKeyB = true;
            else
                KeyboardKeyB = false;
            if (GetAsyncKeyState(Keys.N))
                KeyboardKeyN = true;
            else
                KeyboardKeyN = false;
            if (GetAsyncKeyState(Keys.M))
                KeyboardKeyM = true;
            else
                KeyboardKeyM = false;
            if (GetAsyncKeyState(Keys.OemComma))
                KeyboardKeyComma = true;
            else
                KeyboardKeyComma = false;
            if (GetAsyncKeyState(Keys.OemPeriod))
                KeyboardKeyPeriod = true;
            else
                KeyboardKeyPeriod = false;
            if (GetAsyncKeyState(Keys.SelectMedia))
                KeyboardKeySlash = true;
            else
                KeyboardKeySlash = false;
            if (GetAsyncKeyState(Keys.RightShift))
                KeyboardKeyRightShift = true;
            else
                KeyboardKeyRightShift = false;
            if (GetAsyncKeyState(Keys.Multiply))
                KeyboardKeyMultiply = true;
            else
                KeyboardKeyMultiply = false;
            if (GetAsyncKeyState(Keys.LeftAlt))
                KeyboardKeyLeftAlt = true;
            else
                KeyboardKeyLeftAlt = false;
            if (GetAsyncKeyState(Keys.Space))
                KeyboardKeySpace = true;
            else
                KeyboardKeySpace = false;
            if (GetAsyncKeyState(Keys.CapsLock))
                KeyboardKeyCapital = true;
            else
                KeyboardKeyCapital = false;
            if (GetAsyncKeyState(Keys.F1))
                KeyboardKeyF1 = true;
            else
                KeyboardKeyF1 = false;
            if (GetAsyncKeyState(Keys.F2))
                KeyboardKeyF2 = true;
            else
                KeyboardKeyF2 = false;
            if (GetAsyncKeyState(Keys.F3))
                KeyboardKeyF3 = true;
            else
                KeyboardKeyF3 = false;
            if (GetAsyncKeyState(Keys.F4))
                KeyboardKeyF4 = true;
            else
                KeyboardKeyF4 = false;
            if (GetAsyncKeyState(Keys.F5))
                KeyboardKeyF5 = true;
            else
                KeyboardKeyF5 = false;
            if (GetAsyncKeyState(Keys.F6))
                KeyboardKeyF6 = true;
            else
                KeyboardKeyF6 = false;
            if (GetAsyncKeyState(Keys.F7))
                KeyboardKeyF7 = true;
            else
                KeyboardKeyF7 = false;
            if (GetAsyncKeyState(Keys.F8))
                KeyboardKeyF8 = true;
            else
                KeyboardKeyF8 = false;
            if (GetAsyncKeyState(Keys.F9))
                KeyboardKeyF9 = true;
            else
                KeyboardKeyF9 = false;
            if (GetAsyncKeyState(Keys.F10))
                KeyboardKeyF10 = true;
            else
                KeyboardKeyF10 = false;
            if (GetAsyncKeyState(Keys.NumLock))
                KeyboardKeyNumberLock = true;
            else
                KeyboardKeyNumberLock = false;
            if (GetAsyncKeyState(Keys.Scroll))
                KeyboardKeyScrollLock = true;
            else
                KeyboardKeyScrollLock = false;
            if (GetAsyncKeyState(Keys.NumPad7))
                KeyboardKeyNumberPad7 = true;
            else
                KeyboardKeyNumberPad7 = false;
            if (GetAsyncKeyState(Keys.NumPad8))
                KeyboardKeyNumberPad8 = true;
            else
                KeyboardKeyNumberPad8 = false;
            if (GetAsyncKeyState(Keys.NumPad9))
                KeyboardKeyNumberPad9 = true;
            else
                KeyboardKeyNumberPad9 = false;
            if (GetAsyncKeyState(Keys.Subtract))
                KeyboardKeySubtract = true;
            else
                KeyboardKeySubtract = false;
            if (GetAsyncKeyState(Keys.NumPad4))
                KeyboardKeyNumberPad4 = true;
            else
                KeyboardKeyNumberPad4 = false;
            if (GetAsyncKeyState(Keys.NumPad5))
                KeyboardKeyNumberPad5 = true;
            else
                KeyboardKeyNumberPad5 = false;
            if (GetAsyncKeyState(Keys.NumPad6))
                KeyboardKeyNumberPad6 = true;
            else
                KeyboardKeyNumberPad6 = false;
            if (GetAsyncKeyState(Keys.Add))
                KeyboardKeyAdd = true;
            else
                KeyboardKeyAdd = false;
            if (GetAsyncKeyState(Keys.NumPad1))
                KeyboardKeyNumberPad1 = true;
            else
                KeyboardKeyNumberPad1 = false;
            if (GetAsyncKeyState(Keys.NumPad2))
                KeyboardKeyNumberPad2 = true;
            else
                KeyboardKeyNumberPad2 = false;
            if (GetAsyncKeyState(Keys.NumPad3))
                KeyboardKeyNumberPad3 = true;
            else
                KeyboardKeyNumberPad3 = false;
            if (GetAsyncKeyState(Keys.NumPad0))
                KeyboardKeyNumberPad0 = true;
            else
                KeyboardKeyNumberPad0 = false;
            if (GetAsyncKeyState(Keys.Decimal))
                KeyboardKeyDecimal = true;
            else
                KeyboardKeyDecimal = false;
            if (GetAsyncKeyState(Keys.OemPipe))
                KeyboardKeyOem102 = true;
            else
                KeyboardKeyOem102 = false;
            if (GetAsyncKeyState(Keys.F11))
                KeyboardKeyF11 = true;
            else
                KeyboardKeyF11 = false;
            if (GetAsyncKeyState(Keys.F12))
                KeyboardKeyF12 = true;
            else
                KeyboardKeyF12 = false;
            if (GetAsyncKeyState(Keys.F13))
                KeyboardKeyF13 = true;
            else
                KeyboardKeyF13 = false;
            if (GetAsyncKeyState(Keys.F14))
                KeyboardKeyF14 = true;
            else
                KeyboardKeyF14 = false;
            if (GetAsyncKeyState(Keys.F15))
                KeyboardKeyF15 = true;
            else
                KeyboardKeyF15 = false;
            if (GetAsyncKeyState(Keys.Kana))
                KeyboardKeyKana = true;
            else
                KeyboardKeyKana = false;
            if (GetAsyncKeyState(Keys.OemAuto))
                KeyboardKeyAbntC1 = true;
            else
                KeyboardKeyAbntC1 = false;
            if (GetAsyncKeyState(Keys.ImeConvert))
                KeyboardKeyConvert = true;
            else
                KeyboardKeyConvert = false;
            if (GetAsyncKeyState(Keys.ImeNoConvert))
                KeyboardKeyNoConvert = true;
            else
                KeyboardKeyNoConvert = false;
            if (GetAsyncKeyState(Keys.Kana))
                KeyboardKeyYen = true;
            else
                KeyboardKeyYen = false;
            if (GetAsyncKeyState(Keys.OemEnlW))
                KeyboardKeyAbntC2 = true;
            else
                KeyboardKeyAbntC2 = false;
            if (GetAsyncKeyState(Keys.BrowserStop))
                KeyboardKeyNumberPadEquals = true;
            else
                KeyboardKeyNumberPadEquals = false;
            if (GetAsyncKeyState(Keys.MediaPreviousTrack))
                KeyboardKeyPreviousTrack = true;
            else
                KeyboardKeyPreviousTrack = false;
            if (GetAsyncKeyState(Keys.Attn))
                KeyboardKeyAT = true;
            else
                KeyboardKeyAT = false;
            if (GetAsyncKeyState(Keys.OemSemicolon))
                KeyboardKeyColon = true;
            else
                KeyboardKeyColon = false;
            if (GetAsyncKeyState(Keys.Pa1))
                KeyboardKeyUnderline = true;
            else
                KeyboardKeyUnderline = false;
            if (GetAsyncKeyState(Keys.Kanji))
                KeyboardKeyKanji = true;
            else
                KeyboardKeyKanji = false;
            if (GetAsyncKeyState(Keys.MediaStop))
                KeyboardKeyStop = true;
            else
                KeyboardKeyStop = false;
            if (GetAsyncKeyState(Keys.Pause))
                KeyboardKeyAX = true;
            else
                KeyboardKeyAX = false;
            if (GetAsyncKeyState(Keys.Add))
                KeyboardKeyUnlabeled = true;
            else
                KeyboardKeyUnlabeled = false;
            if (GetAsyncKeyState(Keys.MediaNextTrack))
                KeyboardKeyNextTrack = true;
            else
                KeyboardKeyNextTrack = false;
            if (GetAsyncKeyState(Keys.End))
                KeyboardKeyNumberPadEnter = true;
            else
                KeyboardKeyNumberPadEnter = false;
            if (GetAsyncKeyState(Keys.RightControl))
                KeyboardKeyRightControl = true;
            else
                KeyboardKeyRightControl = false;
            if (GetAsyncKeyState(Keys.VolumeMute))
                KeyboardKeyMute = true;
            else
                KeyboardKeyMute = false;
            if (GetAsyncKeyState(Keys.CapsLock))
                KeyboardKeyCalculator = true;
            else
                KeyboardKeyCalculator = false;
            if (GetAsyncKeyState(Keys.MediaPlayPause))
                KeyboardKeyPlayPause = true;
            else
                KeyboardKeyPlayPause = false;
            if (GetAsyncKeyState(Keys.MediaStop))
                KeyboardKeyMediaStop = true;
            else
                KeyboardKeyMediaStop = false;
            if (GetAsyncKeyState(Keys.VolumeDown))
                KeyboardKeyVolumeDown = true;
            else
                KeyboardKeyVolumeDown = false;
            if (GetAsyncKeyState(Keys.VolumeUp))
                KeyboardKeyVolumeUp = true;
            else
                KeyboardKeyVolumeUp = false;
            if (GetAsyncKeyState(Keys.BrowserHome))
                KeyboardKeyWebHome = true;
            else
                KeyboardKeyWebHome = false;
            if (GetAsyncKeyState(Keys.OemComma))
                KeyboardKeyNumberPadComma = true;
            else
                KeyboardKeyNumberPadComma = false;
            if (GetAsyncKeyState(Keys.Divide))
                KeyboardKeyDivide = true;
            else
                KeyboardKeyDivide = false;
            if (GetAsyncKeyState(Keys.PrintScreen))
                KeyboardKeyPrintScreen = true;
            else
                KeyboardKeyPrintScreen = false;
            if (GetAsyncKeyState(Keys.RightAlt))
                KeyboardKeyRightAlt = true;
            else
                KeyboardKeyRightAlt = false;
            if (GetAsyncKeyState(Keys.Pause))
                KeyboardKeyPause = true;
            else
                KeyboardKeyPause = false;
            if (GetAsyncKeyState(Keys.Home))
                KeyboardKeyHome = true;
            else
                KeyboardKeyHome = false;
            if (GetAsyncKeyState(Keys.Up))
                KeyboardKeyUp = true;
            else
                KeyboardKeyUp = false;
            if (GetAsyncKeyState(Keys.PageUp))
                KeyboardKeyPageUp = true;
            else
                KeyboardKeyPageUp = false;
            if (GetAsyncKeyState(Keys.Left))
                KeyboardKeyLeft = true;
            else
                KeyboardKeyLeft = false;
            if (GetAsyncKeyState(Keys.Right))
                KeyboardKeyRight = true;
            else
                KeyboardKeyRight = false;
            if (GetAsyncKeyState(Keys.End))
                KeyboardKeyEnd = true;
            else
                KeyboardKeyEnd = false;
            if (GetAsyncKeyState(Keys.Down))
                KeyboardKeyDown = true;
            else
                KeyboardKeyDown = false;
            if (GetAsyncKeyState(Keys.PageDown))
                KeyboardKeyPageDown = true;
            else
                KeyboardKeyPageDown = false;
            if (GetAsyncKeyState(Keys.Insert))
                KeyboardKeyInsert = true;
            else
                KeyboardKeyInsert = false;
            if (GetAsyncKeyState(Keys.Delete))
                KeyboardKeyDelete = true;
            else
                KeyboardKeyDelete = false;
            if (GetAsyncKeyState(Keys.LeftWindows))
                KeyboardKeyLeftWindowsKey = true;
            else
                KeyboardKeyLeftWindowsKey = false;
            if (GetAsyncKeyState(Keys.RightWindows))
                KeyboardKeyRightWindowsKey = true;
            else
                KeyboardKeyRightWindowsKey = false;
            if (GetAsyncKeyState(Keys.Apps))
                KeyboardKeyApplications = true;
            else
                KeyboardKeyApplications = false;
            if (GetAsyncKeyState(Keys.ChatPadOrange))
                KeyboardKeyPower = true;
            else
                KeyboardKeyPower = false;
            if (GetAsyncKeyState(Keys.Sleep))
                KeyboardKeySleep = true;
            else
                KeyboardKeySleep = false;
            if (GetAsyncKeyState(Keys.Sleep))
                KeyboardKeyWake = true;
            else
                KeyboardKeyWake = false;
            if (GetAsyncKeyState(Keys.BrowserSearch))
                KeyboardKeyWebSearch = true;
            else
                KeyboardKeyWebSearch = false;
            if (GetAsyncKeyState(Keys.BrowserFavorites))
                KeyboardKeyWebFavorites = true;
            else
                KeyboardKeyWebFavorites = false;
            if (GetAsyncKeyState(Keys.BrowserRefresh))
                KeyboardKeyWebRefresh = true;
            else
                KeyboardKeyWebRefresh = false;
            if (GetAsyncKeyState(Keys.BrowserStop))
                KeyboardKeyWebStop = true;
            else
                KeyboardKeyWebStop = false;
            if (GetAsyncKeyState(Keys.BrowserForward))
                KeyboardKeyWebForward = true;
            else
                KeyboardKeyWebForward = false;
            if (GetAsyncKeyState(Keys.BrowserBack))
                KeyboardKeyWebBack = true;
            else
                KeyboardKeyWebBack = false;
            if (GetAsyncKeyState(Keys.Attn))
                KeyboardKeyMyComputer = true;
            else
                KeyboardKeyMyComputer = false;
            if (GetAsyncKeyState(Keys.LaunchMail))
                KeyboardKeyMail = true;
            else
                KeyboardKeyMail = false;
            if (GetAsyncKeyState(Keys.Select))
                KeyboardKeyMediaSelect = true;
            else
                KeyboardKeyMediaSelect = false;
            if (GetAsyncKeyState(Keys.None))
                KeyboardKeyUnknown = true;
            else
                KeyboardKeyUnknown = false;
        }
    }
}