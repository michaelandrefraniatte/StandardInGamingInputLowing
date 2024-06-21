using System;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using System.Diagnostics;
using Valuechanges;
using sendinputs;

namespace SendInputs
{
    public class Valuechanges
    {
        public double[] _valuechange = { 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2 };
        public double[] _ValueChange = { 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2 };
        public double this[int index]
        {
            get { return _ValueChange[index]; }
            set
            {
                if (_valuechange[index] != value)
                    _ValueChange[index] = value - _valuechange[index];
                else
                    _ValueChange[index] = 0;
                _valuechange[index] = value;
            }
        }
    }
    public class Sendinput : IDisposable
    {
        [DllImport("user32.dll")]
        private static extern void SendInput(uint numberOfInputs, INPUT[] inputs, int sizeOfInputStructure);
        [DllImport("user32.dll")]
        private static extern void mouse_event(uint dwFlags, int dx, int dy, int dwData, int dwExtraInfo);
        [DllImport("user32.dll", SetLastError = true)]
        private static extern void keybd_event(uint bVk, uint bScan, uint dwFlags, int dwExtraInfo);
        [DllImport("user32.dll")]
        private static extern void SetPhysicalCursorPos(int X, int Y);
        [DllImport("user32.dll")]
        private static extern void SetCaretPos(int X, int Y);
        [DllImport("user32.dll")]
        private static extern void SetCursorPos(int X, int Y);
        [DllImport("winmm.dll", EntryPoint = "timeBeginPeriod")]
        private static extern uint TimeBeginPeriod(uint ms);
        [DllImport("winmm.dll", EntryPoint = "timeEndPeriod")]
        private static extern uint TimeEndPeriod(uint ms);
        [DllImport("ntdll.dll", EntryPoint = "NtSetTimerResolution")]
        private static extern void NtSetTimerResolution(uint DesiredResolution, bool SetResolution, ref uint CurrentResolution);
        private static uint CurrentResolution = 0;
        private const ushort VK_LBUTTON = (ushort)0x01;
        private const ushort VK_RBUTTON = (ushort)0x02;
        private const ushort VK_CANCEL = (ushort)0x03;
        private const ushort VK_MBUTTON = (ushort)0x04;
        private const ushort VK_XBUTTON1 = (ushort)0x05;
        private const ushort VK_XBUTTON2 = (ushort)0x06;
        private const ushort VK_BACK = (ushort)0x08;
        private const ushort VK_Tab = (ushort)0x09;
        private const ushort VK_CLEAR = (ushort)0x0C;
        private const ushort VK_Return = (ushort)0x0D;
        private const ushort VK_SHIFT = (ushort)0x10;
        private const ushort VK_CONTROL = (ushort)0x11;
        private const ushort VK_MENU = (ushort)0x12;
        private const ushort VK_PAUSE = (ushort)0x13;
        private const ushort VK_CAPITAL = (ushort)0x14;
        private const ushort VK_KANA = (ushort)0x15;
        private const ushort VK_HANGEUL = (ushort)0x15;
        private const ushort VK_HANGUL = (ushort)0x15;
        private const ushort VK_JUNJA = (ushort)0x17;
        private const ushort VK_FINAL = (ushort)0x18;
        private const ushort VK_HANJA = (ushort)0x19;
        private const ushort VK_KANJI = (ushort)0x19;
        private const ushort VK_Escape = (ushort)0x1B;
        private const ushort VK_CONVERT = (ushort)0x1C;
        private const ushort VK_NONCONVERT = (ushort)0x1D;
        private const ushort VK_ACCEPT = (ushort)0x1E;
        private const ushort VK_MODECHANGE = (ushort)0x1F;
        private const ushort VK_Space = (ushort)0x20;
        private const ushort VK_PRIOR = (ushort)0x21;
        private const ushort VK_NEXT = (ushort)0x22;
        private const ushort VK_END = (ushort)0x23;
        private const ushort VK_HOME = (ushort)0x24;
        private const ushort VK_LEFT = (ushort)0x25;
        private const ushort VK_UP = (ushort)0x26;
        private const ushort VK_RIGHT = (ushort)0x27;
        private const ushort VK_DOWN = (ushort)0x28;
        private const ushort VK_SELECT = (ushort)0x29;
        private const ushort VK_PRINT = (ushort)0x2A;
        private const ushort VK_EXECUTE = (ushort)0x2B;
        private const ushort VK_SNAPSHOT = (ushort)0x2C;
        private const ushort VK_INSERT = (ushort)0x2D;
        private const ushort VK_DELETE = (ushort)0x2E;
        private const ushort VK_HELP = (ushort)0x2F;
        private const ushort VK_APOSTROPHE = (ushort)0xDE;
        private const ushort VK_0 = (ushort)0x30;
        private const ushort VK_1 = (ushort)0x31;
        private const ushort VK_2 = (ushort)0x32;
        private const ushort VK_3 = (ushort)0x33;
        private const ushort VK_4 = (ushort)0x34;
        private const ushort VK_5 = (ushort)0x35;
        private const ushort VK_6 = (ushort)0x36;
        private const ushort VK_7 = (ushort)0x37;
        private const ushort VK_8 = (ushort)0x38;
        private const ushort VK_9 = (ushort)0x39;
        private const ushort VK_A = (ushort)0x41;
        private const ushort VK_B = (ushort)0x42;
        private const ushort VK_C = (ushort)0x43;
        private const ushort VK_D = (ushort)0x44;
        private const ushort VK_E = (ushort)0x45;
        private const ushort VK_F = (ushort)0x46;
        private const ushort VK_G = (ushort)0x47;
        private const ushort VK_H = (ushort)0x48;
        private const ushort VK_I = (ushort)0x49;
        private const ushort VK_J = (ushort)0x4A;
        private const ushort VK_K = (ushort)0x4B;
        private const ushort VK_L = (ushort)0x4C;
        private const ushort VK_M = (ushort)0x4D;
        private const ushort VK_N = (ushort)0x4E;
        private const ushort VK_O = (ushort)0x4F;
        private const ushort VK_P = (ushort)0x50;
        private const ushort VK_Q = (ushort)0x51;
        private const ushort VK_R = (ushort)0x52;
        private const ushort VK_S = (ushort)0x53;
        private const ushort VK_T = (ushort)0x54;
        private const ushort VK_U = (ushort)0x55;
        private const ushort VK_V = (ushort)0x56;
        private const ushort VK_W = (ushort)0x57;
        private const ushort VK_X = (ushort)0x58;
        private const ushort VK_Y = (ushort)0x59;
        private const ushort VK_Z = (ushort)0x5A;
        private const ushort VK_LWIN = (ushort)0x5B;
        private const ushort VK_RWIN = (ushort)0x5C;
        private const ushort VK_APPS = (ushort)0x5D;
        private const ushort VK_SLEEP = (ushort)0x5F;
        private const ushort VK_NUMPAD0 = (ushort)0x60;
        private const ushort VK_NUMPAD1 = (ushort)0x61;
        private const ushort VK_NUMPAD2 = (ushort)0x62;
        private const ushort VK_NUMPAD3 = (ushort)0x63;
        private const ushort VK_NUMPAD4 = (ushort)0x64;
        private const ushort VK_NUMPAD5 = (ushort)0x65;
        private const ushort VK_NUMPAD6 = (ushort)0x66;
        private const ushort VK_NUMPAD7 = (ushort)0x67;
        private const ushort VK_NUMPAD8 = (ushort)0x68;
        private const ushort VK_NUMPAD9 = (ushort)0x69;
        private const ushort VK_MULTIPLY = (ushort)0x6A;
        private const ushort VK_ADD = (ushort)0x6B;
        private const ushort VK_SEPARATOR = (ushort)0x6C;
        private const ushort VK_SUBTRACT = (ushort)0x6D;
        private const ushort VK_DECIMAL = (ushort)0x6E;
        private const ushort VK_DIVIDE = (ushort)0x6F;
        private const ushort VK_F1 = (ushort)0x70;
        private const ushort VK_F2 = (ushort)0x71;
        private const ushort VK_F3 = (ushort)0x72;
        private const ushort VK_F4 = (ushort)0x73;
        private const ushort VK_F5 = (ushort)0x74;
        private const ushort VK_F6 = (ushort)0x75;
        private const ushort VK_F7 = (ushort)0x76;
        private const ushort VK_F8 = (ushort)0x77;
        private const ushort VK_F9 = (ushort)0x78;
        private const ushort VK_F10 = (ushort)0x79;
        private const ushort VK_F11 = (ushort)0x7A;
        private const ushort VK_F12 = (ushort)0x7B;
        private const ushort VK_F13 = (ushort)0x7C;
        private const ushort VK_F14 = (ushort)0x7D;
        private const ushort VK_F15 = (ushort)0x7E;
        private const ushort VK_F16 = (ushort)0x7F;
        private const ushort VK_F17 = (ushort)0x80;
        private const ushort VK_F18 = (ushort)0x81;
        private const ushort VK_F19 = (ushort)0x82;
        private const ushort VK_F20 = (ushort)0x83;
        private const ushort VK_F21 = (ushort)0x84;
        private const ushort VK_F22 = (ushort)0x85;
        private const ushort VK_F23 = (ushort)0x86;
        private const ushort VK_F24 = (ushort)0x87;
        private const ushort VK_NUMLOCK = (ushort)0x90;
        private const ushort VK_SCROLL = (ushort)0x91;
        private const ushort VK_LeftShift = (ushort)0xA0;
        private const ushort VK_RightShift = (ushort)0xA1;
        private const ushort VK_LeftControl = (ushort)0xA2;
        private const ushort VK_RightControl = (ushort)0xA3;
        private const ushort VK_LMENU = (ushort)0xA4;
        private const ushort VK_RMENU = (ushort)0xA5;
        private const ushort VK_BROWSER_BACK = (ushort)0xA6;
        private const ushort VK_BROWSER_FORWARD = (ushort)0xA7;
        private const ushort VK_BROWSER_REFRESH = (ushort)0xA8;
        private const ushort VK_BROWSER_STOP = (ushort)0xA9;
        private const ushort VK_BROWSER_SEARCH = (ushort)0xAA;
        private const ushort VK_BROWSER_FAVORITES = (ushort)0xAB;
        private const ushort VK_BROWSER_HOME = (ushort)0xAC;
        private const ushort VK_VOLUME_MUTE = (ushort)0xAD;
        private const ushort VK_VOLUME_DOWN = (ushort)0xAE;
        private const ushort VK_VOLUME_UP = (ushort)0xAF;
        private const ushort VK_MEDIA_NEXT_TRACK = (ushort)0xB0;
        private const ushort VK_MEDIA_PREV_TRACK = (ushort)0xB1;
        private const ushort VK_MEDIA_STOP = (ushort)0xB2;
        private const ushort VK_MEDIA_PLAY_PAUSE = (ushort)0xB3;
        private const ushort VK_LAUNCH_MAIL = (ushort)0xB4;
        private const ushort VK_LAUNCH_MEDIA_SELECT = (ushort)0xB5;
        private const ushort VK_LAUNCH_APP1 = (ushort)0xB6;
        private const ushort VK_LAUNCH_APP2 = (ushort)0xB7;
        private const ushort VK_OEM_1 = (ushort)0xBA;
        private const ushort VK_OEM_PLUS = (ushort)0xBB;
        private const ushort VK_OEM_COMMA = (ushort)0xBC;
        private const ushort VK_OEM_MINUS = (ushort)0xBD;
        private const ushort VK_OEM_PERIOD = (ushort)0xBE;
        private const ushort VK_OEM_2 = (ushort)0xBF;
        private const ushort VK_OEM_3 = (ushort)0xC0;
        private const ushort VK_OEM_4 = (ushort)0xDB;
        private const ushort VK_OEM_5 = (ushort)0xDC;
        private const ushort VK_OEM_6 = (ushort)0xDD;
        private const ushort VK_OEM_7 = (ushort)0xDE;
        private const ushort VK_OEM_8 = (ushort)0xDF;
        private const ushort VK_OEM_102 = (ushort)0xE2;
        private const ushort VK_PROCESSKEY = (ushort)0xE5;
        private const ushort VK_PACKET = (ushort)0xE7;
        private const ushort VK_ATTN = (ushort)0xF6;
        private const ushort VK_CRSEL = (ushort)0xF7;
        private const ushort VK_EXSEL = (ushort)0xF8;
        private const ushort VK_EREOF = (ushort)0xF9;
        private const ushort VK_PLAY = (ushort)0xFA;
        private const ushort VK_ZOOM = (ushort)0xFB;
        private const ushort VK_NONAME = (ushort)0xFC;
        private const ushort VK_PA1 = (ushort)0xFD;
        private const ushort VK_OEM_CLEAR = (ushort)0xFE;
        private const ushort S_LBUTTON = (ushort)0x0;
        private const ushort S_RBUTTON = 0;
        private const ushort S_CANCEL = 70;
        private const ushort S_MBUTTON = 0;
        private const ushort S_XBUTTON1 = 0;
        private const ushort S_XBUTTON2 = 0;
        private const ushort S_BACK = 14;
        private const ushort S_Tab = 15;
        private const ushort S_CLEAR = 76;
        private const ushort S_Return = 28;
        private const ushort S_SHIFT = 42;
        private const ushort S_CONTROL = 29;
        private const ushort S_MENU = 56;
        private const ushort S_PAUSE = 0;
        private const ushort S_CAPITAL = 58;
        private const ushort S_KANA = 0;
        private const ushort S_HANGEUL = 0;
        private const ushort S_HANGUL = 0;
        private const ushort S_JUNJA = 0;
        private const ushort S_FINAL = 0;
        private const ushort S_HANJA = 0;
        private const ushort S_KANJI = 0;
        private const ushort S_Escape = 1;
        private const ushort S_CONVERT = 0;
        private const ushort S_NONCONVERT = 0;
        private const ushort S_ACCEPT = 0;
        private const ushort S_MODECHANGE = 0;
        private const ushort S_Space = 57;
        private const ushort S_PRIOR = 73;
        private const ushort S_NEXT = 81;
        private const ushort S_END = 79;
        private const ushort S_HOME = 71;
        private const ushort S_LEFT = 75;
        private const ushort S_UP = 72;
        private const ushort S_RIGHT = 77;
        private const ushort S_DOWN = 80;
        private const ushort S_SELECT = 0;
        private const ushort S_PRINT = 0;
        private const ushort S_EXECUTE = 0;
        private const ushort S_SNAPSHOT = 84;
        private const ushort S_INSERT = 82;
        private const ushort S_DELETE = 83;
        private const ushort S_HELP = 99;
        private const ushort S_APOSTROPHE = 41;
        private const ushort S_0 = 11;
        private const ushort S_1 = 2;
        private const ushort S_2 = 3;
        private const ushort S_3 = 4;
        private const ushort S_4 = 5;
        private const ushort S_5 = 6;
        private const ushort S_6 = 7;
        private const ushort S_7 = 8;
        private const ushort S_8 = 9;
        private const ushort S_9 = 10;
        private const ushort S_A = 16;
        private const ushort S_B = 48;
        private const ushort S_C = 46;
        private const ushort S_D = 32;
        private const ushort S_E = 18;
        private const ushort S_F = 33;
        private const ushort S_G = 34;
        private const ushort S_H = 35;
        private const ushort S_I = 23;
        private const ushort S_J = 36;
        private const ushort S_K = 37;
        private const ushort S_L = 38;
        private const ushort S_M = 39;
        private const ushort S_N = 49;
        private const ushort S_O = 24;
        private const ushort S_P = 25;
        private const ushort S_Q = 30;
        private const ushort S_R = 19;
        private const ushort S_S = 31;
        private const ushort S_T = 20;
        private const ushort S_U = 22;
        private const ushort S_V = 47;
        private const ushort S_W = 44;
        private const ushort S_X = 45;
        private const ushort S_Y = 21;
        private const ushort S_Z = 17;
        private const ushort S_LWIN = 91;
        private const ushort S_RWIN = 92;
        private const ushort S_APPS = 93;
        private const ushort S_SLEEP = 95;
        private const ushort S_NUMPAD0 = 82;
        private const ushort S_NUMPAD1 = 79;
        private const ushort S_NUMPAD2 = 80;
        private const ushort S_NUMPAD3 = 81;
        private const ushort S_NUMPAD4 = 75;
        private const ushort S_NUMPAD5 = 76;
        private const ushort S_NUMPAD6 = 77;
        private const ushort S_NUMPAD7 = 71;
        private const ushort S_NUMPAD8 = 72;
        private const ushort S_NUMPAD9 = 73;
        private const ushort S_MULTIPLY = 55;
        private const ushort S_ADD = 78;
        private const ushort S_SEPARATOR = 0;
        private const ushort S_SUBTRACT = 74;
        private const ushort S_DECIMAL = 83;
        private const ushort S_DIVIDE = 53;
        private const ushort S_F1 = 59;
        private const ushort S_F2 = 60;
        private const ushort S_F3 = 61;
        private const ushort S_F4 = 62;
        private const ushort S_F5 = 63;
        private const ushort S_F6 = 64;
        private const ushort S_F7 = 65;
        private const ushort S_F8 = 66;
        private const ushort S_F9 = 67;
        private const ushort S_F10 = 68;
        private const ushort S_F11 = 87;
        private const ushort S_F12 = 88;
        private const ushort S_F13 = 100;
        private const ushort S_F14 = 101;
        private const ushort S_F15 = 102;
        private const ushort S_F16 = 103;
        private const ushort S_F17 = 104;
        private const ushort S_F18 = 105;
        private const ushort S_F19 = 106;
        private const ushort S_F20 = 107;
        private const ushort S_F21 = 108;
        private const ushort S_F22 = 109;
        private const ushort S_F23 = 110;
        private const ushort S_F24 = 118;
        private const ushort S_NUMLOCK = 69;
        private const ushort S_SCROLL = 70;
        private const ushort S_LeftShift = 42;
        private const ushort S_RightShift = 54;
        private const ushort S_LeftControl = 29;
        private const ushort S_RightControl = 29;
        private const ushort S_LMENU = 56;
        private const ushort S_RMENU = 56;
        private const ushort S_BROWSER_BACK = 106;
        private const ushort S_BROWSER_FORWARD = 105;
        private const ushort S_BROWSER_REFRESH = 103;
        private const ushort S_BROWSER_STOP = 104;
        private const ushort S_BROWSER_SEARCH = 101;
        private const ushort S_BROWSER_FAVORITES = 102;
        private const ushort S_BROWSER_HOME = 50;
        private const ushort S_VOLUME_MUTE = 32;
        private const ushort S_VOLUME_DOWN = 46;
        private const ushort S_VOLUME_UP = 48;
        private const ushort S_MEDIA_NEXT_TRACK = 25;
        private const ushort S_MEDIA_PREV_TRACK = 16;
        private const ushort S_MEDIA_STOP = 36;
        private const ushort S_MEDIA_PLAY_PAUSE = 34;
        private const ushort S_LAUNCH_MAIL = 108;
        private const ushort S_LAUNCH_MEDIA_SELECT = 109;
        private const ushort S_LAUNCH_APP1 = 107;
        private const ushort S_LAUNCH_APP2 = 33;
        private const ushort S_OEM_1 = 27;
        private const ushort S_OEM_PLUS = 13;
        private const ushort S_OEM_COMMA = 50;
        private const ushort S_OEM_MINUS = 0;
        private const ushort S_OEM_PERIOD = 51;
        private const ushort S_OEM_2 = 52;
        private const ushort S_OEM_3 = 40;
        private const ushort S_OEM_4 = 12;
        private const ushort S_OEM_5 = 43;
        private const ushort S_OEM_6 = 26;
        private const ushort S_OEM_7 = 41;
        private const ushort S_OEM_8 = 53;
        private const ushort S_OEM_102 = 86;
        private const ushort S_PROCESSKEY = 0;
        private const ushort S_PACKET = 0;
        private const ushort S_ATTN = 0;
        private const ushort S_CRSEL = 0;
        private const ushort S_EXSEL = 0;
        private const ushort S_EREOF = 93;
        private const ushort S_PLAY = 0;
        private const ushort S_ZOOM = 98;
        private const ushort S_NONAME = 0;
        private const ushort S_PA1 = 0;
        private const ushort S_OEM_CLEAR = 0;
        private string drivertype;
        private Valuechanges ValueChanges = new Valuechanges();
        private bool formvisible;
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
        public Sendinput()
        {
            TimeBeginPeriod(1);
            NtSetTimerResolution(1, true, ref CurrentResolution);
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
        public void Connect(int number = 0)
        {
        }
        public void Disconnect()
        {
            Set(drivertype, 0, 0, 0, 0, 0, 0, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false);
        }
        public void Set(string KeyboardMouseDriverType, double MouseMoveX, double MouseMoveY, double MouseAbsX, double MouseAbsY, double MouseDesktopX, double MouseDesktopY, bool SendLeftClick, bool SendRightClick, bool SendMiddleClick, bool SendWheelUp, bool SendWheelDown, bool SendLeft, bool SendRight, bool SendUp, bool SendDown, bool SendLButton, bool SendRButton, bool SendCancel, bool SendMBUTTON, bool SendXBUTTON1, bool SendXBUTTON2, bool SendBack, bool SendTab, bool SendClear, bool SendReturn, bool SendSHIFT, bool SendCONTROL, bool SendMENU, bool SendPAUSE, bool SendCAPITAL, bool SendKANA, bool SendHANGEUL, bool SendHANGUL, bool SendJUNJA, bool SendFINAL, bool SendHANJA, bool SendKANJI, bool SendEscape, bool SendCONVERT, bool SendNONCONVERT, bool SendACCEPT, bool SendMODECHANGE, bool SendSpace, bool SendPRIOR, bool SendNEXT, bool SendEND, bool SendHOME, bool SendLEFT, bool SendUP, bool SendRIGHT, bool SendDOWN, bool SendSELECT, bool SendPRINT, bool SendEXECUTE, bool SendSNAPSHOT, bool SendINSERT, bool SendDELETE, bool SendHELP, bool SendAPOSTROPHE, bool Send0, bool Send1, bool Send2, bool Send3, bool Send4, bool Send5, bool Send6, bool Send7, bool Send8, bool Send9, bool SendA, bool SendB, bool SendC, bool SendD, bool SendE, bool SendF, bool SendG, bool SendH, bool SendI, bool SendJ, bool SendK, bool SendL, bool SendM, bool SendN, bool SendO, bool SendP, bool SendQ, bool SendR, bool SendS, bool SendT, bool SendU, bool SendV, bool SendW, bool SendX, bool SendY, bool SendZ, bool SendLWIN, bool SendRWIN, bool SendAPPS, bool SendSLEEP, bool SendNUMPAD0, bool SendNUMPAD1, bool SendNUMPAD2, bool SendNUMPAD3, bool SendNUMPAD4, bool SendNUMPAD5, bool SendNUMPAD6, bool SendNUMPAD7, bool SendNUMPAD8, bool SendNUMPAD9, bool SendMULTIPLY, bool SendADD, bool SendSEPARATOR, bool SendSUBTRACT, bool SendDECIMAL, bool SendDIVIDE, bool SendF1, bool SendF2, bool SendF3, bool SendF4, bool SendF5, bool SendF6, bool SendF7, bool SendF8, bool SendF9, bool SendF10, bool SendF11, bool SendF12, bool SendF13, bool SendF14, bool SendF15, bool SendF16, bool SendF17, bool SendF18, bool SendF19, bool SendF20, bool SendF21, bool SendF22, bool SendF23, bool SendF24, bool SendNUMLOCK, bool SendSCROLL, bool SendLeftShift, bool SendRightShift, bool SendLeftControl, bool SendRightControl, bool SendLMENU, bool SendRMENU)
        {
            this.drivertype = KeyboardMouseDriverType;
            if (MouseMoveX != 0f | MouseMoveY != 0f)
                mousebrink((int)MouseMoveX, (int)MouseMoveY);
            if (MouseAbsX != 0f | MouseAbsY != 0f)
                mousemw3((int)MouseAbsX, (int)MouseAbsY);
            if (MouseDesktopX != 0f | MouseDesktopY != 0f)
            {
                System.Windows.Forms.Cursor.Position = new System.Drawing.Point((int)MouseDesktopX, (int)MouseDesktopY);
                SetPhysicalCursorPos((int)MouseDesktopX, (int)MouseDesktopY);
                SetCaretPos((int)MouseDesktopX, (int)MouseDesktopY);
                SetCursorPos((int)MouseDesktopX, (int)MouseDesktopY);
                Microsoft.Xna.Framework.Input.Mouse.SetPosition((int)MouseDesktopX, (int)MouseDesktopY);
            }
            ValueChanges[1] = SendLeftClick ? 1 : 0;
            if (ValueChanges._ValueChange[1] > 0f)
                mouseclickleft();
            if (ValueChanges._ValueChange[1] < 0f)
                mouseclickleftF();
            ValueChanges[2] = SendRightClick ? 1 : 0;
            if (ValueChanges._ValueChange[2] > 0f)
                mouseclickright();
            if (ValueChanges._ValueChange[2] < 0f)
                mouseclickrightF();
            ValueChanges[3] = SendMiddleClick ? 1 : 0;
            if (ValueChanges._ValueChange[3] > 0f)
                mouseclickmiddle();
            if (ValueChanges._ValueChange[3] < 0f)
                mouseclickmiddleF();
            ValueChanges[4] = SendWheelUp ? 1 : 0;
            if (ValueChanges._ValueChange[4] > 0f)
                mousewheelup();
            ValueChanges[5] = SendWheelDown ? 1 : 0;
            if (ValueChanges._ValueChange[5] > 0f)
                mousewheeldown();
            ValueChanges[6] = SendLeft ? 1 : 0;
            if (ValueChanges._ValueChange[6] > 0f)
                keyboardArrows(VK_LEFT, S_LEFT);
            if (ValueChanges._ValueChange[6] < 0f)
                keyboardArrowsF(VK_LEFT, S_LEFT);
            ValueChanges[7] = SendRight ? 1 : 0;
            if (ValueChanges._ValueChange[7] > 0f)
                keyboardArrows(VK_RIGHT, S_RIGHT);
            if (ValueChanges._ValueChange[7] < 0f)
                keyboardArrowsF(VK_RIGHT, S_RIGHT);
            ValueChanges[8] = SendUp ? 1 : 0;
            if (ValueChanges._ValueChange[8] > 0f)
                keyboardArrows(VK_UP, S_UP);
            if (ValueChanges._ValueChange[8] < 0f)
                keyboardArrowsF(VK_UP, S_UP);
            ValueChanges[9] = SendDown ? 1 : 0;
            if (ValueChanges._ValueChange[9] > 0f)
                keyboardArrows(VK_DOWN, S_DOWN);
            if (ValueChanges._ValueChange[9] < 0f)
                keyboardArrowsF(VK_DOWN, S_DOWN);
            ValueChanges[10] = SendLButton ? 1 : 0;
            if (ValueChanges._ValueChange[10] > 0f)
                keyboard(VK_LBUTTON, S_LBUTTON);
            if (ValueChanges._ValueChange[10] < 0f)
                keyboardF(VK_LBUTTON, S_LBUTTON);
            ValueChanges[11] = SendRButton ? 1 : 0;
            if (ValueChanges._ValueChange[11] > 0f)
                keyboard(VK_RBUTTON, S_RBUTTON);
            if (ValueChanges._ValueChange[11] < 0f)
                keyboardF(VK_RBUTTON, S_RBUTTON);
            ValueChanges[12] = SendCancel ? 1 : 0;
            if (ValueChanges._ValueChange[12] > 0f)
                keyboard(VK_CANCEL, S_CANCEL);
            if (ValueChanges._ValueChange[12] < 0f)
                keyboardF(VK_CANCEL, S_CANCEL);
            ValueChanges[13] = SendMBUTTON ? 1 : 0;
            if (ValueChanges._ValueChange[13] > 0f)
                keyboard(VK_MBUTTON, S_MBUTTON);
            if (ValueChanges._ValueChange[13] < 0f)
                keyboardF(VK_MBUTTON, S_MBUTTON);
            ValueChanges[14] = SendXBUTTON1 ? 1 : 0;
            if (ValueChanges._ValueChange[14] > 0f)
                keyboard(VK_XBUTTON1, S_XBUTTON1);
            if (ValueChanges._ValueChange[14] < 0f)
                keyboardF(VK_XBUTTON1, S_XBUTTON1);
            ValueChanges[15] = SendXBUTTON2 ? 1 : 0;
            if (ValueChanges._ValueChange[15] > 0f)
                keyboard(VK_XBUTTON2, S_XBUTTON2);
            if (ValueChanges._ValueChange[15] < 0f)
                keyboardF(VK_XBUTTON2, S_XBUTTON2);
            ValueChanges[16] = SendBack ? 1 : 0;
            if (ValueChanges._ValueChange[16] > 0f)
                keyboard(VK_BACK, S_BACK);
            if (ValueChanges._ValueChange[16] < 0f)
                keyboardF(VK_BACK, S_BACK);
            ValueChanges[17] = SendTab ? 1 : 0;
            if (ValueChanges._ValueChange[17] > 0f)
                keyboard(VK_Tab, S_Tab);
            if (ValueChanges._ValueChange[17] < 0f)
                keyboardF(VK_Tab, S_Tab);
            ValueChanges[18] = SendClear ? 1 : 0;
            if (ValueChanges._ValueChange[18] > 0f)
                keyboard(VK_CLEAR, S_CLEAR);
            if (ValueChanges._ValueChange[18] < 0f)
                keyboardF(VK_CLEAR, S_CLEAR);
            ValueChanges[19] = SendReturn ? 1 : 0;
            if (ValueChanges._ValueChange[19] > 0f)
                keyboard(VK_Return, S_Return);
            if (ValueChanges._ValueChange[19] < 0f)
                keyboardF(VK_Return, S_Return);
            ValueChanges[20] = SendSHIFT ? 1 : 0;
            if (ValueChanges._ValueChange[20] > 0f)
                keyboard(VK_SHIFT, S_SHIFT);
            if (ValueChanges._ValueChange[20] < 0f)
                keyboardF(VK_SHIFT, S_SHIFT);
            ValueChanges[21] = SendCONTROL ? 1 : 0;
            if (ValueChanges._ValueChange[21] > 0f)
                keyboard(VK_CONTROL, S_CONTROL);
            if (ValueChanges._ValueChange[21] < 0f)
                keyboardF(VK_CONTROL, S_CONTROL);
            ValueChanges[22] = SendMENU ? 1 : 0;
            if (ValueChanges._ValueChange[22] > 0f)
                keyboard(VK_MENU, S_MENU);
            if (ValueChanges._ValueChange[22] < 0f)
                keyboardF(VK_MENU, S_MENU);
            ValueChanges[23] = SendPAUSE ? 1 : 0;
            if (ValueChanges._ValueChange[23] > 0f)
                keyboard(VK_PAUSE, S_PAUSE);
            if (ValueChanges._ValueChange[23] < 0f)
                keyboardF(VK_PAUSE, S_PAUSE);
            ValueChanges[24] = SendCAPITAL ? 1 : 0;
            if (ValueChanges._ValueChange[24] > 0f)
                keyboard(VK_CAPITAL, S_CAPITAL);
            if (ValueChanges._ValueChange[24] < 0f)
                keyboardF(VK_CAPITAL, S_CAPITAL);
            ValueChanges[25] = SendKANA ? 1 : 0;
            if (ValueChanges._ValueChange[25] > 0f)
                keyboard(VK_KANA, S_KANA);
            if (ValueChanges._ValueChange[25] < 0f)
                keyboardF(VK_KANA, S_KANA);
            ValueChanges[26] = SendHANGEUL ? 1 : 0;
            if (ValueChanges._ValueChange[26] > 0f)
                keyboard(VK_HANGEUL, S_HANGEUL);
            if (ValueChanges._ValueChange[26] < 0f)
                keyboardF(VK_HANGEUL, S_HANGEUL);
            ValueChanges[27] = SendHANGUL ? 1 : 0;
            if (ValueChanges._ValueChange[27] > 0f)
                keyboard(VK_HANGUL, S_HANGUL);
            if (ValueChanges._ValueChange[27] < 0f)
                keyboardF(VK_HANGUL, S_HANGUL);
            ValueChanges[28] = SendJUNJA ? 1 : 0;
            if (ValueChanges._ValueChange[28] > 0f)
                keyboard(VK_JUNJA, S_JUNJA);
            if (ValueChanges._ValueChange[28] < 0f)
                keyboardF(VK_JUNJA, S_JUNJA);
            ValueChanges[29] = SendFINAL ? 1 : 0;
            if (ValueChanges._ValueChange[29] > 0f)
                keyboard(VK_FINAL, S_FINAL);
            if (ValueChanges._ValueChange[29] < 0f)
                keyboardF(VK_FINAL, S_FINAL);
            ValueChanges[30] = SendHANJA ? 1 : 0;
            if (ValueChanges._ValueChange[30] > 0f)
                keyboard(VK_HANJA, S_HANJA);
            if (ValueChanges._ValueChange[30] < 0f)
                keyboardF(VK_HANJA, S_HANJA);
            ValueChanges[31] = SendKANJI ? 1 : 0;
            if (ValueChanges._ValueChange[31] > 0f)
                keyboard(VK_KANJI, S_KANJI);
            if (ValueChanges._ValueChange[31] < 0f)
                keyboardF(VK_KANJI, S_KANJI);
            ValueChanges[32] = SendEscape ? 1 : 0;
            if (ValueChanges._ValueChange[32] > 0f)
                keyboard(VK_Escape, S_Escape);
            if (ValueChanges._ValueChange[32] < 0f)
                keyboardF(VK_Escape, S_Escape);
            ValueChanges[33] = SendCONVERT ? 1 : 0;
            if (ValueChanges._ValueChange[33] > 0f)
                keyboard(VK_CONVERT, S_CONVERT);
            if (ValueChanges._ValueChange[33] < 0f)
                keyboardF(VK_CONVERT, S_CONVERT);
            ValueChanges[34] = SendNONCONVERT ? 1 : 0;
            if (ValueChanges._ValueChange[34] > 0f)
                keyboard(VK_NONCONVERT, S_NONCONVERT);
            if (ValueChanges._ValueChange[34] < 0f)
                keyboardF(VK_NONCONVERT, S_NONCONVERT);
            ValueChanges[35] = SendACCEPT ? 1 : 0;
            if (ValueChanges._ValueChange[35] > 0f)
                keyboard(VK_ACCEPT, S_ACCEPT);
            if (ValueChanges._ValueChange[35] < 0f)
                keyboardF(VK_ACCEPT, S_ACCEPT);
            ValueChanges[36] = SendMODECHANGE ? 1 : 0;
            if (ValueChanges._ValueChange[36] > 0f)
                keyboard(VK_MODECHANGE, S_MODECHANGE);
            if (ValueChanges._ValueChange[36] < 0f)
                keyboardF(VK_MODECHANGE, S_MODECHANGE);
            ValueChanges[37] = SendSpace ? 1 : 0;
            if (ValueChanges._ValueChange[37] > 0f)
                keyboard(VK_Space, S_Space);
            if (ValueChanges._ValueChange[37] < 0f)
                keyboardF(VK_Space, S_Space);
            ValueChanges[38] = SendPRIOR ? 1 : 0;
            if (ValueChanges._ValueChange[38] > 0f)
                keyboard(VK_PRIOR, S_PRIOR);
            if (ValueChanges._ValueChange[38] < 0f)
                keyboardF(VK_PRIOR, S_PRIOR);
            ValueChanges[39] = SendNEXT ? 1 : 0;
            if (ValueChanges._ValueChange[39] > 0f)
                keyboard(VK_NEXT, S_NEXT);
            if (ValueChanges._ValueChange[39] < 0f)
                keyboardF(VK_NEXT, S_NEXT);
            ValueChanges[40] = SendEND ? 1 : 0;
            if (ValueChanges._ValueChange[40] > 0f)
                keyboard(VK_END, S_END);
            if (ValueChanges._ValueChange[40] < 0f)
                keyboardF(VK_END, S_END);
            ValueChanges[41] = SendHOME ? 1 : 0;
            if (ValueChanges._ValueChange[41] > 0f)
                keyboard(VK_HOME, S_HOME);
            if (ValueChanges._ValueChange[41] < 0f)
                keyboardF(VK_HOME, S_HOME);
            ValueChanges[42] = SendLEFT ? 1 : 0;
            if (ValueChanges._ValueChange[42] > 0f)
                keyboard(VK_LEFT, S_LEFT);
            if (ValueChanges._ValueChange[42] < 0f)
                keyboardF(VK_LEFT, S_LEFT);
            ValueChanges[43] = SendUP ? 1 : 0;
            if (ValueChanges._ValueChange[43] > 0f)
                keyboard(VK_UP, S_UP);
            if (ValueChanges._ValueChange[43] < 0f)
                keyboardF(VK_UP, S_UP);
            ValueChanges[44] = SendRIGHT ? 1 : 0;
            if (ValueChanges._ValueChange[44] > 0f)
                keyboard(VK_RIGHT, S_RIGHT);
            if (ValueChanges._ValueChange[44] < 0f)
                keyboardF(VK_RIGHT, S_RIGHT);
            ValueChanges[45] = SendDOWN ? 1 : 0;
            if (ValueChanges._ValueChange[45] > 0f)
                keyboard(VK_DOWN, S_DOWN);
            if (ValueChanges._ValueChange[45] < 0f)
                keyboardF(VK_DOWN, S_DOWN);
            ValueChanges[46] = SendSELECT ? 1 : 0;
            if (ValueChanges._ValueChange[46] > 0f)
                keyboard(VK_SELECT, S_SELECT);
            if (ValueChanges._ValueChange[46] < 0f)
                keyboardF(VK_SELECT, S_SELECT);
            ValueChanges[47] = SendPRINT ? 1 : 0;
            if (ValueChanges._ValueChange[47] > 0f)
                keyboard(VK_PRINT, S_PRINT);
            if (ValueChanges._ValueChange[47] < 0f)
                keyboardF(VK_PRINT, S_PRINT);
            ValueChanges[48] = SendEXECUTE ? 1 : 0;
            if (ValueChanges._ValueChange[48] > 0f)
                keyboard(VK_EXECUTE, S_EXECUTE);
            if (ValueChanges._ValueChange[48] < 0f)
                keyboardF(VK_EXECUTE, S_EXECUTE);
            ValueChanges[49] = SendSNAPSHOT ? 1 : 0;
            if (ValueChanges._ValueChange[49] > 0f)
                keyboard(VK_SNAPSHOT, S_SNAPSHOT);
            if (ValueChanges._ValueChange[49] < 0f)
                keyboardF(VK_SNAPSHOT, S_SNAPSHOT);
            ValueChanges[50] = SendINSERT ? 1 : 0;
            if (ValueChanges._ValueChange[50] > 0f)
                keyboard(VK_INSERT, S_INSERT);
            if (ValueChanges._ValueChange[50] < 0f)
                keyboardF(VK_INSERT, S_INSERT);
            ValueChanges[51] = SendDELETE ? 1 : 0;
            if (ValueChanges._ValueChange[51] > 0f)
                keyboard(VK_DELETE, S_DELETE);
            if (ValueChanges._ValueChange[51] < 0f)
                keyboardF(VK_DELETE, S_DELETE);
            ValueChanges[52] = SendHELP ? 1 : 0;
            if (ValueChanges._ValueChange[52] > 0f)
                keyboard(VK_HELP, S_HELP);
            if (ValueChanges._ValueChange[52] < 0f)
                keyboardF(VK_HELP, S_HELP);
            ValueChanges[53] = SendAPOSTROPHE ? 1 : 0;
            if (ValueChanges._ValueChange[53] > 0f)
                keyboard(VK_APOSTROPHE, S_APOSTROPHE);
            if (ValueChanges._ValueChange[53] < 0f)
                keyboardF(VK_APOSTROPHE, S_APOSTROPHE);
            ValueChanges[54] = Send0 ? 1 : 0;
            if (ValueChanges._ValueChange[54] > 0f)
                keyboard(VK_0, S_0);
            if (ValueChanges._ValueChange[54] < 0f)
                keyboardF(VK_0, S_0);
            ValueChanges[55] = Send1 ? 1 : 0;
            if (ValueChanges._ValueChange[55] > 0f)
                keyboard(VK_1, S_1);
            if (ValueChanges._ValueChange[55] < 0f)
                keyboardF(VK_1, S_1);
            ValueChanges[56] = Send2 ? 1 : 0;
            if (ValueChanges._ValueChange[56] > 0f)
                keyboard(VK_2, S_2);
            if (ValueChanges._ValueChange[56] < 0f)
                keyboardF(VK_2, S_2);
            ValueChanges[57] = Send3 ? 1 : 0;
            if (ValueChanges._ValueChange[57] > 0f)
                keyboard(VK_3, S_3);
            if (ValueChanges._ValueChange[57] < 0f)
                keyboardF(VK_3, S_3);
            ValueChanges[58] = Send4 ? 1 : 0;
            if (ValueChanges._ValueChange[58] > 0f)
                keyboard(VK_4, S_4);
            if (ValueChanges._ValueChange[58] < 0f)
                keyboardF(VK_4, S_4);
            ValueChanges[59] = Send5 ? 1 : 0;
            if (ValueChanges._ValueChange[59] > 0f)
                keyboard(VK_5, S_5);
            if (ValueChanges._ValueChange[59] < 0f)
                keyboardF(VK_5, S_5);
            ValueChanges[60] = Send6 ? 1 : 0;
            if (ValueChanges._ValueChange[60] > 0f)
                keyboard(VK_6, S_6);
            if (ValueChanges._ValueChange[60] < 0f)
                keyboardF(VK_6, S_6);
            ValueChanges[61] = Send7 ? 1 : 0;
            if (ValueChanges._ValueChange[61] > 0f)
                keyboard(VK_7, S_7);
            if (ValueChanges._ValueChange[61] < 0f)
                keyboardF(VK_7, S_7);
            ValueChanges[62] = Send8 ? 1 : 0;
            if (ValueChanges._ValueChange[62] > 0f)
                keyboard(VK_8, S_8);
            if (ValueChanges._ValueChange[62] < 0f)
                keyboardF(VK_8, S_8);
            ValueChanges[63] = Send9 ? 1 : 0;
            if (ValueChanges._ValueChange[63] > 0f)
                keyboard(VK_9, S_9);
            if (ValueChanges._ValueChange[63] < 0f)
                keyboardF(VK_9, S_9);
            ValueChanges[64] = SendA ? 1 : 0;
            if (ValueChanges._ValueChange[64] > 0f)
                keyboard(VK_A, S_A);
            if (ValueChanges._ValueChange[64] < 0f)
                keyboardF(VK_A, S_A);
            ValueChanges[65] = SendB ? 1 : 0;
            if (ValueChanges._ValueChange[65] > 0f)
                keyboard(VK_B, S_B);
            if (ValueChanges._ValueChange[65] < 0f)
                keyboardF(VK_B, S_B);
            ValueChanges[66] = SendC ? 1 : 0;
            if (ValueChanges._ValueChange[66] > 0f)
                keyboard(VK_C, S_C);
            if (ValueChanges._ValueChange[66] < 0f)
                keyboardF(VK_C, S_C);
            ValueChanges[67] = SendD ? 1 : 0;
            if (ValueChanges._ValueChange[67] > 0f)
                keyboard(VK_D, S_D);
            if (ValueChanges._ValueChange[67] < 0f)
                keyboardF(VK_D, S_D);
            ValueChanges[68] = SendE ? 1 : 0;
            if (ValueChanges._ValueChange[68] > 0f)
                keyboard(VK_E, S_E);
            if (ValueChanges._ValueChange[68] < 0f)
                keyboardF(VK_E, S_E);
            ValueChanges[69] = SendF ? 1 : 0;
            if (ValueChanges._ValueChange[69] > 0f)
                keyboard(VK_F, S_F);
            if (ValueChanges._ValueChange[69] < 0f)
                keyboardF(VK_F, S_F);
            ValueChanges[70] = SendG ? 1 : 0;
            if (ValueChanges._ValueChange[70] > 0f)
                keyboard(VK_G, S_G);
            if (ValueChanges._ValueChange[70] < 0f)
                keyboardF(VK_G, S_G);
            ValueChanges[71] = SendH ? 1 : 0;
            if (ValueChanges._ValueChange[71] > 0f)
                keyboard(VK_H, S_H);
            if (ValueChanges._ValueChange[71] < 0f)
                keyboardF(VK_H, S_H);
            ValueChanges[72] = SendI ? 1 : 0;
            if (ValueChanges._ValueChange[72] > 0f)
                keyboard(VK_I, S_I);
            if (ValueChanges._ValueChange[72] < 0f)
                keyboardF(VK_I, S_I);
            ValueChanges[73] = SendJ ? 1 : 0;
            if (ValueChanges._ValueChange[73] > 0f)
                keyboard(VK_J, S_J);
            if (ValueChanges._ValueChange[73] < 0f)
                keyboardF(VK_J, S_J);
            ValueChanges[74] = SendK ? 1 : 0;
            if (ValueChanges._ValueChange[74] > 0f)
                keyboard(VK_K, S_K);
            if (ValueChanges._ValueChange[74] < 0f)
                keyboardF(VK_K, S_K);
            ValueChanges[75] = SendL ? 1 : 0;
            if (ValueChanges._ValueChange[75] > 0f)
                keyboard(VK_L, S_L);
            if (ValueChanges._ValueChange[75] < 0f)
                keyboardF(VK_L, S_L);
            ValueChanges[76] = SendM ? 1 : 0;
            if (ValueChanges._ValueChange[76] > 0f)
                keyboard(VK_M, S_M);
            if (ValueChanges._ValueChange[76] < 0f)
                keyboardF(VK_M, S_M);
            ValueChanges[77] = SendN ? 1 : 0;
            if (ValueChanges._ValueChange[77] > 0f)
                keyboard(VK_N, S_N);
            if (ValueChanges._ValueChange[77] < 0f)
                keyboardF(VK_N, S_N);
            ValueChanges[78] = SendO ? 1 : 0;
            if (ValueChanges._ValueChange[78] > 0f)
                keyboard(VK_O, S_O);
            if (ValueChanges._ValueChange[78] < 0f)
                keyboardF(VK_O, S_O);
            ValueChanges[79] = SendP ? 1 : 0;
            if (ValueChanges._ValueChange[79] > 0f)
                keyboard(VK_P, S_P);
            if (ValueChanges._ValueChange[79] < 0f)
                keyboardF(VK_P, S_P);
            ValueChanges[80] = SendQ ? 1 : 0;
            if (ValueChanges._ValueChange[80] > 0f)
                keyboard(VK_Q, S_Q);
            if (ValueChanges._ValueChange[80] < 0f)
                keyboardF(VK_Q, S_Q);
            ValueChanges[81] = SendR ? 1 : 0;
            if (ValueChanges._ValueChange[81] > 0f)
                keyboard(VK_R, S_R);
            if (ValueChanges._ValueChange[81] < 0f)
                keyboardF(VK_R, S_R);
            ValueChanges[82] = SendS ? 1 : 0;
            if (ValueChanges._ValueChange[82] > 0f)
                keyboard(VK_S, S_S);
            if (ValueChanges._ValueChange[82] < 0f)
                keyboardF(VK_S, S_S);
            ValueChanges[83] = SendT ? 1 : 0;
            if (ValueChanges._ValueChange[83] > 0f)
                keyboard(VK_T, S_T);
            if (ValueChanges._ValueChange[83] < 0f)
                keyboardF(VK_T, S_T);
            ValueChanges[84] = SendU ? 1 : 0;
            if (ValueChanges._ValueChange[84] > 0f)
                keyboard(VK_U, S_U);
            if (ValueChanges._ValueChange[84] < 0f)
                keyboardF(VK_U, S_U);
            ValueChanges[85] = SendV ? 1 : 0;
            if (ValueChanges._ValueChange[85] > 0f)
                keyboard(VK_V, S_V);
            if (ValueChanges._ValueChange[85] < 0f)
                keyboardF(VK_V, S_V);
            ValueChanges[86] = SendW ? 1 : 0;
            if (ValueChanges._ValueChange[86] > 0f)
                keyboard(VK_W, S_W);
            if (ValueChanges._ValueChange[86] < 0f)
                keyboardF(VK_W, S_W);
            ValueChanges[87] = SendX ? 1 : 0;
            if (ValueChanges._ValueChange[87] > 0f)
                keyboard(VK_X, S_X);
            if (ValueChanges._ValueChange[87] < 0f)
                keyboardF(VK_X, S_X);
            ValueChanges[88] = SendY ? 1 : 0;
            if (ValueChanges._ValueChange[88] > 0f)
                keyboard(VK_Y, S_Y);
            if (ValueChanges._ValueChange[88] < 0f)
                keyboardF(VK_Y, S_Y);
            ValueChanges[89] = SendZ ? 1 : 0;
            if (ValueChanges._ValueChange[89] > 0f)
                keyboard(VK_Z, S_Z);
            if (ValueChanges._ValueChange[89] < 0f)
                keyboardF(VK_Z, S_Z);
            ValueChanges[90] = SendLWIN ? 1 : 0;
            if (ValueChanges._ValueChange[90] > 0f)
                keyboard(VK_LWIN, S_LWIN);
            if (ValueChanges._ValueChange[90] < 0f)
                keyboardF(VK_LWIN, S_LWIN);
            ValueChanges[91] = SendRWIN ? 1 : 0;
            if (ValueChanges._ValueChange[91] > 0f)
                keyboard(VK_RWIN, S_RWIN);
            if (ValueChanges._ValueChange[91] < 0f)
                keyboardF(VK_RWIN, S_RWIN);
            ValueChanges[92] = SendAPPS ? 1 : 0;
            if (ValueChanges._ValueChange[92] > 0f)
                keyboard(VK_APPS, S_APPS);
            if (ValueChanges._ValueChange[92] < 0f)
                keyboardF(VK_APPS, S_APPS);
            ValueChanges[93] = SendSLEEP ? 1 : 0;
            if (ValueChanges._ValueChange[93] > 0f)
                keyboard(VK_SLEEP, S_SLEEP);
            if (ValueChanges._ValueChange[93] < 0f)
                keyboardF(VK_SLEEP, S_SLEEP);
            ValueChanges[94] = SendNUMPAD0 ? 1 : 0;
            if (ValueChanges._ValueChange[94] > 0f)
                keyboard(VK_NUMPAD0, S_NUMPAD0);
            if (ValueChanges._ValueChange[94] < 0f)
                keyboardF(VK_NUMPAD0, S_NUMPAD0);
            ValueChanges[95] = SendNUMPAD1 ? 1 : 0;
            if (ValueChanges._ValueChange[95] > 0f)
                keyboard(VK_NUMPAD1, S_NUMPAD1);
            if (ValueChanges._ValueChange[95] < 0f)
                keyboardF(VK_NUMPAD1, S_NUMPAD1);
            ValueChanges[96] = SendNUMPAD2 ? 1 : 0;
            if (ValueChanges._ValueChange[96] > 0f)
                keyboard(VK_NUMPAD2, S_NUMPAD2);
            if (ValueChanges._ValueChange[96] < 0f)
                keyboardF(VK_NUMPAD2, S_NUMPAD2);
            ValueChanges[97] = SendNUMPAD3 ? 1 : 0;
            if (ValueChanges._ValueChange[97] > 0f)
                keyboard(VK_NUMPAD3, S_NUMPAD3);
            if (ValueChanges._ValueChange[97] < 0f)
                keyboardF(VK_NUMPAD3, S_NUMPAD3);
            ValueChanges[98] = SendNUMPAD4 ? 1 : 0;
            if (ValueChanges._ValueChange[98] > 0f)
                keyboard(VK_NUMPAD4, S_NUMPAD4);
            if (ValueChanges._ValueChange[98] < 0f)
                keyboardF(VK_NUMPAD4, S_NUMPAD4);
            ValueChanges[99] = SendNUMPAD5 ? 1 : 0;
            if (ValueChanges._ValueChange[99] > 0f)
                keyboard(VK_NUMPAD5, S_NUMPAD5);
            if (ValueChanges._ValueChange[99] < 0f)
                keyboardF(VK_NUMPAD5, S_NUMPAD5);
            ValueChanges[100] = SendNUMPAD6 ? 1 : 0;
            if (ValueChanges._ValueChange[100] > 0f)
                keyboard(VK_NUMPAD6, S_NUMPAD6);
            if (ValueChanges._ValueChange[100] < 0f)
                keyboardF(VK_NUMPAD6, S_NUMPAD6);
            ValueChanges[101] = SendNUMPAD7 ? 1 : 0;
            if (ValueChanges._ValueChange[101] > 0f)
                keyboard(VK_NUMPAD7, S_NUMPAD7);
            if (ValueChanges._ValueChange[101] < 0f)
                keyboardF(VK_NUMPAD7, S_NUMPAD7);
            ValueChanges[102] = SendNUMPAD8 ? 1 : 0;
            if (ValueChanges._ValueChange[102] > 0f)
                keyboard(VK_NUMPAD8, S_NUMPAD8);
            if (ValueChanges._ValueChange[102] < 0f)
                keyboardF(VK_NUMPAD8, S_NUMPAD8);
            ValueChanges[103] = SendNUMPAD9 ? 1 : 0;
            if (ValueChanges._ValueChange[103] > 0f)
                keyboard(VK_NUMPAD9, S_NUMPAD9);
            if (ValueChanges._ValueChange[103] < 0f)
                keyboardF(VK_NUMPAD9, S_NUMPAD9);
            ValueChanges[104] = SendMULTIPLY ? 1 : 0;
            if (ValueChanges._ValueChange[104] > 0f)
                keyboard(VK_MULTIPLY, S_MULTIPLY);
            if (ValueChanges._ValueChange[104] < 0f)
                keyboardF(VK_MULTIPLY, S_MULTIPLY);
            ValueChanges[105] = SendADD ? 1 : 0;
            if (ValueChanges._ValueChange[105] > 0f)
                keyboard(VK_ADD, S_ADD);
            if (ValueChanges._ValueChange[105] < 0f)
                keyboardF(VK_ADD, S_ADD);
            ValueChanges[106] = SendSEPARATOR ? 1 : 0;
            if (ValueChanges._ValueChange[106] > 0f)
                keyboard(VK_SEPARATOR, S_SEPARATOR);
            if (ValueChanges._ValueChange[106] < 0f)
                keyboardF(VK_SEPARATOR, S_SEPARATOR);
            ValueChanges[107] = SendSUBTRACT ? 1 : 0;
            if (ValueChanges._ValueChange[107] > 0f)
                keyboard(VK_SUBTRACT, S_SUBTRACT);
            if (ValueChanges._ValueChange[107] < 0f)
                keyboardF(VK_SUBTRACT, S_SUBTRACT);
            ValueChanges[108] = SendDECIMAL ? 1 : 0;
            if (ValueChanges._ValueChange[108] > 0f)
                keyboard(VK_DECIMAL, S_DECIMAL);
            if (ValueChanges._ValueChange[108] < 0f)
                keyboardF(VK_DECIMAL, S_DECIMAL);
            ValueChanges[109] = SendDIVIDE ? 1 : 0;
            if (ValueChanges._ValueChange[109] > 0f)
                keyboard(VK_DIVIDE, S_DIVIDE);
            if (ValueChanges._ValueChange[109] < 0f)
                keyboardF(VK_DIVIDE, S_DIVIDE);
            ValueChanges[110] = SendF1 ? 1 : 0;
            if (ValueChanges._ValueChange[110] > 0f)
                keyboard(VK_F1, S_F1);
            if (ValueChanges._ValueChange[110] < 0f)
                keyboardF(VK_F1, S_F1);
            ValueChanges[111] = SendF2 ? 1 : 0;
            if (ValueChanges._ValueChange[111] > 0f)
                keyboard(VK_F2, S_F2);
            if (ValueChanges._ValueChange[111] < 0f)
                keyboardF(VK_F2, S_F2);
            ValueChanges[112] = SendF3 ? 1 : 0;
            if (ValueChanges._ValueChange[112] > 0f)
                keyboard(VK_F3, S_F3);
            if (ValueChanges._ValueChange[112] < 0f)
                keyboardF(VK_F3, S_F3);
            ValueChanges[113] = SendF4 ? 1 : 0;
            if (ValueChanges._ValueChange[113] > 0f)
                keyboard(VK_F4, S_F4);
            if (ValueChanges._ValueChange[113] < 0f)
                keyboardF(VK_F4, S_F4);
            ValueChanges[114] = SendF5 ? 1 : 0;
            if (ValueChanges._ValueChange[114] > 0f)
                keyboard(VK_F5, S_F5);
            if (ValueChanges._ValueChange[114] < 0f)
                keyboardF(VK_F5, S_F5);
            ValueChanges[115] = SendF6 ? 1 : 0;
            if (ValueChanges._ValueChange[115] > 0f)
                keyboard(VK_F6, S_F6);
            if (ValueChanges._ValueChange[115] < 0f)
                keyboardF(VK_F6, S_F6);
            ValueChanges[116] = SendF7 ? 1 : 0;
            if (ValueChanges._ValueChange[116] > 0f)
                keyboard(VK_F7, S_F7);
            if (ValueChanges._ValueChange[116] < 0f)
                keyboardF(VK_F7, S_F7);
            ValueChanges[117] = SendF8 ? 1 : 0;
            if (ValueChanges._ValueChange[117] > 0f)
                keyboard(VK_F8, S_F8);
            if (ValueChanges._ValueChange[117] < 0f)
                keyboardF(VK_F8, S_F8);
            ValueChanges[118] = SendF9 ? 1 : 0;
            if (ValueChanges._ValueChange[118] > 0f)
                keyboard(VK_F9, S_F9);
            if (ValueChanges._ValueChange[118] < 0f)
                keyboardF(VK_F9, S_F9);
            ValueChanges[119] = SendF10 ? 1 : 0;
            if (ValueChanges._ValueChange[119] > 0f)
                keyboard(VK_F10, S_F10);
            if (ValueChanges._ValueChange[119] < 0f)
                keyboardF(VK_F10, S_F10);
            ValueChanges[120] = SendF11 ? 1 : 0;
            if (ValueChanges._ValueChange[120] > 0f)
                keyboard(VK_F11, S_F11);
            if (ValueChanges._ValueChange[120] < 0f)
                keyboardF(VK_F11, S_F11);
            ValueChanges[121] = SendF12 ? 1 : 0;
            if (ValueChanges._ValueChange[121] > 0f)
                keyboard(VK_F12, S_F12);
            if (ValueChanges._ValueChange[121] < 0f)
                keyboardF(VK_F12, S_F12);
            ValueChanges[122] = SendF13 ? 1 : 0;
            if (ValueChanges._ValueChange[122] > 0f)
                keyboard(VK_F13, S_F13);
            if (ValueChanges._ValueChange[122] < 0f)
                keyboardF(VK_F13, S_F13);
            ValueChanges[123] = SendF14 ? 1 : 0;
            if (ValueChanges._ValueChange[123] > 0f)
                keyboard(VK_F14, S_F14);
            if (ValueChanges._ValueChange[123] < 0f)
                keyboardF(VK_F14, S_F14);
            ValueChanges[124] = SendF15 ? 1 : 0;
            if (ValueChanges._ValueChange[124] > 0f)
                keyboard(VK_F15, S_F15);
            if (ValueChanges._ValueChange[124] < 0f)
                keyboardF(VK_F15, S_F15);
            ValueChanges[125] = SendF16 ? 1 : 0;
            if (ValueChanges._ValueChange[125] > 0f)
                keyboard(VK_F16, S_F16);
            if (ValueChanges._ValueChange[125] < 0f)
                keyboardF(VK_F16, S_F16);
            ValueChanges[126] = SendF17 ? 1 : 0;
            if (ValueChanges._ValueChange[126] > 0f)
                keyboard(VK_F17, S_F17);
            if (ValueChanges._ValueChange[126] < 0f)
                keyboardF(VK_F17, S_F17);
            ValueChanges[127] = SendF18 ? 1 : 0;
            if (ValueChanges._ValueChange[127] > 0f)
                keyboard(VK_F18, S_F18);
            if (ValueChanges._ValueChange[127] < 0f)
                keyboardF(VK_F18, S_F18);
            ValueChanges[128] = SendF19 ? 1 : 0;
            if (ValueChanges._ValueChange[128] > 0f)
                keyboard(VK_F19, S_F19);
            if (ValueChanges._ValueChange[128] < 0f)
                keyboardF(VK_F19, S_F19);
            ValueChanges[129] = SendF20 ? 1 : 0;
            if (ValueChanges._ValueChange[129] > 0f)
                keyboard(VK_F20, S_F20);
            if (ValueChanges._ValueChange[129] < 0f)
                keyboardF(VK_F20, S_F20);
            ValueChanges[130] = SendF21 ? 1 : 0;
            if (ValueChanges._ValueChange[130] > 0f)
                keyboard(VK_F21, S_F21);
            if (ValueChanges._ValueChange[130] < 0f)
                keyboardF(VK_F21, S_F21);
            ValueChanges[131] = SendF22 ? 1 : 0;
            if (ValueChanges._ValueChange[131] > 0f)
                keyboard(VK_F22, S_F22);
            if (ValueChanges._ValueChange[131] < 0f)
                keyboardF(VK_F22, S_F22);
            ValueChanges[132] = SendF23 ? 1 : 0;
            if (ValueChanges._ValueChange[132] > 0f)
                keyboard(VK_F23, S_F23);
            if (ValueChanges._ValueChange[132] < 0f)
                keyboardF(VK_F23, S_F23);
            ValueChanges[133] = SendF24 ? 1 : 0;
            if (ValueChanges._ValueChange[133] > 0f)
                keyboard(VK_F24, S_F24);
            if (ValueChanges._ValueChange[133] < 0f)
                keyboardF(VK_F24, S_F24);
            ValueChanges[134] = SendNUMLOCK ? 1 : 0;
            if (ValueChanges._ValueChange[134] > 0f)
                keyboard(VK_NUMLOCK, S_NUMLOCK);
            if (ValueChanges._ValueChange[134] < 0f)
                keyboardF(VK_NUMLOCK, S_NUMLOCK);
            ValueChanges[135] = SendSCROLL ? 1 : 0;
            if (ValueChanges._ValueChange[135] > 0f)
                keyboard(VK_SCROLL, S_SCROLL);
            if (ValueChanges._ValueChange[135] < 0f)
                keyboardF(VK_SCROLL, S_SCROLL);
            ValueChanges[136] = SendLeftShift ? 1 : 0;
            if (ValueChanges._ValueChange[136] > 0f)
                keyboard(VK_LeftShift, S_LeftShift);
            if (ValueChanges._ValueChange[136] < 0f)
                keyboardF(VK_LeftShift, S_LeftShift);
            ValueChanges[137] = SendRightShift ? 1 : 0;
            if (ValueChanges._ValueChange[137] > 0f)
                keyboard(VK_RightShift, S_RightShift);
            if (ValueChanges._ValueChange[137] < 0f)
                keyboardF(VK_RightShift, S_RightShift);
            ValueChanges[138] = SendLeftControl ? 1 : 0;
            if (ValueChanges._ValueChange[138] > 0f)
                keyboard(VK_LeftControl, S_LeftControl);
            if (ValueChanges._ValueChange[138] < 0f)
                keyboardF(VK_LeftControl, S_LeftControl);
            ValueChanges[139] = SendRightControl ? 1 : 0;
            if (ValueChanges._ValueChange[139] > 0f)
                keyboard(VK_RightControl, S_RightControl);
            if (ValueChanges._ValueChange[139] < 0f)
                keyboardF(VK_RightControl, S_RightControl);
            ValueChanges[140] = SendLMENU ? 1 : 0;
            if (ValueChanges._ValueChange[140] > 0f)
                keyboard(VK_LMENU, S_LMENU);
            if (ValueChanges._ValueChange[140] < 0f)
                keyboardF(VK_LMENU, S_LMENU);
            ValueChanges[141] = SendRMENU ? 1 : 0;
            if (ValueChanges._ValueChange[141] > 0f)
                keyboard(VK_RMENU, S_RMENU);
            if (ValueChanges._ValueChange[141] < 0f)
                keyboardF(VK_RMENU, S_RMENU);
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
                string str = "KeyboardMouseDriverType : " + KeyboardMouseDriverType + Environment.NewLine;
                str += "MouseMoveX : " + MouseMoveX + Environment.NewLine;
                str += "MouseMoveY : " + MouseMoveY + Environment.NewLine;
                str += "MouseAbsX : " + MouseAbsX + Environment.NewLine;
                str += "MouseAbsY : " + MouseAbsY + Environment.NewLine;
                str += "MouseDesktopX : " + MouseDesktopX + Environment.NewLine;
                str += "MouseDesktopY : " + MouseDesktopY + Environment.NewLine;
                str += "SendLeftClick : " + SendLeftClick + Environment.NewLine;
                str += "SendRightClick : " + SendRightClick + Environment.NewLine;
                str += "SendMiddleClick : " + SendMiddleClick + Environment.NewLine;
                str += "SendWheelUp : " + SendWheelUp + Environment.NewLine;
                str += "SendWheelDown : " + SendWheelDown + Environment.NewLine;
                str += "SendLeft : " + SendLeft + Environment.NewLine;
                str += "SendRight : " + SendRight + Environment.NewLine;
                str += "SendUp : " + SendUp + Environment.NewLine;
                str += "SendDown : " + SendDown + Environment.NewLine;
                str += "SendLButton : " + SendLButton + Environment.NewLine;
                str += "SendRButton : " + SendRButton + Environment.NewLine;
                str += "SendCancel : " + SendCancel + Environment.NewLine;
                str += "SendMBUTTON : " + SendMBUTTON + Environment.NewLine;
                str += "SendXBUTTON1 : " + SendXBUTTON1 + Environment.NewLine;
                str += "SendXBUTTON2 : " + SendXBUTTON2 + Environment.NewLine;
                str += "SendBack : " + SendBack + Environment.NewLine;
                str += "SendTab : " + SendTab + Environment.NewLine;
                str += "SendClear : " + SendClear + Environment.NewLine;
                str += "SendReturn : " + SendReturn + Environment.NewLine;
                str += "SendSHIFT : " + SendSHIFT + Environment.NewLine;
                str += "SendCONTROL : " + SendCONTROL + Environment.NewLine;
                str += "SendMENU : " + SendMENU + Environment.NewLine;
                str += "SendPAUSE : " + SendPAUSE + Environment.NewLine;
                str += "SendCAPITAL : " + SendCAPITAL + Environment.NewLine;
                str += "SendKANA : " + SendKANA + Environment.NewLine;
                str += "SendHANGEUL : " + SendHANGEUL + Environment.NewLine;
                str += "SendHANGUL : " + SendHANGUL + Environment.NewLine;
                str += "SendJUNJA : " + SendJUNJA + Environment.NewLine;
                str += "SendFINAL : " + SendFINAL + Environment.NewLine;
                str += "SendHANJA : " + SendHANJA + Environment.NewLine;
                str += "SendKANJI : " + SendKANJI + Environment.NewLine;
                str += "SendEscape : " + SendEscape + Environment.NewLine;
                str += "SendCONVERT : " + SendCONVERT + Environment.NewLine;
                str += "SendNONCONVERT : " + SendNONCONVERT + Environment.NewLine;
                str += "SendACCEPT : " + SendACCEPT + Environment.NewLine;
                str += "SendMODECHANGE : " + SendMODECHANGE + Environment.NewLine;
                str += "SendSpace : " + SendSpace + Environment.NewLine;
                str += "SendPRIOR : " + SendPRIOR + Environment.NewLine;
                str += "SendNEXT : " + SendNEXT + Environment.NewLine;
                str += "SendEND : " + SendEND + Environment.NewLine;
                str += "SendHOME : " + SendHOME + Environment.NewLine;
                str += "SendLEFT : " + SendLEFT + Environment.NewLine;
                str += "SendUP : " + SendUP + Environment.NewLine;
                str += "SendRIGHT : " + SendRIGHT + Environment.NewLine;
                str += "SendDOWN : " + SendDOWN + Environment.NewLine;
                str += "SendSELECT : " + SendSELECT + Environment.NewLine;
                str += "SendPRINT : " + SendPRINT + Environment.NewLine;
                str += "SendEXECUTE : " + SendEXECUTE + Environment.NewLine;
                str += "SendSNAPSHOT : " + SendSNAPSHOT + Environment.NewLine;
                str += "SendINSERT : " + SendINSERT + Environment.NewLine;
                str += "SendDELETE : " + SendDELETE + Environment.NewLine;
                str += "SendHELP : " + SendHELP + Environment.NewLine;
                str += "SendAPOSTROPHE : " + SendAPOSTROPHE + Environment.NewLine;
                str += "Send0 : " + Send0 + Environment.NewLine;
                str += "Send1 : " + Send1 + Environment.NewLine;
                str += "Send2 : " + Send2 + Environment.NewLine;
                str += "Send3 : " + Send3 + Environment.NewLine;
                str += "Send4 : " + Send4 + Environment.NewLine;
                str += "Send5 : " + Send5 + Environment.NewLine;
                str += "Send6 : " + Send6 + Environment.NewLine;
                str += "Send7 : " + Send7 + Environment.NewLine;
                str += "Send8 : " + Send8 + Environment.NewLine;
                str += "Send9 : " + Send9 + Environment.NewLine;
                str += "SendA : " + SendA + Environment.NewLine;
                str += "SendB : " + SendB + Environment.NewLine;
                str += "SendC : " + SendC + Environment.NewLine;
                str += "SendD : " + SendD + Environment.NewLine;
                str += "SendE : " + SendE + Environment.NewLine;
                str += "SendF : " + SendF + Environment.NewLine;
                str += "SendG : " + SendG + Environment.NewLine;
                str += "SendH : " + SendH + Environment.NewLine;
                str += "SendI : " + SendI + Environment.NewLine;
                str += "SendJ : " + SendJ + Environment.NewLine;
                str += "SendK : " + SendK + Environment.NewLine;
                str += "SendL : " + SendL + Environment.NewLine;
                str += "SendM : " + SendM + Environment.NewLine;
                str += "SendN : " + SendN + Environment.NewLine;
                str += "SendO : " + SendO + Environment.NewLine;
                str += "SendP : " + SendP + Environment.NewLine;
                str += "SendQ : " + SendQ + Environment.NewLine;
                str += "SendR : " + SendR + Environment.NewLine;
                str += "SendS : " + SendS + Environment.NewLine;
                str += "SendT : " + SendT + Environment.NewLine;
                str += "SendU : " + SendU + Environment.NewLine;
                str += "SendV : " + SendV + Environment.NewLine;
                str += "SendX : " + SendX + Environment.NewLine;
                str += "SendY : " + SendY + Environment.NewLine;
                str += "SendZ : " + SendZ + Environment.NewLine;
                str += "SendLWIN : " + SendLWIN + Environment.NewLine;
                str += "SendRWIN : " + SendRWIN + Environment.NewLine;
                str += "SendAPPS : " + SendAPPS + Environment.NewLine;
                str += "SendSLEEP : " + SendSLEEP + Environment.NewLine;
                str += "SendNUMPAD0 : " + SendNUMPAD0 + Environment.NewLine;
                str += "SendNUMPAD1 : " + SendNUMPAD1 + Environment.NewLine;
                str += "SendNUMPAD2 : " + SendNUMPAD2 + Environment.NewLine;
                str += "SendNUMPAD3 : " + SendNUMPAD3 + Environment.NewLine;
                str += "SendNUMPAD4 : " + SendNUMPAD4 + Environment.NewLine;
                str += "SendNUMPAD5 : " + SendNUMPAD5 + Environment.NewLine;
                str += "SendNUMPAD6 : " + SendNUMPAD6 + Environment.NewLine;
                str += "SendNUMPAD7 : " + SendNUMPAD7 + Environment.NewLine;
                str += "SendNUMPAD8 : " + SendNUMPAD8 + Environment.NewLine;
                str += "SendNUMPAD9 : " + SendNUMPAD9 + Environment.NewLine;
                str += "SendMULTIPLY : " + SendMULTIPLY + Environment.NewLine;
                str += "SendADD : " + SendADD + Environment.NewLine;
                str += "SendSEPARATOR : " + SendSEPARATOR + Environment.NewLine;
                str += "SendSUBTRACT : " + SendSUBTRACT + Environment.NewLine;
                str += "SendDECIMAL : " + SendDECIMAL + Environment.NewLine;
                str += "SendDIVIDE : " + SendDIVIDE + Environment.NewLine;
                str += "SendF1 : " + SendF1 + Environment.NewLine;
                str += "SendF2 : " + SendF2 + Environment.NewLine;
                str += "SendF3 : " + SendF3 + Environment.NewLine;
                str += "SendF4 : " + SendF4 + Environment.NewLine;
                str += "SendF5 : " + SendF5 + Environment.NewLine;
                str += "SendF6 : " + SendF6 + Environment.NewLine;
                str += "SendF7 : " + SendF7 + Environment.NewLine;
                str += "SendF8 : " + SendF8 + Environment.NewLine;
                str += "SendF9 : " + SendF9 + Environment.NewLine;
                str += "SendF10 : " + SendF10 + Environment.NewLine;
                str += "SendF11 : " + SendF11 + Environment.NewLine;
                str += "SendF12 : " + SendF12 + Environment.NewLine;
                str += "SendF13 : " + SendF13 + Environment.NewLine;
                str += "SendF14 : " + SendF14 + Environment.NewLine;
                str += "SendF15 : " + SendF15 + Environment.NewLine;
                str += "SendF16 : " + SendF16 + Environment.NewLine;
                str += "SendF17 : " + SendF17 + Environment.NewLine;
                str += "SendF18 : " + SendF18 + Environment.NewLine;
                str += "SendF19 : " + SendF19 + Environment.NewLine;
                str += "SendF20 : " + SendF20 + Environment.NewLine;
                str += "SendF21 : " + SendF21 + Environment.NewLine;
                str += "SendF22 : " + SendF22 + Environment.NewLine;
                str += "SendF23 : " + SendF23 + Environment.NewLine;
                str += "SendF24 : " + SendF24 + Environment.NewLine;
                str += "SendNUMLOCK : " + SendNUMLOCK + Environment.NewLine;
                str += "SendSCROLL : " + SendSCROLL + Environment.NewLine;
                str += "SendLeftShift : " + SendLeftShift + Environment.NewLine;
                str += "SendRightShift : " + SendRightShift + Environment.NewLine;
                str += "SendLeftControl : " + SendLeftControl + Environment.NewLine;
                str += "SendRightControl : " + SendRightControl + Environment.NewLine;
                str += "SendLMENU : " + SendLMENU + Environment.NewLine;
                str += "SendRMENU : " + SendRMENU + Environment.NewLine;
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
        private void mousebrink(int x, int y)
        {
            if (drivertype == "sendinput")
                MouseBrink(x, y);
            else
                mouse_event(0x0001, x, y, 0, 0);
        }
        private void mousemw3(int x, int y)
        {
            if (drivertype == "sendinput")
                MouseMW3(x, y);
            else
                mouse_event(0x8001, x, y, 0, 0);
        }
        private void mouseclickleft()
        {
            if (drivertype == "sendinput")
                Task.Run(() => LeftClick());
            else
                Task.Run(() => mouse_event(0x0002, 0, 0, 0, 0));
        }
        private void mouseclickleftF()
        {
            if (drivertype == "sendinput")
                Task.Run(() => LeftClickF());
            else
                Task.Run(() => mouse_event(0x0004, 0, 0, 0, 0));
        }
        private void mouseclickright()
        {
            if (drivertype == "sendinput")
                Task.Run(() => RightClick());
            else
                Task.Run(() => mouse_event(0x0008, 0, 0, 0, 0));
        }
        private void mouseclickrightF()
        {
            if (drivertype == "sendinput")
                Task.Run(() => RightClickF());
            else
                Task.Run(() => mouse_event(0x0010, 0, 0, 0, 0));
        }
        private void mouseclickmiddle()
        {
            if (drivertype == "sendinput")
                Task.Run(() => MiddleClick());
            else
                Task.Run(() => mouse_event(0x0020, 0, 0, 0, 0));
        }
        private void mouseclickmiddleF()
        {
            if (drivertype == "sendinput")
                Task.Run(() => MiddleClickF());
            else
                Task.Run(() => mouse_event(0x0040, 0, 0, 0, 0));
        }
        private void mousewheelup()
        {
            if (drivertype == "sendinput")
                Task.Run(() => WheelUpF());
            else
                Task.Run(() => mouse_event(0x0800, 0, 0, 120, 0));
        }
        private void mousewheeldown()
        {
            if (drivertype == "sendinput")
                Task.Run(() => WheelDownF());
            else
                Task.Run(() => mouse_event(0x0800, 0, 0, -120, 0));
        }
        private void keyboard(UInt16 bVk, UInt16 bScan)
        {
            if (drivertype == "sendinput")
                Task.Run(() => SimulateKeyDown(bVk, bScan));
            else
                Task.Run(() => keybd_event(bVk, bScan, 0, 0));
        }
        private void keyboardF(UInt16 bVk, UInt16 bScan)
        {
            if (drivertype == "sendinput")
                Task.Run(() => SimulateKeyUp(bVk, bScan));
            else
                Task.Run(() => keybd_event(bVk, bScan, 0x0002, 0));
        }
        private void keyboardArrows(UInt16 bVk, UInt16 bScan)
        {
            if (drivertype == "sendinput")
                Task.Run(() => SimulateKeyDOWN(bVk, bScan));
            else
                Task.Run(() => { 
                    keybd_event(bVk, bScan, 0x0001 | 0x0008, 0);
                    keybd_event(bVk, bScan, 0, 0);
                });
        }
        private void keyboardArrowsF(UInt16 bVk, UInt16 bScan)
        {
            if (drivertype == "sendinput")
                Task.Run(() => SimulateKeyUP(bVk, bScan));
            else
                Task.Run(() => { 
                    keybd_event(bVk, bScan, 0x0002 | 0x0001 | 0x0008, 0);
                    keybd_event(bVk, bScan, 0x0002, 0);
                });
        }
        private static INPUT[] Micek = new INPUT[1], MiceW3 = new INPUT[1], Micewu = new INPUT[1], down = new INPUT[1], up = new INPUT[1], Micel = new INPUT[1], Micelf = new INPUT[1], Micerc = new INPUT[1], Micercf = new INPUT[1], Micemc = new INPUT[1], Micewd = new INPUT[1], Micemcf = new INPUT[1];
        private static void SimulateKeyDown(UInt16 keyCode, UInt16 bScan)
        {
            down[0].Type = (UInt32)InputType.KEYBOARD;
            down[0].Data.Keyboard = new KEYBDINPUT();
            down[0].Data.Keyboard.Vk = keyCode;
            down[0].Data.Keyboard.Scan = bScan;
            down[0].Data.Keyboard.Flags = 0;
            down[0].Data.Keyboard.Time = 0;
            down[0].Data.Keyboard.ExtraInfo = IntPtr.Zero;
            SendInput(1, down, Marshal.SizeOf(typeof(INPUT)));
        }
        private static void SimulateKeyUp(UInt16 keyCode, UInt16 bScan)
        {
            up[0].Type = (UInt32)InputType.KEYBOARD;
            up[0].Data.Keyboard = new KEYBDINPUT();
            up[0].Data.Keyboard.Vk = keyCode;
            up[0].Data.Keyboard.Scan = bScan;
            up[0].Data.Keyboard.Flags = (UInt16)(0x0002);
            up[0].Data.Keyboard.Time = 0;
            up[0].Data.Keyboard.ExtraInfo = IntPtr.Zero;
            SendInput(1, up, Marshal.SizeOf(typeof(INPUT)));
        }
        private static void SimulateKeyDOWN(UInt16 keyCode, UInt16 bScan)
        {
            down[0].Type = (UInt32)InputType.KEYBOARD;
            down[0].Data.Keyboard = new KEYBDINPUT();
            down[0].Data.Keyboard.Vk = keyCode;
            down[0].Data.Keyboard.Scan = bScan;
            down[0].Data.Keyboard.Flags = (UInt16)(0x0001) | (UInt16)(0x0008);
            down[0].Data.Keyboard.Time = 0;
            down[0].Data.Keyboard.ExtraInfo = IntPtr.Zero;
            SendInput(1, down, Marshal.SizeOf(typeof(INPUT)));
            down[0].Data.Keyboard.Flags = 0;
            SendInput(1, down, Marshal.SizeOf(typeof(INPUT)));
        }
        private static void SimulateKeyUP(UInt16 keyCode, UInt16 bScan)
        {
            up[0].Type = (UInt32)InputType.KEYBOARD;
            up[0].Data.Keyboard = new KEYBDINPUT();
            up[0].Data.Keyboard.Vk = keyCode;
            up[0].Data.Keyboard.Scan = bScan;
            up[0].Data.Keyboard.Flags = (UInt16)(0x0002) | (UInt16)(0x0001) | (UInt16)(0x0008);
            up[0].Data.Keyboard.Time = 0;
            up[0].Data.Keyboard.ExtraInfo = IntPtr.Zero;
            SendInput(1, up, Marshal.SizeOf(typeof(INPUT)));
            up[0].Data.Keyboard.Flags = (UInt16)(0x0002);
            SendInput(1, up, Marshal.SizeOf(typeof(INPUT)));
        }
        private static void MouseMW3(int x, int y)
        {
            MiceW3[0].Type = (UInt32)InputType.MOUSE;
            MiceW3[0].Data.Mouse = new MOUSEINPUT();
            MiceW3[0].Data.Mouse.MouseData = 1;
            MiceW3[0].Data.Mouse.Flags = (UInt16)(0x8001);
            MiceW3[0].Data.Mouse.Time = 0;
            MiceW3[0].Data.Mouse.X = x;
            MiceW3[0].Data.Mouse.Y = y;
            MiceW3[0].Data.Mouse.ExtraInfo = IntPtr.Zero;
            SendInput(1, MiceW3, Marshal.SizeOf(typeof(INPUT)));
        }
        private static void MouseBrink(int x, int y)
        {
            Micek[0].Type = (UInt32)InputType.MOUSE;
            Micek[0].Data.Mouse = new MOUSEINPUT();
            Micek[0].Data.Mouse.MouseData = 1;
            Micek[0].Data.Mouse.Flags = (UInt16)(0x0001);
            Micek[0].Data.Mouse.Time = 0;
            Micek[0].Data.Mouse.X = x;
            Micek[0].Data.Mouse.Y = y;
            Micek[0].Data.Mouse.ExtraInfo = IntPtr.Zero;
            SendInput(1, Micek, Marshal.SizeOf(typeof(INPUT)));
        }
        private static void LeftClick()
        {
            Micel[0].Type = (UInt32)InputType.MOUSE;
            Micel[0].Data.Mouse = new MOUSEINPUT();
            Micel[0].Data.Mouse.MouseData = 0;
            Micel[0].Data.Mouse.Flags = (UInt16)(0x0002);
            Micel[0].Data.Mouse.Time = 0;
            Micel[0].Data.Mouse.ExtraInfo = IntPtr.Zero;
            SendInput(1, Micel, Marshal.SizeOf(typeof(INPUT)));
        }
        private static void LeftClickF()
        {
            Micelf[0].Type = (UInt32)InputType.MOUSE;
            Micelf[0].Data.Mouse = new MOUSEINPUT();
            Micelf[0].Data.Mouse.MouseData = 0;
            Micelf[0].Data.Mouse.Flags = (UInt16)(0x0004);
            Micelf[0].Data.Mouse.Time = 0;
            Micelf[0].Data.Mouse.ExtraInfo = IntPtr.Zero;
            SendInput(1, Micelf, Marshal.SizeOf(typeof(INPUT)));
        }
        private static void RightClick()
        {
            Micerc[0].Type = (UInt32)InputType.MOUSE;
            Micerc[0].Data.Mouse = new MOUSEINPUT();
            Micerc[0].Data.Mouse.MouseData = 0;
            Micerc[0].Data.Mouse.Flags = (UInt16)(0x0008);
            Micerc[0].Data.Mouse.Time = 0;
            Micerc[0].Data.Mouse.ExtraInfo = IntPtr.Zero;
            SendInput(1, Micerc, Marshal.SizeOf(typeof(INPUT)));
        }
        private static void RightClickF()
        {
            Micercf[0].Type = (UInt32)InputType.MOUSE;
            Micercf[0].Data.Mouse = new MOUSEINPUT();
            Micercf[0].Data.Mouse.MouseData = 0;
            Micercf[0].Data.Mouse.Flags = (UInt16)(0x0010);
            Micercf[0].Data.Mouse.Time = 0;
            Micercf[0].Data.Mouse.ExtraInfo = IntPtr.Zero;
            SendInput(1, Micercf, Marshal.SizeOf(typeof(INPUT)));
        }
        private static void MiddleClick()
        {
            Micemc[0].Type = (UInt32)InputType.MOUSE;
            Micemc[0].Data.Mouse = new MOUSEINPUT();
            Micemc[0].Data.Mouse.MouseData = 0;
            Micemc[0].Data.Mouse.Flags = (UInt16)(0x0020);
            Micemc[0].Data.Mouse.Time = 0;
            Micemc[0].Data.Mouse.ExtraInfo = IntPtr.Zero;
            SendInput(1, Micemc, Marshal.SizeOf(typeof(INPUT)));
        }
        private static void MiddleClickF()
        {
            Micemcf[0].Type = (UInt32)InputType.MOUSE;
            Micemcf[0].Data.Mouse = new MOUSEINPUT();
            Micemcf[0].Data.Mouse.MouseData = 0;
            Micemcf[0].Data.Mouse.Flags = (UInt16)(0x0040);
            Micemcf[0].Data.Mouse.Time = 0;
            Micemcf[0].Data.Mouse.ExtraInfo = IntPtr.Zero;
            SendInput(1, Micemcf, Marshal.SizeOf(typeof(INPUT)));
        }
        private static void WheelDownF()
        {
            Micewd[0].Type = (UInt32)InputType.MOUSE;
            Micewd[0].Data.Mouse = new MOUSEINPUT();
            Micewd[0].Data.Mouse.MouseData = -120;
            Micewd[0].Data.Mouse.Flags = (UInt16)(0x0800);
            Micewd[0].Data.Mouse.Time = 0;
            Micewd[0].Data.Mouse.ExtraInfo = IntPtr.Zero;
            SendInput(1, Micewd, Marshal.SizeOf(typeof(INPUT)));
        }
        private static void WheelUpF()
        {
            Micewu[0].Type = (UInt32)InputType.MOUSE;
            Micewu[0].Data.Mouse = new MOUSEINPUT();
            Micewu[0].Data.Mouse.MouseData = 120;
            Micewu[0].Data.Mouse.Flags = (UInt16)(0x0800);
            Micewu[0].Data.Mouse.Time = 0;
            Micewu[0].Data.Mouse.ExtraInfo = IntPtr.Zero;
            SendInput(1, Micewu, Marshal.SizeOf(typeof(INPUT)));
        }
        [StructLayout(LayoutKind.Explicit)]
        private struct MOUSEKEYBDHARDWAREINPUT
        {
            [FieldOffset(0)]
            public MOUSEINPUT Mouse;
            [FieldOffset(0)]
            public KEYBDINPUT Keyboard;
        }
        private struct INPUT
        {
            public UInt32 Type;
            public MOUSEKEYBDHARDWAREINPUT Data;
        }
        private enum InputType : uint // UInt32
        {
            MOUSE = 0, KEYBOARD = 1, HARDWARE = 2,
        }
        private struct KEYBDINPUT
        {
            public UInt16 Vk;
            public UInt16 Scan;
            public uint Flags;
            public uint Time;
            public IntPtr ExtraInfo;
        }
        private struct MOUSEINPUT
        {
            public int X;
            public int Y;
            public int MouseData;
            public uint Flags;
            public uint Time;
            public IntPtr ExtraInfo;
        }
        public void Dispose()
        {
            GC.Collect();
            GC.SuppressFinalize(this);
        }
    }
}