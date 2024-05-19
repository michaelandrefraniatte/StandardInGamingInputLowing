using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Keyboardrawhooks;
using RawInput_dll;
using Valuechanges;

namespace KeyboardRawHooksAPI
{
    public class KeyboardRawHook
    {
        [DllImport("winmm.dll", EntryPoint = "timeBeginPeriod")]
        private static extern uint TimeBeginPeriod(uint ms);
        [DllImport("winmm.dll", EntryPoint = "timeEndPeriod")]
        private static extern uint TimeEndPeriod(uint ms);
        [DllImport("ntdll.dll", EntryPoint = "NtSetTimerResolution")]
        private static extern void NtSetTimerResolution(uint DesiredResolution, bool SetResolution, ref uint CurrentResolution);
        private static uint CurrentResolution = 0;
        private bool running, formvisible;
        private int number, inc;
        private RawInput _rawinput;
        private const bool CaptureOnlyInForeground = false;
        private static List<string> devicehandle = new List<string>();
        private Form1 form1 = new Form1();
        private Stopwatch PollingRate;
        private double pollingrateperm = 0, pollingratetemp = 0, pollingratedisplay = 0, pollingrate;
        private string inputdelaybutton = "", inputdelay = "", inputdelaytemp = "";
        public Valuechange ValueChange;
        private double delay, elapseddown, elapsedup, elapsed;
        private bool getstate = false;
        private int[] wd = { 2 };
        private int[] wu = { 2 };
        public void valchanged(int n, bool val)
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
        public KeyboardRawHook()
        {
            TimeBeginPeriod(1);
            NtSetTimerResolution(1, true, ref CurrentResolution);
            _rawinput = new RawInput(this.form1.Handle, CaptureOnlyInForeground);
            _rawinput.AddMessageFilter();
            _rawinput.KeyPressed += OnKeyPressed;
            running = true;
        }
        public void ViewData(string inputdelaybutton = "")
        {
            if (!formvisible)
            {
                PollingRate = new Stopwatch();
                PollingRate.Start();
                ValueChange = new Valuechange();
                this.inputdelaybutton = inputdelaybutton;
                formvisible = true;
                Task.Run(() => form1.SetVisible());
            }
        }
        public void Close()
        {
            running = false;
            Thread.Sleep(100);
            _rawinput.KeyPressed -= OnKeyPressed;
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
                    pollingratedisplay++;
                    pollingratetemp = pollingrateperm;
                    pollingrateperm = (double)PollingRate.ElapsedTicks / (Stopwatch.Frequency / 1000L);
                    if (pollingratedisplay > 300)
                    {
                        pollingrate = pollingrateperm - pollingratetemp;
                        pollingratedisplay = 0;
                    }
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
                    if (wd[0] == 1)
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
                    if (wu[0] == 1)
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
        }
        public void BeginPolling()
        {
            Task.Run(() => taskK());
        }
        public bool Scan(int number = 0)
        {
            this.number = number;
            inc = number < 2 ? 0 : number - 1;
            return true;
        }
        public void Init()
        {
        }
        private void OnKeyPressed(object sender, RawInputEventArg e)
        {
            devicehandle.Add(e.KeyPressEvent.DeviceHandle.ToString());
            devicehandle = devicehandle.Distinct().ToList();
            if (devicehandle[inc] == e.KeyPressEvent.DeviceHandle.ToString())
            {
                if (e.KeyPressEvent.KeyPressState == "MAKE")
                {
                    if (e.KeyPressEvent.VKey == VK_LBUTTON)
                        Key_LBUTTON = true;
                    if (e.KeyPressEvent.VKey == VK_RBUTTON)
                        Key_RBUTTON = true;
                    if (e.KeyPressEvent.VKey == VK_CANCEL)
                        Key_CANCEL = true;
                    if (e.KeyPressEvent.VKey == VK_MBUTTON)
                        Key_MBUTTON = true;
                    if (e.KeyPressEvent.VKey == VK_XBUTTON1)
                        Key_XBUTTON1 = true;
                    if (e.KeyPressEvent.VKey == VK_XBUTTON2)
                        Key_XBUTTON2 = true;
                    if (e.KeyPressEvent.VKey == VK_BACK)
                        Key_BACK = true;
                    if (e.KeyPressEvent.VKey == VK_Tab)
                        Key_Tab = true;
                    if (e.KeyPressEvent.VKey == VK_CLEAR)
                        Key_CLEAR = true;
                    if (e.KeyPressEvent.VKey == VK_Return)
                        Key_Return = true;
                    if (e.KeyPressEvent.VKey == VK_SHIFT)
                        Key_SHIFT = true;
                    if (e.KeyPressEvent.VKey == VK_CONTROL)
                        Key_CONTROL = true;
                    if (e.KeyPressEvent.VKey == VK_MENU)
                        Key_MENU = true;
                    if (e.KeyPressEvent.VKey == VK_PAUSE)
                        Key_PAUSE = true;
                    if (e.KeyPressEvent.VKey == VK_CAPITAL)
                        Key_CAPITAL = true;
                    if (e.KeyPressEvent.VKey == VK_KANA)
                        Key_KANA = true;
                    if (e.KeyPressEvent.VKey == VK_HANGEUL)
                        Key_HANGEUL = true;
                    if (e.KeyPressEvent.VKey == VK_HANGUL)
                        Key_HANGUL = true;
                    if (e.KeyPressEvent.VKey == VK_JUNJA)
                        Key_JUNJA = true;
                    if (e.KeyPressEvent.VKey == VK_FINAL)
                        Key_FINAL = true;
                    if (e.KeyPressEvent.VKey == VK_HANJA)
                        Key_HANJA = true;
                    if (e.KeyPressEvent.VKey == VK_KANJI)
                        Key_KANJI = true;
                    if (e.KeyPressEvent.VKey == VK_Escape)
                        Key_Escape = true;
                    if (e.KeyPressEvent.VKey == VK_CONVERT)
                        Key_CONVERT = true;
                    if (e.KeyPressEvent.VKey == VK_NONCONVERT)
                        Key_NONCONVERT = true;
                    if (e.KeyPressEvent.VKey == VK_ACCEPT)
                        Key_ACCEPT = true;
                    if (e.KeyPressEvent.VKey == VK_MODECHANGE)
                        Key_MODECHANGE = true;
                    if (e.KeyPressEvent.VKey == VK_Space)
                        Key_Space = true;
                    if (e.KeyPressEvent.VKey == VK_PRIOR)
                        Key_PRIOR = true;
                    if (e.KeyPressEvent.VKey == VK_NEXT)
                        Key_NEXT = true;
                    if (e.KeyPressEvent.VKey == VK_END)
                        Key_END = true;
                    if (e.KeyPressEvent.VKey == VK_HOME)
                        Key_HOME = true;
                    if (e.KeyPressEvent.VKey == VK_LEFT)
                        Key_LEFT = true;
                    if (e.KeyPressEvent.VKey == VK_UP)
                        Key_UP = true;
                    if (e.KeyPressEvent.VKey == VK_RIGHT)
                        Key_RIGHT = true;
                    if (e.KeyPressEvent.VKey == VK_DOWN)
                        Key_DOWN = true;
                    if (e.KeyPressEvent.VKey == VK_SELECT)
                        Key_SELECT = true;
                    if (e.KeyPressEvent.VKey == VK_PRINT)
                        Key_PRINT = true;
                    if (e.KeyPressEvent.VKey == VK_EXECUTE)
                        Key_EXECUTE = true;
                    if (e.KeyPressEvent.VKey == VK_SNAPSHOT)
                        Key_SNAPSHOT = true;
                    if (e.KeyPressEvent.VKey == VK_INSERT)
                        Key_INSERT = true;
                    if (e.KeyPressEvent.VKey == VK_DELETE)
                        Key_DELETE = true;
                    if (e.KeyPressEvent.VKey == VK_HELP)
                        Key_HELP = true;
                    if (e.KeyPressEvent.VKey == VK_APOSTROPHE)
                        Key_APOSTROPHE = true;
                    if (e.KeyPressEvent.VKey == VK_0)
                        Key_0 = true;
                    if (e.KeyPressEvent.VKey == VK_1)
                        Key_1 = true;
                    if (e.KeyPressEvent.VKey == VK_2)
                        Key_2 = true;
                    if (e.KeyPressEvent.VKey == VK_3)
                        Key_3 = true;
                    if (e.KeyPressEvent.VKey == VK_4)
                        Key_4 = true;
                    if (e.KeyPressEvent.VKey == VK_5)
                        Key_5 = true;
                    if (e.KeyPressEvent.VKey == VK_6)
                        Key_6 = true;
                    if (e.KeyPressEvent.VKey == VK_7)
                        Key_7 = true;
                    if (e.KeyPressEvent.VKey == VK_8)
                        Key_8 = true;
                    if (e.KeyPressEvent.VKey == VK_9)
                        Key_9 = true;
                    if (e.KeyPressEvent.VKey == VK_A)
                        Key_A = true;
                    if (e.KeyPressEvent.VKey == VK_B)
                        Key_B = true;
                    if (e.KeyPressEvent.VKey == VK_C)
                        Key_C = true;
                    if (e.KeyPressEvent.VKey == VK_D)
                        Key_D = true;
                    if (e.KeyPressEvent.VKey == VK_E)
                        Key_E = true;
                    if (e.KeyPressEvent.VKey == VK_F)
                        Key_F = true;
                    if (e.KeyPressEvent.VKey == VK_G)
                        Key_G = true;
                    if (e.KeyPressEvent.VKey == VK_H)
                        Key_H = true;
                    if (e.KeyPressEvent.VKey == VK_I)
                        Key_I = true;
                    if (e.KeyPressEvent.VKey == VK_J)
                        Key_J = true;
                    if (e.KeyPressEvent.VKey == VK_K)
                        Key_K = true;
                    if (e.KeyPressEvent.VKey == VK_L)
                        Key_L = true;
                    if (e.KeyPressEvent.VKey == VK_M)
                        Key_M = true;
                    if (e.KeyPressEvent.VKey == VK_N)
                        Key_N = true;
                    if (e.KeyPressEvent.VKey == VK_O)
                        Key_O = true;
                    if (e.KeyPressEvent.VKey == VK_P)
                        Key_P = true;
                    if (e.KeyPressEvent.VKey == VK_Q)
                        Key_Q = true;
                    if (e.KeyPressEvent.VKey == VK_R)
                        Key_R = true;
                    if (e.KeyPressEvent.VKey == VK_S)
                        Key_S = true;
                    if (e.KeyPressEvent.VKey == VK_T)
                        Key_T = true;
                    if (e.KeyPressEvent.VKey == VK_U)
                        Key_U = true;
                    if (e.KeyPressEvent.VKey == VK_V)
                        Key_V = true;
                    if (e.KeyPressEvent.VKey == VK_W)
                        Key_W = true;
                    if (e.KeyPressEvent.VKey == VK_X)
                        Key_X = true;
                    if (e.KeyPressEvent.VKey == VK_Y)
                        Key_Y = true;
                    if (e.KeyPressEvent.VKey == VK_Z)
                        Key_Z = true;
                    if (e.KeyPressEvent.VKey == VK_LWIN)
                        Key_LWIN = true;
                    if (e.KeyPressEvent.VKey == VK_RWIN)
                        Key_RWIN = true;
                    if (e.KeyPressEvent.VKey == VK_APPS)
                        Key_APPS = true;
                    if (e.KeyPressEvent.VKey == VK_SLEEP)
                        Key_SLEEP = true;
                    if (e.KeyPressEvent.VKey == VK_NUMPAD0)
                        Key_NUMPAD0 = true;
                    if (e.KeyPressEvent.VKey == VK_NUMPAD1)
                        Key_NUMPAD1 = true;
                    if (e.KeyPressEvent.VKey == VK_NUMPAD2)
                        Key_NUMPAD2 = true;
                    if (e.KeyPressEvent.VKey == VK_NUMPAD3)
                        Key_NUMPAD3 = true;
                    if (e.KeyPressEvent.VKey == VK_NUMPAD4)
                        Key_NUMPAD4 = true;
                    if (e.KeyPressEvent.VKey == VK_NUMPAD5)
                        Key_NUMPAD5 = true;
                    if (e.KeyPressEvent.VKey == VK_NUMPAD6)
                        Key_NUMPAD6 = true;
                    if (e.KeyPressEvent.VKey == VK_NUMPAD7)
                        Key_NUMPAD7 = true;
                    if (e.KeyPressEvent.VKey == VK_NUMPAD8)
                        Key_NUMPAD8 = true;
                    if (e.KeyPressEvent.VKey == VK_NUMPAD9)
                        Key_NUMPAD9 = true;
                    if (e.KeyPressEvent.VKey == VK_MULTIPLY)
                        Key_MULTIPLY = true;
                    if (e.KeyPressEvent.VKey == VK_ADD)
                        Key_ADD = true;
                    if (e.KeyPressEvent.VKey == VK_SEPARATOR)
                        Key_SEPARATOR = true;
                    if (e.KeyPressEvent.VKey == VK_SUBTRACT)
                        Key_SUBTRACT = true;
                    if (e.KeyPressEvent.VKey == VK_DECIMAL)
                        Key_DECIMAL = true;
                    if (e.KeyPressEvent.VKey == VK_DIVIDE)
                        Key_DIVIDE = true;
                    if (e.KeyPressEvent.VKey == VK_F1)
                        Key_F1 = true;
                    if (e.KeyPressEvent.VKey == VK_F2)
                        Key_F2 = true;
                    if (e.KeyPressEvent.VKey == VK_F3)
                        Key_F3 = true;
                    if (e.KeyPressEvent.VKey == VK_F4)
                        Key_F4 = true;
                    if (e.KeyPressEvent.VKey == VK_F5)
                        Key_F5 = true;
                    if (e.KeyPressEvent.VKey == VK_F6)
                        Key_F6 = true;
                    if (e.KeyPressEvent.VKey == VK_F7)
                        Key_F7 = true;
                    if (e.KeyPressEvent.VKey == VK_F8)
                        Key_F8 = true;
                    if (e.KeyPressEvent.VKey == VK_F9)
                        Key_F9 = true;
                    if (e.KeyPressEvent.VKey == VK_F10)
                        Key_F10 = true;
                    if (e.KeyPressEvent.VKey == VK_F11)
                        Key_F11 = true;
                    if (e.KeyPressEvent.VKey == VK_F12)
                        Key_F12 = true;
                    if (e.KeyPressEvent.VKey == VK_F13)
                        Key_F13 = true;
                    if (e.KeyPressEvent.VKey == VK_F14)
                        Key_F14 = true;
                    if (e.KeyPressEvent.VKey == VK_F15)
                        Key_F15 = true;
                    if (e.KeyPressEvent.VKey == VK_F16)
                        Key_F16 = true;
                    if (e.KeyPressEvent.VKey == VK_F17)
                        Key_F17 = true;
                    if (e.KeyPressEvent.VKey == VK_F18)
                        Key_F18 = true;
                    if (e.KeyPressEvent.VKey == VK_F19)
                        Key_F19 = true;
                    if (e.KeyPressEvent.VKey == VK_F20)
                        Key_F20 = true;
                    if (e.KeyPressEvent.VKey == VK_F21)
                        Key_F21 = true;
                    if (e.KeyPressEvent.VKey == VK_F22)
                        Key_F22 = true;
                    if (e.KeyPressEvent.VKey == VK_F23)
                        Key_F23 = true;
                    if (e.KeyPressEvent.VKey == VK_F24)
                        Key_F24 = true;
                    if (e.KeyPressEvent.VKey == VK_NUMLOCK)
                        Key_NUMLOCK = true;
                    if (e.KeyPressEvent.VKey == VK_SCROLL)
                        Key_SCROLL = true;
                    if (e.KeyPressEvent.VKey == VK_LeftShift)
                        Key_LeftShift = true;
                    if (e.KeyPressEvent.VKey == VK_RightShift)
                        Key_RightShift = true;
                    if (e.KeyPressEvent.VKey == VK_LeftControl)
                        Key_LeftControl = true;
                    if (e.KeyPressEvent.VKey == VK_RightControl)
                        Key_RightControl = true;
                    if (e.KeyPressEvent.VKey == VK_LMENU)
                        Key_LMENU = true;
                    if (e.KeyPressEvent.VKey == VK_RMENU)
                        Key_RMENU = true;
                    if (e.KeyPressEvent.VKey == VK_BROWSER_BACK)
                        Key_BROWSER_BACK = true;
                    if (e.KeyPressEvent.VKey == VK_BROWSER_FORWARD)
                        Key_BROWSER_FORWARD = true;
                    if (e.KeyPressEvent.VKey == VK_BROWSER_REFRESH)
                        Key_BROWSER_REFRESH = true;
                    if (e.KeyPressEvent.VKey == VK_BROWSER_STOP)
                        Key_BROWSER_STOP = true;
                    if (e.KeyPressEvent.VKey == VK_BROWSER_SEARCH)
                        Key_BROWSER_SEARCH = true;
                    if (e.KeyPressEvent.VKey == VK_BROWSER_FAVORITES)
                        Key_BROWSER_FAVORITES = true;
                    if (e.KeyPressEvent.VKey == VK_BROWSER_HOME)
                        Key_BROWSER_HOME = true;
                    if (e.KeyPressEvent.VKey == VK_VOLUME_MUTE)
                        Key_VOLUME_MUTE = true;
                    if (e.KeyPressEvent.VKey == VK_VOLUME_DOWN)
                        Key_VOLUME_DOWN = true;
                    if (e.KeyPressEvent.VKey == VK_VOLUME_UP)
                        Key_VOLUME_UP = true;
                    if (e.KeyPressEvent.VKey == VK_MEDIA_NEXT_TRACK)
                        Key_MEDIA_NEXT_TRACK = true;
                    if (e.KeyPressEvent.VKey == VK_MEDIA_PREV_TRACK)
                        Key_MEDIA_PREV_TRACK = true;
                    if (e.KeyPressEvent.VKey == VK_MEDIA_STOP)
                        Key_MEDIA_STOP = true;
                    if (e.KeyPressEvent.VKey == VK_MEDIA_PLAY_PAUSE)
                        Key_MEDIA_PLAY_PAUSE = true;
                    if (e.KeyPressEvent.VKey == VK_LAUNCH_MAIL)
                        Key_LAUNCH_MAIL = true;
                    if (e.KeyPressEvent.VKey == VK_LAUNCH_MEDIA_SELECT)
                        Key_LAUNCH_MEDIA_SELECT = true;
                    if (e.KeyPressEvent.VKey == VK_LAUNCH_APP1)
                        Key_LAUNCH_APP1 = true;
                    if (e.KeyPressEvent.VKey == VK_LAUNCH_APP2)
                        Key_LAUNCH_APP2 = true;
                    if (e.KeyPressEvent.VKey == VK_OEM_1)
                        Key_OEM_1 = true;
                    if (e.KeyPressEvent.VKey == VK_OEM_PLUS)
                        Key_OEM_PLUS = true;
                    if (e.KeyPressEvent.VKey == VK_OEM_COMMA)
                        Key_OEM_COMMA = true;
                    if (e.KeyPressEvent.VKey == VK_OEM_MINUS)
                        Key_OEM_MINUS = true;
                    if (e.KeyPressEvent.VKey == VK_OEM_PERIOD)
                        Key_OEM_PERIOD = true;
                    if (e.KeyPressEvent.VKey == VK_OEM_2)
                        Key_OEM_2 = true;
                    if (e.KeyPressEvent.VKey == VK_OEM_3)
                        Key_OEM_3 = true;
                    if (e.KeyPressEvent.VKey == VK_OEM_4)
                        Key_OEM_4 = true;
                    if (e.KeyPressEvent.VKey == VK_OEM_5)
                        Key_OEM_5 = true;
                    if (e.KeyPressEvent.VKey == VK_OEM_6)
                        Key_OEM_6 = true;
                    if (e.KeyPressEvent.VKey == VK_OEM_7)
                        Key_OEM_7 = true;
                    if (e.KeyPressEvent.VKey == VK_OEM_8)
                        Key_OEM_8 = true;
                    if (e.KeyPressEvent.VKey == VK_OEM_102)
                        Key_OEM_102 = true;
                    if (e.KeyPressEvent.VKey == VK_PROCESSKEY)
                        Key_PROCESSKEY = true;
                    if (e.KeyPressEvent.VKey == VK_PACKET)
                        Key_PACKET = true;
                    if (e.KeyPressEvent.VKey == VK_ATTN)
                        Key_ATTN = true;
                    if (e.KeyPressEvent.VKey == VK_CRSEL)
                        Key_CRSEL = true;
                    if (e.KeyPressEvent.VKey == VK_EXSEL)
                        Key_EXSEL = true;
                    if (e.KeyPressEvent.VKey == VK_EREOF)
                        Key_EREOF = true;
                    if (e.KeyPressEvent.VKey == VK_PLAY)
                        Key_PLAY = true;
                    if (e.KeyPressEvent.VKey == VK_ZOOM)
                        Key_ZOOM = true;
                    if (e.KeyPressEvent.VKey == VK_NONAME)
                        Key_NONAME = true;
                    if (e.KeyPressEvent.VKey == VK_PA1)
                        Key_PA1 = true;
                    if (e.KeyPressEvent.VKey == VK_OEM_CLEAR)
                        Key_OEM_CLEAR = true;
                }
                if (e.KeyPressEvent.KeyPressState == "BREAK")
                {
                    if (e.KeyPressEvent.VKey == VK_LBUTTON)
                        Key_LBUTTON = false;
                    if (e.KeyPressEvent.VKey == VK_RBUTTON)
                        Key_RBUTTON = false;
                    if (e.KeyPressEvent.VKey == VK_CANCEL)
                        Key_CANCEL = false;
                    if (e.KeyPressEvent.VKey == VK_MBUTTON)
                        Key_MBUTTON = false;
                    if (e.KeyPressEvent.VKey == VK_XBUTTON1)
                        Key_XBUTTON1 = false;
                    if (e.KeyPressEvent.VKey == VK_XBUTTON2)
                        Key_XBUTTON2 = false;
                    if (e.KeyPressEvent.VKey == VK_BACK)
                        Key_BACK = false;
                    if (e.KeyPressEvent.VKey == VK_Tab)
                        Key_Tab = false;
                    if (e.KeyPressEvent.VKey == VK_CLEAR)
                        Key_CLEAR = false;
                    if (e.KeyPressEvent.VKey == VK_Return)
                        Key_Return = false;
                    if (e.KeyPressEvent.VKey == VK_SHIFT)
                        Key_SHIFT = false;
                    if (e.KeyPressEvent.VKey == VK_CONTROL)
                        Key_CONTROL = false;
                    if (e.KeyPressEvent.VKey == VK_MENU)
                        Key_MENU = false;
                    if (e.KeyPressEvent.VKey == VK_PAUSE)
                        Key_PAUSE = false;
                    if (e.KeyPressEvent.VKey == VK_CAPITAL)
                        Key_CAPITAL = false;
                    if (e.KeyPressEvent.VKey == VK_KANA)
                        Key_KANA = false;
                    if (e.KeyPressEvent.VKey == VK_HANGEUL)
                        Key_HANGEUL = false;
                    if (e.KeyPressEvent.VKey == VK_HANGUL)
                        Key_HANGUL = false;
                    if (e.KeyPressEvent.VKey == VK_JUNJA)
                        Key_JUNJA = false;
                    if (e.KeyPressEvent.VKey == VK_FINAL)
                        Key_FINAL = false;
                    if (e.KeyPressEvent.VKey == VK_HANJA)
                        Key_HANJA = false;
                    if (e.KeyPressEvent.VKey == VK_KANJI)
                        Key_KANJI = false;
                    if (e.KeyPressEvent.VKey == VK_Escape)
                        Key_Escape = false;
                    if (e.KeyPressEvent.VKey == VK_CONVERT)
                        Key_CONVERT = false;
                    if (e.KeyPressEvent.VKey == VK_NONCONVERT)
                        Key_NONCONVERT = false;
                    if (e.KeyPressEvent.VKey == VK_ACCEPT)
                        Key_ACCEPT = false;
                    if (e.KeyPressEvent.VKey == VK_MODECHANGE)
                        Key_MODECHANGE = false;
                    if (e.KeyPressEvent.VKey == VK_Space)
                        Key_Space = false;
                    if (e.KeyPressEvent.VKey == VK_PRIOR)
                        Key_PRIOR = false;
                    if (e.KeyPressEvent.VKey == VK_NEXT)
                        Key_NEXT = false;
                    if (e.KeyPressEvent.VKey == VK_END)
                        Key_END = false;
                    if (e.KeyPressEvent.VKey == VK_HOME)
                        Key_HOME = false;
                    if (e.KeyPressEvent.VKey == VK_LEFT)
                        Key_LEFT = false;
                    if (e.KeyPressEvent.VKey == VK_UP)
                        Key_UP = false;
                    if (e.KeyPressEvent.VKey == VK_RIGHT)
                        Key_RIGHT = false;
                    if (e.KeyPressEvent.VKey == VK_DOWN)
                        Key_DOWN = false;
                    if (e.KeyPressEvent.VKey == VK_SELECT)
                        Key_SELECT = false;
                    if (e.KeyPressEvent.VKey == VK_PRINT)
                        Key_PRINT = false;
                    if (e.KeyPressEvent.VKey == VK_EXECUTE)
                        Key_EXECUTE = false;
                    if (e.KeyPressEvent.VKey == VK_SNAPSHOT)
                        Key_SNAPSHOT = false;
                    if (e.KeyPressEvent.VKey == VK_INSERT)
                        Key_INSERT = false;
                    if (e.KeyPressEvent.VKey == VK_DELETE)
                        Key_DELETE = false;
                    if (e.KeyPressEvent.VKey == VK_HELP)
                        Key_HELP = false;
                    if (e.KeyPressEvent.VKey == VK_APOSTROPHE)
                        Key_APOSTROPHE = false;
                    if (e.KeyPressEvent.VKey == VK_0)
                        Key_0 = false;
                    if (e.KeyPressEvent.VKey == VK_1)
                        Key_1 = false;
                    if (e.KeyPressEvent.VKey == VK_2)
                        Key_2 = false;
                    if (e.KeyPressEvent.VKey == VK_3)
                        Key_3 = false;
                    if (e.KeyPressEvent.VKey == VK_4)
                        Key_4 = false;
                    if (e.KeyPressEvent.VKey == VK_5)
                        Key_5 = false;
                    if (e.KeyPressEvent.VKey == VK_6)
                        Key_6 = false;
                    if (e.KeyPressEvent.VKey == VK_7)
                        Key_7 = false;
                    if (e.KeyPressEvent.VKey == VK_8)
                        Key_8 = false;
                    if (e.KeyPressEvent.VKey == VK_9)
                        Key_9 = false;
                    if (e.KeyPressEvent.VKey == VK_A)
                        Key_A = false;
                    if (e.KeyPressEvent.VKey == VK_B)
                        Key_B = false;
                    if (e.KeyPressEvent.VKey == VK_C)
                        Key_C = false;
                    if (e.KeyPressEvent.VKey == VK_D)
                        Key_D = false;
                    if (e.KeyPressEvent.VKey == VK_E)
                        Key_E = false;
                    if (e.KeyPressEvent.VKey == VK_F)
                        Key_F = false;
                    if (e.KeyPressEvent.VKey == VK_G)
                        Key_G = false;
                    if (e.KeyPressEvent.VKey == VK_H)
                        Key_H = false;
                    if (e.KeyPressEvent.VKey == VK_I)
                        Key_I = false;
                    if (e.KeyPressEvent.VKey == VK_J)
                        Key_J = false;
                    if (e.KeyPressEvent.VKey == VK_K)
                        Key_K = false;
                    if (e.KeyPressEvent.VKey == VK_L)
                        Key_L = false;
                    if (e.KeyPressEvent.VKey == VK_M)
                        Key_M = false;
                    if (e.KeyPressEvent.VKey == VK_N)
                        Key_N = false;
                    if (e.KeyPressEvent.VKey == VK_O)
                        Key_O = false;
                    if (e.KeyPressEvent.VKey == VK_P)
                        Key_P = false;
                    if (e.KeyPressEvent.VKey == VK_Q)
                        Key_Q = false;
                    if (e.KeyPressEvent.VKey == VK_R)
                        Key_R = false;
                    if (e.KeyPressEvent.VKey == VK_S)
                        Key_S = false;
                    if (e.KeyPressEvent.VKey == VK_T)
                        Key_T = false;
                    if (e.KeyPressEvent.VKey == VK_U)
                        Key_U = false;
                    if (e.KeyPressEvent.VKey == VK_V)
                        Key_V = false;
                    if (e.KeyPressEvent.VKey == VK_W)
                        Key_W = false;
                    if (e.KeyPressEvent.VKey == VK_X)
                        Key_X = false;
                    if (e.KeyPressEvent.VKey == VK_Y)
                        Key_Y = false;
                    if (e.KeyPressEvent.VKey == VK_Z)
                        Key_Z = false;
                    if (e.KeyPressEvent.VKey == VK_LWIN)
                        Key_LWIN = false;
                    if (e.KeyPressEvent.VKey == VK_RWIN)
                        Key_RWIN = false;
                    if (e.KeyPressEvent.VKey == VK_APPS)
                        Key_APPS = false;
                    if (e.KeyPressEvent.VKey == VK_SLEEP)
                        Key_SLEEP = false;
                    if (e.KeyPressEvent.VKey == VK_NUMPAD0)
                        Key_NUMPAD0 = false;
                    if (e.KeyPressEvent.VKey == VK_NUMPAD1)
                        Key_NUMPAD1 = false;
                    if (e.KeyPressEvent.VKey == VK_NUMPAD2)
                        Key_NUMPAD2 = false;
                    if (e.KeyPressEvent.VKey == VK_NUMPAD3)
                        Key_NUMPAD3 = false;
                    if (e.KeyPressEvent.VKey == VK_NUMPAD4)
                        Key_NUMPAD4 = false;
                    if (e.KeyPressEvent.VKey == VK_NUMPAD5)
                        Key_NUMPAD5 = false;
                    if (e.KeyPressEvent.VKey == VK_NUMPAD6)
                        Key_NUMPAD6 = false;
                    if (e.KeyPressEvent.VKey == VK_NUMPAD7)
                        Key_NUMPAD7 = false;
                    if (e.KeyPressEvent.VKey == VK_NUMPAD8)
                        Key_NUMPAD8 = false;
                    if (e.KeyPressEvent.VKey == VK_NUMPAD9)
                        Key_NUMPAD9 = false;
                    if (e.KeyPressEvent.VKey == VK_MULTIPLY)
                        Key_MULTIPLY = false;
                    if (e.KeyPressEvent.VKey == VK_ADD)
                        Key_ADD = false;
                    if (e.KeyPressEvent.VKey == VK_SEPARATOR)
                        Key_SEPARATOR = false;
                    if (e.KeyPressEvent.VKey == VK_SUBTRACT)
                        Key_SUBTRACT = false;
                    if (e.KeyPressEvent.VKey == VK_DECIMAL)
                        Key_DECIMAL = false;
                    if (e.KeyPressEvent.VKey == VK_DIVIDE)
                        Key_DIVIDE = false;
                    if (e.KeyPressEvent.VKey == VK_F1)
                        Key_F1 = false;
                    if (e.KeyPressEvent.VKey == VK_F2)
                        Key_F2 = false;
                    if (e.KeyPressEvent.VKey == VK_F3)
                        Key_F3 = false;
                    if (e.KeyPressEvent.VKey == VK_F4)
                        Key_F4 = false;
                    if (e.KeyPressEvent.VKey == VK_F5)
                        Key_F5 = false;
                    if (e.KeyPressEvent.VKey == VK_F6)
                        Key_F6 = false;
                    if (e.KeyPressEvent.VKey == VK_F7)
                        Key_F7 = false;
                    if (e.KeyPressEvent.VKey == VK_F8)
                        Key_F8 = false;
                    if (e.KeyPressEvent.VKey == VK_F9)
                        Key_F9 = false;
                    if (e.KeyPressEvent.VKey == VK_F10)
                        Key_F10 = false;
                    if (e.KeyPressEvent.VKey == VK_F11)
                        Key_F11 = false;
                    if (e.KeyPressEvent.VKey == VK_F12)
                        Key_F12 = false;
                    if (e.KeyPressEvent.VKey == VK_F13)
                        Key_F13 = false;
                    if (e.KeyPressEvent.VKey == VK_F14)
                        Key_F14 = false;
                    if (e.KeyPressEvent.VKey == VK_F15)
                        Key_F15 = false;
                    if (e.KeyPressEvent.VKey == VK_F16)
                        Key_F16 = false;
                    if (e.KeyPressEvent.VKey == VK_F17)
                        Key_F17 = false;
                    if (e.KeyPressEvent.VKey == VK_F18)
                        Key_F18 = false;
                    if (e.KeyPressEvent.VKey == VK_F19)
                        Key_F19 = false;
                    if (e.KeyPressEvent.VKey == VK_F20)
                        Key_F20 = false;
                    if (e.KeyPressEvent.VKey == VK_F21)
                        Key_F21 = false;
                    if (e.KeyPressEvent.VKey == VK_F22)
                        Key_F22 = false;
                    if (e.KeyPressEvent.VKey == VK_F23)
                        Key_F23 = false;
                    if (e.KeyPressEvent.VKey == VK_F24)
                        Key_F24 = false;
                    if (e.KeyPressEvent.VKey == VK_NUMLOCK)
                        Key_NUMLOCK = false;
                    if (e.KeyPressEvent.VKey == VK_SCROLL)
                        Key_SCROLL = false;
                    if (e.KeyPressEvent.VKey == VK_LeftShift)
                        Key_LeftShift = false;
                    if (e.KeyPressEvent.VKey == VK_RightShift)
                        Key_RightShift = false;
                    if (e.KeyPressEvent.VKey == VK_LeftControl)
                        Key_LeftControl = false;
                    if (e.KeyPressEvent.VKey == VK_RightControl)
                        Key_RightControl = false;
                    if (e.KeyPressEvent.VKey == VK_LMENU)
                        Key_LMENU = false;
                    if (e.KeyPressEvent.VKey == VK_RMENU)
                        Key_RMENU = false;
                    if (e.KeyPressEvent.VKey == VK_BROWSER_BACK)
                        Key_BROWSER_BACK = false;
                    if (e.KeyPressEvent.VKey == VK_BROWSER_FORWARD)
                        Key_BROWSER_FORWARD = false;
                    if (e.KeyPressEvent.VKey == VK_BROWSER_REFRESH)
                        Key_BROWSER_REFRESH = false;
                    if (e.KeyPressEvent.VKey == VK_BROWSER_STOP)
                        Key_BROWSER_STOP = false;
                    if (e.KeyPressEvent.VKey == VK_BROWSER_SEARCH)
                        Key_BROWSER_SEARCH = false;
                    if (e.KeyPressEvent.VKey == VK_BROWSER_FAVORITES)
                        Key_BROWSER_FAVORITES = false;
                    if (e.KeyPressEvent.VKey == VK_BROWSER_HOME)
                        Key_BROWSER_HOME = false;
                    if (e.KeyPressEvent.VKey == VK_VOLUME_MUTE)
                        Key_VOLUME_MUTE = false;
                    if (e.KeyPressEvent.VKey == VK_VOLUME_DOWN)
                        Key_VOLUME_DOWN = false;
                    if (e.KeyPressEvent.VKey == VK_VOLUME_UP)
                        Key_VOLUME_UP = false;
                    if (e.KeyPressEvent.VKey == VK_MEDIA_NEXT_TRACK)
                        Key_MEDIA_NEXT_TRACK = false;
                    if (e.KeyPressEvent.VKey == VK_MEDIA_PREV_TRACK)
                        Key_MEDIA_PREV_TRACK = false;
                    if (e.KeyPressEvent.VKey == VK_MEDIA_STOP)
                        Key_MEDIA_STOP = false;
                    if (e.KeyPressEvent.VKey == VK_MEDIA_PLAY_PAUSE)
                        Key_MEDIA_PLAY_PAUSE = false;
                    if (e.KeyPressEvent.VKey == VK_LAUNCH_MAIL)
                        Key_LAUNCH_MAIL = false;
                    if (e.KeyPressEvent.VKey == VK_LAUNCH_MEDIA_SELECT)
                        Key_LAUNCH_MEDIA_SELECT = false;
                    if (e.KeyPressEvent.VKey == VK_LAUNCH_APP1)
                        Key_LAUNCH_APP1 = false;
                    if (e.KeyPressEvent.VKey == VK_LAUNCH_APP2)
                        Key_LAUNCH_APP2 = false;
                    if (e.KeyPressEvent.VKey == VK_OEM_1)
                        Key_OEM_1 = false;
                    if (e.KeyPressEvent.VKey == VK_OEM_PLUS)
                        Key_OEM_PLUS = false;
                    if (e.KeyPressEvent.VKey == VK_OEM_COMMA)
                        Key_OEM_COMMA = false;
                    if (e.KeyPressEvent.VKey == VK_OEM_MINUS)
                        Key_OEM_MINUS = false;
                    if (e.KeyPressEvent.VKey == VK_OEM_PERIOD)
                        Key_OEM_PERIOD = false;
                    if (e.KeyPressEvent.VKey == VK_OEM_2)
                        Key_OEM_2 = false;
                    if (e.KeyPressEvent.VKey == VK_OEM_3)
                        Key_OEM_3 = false;
                    if (e.KeyPressEvent.VKey == VK_OEM_4)
                        Key_OEM_4 = false;
                    if (e.KeyPressEvent.VKey == VK_OEM_5)
                        Key_OEM_5 = false;
                    if (e.KeyPressEvent.VKey == VK_OEM_6)
                        Key_OEM_6 = false;
                    if (e.KeyPressEvent.VKey == VK_OEM_7)
                        Key_OEM_7 = false;
                    if (e.KeyPressEvent.VKey == VK_OEM_8)
                        Key_OEM_8 = false;
                    if (e.KeyPressEvent.VKey == VK_OEM_102)
                        Key_OEM_102 = false;
                    if (e.KeyPressEvent.VKey == VK_PROCESSKEY)
                        Key_PROCESSKEY = false;
                    if (e.KeyPressEvent.VKey == VK_PACKET)
                        Key_PACKET = false;
                    if (e.KeyPressEvent.VKey == VK_ATTN)
                        Key_ATTN = false;
                    if (e.KeyPressEvent.VKey == VK_CRSEL)
                        Key_CRSEL = false;
                    if (e.KeyPressEvent.VKey == VK_EXSEL)
                        Key_EXSEL = false;
                    if (e.KeyPressEvent.VKey == VK_EREOF)
                        Key_EREOF = false;
                    if (e.KeyPressEvent.VKey == VK_PLAY)
                        Key_PLAY = false;
                    if (e.KeyPressEvent.VKey == VK_ZOOM)
                        Key_ZOOM = false;
                    if (e.KeyPressEvent.VKey == VK_NONAME)
                        Key_NONAME = false;
                    if (e.KeyPressEvent.VKey == VK_PA1)
                        Key_PA1 = false;
                    if (e.KeyPressEvent.VKey == VK_OEM_CLEAR)
                        Key_OEM_CLEAR = false;
                }
            }
        }
        private const int VK_LBUTTON = (int)0x01;
        private const int VK_RBUTTON = (int)0x02;
        private const int VK_CANCEL = (int)0x03;
        private const int VK_MBUTTON = (int)0x04;
        private const int VK_XBUTTON1 = (int)0x05;
        private const int VK_XBUTTON2 = (int)0x06;
        private const int VK_BACK = (int)0x08;
        private const int VK_Tab = (int)0x09;
        private const int VK_CLEAR = (int)0x0C;
        private const int VK_Return = (int)0x0D;
        private const int VK_SHIFT = (int)0x10;
        private const int VK_CONTROL = (int)0x11;
        private const int VK_MENU = (int)0x12;
        private const int VK_PAUSE = (int)0x13;
        private const int VK_CAPITAL = (int)0x14;
        private const int VK_KANA = (int)0x15;
        private const int VK_HANGEUL = (int)0x15;
        private const int VK_HANGUL = (int)0x15;
        private const int VK_JUNJA = (int)0x17;
        private const int VK_FINAL = (int)0x18;
        private const int VK_HANJA = (int)0x19;
        private const int VK_KANJI = (int)0x19;
        private const int VK_Escape = (int)0x1B;
        private const int VK_CONVERT = (int)0x1C;
        private const int VK_NONCONVERT = (int)0x1D;
        private const int VK_ACCEPT = (int)0x1E;
        private const int VK_MODECHANGE = (int)0x1F;
        private const int VK_Space = (int)0x20;
        private const int VK_PRIOR = (int)0x21;
        private const int VK_NEXT = (int)0x22;
        private const int VK_END = (int)0x23;
        private const int VK_HOME = (int)0x24;
        private const int VK_LEFT = (int)0x25;
        private const int VK_UP = (int)0x26;
        private const int VK_RIGHT = (int)0x27;
        private const int VK_DOWN = (int)0x28;
        private const int VK_SELECT = (int)0x29;
        private const int VK_PRINT = (int)0x2A;
        private const int VK_EXECUTE = (int)0x2B;
        private const int VK_SNAPSHOT = (int)0x2C;
        private const int VK_INSERT = (int)0x2D;
        private const int VK_DELETE = (int)0x2E;
        private const int VK_HELP = (int)0x2F;
        private const int VK_APOSTROPHE = (int)0xDE;
        private const int VK_0 = (int)0x30;
        private const int VK_1 = (int)0x31;
        private const int VK_2 = (int)0x32;
        private const int VK_3 = (int)0x33;
        private const int VK_4 = (int)0x34;
        private const int VK_5 = (int)0x35;
        private const int VK_6 = (int)0x36;
        private const int VK_7 = (int)0x37;
        private const int VK_8 = (int)0x38;
        private const int VK_9 = (int)0x39;
        private const int VK_A = (int)0x41;
        private const int VK_B = (int)0x42;
        private const int VK_C = (int)0x43;
        private const int VK_D = (int)0x44;
        private const int VK_E = (int)0x45;
        private const int VK_F = (int)0x46;
        private const int VK_G = (int)0x47;
        private const int VK_H = (int)0x48;
        private const int VK_I = (int)0x49;
        private const int VK_J = (int)0x4A;
        private const int VK_K = (int)0x4B;
        private const int VK_L = (int)0x4C;
        private const int VK_M = (int)0x4D;
        private const int VK_N = (int)0x4E;
        private const int VK_O = (int)0x4F;
        private const int VK_P = (int)0x50;
        private const int VK_Q = (int)0x51;
        private const int VK_R = (int)0x52;
        private const int VK_S = (int)0x53;
        private const int VK_T = (int)0x54;
        private const int VK_U = (int)0x55;
        private const int VK_V = (int)0x56;
        private const int VK_W = (int)0x57;
        private const int VK_X = (int)0x58;
        private const int VK_Y = (int)0x59;
        private const int VK_Z = (int)0x5A;
        private const int VK_LWIN = (int)0x5B;
        private const int VK_RWIN = (int)0x5C;
        private const int VK_APPS = (int)0x5D;
        private const int VK_SLEEP = (int)0x5F;
        private const int VK_NUMPAD0 = (int)0x60;
        private const int VK_NUMPAD1 = (int)0x61;
        private const int VK_NUMPAD2 = (int)0x62;
        private const int VK_NUMPAD3 = (int)0x63;
        private const int VK_NUMPAD4 = (int)0x64;
        private const int VK_NUMPAD5 = (int)0x65;
        private const int VK_NUMPAD6 = (int)0x66;
        private const int VK_NUMPAD7 = (int)0x67;
        private const int VK_NUMPAD8 = (int)0x68;
        private const int VK_NUMPAD9 = (int)0x69;
        private const int VK_MULTIPLY = (int)0x6A;
        private const int VK_ADD = (int)0x6B;
        private const int VK_SEPARATOR = (int)0x6C;
        private const int VK_SUBTRACT = (int)0x6D;
        private const int VK_DECIMAL = (int)0x6E;
        private const int VK_DIVIDE = (int)0x6F;
        private const int VK_F1 = (int)0x70;
        private const int VK_F2 = (int)0x71;
        private const int VK_F3 = (int)0x72;
        private const int VK_F4 = (int)0x73;
        private const int VK_F5 = (int)0x74;
        private const int VK_F6 = (int)0x75;
        private const int VK_F7 = (int)0x76;
        private const int VK_F8 = (int)0x77;
        private const int VK_F9 = (int)0x78;
        private const int VK_F10 = (int)0x79;
        private const int VK_F11 = (int)0x7A;
        private const int VK_F12 = (int)0x7B;
        private const int VK_F13 = (int)0x7C;
        private const int VK_F14 = (int)0x7D;
        private const int VK_F15 = (int)0x7E;
        private const int VK_F16 = (int)0x7F;
        private const int VK_F17 = (int)0x80;
        private const int VK_F18 = (int)0x81;
        private const int VK_F19 = (int)0x82;
        private const int VK_F20 = (int)0x83;
        private const int VK_F21 = (int)0x84;
        private const int VK_F22 = (int)0x85;
        private const int VK_F23 = (int)0x86;
        private const int VK_F24 = (int)0x87;
        private const int VK_NUMLOCK = (int)0x90;
        private const int VK_SCROLL = (int)0x91;
        private const int VK_LeftShift = (int)0xA0;
        private const int VK_RightShift = (int)0xA1;
        private const int VK_LeftControl = (int)0xA2;
        private const int VK_RightControl = (int)0xA3;
        private const int VK_LMENU = (int)0xA4;
        private const int VK_RMENU = (int)0xA5;
        private const int VK_BROWSER_BACK = (int)0xA6;
        private const int VK_BROWSER_FORWARD = (int)0xA7;
        private const int VK_BROWSER_REFRESH = (int)0xA8;
        private const int VK_BROWSER_STOP = (int)0xA9;
        private const int VK_BROWSER_SEARCH = (int)0xAA;
        private const int VK_BROWSER_FAVORITES = (int)0xAB;
        private const int VK_BROWSER_HOME = (int)0xAC;
        private const int VK_VOLUME_MUTE = (int)0xAD;
        private const int VK_VOLUME_DOWN = (int)0xAE;
        private const int VK_VOLUME_UP = (int)0xAF;
        private const int VK_MEDIA_NEXT_TRACK = (int)0xB0;
        private const int VK_MEDIA_PREV_TRACK = (int)0xB1;
        private const int VK_MEDIA_STOP = (int)0xB2;
        private const int VK_MEDIA_PLAY_PAUSE = (int)0xB3;
        private const int VK_LAUNCH_MAIL = (int)0xB4;
        private const int VK_LAUNCH_MEDIA_SELECT = (int)0xB5;
        private const int VK_LAUNCH_APP1 = (int)0xB6;
        private const int VK_LAUNCH_APP2 = (int)0xB7;
        private const int VK_OEM_1 = (int)0xBA;
        private const int VK_OEM_PLUS = (int)0xBB;
        private const int VK_OEM_COMMA = (int)0xBC;
        private const int VK_OEM_MINUS = (int)0xBD;
        private const int VK_OEM_PERIOD = (int)0xBE;
        private const int VK_OEM_2 = (int)0xBF;
        private const int VK_OEM_3 = (int)0xC0;
        private const int VK_OEM_4 = (int)0xDB;
        private const int VK_OEM_5 = (int)0xDC;
        private const int VK_OEM_6 = (int)0xDD;
        private const int VK_OEM_7 = (int)0xDE;
        private const int VK_OEM_8 = (int)0xDF;
        private const int VK_OEM_102 = (int)0xE2;
        private const int VK_PROCESSKEY = (int)0xE5;
        private const int VK_PACKET = (int)0xE7;
        private const int VK_ATTN = (int)0xF6;
        private const int VK_CRSEL = (int)0xF7;
        private const int VK_EXSEL = (int)0xF8;
        private const int VK_EREOF = (int)0xF9;
        private const int VK_PLAY = (int)0xFA;
        private const int VK_ZOOM = (int)0xFB;
        private const int VK_NONAME = (int)0xFC;
        private const int VK_PA1 = (int)0xFD;
        private const int VK_OEM_CLEAR = (int)0xFE;
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
    }
}