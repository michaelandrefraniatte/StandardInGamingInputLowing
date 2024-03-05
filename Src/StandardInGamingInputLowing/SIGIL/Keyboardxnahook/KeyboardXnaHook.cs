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
            keys = Keyboard.GetState().GetPressedKeys();
            if (keys.Contains(Microsoft.Xna.Framework.Input.Keys.Escape))
                KeyboardKeyEscape = true;
            else
                KeyboardKeyEscape = false;
            if (keys.Contains(Microsoft.Xna.Framework.Input.Keys.D1))
                KeyboardKeyD1 = true;
            else
                KeyboardKeyD1 = false;
            if (keys.Contains(Microsoft.Xna.Framework.Input.Keys.D2))
                KeyboardKeyD2 = true;
            else
                KeyboardKeyD2 = false;
            if (keys.Contains(Microsoft.Xna.Framework.Input.Keys.D3))
                KeyboardKeyD3 = true;
            else
                KeyboardKeyD3 = false;
            if (keys.Contains(Microsoft.Xna.Framework.Input.Keys.D4))
                KeyboardKeyD4 = true;
            else
                KeyboardKeyD4 = false;
            if (keys.Contains(Microsoft.Xna.Framework.Input.Keys.D5))
                KeyboardKeyD5 = true;
            else
                KeyboardKeyD5 = false;
            if (keys.Contains(Microsoft.Xna.Framework.Input.Keys.D6))
                KeyboardKeyD6 = true;
            else
                KeyboardKeyD6 = false;
            if (keys.Contains(Microsoft.Xna.Framework.Input.Keys.D7))
                KeyboardKeyD7 = true;
            else
                KeyboardKeyD7 = false;
            if (keys.Contains(Microsoft.Xna.Framework.Input.Keys.D8))
                KeyboardKeyD8 = true;
            else
                KeyboardKeyD8 = false;
            if (keys.Contains(Microsoft.Xna.Framework.Input.Keys.D9))
                KeyboardKeyD9 = true;
            else
                KeyboardKeyD9 = false;
            if (keys.Contains(Microsoft.Xna.Framework.Input.Keys.D0))
                KeyboardKeyD0 = true;
            else
                KeyboardKeyD0 = false;
            if (keys.Contains(Microsoft.Xna.Framework.Input.Keys.OemMinus))
                KeyboardKeyMinus = true;
            else
                KeyboardKeyMinus = false;
            if (keys.Contains(Microsoft.Xna.Framework.Input.Keys.OemCloseBrackets))
                KeyboardKeyEquals = true;
            else
                KeyboardKeyEquals = false;
            if (keys.Contains(Microsoft.Xna.Framework.Input.Keys.Back))
                KeyboardKeyBack = true;
            else
                KeyboardKeyBack = false;
            if (keys.Contains(Microsoft.Xna.Framework.Input.Keys.Tab))
                KeyboardKeyTab = true;
            else
                KeyboardKeyTab = false;
            if (keys.Contains(Microsoft.Xna.Framework.Input.Keys.Q))
                KeyboardKeyQ = true;
            else
                KeyboardKeyQ = false;
            if (keys.Contains(Microsoft.Xna.Framework.Input.Keys.W))
                KeyboardKeyW = true;
            else
                KeyboardKeyW = false;
            if (keys.Contains(Microsoft.Xna.Framework.Input.Keys.E))
                KeyboardKeyE = true;
            else
                KeyboardKeyE = false;
            if (keys.Contains(Microsoft.Xna.Framework.Input.Keys.R))
                KeyboardKeyR = true;
            else
                KeyboardKeyR = false;
            if (keys.Contains(Microsoft.Xna.Framework.Input.Keys.T))
                KeyboardKeyT = true;
            else
                KeyboardKeyT = false;
            if (keys.Contains(Microsoft.Xna.Framework.Input.Keys.Y))
                KeyboardKeyY = true;
            else
                KeyboardKeyY = false;
            if (keys.Contains(Microsoft.Xna.Framework.Input.Keys.U))
                KeyboardKeyU = true;
            else
                KeyboardKeyU = false;
            if (keys.Contains(Microsoft.Xna.Framework.Input.Keys.I))
                KeyboardKeyI = true;
            else
                KeyboardKeyI = false;
            if (keys.Contains(Microsoft.Xna.Framework.Input.Keys.O))
                KeyboardKeyO = true;
            else
                KeyboardKeyO = false;
            if (keys.Contains(Microsoft.Xna.Framework.Input.Keys.P))
                KeyboardKeyP = true;
            else
                KeyboardKeyP = false;
            if (keys.Contains(Microsoft.Xna.Framework.Input.Keys.OemOpenBrackets))
                KeyboardKeyLeftBracket = true;
            else
                KeyboardKeyLeftBracket = false;
            if (keys.Contains(Microsoft.Xna.Framework.Input.Keys.OemCloseBrackets))
                KeyboardKeyRightBracket = true;
            else
                KeyboardKeyRightBracket = false;
            if (keys.Contains(Microsoft.Xna.Framework.Input.Keys.Enter))
                KeyboardKeyReturn = true;
            else
                KeyboardKeyReturn = false;
            if (keys.Contains(Microsoft.Xna.Framework.Input.Keys.LeftControl))
                KeyboardKeyLeftControl = true;
            else
                KeyboardKeyLeftControl = false;
            if (keys.Contains(Microsoft.Xna.Framework.Input.Keys.A))
                KeyboardKeyA = true;
            else
                KeyboardKeyA = false;
            if (keys.Contains(Microsoft.Xna.Framework.Input.Keys.S))
                KeyboardKeyS = true;
            else
                KeyboardKeyS = false;
            if (keys.Contains(Microsoft.Xna.Framework.Input.Keys.D))
                KeyboardKeyD = true;
            else
                KeyboardKeyD = false;
            if (keys.Contains(Microsoft.Xna.Framework.Input.Keys.F))
                KeyboardKeyF = true;
            else
                KeyboardKeyF = false;
            if (keys.Contains(Microsoft.Xna.Framework.Input.Keys.G))
                KeyboardKeyG = true;
            else
                KeyboardKeyG = false;
            if (keys.Contains(Microsoft.Xna.Framework.Input.Keys.H))
                KeyboardKeyH = true;
            else
                KeyboardKeyH = false;
            if (keys.Contains(Microsoft.Xna.Framework.Input.Keys.J))
                KeyboardKeyJ = true;
            else
                KeyboardKeyJ = false;
            if (keys.Contains(Microsoft.Xna.Framework.Input.Keys.K))
                KeyboardKeyK = true;
            else
                KeyboardKeyK = false;
            if (keys.Contains(Microsoft.Xna.Framework.Input.Keys.L))
                KeyboardKeyL = true;
            else
                KeyboardKeyL = false;
            if (keys.Contains(Microsoft.Xna.Framework.Input.Keys.OemSemicolon))
                KeyboardKeySemicolon = true;
            else
                KeyboardKeySemicolon = false;
            if (keys.Contains(Microsoft.Xna.Framework.Input.Keys.Apps))
                KeyboardKeyApostrophe = true;
            else
                KeyboardKeyApostrophe = false;
            if (keys.Contains(Microsoft.Xna.Framework.Input.Keys.ChatPadGreen))
                KeyboardKeyGrave = true;
            else
                KeyboardKeyGrave = false;
            if (keys.Contains(Microsoft.Xna.Framework.Input.Keys.LeftShift))
                KeyboardKeyLeftShift = true;
            else
                KeyboardKeyLeftShift = false;
            if (keys.Contains(Microsoft.Xna.Framework.Input.Keys.OemBackslash))
                KeyboardKeyBackslash = true;
            else
                KeyboardKeyBackslash = false;
            if (keys.Contains(Microsoft.Xna.Framework.Input.Keys.Z))
                KeyboardKeyZ = true;
            else
                KeyboardKeyZ = false;
            if (keys.Contains(Microsoft.Xna.Framework.Input.Keys.X))
                KeyboardKeyX = true;
            else
                KeyboardKeyX = false;
            if (keys.Contains(Microsoft.Xna.Framework.Input.Keys.C))
                KeyboardKeyC = true;
            else
                KeyboardKeyC = false;
            if (keys.Contains(Microsoft.Xna.Framework.Input.Keys.V))
                KeyboardKeyV = true;
            else
                KeyboardKeyV = false;
            if (keys.Contains(Microsoft.Xna.Framework.Input.Keys.B))
                KeyboardKeyB = true;
            else
                KeyboardKeyB = false;
            if (keys.Contains(Microsoft.Xna.Framework.Input.Keys.N))
                KeyboardKeyN = true;
            else
                KeyboardKeyN = false;
            if (keys.Contains(Microsoft.Xna.Framework.Input.Keys.M))
                KeyboardKeyM = true;
            else
                KeyboardKeyM = false;
            if (keys.Contains(Microsoft.Xna.Framework.Input.Keys.OemComma))
                KeyboardKeyComma = true;
            else
                KeyboardKeyComma = false;
            if (keys.Contains(Microsoft.Xna.Framework.Input.Keys.OemPeriod))
                KeyboardKeyPeriod = true;
            else
                KeyboardKeyPeriod = false;
            if (keys.Contains(Microsoft.Xna.Framework.Input.Keys.SelectMedia))
                KeyboardKeySlash = true;
            else
                KeyboardKeySlash = false;
            if (keys.Contains(Microsoft.Xna.Framework.Input.Keys.RightShift))
                KeyboardKeyRightShift = true;
            else
                KeyboardKeyRightShift = false;
            if (keys.Contains(Microsoft.Xna.Framework.Input.Keys.Multiply))
                KeyboardKeyMultiply = true;
            else
                KeyboardKeyMultiply = false;
            if (keys.Contains(Microsoft.Xna.Framework.Input.Keys.LeftAlt))
                KeyboardKeyLeftAlt = true;
            else
                KeyboardKeyLeftAlt = false;
            if (keys.Contains(Microsoft.Xna.Framework.Input.Keys.Space))
                KeyboardKeySpace = true;
            else
                KeyboardKeySpace = false;
            if (keys.Contains(Microsoft.Xna.Framework.Input.Keys.CapsLock))
                KeyboardKeyCapital = true;
            else
                KeyboardKeyCapital = false;
            if (keys.Contains(Microsoft.Xna.Framework.Input.Keys.F1))
                KeyboardKeyF1 = true;
            else
                KeyboardKeyF1 = false;
            if (keys.Contains(Microsoft.Xna.Framework.Input.Keys.F2))
                KeyboardKeyF2 = true;
            else
                KeyboardKeyF2 = false;
            if (keys.Contains(Microsoft.Xna.Framework.Input.Keys.F3))
                KeyboardKeyF3 = true;
            else
                KeyboardKeyF3 = false;
            if (keys.Contains(Microsoft.Xna.Framework.Input.Keys.F4))
                KeyboardKeyF4 = true;
            else
                KeyboardKeyF4 = false;
            if (keys.Contains(Microsoft.Xna.Framework.Input.Keys.F5))
                KeyboardKeyF5 = true;
            else
                KeyboardKeyF5 = false;
            if (keys.Contains(Microsoft.Xna.Framework.Input.Keys.F6))
                KeyboardKeyF6 = true;
            else
                KeyboardKeyF6 = false;
            if (keys.Contains(Microsoft.Xna.Framework.Input.Keys.F7))
                KeyboardKeyF7 = true;
            else
                KeyboardKeyF7 = false;
            if (keys.Contains(Microsoft.Xna.Framework.Input.Keys.F8))
                KeyboardKeyF8 = true;
            else
                KeyboardKeyF8 = false;
            if (keys.Contains(Microsoft.Xna.Framework.Input.Keys.F9))
                KeyboardKeyF9 = true;
            else
                KeyboardKeyF9 = false;
            if (keys.Contains(Microsoft.Xna.Framework.Input.Keys.F10))
                KeyboardKeyF10 = true;
            else
                KeyboardKeyF10 = false;
            if (keys.Contains(Microsoft.Xna.Framework.Input.Keys.NumLock))
                KeyboardKeyNumberLock = true;
            else
                KeyboardKeyNumberLock = false;
            if (keys.Contains(Microsoft.Xna.Framework.Input.Keys.Scroll))
                KeyboardKeyScrollLock = true;
            else
                KeyboardKeyScrollLock = false;
            if (keys.Contains(Microsoft.Xna.Framework.Input.Keys.NumPad7))
                KeyboardKeyNumberPad7 = true;
            else
                KeyboardKeyNumberPad7 = false;
            if (keys.Contains(Microsoft.Xna.Framework.Input.Keys.NumPad8))
                KeyboardKeyNumberPad8 = true;
            else
                KeyboardKeyNumberPad8 = false;
            if (keys.Contains(Microsoft.Xna.Framework.Input.Keys.NumPad9))
                KeyboardKeyNumberPad9 = true;
            else
                KeyboardKeyNumberPad9 = false;
            if (keys.Contains(Microsoft.Xna.Framework.Input.Keys.Subtract))
                KeyboardKeySubtract = true;
            else
                KeyboardKeySubtract = false;
            if (keys.Contains(Microsoft.Xna.Framework.Input.Keys.NumPad4))
                KeyboardKeyNumberPad4 = true;
            else
                KeyboardKeyNumberPad4 = false;
            if (keys.Contains(Microsoft.Xna.Framework.Input.Keys.NumPad5))
                KeyboardKeyNumberPad5 = true;
            else
                KeyboardKeyNumberPad5 = false;
            if (keys.Contains(Microsoft.Xna.Framework.Input.Keys.NumPad6))
                KeyboardKeyNumberPad6 = true;
            else
                KeyboardKeyNumberPad6 = false;
            if (keys.Contains(Microsoft.Xna.Framework.Input.Keys.Add))
                KeyboardKeyAdd = true;
            else
                KeyboardKeyAdd = false;
            if (keys.Contains(Microsoft.Xna.Framework.Input.Keys.NumPad1))
                KeyboardKeyNumberPad1 = true;
            else
                KeyboardKeyNumberPad1 = false;
            if (keys.Contains(Microsoft.Xna.Framework.Input.Keys.NumPad2))
                KeyboardKeyNumberPad2 = true;
            else
                KeyboardKeyNumberPad2 = false;
            if (keys.Contains(Microsoft.Xna.Framework.Input.Keys.NumPad3))
                KeyboardKeyNumberPad3 = true;
            else
                KeyboardKeyNumberPad3 = false;
            if (keys.Contains(Microsoft.Xna.Framework.Input.Keys.NumPad0))
                KeyboardKeyNumberPad0 = true;
            else
                KeyboardKeyNumberPad0 = false;
            if (keys.Contains(Microsoft.Xna.Framework.Input.Keys.Decimal))
                KeyboardKeyDecimal = true;
            else
                KeyboardKeyDecimal = false;
            if (keys.Contains(Microsoft.Xna.Framework.Input.Keys.OemPipe))
                KeyboardKeyOem102 = true;
            else
                KeyboardKeyOem102 = false;
            if (keys.Contains(Microsoft.Xna.Framework.Input.Keys.F11))
                KeyboardKeyF11 = true;
            else
                KeyboardKeyF11 = false;
            if (keys.Contains(Microsoft.Xna.Framework.Input.Keys.F12))
                KeyboardKeyF12 = true;
            else
                KeyboardKeyF12 = false;
            if (keys.Contains(Microsoft.Xna.Framework.Input.Keys.F13))
                KeyboardKeyF13 = true;
            else
                KeyboardKeyF13 = false;
            if (keys.Contains(Microsoft.Xna.Framework.Input.Keys.F14))
                KeyboardKeyF14 = true;
            else
                KeyboardKeyF14 = false;
            if (keys.Contains(Microsoft.Xna.Framework.Input.Keys.F15))
                KeyboardKeyF15 = true;
            else
                KeyboardKeyF15 = false;
            if (keys.Contains(Microsoft.Xna.Framework.Input.Keys.Kana))
                KeyboardKeyKana = true;
            else
                KeyboardKeyKana = false;
            if (keys.Contains(Microsoft.Xna.Framework.Input.Keys.OemAuto))
                KeyboardKeyAbntC1 = true;
            else
                KeyboardKeyAbntC1 = false;
            if (keys.Contains(Microsoft.Xna.Framework.Input.Keys.ImeConvert))
                KeyboardKeyConvert = true;
            else
                KeyboardKeyConvert = false;
            if (keys.Contains(Microsoft.Xna.Framework.Input.Keys.ImeNoConvert))
                KeyboardKeyNoConvert = true;
            else
                KeyboardKeyNoConvert = false;
            if (keys.Contains(Microsoft.Xna.Framework.Input.Keys.Kana))
                KeyboardKeyYen = true;
            else
                KeyboardKeyYen = false;
            if (keys.Contains(Microsoft.Xna.Framework.Input.Keys.OemEnlW))
                KeyboardKeyAbntC2 = true;
            else
                KeyboardKeyAbntC2 = false;
            if (keys.Contains(Microsoft.Xna.Framework.Input.Keys.BrowserStop))
                KeyboardKeyNumberPadEquals = true;
            else
                KeyboardKeyNumberPadEquals = false;
            if (keys.Contains(Microsoft.Xna.Framework.Input.Keys.MediaPreviousTrack))
                KeyboardKeyPreviousTrack = true;
            else
                KeyboardKeyPreviousTrack = false;
            if (keys.Contains(Microsoft.Xna.Framework.Input.Keys.Attn))
                KeyboardKeyAT = true;
            else
                KeyboardKeyAT = false;
            if (keys.Contains(Microsoft.Xna.Framework.Input.Keys.OemSemicolon))
                KeyboardKeyColon = true;
            else
                KeyboardKeyColon = false;
            if (keys.Contains(Microsoft.Xna.Framework.Input.Keys.Pa1))
                KeyboardKeyUnderline = true;
            else
                KeyboardKeyUnderline = false;
            if (keys.Contains(Microsoft.Xna.Framework.Input.Keys.Kanji))
                KeyboardKeyKanji = true;
            else
                KeyboardKeyKanji = false;
            if (keys.Contains(Microsoft.Xna.Framework.Input.Keys.MediaStop))
                KeyboardKeyStop = true;
            else
                KeyboardKeyStop = false;
            if (keys.Contains(Microsoft.Xna.Framework.Input.Keys.Pause))
                KeyboardKeyAX = true;
            else
                KeyboardKeyAX = false;
            if (keys.Contains(Microsoft.Xna.Framework.Input.Keys.Add))
                KeyboardKeyUnlabeled = true;
            else
                KeyboardKeyUnlabeled = false;
            if (keys.Contains(Microsoft.Xna.Framework.Input.Keys.MediaNextTrack))
                KeyboardKeyNextTrack = true;
            else
                KeyboardKeyNextTrack = false;
            if (keys.Contains(Microsoft.Xna.Framework.Input.Keys.End))
                KeyboardKeyNumberPadEnter = true;
            else
                KeyboardKeyNumberPadEnter = false;
            if (keys.Contains(Microsoft.Xna.Framework.Input.Keys.RightControl))
                KeyboardKeyRightControl = true;
            else
                KeyboardKeyRightControl = false;
            if (keys.Contains(Microsoft.Xna.Framework.Input.Keys.VolumeMute))
                KeyboardKeyMute = true;
            else
                KeyboardKeyMute = false;
            if (keys.Contains(Microsoft.Xna.Framework.Input.Keys.CapsLock))
                KeyboardKeyCalculator = true;
            else
                KeyboardKeyCalculator = false;
            if (keys.Contains(Microsoft.Xna.Framework.Input.Keys.MediaPlayPause))
                KeyboardKeyPlayPause = true;
            else
                KeyboardKeyPlayPause = false;
            if (keys.Contains(Microsoft.Xna.Framework.Input.Keys.MediaStop))
                KeyboardKeyMediaStop = true;
            else
                KeyboardKeyMediaStop = false;
            if (keys.Contains(Microsoft.Xna.Framework.Input.Keys.VolumeDown))
                KeyboardKeyVolumeDown = true;
            else
                KeyboardKeyVolumeDown = false;
            if (keys.Contains(Microsoft.Xna.Framework.Input.Keys.VolumeUp))
                KeyboardKeyVolumeUp = true;
            else
                KeyboardKeyVolumeUp = false;
            if (keys.Contains(Microsoft.Xna.Framework.Input.Keys.BrowserHome))
                KeyboardKeyWebHome = true;
            else
                KeyboardKeyWebHome = false;
            if (keys.Contains(Microsoft.Xna.Framework.Input.Keys.OemComma))
                KeyboardKeyNumberPadComma = true;
            else
                KeyboardKeyNumberPadComma = false;
            if (keys.Contains(Microsoft.Xna.Framework.Input.Keys.Divide))
                KeyboardKeyDivide = true;
            else
                KeyboardKeyDivide = false;
            if (keys.Contains(Microsoft.Xna.Framework.Input.Keys.PrintScreen))
                KeyboardKeyPrintScreen = true;
            else
                KeyboardKeyPrintScreen = false;
            if (keys.Contains(Microsoft.Xna.Framework.Input.Keys.RightAlt))
                KeyboardKeyRightAlt = true;
            else
                KeyboardKeyRightAlt = false;
            if (keys.Contains(Microsoft.Xna.Framework.Input.Keys.Pause))
                KeyboardKeyPause = true;
            else
                KeyboardKeyPause = false;
            if (keys.Contains(Microsoft.Xna.Framework.Input.Keys.Home))
                KeyboardKeyHome = true;
            else
                KeyboardKeyHome = false;
            if (keys.Contains(Microsoft.Xna.Framework.Input.Keys.Up))
                KeyboardKeyUp = true;
            else
                KeyboardKeyUp = false;
            if (keys.Contains(Microsoft.Xna.Framework.Input.Keys.PageUp))
                KeyboardKeyPageUp = true;
            else
                KeyboardKeyPageUp = false;
            if (keys.Contains(Microsoft.Xna.Framework.Input.Keys.Left))
                KeyboardKeyLeft = true;
            else
                KeyboardKeyLeft = false;
            if (keys.Contains(Microsoft.Xna.Framework.Input.Keys.Right))
                KeyboardKeyRight = true;
            else
                KeyboardKeyRight = false;
            if (keys.Contains(Microsoft.Xna.Framework.Input.Keys.End))
                KeyboardKeyEnd = true;
            else
                KeyboardKeyEnd = false;
            if (keys.Contains(Microsoft.Xna.Framework.Input.Keys.Down))
                KeyboardKeyDown = true;
            else
                KeyboardKeyDown = false;
            if (keys.Contains(Microsoft.Xna.Framework.Input.Keys.PageDown))
                KeyboardKeyPageDown = true;
            else
                KeyboardKeyPageDown = false;
            if (keys.Contains(Microsoft.Xna.Framework.Input.Keys.Insert))
                KeyboardKeyInsert = true;
            else
                KeyboardKeyInsert = false;
            if (keys.Contains(Microsoft.Xna.Framework.Input.Keys.Delete))
                KeyboardKeyDelete = true;
            else
                KeyboardKeyDelete = false;
            if (keys.Contains(Microsoft.Xna.Framework.Input.Keys.LeftWindows))
                KeyboardKeyLeftWindowsKey = true;
            else
                KeyboardKeyLeftWindowsKey = false;
            if (keys.Contains(Microsoft.Xna.Framework.Input.Keys.RightWindows))
                KeyboardKeyRightWindowsKey = true;
            else
                KeyboardKeyRightWindowsKey = false;
            if (keys.Contains(Microsoft.Xna.Framework.Input.Keys.Apps))
                KeyboardKeyApplications = true;
            else
                KeyboardKeyApplications = false;
            if (keys.Contains(Microsoft.Xna.Framework.Input.Keys.ChatPadOrange))
                KeyboardKeyPower = true;
            else
                KeyboardKeyPower = false;
            if (keys.Contains(Microsoft.Xna.Framework.Input.Keys.Sleep))
                KeyboardKeySleep = true;
            else
                KeyboardKeySleep = false;
            if (keys.Contains(Microsoft.Xna.Framework.Input.Keys.Sleep))
                KeyboardKeyWake = true;
            else
                KeyboardKeyWake = false;
            if (keys.Contains(Microsoft.Xna.Framework.Input.Keys.BrowserSearch))
                KeyboardKeyWebSearch = true;
            else
                KeyboardKeyWebSearch = false;
            if (keys.Contains(Microsoft.Xna.Framework.Input.Keys.BrowserFavorites))
                KeyboardKeyWebFavorites = true;
            else
                KeyboardKeyWebFavorites = false;
            if (keys.Contains(Microsoft.Xna.Framework.Input.Keys.BrowserRefresh))
                KeyboardKeyWebRefresh = true;
            else
                KeyboardKeyWebRefresh = false;
            if (keys.Contains(Microsoft.Xna.Framework.Input.Keys.BrowserStop))
                KeyboardKeyWebStop = true;
            else
                KeyboardKeyWebStop = false;
            if (keys.Contains(Microsoft.Xna.Framework.Input.Keys.BrowserForward))
                KeyboardKeyWebForward = true;
            else
                KeyboardKeyWebForward = false;
            if (keys.Contains(Microsoft.Xna.Framework.Input.Keys.BrowserBack))
                KeyboardKeyWebBack = true;
            else
                KeyboardKeyWebBack = false;
            if (keys.Contains(Microsoft.Xna.Framework.Input.Keys.Attn))
                KeyboardKeyMyComputer = true;
            else
                KeyboardKeyMyComputer = false;
            if (keys.Contains(Microsoft.Xna.Framework.Input.Keys.LaunchMail))
                KeyboardKeyMail = true;
            else
                KeyboardKeyMail = false;
            if (keys.Contains(Microsoft.Xna.Framework.Input.Keys.Select))
                KeyboardKeyMediaSelect = true;
            else
                KeyboardKeyMediaSelect = false;
            if (keys.Contains(Microsoft.Xna.Framework.Input.Keys.None))
                KeyboardKeyUnknown = true;
            else
                KeyboardKeyUnknown = false;
        }
    }
}