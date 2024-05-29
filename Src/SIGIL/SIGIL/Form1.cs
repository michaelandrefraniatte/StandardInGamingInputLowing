using System;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Reflection;
using System.IO;
using System.Threading;
using Microsoft.Win32;
using System.Runtime.InteropServices;
using OpenWithSingleInstance;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using FastColoredTextBoxNS;
using Range = FastColoredTextBoxNS.Range;
using AutocompleteItem = AutocompleteMenuNS.AutocompleteItem;
using SnippetAutocompleteItem = AutocompleteMenuNS.SnippetAutocompleteItem;
using MethodAutocompleteItem = AutocompleteMenuNS.MethodAutocompleteItem;
using System.ServiceProcess;
using System.Linq;

namespace SIGIL
{
    public partial class Form1 : Form
    {
        [DllImport("stopitkills.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Unicode, EntryPoint = "killProcessByNames")]
        [return: MarshalAs(UnmanagedType.BStr)]
        public static extern string killProcessByNames(string processnames);
        [DllImport("user32.dll")]
        public static extern bool GetAsyncKeyState(Keys vKey);
        [DllImport("USER32.DLL")]
        public static extern int SetWindowLong(IntPtr hWnd, int nIndex, uint dwNewLong);
        [DllImport("user32.dll")]
        static extern bool DrawMenuBar(IntPtr hWnd);
        [DllImport("user32.dll", EntryPoint = "SetWindowPos")]
        public static extern IntPtr SetWindowPos(IntPtr hWnd, int hWndInsertAfter, int x, int Y, int cx, int cy, int wFlags);
        [DllImport("user32.dll")]
        static extern IntPtr GetForegroundWindow();
        [DllImport("user32.dll", SetLastError = true)]
        private static extern IntPtr FindWindow(string lpClassName, string lpWindowName);
        [DllImport("user32.dll")]
        static extern bool SetForegroundWindow(IntPtr hWnd);
        [DllImport("user32.dll")]
        private static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);
        [DllImport("winmm.dll", EntryPoint = "timeBeginPeriod")]
        private static extern uint TimeBeginPeriod(uint ms);
        [DllImport("winmm.dll", EntryPoint = "timeEndPeriod")]
        private static extern uint TimeEndPeriod(uint ms);
        [DllImport("ntdll.dll", EntryPoint = "NtSetTimerResolution")]
        private static extern void NtSetTimerResolution(uint DesiredResolution, bool SetResolution, ref uint CurrentResolution);
        private static uint CurrentResolution = 0;
        private int GWL_STYLE = -16;
        private uint WS_BORDER = 0x00800000;
        private uint WS_CAPTION = 0x00C00000;
        private uint WS_SYSMENU = 0x00080000;
        private uint WS_MINIMIZEBOX = 0x00020000;
        private uint WS_MAXIMIZEBOX = 0x00010000;
        private uint WS_OVERLAPPED = 0x00000000;
        private uint WS_POPUP = 0x80000000;
        private uint WS_TABSTOP = 0x00010000;
        private uint WS_VISIBLE = 0x10000000;
        private static int width, height;
        private static DialogResult result;
        private static ContextMenu contextMenu = new ContextMenu();
        private static MenuItem menuItem;
        private static bool justSaved = true, onopenwith = false, runstopbool = false, closeonicon = false;
        public static bool replaceformvisible = false, removewindowtitle = false, optimizewindows = false;
        private static string filename = "", fastColoredTextBoxSaved = "", code = "";
        public static ReplaceForm replaceform;
        private static Range range;
        private static Style StyleInput = new TextStyle(Brushes.Blue, null, FontStyle.Regular), StyleOutput = new TextStyle(Brushes.Orange, null, FontStyle.Regular), StyleLibrary = new TextStyle(Brushes.BlueViolet, null, FontStyle.Regular), StyleClass = new TextStyle(Brushes.DodgerBlue, null, FontStyle.Regular), StyleMethod = new TextStyle(Brushes.Magenta, null, FontStyle.Regular), StyleObject = new TextStyle(Brushes.DarkOrange, null, FontStyle.Regular), StyleExtra = new TextStyle(Brushes.Red, null, FontStyle.Regular), StyleSpecial = new TextStyle(Brushes.DarkCyan, null, FontStyle.Regular), StyleNone = new TextStyle(Brushes.Black, null, FontStyle.Regular);
        private Type program;
        private object obj;
        private Assembly assembly;
        private System.CodeDom.Compiler.CompilerResults results;
        private Microsoft.CSharp.CSharpCodeProvider provider;
        private System.CodeDom.Compiler.CompilerParameters parameters;
        private static Form2 form2 = new Form2();
        private static Form3 form3 = new Form3();
        public static int processid = 0;
        private static List<string> servBLs = new List<string>();
        private static string procnamesbl = "", servNames = "";
        private static ServiceController[] services;
        private static TimeSpan timeout = new TimeSpan(0, 0, 1);
        private bool[] wd = { false, false };
        private bool[] wu = { false, false };
        private bool[] ws = { false, false };
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
        public Form1(string filePath)
        {
            InitializeComponent();
            if (filePath != null)
            {
                onopenwith = true;
                OpenFileWith(filePath);
            }
        }
        protected override void WndProc(ref Message m)
        {
            if (m.Msg == MessageHelper.WM_COPYDATA)
            {
                COPYDATASTRUCT _dataStruct = Marshal.PtrToStructure<COPYDATASTRUCT>(m.LParam);
                string _strMsg = Marshal.PtrToStringUni(_dataStruct.lpData, _dataStruct.cbData / 2);
                OpenFileWith(_strMsg);
            }
            base.WndProc(ref m);
        }
        private void OpenFileWith(string filePath)
        {
            if (runstopbool)
                StopProcess();
            if (!justSaved)
            {
                result = MessageBox.Show("Content will be lost! Are you sure?", "Open", MessageBoxButtons.OKCancel);
                if (result == DialogResult.OK)
                {
                    fastColoredTextBox1.Clear();
                    string readText = File.ReadAllText(filePath);
                    fastColoredTextBox1.Text = readText;
                    filename = filePath;
                    this.Text = "SIGIL: " + Path.GetFileName(filename);
                    fastColoredTextBoxSaved = fastColoredTextBox1.Text;
                    justSaved = true;
                }
            }
            else
            {
                fastColoredTextBox1.Clear();
                string readText = File.ReadAllText(filePath);
                fastColoredTextBox1.Text = readText;
                filename = filePath;
                this.Text = "SIGIL: " + Path.GetFileName(filename);
                fastColoredTextBoxSaved = fastColoredTextBox1.Text;
                justSaved = true;
            }
        }
        private void ChangeScriptColors(object sender)
        {
            try
            {
                range = (sender as FastColoredTextBox).Range;
                range.SetStyle(StyleClass, new Regex(@"\bDllImport\b"));
                range.SetStyle(StyleClass, new Regex(@"\bEntryPoint\b"));
                range.SetStyle(StyleMethod, new Regex(@"\bTimeBeginPeriod\b"));
                range.SetStyle(StyleMethod, new Regex(@"\bTimeEndPeriod\b"));
                range.SetStyle(StyleMethod, new Regex(@"\bNtSetTimerResolution\b"));
                range.SetStyle(StyleClass, new Regex(@"\bList\b"));
                range.SetStyle(StyleMethod, new Regex(@"\bScan\b"));
                range.SetStyle(StyleExtra, new Regex(@"\bms\b"));
                range.SetStyle(StyleExtra, new Regex(@"\bCurrentResolution\b"));
                range.SetStyle(StyleExtra, new Regex(@"\bDesiredResolution\b"));
                range.SetStyle(StyleExtra, new Regex(@"\bSetResolution\b"));
                range.SetStyle(StyleExtra, new Regex(@"\brunning\b"));
                range.SetStyle(StyleExtra, new Regex(@"\birmode\b"));
                range.SetStyle(StyleExtra, new Regex(@"\bnumber\b"));
                range.SetStyle(StyleExtra, new Regex(@"\bcentery\b"));
                range.SetStyle(StyleExtra, new Regex(@"\bvendor_ds_id\b"));
                range.SetStyle(StyleExtra, new Regex(@"\bproduct_ds_id\b"));
                range.SetStyle(StyleExtra, new Regex(@"\bproduct_ds_label\b"));
                range.SetStyle(StyleExtra, new Regex(@"\bvendor_ds4_id\b"));
                range.SetStyle(StyleExtra, new Regex(@"\bproduct_ds4_id\b"));
                range.SetStyle(StyleExtra, new Regex(@"\bproduct_ds4_label\b"));
                range.SetStyle(StyleMethod, new Regex(@"\bStart\b"));
                range.SetStyle(StyleExtra, new Regex(@"\bstatex\b"));
                range.SetStyle(StyleExtra, new Regex(@"\bstatey\b"));
                range.SetStyle(StyleExtra, new Regex(@"\bmousex\b"));
                range.SetStyle(StyleExtra, new Regex(@"\bmousey\b"));
                range.SetStyle(StyleExtra, new Regex(@"\bmousexp\b"));
                range.SetStyle(StyleExtra, new Regex(@"\bmouseyp\b"));
                range.SetStyle(StyleExtra, new Regex(@"\bmousestatex\b"));
                range.SetStyle(StyleExtra, new Regex(@"\bmousestatey\b"));
                range.SetStyle(StyleExtra, new Regex(@"\bdzx\b"));
                range.SetStyle(StyleExtra, new Regex(@"\bdzy\b"));
                range.SetStyle(StyleExtra, new Regex(@"\bviewpower1x\b"));
                range.SetStyle(StyleExtra, new Regex(@"\bviewpower2x\b"));
                range.SetStyle(StyleExtra, new Regex(@"\bviewpower3x\b"));
                range.SetStyle(StyleExtra, new Regex(@"\bviewpower1y\b"));
                range.SetStyle(StyleExtra, new Regex(@"\bviewpower2y\b"));
                range.SetStyle(StyleExtra, new Regex(@"\bviewpower3y\b"));
                range.SetStyle(StyleExtra, new Regex(@"\bviewpower05x\b"));
                range.SetStyle(StyleExtra, new Regex(@"\bviewpower05y\b"));
                range.SetStyle(StyleMethod, new Regex(@"\btask\b"));
                range.SetStyle(StyleExtra, new Regex(@"\bsleeptime\b"));
                range.SetStyle(StyleLibrary, new Regex(@"\bTask\b"));
                range.SetStyle(StyleLibrary, new Regex(@"\bRun\b"));
                range.SetStyle(StyleSpecial, new Regex(@"\bThread\b"));
                range.SetStyle(StyleSpecial, new Regex(@"\bSleep\b"));
                range.SetStyle(StyleExtra, new Regex(@"\bStringToCode\b"));
                range.SetStyle(StyleSpecial, new Regex(@"\bFooClass\b"));
                range.SetStyle(StyleSpecial, new Regex(@"\bDllImport\b"));
                range.SetStyle(StyleLibrary, new Regex(@"\bSystem\b"));
                range.SetStyle(StyleLibrary, new Regex(@"\bGlobalization\b"));
                range.SetStyle(StyleLibrary, new Regex(@"\bIO\b"));
                range.SetStyle(StyleLibrary, new Regex(@"\bNumerics\b"));
                range.SetStyle(StyleLibrary, new Regex(@"\bDrawing\b"));
                range.SetStyle(StyleLibrary, new Regex(@"\bRuntime\b"));
                range.SetStyle(StyleLibrary, new Regex(@"\bInteropServices\b"));
                range.SetStyle(StyleLibrary, new Regex(@"\bThreading\b"));
                range.SetStyle(StyleLibrary, new Regex(@"\bTasks\b"));
                range.SetStyle(StyleLibrary, new Regex(@"\bWindows\b"));
                range.SetStyle(StyleLibrary, new Regex(@"\bForms\b"));
                range.SetStyle(StyleLibrary, new Regex(@"\bReflection\b"));
                range.SetStyle(StyleLibrary, new Regex(@"\bDiagnostics\b"));
                range.SetStyle(StyleLibrary, new Regex(@"\bCollections\b"));
                range.SetStyle(StyleLibrary, new Regex(@"\bGeneric\b"));
                range.SetStyle(StyleLibrary, new Regex(@"\bLinq\b"));
                range.SetStyle(StyleLibrary, new Regex(@"\bVectors\b"));
                range.SetStyle(StyleLibrary, new Regex(@"\bCore\b"));
                range.SetStyle(StyleLibrary, new Regex(@"\bvalListX\b"));
                range.SetStyle(StyleLibrary, new Regex(@"\bvalListY\b"));
                range.SetStyle(StyleMethod, new Regex(@"\bAdd\b"));
                range.SetStyle(StyleLibrary, new Regex(@"\bAverage\b"));
                range.SetStyle(StyleLibrary, new Regex(@"\bCount\b"));
                range.SetStyle(StyleLibrary, new Regex(@"\bClear\b"));
                range.SetStyle(StyleLibrary, new Regex(@"\bToInt32\b"));
                range.SetStyle(StyleLibrary, new Regex(@"\bRemoveAt\b"));
                range.SetStyle(StyleLibrary, new Regex(@"\bAverage\b"));
                range.SetStyle(StyleMethod, new Regex(@"\bViewData\b"));
                range.SetStyle(StyleMethod, new Regex(@"\bClose\b"));
                range.SetStyle(StyleMethod, new Regex(@"\bBeginPolling\b"));
                range.SetStyle(StyleMethod, new Regex(@"\bConnect\b"));
                range.SetStyle(StyleMethod, new Regex(@"\bDisconnect\b"));
                range.SetStyle(StyleMethod, new Regex(@"\bSet\b"));
                range.SetStyle(StyleLibrary, new Regex(@"\bValuechanges\b"));
                range.SetStyle(StyleClass, new Regex(@"\bValuechange\b"));
                range.SetStyle(StyleObject, new Regex(@"\bValueChange\b"));
                range.SetStyle(StyleMethod, new Regex(@"\b_ValueChange\b"));
                range.SetStyle(StyleLibrary, new Regex(@"\bcontrollers\b"));
                range.SetStyle(StyleClass, new Regex(@"\bXBoxController\b"));
                range.SetStyle(StyleObject, new Regex(@"\bXBC\b"));
                range.SetStyle(StyleObject, new Regex(@"\bXBC1\b"));
                range.SetStyle(StyleObject, new Regex(@"\bXBC2\b"));
                range.SetStyle(StyleLibrary, new Regex(@"\bXInputsAPI\b"));
                range.SetStyle(StyleClass, new Regex(@"\bXInput\b"));
                range.SetStyle(StyleObject, new Regex(@"\bxi\b"));
                range.SetStyle(StyleObject, new Regex(@"\bxi1\b"));
                range.SetStyle(StyleObject, new Regex(@"\bxi2\b"));
                range.SetStyle(StyleLibrary, new Regex(@"\bcontrollersds4\b"));
                range.SetStyle(StyleClass, new Regex(@"\bDS4Controller\b"));
                range.SetStyle(StyleObject, new Regex(@"\bDS4\b"));
                range.SetStyle(StyleObject, new Regex(@"\bDS41\b"));
                range.SetStyle(StyleObject, new Regex(@"\bDS42\b"));
                range.SetStyle(StyleLibrary, new Regex(@"\bcontrollersvjoy\b"));
                range.SetStyle(StyleClass, new Regex(@"\bVJoyController\b"));
                range.SetStyle(StyleObject, new Regex(@"\bVJoy\b"));
                range.SetStyle(StyleObject, new Regex(@"\bVJoy1\b"));
                range.SetStyle(StyleObject, new Regex(@"\bVJoy2\b"));
                range.SetStyle(StyleLibrary, new Regex(@"\bDirectInputsAPI\b"));
                range.SetStyle(StyleClass, new Regex(@"\bDirectInput\b"));
                range.SetStyle(StyleObject, new Regex(@"\bdi\b"));
                range.SetStyle(StyleObject, new Regex(@"\bdi1\b"));
                range.SetStyle(StyleObject, new Regex(@"\bdi2\b"));
                range.SetStyle(StyleLibrary, new Regex(@"\bDualSensesAPI\b"));
                range.SetStyle(StyleClass, new Regex(@"\bDualSense\b"));
                range.SetStyle(StyleObject, new Regex(@"\bds\b"));
                range.SetStyle(StyleObject, new Regex(@"\bds1\b"));
                range.SetStyle(StyleObject, new Regex(@"\bds2\b"));
                range.SetStyle(StyleMethod, new Regex(@"\bInit\b"));
                range.SetStyle(StyleLibrary, new Regex(@"\bDualShocks4API\b"));
                range.SetStyle(StyleClass, new Regex(@"\bDualShock4\b"));
                range.SetStyle(StyleObject, new Regex(@"\bds4\b"));
                range.SetStyle(StyleObject, new Regex(@"\bds41\b"));
                range.SetStyle(StyleObject, new Regex(@"\bds42\b"));
                range.SetStyle(StyleLibrary, new Regex(@"\bSendInputs\b"));
                range.SetStyle(StyleClass, new Regex(@"\bSendinput\b"));
                range.SetStyle(StyleObject, new Regex(@"\bsendinput\b"));
                range.SetStyle(StyleLibrary, new Regex(@"\bInterceptions\b"));
                range.SetStyle(StyleClass, new Regex(@"\bSendInterception\b"));
                range.SetStyle(StyleObject, new Regex(@"\bSI\b"));
                range.SetStyle(StyleMethod, new Regex(@"\bLoad\b"));
                range.SetStyle(StyleMethod, new Regex(@"\bMain\b"));
                range.SetStyle(StyleLibrary, new Regex(@"\bWiiMotesAPI\b"));
                range.SetStyle(StyleClass, new Regex(@"\bWiiMote\b"));
                range.SetStyle(StyleLibrary, new Regex(@"\bWiiMotesLibAPI\b"));
                range.SetStyle(StyleClass, new Regex(@"\bWiiMoteLib\b"));
                range.SetStyle(StyleLibrary, new Regex(@"\bWiiMotesLibMPAPI\b"));
                range.SetStyle(StyleClass, new Regex(@"\bWiiMoteLibMP\b"));
                range.SetStyle(StyleObject, new Regex(@"\bwm\b"));
                range.SetStyle(StyleObject, new Regex(@"\bwm1\b"));
                range.SetStyle(StyleObject, new Regex(@"\bwm2\b"));
                range.SetStyle(StyleLibrary, new Regex(@"\bSwitchProControllersAPI\b"));
                range.SetStyle(StyleClass, new Regex(@"\bSwitchProController\b"));
                range.SetStyle(StyleObject, new Regex(@"\bspc\b"));
                range.SetStyle(StyleObject, new Regex(@"\bspc1\b"));
                range.SetStyle(StyleObject, new Regex(@"\bspc2\b"));
                range.SetStyle(StyleLibrary, new Regex(@"\bJoyconChargingGripsAPI\b"));
                range.SetStyle(StyleClass, new Regex(@"\bJoyconChargingGrip\b"));
                range.SetStyle(StyleObject, new Regex(@"\bjcg\b"));
                range.SetStyle(StyleObject, new Regex(@"\bjcg1\b"));
                range.SetStyle(StyleObject, new Regex(@"\bjcg2\b"));
                range.SetStyle(StyleLibrary, new Regex(@"\bJoyconsLeftAPI\b"));
                range.SetStyle(StyleClass, new Regex(@"\bJoyconLeft\b"));
                range.SetStyle(StyleObject, new Regex(@"\bjl\b"));
                range.SetStyle(StyleObject, new Regex(@"\bjl1\b"));
                range.SetStyle(StyleObject, new Regex(@"\bjl2\b"));
                range.SetStyle(StyleLibrary, new Regex(@"\bJoyconsRightAPI\b"));
                range.SetStyle(StyleClass, new Regex(@"\bJoyconRight\b"));
                range.SetStyle(StyleObject, new Regex(@"\bjr\b"));
                range.SetStyle(StyleObject, new Regex(@"\bjr1\b"));
                range.SetStyle(StyleObject, new Regex(@"\bjr2\b"));
                range.SetStyle(StyleLibrary, new Regex(@"\bKeyboardInputsAPI\b"));
                range.SetStyle(StyleLibrary, new Regex(@"\bMouseInputsAPI\b"));
                range.SetStyle(StyleClass, new Regex(@"\bKeyboardInput\b"));
                range.SetStyle(StyleClass, new Regex(@"\bMouseInput\b"));
                range.SetStyle(StyleObject, new Regex(@"\bmi\b"));
                range.SetStyle(StyleObject, new Regex(@"\bki\b"));
                range.SetStyle(StyleObject, new Regex(@"\bmi1\b"));
                range.SetStyle(StyleObject, new Regex(@"\bki1\b"));
                range.SetStyle(StyleObject, new Regex(@"\bmi2\b"));
                range.SetStyle(StyleObject, new Regex(@"\bki2\b"));
                range.SetStyle(StyleLibrary, new Regex(@"\bkeyboardsmouses\b"));
                range.SetStyle(StyleClass, new Regex(@"\bSendKeyboardMouse\b"));
                range.SetStyle(StyleObject, new Regex(@"\bSKM\b"));
                range.SetStyle(StyleInput, new Regex(@"\bgetstate\b"));
                range.SetStyle(StyleInput, new Regex(@"\bpollcount\b"));
                range.SetStyle(StyleInput, new Regex(@"\bmin\b"));
                range.SetStyle(StyleInput, new Regex(@"\bmax\b"));
                range.SetStyle(StyleInput, new Regex(@"\bminScale\b"));
                range.SetStyle(StyleInput, new Regex(@"\bmaxScale\b"));
                range.SetStyle(StyleInput, new Regex(@"\bvalue\b"));
                range.SetStyle(StyleInput, new Regex(@"\bval\b"));
                range.SetStyle(StyleInput, new Regex(@"\bn\b"));
                range.SetStyle(StyleInput, new Regex(@"\bscaled\b"));
                range.SetStyle(StyleLibrary, new Regex(@"\bSystem.Windows.Forms.Screen.PrimaryScreen.Bounds.Width\b"));
                range.SetStyle(StyleLibrary, new Regex(@"\bSystem.Windows.Forms.Screen.PrimaryScreen.Bounds.Height\b"));
                range.SetStyle(StyleLibrary, new Regex(@"\bMath\b"));
                range.SetStyle(StyleLibrary, new Regex(@"\bAbs\b"));
                range.SetStyle(StyleLibrary, new Regex(@"\bSign\b"));
                range.SetStyle(StyleLibrary, new Regex(@"\bRound\b"));
                range.SetStyle(StyleLibrary, new Regex(@"\bPow\b"));
                range.SetStyle(StyleLibrary, new Regex(@"\bSqrt\b"));
                range.SetStyle(StyleLibrary, new Regex(@"\bLog\b"));
                range.SetStyle(StyleLibrary, new Regex(@"\bExp\b"));
                range.SetStyle(StyleLibrary, new Regex(@"\bMin\b"));
                range.SetStyle(StyleLibrary, new Regex(@"\bMax\b"));
                range.SetStyle(StyleLibrary, new Regex(@"\bFloor\b"));
                range.SetStyle(StyleLibrary, new Regex(@"\bTruncate\b"));
                range.SetStyle(StyleMethod, new Regex(@"\bwd\b"));
                range.SetStyle(StyleMethod, new Regex(@"\bwu\b"));
                range.SetStyle(StyleMethod, new Regex(@"\bvalchanged\b"));
                range.SetStyle(StyleMethod, new Regex(@"\bScale\b"));
                range.SetStyle(StyleInput, new Regex(@"\bwidth\b"));
                range.SetStyle(StyleInput, new Regex(@"\bheight\b"));
                range.SetStyle(StyleInput, new Regex(@"\bControllerButtonAPressed\b"));
                range.SetStyle(StyleInput, new Regex(@"\bControllerButtonBPressed\b"));
                range.SetStyle(StyleInput, new Regex(@"\bControllerButtonXPressed\b"));
                range.SetStyle(StyleInput, new Regex(@"\bControllerButtonYPressed\b"));
                range.SetStyle(StyleInput, new Regex(@"\bControllerButtonStartPressed\b"));
                range.SetStyle(StyleInput, new Regex(@"\bControllerButtonBackPressed\b"));
                range.SetStyle(StyleInput, new Regex(@"\bControllerButtonDownPressed\b"));
                range.SetStyle(StyleInput, new Regex(@"\bControllerButtonUpPressed\b"));
                range.SetStyle(StyleInput, new Regex(@"\bControllerButtonLeftPressed\b"));
                range.SetStyle(StyleInput, new Regex(@"\bControllerButtonRightPressed\b"));
                range.SetStyle(StyleInput, new Regex(@"\bControllerButtonShoulderLeftPressed\b"));
                range.SetStyle(StyleInput, new Regex(@"\bControllerButtonShoulderRightPressed\b"));
                range.SetStyle(StyleInput, new Regex(@"\bControllerThumbpadLeftPressed\b"));
                range.SetStyle(StyleInput, new Regex(@"\bControllerThumbpadRightPressed\b"));
                range.SetStyle(StyleInput, new Regex(@"\bControllerTriggerLeftPosition\b"));
                range.SetStyle(StyleInput, new Regex(@"\bControllerTriggerRightPosition\b"));
                range.SetStyle(StyleInput, new Regex(@"\bControllerThumbLeftX\b"));
                range.SetStyle(StyleInput, new Regex(@"\bControllerThumbLeftY\b"));
                range.SetStyle(StyleInput, new Regex(@"\bControllerThumbRightX\b"));
                range.SetStyle(StyleInput, new Regex(@"\bControllerThumbRightY\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bController_Send_xbox\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bController_Send_back\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bController_Send_start\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bController_Send_A\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bController_Send_B\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bController_Send_X\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bController_Send_Y\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bController_Send_up\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bController_Send_left\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bController_Send_down\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bController_Send_right\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bController_Send_leftstick\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bController_Send_rightstick\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bController_Send_leftbumper\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bController_Send_rightbumper\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bController_Send_lefttriggerposition\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bController_Send_righttriggerposition\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bController_Send_leftstickx\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bController_Send_leftsticky\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bController_Send_rightstickx\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bController_Send_rightsticky\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bControllerDS4_Send_Options\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bControllerDS4_Send_ThumbLeft\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bControllerDS4_Send_ThumbRight\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bControllerDS4_Send_ShoulderLeft\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bControllerDS4_Send_ShoulderRight\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bControllerDS4_Send_Cross\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bControllerDS4_Send_Circle\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bControllerDS4_Send_Square\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bControllerDS4_Send_Triangle\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bControllerDS4_Send_Ps\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bControllerDS4_Send_Touchpad\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bControllerDS4_Send_Share\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bControllerDS4_Send_DPadUp\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bControllerDS4_Send_DPadDown\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bControllerDS4_Send_DPadLeft\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bControllerDS4_Send_DPadRight\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bControllerDS4_Send_LeftTrigger\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bControllerDS4_Send_RightTrigger\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bControllerDS4_Send_LeftTriggerPosition\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bControllerDS4_Send_RightTriggerPosition\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bControllerDS4_Send_LeftThumbX\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bControllerDS4_Send_RightThumbX\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bControllerDS4_Send_LeftThumbY\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bControllerDS4_Send_RightThumbY\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bControllerVJoy_Send_1\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bControllerVJoy_Send_2\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bControllerVJoy_Send_3\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bControllerVJoy_Send_4\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bControllerVJoy_Send_5\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bControllerVJoy_Send_6\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bControllerVJoy_Send_7\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bControllerVJoy_Send_8\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bControllerVJoy_Send_X\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bControllerVJoy_Send_Y\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bControllerVJoy_Send_Z\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bControllerVJoy_Send_WHL\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bControllerVJoy_Send_SL0\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bControllerVJoy_Send_SL1\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bControllerVJoy_Send_RX\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bControllerVJoy_Send_RY\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bControllerVJoy_Send_RZ\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bControllerVJoy_Send_POV\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bControllerVJoy_Send_Hat\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bControllerVJoy_Send_HatExt1\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bControllerVJoy_Send_HatExt2\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bControllerVJoy_Send_HatExt3\b"));
                range.SetStyle(StyleInput, new Regex(@"\bJoystickAxisX\b"));
                range.SetStyle(StyleInput, new Regex(@"\bJoystickAxisY\b"));
                range.SetStyle(StyleInput, new Regex(@"\bJoystickAxisZ\b"));
                range.SetStyle(StyleInput, new Regex(@"\bJoystickRotationX\b"));
                range.SetStyle(StyleInput, new Regex(@"\bJoystickRotationY\b"));
                range.SetStyle(StyleInput, new Regex(@"\bJoystickRotationZ\b"));
                range.SetStyle(StyleInput, new Regex(@"\bJoystickSliders0\b"));
                range.SetStyle(StyleInput, new Regex(@"\bJoystickSliders1\b"));
                range.SetStyle(StyleInput, new Regex(@"\bJoystickPointOfViewControllers0\b"));
                range.SetStyle(StyleInput, new Regex(@"\bJoystickPointOfViewControllers1\b"));
                range.SetStyle(StyleInput, new Regex(@"\bJoystickPointOfViewControllers2\b"));
                range.SetStyle(StyleInput, new Regex(@"\bJoystickPointOfViewControllers3\b"));
                range.SetStyle(StyleInput, new Regex(@"\bJoystickVelocityX\b"));
                range.SetStyle(StyleInput, new Regex(@"\bJoystickVelocityY\b"));
                range.SetStyle(StyleInput, new Regex(@"\bJoystickVelocityZ\b"));
                range.SetStyle(StyleInput, new Regex(@"\bJoystickAngularVelocityX\b"));
                range.SetStyle(StyleInput, new Regex(@"\bJoystickAngularVelocityY\b"));
                range.SetStyle(StyleInput, new Regex(@"\bJoystickAngularVelocityZ\b"));
                range.SetStyle(StyleInput, new Regex(@"\bJoystickVelocitySliders0\b"));
                range.SetStyle(StyleInput, new Regex(@"\bJoystickVelocitySliders1\b"));
                range.SetStyle(StyleInput, new Regex(@"\bJoystickAccelerationX\b"));
                range.SetStyle(StyleInput, new Regex(@"\bJoystickAccelerationY\b"));
                range.SetStyle(StyleInput, new Regex(@"\bJoystickAccelerationZ\b"));
                range.SetStyle(StyleInput, new Regex(@"\bJoystickAngularAccelerationX\b"));
                range.SetStyle(StyleInput, new Regex(@"\bJoystickAngularAccelerationY\b"));
                range.SetStyle(StyleInput, new Regex(@"\bJoystickAngularAccelerationZ\b"));
                range.SetStyle(StyleInput, new Regex(@"\bJoystickAccelerationSliders0\b"));
                range.SetStyle(StyleInput, new Regex(@"\bJoystickAccelerationSliders1\b"));
                range.SetStyle(StyleInput, new Regex(@"\bJoystickForceX\b"));
                range.SetStyle(StyleInput, new Regex(@"\bJoystickForceY\b"));
                range.SetStyle(StyleInput, new Regex(@"\bJoystickForceZ\b"));
                range.SetStyle(StyleInput, new Regex(@"\bJoystickTorqueX\b"));
                range.SetStyle(StyleInput, new Regex(@"\bJoystickTorqueY\b"));
                range.SetStyle(StyleInput, new Regex(@"\bJoystickTorqueZ\b"));
                range.SetStyle(StyleInput, new Regex(@"\bJoystickForceSliders0\b"));
                range.SetStyle(StyleInput, new Regex(@"\bJoystickForceSliders1\b"));
                range.SetStyle(StyleInput, new Regex(@"\bJoystickButtons0\b"));
                range.SetStyle(StyleInput, new Regex(@"\bJoystickButtons1\b"));
                range.SetStyle(StyleInput, new Regex(@"\bJoystickButtons2\b"));
                range.SetStyle(StyleInput, new Regex(@"\bJoystickButtons3\b"));
                range.SetStyle(StyleInput, new Regex(@"\bJoystickButtons4\b"));
                range.SetStyle(StyleInput, new Regex(@"\bJoystickButtons5\b"));
                range.SetStyle(StyleInput, new Regex(@"\bJoystickButtons6\b"));
                range.SetStyle(StyleInput, new Regex(@"\bJoystickButtons7\b"));
                range.SetStyle(StyleInput, new Regex(@"\bJoystickButtons8\b"));
                range.SetStyle(StyleInput, new Regex(@"\bJoystickButtons9\b"));
                range.SetStyle(StyleInput, new Regex(@"\bJoystickButtons10\b"));
                range.SetStyle(StyleInput, new Regex(@"\bJoystickButtons11\b"));
                range.SetStyle(StyleInput, new Regex(@"\bJoystickButtons12\b"));
                range.SetStyle(StyleInput, new Regex(@"\bJoystickButtons13\b"));
                range.SetStyle(StyleInput, new Regex(@"\bJoystickButtons14\b"));
                range.SetStyle(StyleInput, new Regex(@"\bJoystickButtons15\b"));
                range.SetStyle(StyleInput, new Regex(@"\bJoystickButtons16\b"));
                range.SetStyle(StyleInput, new Regex(@"\bJoystickButtons17\b"));
                range.SetStyle(StyleInput, new Regex(@"\bJoystickButtons18\b"));
                range.SetStyle(StyleInput, new Regex(@"\bJoystickButtons19\b"));
                range.SetStyle(StyleInput, new Regex(@"\bJoystickButtons20\b"));
                range.SetStyle(StyleInput, new Regex(@"\bJoystickButtons21\b"));
                range.SetStyle(StyleInput, new Regex(@"\bJoystickButtons22\b"));
                range.SetStyle(StyleInput, new Regex(@"\bJoystickButtons23\b"));
                range.SetStyle(StyleInput, new Regex(@"\bJoystickButtons24\b"));
                range.SetStyle(StyleInput, new Regex(@"\bJoystickButtons25\b"));
                range.SetStyle(StyleInput, new Regex(@"\bJoystickButtons26\b"));
                range.SetStyle(StyleInput, new Regex(@"\bJoystickButtons27\b"));
                range.SetStyle(StyleInput, new Regex(@"\bJoystickButtons28\b"));
                range.SetStyle(StyleInput, new Regex(@"\bJoystickButtons29\b"));
                range.SetStyle(StyleInput, new Regex(@"\bJoystickButtons30\b"));
                range.SetStyle(StyleInput, new Regex(@"\bJoystickButtons31\b"));
                range.SetStyle(StyleInput, new Regex(@"\bJoystickButtons32\b"));
                range.SetStyle(StyleInput, new Regex(@"\bJoystickButtons33\b"));
                range.SetStyle(StyleInput, new Regex(@"\bJoystickButtons34\b"));
                range.SetStyle(StyleInput, new Regex(@"\bJoystickButtons35\b"));
                range.SetStyle(StyleInput, new Regex(@"\bJoystickButtons36\b"));
                range.SetStyle(StyleInput, new Regex(@"\bJoystickButtons37\b"));
                range.SetStyle(StyleInput, new Regex(@"\bJoystickButtons38\b"));
                range.SetStyle(StyleInput, new Regex(@"\bJoystickButtons39\b"));
                range.SetStyle(StyleInput, new Regex(@"\bJoystickButtons40\b"));
                range.SetStyle(StyleInput, new Regex(@"\bJoystickButtons41\b"));
                range.SetStyle(StyleInput, new Regex(@"\bJoystickButtons42\b"));
                range.SetStyle(StyleInput, new Regex(@"\bJoystickButtons43\b"));
                range.SetStyle(StyleInput, new Regex(@"\bJoystickButtons44\b"));
                range.SetStyle(StyleInput, new Regex(@"\bJoystickButtons45\b"));
                range.SetStyle(StyleInput, new Regex(@"\bJoystickButtons46\b"));
                range.SetStyle(StyleInput, new Regex(@"\bJoystickButtons47\b"));
                range.SetStyle(StyleInput, new Regex(@"\bJoystickButtons48\b"));
                range.SetStyle(StyleInput, new Regex(@"\bJoystickButtons49\b"));
                range.SetStyle(StyleInput, new Regex(@"\bJoystickButtons50\b"));
                range.SetStyle(StyleInput, new Regex(@"\bJoystickButtons51\b"));
                range.SetStyle(StyleInput, new Regex(@"\bJoystickButtons52\b"));
                range.SetStyle(StyleInput, new Regex(@"\bJoystickButtons53\b"));
                range.SetStyle(StyleInput, new Regex(@"\bJoystickButtons54\b"));
                range.SetStyle(StyleInput, new Regex(@"\bJoystickButtons55\b"));
                range.SetStyle(StyleInput, new Regex(@"\bJoystickButtons56\b"));
                range.SetStyle(StyleInput, new Regex(@"\bJoystickButtons57\b"));
                range.SetStyle(StyleInput, new Regex(@"\bJoystickButtons58\b"));
                range.SetStyle(StyleInput, new Regex(@"\bJoystickButtons59\b"));
                range.SetStyle(StyleInput, new Regex(@"\bJoystickButtons60\b"));
                range.SetStyle(StyleInput, new Regex(@"\bJoystickButtons61\b"));
                range.SetStyle(StyleInput, new Regex(@"\bJoystickButtons62\b"));
                range.SetStyle(StyleInput, new Regex(@"\bJoystickButtons63\b"));
                range.SetStyle(StyleInput, new Regex(@"\bJoystickButtons64\b"));
                range.SetStyle(StyleInput, new Regex(@"\bJoystickButtons65\b"));
                range.SetStyle(StyleInput, new Regex(@"\bJoystickButtons66\b"));
                range.SetStyle(StyleInput, new Regex(@"\bJoystickButtons67\b"));
                range.SetStyle(StyleInput, new Regex(@"\bJoystickButtons68\b"));
                range.SetStyle(StyleInput, new Regex(@"\bJoystickButtons69\b"));
                range.SetStyle(StyleInput, new Regex(@"\bJoystickButtons70\b"));
                range.SetStyle(StyleInput, new Regex(@"\bJoystickButtons71\b"));
                range.SetStyle(StyleInput, new Regex(@"\bJoystickButtons72\b"));
                range.SetStyle(StyleInput, new Regex(@"\bJoystickButtons73\b"));
                range.SetStyle(StyleInput, new Regex(@"\bJoystickButtons74\b"));
                range.SetStyle(StyleInput, new Regex(@"\bJoystickButtons75\b"));
                range.SetStyle(StyleInput, new Regex(@"\bJoystickButtons76\b"));
                range.SetStyle(StyleInput, new Regex(@"\bJoystickButtons77\b"));
                range.SetStyle(StyleInput, new Regex(@"\bJoystickButtons78\b"));
                range.SetStyle(StyleInput, new Regex(@"\bJoystickButtons79\b"));
                range.SetStyle(StyleInput, new Regex(@"\bJoystickButtons80\b"));
                range.SetStyle(StyleInput, new Regex(@"\bJoystickButtons81\b"));
                range.SetStyle(StyleInput, new Regex(@"\bJoystickButtons82\b"));
                range.SetStyle(StyleInput, new Regex(@"\bJoystickButtons83\b"));
                range.SetStyle(StyleInput, new Regex(@"\bJoystickButtons84\b"));
                range.SetStyle(StyleInput, new Regex(@"\bJoystickButtons85\b"));
                range.SetStyle(StyleInput, new Regex(@"\bJoystickButtons86\b"));
                range.SetStyle(StyleInput, new Regex(@"\bJoystickButtons87\b"));
                range.SetStyle(StyleInput, new Regex(@"\bJoystickButtons88\b"));
                range.SetStyle(StyleInput, new Regex(@"\bJoystickButtons89\b"));
                range.SetStyle(StyleInput, new Regex(@"\bJoystickButtons90\b"));
                range.SetStyle(StyleInput, new Regex(@"\bJoystickButtons91\b"));
                range.SetStyle(StyleInput, new Regex(@"\bJoystickButtons92\b"));
                range.SetStyle(StyleInput, new Regex(@"\bJoystickButtons93\b"));
                range.SetStyle(StyleInput, new Regex(@"\bJoystickButtons94\b"));
                range.SetStyle(StyleInput, new Regex(@"\bJoystickButtons95\b"));
                range.SetStyle(StyleInput, new Regex(@"\bJoystickButtons96\b"));
                range.SetStyle(StyleInput, new Regex(@"\bJoystickButtons97\b"));
                range.SetStyle(StyleInput, new Regex(@"\bJoystickButtons98\b"));
                range.SetStyle(StyleInput, new Regex(@"\bJoystickButtons99\b"));
                range.SetStyle(StyleInput, new Regex(@"\bJoystickButtons100\b"));
                range.SetStyle(StyleInput, new Regex(@"\bJoystickButtons101\b"));
                range.SetStyle(StyleInput, new Regex(@"\bJoystickButtons102\b"));
                range.SetStyle(StyleInput, new Regex(@"\bJoystickButtons103\b"));
                range.SetStyle(StyleInput, new Regex(@"\bJoystickButtons104\b"));
                range.SetStyle(StyleInput, new Regex(@"\bJoystickButtons105\b"));
                range.SetStyle(StyleInput, new Regex(@"\bJoystickButtons106\b"));
                range.SetStyle(StyleInput, new Regex(@"\bJoystickButtons107\b"));
                range.SetStyle(StyleInput, new Regex(@"\bJoystickButtons108\b"));
                range.SetStyle(StyleInput, new Regex(@"\bJoystickButtons109\b"));
                range.SetStyle(StyleInput, new Regex(@"\bJoystickButtons110\b"));
                range.SetStyle(StyleInput, new Regex(@"\bJoystickButtons111\b"));
                range.SetStyle(StyleInput, new Regex(@"\bJoystickButtons112\b"));
                range.SetStyle(StyleInput, new Regex(@"\bJoystickButtons113\b"));
                range.SetStyle(StyleInput, new Regex(@"\bJoystickButtons114\b"));
                range.SetStyle(StyleInput, new Regex(@"\bJoystickButtons115\b"));
                range.SetStyle(StyleInput, new Regex(@"\bJoystickButtons116\b"));
                range.SetStyle(StyleInput, new Regex(@"\bJoystickButtons117\b"));
                range.SetStyle(StyleInput, new Regex(@"\bJoystickButtons118\b"));
                range.SetStyle(StyleInput, new Regex(@"\bJoystickButtons119\b"));
                range.SetStyle(StyleInput, new Regex(@"\bJoystickButtons120\b"));
                range.SetStyle(StyleInput, new Regex(@"\bJoystickButtons121\b"));
                range.SetStyle(StyleInput, new Regex(@"\bJoystickButtons122\b"));
                range.SetStyle(StyleInput, new Regex(@"\bJoystickButtons123\b"));
                range.SetStyle(StyleInput, new Regex(@"\bJoystickButtons124\b"));
                range.SetStyle(StyleInput, new Regex(@"\bJoystickButtons125\b"));
                range.SetStyle(StyleInput, new Regex(@"\bJoystickButtons126\b"));
                range.SetStyle(StyleInput, new Regex(@"\bJoystickButtons127\b"));
                range.SetStyle(StyleInput, new Regex(@"\bPS5ControllerLeftStickX\b"));
                range.SetStyle(StyleInput, new Regex(@"\bPS5ControllerLeftStickY\b"));
                range.SetStyle(StyleInput, new Regex(@"\bPS5ControllerRightStickX\b"));
                range.SetStyle(StyleInput, new Regex(@"\bPS5ControllerRightStickY\b"));
                range.SetStyle(StyleInput, new Regex(@"\bPS5ControllerLeftTriggerPosition\b"));
                range.SetStyle(StyleInput, new Regex(@"\bPS5ControllerRightTriggerPosition\b"));
                range.SetStyle(StyleInput, new Regex(@"\bPS5ControllerTouchX\b"));
                range.SetStyle(StyleInput, new Regex(@"\bPS5ControllerTouchY\b"));
                range.SetStyle(StyleInput, new Regex(@"\bPS5ControllerTouchOn\b"));
                range.SetStyle(StyleInput, new Regex(@"\bPS5ControllerGyroX\b"));
                range.SetStyle(StyleInput, new Regex(@"\bPS5ControllerGyroY\b"));
                range.SetStyle(StyleInput, new Regex(@"\bPS5ControllerAccelX\b"));
                range.SetStyle(StyleInput, new Regex(@"\bPS5ControllerAccelY\b"));
                range.SetStyle(StyleInput, new Regex(@"\bPS5ControllerButtonCrossPressed\b"));
                range.SetStyle(StyleInput, new Regex(@"\bPS5ControllerButtonCirclePressed\b"));
                range.SetStyle(StyleInput, new Regex(@"\bPS5ControllerButtonSquarePressed\b"));
                range.SetStyle(StyleInput, new Regex(@"\bPS5ControllerButtonTrianglePressed\b"));
                range.SetStyle(StyleInput, new Regex(@"\bPS5ControllerButtonDPadUpPressed\b"));
                range.SetStyle(StyleInput, new Regex(@"\bPS5ControllerButtonDPadRightPressed\b"));
                range.SetStyle(StyleInput, new Regex(@"\bPS5ControllerButtonDPadDownPressed\b"));
                range.SetStyle(StyleInput, new Regex(@"\bPS5ControllerButtonDPadLeftPressed\b"));
                range.SetStyle(StyleInput, new Regex(@"\bPS5ControllerButtonL1Pressed\b"));
                range.SetStyle(StyleInput, new Regex(@"\bPS5ControllerButtonR1Pressed\b"));
                range.SetStyle(StyleInput, new Regex(@"\bPS5ControllerButtonL2Pressed\b"));
                range.SetStyle(StyleInput, new Regex(@"\bPS5ControllerButtonR2Pressed\b"));
                range.SetStyle(StyleInput, new Regex(@"\bPS5ControllerButtonL3Pressed\b"));
                range.SetStyle(StyleInput, new Regex(@"\bPS5ControllerButtonR3Pressed\b"));
                range.SetStyle(StyleInput, new Regex(@"\bPS5ControllerButtonCreatePressed\b"));
                range.SetStyle(StyleInput, new Regex(@"\bPS5ControllerButtonMenuPressed\b"));
                range.SetStyle(StyleInput, new Regex(@"\bPS5ControllerButtonLogoPressed\b"));
                range.SetStyle(StyleInput, new Regex(@"\bPS5ControllerButtonTouchpadPressed\b"));
                range.SetStyle(StyleInput, new Regex(@"\bPS5ControllerButtonFnLPressed\b"));
                range.SetStyle(StyleInput, new Regex(@"\bPS5ControllerButtonFnRPressed\b"));
                range.SetStyle(StyleInput, new Regex(@"\bPS5ControllerButtonBLPPressed\b"));
                range.SetStyle(StyleInput, new Regex(@"\bPS5ControllerButtonBRPPressed\b"));
                range.SetStyle(StyleInput, new Regex(@"\bPS5ControllerButtonMicPressed\b"));
                range.SetStyle(StyleInput, new Regex(@"\bPS4ControllerLeftStickX\b"));
                range.SetStyle(StyleInput, new Regex(@"\bPS4ControllerLeftStickY\b"));
                range.SetStyle(StyleInput, new Regex(@"\bPS4ControllerRightStickX\b"));
                range.SetStyle(StyleInput, new Regex(@"\bPS4ControllerRightStickY\b"));
                range.SetStyle(StyleInput, new Regex(@"\bPS4ControllerLeftTriggerPosition\b"));
                range.SetStyle(StyleInput, new Regex(@"\bPS4ControllerRightTriggerPosition\b"));
                range.SetStyle(StyleInput, new Regex(@"\bPS4ControllerTouchX\b"));
                range.SetStyle(StyleInput, new Regex(@"\bPS4ControllerTouchY\b"));
                range.SetStyle(StyleInput, new Regex(@"\bPS4ControllerTouchOn\b"));
                range.SetStyle(StyleInput, new Regex(@"\bPS4ControllerGyroX\b"));
                range.SetStyle(StyleInput, new Regex(@"\bPS4ControllerGyroY\b"));
                range.SetStyle(StyleInput, new Regex(@"\bPS4ControllerAccelX\b"));
                range.SetStyle(StyleInput, new Regex(@"\bPS4ControllerAccelY\b"));
                range.SetStyle(StyleInput, new Regex(@"\bPS4ControllerButtonCrossPressed\b"));
                range.SetStyle(StyleInput, new Regex(@"\bPS4ControllerButtonCirclePressed\b"));
                range.SetStyle(StyleInput, new Regex(@"\bPS4ControllerButtonSquarePressed\b"));
                range.SetStyle(StyleInput, new Regex(@"\bPS4ControllerButtonTrianglePressed\b"));
                range.SetStyle(StyleInput, new Regex(@"\bPS4ControllerButtonDPadUpPressed\b"));
                range.SetStyle(StyleInput, new Regex(@"\bPS4ControllerButtonDPadRightPressed\b"));
                range.SetStyle(StyleInput, new Regex(@"\bPS4ControllerButtonDPadDownPressed\b"));
                range.SetStyle(StyleInput, new Regex(@"\bPS4ControllerButtonDPadLeftPressed\b"));
                range.SetStyle(StyleInput, new Regex(@"\bPS4ControllerButtonL1Pressed\b"));
                range.SetStyle(StyleInput, new Regex(@"\bPS4ControllerButtonR1Pressed\b"));
                range.SetStyle(StyleInput, new Regex(@"\bPS4ControllerButtonL2Pressed\b"));
                range.SetStyle(StyleInput, new Regex(@"\bPS4ControllerButtonR2Pressed\b"));
                range.SetStyle(StyleInput, new Regex(@"\bPS4ControllerButtonL3Pressed\b"));
                range.SetStyle(StyleInput, new Regex(@"\bPS4ControllerButtonR3Pressed\b"));
                range.SetStyle(StyleInput, new Regex(@"\bPS4ControllerButtonCreatePressed\b"));
                range.SetStyle(StyleInput, new Regex(@"\bPS4ControllerButtonMenuPressed\b"));
                range.SetStyle(StyleInput, new Regex(@"\bPS4ControllerButtonLogoPressed\b"));
                range.SetStyle(StyleInput, new Regex(@"\bPS4ControllerButtonTouchpadPressed\b"));
                range.SetStyle(StyleInput, new Regex(@"\bPS4ControllerButtonMicPressed\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bkeyboard_1_id\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bmouse_1_id\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bkeyboard_2_id\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bmouse_2_id\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bint_deltaX\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bint_deltaY\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bint_x\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bint_y\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bint_SendLeftClick\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bint_SendRightClick\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bint_SendMiddleClick\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bint_SendWheelUp\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bint_SendWheelDown\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bint_SendCANCEL\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bint_SendBACK\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bint_SendTAB\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bint_SendCLEAR\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bint_SendRETURN\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bint_SendSHIFT\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bint_SendCONTROL\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bint_SendMENU\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bint_SendCAPITAL\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bint_SendESCAPE\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bint_SendSPACE\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bint_SendPRIOR\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bint_SendNEXT\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bint_SendEND\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bint_SendHOME\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bint_SendLEFT\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bint_SendUP\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bint_SendRIGHT\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bint_SendDOWN\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bint_SendSNAPSHOT\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bint_SendINSERT\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bint_SendNUMPADDEL\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bint_SendNUMPADINSERT\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bint_SendHELP\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bint_SendAPOSTROPHE\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bint_SendBACKSPACE\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bint_SendPAGEDOWN\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bint_SendPAGEUP\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bint_SendFIN\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bint_SendMOUSE\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bint_SendA\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bint_SendB\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bint_SendC\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bint_SendD\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bint_SendE\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bint_SendF\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bint_SendG\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bint_SendH\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bint_SendI\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bint_SendJ\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bint_SendK\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bint_SendL\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bint_SendM\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bint_SendN\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bint_SendO\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bint_SendP\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bint_SendQ\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bint_SendR\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bint_SendS\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bint_SendT\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bint_SendU\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bint_SendV\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bint_SendW\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bint_SendX\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bint_SendY\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bint_SendZ\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bint_SendLWIN\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bint_SendRWIN\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bint_SendAPPS\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bint_SendDELETE\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bint_SendNUMPAD0\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bint_SendNUMPAD1\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bint_SendNUMPAD2\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bint_SendNUMPAD3\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bint_SendNUMPAD4\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bint_SendNUMPAD5\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bint_SendNUMPAD6\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bint_SendNUMPAD7\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bint_SendNUMPAD8\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bint_SendNUMPAD9\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bint_SendMULTIPLY\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bint_SendADD\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bint_SendSUBTRACT\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bint_SendDECIMAL\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bint_SendPRINTSCREEN\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bint_SendDIVIDE\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bint_SendF1\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bint_SendF2\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bint_SendF3\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bint_SendF4\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bint_SendF5\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bint_SendF6\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bint_SendF7\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bint_SendF8\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bint_SendF9\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bint_SendF10\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bint_SendF11\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bint_SendF12\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bint_SendNUMLOCK\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bint_SendSCROLLLOCK\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bint_SendLEFTSHIFT\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bint_SendRIGHTSHIFT\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bint_SendLEFTCONTROL\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bint_SendRIGHTCONTROL\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bint_SendLEFTALT\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bint_SendRIGHTALT\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bint_SendBROWSER_BACK\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bint_SendBROWSER_FORWARD\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bint_SendBROWSER_REFRESH\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bint_SendBROWSER_STOP\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bint_SendBROWSER_SEARCH\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bint_SendBROWSER_FAVORITES\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bint_SendBROWSER_HOME\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bint_SendVOLUME_MUTE\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bint_SendVOLUME_DOWN\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bint_SendVOLUME_UP\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bint_SendMEDIA_NEXT_TRACK\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bint_SendMEDIA_PREV_TRACK\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bint_SendMEDIA_STOP\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bint_SendMEDIA_PLAY_PAUSE\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bint_SendLAUNCH_MAIL\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bint_SendLAUNCH_MEDIA_SELECT\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bint_SendLAUNCH_APP1\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bint_SendLAUNCH_APP2\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bint_SendOEM_1\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bint_SendOEM_PLUS\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bint_SendOEM_COMMA\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bint_SendOEM_MINUS\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bint_SendOEM_PERIOD\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bint_SendOEM_2\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bint_SendOEM_3\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bint_SendOEM_4\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bint_SendOEM_5\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bint_SendOEM_6\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bint_SendOEM_7\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bint_SendOEM_8\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bint_SendOEM_102\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bint_SendEREOF\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bint_SendZOOM\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bint_SendEscape\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bint_SendOne\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bint_SendTwo\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bint_SendThree\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bint_SendFour\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bint_SendFive\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bint_SendSix\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bint_SendSeven\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bint_SendEight\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bint_SendNine\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bint_SendZero\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bint_SendDashUnderscore\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bint_SendPlusEquals\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bint_SendBackspace\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bint_SendTab\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bint_SendOpenBracketBrace\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bint_SendCloseBracketBrace\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bint_SendEnter\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bint_SendControl\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bint_SendSemicolonColon\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bint_SendSingleDoubleQuote\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bint_SendTilde\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bint_SendLeftShift\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bint_SendBackslashPipe\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bint_SendCommaLeftArrow\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bint_SendPeriodRightArrow\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bint_SendForwardSlashQuestionMark\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bint_SendRightShift\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bint_SendRightAlt\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bint_SendSpace\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bint_SendCapsLock\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bint_SendUp\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bint_SendDown\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bint_SendRight\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bint_SendLeft\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bint_SendHome\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bint_SendEnd\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bint_SendDelete\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bint_SendPageUp\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bint_SendPageDown\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bint_SendInsert\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bint_SendPrintScreen\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bint_SendNumLock\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bint_SendScrollLock\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bint_SendMenu\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bint_SendWindowsKey\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bint_SendNumpadDivide\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bint_SendNumpadAsterisk\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bint_SendNumpad7\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bint_SendNumpad8\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bint_SendNumpad9\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bint_SendNumpad4\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bint_SendNumpad5\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bint_SendNumpad6\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bint_SendNumpad1\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bint_SendNumpad2\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bint_SendNumpad3\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bint_SendNumpad0\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bint_SendNumpadDelete\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bint_SendNumpadEnter\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bint_SendNumpadPlus\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bint_SendNumpadMinus\b"));
                range.SetStyle(StyleInput, new Regex(@"\birx\b"));
                range.SetStyle(StyleInput, new Regex(@"\biry\b"));
                range.SetStyle(StyleInput, new Regex(@"\bWiimoteButtonStateA\b"));
                range.SetStyle(StyleInput, new Regex(@"\bWiimoteButtonStateB\b"));
                range.SetStyle(StyleInput, new Regex(@"\bWiimoteButtonStateMinus\b"));
                range.SetStyle(StyleInput, new Regex(@"\bWiimoteButtonStateHome\b"));
                range.SetStyle(StyleInput, new Regex(@"\bWiimoteButtonStatePlus\b"));
                range.SetStyle(StyleInput, new Regex(@"\bWiimoteButtonStateOne\b"));
                range.SetStyle(StyleInput, new Regex(@"\bWiimoteButtonStateTwo\b"));
                range.SetStyle(StyleInput, new Regex(@"\bWiimoteButtonStateUp\b"));
                range.SetStyle(StyleInput, new Regex(@"\bWiimoteButtonStateDown\b"));
                range.SetStyle(StyleInput, new Regex(@"\bWiimoteButtonStateLeft\b"));
                range.SetStyle(StyleInput, new Regex(@"\bWiimoteButtonStateRight\b"));
                range.SetStyle(StyleInput, new Regex(@"\bWiimoteRawValuesX\b"));
                range.SetStyle(StyleInput, new Regex(@"\bWiimoteRawValuesY\b"));
                range.SetStyle(StyleInput, new Regex(@"\bWiimoteRawValuesZ\b"));
                range.SetStyle(StyleInput, new Regex(@"\bWiimoteNunchuckStateRawJoystickX\b"));
                range.SetStyle(StyleInput, new Regex(@"\bWiimoteNunchuckStateRawJoystickY\b"));
                range.SetStyle(StyleInput, new Regex(@"\bWiimoteNunchuckStateRawValuesX\b"));
                range.SetStyle(StyleInput, new Regex(@"\bWiimoteNunchuckStateRawValuesY\b"));
                range.SetStyle(StyleInput, new Regex(@"\bWiimoteNunchuckStateRawValuesZ\b"));
                range.SetStyle(StyleInput, new Regex(@"\bWiimoteNunchuckStateC\b"));
                range.SetStyle(StyleInput, new Regex(@"\bWiimoteNunchuckStateZ\b"));
                range.SetStyle(StyleInput, new Regex(@"\bJoyconRightStickX\b"));
                range.SetStyle(StyleInput, new Regex(@"\bJoyconRightStickY\b"));
                range.SetStyle(StyleInput, new Regex(@"\bJoyconRightButtonSHOULDER_1\b"));
                range.SetStyle(StyleInput, new Regex(@"\bJoyconRightButtonSHOULDER_2\b"));
                range.SetStyle(StyleInput, new Regex(@"\bJoyconRightButtonSR\b"));
                range.SetStyle(StyleInput, new Regex(@"\bJoyconRightButtonSL\b"));
                range.SetStyle(StyleInput, new Regex(@"\bJoyconRightButtonDPAD_DOWN\b"));
                range.SetStyle(StyleInput, new Regex(@"\bJoyconRightButtonDPAD_RIGHT\b"));
                range.SetStyle(StyleInput, new Regex(@"\bJoyconRightButtonDPAD_UP\b"));
                range.SetStyle(StyleInput, new Regex(@"\bJoyconRightButtonDPAD_LEFT\b"));
                range.SetStyle(StyleInput, new Regex(@"\bJoyconRightButtonPLUS\b"));
                range.SetStyle(StyleInput, new Regex(@"\bJoyconRightButtonHOME\b"));
                range.SetStyle(StyleInput, new Regex(@"\bJoyconRightButtonSTICK\b"));
                range.SetStyle(StyleInput, new Regex(@"\bJoyconRightButtonACC\b"));
                range.SetStyle(StyleInput, new Regex(@"\bJoyconRightButtonSPA\b"));
                range.SetStyle(StyleInput, new Regex(@"\bJoyconRightRollLeft\b"));
                range.SetStyle(StyleInput, new Regex(@"\bJoyconRightRollRight\b"));
                range.SetStyle(StyleInput, new Regex(@"\bJoyconRightAccelX\b"));
                range.SetStyle(StyleInput, new Regex(@"\bJoyconRightAccelY\b"));
                range.SetStyle(StyleInput, new Regex(@"\bJoyconRightGyroX\b"));
                range.SetStyle(StyleInput, new Regex(@"\bJoyconRightGyroY\b"));
                range.SetStyle(StyleInput, new Regex(@"\bJoyconLeftStickX\b"));
                range.SetStyle(StyleInput, new Regex(@"\bJoyconLeftStickY\b"));
                range.SetStyle(StyleInput, new Regex(@"\bJoyconLeftButtonSHOULDER_1\b"));
                range.SetStyle(StyleInput, new Regex(@"\bJoyconLeftButtonSHOULDER_2\b"));
                range.SetStyle(StyleInput, new Regex(@"\bJoyconLeftButtonSR\b"));
                range.SetStyle(StyleInput, new Regex(@"\bJoyconLeftButtonSL\b"));
                range.SetStyle(StyleInput, new Regex(@"\bJoyconLeftButtonDPAD_DOWN\b"));
                range.SetStyle(StyleInput, new Regex(@"\bJoyconLeftButtonDPAD_RIGHT\b"));
                range.SetStyle(StyleInput, new Regex(@"\bJoyconLeftButtonDPAD_UP\b"));
                range.SetStyle(StyleInput, new Regex(@"\bJoyconLeftButtonDPAD_LEFT\b"));
                range.SetStyle(StyleInput, new Regex(@"\bJoyconLeftButtonMINUS\b"));
                range.SetStyle(StyleInput, new Regex(@"\bJoyconLeftButtonCAPTURE\b"));
                range.SetStyle(StyleInput, new Regex(@"\bJoyconLeftButtonSTICK\b"));
                range.SetStyle(StyleInput, new Regex(@"\bJoyconLeftButtonACC\b"));
                range.SetStyle(StyleInput, new Regex(@"\bJoyconLeftButtonSMA\b"));
                range.SetStyle(StyleInput, new Regex(@"\bJoyconLeftRollLeft\b"));
                range.SetStyle(StyleInput, new Regex(@"\bJoyconLeftRollRight\b"));
                range.SetStyle(StyleInput, new Regex(@"\bJoyconLeftAccelX\b"));
                range.SetStyle(StyleInput, new Regex(@"\bJoyconLeftAccelY\b"));
                range.SetStyle(StyleInput, new Regex(@"\bJoyconLeftGyroX\b"));
                range.SetStyle(StyleInput, new Regex(@"\bJoyconLeftGyroY\b"));
                range.SetStyle(StyleInput, new Regex(@"\bProControllerLeftStickX\b"));
                range.SetStyle(StyleInput, new Regex(@"\bProControllerLeftStickY\b"));
                range.SetStyle(StyleInput, new Regex(@"\bProControllerRightStickX\b"));
                range.SetStyle(StyleInput, new Regex(@"\bProControllerRightStickY\b"));
                range.SetStyle(StyleInput, new Regex(@"\bProControllerButtonSHOULDER_Left_1\b"));
                range.SetStyle(StyleInput, new Regex(@"\bProControllerButtonSHOULDER_Left_2\b"));
                range.SetStyle(StyleInput, new Regex(@"\bProControllerButtonDPAD_DOWN\b"));
                range.SetStyle(StyleInput, new Regex(@"\bProControllerButtonDPAD_RIGHT\b"));
                range.SetStyle(StyleInput, new Regex(@"\bProControllerButtonDPAD_UP\b"));
                range.SetStyle(StyleInput, new Regex(@"\bProControllerButtonDPAD_LEFT\b"));
                range.SetStyle(StyleInput, new Regex(@"\bProControllerButtonMINUS\b"));
                range.SetStyle(StyleInput, new Regex(@"\bProControllerButtonCAPTURE\b"));
                range.SetStyle(StyleInput, new Regex(@"\bProControllerButtonSTICK_Left\b"));
                range.SetStyle(StyleInput, new Regex(@"\bProControllerButtonSHOULDER_Right_1\b"));
                range.SetStyle(StyleInput, new Regex(@"\bProControllerButtonSHOULDER_Right_2\b"));
                range.SetStyle(StyleInput, new Regex(@"\bProControllerButtonA\b"));
                range.SetStyle(StyleInput, new Regex(@"\bProControllerButtonB\b"));
                range.SetStyle(StyleInput, new Regex(@"\bProControllerButtonX\b"));
                range.SetStyle(StyleInput, new Regex(@"\bProControllerButtonY\b"));
                range.SetStyle(StyleInput, new Regex(@"\bProControllerButtonPLUS\b"));
                range.SetStyle(StyleInput, new Regex(@"\bProControllerButtonHOME\b"));
                range.SetStyle(StyleInput, new Regex(@"\bProControllerButtonSTICK_Right\b"));
                range.SetStyle(StyleInput, new Regex(@"\bProControllerAccelX\b"));
                range.SetStyle(StyleInput, new Regex(@"\bProControllerAccelY\b"));
                range.SetStyle(StyleInput, new Regex(@"\bProControllerGyroX\b"));
                range.SetStyle(StyleInput, new Regex(@"\bProControllerGyroY\b"));
                range.SetStyle(StyleInput, new Regex(@"\bMouseButtons0\b"));
                range.SetStyle(StyleInput, new Regex(@"\bMouseButtons1\b"));
                range.SetStyle(StyleInput, new Regex(@"\bMouseButtons2\b"));
                range.SetStyle(StyleInput, new Regex(@"\bMouseButtons3\b"));
                range.SetStyle(StyleInput, new Regex(@"\bMouseButtons4\b"));
                range.SetStyle(StyleInput, new Regex(@"\bMouseButtons5\b"));
                range.SetStyle(StyleInput, new Regex(@"\bMouseButtons6\b"));
                range.SetStyle(StyleInput, new Regex(@"\bMouseButtons7\b"));
                range.SetStyle(StyleInput, new Regex(@"\bMouseAxisX\b"));
                range.SetStyle(StyleInput, new Regex(@"\bMouseAxisY\b"));
                range.SetStyle(StyleInput, new Regex(@"\bMouseAxisZ\b"));
                range.SetStyle(StyleInput, new Regex(@"\bKeyboardKeyEscape\b"));
                range.SetStyle(StyleInput, new Regex(@"\bKeyboardKeyD1\b"));
                range.SetStyle(StyleInput, new Regex(@"\bKeyboardKeyD2\b"));
                range.SetStyle(StyleInput, new Regex(@"\bKeyboardKeyD3\b"));
                range.SetStyle(StyleInput, new Regex(@"\bKeyboardKeyD4\b"));
                range.SetStyle(StyleInput, new Regex(@"\bKeyboardKeyD5\b"));
                range.SetStyle(StyleInput, new Regex(@"\bKeyboardKeyD6\b"));
                range.SetStyle(StyleInput, new Regex(@"\bKeyboardKeyD7\b"));
                range.SetStyle(StyleInput, new Regex(@"\bKeyboardKeyD8\b"));
                range.SetStyle(StyleInput, new Regex(@"\bKeyboardKeyD9\b"));
                range.SetStyle(StyleInput, new Regex(@"\bKeyboardKeyD0\b"));
                range.SetStyle(StyleInput, new Regex(@"\bKeyboardKeyMinus\b"));
                range.SetStyle(StyleInput, new Regex(@"\bKeyboardKeyEquals\b"));
                range.SetStyle(StyleInput, new Regex(@"\bKeyboardKeyBack\b"));
                range.SetStyle(StyleInput, new Regex(@"\bKeyboardKeyTab\b"));
                range.SetStyle(StyleInput, new Regex(@"\bKeyboardKeyQ\b"));
                range.SetStyle(StyleInput, new Regex(@"\bKeyboardKeyW\b"));
                range.SetStyle(StyleInput, new Regex(@"\bKeyboardKeyE\b"));
                range.SetStyle(StyleInput, new Regex(@"\bKeyboardKeyR\b"));
                range.SetStyle(StyleInput, new Regex(@"\bKeyboardKeyT\b"));
                range.SetStyle(StyleInput, new Regex(@"\bKeyboardKeyY\b"));
                range.SetStyle(StyleInput, new Regex(@"\bKeyboardKeyU\b"));
                range.SetStyle(StyleInput, new Regex(@"\bKeyboardKeyI\b"));
                range.SetStyle(StyleInput, new Regex(@"\bKeyboardKeyO\b"));
                range.SetStyle(StyleInput, new Regex(@"\bKeyboardKeyP\b"));
                range.SetStyle(StyleInput, new Regex(@"\bKeyboardKeyLeftBracket\b"));
                range.SetStyle(StyleInput, new Regex(@"\bKeyboardKeyRightBracket\b"));
                range.SetStyle(StyleInput, new Regex(@"\bKeyboardKeyReturn\b"));
                range.SetStyle(StyleInput, new Regex(@"\bKeyboardKeyLeftControl\b"));
                range.SetStyle(StyleInput, new Regex(@"\bKeyboardKeyA\b"));
                range.SetStyle(StyleInput, new Regex(@"\bKeyboardKeyS\b"));
                range.SetStyle(StyleInput, new Regex(@"\bKeyboardKeyD\b"));
                range.SetStyle(StyleInput, new Regex(@"\bKeyboardKeyF\b"));
                range.SetStyle(StyleInput, new Regex(@"\bKeyboardKeyG\b"));
                range.SetStyle(StyleInput, new Regex(@"\bKeyboardKeyH\b"));
                range.SetStyle(StyleInput, new Regex(@"\bKeyboardKeyJ\b"));
                range.SetStyle(StyleInput, new Regex(@"\bKeyboardKeyK\b"));
                range.SetStyle(StyleInput, new Regex(@"\bKeyboardKeyL\b"));
                range.SetStyle(StyleInput, new Regex(@"\bKeyboardKeySemicolon\b"));
                range.SetStyle(StyleInput, new Regex(@"\bKeyboardKeyApostrophe\b"));
                range.SetStyle(StyleInput, new Regex(@"\bKeyboardKeyGrave\b"));
                range.SetStyle(StyleInput, new Regex(@"\bKeyboardKeyLeftShift\b"));
                range.SetStyle(StyleInput, new Regex(@"\bKeyboardKeyBackslash\b"));
                range.SetStyle(StyleInput, new Regex(@"\bKeyboardKeyZ\b"));
                range.SetStyle(StyleInput, new Regex(@"\bKeyboardKeyX\b"));
                range.SetStyle(StyleInput, new Regex(@"\bKeyboardKeyC\b"));
                range.SetStyle(StyleInput, new Regex(@"\bKeyboardKeyV\b"));
                range.SetStyle(StyleInput, new Regex(@"\bKeyboardKeyB\b"));
                range.SetStyle(StyleInput, new Regex(@"\bKeyboardKeyN\b"));
                range.SetStyle(StyleInput, new Regex(@"\bKeyboardKeyM\b"));
                range.SetStyle(StyleInput, new Regex(@"\bKeyboardKeyComma\b"));
                range.SetStyle(StyleInput, new Regex(@"\bKeyboardKeyPeriod\b"));
                range.SetStyle(StyleInput, new Regex(@"\bKeyboardKeySlash\b"));
                range.SetStyle(StyleInput, new Regex(@"\bKeyboardKeyRightShift\b"));
                range.SetStyle(StyleInput, new Regex(@"\bKeyboardKeyMultiply\b"));
                range.SetStyle(StyleInput, new Regex(@"\bKeyboardKeyLeftAlt\b"));
                range.SetStyle(StyleInput, new Regex(@"\bKeyboardKeySpace\b"));
                range.SetStyle(StyleInput, new Regex(@"\bKeyboardKeyCapital\b"));
                range.SetStyle(StyleInput, new Regex(@"\bKeyboardKeyF1\b"));
                range.SetStyle(StyleInput, new Regex(@"\bKeyboardKeyF2\b"));
                range.SetStyle(StyleInput, new Regex(@"\bKeyboardKeyF3\b"));
                range.SetStyle(StyleInput, new Regex(@"\bKeyboardKeyF4\b"));
                range.SetStyle(StyleInput, new Regex(@"\bKeyboardKeyF5\b"));
                range.SetStyle(StyleInput, new Regex(@"\bKeyboardKeyF6\b"));
                range.SetStyle(StyleInput, new Regex(@"\bKeyboardKeyF7\b"));
                range.SetStyle(StyleInput, new Regex(@"\bKeyboardKeyF8\b"));
                range.SetStyle(StyleInput, new Regex(@"\bKeyboardKeyF9\b"));
                range.SetStyle(StyleInput, new Regex(@"\bKeyboardKeyF10\b"));
                range.SetStyle(StyleInput, new Regex(@"\bKeyboardKeyNumberLock\b"));
                range.SetStyle(StyleInput, new Regex(@"\bKeyboardKeyScrollLock\b"));
                range.SetStyle(StyleInput, new Regex(@"\bKeyboardKeyNumberPad7\b"));
                range.SetStyle(StyleInput, new Regex(@"\bKeyboardKeyNumberPad8\b"));
                range.SetStyle(StyleInput, new Regex(@"\bKeyboardKeyNumberPad9\b"));
                range.SetStyle(StyleInput, new Regex(@"\bKeyboardKeySubtract\b"));
                range.SetStyle(StyleInput, new Regex(@"\bKeyboardKeyNumberPad4\b"));
                range.SetStyle(StyleInput, new Regex(@"\bKeyboardKeyNumberPad5\b"));
                range.SetStyle(StyleInput, new Regex(@"\bKeyboardKeyNumberPad6\b"));
                range.SetStyle(StyleInput, new Regex(@"\bKeyboardKeyAdd\b"));
                range.SetStyle(StyleInput, new Regex(@"\bKeyboardKeyNumberPad1\b"));
                range.SetStyle(StyleInput, new Regex(@"\bKeyboardKeyNumberPad2\b"));
                range.SetStyle(StyleInput, new Regex(@"\bKeyboardKeyNumberPad3\b"));
                range.SetStyle(StyleInput, new Regex(@"\bKeyboardKeyNumberPad0\b"));
                range.SetStyle(StyleInput, new Regex(@"\bKeyboardKeyDecimal\b"));
                range.SetStyle(StyleInput, new Regex(@"\bKeyboardKeyOem102\b"));
                range.SetStyle(StyleInput, new Regex(@"\bKeyboardKeyF11\b"));
                range.SetStyle(StyleInput, new Regex(@"\bKeyboardKeyF12\b"));
                range.SetStyle(StyleInput, new Regex(@"\bKeyboardKeyF13\b"));
                range.SetStyle(StyleInput, new Regex(@"\bKeyboardKeyF14\b"));
                range.SetStyle(StyleInput, new Regex(@"\bKeyboardKeyF15\b"));
                range.SetStyle(StyleInput, new Regex(@"\bKeyboardKeyKana\b"));
                range.SetStyle(StyleInput, new Regex(@"\bKeyboardKeyAbntC1\b"));
                range.SetStyle(StyleInput, new Regex(@"\bKeyboardKeyConvert\b"));
                range.SetStyle(StyleInput, new Regex(@"\bKeyboardKeyNoConvert\b"));
                range.SetStyle(StyleInput, new Regex(@"\bKeyboardKeyYen\b"));
                range.SetStyle(StyleInput, new Regex(@"\bKeyboardKeyAbntC2\b"));
                range.SetStyle(StyleInput, new Regex(@"\bKeyboardKeyNumberPadEquals\b"));
                range.SetStyle(StyleInput, new Regex(@"\bKeyboardKeyPreviousTrack\b"));
                range.SetStyle(StyleInput, new Regex(@"\bKeyboardKeyAT\b"));
                range.SetStyle(StyleInput, new Regex(@"\bKeyboardKeyColon\b"));
                range.SetStyle(StyleInput, new Regex(@"\bKeyboardKeyUnderline\b"));
                range.SetStyle(StyleInput, new Regex(@"\bKeyboardKeyKanji\b"));
                range.SetStyle(StyleInput, new Regex(@"\bKeyboardKeyStop\b"));
                range.SetStyle(StyleInput, new Regex(@"\bKeyboardKeyAX\b"));
                range.SetStyle(StyleInput, new Regex(@"\bKeyboardKeyUnlabeled\b"));
                range.SetStyle(StyleInput, new Regex(@"\bKeyboardKeyNextTrack\b"));
                range.SetStyle(StyleInput, new Regex(@"\bKeyboardKeyNumberPadEnter\b"));
                range.SetStyle(StyleInput, new Regex(@"\bKeyboardKeyRightControl\b"));
                range.SetStyle(StyleInput, new Regex(@"\bKeyboardKeyMute\b"));
                range.SetStyle(StyleInput, new Regex(@"\bKeyboardKeyCalculator\b"));
                range.SetStyle(StyleInput, new Regex(@"\bKeyboardKeyPlayPause\b"));
                range.SetStyle(StyleInput, new Regex(@"\bKeyboardKeyMediaStop\b"));
                range.SetStyle(StyleInput, new Regex(@"\bKeyboardKeyVolumeDown\b"));
                range.SetStyle(StyleInput, new Regex(@"\bKeyboardKeyVolumeUp\b"));
                range.SetStyle(StyleInput, new Regex(@"\bKeyboardKeyWebHome\b"));
                range.SetStyle(StyleInput, new Regex(@"\bKeyboardKeyNumberPadComma\b"));
                range.SetStyle(StyleInput, new Regex(@"\bKeyboardKeyDivide\b"));
                range.SetStyle(StyleInput, new Regex(@"\bKeyboardKeyPrintScreen\b"));
                range.SetStyle(StyleInput, new Regex(@"\bKeyboardKeyRightAlt\b"));
                range.SetStyle(StyleInput, new Regex(@"\bKeyboardKeyPause\b"));
                range.SetStyle(StyleInput, new Regex(@"\bKeyboardKeyHome\b"));
                range.SetStyle(StyleInput, new Regex(@"\bKeyboardKeyUp\b"));
                range.SetStyle(StyleInput, new Regex(@"\bKeyboardKeyPageUp\b"));
                range.SetStyle(StyleInput, new Regex(@"\bKeyboardKeyLeft\b"));
                range.SetStyle(StyleInput, new Regex(@"\bKeyboardKeyRight\b"));
                range.SetStyle(StyleInput, new Regex(@"\bKeyboardKeyEnd\b"));
                range.SetStyle(StyleInput, new Regex(@"\bKeyboardKeyDown\b"));
                range.SetStyle(StyleInput, new Regex(@"\bKeyboardKeyPageDown\b"));
                range.SetStyle(StyleInput, new Regex(@"\bKeyboardKeyInsert\b"));
                range.SetStyle(StyleInput, new Regex(@"\bKeyboardKeyDelete\b"));
                range.SetStyle(StyleInput, new Regex(@"\bKeyboardKeyLeftWindowsKey\b"));
                range.SetStyle(StyleInput, new Regex(@"\bKeyboardKeyRightWindowsKey\b"));
                range.SetStyle(StyleInput, new Regex(@"\bKeyboardKeyApplications\b"));
                range.SetStyle(StyleInput, new Regex(@"\bKeyboardKeyPower\b"));
                range.SetStyle(StyleInput, new Regex(@"\bKeyboardKeySleep\b"));
                range.SetStyle(StyleInput, new Regex(@"\bKeyboardKeyWake\b"));
                range.SetStyle(StyleInput, new Regex(@"\bKeyboardKeyWebSearch\b"));
                range.SetStyle(StyleInput, new Regex(@"\bKeyboardKeyWebFavorites\b"));
                range.SetStyle(StyleInput, new Regex(@"\bKeyboardKeyWebRefresh\b"));
                range.SetStyle(StyleInput, new Regex(@"\bKeyboardKeyWebStop\b"));
                range.SetStyle(StyleInput, new Regex(@"\bKeyboardKeyWebForward\b"));
                range.SetStyle(StyleInput, new Regex(@"\bKeyboardKeyWebBack\b"));
                range.SetStyle(StyleInput, new Regex(@"\bKeyboardKeyMyComputer\b"));
                range.SetStyle(StyleInput, new Regex(@"\bKeyboardKeyMail\b"));
                range.SetStyle(StyleInput, new Regex(@"\bKeyboardKeyMediaSelect\b"));
                range.SetStyle(StyleInput, new Regex(@"\bKeyboardKeyUnknown\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bKeyboardMouseDriverType\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bMouseMoveX\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bMouseMoveY\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bMouseAbsX\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bMouseAbsY\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bMouseDesktopX\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bMouseDesktopY\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bSendLeftClick\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bSendRightClick\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bSendMiddleClick\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bSendWheelUp\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bSendWheelDown\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bSendLeft\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bSendRight\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bSendUp\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bSendDown\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bSendLButton\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bSendRButton\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bSendCancel\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bSendMBUTTON\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bSendXBUTTON1\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bSendXBUTTON2\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bSendBack\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bSendTab\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bSendClear\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bSendReturn\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bSendSHIFT\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bSendCONTROL\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bSendMENU\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bSendPAUSE\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bSendCAPITAL\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bSendKANA\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bSendHANGEUL\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bSendHANGUL\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bSendJUNJA\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bSendFINAL\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bSendHANJA\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bSendKANJI\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bSendEscape\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bSendCONVERT\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bSendNONCONVERT\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bSendACCEPT\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bSendMODECHANGE\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bSendSpace\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bSendPRIOR\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bSendNEXT\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bSendEND\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bSendHOME\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bSendLEFT\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bSendUP\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bSendRIGHT\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bSendDOWN\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bSendSELECT\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bSendPRINT\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bSendEXECUTE\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bSendSNAPSHOT\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bSendINSERT\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bSendDELETE\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bSendHELP\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bSendAPOSTROPHE\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bSend0\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bSend1\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bSend2\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bSend3\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bSend4\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bSend5\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bSend6\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bSend7\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bSend8\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bSend9\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bSendA\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bSendB\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bSendC\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bSendD\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bSendE\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bSendF\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bSendG\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bSendH\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bSendI\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bSendJ\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bSendK\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bSendL\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bSendM\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bSendN\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bSendO\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bSendP\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bSendQ\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bSendR\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bSendS\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bSendT\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bSendU\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bSendV\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bSendW\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bSendX\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bSendY\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bSendZ\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bSendLWIN\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bSendRWIN\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bSendAPPS\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bSendSLEEP\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bSendNUMPAD0\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bSendNUMPAD1\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bSendNUMPAD2\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bSendNUMPAD3\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bSendNUMPAD4\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bSendNUMPAD5\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bSendNUMPAD6\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bSendNUMPAD7\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bSendNUMPAD8\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bSendNUMPAD9\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bSendMULTIPLY\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bSendADD\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bSendSEPARATOR\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bSendSUBTRACT\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bSendDECIMAL\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bSendDIVIDE\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bSendF1\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bSendF2\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bSendF3\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bSendF4\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bSendF5\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bSendF6\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bSendF7\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bSendF8\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bSendF9\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bSendF10\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bSendF11\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bSendF12\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bSendF13\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bSendF14\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bSendF15\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bSendF16\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bSendF17\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bSendF18\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bSendF19\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bSendF20\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bSendF21\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bSendF22\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bSendF23\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bSendF24\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bSendNUMLOCK\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bSendSCROLL\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bSendLeftShift\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bSendRightShift\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bSendLeftControl\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bSendRightControl\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bSendLMENU\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bSendRMENU\b"));
                range.SetStyle(StyleLibrary, new Regex(@"\bCameraAPI\b"));
                range.SetStyle(StyleClass, new Regex(@"\bCameraToLed\b"));
                range.SetStyle(StyleObject, new Regex(@"\bcamera\b"));
                range.SetStyle(StyleInput, new Regex(@"\bcamx\b"));
                range.SetStyle(StyleInput, new Regex(@"\bcamy\b"));
                range.SetStyle(StyleLibrary, new Regex(@"\bSpeechAPI\b"));
                range.SetStyle(StyleClass, new Regex(@"\bSpeechToText\b"));
                range.SetStyle(StyleObject, new Regex(@"\bspeech\b"));
                range.SetStyle(StyleInput, new Regex(@"\bspeechtext\b"));
                range.SetStyle(StyleLibrary, new Regex(@"\bKeyboardHooksAPI\b"));
                range.SetStyle(StyleClass, new Regex(@"\bKeyboardHooks\b"));
                range.SetStyle(StyleObject, new Regex(@"\bkh\b"));
                range.SetStyle(StyleLibrary, new Regex(@"\bMouseHooksAPI\b"));
                range.SetStyle(StyleClass, new Regex(@"\bMouseHooks\b"));
                range.SetStyle(StyleObject, new Regex(@"\bmh\b"));
                range.SetStyle(StyleLibrary, new Regex(@"\bGamepadsHooksAPI\b"));
                range.SetStyle(StyleClass, new Regex(@"\bGamepadsHooks\b"));
                range.SetStyle(StyleObject, new Regex(@"\bgh\b"));
                range.SetStyle(StyleObject, new Regex(@"\bgh1\b"));
                range.SetStyle(StyleObject, new Regex(@"\bgh2\b"));
                range.SetStyle(StyleLibrary, new Regex(@"\bJoysticksHooksAPI\b"));
                range.SetStyle(StyleClass, new Regex(@"\bJoysticksHooks\b"));
                range.SetStyle(StyleObject, new Regex(@"\bjh\b"));
                range.SetStyle(StyleObject, new Regex(@"\bjh1\b"));
                range.SetStyle(StyleObject, new Regex(@"\bjh2\b"));
                range.SetStyle(StyleInput, new Regex(@"\bKey_LBUTTON\b"));
                range.SetStyle(StyleInput, new Regex(@"\bKey_RBUTTON\b"));
                range.SetStyle(StyleInput, new Regex(@"\bKey_CANCEL\b"));
                range.SetStyle(StyleInput, new Regex(@"\bKey_MBUTTON\b"));
                range.SetStyle(StyleInput, new Regex(@"\bKey_XBUTTON1\b"));
                range.SetStyle(StyleInput, new Regex(@"\bKey_XBUTTON2\b"));
                range.SetStyle(StyleInput, new Regex(@"\bKey_BACK\b"));
                range.SetStyle(StyleInput, new Regex(@"\bKey_Tab\b"));
                range.SetStyle(StyleInput, new Regex(@"\bKey_CLEAR\b"));
                range.SetStyle(StyleInput, new Regex(@"\bKey_Return\b"));
                range.SetStyle(StyleInput, new Regex(@"\bKey_SHIFT\b"));
                range.SetStyle(StyleInput, new Regex(@"\bKey_CONTROL\b"));
                range.SetStyle(StyleInput, new Regex(@"\bKey_MENU\b"));
                range.SetStyle(StyleInput, new Regex(@"\bKey_PAUSE\b"));
                range.SetStyle(StyleInput, new Regex(@"\bKey_CAPITAL\b"));
                range.SetStyle(StyleInput, new Regex(@"\bKey_KANA\b"));
                range.SetStyle(StyleInput, new Regex(@"\bKey_HANGEUL\b"));
                range.SetStyle(StyleInput, new Regex(@"\bKey_HANGUL\b"));
                range.SetStyle(StyleInput, new Regex(@"\bKey_JUNJA\b"));
                range.SetStyle(StyleInput, new Regex(@"\bKey_FINAL\b"));
                range.SetStyle(StyleInput, new Regex(@"\bKey_HANJA\b"));
                range.SetStyle(StyleInput, new Regex(@"\bKey_KANJI\b"));
                range.SetStyle(StyleInput, new Regex(@"\bKey_Escape\b"));
                range.SetStyle(StyleInput, new Regex(@"\bKey_CONVERT\b"));
                range.SetStyle(StyleInput, new Regex(@"\bKey_NONCONVERT\b"));
                range.SetStyle(StyleInput, new Regex(@"\bKey_ACCEPT\b"));
                range.SetStyle(StyleInput, new Regex(@"\bKey_MODECHANGE\b"));
                range.SetStyle(StyleInput, new Regex(@"\bKey_Space\b"));
                range.SetStyle(StyleInput, new Regex(@"\bKey_PRIOR\b"));
                range.SetStyle(StyleInput, new Regex(@"\bKey_NEXT\b"));
                range.SetStyle(StyleInput, new Regex(@"\bKey_END\b"));
                range.SetStyle(StyleInput, new Regex(@"\bKey_HOME\b"));
                range.SetStyle(StyleInput, new Regex(@"\bKey_LEFT\b"));
                range.SetStyle(StyleInput, new Regex(@"\bKey_UP\b"));
                range.SetStyle(StyleInput, new Regex(@"\bKey_RIGHT\b"));
                range.SetStyle(StyleInput, new Regex(@"\bKey_DOWN\b"));
                range.SetStyle(StyleInput, new Regex(@"\bKey_SELECT\b"));
                range.SetStyle(StyleInput, new Regex(@"\bKey_PRINT\b"));
                range.SetStyle(StyleInput, new Regex(@"\bKey_EXECUTE\b"));
                range.SetStyle(StyleInput, new Regex(@"\bKey_SNAPSHOT\b"));
                range.SetStyle(StyleInput, new Regex(@"\bKey_INSERT\b"));
                range.SetStyle(StyleInput, new Regex(@"\bKey_DELETE\b"));
                range.SetStyle(StyleInput, new Regex(@"\bKey_HELP\b"));
                range.SetStyle(StyleInput, new Regex(@"\bKey_APOSTROPHE\b"));
                range.SetStyle(StyleInput, new Regex(@"\bKey_0\b"));
                range.SetStyle(StyleInput, new Regex(@"\bKey_1\b"));
                range.SetStyle(StyleInput, new Regex(@"\bKey_2\b"));
                range.SetStyle(StyleInput, new Regex(@"\bKey_3\b"));
                range.SetStyle(StyleInput, new Regex(@"\bKey_4\b"));
                range.SetStyle(StyleInput, new Regex(@"\bKey_5\b"));
                range.SetStyle(StyleInput, new Regex(@"\bKey_6\b"));
                range.SetStyle(StyleInput, new Regex(@"\bKey_7\b"));
                range.SetStyle(StyleInput, new Regex(@"\bKey_8\b"));
                range.SetStyle(StyleInput, new Regex(@"\bKey_9\b"));
                range.SetStyle(StyleInput, new Regex(@"\bKey_A\b"));
                range.SetStyle(StyleInput, new Regex(@"\bKey_B\b"));
                range.SetStyle(StyleInput, new Regex(@"\bKey_C\b"));
                range.SetStyle(StyleInput, new Regex(@"\bKey_D\b"));
                range.SetStyle(StyleInput, new Regex(@"\bKey_E\b"));
                range.SetStyle(StyleInput, new Regex(@"\bKey_F\b"));
                range.SetStyle(StyleInput, new Regex(@"\bKey_G\b"));
                range.SetStyle(StyleInput, new Regex(@"\bKey_H\b"));
                range.SetStyle(StyleInput, new Regex(@"\bKey_I\b"));
                range.SetStyle(StyleInput, new Regex(@"\bKey_J\b"));
                range.SetStyle(StyleInput, new Regex(@"\bKey_K\b"));
                range.SetStyle(StyleInput, new Regex(@"\bKey_L\b"));
                range.SetStyle(StyleInput, new Regex(@"\bKey_M\b"));
                range.SetStyle(StyleInput, new Regex(@"\bKey_N\b"));
                range.SetStyle(StyleInput, new Regex(@"\bKey_O\b"));
                range.SetStyle(StyleInput, new Regex(@"\bKey_P\b"));
                range.SetStyle(StyleInput, new Regex(@"\bKey_Q\b"));
                range.SetStyle(StyleInput, new Regex(@"\bKey_R\b"));
                range.SetStyle(StyleInput, new Regex(@"\bKey_S\b"));
                range.SetStyle(StyleInput, new Regex(@"\bKey_T\b"));
                range.SetStyle(StyleInput, new Regex(@"\bKey_U\b"));
                range.SetStyle(StyleInput, new Regex(@"\bKey_V\b"));
                range.SetStyle(StyleInput, new Regex(@"\bKey_W\b"));
                range.SetStyle(StyleInput, new Regex(@"\bKey_X\b"));
                range.SetStyle(StyleInput, new Regex(@"\bKey_Y\b"));
                range.SetStyle(StyleInput, new Regex(@"\bKey_Z\b"));
                range.SetStyle(StyleInput, new Regex(@"\bKey_LWIN\b"));
                range.SetStyle(StyleInput, new Regex(@"\bKey_RWIN\b"));
                range.SetStyle(StyleInput, new Regex(@"\bKey_APPS\b"));
                range.SetStyle(StyleInput, new Regex(@"\bKey_SLEEP\b"));
                range.SetStyle(StyleInput, new Regex(@"\bKey_NUMPAD0\b"));
                range.SetStyle(StyleInput, new Regex(@"\bKey_NUMPAD1\b"));
                range.SetStyle(StyleInput, new Regex(@"\bKey_NUMPAD2\b"));
                range.SetStyle(StyleInput, new Regex(@"\bKey_NUMPAD3\b"));
                range.SetStyle(StyleInput, new Regex(@"\bKey_NUMPAD4\b"));
                range.SetStyle(StyleInput, new Regex(@"\bKey_NUMPAD5\b"));
                range.SetStyle(StyleInput, new Regex(@"\bKey_NUMPAD6\b"));
                range.SetStyle(StyleInput, new Regex(@"\bKey_NUMPAD7\b"));
                range.SetStyle(StyleInput, new Regex(@"\bKey_NUMPAD8\b"));
                range.SetStyle(StyleInput, new Regex(@"\bKey_NUMPAD9\b"));
                range.SetStyle(StyleInput, new Regex(@"\bKey_MULTIPLY\b"));
                range.SetStyle(StyleInput, new Regex(@"\bKey_ADD\b"));
                range.SetStyle(StyleInput, new Regex(@"\bKey_SEPARATOR\b"));
                range.SetStyle(StyleInput, new Regex(@"\bKey_SUBTRACT\b"));
                range.SetStyle(StyleInput, new Regex(@"\bKey_DECIMAL\b"));
                range.SetStyle(StyleInput, new Regex(@"\bKey_DIVIDE\b"));
                range.SetStyle(StyleInput, new Regex(@"\bKey_F1\b"));
                range.SetStyle(StyleInput, new Regex(@"\bKey_F2\b"));
                range.SetStyle(StyleInput, new Regex(@"\bKey_F3\b"));
                range.SetStyle(StyleInput, new Regex(@"\bKey_F4\b"));
                range.SetStyle(StyleInput, new Regex(@"\bKey_F5\b"));
                range.SetStyle(StyleInput, new Regex(@"\bKey_F6\b"));
                range.SetStyle(StyleInput, new Regex(@"\bKey_F7\b"));
                range.SetStyle(StyleInput, new Regex(@"\bKey_F8\b"));
                range.SetStyle(StyleInput, new Regex(@"\bKey_F9\b"));
                range.SetStyle(StyleInput, new Regex(@"\bKey_F10\b"));
                range.SetStyle(StyleInput, new Regex(@"\bKey_F11\b"));
                range.SetStyle(StyleInput, new Regex(@"\bKey_F12\b"));
                range.SetStyle(StyleInput, new Regex(@"\bKey_F13\b"));
                range.SetStyle(StyleInput, new Regex(@"\bKey_F14\b"));
                range.SetStyle(StyleInput, new Regex(@"\bKey_F15\b"));
                range.SetStyle(StyleInput, new Regex(@"\bKey_F16\b"));
                range.SetStyle(StyleInput, new Regex(@"\bKey_F17\b"));
                range.SetStyle(StyleInput, new Regex(@"\bKey_F18\b"));
                range.SetStyle(StyleInput, new Regex(@"\bKey_F19\b"));
                range.SetStyle(StyleInput, new Regex(@"\bKey_F20\b"));
                range.SetStyle(StyleInput, new Regex(@"\bKey_F21\b"));
                range.SetStyle(StyleInput, new Regex(@"\bKey_F22\b"));
                range.SetStyle(StyleInput, new Regex(@"\bKey_F23\b"));
                range.SetStyle(StyleInput, new Regex(@"\bKey_F24\b"));
                range.SetStyle(StyleInput, new Regex(@"\bKey_NUMLOCK\b"));
                range.SetStyle(StyleInput, new Regex(@"\bKey_SCROLL\b"));
                range.SetStyle(StyleInput, new Regex(@"\bKey_LeftShift\b"));
                range.SetStyle(StyleInput, new Regex(@"\bKey_RightShift\b"));
                range.SetStyle(StyleInput, new Regex(@"\bKey_LeftControl\b"));
                range.SetStyle(StyleInput, new Regex(@"\bKey_RightControl\b"));
                range.SetStyle(StyleInput, new Regex(@"\bKey_LMENU\b"));
                range.SetStyle(StyleInput, new Regex(@"\bKey_RMENU\b"));
                range.SetStyle(StyleInput, new Regex(@"\bKey_BROWSER_BACK\b"));
                range.SetStyle(StyleInput, new Regex(@"\bKey_BROWSER_FORWARD\b"));
                range.SetStyle(StyleInput, new Regex(@"\bKey_BROWSER_REFRESH\b"));
                range.SetStyle(StyleInput, new Regex(@"\bKey_BROWSER_STOP\b"));
                range.SetStyle(StyleInput, new Regex(@"\bKey_BROWSER_SEARCH\b"));
                range.SetStyle(StyleInput, new Regex(@"\bKey_BROWSER_FAVORITES\b"));
                range.SetStyle(StyleInput, new Regex(@"\bKey_BROWSER_HOME\b"));
                range.SetStyle(StyleInput, new Regex(@"\bKey_VOLUME_MUTE\b"));
                range.SetStyle(StyleInput, new Regex(@"\bKey_VOLUME_DOWN\b"));
                range.SetStyle(StyleInput, new Regex(@"\bKey_VOLUME_UP\b"));
                range.SetStyle(StyleInput, new Regex(@"\bKey_MEDIA_NEXT_TRACK\b"));
                range.SetStyle(StyleInput, new Regex(@"\bKey_MEDIA_PREV_TRACK\b"));
                range.SetStyle(StyleInput, new Regex(@"\bKey_MEDIA_STOP\b"));
                range.SetStyle(StyleInput, new Regex(@"\bKey_MEDIA_PLAY_PAUSE\b"));
                range.SetStyle(StyleInput, new Regex(@"\bKey_LAUNCH_MAIL\b"));
                range.SetStyle(StyleInput, new Regex(@"\bKey_LAUNCH_MEDIA_SELECT\b"));
                range.SetStyle(StyleInput, new Regex(@"\bKey_LAUNCH_APP1\b"));
                range.SetStyle(StyleInput, new Regex(@"\bKey_LAUNCH_APP2\b"));
                range.SetStyle(StyleInput, new Regex(@"\bKey_OEM_1\b"));
                range.SetStyle(StyleInput, new Regex(@"\bKey_OEM_PLUS\b"));
                range.SetStyle(StyleInput, new Regex(@"\bKey_OEM_COMMA\b"));
                range.SetStyle(StyleInput, new Regex(@"\bKey_OEM_MINUS\b"));
                range.SetStyle(StyleInput, new Regex(@"\bKey_OEM_PERIOD\b"));
                range.SetStyle(StyleInput, new Regex(@"\bKey_OEM_2\b"));
                range.SetStyle(StyleInput, new Regex(@"\bKey_OEM_3\b"));
                range.SetStyle(StyleInput, new Regex(@"\bKey_OEM_4\b"));
                range.SetStyle(StyleInput, new Regex(@"\bKey_OEM_5\b"));
                range.SetStyle(StyleInput, new Regex(@"\bKey_OEM_6\b"));
                range.SetStyle(StyleInput, new Regex(@"\bKey_OEM_7\b"));
                range.SetStyle(StyleInput, new Regex(@"\bKey_OEM_8\b"));
                range.SetStyle(StyleInput, new Regex(@"\bKey_OEM_102\b"));
                range.SetStyle(StyleInput, new Regex(@"\bKey_PROCESS\bKey\b"));
                range.SetStyle(StyleInput, new Regex(@"\bKey_PACKET\b"));
                range.SetStyle(StyleInput, new Regex(@"\bKey_ATTN\b"));
                range.SetStyle(StyleInput, new Regex(@"\bKey_CRSEL\b"));
                range.SetStyle(StyleInput, new Regex(@"\bKey_EXSEL\b"));
                range.SetStyle(StyleInput, new Regex(@"\bKey_EREOF\b"));
                range.SetStyle(StyleInput, new Regex(@"\bKey_PLAY\b"));
                range.SetStyle(StyleInput, new Regex(@"\bKey_ZOOM\b"));
                range.SetStyle(StyleInput, new Regex(@"\bKey_NONAME\b"));
                range.SetStyle(StyleInput, new Regex(@"\bKey_PA1\b"));
                range.SetStyle(StyleInput, new Regex(@"\bKey_OEM_CLEAR\b"));
                range.SetStyle(StyleInput, new Regex(@"\bCursorX\b"));
                range.SetStyle(StyleInput, new Regex(@"\bCursorY\b"));
                range.SetStyle(StyleInput, new Regex(@"\bMouseX\b"));
                range.SetStyle(StyleInput, new Regex(@"\bMouseY\b"));
                range.SetStyle(StyleInput, new Regex(@"\bMouseZ\b"));
                range.SetStyle(StyleInput, new Regex(@"\bMouseRightButton\b"));
                range.SetStyle(StyleInput, new Regex(@"\bMouseLeftButton\b"));
                range.SetStyle(StyleInput, new Regex(@"\bMouseMiddleButton\b"));
                range.SetStyle(StyleInput, new Regex(@"\bMouseXButton\b"));
                range.SetStyle(StyleInput, new Regex(@"\bMouseButtonX\b"));
                range.SetStyle(StyleMethod, new Regex(@"\bbyteArrayToControl\b"));
                range.SetStyle(StyleMethod, new Regex(@"\bcontrolToByteArray\b"));
                range.SetStyle(StyleMethod, new Regex(@"\bstringToControl\b"));
                range.SetStyle(StyleLibrary, new Regex(@"\bNetworkshost\b"));
                range.SetStyle(StyleClass, new Regex(@"\bNetworkHost\b"));
                range.SetStyle(StyleLibrary, new Regex(@"\bNetworks\b"));
                range.SetStyle(StyleClass, new Regex(@"\bNetwork\b"));
                range.SetStyle(StyleLibrary, new Regex(@"\bWebSocketSharp\b"));
                range.SetStyle(StyleClass, new Regex(@"\bWebSocket\b"));
                range.SetStyle(StyleMethod, new Regex(@"\bWs_OnMessage\b"));
                range.SetStyle(StyleObject, new Regex(@"\bwsc\b"));
                range.SetStyle(StyleExtra, new Regex(@"\bobject\b"));
                range.SetStyle(StyleInput, new Regex(@"\bMessageEventArgs\b"));
                range.SetStyle(StyleInput, new Regex(@"\brawdataavailable\b"));
                range.SetStyle(StyleInput, new Regex(@"\bbyteArrayIn\b"));
                range.SetStyle(StyleInput, new Regex(@"\bcontrol\b"));
                range.SetStyle(StyleMethod, new Regex(@"\bEncoding.ASCII.GetString\b"));
                range.SetStyle(StyleMethod, new Regex(@"\bSplit\b"));
                range.SetStyle(StyleInput, new Regex(@"\bstr\b"));
                range.SetStyle(StyleInput, new Regex(@"\bRawData\b"));
                range.SetStyle(StyleOutput, new Regex(@"\blocalip\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bport\b"));
                range.SetStyle(StyleLibrary, new Regex(@"\bText\b"));
                range.SetStyle(StyleMethod, new Regex(@"\bOnMessage\b"));
                range.SetStyle(StyleInput, new Regex(@"\bsender\b"));
                range.SetStyle(StyleInput, new Regex(@"\be\b"));
                range.SetStyle(StyleMethod, new Regex(@"\bIsAlive\b"));
                range.SetStyle(StyleMethod, new Regex(@"\bSend\b"));
                range.SetStyle(StyleInput, new Regex(@"\bConvert\b"));
                range.SetStyle(StyleInput, new Regex(@"\bconnectionString\b"));
                range.SetStyle(StyleInput, new Regex(@"\bToSingle\b"));
                range.SetStyle(StyleInput, new Regex(@"\bParse\b"));
                range.SetStyle(StyleMethod, new Regex(@"\bToArray\b"));
                range.SetStyle(StyleMethod, new Regex(@"\bEncoding.ASCII.GetBytes\b"));
                range.SetStyle(StyleMethod, new Regex(@"\bIndexOf\b"));
                range.SetStyle(StyleInput, new Regex(@"\bdata\b"));
                range.SetStyle(StyleInput, new Regex(@"\bunsplitstring\b"));
                range.SetStyle(StyleInput, new Regex(@"\bsplitstring\b"));
                range.SetStyle(StyleInput, new Regex(@"\bnewsplitstring\b"));
                range.SetStyle(StyleInput, new Regex(@"\bvaluestring\b"));
                range.SetStyle(StyleInput, new Regex(@"\bpFrom\b"));
                range.SetStyle(StyleInput, new Regex(@"\bnewvaluestring\b"));
                range.SetStyle(StyleInput, new Regex(@"\bpTo\b"));
                range.SetStyle(StyleInput, new Regex(@"\bresult\b"));
                range.SetStyle(StyleMethod, new Regex(@"\bLength\b"));
                range.SetStyle(StyleMethod, new Regex(@"\bSubstring\b"));
                range.SetStyle(StyleMethod, new Regex(@"\bReplace\b"));
                range.SetStyle(StyleLibrary, new Regex(@"\bKeyboardRawInputsAPI\b"));
                range.SetStyle(StyleLibrary, new Regex(@"\bMouseRawInputsAPI\b"));
                range.SetStyle(StyleClass, new Regex(@"\bKeyboardRawInputs\b"));
                range.SetStyle(StyleClass, new Regex(@"\bMouseRawInputs\b"));
                range.SetStyle(StyleObject, new Regex(@"\bmri\b"));
                range.SetStyle(StyleObject, new Regex(@"\bkri\b"));
                range.SetStyle(StyleLibrary, new Regex(@"\bKeyboardXnaHookAPI\b"));
                range.SetStyle(StyleLibrary, new Regex(@"\bMouseXnaHookAPI\b"));
                range.SetStyle(StyleClass, new Regex(@"\bKeyboardXnaHook\b"));
                range.SetStyle(StyleClass, new Regex(@"\bMouseXnaHook\b"));
                range.SetStyle(StyleObject, new Regex(@"\bmxh\b"));
                range.SetStyle(StyleObject, new Regex(@"\bkxh\b"));
                range.SetStyle(StyleLibrary, new Regex(@"\bKeyboardRawHooksAPI\b"));
                range.SetStyle(StyleLibrary, new Regex(@"\bMouseRawHooksAPI\b"));
                range.SetStyle(StyleClass, new Regex(@"\bKeyboardRawHook\b"));
                range.SetStyle(StyleClass, new Regex(@"\bMouseRawHook\b"));
                range.SetStyle(StyleObject, new Regex(@"\bmrh\b"));
                range.SetStyle(StyleObject, new Regex(@"\bkrh\b"));
                range.SetStyle(StyleObject, new Regex(@"\bmrh1\b"));
                range.SetStyle(StyleObject, new Regex(@"\bkrh1\b"));
                range.SetStyle(StyleObject, new Regex(@"\bmrh2\b"));
                range.SetStyle(StyleObject, new Regex(@"\bkrh2\b"));
                range.SetStyle(StyleLibrary, new Regex(@"\bScalingFactorAPI\b"));
                range.SetStyle(StyleClass, new Regex(@"\bScalingFactor\b"));
                range.SetStyle(StyleObject, new Regex(@"\bsf\b"));
                range.SetStyle(StyleInput, new Regex(@"\bscalingfactorx\b"));
                range.SetStyle(StyleInput, new Regex(@"\bscalingfactory\b"));
                range.SetStyle(StyleInput, new Regex(@"\bWiimoteGuitarStatePlus\b"));
                range.SetStyle(StyleInput, new Regex(@"\bWiimoteGuitarStateMinus\b"));
                range.SetStyle(StyleInput, new Regex(@"\bWiimoteGuitarStateStrumDown\b"));
                range.SetStyle(StyleInput, new Regex(@"\bWiimoteGuitarStateStrumUp\b"));
                range.SetStyle(StyleInput, new Regex(@"\bWiimoteGuitarStateFretBlue\b"));
                range.SetStyle(StyleInput, new Regex(@"\bWiimoteGuitarStateFretGreen\b"));
                range.SetStyle(StyleInput, new Regex(@"\bWiimoteGuitarStateFretOrange\b"));
                range.SetStyle(StyleInput, new Regex(@"\bWiimoteGuitarStateFretRed\b"));
                range.SetStyle(StyleInput, new Regex(@"\bWiimoteGuitarStateFretYellow\b"));
                range.SetStyle(StyleInput, new Regex(@"\bWiimoteGuitarStateRawJoystickX\b"));
                range.SetStyle(StyleInput, new Regex(@"\bWiimoteGuitarStateRawJoystickY\b"));
                range.SetStyle(StyleInput, new Regex(@"\bWiimoteGuitarStateRawWhammyBar\b"));
                range.SetStyle(StyleInput, new Regex(@"\bWiimoteClassicControllerStateA\b"));
                range.SetStyle(StyleInput, new Regex(@"\bWiimoteClassicControllerStateB\b"));
                range.SetStyle(StyleInput, new Regex(@"\bWiimoteClassicControllerStateDown\b"));
                range.SetStyle(StyleInput, new Regex(@"\bWiimoteClassicControllerStateHome\b"));
                range.SetStyle(StyleInput, new Regex(@"\bWiimoteClassicControllerStateLeft\b"));
                range.SetStyle(StyleInput, new Regex(@"\bWiimoteClassicControllerStateMinus\b"));
                range.SetStyle(StyleInput, new Regex(@"\bWiimoteClassicControllerStatePlus\b"));
                range.SetStyle(StyleInput, new Regex(@"\bWiimoteClassicControllerStateRight\b"));
                range.SetStyle(StyleInput, new Regex(@"\bWiimoteClassicControllerStateTriggerL\b"));
                range.SetStyle(StyleInput, new Regex(@"\bWiimoteClassicControllerStateTriggerR\b"));
                range.SetStyle(StyleInput, new Regex(@"\bWiimoteClassicControllerStateUp\b"));
                range.SetStyle(StyleInput, new Regex(@"\bWiimoteClassicControllerStateX\b"));
                range.SetStyle(StyleInput, new Regex(@"\bWiimoteClassicControllerStateY\b"));
                range.SetStyle(StyleInput, new Regex(@"\bWiimoteClassicControllerStateZL\b"));
                range.SetStyle(StyleInput, new Regex(@"\bWiimoteClassicControllerStateZR\b"));
                range.SetStyle(StyleInput, new Regex(@"\bWiimoteClassicControllerStateRawJoystickLeftX\b"));
                range.SetStyle(StyleInput, new Regex(@"\bWiimoteClassicControllerStateRawJoystickLeftY\b"));
                range.SetStyle(StyleInput, new Regex(@"\bWiimoteClassicControllerStateRawJoystickRightX\b"));
                range.SetStyle(StyleInput, new Regex(@"\bWiimoteClassicControllerStateRawJoystickRightY\b"));
                range.SetStyle(StyleInput, new Regex(@"\bWiimoteClassicControllerStateRawTriggerL\b"));
                range.SetStyle(StyleInput, new Regex(@"\bWiimoteClassicControllerStateRawTriggerR\b"));
                range.SetStyle(StyleInput, new Regex(@"\bWiimoteBalanceBoardStateCenterOfGravityX\b"));
                range.SetStyle(StyleInput, new Regex(@"\bWiimoteBalanceBoardStateCenterOfGravityY\b"));
                range.SetStyle(StyleInput, new Regex(@"\bWiimoteBalanceBoardStateSensorValuesKgBottomLeft\b"));
                range.SetStyle(StyleInput, new Regex(@"\bWiimoteBalanceBoardStateSensorValuesKgBottomRight\b"));
                range.SetStyle(StyleInput, new Regex(@"\bWiimoteBalanceBoardStateSensorValuesKgTopLeft\b"));
                range.SetStyle(StyleInput, new Regex(@"\bWiimoteBalanceBoardStateSensorValuesKgTopRight\b"));
                range.SetStyle(StyleInput, new Regex(@"\bWiimoteBalanceBoardStateWeightKg\b"));
                range.SetStyle(StyleInput, new Regex(@"\bWiimoteDrumsStateBlue\b"));
                range.SetStyle(StyleInput, new Regex(@"\bWiimoteDrumsStateBlueVelocity\b"));
                range.SetStyle(StyleInput, new Regex(@"\bWiimoteDrumsStateGreen\b"));
                range.SetStyle(StyleInput, new Regex(@"\bWiimoteDrumsStateGreenVelocity\b"));
                range.SetStyle(StyleInput, new Regex(@"\bWiimoteDrumsStateOrange\b"));
                range.SetStyle(StyleInput, new Regex(@"\bWiimoteDrumsStateOrangeVelocity\b"));
                range.SetStyle(StyleInput, new Regex(@"\bWiimoteDrumsStateRed\b"));
                range.SetStyle(StyleInput, new Regex(@"\bWiimoteDrumsStateRedVelocity\b"));
                range.SetStyle(StyleInput, new Regex(@"\bWiimoteDrumsStateYellow\b"));
                range.SetStyle(StyleInput, new Regex(@"\bWiimoteDrumsStateYellowVelocity\b"));
                range.SetStyle(StyleInput, new Regex(@"\bWiimoteDrumsStateMinus\b"));
                range.SetStyle(StyleInput, new Regex(@"\bWiimoteDrumsStatePlus\b"));
                range.SetStyle(StyleInput, new Regex(@"\bWiimoteDrumsStatePedal\b"));
                range.SetStyle(StyleInput, new Regex(@"\bWiimoteDrumsStatePedalVelocity\b"));
                range.SetStyle(StyleInput, new Regex(@"\bWiimoteDrumsStateRawJoystickX\b"));
                range.SetStyle(StyleInput, new Regex(@"\bWiimoteDrumsStateRawJoystickY\b")); 
                range.SetStyle(StyleInput, new Regex(@"\bWiimoteMotionPlusStateRawX\b"));
                range.SetStyle(StyleInput, new Regex(@"\bWiimoteMotionPlusStateRawY\b"));
                range.SetStyle(StyleInput, new Regex(@"\bWiimoteMotionPlusStateRawZ\b"));
                range.SetStyle(StyleInput, new Regex(@"\bWiimoteMotionPlusStatePitchFast\b"));
                range.SetStyle(StyleInput, new Regex(@"\bWiimoteMotionPlusStateRollFast\b"));
                range.SetStyle(StyleInput, new Regex(@"\bWiimoteMotionPlusStateYawFast\b"));
                range.SetStyle(StyleInput, new Regex(@"\bWiimoteTaikoDrumStateInnerLeft\b"));
                range.SetStyle(StyleInput, new Regex(@"\bWiimoteTaikoDrumStateOuterLeft\b"));
                range.SetStyle(StyleInput, new Regex(@"\bWiimoteTaikoDrumStateInnerRight\b"));
                range.SetStyle(StyleInput, new Regex(@"\bWiimoteTaikoDrumStateOuterRight\b"));
                range.SetStyle(StyleLibrary, new Regex(@"\bSound\b"));
                range.SetStyle(StyleClass, new Regex(@"\bPlayer\b"));
                range.SetStyle(StyleObject, new Regex(@"\bplayer\b"));
                range.SetStyle(StyleLibrary, new Regex(@"\bTimersAPI\b"));
                range.SetStyle(StyleClass, new Regex(@"\bTimer\b"));
                range.SetStyle(StyleObject, new Regex(@"\btimer\b"));
                range.SetStyle(StyleExtra, new Regex(@"\bpathsound1\b"));
                range.SetStyle(StyleExtra, new Regex(@"\bpathsound2\b"));
                range.SetStyle(StyleExtra, new Regex(@"\bpathsound3\b"));
                range.SetStyle(StyleExtra, new Regex(@"\bpathsound4\b"));
                range.SetStyle(StyleExtra, new Regex(@"\bpathsound5\b"));
                range.SetStyle(StyleExtra, new Regex(@"\bpathsound6\b"));
                range.SetStyle(StyleExtra, new Regex(@"\bpathsound7\b"));
                range.SetStyle(StyleExtra, new Regex(@"\bpathsound8\b"));
                range.SetStyle(StyleExtra, new Regex(@"\bpathsound9\b"));
                range.SetStyle(StyleExtra, new Regex(@"\bpathsound10\b"));
                range.SetStyle(StyleExtra, new Regex(@"\bpathsound11\b"));
                range.SetStyle(StyleExtra, new Regex(@"\bpathsound12\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bsound1\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bsound2\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bsound3\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bsound4\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bsound5\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bsound6\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bsound7\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bsound8\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bsound9\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bsound10\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bsound11\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bsound12\b"));
                range.SetStyle(StyleExtra, new Regex(@"\bpathtempsound1\b"));
                range.SetStyle(StyleExtra, new Regex(@"\bpathtempsound2\b"));
                range.SetStyle(StyleExtra, new Regex(@"\bpathtempsound3\b"));
                range.SetStyle(StyleExtra, new Regex(@"\bpathtempsound4\b"));
                range.SetStyle(StyleExtra, new Regex(@"\bpathtempsound5\b"));
                range.SetStyle(StyleExtra, new Regex(@"\bpathtempsound6\b"));
                range.SetStyle(StyleExtra, new Regex(@"\bpathtempsound7\b"));
                range.SetStyle(StyleExtra, new Regex(@"\bpathtempsound8\b"));
                range.SetStyle(StyleExtra, new Regex(@"\bpathtempsound9\b"));
                range.SetStyle(StyleExtra, new Regex(@"\bpathtempsound10\b"));
                range.SetStyle(StyleExtra, new Regex(@"\bpathtempsound11\b"));
                range.SetStyle(StyleExtra, new Regex(@"\bpathtempsound12\b"));
                range.SetStyle(StyleOutput, new Regex(@"\btempsound1\b"));
                range.SetStyle(StyleOutput, new Regex(@"\btempsound2\b"));
                range.SetStyle(StyleOutput, new Regex(@"\btempsound3\b"));
                range.SetStyle(StyleOutput, new Regex(@"\btempsound4\b"));
                range.SetStyle(StyleOutput, new Regex(@"\btempsound5\b"));
                range.SetStyle(StyleOutput, new Regex(@"\btempsound6\b"));
                range.SetStyle(StyleOutput, new Regex(@"\btempsound7\b"));
                range.SetStyle(StyleOutput, new Regex(@"\btempsound8\b"));
                range.SetStyle(StyleOutput, new Regex(@"\btempsound9\b"));
                range.SetStyle(StyleOutput, new Regex(@"\btempsound10\b"));
                range.SetStyle(StyleOutput, new Regex(@"\btempsound11\b"));
                range.SetStyle(StyleOutput, new Regex(@"\btempsound12\b"));
                range.SetStyle(StyleInput, new Regex(@"\btimeelapsed\b"));
                range.SetStyle(StyleInput, new Regex(@"\bdelay\b"));
                range.SetStyle(StyleClass, new Regex(@"\bMessageBox.Show\b"));
                range.SetStyle(StyleExtra, new Regex(@"\bToString\b"));
                range.SetStyle(StyleNone, new Regex(@"\w", RegexOptions.Singleline));
            }
            catch { }
        }
        private void FillAutocompletion()
        {
            string[] keywords = { 
                "abstract", 
                "as", 
                "base", 
                "bool", 
                "break", 
                "byte", 
                "case", 
                "catch", 
                "char", 
                "checked", 
                "class", 
                "const", 
                "continue", 
                "decimal", 
                "default", 
                "delegate", 
                "do", 
                "double", 
                "else", 
                "enum", 
                "event", 
                "explicit", 
                "extern", 
                "false", 
                "finally", 
                "fixed", 
                "float", 
                "for", 
                "foreach", 
                "goto", 
                "if", 
                "implicit", 
                "in", 
                "int", 
                "interface", 
                "internal", 
                "is", 
                "lock", 
                "long", 
                "namespace", 
                "new", 
                "null", 
                "object", 
                "operator", 
                "out", 
                "override", 
                "params", 
                "private", 
                "protected", 
                "public", 
                "readonly", 
                "ref", 
                "return", 
                "sbyte", 
                "sealed", 
                "short", 
                "sizeof", 
                "stackalloc", 
                "static", 
                "string", 
                "struct", 
                "switch", 
                "this", 
                "throw", 
                "true", 
                "try", 
                "typeof", 
                "uint", 
                "ulong", 
                "unchecked", 
                "unsafe", 
                "ushort", 
                "using", 
                "virtual", 
                "void", 
                "volatile", 
                "while", 
                "add", 
                "alias", 
                "ascending", 
                "descending", 
                "dynamic", 
                "from", 
                "get", 
                "global", 
                "group", 
                "into", 
                "join", 
                "let", 
                "orderby", 
                "partial", 
                "remove", 
                "select", 
                "set", 
                "value", 
                "var", 
                "where", 
                "yield",
                "void",
                "bool",
                "int",
                "float",
                "double",
                "Double",
                "List",
                "[",
                "]",
                "if",
                "input",
                "/* */",
                "this",
                "XBC",
                "XBC1",
                "XBC2",
                "xi",
                "xi1",
                "xi2",
                "DS4",
                "DS41",
                "DS42",
                "VJoy",
                "VJoy1",
                "VJoy2",
                "di",
                "di1",
                "di2",
                "ds",
                "ds1",
                "ds2",
                "ds4",
                "ds41",
                "ds42",
                "SI",
                "wm",
                "wm1",
                "wm2",
                "spc",
                "spc1",
                "spc2",
                "jcg",
                "jcg1",
                "jcg2",
                "jl",
                "jl1",
                "jl2",
                "jr",
                "jr1",
                "jr2",
                "mi",
                "ki",
                "mi1",
                "ki1",
                "mi2",
                "ki2",
                "SKM",
                "kh",
                "mh",
                "gh",
                "gh1",
                "gh2",
                "jh",
                "jh1",
                "jh2",
                "mri",
                "kri",
                "mxh",
                "kxh",
                "mrh",
                "krh",
                "mrh1",
                "krh1",
                "mrh2",
                "krh2",
                "sf",
                "DllImport",
                "EntryPoint",
                "TimeBeginPeriod",
                "TimeEndPeriod",
                "NtSetTimerResolution",
                "List",
                "ms",
                "CurrentResolution",
                "DesiredResolution",
                "SetResolution",
                "running",
                "irmode",
                "number",
                "centery",
                "vendor_ds_id",
                "product_ds_id",
                "product_ds_label",
                "vendor_ds4_id",
                "product_ds4_id",
                "product_ds4_label",
                "Start",
                "statex",
                "statey",
                "mousex",
                "mousey",
                "mousexp",
                "mouseyp",
                "mousestatex",
                "mousestatey",
                "dzx",
                "dzy",
                "viewpower1x",
                "viewpower2x",
                "viewpower3x",
                "viewpower1y",
                "viewpower2y",
                "viewpower3y",
                "viewpower05x",
                "viewpower05y",
                "task",
                "sleeptime",
                "Task",
                "Thread",
                "Sleep",
                "StringToCode",
                "FooClass",
                "DllImport",
                "System",
                "Globalization",
                "IO",
                "Numerics",
                "Drawing",
                "Runtime",
                "InteropServices",
                "Threading",
                "Tasks",
                "Windows",
                "Forms",
                "Reflection",
                "Diagnostics",
                "Collections",
                "Generic",
                "Linq",
                "Vectors",
                "Core",
                "valListX",
                "valListY",
                "Valuechanges",
                "Valuechange",
                "ValueChange",
                "_ValueChange",
                "controllers",
                "XBoxController",
                "XInputsAPI",
                "XInput",
                "controllersds4",
                "DS4Controller",
                "controllersvjoy",
                "VJoyController",
                "DirectInputsAPI",
                "DirectInput",
                "DualSensesAPI",
                "DualSense",
                "DualShocks4API",
                "DualShock4",
                "SendInputs",
                "Sendinput",
                "sendinput",
                "Interceptions",
                "SendInterception",
                "Load",
                "Main",
                "WiiMotesAPI",
                "WiiMote",
                "WiiMotesLibAPI",
                "WiiMoteLib",
                "WiiMotesLibMPAPI",
                "WiiMoteLibMP",
                "SwitchProControllersAPI",
                "SwitchProController",
                "JoyconChargingGripsAPI",
                "JoyconChargingGrip",
                "JoyconsLeftAPI",
                "JoyconLeft",
                "JoyconsRightAPI",
                "JoyconRight",
                "KeyboardInputsAPI",
                "MouseInputsAPI",
                "KeyboardInput",
                "MouseInput",
                "keyboardsmouses",
                "SendKeyboardMouse",
                "getstate",
                "pollcount",
                "min",
                "max",
                "minScale",
                "maxScale",
                "value",
                "val",
                "n",
                "scaled",
                "System.Windows.Forms.Screen.PrimaryScreen.Bounds.Width",
                "System.Windows.Forms.Screen.PrimaryScreen.Bounds.Height",
                "wd",
                "wu",
                "valchanged",
                "Scale",
                "width",
                "height",
                "Controller_Send_xbox",
                "Controller_Send_back",
                "Controller_Send_start",
                "Controller_Send_A",
                "Controller_Send_B",
                "Controller_Send_X",
                "Controller_Send_Y",
                "Controller_Send_up",
                "Controller_Send_left",
                "Controller_Send_down",
                "Controller_Send_right",
                "Controller_Send_leftstick",
                "Controller_Send_rightstick",
                "Controller_Send_leftbumper",
                "Controller_Send_rightbumper",
                "Controller_Send_lefttriggerposition",
                "Controller_Send_righttriggerposition",
                "Controller_Send_leftstickx",
                "Controller_Send_leftsticky",
                "Controller_Send_rightstickx",
                "Controller_Send_rightsticky",
                "ControllerDS4_Send_Options",
                "ControllerDS4_Send_ThumbLeft",
                "ControllerDS4_Send_ThumbRight",
                "ControllerDS4_Send_ShoulderLeft",
                "ControllerDS4_Send_ShoulderRight",
                "ControllerDS4_Send_Cross",
                "ControllerDS4_Send_Circle",
                "ControllerDS4_Send_Square",
                "ControllerDS4_Send_Triangle",
                "ControllerDS4_Send_Ps",
                "ControllerDS4_Send_Touchpad",
                "ControllerDS4_Send_Share",
                "ControllerDS4_Send_DPadUp",
                "ControllerDS4_Send_DPadDown",
                "ControllerDS4_Send_DPadLeft",
                "ControllerDS4_Send_DPadRight",
                "ControllerDS4_Send_LeftTrigger",
                "ControllerDS4_Send_RightTrigger",
                "ControllerDS4_Send_LeftTriggerPosition",
                "ControllerDS4_Send_RightTriggerPosition",
                "ControllerDS4_Send_LeftThumbX",
                "ControllerDS4_Send_RightThumbX",
                "ControllerDS4_Send_LeftThumbY",
                "ControllerDS4_Send_RightThumbY",
                "ControllerVJoy_Send_1",
                "ControllerVJoy_Send_2",
                "ControllerVJoy_Send_3",
                "ControllerVJoy_Send_4",
                "ControllerVJoy_Send_5",
                "ControllerVJoy_Send_6",
                "ControllerVJoy_Send_7",
                "ControllerVJoy_Send_8",
                "ControllerVJoy_Send_X",
                "ControllerVJoy_Send_Y",
                "ControllerVJoy_Send_Z",
                "ControllerVJoy_Send_WHL",
                "ControllerVJoy_Send_SL0",
                "ControllerVJoy_Send_SL1",
                "ControllerVJoy_Send_RX",
                "ControllerVJoy_Send_RY",
                "ControllerVJoy_Send_RZ",
                "ControllerVJoy_Send_POV",
                "ControllerVJoy_Send_Hat",
                "ControllerVJoy_Send_HatExt1",
                "ControllerVJoy_Send_HatExt2",
                "ControllerVJoy_Send_HatExt3",
                "keyboard_1_id",
                "mouse_1_id",
                "keyboard_2_id",
                "mouse_2_id",
                "int_deltaX",
                "int_deltaY",
                "int_x",
                "int_y",
                "int_SendLeftClick",
                "int_SendRightClick",
                "int_SendMiddleClick",
                "int_SendWheelUp",
                "int_SendWheelDown",
                "int_SendCANCEL",
                "int_SendBACK",
                "int_SendTAB",
                "int_SendCLEAR",
                "int_SendRETURN",
                "int_SendSHIFT",
                "int_SendCONTROL",
                "int_SendMENU",
                "int_SendCAPITAL",
                "int_SendESCAPE",
                "int_SendSPACE",
                "int_SendPRIOR",
                "int_SendNEXT",
                "int_SendEND",
                "int_SendHOME",
                "int_SendLEFT",
                "int_SendUP",
                "int_SendRIGHT",
                "int_SendDOWN",
                "int_SendSNAPSHOT",
                "int_SendINSERT",
                "int_SendNUMPADDEL",
                "int_SendNUMPADINSERT",
                "int_SendHELP",
                "int_SendAPOSTROPHE",
                "int_SendBACKSPACE",
                "int_SendPAGEDOWN",
                "int_SendPAGEUP",
                "int_SendFIN",
                "int_SendMOUSE",
                "int_SendA",
                "int_SendB",
                "int_SendC",
                "int_SendD",
                "int_SendE",
                "int_SendF",
                "int_SendG",
                "int_SendH",
                "int_SendI",
                "int_SendJ",
                "int_SendK",
                "int_SendL",
                "int_SendM",
                "int_SendN",
                "int_SendO",
                "int_SendP",
                "int_SendQ",
                "int_SendR",
                "int_SendS",
                "int_SendT",
                "int_SendU",
                "int_SendV",
                "int_SendW",
                "int_SendX",
                "int_SendY",
                "int_SendZ",
                "int_SendLWIN",
                "int_SendRWIN",
                "int_SendAPPS",
                "int_SendDELETE",
                "int_SendNUMPAD0",
                "int_SendNUMPAD1",
                "int_SendNUMPAD2",
                "int_SendNUMPAD3",
                "int_SendNUMPAD4",
                "int_SendNUMPAD5",
                "int_SendNUMPAD6",
                "int_SendNUMPAD7",
                "int_SendNUMPAD8",
                "int_SendNUMPAD9",
                "int_SendMULTIPLY",
                "int_SendADD",
                "int_SendSUBTRACT",
                "int_SendDECIMAL",
                "int_SendPRINTSCREEN",
                "int_SendDIVIDE",
                "int_SendF1",
                "int_SendF2",
                "int_SendF3",
                "int_SendF4",
                "int_SendF5",
                "int_SendF6",
                "int_SendF7",
                "int_SendF8",
                "int_SendF9",
                "int_SendF10",
                "int_SendF11",
                "int_SendF12",
                "int_SendNUMLOCK",
                "int_SendSCROLLLOCK",
                "int_SendLEFTSHIFT",
                "int_SendRIGHTSHIFT",
                "int_SendLEFTCONTROL",
                "int_SendRIGHTCONTROL",
                "int_SendLEFTALT",
                "int_SendRIGHTALT",
                "int_SendBROWSER_BACK",
                "int_SendBROWSER_FORWARD",
                "int_SendBROWSER_REFRESH",
                "int_SendBROWSER_STOP",
                "int_SendBROWSER_SEARCH",
                "int_SendBROWSER_FAVORITES",
                "int_SendBROWSER_HOME",
                "int_SendVOLUME_MUTE",
                "int_SendVOLUME_DOWN",
                "int_SendVOLUME_UP",
                "int_SendMEDIA_NEXT_TRACK",
                "int_SendMEDIA_PREV_TRACK",
                "int_SendMEDIA_STOP",
                "int_SendMEDIA_PLAY_PAUSE",
                "int_SendLAUNCH_MAIL",
                "int_SendLAUNCH_MEDIA_SELECT",
                "int_SendLAUNCH_APP1",
                "int_SendLAUNCH_APP2",
                "int_SendOEM_1",
                "int_SendOEM_PLUS",
                "int_SendOEM_COMMA",
                "int_SendOEM_MINUS",
                "int_SendOEM_PERIOD",
                "int_SendOEM_2",
                "int_SendOEM_3",
                "int_SendOEM_4",
                "int_SendOEM_5",
                "int_SendOEM_6",
                "int_SendOEM_7",
                "int_SendOEM_8",
                "int_SendOEM_102",
                "int_SendEREOF",
                "int_SendZOOM",
                "int_SendEscape",
                "int_SendOne",
                "int_SendTwo",
                "int_SendThree",
                "int_SendFour",
                "int_SendFive",
                "int_SendSix",
                "int_SendSeven",
                "int_SendEight",
                "int_SendNine",
                "int_SendZero",
                "int_SendDashUnderscore",
                "int_SendPlusEquals",
                "int_SendBackspace",
                "int_SendTab",
                "int_SendOpenBracketBrace",
                "int_SendCloseBracketBrace",
                "int_SendEnter",
                "int_SendControl",
                "int_SendSemicolonColon",
                "int_SendSingleDoubleQuote",
                "int_SendTilde",
                "int_SendLeftShift",
                "int_SendBackslashPipe",
                "int_SendCommaLeftArrow",
                "int_SendPeriodRightArrow",
                "int_SendForwardSlashQuestionMark",
                "int_SendRightShift",
                "int_SendRightAlt",
                "int_SendSpace",
                "int_SendCapsLock",
                "int_SendUp",
                "int_SendDown",
                "int_SendRight",
                "int_SendLeft",
                "int_SendHome",
                "int_SendEnd",
                "int_SendDelete",
                "int_SendPageUp",
                "int_SendPageDown",
                "int_SendInsert",
                "int_SendPrintScreen",
                "int_SendNumLock",
                "int_SendScrollLock",
                "int_SendMenu",
                "int_SendWindowsKey",
                "int_SendNumpadDivide",
                "int_SendNumpadAsterisk",
                "int_SendNumpad7",
                "int_SendNumpad8",
                "int_SendNumpad9",
                "int_SendNumpad4",
                "int_SendNumpad5",
                "int_SendNumpad6",
                "int_SendNumpad1",
                "int_SendNumpad2",
                "int_SendNumpad3",
                "int_SendNumpad0",
                "int_SendNumpadDelete",
                "int_SendNumpadEnter",
                "int_SendNumpadPlus",
                "int_SendNumpadMinus",
                "KeyboardMouseDriverType",
                "MouseMoveX",
                "MouseMoveY",
                "MouseAbsX",
                "MouseAbsY",
                "MouseDesktopX",
                "MouseDesktopY",
                "SendLeftClick",
                "SendRightClick",
                "SendMiddleClick",
                "SendWheelUp",
                "SendWheelDown",
                "SendLeft",
                "SendRight",
                "SendUp",
                "SendDown",
                "SendLButton",
                "SendRButton",
                "SendCancel",
                "SendMBUTTON",
                "SendXBUTTON1",
                "SendXBUTTON2",
                "SendBack",
                "SendTab",
                "SendClear",
                "SendReturn",
                "SendSHIFT",
                "SendCONTROL",
                "SendMENU",
                "SendPAUSE",
                "SendCAPITAL",
                "SendKANA",
                "SendHANGEUL",
                "SendHANGUL",
                "SendJUNJA",
                "SendFINAL",
                "SendHANJA",
                "SendKANJI",
                "SendEscape",
                "SendCONVERT",
                "SendNONCONVERT",
                "SendACCEPT",
                "SendMODECHANGE",
                "SendSpace",
                "SendPRIOR",
                "SendNEXT",
                "SendEND",
                "SendHOME",
                "SendLEFT",
                "SendUP",
                "SendRIGHT",
                "SendDOWN",
                "SendSELECT",
                "SendPRINT",
                "SendEXECUTE",
                "SendSNAPSHOT",
                "SendINSERT",
                "SendDELETE",
                "SendHELP",
                "SendAPOSTROPHE",
                "Send0",
                "Send1",
                "Send2",
                "Send3",
                "Send4",
                "Send5",
                "Send6",
                "Send7",
                "Send8",
                "Send9",
                "SendA",
                "SendB",
                "SendC",
                "SendD",
                "SendE",
                "SendF",
                "SendG",
                "SendH",
                "SendI",
                "SendJ",
                "SendK",
                "SendL",
                "SendM",
                "SendN",
                "SendO",
                "SendP",
                "SendQ",
                "SendR",
                "SendS",
                "SendT",
                "SendU",
                "SendV",
                "SendW",
                "SendX",
                "SendY",
                "SendZ",
                "SendLWIN",
                "SendRWIN",
                "SendAPPS",
                "SendSLEEP",
                "SendNUMPAD0",
                "SendNUMPAD1",
                "SendNUMPAD2",
                "SendNUMPAD3",
                "SendNUMPAD4",
                "SendNUMPAD5",
                "SendNUMPAD6",
                "SendNUMPAD7",
                "SendNUMPAD8",
                "SendNUMPAD9",
                "SendMULTIPLY",
                "SendADD",
                "SendSEPARATOR",
                "SendSUBTRACT",
                "SendDECIMAL",
                "SendDIVIDE",
                "SendF1",
                "SendF2",
                "SendF3",
                "SendF4",
                "SendF5",
                "SendF6",
                "SendF7",
                "SendF8",
                "SendF9",
                "SendF10",
                "SendF11",
                "SendF12",
                "SendF13",
                "SendF14",
                "SendF15",
                "SendF16",
                "SendF17",
                "SendF18",
                "SendF19",
                "SendF20",
                "SendF21",
                "SendF22",
                "SendF23",
                "SendF24",
                "SendNUMLOCK",
                "SendSCROLL",
                "SendLeftShift",
                "SendRightShift",
                "SendLeftControl",
                "SendRightControl",
                "SendLMENU",
                "SendRMENU",
                "CameraAPI",
                "CameraToLed",
                "camera",
                "SpeechAPI",
                "SpeechToText",
                "speech",
                "KeyboardHooksAPI",
                "KeyboardHooks",
                "MouseHooksAPI",
                "MouseHooks",
                "GamepadsHooksAPI",
                "GamepadsHooks",
                "JoysticksHooksAPI",
                "JoysticksHooks",
                "byteArrayToControl",
                "controlToByteArray",
                "stringToControl",
                "Networkshost",
                "NetworkHost",
                "Networks",
                "Network",
                "WebSocketSharp",
                "WebSocket",
                "Ws_OnMessage",
                "wsc",
                "object",
                "MessageEventArgs",
                "byteArrayIn",
                "control",
                "Encoding.ASCII.GetString",
                "str",
                "localip",
                "port",
                "Text",
                "sender",
                "e",
                "Convert",
                "connectionString",
                "data",
                "Encoding.ASCII.GetBytes",
                "unsplitstring",
                "splitstring",
                "newsplitstring",
                "valuestring",
                "pFrom",
                "newvaluestring",
                "pTo",
                "result",
                "KeyboardRawInputsAPI",
                "MouseRawInputsAPI",
                "KeyboardRawInputs",
                "MouseRawInputs",
                "KeyboardXnaHookAPI",
                "MouseXnaHookAPI",
                "KeyboardXnaHook",
                "MouseXnaHook",
                "KeyboardRawHooksAPI",
                "MouseRawHooksAPI",
                "KeyboardRawHook",
                "MouseRawHook",
                "ScalingFactorAPI",
                "ScalingFactor",
                "Sound",
                "Player",
                "player",
                "TimersAPI",
                "Timer",
                "timer",
                "pathsound1",
                "pathsound2",
                "pathsound3",
                "pathsound4",
                "pathsound5",
                "pathsound6",
                "pathsound7",
                "pathsound8",
                "pathsound9",
                "pathsound10",
                "pathsound11",
                "pathsound12",
                "sound1",
                "sound2",
                "sound3",
                "sound4",
                "sound5",
                "sound6",
                "sound7",
                "sound8",
                "sound9",
                "sound10",
                "sound11",
                "sound12",
                "pathtempsound1",
                "pathtempsound2",
                "pathtempsound3",
                "pathtempsound4",
                "pathtempsound5",
                "pathtempsound6",
                "pathtempsound7",
                "pathtempsound8",
                "pathtempsound9",
                "pathtempsound10",
                "pathtempsound11",
                "pathtempsound12",
                "tempsound1",
                "tempsound2",
                "tempsound3",
                "tempsound4",
                "tempsound5",
                "tempsound6",
                "tempsound7",
                "tempsound8",
                "tempsound9",
                "tempsound10",
                "tempsound11",
                "tempsound12",
                "MessageBox.Show",
                "delay"
            };
            string[] methods = {
                "ToInt32",
                "ToString()",
                "RemoveAt(0)",
                "Count",
                "Clear",
                "RemoveAt",
                "Add",
                "Average",
                "Average",
                "ViewData",
                "Close",
                "BeginPolling",
                "Connect",
                "Disconnect",
                "Set",
                "Run",
                "Scan",
                "Init",
                "Math",
                "Abs",
                "Sign",
                "Round",
                "Pow",
                "Sqrt",
                "Log",
                "Exp",
                "Min",
                "Max",
                "Floor",
                "Truncate",
                "IsAlive",
                "Send",
                "ToSingle",
                "Parse",
                "ToArray",
                "IndexOf",
                "OnMessage",
                "Length",
                "Substring",
                "Replace",
                "Split",
                "Key_LBUTTON",
                "Key_RBUTTON",
                "Key_CANCEL",
                "Key_MBUTTON",
                "Key_XBUTTON1",
                "Key_XBUTTON2",
                "Key_BACK",
                "Key_Tab",
                "Key_CLEAR",
                "Key_Return",
                "Key_SHIFT",
                "Key_CONTROL",
                "Key_MENU",
                "Key_PAUSE",
                "Key_CAPITAL",
                "Key_KANA",
                "Key_HANGEUL",
                "Key_HANGUL",
                "Key_JUNJA",
                "Key_FINAL",
                "Key_HANJA",
                "Key_KANJI",
                "Key_Escape",
                "Key_CONVERT",
                "Key_NONCONVERT",
                "Key_ACCEPT",
                "Key_MODECHANGE",
                "Key_Space",
                "Key_PRIOR",
                "Key_NEXT",
                "Key_END",
                "Key_HOME",
                "Key_LEFT",
                "Key_UP",
                "Key_RIGHT",
                "Key_DOWN",
                "Key_SELECT",
                "Key_PRINT",
                "Key_EXECUTE",
                "Key_SNAPSHOT",
                "Key_INSERT",
                "Key_DELETE",
                "Key_HELP",
                "Key_APOSTROPHE",
                "Key_0",
                "Key_1",
                "Key_2",
                "Key_3",
                "Key_4",
                "Key_5",
                "Key_6",
                "Key_7",
                "Key_8",
                "Key_9",
                "Key_A",
                "Key_B",
                "Key_C",
                "Key_D",
                "Key_E",
                "Key_F",
                "Key_G",
                "Key_H",
                "Key_I",
                "Key_J",
                "Key_K",
                "Key_L",
                "Key_M",
                "Key_N",
                "Key_O",
                "Key_P",
                "Key_Q",
                "Key_R",
                "Key_S",
                "Key_T",
                "Key_U",
                "Key_V",
                "Key_W",
                "Key_X",
                "Key_Y",
                "Key_Z",
                "Key_LWIN",
                "Key_RWIN",
                "Key_APPS",
                "Key_SLEEP",
                "Key_NUMPAD0",
                "Key_NUMPAD1",
                "Key_NUMPAD2",
                "Key_NUMPAD3",
                "Key_NUMPAD4",
                "Key_NUMPAD5",
                "Key_NUMPAD6",
                "Key_NUMPAD7",
                "Key_NUMPAD8",
                "Key_NUMPAD9",
                "Key_MULTIPLY",
                "Key_ADD",
                "Key_SEPARATOR",
                "Key_SUBTRACT",
                "Key_DECIMAL",
                "Key_DIVIDE",
                "Key_F1",
                "Key_F2",
                "Key_F3",
                "Key_F4",
                "Key_F5",
                "Key_F6",
                "Key_F7",
                "Key_F8",
                "Key_F9",
                "Key_F10",
                "Key_F11",
                "Key_F12",
                "Key_F13",
                "Key_F14",
                "Key_F15",
                "Key_F16",
                "Key_F17",
                "Key_F18",
                "Key_F19",
                "Key_F20",
                "Key_F21",
                "Key_F22",
                "Key_F23",
                "Key_F24",
                "Key_NUMLOCK",
                "Key_SCROLL",
                "Key_LeftShift",
                "Key_RightShift",
                "Key_LeftControl",
                "Key_RightControl",
                "Key_LMENU",
                "Key_RMENU",
                "Key_BROWSER_BACK",
                "Key_BROWSER_FORWARD",
                "Key_BROWSER_REFRESH",
                "Key_BROWSER_STOP",
                "Key_BROWSER_SEARCH",
                "Key_BROWSER_FAVORITES",
                "Key_BROWSER_HOME",
                "Key_VOLUME_MUTE",
                "Key_VOLUME_DOWN",
                "Key_VOLUME_UP",
                "Key_MEDIA_NEXT_TRACK",
                "Key_MEDIA_PREV_TRACK",
                "Key_MEDIA_STOP",
                "Key_MEDIA_PLAY_PAUSE",
                "Key_LAUNCH_MAIL",
                "Key_LAUNCH_MEDIA_SELECT",
                "Key_LAUNCH_APP1",
                "Key_LAUNCH_APP2",
                "Key_OEM_1",
                "Key_OEM_PLUS",
                "Key_OEM_COMMA",
                "Key_OEM_MINUS",
                "Key_OEM_PERIOD",
                "Key_OEM_2",
                "Key_OEM_3",
                "Key_OEM_4",
                "Key_OEM_5",
                "Key_OEM_6",
                "Key_OEM_7",
                "Key_OEM_8",
                "Key_OEM_102",
                "Key_PROCESS\bKey",
                "Key_PACKET",
                "Key_ATTN",
                "Key_CRSEL",
                "Key_EXSEL",
                "Key_EREOF",
                "Key_PLAY",
                "Key_ZOOM",
                "Key_NONAME",
                "Key_PA1",
                "Key_OEM_CLEAR",
                "CursorX",
                "CursorY",
                "MouseX",
                "MouseY",
                "MouseZ",
                "MouseRightButton",
                "MouseLeftButton",
                "MouseMiddleButton",
                "MouseXButton",
                "MouseButtonX",
                "speechtext",
                "camx",
                "camy",
                "irx",
                "iry",
                "WiimoteButtonStateA",
                "WiimoteButtonStateB",
                "WiimoteButtonStateMinus",
                "WiimoteButtonStateHome",
                "WiimoteButtonStatePlus",
                "WiimoteButtonStateOne",
                "WiimoteButtonStateTwo",
                "WiimoteButtonStateUp",
                "WiimoteButtonStateDown",
                "WiimoteButtonStateLeft",
                "WiimoteButtonStateRight",
                "WiimoteRawValuesX",
                "WiimoteRawValuesY",
                "WiimoteRawValuesZ",
                "WiimoteNunchuckStateRawJoystickX",
                "WiimoteNunchuckStateRawJoystickY",
                "WiimoteNunchuckStateRawValuesX",
                "WiimoteNunchuckStateRawValuesY",
                "WiimoteNunchuckStateRawValuesZ",
                "WiimoteNunchuckStateC",
                "WiimoteNunchuckStateZ",
                "JoyconRightStickX",
                "JoyconRightStickY",
                "JoyconRightButtonSHOULDER_1",
                "JoyconRightButtonSHOULDER_2",
                "JoyconRightButtonSR",
                "JoyconRightButtonSL",
                "JoyconRightButtonDPAD_DOWN",
                "JoyconRightButtonDPAD_RIGHT",
                "JoyconRightButtonDPAD_UP",
                "JoyconRightButtonDPAD_LEFT",
                "JoyconRightButtonPLUS",
                "JoyconRightButtonHOME",
                "JoyconRightButtonSTICK",
                "JoyconRightButtonACC",
                "JoyconRightButtonSPA",
                "JoyconRightRollLeft",
                "JoyconRightRollRight",
                "JoyconRightAccelX",
                "JoyconRightAccelY",
                "JoyconRightGyroX",
                "JoyconRightGyroY",
                "JoyconLeftStickX",
                "JoyconLeftStickY",
                "JoyconLeftButtonSHOULDER_1",
                "JoyconLeftButtonSHOULDER_2",
                "JoyconLeftButtonSR",
                "JoyconLeftButtonSL",
                "JoyconLeftButtonDPAD_DOWN",
                "JoyconLeftButtonDPAD_RIGHT",
                "JoyconLeftButtonDPAD_UP",
                "JoyconLeftButtonDPAD_LEFT",
                "JoyconLeftButtonMINUS",
                "JoyconLeftButtonCAPTURE",
                "JoyconLeftButtonSTICK",
                "JoyconLeftButtonACC",
                "JoyconLeftButtonSMA",
                "JoyconLeftRollLeft",
                "JoyconLeftRollRight",
                "JoyconLeftAccelX",
                "JoyconLeftAccelY",
                "JoyconLeftGyroX",
                "JoyconLeftGyroY",
                "ProControllerLeftStickX",
                "ProControllerLeftStickY",
                "ProControllerRightStickX",
                "ProControllerRightStickY",
                "ProControllerButtonSHOULDER_Left_1",
                "ProControllerButtonSHOULDER_Left_2",
                "ProControllerButtonDPAD_DOWN",
                "ProControllerButtonDPAD_RIGHT",
                "ProControllerButtonDPAD_UP",
                "ProControllerButtonDPAD_LEFT",
                "ProControllerButtonMINUS",
                "ProControllerButtonCAPTURE",
                "ProControllerButtonSTICK_Left",
                "ProControllerButtonSHOULDER_Right_1",
                "ProControllerButtonSHOULDER_Right_2",
                "ProControllerButtonA",
                "ProControllerButtonB",
                "ProControllerButtonX",
                "ProControllerButtonY",
                "ProControllerButtonPLUS",
                "ProControllerButtonHOME",
                "ProControllerButtonSTICK_Right",
                "ProControllerAccelX",
                "ProControllerAccelY",
                "ProControllerGyroX",
                "ProControllerGyroY",
                "ControllerButtonAPressed",
                "ControllerButtonBPressed",
                "ControllerButtonXPressed",
                "ControllerButtonYPressed",
                "ControllerButtonStartPressed",
                "ControllerButtonBackPressed",
                "ControllerButtonDownPressed",
                "ControllerButtonUpPressed",
                "ControllerButtonLeftPressed",
                "ControllerButtonRightPressed",
                "ControllerButtonShoulderLeftPressed",
                "ControllerButtonShoulderRightPressed",
                "ControllerThumbpadLeftPressed",
                "ControllerThumbpadRightPressed",
                "ControllerTriggerLeftPosition",
                "ControllerTriggerRightPosition",
                "ControllerThumbLeftX",
                "ControllerThumbLeftY",
                "ControllerThumbRightX",
                "ControllerThumbRightY",
                "MouseButtons0",
                "MouseButtons1",
                "MouseButtons2",
                "MouseButtons3",
                "MouseButtons4",
                "MouseButtons5",
                "MouseButtons6",
                "MouseButtons7",
                "MouseAxisX",
                "MouseAxisY",
                "MouseAxisZ",
                "KeyboardKeyEscape",
                "KeyboardKeyD1",
                "KeyboardKeyD2",
                "KeyboardKeyD3",
                "KeyboardKeyD4",
                "KeyboardKeyD5",
                "KeyboardKeyD6",
                "KeyboardKeyD7",
                "KeyboardKeyD8",
                "KeyboardKeyD9",
                "KeyboardKeyD0",
                "KeyboardKeyMinus",
                "KeyboardKeyEquals",
                "KeyboardKeyBack",
                "KeyboardKeyTab",
                "KeyboardKeyQ",
                "KeyboardKeyW",
                "KeyboardKeyE",
                "KeyboardKeyR",
                "KeyboardKeyT",
                "KeyboardKeyY",
                "KeyboardKeyU",
                "KeyboardKeyI",
                "KeyboardKeyO",
                "KeyboardKeyP",
                "KeyboardKeyLeftBracket",
                "KeyboardKeyRightBracket",
                "KeyboardKeyReturn",
                "KeyboardKeyLeftControl",
                "KeyboardKeyA",
                "KeyboardKeyS",
                "KeyboardKeyD",
                "KeyboardKeyF",
                "KeyboardKeyG",
                "KeyboardKeyH",
                "KeyboardKeyJ",
                "KeyboardKeyK",
                "KeyboardKeyL",
                "KeyboardKeySemicolon",
                "KeyboardKeyApostrophe",
                "KeyboardKeyGrave",
                "KeyboardKeyLeftShift",
                "KeyboardKeyBackslash",
                "KeyboardKeyZ",
                "KeyboardKeyX",
                "KeyboardKeyC",
                "KeyboardKeyV",
                "KeyboardKeyB",
                "KeyboardKeyN",
                "KeyboardKeyM",
                "KeyboardKeyComma",
                "KeyboardKeyPeriod",
                "KeyboardKeySlash",
                "KeyboardKeyRightShift",
                "KeyboardKeyMultiply",
                "KeyboardKeyLeftAlt",
                "KeyboardKeySpace",
                "KeyboardKeyCapital",
                "KeyboardKeyF1",
                "KeyboardKeyF2",
                "KeyboardKeyF3",
                "KeyboardKeyF4",
                "KeyboardKeyF5",
                "KeyboardKeyF6",
                "KeyboardKeyF7",
                "KeyboardKeyF8",
                "KeyboardKeyF9",
                "KeyboardKeyF10",
                "KeyboardKeyNumberLock",
                "KeyboardKeyScrollLock",
                "KeyboardKeyNumberPad7",
                "KeyboardKeyNumberPad8",
                "KeyboardKeyNumberPad9",
                "KeyboardKeySubtract",
                "KeyboardKeyNumberPad4",
                "KeyboardKeyNumberPad5",
                "KeyboardKeyNumberPad6",
                "KeyboardKeyAdd",
                "KeyboardKeyNumberPad1",
                "KeyboardKeyNumberPad2",
                "KeyboardKeyNumberPad3",
                "KeyboardKeyNumberPad0",
                "KeyboardKeyDecimal",
                "KeyboardKeyOem102",
                "KeyboardKeyF11",
                "KeyboardKeyF12",
                "KeyboardKeyF13",
                "KeyboardKeyF14",
                "KeyboardKeyF15",
                "KeyboardKeyKana",
                "KeyboardKeyAbntC1",
                "KeyboardKeyConvert",
                "KeyboardKeyNoConvert",
                "KeyboardKeyYen",
                "KeyboardKeyAbntC2",
                "KeyboardKeyNumberPadEquals",
                "KeyboardKeyPreviousTrack",
                "KeyboardKeyAT",
                "KeyboardKeyColon",
                "KeyboardKeyUnderline",
                "KeyboardKeyKanji",
                "KeyboardKeyStop",
                "KeyboardKeyAX",
                "KeyboardKeyUnlabeled",
                "KeyboardKeyNextTrack",
                "KeyboardKeyNumberPadEnter",
                "KeyboardKeyRightControl",
                "KeyboardKeyMute",
                "KeyboardKeyCalculator",
                "KeyboardKeyPlayPause",
                "KeyboardKeyMediaStop",
                "KeyboardKeyVolumeDown",
                "KeyboardKeyVolumeUp",
                "KeyboardKeyWebHome",
                "KeyboardKeyNumberPadComma",
                "KeyboardKeyDivide",
                "KeyboardKeyPrintScreen",
                "KeyboardKeyRightAlt",
                "KeyboardKeyPause",
                "KeyboardKeyHome",
                "KeyboardKeyUp",
                "KeyboardKeyPageUp",
                "KeyboardKeyLeft",
                "KeyboardKeyRight",
                "KeyboardKeyEnd",
                "KeyboardKeyDown",
                "KeyboardKeyPageDown",
                "KeyboardKeyInsert",
                "KeyboardKeyDelete",
                "KeyboardKeyLeftWindowsKey",
                "KeyboardKeyRightWindowsKey",
                "KeyboardKeyApplications",
                "KeyboardKeyPower",
                "KeyboardKeySleep",
                "KeyboardKeyWake",
                "KeyboardKeyWebSearch",
                "KeyboardKeyWebFavorites",
                "KeyboardKeyWebRefresh",
                "KeyboardKeyWebStop",
                "KeyboardKeyWebForward",
                "KeyboardKeyWebBack",
                "KeyboardKeyMyComputer",
                "KeyboardKeyMail",
                "KeyboardKeyMediaSelect",
                "KeyboardKeyUnknown",
                "JoystickAxisX",
                "JoystickAxisY",
                "JoystickAxisZ",
                "JoystickRotationX",
                "JoystickRotationY",
                "JoystickRotationZ",
                "JoystickSliders0",
                "JoystickSliders1",
                "JoystickPointOfViewControllers0",
                "JoystickPointOfViewControllers1",
                "JoystickPointOfViewControllers2",
                "JoystickPointOfViewControllers3",
                "JoystickVelocityX",
                "JoystickVelocityY",
                "JoystickVelocityZ",
                "JoystickAngularVelocityX",
                "JoystickAngularVelocityY",
                "JoystickAngularVelocityZ",
                "JoystickVelocitySliders0",
                "JoystickVelocitySliders1",
                "JoystickAccelerationX",
                "JoystickAccelerationY",
                "JoystickAccelerationZ",
                "JoystickAngularAccelerationX",
                "JoystickAngularAccelerationY",
                "JoystickAngularAccelerationZ",
                "JoystickAccelerationSliders0",
                "JoystickAccelerationSliders1",
                "JoystickForceX",
                "JoystickForceY",
                "JoystickForceZ",
                "JoystickTorqueX",
                "JoystickTorqueY",
                "JoystickTorqueZ",
                "JoystickForceSliders0",
                "JoystickForceSliders1",
                "JoystickButtons0",
                "JoystickButtons1",
                "JoystickButtons2",
                "JoystickButtons3",
                "JoystickButtons4",
                "JoystickButtons5",
                "JoystickButtons6",
                "JoystickButtons7",
                "JoystickButtons8",
                "JoystickButtons9",
                "JoystickButtons10",
                "JoystickButtons11",
                "JoystickButtons12",
                "JoystickButtons13",
                "JoystickButtons14",
                "JoystickButtons15",
                "JoystickButtons16",
                "JoystickButtons17",
                "JoystickButtons18",
                "JoystickButtons19",
                "JoystickButtons20",
                "JoystickButtons21",
                "JoystickButtons22",
                "JoystickButtons23",
                "JoystickButtons24",
                "JoystickButtons25",
                "JoystickButtons26",
                "JoystickButtons27",
                "JoystickButtons28",
                "JoystickButtons29",
                "JoystickButtons30",
                "JoystickButtons31",
                "JoystickButtons32",
                "JoystickButtons33",
                "JoystickButtons34",
                "JoystickButtons35",
                "JoystickButtons36",
                "JoystickButtons37",
                "JoystickButtons38",
                "JoystickButtons39",
                "JoystickButtons40",
                "JoystickButtons41",
                "JoystickButtons42",
                "JoystickButtons43",
                "JoystickButtons44",
                "JoystickButtons45",
                "JoystickButtons46",
                "JoystickButtons47",
                "JoystickButtons48",
                "JoystickButtons49",
                "JoystickButtons50",
                "JoystickButtons51",
                "JoystickButtons52",
                "JoystickButtons53",
                "JoystickButtons54",
                "JoystickButtons55",
                "JoystickButtons56",
                "JoystickButtons57",
                "JoystickButtons58",
                "JoystickButtons59",
                "JoystickButtons60",
                "JoystickButtons61",
                "JoystickButtons62",
                "JoystickButtons63",
                "JoystickButtons64",
                "JoystickButtons65",
                "JoystickButtons66",
                "JoystickButtons67",
                "JoystickButtons68",
                "JoystickButtons69",
                "JoystickButtons70",
                "JoystickButtons71",
                "JoystickButtons72",
                "JoystickButtons73",
                "JoystickButtons74",
                "JoystickButtons75",
                "JoystickButtons76",
                "JoystickButtons77",
                "JoystickButtons78",
                "JoystickButtons79",
                "JoystickButtons80",
                "JoystickButtons81",
                "JoystickButtons82",
                "JoystickButtons83",
                "JoystickButtons84",
                "JoystickButtons85",
                "JoystickButtons86",
                "JoystickButtons87",
                "JoystickButtons88",
                "JoystickButtons89",
                "JoystickButtons90",
                "JoystickButtons91",
                "JoystickButtons92",
                "JoystickButtons93",
                "JoystickButtons94",
                "JoystickButtons95",
                "JoystickButtons96",
                "JoystickButtons97",
                "JoystickButtons98",
                "JoystickButtons99",
                "JoystickButtons100",
                "JoystickButtons101",
                "JoystickButtons102",
                "JoystickButtons103",
                "JoystickButtons104",
                "JoystickButtons105",
                "JoystickButtons106",
                "JoystickButtons107",
                "JoystickButtons108",
                "JoystickButtons109",
                "JoystickButtons110",
                "JoystickButtons111",
                "JoystickButtons112",
                "JoystickButtons113",
                "JoystickButtons114",
                "JoystickButtons115",
                "JoystickButtons116",
                "JoystickButtons117",
                "JoystickButtons118",
                "JoystickButtons119",
                "JoystickButtons120",
                "JoystickButtons121",
                "JoystickButtons122",
                "JoystickButtons123",
                "JoystickButtons124",
                "JoystickButtons125",
                "JoystickButtons126",
                "JoystickButtons127",
                "PS5ControllerLeftStickX",
                "PS5ControllerLeftStickY",
                "PS5ControllerRightStickX",
                "PS5ControllerRightStickY",
                "PS5ControllerLeftTriggerPosition",
                "PS5ControllerRightTriggerPosition",
                "PS5ControllerTouchX",
                "PS5ControllerTouchY",
                "PS5ControllerTouchOn",
                "PS5ControllerGyroX",
                "PS5ControllerGyroY",
                "PS5ControllerAccelX",
                "PS5ControllerAccelY",
                "PS5ControllerButtonCrossPressed",
                "PS5ControllerButtonCirclePressed",
                "PS5ControllerButtonSquarePressed",
                "PS5ControllerButtonTrianglePressed",
                "PS5ControllerButtonDPadUpPressed",
                "PS5ControllerButtonDPadRightPressed",
                "PS5ControllerButtonDPadDownPressed",
                "PS5ControllerButtonDPadLeftPressed",
                "PS5ControllerButtonL1Pressed",
                "PS5ControllerButtonR1Pressed",
                "PS5ControllerButtonL2Pressed",
                "PS5ControllerButtonR2Pressed",
                "PS5ControllerButtonL3Pressed",
                "PS5ControllerButtonR3Pressed",
                "PS5ControllerButtonCreatePressed",
                "PS5ControllerButtonMenuPressed",
                "PS5ControllerButtonLogoPressed",
                "PS5ControllerButtonTouchpadPressed",
                "PS5ControllerButtonFnLPressed",
                "PS5ControllerButtonFnRPressed",
                "PS5ControllerButtonBLPPressed",
                "PS5ControllerButtonBRPPressed",
                "PS5ControllerButtonMicPressed",
                "PS4ControllerLeftStickX",
                "PS4ControllerLeftStickY",
                "PS4ControllerRightStickX",
                "PS4ControllerRightStickY",
                "PS4ControllerLeftTriggerPosition",
                "PS4ControllerRightTriggerPosition",
                "PS4ControllerTouchX",
                "PS4ControllerTouchY",
                "PS4ControllerTouchOn",
                "PS4ControllerGyroX",
                "PS4ControllerGyroY",
                "PS4ControllerAccelX",
                "PS4ControllerAccelY",
                "PS4ControllerButtonCrossPressed",
                "PS4ControllerButtonCirclePressed",
                "PS4ControllerButtonSquarePressed",
                "PS4ControllerButtonTrianglePressed",
                "PS4ControllerButtonDPadUpPressed",
                "PS4ControllerButtonDPadRightPressed",
                "PS4ControllerButtonDPadDownPressed",
                "PS4ControllerButtonDPadLeftPressed",
                "PS4ControllerButtonL1Pressed",
                "PS4ControllerButtonR1Pressed",
                "PS4ControllerButtonL2Pressed",
                "PS4ControllerButtonR2Pressed",
                "PS4ControllerButtonL3Pressed",
                "PS4ControllerButtonR3Pressed",
                "PS4ControllerButtonCreatePressed",
                "PS4ControllerButtonMenuPressed",
                "PS4ControllerButtonLogoPressed",
                "PS4ControllerButtonTouchpadPressed",
                "PS4ControllerButtonMicPressed",
                "scalingfactorx",
                "scalingfactory",
                "WiimoteGuitarStatePlus",
                "WiimoteGuitarStateMinus",
                "WiimoteGuitarStateStrumDown",
                "WiimoteGuitarStateStrumUp",
                "WiimoteGuitarStateFretBlue",
                "WiimoteGuitarStateFretGreen",
                "WiimoteGuitarStateFretOrange",
                "WiimoteGuitarStateFretRed",
                "WiimoteGuitarStateFretYellow",
                "WiimoteGuitarStateRawJoystickX",
                "WiimoteGuitarStateRawJoystickY",
                "WiimoteGuitarStateRawWhammyBar",
                "WiimoteClassicControllerStateA",
                "WiimoteClassicControllerStateB",
                "WiimoteClassicControllerStateDown",
                "WiimoteClassicControllerStateHome",
                "WiimoteClassicControllerStateLeft",
                "WiimoteClassicControllerStateMinus",
                "WiimoteClassicControllerStatePlus",
                "WiimoteClassicControllerStateRight",
                "WiimoteClassicControllerStateTriggerL",
                "WiimoteClassicControllerStateTriggerR",
                "WiimoteClassicControllerStateUp",
                "WiimoteClassicControllerStateX",
                "WiimoteClassicControllerStateY",
                "WiimoteClassicControllerStateZL",
                "WiimoteClassicControllerStateZR",
                "WiimoteClassicControllerStateRawJoystickLeftX",
                "WiimoteClassicControllerStateRawJoystickLeftY",
                "WiimoteClassicControllerStateRawJoystickRightX",
                "WiimoteClassicControllerStateRawJoystickRightY",
                "WiimoteClassicControllerStateRawTriggerL",
                "WiimoteClassicControllerStateRawTriggerR",
                "WiimoteBalanceBoardStateCenterOfGravityX",
                "WiimoteBalanceBoardStateCenterOfGravityY",
                "WiimoteBalanceBoardStateSensorValuesKgBottomLeft",
                "WiimoteBalanceBoardStateSensorValuesKgBottomRight",
                "WiimoteBalanceBoardStateSensorValuesKgTopLeft",
                "WiimoteBalanceBoardStateSensorValuesKgTopRight",
                "WiimoteBalanceBoardStateWeightKg",
                "WiimoteDrumsStateBlue",
                "WiimoteDrumsStateBlueVelocity",
                "WiimoteDrumsStateGreen",
                "WiimoteDrumsStateGreenVelocity",
                "WiimoteDrumsStateOrange",
                "WiimoteDrumsStateOrangeVelocity",
                "WiimoteDrumsStateRed",
                "WiimoteDrumsStateRedVelocity",
                "WiimoteDrumsStateYellow",
                "WiimoteDrumsStateYellowVelocity",
                "WiimoteDrumsStateMinus",
                "WiimoteDrumsStatePlus",
                "WiimoteDrumsStatePedal",
                "WiimoteDrumsStatePedalVelocity",
                "WiimoteDrumsStateRawJoystickX",
                "WiimoteDrumsStateRawJoystickY",
                "WiimoteMotionPlusStateRawX",
                "WiimoteMotionPlusStateRawY",
                "WiimoteMotionPlusStateRawZ",
                "WiimoteMotionPlusStatePitchFast",
                "WiimoteMotionPlusStateRollFast",
                "WiimoteMotionPlusStateYawFast",
                "WiimoteTaikoDrumStateInnerLeft",
                "WiimoteTaikoDrumStateOuterLeft",
                "WiimoteTaikoDrumStateInnerRight",
                "WiimoteTaikoDrumStateOuterRight",
                "timeelapsed",
                "rawdataavailable",
                "RawData"
                };
            string[] snippets = { "if(^)\n{\n}", "if(^)\n{\n}\nelse\n{\n}", "for(^;;)\n{\n}", "while(^)\n{\n}", "do${\n^}while();", "switch(^)\n{\n\tcase : break;\n}" };
            string[] declarationSnippets = {
               "public class ^\n{\n}", "private class ^\n{\n}", "internal class ^\n{\n}",
               "public struct ^\n{\n}", "private struct ^\n{\n}", "internal struct ^\n{\n}",
               "public void ^()\n{\n}", "private void ^()\n{\n}", "internal void ^()\n{\n}", "protected void ^()\n{\n}",
               "public ^{ get; set; }", "private ^{ get; set; }", "internal ^{ get; set; }", "protected ^{ get; set; }",
               "public void ^()\n{\n}", "private void ^()\n{\n}", "public string ^()\n{\n}", "private string ^()\n{\n}",
               "public bool ^()\n{\n}", "private bool ^()\n{\n}", "public int ^()\n{\n}", "private int ^()\n{\n}",
               "public double ^()\n{\n}", "private double ^()\n{\n}", "public float ^()\n{\n}", "private float ^()\n{\n}"
               };
            Array.Sort(methods);
            Array.Sort(keywords);
            var items = new List<AutocompleteItem>();
            foreach (var item in snippets)
                items.Add(new SnippetAutocompleteItem(item) { ImageIndex = 1 });
            foreach (var item in declarationSnippets)
                items.Add(new DeclarationSnippet(item) { ImageIndex = 0 });
            foreach (var item in methods)
                items.Add(new MethodAutocompleteItem(item) { ImageIndex = 2 });
            foreach (var item in keywords)
                items.Add(new AutocompleteItem(item));
            items.Add(new InsertSpaceSnippet());
            items.Add(new InsertSpaceSnippet(@"^(\w+)([=<>!:]+)(\w+)$"));
            items.Add(new InsertEnterSnippet());
            this.autocompleteMenu1.SetAutocompleteItems(items);
        }
        private void fastColoredTextBox1_TextChanged(object sender, FastColoredTextBoxNS.TextChangedEventArgs e)
        {
            if (fastColoredTextBoxSaved != fastColoredTextBox1.Text)
                justSaved = false;
            ChangeScriptColors(sender);
        }
        private void helpToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string message = "• Input Devices : Wiimote(s) and Nunchuck(s), Joycon(s) left, Joycon(s) right, Switch Pro Controller(s), Joycon Charging Grip(s), DirectInput Controller(s), Keyboard(s), Mouse(s), Dualsense(s), Dualshock(s)4, Xbox Controller(s).\n\r\n\r• Output Devices : Xbox Controller(s), Keyboard and Mouse, Interception(s) (Int), Dualshock4 Controller(s), VJoy Controller(s).\n\r\n\r• Pairing Devices : Wiimote(s) or Joycon(s) left or Joycon(s) right need to be set in pairing mode after starting the run process, Switch Pro Controller(s) or Joycon Charging Grip(s) or DirectInput Controller(s) or Dualsense(s) or Dualshock(s)4 or Xbox Controller(s) or Keyboard(s) or Mouse(s) need to be USB wired.\n\r\n\r• Shortcuts : Page Down is used to remove a foreground window title, Page Up is used to add a foreground window title, when the option is enable.";
            string caption = "Help";
            MessageBox.Show(message, caption, MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string message = "• Author: Michaël André Franiatte.\n\r\n\r• Contact: michael.franiatte@gmail.com.\n\r\n\r• Publisher: https://github.com/michaelandrefraniatte.\n\r\n\r• Copyrights: All rights reserved, no permissions granted.\n\r\n\r• License: Not open source, not free of charge to use.";
            string caption = "About";
            MessageBox.Show(message, caption, MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        private void cutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            fastColoredTextBox1.Cut();
        }
        private void copyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            fastColoredTextBox1.Copy();
        }
        private void pasteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            fastColoredTextBox1.Paste();
        }
        private void undoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            fastColoredTextBox1.Undo();
        }
        private void redoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            fastColoredTextBox1.Redo();
        }
        private void fastColoredTextBox1_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                fastColoredTextBox1.ContextMenu = contextMenu;
            }
        }
        private void changeCursor(object sender, EventArgs e)
        {
            this.Cursor = Cursors.Hand;
        }
        private void cutAction(object sender, EventArgs e)
        {
            fastColoredTextBox1.Cut();
        }
        private void copyAction(object sender, EventArgs e)
        {
            if (fastColoredTextBox1.SelectedText != "")
                Clipboard.SetText(fastColoredTextBox1.SelectedText);
        }
        private void pasteAction(object sender, EventArgs e)
        {
            if (Clipboard.ContainsText())
            {
                fastColoredTextBox1.SelectedText = Clipboard.GetText(TextDataFormat.Text).ToString();
            }
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            TimeBeginPeriod(1);
            NtSetTimerResolution(1, true, ref CurrentResolution);
            menuItem = new MenuItem("Cut");
            contextMenu.MenuItems.Add(menuItem);
            menuItem.Select += new EventHandler(changeCursor);
            menuItem.Click += new EventHandler(cutAction);
            menuItem = new MenuItem("Copy");
            contextMenu.MenuItems.Add(menuItem);
            menuItem.Select += new EventHandler(changeCursor);
            menuItem.Click += new EventHandler(copyAction);
            menuItem = new MenuItem("Paste");
            contextMenu.MenuItems.Add(menuItem);
            menuItem.Select += new EventHandler(changeCursor);
            menuItem.Click += new EventHandler(pasteAction);
            fastColoredTextBox1.ContextMenu = contextMenu;
            TrayMenuContext();
            replaceform = new ReplaceForm(this.fastColoredTextBox1);
        }
        private void TrayMenuContext()
        {
            this.notifyIcon1.ContextMenuStrip = new System.Windows.Forms.ContextMenuStrip();
            this.notifyIcon1.ContextMenuStrip.Items.Add("Quit", null, this.MenuTest1_Click);
        }
        private void MenuTest1_Click(object sender, EventArgs e)
        {
            closeonicon = true;
            this.Close();
        }
        private void MinimzedTray()
        {
            ShowWindow(Process.GetCurrentProcess().MainWindowHandle, 0);
        }
        private void MaxmizedFromTray()
        {
            if (File.Exists(Application.StartupPath + @"\temphandle"))
            {
                using (System.IO.StreamReader file = new System.IO.StreamReader(Application.StartupPath + @"\temphandle"))
                {
                    IntPtr handle = new IntPtr(int.Parse(file.ReadLine()));
                    ShowWindow(handle, 9);
                    SetForegroundWindow(handle);
                    Microsoft.VisualBasic.Interaction.AppActivate(file.ReadLine());
                }
            }
        }
        private void notifyIcon1_DoubleClick(object sender, EventArgs e)
        {
            Task.Run(() => MaxmizedFromTray());
        }
        private void Form1_Shown(object sender, EventArgs e)
        {
            if (!onopenwith)
            {
                if (File.Exists(Application.StartupPath + @"\tempsave"))
                {
                    using (StreamReader file = new StreamReader(Application.StartupPath + @"\tempsave"))
                    {
                        filename = file.ReadLine();
                        runProcessAtLaunchToolStripMenuItem.Checked = bool.Parse(file.ReadLine());
                        startProgramAtBootToolStripMenuItem.Checked = bool.Parse(file.ReadLine());
                        minimizeToSystrayAtCloseToolStripMenuItem.Checked = bool.Parse(file.ReadLine());
                        minimizeToSystrayAtBootToolStripMenuItem.Checked = bool.Parse(file.ReadLine());
                        associateFileExtensionToolStripMenuItem.Checked = bool.Parse(file.ReadLine());
                        showATransparentClickableOverlayToolStripMenuItem.Checked = bool.Parse(file.ReadLine());
                        optimizeByStopingProcessAndServiceToolStripMenuItem.Checked = bool.Parse(file.ReadLine());
                        removeWindowTitleToolStripMenuItem.Checked = bool.Parse(file.ReadLine());
                    }
                    if (filename != "" & File.Exists(filename))
                    {
                        string readText = File.ReadAllText(filename);
                        fastColoredTextBox1.Text = readText;
                        this.Text = "SIGIL: " + Path.GetFileName(filename);
                        fastColoredTextBoxSaved = fastColoredTextBox1.Text;
                        justSaved = true;
                        if (runProcessAtLaunchToolStripMenuItem.Checked)
                            StartProcess();
                    }
                }
            }
            using (System.IO.StreamWriter createdfile = new System.IO.StreamWriter(Application.StartupPath + @"\temphandle"))
            {
                createdfile.WriteLine(Process.GetCurrentProcess().MainWindowHandle);
                createdfile.WriteLine(this.Text);
            }
            if (minimizeToSystrayAtBootToolStripMenuItem.Checked)
            {
                MinimzedTray();
            }
            FillAutocompletion();
        }
        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (!closeonicon & minimizeToSystrayAtCloseToolStripMenuItem.Checked)
            {
                e.Cancel = true;
                MinimzedTray();
                return;
            }
            if (runstopbool)
                StopProcess();
            Thread.Sleep(100);
            DisconnectControllers();
            if (!justSaved)
            {
                result = MessageBox.Show("Content will be lost! Are you sure?", "Exit", MessageBoxButtons.OKCancel);
                if (result == DialogResult.Cancel)
                {
                    e.Cancel = true;
                    return;
                }
            }
            using (StreamWriter createdfile = new StreamWriter(Application.StartupPath + @"\tempsave"))
            {
                createdfile.WriteLine(filename);
                createdfile.WriteLine(runProcessAtLaunchToolStripMenuItem.Checked);
                createdfile.WriteLine(startProgramAtBootToolStripMenuItem.Checked);
                createdfile.WriteLine(minimizeToSystrayAtCloseToolStripMenuItem.Checked);
                createdfile.WriteLine(minimizeToSystrayAtBootToolStripMenuItem.Checked);
                createdfile.WriteLine(associateFileExtensionToolStripMenuItem.Checked);
                createdfile.WriteLine(showATransparentClickableOverlayToolStripMenuItem.Checked);
                createdfile.WriteLine(optimizeByStopingProcessAndServiceToolStripMenuItem.Checked);
                createdfile.WriteLine(removeWindowTitleToolStripMenuItem.Checked);
            }
            removewindowtitle = false;
            optimizewindows = false;
        }
        private void newToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (runstopbool)
                StopProcess();
            if (!justSaved)
            {
                result = MessageBox.Show("Content will be lost! Are you sure?", "New", MessageBoxButtons.OKCancel);
                if (result == DialogResult.OK)
                {
                    fastColoredTextBox1.Clear();
                    this.Text = "SIGIL";
                    filename = "";
                    fastColoredTextBoxSaved = fastColoredTextBox1.Text;
                    justSaved = true;
                }
            }
            else
            {
                fastColoredTextBox1.Clear();
                this.Text = "SIGIL";
                filename = "";
                fastColoredTextBoxSaved = fastColoredTextBox1.Text;
                justSaved = true;
            }
        }
        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (runstopbool)
                StopProcess();
            if (!justSaved)
            {
                result = MessageBox.Show("Content will be lost! Are you sure?", "Open", MessageBoxButtons.OKCancel);
                if (result == DialogResult.OK)
                {
                    OpenFileDialog op = new OpenFileDialog();
                    op.Filter = "Script SIGIL File(*.sig)|*.sig";
                    if (op.ShowDialog() == DialogResult.OK)
                    {
                        fastColoredTextBox1.Clear();
                        string readText = File.ReadAllText(op.FileName);
                        fastColoredTextBox1.Text = readText;
                        filename = op.FileName;
                        this.Text = "SIGIL: " + Path.GetFileName(filename);
                        fastColoredTextBoxSaved = fastColoredTextBox1.Text;
                        justSaved = true;
                    }
                }
            }
            else
            {
                OpenFileDialog op = new OpenFileDialog();
                op.Filter = "Script SIGIL File(*.sig)|*.sig";
                if (op.ShowDialog() == DialogResult.OK)
                {
                    fastColoredTextBox1.Clear();
                    string readText = File.ReadAllText(op.FileName);
                    fastColoredTextBox1.Text = readText;
                    filename = op.FileName;
                    this.Text = "SIGIL: " + Path.GetFileName(filename);
                    fastColoredTextBoxSaved = fastColoredTextBox1.Text;
                    justSaved = true;
                }
            }
        }
        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (filename == "")
            {
                saveAsToolStripMenuItem_Click(sender, e);
            }
            else
            {
                if (runstopbool)
                    StopProcess();
                code = fastColoredTextBox1.Text;
                File.WriteAllText(filename, code);
                fastColoredTextBoxSaved = fastColoredTextBox1.Text;
                justSaved = true;
            }
        }
        private void saveAsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (runstopbool)
                StopProcess();
            SaveFileDialog sf = new SaveFileDialog();
            sf.Filter = "Script SIGIL File(*.sig)|*.sig";
            if (filename != "")
                sf.FileName = Path.GetFileName(filename);
            if (sf.ShowDialog() == DialogResult.OK)
            {
                code = fastColoredTextBox1.Text;
                File.WriteAllText(sf.FileName, code);
                this.Text = "SIGIL: " + Path.GetFileName(sf.FileName);
                filename = sf.FileName;
                fastColoredTextBoxSaved = fastColoredTextBox1.Text;
                justSaved = true;
            }
        }
        private void replaceToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!replaceformvisible)
            {
                replaceformvisible = true;
                try
                {
                    replaceform.Visible = true;
                }
                catch { }
            }
            else
            {
                if (replaceformvisible)
                {
                    replaceformvisible = false;
                    try
                    {
                        replaceform.Hide();
                    }
                    catch { }
                }
            }
        }
        private void FillCode()
        {
            code = fastColoredTextBox1.Text;
            parameters = new System.CodeDom.Compiler.CompilerParameters();
            parameters.GenerateExecutable = true;
            parameters.GenerateInMemory = false;
            parameters.IncludeDebugInformation = true;
            parameters.TreatWarningsAsErrors = false;
            parameters.WarningLevel = 0;
            parameters.CompilerOptions = "/optimize+ /platform:x86 /target:exe /unsafe";
            if (code.Contains("using System;"))
                parameters.ReferencedAssemblies.Add(Application.StartupPath + @"\System.dll");
            if (code.Contains("using System.Windows.Forms;"))
                parameters.ReferencedAssemblies.Add(Application.StartupPath + @"\System.Windows.Forms.dll");
            if (code.Contains("using System.Drawing;"))
                parameters.ReferencedAssemblies.Add(Application.StartupPath + @"\System.Drawing.dll");
            if (code.Contains("using System.Runtime;"))
                parameters.ReferencedAssemblies.Add(Application.StartupPath + @"\System.Runtime.dll");
            if (code.Contains("using System.Collections;"))
                parameters.ReferencedAssemblies.Add(Application.StartupPath + @"\System.Collections.dll");
            if (code.Contains("using System.Linq;"))
                parameters.ReferencedAssemblies.Add(Application.StartupPath + @"\System.Linq.dll");
            if (code.Contains("using System.Numerics.Vectors;"))
                parameters.ReferencedAssemblies.Add(Application.StartupPath + @"\System.Numerics.Vectors.dll");
            if (code.Contains("using System.Numerics;"))
                parameters.ReferencedAssemblies.Add(Application.StartupPath + @"\System.Numerics.dll");
            if (code.Contains("using System.Core;"))
                parameters.ReferencedAssemblies.Add(Application.StartupPath + @"\System.Core.dll");
            if (code.Contains("using netstandard;"))
                parameters.ReferencedAssemblies.Add(Application.StartupPath + @"\netstandard.dll");
            if (code.Contains("using controllers;"))
                AddAssembly("controllers");
            if (code.Contains("using controllersds4;"))
                AddAssembly("controllersds4");
            if (code.Contains("using controllersvjoy;"))
                AddAssembly("controllersvjoy");
            if (code.Contains("using keyboardsmouses;"))
                AddAssembly("keyboardsmouses");
            if (code.Contains("using Interceptions;"))
                AddAssembly("Interceptions");
            if (code.Contains("using Valuechanges;"))
                AddAssembly("Valuechanges");
            if (code.Contains("using KeyboardInputsAPI;"))
                AddAssembly("Keyboardinputs");
            if (code.Contains("using MouseInputsAPI;"))
                AddAssembly("Mouseinputs");
            if (code.Contains("using DualSensesAPI;"))
                AddAssembly("Dualsenses");
            if (code.Contains("using DualShocks4API;"))
                AddAssembly("Dualshocks4");
            if (code.Contains("using DirectInputsAPI;"))
                AddAssembly("Directinputs");
            if (code.Contains("using JoyconChargingGripsAPI;"))
                AddAssembly("Joyconcharginggrips");
            if (code.Contains("using JoyconsLeftAPI;"))
                AddAssembly("Joyconsleft");
            if (code.Contains("using JoyconsRightAPI;"))
                AddAssembly("Joyconsright");
            if (code.Contains("using SwitchProControllersAPI;"))
                AddAssembly("Switchprocontrollers");
            if (code.Contains("using WiiMotesAPI;"))
                AddAssembly("Wiimotes");
            if (code.Contains("using XInputsAPI;"))
                AddAssembly("Xinputs");
            if (code.Contains("using CameraAPI;"))
                AddAssembly("Camera");
            if (code.Contains("using SpeechAPI;"))
                AddAssembly("Speech");
            if (code.Contains("using MouseHooksAPI;"))
                AddAssembly("Mousehook");
            if (code.Contains("using KeyboardHooksAPI;"))
                AddAssembly("Keyboardhook");
            if (code.Contains("using SendInputs;"))
                AddAssembly("sendinputs");
            if (code.Contains("using GamepadsHooksAPI;"))
                AddAssembly("Gamepadshook");
            if (code.Contains("using JoysticksHooksAPI;"))
                AddAssembly("Joystickshook");
            if (code.Contains("using Networkshost;"))
                AddAssembly("Networkshost");
            if (code.Contains("using Networks;"))
                AddAssembly("Networks");
            if (code.Contains("using WebSocketSharp;"))
                AddAssembly("websocket-sharp");
            if (code.Contains("using KeyboardRawInputsAPI;"))
                AddAssembly("Keyboardrawinputs");
            if (code.Contains("using MouseRawInputsAPI;"))
                AddAssembly("Mouserawinputs");
            if (code.Contains("using KeyboardXnaHookAPI;"))
                AddAssembly("Keyboardxnahook");
            if (code.Contains("using MouseXnaHookAPI;"))
                AddAssembly("Mousexnahook");
            if (code.Contains("using KeyboardRawHooksAPI;"))
                AddAssembly("Keyboardrawhooks");
            if (code.Contains("using MouseRawHooksAPI;"))
                AddAssembly("Mouserawhooks");
            if (code.Contains("using ScalingFactorAPI;"))
                AddAssembly("Scalingfactor");
            if (code.Contains("using WiiMotesLibAPI;"))
                AddAssembly("Wiimoteslib");
            if (code.Contains("using WiiMotesLibMPAPI;"))
                AddAssembly("Wiimoteslibmp");
            if (code.Contains("using Sound;"))
                AddAssembly("Sound");
            if (code.Contains("using TimersAPI;"))
                AddAssembly("Timers");
        }
        private void AddAssembly(string dllName)
        {
             parameters.ReferencedAssemblies.Add(Application.StartupPath + @"\" + dllName + ".dll");
        }
        private void testToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FillCode();
            provider = new Microsoft.CSharp.CSharpCodeProvider();
            results = provider.CompileAssemblyFromSource(parameters, code);
            if (results.Errors.HasErrors)
            {
                StringBuilder sb = new StringBuilder();
                foreach (System.CodeDom.Compiler.CompilerError error in results.Errors)
                {
                    sb.AppendLine(String.Format("Error ({0}) : {1}", error.ErrorNumber, error.ErrorText));
                }
                MessageBox.Show("Script Error :\n\r" + sb.ToString());
            }
            else
            {
                MessageBox.Show("Script Ok.");
            }
        }
        private void StartProcess()
        {
            FillCode();
            provider = new Microsoft.CSharp.CSharpCodeProvider();
            results = provider.CompileAssemblyFromSource(parameters, code);
            if (results.Errors.HasErrors)
            {
                StringBuilder sb = new StringBuilder();
                foreach (System.CodeDom.Compiler.CompilerError error in results.Errors)
                {
                    sb.AppendLine(String.Format("Error ({0}) : {1}", error.ErrorNumber, error.ErrorText));
                }
                MessageBox.Show("Script Error :\n\r" + sb.ToString());
                return;
            }
            assembly = results.CompiledAssembly;
            program = assembly.GetType("StringToCode.FooClass");
            obj = Activator.CreateInstance(program);
            program.InvokeMember("Load", BindingFlags.Default | BindingFlags.InvokeMethod, null, obj, new object[] { });
            runToolStripMenuItem.Text = "Stop";
            fastColoredTextBox1.ReadOnly = true;
            fastColoredTextBox1.Enabled = false;
            runstopbool = true;
        }
        private void runToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!runstopbool)
            {
                StartProcess();
            }
            else
            {
                StopProcess();
            }
        }
        private void StopProcess()
        {
            runToolStripMenuItem.Text = "Run";
            fastColoredTextBox1.ReadOnly = false;
            fastColoredTextBox1.Enabled = true;
            runstopbool = false;
            try
            {
                program.InvokeMember("Close", BindingFlags.IgnoreReturn | BindingFlags.InvokeMethod, null, obj, new object[] { });
            }
            catch { }
        }
        [STAThread]
        private void DisconnectControllers()
        {
            Thread newThread = new Thread(new ThreadStart(disconnectControllers));
            newThread.SetApartmentState(ApartmentState.STA);
            newThread.Start();
        }
        private void disconnectControllers()
        {
            string finalcode = @"
                using System;
                using System.Globalization;
                using System.IO;
                using System.Runtime.InteropServices;
                using System.Threading;
                using System.Threading.Tasks;
                using System.Reflection;
                namespace StringToCode
                {
                    public class FooClass 
                    { 
                        [DllImport(""MotionInputPairing.dll"", EntryPoint = ""joyconrightconnect"")]
                        public static extern bool joyconrightconnect();
                        [DllImport(""MotionInputPairing.dll"", EntryPoint = ""joyconleftconnect"")]
                        public static extern bool joyconleftconnect();
                        [DllImport(""MotionInputPairing.dll"", EntryPoint = ""wiimoteconnect"")]
                        public static extern bool wiimoteconnect();
                        [DllImport(""MotionInputPairing.dll"", EntryPoint = ""joyconrightdisconnect"")]
                        public static extern bool joyconrightdisconnect();
                        [DllImport(""MotionInputPairing.dll"", EntryPoint = ""joyconleftdisconnect"")]
                        public static extern bool joyconleftdisconnect();
                        [DllImport(""MotionInputPairing.dll"", EntryPoint = ""wiimotedisconnect"")]
                        public static extern bool wiimotedisconnect();
                        public void Disconnect()
                        {
                            try
                            {
                                wiimoteconnect();
                                wiimotedisconnect();
                            }
                            catch { }
                            try
                            {
                                joyconrightconnect();
                                joyconrightdisconnect();
                            }
                            catch { }
                            try
                            {
                                joyconleftconnect();
                                joyconleftdisconnect();
                            }
                            catch { }
                            try
                            {
                                wiimoteconnect();
                                wiimotedisconnect();
                            }
                            catch { }
                            try
                            {
                                joyconrightconnect();
                                joyconrightdisconnect();
                            }
                            catch { }
                            try
                            {
                                joyconleftconnect();
                                joyconleftdisconnect();
                            }
                            catch { }
                        }
                    }
                }";
            parameters = new System.CodeDom.Compiler.CompilerParameters();
            parameters.GenerateExecutable = false;
            parameters.GenerateInMemory = true;
            provider = new Microsoft.CSharp.CSharpCodeProvider();
            results = provider.CompileAssemblyFromSource(parameters, finalcode);
            assembly = results.CompiledAssembly;
            program = assembly.GetType("StringToCode.FooClass");
            obj = Activator.CreateInstance(program);
            program.InvokeMember("Disconnect", BindingFlags.Default | BindingFlags.InvokeMethod, null, obj, new object[] { });
        }
        private void startProgramAtBootToolStripMenuItem_CheckStateChanged(object sender, EventArgs e)
        {
            if (startProgramAtBootToolStripMenuItem.Checked & optimizeByStopingProcessAndServiceToolStripMenuItem.Checked)
                optimizeByStopingProcessAndServiceToolStripMenuItem.Checked = false;
            RegistryKey rk;
            rk = Registry.CurrentUser.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", true);
            if (startProgramAtBootToolStripMenuItem.Checked)
                rk.SetValue("SIGIL", Application.ExecutablePath);
            else
                rk.DeleteValue("SIGIL", false);
            try
            {
                rk = Registry.CurrentUser.CreateSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Explorer\\Serialize", true);
                if (startProgramAtBootToolStripMenuItem.Checked)
                    rk.SetValue("Startupdelayinmsec", "5000", RegistryValueKind.DWord);
                else
                    rk.DeleteValue("Startupdelayinmsec", false);
            }
            catch
            {
                rk = Registry.CurrentUser.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Explorer\\Serialize", true);
                if (startProgramAtBootToolStripMenuItem.Checked)
                    rk.SetValue("Startupdelayinmsec", "5000", RegistryValueKind.DWord);
                else
                    rk.DeleteValue("Startupdelayinmsec", false);
            }
        }
        private void associateFileExtensionToolStripMenuItem_CheckStateChanged(object sender, EventArgs e)
        {
            if (associateFileExtensionToolStripMenuItem.Checked)
                FileAssociationHelper.AssociateFileExtension(".sig", "ScriptSIGILFile", "Script SIGIL File", Application.ExecutablePath);
            else
                FileAssociationHelper.RemoveFileAssociation(".sig", "ScriptSIGILFile");
        }
        private void showATransparentClickableOverlayToolStripMenuItem_CheckStateChanged(object sender, EventArgs e)
        {
            if (showATransparentClickableOverlayToolStripMenuItem.Checked)
            {
                try
                {
                    form2.Visible = true;
                }
                catch { }
            }
            else
            {
                try
                {
                    form2.Hide();
                }
                catch { }
            }
        }
        private void enumerateDevicesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string message = EnumerateDevices.GetDevices();
            string caption = "Device paths";
            MessageBox.Show(message, caption, MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        private void interceptionTestToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                form3 = new Form3();
                form3.Show();
            }
            catch { }
        }
        private void removeWindowTitleToolStripMenuItem_CheckStateChanged(object sender, EventArgs e)
        {
            if (removeWindowTitleToolStripMenuItem.Checked)
            {
                removewindowtitle = true;
                Task.Run(() => RemoveWindowTitle());
            }
            else
                removewindowtitle = false;
        }
        private void RemoveWindowTitle()
        {
            while (removewindowtitle)
            {
                valchanged(0, GetAsyncKeyState(Keys.PageDown));
                if (wu[0])
                {
                    width = Screen.PrimaryScreen.Bounds.Width;
                    height = Screen.PrimaryScreen.Bounds.Height;
                    IntPtr window = GetForegroundWindow();
                    SetWindowLong(window, GWL_STYLE, WS_SYSMENU);
                    SetWindowPos(window, -2, 0, 0, width, height, 0x0040);
                    DrawMenuBar(window);
                }
                valchanged(1, GetAsyncKeyState(Keys.PageUp));
                if (wu[1])
                {
                    IntPtr window = GetForegroundWindow();
                    SetWindowLong(window, GWL_STYLE, WS_CAPTION | WS_POPUP | WS_BORDER | WS_SYSMENU | WS_TABSTOP | WS_VISIBLE | WS_OVERLAPPED | WS_MINIMIZEBOX | WS_MAXIMIZEBOX);
                    DrawMenuBar(window);
                }
                System.Threading.Thread.Sleep(100);
            }
        }
        private void optimizeByStopingProcessAndServiceToolStripMenuItem_CheckStateChanged(object sender, EventArgs e)
        {
            if (startProgramAtBootToolStripMenuItem.Checked & optimizeByStopingProcessAndServiceToolStripMenuItem.Checked)
                startProgramAtBootToolStripMenuItem.Checked = false;
            if (optimizeByStopingProcessAndServiceToolStripMenuItem.Checked)
            {
                optimizewindows = true;
                Task.Run(() => StartStopItBlockProc());
                Task.Run(() => StartStopItBlockServ());
            }
            else
                optimizewindows = false;
        }
        public void StartStopItBlockProc()
        {
            using (StreamReader file = new StreamReader("siprocblacklist.txt"))
            {
                while (optimizewindows)
                {
                    string procName = file.ReadLine();
                    if (procName == "")
                    {
                        file.Close();
                        break;
                    }
                    else
                    {
                        procnamesbl += procName + ".exe ";
                    }
                }
            }
            for (; ; )
            {
                if (!optimizewindows)
                    break;
                if (procnamesbl != "")
                    killProcessByNames(procnamesbl);
                Thread.Sleep(1000);
            }
        }
        public void StartStopItBlockServ()
        {
            using (StreamReader file = new StreamReader("siservblacklist.txt"))
            {
                while (optimizewindows)
                {
                    string servName = file.ReadLine();
                    if (servName == "")
                    {
                        file.Close();
                        break;
                    }
                    else
                    {
                        servBLs.Add(servName);
                    }
                }
            }
            for (; ; )
            {
                if (!optimizewindows)
                    break;
                services = ServiceController.GetServices();
                foreach (ServiceController service in services)
                {
                    try
                    {
                        if (service.Status == ServiceControllerStatus.Running)
                        {
                            servNames = service.ServiceName;
                            if (servNames.Length > 7)
                                servNames = servNames.Substring(0, 7);
                            if (servBLs.Any(n => n.StartsWith(servNames)))
                            {
                                service.Stop();
                                service.WaitForStatus(ServiceControllerStatus.Stopped, timeout);
                            }
                        }
                    }
                    catch { }
                    Thread.Sleep(1);
                }
                Thread.Sleep(1000);
            }
        }
    }
}