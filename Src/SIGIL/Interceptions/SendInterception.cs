using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Diagnostics;
using System.Threading.Tasks;
using Valuechanges;

namespace Interceptions
{
    public class Valuechanges
    {
        public bool[] _valuechange = { false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false };
        public bool[] _ValueChange = { false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false };
        public bool this[int index]
        {
            get { return _ValueChange[index]; }
            set
            {
                if (_valuechange[index] != value)
                    _ValueChange[index] = true;
                else
                    _ValueChange[index] = false;
                _valuechange[index] = value;
            }
        }
    }
    public class SendInterception
    {
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
        private Valuechanges ValueChanges = new Valuechanges();
        private Input input = new Input();
        private List<int> keyboard_ids = new List<int>(), mouse_ids = new List<int>();
        private bool keyboard_id_alreadyexist, mouse_id_alreadyexist;
        private bool formvisible;
        private Form1 form1 = new Form1();
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
        public SendInterception()
        {
            TimeBeginPeriod(1);
            NtSetTimerResolution(1, true, ref CurrentResolution);
            input.KeyboardFilterMode = KeyboardFilterMode.All;
            input.MouseFilterMode = MouseFilterMode.All;
            input.Load();
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
        public void Connect(int number = 0)
        {
        }
        public void Disconnect()
        {
            foreach (int keyboard_id in keyboard_ids)
                foreach (int mouse_id in mouse_ids)
                    Set(keyboard_id, mouse_id, 0, 0, 0, 0, 0, 0, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false);
            input.Unload();
        }
        public void Set(int keyboard_id, int mouse_id, double MouseDesktopX, double MouseDesktopY, double deltaX, double deltaY, double x, double y, bool SendLeftClick, bool SendRightClick, bool SendMiddleClick, bool SendWheelUp, bool SendWheelDown, bool SendCANCEL, bool SendBACK, bool SendTAB, bool SendCLEAR, bool SendRETURN, bool SendSHIFT, bool SendCONTROL, bool SendMENU, bool SendCAPITAL, bool SendESCAPE, bool SendSPACE, bool SendPRIOR, bool SendNEXT, bool SendEND, bool SendHOME, bool SendLEFT, bool SendUP, bool SendRIGHT, bool SendDOWN, bool SendSNAPSHOT, bool SendINSERT, bool SendNUMPADDEL, bool SendNUMPADINSERT, bool SendHELP, bool SendAPOSTROPHE, bool SendBACKSPACE, bool SendPAGEDOWN, bool SendPAGEUP, bool SendFIN, bool SendMOUSE, bool SendA, bool SendB, bool SendC, bool SendD, bool SendE, bool SendF, bool SendG, bool SendH, bool SendI, bool SendJ, bool SendK, bool SendL, bool SendM, bool SendN, bool SendO, bool SendP, bool SendQ, bool SendR, bool SendS, bool SendT, bool SendU, bool SendV, bool SendW, bool SendX, bool SendY, bool SendZ, bool SendLWIN, bool SendRWIN, bool SendAPPS, bool SendDELETE, bool SendNUMPAD0, bool SendNUMPAD1, bool SendNUMPAD2, bool SendNUMPAD3, bool SendNUMPAD4, bool SendNUMPAD5, bool SendNUMPAD6, bool SendNUMPAD7, bool SendNUMPAD8, bool SendNUMPAD9, bool SendMULTIPLY, bool SendADD, bool SendSUBTRACT, bool SendDECIMAL, bool SendPRINTSCREEN, bool SendDIVIDE, bool SendF1, bool SendF2, bool SendF3, bool SendF4, bool SendF5, bool SendF6, bool SendF7, bool SendF8, bool SendF9, bool SendF10, bool SendF11, bool SendF12, bool SendNUMLOCK, bool SendSCROLLLOCK, bool SendLEFTSHIFT, bool SendRIGHTSHIFT, bool SendLEFTCONTROL, bool SendRIGHTCONTROL, bool SendLEFTALT, bool SendRIGHTALT, bool SendBROWSER_BACK, bool SendBROWSER_FORWARD, bool SendBROWSER_REFRESH, bool SendBROWSER_STOP, bool SendBROWSER_SEARCH, bool SendBROWSER_FAVORITES, bool SendBROWSER_HOME, bool SendVOLUME_MUTE, bool SendVOLUME_DOWN, bool SendVOLUME_UP, bool SendMEDIA_NEXT_TRACK, bool SendMEDIA_PREV_TRACK, bool SendMEDIA_STOP, bool SendMEDIA_PLAY_PAUSE, bool SendLAUNCH_MAIL, bool SendLAUNCH_MEDIA_SELECT, bool SendLAUNCH_APP1, bool SendLAUNCH_APP2, bool SendOEM_1, bool SendOEM_PLUS, bool SendOEM_COMMA, bool SendOEM_MINUS, bool SendOEM_PERIOD, bool SendOEM_2, bool SendOEM_3, bool SendOEM_4, bool SendOEM_5, bool SendOEM_6, bool SendOEM_7, bool SendOEM_8, bool SendOEM_102, bool SendEREOF, bool SendZOOM, bool SendEscape, bool SendOne, bool SendTwo, bool SendThree, bool SendFour, bool SendFive, bool SendSix, bool SendSeven, bool SendEight, bool SendNine, bool SendZero, bool SendDashUnderscore, bool SendPlusEquals, bool SendBackspace, bool SendTab, bool SendOpenBracketBrace, bool SendCloseBracketBrace, bool SendEnter, bool SendControl, bool SendSemicolonColon, bool SendSingleDoubleQuote, bool SendTilde, bool SendLeftShift, bool SendBackslashPipe, bool SendCommaLeftArrow, bool SendPeriodRightArrow, bool SendForwardSlashQuestionMark, bool SendRightShift, bool SendRightAlt, bool SendSpace, bool SendCapsLock, bool SendUp, bool SendDown, bool SendRight, bool SendLeft, bool SendHome, bool SendEnd, bool SendDelete, bool SendPageUp, bool SendPageDown, bool SendInsert, bool SendPrintScreen, bool SendNumLock, bool SendScrollLock, bool SendMenu, bool SendWindowsKey, bool SendNumpadDivide, bool SendNumpadAsterisk, bool SendNumpad7, bool SendNumpad8, bool SendNumpad9, bool SendNumpad4, bool SendNumpad5, bool SendNumpad6, bool SendNumpad1, bool SendNumpad2, bool SendNumpad3, bool SendNumpad0, bool SendNumpadDelete, bool SendNumpadEnter, bool SendNumpadPlus, bool SendNumpadMinus)
        {
            keyboard_id_alreadyexist = keyboard_ids.Contains(keyboard_id);
            if (!keyboard_id_alreadyexist)
                keyboard_ids.Add(keyboard_id);
            mouse_id_alreadyexist = mouse_ids.Contains(mouse_id);
            if (!mouse_id_alreadyexist)
                mouse_ids.Add(mouse_id);
            if (deltaX != 0f | deltaY != 0f)
                MoveMouseBy(input, (int)deltaX, (int)deltaY, mouse_id);
            if (x != 0f | y != 0f)
                MoveMouseTo(input, (int)x, (int)y, mouse_id);
            if (MouseDesktopX != 0f | MouseDesktopY != 0f)
            {
                System.Windows.Forms.Cursor.Position = new System.Drawing.Point((int)MouseDesktopX, (int)MouseDesktopY);
                SetPhysicalCursorPos((int)MouseDesktopX, (int)MouseDesktopY);
                SetCaretPos((int)MouseDesktopX, (int)MouseDesktopY);
                SetCursorPos((int)MouseDesktopX, (int)MouseDesktopY);
                Microsoft.Xna.Framework.Input.Mouse.SetPosition((int)MouseDesktopX, (int)MouseDesktopY);
            }
            ValueChanges[1] = SendLeftClick;
            if (ValueChanges._ValueChange[1] & ValueChanges._valuechange[1])
                mouseclickleft(input, mouse_id);
            if (ValueChanges._ValueChange[1] & !ValueChanges._valuechange[1])
                mouseclickleftF(input, mouse_id);
            ValueChanges[2] = SendRightClick;
            if (ValueChanges._ValueChange[2] & ValueChanges._valuechange[2])
                mouseclickright(input, mouse_id);
            if (ValueChanges._ValueChange[2] & !ValueChanges._valuechange[2])
                mouseclickrightF(input, mouse_id);
            ValueChanges[3] = SendMiddleClick;
            if (ValueChanges._ValueChange[3] & ValueChanges._valuechange[3])
                mouseclickmiddle(input, mouse_id);
            if (ValueChanges._ValueChange[3] & !ValueChanges._valuechange[3])
                mouseclickmiddleF(input, mouse_id);
            ValueChanges[4] = SendWheelUp;
            if (ValueChanges._ValueChange[4] & ValueChanges._valuechange[4])
                mousewheelup(input, mouse_id);
            ValueChanges[5] = SendWheelDown;
            if (ValueChanges._ValueChange[5] & ValueChanges._valuechange[5])
                mousewheeldown(input, mouse_id);
            ValueChanges[6] = SendCANCEL;
            if (ValueChanges._ValueChange[6] & ValueChanges._valuechange[6])
                keyboardkey(input, Keys.CANCEL, keyboard_id);
            if (ValueChanges._ValueChange[6] & !ValueChanges._valuechange[6])
                keyboardkeyF(input, Keys.CANCEL, keyboard_id);
            ValueChanges[7] = SendBACK;
            if (ValueChanges._ValueChange[7] & ValueChanges._valuechange[7])
                keyboardkey(input, Keys.BACK, keyboard_id);
            if (ValueChanges._ValueChange[7] & !ValueChanges._valuechange[7])
                keyboardkeyF(input, Keys.BACK, keyboard_id);
            ValueChanges[8] = SendTAB;
            if (ValueChanges._ValueChange[8] & ValueChanges._valuechange[8])
                keyboardkey(input, Keys.TAB, keyboard_id);
            if (ValueChanges._ValueChange[8] & !ValueChanges._valuechange[8])
                keyboardkeyF(input, Keys.TAB, keyboard_id);
            ValueChanges[9] = SendCLEAR;
            if (ValueChanges._ValueChange[9] & ValueChanges._valuechange[9])
                keyboardkey(input, Keys.CLEAR, keyboard_id);
            if (ValueChanges._ValueChange[9] & !ValueChanges._valuechange[9])
                keyboardkeyF(input, Keys.CLEAR, keyboard_id);
            ValueChanges[10] = SendRETURN;
            if (ValueChanges._ValueChange[10] & ValueChanges._valuechange[10])
                keyboardkey(input, Keys.RETURN, keyboard_id);
            if (ValueChanges._ValueChange[10] & !ValueChanges._valuechange[10])
                keyboardkeyF(input, Keys.RETURN, keyboard_id);
            ValueChanges[11] = SendSHIFT;
            if (ValueChanges._ValueChange[11] & ValueChanges._valuechange[11])
                keyboardkey(input, Keys.SHIFT, keyboard_id);
            if (ValueChanges._ValueChange[11] & !ValueChanges._valuechange[11])
                keyboardkeyF(input, Keys.SHIFT, keyboard_id);
            ValueChanges[12] = SendCONTROL;
            if (ValueChanges._ValueChange[12] & ValueChanges._valuechange[12])
                keyboardkey(input, Keys.CONTROL, keyboard_id);
            if (ValueChanges._ValueChange[12] & !ValueChanges._valuechange[12])
                keyboardkeyF(input, Keys.CONTROL, keyboard_id);
            ValueChanges[13] = SendMENU;
            if (ValueChanges._ValueChange[13] & ValueChanges._valuechange[13])
                keyboardkey(input, Keys.MENU, keyboard_id);
            if (ValueChanges._ValueChange[13] & !ValueChanges._valuechange[13])
                keyboardkeyF(input, Keys.MENU, keyboard_id);
            ValueChanges[14] = SendCAPITAL;
            if (ValueChanges._ValueChange[14] & ValueChanges._valuechange[14])
                keyboardkey(input, Keys.CAPITAL, keyboard_id);
            if (ValueChanges._ValueChange[14] & !ValueChanges._valuechange[14])
                keyboardkeyF(input, Keys.CAPITAL, keyboard_id);
            ValueChanges[15] = SendESCAPE;
            if (ValueChanges._ValueChange[15] & ValueChanges._valuechange[15])
                keyboardkey(input, Keys.ESCAPE, keyboard_id);
            if (ValueChanges._ValueChange[15] & !ValueChanges._valuechange[15])
                keyboardkeyF(input, Keys.ESCAPE, keyboard_id);
            ValueChanges[16] = SendSPACE;
            if (ValueChanges._ValueChange[16] & ValueChanges._valuechange[16])
                keyboardkey(input, Keys.SPACE, keyboard_id);
            if (ValueChanges._ValueChange[16] & !ValueChanges._valuechange[16])
                keyboardkeyF(input, Keys.SPACE, keyboard_id);
            ValueChanges[17] = SendPRIOR;
            if (ValueChanges._ValueChange[17] & ValueChanges._valuechange[17])
                keyboardkey(input, Keys.PRIOR, keyboard_id);
            if (ValueChanges._ValueChange[17] & !ValueChanges._valuechange[17])
                keyboardkeyF(input, Keys.PRIOR, keyboard_id);
            ValueChanges[18] = SendNEXT;
            if (ValueChanges._ValueChange[18] & ValueChanges._valuechange[18])
                keyboardkey(input, Keys.NEXT, keyboard_id);
            if (ValueChanges._ValueChange[18] & !ValueChanges._valuechange[18])
                keyboardkeyF(input, Keys.NEXT, keyboard_id);
            ValueChanges[19] = SendEND;
            if (ValueChanges._ValueChange[19] & ValueChanges._valuechange[19])
                keyboardkey(input, Keys.END, keyboard_id);
            if (ValueChanges._ValueChange[19] & !ValueChanges._valuechange[19])
                keyboardkeyF(input, Keys.END, keyboard_id);
            ValueChanges[20] = SendHOME;
            if (ValueChanges._ValueChange[20] & ValueChanges._valuechange[20])
                keyboardkey(input, Keys.HOME, keyboard_id);
            if (ValueChanges._ValueChange[20] & !ValueChanges._valuechange[20])
                keyboardkeyF(input, Keys.HOME, keyboard_id);
            ValueChanges[21] = SendLEFT;
            if (ValueChanges._ValueChange[21] & ValueChanges._valuechange[21])
                keyboardkey(input, Keys.LEFT, keyboard_id);
            if (ValueChanges._ValueChange[21] & !ValueChanges._valuechange[21])
                keyboardkeyF(input, Keys.LEFT, keyboard_id);
            ValueChanges[22] = SendUP;
            if (ValueChanges._ValueChange[22] & ValueChanges._valuechange[22])
                keyboardkey(input, Keys.UP, keyboard_id);
            if (ValueChanges._ValueChange[22] & !ValueChanges._valuechange[22])
                keyboardkeyF(input, Keys.UP, keyboard_id);
            ValueChanges[23] = SendRIGHT;
            if (ValueChanges._ValueChange[23] & ValueChanges._valuechange[23])
                keyboardkey(input, Keys.RIGHT, keyboard_id);
            if (ValueChanges._ValueChange[23] & !ValueChanges._valuechange[23])
                keyboardkeyF(input, Keys.RIGHT, keyboard_id);
            ValueChanges[24] = SendDOWN;
            if (ValueChanges._ValueChange[24] & ValueChanges._valuechange[24])
                keyboardkey(input, Keys.DOWN, keyboard_id);
            if (ValueChanges._ValueChange[24] & !ValueChanges._valuechange[24])
                keyboardkeyF(input, Keys.DOWN, keyboard_id);
            ValueChanges[25] = SendSNAPSHOT;
            if (ValueChanges._ValueChange[25] & ValueChanges._valuechange[25])
                keyboardkey(input, Keys.SNAPSHOT, keyboard_id);
            if (ValueChanges._ValueChange[25] & !ValueChanges._valuechange[25])
                keyboardkeyF(input, Keys.SNAPSHOT, keyboard_id);
            ValueChanges[26] = SendINSERT;
            if (ValueChanges._ValueChange[26] & ValueChanges._valuechange[26])
                keyboardkey(input, Keys.INSERT, keyboard_id);
            if (ValueChanges._ValueChange[26] & !ValueChanges._valuechange[26])
                keyboardkeyF(input, Keys.INSERT, keyboard_id);
            ValueChanges[27] = SendNUMPADDEL;
            if (ValueChanges._ValueChange[27] & ValueChanges._valuechange[27])
                keyboardkey(input, Keys.NUMPADDEL, keyboard_id);
            if (ValueChanges._ValueChange[27] & !ValueChanges._valuechange[27])
                keyboardkeyF(input, Keys.NUMPADDEL, keyboard_id);
            ValueChanges[28] = SendNUMPADINSERT;
            if (ValueChanges._ValueChange[28] & ValueChanges._valuechange[28])
                keyboardkey(input, Keys.NUMPADINSERT, keyboard_id);
            if (ValueChanges._ValueChange[28] & !ValueChanges._valuechange[28])
                keyboardkeyF(input, Keys.NUMPADINSERT, keyboard_id);
            ValueChanges[29] = SendHELP;
            if (ValueChanges._ValueChange[29] & ValueChanges._valuechange[29])
                keyboardkey(input, Keys.HELP, keyboard_id);
            if (ValueChanges._ValueChange[29] & !ValueChanges._valuechange[29])
                keyboardkeyF(input, Keys.HELP, keyboard_id);
            ValueChanges[30] = SendAPOSTROPHE;
            if (ValueChanges._ValueChange[30] & ValueChanges._valuechange[30])
                keyboardkey(input, Keys.APOSTROPHE, keyboard_id);
            if (ValueChanges._ValueChange[30] & !ValueChanges._valuechange[30])
                keyboardkeyF(input, Keys.APOSTROPHE, keyboard_id);
            ValueChanges[31] = SendBACKSPACE;
            if (ValueChanges._ValueChange[31] & ValueChanges._valuechange[31])
                keyboardkey(input, Keys.BACKSPACE, keyboard_id);
            if (ValueChanges._ValueChange[31] & !ValueChanges._valuechange[31])
                keyboardkeyF(input, Keys.BACKSPACE, keyboard_id);
            ValueChanges[32] = SendPAGEDOWN;
            if (ValueChanges._ValueChange[32] & ValueChanges._valuechange[32])
                keyboardkey(input, Keys.PAGEDOWN, keyboard_id);
            if (ValueChanges._ValueChange[32] & !ValueChanges._valuechange[32])
                keyboardkeyF(input, Keys.PAGEDOWN, keyboard_id);
            ValueChanges[33] = SendPAGEUP;
            if (ValueChanges._ValueChange[33] & ValueChanges._valuechange[33])
                keyboardkey(input, Keys.PAGEUP, keyboard_id);
            if (ValueChanges._ValueChange[33] & !ValueChanges._valuechange[33])
                keyboardkeyF(input, Keys.PAGEUP, keyboard_id);
            ValueChanges[34] = SendFIN;
            if (ValueChanges._ValueChange[34] & ValueChanges._valuechange[34])
                keyboardkey(input, Keys.FIN, keyboard_id);
            if (ValueChanges._ValueChange[34] & !ValueChanges._valuechange[34])
                keyboardkeyF(input, Keys.FIN, keyboard_id);
            ValueChanges[35] = SendMOUSE;
            if (ValueChanges._ValueChange[35] & ValueChanges._valuechange[35])
                keyboardkey(input, Keys.MOUSE, keyboard_id);
            if (ValueChanges._ValueChange[35] & !ValueChanges._valuechange[35])
                keyboardkeyF(input, Keys.MOUSE, keyboard_id);
            ValueChanges[36] = SendA;
            if (ValueChanges._ValueChange[36] & ValueChanges._valuechange[36])
                keyboardkey(input, Keys.A, keyboard_id);
            if (ValueChanges._ValueChange[36] & !ValueChanges._valuechange[36])
                keyboardkeyF(input, Keys.A, keyboard_id);
            ValueChanges[37] = SendB;
            if (ValueChanges._ValueChange[37] & ValueChanges._valuechange[37])
                keyboardkey(input, Keys.B, keyboard_id);
            if (ValueChanges._ValueChange[37] & !ValueChanges._valuechange[37])
                keyboardkeyF(input, Keys.B, keyboard_id);
            ValueChanges[38] = SendC;
            if (ValueChanges._ValueChange[38] & ValueChanges._valuechange[38])
                keyboardkey(input, Keys.C, keyboard_id);
            if (ValueChanges._ValueChange[38] & !ValueChanges._valuechange[38])
                keyboardkeyF(input, Keys.C, keyboard_id);
            ValueChanges[39] = SendD;
            if (ValueChanges._ValueChange[39] & ValueChanges._valuechange[39])
                keyboardkey(input, Keys.D, keyboard_id);
            if (ValueChanges._ValueChange[39] & !ValueChanges._valuechange[39])
                keyboardkeyF(input, Keys.D, keyboard_id);
            ValueChanges[40] = SendE;
            if (ValueChanges._ValueChange[40] & ValueChanges._valuechange[40])
                keyboardkey(input, Keys.E, keyboard_id);
            if (ValueChanges._ValueChange[40] & !ValueChanges._valuechange[40])
                keyboardkeyF(input, Keys.E, keyboard_id);
            ValueChanges[41] = SendF;
            if (ValueChanges._ValueChange[41] & ValueChanges._valuechange[41])
                keyboardkey(input, Keys.F, keyboard_id);
            if (ValueChanges._ValueChange[41] & !ValueChanges._valuechange[41])
                keyboardkeyF(input, Keys.F, keyboard_id);
            ValueChanges[42] = SendG;
            if (ValueChanges._ValueChange[42] & ValueChanges._valuechange[42])
                keyboardkey(input, Keys.G, keyboard_id);
            if (ValueChanges._ValueChange[42] & !ValueChanges._valuechange[42])
                keyboardkeyF(input, Keys.G, keyboard_id);
            ValueChanges[43] = SendH;
            if (ValueChanges._ValueChange[43] & ValueChanges._valuechange[43])
                keyboardkey(input, Keys.H, keyboard_id);
            if (ValueChanges._ValueChange[43] & !ValueChanges._valuechange[43])
                keyboardkeyF(input, Keys.H, keyboard_id);
            ValueChanges[44] = SendI;
            if (ValueChanges._ValueChange[44] & ValueChanges._valuechange[44])
                keyboardkey(input, Keys.I, keyboard_id);
            if (ValueChanges._ValueChange[44] & !ValueChanges._valuechange[44])
                keyboardkeyF(input, Keys.I, keyboard_id);
            ValueChanges[45] = SendJ;
            if (ValueChanges._ValueChange[45] & ValueChanges._valuechange[45])
                keyboardkey(input, Keys.J, keyboard_id);
            if (ValueChanges._ValueChange[45] & !ValueChanges._valuechange[45])
                keyboardkeyF(input, Keys.J, keyboard_id);
            ValueChanges[46] = SendK;
            if (ValueChanges._ValueChange[46] & ValueChanges._valuechange[46])
                keyboardkey(input, Keys.K, keyboard_id);
            if (ValueChanges._ValueChange[46] & !ValueChanges._valuechange[46])
                keyboardkeyF(input, Keys.K, keyboard_id);
            ValueChanges[47] = SendL;
            if (ValueChanges._ValueChange[47] & ValueChanges._valuechange[47])
                keyboardkey(input, Keys.L, keyboard_id);
            if (ValueChanges._ValueChange[47] & !ValueChanges._valuechange[47])
                keyboardkeyF(input, Keys.L, keyboard_id);
            ValueChanges[48] = SendM;
            if (ValueChanges._ValueChange[48] & ValueChanges._valuechange[48])
                keyboardkey(input, Keys.M, keyboard_id);
            if (ValueChanges._ValueChange[48] & !ValueChanges._valuechange[48])
                keyboardkeyF(input, Keys.M, keyboard_id);
            ValueChanges[49] = SendN;
            if (ValueChanges._ValueChange[49] & ValueChanges._valuechange[49])
                keyboardkey(input, Keys.N, keyboard_id);
            if (ValueChanges._ValueChange[49] & !ValueChanges._valuechange[49])
                keyboardkeyF(input, Keys.N, keyboard_id);
            ValueChanges[50] = SendO;
            if (ValueChanges._ValueChange[50] & ValueChanges._valuechange[50])
                keyboardkey(input, Keys.O, keyboard_id);
            if (ValueChanges._ValueChange[50] & !ValueChanges._valuechange[50])
                keyboardkeyF(input, Keys.O, keyboard_id);
            ValueChanges[51] = SendP;
            if (ValueChanges._ValueChange[51] & ValueChanges._valuechange[51])
                keyboardkey(input, Keys.P, keyboard_id);
            if (ValueChanges._ValueChange[51] & !ValueChanges._valuechange[51])
                keyboardkeyF(input, Keys.P, keyboard_id);
            ValueChanges[52] = SendQ;
            if (ValueChanges._ValueChange[52] & ValueChanges._valuechange[52])
                keyboardkey(input, Keys.Q, keyboard_id);
            if (ValueChanges._ValueChange[52] & !ValueChanges._valuechange[52])
                keyboardkeyF(input, Keys.Q, keyboard_id);
            ValueChanges[53] = SendR;
            if (ValueChanges._ValueChange[53] & ValueChanges._valuechange[53])
                keyboardkey(input, Keys.R, keyboard_id);
            if (ValueChanges._ValueChange[53] & !ValueChanges._valuechange[53])
                keyboardkeyF(input, Keys.R, keyboard_id);
            ValueChanges[54] = SendS;
            if (ValueChanges._ValueChange[54] & ValueChanges._valuechange[54])
                keyboardkey(input, Keys.S, keyboard_id);
            if (ValueChanges._ValueChange[54] & !ValueChanges._valuechange[54])
                keyboardkeyF(input, Keys.S, keyboard_id);
            ValueChanges[55] = SendT;
            if (ValueChanges._ValueChange[55] & ValueChanges._valuechange[55])
                keyboardkey(input, Keys.T, keyboard_id);
            if (ValueChanges._ValueChange[55] & !ValueChanges._valuechange[55])
                keyboardkeyF(input, Keys.T, keyboard_id);
            ValueChanges[56] = SendU;
            if (ValueChanges._ValueChange[56] & ValueChanges._valuechange[56])
                keyboardkey(input, Keys.U, keyboard_id);
            if (ValueChanges._ValueChange[56] & !ValueChanges._valuechange[56])
                keyboardkeyF(input, Keys.U, keyboard_id);
            ValueChanges[57] = SendV;
            if (ValueChanges._ValueChange[57] & ValueChanges._valuechange[57])
                keyboardkey(input, Keys.V, keyboard_id);
            if (ValueChanges._ValueChange[57] & !ValueChanges._valuechange[57])
                keyboardkeyF(input, Keys.V, keyboard_id);
            ValueChanges[58] = SendW;
            if (ValueChanges._ValueChange[58] & ValueChanges._valuechange[58])
                keyboardkey(input, Keys.W, keyboard_id);
            if (ValueChanges._ValueChange[58] & !ValueChanges._valuechange[58])
                keyboardkeyF(input, Keys.W, keyboard_id);
            ValueChanges[59] = SendX;
            if (ValueChanges._ValueChange[59] & ValueChanges._valuechange[59])
                keyboardkey(input, Keys.X, keyboard_id);
            if (ValueChanges._ValueChange[59] & !ValueChanges._valuechange[59])
                keyboardkeyF(input, Keys.X, keyboard_id);
            ValueChanges[60] = SendY;
            if (ValueChanges._ValueChange[60] & ValueChanges._valuechange[60])
                keyboardkey(input, Keys.Y, keyboard_id);
            if (ValueChanges._ValueChange[60] & !ValueChanges._valuechange[60])
                keyboardkeyF(input, Keys.Y, keyboard_id);
            ValueChanges[61] = SendZ;
            if (ValueChanges._ValueChange[61] & ValueChanges._valuechange[61])
                keyboardkey(input, Keys.Z, keyboard_id);
            if (ValueChanges._ValueChange[61] & !ValueChanges._valuechange[61])
                keyboardkeyF(input, Keys.Z, keyboard_id);
            ValueChanges[62] = SendLWIN;
            if (ValueChanges._ValueChange[62] & ValueChanges._valuechange[62])
                keyboardkey(input, Keys.LWIN, keyboard_id);
            if (ValueChanges._ValueChange[62] & !ValueChanges._valuechange[62])
                keyboardkeyF(input, Keys.LWIN, keyboard_id);
            ValueChanges[63] = SendRWIN;
            if (ValueChanges._ValueChange[63] & ValueChanges._valuechange[63])
                keyboardkey(input, Keys.RWIN, keyboard_id);
            if (ValueChanges._ValueChange[63] & !ValueChanges._valuechange[63])
                keyboardkeyF(input, Keys.RWIN, keyboard_id);
            ValueChanges[64] = SendAPPS;
            if (ValueChanges._ValueChange[64] & ValueChanges._valuechange[64])
                keyboardkey(input, Keys.APPS, keyboard_id);
            if (ValueChanges._ValueChange[64] & !ValueChanges._valuechange[64])
                keyboardkeyF(input, Keys.APPS, keyboard_id);
            ValueChanges[65] = SendDELETE;
            if (ValueChanges._ValueChange[65] & ValueChanges._valuechange[65])
                keyboardkey(input, Keys.DELETE, keyboard_id);
            if (ValueChanges._ValueChange[65] & !ValueChanges._valuechange[65])
                keyboardkeyF(input, Keys.DELETE, keyboard_id);
            ValueChanges[66] = SendNUMPAD0;
            if (ValueChanges._ValueChange[66] & ValueChanges._valuechange[66])
                keyboardkey(input, Keys.NUMPAD0, keyboard_id);
            if (ValueChanges._ValueChange[66] & !ValueChanges._valuechange[66])
                keyboardkeyF(input, Keys.NUMPAD0, keyboard_id);
            ValueChanges[67] = SendNUMPAD1;
            if (ValueChanges._ValueChange[67] & ValueChanges._valuechange[67])
                keyboardkey(input, Keys.NUMPAD1, keyboard_id);
            if (ValueChanges._ValueChange[67] & !ValueChanges._valuechange[67])
                keyboardkeyF(input, Keys.NUMPAD1, keyboard_id);
            ValueChanges[68] = SendNUMPAD2;
            if (ValueChanges._ValueChange[68] & ValueChanges._valuechange[68])
                keyboardkey(input, Keys.NUMPAD2, keyboard_id);
            if (ValueChanges._ValueChange[68] & !ValueChanges._valuechange[68])
                keyboardkeyF(input, Keys.NUMPAD2, keyboard_id);
            ValueChanges[69] = SendNUMPAD3;
            if (ValueChanges._ValueChange[69] & ValueChanges._valuechange[69])
                keyboardkey(input, Keys.NUMPAD3, keyboard_id);
            if (ValueChanges._ValueChange[69] & !ValueChanges._valuechange[69])
                keyboardkeyF(input, Keys.NUMPAD3, keyboard_id);
            ValueChanges[70] = SendNUMPAD4;
            if (ValueChanges._ValueChange[70] & ValueChanges._valuechange[70])
                keyboardkey(input, Keys.NUMPAD4, keyboard_id);
            if (ValueChanges._ValueChange[70] & !ValueChanges._valuechange[70])
                keyboardkeyF(input, Keys.NUMPAD4, keyboard_id);
            ValueChanges[71] = SendNUMPAD5;
            if (ValueChanges._ValueChange[71] & ValueChanges._valuechange[71])
                keyboardkey(input, Keys.NUMPAD5, keyboard_id);
            if (ValueChanges._ValueChange[71] & !ValueChanges._valuechange[71])
                keyboardkeyF(input, Keys.NUMPAD5, keyboard_id);
            ValueChanges[72] = SendNUMPAD6;
            if (ValueChanges._ValueChange[72] & ValueChanges._valuechange[72])
                keyboardkey(input, Keys.NUMPAD6, keyboard_id);
            if (ValueChanges._ValueChange[72] & !ValueChanges._valuechange[72])
                keyboardkeyF(input, Keys.NUMPAD6, keyboard_id);
            ValueChanges[73] = SendNUMPAD7;
            if (ValueChanges._ValueChange[73] & ValueChanges._valuechange[73])
                keyboardkey(input, Keys.NUMPAD7, keyboard_id);
            if (ValueChanges._ValueChange[73] & !ValueChanges._valuechange[73])
                keyboardkeyF(input, Keys.NUMPAD7, keyboard_id);
            ValueChanges[74] = SendNUMPAD8;
            if (ValueChanges._ValueChange[74] & ValueChanges._valuechange[74])
                keyboardkey(input, Keys.NUMPAD8, keyboard_id);
            if (ValueChanges._ValueChange[74] & !ValueChanges._valuechange[74])
                keyboardkeyF(input, Keys.NUMPAD8, keyboard_id);
            ValueChanges[75] = SendNUMPAD9;
            if (ValueChanges._ValueChange[75] & ValueChanges._valuechange[75])
                keyboardkey(input, Keys.NUMPAD9, keyboard_id);
            if (ValueChanges._ValueChange[75] & !ValueChanges._valuechange[75])
                keyboardkeyF(input, Keys.NUMPAD9, keyboard_id);
            ValueChanges[76] = SendMULTIPLY;
            if (ValueChanges._ValueChange[76] & ValueChanges._valuechange[76])
                keyboardkey(input, Keys.MULTIPLY, keyboard_id);
            if (ValueChanges._ValueChange[76] & !ValueChanges._valuechange[76])
                keyboardkeyF(input, Keys.MULTIPLY, keyboard_id);
            ValueChanges[77] = SendADD;
            if (ValueChanges._ValueChange[77] & ValueChanges._valuechange[77])
                keyboardkey(input, Keys.ADD, keyboard_id);
            if (ValueChanges._ValueChange[77] & !ValueChanges._valuechange[77])
                keyboardkeyF(input, Keys.ADD, keyboard_id);
            ValueChanges[78] = SendSUBTRACT;
            if (ValueChanges._ValueChange[78] & ValueChanges._valuechange[78])
                keyboardkey(input, Keys.SUBTRACT, keyboard_id);
            if (ValueChanges._ValueChange[78] & !ValueChanges._valuechange[78])
                keyboardkeyF(input, Keys.SUBTRACT, keyboard_id);
            ValueChanges[79] = SendDECIMAL;
            if (ValueChanges._ValueChange[79] & ValueChanges._valuechange[79])
                keyboardkey(input, Keys.DECIMAL, keyboard_id);
            if (ValueChanges._ValueChange[79] & !ValueChanges._valuechange[79])
                keyboardkeyF(input, Keys.DECIMAL, keyboard_id);
            ValueChanges[80] = SendPRINTSCREEN;
            if (ValueChanges._ValueChange[80] & ValueChanges._valuechange[80])
                keyboardkey(input, Keys.PRINTSCREEN, keyboard_id);
            if (ValueChanges._ValueChange[80] & !ValueChanges._valuechange[80])
                keyboardkeyF(input, Keys.PRINTSCREEN, keyboard_id);
            ValueChanges[81] = SendDIVIDE;
            if (ValueChanges._ValueChange[81] & ValueChanges._valuechange[81])
                keyboardkey(input, Keys.DIVIDE, keyboard_id);
            if (ValueChanges._ValueChange[81] & !ValueChanges._valuechange[81])
                keyboardkeyF(input, Keys.DIVIDE, keyboard_id);
            ValueChanges[82] = SendF1;
            if (ValueChanges._ValueChange[82] & ValueChanges._valuechange[82])
                keyboardkey(input, Keys.F1, keyboard_id);
            if (ValueChanges._ValueChange[82] & !ValueChanges._valuechange[82])
                keyboardkeyF(input, Keys.F1, keyboard_id);
            ValueChanges[83] = SendF2;
            if (ValueChanges._ValueChange[83] & ValueChanges._valuechange[83])
                keyboardkey(input, Keys.F2, keyboard_id);
            if (ValueChanges._ValueChange[83] & !ValueChanges._valuechange[83])
                keyboardkeyF(input, Keys.F2, keyboard_id);
            ValueChanges[84] = SendF3;
            if (ValueChanges._ValueChange[84] & ValueChanges._valuechange[84])
                keyboardkey(input, Keys.F3, keyboard_id);
            if (ValueChanges._ValueChange[84] & !ValueChanges._valuechange[84])
                keyboardkeyF(input, Keys.F3, keyboard_id);
            ValueChanges[85] = SendF4;
            if (ValueChanges._ValueChange[85] & ValueChanges._valuechange[85])
                keyboardkey(input, Keys.F4, keyboard_id);
            if (ValueChanges._ValueChange[85] & !ValueChanges._valuechange[85])
                keyboardkeyF(input, Keys.F4, keyboard_id);
            ValueChanges[86] = SendF5;
            if (ValueChanges._ValueChange[86] & ValueChanges._valuechange[86])
                keyboardkey(input, Keys.F5, keyboard_id);
            if (ValueChanges._ValueChange[86] & !ValueChanges._valuechange[86])
                keyboardkeyF(input, Keys.F5, keyboard_id);
            ValueChanges[87] = SendF6;
            if (ValueChanges._ValueChange[87] & ValueChanges._valuechange[87])
                keyboardkey(input, Keys.F6, keyboard_id);
            if (ValueChanges._ValueChange[87] & !ValueChanges._valuechange[87])
                keyboardkeyF(input, Keys.F6, keyboard_id);
            ValueChanges[88] = SendF7;
            if (ValueChanges._ValueChange[88] & ValueChanges._valuechange[88])
                keyboardkey(input, Keys.F7, keyboard_id);
            if (ValueChanges._ValueChange[88] & !ValueChanges._valuechange[88])
                keyboardkeyF(input, Keys.F7, keyboard_id);
            ValueChanges[89] = SendF8;
            if (ValueChanges._ValueChange[89] & ValueChanges._valuechange[89])
                keyboardkey(input, Keys.F8, keyboard_id);
            if (ValueChanges._ValueChange[89] & !ValueChanges._valuechange[89])
                keyboardkeyF(input, Keys.F8, keyboard_id);
            ValueChanges[90] = SendF9;
            if (ValueChanges._ValueChange[90] & ValueChanges._valuechange[90])
                keyboardkey(input, Keys.F9, keyboard_id);
            if (ValueChanges._ValueChange[90] & !ValueChanges._valuechange[90])
                keyboardkeyF(input, Keys.F9, keyboard_id);
            ValueChanges[91] = SendF10;
            if (ValueChanges._ValueChange[91] & ValueChanges._valuechange[91])
                keyboardkey(input, Keys.F10, keyboard_id);
            if (ValueChanges._ValueChange[91] & !ValueChanges._valuechange[91])
                keyboardkeyF(input, Keys.F10, keyboard_id);
            ValueChanges[92] = SendF11;
            if (ValueChanges._ValueChange[92] & ValueChanges._valuechange[92])
                keyboardkey(input, Keys.F11, keyboard_id);
            if (ValueChanges._ValueChange[92] & !ValueChanges._valuechange[92])
                keyboardkeyF(input, Keys.F11, keyboard_id);
            ValueChanges[93] = SendF12;
            if (ValueChanges._ValueChange[93] & ValueChanges._valuechange[93])
                keyboardkey(input, Keys.F12, keyboard_id);
            if (ValueChanges._ValueChange[93] & !ValueChanges._valuechange[93])
                keyboardkeyF(input, Keys.F12, keyboard_id);
            ValueChanges[94] = SendNUMLOCK;
            if (ValueChanges._ValueChange[94] & ValueChanges._valuechange[94])
                keyboardkey(input, Keys.NUMLOCK, keyboard_id);
            if (ValueChanges._ValueChange[94] & !ValueChanges._valuechange[94])
                keyboardkeyF(input, Keys.NUMLOCK, keyboard_id);
            ValueChanges[95] = SendSCROLLLOCK;
            if (ValueChanges._ValueChange[95] & ValueChanges._valuechange[95])
                keyboardkey(input, Keys.SCROLLLOCK, keyboard_id);
            if (ValueChanges._ValueChange[95] & !ValueChanges._valuechange[95])
                keyboardkeyF(input, Keys.SCROLLLOCK, keyboard_id);
            ValueChanges[96] = SendLEFTSHIFT;
            if (ValueChanges._ValueChange[96] & ValueChanges._valuechange[96])
                keyboardkey(input, Keys.LEFTSHIFT, keyboard_id);
            if (ValueChanges._ValueChange[96] & !ValueChanges._valuechange[96])
                keyboardkeyF(input, Keys.LEFTSHIFT, keyboard_id);
            ValueChanges[97] = SendRIGHTSHIFT;
            if (ValueChanges._ValueChange[97] & ValueChanges._valuechange[97])
                keyboardkey(input, Keys.RIGHTSHIFT, keyboard_id);
            if (ValueChanges._ValueChange[97] & !ValueChanges._valuechange[97])
                keyboardkeyF(input, Keys.RIGHTSHIFT, keyboard_id);
            ValueChanges[98] = SendLEFTCONTROL;
            if (ValueChanges._ValueChange[98] & ValueChanges._valuechange[98])
                keyboardkey(input, Keys.LEFTCONTROL, keyboard_id);
            if (ValueChanges._ValueChange[98] & !ValueChanges._valuechange[98])
                keyboardkeyF(input, Keys.LEFTCONTROL, keyboard_id);
            ValueChanges[99] = SendRIGHTCONTROL;
            if (ValueChanges._ValueChange[99] & ValueChanges._valuechange[99])
                keyboardkey(input, Keys.RIGHTCONTROL, keyboard_id);
            if (ValueChanges._ValueChange[99] & !ValueChanges._valuechange[99])
                keyboardkeyF(input, Keys.RIGHTCONTROL, keyboard_id);
            ValueChanges[100] = SendLEFTALT;
            if (ValueChanges._ValueChange[100] & ValueChanges._valuechange[100])
                keyboardkey(input, Keys.LEFTALT, keyboard_id);
            if (ValueChanges._ValueChange[100] & !ValueChanges._valuechange[100])
                keyboardkeyF(input, Keys.LEFTALT, keyboard_id);
            ValueChanges[101] = SendRIGHTALT;
            if (ValueChanges._ValueChange[101] & ValueChanges._valuechange[101])
                keyboardkey(input, Keys.RIGHTALT, keyboard_id);
            if (ValueChanges._ValueChange[101] & !ValueChanges._valuechange[101])
                keyboardkeyF(input, Keys.RIGHTALT, keyboard_id);
            ValueChanges[102] = SendBROWSER_BACK;
            if (ValueChanges._ValueChange[102] & ValueChanges._valuechange[102])
                keyboardkey(input, Keys.BROWSER_BACK, keyboard_id);
            if (ValueChanges._ValueChange[102] & !ValueChanges._valuechange[102])
                keyboardkeyF(input, Keys.BROWSER_BACK, keyboard_id);
            ValueChanges[103] = SendBROWSER_FORWARD;
            if (ValueChanges._ValueChange[103] & ValueChanges._valuechange[103])
                keyboardkey(input, Keys.BROWSER_FORWARD, keyboard_id);
            if (ValueChanges._ValueChange[103] & !ValueChanges._valuechange[103])
                keyboardkeyF(input, Keys.BROWSER_FORWARD, keyboard_id);
            ValueChanges[104] = SendBROWSER_REFRESH;
            if (ValueChanges._ValueChange[104] & ValueChanges._valuechange[104])
                keyboardkey(input, Keys.BROWSER_REFRESH, keyboard_id);
            if (ValueChanges._ValueChange[104] & !ValueChanges._valuechange[104])
                keyboardkeyF(input, Keys.BROWSER_REFRESH, keyboard_id);
            ValueChanges[105] = SendBROWSER_STOP;
            if (ValueChanges._ValueChange[105] & ValueChanges._valuechange[105])
                keyboardkey(input, Keys.BROWSER_STOP, keyboard_id);
            if (ValueChanges._ValueChange[105] & !ValueChanges._valuechange[105])
                keyboardkeyF(input, Keys.BROWSER_STOP, keyboard_id);
            ValueChanges[106] = SendBROWSER_SEARCH;
            if (ValueChanges._ValueChange[106] & ValueChanges._valuechange[106])
                keyboardkey(input, Keys.BROWSER_SEARCH, keyboard_id);
            if (ValueChanges._ValueChange[106] & !ValueChanges._valuechange[106])
                keyboardkeyF(input, Keys.BROWSER_SEARCH, keyboard_id);
            ValueChanges[107] = SendBROWSER_FAVORITES;
            if (ValueChanges._ValueChange[107] & ValueChanges._valuechange[107])
                keyboardkey(input, Keys.BROWSER_FAVORITES, keyboard_id);
            if (ValueChanges._ValueChange[107] & !ValueChanges._valuechange[107])
                keyboardkeyF(input, Keys.BROWSER_FAVORITES, keyboard_id);
            ValueChanges[108] = SendBROWSER_HOME;
            if (ValueChanges._ValueChange[108] & ValueChanges._valuechange[108])
                keyboardkey(input, Keys.BROWSER_HOME, keyboard_id);
            if (ValueChanges._ValueChange[108] & !ValueChanges._valuechange[108])
                keyboardkeyF(input, Keys.BROWSER_HOME, keyboard_id);
            ValueChanges[109] = SendVOLUME_MUTE;
            if (ValueChanges._ValueChange[109] & ValueChanges._valuechange[109])
                keyboardkey(input, Keys.VOLUME_MUTE, keyboard_id);
            if (ValueChanges._ValueChange[109] & !ValueChanges._valuechange[109])
                keyboardkeyF(input, Keys.VOLUME_MUTE, keyboard_id);
            ValueChanges[110] = SendVOLUME_DOWN;
            if (ValueChanges._ValueChange[110] & ValueChanges._valuechange[110])
                keyboardkey(input, Keys.VOLUME_DOWN, keyboard_id);
            if (ValueChanges._ValueChange[110] & !ValueChanges._valuechange[110])
                keyboardkeyF(input, Keys.VOLUME_DOWN, keyboard_id);
            ValueChanges[111] = SendVOLUME_UP;
            if (ValueChanges._ValueChange[111] & ValueChanges._valuechange[111])
                keyboardkey(input, Keys.VOLUME_UP, keyboard_id);
            if (ValueChanges._ValueChange[111] & !ValueChanges._valuechange[111])
                keyboardkeyF(input, Keys.VOLUME_UP, keyboard_id);
            ValueChanges[112] = SendMEDIA_NEXT_TRACK;
            if (ValueChanges._ValueChange[112] & ValueChanges._valuechange[112])
                keyboardkey(input, Keys.MEDIA_NEXT_TRACK, keyboard_id);
            if (ValueChanges._ValueChange[112] & !ValueChanges._valuechange[112])
                keyboardkeyF(input, Keys.MEDIA_NEXT_TRACK, keyboard_id);
            ValueChanges[113] = SendMEDIA_PREV_TRACK;
            if (ValueChanges._ValueChange[113] & ValueChanges._valuechange[113])
                keyboardkey(input, Keys.MEDIA_PREV_TRACK, keyboard_id);
            if (ValueChanges._ValueChange[113] & !ValueChanges._valuechange[113])
                keyboardkeyF(input, Keys.MEDIA_PREV_TRACK, keyboard_id);
            ValueChanges[114] = SendMEDIA_STOP;
            if (ValueChanges._ValueChange[114] & ValueChanges._valuechange[114])
                keyboardkey(input, Keys.MEDIA_STOP, keyboard_id);
            if (ValueChanges._ValueChange[114] & !ValueChanges._valuechange[114])
                keyboardkeyF(input, Keys.MEDIA_STOP, keyboard_id);
            ValueChanges[115] = SendMEDIA_PLAY_PAUSE;
            if (ValueChanges._ValueChange[115] & ValueChanges._valuechange[115])
                keyboardkey(input, Keys.MEDIA_PLAY_PAUSE, keyboard_id);
            if (ValueChanges._ValueChange[115] & !ValueChanges._valuechange[115])
                keyboardkeyF(input, Keys.MEDIA_PLAY_PAUSE, keyboard_id);
            ValueChanges[116] = SendLAUNCH_MAIL;
            if (ValueChanges._ValueChange[116] & ValueChanges._valuechange[116])
                keyboardkey(input, Keys.LAUNCH_MAIL, keyboard_id);
            if (ValueChanges._ValueChange[116] & !ValueChanges._valuechange[116])
                keyboardkeyF(input, Keys.LAUNCH_MAIL, keyboard_id);
            ValueChanges[117] = SendLAUNCH_MEDIA_SELECT;
            if (ValueChanges._ValueChange[117] & ValueChanges._valuechange[117])
                keyboardkey(input, Keys.LAUNCH_MEDIA_SELECT, keyboard_id);
            if (ValueChanges._ValueChange[117] & !ValueChanges._valuechange[117])
                keyboardkeyF(input, Keys.LAUNCH_MEDIA_SELECT, keyboard_id);
            ValueChanges[118] = SendLAUNCH_APP1;
            if (ValueChanges._ValueChange[118] & ValueChanges._valuechange[118])
                keyboardkey(input, Keys.LAUNCH_APP1, keyboard_id);
            if (ValueChanges._ValueChange[118] & !ValueChanges._valuechange[118])
                keyboardkeyF(input, Keys.LAUNCH_APP1, keyboard_id);
            ValueChanges[119] = SendLAUNCH_APP2;
            if (ValueChanges._ValueChange[119] & ValueChanges._valuechange[119])
                keyboardkey(input, Keys.LAUNCH_APP2, keyboard_id);
            if (ValueChanges._ValueChange[119] & !ValueChanges._valuechange[119])
                keyboardkeyF(input, Keys.LAUNCH_APP2, keyboard_id);
            ValueChanges[120] = SendOEM_1;
            if (ValueChanges._ValueChange[120] & ValueChanges._valuechange[120])
                keyboardkey(input, Keys.OEM_1, keyboard_id);
            if (ValueChanges._ValueChange[120] & !ValueChanges._valuechange[120])
                keyboardkeyF(input, Keys.OEM_1, keyboard_id);
            ValueChanges[121] = SendOEM_PLUS;
            if (ValueChanges._ValueChange[121] & ValueChanges._valuechange[121])
                keyboardkey(input, Keys.OEM_PLUS, keyboard_id);
            if (ValueChanges._ValueChange[121] & !ValueChanges._valuechange[121])
                keyboardkeyF(input, Keys.OEM_PLUS, keyboard_id);
            ValueChanges[122] = SendOEM_COMMA;
            if (ValueChanges._ValueChange[122] & ValueChanges._valuechange[122])
                keyboardkey(input, Keys.OEM_COMMA, keyboard_id);
            if (ValueChanges._ValueChange[122] & !ValueChanges._valuechange[122])
                keyboardkeyF(input, Keys.OEM_COMMA, keyboard_id);
            ValueChanges[123] = SendOEM_MINUS;
            if (ValueChanges._ValueChange[123] & ValueChanges._valuechange[123])
                keyboardkey(input, Keys.OEM_MINUS, keyboard_id);
            if (ValueChanges._ValueChange[123] & !ValueChanges._valuechange[123])
                keyboardkeyF(input, Keys.OEM_MINUS, keyboard_id);
            ValueChanges[124] = SendOEM_PERIOD;
            if (ValueChanges._ValueChange[124] & ValueChanges._valuechange[124])
                keyboardkey(input, Keys.OEM_PERIOD, keyboard_id);
            if (ValueChanges._ValueChange[124] & !ValueChanges._valuechange[124])
                keyboardkeyF(input, Keys.OEM_PERIOD, keyboard_id);
            ValueChanges[125] = SendOEM_2;
            if (ValueChanges._ValueChange[125] & ValueChanges._valuechange[125])
                keyboardkey(input, Keys.OEM_2, keyboard_id);
            if (ValueChanges._ValueChange[125] & !ValueChanges._valuechange[125])
                keyboardkeyF(input, Keys.OEM_2, keyboard_id);
            ValueChanges[126] = SendOEM_3;
            if (ValueChanges._ValueChange[126] & ValueChanges._valuechange[126])
                keyboardkey(input, Keys.OEM_3, keyboard_id);
            if (ValueChanges._ValueChange[126] & !ValueChanges._valuechange[126])
                keyboardkeyF(input, Keys.OEM_3, keyboard_id);
            ValueChanges[127] = SendOEM_4;
            if (ValueChanges._ValueChange[127] & ValueChanges._valuechange[127])
                keyboardkey(input, Keys.OEM_4, keyboard_id);
            if (ValueChanges._ValueChange[127] & !ValueChanges._valuechange[127])
                keyboardkeyF(input, Keys.OEM_4, keyboard_id);
            ValueChanges[128] = SendOEM_5;
            if (ValueChanges._ValueChange[128] & ValueChanges._valuechange[128])
                keyboardkey(input, Keys.OEM_5, keyboard_id);
            if (ValueChanges._ValueChange[128] & !ValueChanges._valuechange[128])
                keyboardkeyF(input, Keys.OEM_5, keyboard_id);
            ValueChanges[129] = SendOEM_6;
            if (ValueChanges._ValueChange[129] & ValueChanges._valuechange[129])
                keyboardkey(input, Keys.OEM_6, keyboard_id);
            if (ValueChanges._ValueChange[129] & !ValueChanges._valuechange[129])
                keyboardkeyF(input, Keys.OEM_6, keyboard_id);
            ValueChanges[130] = SendOEM_7;
            if (ValueChanges._ValueChange[130] & ValueChanges._valuechange[130])
                keyboardkey(input, Keys.OEM_7, keyboard_id);
            if (ValueChanges._ValueChange[130] & !ValueChanges._valuechange[130])
                keyboardkeyF(input, Keys.OEM_7, keyboard_id);
            ValueChanges[131] = SendOEM_8;
            if (ValueChanges._ValueChange[131] & ValueChanges._valuechange[131])
                keyboardkey(input, Keys.OEM_8, keyboard_id);
            if (ValueChanges._ValueChange[131] & !ValueChanges._valuechange[131])
                keyboardkeyF(input, Keys.OEM_8, keyboard_id);
            ValueChanges[132] = SendOEM_102;
            if (ValueChanges._ValueChange[132] & ValueChanges._valuechange[132])
                keyboardkey(input, Keys.OEM_102, keyboard_id);
            if (ValueChanges._ValueChange[132] & !ValueChanges._valuechange[132])
                keyboardkeyF(input, Keys.OEM_102, keyboard_id);
            ValueChanges[133] = SendEREOF;
            if (ValueChanges._ValueChange[133] & ValueChanges._valuechange[133])
                keyboardkey(input, Keys.EREOF, keyboard_id);
            if (ValueChanges._ValueChange[133] & !ValueChanges._valuechange[133])
                keyboardkeyF(input, Keys.EREOF, keyboard_id);
            ValueChanges[134] = SendZOOM;
            if (ValueChanges._ValueChange[134] & ValueChanges._valuechange[134])
                keyboardkey(input, Keys.ZOOM, keyboard_id);
            if (ValueChanges._ValueChange[134] & !ValueChanges._valuechange[134])
                keyboardkeyF(input, Keys.ZOOM, keyboard_id);
            ValueChanges[135] = SendEscape;
            if (ValueChanges._ValueChange[135] & ValueChanges._valuechange[135])
                keyboardkey(input, Keys.Escape, keyboard_id);
            if (ValueChanges._ValueChange[135] & !ValueChanges._valuechange[135])
                keyboardkeyF(input, Keys.Escape, keyboard_id);
            ValueChanges[136] = SendOne;
            if (ValueChanges._ValueChange[136] & ValueChanges._valuechange[136])
                keyboardkey(input, Keys.One, keyboard_id);
            if (ValueChanges._ValueChange[136] & !ValueChanges._valuechange[136])
                keyboardkeyF(input, Keys.One, keyboard_id);
            ValueChanges[137] = SendTwo;
            if (ValueChanges._ValueChange[137] & ValueChanges._valuechange[137])
                keyboardkey(input, Keys.Two, keyboard_id);
            if (ValueChanges._ValueChange[137] & !ValueChanges._valuechange[137])
                keyboardkeyF(input, Keys.Two, keyboard_id);
            ValueChanges[138] = SendThree;
            if (ValueChanges._ValueChange[138] & ValueChanges._valuechange[138])
                keyboardkey(input, Keys.Three, keyboard_id);
            if (ValueChanges._ValueChange[138] & !ValueChanges._valuechange[138])
                keyboardkeyF(input, Keys.Three, keyboard_id);
            ValueChanges[139] = SendFour;
            if (ValueChanges._ValueChange[139] & ValueChanges._valuechange[139])
                keyboardkey(input, Keys.Four, keyboard_id);
            if (ValueChanges._ValueChange[139] & !ValueChanges._valuechange[139])
                keyboardkeyF(input, Keys.Four, keyboard_id);
            ValueChanges[140] = SendFive;
            if (ValueChanges._ValueChange[140] & ValueChanges._valuechange[140])
                keyboardkey(input, Keys.Five, keyboard_id);
            if (ValueChanges._ValueChange[140] & !ValueChanges._valuechange[140])
                keyboardkeyF(input, Keys.Five, keyboard_id);
            ValueChanges[141] = SendSix;
            if (ValueChanges._ValueChange[141] & ValueChanges._valuechange[141])
                keyboardkey(input, Keys.Six, keyboard_id);
            if (ValueChanges._ValueChange[141] & !ValueChanges._valuechange[141])
                keyboardkeyF(input, Keys.Six, keyboard_id);
            ValueChanges[142] = SendSeven;
            if (ValueChanges._ValueChange[142] & ValueChanges._valuechange[142])
                keyboardkey(input, Keys.Seven, keyboard_id);
            if (ValueChanges._ValueChange[142] & !ValueChanges._valuechange[142])
                keyboardkeyF(input, Keys.Seven, keyboard_id);
            ValueChanges[143] = SendEight;
            if (ValueChanges._ValueChange[143] & ValueChanges._valuechange[143])
                keyboardkey(input, Keys.Eight, keyboard_id);
            if (ValueChanges._ValueChange[143] & !ValueChanges._valuechange[143])
                keyboardkeyF(input, Keys.Eight, keyboard_id);
            ValueChanges[144] = SendNine;
            if (ValueChanges._ValueChange[144] & ValueChanges._valuechange[144])
                keyboardkey(input, Keys.Nine, keyboard_id);
            if (ValueChanges._ValueChange[144] & !ValueChanges._valuechange[144])
                keyboardkeyF(input, Keys.Nine, keyboard_id);
            ValueChanges[145] = SendZero;
            if (ValueChanges._ValueChange[145] & ValueChanges._valuechange[145])
                keyboardkey(input, Keys.Zero, keyboard_id);
            if (ValueChanges._ValueChange[145] & !ValueChanges._valuechange[145])
                keyboardkeyF(input, Keys.Zero, keyboard_id);
            ValueChanges[146] = SendDashUnderscore;
            if (ValueChanges._ValueChange[146] & ValueChanges._valuechange[146])
                keyboardkey(input, Keys.DashUnderscore, keyboard_id);
            if (ValueChanges._ValueChange[146] & !ValueChanges._valuechange[146])
                keyboardkeyF(input, Keys.DashUnderscore, keyboard_id);
            ValueChanges[147] = SendPlusEquals;
            if (ValueChanges._ValueChange[147] & ValueChanges._valuechange[147])
                keyboardkey(input, Keys.PlusEquals, keyboard_id);
            if (ValueChanges._ValueChange[147] & !ValueChanges._valuechange[147])
                keyboardkeyF(input, Keys.PlusEquals, keyboard_id);
            ValueChanges[148] = SendBackspace;
            if (ValueChanges._ValueChange[148] & ValueChanges._valuechange[148])
                keyboardkey(input, Keys.Backspace, keyboard_id);
            if (ValueChanges._ValueChange[148] & !ValueChanges._valuechange[148])
                keyboardkeyF(input, Keys.Backspace, keyboard_id);
            ValueChanges[149] = SendTab;
            if (ValueChanges._ValueChange[149] & ValueChanges._valuechange[149])
                keyboardkey(input, Keys.Tab, keyboard_id);
            if (ValueChanges._ValueChange[149] & !ValueChanges._valuechange[149])
                keyboardkeyF(input, Keys.Tab, keyboard_id);
            ValueChanges[150] = SendOpenBracketBrace;
            if (ValueChanges._ValueChange[150] & ValueChanges._valuechange[150])
                keyboardkey(input, Keys.OpenBracketBrace, keyboard_id);
            if (ValueChanges._ValueChange[150] & !ValueChanges._valuechange[150])
                keyboardkeyF(input, Keys.OpenBracketBrace, keyboard_id);
            ValueChanges[151] = SendCloseBracketBrace;
            if (ValueChanges._ValueChange[151] & ValueChanges._valuechange[151])
                keyboardkey(input, Keys.CloseBracketBrace, keyboard_id);
            if (ValueChanges._ValueChange[151] & !ValueChanges._valuechange[151])
                keyboardkeyF(input, Keys.CloseBracketBrace, keyboard_id);
            ValueChanges[152] = SendEnter;
            if (ValueChanges._ValueChange[152] & ValueChanges._valuechange[152])
                keyboardkey(input, Keys.Enter, keyboard_id);
            if (ValueChanges._ValueChange[152] & !ValueChanges._valuechange[152])
                keyboardkeyF(input, Keys.Enter, keyboard_id);
            ValueChanges[153] = SendSemicolonColon;
            if (ValueChanges._ValueChange[153] & ValueChanges._valuechange[153])
                keyboardkey(input, Keys.SemicolonColon, keyboard_id);
            if (ValueChanges._ValueChange[153] & !ValueChanges._valuechange[153])
                keyboardkeyF(input, Keys.SemicolonColon, keyboard_id);
            ValueChanges[154] = SendSingleDoubleQuote;
            if (ValueChanges._ValueChange[154] & ValueChanges._valuechange[154])
                keyboardkey(input, Keys.SingleDoubleQuote, keyboard_id);
            if (ValueChanges._ValueChange[154] & !ValueChanges._valuechange[154])
                keyboardkeyF(input, Keys.SingleDoubleQuote, keyboard_id);
            ValueChanges[155] = SendTilde;
            if (ValueChanges._ValueChange[155] & ValueChanges._valuechange[155])
                keyboardkey(input, Keys.Tilde, keyboard_id);
            if (ValueChanges._ValueChange[155] & !ValueChanges._valuechange[155])
                keyboardkeyF(input, Keys.Tilde, keyboard_id);
            ValueChanges[156] = SendLeftShift;
            if (ValueChanges._ValueChange[156] & ValueChanges._valuechange[156])
                keyboardkey(input, Keys.LeftShift, keyboard_id);
            if (ValueChanges._ValueChange[156] & !ValueChanges._valuechange[156])
                keyboardkeyF(input, Keys.LeftShift, keyboard_id);
            ValueChanges[157] = SendBackslashPipe;
            if (ValueChanges._ValueChange[157] & ValueChanges._valuechange[157])
                keyboardkey(input, Keys.BackslashPipe, keyboard_id);
            if (ValueChanges._ValueChange[157] & !ValueChanges._valuechange[157])
                keyboardkeyF(input, Keys.BackslashPipe, keyboard_id);
            ValueChanges[158] = SendCommaLeftArrow;
            if (ValueChanges._ValueChange[158] & ValueChanges._valuechange[158])
                keyboardkey(input, Keys.CommaLeftArrow, keyboard_id);
            if (ValueChanges._ValueChange[158] & !ValueChanges._valuechange[158])
                keyboardkeyF(input, Keys.CommaLeftArrow, keyboard_id);
            ValueChanges[159] = SendPeriodRightArrow;
            if (ValueChanges._ValueChange[159] & ValueChanges._valuechange[159])
                keyboardkey(input, Keys.PeriodRightArrow, keyboard_id);
            if (ValueChanges._ValueChange[159] & !ValueChanges._valuechange[159])
                keyboardkeyF(input, Keys.PeriodRightArrow, keyboard_id);
            ValueChanges[160] = SendForwardSlashQuestionMark;
            if (ValueChanges._ValueChange[160] & ValueChanges._valuechange[160])
                keyboardkey(input, Keys.ForwardSlashQuestionMark, keyboard_id);
            if (ValueChanges._ValueChange[160] & !ValueChanges._valuechange[160])
                keyboardkeyF(input, Keys.ForwardSlashQuestionMark, keyboard_id);
            ValueChanges[161] = SendRightShift;
            if (ValueChanges._ValueChange[161] & ValueChanges._valuechange[161])
                keyboardkey(input, Keys.RightShift, keyboard_id);
            if (ValueChanges._ValueChange[161] & !ValueChanges._valuechange[161])
                keyboardkeyF(input, Keys.RightShift, keyboard_id);
            ValueChanges[162] = SendRightAlt;
            if (ValueChanges._ValueChange[162] & ValueChanges._valuechange[162])
                keyboardkey(input, Keys.RightAlt, keyboard_id);
            if (ValueChanges._ValueChange[162] & !ValueChanges._valuechange[162])
                keyboardkeyF(input, Keys.RightAlt, keyboard_id);
            ValueChanges[163] = SendSpace;
            if (ValueChanges._ValueChange[163] & ValueChanges._valuechange[163])
                keyboardkey(input, Keys.Space, keyboard_id);
            if (ValueChanges._ValueChange[163] & !ValueChanges._valuechange[163])
                keyboardkeyF(input, Keys.Space, keyboard_id);
            ValueChanges[164] = SendCapsLock;
            if (ValueChanges._ValueChange[164] & ValueChanges._valuechange[164])
                keyboardkey(input, Keys.CapsLock, keyboard_id);
            if (ValueChanges._ValueChange[164] & !ValueChanges._valuechange[164])
                keyboardkeyF(input, Keys.CapsLock, keyboard_id);
            ValueChanges[165] = SendUp;
            if (ValueChanges._ValueChange[165] & ValueChanges._valuechange[165])
                keyboardkey(input, Keys.Up, keyboard_id);
            if (ValueChanges._ValueChange[165] & !ValueChanges._valuechange[165])
                keyboardkeyF(input, Keys.Up, keyboard_id);
            ValueChanges[166] = SendDown;
            if (ValueChanges._ValueChange[166] & ValueChanges._valuechange[166])
                keyboardkey(input, Keys.Down, keyboard_id);
            if (ValueChanges._ValueChange[166] & !ValueChanges._valuechange[166])
                keyboardkeyF(input, Keys.Down, keyboard_id);
            ValueChanges[167] = SendRight;
            if (ValueChanges._ValueChange[167] & ValueChanges._valuechange[167])
                keyboardkey(input, Keys.Right, keyboard_id);
            if (ValueChanges._ValueChange[167] & !ValueChanges._valuechange[167])
                keyboardkeyF(input, Keys.Right, keyboard_id);
            ValueChanges[168] = SendLeft;
            if (ValueChanges._ValueChange[168] & ValueChanges._valuechange[168])
                keyboardkey(input, Keys.Left, keyboard_id);
            if (ValueChanges._ValueChange[168] & !ValueChanges._valuechange[168])
                keyboardkeyF(input, Keys.Left, keyboard_id);
            ValueChanges[169] = SendHome;
            if (ValueChanges._ValueChange[169] & ValueChanges._valuechange[169])
                keyboardkey(input, Keys.Home, keyboard_id);
            if (ValueChanges._ValueChange[169] & !ValueChanges._valuechange[169])
                keyboardkeyF(input, Keys.Home, keyboard_id);
            ValueChanges[170] = SendEnd;
            if (ValueChanges._ValueChange[170] & ValueChanges._valuechange[170])
                keyboardkey(input, Keys.End, keyboard_id);
            if (ValueChanges._ValueChange[170] & !ValueChanges._valuechange[170])
                keyboardkeyF(input, Keys.End, keyboard_id);
            ValueChanges[171] = SendDelete;
            if (ValueChanges._ValueChange[171] & ValueChanges._valuechange[171])
                keyboardkey(input, Keys.Delete, keyboard_id);
            if (ValueChanges._ValueChange[171] & !ValueChanges._valuechange[171])
                keyboardkeyF(input, Keys.Delete, keyboard_id);
            ValueChanges[172] = SendPageUp;
            if (ValueChanges._ValueChange[172] & ValueChanges._valuechange[172])
                keyboardkey(input, Keys.PageUp, keyboard_id);
            if (ValueChanges._ValueChange[172] & !ValueChanges._valuechange[172])
                keyboardkeyF(input, Keys.PageUp, keyboard_id);
            ValueChanges[173] = SendPageDown;
            if (ValueChanges._ValueChange[173] & ValueChanges._valuechange[173])
                keyboardkey(input, Keys.PageDown, keyboard_id);
            if (ValueChanges._ValueChange[173] & !ValueChanges._valuechange[173])
                keyboardkeyF(input, Keys.PageDown, keyboard_id);
            ValueChanges[174] = SendInsert;
            if (ValueChanges._ValueChange[174] & ValueChanges._valuechange[174])
                keyboardkey(input, Keys.Insert, keyboard_id);
            if (ValueChanges._ValueChange[174] & !ValueChanges._valuechange[174])
                keyboardkeyF(input, Keys.Insert, keyboard_id);
            ValueChanges[175] = SendPrintScreen;
            if (ValueChanges._ValueChange[175] & ValueChanges._valuechange[175])
                keyboardkey(input, Keys.PrintScreen, keyboard_id);
            if (ValueChanges._ValueChange[175] & !ValueChanges._valuechange[175])
                keyboardkeyF(input, Keys.PrintScreen, keyboard_id);
            ValueChanges[176] = SendNumLock;
            if (ValueChanges._ValueChange[176] & ValueChanges._valuechange[176])
                keyboardkey(input, Keys.NumLock, keyboard_id);
            if (ValueChanges._ValueChange[176] & !ValueChanges._valuechange[176])
                keyboardkeyF(input, Keys.NumLock, keyboard_id);
            ValueChanges[177] = SendScrollLock;
            if (ValueChanges._ValueChange[177] & ValueChanges._valuechange[177])
                keyboardkey(input, Keys.ScrollLock, keyboard_id);
            if (ValueChanges._ValueChange[177] & !ValueChanges._valuechange[177])
                keyboardkeyF(input, Keys.ScrollLock, keyboard_id);
            ValueChanges[178] = SendMenu;
            if (ValueChanges._ValueChange[178] & ValueChanges._valuechange[178])
                keyboardkey(input, Keys.Menu, keyboard_id);
            if (ValueChanges._ValueChange[178] & !ValueChanges._valuechange[178])
                keyboardkeyF(input, Keys.Menu, keyboard_id);
            ValueChanges[179] = SendWindowsKey;
            if (ValueChanges._ValueChange[179] & ValueChanges._valuechange[179])
                keyboardkey(input, Keys.WindowsKey, keyboard_id);
            if (ValueChanges._ValueChange[179] & !ValueChanges._valuechange[179])
                keyboardkeyF(input, Keys.WindowsKey, keyboard_id);
            ValueChanges[180] = SendNumpadDivide;
            if (ValueChanges._ValueChange[180] & ValueChanges._valuechange[180])
                keyboardkey(input, Keys.NumpadDivide, keyboard_id);
            if (ValueChanges._ValueChange[180] & !ValueChanges._valuechange[180])
                keyboardkeyF(input, Keys.NumpadDivide, keyboard_id);
            ValueChanges[181] = SendNumpadAsterisk;
            if (ValueChanges._ValueChange[181] & ValueChanges._valuechange[181])
                keyboardkey(input, Keys.NumpadAsterisk, keyboard_id);
            if (ValueChanges._ValueChange[181] & !ValueChanges._valuechange[181])
                keyboardkeyF(input, Keys.NumpadAsterisk, keyboard_id);
            ValueChanges[182] = SendNumpad7;
            if (ValueChanges._ValueChange[182] & ValueChanges._valuechange[182])
                keyboardkey(input, Keys.Numpad7, keyboard_id);
            if (ValueChanges._ValueChange[182] & !ValueChanges._valuechange[182])
                keyboardkeyF(input, Keys.Numpad7, keyboard_id);
            ValueChanges[183] = SendNumpad8;
            if (ValueChanges._ValueChange[183] & ValueChanges._valuechange[183])
                keyboardkey(input, Keys.Numpad8, keyboard_id);
            if (ValueChanges._ValueChange[183] & !ValueChanges._valuechange[183])
                keyboardkeyF(input, Keys.Numpad8, keyboard_id);
            ValueChanges[184] = SendNumpad9;
            if (ValueChanges._ValueChange[184] & ValueChanges._valuechange[184])
                keyboardkey(input, Keys.Numpad9, keyboard_id);
            if (ValueChanges._ValueChange[184] & !ValueChanges._valuechange[184])
                keyboardkeyF(input, Keys.Numpad9, keyboard_id);
            ValueChanges[185] = SendNumpad4;
            if (ValueChanges._ValueChange[185] & ValueChanges._valuechange[185])
                keyboardkey(input, Keys.Numpad4, keyboard_id);
            if (ValueChanges._ValueChange[185] & !ValueChanges._valuechange[185])
                keyboardkeyF(input, Keys.Numpad4, keyboard_id);
            ValueChanges[186] = SendNumpad5;
            if (ValueChanges._ValueChange[186] & ValueChanges._valuechange[186])
                keyboardkey(input, Keys.Numpad5, keyboard_id);
            if (ValueChanges._ValueChange[186] & !ValueChanges._valuechange[186])
                keyboardkeyF(input, Keys.Numpad5, keyboard_id);
            ValueChanges[187] = SendNumpad6;
            if (ValueChanges._ValueChange[187] & ValueChanges._valuechange[187])
                keyboardkey(input, Keys.Numpad6, keyboard_id);
            if (ValueChanges._ValueChange[187] & !ValueChanges._valuechange[187])
                keyboardkeyF(input, Keys.Numpad6, keyboard_id);
            ValueChanges[188] = SendNumpad1;
            if (ValueChanges._ValueChange[188] & ValueChanges._valuechange[188])
                keyboardkey(input, Keys.Numpad1, keyboard_id);
            if (ValueChanges._ValueChange[188] & !ValueChanges._valuechange[188])
                keyboardkeyF(input, Keys.Numpad1, keyboard_id);
            ValueChanges[189] = SendNumpad2;
            if (ValueChanges._ValueChange[189] & ValueChanges._valuechange[189])
                keyboardkey(input, Keys.Numpad2, keyboard_id);
            if (ValueChanges._ValueChange[189] & !ValueChanges._valuechange[189])
                keyboardkeyF(input, Keys.Numpad2, keyboard_id);
            ValueChanges[190] = SendNumpad3;
            if (ValueChanges._ValueChange[190] & ValueChanges._valuechange[190])
                keyboardkey(input, Keys.Numpad3, keyboard_id);
            if (ValueChanges._ValueChange[190] & !ValueChanges._valuechange[190])
                keyboardkeyF(input, Keys.Numpad3, keyboard_id);
            ValueChanges[191] = SendNumpad0;
            if (ValueChanges._ValueChange[191] & ValueChanges._valuechange[191])
                keyboardkey(input, Keys.Numpad0, keyboard_id);
            if (ValueChanges._ValueChange[191] & !ValueChanges._valuechange[191])
                keyboardkeyF(input, Keys.Numpad0, keyboard_id);
            ValueChanges[192] = SendNumpadDelete;
            if (ValueChanges._ValueChange[192] & ValueChanges._valuechange[192])
                keyboardkey(input, Keys.NumpadDelete, keyboard_id);
            if (ValueChanges._ValueChange[192] & !ValueChanges._valuechange[192])
                keyboardkeyF(input, Keys.NumpadDelete, keyboard_id);
            ValueChanges[193] = SendNumpadEnter;
            if (ValueChanges._ValueChange[193] & ValueChanges._valuechange[193])
                keyboardkey(input, Keys.NumpadEnter, keyboard_id);
            if (ValueChanges._ValueChange[193] & !ValueChanges._valuechange[193])
                keyboardkeyF(input, Keys.NumpadEnter, keyboard_id);
            ValueChanges[194] = SendNumpadPlus;
            if (ValueChanges._ValueChange[194] & ValueChanges._valuechange[194])
                keyboardkey(input, Keys.NumpadPlus, keyboard_id);
            if (ValueChanges._ValueChange[194] & !ValueChanges._valuechange[194])
                keyboardkeyF(input, Keys.NumpadPlus, keyboard_id);
            ValueChanges[195] = SendNumpadMinus;
            if (ValueChanges._ValueChange[195] & ValueChanges._valuechange[195])
                keyboardkey(input, Keys.NumpadMinus, keyboard_id);
            if (ValueChanges._ValueChange[195] & !ValueChanges._valuechange[195])
                keyboardkeyF(input, Keys.NumpadMinus, keyboard_id);
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
                string str = "keyboard_id : " + keyboard_id + Environment.NewLine;
                str += "mouse_id : " + mouse_id + Environment.NewLine;
                str += "MouseDesktopX : " + MouseDesktopX + Environment.NewLine;
                str += "MouseDesktopY : " + MouseDesktopY + Environment.NewLine;
                str += "deltaX : " + deltaX + Environment.NewLine;
                str += "deltaY : " + deltaY + Environment.NewLine;
                str += "x : " + x + Environment.NewLine;
                str += "y : " + y + Environment.NewLine;
                str += "SendLeftClick : " + SendLeftClick + Environment.NewLine;
                str += "SendRightClick : " + SendRightClick + Environment.NewLine;
                str += "SendMiddleClick : " + SendMiddleClick + Environment.NewLine;
                str += "SendWheelUp : " + SendWheelUp + Environment.NewLine;
                str += "SendWheelDown : " + SendWheelDown + Environment.NewLine;
                str += "SendCANCEL : " + SendCANCEL + Environment.NewLine;
                str += "SendBACK : " + SendBACK + Environment.NewLine;
                str += "SendTAB : " + SendTAB + Environment.NewLine;
                str += "SendCLEAR : " + SendCLEAR + Environment.NewLine;
                str += "SendRETURN : " + SendRETURN + Environment.NewLine;
                str += "SendSHIFT : " + SendSHIFT + Environment.NewLine;
                str += "SendCONTROL : " + SendCONTROL + Environment.NewLine;
                str += "SendMENU : " + SendMENU + Environment.NewLine;
                str += "SendCAPITAL : " + SendCAPITAL + Environment.NewLine;
                str += "SendESCAPE : " + SendESCAPE + Environment.NewLine;
                str += "SendSPACE : " + SendSPACE + Environment.NewLine;
                str += "SendPRIOR : " + SendPRIOR + Environment.NewLine;
                str += "SendNEXT : " + SendNEXT + Environment.NewLine;
                str += "SendEND : " + SendEND + Environment.NewLine;
                str += "SendHOME : " + SendHOME + Environment.NewLine;
                str += "SendLEFT : " + SendLEFT + Environment.NewLine;
                str += "SendUP : " + SendUP + Environment.NewLine;
                str += "SendRIGHT : " + SendRIGHT + Environment.NewLine;
                str += "SendDOWN : " + SendDOWN + Environment.NewLine;
                str += "SendSNAPSHOT : " + SendSNAPSHOT + Environment.NewLine;
                str += "SendINSERT : " + SendINSERT + Environment.NewLine;
                str += "SendNUMPADDEL : " + SendNUMPADDEL + Environment.NewLine;
                str += "SendNUMPADINSERT : " + SendNUMPADINSERT + Environment.NewLine;
                str += "SendHELP : " + SendHELP + Environment.NewLine;
                str += "SendAPOSTROPHE : " + SendAPOSTROPHE + Environment.NewLine;
                str += "SendBACKSPACE : " + SendBACKSPACE + Environment.NewLine;
                str += "SendPAGEDOWN : " + SendPAGEDOWN + Environment.NewLine;
                str += "SendPAGEUP : " + SendPAGEUP + Environment.NewLine;
                str += "SendFIN : " + SendFIN + Environment.NewLine;
                str += "SendMOUSE : " + SendMOUSE + Environment.NewLine;
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
                str += "SendDELETE : " + SendDELETE + Environment.NewLine;
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
                str += "SendSUBTRACT : " + SendSUBTRACT + Environment.NewLine;
                str += "SendDECIMAL : " + SendDECIMAL + Environment.NewLine;
                str += "SendPRINTSCREEN : " + SendPRINTSCREEN + Environment.NewLine;
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
                str += "SendNUMLOCK : " + SendNUMLOCK + Environment.NewLine;
                str += "SendSCROLLLOCK : " + SendSCROLLLOCK + Environment.NewLine;
                str += "SendLEFTSHIFT : " + SendLEFTSHIFT + Environment.NewLine;
                str += "SendRIGHTSHIFT : " + SendRIGHTSHIFT + Environment.NewLine;
                str += "SendLEFTCONTROL : " + SendLEFTCONTROL + Environment.NewLine;
                str += "SendRIGHTCONTROL : " + SendRIGHTCONTROL + Environment.NewLine;
                str += "SendLEFTALT : " + SendLEFTALT + Environment.NewLine;
                str += "SendRIGHTALT : " + SendRIGHTALT + Environment.NewLine;
                str += "SendBROWSER_BACK : " + SendBROWSER_BACK + Environment.NewLine;
                str += "SendBROWSER_FORWARD : " + SendBROWSER_FORWARD + Environment.NewLine;
                str += "SendBROWSER_REFRESH : " + SendBROWSER_REFRESH + Environment.NewLine;
                str += "SendBROWSER_STOP : " + SendBROWSER_STOP + Environment.NewLine;
                str += "SendBROWSER_SEARCH : " + SendBROWSER_SEARCH + Environment.NewLine;
                str += "SendBROWSER_FAVORITES : " + SendBROWSER_FAVORITES + Environment.NewLine;
                str += "SendBROWSER_HOME : " + SendBROWSER_HOME + Environment.NewLine;
                str += "SendVOLUME_MUTE : " + SendVOLUME_MUTE + Environment.NewLine;
                str += "SendVOLUME_DOWN : " + SendVOLUME_DOWN + Environment.NewLine;
                str += "SendVOLUME_UP : " + SendVOLUME_UP + Environment.NewLine;
                str += "SendMEDIA_NEXT_TRACK : " + SendMEDIA_NEXT_TRACK + Environment.NewLine;
                str += "SendMEDIA_PREV_TRACK : " + SendMEDIA_PREV_TRACK + Environment.NewLine;
                str += "SendMEDIA_STOP : " + SendMEDIA_STOP + Environment.NewLine;
                str += "SendMEDIA_PLAY_PAUSE : " + SendMEDIA_PLAY_PAUSE + Environment.NewLine;
                str += "SendLAUNCH_MAIL : " + SendLAUNCH_MAIL + Environment.NewLine;
                str += "SendLAUNCH_MEDIA_SELECT : " + SendLAUNCH_MEDIA_SELECT + Environment.NewLine;
                str += "SendLAUNCH_APP1 : " + SendLAUNCH_APP1 + Environment.NewLine;
                str += "SendLAUNCH_APP2 : " + SendLAUNCH_APP2 + Environment.NewLine;
                str += "SendOEM_1 : " + SendOEM_1 + Environment.NewLine;
                str += "SendOEM_PLUS : " + SendOEM_PLUS + Environment.NewLine;
                str += "SendOEM_COMMA : " + SendOEM_COMMA + Environment.NewLine;
                str += "SendOEM_MINUS : " + SendOEM_MINUS + Environment.NewLine;
                str += "SendOEM_PERIOD : " + SendOEM_PERIOD + Environment.NewLine;
                str += "SendOEM_2 : " + SendOEM_2 + Environment.NewLine;
                str += "SendOEM_3 : " + SendOEM_3 + Environment.NewLine;
                str += "SendOEM_4 : " + SendOEM_4 + Environment.NewLine;
                str += "SendOEM_5 : " + SendOEM_5 + Environment.NewLine;
                str += "SendOEM_6 : " + SendOEM_6 + Environment.NewLine;
                str += "SendOEM_7 : " + SendOEM_7 + Environment.NewLine;
                str += "SendOEM_8 : " + SendOEM_8 + Environment.NewLine;
                str += "SendOEM_102 : " + SendOEM_102 + Environment.NewLine;
                str += "SendEREOF : " + SendEREOF + Environment.NewLine;
                str += "SendZOOM : " + SendZOOM + Environment.NewLine;
                str += "SendEscape : " + SendEscape + Environment.NewLine;
                str += "SendOne : " + SendOne + Environment.NewLine;
                str += "SendTwo : " + SendTwo + Environment.NewLine;
                str += "SendThree : " + SendThree + Environment.NewLine;
                str += "SendFour : " + SendFour + Environment.NewLine;
                str += "SendFive : " + SendFive + Environment.NewLine;
                str += "SendSix : " + SendSix + Environment.NewLine;
                str += "SendSeven : " + SendSeven + Environment.NewLine;
                str += "SendEight : " + SendEight + Environment.NewLine;
                str += "SendNine : " + SendNine + Environment.NewLine;
                str += "SendZero : " + SendZero + Environment.NewLine;
                str += "SendDashUnderscore : " + SendDashUnderscore + Environment.NewLine;
                str += "SendPlusEquals : " + SendPlusEquals + Environment.NewLine;
                str += "SendBackspace : " + SendBackspace + Environment.NewLine;
                str += "SendTab : " + SendTab + Environment.NewLine;
                str += "SendOpenBracketBrace : " + SendOpenBracketBrace + Environment.NewLine;
                str += "SendCloseBracketBrace : " + SendCloseBracketBrace + Environment.NewLine;
                str += "SendEnter : " + SendEnter + Environment.NewLine;
                str += "SendControl : " + SendControl + Environment.NewLine;
                str += "SendSemicolonColon : " + SendSemicolonColon + Environment.NewLine;
                str += "SendSingleDoubleQuote : " + SendSingleDoubleQuote + Environment.NewLine;
                str += "SendTilde : " + SendTilde + Environment.NewLine;
                str += "SendLeftShift : " + SendLeftShift + Environment.NewLine;
                str += "SendBackslashPipe : " + SendBackslashPipe + Environment.NewLine;
                str += "SendCommaLeftArrow : " + SendCommaLeftArrow + Environment.NewLine;
                str += "SendPeriodRightArrow : " + SendPeriodRightArrow + Environment.NewLine;
                str += "SendForwardSlashQuestionMark : " + SendForwardSlashQuestionMark + Environment.NewLine;
                str += "SendRightShift : " + SendRightShift + Environment.NewLine;
                str += "SendRightAlt : " + SendRightAlt + Environment.NewLine;
                str += "SendSpace : " + SendSpace + Environment.NewLine;
                str += "SendCapsLock : " + SendCapsLock + Environment.NewLine;
                str += "SendUp : " + SendUp + Environment.NewLine;
                str += "SendDown : " + SendDown + Environment.NewLine;
                str += "SendRight : " + SendRight + Environment.NewLine;
                str += "SendLeft : " + SendLeft + Environment.NewLine;
                str += "SendHome : " + SendHome + Environment.NewLine;
                str += "SendEnd : " + SendEnd + Environment.NewLine;
                str += "SendDelete : " + SendDelete + Environment.NewLine;
                str += "SendPageUp : " + SendPageUp + Environment.NewLine;
                str += "SendPageDown : " + SendPageDown + Environment.NewLine;
                str += "SendInsert : " + SendInsert + Environment.NewLine;
                str += "SendPrintScreen : " + SendPrintScreen + Environment.NewLine;
                str += "SendNumLock : " + SendNumLock + Environment.NewLine;
                str += "SendScrollLock : " + SendScrollLock + Environment.NewLine;
                str += "SendMenu : " + SendMenu + Environment.NewLine;
                str += "SendWindowsKey : " + SendWindowsKey + Environment.NewLine;
                str += "SendNumpadDivide : " + SendNumpadDivide + Environment.NewLine;
                str += "SendNumpadAsterisk : " + SendNumpadAsterisk + Environment.NewLine;
                str += "SendNumpad7 : " + SendNumpad7 + Environment.NewLine;
                str += "SendNumpad8 : " + SendNumpad8 + Environment.NewLine;
                str += "SendNumpad9 : " + SendNumpad9 + Environment.NewLine;
                str += "SendNumpad4 : " + SendNumpad4 + Environment.NewLine;
                str += "SendNumpad5 : " + SendNumpad5 + Environment.NewLine;
                str += "SendNumpad6 : " + SendNumpad6 + Environment.NewLine;
                str += "SendNumpad1 : " + SendNumpad1 + Environment.NewLine;
                str += "SendNumpad2 : " + SendNumpad2 + Environment.NewLine;
                str += "SendNumpad3 : " + SendNumpad3 + Environment.NewLine;
                str += "SendNumpad0 : " + SendNumpad0 + Environment.NewLine;
                str += "SendNumpadDelete : " + SendNumpadDelete + Environment.NewLine;
                str += "SendNumpadEnter : " + SendNumpadEnter + Environment.NewLine;
                str += "SendNumpadPlus : " + SendNumpadPlus + Environment.NewLine;
                str += "SendNumpadMinus : " + SendNumpadMinus + Environment.NewLine;
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
        private void mouseclickleft(Input input, int mouse_id)
        {
            Task.Run(() => input.SendLeftClick(mouse_id));
        }
        private void mouseclickleftF(Input input, int mouse_id)
        {
            Task.Run(() => input.SendLeftClickF(mouse_id));
        }
        private void mouseclickright(Input input, int mouse_id)
        {
            Task.Run(() => input.SendRightClick(mouse_id));
        }
        private void mouseclickrightF(Input input, int mouse_id)
        {
            Task.Run(() => input.SendRightClickF(mouse_id));
        }
        private void mouseclickmiddle(Input input, int mouse_id)
        {
            Task.Run(() => input.SendMiddleClick(mouse_id));
        }
        private void mouseclickmiddleF(Input input, int mouse_id)
        {
            Task.Run(() => input.SendMiddleClickF(mouse_id));
        }
        private void mousewheelup(Input input, int mouse_id)
        {
            Task.Run(() => input.SendWheelUp(mouse_id));
        }
        private void mousewheeldown(Input input, int mouse_id)
        {
            Task.Run(() => input.SendWheelDown(mouse_id));
        }
        private void keyboardkey(Input input, Keys key, int keyboard_id)
        {
            Task.Run(() => input.SendKey(key, keyboard_id));
        }
        private void keyboardkeyF(Input input, Keys key, int keyboard_id)
        {
            Task.Run(() => input.SendKeyF(key, keyboard_id));
        }
        public void MoveMouseBy(Input input, int deltaX, int deltaY, int mouseId)
        {
            Task.Run(() => input.MoveMouseBy(deltaX, deltaY, mouseId));
        }
        private void MoveMouseTo(Input input, int x, int y, int mouseId)
        {
            Task.Run(() => input.MoveMouseTo(x, y, mouseId));
        }
    }
    public class Input
    {
        private IntPtr context;
        public KeyboardFilterMode KeyboardFilterMode { get; set; }
        public MouseFilterMode MouseFilterMode { get; set; }
        public bool IsLoaded { get; set; }
        public Input()
        {
            context = IntPtr.Zero;
            KeyboardFilterMode = KeyboardFilterMode.None;
            MouseFilterMode = MouseFilterMode.None;
        }
        public bool Load()
        {
            context = InterceptionDriver.CreateContext();
            return true;
        }
        public void Unload()
        {
            InterceptionDriver.DestroyContext(context);
        }
        public void SendKey(Keys key, KeyState state, int keyboardId)
        {
            Stroke stroke = new Stroke();
            KeyStroke keyStroke = new KeyStroke();
            keyStroke.Code = key;
            keyStroke.State = state;
            stroke.Key = keyStroke;
            InterceptionDriver.Send(context, keyboardId, ref stroke, 1);
        }
        public void SendKey(Keys key, int keyboardId)
        {
            SendKey(key, KeyState.Down, keyboardId);
        }
        public void SendKeyF(Keys key, int keyboardId)
        {
            SendKey(key, KeyState.Up, keyboardId);
        }
        public void SendMouseEvent(MouseState state, int mouseId)
        {
            Stroke stroke = new Stroke();
            MouseStroke mouseStroke = new MouseStroke();
            mouseStroke.State = state;
            if (state == MouseState.ScrollUp)
            {
                mouseStroke.Rolling = 120;
            }
            else if (state == MouseState.ScrollDown)
            {
                mouseStroke.Rolling = -120;
            }
            stroke.Mouse = mouseStroke;
            InterceptionDriver.Send(context, mouseId, ref stroke, 1);
        }
        public void SendLeftClick(int mouseId)
        {
            SendMouseEvent(MouseState.LeftDown, mouseId);
        }
        public void SendRightClick(int mouseId)
        {
            SendMouseEvent(MouseState.RightDown, mouseId);
        }
        public void SendLeftClickF(int mouseId)
        {
            SendMouseEvent(MouseState.LeftUp, mouseId);
        }
        public void SendRightClickF(int mouseId)
        {
            SendMouseEvent(MouseState.RightUp, mouseId);
        }
        public void SendMiddleClick(int mouseId)
        {
            SendMouseEvent(MouseState.MiddleDown, mouseId);
        }
        public void SendMiddleClickF(int mouseId)
        {
            SendMouseEvent(MouseState.MiddleUp, mouseId);
        }
        public void SendWheelUp(int mouseId)
        {
            SendMouseEvent(MouseState.ScrollUp, mouseId);
        }
        public void SendWheelDown(int mouseId)
        {
            SendMouseEvent(MouseState.ScrollDown, mouseId);
        }
        public void MoveMouseBy(int deltaX, int deltaY, int mouseId)
        {
            if (deltaX != 0 | deltaY != 0)
            {
                Stroke stroke = new Stroke();
                MouseStroke mouseStroke = new MouseStroke();
                mouseStroke.X = deltaX;
                mouseStroke.Y = deltaY;
                stroke.Mouse = mouseStroke;
                stroke.Mouse.Flags = MouseFlags.MoveRelative;
                InterceptionDriver.Send(context, mouseId, ref stroke, 1);
            }
        }
        public void MoveMouseTo(int x, int y, int mouseId)
        {
            if (x != 0 | y != 0)
            {
                Stroke stroke = new Stroke();
                MouseStroke mouseStroke = new MouseStroke();
                mouseStroke.X = x;
                mouseStroke.Y = y;
                stroke.Mouse = mouseStroke;
                stroke.Mouse.Flags = MouseFlags.MoveAbsolute;
                InterceptionDriver.Send(context, mouseId, ref stroke, 1);
            }
        }
    }
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate int Predicate(int device);
    [Flags]
    public enum KeyState : ushort
    {
        Down = 0x00,
        Up = 0x01,
        E0 = 0x02,
        E1 = 0x04,
        TermsrvSetLED = 0x08,
        TermsrvShadow = 0x10,
        TermsrvVKPacket = 0x20
    }
    [Flags]
    public enum KeyboardFilterMode : ushort
    {
        None = 0x0000,
        All = 0xFFFF,
        KeyDown = KeyState.Up,
        KeyUp = KeyState.Up << 1,
        KeyE0 = KeyState.E0 << 1,
        KeyE1 = KeyState.E1 << 1,
        KeyTermsrvSetLED = KeyState.TermsrvSetLED << 1,
        KeyTermsrvShadow = KeyState.TermsrvShadow << 1,
        KeyTermsrvVKPacket = KeyState.TermsrvVKPacket << 1
    }
    [Flags]
    public enum MouseState : ushort
    {
        LeftDown = 0x01,
        LeftUp = 0x02,
        RightDown = 0x04,
        RightUp = 0x08,
        MiddleDown = 0x10,
        MiddleUp = 0x20,
        LeftExtraDown = 0x40,
        LeftExtraUp = 0x80,
        RightExtraDown = 0x100,
        RightExtraUp = 0x200,
        ScrollVertical = 0x400,
        ScrollUp = 0x400,
        ScrollDown = 0x400,
        ScrollHorizontal = 0x800,
        ScrollLeft = 0x800,
        ScrollRight = 0x800,
    }
    [Flags]
    public enum MouseFilterMode : ushort
    {
        None = 0x0000,
        All = 0xFFFF,
        LeftDown = 0x01,
        LeftUp = 0x02,
        RightDown = 0x04,
        RightUp = 0x08,
        MiddleDown = 0x10,
        MiddleUp = 0x20,
        LeftExtraDown = 0x40,
        LeftExtraUp = 0x80,
        RightExtraDown = 0x100,
        RightExtraUp = 0x200,
        MouseWheelVertical = 0x400,
        MouseWheelHorizontal = 0x800,
        MouseMove = 0x1000,
    }
    [Flags]
    public enum MouseFlags : ushort
    {
        MoveRelative = 0x000,
        MoveAbsolute = 0x001,
        VirtualDesktop = 0x002,
        AttributesChanged = 0x004,
        MoveWithoutCoalescing = 0x008,
        TerminalServicesSourceShadow = 0x100
    }
    [StructLayout(LayoutKind.Sequential)]
    public struct MouseStroke
    {
        public MouseState State;
        public MouseFlags Flags;
        public Int16 Rolling;
        public Int32 X;
        public Int32 Y;
        public UInt16 Information;
    }
    [StructLayout(LayoutKind.Sequential)]
    public struct KeyStroke
    {
        public Keys Code;
        public KeyState State;
        public UInt32 Information;
    }
    [StructLayout(LayoutKind.Explicit)]
    public struct Stroke
    {
        [FieldOffset(0)]
        public MouseStroke Mouse;
        [FieldOffset(0)]
        public KeyStroke Key;
    }
    public class InterceptionDriver
    {
        [DllImport("interception.dll", EntryPoint = "interception_create_context", CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr CreateContext();
        [DllImport("interception.dll", EntryPoint = "interception_destroy_context", CallingConvention = CallingConvention.Cdecl)]
        public static extern void DestroyContext(IntPtr context);
        [DllImport("interception.dll", EntryPoint = "interception_get_precedence", CallingConvention = CallingConvention.Cdecl)]
        public static extern void GetPrecedence(IntPtr context, Int32 device);
        [DllImport("interception.dll", EntryPoint = "interception_set_precedence", CallingConvention = CallingConvention.Cdecl)]
        public static extern void SetPrecedence(IntPtr context, Int32 device, Int32 Precedence);
        [DllImport("interception.dll", EntryPoint = "interception_get_filter", CallingConvention = CallingConvention.Cdecl)]
        public static extern void GetFilter(IntPtr context, Int32 device);
        [DllImport("interception.dll", EntryPoint = "interception_set_filter", CallingConvention = CallingConvention.Cdecl)]
        public static extern void SetFilter(IntPtr context, Predicate predicate, Int32 keyboardFilterMode);
        [DllImport("interception.dll", EntryPoint = "interception_wait", CallingConvention = CallingConvention.Cdecl)]
        public static extern Int32 Wait(IntPtr context);
        [DllImport("interception.dll", EntryPoint = "interception_wait_with_timeout", CallingConvention = CallingConvention.Cdecl)]
        public static extern Int32 WaitWithTimeout(IntPtr context, UInt64 milliseconds);
        [DllImport("interception.dll", EntryPoint = "interception_send", CallingConvention = CallingConvention.Cdecl)]
        public static extern Int32 Send(IntPtr context, Int32 device, ref Stroke stroke, UInt32 numStrokes);
        [DllImport("interception.dll", EntryPoint = "interception_receive", CallingConvention = CallingConvention.Cdecl)]
        public static extern Int32 Receive(IntPtr context, Int32 device, ref Stroke stroke, UInt32 numStrokes);
        [DllImport("interception.dll", EntryPoint = "interception_get_hardware_id", CallingConvention = CallingConvention.Cdecl)]
        public static extern Int32 GetHardwareId(IntPtr context, Int32 device, String hardwareIdentifier, UInt32 sizeOfString);
        [DllImport("interception.dll", EntryPoint = "interception_is_invalid", CallingConvention = CallingConvention.Cdecl)]
        public static extern Int32 IsInvalid(Int32 device);
        [DllImport("interception.dll", EntryPoint = "interception_is_keyboard", CallingConvention = CallingConvention.Cdecl)]
        public static extern Int32 IsKeyboard(Int32 device);
        [DllImport("interception.dll", EntryPoint = "interception_is_mouse", CallingConvention = CallingConvention.Cdecl)]
        public static extern Int32 IsMouse(Int32 device);
    }
    public class KeyPressedEventArgs : EventArgs
    {
        public Keys Key { get; set; }
        public KeyState State { get; set; }
        public bool Handled { get; set; }
    }
    public enum Keys : ushort
    {
        CANCEL = 70,
        BACK = 14,
        TAB = 15,
        CLEAR = 76,
        RETURN = 28,
        SHIFT = 42,
        CONTROL = 29,
        MENU = 56,
        CAPITAL = 58,
        ESCAPE = 1,
        SPACE = 57,
        PRIOR = 73,
        NEXT = 81,
        END = 79,
        HOME = 71,
        LEFT = 101,
        UP = 100,
        RIGHT = 103,
        DOWN = 102,
        SNAPSHOT = 84,
        INSERT = 91,
        NUMPADDEL = 83,
        NUMPADINSERT = 82,
        HELP = 99,
        APOSTROPHE = 41,
        BACKSPACE = 14,
        PAGEDOWN = 97,
        PAGEUP = 93,
        FIN = 96,
        MOUSE = 105,
        A = 16,
        B = 48,
        C = 46,
        D = 32,
        E = 18,
        F = 33,
        G = 34,
        H = 35,
        I = 23,
        J = 36,
        K = 37,
        L = 38,
        M = 39,
        N = 49,
        O = 24,
        P = 25,
        Q = 30,
        R = 19,
        S = 31,
        T = 20,
        U = 22,
        V = 47,
        W = 44,
        X = 45,
        Y = 21,
        Z = 17,
        LWIN = 91,
        RWIN = 92,
        APPS = 93,
        DELETE = 95,
        NUMPAD0 = 82,
        NUMPAD1 = 79,
        NUMPAD2 = 80,
        NUMPAD3 = 81,
        NUMPAD4 = 75,
        NUMPAD5 = 76,
        NUMPAD6 = 77,
        NUMPAD7 = 71,
        NUMPAD8 = 72,
        NUMPAD9 = 73,
        MULTIPLY = 55,
        ADD = 78,
        SUBTRACT = 74,
        DECIMAL = 83,
        PRINTSCREEN = 84,
        DIVIDE = 53,
        F1 = 59,
        F2 = 60,
        F3 = 61,
        F4 = 62,
        F5 = 63,
        F6 = 64,
        F7 = 65,
        F8 = 66,
        F9 = 67,
        F10 = 68,
        F11 = 87,
        F12 = 88,
        NUMLOCK = 69,
        SCROLLLOCK = 70,
        LEFTSHIFT = 42,
        RIGHTSHIFT = 54,
        LEFTCONTROL = 29,
        RIGHTCONTROL = 99,
        LEFTALT = 56,
        RIGHTALT = 98,
        BROWSER_BACK = 106,
        BROWSER_FORWARD = 105,
        BROWSER_REFRESH = 103,
        BROWSER_STOP = 104,
        BROWSER_SEARCH = 101,
        BROWSER_FAVORITES = 102,
        BROWSER_HOME = 50,
        VOLUME_MUTE = 32,
        VOLUME_DOWN = 46,
        VOLUME_UP = 48,
        MEDIA_NEXT_TRACK = 25,
        MEDIA_PREV_TRACK = 16,
        MEDIA_STOP = 36,
        MEDIA_PLAY_PAUSE = 34,
        LAUNCH_MAIL = 108,
        LAUNCH_MEDIA_SELECT = 109,
        LAUNCH_APP1 = 107,
        LAUNCH_APP2 = 33,
        OEM_1 = 27,
        OEM_PLUS = 13,
        OEM_COMMA = 50,
        OEM_MINUS = 0,
        OEM_PERIOD = 51,
        OEM_2 = 52,
        OEM_3 = 40,
        OEM_4 = 12,
        OEM_5 = 43,
        OEM_6 = 26,
        OEM_7 = 41,
        OEM_8 = 53,
        OEM_102 = 86,
        EREOF = 93,
        ZOOM = 98,
        Escape = 1,
        One = 2,
        Two = 3,
        Three = 4,
        Four = 5,
        Five = 6,
        Six = 7,
        Seven = 8,
        Eight = 9,
        Nine = 10,
        Zero = 11,
        DashUnderscore = 12,
        PlusEquals = 13,
        Backspace = 14,
        Tab = 15,
        OpenBracketBrace = 26,
        CloseBracketBrace = 27,
        Enter = 28,
        Control = 29,
        SemicolonColon = 39,
        SingleDoubleQuote = 40,
        Tilde = 41,
        LeftShift = 42,
        BackslashPipe = 43,
        CommaLeftArrow = 51,
        PeriodRightArrow = 52,
        ForwardSlashQuestionMark = 53,
        RightShift = 54,
        RightAlt = 56,
        Space = 57,
        CapsLock = 58,
        Up = 72,
        Down = 80,
        Right = 77,
        Left = 75,
        Home = 71,
        End = 79,
        Delete = 83,
        PageUp = 73,
        PageDown = 81,
        Insert = 82,
        PrintScreen = 55,
        NumLock = 69,
        ScrollLock = 70,
        Menu = 93,
        WindowsKey = 91,
        NumpadDivide = 53,
        NumpadAsterisk = 55,
        Numpad7 = 71,
        Numpad8 = 72,
        Numpad9 = 73,
        Numpad4 = 75,
        Numpad5 = 76,
        Numpad6 = 77,
        Numpad1 = 79,
        Numpad2 = 80,
        Numpad3 = 81,
        Numpad0 = 82,
        NumpadDelete = 83,
        NumpadEnter = 28,
        NumpadPlus = 78,
        NumpadMinus = 74,
    }
    public class MousePressedEventArgs : EventArgs
    {
        public MouseState State { get; set; }
        public bool Handled { get; set; }
        public int X { get; set; }
        public int Y { get; set; }
        public short Rolling { get; set; }
    }
    public enum ScrollDirection
    {
        Down,
        Up
    }
}