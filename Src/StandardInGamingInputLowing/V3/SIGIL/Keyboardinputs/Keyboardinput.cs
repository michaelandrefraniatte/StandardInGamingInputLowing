using SharpDX.DirectInput;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using Keyboardinputs;

namespace KeyboardInputsAPI
{
    public class KeyboardInput
    {
        [DllImport("winmm.dll", EntryPoint = "timeBeginPeriod")]
        private static extern uint TimeBeginPeriod(uint ms);
        [DllImport("winmm.dll", EntryPoint = "timeEndPeriod")]
        private static extern uint TimeEndPeriod(uint ms);
        [DllImport("ntdll.dll", EntryPoint = "NtSetTimerResolution")]
        private static extern void NtSetTimerResolution(uint DesiredResolution, bool SetResolution, ref uint CurrentResolution);
        private static uint CurrentResolution = 0;
        private static bool running, formvisible;
        static DirectInput directInput = new DirectInput();
        public Form1 form1 = new Form1();
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
        public KeyboardInput()
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
        public void taskK()
        {
            for (; ; )
            {
                if (!running)
                    break;
                KeyboardInputProcess();
                System.Threading.Thread.Sleep(1);
                if (formvisible)
                {
                    string str = "Keyboard1KeyA : " + Keyboard1KeyA + Environment.NewLine;
                    str += "Keyboard1KeyB : " + Keyboard1KeyB + Environment.NewLine;
                    str += "Keyboard1KeyC : " + Keyboard1KeyC + Environment.NewLine;
                    str += "Keyboard1KeyD : " + Keyboard1KeyD + Environment.NewLine;
                    str += "Keyboard1KeyE : " + Keyboard1KeyE + Environment.NewLine;
                    str += "Keyboard1KeyF : " + Keyboard1KeyF + Environment.NewLine;
                    str += "Keyboard1KeyG : " + Keyboard1KeyG + Environment.NewLine;
                    str += "Keyboard1KeyH : " + Keyboard1KeyH + Environment.NewLine;
                    str += "Keyboard1KeyI : " + Keyboard1KeyI + Environment.NewLine;
                    str += "Keyboard1KeyJ : " + Keyboard1KeyJ + Environment.NewLine;
                    str += "Keyboard1KeyK : " + Keyboard1KeyK + Environment.NewLine;
                    str += "Keyboard1KeyL : " + Keyboard1KeyL + Environment.NewLine;
                    str += "Keyboard1KeyM : " + Keyboard1KeyM + Environment.NewLine;
                    str += "Keyboard1KeyN : " + Keyboard1KeyN + Environment.NewLine;
                    str += "Keyboard1KeyO : " + Keyboard1KeyO + Environment.NewLine;
                    str += "Keyboard1KeyP : " + Keyboard1KeyP + Environment.NewLine;
                    str += "Keyboard1KeyQ : " + Keyboard1KeyQ + Environment.NewLine;
                    str += "Keyboard1KeyR : " + Keyboard1KeyR + Environment.NewLine;
                    str += "Keyboard1KeyS : " + Keyboard1KeyS + Environment.NewLine;
                    str += "Keyboard1KeyT : " + Keyboard1KeyT + Environment.NewLine;
                    str += "Keyboard1KeyU : " + Keyboard1KeyU + Environment.NewLine;
                    str += "Keyboard1KeyV : " + Keyboard1KeyV + Environment.NewLine;
                    str += "Keyboard1KeyW : " + Keyboard1KeyW + Environment.NewLine;
                    str += "Keyboard1KeyX : " + Keyboard1KeyX + Environment.NewLine;
                    str += "Keyboard1KeyY : " + Keyboard1KeyY + Environment.NewLine;
                    str += "Keyboard1KeyZ : " + Keyboard1KeyZ + Environment.NewLine;
                    str += "Keyboard1KeyEscape : " + Keyboard1KeyEscape + Environment.NewLine;
                    str += "Keyboard1KeyD1 : " + Keyboard1KeyD1 + Environment.NewLine;
                    str += "Keyboard1KeyD2 : " + Keyboard1KeyD2 + Environment.NewLine;
                    str += "Keyboard1KeyD3 : " + Keyboard1KeyD3 + Environment.NewLine;
                    str += "Keyboard1KeyD4 : " + Keyboard1KeyD4 + Environment.NewLine;
                    str += "Keyboard1KeyD5 : " + Keyboard1KeyD5 + Environment.NewLine;
                    str += "Keyboard1KeyD6 : " + Keyboard1KeyD6 + Environment.NewLine;
                    str += "Keyboard1KeyD7 : " + Keyboard1KeyD7 + Environment.NewLine;
                    str += "Keyboard1KeyD8 : " + Keyboard1KeyD8 + Environment.NewLine;
                    str += "Keyboard1KeyD9 : " + Keyboard1KeyD9 + Environment.NewLine;
                    str += "Keyboard1KeyD0 : " + Keyboard1KeyD0 + Environment.NewLine;
                    str += "Keyboard1KeyMinus : " + Keyboard1KeyMinus + Environment.NewLine;
                    str += "Keyboard1KeyEquals : " + Keyboard1KeyEquals + Environment.NewLine;
                    str += "Keyboard1KeyBack : " + Keyboard1KeyBack + Environment.NewLine;
                    str += "Keyboard1KeyTab : " + Keyboard1KeyTab + Environment.NewLine;
                    str += "Keyboard1KeyLeftBracket : " + Keyboard1KeyLeftBracket + Environment.NewLine;
                    str += "Keyboard1KeyRightBracket : " + Keyboard1KeyRightBracket + Environment.NewLine;
                    str += "Keyboard1KeyReturn : " + Keyboard1KeyReturn + Environment.NewLine;
                    str += "Keyboard1KeyLeftControl : " + Keyboard1KeyLeftControl + Environment.NewLine;
                    str += "Keyboard1KeySemicolon : " + Keyboard1KeySemicolon + Environment.NewLine;
                    str += "Keyboard1KeyApostrophe : " + Keyboard1KeyApostrophe + Environment.NewLine;
                    str += "Keyboard1KeyGrave : " + Keyboard1KeyGrave + Environment.NewLine;
                    str += "Keyboard1KeyLeftShift : " + Keyboard1KeyLeftShift + Environment.NewLine;
                    str += "Keyboard1KeyBackslash : " + Keyboard1KeyBackslash + Environment.NewLine;
                    str += "Keyboard1KeyComma : " + Keyboard1KeyComma + Environment.NewLine;
                    str += "Keyboard1KeyPeriod : " + Keyboard1KeyPeriod + Environment.NewLine;
                    str += "Keyboard1KeySlash : " + Keyboard1KeySlash + Environment.NewLine;
                    str += "Keyboard1KeyRightShift : " + Keyboard1KeyRightShift + Environment.NewLine;
                    str += "Keyboard1KeyMultiply : " + Keyboard1KeyMultiply + Environment.NewLine;
                    str += "Keyboard1KeyLeftAlt : " + Keyboard1KeyLeftAlt + Environment.NewLine;
                    str += "Keyboard1KeySpace : " + Keyboard1KeySpace + Environment.NewLine;
                    str += "Keyboard1KeyCapital : " + Keyboard1KeyCapital + Environment.NewLine;
                    str += "Keyboard1KeyF1 : " + Keyboard1KeyF1 + Environment.NewLine;
                    str += "Keyboard1KeyF2 : " + Keyboard1KeyF2 + Environment.NewLine;
                    str += "Keyboard1KeyF3 : " + Keyboard1KeyF3 + Environment.NewLine;
                    str += "Keyboard1KeyF4 : " + Keyboard1KeyF4 + Environment.NewLine;
                    str += "Keyboard1KeyF5 : " + Keyboard1KeyF5 + Environment.NewLine;
                    str += "Keyboard1KeyF6 : " + Keyboard1KeyF6 + Environment.NewLine;
                    str += "Keyboard1KeyF7 : " + Keyboard1KeyF7 + Environment.NewLine;
                    str += "Keyboard1KeyF8 : " + Keyboard1KeyF8 + Environment.NewLine;
                    str += "Keyboard1KeyF9 : " + Keyboard1KeyF9 + Environment.NewLine;
                    str += "Keyboard1KeyF10 : " + Keyboard1KeyF10 + Environment.NewLine;
                    str += "Keyboard1KeyF11 : " + Keyboard1KeyF11 + Environment.NewLine;
                    str += "Keyboard1KeyF12 : " + Keyboard1KeyF12 + Environment.NewLine;
                    str += "Keyboard1KeyF13 : " + Keyboard1KeyF13 + Environment.NewLine;
                    str += "Keyboard1KeyF14 : " + Keyboard1KeyF14 + Environment.NewLine;
                    str += "Keyboard1KeyF15 : " + Keyboard1KeyF15 + Environment.NewLine;
                    str += "Keyboard1KeyNumberLock : " + Keyboard1KeyNumberLock + Environment.NewLine;
                    str += "Keyboard1KeyScrollLock : " + Keyboard1KeyScrollLock + Environment.NewLine;
                    str += "Keyboard1KeyNumberPad0 : " + Keyboard1KeyNumberPad0 + Environment.NewLine;
                    str += "Keyboard1KeyNumberPad1 : " + Keyboard1KeyNumberPad1 + Environment.NewLine;
                    str += "Keyboard1KeyNumberPad2 : " + Keyboard1KeyNumberPad2 + Environment.NewLine;
                    str += "Keyboard1KeyNumberPad3 : " + Keyboard1KeyNumberPad3 + Environment.NewLine;
                    str += "Keyboard1KeyNumberPad4 : " + Keyboard1KeyNumberPad4 + Environment.NewLine;
                    str += "Keyboard1KeyNumberPad5 : " + Keyboard1KeyNumberPad5 + Environment.NewLine;
                    str += "Keyboard1KeyNumberPad6 : " + Keyboard1KeyNumberPad6 + Environment.NewLine;
                    str += "Keyboard1KeyNumberPad7 : " + Keyboard1KeyNumberPad7 + Environment.NewLine;
                    str += "Keyboard1KeyNumberPad8 : " + Keyboard1KeyNumberPad8 + Environment.NewLine;
                    str += "Keyboard1KeyNumberPad9 : " + Keyboard1KeyNumberPad9 + Environment.NewLine;
                    str += "Keyboard1KeySubtract : " + Keyboard1KeySubtract + Environment.NewLine;
                    str += "Keyboard1KeyAdd : " + Keyboard1KeyAdd + Environment.NewLine;
                    str += "Keyboard1KeyDecimal : " + Keyboard1KeyDecimal + Environment.NewLine;
                    str += "Keyboard1KeyOem102 : " + Keyboard1KeyOem102 + Environment.NewLine;
                    str += "Keyboard1KeyKana : " + Keyboard1KeyKana + Environment.NewLine;
                    str += "Keyboard1KeyAbntC1 : " + Keyboard1KeyAbntC1 + Environment.NewLine;
                    str += "Keyboard1KeyConvert : " + Keyboard1KeyConvert + Environment.NewLine;
                    str += "Keyboard1KeyNoConvert : " + Keyboard1KeyNoConvert + Environment.NewLine;
                    str += "Keyboard1KeyYen : " + Keyboard1KeyYen + Environment.NewLine;
                    str += "Keyboard1KeyAbntC2 : " + Keyboard1KeyAbntC2 + Environment.NewLine;
                    str += "Keyboard1KeyNumberPadEquals : " + Keyboard1KeyNumberPadEquals + Environment.NewLine;
                    str += "Keyboard1KeyPreviousTrack : " + Keyboard1KeyPreviousTrack + Environment.NewLine;
                    str += "Keyboard1KeyAT : " + Keyboard1KeyAT + Environment.NewLine;
                    str += "Keyboard1KeyColon : " + Keyboard1KeyColon + Environment.NewLine;
                    str += "Keyboard1KeyUnderline : " + Keyboard1KeyUnderline + Environment.NewLine;
                    str += "Keyboard1KeyKanji : " + Keyboard1KeyKanji + Environment.NewLine;
                    str += "Keyboard1KeyStop : " + Keyboard1KeyStop + Environment.NewLine;
                    str += "Keyboard1KeyAX : " + Keyboard1KeyAX + Environment.NewLine;
                    str += "Keyboard1KeyUnlabeled : " + Keyboard1KeyUnlabeled + Environment.NewLine;
                    str += "Keyboard1KeyNextTrack : " + Keyboard1KeyNextTrack + Environment.NewLine;
                    str += "Keyboard1KeyNumberPadEnter : " + Keyboard1KeyNumberPadEnter + Environment.NewLine;
                    str += "Keyboard1KeyRightControl : " + Keyboard1KeyRightControl + Environment.NewLine;
                    str += "Keyboard1KeyMute : " + Keyboard1KeyMute + Environment.NewLine;
                    str += "Keyboard1KeyCalculator : " + Keyboard1KeyCalculator + Environment.NewLine;
                    str += "Keyboard1KeyPlayPause : " + Keyboard1KeyPlayPause + Environment.NewLine;
                    str += "Keyboard1KeyMediaStop : " + Keyboard1KeyMediaStop + Environment.NewLine;
                    str += "Keyboard1KeyVolumeDown : " + Keyboard1KeyVolumeDown + Environment.NewLine;
                    str += "Keyboard1KeyVolumeUp : " + Keyboard1KeyVolumeUp + Environment.NewLine;
                    str += "Keyboard1KeyWebHome : " + Keyboard1KeyWebHome + Environment.NewLine;
                    str += "Keyboard1KeyNumberPadComma : " + Keyboard1KeyNumberPadComma + Environment.NewLine;
                    str += "Keyboard1KeyDivide : " + Keyboard1KeyDivide + Environment.NewLine;
                    str += "Keyboard1KeyPrintScreen : " + Keyboard1KeyPrintScreen + Environment.NewLine;
                    str += "Keyboard1KeyRightAlt : " + Keyboard1KeyRightAlt + Environment.NewLine;
                    str += "Keyboard1KeyPause : " + Keyboard1KeyPause + Environment.NewLine;
                    str += "Keyboard1KeyHome : " + Keyboard1KeyHome + Environment.NewLine;
                    str += "Keyboard1KeyUp : " + Keyboard1KeyUp + Environment.NewLine;
                    str += "Keyboard1KeyPageUp : " + Keyboard1KeyPageUp + Environment.NewLine;
                    str += "Keyboard1KeyLeft : " + Keyboard1KeyLeft + Environment.NewLine;
                    str += "Keyboard1KeyRight : " + Keyboard1KeyRight + Environment.NewLine;
                    str += "Keyboard1KeyEnd : " + Keyboard1KeyEnd + Environment.NewLine;
                    str += "Keyboard1KeyDown : " + Keyboard1KeyDown + Environment.NewLine;
                    str += "Keyboard1KeyPageDown : " + Keyboard1KeyPageDown + Environment.NewLine;
                    str += "Keyboard1KeyInsert : " + Keyboard1KeyInsert + Environment.NewLine;
                    str += "Keyboard1KeyDelete : " + Keyboard1KeyDelete + Environment.NewLine;
                    str += "Keyboard1KeyLeftWindowsKey : " + Keyboard1KeyLeftWindowsKey + Environment.NewLine;
                    str += "Keyboard1KeyRightWindowsKey : " + Keyboard1KeyRightWindowsKey + Environment.NewLine;
                    str += "Keyboard1KeyApplications : " + Keyboard1KeyApplications + Environment.NewLine;
                    str += "Keyboard1KeyPower : " + Keyboard1KeyPower + Environment.NewLine;
                    str += "Keyboard1KeySleep : " + Keyboard1KeySleep + Environment.NewLine;
                    str += "Keyboard1KeyWake : " + Keyboard1KeyWake + Environment.NewLine;
                    str += "Keyboard1KeyWebSearch : " + Keyboard1KeyWebSearch + Environment.NewLine;
                    str += "Keyboard1KeyWebFavorites : " + Keyboard1KeyWebFavorites + Environment.NewLine;
                    str += "Keyboard1KeyWebRefresh : " + Keyboard1KeyWebRefresh + Environment.NewLine;
                    str += "Keyboard1KeyWebStop : " + Keyboard1KeyWebStop + Environment.NewLine;
                    str += "Keyboard1KeyWebForward : " + Keyboard1KeyWebForward + Environment.NewLine;
                    str += "Keyboard1KeyWebBack : " + Keyboard1KeyWebBack + Environment.NewLine;
                    str += "Keyboard1KeyMyComputer : " + Keyboard1KeyMyComputer + Environment.NewLine;
                    str += "Keyboard1KeyMail : " + Keyboard1KeyMail + Environment.NewLine;
                    str += "Keyboard1KeyMediaSelect : " + Keyboard1KeyMediaSelect + Environment.NewLine;
                    str += "Keyboard1KeyUnknown : " + Keyboard1KeyUnknown + Environment.NewLine;
                    str += "Keyboard2KeyA : " + Keyboard2KeyA + Environment.NewLine;
                    str += "Keyboard2KeyB : " + Keyboard2KeyB + Environment.NewLine;
                    str += "Keyboard2KeyC : " + Keyboard2KeyC + Environment.NewLine;
                    str += "Keyboard2KeyD : " + Keyboard2KeyD + Environment.NewLine;
                    str += "Keyboard2KeyE : " + Keyboard2KeyE + Environment.NewLine;
                    str += "Keyboard2KeyF : " + Keyboard2KeyF + Environment.NewLine;
                    str += "Keyboard2KeyG : " + Keyboard2KeyG + Environment.NewLine;
                    str += "Keyboard2KeyH : " + Keyboard2KeyH + Environment.NewLine;
                    str += "Keyboard2KeyI : " + Keyboard2KeyI + Environment.NewLine;
                    str += "Keyboard2KeyJ : " + Keyboard2KeyJ + Environment.NewLine;
                    str += "Keyboard2KeyK : " + Keyboard2KeyK + Environment.NewLine;
                    str += "Keyboard2KeyL : " + Keyboard2KeyL + Environment.NewLine;
                    str += "Keyboard2KeyM : " + Keyboard2KeyM + Environment.NewLine;
                    str += "Keyboard2KeyN : " + Keyboard2KeyN + Environment.NewLine;
                    str += "Keyboard2KeyO : " + Keyboard2KeyO + Environment.NewLine;
                    str += "Keyboard2KeyP : " + Keyboard2KeyP + Environment.NewLine;
                    str += "Keyboard2KeyQ : " + Keyboard2KeyQ + Environment.NewLine;
                    str += "Keyboard2KeyR : " + Keyboard2KeyR + Environment.NewLine;
                    str += "Keyboard2KeyS : " + Keyboard2KeyS + Environment.NewLine;
                    str += "Keyboard2KeyT : " + Keyboard2KeyT + Environment.NewLine;
                    str += "Keyboard2KeyU : " + Keyboard2KeyU + Environment.NewLine;
                    str += "Keyboard2KeyV : " + Keyboard2KeyV + Environment.NewLine;
                    str += "Keyboard2KeyW : " + Keyboard2KeyW + Environment.NewLine;
                    str += "Keyboard2KeyX : " + Keyboard2KeyX + Environment.NewLine;
                    str += "Keyboard2KeyY : " + Keyboard2KeyY + Environment.NewLine;
                    str += "Keyboard2KeyZ : " + Keyboard2KeyZ + Environment.NewLine;
                    str += "Keyboard2KeyEscape : " + Keyboard2KeyEscape + Environment.NewLine;
                    str += "Keyboard2KeyD1 : " + Keyboard2KeyD1 + Environment.NewLine;
                    str += "Keyboard2KeyD2 : " + Keyboard2KeyD2 + Environment.NewLine;
                    str += "Keyboard2KeyD3 : " + Keyboard2KeyD3 + Environment.NewLine;
                    str += "Keyboard2KeyD4 : " + Keyboard2KeyD4 + Environment.NewLine;
                    str += "Keyboard2KeyD5 : " + Keyboard2KeyD5 + Environment.NewLine;
                    str += "Keyboard2KeyD6 : " + Keyboard2KeyD6 + Environment.NewLine;
                    str += "Keyboard2KeyD7 : " + Keyboard2KeyD7 + Environment.NewLine;
                    str += "Keyboard2KeyD8 : " + Keyboard2KeyD8 + Environment.NewLine;
                    str += "Keyboard2KeyD9 : " + Keyboard2KeyD9 + Environment.NewLine;
                    str += "Keyboard2KeyD0 : " + Keyboard2KeyD0 + Environment.NewLine;
                    str += "Keyboard2KeyMinus : " + Keyboard2KeyMinus + Environment.NewLine;
                    str += "Keyboard2KeyEquals : " + Keyboard2KeyEquals + Environment.NewLine;
                    str += "Keyboard2KeyBack : " + Keyboard2KeyBack + Environment.NewLine;
                    str += "Keyboard2KeyTab : " + Keyboard2KeyTab + Environment.NewLine;
                    str += "Keyboard2KeyLeftBracket : " + Keyboard2KeyLeftBracket + Environment.NewLine;
                    str += "Keyboard2KeyRightBracket : " + Keyboard2KeyRightBracket + Environment.NewLine;
                    str += "Keyboard2KeyReturn : " + Keyboard2KeyReturn + Environment.NewLine;
                    str += "Keyboard2KeyLeftControl : " + Keyboard2KeyLeftControl + Environment.NewLine;
                    str += "Keyboard2KeySemicolon : " + Keyboard2KeySemicolon + Environment.NewLine;
                    str += "Keyboard2KeyApostrophe : " + Keyboard2KeyApostrophe + Environment.NewLine;
                    str += "Keyboard2KeyGrave : " + Keyboard2KeyGrave + Environment.NewLine;
                    str += "Keyboard2KeyLeftShift : " + Keyboard2KeyLeftShift + Environment.NewLine;
                    str += "Keyboard2KeyBackslash : " + Keyboard2KeyBackslash + Environment.NewLine;
                    str += "Keyboard2KeyComma : " + Keyboard2KeyComma + Environment.NewLine;
                    str += "Keyboard2KeyPeriod : " + Keyboard2KeyPeriod + Environment.NewLine;
                    str += "Keyboard2KeySlash : " + Keyboard2KeySlash + Environment.NewLine;
                    str += "Keyboard2KeyRightShift : " + Keyboard2KeyRightShift + Environment.NewLine;
                    str += "Keyboard2KeyMultiply : " + Keyboard2KeyMultiply + Environment.NewLine;
                    str += "Keyboard2KeyLeftAlt : " + Keyboard2KeyLeftAlt + Environment.NewLine;
                    str += "Keyboard2KeySpace : " + Keyboard2KeySpace + Environment.NewLine;
                    str += "Keyboard2KeyCapital : " + Keyboard2KeyCapital + Environment.NewLine;
                    str += "Keyboard2KeyF1 : " + Keyboard2KeyF1 + Environment.NewLine;
                    str += "Keyboard2KeyF2 : " + Keyboard2KeyF2 + Environment.NewLine;
                    str += "Keyboard2KeyF3 : " + Keyboard2KeyF3 + Environment.NewLine;
                    str += "Keyboard2KeyF4 : " + Keyboard2KeyF4 + Environment.NewLine;
                    str += "Keyboard2KeyF5 : " + Keyboard2KeyF5 + Environment.NewLine;
                    str += "Keyboard2KeyF6 : " + Keyboard2KeyF6 + Environment.NewLine;
                    str += "Keyboard2KeyF7 : " + Keyboard2KeyF7 + Environment.NewLine;
                    str += "Keyboard2KeyF8 : " + Keyboard2KeyF8 + Environment.NewLine;
                    str += "Keyboard2KeyF9 : " + Keyboard2KeyF9 + Environment.NewLine;
                    str += "Keyboard2KeyF10 : " + Keyboard2KeyF10 + Environment.NewLine;
                    str += "Keyboard2KeyF11 : " + Keyboard2KeyF11 + Environment.NewLine;
                    str += "Keyboard2KeyF12 : " + Keyboard2KeyF12 + Environment.NewLine;
                    str += "Keyboard2KeyF13 : " + Keyboard2KeyF13 + Environment.NewLine;
                    str += "Keyboard2KeyF14 : " + Keyboard2KeyF14 + Environment.NewLine;
                    str += "Keyboard2KeyF15 : " + Keyboard2KeyF15 + Environment.NewLine;
                    str += "Keyboard2KeyNumberLock : " + Keyboard2KeyNumberLock + Environment.NewLine;
                    str += "Keyboard2KeyScrollLock : " + Keyboard2KeyScrollLock + Environment.NewLine;
                    str += "Keyboard2KeyNumberPad0 : " + Keyboard2KeyNumberPad0 + Environment.NewLine;
                    str += "Keyboard2KeyNumberPad1 : " + Keyboard2KeyNumberPad1 + Environment.NewLine;
                    str += "Keyboard2KeyNumberPad2 : " + Keyboard2KeyNumberPad2 + Environment.NewLine;
                    str += "Keyboard2KeyNumberPad3 : " + Keyboard2KeyNumberPad3 + Environment.NewLine;
                    str += "Keyboard2KeyNumberPad4 : " + Keyboard2KeyNumberPad4 + Environment.NewLine;
                    str += "Keyboard2KeyNumberPad5 : " + Keyboard2KeyNumberPad5 + Environment.NewLine;
                    str += "Keyboard2KeyNumberPad6 : " + Keyboard2KeyNumberPad6 + Environment.NewLine;
                    str += "Keyboard2KeyNumberPad7 : " + Keyboard2KeyNumberPad7 + Environment.NewLine;
                    str += "Keyboard2KeyNumberPad8 : " + Keyboard2KeyNumberPad8 + Environment.NewLine;
                    str += "Keyboard2KeyNumberPad9 : " + Keyboard2KeyNumberPad9 + Environment.NewLine;
                    str += "Keyboard2KeySubtract : " + Keyboard2KeySubtract + Environment.NewLine;
                    str += "Keyboard2KeyAdd : " + Keyboard2KeyAdd + Environment.NewLine;
                    str += "Keyboard2KeyDecimal : " + Keyboard2KeyDecimal + Environment.NewLine;
                    str += "Keyboard2KeyOem102 : " + Keyboard2KeyOem102 + Environment.NewLine;
                    str += "Keyboard2KeyKana : " + Keyboard2KeyKana + Environment.NewLine;
                    str += "Keyboard2KeyAbntC1 : " + Keyboard2KeyAbntC1 + Environment.NewLine;
                    str += "Keyboard2KeyConvert : " + Keyboard2KeyConvert + Environment.NewLine;
                    str += "Keyboard2KeyNoConvert : " + Keyboard2KeyNoConvert + Environment.NewLine;
                    str += "Keyboard2KeyYen : " + Keyboard2KeyYen + Environment.NewLine;
                    str += "Keyboard2KeyAbntC2 : " + Keyboard2KeyAbntC2 + Environment.NewLine;
                    str += "Keyboard2KeyNumberPadEquals : " + Keyboard2KeyNumberPadEquals + Environment.NewLine;
                    str += "Keyboard2KeyPreviousTrack : " + Keyboard2KeyPreviousTrack + Environment.NewLine;
                    str += "Keyboard2KeyAT : " + Keyboard2KeyAT + Environment.NewLine;
                    str += "Keyboard2KeyColon : " + Keyboard2KeyColon + Environment.NewLine;
                    str += "Keyboard2KeyUnderline : " + Keyboard2KeyUnderline + Environment.NewLine;
                    str += "Keyboard2KeyKanji : " + Keyboard2KeyKanji + Environment.NewLine;
                    str += "Keyboard2KeyStop : " + Keyboard2KeyStop + Environment.NewLine;
                    str += "Keyboard2KeyAX : " + Keyboard2KeyAX + Environment.NewLine;
                    str += "Keyboard2KeyUnlabeled : " + Keyboard2KeyUnlabeled + Environment.NewLine;
                    str += "Keyboard2KeyNextTrack : " + Keyboard2KeyNextTrack + Environment.NewLine;
                    str += "Keyboard2KeyNumberPadEnter : " + Keyboard2KeyNumberPadEnter + Environment.NewLine;
                    str += "Keyboard2KeyRightControl : " + Keyboard2KeyRightControl + Environment.NewLine;
                    str += "Keyboard2KeyMute : " + Keyboard2KeyMute + Environment.NewLine;
                    str += "Keyboard2KeyCalculator : " + Keyboard2KeyCalculator + Environment.NewLine;
                    str += "Keyboard2KeyPlayPause : " + Keyboard2KeyPlayPause + Environment.NewLine;
                    str += "Keyboard2KeyMediaStop : " + Keyboard2KeyMediaStop + Environment.NewLine;
                    str += "Keyboard2KeyVolumeDown : " + Keyboard2KeyVolumeDown + Environment.NewLine;
                    str += "Keyboard2KeyVolumeUp : " + Keyboard2KeyVolumeUp + Environment.NewLine;
                    str += "Keyboard2KeyWebHome : " + Keyboard2KeyWebHome + Environment.NewLine;
                    str += "Keyboard2KeyNumberPadComma : " + Keyboard2KeyNumberPadComma + Environment.NewLine;
                    str += "Keyboard2KeyDivide : " + Keyboard2KeyDivide + Environment.NewLine;
                    str += "Keyboard2KeyPrintScreen : " + Keyboard2KeyPrintScreen + Environment.NewLine;
                    str += "Keyboard2KeyRightAlt : " + Keyboard2KeyRightAlt + Environment.NewLine;
                    str += "Keyboard2KeyPause : " + Keyboard2KeyPause + Environment.NewLine;
                    str += "Keyboard2KeyHome : " + Keyboard2KeyHome + Environment.NewLine;
                    str += "Keyboard2KeyUp : " + Keyboard2KeyUp + Environment.NewLine;
                    str += "Keyboard2KeyPageUp : " + Keyboard2KeyPageUp + Environment.NewLine;
                    str += "Keyboard2KeyLeft : " + Keyboard2KeyLeft + Environment.NewLine;
                    str += "Keyboard2KeyRight : " + Keyboard2KeyRight + Environment.NewLine;
                    str += "Keyboard2KeyEnd : " + Keyboard2KeyEnd + Environment.NewLine;
                    str += "Keyboard2KeyDown : " + Keyboard2KeyDown + Environment.NewLine;
                    str += "Keyboard2KeyPageDown : " + Keyboard2KeyPageDown + Environment.NewLine;
                    str += "Keyboard2KeyInsert : " + Keyboard2KeyInsert + Environment.NewLine;
                    str += "Keyboard2KeyDelete : " + Keyboard2KeyDelete + Environment.NewLine;
                    str += "Keyboard2KeyLeftWindowsKey : " + Keyboard2KeyLeftWindowsKey + Environment.NewLine;
                    str += "Keyboard2KeyRightWindowsKey : " + Keyboard2KeyRightWindowsKey + Environment.NewLine;
                    str += "Keyboard2KeyApplications : " + Keyboard2KeyApplications + Environment.NewLine;
                    str += "Keyboard2KeyPower : " + Keyboard2KeyPower + Environment.NewLine;
                    str += "Keyboard2KeySleep : " + Keyboard2KeySleep + Environment.NewLine;
                    str += "Keyboard2KeyWake : " + Keyboard2KeyWake + Environment.NewLine;
                    str += "Keyboard2KeyWebSearch : " + Keyboard2KeyWebSearch + Environment.NewLine;
                    str += "Keyboard2KeyWebFavorites : " + Keyboard2KeyWebFavorites + Environment.NewLine;
                    str += "Keyboard2KeyWebRefresh : " + Keyboard2KeyWebRefresh + Environment.NewLine;
                    str += "Keyboard2KeyWebStop : " + Keyboard2KeyWebStop + Environment.NewLine;
                    str += "Keyboard2KeyWebForward : " + Keyboard2KeyWebForward + Environment.NewLine;
                    str += "Keyboard2KeyWebBack : " + Keyboard2KeyWebBack + Environment.NewLine;
                    str += "Keyboard2KeyMyComputer : " + Keyboard2KeyMyComputer + Environment.NewLine;
                    str += "Keyboard2KeyMail : " + Keyboard2KeyMail + Environment.NewLine;
                    str += "Keyboard2KeyMediaSelect : " + Keyboard2KeyMediaSelect + Environment.NewLine;
                    str += "Keyboard2KeyUnknown : " + Keyboard2KeyUnknown + Environment.NewLine;
                    str += Environment.NewLine;
                    form1.SetLabel1(str);
                }
            }
        }
        public void BeginPolling()
        {
            Task.Run(() => taskK());
        }
        private static Keyboard[] keyboard = new Keyboard[] { null };
        private static Guid[] keyboardGuid = new Guid[] { Guid.Empty };
        private static int knum = 0;
        public bool Keyboard1KeyEscape;
        public bool Keyboard1KeyD1;
        public bool Keyboard1KeyD2;
        public bool Keyboard1KeyD3;
        public bool Keyboard1KeyD4;
        public bool Keyboard1KeyD5;
        public bool Keyboard1KeyD6;
        public bool Keyboard1KeyD7;
        public bool Keyboard1KeyD8;
        public bool Keyboard1KeyD9;
        public bool Keyboard1KeyD0;
        public bool Keyboard1KeyMinus;
        public bool Keyboard1KeyEquals;
        public bool Keyboard1KeyBack;
        public bool Keyboard1KeyTab;
        public bool Keyboard1KeyQ;
        public bool Keyboard1KeyW;
        public bool Keyboard1KeyE;
        public bool Keyboard1KeyR;
        public bool Keyboard1KeyT;
        public bool Keyboard1KeyY;
        public bool Keyboard1KeyU;
        public bool Keyboard1KeyI;
        public bool Keyboard1KeyO;
        public bool Keyboard1KeyP;
        public bool Keyboard1KeyLeftBracket;
        public bool Keyboard1KeyRightBracket;
        public bool Keyboard1KeyReturn;
        public bool Keyboard1KeyLeftControl;
        public bool Keyboard1KeyA;
        public bool Keyboard1KeyS;
        public bool Keyboard1KeyD;
        public bool Keyboard1KeyF;
        public bool Keyboard1KeyG;
        public bool Keyboard1KeyH;
        public bool Keyboard1KeyJ;
        public bool Keyboard1KeyK;
        public bool Keyboard1KeyL;
        public bool Keyboard1KeySemicolon;
        public bool Keyboard1KeyApostrophe;
        public bool Keyboard1KeyGrave;
        public bool Keyboard1KeyLeftShift;
        public bool Keyboard1KeyBackslash;
        public bool Keyboard1KeyZ;
        public bool Keyboard1KeyX;
        public bool Keyboard1KeyC;
        public bool Keyboard1KeyV;
        public bool Keyboard1KeyB;
        public bool Keyboard1KeyN;
        public bool Keyboard1KeyM;
        public bool Keyboard1KeyComma;
        public bool Keyboard1KeyPeriod;
        public bool Keyboard1KeySlash;
        public bool Keyboard1KeyRightShift;
        public bool Keyboard1KeyMultiply;
        public bool Keyboard1KeyLeftAlt;
        public bool Keyboard1KeySpace;
        public bool Keyboard1KeyCapital;
        public bool Keyboard1KeyF1;
        public bool Keyboard1KeyF2;
        public bool Keyboard1KeyF3;
        public bool Keyboard1KeyF4;
        public bool Keyboard1KeyF5;
        public bool Keyboard1KeyF6;
        public bool Keyboard1KeyF7;
        public bool Keyboard1KeyF8;
        public bool Keyboard1KeyF9;
        public bool Keyboard1KeyF10;
        public bool Keyboard1KeyNumberLock;
        public bool Keyboard1KeyScrollLock;
        public bool Keyboard1KeyNumberPad7;
        public bool Keyboard1KeyNumberPad8;
        public bool Keyboard1KeyNumberPad9;
        public bool Keyboard1KeySubtract;
        public bool Keyboard1KeyNumberPad4;
        public bool Keyboard1KeyNumberPad5;
        public bool Keyboard1KeyNumberPad6;
        public bool Keyboard1KeyAdd;
        public bool Keyboard1KeyNumberPad1;
        public bool Keyboard1KeyNumberPad2;
        public bool Keyboard1KeyNumberPad3;
        public bool Keyboard1KeyNumberPad0;
        public bool Keyboard1KeyDecimal;
        public bool Keyboard1KeyOem102;
        public bool Keyboard1KeyF11;
        public bool Keyboard1KeyF12;
        public bool Keyboard1KeyF13;
        public bool Keyboard1KeyF14;
        public bool Keyboard1KeyF15;
        public bool Keyboard1KeyKana;
        public bool Keyboard1KeyAbntC1;
        public bool Keyboard1KeyConvert;
        public bool Keyboard1KeyNoConvert;
        public bool Keyboard1KeyYen;
        public bool Keyboard1KeyAbntC2;
        public bool Keyboard1KeyNumberPadEquals;
        public bool Keyboard1KeyPreviousTrack;
        public bool Keyboard1KeyAT;
        public bool Keyboard1KeyColon;
        public bool Keyboard1KeyUnderline;
        public bool Keyboard1KeyKanji;
        public bool Keyboard1KeyStop;
        public bool Keyboard1KeyAX;
        public bool Keyboard1KeyUnlabeled;
        public bool Keyboard1KeyNextTrack;
        public bool Keyboard1KeyNumberPadEnter;
        public bool Keyboard1KeyRightControl;
        public bool Keyboard1KeyMute;
        public bool Keyboard1KeyCalculator;
        public bool Keyboard1KeyPlayPause;
        public bool Keyboard1KeyMediaStop;
        public bool Keyboard1KeyVolumeDown;
        public bool Keyboard1KeyVolumeUp;
        public bool Keyboard1KeyWebHome;
        public bool Keyboard1KeyNumberPadComma;
        public bool Keyboard1KeyDivide;
        public bool Keyboard1KeyPrintScreen;
        public bool Keyboard1KeyRightAlt;
        public bool Keyboard1KeyPause;
        public bool Keyboard1KeyHome;
        public bool Keyboard1KeyUp;
        public bool Keyboard1KeyPageUp;
        public bool Keyboard1KeyLeft;
        public bool Keyboard1KeyRight;
        public bool Keyboard1KeyEnd;
        public bool Keyboard1KeyDown;
        public bool Keyboard1KeyPageDown;
        public bool Keyboard1KeyInsert;
        public bool Keyboard1KeyDelete;
        public bool Keyboard1KeyLeftWindowsKey;
        public bool Keyboard1KeyRightWindowsKey;
        public bool Keyboard1KeyApplications;
        public bool Keyboard1KeyPower;
        public bool Keyboard1KeySleep;
        public bool Keyboard1KeyWake;
        public bool Keyboard1KeyWebSearch;
        public bool Keyboard1KeyWebFavorites;
        public bool Keyboard1KeyWebRefresh;
        public bool Keyboard1KeyWebStop;
        public bool Keyboard1KeyWebForward;
        public bool Keyboard1KeyWebBack;
        public bool Keyboard1KeyMyComputer;
        public bool Keyboard1KeyMail;
        public bool Keyboard1KeyMediaSelect;
        public bool Keyboard1KeyUnknown;
        public static bool Keyboard2KeyEscape;
        public static bool Keyboard2KeyD1;
        public static bool Keyboard2KeyD2;
        public static bool Keyboard2KeyD3;
        public static bool Keyboard2KeyD4;
        public static bool Keyboard2KeyD5;
        public static bool Keyboard2KeyD6;
        public static bool Keyboard2KeyD7;
        public static bool Keyboard2KeyD8;
        public static bool Keyboard2KeyD9;
        public static bool Keyboard2KeyD0;
        public static bool Keyboard2KeyMinus;
        public static bool Keyboard2KeyEquals;
        public static bool Keyboard2KeyBack;
        public static bool Keyboard2KeyTab;
        public static bool Keyboard2KeyQ;
        public static bool Keyboard2KeyW;
        public static bool Keyboard2KeyE;
        public static bool Keyboard2KeyR;
        public static bool Keyboard2KeyT;
        public static bool Keyboard2KeyY;
        public static bool Keyboard2KeyU;
        public static bool Keyboard2KeyI;
        public static bool Keyboard2KeyO;
        public static bool Keyboard2KeyP;
        public static bool Keyboard2KeyLeftBracket;
        public static bool Keyboard2KeyRightBracket;
        public static bool Keyboard2KeyReturn;
        public static bool Keyboard2KeyLeftControl;
        public static bool Keyboard2KeyA;
        public static bool Keyboard2KeyS;
        public static bool Keyboard2KeyD;
        public static bool Keyboard2KeyF;
        public static bool Keyboard2KeyG;
        public static bool Keyboard2KeyH;
        public static bool Keyboard2KeyJ;
        public static bool Keyboard2KeyK;
        public static bool Keyboard2KeyL;
        public static bool Keyboard2KeySemicolon;
        public static bool Keyboard2KeyApostrophe;
        public static bool Keyboard2KeyGrave;
        public static bool Keyboard2KeyLeftShift;
        public static bool Keyboard2KeyBackslash;
        public static bool Keyboard2KeyZ;
        public static bool Keyboard2KeyX;
        public static bool Keyboard2KeyC;
        public static bool Keyboard2KeyV;
        public static bool Keyboard2KeyB;
        public static bool Keyboard2KeyN;
        public static bool Keyboard2KeyM;
        public static bool Keyboard2KeyComma;
        public static bool Keyboard2KeyPeriod;
        public static bool Keyboard2KeySlash;
        public static bool Keyboard2KeyRightShift;
        public static bool Keyboard2KeyMultiply;
        public static bool Keyboard2KeyLeftAlt;
        public static bool Keyboard2KeySpace;
        public static bool Keyboard2KeyCapital;
        public static bool Keyboard2KeyF1;
        public static bool Keyboard2KeyF2;
        public static bool Keyboard2KeyF3;
        public static bool Keyboard2KeyF4;
        public static bool Keyboard2KeyF5;
        public static bool Keyboard2KeyF6;
        public static bool Keyboard2KeyF7;
        public static bool Keyboard2KeyF8;
        public static bool Keyboard2KeyF9;
        public static bool Keyboard2KeyF10;
        public static bool Keyboard2KeyNumberLock;
        public static bool Keyboard2KeyScrollLock;
        public static bool Keyboard2KeyNumberPad7;
        public static bool Keyboard2KeyNumberPad8;
        public static bool Keyboard2KeyNumberPad9;
        public static bool Keyboard2KeySubtract;
        public static bool Keyboard2KeyNumberPad4;
        public static bool Keyboard2KeyNumberPad5;
        public static bool Keyboard2KeyNumberPad6;
        public static bool Keyboard2KeyAdd;
        public static bool Keyboard2KeyNumberPad1;
        public static bool Keyboard2KeyNumberPad2;
        public static bool Keyboard2KeyNumberPad3;
        public static bool Keyboard2KeyNumberPad0;
        public static bool Keyboard2KeyDecimal;
        public static bool Keyboard2KeyOem102;
        public static bool Keyboard2KeyF11;
        public static bool Keyboard2KeyF12;
        public static bool Keyboard2KeyF13;
        public static bool Keyboard2KeyF14;
        public static bool Keyboard2KeyF15;
        public static bool Keyboard2KeyKana;
        public static bool Keyboard2KeyAbntC1;
        public static bool Keyboard2KeyConvert;
        public static bool Keyboard2KeyNoConvert;
        public static bool Keyboard2KeyYen;
        public static bool Keyboard2KeyAbntC2;
        public static bool Keyboard2KeyNumberPadEquals;
        public static bool Keyboard2KeyPreviousTrack;
        public static bool Keyboard2KeyAT;
        public static bool Keyboard2KeyColon;
        public static bool Keyboard2KeyUnderline;
        public static bool Keyboard2KeyKanji;
        public static bool Keyboard2KeyStop;
        public static bool Keyboard2KeyAX;
        public static bool Keyboard2KeyUnlabeled;
        public static bool Keyboard2KeyNextTrack;
        public static bool Keyboard2KeyNumberPadEnter;
        public static bool Keyboard2KeyRightControl;
        public static bool Keyboard2KeyMute;
        public static bool Keyboard2KeyCalculator;
        public static bool Keyboard2KeyPlayPause;
        public static bool Keyboard2KeyMediaStop;
        public static bool Keyboard2KeyVolumeDown;
        public static bool Keyboard2KeyVolumeUp;
        public static bool Keyboard2KeyWebHome;
        public static bool Keyboard2KeyNumberPadComma;
        public static bool Keyboard2KeyDivide;
        public static bool Keyboard2KeyPrintScreen;
        public static bool Keyboard2KeyRightAlt;
        public static bool Keyboard2KeyPause;
        public static bool Keyboard2KeyHome;
        public static bool Keyboard2KeyUp;
        public static bool Keyboard2KeyPageUp;
        public static bool Keyboard2KeyLeft;
        public static bool Keyboard2KeyRight;
        public static bool Keyboard2KeyEnd;
        public static bool Keyboard2KeyDown;
        public static bool Keyboard2KeyPageDown;
        public static bool Keyboard2KeyInsert;
        public static bool Keyboard2KeyDelete;
        public static bool Keyboard2KeyLeftWindowsKey;
        public static bool Keyboard2KeyRightWindowsKey;
        public static bool Keyboard2KeyApplications;
        public static bool Keyboard2KeyPower;
        public static bool Keyboard2KeySleep;
        public static bool Keyboard2KeyWake;
        public static bool Keyboard2KeyWebSearch;
        public static bool Keyboard2KeyWebFavorites;
        public static bool Keyboard2KeyWebRefresh;
        public static bool Keyboard2KeyWebStop;
        public static bool Keyboard2KeyWebForward;
        public static bool Keyboard2KeyWebBack;
        public static bool Keyboard2KeyMyComputer;
        public static bool Keyboard2KeyMail;
        public static bool Keyboard2KeyMediaSelect;
        public static bool Keyboard2KeyUnknown;
        public bool ScanKeyboard()
        {
            try
            {
                directInput = new DirectInput();
                keyboard = new Keyboard[] { null, null };
                keyboardGuid = new Guid[] { Guid.Empty, Guid.Empty };
                knum = 0;
                foreach (var deviceInstance in directInput.GetDevices(SharpDX.DirectInput.DeviceType.Keyboard, DeviceEnumerationFlags.AllDevices))
                {
                    keyboardGuid[knum] = deviceInstance.InstanceGuid;
                    knum++;
                    if (knum >= 2)
                        break;
                }
            }
            catch { }
            if (keyboardGuid[0] == Guid.Empty)
            {
                return false;
            }
            else
            {
                for (int inc = 0; inc < knum; inc++)
                {
                    keyboard[inc] = new Keyboard(directInput);
                    keyboard[inc].Properties.BufferSize = 128;
                    keyboard[inc].Acquire();
                }
                return true;
            }
        }
        public void KeyboardInputProcess()
        {
            for (int inc = 0; inc < knum; inc++)
            {
                keyboard[inc].Poll();
                var datas = keyboard[inc].GetBufferedData();
                foreach (var state in datas)
                {
                    if (inc == 0 & state.IsPressed & state.Key == Key.Escape)
                        Keyboard1KeyEscape = true;
                    if (inc == 0 & state.IsReleased & state.Key == Key.Escape)
                        Keyboard1KeyEscape = false;
                    if (inc == 0 & state.IsPressed & state.Key == Key.D1)
                        Keyboard1KeyD1 = true;
                    if (inc == 0 & state.IsReleased & state.Key == Key.D1)
                        Keyboard1KeyD1 = false;
                    if (inc == 0 & state.IsPressed & state.Key == Key.D2)
                        Keyboard1KeyD2 = true;
                    if (inc == 0 & state.IsReleased & state.Key == Key.D2)
                        Keyboard1KeyD2 = false;
                    if (inc == 0 & state.IsPressed & state.Key == Key.D3)
                        Keyboard1KeyD3 = true;
                    if (inc == 0 & state.IsReleased & state.Key == Key.D3)
                        Keyboard1KeyD3 = false;
                    if (inc == 0 & state.IsPressed & state.Key == Key.D4)
                        Keyboard1KeyD4 = true;
                    if (inc == 0 & state.IsReleased & state.Key == Key.D4)
                        Keyboard1KeyD4 = false;
                    if (inc == 0 & state.IsPressed & state.Key == Key.D5)
                        Keyboard1KeyD5 = true;
                    if (inc == 0 & state.IsReleased & state.Key == Key.D5)
                        Keyboard1KeyD5 = false;
                    if (inc == 0 & state.IsPressed & state.Key == Key.D6)
                        Keyboard1KeyD6 = true;
                    if (inc == 0 & state.IsReleased & state.Key == Key.D6)
                        Keyboard1KeyD6 = false;
                    if (inc == 0 & state.IsPressed & state.Key == Key.D7)
                        Keyboard1KeyD7 = true;
                    if (inc == 0 & state.IsReleased & state.Key == Key.D7)
                        Keyboard1KeyD7 = false;
                    if (inc == 0 & state.IsPressed & state.Key == Key.D8)
                        Keyboard1KeyD8 = true;
                    if (inc == 0 & state.IsReleased & state.Key == Key.D8)
                        Keyboard1KeyD8 = false;
                    if (inc == 0 & state.IsPressed & state.Key == Key.D9)
                        Keyboard1KeyD9 = true;
                    if (inc == 0 & state.IsReleased & state.Key == Key.D9)
                        Keyboard1KeyD9 = false;
                    if (inc == 0 & state.IsPressed & state.Key == Key.D0)
                        Keyboard1KeyD0 = true;
                    if (inc == 0 & state.IsReleased & state.Key == Key.D0)
                        Keyboard1KeyD0 = false;
                    if (inc == 0 & state.IsPressed & state.Key == Key.Minus)
                        Keyboard1KeyMinus = true;
                    if (inc == 0 & state.IsReleased & state.Key == Key.Minus)
                        Keyboard1KeyMinus = false;
                    if (inc == 0 & state.IsPressed & state.Key == Key.Equals)
                        Keyboard1KeyEquals = true;
                    if (inc == 0 & state.IsReleased & state.Key == Key.Equals)
                        Keyboard1KeyEquals = false;
                    if (inc == 0 & state.IsPressed & state.Key == Key.Back)
                        Keyboard1KeyBack = true;
                    if (inc == 0 & state.IsReleased & state.Key == Key.Back)
                        Keyboard1KeyBack = false;
                    if (inc == 0 & state.IsPressed & state.Key == Key.Tab)
                        Keyboard1KeyTab = true;
                    if (inc == 0 & state.IsReleased & state.Key == Key.Tab)
                        Keyboard1KeyTab = false;
                    if (inc == 0 & state.IsPressed & state.Key == Key.Q)
                        Keyboard1KeyQ = true;
                    if (inc == 0 & state.IsReleased & state.Key == Key.Q)
                        Keyboard1KeyQ = false;
                    if (inc == 0 & state.IsPressed & state.Key == Key.W)
                        Keyboard1KeyW = true;
                    if (inc == 0 & state.IsReleased & state.Key == Key.W)
                        Keyboard1KeyW = false;
                    if (inc == 0 & state.IsPressed & state.Key == Key.E)
                        Keyboard1KeyE = true;
                    if (inc == 0 & state.IsReleased & state.Key == Key.E)
                        Keyboard1KeyE = false;
                    if (inc == 0 & state.IsPressed & state.Key == Key.R)
                        Keyboard1KeyR = true;
                    if (inc == 0 & state.IsReleased & state.Key == Key.R)
                        Keyboard1KeyR = false;
                    if (inc == 0 & state.IsPressed & state.Key == Key.T)
                        Keyboard1KeyT = true;
                    if (inc == 0 & state.IsReleased & state.Key == Key.T)
                        Keyboard1KeyT = false;
                    if (inc == 0 & state.IsPressed & state.Key == Key.Y)
                        Keyboard1KeyY = true;
                    if (inc == 0 & state.IsReleased & state.Key == Key.Y)
                        Keyboard1KeyY = false;
                    if (inc == 0 & state.IsPressed & state.Key == Key.U)
                        Keyboard1KeyU = true;
                    if (inc == 0 & state.IsReleased & state.Key == Key.U)
                        Keyboard1KeyU = false;
                    if (inc == 0 & state.IsPressed & state.Key == Key.I)
                        Keyboard1KeyI = true;
                    if (inc == 0 & state.IsReleased & state.Key == Key.I)
                        Keyboard1KeyI = false;
                    if (inc == 0 & state.IsPressed & state.Key == Key.O)
                        Keyboard1KeyO = true;
                    if (inc == 0 & state.IsReleased & state.Key == Key.O)
                        Keyboard1KeyO = false;
                    if (inc == 0 & state.IsPressed & state.Key == Key.P)
                        Keyboard1KeyP = true;
                    if (inc == 0 & state.IsReleased & state.Key == Key.P)
                        Keyboard1KeyP = false;
                    if (inc == 0 & state.IsPressed & state.Key == Key.LeftBracket)
                        Keyboard1KeyLeftBracket = true;
                    if (inc == 0 & state.IsReleased & state.Key == Key.LeftBracket)
                        Keyboard1KeyLeftBracket = false;
                    if (inc == 0 & state.IsPressed & state.Key == Key.RightBracket)
                        Keyboard1KeyRightBracket = true;
                    if (inc == 0 & state.IsReleased & state.Key == Key.RightBracket)
                        Keyboard1KeyRightBracket = false;
                    if (inc == 0 & state.IsPressed & state.Key == Key.Return)
                        Keyboard1KeyReturn = true;
                    if (inc == 0 & state.IsReleased & state.Key == Key.Return)
                        Keyboard1KeyReturn = false;
                    if (inc == 0 & state.IsPressed & state.Key == Key.LeftControl)
                        Keyboard1KeyLeftControl = true;
                    if (inc == 0 & state.IsReleased & state.Key == Key.LeftControl)
                        Keyboard1KeyLeftControl = false;
                    if (inc == 0 & state.IsPressed & state.Key == Key.A)
                        Keyboard1KeyA = true;
                    if (inc == 0 & state.IsReleased & state.Key == Key.A)
                        Keyboard1KeyA = false;
                    if (inc == 0 & state.IsPressed & state.Key == Key.S)
                        Keyboard1KeyS = true;
                    if (inc == 0 & state.IsReleased & state.Key == Key.S)
                        Keyboard1KeyS = false;
                    if (inc == 0 & state.IsPressed & state.Key == Key.D)
                        Keyboard1KeyD = true;
                    if (inc == 0 & state.IsReleased & state.Key == Key.D)
                        Keyboard1KeyD = false;
                    if (inc == 0 & state.IsPressed & state.Key == Key.F)
                        Keyboard1KeyF = true;
                    if (inc == 0 & state.IsReleased & state.Key == Key.F)
                        Keyboard1KeyF = false;
                    if (inc == 0 & state.IsPressed & state.Key == Key.G)
                        Keyboard1KeyG = true;
                    if (inc == 0 & state.IsReleased & state.Key == Key.G)
                        Keyboard1KeyG = false;
                    if (inc == 0 & state.IsPressed & state.Key == Key.H)
                        Keyboard1KeyH = true;
                    if (inc == 0 & state.IsReleased & state.Key == Key.H)
                        Keyboard1KeyH = false;
                    if (inc == 0 & state.IsPressed & state.Key == Key.J)
                        Keyboard1KeyJ = true;
                    if (inc == 0 & state.IsReleased & state.Key == Key.J)
                        Keyboard1KeyJ = false;
                    if (inc == 0 & state.IsPressed & state.Key == Key.K)
                        Keyboard1KeyK = true;
                    if (inc == 0 & state.IsReleased & state.Key == Key.K)
                        Keyboard1KeyK = false;
                    if (inc == 0 & state.IsPressed & state.Key == Key.L)
                        Keyboard1KeyL = true;
                    if (inc == 0 & state.IsReleased & state.Key == Key.L)
                        Keyboard1KeyL = false;
                    if (inc == 0 & state.IsPressed & state.Key == Key.Semicolon)
                        Keyboard1KeySemicolon = true;
                    if (inc == 0 & state.IsReleased & state.Key == Key.Semicolon)
                        Keyboard1KeySemicolon = false;
                    if (inc == 0 & state.IsPressed & state.Key == Key.Apostrophe)
                        Keyboard1KeyApostrophe = true;
                    if (inc == 0 & state.IsReleased & state.Key == Key.Apostrophe)
                        Keyboard1KeyApostrophe = false;
                    if (inc == 0 & state.IsPressed & state.Key == Key.Grave)
                        Keyboard1KeyGrave = true;
                    if (inc == 0 & state.IsReleased & state.Key == Key.Grave)
                        Keyboard1KeyGrave = false;
                    if (inc == 0 & state.IsPressed & state.Key == Key.LeftShift)
                        Keyboard1KeyLeftShift = true;
                    if (inc == 0 & state.IsReleased & state.Key == Key.LeftShift)
                        Keyboard1KeyLeftShift = false;
                    if (inc == 0 & state.IsPressed & state.Key == Key.Backslash)
                        Keyboard1KeyBackslash = true;
                    if (inc == 0 & state.IsReleased & state.Key == Key.Backslash)
                        Keyboard1KeyBackslash = false;
                    if (inc == 0 & state.IsPressed & state.Key == Key.Z)
                        Keyboard1KeyZ = true;
                    if (inc == 0 & state.IsReleased & state.Key == Key.Z)
                        Keyboard1KeyZ = false;
                    if (inc == 0 & state.IsPressed & state.Key == Key.X)
                        Keyboard1KeyX = true;
                    if (inc == 0 & state.IsReleased & state.Key == Key.X)
                        Keyboard1KeyX = false;
                    if (inc == 0 & state.IsPressed & state.Key == Key.C)
                        Keyboard1KeyC = true;
                    if (inc == 0 & state.IsReleased & state.Key == Key.C)
                        Keyboard1KeyC = false;
                    if (inc == 0 & state.IsPressed & state.Key == Key.V)
                        Keyboard1KeyV = true;
                    if (inc == 0 & state.IsReleased & state.Key == Key.V)
                        Keyboard1KeyV = false;
                    if (inc == 0 & state.IsPressed & state.Key == Key.B)
                        Keyboard1KeyB = true;
                    if (inc == 0 & state.IsReleased & state.Key == Key.B)
                        Keyboard1KeyB = false;
                    if (inc == 0 & state.IsPressed & state.Key == Key.N)
                        Keyboard1KeyN = true;
                    if (inc == 0 & state.IsReleased & state.Key == Key.N)
                        Keyboard1KeyN = false;
                    if (inc == 0 & state.IsPressed & state.Key == Key.M)
                        Keyboard1KeyM = true;
                    if (inc == 0 & state.IsReleased & state.Key == Key.M)
                        Keyboard1KeyM = false;
                    if (inc == 0 & state.IsPressed & state.Key == Key.Comma)
                        Keyboard1KeyComma = true;
                    if (inc == 0 & state.IsReleased & state.Key == Key.Comma)
                        Keyboard1KeyComma = false;
                    if (inc == 0 & state.IsPressed & state.Key == Key.Period)
                        Keyboard1KeyPeriod = true;
                    if (inc == 0 & state.IsReleased & state.Key == Key.Period)
                        Keyboard1KeyPeriod = false;
                    if (inc == 0 & state.IsPressed & state.Key == Key.Slash)
                        Keyboard1KeySlash = true;
                    if (inc == 0 & state.IsReleased & state.Key == Key.Slash)
                        Keyboard1KeySlash = false;
                    if (inc == 0 & state.IsPressed & state.Key == Key.RightShift)
                        Keyboard1KeyRightShift = true;
                    if (inc == 0 & state.IsReleased & state.Key == Key.RightShift)
                        Keyboard1KeyRightShift = false;
                    if (inc == 0 & state.IsPressed & state.Key == Key.Multiply)
                        Keyboard1KeyMultiply = true;
                    if (inc == 0 & state.IsReleased & state.Key == Key.Multiply)
                        Keyboard1KeyMultiply = false;
                    if (inc == 0 & state.IsPressed & state.Key == Key.LeftAlt)
                        Keyboard1KeyLeftAlt = true;
                    if (inc == 0 & state.IsReleased & state.Key == Key.LeftAlt)
                        Keyboard1KeyLeftAlt = false;
                    if (inc == 0 & state.IsPressed & state.Key == Key.Space)
                        Keyboard1KeySpace = true;
                    if (inc == 0 & state.IsReleased & state.Key == Key.Space)
                        Keyboard1KeySpace = false;
                    if (inc == 0 & state.IsPressed & state.Key == Key.Capital)
                        Keyboard1KeyCapital = true;
                    if (inc == 0 & state.IsReleased & state.Key == Key.Capital)
                        Keyboard1KeyCapital = false;
                    if (inc == 0 & state.IsPressed & state.Key == Key.F1)
                        Keyboard1KeyF1 = true;
                    if (inc == 0 & state.IsReleased & state.Key == Key.F1)
                        Keyboard1KeyF1 = false;
                    if (inc == 0 & state.IsPressed & state.Key == Key.F2)
                        Keyboard1KeyF2 = true;
                    if (inc == 0 & state.IsReleased & state.Key == Key.F2)
                        Keyboard1KeyF2 = false;
                    if (inc == 0 & state.IsPressed & state.Key == Key.F3)
                        Keyboard1KeyF3 = true;
                    if (inc == 0 & state.IsReleased & state.Key == Key.F3)
                        Keyboard1KeyF3 = false;
                    if (inc == 0 & state.IsPressed & state.Key == Key.F4)
                        Keyboard1KeyF4 = true;
                    if (inc == 0 & state.IsReleased & state.Key == Key.F4)
                        Keyboard1KeyF4 = false;
                    if (inc == 0 & state.IsPressed & state.Key == Key.F5)
                        Keyboard1KeyF5 = true;
                    if (inc == 0 & state.IsReleased & state.Key == Key.F5)
                        Keyboard1KeyF5 = false;
                    if (inc == 0 & state.IsPressed & state.Key == Key.F6)
                        Keyboard1KeyF6 = true;
                    if (inc == 0 & state.IsReleased & state.Key == Key.F6)
                        Keyboard1KeyF6 = false;
                    if (inc == 0 & state.IsPressed & state.Key == Key.F7)
                        Keyboard1KeyF7 = true;
                    if (inc == 0 & state.IsReleased & state.Key == Key.F7)
                        Keyboard1KeyF7 = false;
                    if (inc == 0 & state.IsPressed & state.Key == Key.F8)
                        Keyboard1KeyF8 = true;
                    if (inc == 0 & state.IsReleased & state.Key == Key.F8)
                        Keyboard1KeyF8 = false;
                    if (inc == 0 & state.IsPressed & state.Key == Key.F9)
                        Keyboard1KeyF9 = true;
                    if (inc == 0 & state.IsReleased & state.Key == Key.F9)
                        Keyboard1KeyF9 = false;
                    if (inc == 0 & state.IsPressed & state.Key == Key.F10)
                        Keyboard1KeyF10 = true;
                    if (inc == 0 & state.IsReleased & state.Key == Key.F10)
                        Keyboard1KeyF10 = false;
                    if (inc == 0 & state.IsPressed & state.Key == Key.NumberLock)
                        Keyboard1KeyNumberLock = true;
                    if (inc == 0 & state.IsReleased & state.Key == Key.NumberLock)
                        Keyboard1KeyNumberLock = false;
                    if (inc == 0 & state.IsPressed & state.Key == Key.ScrollLock)
                        Keyboard1KeyScrollLock = true;
                    if (inc == 0 & state.IsReleased & state.Key == Key.ScrollLock)
                        Keyboard1KeyScrollLock = false;
                    if (inc == 0 & state.IsPressed & state.Key == Key.NumberPad7)
                        Keyboard1KeyNumberPad7 = true;
                    if (inc == 0 & state.IsReleased & state.Key == Key.NumberPad7)
                        Keyboard1KeyNumberPad7 = false;
                    if (inc == 0 & state.IsPressed & state.Key == Key.NumberPad8)
                        Keyboard1KeyNumberPad8 = true;
                    if (inc == 0 & state.IsReleased & state.Key == Key.NumberPad8)
                        Keyboard1KeyNumberPad8 = false;
                    if (inc == 0 & state.IsPressed & state.Key == Key.NumberPad9)
                        Keyboard1KeyNumberPad9 = true;
                    if (inc == 0 & state.IsReleased & state.Key == Key.NumberPad9)
                        Keyboard1KeyNumberPad9 = false;
                    if (inc == 0 & state.IsPressed & state.Key == Key.Subtract)
                        Keyboard1KeySubtract = true;
                    if (inc == 0 & state.IsReleased & state.Key == Key.Subtract)
                        Keyboard1KeySubtract = false;
                    if (inc == 0 & state.IsPressed & state.Key == Key.NumberPad4)
                        Keyboard1KeyNumberPad4 = true;
                    if (inc == 0 & state.IsReleased & state.Key == Key.NumberPad4)
                        Keyboard1KeyNumberPad4 = false;
                    if (inc == 0 & state.IsPressed & state.Key == Key.NumberPad5)
                        Keyboard1KeyNumberPad5 = true;
                    if (inc == 0 & state.IsReleased & state.Key == Key.NumberPad5)
                        Keyboard1KeyNumberPad5 = false;
                    if (inc == 0 & state.IsPressed & state.Key == Key.NumberPad6)
                        Keyboard1KeyNumberPad6 = true;
                    if (inc == 0 & state.IsReleased & state.Key == Key.NumberPad6)
                        Keyboard1KeyNumberPad6 = false;
                    if (inc == 0 & state.IsPressed & state.Key == Key.Add)
                        Keyboard1KeyAdd = true;
                    if (inc == 0 & state.IsReleased & state.Key == Key.Add)
                        Keyboard1KeyAdd = false;
                    if (inc == 0 & state.IsPressed & state.Key == Key.NumberPad1)
                        Keyboard1KeyNumberPad1 = true;
                    if (inc == 0 & state.IsReleased & state.Key == Key.NumberPad1)
                        Keyboard1KeyNumberPad1 = false;
                    if (inc == 0 & state.IsPressed & state.Key == Key.NumberPad2)
                        Keyboard1KeyNumberPad2 = true;
                    if (inc == 0 & state.IsReleased & state.Key == Key.NumberPad2)
                        Keyboard1KeyNumberPad2 = false;
                    if (inc == 0 & state.IsPressed & state.Key == Key.NumberPad3)
                        Keyboard1KeyNumberPad3 = true;
                    if (inc == 0 & state.IsReleased & state.Key == Key.NumberPad3)
                        Keyboard1KeyNumberPad3 = false;
                    if (inc == 0 & state.IsPressed & state.Key == Key.NumberPad0)
                        Keyboard1KeyNumberPad0 = true;
                    if (inc == 0 & state.IsReleased & state.Key == Key.NumberPad0)
                        Keyboard1KeyNumberPad0 = false;
                    if (inc == 0 & state.IsPressed & state.Key == Key.Decimal)
                        Keyboard1KeyDecimal = true;
                    if (inc == 0 & state.IsReleased & state.Key == Key.Decimal)
                        Keyboard1KeyDecimal = false;
                    if (inc == 0 & state.IsPressed & state.Key == Key.Oem102)
                        Keyboard1KeyOem102 = true;
                    if (inc == 0 & state.IsReleased & state.Key == Key.Oem102)
                        Keyboard1KeyOem102 = false;
                    if (inc == 0 & state.IsPressed & state.Key == Key.F11)
                        Keyboard1KeyF11 = true;
                    if (inc == 0 & state.IsReleased & state.Key == Key.F11)
                        Keyboard1KeyF11 = false;
                    if (inc == 0 & state.IsPressed & state.Key == Key.F12)
                        Keyboard1KeyF12 = true;
                    if (inc == 0 & state.IsReleased & state.Key == Key.F12)
                        Keyboard1KeyF12 = false;
                    if (inc == 0 & state.IsPressed & state.Key == Key.F13)
                        Keyboard1KeyF13 = true;
                    if (inc == 0 & state.IsReleased & state.Key == Key.F13)
                        Keyboard1KeyF13 = false;
                    if (inc == 0 & state.IsPressed & state.Key == Key.F14)
                        Keyboard1KeyF14 = true;
                    if (inc == 0 & state.IsReleased & state.Key == Key.F14)
                        Keyboard1KeyF14 = false;
                    if (inc == 0 & state.IsPressed & state.Key == Key.F15)
                        Keyboard1KeyF15 = true;
                    if (inc == 0 & state.IsReleased & state.Key == Key.F15)
                        Keyboard1KeyF15 = false;
                    if (inc == 0 & state.IsPressed & state.Key == Key.Kana)
                        Keyboard1KeyKana = true;
                    if (inc == 0 & state.IsReleased & state.Key == Key.Kana)
                        Keyboard1KeyKana = false;
                    if (inc == 0 & state.IsPressed & state.Key == Key.AbntC1)
                        Keyboard1KeyAbntC1 = true;
                    if (inc == 0 & state.IsReleased & state.Key == Key.AbntC1)
                        Keyboard1KeyAbntC1 = false;
                    if (inc == 0 & state.IsPressed & state.Key == Key.Convert)
                        Keyboard1KeyConvert = true;
                    if (inc == 0 & state.IsReleased & state.Key == Key.Convert)
                        Keyboard1KeyConvert = false;
                    if (inc == 0 & state.IsPressed & state.Key == Key.NoConvert)
                        Keyboard1KeyNoConvert = true;
                    if (inc == 0 & state.IsReleased & state.Key == Key.NoConvert)
                        Keyboard1KeyNoConvert = false;
                    if (inc == 0 & state.IsPressed & state.Key == Key.Yen)
                        Keyboard1KeyYen = true;
                    if (inc == 0 & state.IsReleased & state.Key == Key.Yen)
                        Keyboard1KeyYen = false;
                    if (inc == 0 & state.IsPressed & state.Key == Key.AbntC2)
                        Keyboard1KeyAbntC2 = true;
                    if (inc == 0 & state.IsReleased & state.Key == Key.AbntC2)
                        Keyboard1KeyAbntC2 = false;
                    if (inc == 0 & state.IsPressed & state.Key == Key.NumberPadEquals)
                        Keyboard1KeyNumberPadEquals = true;
                    if (inc == 0 & state.IsReleased & state.Key == Key.NumberPadEquals)
                        Keyboard1KeyNumberPadEquals = false;
                    if (inc == 0 & state.IsPressed & state.Key == Key.PreviousTrack)
                        Keyboard1KeyPreviousTrack = true;
                    if (inc == 0 & state.IsReleased & state.Key == Key.PreviousTrack)
                        Keyboard1KeyPreviousTrack = false;
                    if (inc == 0 & state.IsPressed & state.Key == Key.AT)
                        Keyboard1KeyAT = true;
                    if (inc == 0 & state.IsReleased & state.Key == Key.AT)
                        Keyboard1KeyAT = false;
                    if (inc == 0 & state.IsPressed & state.Key == Key.Colon)
                        Keyboard1KeyColon = true;
                    if (inc == 0 & state.IsReleased & state.Key == Key.Colon)
                        Keyboard1KeyColon = false;
                    if (inc == 0 & state.IsPressed & state.Key == Key.Underline)
                        Keyboard1KeyUnderline = true;
                    if (inc == 0 & state.IsReleased & state.Key == Key.Underline)
                        Keyboard1KeyUnderline = false;
                    if (inc == 0 & state.IsPressed & state.Key == Key.Kanji)
                        Keyboard1KeyKanji = true;
                    if (inc == 0 & state.IsReleased & state.Key == Key.Kanji)
                        Keyboard1KeyKanji = false;
                    if (inc == 0 & state.IsPressed & state.Key == Key.Stop)
                        Keyboard1KeyStop = true;
                    if (inc == 0 & state.IsReleased & state.Key == Key.Stop)
                        Keyboard1KeyStop = false;
                    if (inc == 0 & state.IsPressed & state.Key == Key.AX)
                        Keyboard1KeyAX = true;
                    if (inc == 0 & state.IsReleased & state.Key == Key.AX)
                        Keyboard1KeyAX = false;
                    if (inc == 0 & state.IsPressed & state.Key == Key.Unlabeled)
                        Keyboard1KeyUnlabeled = true;
                    if (inc == 0 & state.IsReleased & state.Key == Key.Unlabeled)
                        Keyboard1KeyUnlabeled = false;
                    if (inc == 0 & state.IsPressed & state.Key == Key.NextTrack)
                        Keyboard1KeyNextTrack = true;
                    if (inc == 0 & state.IsReleased & state.Key == Key.NextTrack)
                        Keyboard1KeyNextTrack = false;
                    if (inc == 0 & state.IsPressed & state.Key == Key.NumberPadEnter)
                        Keyboard1KeyNumberPadEnter = true;
                    if (inc == 0 & state.IsReleased & state.Key == Key.NumberPadEnter)
                        Keyboard1KeyNumberPadEnter = false;
                    if (inc == 0 & state.IsPressed & state.Key == Key.RightControl)
                        Keyboard1KeyRightControl = true;
                    if (inc == 0 & state.IsReleased & state.Key == Key.RightControl)
                        Keyboard1KeyRightControl = false;
                    if (inc == 0 & state.IsPressed & state.Key == Key.Mute)
                        Keyboard1KeyMute = true;
                    if (inc == 0 & state.IsReleased & state.Key == Key.Mute)
                        Keyboard1KeyMute = false;
                    if (inc == 0 & state.IsPressed & state.Key == Key.Calculator)
                        Keyboard1KeyCalculator = true;
                    if (inc == 0 & state.IsReleased & state.Key == Key.Calculator)
                        Keyboard1KeyCalculator = false;
                    if (inc == 0 & state.IsPressed & state.Key == Key.PlayPause)
                        Keyboard1KeyPlayPause = true;
                    if (inc == 0 & state.IsReleased & state.Key == Key.PlayPause)
                        Keyboard1KeyPlayPause = false;
                    if (inc == 0 & state.IsPressed & state.Key == Key.MediaStop)
                        Keyboard1KeyMediaStop = true;
                    if (inc == 0 & state.IsReleased & state.Key == Key.MediaStop)
                        Keyboard1KeyMediaStop = false;
                    if (inc == 0 & state.IsPressed & state.Key == Key.VolumeDown)
                        Keyboard1KeyVolumeDown = true;
                    if (inc == 0 & state.IsReleased & state.Key == Key.VolumeDown)
                        Keyboard1KeyVolumeDown = false;
                    if (inc == 0 & state.IsPressed & state.Key == Key.VolumeUp)
                        Keyboard1KeyVolumeUp = true;
                    if (inc == 0 & state.IsReleased & state.Key == Key.VolumeUp)
                        Keyboard1KeyVolumeUp = false;
                    if (inc == 0 & state.IsPressed & state.Key == Key.WebHome)
                        Keyboard1KeyWebHome = true;
                    if (inc == 0 & state.IsReleased & state.Key == Key.WebHome)
                        Keyboard1KeyWebHome = false;
                    if (inc == 0 & state.IsPressed & state.Key == Key.NumberPadComma)
                        Keyboard1KeyNumberPadComma = true;
                    if (inc == 0 & state.IsReleased & state.Key == Key.NumberPadComma)
                        Keyboard1KeyNumberPadComma = false;
                    if (inc == 0 & state.IsPressed & state.Key == Key.Divide)
                        Keyboard1KeyDivide = true;
                    if (inc == 0 & state.IsReleased & state.Key == Key.Divide)
                        Keyboard1KeyDivide = false;
                    if (inc == 0 & state.IsPressed & state.Key == Key.PrintScreen)
                        Keyboard1KeyPrintScreen = true;
                    if (inc == 0 & state.IsReleased & state.Key == Key.PrintScreen)
                        Keyboard1KeyPrintScreen = false;
                    if (inc == 0 & state.IsPressed & state.Key == Key.RightAlt)
                        Keyboard1KeyRightAlt = true;
                    if (inc == 0 & state.IsReleased & state.Key == Key.RightAlt)
                        Keyboard1KeyRightAlt = false;
                    if (inc == 0 & state.IsPressed & state.Key == Key.Pause)
                        Keyboard1KeyPause = true;
                    if (inc == 0 & state.IsReleased & state.Key == Key.Pause)
                        Keyboard1KeyPause = false;
                    if (inc == 0 & state.IsPressed & state.Key == Key.Home)
                        Keyboard1KeyHome = true;
                    if (inc == 0 & state.IsReleased & state.Key == Key.Home)
                        Keyboard1KeyHome = false;
                    if (inc == 0 & state.IsPressed & state.Key == Key.Up)
                        Keyboard1KeyUp = true;
                    if (inc == 0 & state.IsReleased & state.Key == Key.Up)
                        Keyboard1KeyUp = false;
                    if (inc == 0 & state.IsPressed & state.Key == Key.PageUp)
                        Keyboard1KeyPageUp = true;
                    if (inc == 0 & state.IsReleased & state.Key == Key.PageUp)
                        Keyboard1KeyPageUp = false;
                    if (inc == 0 & state.IsPressed & state.Key == Key.Left)
                        Keyboard1KeyLeft = true;
                    if (inc == 0 & state.IsReleased & state.Key == Key.Left)
                        Keyboard1KeyLeft = false;
                    if (inc == 0 & state.IsPressed & state.Key == Key.Right)
                        Keyboard1KeyRight = true;
                    if (inc == 0 & state.IsReleased & state.Key == Key.Right)
                        Keyboard1KeyRight = false;
                    if (inc == 0 & state.IsPressed & state.Key == Key.End)
                        Keyboard1KeyEnd = true;
                    if (inc == 0 & state.IsReleased & state.Key == Key.End)
                        Keyboard1KeyEnd = false;
                    if (inc == 0 & state.IsPressed & state.Key == Key.Down)
                        Keyboard1KeyDown = true;
                    if (inc == 0 & state.IsReleased & state.Key == Key.Down)
                        Keyboard1KeyDown = false;
                    if (inc == 0 & state.IsPressed & state.Key == Key.PageDown)
                        Keyboard1KeyPageDown = true;
                    if (inc == 0 & state.IsReleased & state.Key == Key.PageDown)
                        Keyboard1KeyPageDown = false;
                    if (inc == 0 & state.IsPressed & state.Key == Key.Insert)
                        Keyboard1KeyInsert = true;
                    if (inc == 0 & state.IsReleased & state.Key == Key.Insert)
                        Keyboard1KeyInsert = false;
                    if (inc == 0 & state.IsPressed & state.Key == Key.Delete)
                        Keyboard1KeyDelete = true;
                    if (inc == 0 & state.IsReleased & state.Key == Key.Delete)
                        Keyboard1KeyDelete = false;
                    if (inc == 0 & state.IsPressed & state.Key == Key.LeftWindowsKey)
                        Keyboard1KeyLeftWindowsKey = true;
                    if (inc == 0 & state.IsReleased & state.Key == Key.LeftWindowsKey)
                        Keyboard1KeyLeftWindowsKey = false;
                    if (inc == 0 & state.IsPressed & state.Key == Key.RightWindowsKey)
                        Keyboard1KeyRightWindowsKey = true;
                    if (inc == 0 & state.IsReleased & state.Key == Key.RightWindowsKey)
                        Keyboard1KeyRightWindowsKey = false;
                    if (inc == 0 & state.IsPressed & state.Key == Key.Applications)
                        Keyboard1KeyApplications = true;
                    if (inc == 0 & state.IsReleased & state.Key == Key.Applications)
                        Keyboard1KeyApplications = false;
                    if (inc == 0 & state.IsPressed & state.Key == Key.Power)
                        Keyboard1KeyPower = true;
                    if (inc == 0 & state.IsReleased & state.Key == Key.Power)
                        Keyboard1KeyPower = false;
                    if (inc == 0 & state.IsPressed & state.Key == Key.Sleep)
                        Keyboard1KeySleep = true;
                    if (inc == 0 & state.IsReleased & state.Key == Key.Sleep)
                        Keyboard1KeySleep = false;
                    if (inc == 0 & state.IsPressed & state.Key == Key.Wake)
                        Keyboard1KeyWake = true;
                    if (inc == 0 & state.IsReleased & state.Key == Key.Wake)
                        Keyboard1KeyWake = false;
                    if (inc == 0 & state.IsPressed & state.Key == Key.WebSearch)
                        Keyboard1KeyWebSearch = true;
                    if (inc == 0 & state.IsReleased & state.Key == Key.WebSearch)
                        Keyboard1KeyWebSearch = false;
                    if (inc == 0 & state.IsPressed & state.Key == Key.WebFavorites)
                        Keyboard1KeyWebFavorites = true;
                    if (inc == 0 & state.IsReleased & state.Key == Key.WebFavorites)
                        Keyboard1KeyWebFavorites = false;
                    if (inc == 0 & state.IsPressed & state.Key == Key.WebRefresh)
                        Keyboard1KeyWebRefresh = true;
                    if (inc == 0 & state.IsReleased & state.Key == Key.WebRefresh)
                        Keyboard1KeyWebRefresh = false;
                    if (inc == 0 & state.IsPressed & state.Key == Key.WebStop)
                        Keyboard1KeyWebStop = true;
                    if (inc == 0 & state.IsReleased & state.Key == Key.WebStop)
                        Keyboard1KeyWebStop = false;
                    if (inc == 0 & state.IsPressed & state.Key == Key.WebForward)
                        Keyboard1KeyWebForward = true;
                    if (inc == 0 & state.IsReleased & state.Key == Key.WebForward)
                        Keyboard1KeyWebForward = false;
                    if (inc == 0 & state.IsPressed & state.Key == Key.WebBack)
                        Keyboard1KeyWebBack = true;
                    if (inc == 0 & state.IsReleased & state.Key == Key.WebBack)
                        Keyboard1KeyWebBack = false;
                    if (inc == 0 & state.IsPressed & state.Key == Key.MyComputer)
                        Keyboard1KeyMyComputer = true;
                    if (inc == 0 & state.IsReleased & state.Key == Key.MyComputer)
                        Keyboard1KeyMyComputer = false;
                    if (inc == 0 & state.IsPressed & state.Key == Key.Mail)
                        Keyboard1KeyMail = true;
                    if (inc == 0 & state.IsReleased & state.Key == Key.Mail)
                        Keyboard1KeyMail = false;
                    if (inc == 0 & state.IsPressed & state.Key == Key.MediaSelect)
                        Keyboard1KeyMediaSelect = true;
                    if (inc == 0 & state.IsReleased & state.Key == Key.MediaSelect)
                        Keyboard1KeyMediaSelect = false;
                    if (inc == 0 & state.IsPressed & state.Key == Key.Unknown)
                        Keyboard1KeyUnknown = true;
                    if (inc == 0 & state.IsReleased & state.Key == Key.Unknown)
                        Keyboard1KeyUnknown = false;
                    if (inc == 1 & state.IsPressed & state.Key == Key.Escape)
                        Keyboard2KeyEscape = true;
                    if (inc == 1 & state.IsReleased & state.Key == Key.Escape)
                        Keyboard2KeyEscape = false;
                    if (inc == 1 & state.IsPressed & state.Key == Key.D1)
                        Keyboard2KeyD1 = true;
                    if (inc == 1 & state.IsReleased & state.Key == Key.D1)
                        Keyboard2KeyD1 = false;
                    if (inc == 1 & state.IsPressed & state.Key == Key.D2)
                        Keyboard2KeyD2 = true;
                    if (inc == 1 & state.IsReleased & state.Key == Key.D2)
                        Keyboard2KeyD2 = false;
                    if (inc == 1 & state.IsPressed & state.Key == Key.D3)
                        Keyboard2KeyD3 = true;
                    if (inc == 1 & state.IsReleased & state.Key == Key.D3)
                        Keyboard2KeyD3 = false;
                    if (inc == 1 & state.IsPressed & state.Key == Key.D4)
                        Keyboard2KeyD4 = true;
                    if (inc == 1 & state.IsReleased & state.Key == Key.D4)
                        Keyboard2KeyD4 = false;
                    if (inc == 1 & state.IsPressed & state.Key == Key.D5)
                        Keyboard2KeyD5 = true;
                    if (inc == 1 & state.IsReleased & state.Key == Key.D5)
                        Keyboard2KeyD5 = false;
                    if (inc == 1 & state.IsPressed & state.Key == Key.D6)
                        Keyboard2KeyD6 = true;
                    if (inc == 1 & state.IsReleased & state.Key == Key.D6)
                        Keyboard2KeyD6 = false;
                    if (inc == 1 & state.IsPressed & state.Key == Key.D7)
                        Keyboard2KeyD7 = true;
                    if (inc == 1 & state.IsReleased & state.Key == Key.D7)
                        Keyboard2KeyD7 = false;
                    if (inc == 1 & state.IsPressed & state.Key == Key.D8)
                        Keyboard2KeyD8 = true;
                    if (inc == 1 & state.IsReleased & state.Key == Key.D8)
                        Keyboard2KeyD8 = false;
                    if (inc == 1 & state.IsPressed & state.Key == Key.D9)
                        Keyboard2KeyD9 = true;
                    if (inc == 1 & state.IsReleased & state.Key == Key.D9)
                        Keyboard2KeyD9 = false;
                    if (inc == 1 & state.IsPressed & state.Key == Key.D0)
                        Keyboard2KeyD0 = true;
                    if (inc == 1 & state.IsReleased & state.Key == Key.D0)
                        Keyboard2KeyD0 = false;
                    if (inc == 1 & state.IsPressed & state.Key == Key.Minus)
                        Keyboard2KeyMinus = true;
                    if (inc == 1 & state.IsReleased & state.Key == Key.Minus)
                        Keyboard2KeyMinus = false;
                    if (inc == 1 & state.IsPressed & state.Key == Key.Equals)
                        Keyboard2KeyEquals = true;
                    if (inc == 1 & state.IsReleased & state.Key == Key.Equals)
                        Keyboard2KeyEquals = false;
                    if (inc == 1 & state.IsPressed & state.Key == Key.Back)
                        Keyboard2KeyBack = true;
                    if (inc == 1 & state.IsReleased & state.Key == Key.Back)
                        Keyboard2KeyBack = false;
                    if (inc == 1 & state.IsPressed & state.Key == Key.Tab)
                        Keyboard2KeyTab = true;
                    if (inc == 1 & state.IsReleased & state.Key == Key.Tab)
                        Keyboard2KeyTab = false;
                    if (inc == 1 & state.IsPressed & state.Key == Key.Q)
                        Keyboard2KeyQ = true;
                    if (inc == 1 & state.IsReleased & state.Key == Key.Q)
                        Keyboard2KeyQ = false;
                    if (inc == 1 & state.IsPressed & state.Key == Key.W)
                        Keyboard2KeyW = true;
                    if (inc == 1 & state.IsReleased & state.Key == Key.W)
                        Keyboard2KeyW = false;
                    if (inc == 1 & state.IsPressed & state.Key == Key.E)
                        Keyboard2KeyE = true;
                    if (inc == 1 & state.IsReleased & state.Key == Key.E)
                        Keyboard2KeyE = false;
                    if (inc == 1 & state.IsPressed & state.Key == Key.R)
                        Keyboard2KeyR = true;
                    if (inc == 1 & state.IsReleased & state.Key == Key.R)
                        Keyboard2KeyR = false;
                    if (inc == 1 & state.IsPressed & state.Key == Key.T)
                        Keyboard2KeyT = true;
                    if (inc == 1 & state.IsReleased & state.Key == Key.T)
                        Keyboard2KeyT = false;
                    if (inc == 1 & state.IsPressed & state.Key == Key.Y)
                        Keyboard2KeyY = true;
                    if (inc == 1 & state.IsReleased & state.Key == Key.Y)
                        Keyboard2KeyY = false;
                    if (inc == 1 & state.IsPressed & state.Key == Key.U)
                        Keyboard2KeyU = true;
                    if (inc == 1 & state.IsReleased & state.Key == Key.U)
                        Keyboard2KeyU = false;
                    if (inc == 1 & state.IsPressed & state.Key == Key.I)
                        Keyboard2KeyI = true;
                    if (inc == 1 & state.IsReleased & state.Key == Key.I)
                        Keyboard2KeyI = false;
                    if (inc == 1 & state.IsPressed & state.Key == Key.O)
                        Keyboard2KeyO = true;
                    if (inc == 1 & state.IsReleased & state.Key == Key.O)
                        Keyboard2KeyO = false;
                    if (inc == 1 & state.IsPressed & state.Key == Key.P)
                        Keyboard2KeyP = true;
                    if (inc == 1 & state.IsReleased & state.Key == Key.P)
                        Keyboard2KeyP = false;
                    if (inc == 1 & state.IsPressed & state.Key == Key.LeftBracket)
                        Keyboard2KeyLeftBracket = true;
                    if (inc == 1 & state.IsReleased & state.Key == Key.LeftBracket)
                        Keyboard2KeyLeftBracket = false;
                    if (inc == 1 & state.IsPressed & state.Key == Key.RightBracket)
                        Keyboard2KeyRightBracket = true;
                    if (inc == 1 & state.IsReleased & state.Key == Key.RightBracket)
                        Keyboard2KeyRightBracket = false;
                    if (inc == 1 & state.IsPressed & state.Key == Key.Return)
                        Keyboard2KeyReturn = true;
                    if (inc == 1 & state.IsReleased & state.Key == Key.Return)
                        Keyboard2KeyReturn = false;
                    if (inc == 1 & state.IsPressed & state.Key == Key.LeftControl)
                        Keyboard2KeyLeftControl = true;
                    if (inc == 1 & state.IsReleased & state.Key == Key.LeftControl)
                        Keyboard2KeyLeftControl = false;
                    if (inc == 1 & state.IsPressed & state.Key == Key.A)
                        Keyboard2KeyA = true;
                    if (inc == 1 & state.IsReleased & state.Key == Key.A)
                        Keyboard2KeyA = false;
                    if (inc == 1 & state.IsPressed & state.Key == Key.S)
                        Keyboard2KeyS = true;
                    if (inc == 1 & state.IsReleased & state.Key == Key.S)
                        Keyboard2KeyS = false;
                    if (inc == 1 & state.IsPressed & state.Key == Key.D)
                        Keyboard2KeyD = true;
                    if (inc == 1 & state.IsReleased & state.Key == Key.D)
                        Keyboard2KeyD = false;
                    if (inc == 1 & state.IsPressed & state.Key == Key.F)
                        Keyboard2KeyF = true;
                    if (inc == 1 & state.IsReleased & state.Key == Key.F)
                        Keyboard2KeyF = false;
                    if (inc == 1 & state.IsPressed & state.Key == Key.G)
                        Keyboard2KeyG = true;
                    if (inc == 1 & state.IsReleased & state.Key == Key.G)
                        Keyboard2KeyG = false;
                    if (inc == 1 & state.IsPressed & state.Key == Key.H)
                        Keyboard2KeyH = true;
                    if (inc == 1 & state.IsReleased & state.Key == Key.H)
                        Keyboard2KeyH = false;
                    if (inc == 1 & state.IsPressed & state.Key == Key.J)
                        Keyboard2KeyJ = true;
                    if (inc == 1 & state.IsReleased & state.Key == Key.J)
                        Keyboard2KeyJ = false;
                    if (inc == 1 & state.IsPressed & state.Key == Key.K)
                        Keyboard2KeyK = true;
                    if (inc == 1 & state.IsReleased & state.Key == Key.K)
                        Keyboard2KeyK = false;
                    if (inc == 1 & state.IsPressed & state.Key == Key.L)
                        Keyboard2KeyL = true;
                    if (inc == 1 & state.IsReleased & state.Key == Key.L)
                        Keyboard2KeyL = false;
                    if (inc == 1 & state.IsPressed & state.Key == Key.Semicolon)
                        Keyboard2KeySemicolon = true;
                    if (inc == 1 & state.IsReleased & state.Key == Key.Semicolon)
                        Keyboard2KeySemicolon = false;
                    if (inc == 1 & state.IsPressed & state.Key == Key.Apostrophe)
                        Keyboard2KeyApostrophe = true;
                    if (inc == 1 & state.IsReleased & state.Key == Key.Apostrophe)
                        Keyboard2KeyApostrophe = false;
                    if (inc == 1 & state.IsPressed & state.Key == Key.Grave)
                        Keyboard2KeyGrave = true;
                    if (inc == 1 & state.IsReleased & state.Key == Key.Grave)
                        Keyboard2KeyGrave = false;
                    if (inc == 1 & state.IsPressed & state.Key == Key.LeftShift)
                        Keyboard2KeyLeftShift = true;
                    if (inc == 1 & state.IsReleased & state.Key == Key.LeftShift)
                        Keyboard2KeyLeftShift = false;
                    if (inc == 1 & state.IsPressed & state.Key == Key.Backslash)
                        Keyboard2KeyBackslash = true;
                    if (inc == 1 & state.IsReleased & state.Key == Key.Backslash)
                        Keyboard2KeyBackslash = false;
                    if (inc == 1 & state.IsPressed & state.Key == Key.Z)
                        Keyboard2KeyZ = true;
                    if (inc == 1 & state.IsReleased & state.Key == Key.Z)
                        Keyboard2KeyZ = false;
                    if (inc == 1 & state.IsPressed & state.Key == Key.X)
                        Keyboard2KeyX = true;
                    if (inc == 1 & state.IsReleased & state.Key == Key.X)
                        Keyboard2KeyX = false;
                    if (inc == 1 & state.IsPressed & state.Key == Key.C)
                        Keyboard2KeyC = true;
                    if (inc == 1 & state.IsReleased & state.Key == Key.C)
                        Keyboard2KeyC = false;
                    if (inc == 1 & state.IsPressed & state.Key == Key.V)
                        Keyboard2KeyV = true;
                    if (inc == 1 & state.IsReleased & state.Key == Key.V)
                        Keyboard2KeyV = false;
                    if (inc == 1 & state.IsPressed & state.Key == Key.B)
                        Keyboard2KeyB = true;
                    if (inc == 1 & state.IsReleased & state.Key == Key.B)
                        Keyboard2KeyB = false;
                    if (inc == 1 & state.IsPressed & state.Key == Key.N)
                        Keyboard2KeyN = true;
                    if (inc == 1 & state.IsReleased & state.Key == Key.N)
                        Keyboard2KeyN = false;
                    if (inc == 1 & state.IsPressed & state.Key == Key.M)
                        Keyboard2KeyM = true;
                    if (inc == 1 & state.IsReleased & state.Key == Key.M)
                        Keyboard2KeyM = false;
                    if (inc == 1 & state.IsPressed & state.Key == Key.Comma)
                        Keyboard2KeyComma = true;
                    if (inc == 1 & state.IsReleased & state.Key == Key.Comma)
                        Keyboard2KeyComma = false;
                    if (inc == 1 & state.IsPressed & state.Key == Key.Period)
                        Keyboard2KeyPeriod = true;
                    if (inc == 1 & state.IsReleased & state.Key == Key.Period)
                        Keyboard2KeyPeriod = false;
                    if (inc == 1 & state.IsPressed & state.Key == Key.Slash)
                        Keyboard2KeySlash = true;
                    if (inc == 1 & state.IsReleased & state.Key == Key.Slash)
                        Keyboard2KeySlash = false;
                    if (inc == 1 & state.IsPressed & state.Key == Key.RightShift)
                        Keyboard2KeyRightShift = true;
                    if (inc == 1 & state.IsReleased & state.Key == Key.RightShift)
                        Keyboard2KeyRightShift = false;
                    if (inc == 1 & state.IsPressed & state.Key == Key.Multiply)
                        Keyboard2KeyMultiply = true;
                    if (inc == 1 & state.IsReleased & state.Key == Key.Multiply)
                        Keyboard2KeyMultiply = false;
                    if (inc == 1 & state.IsPressed & state.Key == Key.LeftAlt)
                        Keyboard2KeyLeftAlt = true;
                    if (inc == 1 & state.IsReleased & state.Key == Key.LeftAlt)
                        Keyboard2KeyLeftAlt = false;
                    if (inc == 1 & state.IsPressed & state.Key == Key.Space)
                        Keyboard2KeySpace = true;
                    if (inc == 1 & state.IsReleased & state.Key == Key.Space)
                        Keyboard2KeySpace = false;
                    if (inc == 1 & state.IsPressed & state.Key == Key.Capital)
                        Keyboard2KeyCapital = true;
                    if (inc == 1 & state.IsReleased & state.Key == Key.Capital)
                        Keyboard2KeyCapital = false;
                    if (inc == 1 & state.IsPressed & state.Key == Key.F1)
                        Keyboard2KeyF1 = true;
                    if (inc == 1 & state.IsReleased & state.Key == Key.F1)
                        Keyboard2KeyF1 = false;
                    if (inc == 1 & state.IsPressed & state.Key == Key.F2)
                        Keyboard2KeyF2 = true;
                    if (inc == 1 & state.IsReleased & state.Key == Key.F2)
                        Keyboard2KeyF2 = false;
                    if (inc == 1 & state.IsPressed & state.Key == Key.F3)
                        Keyboard2KeyF3 = true;
                    if (inc == 1 & state.IsReleased & state.Key == Key.F3)
                        Keyboard2KeyF3 = false;
                    if (inc == 1 & state.IsPressed & state.Key == Key.F4)
                        Keyboard2KeyF4 = true;
                    if (inc == 1 & state.IsReleased & state.Key == Key.F4)
                        Keyboard2KeyF4 = false;
                    if (inc == 1 & state.IsPressed & state.Key == Key.F5)
                        Keyboard2KeyF5 = true;
                    if (inc == 1 & state.IsReleased & state.Key == Key.F5)
                        Keyboard2KeyF5 = false;
                    if (inc == 1 & state.IsPressed & state.Key == Key.F6)
                        Keyboard2KeyF6 = true;
                    if (inc == 1 & state.IsReleased & state.Key == Key.F6)
                        Keyboard2KeyF6 = false;
                    if (inc == 1 & state.IsPressed & state.Key == Key.F7)
                        Keyboard2KeyF7 = true;
                    if (inc == 1 & state.IsReleased & state.Key == Key.F7)
                        Keyboard2KeyF7 = false;
                    if (inc == 1 & state.IsPressed & state.Key == Key.F8)
                        Keyboard2KeyF8 = true;
                    if (inc == 1 & state.IsReleased & state.Key == Key.F8)
                        Keyboard2KeyF8 = false;
                    if (inc == 1 & state.IsPressed & state.Key == Key.F9)
                        Keyboard2KeyF9 = true;
                    if (inc == 1 & state.IsReleased & state.Key == Key.F9)
                        Keyboard2KeyF9 = false;
                    if (inc == 1 & state.IsPressed & state.Key == Key.F10)
                        Keyboard2KeyF10 = true;
                    if (inc == 1 & state.IsReleased & state.Key == Key.F10)
                        Keyboard2KeyF10 = false;
                    if (inc == 1 & state.IsPressed & state.Key == Key.NumberLock)
                        Keyboard2KeyNumberLock = true;
                    if (inc == 1 & state.IsReleased & state.Key == Key.NumberLock)
                        Keyboard2KeyNumberLock = false;
                    if (inc == 1 & state.IsPressed & state.Key == Key.ScrollLock)
                        Keyboard2KeyScrollLock = true;
                    if (inc == 1 & state.IsReleased & state.Key == Key.ScrollLock)
                        Keyboard2KeyScrollLock = false;
                    if (inc == 1 & state.IsPressed & state.Key == Key.NumberPad7)
                        Keyboard2KeyNumberPad7 = true;
                    if (inc == 1 & state.IsReleased & state.Key == Key.NumberPad7)
                        Keyboard2KeyNumberPad7 = false;
                    if (inc == 1 & state.IsPressed & state.Key == Key.NumberPad8)
                        Keyboard2KeyNumberPad8 = true;
                    if (inc == 1 & state.IsReleased & state.Key == Key.NumberPad8)
                        Keyboard2KeyNumberPad8 = false;
                    if (inc == 1 & state.IsPressed & state.Key == Key.NumberPad9)
                        Keyboard2KeyNumberPad9 = true;
                    if (inc == 1 & state.IsReleased & state.Key == Key.NumberPad9)
                        Keyboard2KeyNumberPad9 = false;
                    if (inc == 1 & state.IsPressed & state.Key == Key.Subtract)
                        Keyboard2KeySubtract = true;
                    if (inc == 1 & state.IsReleased & state.Key == Key.Subtract)
                        Keyboard2KeySubtract = false;
                    if (inc == 1 & state.IsPressed & state.Key == Key.NumberPad4)
                        Keyboard2KeyNumberPad4 = true;
                    if (inc == 1 & state.IsReleased & state.Key == Key.NumberPad4)
                        Keyboard2KeyNumberPad4 = false;
                    if (inc == 1 & state.IsPressed & state.Key == Key.NumberPad5)
                        Keyboard2KeyNumberPad5 = true;
                    if (inc == 1 & state.IsReleased & state.Key == Key.NumberPad5)
                        Keyboard2KeyNumberPad5 = false;
                    if (inc == 1 & state.IsPressed & state.Key == Key.NumberPad6)
                        Keyboard2KeyNumberPad6 = true;
                    if (inc == 1 & state.IsReleased & state.Key == Key.NumberPad6)
                        Keyboard2KeyNumberPad6 = false;
                    if (inc == 1 & state.IsPressed & state.Key == Key.Add)
                        Keyboard2KeyAdd = true;
                    if (inc == 1 & state.IsReleased & state.Key == Key.Add)
                        Keyboard2KeyAdd = false;
                    if (inc == 1 & state.IsPressed & state.Key == Key.NumberPad1)
                        Keyboard2KeyNumberPad1 = true;
                    if (inc == 1 & state.IsReleased & state.Key == Key.NumberPad1)
                        Keyboard2KeyNumberPad1 = false;
                    if (inc == 1 & state.IsPressed & state.Key == Key.NumberPad2)
                        Keyboard2KeyNumberPad2 = true;
                    if (inc == 1 & state.IsReleased & state.Key == Key.NumberPad2)
                        Keyboard2KeyNumberPad2 = false;
                    if (inc == 1 & state.IsPressed & state.Key == Key.NumberPad3)
                        Keyboard2KeyNumberPad3 = true;
                    if (inc == 1 & state.IsReleased & state.Key == Key.NumberPad3)
                        Keyboard2KeyNumberPad3 = false;
                    if (inc == 1 & state.IsPressed & state.Key == Key.NumberPad0)
                        Keyboard2KeyNumberPad0 = true;
                    if (inc == 1 & state.IsReleased & state.Key == Key.NumberPad0)
                        Keyboard2KeyNumberPad0 = false;
                    if (inc == 1 & state.IsPressed & state.Key == Key.Decimal)
                        Keyboard2KeyDecimal = true;
                    if (inc == 1 & state.IsReleased & state.Key == Key.Decimal)
                        Keyboard2KeyDecimal = false;
                    if (inc == 1 & state.IsPressed & state.Key == Key.Oem102)
                        Keyboard2KeyOem102 = true;
                    if (inc == 1 & state.IsReleased & state.Key == Key.Oem102)
                        Keyboard2KeyOem102 = false;
                    if (inc == 1 & state.IsPressed & state.Key == Key.F11)
                        Keyboard2KeyF11 = true;
                    if (inc == 1 & state.IsReleased & state.Key == Key.F11)
                        Keyboard2KeyF11 = false;
                    if (inc == 1 & state.IsPressed & state.Key == Key.F12)
                        Keyboard2KeyF12 = true;
                    if (inc == 1 & state.IsReleased & state.Key == Key.F12)
                        Keyboard2KeyF12 = false;
                    if (inc == 1 & state.IsPressed & state.Key == Key.F13)
                        Keyboard2KeyF13 = true;
                    if (inc == 1 & state.IsReleased & state.Key == Key.F13)
                        Keyboard2KeyF13 = false;
                    if (inc == 1 & state.IsPressed & state.Key == Key.F14)
                        Keyboard2KeyF14 = true;
                    if (inc == 1 & state.IsReleased & state.Key == Key.F14)
                        Keyboard2KeyF14 = false;
                    if (inc == 1 & state.IsPressed & state.Key == Key.F15)
                        Keyboard2KeyF15 = true;
                    if (inc == 1 & state.IsReleased & state.Key == Key.F15)
                        Keyboard2KeyF15 = false;
                    if (inc == 1 & state.IsPressed & state.Key == Key.Kana)
                        Keyboard2KeyKana = true;
                    if (inc == 1 & state.IsReleased & state.Key == Key.Kana)
                        Keyboard2KeyKana = false;
                    if (inc == 1 & state.IsPressed & state.Key == Key.AbntC1)
                        Keyboard2KeyAbntC1 = true;
                    if (inc == 1 & state.IsReleased & state.Key == Key.AbntC1)
                        Keyboard2KeyAbntC1 = false;
                    if (inc == 1 & state.IsPressed & state.Key == Key.Convert)
                        Keyboard2KeyConvert = true;
                    if (inc == 1 & state.IsReleased & state.Key == Key.Convert)
                        Keyboard2KeyConvert = false;
                    if (inc == 1 & state.IsPressed & state.Key == Key.NoConvert)
                        Keyboard2KeyNoConvert = true;
                    if (inc == 1 & state.IsReleased & state.Key == Key.NoConvert)
                        Keyboard2KeyNoConvert = false;
                    if (inc == 1 & state.IsPressed & state.Key == Key.Yen)
                        Keyboard2KeyYen = true;
                    if (inc == 1 & state.IsReleased & state.Key == Key.Yen)
                        Keyboard2KeyYen = false;
                    if (inc == 1 & state.IsPressed & state.Key == Key.AbntC2)
                        Keyboard2KeyAbntC2 = true;
                    if (inc == 1 & state.IsReleased & state.Key == Key.AbntC2)
                        Keyboard2KeyAbntC2 = false;
                    if (inc == 1 & state.IsPressed & state.Key == Key.NumberPadEquals)
                        Keyboard2KeyNumberPadEquals = true;
                    if (inc == 1 & state.IsReleased & state.Key == Key.NumberPadEquals)
                        Keyboard2KeyNumberPadEquals = false;
                    if (inc == 1 & state.IsPressed & state.Key == Key.PreviousTrack)
                        Keyboard2KeyPreviousTrack = true;
                    if (inc == 1 & state.IsReleased & state.Key == Key.PreviousTrack)
                        Keyboard2KeyPreviousTrack = false;
                    if (inc == 1 & state.IsPressed & state.Key == Key.AT)
                        Keyboard2KeyAT = true;
                    if (inc == 1 & state.IsReleased & state.Key == Key.AT)
                        Keyboard2KeyAT = false;
                    if (inc == 1 & state.IsPressed & state.Key == Key.Colon)
                        Keyboard2KeyColon = true;
                    if (inc == 1 & state.IsReleased & state.Key == Key.Colon)
                        Keyboard2KeyColon = false;
                    if (inc == 1 & state.IsPressed & state.Key == Key.Underline)
                        Keyboard2KeyUnderline = true;
                    if (inc == 1 & state.IsReleased & state.Key == Key.Underline)
                        Keyboard2KeyUnderline = false;
                    if (inc == 1 & state.IsPressed & state.Key == Key.Kanji)
                        Keyboard2KeyKanji = true;
                    if (inc == 1 & state.IsReleased & state.Key == Key.Kanji)
                        Keyboard2KeyKanji = false;
                    if (inc == 1 & state.IsPressed & state.Key == Key.Stop)
                        Keyboard2KeyStop = true;
                    if (inc == 1 & state.IsReleased & state.Key == Key.Stop)
                        Keyboard2KeyStop = false;
                    if (inc == 1 & state.IsPressed & state.Key == Key.AX)
                        Keyboard2KeyAX = true;
                    if (inc == 1 & state.IsReleased & state.Key == Key.AX)
                        Keyboard2KeyAX = false;
                    if (inc == 1 & state.IsPressed & state.Key == Key.Unlabeled)
                        Keyboard2KeyUnlabeled = true;
                    if (inc == 1 & state.IsReleased & state.Key == Key.Unlabeled)
                        Keyboard2KeyUnlabeled = false;
                    if (inc == 1 & state.IsPressed & state.Key == Key.NextTrack)
                        Keyboard2KeyNextTrack = true;
                    if (inc == 1 & state.IsReleased & state.Key == Key.NextTrack)
                        Keyboard2KeyNextTrack = false;
                    if (inc == 1 & state.IsPressed & state.Key == Key.NumberPadEnter)
                        Keyboard2KeyNumberPadEnter = true;
                    if (inc == 1 & state.IsReleased & state.Key == Key.NumberPadEnter)
                        Keyboard2KeyNumberPadEnter = false;
                    if (inc == 1 & state.IsPressed & state.Key == Key.RightControl)
                        Keyboard2KeyRightControl = true;
                    if (inc == 1 & state.IsReleased & state.Key == Key.RightControl)
                        Keyboard2KeyRightControl = false;
                    if (inc == 1 & state.IsPressed & state.Key == Key.Mute)
                        Keyboard2KeyMute = true;
                    if (inc == 1 & state.IsReleased & state.Key == Key.Mute)
                        Keyboard2KeyMute = false;
                    if (inc == 1 & state.IsPressed & state.Key == Key.Calculator)
                        Keyboard2KeyCalculator = true;
                    if (inc == 1 & state.IsReleased & state.Key == Key.Calculator)
                        Keyboard2KeyCalculator = false;
                    if (inc == 1 & state.IsPressed & state.Key == Key.PlayPause)
                        Keyboard2KeyPlayPause = true;
                    if (inc == 1 & state.IsReleased & state.Key == Key.PlayPause)
                        Keyboard2KeyPlayPause = false;
                    if (inc == 1 & state.IsPressed & state.Key == Key.MediaStop)
                        Keyboard2KeyMediaStop = true;
                    if (inc == 1 & state.IsReleased & state.Key == Key.MediaStop)
                        Keyboard2KeyMediaStop = false;
                    if (inc == 1 & state.IsPressed & state.Key == Key.VolumeDown)
                        Keyboard2KeyVolumeDown = true;
                    if (inc == 1 & state.IsReleased & state.Key == Key.VolumeDown)
                        Keyboard2KeyVolumeDown = false;
                    if (inc == 1 & state.IsPressed & state.Key == Key.VolumeUp)
                        Keyboard2KeyVolumeUp = true;
                    if (inc == 1 & state.IsReleased & state.Key == Key.VolumeUp)
                        Keyboard2KeyVolumeUp = false;
                    if (inc == 1 & state.IsPressed & state.Key == Key.WebHome)
                        Keyboard2KeyWebHome = true;
                    if (inc == 1 & state.IsReleased & state.Key == Key.WebHome)
                        Keyboard2KeyWebHome = false;
                    if (inc == 1 & state.IsPressed & state.Key == Key.NumberPadComma)
                        Keyboard2KeyNumberPadComma = true;
                    if (inc == 1 & state.IsReleased & state.Key == Key.NumberPadComma)
                        Keyboard2KeyNumberPadComma = false;
                    if (inc == 1 & state.IsPressed & state.Key == Key.Divide)
                        Keyboard2KeyDivide = true;
                    if (inc == 1 & state.IsReleased & state.Key == Key.Divide)
                        Keyboard2KeyDivide = false;
                    if (inc == 1 & state.IsPressed & state.Key == Key.PrintScreen)
                        Keyboard2KeyPrintScreen = true;
                    if (inc == 1 & state.IsReleased & state.Key == Key.PrintScreen)
                        Keyboard2KeyPrintScreen = false;
                    if (inc == 1 & state.IsPressed & state.Key == Key.RightAlt)
                        Keyboard2KeyRightAlt = true;
                    if (inc == 1 & state.IsReleased & state.Key == Key.RightAlt)
                        Keyboard2KeyRightAlt = false;
                    if (inc == 1 & state.IsPressed & state.Key == Key.Pause)
                        Keyboard2KeyPause = true;
                    if (inc == 1 & state.IsReleased & state.Key == Key.Pause)
                        Keyboard2KeyPause = false;
                    if (inc == 1 & state.IsPressed & state.Key == Key.Home)
                        Keyboard2KeyHome = true;
                    if (inc == 1 & state.IsReleased & state.Key == Key.Home)
                        Keyboard2KeyHome = false;
                    if (inc == 1 & state.IsPressed & state.Key == Key.Up)
                        Keyboard2KeyUp = true;
                    if (inc == 1 & state.IsReleased & state.Key == Key.Up)
                        Keyboard2KeyUp = false;
                    if (inc == 1 & state.IsPressed & state.Key == Key.PageUp)
                        Keyboard2KeyPageUp = true;
                    if (inc == 1 & state.IsReleased & state.Key == Key.PageUp)
                        Keyboard2KeyPageUp = false;
                    if (inc == 1 & state.IsPressed & state.Key == Key.Left)
                        Keyboard2KeyLeft = true;
                    if (inc == 1 & state.IsReleased & state.Key == Key.Left)
                        Keyboard2KeyLeft = false;
                    if (inc == 1 & state.IsPressed & state.Key == Key.Right)
                        Keyboard2KeyRight = true;
                    if (inc == 1 & state.IsReleased & state.Key == Key.Right)
                        Keyboard2KeyRight = false;
                    if (inc == 1 & state.IsPressed & state.Key == Key.End)
                        Keyboard2KeyEnd = true;
                    if (inc == 1 & state.IsReleased & state.Key == Key.End)
                        Keyboard2KeyEnd = false;
                    if (inc == 1 & state.IsPressed & state.Key == Key.Down)
                        Keyboard2KeyDown = true;
                    if (inc == 1 & state.IsReleased & state.Key == Key.Down)
                        Keyboard2KeyDown = false;
                    if (inc == 1 & state.IsPressed & state.Key == Key.PageDown)
                        Keyboard2KeyPageDown = true;
                    if (inc == 1 & state.IsReleased & state.Key == Key.PageDown)
                        Keyboard2KeyPageDown = false;
                    if (inc == 1 & state.IsPressed & state.Key == Key.Insert)
                        Keyboard2KeyInsert = true;
                    if (inc == 1 & state.IsReleased & state.Key == Key.Insert)
                        Keyboard2KeyInsert = false;
                    if (inc == 1 & state.IsPressed & state.Key == Key.Delete)
                        Keyboard2KeyDelete = true;
                    if (inc == 1 & state.IsReleased & state.Key == Key.Delete)
                        Keyboard2KeyDelete = false;
                    if (inc == 1 & state.IsPressed & state.Key == Key.LeftWindowsKey)
                        Keyboard2KeyLeftWindowsKey = true;
                    if (inc == 1 & state.IsReleased & state.Key == Key.LeftWindowsKey)
                        Keyboard2KeyLeftWindowsKey = false;
                    if (inc == 1 & state.IsPressed & state.Key == Key.RightWindowsKey)
                        Keyboard2KeyRightWindowsKey = true;
                    if (inc == 1 & state.IsReleased & state.Key == Key.RightWindowsKey)
                        Keyboard2KeyRightWindowsKey = false;
                    if (inc == 1 & state.IsPressed & state.Key == Key.Applications)
                        Keyboard2KeyApplications = true;
                    if (inc == 1 & state.IsReleased & state.Key == Key.Applications)
                        Keyboard2KeyApplications = false;
                    if (inc == 1 & state.IsPressed & state.Key == Key.Power)
                        Keyboard2KeyPower = true;
                    if (inc == 1 & state.IsReleased & state.Key == Key.Power)
                        Keyboard2KeyPower = false;
                    if (inc == 1 & state.IsPressed & state.Key == Key.Sleep)
                        Keyboard2KeySleep = true;
                    if (inc == 1 & state.IsReleased & state.Key == Key.Sleep)
                        Keyboard2KeySleep = false;
                    if (inc == 1 & state.IsPressed & state.Key == Key.Wake)
                        Keyboard2KeyWake = true;
                    if (inc == 1 & state.IsReleased & state.Key == Key.Wake)
                        Keyboard2KeyWake = false;
                    if (inc == 1 & state.IsPressed & state.Key == Key.WebSearch)
                        Keyboard2KeyWebSearch = true;
                    if (inc == 1 & state.IsReleased & state.Key == Key.WebSearch)
                        Keyboard2KeyWebSearch = false;
                    if (inc == 1 & state.IsPressed & state.Key == Key.WebFavorites)
                        Keyboard2KeyWebFavorites = true;
                    if (inc == 1 & state.IsReleased & state.Key == Key.WebFavorites)
                        Keyboard2KeyWebFavorites = false;
                    if (inc == 1 & state.IsPressed & state.Key == Key.WebRefresh)
                        Keyboard2KeyWebRefresh = true;
                    if (inc == 1 & state.IsReleased & state.Key == Key.WebRefresh)
                        Keyboard2KeyWebRefresh = false;
                    if (inc == 1 & state.IsPressed & state.Key == Key.WebStop)
                        Keyboard2KeyWebStop = true;
                    if (inc == 1 & state.IsReleased & state.Key == Key.WebStop)
                        Keyboard2KeyWebStop = false;
                    if (inc == 1 & state.IsPressed & state.Key == Key.WebForward)
                        Keyboard2KeyWebForward = true;
                    if (inc == 1 & state.IsReleased & state.Key == Key.WebForward)
                        Keyboard2KeyWebForward = false;
                    if (inc == 1 & state.IsPressed & state.Key == Key.WebBack)
                        Keyboard2KeyWebBack = true;
                    if (inc == 1 & state.IsReleased & state.Key == Key.WebBack)
                        Keyboard2KeyWebBack = false;
                    if (inc == 1 & state.IsPressed & state.Key == Key.MyComputer)
                        Keyboard2KeyMyComputer = true;
                    if (inc == 1 & state.IsReleased & state.Key == Key.MyComputer)
                        Keyboard2KeyMyComputer = false;
                    if (inc == 1 & state.IsPressed & state.Key == Key.Mail)
                        Keyboard2KeyMail = true;
                    if (inc == 1 & state.IsReleased & state.Key == Key.Mail)
                        Keyboard2KeyMail = false;
                    if (inc == 1 & state.IsPressed & state.Key == Key.MediaSelect)
                        Keyboard2KeyMediaSelect = true;
                    if (inc == 1 & state.IsReleased & state.Key == Key.MediaSelect)
                        Keyboard2KeyMediaSelect = false;
                    if (inc == 1 & state.IsPressed & state.Key == Key.Unknown)
                        Keyboard2KeyUnknown = true;
                    if (inc == 1 & state.IsReleased & state.Key == Key.Unknown)
                        Keyboard2KeyUnknown = false;
                }
            }
        }
    }
}