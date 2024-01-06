using System;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Reflection;
using FastColoredTextBoxNS;
using Range = FastColoredTextBoxNS.Range;
using System.Text.RegularExpressions;
using System.IO;
using System.Threading;
using Microsoft.Win32;
using System.Runtime.InteropServices;
using OpenWithSingleInstance;
using System.Diagnostics;
using System.Threading.Tasks;

namespace SIGIL
{
    public partial class Form1 : Form
    {
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
        public Form1(string filePath)
        {
            InitializeComponent();
            if (filePath != null)
            {
                onopenwith = true;
                OpenFileWith(filePath);
            }
        }
        private static bool closeonicon = false;
        private static DialogResult result;
        private static ContextMenu contextMenu = new ContextMenu();
        private static MenuItem menuItem;
        public static bool justSaved = true, onopenwith = false, replaceformvisible = false;
        private static string filename = "", fastColoredTextBoxSaved = "", code = "";
        public static ReplaceForm replaceform;
        public static bool runstopbool = false;
        private static Range range;
        private static Style StyleInput = new TextStyle(Brushes.Blue, null, System.Drawing.FontStyle.Regular), StyleOutput = new TextStyle(Brushes.Orange, null, System.Drawing.FontStyle.Regular), StyleLibrary = new TextStyle(Brushes.BlueViolet, null, System.Drawing.FontStyle.Regular), StyleClass = new TextStyle(Brushes.DodgerBlue, null, System.Drawing.FontStyle.Regular), StyleMethod = new TextStyle(Brushes.Magenta, null, System.Drawing.FontStyle.Regular), StyleObject = new TextStyle(Brushes.DarkOrange, null, System.Drawing.FontStyle.Regular), StyleExtra = new TextStyle(Brushes.Red, null, System.Drawing.FontStyle.Regular), StyleSpecial = new TextStyle(Brushes.DarkCyan, null, System.Drawing.FontStyle.Regular), StyleNone = new TextStyle(Brushes.Black, null, System.Drawing.FontStyle.Regular);
        private Type program;
        private object obj;
        private Assembly assembly;
        private System.CodeDom.Compiler.CompilerResults results;
        private Microsoft.CSharp.CSharpCodeProvider provider;
        private System.CodeDom.Compiler.CompilerParameters parameters;
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
        public void OpenFileWith(string filePath)
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
            using (System.IO.StreamWriter createdfile = new System.IO.StreamWriter(Application.StartupPath + @"\temphandle"))
            {
                createdfile.WriteLine(Process.GetCurrentProcess().MainWindowHandle);
                createdfile.WriteLine(this.Text);
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
                range.SetStyle(StyleLibrary, new Regex(@"\bvalListX\b"));
                range.SetStyle(StyleLibrary, new Regex(@"\bvalListY\b"));
                range.SetStyle(StyleLibrary, new Regex(@"\bCount\b"));
                range.SetStyle(StyleLibrary, new Regex(@"\bClear\b"));
                range.SetStyle(StyleLibrary, new Regex(@"\bRemoveAt\b"));
                range.SetStyle(StyleLibrary, new Regex(@"\bAdd\b"));
                range.SetStyle(StyleLibrary, new Regex(@"\bAverage\b"));
                range.SetStyle(StyleLibrary, new Regex(@"\bCount\b"));
                range.SetStyle(StyleLibrary, new Regex(@"\bClear\b"));
                range.SetStyle(StyleLibrary, new Regex(@"\bRemoveAt\b"));
                range.SetStyle(StyleLibrary, new Regex(@"\bAdd\b"));
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
                range.SetStyle(StyleLibrary, new Regex(@"\bInterceptions\b"));
                range.SetStyle(StyleClass, new Regex(@"\bSendInterception\b"));
                range.SetStyle(StyleClass, new Regex(@"\bInput\b"));
                range.SetStyle(StyleObject, new Regex(@"\bSI\b"));
                range.SetStyle(StyleMethod, new Regex(@"\bLoad\b"));
                range.SetStyle(StyleLibrary, new Regex(@"\bWiiMotesAPI\b"));
                range.SetStyle(StyleClass, new Regex(@"\bWiiMote\b"));
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
                range.SetStyle(StyleOutput, new Regex(@"\bcontroller1_send_xbox\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bcontroller1_send_back\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bcontroller1_send_start\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bcontroller1_send_A\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bcontroller1_send_B\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bcontroller1_send_X\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bcontroller1_send_Y\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bcontroller1_send_up\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bcontroller1_send_left\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bcontroller1_send_down\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bcontroller1_send_right\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bcontroller1_send_leftstick\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bcontroller1_send_rightstick\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bcontroller1_send_leftbumper\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bcontroller1_send_rightbumper\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bcontroller1_send_lefttriggerposition\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bcontroller1_send_righttriggerposition\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bcontroller1_send_leftstickx\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bcontroller1_send_leftsticky\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bcontroller1_send_rightstickx\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bcontroller1_send_rightsticky\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bController1DS4_Send_Options\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bController1DS4_Send_ThumbLeft\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bController1DS4_Send_ThumbRight\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bController1DS4_Send_ShoulderLeft\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bController1DS4_Send_ShoulderRight\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bController1DS4_Send_Cross\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bController1DS4_Send_Circle\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bController1DS4_Send_Square\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bController1DS4_Send_Triangle\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bController1DS4_Send_Ps\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bController1DS4_Send_Touchpad\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bController1DS4_Send_Share\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bController1DS4_Send_DPadUp\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bController1DS4_Send_DPadDown\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bController1DS4_Send_DPadLeft\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bController1DS4_Send_DPadRight\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bController1DS4_Send_LeftTrigger\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bController1DS4_Send_RightTrigger\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bController1DS4_Send_LeftTriggerPosition\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bController1DS4_Send_RightTriggerPosition\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bController1DS4_Send_LeftThumbX\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bController1DS4_Send_RightThumbX\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bController1DS4_Send_LeftThumbY\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bController1DS4_Send_RightThumbY\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bController1VJoy_Send_1\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bController1VJoy_Send_2\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bController1VJoy_Send_3\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bController1VJoy_Send_4\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bController1VJoy_Send_5\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bController1VJoy_Send_6\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bController1VJoy_Send_7\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bController1VJoy_Send_8\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bController1VJoy_Send_X\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bController1VJoy_Send_Y\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bController1VJoy_Send_Z\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bController1VJoy_Send_WHL\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bController1VJoy_Send_SL0\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bController1VJoy_Send_SL1\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bController1VJoy_Send_RX\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bController1VJoy_Send_RY\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bController1VJoy_Send_RZ\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bController1VJoy_Send_POV\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bController1VJoy_Send_Hat\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bController1VJoy_Send_HatExt1\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bController1VJoy_Send_HatExt2\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bController1VJoy_Send_HatExt3\b"));
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
                range.SetStyle(StyleOutput, new Regex(@"\bint_1_deltaX\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bint_1_deltaY\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bint_1_x\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bint_1_y\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bint_1_SendLeftClick\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bint_1_SendRightClick\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bint_1_SendMiddleClick\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bint_1_SendWheelUp\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bint_1_SendWheelDown\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bint_1_SendCANCEL\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bint_1_SendBACK\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bint_1_SendTAB\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bint_1_SendCLEAR\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bint_1_SendRETURN\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bint_1_SendSHIFT\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bint_1_SendCONTROL\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bint_1_SendMENU\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bint_1_SendCAPITAL\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bint_1_SendESCAPE\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bint_1_SendSPACE\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bint_1_SendPRIOR\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bint_1_SendNEXT\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bint_1_SendEND\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bint_1_SendHOME\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bint_1_SendLEFT\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bint_1_SendUP\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bint_1_SendRIGHT\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bint_1_SendDOWN\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bint_1_SendSNAPSHOT\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bint_1_SendINSERT\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bint_1_SendNUMPADDEL\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bint_1_SendNUMPADINSERT\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bint_1_SendHELP\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bint_1_SendAPOSTROPHE\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bint_1_SendBACKSPACE\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bint_1_SendPAGEDOWN\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bint_1_SendPAGEUP\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bint_1_SendFIN\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bint_1_SendMOUSE\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bint_1_SendA\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bint_1_SendB\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bint_1_SendC\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bint_1_SendD\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bint_1_SendE\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bint_1_SendF\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bint_1_SendG\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bint_1_SendH\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bint_1_SendI\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bint_1_SendJ\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bint_1_SendK\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bint_1_SendL\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bint_1_SendM\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bint_1_SendN\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bint_1_SendO\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bint_1_SendP\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bint_1_SendQ\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bint_1_SendR\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bint_1_SendS\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bint_1_SendT\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bint_1_SendU\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bint_1_SendV\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bint_1_SendW\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bint_1_SendX\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bint_1_SendY\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bint_1_SendZ\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bint_1_SendLWIN\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bint_1_SendRWIN\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bint_1_SendAPPS\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bint_1_SendDELETE\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bint_1_SendNUMPAD0\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bint_1_SendNUMPAD1\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bint_1_SendNUMPAD2\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bint_1_SendNUMPAD3\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bint_1_SendNUMPAD4\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bint_1_SendNUMPAD5\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bint_1_SendNUMPAD6\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bint_1_SendNUMPAD7\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bint_1_SendNUMPAD8\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bint_1_SendNUMPAD9\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bint_1_SendMULTIPLY\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bint_1_SendADD\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bint_1_SendSUBTRACT\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bint_1_SendDECIMAL\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bint_1_SendPRINTSCREEN\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bint_1_SendDIVIDE\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bint_1_SendF1\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bint_1_SendF2\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bint_1_SendF3\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bint_1_SendF4\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bint_1_SendF5\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bint_1_SendF6\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bint_1_SendF7\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bint_1_SendF8\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bint_1_SendF9\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bint_1_SendF10\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bint_1_SendF11\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bint_1_SendF12\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bint_1_SendNUMLOCK\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bint_1_SendSCROLLLOCK\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bint_1_SendLEFTSHIFT\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bint_1_SendRIGHTSHIFT\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bint_1_SendLEFTCONTROL\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bint_1_SendRIGHTCONTROL\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bint_1_SendLEFTALT\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bint_1_SendRIGHTALT\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bint_1_SendBROWSER_BACK\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bint_1_SendBROWSER_FORWARD\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bint_1_SendBROWSER_REFRESH\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bint_1_SendBROWSER_STOP\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bint_1_SendBROWSER_SEARCH\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bint_1_SendBROWSER_FAVORITES\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bint_1_SendBROWSER_HOME\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bint_1_SendVOLUME_MUTE\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bint_1_SendVOLUME_DOWN\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bint_1_SendVOLUME_UP\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bint_1_SendMEDIA_NEXT_TRACK\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bint_1_SendMEDIA_PREV_TRACK\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bint_1_SendMEDIA_STOP\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bint_1_SendMEDIA_PLAY_PAUSE\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bint_1_SendLAUNCH_MAIL\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bint_1_SendLAUNCH_MEDIA_SELECT\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bint_1_SendLAUNCH_APP1\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bint_1_SendLAUNCH_APP2\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bint_1_SendOEM_1\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bint_1_SendOEM_PLUS\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bint_1_SendOEM_COMMA\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bint_1_SendOEM_MINUS\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bint_1_SendOEM_PERIOD\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bint_1_SendOEM_2\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bint_1_SendOEM_3\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bint_1_SendOEM_4\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bint_1_SendOEM_5\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bint_1_SendOEM_6\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bint_1_SendOEM_7\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bint_1_SendOEM_8\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bint_1_SendOEM_102\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bint_1_SendEREOF\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bint_1_SendZOOM\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bint_1_SendEscape\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bint_1_SendOne\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bint_1_SendTwo\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bint_1_SendThree\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bint_1_SendFour\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bint_1_SendFive\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bint_1_SendSix\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bint_1_SendSeven\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bint_1_SendEight\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bint_1_SendNine\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bint_1_SendZero\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bint_1_SendDashUnderscore\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bint_1_SendPlusEquals\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bint_1_SendBackspace\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bint_1_SendTab\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bint_1_SendOpenBracketBrace\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bint_1_SendCloseBracketBrace\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bint_1_SendEnter\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bint_1_SendControl\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bint_1_SendSemicolonColon\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bint_1_SendSingleDoubleQuote\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bint_1_SendTilde\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bint_1_SendLeftShift\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bint_1_SendBackslashPipe\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bint_1_SendCommaLeftArrow\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bint_1_SendPeriodRightArrow\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bint_1_SendForwardSlashQuestionMark\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bint_1_SendRightShift\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bint_1_SendRightAlt\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bint_1_SendSpace\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bint_1_SendCapsLock\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bint_1_SendUp\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bint_1_SendDown\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bint_1_SendRight\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bint_1_SendLeft\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bint_1_SendHome\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bint_1_SendEnd\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bint_1_SendDelete\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bint_1_SendPageUp\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bint_1_SendPageDown\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bint_1_SendInsert\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bint_1_SendPrintScreen\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bint_1_SendNumLock\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bint_1_SendScrollLock\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bint_1_SendMenu\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bint_1_SendWindowsKey\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bint_1_SendNumpadDivide\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bint_1_SendNumpadAsterisk\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bint_1_SendNumpad7\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bint_1_SendNumpad8\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bint_1_SendNumpad9\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bint_1_SendNumpad4\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bint_1_SendNumpad5\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bint_1_SendNumpad6\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bint_1_SendNumpad1\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bint_1_SendNumpad2\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bint_1_SendNumpad3\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bint_1_SendNumpad0\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bint_1_SendNumpadDelete\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bint_1_SendNumpadEnter\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bint_1_SendNumpadPlus\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bint_1_SendNumpadMinus\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bkeyboard_2_id\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bmouse_2_id\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bint_2_deltaX\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bint_2_deltaY\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bint_2_x\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bint_2_y\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bint_2_SendLeftClick\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bint_2_SendRightClick\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bint_2_SendMiddleClick\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bint_2_SendWheelUp\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bint_2_SendWheelDown\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bint_2_SendCANCEL\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bint_2_SendBACK\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bint_2_SendTAB\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bint_2_SendCLEAR\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bint_2_SendRETURN\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bint_2_SendSHIFT\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bint_2_SendCONTROL\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bint_2_SendMENU\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bint_2_SendCAPITAL\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bint_2_SendESCAPE\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bint_2_SendSPACE\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bint_2_SendPRIOR\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bint_2_SendNEXT\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bint_2_SendEND\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bint_2_SendHOME\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bint_2_SendLEFT\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bint_2_SendUP\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bint_2_SendRIGHT\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bint_2_SendDOWN\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bint_2_SendSNAPSHOT\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bint_2_SendINSERT\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bint_2_SendNUMPADDEL\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bint_2_SendNUMPADINSERT\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bint_2_SendHELP\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bint_2_SendAPOSTROPHE\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bint_2_SendBACKSPACE\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bint_2_SendPAGEDOWN\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bint_2_SendPAGEUP\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bint_2_SendFIN\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bint_2_SendMOUSE\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bint_2_SendA\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bint_2_SendB\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bint_2_SendC\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bint_2_SendD\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bint_2_SendE\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bint_2_SendF\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bint_2_SendG\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bint_2_SendH\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bint_2_SendI\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bint_2_SendJ\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bint_2_SendK\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bint_2_SendL\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bint_2_SendM\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bint_2_SendN\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bint_2_SendO\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bint_2_SendP\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bint_2_SendQ\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bint_2_SendR\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bint_2_SendS\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bint_2_SendT\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bint_2_SendU\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bint_2_SendV\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bint_2_SendW\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bint_2_SendX\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bint_2_SendY\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bint_2_SendZ\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bint_2_SendLWIN\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bint_2_SendRWIN\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bint_2_SendAPPS\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bint_2_SendDELETE\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bint_2_SendNUMPAD0\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bint_2_SendNUMPAD1\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bint_2_SendNUMPAD2\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bint_2_SendNUMPAD3\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bint_2_SendNUMPAD4\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bint_2_SendNUMPAD5\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bint_2_SendNUMPAD6\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bint_2_SendNUMPAD7\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bint_2_SendNUMPAD8\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bint_2_SendNUMPAD9\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bint_2_SendMULTIPLY\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bint_2_SendADD\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bint_2_SendSUBTRACT\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bint_2_SendDECIMAL\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bint_2_SendPRINTSCREEN\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bint_2_SendDIVIDE\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bint_2_SendF1\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bint_2_SendF2\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bint_2_SendF3\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bint_2_SendF4\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bint_2_SendF5\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bint_2_SendF6\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bint_2_SendF7\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bint_2_SendF8\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bint_2_SendF9\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bint_2_SendF10\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bint_2_SendF11\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bint_2_SendF12\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bint_2_SendNUMLOCK\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bint_2_SendSCROLLLOCK\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bint_2_SendLEFTSHIFT\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bint_2_SendRIGHTSHIFT\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bint_2_SendLEFTCONTROL\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bint_2_SendRIGHTCONTROL\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bint_2_SendLEFTALT\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bint_2_SendRIGHTALT\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bint_2_SendBROWSER_BACK\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bint_2_SendBROWSER_FORWARD\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bint_2_SendBROWSER_REFRESH\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bint_2_SendBROWSER_STOP\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bint_2_SendBROWSER_SEARCH\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bint_2_SendBROWSER_FAVORITES\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bint_2_SendBROWSER_HOME\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bint_2_SendVOLUME_MUTE\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bint_2_SendVOLUME_DOWN\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bint_2_SendVOLUME_UP\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bint_2_SendMEDIA_NEXT_TRACK\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bint_2_SendMEDIA_PREV_TRACK\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bint_2_SendMEDIA_STOP\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bint_2_SendMEDIA_PLAY_PAUSE\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bint_2_SendLAUNCH_MAIL\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bint_2_SendLAUNCH_MEDIA_SELECT\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bint_2_SendLAUNCH_APP1\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bint_2_SendLAUNCH_APP2\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bint_2_SendOEM_1\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bint_2_SendOEM_PLUS\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bint_2_SendOEM_COMMA\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bint_2_SendOEM_MINUS\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bint_2_SendOEM_PERIOD\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bint_2_SendOEM_2\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bint_2_SendOEM_3\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bint_2_SendOEM_4\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bint_2_SendOEM_5\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bint_2_SendOEM_6\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bint_2_SendOEM_7\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bint_2_SendOEM_8\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bint_2_SendOEM_102\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bint_2_SendEREOF\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bint_2_SendZOOM\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bint_2_SendEscape\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bint_2_SendOne\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bint_2_SendTwo\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bint_2_SendThree\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bint_2_SendFour\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bint_2_SendFive\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bint_2_SendSix\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bint_2_SendSeven\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bint_2_SendEight\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bint_2_SendNine\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bint_2_SendZero\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bint_2_SendDashUnderscore\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bint_2_SendPlusEquals\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bint_2_SendBackspace\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bint_2_SendTab\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bint_2_SendOpenBracketBrace\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bint_2_SendCloseBracketBrace\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bint_2_SendEnter\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bint_2_SendControl\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bint_2_SendSemicolonColon\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bint_2_SendSingleDoubleQuote\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bint_2_SendTilde\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bint_2_SendLeftShift\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bint_2_SendBackslashPipe\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bint_2_SendCommaLeftArrow\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bint_2_SendPeriodRightArrow\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bint_2_SendForwardSlashQuestionMark\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bint_2_SendRightShift\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bint_2_SendRightAlt\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bint_2_SendSpace\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bint_2_SendCapsLock\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bint_2_SendUp\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bint_2_SendDown\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bint_2_SendRight\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bint_2_SendLeft\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bint_2_SendHome\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bint_2_SendEnd\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bint_2_SendDelete\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bint_2_SendPageUp\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bint_2_SendPageDown\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bint_2_SendInsert\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bint_2_SendPrintScreen\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bint_2_SendNumLock\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bint_2_SendScrollLock\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bint_2_SendMenu\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bint_2_SendWindowsKey\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bint_2_SendNumpadDivide\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bint_2_SendNumpadAsterisk\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bint_2_SendNumpad7\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bint_2_SendNumpad8\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bint_2_SendNumpad9\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bint_2_SendNumpad4\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bint_2_SendNumpad5\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bint_2_SendNumpad6\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bint_2_SendNumpad1\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bint_2_SendNumpad2\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bint_2_SendNumpad3\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bint_2_SendNumpad0\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bint_2_SendNumpadDelete\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bint_2_SendNumpadEnter\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bint_2_SendNumpadPlus\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bint_2_SendNumpadMinus\b"));
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
                range.SetStyle(StyleOutput, new Regex(@"\bController2_send_xbox\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bController2_send_back\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bController2_send_start\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bController2_send_A\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bController2_send_B\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bController2_send_X\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bController2_send_Y\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bController2_send_up\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bController2_send_left\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bController2_send_down\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bController2_send_right\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bController2_send_leftstick\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bController2_send_rightstick\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bController2_send_leftbumper\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bController2_send_rightbumper\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bController2_send_lefttriggerposition\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bController2_send_righttriggerposition\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bController2_send_leftstickx\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bController2_send_leftsticky\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bController2_send_rightstickx\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bController2_send_rightsticky\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bController2DS4_Send_Options\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bController2DS4_Send_ThumbLeft\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bController2DS4_Send_ThumbRight\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bController2DS4_Send_ShoulderLeft\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bController2DS4_Send_ShoulderRight\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bController2DS4_Send_Cross\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bController2DS4_Send_Circle\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bController2DS4_Send_Square\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bController2DS4_Send_Triangle\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bController2DS4_Send_Ps\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bController2DS4_Send_Touchpad\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bController2DS4_Send_Share\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bController2DS4_Send_DPadUp\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bController2DS4_Send_DPadDown\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bController2DS4_Send_DPadLeft\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bController2DS4_Send_DPadRight\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bController2DS4_Send_LeftTrigger\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bController2DS4_Send_RightTrigger\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bController2DS4_Send_LeftTriggerPosition\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bController2DS4_Send_RightTriggerPosition\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bController2DS4_Send_LeftThumbX\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bController2DS4_Send_RightThumbX\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bController2DS4_Send_LeftThumbY\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bController2DS4_Send_RightThumbY\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bController2VJoy_Send_1\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bController2VJoy_Send_2\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bController2VJoy_Send_3\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bController2VJoy_Send_4\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bController2VJoy_Send_5\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bController2VJoy_Send_6\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bController2VJoy_Send_7\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bController2VJoy_Send_8\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bController2VJoy_Send_X\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bController2VJoy_Send_Y\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bController2VJoy_Send_Z\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bController2VJoy_Send_WHL\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bController2VJoy_Send_SL0\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bController2VJoy_Send_SL1\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bController2VJoy_Send_RX\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bController2VJoy_Send_RY\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bController2VJoy_Send_RZ\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bController2VJoy_Send_POV\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bController2VJoy_Send_Hat\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bController2VJoy_Send_HatExt1\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bController2VJoy_Send_HatExt2\b"));
                range.SetStyle(StyleOutput, new Regex(@"\bController2VJoy_Send_HatExt3\b"));
                range.SetStyle(StyleNone, new Regex(@"\w", RegexOptions.Singleline));
            }
            catch { }
        }
        private void FillAutocompletion()
        {
            this.autocompleteMenu1.Items = new string[] {
                "DllImport",
                "EntryPoint",
                "TimeBeginPeriod",
                "TimeEndPeriod",
                "NtSetTimerResolution",
                "List",
                "Scan",
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
                "Run",
                "Thread",
                "Sleep",
                "StringToCode",
                "FooClass",
                "DllImport",
                "System",
                "Globalization",
                "IO",
                "Numerics",
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
                "valListX",
                "valListY",
                "Count",
                "Clear",
                "RemoveAt",
                "Add",
                "Average",
                "Count",
                "Clear",
                "RemoveAt",
                "Add",
                "Average",
                "ViewData",
                "Close",
                "BeginPolling",
                "Connect",
                "Disconnect",
                "Set",
                "Valuechanges",
                "Valuechange",
                "ValueChange",
                "_ValueChange",
                "controllers",
                "XBoxController",
                "XBC",
                "XBC1",
                "XBC2",
                "XInputsAPI",
                "XInput",
                "xi",
                "xi1",
                "xi2",
                "controllersds4",
                "DS4Controller",
                "DS4",
                "DS41",
                "DS42",
                "controllersvjoy",
                "VJoyController",
                "VJoy",
                "VJoy1",
                "VJoy2",
                "DirectInputsAPI",
                "DirectInput",
                "di",
                "di1",
                "di2",
                "DualSensesAPI",
                "DualSense",
                "ds",
                "ds1",
                "ds2",
                "Init",
                "DualShocks4API",
                "DualShock4",
                "ds4",
                "ds41",
                "ds42",
                "Interceptions",
                "SendInterception",
                "Input",
                "SI",
                "Load",
                "WiiMotesAPI",
                "WiiMote",
                "wm",
                "wm1",
                "wm2",
                "SwitchProControllersAPI",
                "SwitchProController",
                "spc",
                "spc1",
                "spc2",
                "JoyconChargingGripsAPI",
                "JoyconChargingGrip",
                "jcg",
                "jcg1",
                "jcg2",
                "JoyconsLeftAPI",
                "JoyconLeft",
                "jl",
                "jl1",
                "jl2",
                "JoyconsRightAPI",
                "JoyconRight",
                "jr",
                "jr1",
                "jr2",
                "KeyboardInputsAPI",
                "MouseInputsAPI",
                "KeyboardInput",
                "MouseInput",
                "mi",
                "ki",
                "mi1",
                "ki1",
                "mi2",
                "ki2",
                "keyboardsmouses",
                "SendKeyboardMouse",
                "SKM",
                "getstate",
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
                "wd",
                "wu",
                "valchanged",
                "Scale",
                "width",
                "height",
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
                "controller1_send_xbox",
                "controller1_send_back",
                "controller1_send_start",
                "controller1_send_A",
                "controller1_send_B",
                "controller1_send_X",
                "controller1_send_Y",
                "controller1_send_up",
                "controller1_send_left",
                "controller1_send_down",
                "controller1_send_right",
                "controller1_send_leftstick",
                "controller1_send_rightstick",
                "controller1_send_leftbumper",
                "controller1_send_rightbumper",
                "controller1_send_lefttriggerposition",
                "controller1_send_righttriggerposition",
                "controller1_send_leftstickx",
                "controller1_send_leftsticky",
                "controller1_send_rightstickx",
                "controller1_send_rightsticky",
                "Controller1DS4_Send_Options",
                "Controller1DS4_Send_ThumbLeft",
                "Controller1DS4_Send_ThumbRight",
                "Controller1DS4_Send_ShoulderLeft",
                "Controller1DS4_Send_ShoulderRight",
                "Controller1DS4_Send_Cross",
                "Controller1DS4_Send_Circle",
                "Controller1DS4_Send_Square",
                "Controller1DS4_Send_Triangle",
                "Controller1DS4_Send_Ps",
                "Controller1DS4_Send_Touchpad",
                "Controller1DS4_Send_Share",
                "Controller1DS4_Send_DPadUp",
                "Controller1DS4_Send_DPadDown",
                "Controller1DS4_Send_DPadLeft",
                "Controller1DS4_Send_DPadRight",
                "Controller1DS4_Send_LeftTrigger",
                "Controller1DS4_Send_RightTrigger",
                "Controller1DS4_Send_LeftTriggerPosition",
                "Controller1DS4_Send_RightTriggerPosition",
                "Controller1DS4_Send_LeftThumbX",
                "Controller1DS4_Send_RightThumbX",
                "Controller1DS4_Send_LeftThumbY",
                "Controller1DS4_Send_RightThumbY",
                "Controller1VJoy_Send_1",
                "Controller1VJoy_Send_2",
                "Controller1VJoy_Send_3",
                "Controller1VJoy_Send_4",
                "Controller1VJoy_Send_5",
                "Controller1VJoy_Send_6",
                "Controller1VJoy_Send_7",
                "Controller1VJoy_Send_8",
                "Controller1VJoy_Send_X",
                "Controller1VJoy_Send_Y",
                "Controller1VJoy_Send_Z",
                "Controller1VJoy_Send_WHL",
                "Controller1VJoy_Send_SL0",
                "Controller1VJoy_Send_SL1",
                "Controller1VJoy_Send_RX",
                "Controller1VJoy_Send_RY",
                "Controller1VJoy_Send_RZ",
                "Controller1VJoy_Send_POV",
                "Controller1VJoy_Send_Hat",
                "Controller1VJoy_Send_HatExt1",
                "Controller1VJoy_Send_HatExt2",
                "Controller1VJoy_Send_HatExt3",
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
                "keyboard_1_id",
                "mouse_1_id",
                "int_1_deltaX",
                "int_1_deltaY",
                "int_1_x",
                "int_1_y",
                "int_1_SendLeftClick",
                "int_1_SendRightClick",
                "int_1_SendMiddleClick",
                "int_1_SendWheelUp",
                "int_1_SendWheelDown",
                "int_1_SendCANCEL",
                "int_1_SendBACK",
                "int_1_SendTAB",
                "int_1_SendCLEAR",
                "int_1_SendRETURN",
                "int_1_SendSHIFT",
                "int_1_SendCONTROL",
                "int_1_SendMENU",
                "int_1_SendCAPITAL",
                "int_1_SendESCAPE",
                "int_1_SendSPACE",
                "int_1_SendPRIOR",
                "int_1_SendNEXT",
                "int_1_SendEND",
                "int_1_SendHOME",
                "int_1_SendLEFT",
                "int_1_SendUP",
                "int_1_SendRIGHT",
                "int_1_SendDOWN",
                "int_1_SendSNAPSHOT",
                "int_1_SendINSERT",
                "int_1_SendNUMPADDEL",
                "int_1_SendNUMPADINSERT",
                "int_1_SendHELP",
                "int_1_SendAPOSTROPHE",
                "int_1_SendBACKSPACE",
                "int_1_SendPAGEDOWN",
                "int_1_SendPAGEUP",
                "int_1_SendFIN",
                "int_1_SendMOUSE",
                "int_1_SendA",
                "int_1_SendB",
                "int_1_SendC",
                "int_1_SendD",
                "int_1_SendE",
                "int_1_SendF",
                "int_1_SendG",
                "int_1_SendH",
                "int_1_SendI",
                "int_1_SendJ",
                "int_1_SendK",
                "int_1_SendL",
                "int_1_SendM",
                "int_1_SendN",
                "int_1_SendO",
                "int_1_SendP",
                "int_1_SendQ",
                "int_1_SendR",
                "int_1_SendS",
                "int_1_SendT",
                "int_1_SendU",
                "int_1_SendV",
                "int_1_SendW",
                "int_1_SendX",
                "int_1_SendY",
                "int_1_SendZ",
                "int_1_SendLWIN",
                "int_1_SendRWIN",
                "int_1_SendAPPS",
                "int_1_SendDELETE",
                "int_1_SendNUMPAD0",
                "int_1_SendNUMPAD1",
                "int_1_SendNUMPAD2",
                "int_1_SendNUMPAD3",
                "int_1_SendNUMPAD4",
                "int_1_SendNUMPAD5",
                "int_1_SendNUMPAD6",
                "int_1_SendNUMPAD7",
                "int_1_SendNUMPAD8",
                "int_1_SendNUMPAD9",
                "int_1_SendMULTIPLY",
                "int_1_SendADD",
                "int_1_SendSUBTRACT",
                "int_1_SendDECIMAL",
                "int_1_SendPRINTSCREEN",
                "int_1_SendDIVIDE",
                "int_1_SendF1",
                "int_1_SendF2",
                "int_1_SendF3",
                "int_1_SendF4",
                "int_1_SendF5",
                "int_1_SendF6",
                "int_1_SendF7",
                "int_1_SendF8",
                "int_1_SendF9",
                "int_1_SendF10",
                "int_1_SendF11",
                "int_1_SendF12",
                "int_1_SendNUMLOCK",
                "int_1_SendSCROLLLOCK",
                "int_1_SendLEFTSHIFT",
                "int_1_SendRIGHTSHIFT",
                "int_1_SendLEFTCONTROL",
                "int_1_SendRIGHTCONTROL",
                "int_1_SendLEFTALT",
                "int_1_SendRIGHTALT",
                "int_1_SendBROWSER_BACK",
                "int_1_SendBROWSER_FORWARD",
                "int_1_SendBROWSER_REFRESH",
                "int_1_SendBROWSER_STOP",
                "int_1_SendBROWSER_SEARCH",
                "int_1_SendBROWSER_FAVORITES",
                "int_1_SendBROWSER_HOME",
                "int_1_SendVOLUME_MUTE",
                "int_1_SendVOLUME_DOWN",
                "int_1_SendVOLUME_UP",
                "int_1_SendMEDIA_NEXT_TRACK",
                "int_1_SendMEDIA_PREV_TRACK",
                "int_1_SendMEDIA_STOP",
                "int_1_SendMEDIA_PLAY_PAUSE",
                "int_1_SendLAUNCH_MAIL",
                "int_1_SendLAUNCH_MEDIA_SELECT",
                "int_1_SendLAUNCH_APP1",
                "int_1_SendLAUNCH_APP2",
                "int_1_SendOEM_1",
                "int_1_SendOEM_PLUS",
                "int_1_SendOEM_COMMA",
                "int_1_SendOEM_MINUS",
                "int_1_SendOEM_PERIOD",
                "int_1_SendOEM_2",
                "int_1_SendOEM_3",
                "int_1_SendOEM_4",
                "int_1_SendOEM_5",
                "int_1_SendOEM_6",
                "int_1_SendOEM_7",
                "int_1_SendOEM_8",
                "int_1_SendOEM_102",
                "int_1_SendEREOF",
                "int_1_SendZOOM",
                "int_1_SendEscape",
                "int_1_SendOne",
                "int_1_SendTwo",
                "int_1_SendThree",
                "int_1_SendFour",
                "int_1_SendFive",
                "int_1_SendSix",
                "int_1_SendSeven",
                "int_1_SendEight",
                "int_1_SendNine",
                "int_1_SendZero",
                "int_1_SendDashUnderscore",
                "int_1_SendPlusEquals",
                "int_1_SendBackspace",
                "int_1_SendTab",
                "int_1_SendOpenBracketBrace",
                "int_1_SendCloseBracketBrace",
                "int_1_SendEnter",
                "int_1_SendControl",
                "int_1_SendSemicolonColon",
                "int_1_SendSingleDoubleQuote",
                "int_1_SendTilde",
                "int_1_SendLeftShift",
                "int_1_SendBackslashPipe",
                "int_1_SendCommaLeftArrow",
                "int_1_SendPeriodRightArrow",
                "int_1_SendForwardSlashQuestionMark",
                "int_1_SendRightShift",
                "int_1_SendRightAlt",
                "int_1_SendSpace",
                "int_1_SendCapsLock",
                "int_1_SendUp",
                "int_1_SendDown",
                "int_1_SendRight",
                "int_1_SendLeft",
                "int_1_SendHome",
                "int_1_SendEnd",
                "int_1_SendDelete",
                "int_1_SendPageUp",
                "int_1_SendPageDown",
                "int_1_SendInsert",
                "int_1_SendPrintScreen",
                "int_1_SendNumLock",
                "int_1_SendScrollLock",
                "int_1_SendMenu",
                "int_1_SendWindowsKey",
                "int_1_SendNumpadDivide",
                "int_1_SendNumpadAsterisk",
                "int_1_SendNumpad7",
                "int_1_SendNumpad8",
                "int_1_SendNumpad9",
                "int_1_SendNumpad4",
                "int_1_SendNumpad5",
                "int_1_SendNumpad6",
                "int_1_SendNumpad1",
                "int_1_SendNumpad2",
                "int_1_SendNumpad3",
                "int_1_SendNumpad0",
                "int_1_SendNumpadDelete",
                "int_1_SendNumpadEnter",
                "int_1_SendNumpadPlus",
                "int_1_SendNumpadMinus",
                "keyboard_2_id",
                "mouse_2_id",
                "int_2_deltaX",
                "int_2_deltaY",
                "int_2_x",
                "int_2_y",
                "int_2_SendLeftClick",
                "int_2_SendRightClick",
                "int_2_SendMiddleClick",
                "int_2_SendWheelUp",
                "int_2_SendWheelDown",
                "int_2_SendCANCEL",
                "int_2_SendBACK",
                "int_2_SendTAB",
                "int_2_SendCLEAR",
                "int_2_SendRETURN",
                "int_2_SendSHIFT",
                "int_2_SendCONTROL",
                "int_2_SendMENU",
                "int_2_SendCAPITAL",
                "int_2_SendESCAPE",
                "int_2_SendSPACE",
                "int_2_SendPRIOR",
                "int_2_SendNEXT",
                "int_2_SendEND",
                "int_2_SendHOME",
                "int_2_SendLEFT",
                "int_2_SendUP",
                "int_2_SendRIGHT",
                "int_2_SendDOWN",
                "int_2_SendSNAPSHOT",
                "int_2_SendINSERT",
                "int_2_SendNUMPADDEL",
                "int_2_SendNUMPADINSERT",
                "int_2_SendHELP",
                "int_2_SendAPOSTROPHE",
                "int_2_SendBACKSPACE",
                "int_2_SendPAGEDOWN",
                "int_2_SendPAGEUP",
                "int_2_SendFIN",
                "int_2_SendMOUSE",
                "int_2_SendA",
                "int_2_SendB",
                "int_2_SendC",
                "int_2_SendD",
                "int_2_SendE",
                "int_2_SendF",
                "int_2_SendG",
                "int_2_SendH",
                "int_2_SendI",
                "int_2_SendJ",
                "int_2_SendK",
                "int_2_SendL",
                "int_2_SendM",
                "int_2_SendN",
                "int_2_SendO",
                "int_2_SendP",
                "int_2_SendQ",
                "int_2_SendR",
                "int_2_SendS",
                "int_2_SendT",
                "int_2_SendU",
                "int_2_SendV",
                "int_2_SendW",
                "int_2_SendX",
                "int_2_SendY",
                "int_2_SendZ",
                "int_2_SendLWIN",
                "int_2_SendRWIN",
                "int_2_SendAPPS",
                "int_2_SendDELETE",
                "int_2_SendNUMPAD0",
                "int_2_SendNUMPAD1",
                "int_2_SendNUMPAD2",
                "int_2_SendNUMPAD3",
                "int_2_SendNUMPAD4",
                "int_2_SendNUMPAD5",
                "int_2_SendNUMPAD6",
                "int_2_SendNUMPAD7",
                "int_2_SendNUMPAD8",
                "int_2_SendNUMPAD9",
                "int_2_SendMULTIPLY",
                "int_2_SendADD",
                "int_2_SendSUBTRACT",
                "int_2_SendDECIMAL",
                "int_2_SendPRINTSCREEN",
                "int_2_SendDIVIDE",
                "int_2_SendF1",
                "int_2_SendF2",
                "int_2_SendF3",
                "int_2_SendF4",
                "int_2_SendF5",
                "int_2_SendF6",
                "int_2_SendF7",
                "int_2_SendF8",
                "int_2_SendF9",
                "int_2_SendF10",
                "int_2_SendF11",
                "int_2_SendF12",
                "int_2_SendNUMLOCK",
                "int_2_SendSCROLLLOCK",
                "int_2_SendLEFTSHIFT",
                "int_2_SendRIGHTSHIFT",
                "int_2_SendLEFTCONTROL",
                "int_2_SendRIGHTCONTROL",
                "int_2_SendLEFTALT",
                "int_2_SendRIGHTALT",
                "int_2_SendBROWSER_BACK",
                "int_2_SendBROWSER_FORWARD",
                "int_2_SendBROWSER_REFRESH",
                "int_2_SendBROWSER_STOP",
                "int_2_SendBROWSER_SEARCH",
                "int_2_SendBROWSER_FAVORITES",
                "int_2_SendBROWSER_HOME",
                "int_2_SendVOLUME_MUTE",
                "int_2_SendVOLUME_DOWN",
                "int_2_SendVOLUME_UP",
                "int_2_SendMEDIA_NEXT_TRACK",
                "int_2_SendMEDIA_PREV_TRACK",
                "int_2_SendMEDIA_STOP",
                "int_2_SendMEDIA_PLAY_PAUSE",
                "int_2_SendLAUNCH_MAIL",
                "int_2_SendLAUNCH_MEDIA_SELECT",
                "int_2_SendLAUNCH_APP1",
                "int_2_SendLAUNCH_APP2",
                "int_2_SendOEM_1",
                "int_2_SendOEM_PLUS",
                "int_2_SendOEM_COMMA",
                "int_2_SendOEM_MINUS",
                "int_2_SendOEM_PERIOD",
                "int_2_SendOEM_2",
                "int_2_SendOEM_3",
                "int_2_SendOEM_4",
                "int_2_SendOEM_5",
                "int_2_SendOEM_6",
                "int_2_SendOEM_7",
                "int_2_SendOEM_8",
                "int_2_SendOEM_102",
                "int_2_SendEREOF",
                "int_2_SendZOOM",
                "int_2_SendEscape",
                "int_2_SendOne",
                "int_2_SendTwo",
                "int_2_SendThree",
                "int_2_SendFour",
                "int_2_SendFive",
                "int_2_SendSix",
                "int_2_SendSeven",
                "int_2_SendEight",
                "int_2_SendNine",
                "int_2_SendZero",
                "int_2_SendDashUnderscore",
                "int_2_SendPlusEquals",
                "int_2_SendBackspace",
                "int_2_SendTab",
                "int_2_SendOpenBracketBrace",
                "int_2_SendCloseBracketBrace",
                "int_2_SendEnter",
                "int_2_SendControl",
                "int_2_SendSemicolonColon",
                "int_2_SendSingleDoubleQuote",
                "int_2_SendTilde",
                "int_2_SendLeftShift",
                "int_2_SendBackslashPipe",
                "int_2_SendCommaLeftArrow",
                "int_2_SendPeriodRightArrow",
                "int_2_SendForwardSlashQuestionMark",
                "int_2_SendRightShift",
                "int_2_SendRightAlt",
                "int_2_SendSpace",
                "int_2_SendCapsLock",
                "int_2_SendUp",
                "int_2_SendDown",
                "int_2_SendRight",
                "int_2_SendLeft",
                "int_2_SendHome",
                "int_2_SendEnd",
                "int_2_SendDelete",
                "int_2_SendPageUp",
                "int_2_SendPageDown",
                "int_2_SendInsert",
                "int_2_SendPrintScreen",
                "int_2_SendNumLock",
                "int_2_SendScrollLock",
                "int_2_SendMenu",
                "int_2_SendWindowsKey",
                "int_2_SendNumpadDivide",
                "int_2_SendNumpadAsterisk",
                "int_2_SendNumpad7",
                "int_2_SendNumpad8",
                "int_2_SendNumpad9",
                "int_2_SendNumpad4",
                "int_2_SendNumpad5",
                "int_2_SendNumpad6",
                "int_2_SendNumpad1",
                "int_2_SendNumpad2",
                "int_2_SendNumpad3",
                "int_2_SendNumpad0",
                "int_2_SendNumpadDelete",
                "int_2_SendNumpadEnter",
                "int_2_SendNumpadPlus",
                "int_2_SendNumpadMinus",
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
                "Controller2_send_xbox",
                "Controller2_send_back",
                "Controller2_send_start",
                "Controller2_send_A",
                "Controller2_send_B",
                "Controller2_send_X",
                "Controller2_send_Y",
                "Controller2_send_up",
                "Controller2_send_left",
                "Controller2_send_down",
                "Controller2_send_right",
                "Controller2_send_leftstick",
                "Controller2_send_rightstick",
                "Controller2_send_leftbumper",
                "Controller2_send_rightbumper",
                "Controller2_send_lefttriggerposition",
                "Controller2_send_righttriggerposition",
                "Controller2_send_leftstickx",
                "Controller2_send_leftsticky",
                "Controller2_send_rightstickx",
                "Controller2_send_rightsticky",
                "Controller2DS4_Send_Options",
                "Controller2DS4_Send_ThumbLeft",
                "Controller2DS4_Send_ThumbRight",
                "Controller2DS4_Send_ShoulderLeft",
                "Controller2DS4_Send_ShoulderRight",
                "Controller2DS4_Send_Cross",
                "Controller2DS4_Send_Circle",
                "Controller2DS4_Send_Square",
                "Controller2DS4_Send_Triangle",
                "Controller2DS4_Send_Ps",
                "Controller2DS4_Send_Touchpad",
                "Controller2DS4_Send_Share",
                "Controller2DS4_Send_DPadUp",
                "Controller2DS4_Send_DPadDown",
                "Controller2DS4_Send_DPadLeft",
                "Controller2DS4_Send_DPadRight",
                "Controller2DS4_Send_LeftTrigger",
                "Controller2DS4_Send_RightTrigger",
                "Controller2DS4_Send_LeftTriggerPosition",
                "Controller2DS4_Send_RightTriggerPosition",
                "Controller2DS4_Send_LeftThumbX",
                "Controller2DS4_Send_RightThumbX",
                "Controller2DS4_Send_LeftThumbY",
                "Controller2DS4_Send_RightThumbY",
                "Controller2VJoy_Send_1",
                "Controller2VJoy_Send_2",
                "Controller2VJoy_Send_3",
                "Controller2VJoy_Send_4",
                "Controller2VJoy_Send_5",
                "Controller2VJoy_Send_6",
                "Controller2VJoy_Send_7",
                "Controller2VJoy_Send_8",
                "Controller2VJoy_Send_X",
                "Controller2VJoy_Send_Y",
                "Controller2VJoy_Send_Z",
                "Controller2VJoy_Send_WHL",
                "Controller2VJoy_Send_SL0",
                "Controller2VJoy_Send_SL1",
                "Controller2VJoy_Send_RX",
                "Controller2VJoy_Send_RY",
                "Controller2VJoy_Send_RZ",
                "Controller2VJoy_Send_POV",
                "Controller2VJoy_Send_Hat",
                "Controller2VJoy_Send_HatExt1",
                "Controller2VJoy_Send_HatExt2",
                "Controller2VJoy_Send_HatExt3"
            };
        }
        private void fastColoredTextBox1_TextChanged(object sender, FastColoredTextBoxNS.TextChangedEventArgs e)
        {
            if (fastColoredTextBoxSaved != fastColoredTextBox1.Text)
                justSaved = false;
            ChangeScriptColors(sender);
        }
        private void helpToolStripMenuItem_Click(object sender, EventArgs e)
        {
            const string message = "• Input Devices : Wiimote(s) and Nunchuck(s), Joycon(s) left, Joycon(s) right, Switch Pro Controller(s), Joycon Charging Grip(s), DirectInput Controller(s), Keyboard(s), Mouse(s), Dualsense(s), Dualshock(s)4, Xbox Controller(s).\n\r\n\r• Output Devices : Xbox Controller(s), Keyboard and Mouse, Interception(s) (Int), Dualshock4 Controller(s), VJoy Controller(s).\n\r\n\r• Pairing Devices : Wiimote(s) or Joycon(s) left or Joycon(s) right need to be set in pairing mode after starting the run process, Switch Pro Controller(s) or Joycon Charging Grip(s) or DirectInput Controller(s) or Dualsense(s) or Dualshock(s)4 or Xbox Controller(s) or Keyboard(s) or Mouse(s) need to be USB wired.";
            const string caption = "Help";
            MessageBox.Show(message, caption, MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            const string message = "• Author: Michaël André Franiatte.\n\r\n\r• Copyrights: All rights reserved, no permissions granted.\n\r\n\r• Contact: michael.franiatte@gmail.com.";
            const string caption = "About";
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
        void MenuTest1_Click(object sender, EventArgs e)
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
                using (System.IO.StreamReader file = new System.IO.StreamReader(Application.StartupPath + @"\temphandle"))
                {
                    IntPtr handle = new IntPtr(int.Parse(file.ReadLine()));
                    ShowWindow(handle, 9);
                    SetForegroundWindow(handle);
                    Microsoft.VisualBasic.Interaction.AppActivate(file.ReadLine());
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
            }
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
            using (System.IO.StreamWriter createdfile = new System.IO.StreamWriter(Application.StartupPath + @"\temphandle"))
            {
                createdfile.WriteLine(Process.GetCurrentProcess().MainWindowHandle);
                createdfile.WriteLine(this.Text);
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
            using (System.IO.StreamWriter createdfile = new System.IO.StreamWriter(Application.StartupPath + @"\temphandle"))
            {
                createdfile.WriteLine(Process.GetCurrentProcess().MainWindowHandle);
                createdfile.WriteLine(this.Text);
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
            using (System.IO.StreamWriter createdfile = new System.IO.StreamWriter(Application.StartupPath + @"\temphandle"))
            {
                createdfile.WriteLine(Process.GetCurrentProcess().MainWindowHandle);
                createdfile.WriteLine(this.Text);
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
            parameters.GenerateExecutable = false;
            parameters.GenerateInMemory = true;
            parameters.IncludeDebugInformation = false;
            parameters.CompilerOptions = "/optimize";
            parameters.ReferencedAssemblies.Add(Application.StartupPath + @"\System.dll");
            parameters.ReferencedAssemblies.Add(Application.StartupPath + @"\System.Windows.Forms.dll");
            parameters.ReferencedAssemblies.Add(Application.StartupPath + @"\System.Drawing.dll");
            parameters.ReferencedAssemblies.Add(Application.StartupPath + @"\System.Runtime.dll");
            parameters.ReferencedAssemblies.Add(Application.StartupPath + @"\System.Collections.dll");
            parameters.ReferencedAssemblies.Add(Application.StartupPath + @"\System.Linq.dll");
            parameters.ReferencedAssemblies.Add(Application.StartupPath + @"\System.Numerics.Vectors.dll");
            parameters.ReferencedAssemblies.Add(Application.StartupPath + @"\System.Numerics.dll");
            parameters.ReferencedAssemblies.Add(Application.StartupPath + @"\System.Core.dll");
            parameters.ReferencedAssemblies.Add(Application.StartupPath + @"\netstandard.dll");
            if (code.Contains("using controllers;"))
                parameters.ReferencedAssemblies.Add(Application.StartupPath + @"\controllers.dll");
            if (code.Contains("using controllersds4;"))
                parameters.ReferencedAssemblies.Add(Application.StartupPath + @"\controllersds4.dll");
            if (code.Contains("using controllersvjoy;"))
                parameters.ReferencedAssemblies.Add(Application.StartupPath + @"\controllersvjoy.dll");
            if (code.Contains("using keyboardsmouses;"))
                parameters.ReferencedAssemblies.Add(Application.StartupPath + @"\keyboardsmouses.dll");
            if (code.Contains("using Interceptions;"))
                parameters.ReferencedAssemblies.Add(Application.StartupPath + @"\Interceptions.dll");
            if (code.Contains("using Valuechanges;"))
                parameters.ReferencedAssemblies.Add(Application.StartupPath + @"\Valuechanges.dll");
            if (code.Contains("using KeyboardInputsAPI;"))
                parameters.ReferencedAssemblies.Add(Application.StartupPath + @"\Keyboardinputs.dll");
            if (code.Contains("using MouseInputsAPI;"))
                parameters.ReferencedAssemblies.Add(Application.StartupPath + @"\Mouseinputs.dll");
            if (code.Contains("using DualSensesAPI;"))
                parameters.ReferencedAssemblies.Add(Application.StartupPath + @"\Dualsenses.dll");
            if (code.Contains("using DualShocks4API;"))
                parameters.ReferencedAssemblies.Add(Application.StartupPath + @"\Dualshocks4.dll");
            if (code.Contains("using DirectInputsAPI;"))
                parameters.ReferencedAssemblies.Add(Application.StartupPath + @"\Directinputs.dll");
            if (code.Contains("using JoyconChargingGripsAPI;"))
                parameters.ReferencedAssemblies.Add(Application.StartupPath + @"\Joyconcharginggrips.dll");
            if (code.Contains("using JoyconsLeftAPI;"))
                parameters.ReferencedAssemblies.Add(Application.StartupPath + @"\Joyconsleft.dll");
            if (code.Contains("using JoyconsRightAPI;"))
                parameters.ReferencedAssemblies.Add(Application.StartupPath + @"\Joyconsright.dll");
            if (code.Contains("using SwitchProControllersAPI;"))
                parameters.ReferencedAssemblies.Add(Application.StartupPath + @"\Switchprocontrollers.dll");
            if (code.Contains("using WiiMotesAPI;"))
                parameters.ReferencedAssemblies.Add(Application.StartupPath + @"\Wiimotes.dll");
            if (code.Contains("using XInputsAPI;"))
                parameters.ReferencedAssemblies.Add(Application.StartupPath + @"\Xinputs.dll");
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
                using System.Windows;
                using System.Windows.Forms;
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
                        [DllImport(""MotionInputPairing.dll"", EntryPoint = ""wiimotesconnect"")]
                        public static extern bool wiimotesconnect();
                        [DllImport(""MotionInputPairing.dll"", EntryPoint = ""wiimotesdisconnect"")]
                        public static extern bool wiimotesdisconnect();
                        [DllImport(""MotionInputPairing.dll"", EntryPoint = ""joyconsleftconnect"")]
                        public static extern bool joyconsleftconnect();
                        [DllImport(""MotionInputPairing.dll"", EntryPoint = ""joyconsleftdisconnect"")]
                        public static extern bool joyconsleftdisconnect();
                        [DllImport(""MotionInputPairing.dll"", EntryPoint = ""joyconsrightconnect"")]
                        public static extern bool joyconsrightconnect();
                        [DllImport(""MotionInputPairing.dll"", EntryPoint = ""joyconsrightdisconnect"")]
                        public static extern bool joyconsrightdisconnect();
                        public void Disconnect()
                        {
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
                                wiimotesconnect();
                                wiimotesdisconnect();
                            }
                            catch { }
                            try
                            {
                                joyconsleftconnect();
                                joyconsleftdisconnect();
                            }
                            catch { }
                            try
                            {
                                joyconsrightconnect();
                                joyconsrightdisconnect();
                            }
                            catch { }
                        }
                    }
                }";
            parameters = new System.CodeDom.Compiler.CompilerParameters();
            parameters.GenerateExecutable = false;
            parameters.GenerateInMemory = true;
            parameters.ReferencedAssemblies.Add(Application.StartupPath + @"\System.Windows.Forms.dll");
            parameters.ReferencedAssemblies.Add(Application.StartupPath + @"\System.Drawing.dll");
            provider = new Microsoft.CSharp.CSharpCodeProvider();
            results = provider.CompileAssemblyFromSource(parameters, finalcode);
            assembly = results.CompiledAssembly;
            program = assembly.GetType("StringToCode.FooClass");
            obj = Activator.CreateInstance(program);
            program.InvokeMember("Disconnect", BindingFlags.Default | BindingFlags.InvokeMethod, null, obj, new object[] { });
        }
        private void startProgramAtBootToolStripMenuItem_CheckStateChanged(object sender, EventArgs e)
        {
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
    }
}