using System;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using Keyboardrawinputs;
using SharpDX.Multimedia;
using SharpDX.RawInput;

namespace KeyboardRawInputsAPI
{
    public class KeyboardRawInputs
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
        public KeyboardRawInputs()
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
                System.Threading.Thread.Sleep(1);
                if (formvisible)
                {
                    string str = "Key_0 : " + Key_0 + Environment.NewLine;
                    str += "Key_1 : " + Key_1 + Environment.NewLine;
                    str += "Key_2 : " + Key_2 + Environment.NewLine;
                    str += "Key_3 : " + Key_3 + Environment.NewLine;
                    str += "Key_4 : " + Key_4 + Environment.NewLine;
                    str += "Key_5 : " + Key_5 + Environment.NewLine;
                    str += "Key_6 : " + Key_6 + Environment.NewLine;
                    str += "Key_7 : " + Key_7 + Environment.NewLine;
                    str += "Key_8 : " + Key_8 + Environment.NewLine;
                    str += "Key_9 : " + Key_9 + Environment.NewLine;
                    str += "Key_A : " + Key_A + Environment.NewLine;
                    str += "Key_B : " + Key_B + Environment.NewLine;
                    str += "Key_C : " + Key_C + Environment.NewLine;
                    str += "Key_D : " + Key_D + Environment.NewLine;
                    str += "Key_E : " + Key_E + Environment.NewLine;
                    str += "Key_F : " + Key_F + Environment.NewLine;
                    str += "Key_G : " + Key_G + Environment.NewLine;
                    str += "Key_H : " + Key_H + Environment.NewLine;
                    str += "Key_I : " + Key_I + Environment.NewLine;
                    str += "Key_J : " + Key_J + Environment.NewLine;
                    str += "Key_K : " + Key_K + Environment.NewLine;
                    str += "Key_L : " + Key_L + Environment.NewLine;
                    str += "Key_M : " + Key_M + Environment.NewLine;
                    str += "Key_N : " + Key_N + Environment.NewLine;
                    str += "Key_O : " + Key_O + Environment.NewLine;
                    str += "Key_P : " + Key_P + Environment.NewLine;
                    str += "Key_Q : " + Key_Q + Environment.NewLine;
                    str += "Key_R : " + Key_R + Environment.NewLine;
                    str += "Key_S : " + Key_S + Environment.NewLine;
                    str += "Key_T : " + Key_T + Environment.NewLine;
                    str += "Key_U : " + Key_U + Environment.NewLine;
                    str += "Key_V : " + Key_V + Environment.NewLine;
                    str += "Key_W : " + Key_W + Environment.NewLine;
                    str += "Key_X : " + Key_X + Environment.NewLine;
                    str += "Key_Y : " + Key_Y + Environment.NewLine;
                    str += "Key_Z : " + Key_Z + Environment.NewLine;
                    str += "Key_F1 : " + Key_F1 + Environment.NewLine;
                    str += "Key_F2 : " + Key_F2 + Environment.NewLine;
                    str += "Key_F3 : " + Key_F3 + Environment.NewLine;
                    str += "Key_F4 : " + Key_F4 + Environment.NewLine;
                    str += "Key_F5 : " + Key_F5 + Environment.NewLine;
                    str += "Key_F6 : " + Key_F6 + Environment.NewLine;
                    str += "Key_F7 : " + Key_F7 + Environment.NewLine;
                    str += "Key_F8 : " + Key_F8 + Environment.NewLine;
                    str += "Key_F9 : " + Key_F9 + Environment.NewLine;
                    str += "Key_F10 : " + Key_F10 + Environment.NewLine;
                    str += "Key_F11 : " + Key_F11 + Environment.NewLine;
                    str += "Key_F12 : " + Key_F12 + Environment.NewLine;
                    str += "Key_F13 : " + Key_F13 + Environment.NewLine;
                    str += "Key_F14 : " + Key_F14 + Environment.NewLine;
                    str += "Key_F15 : " + Key_F15 + Environment.NewLine;
                    str += "Key_F16 : " + Key_F16 + Environment.NewLine;
                    str += "Key_F17 : " + Key_F17 + Environment.NewLine;
                    str += "Key_F18 : " + Key_F18 + Environment.NewLine;
                    str += "Key_F19 : " + Key_F19 + Environment.NewLine;
                    str += "Key_F20 : " + Key_F20 + Environment.NewLine;
                    str += "Key_F21 : " + Key_F21 + Environment.NewLine;
                    str += "Key_F22 : " + Key_F22 + Environment.NewLine;
                    str += "Key_F23 : " + Key_F23 + Environment.NewLine;
                    str += "Key_F24 : " + Key_F24 + Environment.NewLine;
                    str += "Key_LWIN : " + Key_LWIN + Environment.NewLine;
                    str += "Key_RWIN : " + Key_RWIN + Environment.NewLine;
                    str += "Key_APPS : " + Key_APPS + Environment.NewLine;
                    str += "Key_SLEEP : " + Key_SLEEP + Environment.NewLine;
                    str += "Key_LBUTTON : " + Key_LBUTTON + Environment.NewLine;
                    str += "Key_RBUTTON : " + Key_RBUTTON + Environment.NewLine;
                    str += "Key_CANCEL : " + Key_CANCEL + Environment.NewLine;
                    str += "Key_MBUTTON : " + Key_MBUTTON + Environment.NewLine;
                    str += "Key_XBUTTON1 : " + Key_XBUTTON1 + Environment.NewLine;
                    str += "Key_XBUTTON2 : " + Key_XBUTTON2 + Environment.NewLine;
                    str += "Key_BACK : " + Key_BACK + Environment.NewLine;
                    str += "Key_Tab : " + Key_Tab + Environment.NewLine;
                    str += "Key_CLEAR : " + Key_CLEAR + Environment.NewLine;
                    str += "Key_Return : " + Key_Return + Environment.NewLine;
                    str += "Key_SHIFT : " + Key_SHIFT + Environment.NewLine;
                    str += "Key_CONTROL : " + Key_CONTROL + Environment.NewLine;
                    str += "Key_MENU : " + Key_MENU + Environment.NewLine;
                    str += "Key_PAUSE : " + Key_PAUSE + Environment.NewLine;
                    str += "Key_CAPITAL : " + Key_CAPITAL + Environment.NewLine;
                    str += "Key_KANA : " + Key_KANA + Environment.NewLine;
                    str += "Key_HANGEUL : " + Key_HANGEUL + Environment.NewLine;
                    str += "Key_HANGUL : " + Key_HANGUL + Environment.NewLine;
                    str += "Key_JUNJA : " + Key_JUNJA + Environment.NewLine;
                    str += "Key_FINAL : " + Key_FINAL + Environment.NewLine;
                    str += "Key_HANJA : " + Key_HANJA + Environment.NewLine;
                    str += "Key_KANJI : " + Key_KANJI + Environment.NewLine;
                    str += "Key_Escape : " + Key_Escape + Environment.NewLine;
                    str += "Key_CONVERT : " + Key_CONVERT + Environment.NewLine;
                    str += "Key_NONCONVERT : " + Key_NONCONVERT + Environment.NewLine;
                    str += "Key_ACCEPT : " + Key_ACCEPT + Environment.NewLine;
                    str += "Key_MODECHANGE : " + Key_MODECHANGE + Environment.NewLine;
                    str += "Key_Space : " + Key_Space + Environment.NewLine;
                    str += "Key_PRIOR : " + Key_PRIOR + Environment.NewLine;
                    str += "Key_NEXT : " + Key_NEXT + Environment.NewLine;
                    str += "Key_END : " + Key_END + Environment.NewLine;
                    str += "Key_HOME : " + Key_HOME + Environment.NewLine;
                    str += "Key_LEFT : " + Key_LEFT + Environment.NewLine;
                    str += "Key_UP : " + Key_UP + Environment.NewLine;
                    str += "Key_RIGHT : " + Key_RIGHT + Environment.NewLine;
                    str += "Key_DOWN : " + Key_DOWN + Environment.NewLine;
                    str += "Key_SELECT : " + Key_SELECT + Environment.NewLine;
                    str += "Key_PRINT : " + Key_PRINT + Environment.NewLine;
                    str += "Key_EXECUTE : " + Key_EXECUTE + Environment.NewLine;
                    str += "Key_SNAPSHOT : " + Key_SNAPSHOT + Environment.NewLine;
                    str += "Key_INSERT : " + Key_INSERT + Environment.NewLine;
                    str += "Key_DELETE : " + Key_DELETE + Environment.NewLine;
                    str += "Key_HELP : " + Key_HELP + Environment.NewLine;
                    str += "Key_APOSTROPHE : " + Key_APOSTROPHE + Environment.NewLine;
                    str += "Key_NUMPAD0 : " + Key_NUMPAD0 + Environment.NewLine;
                    str += "Key_NUMPAD1 : " + Key_NUMPAD1 + Environment.NewLine;
                    str += "Key_NUMPAD2 : " + Key_NUMPAD2 + Environment.NewLine;
                    str += "Key_NUMPAD3 : " + Key_NUMPAD3 + Environment.NewLine;
                    str += "Key_NUMPAD4 : " + Key_NUMPAD4 + Environment.NewLine;
                    str += "Key_NUMPAD5 : " + Key_NUMPAD5 + Environment.NewLine;
                    str += "Key_NUMPAD6 : " + Key_NUMPAD6 + Environment.NewLine;
                    str += "Key_NUMPAD7 : " + Key_NUMPAD7 + Environment.NewLine;
                    str += "Key_NUMPAD8 : " + Key_NUMPAD8 + Environment.NewLine;
                    str += "Key_NUMPAD9 : " + Key_NUMPAD9 + Environment.NewLine;
                    str += "Key_MULTIPLY : " + Key_MULTIPLY + Environment.NewLine;
                    str += "Key_ADD : " + Key_ADD + Environment.NewLine;
                    str += "Key_SEPARATOR : " + Key_SEPARATOR + Environment.NewLine;
                    str += "Key_SUBTRACT : " + Key_SUBTRACT + Environment.NewLine;
                    str += "Key_DECIMAL : " + Key_DECIMAL + Environment.NewLine;
                    str += "Key_DIVIDE : " + Key_DIVIDE + Environment.NewLine;
                    str += "Key_NUMLOCK : " + Key_NUMLOCK + Environment.NewLine;
                    str += "Key_SCROLL : " + Key_SCROLL + Environment.NewLine;
                    str += "Key_LeftShift : " + Key_LeftShift + Environment.NewLine;
                    str += "Key_RightShift : " + Key_RightShift + Environment.NewLine;
                    str += "Key_LeftControl : " + Key_LeftControl + Environment.NewLine;
                    str += "Key_RightControl : " + Key_RightControl + Environment.NewLine;
                    str += "Key_LMENU : " + Key_LMENU + Environment.NewLine;
                    str += "Key_RMENU : " + Key_RMENU + Environment.NewLine;
                    str += "Key_BROWSER_BACK : " + Key_BROWSER_BACK + Environment.NewLine;
                    str += "Key_BROWSER_FORWARD : " + Key_BROWSER_FORWARD + Environment.NewLine;
                    str += "Key_BROWSER_REFRESH : " + Key_BROWSER_REFRESH + Environment.NewLine;
                    str += "Key_BROWSER_STOP : " + Key_BROWSER_STOP + Environment.NewLine;
                    str += "Key_BROWSER_SEARCH : " + Key_BROWSER_SEARCH + Environment.NewLine;
                    str += "Key_BROWSER_FAVORITES : " + Key_BROWSER_FAVORITES + Environment.NewLine;
                    str += "Key_BROWSER_HOME : " + Key_BROWSER_HOME + Environment.NewLine;
                    str += "Key_VOLUME_MUTE : " + Key_VOLUME_MUTE + Environment.NewLine;
                    str += "Key_VOLUME_DOWN : " + Key_VOLUME_DOWN + Environment.NewLine;
                    str += "Key_VOLUME_UP : " + Key_VOLUME_UP + Environment.NewLine;
                    str += "Key_MEDIA_NEXT_TRACK : " + Key_MEDIA_NEXT_TRACK + Environment.NewLine;
                    str += "Key_MEDIA_PREV_TRACK : " + Key_MEDIA_PREV_TRACK + Environment.NewLine;
                    str += "Key_MEDIA_STOP : " + Key_MEDIA_STOP + Environment.NewLine;
                    str += "Key_MEDIA_PLAY_PAUSE : " + Key_MEDIA_PLAY_PAUSE + Environment.NewLine;
                    str += "Key_LAUNCH_MAIL : " + Key_LAUNCH_MAIL + Environment.NewLine;
                    str += "Key_LAUNCH_MEDIA_SELECT : " + Key_LAUNCH_MEDIA_SELECT + Environment.NewLine;
                    str += "Key_LAUNCH_APP1 : " + Key_LAUNCH_APP1 + Environment.NewLine;
                    str += "Key_LAUNCH_APP2 : " + Key_LAUNCH_APP2 + Environment.NewLine;
                    str += "Key_OEM_1 : " + Key_OEM_1 + Environment.NewLine;
                    str += "Key_OEM_PLUS : " + Key_OEM_PLUS + Environment.NewLine;
                    str += "Key_OEM_COMMA : " + Key_OEM_COMMA + Environment.NewLine;
                    str += "Key_OEM_MINUS : " + Key_OEM_MINUS + Environment.NewLine;
                    str += "Key_OEM_PERIOD : " + Key_OEM_PERIOD + Environment.NewLine;
                    str += "Key_OEM_2 : " + Key_OEM_2 + Environment.NewLine;
                    str += "Key_OEM_3 : " + Key_OEM_3 + Environment.NewLine;
                    str += "Key_OEM_4 : " + Key_OEM_4 + Environment.NewLine;
                    str += "Key_OEM_5 : " + Key_OEM_5 + Environment.NewLine;
                    str += "Key_OEM_6 : " + Key_OEM_6 + Environment.NewLine;
                    str += "Key_OEM_7 : " + Key_OEM_7 + Environment.NewLine;
                    str += "Key_OEM_8 : " + Key_OEM_8 + Environment.NewLine;
                    str += "Key_OEM_102 : " + Key_OEM_102 + Environment.NewLine;
                    str += "Key_PROCESSKEY : " + Key_PROCESSKEY + Environment.NewLine;
                    str += "Key_PACKET : " + Key_PACKET + Environment.NewLine;
                    str += "Key_ATTN : " + Key_ATTN + Environment.NewLine;
                    str += "Key_CRSEL : " + Key_CRSEL + Environment.NewLine;
                    str += "Key_EXSEL : " + Key_EXSEL + Environment.NewLine;
                    str += "Key_EREOF : " + Key_EREOF + Environment.NewLine;
                    str += "Key_PLAY : " + Key_PLAY + Environment.NewLine;
                    str += "Key_ZOOM : " + Key_ZOOM + Environment.NewLine;
                    str += "Key_NONAME : " + Key_NONAME + Environment.NewLine;
                    str += "Key_PA1 : " + Key_PA1 + Environment.NewLine;
                    str += "Key_OEM_CLEAR : " + Key_OEM_CLEAR + Environment.NewLine;
                    str += Environment.NewLine;
                    form1.SetLabel1(str);
                }
            }
        }
        public void BeginPolling()
        {
            Task.Run(() => taskK());
        }
        private const int S_LBUTTON = (int)0x0;
        private const int S_RBUTTON = 0;
        private const int S_CANCEL = 70;
        private const int S_MBUTTON = 0;
        private const int S_XBUTTON1 = 0;
        private const int S_XBUTTON2 = 0;
        private const int S_BACK = 14;
        private const int S_Tab = 15;
        private const int S_CLEAR = 76;
        private const int S_Return = 28;
        private const int S_SHIFT = 42;
        private const int S_CONTROL = 29;
        private const int S_MENU = 56;
        private const int S_PAUSE = 0;
        private const int S_CAPITAL = 58;
        private const int S_KANA = 0;
        private const int S_HANGEUL = 0;
        private const int S_HANGUL = 0;
        private const int S_JUNJA = 0;
        private const int S_FINAL = 0;
        private const int S_HANJA = 0;
        private const int S_KANJI = 0;
        private const int S_Escape = 1;
        private const int S_CONVERT = 0;
        private const int S_NONCONVERT = 0;
        private const int S_ACCEPT = 0;
        private const int S_MODECHANGE = 0;
        private const int S_Space = 57;
        private const int S_PRIOR = 73;
        private const int S_NEXT = 81;
        private const int S_END = 79;
        private const int S_HOME = 71;
        private const int S_LEFT = 75;
        private const int S_UP = 72;
        private const int S_RIGHT = 77;
        private const int S_DOWN = 80;
        private const int S_SELECT = 0;
        private const int S_PRINT = 0;
        private const int S_EXECUTE = 0;
        private const int S_SNAPSHOT = 84;
        private const int S_INSERT = 82;
        private const int S_DELETE = 83;
        private const int S_HELP = 99;
        private const int S_APOSTROPHE = 41;
        private const int S_0 = 11;
        private const int S_1 = 2;
        private const int S_2 = 3;
        private const int S_3 = 4;
        private const int S_4 = 5;
        private const int S_5 = 6;
        private const int S_6 = 7;
        private const int S_7 = 8;
        private const int S_8 = 9;
        private const int S_9 = 10;
        private const int S_A = 16;
        private const int S_B = 48;
        private const int S_C = 46;
        private const int S_D = 32;
        private const int S_E = 18;
        private const int S_F = 33;
        private const int S_G = 34;
        private const int S_H = 35;
        private const int S_I = 23;
        private const int S_J = 36;
        private const int S_K = 37;
        private const int S_L = 38;
        private const int S_M = 39;
        private const int S_N = 49;
        private const int S_O = 24;
        private const int S_P = 25;
        private const int S_Q = 30;
        private const int S_R = 19;
        private const int S_S = 31;
        private const int S_T = 20;
        private const int S_U = 22;
        private const int S_V = 47;
        private const int S_W = 44;
        private const int S_X = 45;
        private const int S_Y = 21;
        private const int S_Z = 17;
        private const int S_LWIN = 91;
        private const int S_RWIN = 92;
        private const int S_APPS = 93;
        private const int S_SLEEP = 95;
        private const int S_NUMPAD0 = 82;
        private const int S_NUMPAD1 = 79;
        private const int S_NUMPAD2 = 80;
        private const int S_NUMPAD3 = 81;
        private const int S_NUMPAD4 = 75;
        private const int S_NUMPAD5 = 76;
        private const int S_NUMPAD6 = 77;
        private const int S_NUMPAD7 = 71;
        private const int S_NUMPAD8 = 72;
        private const int S_NUMPAD9 = 73;
        private const int S_MULTIPLY = 55;
        private const int S_ADD = 78;
        private const int S_SEPARATOR = 0;
        private const int S_SUBTRACT = 74;
        private const int S_DECIMAL = 83;
        private const int S_DIVIDE = 53;
        private const int S_F1 = 59;
        private const int S_F2 = 60;
        private const int S_F3 = 61;
        private const int S_F4 = 62;
        private const int S_F5 = 63;
        private const int S_F6 = 64;
        private const int S_F7 = 65;
        private const int S_F8 = 66;
        private const int S_F9 = 67;
        private const int S_F10 = 68;
        private const int S_F11 = 87;
        private const int S_F12 = 88;
        private const int S_F13 = 100;
        private const int S_F14 = 101;
        private const int S_F15 = 102;
        private const int S_F16 = 103;
        private const int S_F17 = 104;
        private const int S_F18 = 105;
        private const int S_F19 = 106;
        private const int S_F20 = 107;
        private const int S_F21 = 108;
        private const int S_F22 = 109;
        private const int S_F23 = 110;
        private const int S_F24 = 118;
        private const int S_NUMLOCK = 69;
        private const int S_SCROLL = 70;
        private const int S_LeftShift = 42;
        private const int S_RightShift = 54;
        private const int S_LeftControl = 29;
        private const int S_RightControl = 29;
        private const int S_LMENU = 56;
        private const int S_RMENU = 56;
        private const int S_BROWSER_BACK = 106;
        private const int S_BROWSER_FORWARD = 105;
        private const int S_BROWSER_REFRESH = 103;
        private const int S_BROWSER_STOP = 104;
        private const int S_BROWSER_SEARCH = 101;
        private const int S_BROWSER_FAVORITES = 102;
        private const int S_BROWSER_HOME = 50;
        private const int S_VOLUME_MUTE = 32;
        private const int S_VOLUME_DOWN = 46;
        private const int S_VOLUME_UP = 48;
        private const int S_MEDIA_NEXT_TRACK = 25;
        private const int S_MEDIA_PREV_TRACK = 16;
        private const int S_MEDIA_STOP = 36;
        private const int S_MEDIA_PLAY_PAUSE = 34;
        private const int S_LAUNCH_MAIL = 108;
        private const int S_LAUNCH_MEDIA_SELECT = 109;
        private const int S_LAUNCH_APP1 = 107;
        private const int S_LAUNCH_APP2 = 33;
        private const int S_OEM_1 = 27;
        private const int S_OEM_PLUS = 13;
        private const int S_OEM_COMMA = 50;
        private const int S_OEM_MINUS = 0;
        private const int S_OEM_PERIOD = 51;
        private const int S_OEM_2 = 52;
        private const int S_OEM_3 = 40;
        private const int S_OEM_4 = 12;
        private const int S_OEM_5 = 43;
        private const int S_OEM_6 = 26;
        private const int S_OEM_7 = 41;
        private const int S_OEM_8 = 53;
        private const int S_OEM_102 = 86;
        private const int S_PROCESSKEY = 0;
        private const int S_PACKET = 0;
        private const int S_ATTN = 0;
        private const int S_CRSEL = 0;
        private const int S_EXSEL = 0;
        private const int S_EREOF = 93;
        private const int S_PLAY = 0;
        private const int S_ZOOM = 98;
        private const int S_NONAME = 0;
        private const int S_PA1 = 0;
        private const int S_OEM_CLEAR = 0;
        public bool Key_LBUTTON;
        public bool Key_RBUTTON;
        public bool Key_CANCEL;
        public bool Key_MBUTTON;
        public bool Key_XBUTTON1;
        public bool Key_XBUTTON2;
        public bool Key_BACK;
        public bool Key_Tab;
        public bool Key_CLEAR;
        public bool Key_Return;
        public bool Key_SHIFT;
        public bool Key_CONTROL;
        public bool Key_MENU;
        public bool Key_PAUSE;
        public bool Key_CAPITAL;
        public bool Key_KANA;
        public bool Key_HANGEUL;
        public bool Key_HANGUL;
        public bool Key_JUNJA;
        public bool Key_FINAL;
        public bool Key_HANJA;
        public bool Key_KANJI;
        public bool Key_Escape;
        public bool Key_CONVERT;
        public bool Key_NONCONVERT;
        public bool Key_ACCEPT;
        public bool Key_MODECHANGE;
        public bool Key_Space;
        public bool Key_PRIOR;
        public bool Key_NEXT;
        public bool Key_END;
        public bool Key_HOME;
        public bool Key_LEFT;
        public bool Key_UP;
        public bool Key_RIGHT;
        public bool Key_DOWN;
        public bool Key_SELECT;
        public bool Key_PRINT;
        public bool Key_EXECUTE;
        public bool Key_SNAPSHOT;
        public bool Key_INSERT;
        public bool Key_DELETE;
        public bool Key_HELP;
        public bool Key_APOSTROPHE;
        public bool Key_0;
        public bool Key_1;
        public bool Key_2;
        public bool Key_3;
        public bool Key_4;
        public bool Key_5;
        public bool Key_6;
        public bool Key_7;
        public bool Key_8;
        public bool Key_9;
        public bool Key_A;
        public bool Key_B;
        public bool Key_C;
        public bool Key_D;
        public bool Key_E;
        public bool Key_F;
        public bool Key_G;
        public bool Key_H;
        public bool Key_I;
        public bool Key_J;
        public bool Key_K;
        public bool Key_L;
        public bool Key_M;
        public bool Key_N;
        public bool Key_O;
        public bool Key_P;
        public bool Key_Q;
        public bool Key_R;
        public bool Key_S;
        public bool Key_T;
        public bool Key_U;
        public bool Key_V;
        public bool Key_W;
        public bool Key_X;
        public bool Key_Y;
        public bool Key_Z;
        public bool Key_LWIN;
        public bool Key_RWIN;
        public bool Key_APPS;
        public bool Key_SLEEP;
        public bool Key_NUMPAD0;
        public bool Key_NUMPAD1;
        public bool Key_NUMPAD2;
        public bool Key_NUMPAD3;
        public bool Key_NUMPAD4;
        public bool Key_NUMPAD5;
        public bool Key_NUMPAD6;
        public bool Key_NUMPAD7;
        public bool Key_NUMPAD8;
        public bool Key_NUMPAD9;
        public bool Key_MULTIPLY;
        public bool Key_ADD;
        public bool Key_SEPARATOR;
        public bool Key_SUBTRACT;
        public bool Key_DECIMAL;
        public bool Key_DIVIDE;
        public bool Key_F1;
        public bool Key_F2;
        public bool Key_F3;
        public bool Key_F4;
        public bool Key_F5;
        public bool Key_F6;
        public bool Key_F7;
        public bool Key_F8;
        public bool Key_F9;
        public bool Key_F10;
        public bool Key_F11;
        public bool Key_F12;
        public bool Key_F13;
        public bool Key_F14;
        public bool Key_F15;
        public bool Key_F16;
        public bool Key_F17;
        public bool Key_F18;
        public bool Key_F19;
        public bool Key_F20;
        public bool Key_F21;
        public bool Key_F22;
        public bool Key_F23;
        public bool Key_F24;
        public bool Key_NUMLOCK;
        public bool Key_SCROLL;
        public bool Key_LeftShift;
        public bool Key_RightShift;
        public bool Key_LeftControl;
        public bool Key_RightControl;
        public bool Key_LMENU;
        public bool Key_RMENU;
        public bool Key_BROWSER_BACK;
        public bool Key_BROWSER_FORWARD;
        public bool Key_BROWSER_REFRESH;
        public bool Key_BROWSER_STOP;
        public bool Key_BROWSER_SEARCH;
        public bool Key_BROWSER_FAVORITES;
        public bool Key_BROWSER_HOME;
        public bool Key_VOLUME_MUTE;
        public bool Key_VOLUME_DOWN;
        public bool Key_VOLUME_UP;
        public bool Key_MEDIA_NEXT_TRACK;
        public bool Key_MEDIA_PREV_TRACK;
        public bool Key_MEDIA_STOP;
        public bool Key_MEDIA_PLAY_PAUSE;
        public bool Key_LAUNCH_MAIL;
        public bool Key_LAUNCH_MEDIA_SELECT;
        public bool Key_LAUNCH_APP1;
        public bool Key_LAUNCH_APP2;
        public bool Key_OEM_1;
        public bool Key_OEM_PLUS;
        public bool Key_OEM_COMMA;
        public bool Key_OEM_MINUS;
        public bool Key_OEM_PERIOD;
        public bool Key_OEM_2;
        public bool Key_OEM_3;
        public bool Key_OEM_4;
        public bool Key_OEM_5;
        public bool Key_OEM_6;
        public bool Key_OEM_7;
        public bool Key_OEM_8;
        public bool Key_OEM_102;
        public bool Key_PROCESSKEY;
        public bool Key_PACKET;
        public bool Key_ATTN;
        public bool Key_CRSEL;
        public bool Key_EXSEL;
        public bool Key_EREOF;
        public bool Key_PLAY;
        public bool Key_ZOOM;
        public bool Key_NONAME;
        public bool Key_PA1;
        public bool Key_OEM_CLEAR;
        public bool Scan(int number = 0)
        {
            this.number = number;
            Device.RegisterDevice(UsagePage.Generic, UsageId.GenericKeyboard, DeviceFlags.None);
            Device.KeyboardInput += Device_KeyboardInput;
            return true;
        }
        public void Init()
        {
        }
        private void Device_KeyboardInput(object sender, KeyboardInputEventArgs e)
        {
            if (e.State == KeyState.KeyDown)
            {
                if (e.MakeCode == S_LBUTTON)
                    Key_LBUTTON = true;
                if (e.MakeCode == S_RBUTTON)
                    Key_RBUTTON = true;
                if (e.MakeCode == S_CANCEL)
                    Key_CANCEL = true;
                if (e.MakeCode == S_MBUTTON)
                    Key_MBUTTON = true;
                if (e.MakeCode == S_XBUTTON1)
                    Key_XBUTTON1 = true;
                if (e.MakeCode == S_XBUTTON2)
                    Key_XBUTTON2 = true;
                if (e.MakeCode == S_BACK)
                    Key_BACK = true;
                if (e.MakeCode == S_Tab)
                    Key_Tab = true;
                if (e.MakeCode == S_CLEAR)
                    Key_CLEAR = true;
                if (e.MakeCode == S_Return)
                    Key_Return = true;
                if (e.MakeCode == S_SHIFT)
                    Key_SHIFT = true;
                if (e.MakeCode == S_CONTROL)
                    Key_CONTROL = true;
                if (e.MakeCode == S_MENU)
                    Key_MENU = true;
                if (e.MakeCode == S_PAUSE)
                    Key_PAUSE = true;
                if (e.MakeCode == S_CAPITAL)
                    Key_CAPITAL = true;
                if (e.MakeCode == S_KANA)
                    Key_KANA = true;
                if (e.MakeCode == S_HANGEUL)
                    Key_HANGEUL = true;
                if (e.MakeCode == S_HANGUL)
                    Key_HANGUL = true;
                if (e.MakeCode == S_JUNJA)
                    Key_JUNJA = true;
                if (e.MakeCode == S_FINAL)
                    Key_FINAL = true;
                if (e.MakeCode == S_HANJA)
                    Key_HANJA = true;
                if (e.MakeCode == S_KANJI)
                    Key_KANJI = true;
                if (e.MakeCode == S_Escape)
                    Key_Escape = true;
                if (e.MakeCode == S_CONVERT)
                    Key_CONVERT = true;
                if (e.MakeCode == S_NONCONVERT)
                    Key_NONCONVERT = true;
                if (e.MakeCode == S_ACCEPT)
                    Key_ACCEPT = true;
                if (e.MakeCode == S_MODECHANGE)
                    Key_MODECHANGE = true;
                if (e.MakeCode == S_Space)
                    Key_Space = true;
                if (e.MakeCode == S_PRIOR)
                    Key_PRIOR = true;
                if (e.MakeCode == S_NEXT)
                    Key_NEXT = true;
                if (e.MakeCode == S_END)
                    Key_END = true;
                if (e.MakeCode == S_HOME)
                    Key_HOME = true;
                if (e.MakeCode == S_LEFT)
                    Key_LEFT = true;
                if (e.MakeCode == S_UP)
                    Key_UP = true;
                if (e.MakeCode == S_RIGHT)
                    Key_RIGHT = true;
                if (e.MakeCode == S_DOWN)
                    Key_DOWN = true;
                if (e.MakeCode == S_SELECT)
                    Key_SELECT = true;
                if (e.MakeCode == S_PRINT)
                    Key_PRINT = true;
                if (e.MakeCode == S_EXECUTE)
                    Key_EXECUTE = true;
                if (e.MakeCode == S_SNAPSHOT)
                    Key_SNAPSHOT = true;
                if (e.MakeCode == S_INSERT)
                    Key_INSERT = true;
                if (e.MakeCode == S_DELETE)
                    Key_DELETE = true;
                if (e.MakeCode == S_HELP)
                    Key_HELP = true;
                if (e.MakeCode == S_APOSTROPHE)
                    Key_APOSTROPHE = true;
                if (e.MakeCode == S_0)
                    Key_0 = true;
                if (e.MakeCode == S_1)
                    Key_1 = true;
                if (e.MakeCode == S_2)
                    Key_2 = true;
                if (e.MakeCode == S_3)
                    Key_3 = true;
                if (e.MakeCode == S_4)
                    Key_4 = true;
                if (e.MakeCode == S_5)
                    Key_5 = true;
                if (e.MakeCode == S_6)
                    Key_6 = true;
                if (e.MakeCode == S_7)
                    Key_7 = true;
                if (e.MakeCode == S_8)
                    Key_8 = true;
                if (e.MakeCode == S_9)
                    Key_9 = true;
                if (e.MakeCode == S_A)
                    Key_A = true;
                if (e.MakeCode == S_B)
                    Key_B = true;
                if (e.MakeCode == S_C)
                    Key_C = true;
                if (e.MakeCode == S_D)
                    Key_D = true;
                if (e.MakeCode == S_E)
                    Key_E = true;
                if (e.MakeCode == S_F)
                    Key_F = true;
                if (e.MakeCode == S_G)
                    Key_G = true;
                if (e.MakeCode == S_H)
                    Key_H = true;
                if (e.MakeCode == S_I)
                    Key_I = true;
                if (e.MakeCode == S_J)
                    Key_J = true;
                if (e.MakeCode == S_K)
                    Key_K = true;
                if (e.MakeCode == S_L)
                    Key_L = true;
                if (e.MakeCode == S_M)
                    Key_M = true;
                if (e.MakeCode == S_N)
                    Key_N = true;
                if (e.MakeCode == S_O)
                    Key_O = true;
                if (e.MakeCode == S_P)
                    Key_P = true;
                if (e.MakeCode == S_Q)
                    Key_Q = true;
                if (e.MakeCode == S_R)
                    Key_R = true;
                if (e.MakeCode == S_S)
                    Key_S = true;
                if (e.MakeCode == S_T)
                    Key_T = true;
                if (e.MakeCode == S_U)
                    Key_U = true;
                if (e.MakeCode == S_V)
                    Key_V = true;
                if (e.MakeCode == S_W)
                    Key_W = true;
                if (e.MakeCode == S_X)
                    Key_X = true;
                if (e.MakeCode == S_Y)
                    Key_Y = true;
                if (e.MakeCode == S_Z)
                    Key_Z = true;
                if (e.MakeCode == S_LWIN)
                    Key_LWIN = true;
                if (e.MakeCode == S_RWIN)
                    Key_RWIN = true;
                if (e.MakeCode == S_APPS)
                    Key_APPS = true;
                if (e.MakeCode == S_SLEEP)
                    Key_SLEEP = true;
                if (e.MakeCode == S_NUMPAD0)
                    Key_NUMPAD0 = true;
                if (e.MakeCode == S_NUMPAD1)
                    Key_NUMPAD1 = true;
                if (e.MakeCode == S_NUMPAD2)
                    Key_NUMPAD2 = true;
                if (e.MakeCode == S_NUMPAD3)
                    Key_NUMPAD3 = true;
                if (e.MakeCode == S_NUMPAD4)
                    Key_NUMPAD4 = true;
                if (e.MakeCode == S_NUMPAD5)
                    Key_NUMPAD5 = true;
                if (e.MakeCode == S_NUMPAD6)
                    Key_NUMPAD6 = true;
                if (e.MakeCode == S_NUMPAD7)
                    Key_NUMPAD7 = true;
                if (e.MakeCode == S_NUMPAD8)
                    Key_NUMPAD8 = true;
                if (e.MakeCode == S_NUMPAD9)
                    Key_NUMPAD9 = true;
                if (e.MakeCode == S_MULTIPLY)
                    Key_MULTIPLY = true;
                if (e.MakeCode == S_ADD)
                    Key_ADD = true;
                if (e.MakeCode == S_SEPARATOR)
                    Key_SEPARATOR = true;
                if (e.MakeCode == S_SUBTRACT)
                    Key_SUBTRACT = true;
                if (e.MakeCode == S_DECIMAL)
                    Key_DECIMAL = true;
                if (e.MakeCode == S_DIVIDE)
                    Key_DIVIDE = true;
                if (e.MakeCode == S_F1)
                    Key_F1 = true;
                if (e.MakeCode == S_F2)
                    Key_F2 = true;
                if (e.MakeCode == S_F3)
                    Key_F3 = true;
                if (e.MakeCode == S_F4)
                    Key_F4 = true;
                if (e.MakeCode == S_F5)
                    Key_F5 = true;
                if (e.MakeCode == S_F6)
                    Key_F6 = true;
                if (e.MakeCode == S_F7)
                    Key_F7 = true;
                if (e.MakeCode == S_F8)
                    Key_F8 = true;
                if (e.MakeCode == S_F9)
                    Key_F9 = true;
                if (e.MakeCode == S_F10)
                    Key_F10 = true;
                if (e.MakeCode == S_F11)
                    Key_F11 = true;
                if (e.MakeCode == S_F12)
                    Key_F12 = true;
                if (e.MakeCode == S_F13)
                    Key_F13 = true;
                if (e.MakeCode == S_F14)
                    Key_F14 = true;
                if (e.MakeCode == S_F15)
                    Key_F15 = true;
                if (e.MakeCode == S_F16)
                    Key_F16 = true;
                if (e.MakeCode == S_F17)
                    Key_F17 = true;
                if (e.MakeCode == S_F18)
                    Key_F18 = true;
                if (e.MakeCode == S_F19)
                    Key_F19 = true;
                if (e.MakeCode == S_F20)
                    Key_F20 = true;
                if (e.MakeCode == S_F21)
                    Key_F21 = true;
                if (e.MakeCode == S_F22)
                    Key_F22 = true;
                if (e.MakeCode == S_F23)
                    Key_F23 = true;
                if (e.MakeCode == S_F24)
                    Key_F24 = true;
                if (e.MakeCode == S_NUMLOCK)
                    Key_NUMLOCK = true;
                if (e.MakeCode == S_SCROLL)
                    Key_SCROLL = true;
                if (e.MakeCode == S_LeftShift)
                    Key_LeftShift = true;
                if (e.MakeCode == S_RightShift)
                    Key_RightShift = true;
                if (e.MakeCode == S_LeftControl)
                    Key_LeftControl = true;
                if (e.MakeCode == S_RightControl)
                    Key_RightControl = true;
                if (e.MakeCode == S_LMENU)
                    Key_LMENU = true;
                if (e.MakeCode == S_RMENU)
                    Key_RMENU = true;
                if (e.MakeCode == S_BROWSER_BACK)
                    Key_BROWSER_BACK = true;
                if (e.MakeCode == S_BROWSER_FORWARD)
                    Key_BROWSER_FORWARD = true;
                if (e.MakeCode == S_BROWSER_REFRESH)
                    Key_BROWSER_REFRESH = true;
                if (e.MakeCode == S_BROWSER_STOP)
                    Key_BROWSER_STOP = true;
                if (e.MakeCode == S_BROWSER_SEARCH)
                    Key_BROWSER_SEARCH = true;
                if (e.MakeCode == S_BROWSER_FAVORITES)
                    Key_BROWSER_FAVORITES = true;
                if (e.MakeCode == S_BROWSER_HOME)
                    Key_BROWSER_HOME = true;
                if (e.MakeCode == S_VOLUME_MUTE)
                    Key_VOLUME_MUTE = true;
                if (e.MakeCode == S_VOLUME_DOWN)
                    Key_VOLUME_DOWN = true;
                if (e.MakeCode == S_VOLUME_UP)
                    Key_VOLUME_UP = true;
                if (e.MakeCode == S_MEDIA_NEXT_TRACK)
                    Key_MEDIA_NEXT_TRACK = true;
                if (e.MakeCode == S_MEDIA_PREV_TRACK)
                    Key_MEDIA_PREV_TRACK = true;
                if (e.MakeCode == S_MEDIA_STOP)
                    Key_MEDIA_STOP = true;
                if (e.MakeCode == S_MEDIA_PLAY_PAUSE)
                    Key_MEDIA_PLAY_PAUSE = true;
                if (e.MakeCode == S_LAUNCH_MAIL)
                    Key_LAUNCH_MAIL = true;
                if (e.MakeCode == S_LAUNCH_MEDIA_SELECT)
                    Key_LAUNCH_MEDIA_SELECT = true;
                if (e.MakeCode == S_LAUNCH_APP1)
                    Key_LAUNCH_APP1 = true;
                if (e.MakeCode == S_LAUNCH_APP2)
                    Key_LAUNCH_APP2 = true;
                if (e.MakeCode == S_OEM_1)
                    Key_OEM_1 = true;
                if (e.MakeCode == S_OEM_PLUS)
                    Key_OEM_PLUS = true;
                if (e.MakeCode == S_OEM_COMMA)
                    Key_OEM_COMMA = true;
                if (e.MakeCode == S_OEM_MINUS)
                    Key_OEM_MINUS = true;
                if (e.MakeCode == S_OEM_PERIOD)
                    Key_OEM_PERIOD = true;
                if (e.MakeCode == S_OEM_2)
                    Key_OEM_2 = true;
                if (e.MakeCode == S_OEM_3)
                    Key_OEM_3 = true;
                if (e.MakeCode == S_OEM_4)
                    Key_OEM_4 = true;
                if (e.MakeCode == S_OEM_5)
                    Key_OEM_5 = true;
                if (e.MakeCode == S_OEM_6)
                    Key_OEM_6 = true;
                if (e.MakeCode == S_OEM_7)
                    Key_OEM_7 = true;
                if (e.MakeCode == S_OEM_8)
                    Key_OEM_8 = true;
                if (e.MakeCode == S_OEM_102)
                    Key_OEM_102 = true;
                if (e.MakeCode == S_PROCESSKEY)
                    Key_PROCESSKEY = true;
                if (e.MakeCode == S_PACKET)
                    Key_PACKET = true;
                if (e.MakeCode == S_ATTN)
                    Key_ATTN = true;
                if (e.MakeCode == S_CRSEL)
                    Key_CRSEL = true;
                if (e.MakeCode == S_EXSEL)
                    Key_EXSEL = true;
                if (e.MakeCode == S_EREOF)
                    Key_EREOF = true;
                if (e.MakeCode == S_PLAY)
                    Key_PLAY = true;
                if (e.MakeCode == S_ZOOM)
                    Key_ZOOM = true;
                if (e.MakeCode == S_NONAME)
                    Key_NONAME = true;
                if (e.MakeCode == S_PA1)
                    Key_PA1 = true;
                if (e.MakeCode == S_OEM_CLEAR)
                    Key_OEM_CLEAR = true;
            }
            if (e.State == KeyState.KeyUp)
            {
                if (e.MakeCode == S_LBUTTON)
                    Key_LBUTTON = false;
                if (e.MakeCode == S_RBUTTON)
                    Key_RBUTTON = false;
                if (e.MakeCode == S_CANCEL)
                    Key_CANCEL = false;
                if (e.MakeCode == S_MBUTTON)
                    Key_MBUTTON = false;
                if (e.MakeCode == S_XBUTTON1)
                    Key_XBUTTON1 = false;
                if (e.MakeCode == S_XBUTTON2)
                    Key_XBUTTON2 = false;
                if (e.MakeCode == S_BACK)
                    Key_BACK = false;
                if (e.MakeCode == S_Tab)
                    Key_Tab = false;
                if (e.MakeCode == S_CLEAR)
                    Key_CLEAR = false;
                if (e.MakeCode == S_Return)
                    Key_Return = false;
                if (e.MakeCode == S_SHIFT)
                    Key_SHIFT = false;
                if (e.MakeCode == S_CONTROL)
                    Key_CONTROL = false;
                if (e.MakeCode == S_MENU)
                    Key_MENU = false;
                if (e.MakeCode == S_PAUSE)
                    Key_PAUSE = false;
                if (e.MakeCode == S_CAPITAL)
                    Key_CAPITAL = false;
                if (e.MakeCode == S_KANA)
                    Key_KANA = false;
                if (e.MakeCode == S_HANGEUL)
                    Key_HANGEUL = false;
                if (e.MakeCode == S_HANGUL)
                    Key_HANGUL = false;
                if (e.MakeCode == S_JUNJA)
                    Key_JUNJA = false;
                if (e.MakeCode == S_FINAL)
                    Key_FINAL = false;
                if (e.MakeCode == S_HANJA)
                    Key_HANJA = false;
                if (e.MakeCode == S_KANJI)
                    Key_KANJI = false;
                if (e.MakeCode == S_Escape)
                    Key_Escape = false;
                if (e.MakeCode == S_CONVERT)
                    Key_CONVERT = false;
                if (e.MakeCode == S_NONCONVERT)
                    Key_NONCONVERT = false;
                if (e.MakeCode == S_ACCEPT)
                    Key_ACCEPT = false;
                if (e.MakeCode == S_MODECHANGE)
                    Key_MODECHANGE = false;
                if (e.MakeCode == S_Space)
                    Key_Space = false;
                if (e.MakeCode == S_PRIOR)
                    Key_PRIOR = false;
                if (e.MakeCode == S_NEXT)
                    Key_NEXT = false;
                if (e.MakeCode == S_END)
                    Key_END = false;
                if (e.MakeCode == S_HOME)
                    Key_HOME = false;
                if (e.MakeCode == S_LEFT)
                    Key_LEFT = false;
                if (e.MakeCode == S_UP)
                    Key_UP = false;
                if (e.MakeCode == S_RIGHT)
                    Key_RIGHT = false;
                if (e.MakeCode == S_DOWN)
                    Key_DOWN = false;
                if (e.MakeCode == S_SELECT)
                    Key_SELECT = false;
                if (e.MakeCode == S_PRINT)
                    Key_PRINT = false;
                if (e.MakeCode == S_EXECUTE)
                    Key_EXECUTE = false;
                if (e.MakeCode == S_SNAPSHOT)
                    Key_SNAPSHOT = false;
                if (e.MakeCode == S_INSERT)
                    Key_INSERT = false;
                if (e.MakeCode == S_DELETE)
                    Key_DELETE = false;
                if (e.MakeCode == S_HELP)
                    Key_HELP = false;
                if (e.MakeCode == S_APOSTROPHE)
                    Key_APOSTROPHE = false;
                if (e.MakeCode == S_0)
                    Key_0 = false;
                if (e.MakeCode == S_1)
                    Key_1 = false;
                if (e.MakeCode == S_2)
                    Key_2 = false;
                if (e.MakeCode == S_3)
                    Key_3 = false;
                if (e.MakeCode == S_4)
                    Key_4 = false;
                if (e.MakeCode == S_5)
                    Key_5 = false;
                if (e.MakeCode == S_6)
                    Key_6 = false;
                if (e.MakeCode == S_7)
                    Key_7 = false;
                if (e.MakeCode == S_8)
                    Key_8 = false;
                if (e.MakeCode == S_9)
                    Key_9 = false;
                if (e.MakeCode == S_A)
                    Key_A = false;
                if (e.MakeCode == S_B)
                    Key_B = false;
                if (e.MakeCode == S_C)
                    Key_C = false;
                if (e.MakeCode == S_D)
                    Key_D = false;
                if (e.MakeCode == S_E)
                    Key_E = false;
                if (e.MakeCode == S_F)
                    Key_F = false;
                if (e.MakeCode == S_G)
                    Key_G = false;
                if (e.MakeCode == S_H)
                    Key_H = false;
                if (e.MakeCode == S_I)
                    Key_I = false;
                if (e.MakeCode == S_J)
                    Key_J = false;
                if (e.MakeCode == S_K)
                    Key_K = false;
                if (e.MakeCode == S_L)
                    Key_L = false;
                if (e.MakeCode == S_M)
                    Key_M = false;
                if (e.MakeCode == S_N)
                    Key_N = false;
                if (e.MakeCode == S_O)
                    Key_O = false;
                if (e.MakeCode == S_P)
                    Key_P = false;
                if (e.MakeCode == S_Q)
                    Key_Q = false;
                if (e.MakeCode == S_R)
                    Key_R = false;
                if (e.MakeCode == S_S)
                    Key_S = false;
                if (e.MakeCode == S_T)
                    Key_T = false;
                if (e.MakeCode == S_U)
                    Key_U = false;
                if (e.MakeCode == S_V)
                    Key_V = false;
                if (e.MakeCode == S_W)
                    Key_W = false;
                if (e.MakeCode == S_X)
                    Key_X = false;
                if (e.MakeCode == S_Y)
                    Key_Y = false;
                if (e.MakeCode == S_Z)
                    Key_Z = false;
                if (e.MakeCode == S_LWIN)
                    Key_LWIN = false;
                if (e.MakeCode == S_RWIN)
                    Key_RWIN = false;
                if (e.MakeCode == S_APPS)
                    Key_APPS = false;
                if (e.MakeCode == S_SLEEP)
                    Key_SLEEP = false;
                if (e.MakeCode == S_NUMPAD0)
                    Key_NUMPAD0 = false;
                if (e.MakeCode == S_NUMPAD1)
                    Key_NUMPAD1 = false;
                if (e.MakeCode == S_NUMPAD2)
                    Key_NUMPAD2 = false;
                if (e.MakeCode == S_NUMPAD3)
                    Key_NUMPAD3 = false;
                if (e.MakeCode == S_NUMPAD4)
                    Key_NUMPAD4 = false;
                if (e.MakeCode == S_NUMPAD5)
                    Key_NUMPAD5 = false;
                if (e.MakeCode == S_NUMPAD6)
                    Key_NUMPAD6 = false;
                if (e.MakeCode == S_NUMPAD7)
                    Key_NUMPAD7 = false;
                if (e.MakeCode == S_NUMPAD8)
                    Key_NUMPAD8 = false;
                if (e.MakeCode == S_NUMPAD9)
                    Key_NUMPAD9 = false;
                if (e.MakeCode == S_MULTIPLY)
                    Key_MULTIPLY = false;
                if (e.MakeCode == S_ADD)
                    Key_ADD = false;
                if (e.MakeCode == S_SEPARATOR)
                    Key_SEPARATOR = false;
                if (e.MakeCode == S_SUBTRACT)
                    Key_SUBTRACT = false;
                if (e.MakeCode == S_DECIMAL)
                    Key_DECIMAL = false;
                if (e.MakeCode == S_DIVIDE)
                    Key_DIVIDE = false;
                if (e.MakeCode == S_F1)
                    Key_F1 = false;
                if (e.MakeCode == S_F2)
                    Key_F2 = false;
                if (e.MakeCode == S_F3)
                    Key_F3 = false;
                if (e.MakeCode == S_F4)
                    Key_F4 = false;
                if (e.MakeCode == S_F5)
                    Key_F5 = false;
                if (e.MakeCode == S_F6)
                    Key_F6 = false;
                if (e.MakeCode == S_F7)
                    Key_F7 = false;
                if (e.MakeCode == S_F8)
                    Key_F8 = false;
                if (e.MakeCode == S_F9)
                    Key_F9 = false;
                if (e.MakeCode == S_F10)
                    Key_F10 = false;
                if (e.MakeCode == S_F11)
                    Key_F11 = false;
                if (e.MakeCode == S_F12)
                    Key_F12 = false;
                if (e.MakeCode == S_F13)
                    Key_F13 = false;
                if (e.MakeCode == S_F14)
                    Key_F14 = false;
                if (e.MakeCode == S_F15)
                    Key_F15 = false;
                if (e.MakeCode == S_F16)
                    Key_F16 = false;
                if (e.MakeCode == S_F17)
                    Key_F17 = false;
                if (e.MakeCode == S_F18)
                    Key_F18 = false;
                if (e.MakeCode == S_F19)
                    Key_F19 = false;
                if (e.MakeCode == S_F20)
                    Key_F20 = false;
                if (e.MakeCode == S_F21)
                    Key_F21 = false;
                if (e.MakeCode == S_F22)
                    Key_F22 = false;
                if (e.MakeCode == S_F23)
                    Key_F23 = false;
                if (e.MakeCode == S_F24)
                    Key_F24 = false;
                if (e.MakeCode == S_NUMLOCK)
                    Key_NUMLOCK = false;
                if (e.MakeCode == S_SCROLL)
                    Key_SCROLL = false;
                if (e.MakeCode == S_LeftShift)
                    Key_LeftShift = false;
                if (e.MakeCode == S_RightShift)
                    Key_RightShift = false;
                if (e.MakeCode == S_LeftControl)
                    Key_LeftControl = false;
                if (e.MakeCode == S_RightControl)
                    Key_RightControl = false;
                if (e.MakeCode == S_LMENU)
                    Key_LMENU = false;
                if (e.MakeCode == S_RMENU)
                    Key_RMENU = false;
                if (e.MakeCode == S_BROWSER_BACK)
                    Key_BROWSER_BACK = false;
                if (e.MakeCode == S_BROWSER_FORWARD)
                    Key_BROWSER_FORWARD = false;
                if (e.MakeCode == S_BROWSER_REFRESH)
                    Key_BROWSER_REFRESH = false;
                if (e.MakeCode == S_BROWSER_STOP)
                    Key_BROWSER_STOP = false;
                if (e.MakeCode == S_BROWSER_SEARCH)
                    Key_BROWSER_SEARCH = false;
                if (e.MakeCode == S_BROWSER_FAVORITES)
                    Key_BROWSER_FAVORITES = false;
                if (e.MakeCode == S_BROWSER_HOME)
                    Key_BROWSER_HOME = false;
                if (e.MakeCode == S_VOLUME_MUTE)
                    Key_VOLUME_MUTE = false;
                if (e.MakeCode == S_VOLUME_DOWN)
                    Key_VOLUME_DOWN = false;
                if (e.MakeCode == S_VOLUME_UP)
                    Key_VOLUME_UP = false;
                if (e.MakeCode == S_MEDIA_NEXT_TRACK)
                    Key_MEDIA_NEXT_TRACK = false;
                if (e.MakeCode == S_MEDIA_PREV_TRACK)
                    Key_MEDIA_PREV_TRACK = false;
                if (e.MakeCode == S_MEDIA_STOP)
                    Key_MEDIA_STOP = false;
                if (e.MakeCode == S_MEDIA_PLAY_PAUSE)
                    Key_MEDIA_PLAY_PAUSE = false;
                if (e.MakeCode == S_LAUNCH_MAIL)
                    Key_LAUNCH_MAIL = false;
                if (e.MakeCode == S_LAUNCH_MEDIA_SELECT)
                    Key_LAUNCH_MEDIA_SELECT = false;
                if (e.MakeCode == S_LAUNCH_APP1)
                    Key_LAUNCH_APP1 = false;
                if (e.MakeCode == S_LAUNCH_APP2)
                    Key_LAUNCH_APP2 = false;
                if (e.MakeCode == S_OEM_1)
                    Key_OEM_1 = false;
                if (e.MakeCode == S_OEM_PLUS)
                    Key_OEM_PLUS = false;
                if (e.MakeCode == S_OEM_COMMA)
                    Key_OEM_COMMA = false;
                if (e.MakeCode == S_OEM_MINUS)
                    Key_OEM_MINUS = false;
                if (e.MakeCode == S_OEM_PERIOD)
                    Key_OEM_PERIOD = false;
                if (e.MakeCode == S_OEM_2)
                    Key_OEM_2 = false;
                if (e.MakeCode == S_OEM_3)
                    Key_OEM_3 = false;
                if (e.MakeCode == S_OEM_4)
                    Key_OEM_4 = false;
                if (e.MakeCode == S_OEM_5)
                    Key_OEM_5 = false;
                if (e.MakeCode == S_OEM_6)
                    Key_OEM_6 = false;
                if (e.MakeCode == S_OEM_7)
                    Key_OEM_7 = false;
                if (e.MakeCode == S_OEM_8)
                    Key_OEM_8 = false;
                if (e.MakeCode == S_OEM_102)
                    Key_OEM_102 = false;
                if (e.MakeCode == S_PROCESSKEY)
                    Key_PROCESSKEY = false;
                if (e.MakeCode == S_PACKET)
                    Key_PACKET = false;
                if (e.MakeCode == S_ATTN)
                    Key_ATTN = false;
                if (e.MakeCode == S_CRSEL)
                    Key_CRSEL = false;
                if (e.MakeCode == S_EXSEL)
                    Key_EXSEL = false;
                if (e.MakeCode == S_EREOF)
                    Key_EREOF = false;
                if (e.MakeCode == S_PLAY)
                    Key_PLAY = false;
                if (e.MakeCode == S_ZOOM)
                    Key_ZOOM = false;
                if (e.MakeCode == S_NONAME)
                    Key_NONAME = false;
                if (e.MakeCode == S_PA1)
                    Key_PA1 = false;
                if (e.MakeCode == S_OEM_CLEAR)
                    Key_OEM_CLEAR = false;
            }
        }
    }
}