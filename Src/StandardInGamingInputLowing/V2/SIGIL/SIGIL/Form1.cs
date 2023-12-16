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
        private static string filename = "", stringscript = "", fastColoredTextBoxSaved = "";
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
        private string code;
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
        private void associateFileExtensionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FileAssociationHelper.AssociateFileExtension(".sig", "ScriptSIGILFile", "Script SIGIL File", Application.ExecutablePath);
        }
        private void removeFileAssociationToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FileAssociationHelper.RemoveFileAssociation(".sig", "ScriptSIGILFile");
        }
        private void ChangeScriptColors(object sender)
        {
            try
            {
                range = (sender as FastColoredTextBox).Range;
                range.SetStyle(StyleMethod, new Regex(@"\bTimeBeginPeriod\b"));
                range.SetStyle(StyleMethod, new Regex(@"\bTimeEndPeriod\b"));
                range.SetStyle(StyleMethod, new Regex(@"\bNtSetTimerResolution\b"));
                range.SetStyle(StyleClass, new Regex(@"\bList\b"));
                range.SetStyle(StyleMethod, new Regex(@"\bScanGrip\b"));
                range.SetStyle(StyleMethod, new Regex(@"\bScanLeftJoycon\b"));
                range.SetStyle(StyleMethod, new Regex(@"\bScanRightJoycon\b"));
                range.SetStyle(StyleMethod, new Regex(@"\bScanPro\b"));
                range.SetStyle(StyleMethod, new Regex(@"\bScanWiimote\b"));
                range.SetStyle(StyleExtra, new Regex(@"\bCurrentResolution\b"));
                range.SetStyle(StyleExtra, new Regex(@"\brunning\b"));
                range.SetStyle(StyleExtra, new Regex(@"\birmode\b"));
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
                range.SetStyle(StyleMethod, new Regex(@"\bSetController\b"));
                range.SetStyle(StyleMethod, new Regex(@"\bScanDualsense\b"));
                range.SetStyle(StyleMethod, new Regex(@"\bScanDualshock4\b"));
                range.SetStyle(StyleMethod, new Regex(@"\bSetKM\b"));
                range.SetStyle(StyleLibrary, new Regex(@"\bValuechanges\b"));
                range.SetStyle(StyleClass, new Regex(@"\bValuechange\b"));
                range.SetStyle(StyleObject, new Regex(@"\bValueChange\b"));
                range.SetStyle(StyleMethod, new Regex(@"\b_ValueChange\b"));
                range.SetStyle(StyleLibrary, new Regex(@"\bcontrollers\b"));
                range.SetStyle(StyleLibrary, new Regex(@"\bcontroller\b"));
                range.SetStyle(StyleClass, new Regex(@"\bXBoxController\b"));
                range.SetStyle(StyleObject, new Regex(@"\bXBC\b"));
                range.SetStyle(StyleLibrary, new Regex(@"\bXInputAPI\b"));
                range.SetStyle(StyleLibrary, new Regex(@"\bXInputsAPI\b"));
                range.SetStyle(StyleClass, new Regex(@"\bXInput\b"));
                range.SetStyle(StyleObject, new Regex(@"\bxi\b"));
                range.SetStyle(StyleMethod, new Regex(@"\bScanXInput\b"));
                range.SetStyle(StyleLibrary, new Regex(@"\bcontrollersds4\b"));
                range.SetStyle(StyleLibrary, new Regex(@"\bcontrollerds4\b"));
                range.SetStyle(StyleClass, new Regex(@"\bDS4Controller\b"));
                range.SetStyle(StyleObject, new Regex(@"\bDS4\b"));
                range.SetStyle(StyleLibrary, new Regex(@"\bcontrollersvjoy\b"));
                range.SetStyle(StyleLibrary, new Regex(@"\bcontrollervjoy\b"));
                range.SetStyle(StyleClass, new Regex(@"\bVJoyController\b"));
                range.SetStyle(StyleObject, new Regex(@"\bVJoy\b"));
                range.SetStyle(StyleLibrary, new Regex(@"\bDirectInputAPI\b"));
                range.SetStyle(StyleLibrary, new Regex(@"\bDirectInputsAPI\b"));
                range.SetStyle(StyleClass, new Regex(@"\bDirectInput\b"));
                range.SetStyle(StyleObject, new Regex(@"\bdi\b"));
                range.SetStyle(StyleMethod, new Regex(@"\bScanDirectInput\b"));
                range.SetStyle(StyleLibrary, new Regex(@"\bDualSenseAPI\b"));
                range.SetStyle(StyleLibrary, new Regex(@"\bDualSensesAPI\b"));
                range.SetStyle(StyleClass, new Regex(@"\bDualSense\b"));
                range.SetStyle(StyleObject, new Regex(@"\bds\b"));
                range.SetStyle(StyleMethod, new Regex(@"\bInitDualSenseAccel\b"));
                range.SetStyle(StyleLibrary, new Regex(@"\bDualShock4API\b"));
                range.SetStyle(StyleLibrary, new Regex(@"\bDualShocks4API\b"));
                range.SetStyle(StyleClass, new Regex(@"\bDualShock4\b"));
                range.SetStyle(StyleObject, new Regex(@"\bds4\b"));
                range.SetStyle(StyleMethod, new Regex(@"\bInitDualShock4Accel\b"));
                range.SetStyle(StyleLibrary, new Regex(@"\bInterceptions\b"));
                range.SetStyle(StyleClass, new Regex(@"\bSendInterception\b"));
                range.SetStyle(StyleClass, new Regex(@"\bInput\b"));
                range.SetStyle(StyleObject, new Regex(@"\bsi\b"));
                range.SetStyle(StyleMethod, new Regex(@"\bLoad\b"));
                range.SetStyle(StyleLibrary, new Regex(@"\bWiiMoteAPI\b"));
                range.SetStyle(StyleClass, new Regex(@"\bWiiMote\b"));
                range.SetStyle(StyleObject, new Regex(@"\bwm\b"));
                range.SetStyle(StyleMethod, new Regex(@"\bInitWiimote\b"));
                range.SetStyle(StyleLibrary, new Regex(@"\bSwitchProControllerAPI\b"));
                range.SetStyle(StyleClass, new Regex(@"\bSwitchProController\b"));
                range.SetStyle(StyleObject, new Regex(@"\bspc\b"));
                range.SetStyle(StyleMethod, new Regex(@"\bInitProController\b"));
                range.SetStyle(StyleMethod, new Regex(@"\bInitProControllerAccel\b"));
                range.SetStyle(StyleMethod, new Regex(@"\bInitProControllerStick\b"));
                range.SetStyle(StyleLibrary, new Regex(@"\bJoyconChargingGripAPI\b"));
                range.SetStyle(StyleClass, new Regex(@"\bJoyconChargingGrip\b"));
                range.SetStyle(StyleObject, new Regex(@"\bjcg\b"));
                range.SetStyle(StyleMethod, new Regex(@"\bInitJoyconChargingGrip\b"));
                range.SetStyle(StyleMethod, new Regex(@"\bInitJoyconChargingGripAccel\b"));
                range.SetStyle(StyleMethod, new Regex(@"\bInitJoyconChargingGripStick\b"));
                range.SetStyle(StyleLibrary, new Regex(@"\bJoyconLeftAPI\b"));
                range.SetStyle(StyleClass, new Regex(@"\bJoyconLeft\b"));
                range.SetStyle(StyleObject, new Regex(@"\bjl\b"));
                range.SetStyle(StyleMethod, new Regex(@"\bInitLeftJoycon\b"));
                range.SetStyle(StyleMethod, new Regex(@"\bInitLeftJoyconAccel\b"));
                range.SetStyle(StyleMethod, new Regex(@"\bInitLeftJoyconStick\b"));
                range.SetStyle(StyleLibrary, new Regex(@"\bJoyconRightAPI\b"));
                range.SetStyle(StyleClass, new Regex(@"\bJoyconRight\b"));
                range.SetStyle(StyleObject, new Regex(@"\bjr\b"));
                range.SetStyle(StyleMethod, new Regex(@"\bInitRightJoycon\b"));
                range.SetStyle(StyleMethod, new Regex(@"\bInitRightJoyconAccel\b"));
                range.SetStyle(StyleMethod, new Regex(@"\bInitRightJoyconStick\b"));
                range.SetStyle(StyleLibrary, new Regex(@"\bKeyboardMouseInputAPI\b"));
                range.SetStyle(StyleLibrary, new Regex(@"\bKeyboardsMousesInputAPI\b"));
                range.SetStyle(StyleClass, new Regex(@"\bKeyboardMouseInput\b"));
                range.SetStyle(StyleObject, new Regex(@"\bkmi\b"));
                range.SetStyle(StyleMethod, new Regex(@"\bBeginPollingMouse\b"));
                range.SetStyle(StyleMethod, new Regex(@"\bBeginPollingKeyboard\b"));
                range.SetStyle(StyleMethod, new Regex(@"\bScanMouse\b"));
                range.SetStyle(StyleMethod, new Regex(@"\bScanKeyboard\b"));
                range.SetStyle(StyleLibrary, new Regex(@"\bkeyboards\b"));
                range.SetStyle(StyleClass, new Regex(@"\bSendKeyboard\b"));
                range.SetStyle(StyleObject, new Regex(@"\bsk\b"));
                range.SetStyle(StyleLibrary, new Regex(@"\bmouses\b"));
                range.SetStyle(StyleClass, new Regex(@"\bSendMouse\b"));
                range.SetStyle(StyleObject, new Regex(@"\bsm\b"));
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
                range.SetStyle(StyleInput, new Regex(@"\bController1ButtonAPressed\b"));
                range.SetStyle(StyleInput, new Regex(@"\bController1ButtonBPressed\b"));
                range.SetStyle(StyleInput, new Regex(@"\bController1ButtonXPressed\b"));
                range.SetStyle(StyleInput, new Regex(@"\bController1ButtonYPressed\b"));
                range.SetStyle(StyleInput, new Regex(@"\bController1ButtonStartPressed\b"));
                range.SetStyle(StyleInput, new Regex(@"\bController1ButtonBackPressed\b"));
                range.SetStyle(StyleInput, new Regex(@"\bController1ButtonDownPressed\b"));
                range.SetStyle(StyleInput, new Regex(@"\bController1ButtonUpPressed\b"));
                range.SetStyle(StyleInput, new Regex(@"\bController1ButtonLeftPressed\b"));
                range.SetStyle(StyleInput, new Regex(@"\bController1ButtonRightPressed\b"));
                range.SetStyle(StyleInput, new Regex(@"\bController1ButtonShoulderLeftPressed\b"));
                range.SetStyle(StyleInput, new Regex(@"\bController1ButtonShoulderRightPressed\b"));
                range.SetStyle(StyleInput, new Regex(@"\bController1ThumbpadLeftPressed\b"));
                range.SetStyle(StyleInput, new Regex(@"\bController1ThumbpadRightPressed\b"));
                range.SetStyle(StyleInput, new Regex(@"\bController1TriggerLeftPosition\b"));
                range.SetStyle(StyleInput, new Regex(@"\bController1TriggerRightPosition\b"));
                range.SetStyle(StyleInput, new Regex(@"\bController1ThumbLeftX\b"));
                range.SetStyle(StyleInput, new Regex(@"\bController1ThumbLeftY\b"));
                range.SetStyle(StyleInput, new Regex(@"\bController1ThumbRightX\b"));
                range.SetStyle(StyleInput, new Regex(@"\bController1ThumbRightY\b"));
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
                range.SetStyle(StyleOutput, new Regex(@"\bController1DS4_Send_Option\b"));
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
                range.SetStyle(StyleInput, new Regex(@"\bJoystick1AxisX\b"));
                range.SetStyle(StyleInput, new Regex(@"\bJoystick1AxisY\b"));
                range.SetStyle(StyleInput, new Regex(@"\bJoystick1AxisZ\b"));
                range.SetStyle(StyleInput, new Regex(@"\bJoystick1RotationX\b"));
                range.SetStyle(StyleInput, new Regex(@"\bJoystick1RotationY\b"));
                range.SetStyle(StyleInput, new Regex(@"\bJoystick1RotationZ\b"));
                range.SetStyle(StyleInput, new Regex(@"\bJoystick1Sliders0\b"));
                range.SetStyle(StyleInput, new Regex(@"\bJoystick1Sliders1\b"));
                range.SetStyle(StyleInput, new Regex(@"\bJoystick1PointOfViewControllers0\b"));
                range.SetStyle(StyleInput, new Regex(@"\bJoystick1PointOfViewControllers1\b"));
                range.SetStyle(StyleInput, new Regex(@"\bJoystick1PointOfViewControllers2\b"));
                range.SetStyle(StyleInput, new Regex(@"\bJoystick1PointOfViewControllers3\b"));
                range.SetStyle(StyleInput, new Regex(@"\bJoystick1VelocityX\b"));
                range.SetStyle(StyleInput, new Regex(@"\bJoystick1VelocityY\b"));
                range.SetStyle(StyleInput, new Regex(@"\bJoystick1VelocityZ\b"));
                range.SetStyle(StyleInput, new Regex(@"\bJoystick1AngularVelocityX\b"));
                range.SetStyle(StyleInput, new Regex(@"\bJoystick1AngularVelocityY\b"));
                range.SetStyle(StyleInput, new Regex(@"\bJoystick1AngularVelocityZ\b"));
                range.SetStyle(StyleInput, new Regex(@"\bJoystick1VelocitySliders0\b"));
                range.SetStyle(StyleInput, new Regex(@"\bJoystick1VelocitySliders1\b"));
                range.SetStyle(StyleInput, new Regex(@"\bJoystick1AccelerationX\b"));
                range.SetStyle(StyleInput, new Regex(@"\bJoystick1AccelerationY\b"));
                range.SetStyle(StyleInput, new Regex(@"\bJoystick1AccelerationZ\b"));
                range.SetStyle(StyleInput, new Regex(@"\bJoystick1AngularAccelerationX\b"));
                range.SetStyle(StyleInput, new Regex(@"\bJoystick1AngularAccelerationY\b"));
                range.SetStyle(StyleInput, new Regex(@"\bJoystick1AngularAccelerationZ\b"));
                range.SetStyle(StyleInput, new Regex(@"\bJoystick1AccelerationSliders0\b"));
                range.SetStyle(StyleInput, new Regex(@"\bJoystick1AccelerationSliders1\b"));
                range.SetStyle(StyleInput, new Regex(@"\bJoystick1ForceX\b"));
                range.SetStyle(StyleInput, new Regex(@"\bJoystick1ForceY\b"));
                range.SetStyle(StyleInput, new Regex(@"\bJoystick1ForceZ\b"));
                range.SetStyle(StyleInput, new Regex(@"\bJoystick1TorqueX\b"));
                range.SetStyle(StyleInput, new Regex(@"\bJoystick1TorqueY\b"));
                range.SetStyle(StyleInput, new Regex(@"\bJoystick1TorqueZ\b"));
                range.SetStyle(StyleInput, new Regex(@"\bJoystick1ForceSliders0\b"));
                range.SetStyle(StyleInput, new Regex(@"\bJoystick1ForceSliders1\b"));
                range.SetStyle(StyleInput, new Regex(@"\bJoystick1Buttons0\b"));
                range.SetStyle(StyleInput, new Regex(@"\bJoystick1Buttons1\b"));
                range.SetStyle(StyleInput, new Regex(@"\bJoystick1Buttons2\b"));
                range.SetStyle(StyleInput, new Regex(@"\bJoystick1Buttons3\b"));
                range.SetStyle(StyleInput, new Regex(@"\bJoystick1Buttons4\b"));
                range.SetStyle(StyleInput, new Regex(@"\bJoystick1Buttons5\b"));
                range.SetStyle(StyleInput, new Regex(@"\bJoystick1Buttons6\b"));
                range.SetStyle(StyleInput, new Regex(@"\bJoystick1Buttons7\b"));
                range.SetStyle(StyleInput, new Regex(@"\bJoystick1Buttons8\b"));
                range.SetStyle(StyleInput, new Regex(@"\bJoystick1Buttons9\b"));
                range.SetStyle(StyleInput, new Regex(@"\bJoystick1Buttons10\b"));
                range.SetStyle(StyleInput, new Regex(@"\bJoystick1Buttons11\b"));
                range.SetStyle(StyleInput, new Regex(@"\bJoystick1Buttons12\b"));
                range.SetStyle(StyleInput, new Regex(@"\bJoystick1Buttons13\b"));
                range.SetStyle(StyleInput, new Regex(@"\bJoystick1Buttons14\b"));
                range.SetStyle(StyleInput, new Regex(@"\bJoystick1Buttons15\b"));
                range.SetStyle(StyleInput, new Regex(@"\bJoystick1Buttons16\b"));
                range.SetStyle(StyleInput, new Regex(@"\bJoystick1Buttons17\b"));
                range.SetStyle(StyleInput, new Regex(@"\bJoystick1Buttons18\b"));
                range.SetStyle(StyleInput, new Regex(@"\bJoystick1Buttons19\b"));
                range.SetStyle(StyleInput, new Regex(@"\bJoystick1Buttons20\b"));
                range.SetStyle(StyleInput, new Regex(@"\bJoystick1Buttons21\b"));
                range.SetStyle(StyleInput, new Regex(@"\bJoystick1Buttons22\b"));
                range.SetStyle(StyleInput, new Regex(@"\bJoystick1Buttons23\b"));
                range.SetStyle(StyleInput, new Regex(@"\bJoystick1Buttons24\b"));
                range.SetStyle(StyleInput, new Regex(@"\bJoystick1Buttons25\b"));
                range.SetStyle(StyleInput, new Regex(@"\bJoystick1Buttons26\b"));
                range.SetStyle(StyleInput, new Regex(@"\bJoystick1Buttons27\b"));
                range.SetStyle(StyleInput, new Regex(@"\bJoystick1Buttons28\b"));
                range.SetStyle(StyleInput, new Regex(@"\bJoystick1Buttons29\b"));
                range.SetStyle(StyleInput, new Regex(@"\bJoystick1Buttons30\b"));
                range.SetStyle(StyleInput, new Regex(@"\bJoystick1Buttons31\b"));
                range.SetStyle(StyleInput, new Regex(@"\bJoystick1Buttons32\b"));
                range.SetStyle(StyleInput, new Regex(@"\bJoystick1Buttons33\b"));
                range.SetStyle(StyleInput, new Regex(@"\bJoystick1Buttons34\b"));
                range.SetStyle(StyleInput, new Regex(@"\bJoystick1Buttons35\b"));
                range.SetStyle(StyleInput, new Regex(@"\bJoystick1Buttons36\b"));
                range.SetStyle(StyleInput, new Regex(@"\bJoystick1Buttons37\b"));
                range.SetStyle(StyleInput, new Regex(@"\bJoystick1Buttons38\b"));
                range.SetStyle(StyleInput, new Regex(@"\bJoystick1Buttons39\b"));
                range.SetStyle(StyleInput, new Regex(@"\bJoystick1Buttons40\b"));
                range.SetStyle(StyleInput, new Regex(@"\bJoystick1Buttons41\b"));
                range.SetStyle(StyleInput, new Regex(@"\bJoystick1Buttons42\b"));
                range.SetStyle(StyleInput, new Regex(@"\bJoystick1Buttons43\b"));
                range.SetStyle(StyleInput, new Regex(@"\bJoystick1Buttons44\b"));
                range.SetStyle(StyleInput, new Regex(@"\bJoystick1Buttons45\b"));
                range.SetStyle(StyleInput, new Regex(@"\bJoystick1Buttons46\b"));
                range.SetStyle(StyleInput, new Regex(@"\bJoystick1Buttons47\b"));
                range.SetStyle(StyleInput, new Regex(@"\bJoystick1Buttons48\b"));
                range.SetStyle(StyleInput, new Regex(@"\bJoystick1Buttons49\b"));
                range.SetStyle(StyleInput, new Regex(@"\bJoystick1Buttons50\b"));
                range.SetStyle(StyleInput, new Regex(@"\bJoystick1Buttons51\b"));
                range.SetStyle(StyleInput, new Regex(@"\bJoystick1Buttons52\b"));
                range.SetStyle(StyleInput, new Regex(@"\bJoystick1Buttons53\b"));
                range.SetStyle(StyleInput, new Regex(@"\bJoystick1Buttons54\b"));
                range.SetStyle(StyleInput, new Regex(@"\bJoystick1Buttons55\b"));
                range.SetStyle(StyleInput, new Regex(@"\bJoystick1Buttons56\b"));
                range.SetStyle(StyleInput, new Regex(@"\bJoystick1Buttons57\b"));
                range.SetStyle(StyleInput, new Regex(@"\bJoystick1Buttons58\b"));
                range.SetStyle(StyleInput, new Regex(@"\bJoystick1Buttons59\b"));
                range.SetStyle(StyleInput, new Regex(@"\bJoystick1Buttons60\b"));
                range.SetStyle(StyleInput, new Regex(@"\bJoystick1Buttons61\b"));
                range.SetStyle(StyleInput, new Regex(@"\bJoystick1Buttons62\b"));
                range.SetStyle(StyleInput, new Regex(@"\bJoystick1Buttons63\b"));
                range.SetStyle(StyleInput, new Regex(@"\bJoystick1Buttons64\b"));
                range.SetStyle(StyleInput, new Regex(@"\bJoystick1Buttons65\b"));
                range.SetStyle(StyleInput, new Regex(@"\bJoystick1Buttons66\b"));
                range.SetStyle(StyleInput, new Regex(@"\bJoystick1Buttons67\b"));
                range.SetStyle(StyleInput, new Regex(@"\bJoystick1Buttons68\b"));
                range.SetStyle(StyleInput, new Regex(@"\bJoystick1Buttons69\b"));
                range.SetStyle(StyleInput, new Regex(@"\bJoystick1Buttons70\b"));
                range.SetStyle(StyleInput, new Regex(@"\bJoystick1Buttons71\b"));
                range.SetStyle(StyleInput, new Regex(@"\bJoystick1Buttons72\b"));
                range.SetStyle(StyleInput, new Regex(@"\bJoystick1Buttons73\b"));
                range.SetStyle(StyleInput, new Regex(@"\bJoystick1Buttons74\b"));
                range.SetStyle(StyleInput, new Regex(@"\bJoystick1Buttons75\b"));
                range.SetStyle(StyleInput, new Regex(@"\bJoystick1Buttons76\b"));
                range.SetStyle(StyleInput, new Regex(@"\bJoystick1Buttons77\b"));
                range.SetStyle(StyleInput, new Regex(@"\bJoystick1Buttons78\b"));
                range.SetStyle(StyleInput, new Regex(@"\bJoystick1Buttons79\b"));
                range.SetStyle(StyleInput, new Regex(@"\bJoystick1Buttons80\b"));
                range.SetStyle(StyleInput, new Regex(@"\bJoystick1Buttons81\b"));
                range.SetStyle(StyleInput, new Regex(@"\bJoystick1Buttons82\b"));
                range.SetStyle(StyleInput, new Regex(@"\bJoystick1Buttons83\b"));
                range.SetStyle(StyleInput, new Regex(@"\bJoystick1Buttons84\b"));
                range.SetStyle(StyleInput, new Regex(@"\bJoystick1Buttons85\b"));
                range.SetStyle(StyleInput, new Regex(@"\bJoystick1Buttons86\b"));
                range.SetStyle(StyleInput, new Regex(@"\bJoystick1Buttons87\b"));
                range.SetStyle(StyleInput, new Regex(@"\bJoystick1Buttons88\b"));
                range.SetStyle(StyleInput, new Regex(@"\bJoystick1Buttons89\b"));
                range.SetStyle(StyleInput, new Regex(@"\bJoystick1Buttons90\b"));
                range.SetStyle(StyleInput, new Regex(@"\bJoystick1Buttons91\b"));
                range.SetStyle(StyleInput, new Regex(@"\bJoystick1Buttons92\b"));
                range.SetStyle(StyleInput, new Regex(@"\bJoystick1Buttons93\b"));
                range.SetStyle(StyleInput, new Regex(@"\bJoystick1Buttons94\b"));
                range.SetStyle(StyleInput, new Regex(@"\bJoystick1Buttons95\b"));
                range.SetStyle(StyleInput, new Regex(@"\bJoystick1Buttons96\b"));
                range.SetStyle(StyleInput, new Regex(@"\bJoystick1Buttons97\b"));
                range.SetStyle(StyleInput, new Regex(@"\bJoystick1Buttons98\b"));
                range.SetStyle(StyleInput, new Regex(@"\bJoystick1Buttons99\b"));
                range.SetStyle(StyleInput, new Regex(@"\bJoystick1Buttons100\b"));
                range.SetStyle(StyleInput, new Regex(@"\bJoystick1Buttons101\b"));
                range.SetStyle(StyleInput, new Regex(@"\bJoystick1Buttons102\b"));
                range.SetStyle(StyleInput, new Regex(@"\bJoystick1Buttons103\b"));
                range.SetStyle(StyleInput, new Regex(@"\bJoystick1Buttons104\b"));
                range.SetStyle(StyleInput, new Regex(@"\bJoystick1Buttons105\b"));
                range.SetStyle(StyleInput, new Regex(@"\bJoystick1Buttons106\b"));
                range.SetStyle(StyleInput, new Regex(@"\bJoystick1Buttons107\b"));
                range.SetStyle(StyleInput, new Regex(@"\bJoystick1Buttons108\b"));
                range.SetStyle(StyleInput, new Regex(@"\bJoystick1Buttons109\b"));
                range.SetStyle(StyleInput, new Regex(@"\bJoystick1Buttons110\b"));
                range.SetStyle(StyleInput, new Regex(@"\bJoystick1Buttons111\b"));
                range.SetStyle(StyleInput, new Regex(@"\bJoystick1Buttons112\b"));
                range.SetStyle(StyleInput, new Regex(@"\bJoystick1Buttons113\b"));
                range.SetStyle(StyleInput, new Regex(@"\bJoystick1Buttons114\b"));
                range.SetStyle(StyleInput, new Regex(@"\bJoystick1Buttons115\b"));
                range.SetStyle(StyleInput, new Regex(@"\bJoystick1Buttons116\b"));
                range.SetStyle(StyleInput, new Regex(@"\bJoystick1Buttons117\b"));
                range.SetStyle(StyleInput, new Regex(@"\bJoystick1Buttons118\b"));
                range.SetStyle(StyleInput, new Regex(@"\bJoystick1Buttons119\b"));
                range.SetStyle(StyleInput, new Regex(@"\bJoystick1Buttons120\b"));
                range.SetStyle(StyleInput, new Regex(@"\bJoystick1Buttons121\b"));
                range.SetStyle(StyleInput, new Regex(@"\bJoystick1Buttons122\b"));
                range.SetStyle(StyleInput, new Regex(@"\bJoystick1Buttons123\b"));
                range.SetStyle(StyleInput, new Regex(@"\bJoystick1Buttons124\b"));
                range.SetStyle(StyleInput, new Regex(@"\bJoystick1Buttons125\b"));
                range.SetStyle(StyleInput, new Regex(@"\bJoystick1Buttons126\b"));
                range.SetStyle(StyleInput, new Regex(@"\bJoystick1Buttons127\b"));
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
                range.SetStyle(StyleInput, new Regex(@"\bMouse1Buttons0\b"));
                range.SetStyle(StyleInput, new Regex(@"\bMouse1Buttons1\b"));
                range.SetStyle(StyleInput, new Regex(@"\bMouse1Buttons2\b"));
                range.SetStyle(StyleInput, new Regex(@"\bMouse1Buttons3\b"));
                range.SetStyle(StyleInput, new Regex(@"\bMouse1Buttons4\b"));
                range.SetStyle(StyleInput, new Regex(@"\bMouse1Buttons5\b"));
                range.SetStyle(StyleInput, new Regex(@"\bMouse1Buttons6\b"));
                range.SetStyle(StyleInput, new Regex(@"\bMouse1Buttons7\b"));
                range.SetStyle(StyleInput, new Regex(@"\bMouse1AxisX\b"));
                range.SetStyle(StyleInput, new Regex(@"\bMouse1AxisY\b"));
                range.SetStyle(StyleInput, new Regex(@"\bMouse1AxisZ\b"));
                range.SetStyle(StyleInput, new Regex(@"\bKeyboard1KeyEscape\b"));
                range.SetStyle(StyleInput, new Regex(@"\bKeyboard1KeyD1\b"));
                range.SetStyle(StyleInput, new Regex(@"\bKeyboard1KeyD2\b"));
                range.SetStyle(StyleInput, new Regex(@"\bKeyboard1KeyD3\b"));
                range.SetStyle(StyleInput, new Regex(@"\bKeyboard1KeyD4\b"));
                range.SetStyle(StyleInput, new Regex(@"\bKeyboard1KeyD5\b"));
                range.SetStyle(StyleInput, new Regex(@"\bKeyboard1KeyD6\b"));
                range.SetStyle(StyleInput, new Regex(@"\bKeyboard1KeyD7\b"));
                range.SetStyle(StyleInput, new Regex(@"\bKeyboard1KeyD8\b"));
                range.SetStyle(StyleInput, new Regex(@"\bKeyboard1KeyD9\b"));
                range.SetStyle(StyleInput, new Regex(@"\bKeyboard1KeyD0\b"));
                range.SetStyle(StyleInput, new Regex(@"\bKeyboard1KeyMinus\b"));
                range.SetStyle(StyleInput, new Regex(@"\bKeyboard1KeyEquals\b"));
                range.SetStyle(StyleInput, new Regex(@"\bKeyboard1KeyBack\b"));
                range.SetStyle(StyleInput, new Regex(@"\bKeyboard1KeyTab\b"));
                range.SetStyle(StyleInput, new Regex(@"\bKeyboard1KeyQ\b"));
                range.SetStyle(StyleInput, new Regex(@"\bKeyboard1KeyW\b"));
                range.SetStyle(StyleInput, new Regex(@"\bKeyboard1KeyE\b"));
                range.SetStyle(StyleInput, new Regex(@"\bKeyboard1KeyR\b"));
                range.SetStyle(StyleInput, new Regex(@"\bKeyboard1KeyT\b"));
                range.SetStyle(StyleInput, new Regex(@"\bKeyboard1KeyY\b"));
                range.SetStyle(StyleInput, new Regex(@"\bKeyboard1KeyU\b"));
                range.SetStyle(StyleInput, new Regex(@"\bKeyboard1KeyI\b"));
                range.SetStyle(StyleInput, new Regex(@"\bKeyboard1KeyO\b"));
                range.SetStyle(StyleInput, new Regex(@"\bKeyboard1KeyP\b"));
                range.SetStyle(StyleInput, new Regex(@"\bKeyboard1KeyLeftBracket\b"));
                range.SetStyle(StyleInput, new Regex(@"\bKeyboard1KeyRightBracket\b"));
                range.SetStyle(StyleInput, new Regex(@"\bKeyboard1KeyReturn\b"));
                range.SetStyle(StyleInput, new Regex(@"\bKeyboard1KeyLeftControl\b"));
                range.SetStyle(StyleInput, new Regex(@"\bKeyboard1KeyA\b"));
                range.SetStyle(StyleInput, new Regex(@"\bKeyboard1KeyS\b"));
                range.SetStyle(StyleInput, new Regex(@"\bKeyboard1KeyD\b"));
                range.SetStyle(StyleInput, new Regex(@"\bKeyboard1KeyF\b"));
                range.SetStyle(StyleInput, new Regex(@"\bKeyboard1KeyG\b"));
                range.SetStyle(StyleInput, new Regex(@"\bKeyboard1KeyH\b"));
                range.SetStyle(StyleInput, new Regex(@"\bKeyboard1KeyJ\b"));
                range.SetStyle(StyleInput, new Regex(@"\bKeyboard1KeyK\b"));
                range.SetStyle(StyleInput, new Regex(@"\bKeyboard1KeyL\b"));
                range.SetStyle(StyleInput, new Regex(@"\bKeyboard1KeySemicolon\b"));
                range.SetStyle(StyleInput, new Regex(@"\bKeyboard1KeyApostrophe\b"));
                range.SetStyle(StyleInput, new Regex(@"\bKeyboard1KeyGrave\b"));
                range.SetStyle(StyleInput, new Regex(@"\bKeyboard1KeyLeftShift\b"));
                range.SetStyle(StyleInput, new Regex(@"\bKeyboard1KeyBackslash\b"));
                range.SetStyle(StyleInput, new Regex(@"\bKeyboard1KeyZ\b"));
                range.SetStyle(StyleInput, new Regex(@"\bKeyboard1KeyX\b"));
                range.SetStyle(StyleInput, new Regex(@"\bKeyboard1KeyC\b"));
                range.SetStyle(StyleInput, new Regex(@"\bKeyboard1KeyV\b"));
                range.SetStyle(StyleInput, new Regex(@"\bKeyboard1KeyB\b"));
                range.SetStyle(StyleInput, new Regex(@"\bKeyboard1KeyN\b"));
                range.SetStyle(StyleInput, new Regex(@"\bKeyboard1KeyM\b"));
                range.SetStyle(StyleInput, new Regex(@"\bKeyboard1KeyComma\b"));
                range.SetStyle(StyleInput, new Regex(@"\bKeyboard1KeyPeriod\b"));
                range.SetStyle(StyleInput, new Regex(@"\bKeyboard1KeySlash\b"));
                range.SetStyle(StyleInput, new Regex(@"\bKeyboard1KeyRightShift\b"));
                range.SetStyle(StyleInput, new Regex(@"\bKeyboard1KeyMultiply\b"));
                range.SetStyle(StyleInput, new Regex(@"\bKeyboard1KeyLeftAlt\b"));
                range.SetStyle(StyleInput, new Regex(@"\bKeyboard1KeySpace\b"));
                range.SetStyle(StyleInput, new Regex(@"\bKeyboard1KeyCapital\b"));
                range.SetStyle(StyleInput, new Regex(@"\bKeyboard1KeyF1\b"));
                range.SetStyle(StyleInput, new Regex(@"\bKeyboard1KeyF2\b"));
                range.SetStyle(StyleInput, new Regex(@"\bKeyboard1KeyF3\b"));
                range.SetStyle(StyleInput, new Regex(@"\bKeyboard1KeyF4\b"));
                range.SetStyle(StyleInput, new Regex(@"\bKeyboard1KeyF5\b"));
                range.SetStyle(StyleInput, new Regex(@"\bKeyboard1KeyF6\b"));
                range.SetStyle(StyleInput, new Regex(@"\bKeyboard1KeyF7\b"));
                range.SetStyle(StyleInput, new Regex(@"\bKeyboard1KeyF8\b"));
                range.SetStyle(StyleInput, new Regex(@"\bKeyboard1KeyF9\b"));
                range.SetStyle(StyleInput, new Regex(@"\bKeyboard1KeyF10\b"));
                range.SetStyle(StyleInput, new Regex(@"\bKeyboard1KeyNumberLock\b"));
                range.SetStyle(StyleInput, new Regex(@"\bKeyboard1KeyScrollLock\b"));
                range.SetStyle(StyleInput, new Regex(@"\bKeyboard1KeyNumberPad7\b"));
                range.SetStyle(StyleInput, new Regex(@"\bKeyboard1KeyNumberPad8\b"));
                range.SetStyle(StyleInput, new Regex(@"\bKeyboard1KeyNumberPad9\b"));
                range.SetStyle(StyleInput, new Regex(@"\bKeyboard1KeySubtract\b"));
                range.SetStyle(StyleInput, new Regex(@"\bKeyboard1KeyNumberPad4\b"));
                range.SetStyle(StyleInput, new Regex(@"\bKeyboard1KeyNumberPad5\b"));
                range.SetStyle(StyleInput, new Regex(@"\bKeyboard1KeyNumberPad6\b"));
                range.SetStyle(StyleInput, new Regex(@"\bKeyboard1KeyAdd\b"));
                range.SetStyle(StyleInput, new Regex(@"\bKeyboard1KeyNumberPad1\b"));
                range.SetStyle(StyleInput, new Regex(@"\bKeyboard1KeyNumberPad2\b"));
                range.SetStyle(StyleInput, new Regex(@"\bKeyboard1KeyNumberPad3\b"));
                range.SetStyle(StyleInput, new Regex(@"\bKeyboard1KeyNumberPad0\b"));
                range.SetStyle(StyleInput, new Regex(@"\bKeyboard1KeyDecimal\b"));
                range.SetStyle(StyleInput, new Regex(@"\bKeyboard1KeyOem102\b"));
                range.SetStyle(StyleInput, new Regex(@"\bKeyboard1KeyF11\b"));
                range.SetStyle(StyleInput, new Regex(@"\bKeyboard1KeyF12\b"));
                range.SetStyle(StyleInput, new Regex(@"\bKeyboard1KeyF13\b"));
                range.SetStyle(StyleInput, new Regex(@"\bKeyboard1KeyF14\b"));
                range.SetStyle(StyleInput, new Regex(@"\bKeyboard1KeyF15\b"));
                range.SetStyle(StyleInput, new Regex(@"\bKeyboard1KeyKana\b"));
                range.SetStyle(StyleInput, new Regex(@"\bKeyboard1KeyAbntC1\b"));
                range.SetStyle(StyleInput, new Regex(@"\bKeyboard1KeyConvert\b"));
                range.SetStyle(StyleInput, new Regex(@"\bKeyboard1KeyNoConvert\b"));
                range.SetStyle(StyleInput, new Regex(@"\bKeyboard1KeyYen\b"));
                range.SetStyle(StyleInput, new Regex(@"\bKeyboard1KeyAbntC2\b"));
                range.SetStyle(StyleInput, new Regex(@"\bKeyboard1KeyNumberPadEquals\b"));
                range.SetStyle(StyleInput, new Regex(@"\bKeyboard1KeyPreviousTrack\b"));
                range.SetStyle(StyleInput, new Regex(@"\bKeyboard1KeyAT\b"));
                range.SetStyle(StyleInput, new Regex(@"\bKeyboard1KeyColon\b"));
                range.SetStyle(StyleInput, new Regex(@"\bKeyboard1KeyUnderline\b"));
                range.SetStyle(StyleInput, new Regex(@"\bKeyboard1KeyKanji\b"));
                range.SetStyle(StyleInput, new Regex(@"\bKeyboard1KeyStop\b"));
                range.SetStyle(StyleInput, new Regex(@"\bKeyboard1KeyAX\b"));
                range.SetStyle(StyleInput, new Regex(@"\bKeyboard1KeyUnlabeled\b"));
                range.SetStyle(StyleInput, new Regex(@"\bKeyboard1KeyNextTrack\b"));
                range.SetStyle(StyleInput, new Regex(@"\bKeyboard1KeyNumberPadEnter\b"));
                range.SetStyle(StyleInput, new Regex(@"\bKeyboard1KeyRightControl\b"));
                range.SetStyle(StyleInput, new Regex(@"\bKeyboard1KeyMute\b"));
                range.SetStyle(StyleInput, new Regex(@"\bKeyboard1KeyCalculator\b"));
                range.SetStyle(StyleInput, new Regex(@"\bKeyboard1KeyPlayPause\b"));
                range.SetStyle(StyleInput, new Regex(@"\bKeyboard1KeyMediaStop\b"));
                range.SetStyle(StyleInput, new Regex(@"\bKeyboard1KeyVolumeDown\b"));
                range.SetStyle(StyleInput, new Regex(@"\bKeyboard1KeyVolumeUp\b"));
                range.SetStyle(StyleInput, new Regex(@"\bKeyboard1KeyWebHome\b"));
                range.SetStyle(StyleInput, new Regex(@"\bKeyboard1KeyNumberPadComma\b"));
                range.SetStyle(StyleInput, new Regex(@"\bKeyboard1KeyDivide\b"));
                range.SetStyle(StyleInput, new Regex(@"\bKeyboard1KeyPrintScreen\b"));
                range.SetStyle(StyleInput, new Regex(@"\bKeyboard1KeyRightAlt\b"));
                range.SetStyle(StyleInput, new Regex(@"\bKeyboard1KeyPause\b"));
                range.SetStyle(StyleInput, new Regex(@"\bKeyboard1KeyHome\b"));
                range.SetStyle(StyleInput, new Regex(@"\bKeyboard1KeyUp\b"));
                range.SetStyle(StyleInput, new Regex(@"\bKeyboard1KeyPageUp\b"));
                range.SetStyle(StyleInput, new Regex(@"\bKeyboard1KeyLeft\b"));
                range.SetStyle(StyleInput, new Regex(@"\bKeyboard1KeyRight\b"));
                range.SetStyle(StyleInput, new Regex(@"\bKeyboard1KeyEnd\b"));
                range.SetStyle(StyleInput, new Regex(@"\bKeyboard1KeyDown\b"));
                range.SetStyle(StyleInput, new Regex(@"\bKeyboard1KeyPageDown\b"));
                range.SetStyle(StyleInput, new Regex(@"\bKeyboard1KeyInsert\b"));
                range.SetStyle(StyleInput, new Regex(@"\bKeyboard1KeyDelete\b"));
                range.SetStyle(StyleInput, new Regex(@"\bKeyboard1KeyLeftWindowsKey\b"));
                range.SetStyle(StyleInput, new Regex(@"\bKeyboard1KeyRightWindowsKey\b"));
                range.SetStyle(StyleInput, new Regex(@"\bKeyboard1KeyApplications\b"));
                range.SetStyle(StyleInput, new Regex(@"\bKeyboard1KeyPower\b"));
                range.SetStyle(StyleInput, new Regex(@"\bKeyboard1KeySleep\b"));
                range.SetStyle(StyleInput, new Regex(@"\bKeyboard1KeyWake\b"));
                range.SetStyle(StyleInput, new Regex(@"\bKeyboard1KeyWebSearch\b"));
                range.SetStyle(StyleInput, new Regex(@"\bKeyboard1KeyWebFavorites\b"));
                range.SetStyle(StyleInput, new Regex(@"\bKeyboard1KeyWebRefresh\b"));
                range.SetStyle(StyleInput, new Regex(@"\bKeyboard1KeyWebStop\b"));
                range.SetStyle(StyleInput, new Regex(@"\bKeyboard1KeyWebForward\b"));
                range.SetStyle(StyleInput, new Regex(@"\bKeyboard1KeyWebBack\b"));
                range.SetStyle(StyleInput, new Regex(@"\bKeyboard1KeyMyComputer\b"));
                range.SetStyle(StyleInput, new Regex(@"\bKeyboard1KeyMail\b"));
                range.SetStyle(StyleInput, new Regex(@"\bKeyboard1KeyMediaSelect\b"));
                range.SetStyle(StyleInput, new Regex(@"\bKeyboard1KeyUnknown\b"));
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
                range.SetStyle(StyleInput, new Regex(@"\b,Controller2ButtonAPressed\b"));
                range.SetStyle(StyleInput, new Regex(@"\b,Controller2ButtonBPressed\b"));
                range.SetStyle(StyleInput, new Regex(@"\b,Controller2ButtonXPressed\b"));
                range.SetStyle(StyleInput, new Regex(@"\b,Controller2ButtonYPressed\b"));
                range.SetStyle(StyleInput, new Regex(@"\b,Controller2ButtonStartPressed\b"));
                range.SetStyle(StyleInput, new Regex(@"\b,Controller2ButtonBackPressed\b"));
                range.SetStyle(StyleInput, new Regex(@"\b,Controller2ButtonDownPressed\b"));
                range.SetStyle(StyleInput, new Regex(@"\b,Controller2ButtonUpPressed\b"));
                range.SetStyle(StyleInput, new Regex(@"\b,Controller2ButtonLeftPressed\b"));
                range.SetStyle(StyleInput, new Regex(@"\b,Controller2ButtonRightPressed\b"));
                range.SetStyle(StyleInput, new Regex(@"\b,Controller2ButtonShoulderLeftPressed\b"));
                range.SetStyle(StyleInput, new Regex(@"\b,Controller2ButtonShoulderRightPressed\b"));
                range.SetStyle(StyleInput, new Regex(@"\b,Controller2ThumbpadLeftPressed\b"));
                range.SetStyle(StyleInput, new Regex(@"\b,Controller2ThumbpadRightPressed\b"));
                range.SetStyle(StyleInput, new Regex(@"\b,Controller2TriggerLeftPosition\b"));
                range.SetStyle(StyleInput, new Regex(@"\b,Controller2TriggerRightPosition\b"));
                range.SetStyle(StyleInput, new Regex(@"\b,Controller2ThumbLeftX\b"));
                range.SetStyle(StyleInput, new Regex(@"\b,Controller2ThumbLeftY\b"));
                range.SetStyle(StyleInput, new Regex(@"\b,Controller2ThumbRightX\b"));
                range.SetStyle(StyleInput, new Regex(@"\b,Controller2ThumbRightY\b"));
                range.SetStyle(StyleOutput, new Regex(@"\b,Controller2_send_xbox\b"));
                range.SetStyle(StyleOutput, new Regex(@"\b,Controller2_send_back\b"));
                range.SetStyle(StyleOutput, new Regex(@"\b,Controller2_send_start\b"));
                range.SetStyle(StyleOutput, new Regex(@"\b,Controller2_send_A\b"));
                range.SetStyle(StyleOutput, new Regex(@"\b,Controller2_send_B\b"));
                range.SetStyle(StyleOutput, new Regex(@"\b,Controller2_send_X\b"));
                range.SetStyle(StyleOutput, new Regex(@"\b,Controller2_send_Y\b"));
                range.SetStyle(StyleOutput, new Regex(@"\b,Controller2_send_up\b"));
                range.SetStyle(StyleOutput, new Regex(@"\b,Controller2_send_left\b"));
                range.SetStyle(StyleOutput, new Regex(@"\b,Controller2_send_down\b"));
                range.SetStyle(StyleOutput, new Regex(@"\b,Controller2_send_right\b"));
                range.SetStyle(StyleOutput, new Regex(@"\b,Controller2_send_leftstick\b"));
                range.SetStyle(StyleOutput, new Regex(@"\b,Controller2_send_rightstick\b"));
                range.SetStyle(StyleOutput, new Regex(@"\b,Controller2_send_leftbumper\b"));
                range.SetStyle(StyleOutput, new Regex(@"\b,Controller2_send_rightbumper\b"));
                range.SetStyle(StyleOutput, new Regex(@"\b,Controller2_send_lefttriggerposition\b"));
                range.SetStyle(StyleOutput, new Regex(@"\b,Controller2_send_righttriggerposition\b"));
                range.SetStyle(StyleOutput, new Regex(@"\b,Controller2_send_leftstickx\b"));
                range.SetStyle(StyleOutput, new Regex(@"\b,Controller2_send_leftsticky\b"));
                range.SetStyle(StyleOutput, new Regex(@"\b,Controller2_send_rightstickx\b"));
                range.SetStyle(StyleOutput, new Regex(@"\b,Controller2_send_rightsticky\b"));
                range.SetStyle(StyleOutput, new Regex(@"\b,Controller2DS4_Send_Options\b"));
                range.SetStyle(StyleOutput, new Regex(@"\b,Controller2DS4_Send_Option\b"));
                range.SetStyle(StyleOutput, new Regex(@"\b,Controller2DS4_Send_ThumbLeft\b"));
                range.SetStyle(StyleOutput, new Regex(@"\b,Controller2DS4_Send_ThumbRight\b"));
                range.SetStyle(StyleOutput, new Regex(@"\b,Controller2DS4_Send_ShoulderLeft\b"));
                range.SetStyle(StyleOutput, new Regex(@"\b,Controller2DS4_Send_ShoulderRight\b"));
                range.SetStyle(StyleOutput, new Regex(@"\b,Controller2DS4_Send_Cross\b"));
                range.SetStyle(StyleOutput, new Regex(@"\b,Controller2DS4_Send_Circle\b"));
                range.SetStyle(StyleOutput, new Regex(@"\b,Controller2DS4_Send_Square\b"));
                range.SetStyle(StyleOutput, new Regex(@"\b,Controller2DS4_Send_Triangle\b"));
                range.SetStyle(StyleOutput, new Regex(@"\b,Controller2DS4_Send_Ps\b"));
                range.SetStyle(StyleOutput, new Regex(@"\b,Controller2DS4_Send_Touchpad\b"));
                range.SetStyle(StyleOutput, new Regex(@"\b,Controller2DS4_Send_Share\b"));
                range.SetStyle(StyleOutput, new Regex(@"\b,Controller2DS4_Send_DPadUp\b"));
                range.SetStyle(StyleOutput, new Regex(@"\b,Controller2DS4_Send_DPadDown\b"));
                range.SetStyle(StyleOutput, new Regex(@"\b,Controller2DS4_Send_DPadLeft\b"));
                range.SetStyle(StyleOutput, new Regex(@"\b,Controller2DS4_Send_DPadRight\b"));
                range.SetStyle(StyleOutput, new Regex(@"\b,Controller2DS4_Send_LeftTrigger\b"));
                range.SetStyle(StyleOutput, new Regex(@"\b,Controller2DS4_Send_RightTrigger\b"));
                range.SetStyle(StyleOutput, new Regex(@"\b,Controller2DS4_Send_LeftTriggerPosition\b"));
                range.SetStyle(StyleOutput, new Regex(@"\b,Controller2DS4_Send_RightTriggerPosition\b"));
                range.SetStyle(StyleOutput, new Regex(@"\b,Controller2DS4_Send_LeftThumbX\b"));
                range.SetStyle(StyleOutput, new Regex(@"\b,Controller2DS4_Send_RightThumbX\b"));
                range.SetStyle(StyleOutput, new Regex(@"\b,Controller2DS4_Send_LeftThumbY\b"));
                range.SetStyle(StyleOutput, new Regex(@"\b,Controller2DS4_Send_RightThumbY\b"));
                range.SetStyle(StyleOutput, new Regex(@"\b,Controller2VJoy_Send_1\b"));
                range.SetStyle(StyleOutput, new Regex(@"\b,Controller2VJoy_Send_2\b"));
                range.SetStyle(StyleOutput, new Regex(@"\b,Controller2VJoy_Send_3\b"));
                range.SetStyle(StyleOutput, new Regex(@"\b,Controller2VJoy_Send_4\b"));
                range.SetStyle(StyleOutput, new Regex(@"\b,Controller2VJoy_Send_5\b"));
                range.SetStyle(StyleOutput, new Regex(@"\b,Controller2VJoy_Send_6\b"));
                range.SetStyle(StyleOutput, new Regex(@"\b,Controller2VJoy_Send_7\b"));
                range.SetStyle(StyleOutput, new Regex(@"\b,Controller2VJoy_Send_8\b"));
                range.SetStyle(StyleOutput, new Regex(@"\b,Controller2VJoy_Send_X\b"));
                range.SetStyle(StyleOutput, new Regex(@"\b,Controller2VJoy_Send_Y\b"));
                range.SetStyle(StyleOutput, new Regex(@"\b,Controller2VJoy_Send_Z\b"));
                range.SetStyle(StyleOutput, new Regex(@"\b,Controller2VJoy_Send_WHL\b"));
                range.SetStyle(StyleOutput, new Regex(@"\b,Controller2VJoy_Send_SL0\b"));
                range.SetStyle(StyleOutput, new Regex(@"\b,Controller2VJoy_Send_SL1\b"));
                range.SetStyle(StyleOutput, new Regex(@"\b,Controller2VJoy_Send_RX\b"));
                range.SetStyle(StyleOutput, new Regex(@"\b,Controller2VJoy_Send_RY\b"));
                range.SetStyle(StyleOutput, new Regex(@"\b,Controller2VJoy_Send_RZ\b"));
                range.SetStyle(StyleOutput, new Regex(@"\b,Controller2VJoy_Send_POV\b"));
                range.SetStyle(StyleOutput, new Regex(@"\b,Controller2VJoy_Send_Hat\b"));
                range.SetStyle(StyleOutput, new Regex(@"\b,Controller2VJoy_Send_HatExt1\b"));
                range.SetStyle(StyleOutput, new Regex(@"\b,Controller2VJoy_Send_HatExt2\b"));
                range.SetStyle(StyleOutput, new Regex(@"\b,Controller2VJoy_Send_HatExt3\b"));
                range.SetStyle(StyleInput, new Regex(@"\bJoystick2AxisX\b"));
                range.SetStyle(StyleInput, new Regex(@"\bJoystick2AxisY\b"));
                range.SetStyle(StyleInput, new Regex(@"\bJoystick2AxisZ\b"));
                range.SetStyle(StyleInput, new Regex(@"\bJoystick2RotationX\b"));
                range.SetStyle(StyleInput, new Regex(@"\bJoystick2RotationY\b"));
                range.SetStyle(StyleInput, new Regex(@"\bJoystick2RotationZ\b"));
                range.SetStyle(StyleInput, new Regex(@"\bJoystick2Sliders0\b"));
                range.SetStyle(StyleInput, new Regex(@"\bJoystick2Sliders1\b"));
                range.SetStyle(StyleInput, new Regex(@"\bJoystick2PointOfViewControllers0\b"));
                range.SetStyle(StyleInput, new Regex(@"\bJoystick2PointOfViewControllers1\b"));
                range.SetStyle(StyleInput, new Regex(@"\bJoystick2PointOfViewControllers2\b"));
                range.SetStyle(StyleInput, new Regex(@"\bJoystick2PointOfViewControllers3\b"));
                range.SetStyle(StyleInput, new Regex(@"\bJoystick2VelocityX\b"));
                range.SetStyle(StyleInput, new Regex(@"\bJoystick2VelocityY\b"));
                range.SetStyle(StyleInput, new Regex(@"\bJoystick2VelocityZ\b"));
                range.SetStyle(StyleInput, new Regex(@"\bJoystick2AngularVelocityX\b"));
                range.SetStyle(StyleInput, new Regex(@"\bJoystick2AngularVelocityY\b"));
                range.SetStyle(StyleInput, new Regex(@"\bJoystick2AngularVelocityZ\b"));
                range.SetStyle(StyleInput, new Regex(@"\bJoystick2VelocitySliders0\b"));
                range.SetStyle(StyleInput, new Regex(@"\bJoystick2VelocitySliders1\b"));
                range.SetStyle(StyleInput, new Regex(@"\bJoystick2AccelerationX\b"));
                range.SetStyle(StyleInput, new Regex(@"\bJoystick2AccelerationY\b"));
                range.SetStyle(StyleInput, new Regex(@"\bJoystick2AccelerationZ\b"));
                range.SetStyle(StyleInput, new Regex(@"\bJoystick2AngularAccelerationX\b"));
                range.SetStyle(StyleInput, new Regex(@"\bJoystick2AngularAccelerationY\b"));
                range.SetStyle(StyleInput, new Regex(@"\bJoystick2AngularAccelerationZ\b"));
                range.SetStyle(StyleInput, new Regex(@"\bJoystick2AccelerationSliders0\b"));
                range.SetStyle(StyleInput, new Regex(@"\bJoystick2AccelerationSliders1\b"));
                range.SetStyle(StyleInput, new Regex(@"\bJoystick2ForceX\b"));
                range.SetStyle(StyleInput, new Regex(@"\bJoystick2ForceY\b"));
                range.SetStyle(StyleInput, new Regex(@"\bJoystick2ForceZ\b"));
                range.SetStyle(StyleInput, new Regex(@"\bJoystick2TorqueX\b"));
                range.SetStyle(StyleInput, new Regex(@"\bJoystick2TorqueY\b"));
                range.SetStyle(StyleInput, new Regex(@"\bJoystick2TorqueZ\b"));
                range.SetStyle(StyleInput, new Regex(@"\bJoystick2ForceSliders0\b"));
                range.SetStyle(StyleInput, new Regex(@"\bJoystick2ForceSliders1\b"));
                range.SetStyle(StyleInput, new Regex(@"\bJoystick2Buttons0\b"));
                range.SetStyle(StyleInput, new Regex(@"\bJoystick2Buttons1\b"));
                range.SetStyle(StyleInput, new Regex(@"\bJoystick2Buttons2\b"));
                range.SetStyle(StyleInput, new Regex(@"\bJoystick2Buttons3\b"));
                range.SetStyle(StyleInput, new Regex(@"\bJoystick2Buttons4\b"));
                range.SetStyle(StyleInput, new Regex(@"\bJoystick2Buttons5\b"));
                range.SetStyle(StyleInput, new Regex(@"\bJoystick2Buttons6\b"));
                range.SetStyle(StyleInput, new Regex(@"\bJoystick2Buttons7\b"));
                range.SetStyle(StyleInput, new Regex(@"\bJoystick2Buttons8\b"));
                range.SetStyle(StyleInput, new Regex(@"\bJoystick2Buttons9\b"));
                range.SetStyle(StyleInput, new Regex(@"\bJoystick2Buttons10\b"));
                range.SetStyle(StyleInput, new Regex(@"\bJoystick2Buttons11\b"));
                range.SetStyle(StyleInput, new Regex(@"\bJoystick2Buttons12\b"));
                range.SetStyle(StyleInput, new Regex(@"\bJoystick2Buttons13\b"));
                range.SetStyle(StyleInput, new Regex(@"\bJoystick2Buttons14\b"));
                range.SetStyle(StyleInput, new Regex(@"\bJoystick2Buttons15\b"));
                range.SetStyle(StyleInput, new Regex(@"\bJoystick2Buttons16\b"));
                range.SetStyle(StyleInput, new Regex(@"\bJoystick2Buttons17\b"));
                range.SetStyle(StyleInput, new Regex(@"\bJoystick2Buttons18\b"));
                range.SetStyle(StyleInput, new Regex(@"\bJoystick2Buttons19\b"));
                range.SetStyle(StyleInput, new Regex(@"\bJoystick2Buttons20\b"));
                range.SetStyle(StyleInput, new Regex(@"\bJoystick2Buttons21\b"));
                range.SetStyle(StyleInput, new Regex(@"\bJoystick2Buttons22\b"));
                range.SetStyle(StyleInput, new Regex(@"\bJoystick2Buttons23\b"));
                range.SetStyle(StyleInput, new Regex(@"\bJoystick2Buttons24\b"));
                range.SetStyle(StyleInput, new Regex(@"\bJoystick2Buttons25\b"));
                range.SetStyle(StyleInput, new Regex(@"\bJoystick2Buttons26\b"));
                range.SetStyle(StyleInput, new Regex(@"\bJoystick2Buttons27\b"));
                range.SetStyle(StyleInput, new Regex(@"\bJoystick2Buttons28\b"));
                range.SetStyle(StyleInput, new Regex(@"\bJoystick2Buttons29\b"));
                range.SetStyle(StyleInput, new Regex(@"\bJoystick2Buttons30\b"));
                range.SetStyle(StyleInput, new Regex(@"\bJoystick2Buttons31\b"));
                range.SetStyle(StyleInput, new Regex(@"\bJoystick2Buttons32\b"));
                range.SetStyle(StyleInput, new Regex(@"\bJoystick2Buttons33\b"));
                range.SetStyle(StyleInput, new Regex(@"\bJoystick2Buttons34\b"));
                range.SetStyle(StyleInput, new Regex(@"\bJoystick2Buttons35\b"));
                range.SetStyle(StyleInput, new Regex(@"\bJoystick2Buttons36\b"));
                range.SetStyle(StyleInput, new Regex(@"\bJoystick2Buttons37\b"));
                range.SetStyle(StyleInput, new Regex(@"\bJoystick2Buttons38\b"));
                range.SetStyle(StyleInput, new Regex(@"\bJoystick2Buttons39\b"));
                range.SetStyle(StyleInput, new Regex(@"\bJoystick2Buttons40\b"));
                range.SetStyle(StyleInput, new Regex(@"\bJoystick2Buttons41\b"));
                range.SetStyle(StyleInput, new Regex(@"\bJoystick2Buttons42\b"));
                range.SetStyle(StyleInput, new Regex(@"\bJoystick2Buttons43\b"));
                range.SetStyle(StyleInput, new Regex(@"\bJoystick2Buttons44\b"));
                range.SetStyle(StyleInput, new Regex(@"\bJoystick2Buttons45\b"));
                range.SetStyle(StyleInput, new Regex(@"\bJoystick2Buttons46\b"));
                range.SetStyle(StyleInput, new Regex(@"\bJoystick2Buttons47\b"));
                range.SetStyle(StyleInput, new Regex(@"\bJoystick2Buttons48\b"));
                range.SetStyle(StyleInput, new Regex(@"\bJoystick2Buttons49\b"));
                range.SetStyle(StyleInput, new Regex(@"\bJoystick2Buttons50\b"));
                range.SetStyle(StyleInput, new Regex(@"\bJoystick2Buttons51\b"));
                range.SetStyle(StyleInput, new Regex(@"\bJoystick2Buttons52\b"));
                range.SetStyle(StyleInput, new Regex(@"\bJoystick2Buttons53\b"));
                range.SetStyle(StyleInput, new Regex(@"\bJoystick2Buttons54\b"));
                range.SetStyle(StyleInput, new Regex(@"\bJoystick2Buttons55\b"));
                range.SetStyle(StyleInput, new Regex(@"\bJoystick2Buttons56\b"));
                range.SetStyle(StyleInput, new Regex(@"\bJoystick2Buttons57\b"));
                range.SetStyle(StyleInput, new Regex(@"\bJoystick2Buttons58\b"));
                range.SetStyle(StyleInput, new Regex(@"\bJoystick2Buttons59\b"));
                range.SetStyle(StyleInput, new Regex(@"\bJoystick2Buttons60\b"));
                range.SetStyle(StyleInput, new Regex(@"\bJoystick2Buttons61\b"));
                range.SetStyle(StyleInput, new Regex(@"\bJoystick2Buttons62\b"));
                range.SetStyle(StyleInput, new Regex(@"\bJoystick2Buttons63\b"));
                range.SetStyle(StyleInput, new Regex(@"\bJoystick2Buttons64\b"));
                range.SetStyle(StyleInput, new Regex(@"\bJoystick2Buttons65\b"));
                range.SetStyle(StyleInput, new Regex(@"\bJoystick2Buttons66\b"));
                range.SetStyle(StyleInput, new Regex(@"\bJoystick2Buttons67\b"));
                range.SetStyle(StyleInput, new Regex(@"\bJoystick2Buttons68\b"));
                range.SetStyle(StyleInput, new Regex(@"\bJoystick2Buttons69\b"));
                range.SetStyle(StyleInput, new Regex(@"\bJoystick2Buttons70\b"));
                range.SetStyle(StyleInput, new Regex(@"\bJoystick2Buttons71\b"));
                range.SetStyle(StyleInput, new Regex(@"\bJoystick2Buttons72\b"));
                range.SetStyle(StyleInput, new Regex(@"\bJoystick2Buttons73\b"));
                range.SetStyle(StyleInput, new Regex(@"\bJoystick2Buttons74\b"));
                range.SetStyle(StyleInput, new Regex(@"\bJoystick2Buttons75\b"));
                range.SetStyle(StyleInput, new Regex(@"\bJoystick2Buttons76\b"));
                range.SetStyle(StyleInput, new Regex(@"\bJoystick2Buttons77\b"));
                range.SetStyle(StyleInput, new Regex(@"\bJoystick2Buttons78\b"));
                range.SetStyle(StyleInput, new Regex(@"\bJoystick2Buttons79\b"));
                range.SetStyle(StyleInput, new Regex(@"\bJoystick2Buttons80\b"));
                range.SetStyle(StyleInput, new Regex(@"\bJoystick2Buttons81\b"));
                range.SetStyle(StyleInput, new Regex(@"\bJoystick2Buttons82\b"));
                range.SetStyle(StyleInput, new Regex(@"\bJoystick2Buttons83\b"));
                range.SetStyle(StyleInput, new Regex(@"\bJoystick2Buttons84\b"));
                range.SetStyle(StyleInput, new Regex(@"\bJoystick2Buttons85\b"));
                range.SetStyle(StyleInput, new Regex(@"\bJoystick2Buttons86\b"));
                range.SetStyle(StyleInput, new Regex(@"\bJoystick2Buttons87\b"));
                range.SetStyle(StyleInput, new Regex(@"\bJoystick2Buttons88\b"));
                range.SetStyle(StyleInput, new Regex(@"\bJoystick2Buttons89\b"));
                range.SetStyle(StyleInput, new Regex(@"\bJoystick2Buttons90\b"));
                range.SetStyle(StyleInput, new Regex(@"\bJoystick2Buttons91\b"));
                range.SetStyle(StyleInput, new Regex(@"\bJoystick2Buttons92\b"));
                range.SetStyle(StyleInput, new Regex(@"\bJoystick2Buttons93\b"));
                range.SetStyle(StyleInput, new Regex(@"\bJoystick2Buttons94\b"));
                range.SetStyle(StyleInput, new Regex(@"\bJoystick2Buttons95\b"));
                range.SetStyle(StyleInput, new Regex(@"\bJoystick2Buttons96\b"));
                range.SetStyle(StyleInput, new Regex(@"\bJoystick2Buttons97\b"));
                range.SetStyle(StyleInput, new Regex(@"\bJoystick2Buttons98\b"));
                range.SetStyle(StyleInput, new Regex(@"\bJoystick2Buttons99\b"));
                range.SetStyle(StyleInput, new Regex(@"\bJoystick2Buttons100\b"));
                range.SetStyle(StyleInput, new Regex(@"\bJoystick2Buttons101\b"));
                range.SetStyle(StyleInput, new Regex(@"\bJoystick2Buttons102\b"));
                range.SetStyle(StyleInput, new Regex(@"\bJoystick2Buttons103\b"));
                range.SetStyle(StyleInput, new Regex(@"\bJoystick2Buttons104\b"));
                range.SetStyle(StyleInput, new Regex(@"\bJoystick2Buttons105\b"));
                range.SetStyle(StyleInput, new Regex(@"\bJoystick2Buttons106\b"));
                range.SetStyle(StyleInput, new Regex(@"\bJoystick2Buttons107\b"));
                range.SetStyle(StyleInput, new Regex(@"\bJoystick2Buttons108\b"));
                range.SetStyle(StyleInput, new Regex(@"\bJoystick2Buttons109\b"));
                range.SetStyle(StyleInput, new Regex(@"\bJoystick2Buttons110\b"));
                range.SetStyle(StyleInput, new Regex(@"\bJoystick2Buttons111\b"));
                range.SetStyle(StyleInput, new Regex(@"\bJoystick2Buttons112\b"));
                range.SetStyle(StyleInput, new Regex(@"\bJoystick2Buttons113\b"));
                range.SetStyle(StyleInput, new Regex(@"\bJoystick2Buttons114\b"));
                range.SetStyle(StyleInput, new Regex(@"\bJoystick2Buttons115\b"));
                range.SetStyle(StyleInput, new Regex(@"\bJoystick2Buttons116\b"));
                range.SetStyle(StyleInput, new Regex(@"\bJoystick2Buttons117\b"));
                range.SetStyle(StyleInput, new Regex(@"\bJoystick2Buttons118\b"));
                range.SetStyle(StyleInput, new Regex(@"\bJoystick2Buttons119\b"));
                range.SetStyle(StyleInput, new Regex(@"\bJoystick2Buttons120\b"));
                range.SetStyle(StyleInput, new Regex(@"\bJoystick2Buttons121\b"));
                range.SetStyle(StyleInput, new Regex(@"\bJoystick2Buttons122\b"));
                range.SetStyle(StyleInput, new Regex(@"\bJoystick2Buttons123\b"));
                range.SetStyle(StyleInput, new Regex(@"\bJoystick2Buttons124\b"));
                range.SetStyle(StyleInput, new Regex(@"\bJoystick2Buttons125\b"));
                range.SetStyle(StyleInput, new Regex(@"\bJoystick2Buttons126\b"));
                range.SetStyle(StyleInput, new Regex(@"\bJoystick2Buttons127\b"));
                range.SetStyle(StyleInput, new Regex(@"\bMouse2Buttons0\b"));
                range.SetStyle(StyleInput, new Regex(@"\bMouse2Buttons1\b"));
                range.SetStyle(StyleInput, new Regex(@"\bMouse2Buttons2\b"));
                range.SetStyle(StyleInput, new Regex(@"\bMouse2Buttons3\b"));
                range.SetStyle(StyleInput, new Regex(@"\bMouse2Buttons4\b"));
                range.SetStyle(StyleInput, new Regex(@"\bMouse2Buttons5\b"));
                range.SetStyle(StyleInput, new Regex(@"\bMouse2Buttons6\b"));
                range.SetStyle(StyleInput, new Regex(@"\bMouse2Buttons7\b"));
                range.SetStyle(StyleInput, new Regex(@"\bMouse2AxisX\b"));
                range.SetStyle(StyleInput, new Regex(@"\bMouse2AxisY\b"));
                range.SetStyle(StyleInput, new Regex(@"\bMouse2AxisZ\b"));
                range.SetStyle(StyleInput, new Regex(@"\bKeyboard2KeyEscape\b"));
                range.SetStyle(StyleInput, new Regex(@"\bKeyboard2KeyD1\b"));
                range.SetStyle(StyleInput, new Regex(@"\bKeyboard2KeyD2\b"));
                range.SetStyle(StyleInput, new Regex(@"\bKeyboard2KeyD3\b"));
                range.SetStyle(StyleInput, new Regex(@"\bKeyboard2KeyD4\b"));
                range.SetStyle(StyleInput, new Regex(@"\bKeyboard2KeyD5\b"));
                range.SetStyle(StyleInput, new Regex(@"\bKeyboard2KeyD6\b"));
                range.SetStyle(StyleInput, new Regex(@"\bKeyboard2KeyD7\b"));
                range.SetStyle(StyleInput, new Regex(@"\bKeyboard2KeyD8\b"));
                range.SetStyle(StyleInput, new Regex(@"\bKeyboard2KeyD9\b"));
                range.SetStyle(StyleInput, new Regex(@"\bKeyboard2KeyD0\b"));
                range.SetStyle(StyleInput, new Regex(@"\bKeyboard2KeyMinus\b"));
                range.SetStyle(StyleInput, new Regex(@"\bKeyboard2KeyEquals\b"));
                range.SetStyle(StyleInput, new Regex(@"\bKeyboard2KeyBack\b"));
                range.SetStyle(StyleInput, new Regex(@"\bKeyboard2KeyTab\b"));
                range.SetStyle(StyleInput, new Regex(@"\bKeyboard2KeyQ\b"));
                range.SetStyle(StyleInput, new Regex(@"\bKeyboard2KeyW\b"));
                range.SetStyle(StyleInput, new Regex(@"\bKeyboard2KeyE\b"));
                range.SetStyle(StyleInput, new Regex(@"\bKeyboard2KeyR\b"));
                range.SetStyle(StyleInput, new Regex(@"\bKeyboard2KeyT\b"));
                range.SetStyle(StyleInput, new Regex(@"\bKeyboard2KeyY\b"));
                range.SetStyle(StyleInput, new Regex(@"\bKeyboard2KeyU\b"));
                range.SetStyle(StyleInput, new Regex(@"\bKeyboard2KeyI\b"));
                range.SetStyle(StyleInput, new Regex(@"\bKeyboard2KeyO\b"));
                range.SetStyle(StyleInput, new Regex(@"\bKeyboard2KeyP\b"));
                range.SetStyle(StyleInput, new Regex(@"\bKeyboard2KeyLeftBracket\b"));
                range.SetStyle(StyleInput, new Regex(@"\bKeyboard2KeyRightBracket\b"));
                range.SetStyle(StyleInput, new Regex(@"\bKeyboard2KeyReturn\b"));
                range.SetStyle(StyleInput, new Regex(@"\bKeyboard2KeyLeftControl\b"));
                range.SetStyle(StyleInput, new Regex(@"\bKeyboard2KeyA\b"));
                range.SetStyle(StyleInput, new Regex(@"\bKeyboard2KeyS\b"));
                range.SetStyle(StyleInput, new Regex(@"\bKeyboard2KeyD\b"));
                range.SetStyle(StyleInput, new Regex(@"\bKeyboard2KeyF\b"));
                range.SetStyle(StyleInput, new Regex(@"\bKeyboard2KeyG\b"));
                range.SetStyle(StyleInput, new Regex(@"\bKeyboard2KeyH\b"));
                range.SetStyle(StyleInput, new Regex(@"\bKeyboard2KeyJ\b"));
                range.SetStyle(StyleInput, new Regex(@"\bKeyboard2KeyK\b"));
                range.SetStyle(StyleInput, new Regex(@"\bKeyboard2KeyL\b"));
                range.SetStyle(StyleInput, new Regex(@"\bKeyboard2KeySemicolon\b"));
                range.SetStyle(StyleInput, new Regex(@"\bKeyboard2KeyApostrophe\b"));
                range.SetStyle(StyleInput, new Regex(@"\bKeyboard2KeyGrave\b"));
                range.SetStyle(StyleInput, new Regex(@"\bKeyboard2KeyLeftShift\b"));
                range.SetStyle(StyleInput, new Regex(@"\bKeyboard2KeyBackslash\b"));
                range.SetStyle(StyleInput, new Regex(@"\bKeyboard2KeyZ\b"));
                range.SetStyle(StyleInput, new Regex(@"\bKeyboard2KeyX\b"));
                range.SetStyle(StyleInput, new Regex(@"\bKeyboard2KeyC\b"));
                range.SetStyle(StyleInput, new Regex(@"\bKeyboard2KeyV\b"));
                range.SetStyle(StyleInput, new Regex(@"\bKeyboard2KeyB\b"));
                range.SetStyle(StyleInput, new Regex(@"\bKeyboard2KeyN\b"));
                range.SetStyle(StyleInput, new Regex(@"\bKeyboard2KeyM\b"));
                range.SetStyle(StyleInput, new Regex(@"\bKeyboard2KeyComma\b"));
                range.SetStyle(StyleInput, new Regex(@"\bKeyboard2KeyPeriod\b"));
                range.SetStyle(StyleInput, new Regex(@"\bKeyboard2KeySlash\b"));
                range.SetStyle(StyleInput, new Regex(@"\bKeyboard2KeyRightShift\b"));
                range.SetStyle(StyleInput, new Regex(@"\bKeyboard2KeyMultiply\b"));
                range.SetStyle(StyleInput, new Regex(@"\bKeyboard2KeyLeftAlt\b"));
                range.SetStyle(StyleInput, new Regex(@"\bKeyboard2KeySpace\b"));
                range.SetStyle(StyleInput, new Regex(@"\bKeyboard2KeyCapital\b"));
                range.SetStyle(StyleInput, new Regex(@"\bKeyboard2KeyF1\b"));
                range.SetStyle(StyleInput, new Regex(@"\bKeyboard2KeyF2\b"));
                range.SetStyle(StyleInput, new Regex(@"\bKeyboard2KeyF3\b"));
                range.SetStyle(StyleInput, new Regex(@"\bKeyboard2KeyF4\b"));
                range.SetStyle(StyleInput, new Regex(@"\bKeyboard2KeyF5\b"));
                range.SetStyle(StyleInput, new Regex(@"\bKeyboard2KeyF6\b"));
                range.SetStyle(StyleInput, new Regex(@"\bKeyboard2KeyF7\b"));
                range.SetStyle(StyleInput, new Regex(@"\bKeyboard2KeyF8\b"));
                range.SetStyle(StyleInput, new Regex(@"\bKeyboard2KeyF9\b"));
                range.SetStyle(StyleInput, new Regex(@"\bKeyboard2KeyF10\b"));
                range.SetStyle(StyleInput, new Regex(@"\bKeyboard2KeyNumberLock\b"));
                range.SetStyle(StyleInput, new Regex(@"\bKeyboard2KeyScrollLock\b"));
                range.SetStyle(StyleInput, new Regex(@"\bKeyboard2KeyNumberPad7\b"));
                range.SetStyle(StyleInput, new Regex(@"\bKeyboard2KeyNumberPad8\b"));
                range.SetStyle(StyleInput, new Regex(@"\bKeyboard2KeyNumberPad9\b"));
                range.SetStyle(StyleInput, new Regex(@"\bKeyboard2KeySubtract\b"));
                range.SetStyle(StyleInput, new Regex(@"\bKeyboard2KeyNumberPad4\b"));
                range.SetStyle(StyleInput, new Regex(@"\bKeyboard2KeyNumberPad5\b"));
                range.SetStyle(StyleInput, new Regex(@"\bKeyboard2KeyNumberPad6\b"));
                range.SetStyle(StyleInput, new Regex(@"\bKeyboard2KeyAdd\b"));
                range.SetStyle(StyleInput, new Regex(@"\bKeyboard2KeyNumberPad1\b"));
                range.SetStyle(StyleInput, new Regex(@"\bKeyboard2KeyNumberPad2\b"));
                range.SetStyle(StyleInput, new Regex(@"\bKeyboard2KeyNumberPad3\b"));
                range.SetStyle(StyleInput, new Regex(@"\bKeyboard2KeyNumberPad0\b"));
                range.SetStyle(StyleInput, new Regex(@"\bKeyboard2KeyDecimal\b"));
                range.SetStyle(StyleInput, new Regex(@"\bKeyboard2KeyOem102\b"));
                range.SetStyle(StyleInput, new Regex(@"\bKeyboard2KeyF11\b"));
                range.SetStyle(StyleInput, new Regex(@"\bKeyboard2KeyF12\b"));
                range.SetStyle(StyleInput, new Regex(@"\bKeyboard2KeyF13\b"));
                range.SetStyle(StyleInput, new Regex(@"\bKeyboard2KeyF14\b"));
                range.SetStyle(StyleInput, new Regex(@"\bKeyboard2KeyF15\b"));
                range.SetStyle(StyleInput, new Regex(@"\bKeyboard2KeyKana\b"));
                range.SetStyle(StyleInput, new Regex(@"\bKeyboard2KeyAbntC1\b"));
                range.SetStyle(StyleInput, new Regex(@"\bKeyboard2KeyConvert\b"));
                range.SetStyle(StyleInput, new Regex(@"\bKeyboard2KeyNoConvert\b"));
                range.SetStyle(StyleInput, new Regex(@"\bKeyboard2KeyYen\b"));
                range.SetStyle(StyleInput, new Regex(@"\bKeyboard2KeyAbntC2\b"));
                range.SetStyle(StyleInput, new Regex(@"\bKeyboard2KeyNumberPadEquals\b"));
                range.SetStyle(StyleInput, new Regex(@"\bKeyboard2KeyPreviousTrack\b"));
                range.SetStyle(StyleInput, new Regex(@"\bKeyboard2KeyAT\b"));
                range.SetStyle(StyleInput, new Regex(@"\bKeyboard2KeyColon\b"));
                range.SetStyle(StyleInput, new Regex(@"\bKeyboard2KeyUnderline\b"));
                range.SetStyle(StyleInput, new Regex(@"\bKeyboard2KeyKanji\b"));
                range.SetStyle(StyleInput, new Regex(@"\bKeyboard2KeyStop\b"));
                range.SetStyle(StyleInput, new Regex(@"\bKeyboard2KeyAX\b"));
                range.SetStyle(StyleInput, new Regex(@"\bKeyboard2KeyUnlabeled\b"));
                range.SetStyle(StyleInput, new Regex(@"\bKeyboard2KeyNextTrack\b"));
                range.SetStyle(StyleInput, new Regex(@"\bKeyboard2KeyNumberPadEnter\b"));
                range.SetStyle(StyleInput, new Regex(@"\bKeyboard2KeyRightControl\b"));
                range.SetStyle(StyleInput, new Regex(@"\bKeyboard2KeyMute\b"));
                range.SetStyle(StyleInput, new Regex(@"\bKeyboard2KeyCalculator\b"));
                range.SetStyle(StyleInput, new Regex(@"\bKeyboard2KeyPlayPause\b"));
                range.SetStyle(StyleInput, new Regex(@"\bKeyboard2KeyMediaStop\b"));
                range.SetStyle(StyleInput, new Regex(@"\bKeyboard2KeyVolumeDown\b"));
                range.SetStyle(StyleInput, new Regex(@"\bKeyboard2KeyVolumeUp\b"));
                range.SetStyle(StyleInput, new Regex(@"\bKeyboard2KeyWebHome\b"));
                range.SetStyle(StyleInput, new Regex(@"\bKeyboard2KeyNumberPadComma\b"));
                range.SetStyle(StyleInput, new Regex(@"\bKeyboard2KeyDivide\b"));
                range.SetStyle(StyleInput, new Regex(@"\bKeyboard2KeyPrintScreen\b"));
                range.SetStyle(StyleInput, new Regex(@"\bKeyboard2KeyRightAlt\b"));
                range.SetStyle(StyleInput, new Regex(@"\bKeyboard2KeyPause\b"));
                range.SetStyle(StyleInput, new Regex(@"\bKeyboard2KeyHome\b"));
                range.SetStyle(StyleInput, new Regex(@"\bKeyboard2KeyUp\b"));
                range.SetStyle(StyleInput, new Regex(@"\bKeyboard2KeyPageUp\b"));
                range.SetStyle(StyleInput, new Regex(@"\bKeyboard2KeyLeft\b"));
                range.SetStyle(StyleInput, new Regex(@"\bKeyboard2KeyRight\b"));
                range.SetStyle(StyleInput, new Regex(@"\bKeyboard2KeyEnd\b"));
                range.SetStyle(StyleInput, new Regex(@"\bKeyboard2KeyDown\b"));
                range.SetStyle(StyleInput, new Regex(@"\bKeyboard2KeyPageDown\b"));
                range.SetStyle(StyleInput, new Regex(@"\bKeyboard2KeyInsert\b"));
                range.SetStyle(StyleInput, new Regex(@"\bKeyboard2KeyDelete\b"));
                range.SetStyle(StyleInput, new Regex(@"\bKeyboard2KeyLeftWindowsKey\b"));
                range.SetStyle(StyleInput, new Regex(@"\bKeyboard2KeyRightWindowsKey\b"));
                range.SetStyle(StyleInput, new Regex(@"\bKeyboard2KeyApplications\b"));
                range.SetStyle(StyleInput, new Regex(@"\bKeyboard2KeyPower\b"));
                range.SetStyle(StyleInput, new Regex(@"\bKeyboard2KeySleep\b"));
                range.SetStyle(StyleInput, new Regex(@"\bKeyboard2KeyWake\b"));
                range.SetStyle(StyleInput, new Regex(@"\bKeyboard2KeyWebSearch\b"));
                range.SetStyle(StyleInput, new Regex(@"\bKeyboard2KeyWebFavorites\b"));
                range.SetStyle(StyleInput, new Regex(@"\bKeyboard2KeyWebRefresh\b"));
                range.SetStyle(StyleInput, new Regex(@"\bKeyboard2KeyWebStop\b"));
                range.SetStyle(StyleInput, new Regex(@"\bKeyboard2KeyWebForward\b"));
                range.SetStyle(StyleInput, new Regex(@"\bKeyboard2KeyWebBack\b"));
                range.SetStyle(StyleInput, new Regex(@"\bKeyboard2KeyMyComputer\b"));
                range.SetStyle(StyleInput, new Regex(@"\bKeyboard2KeyMail\b"));
                range.SetStyle(StyleInput, new Regex(@"\bKeyboard2KeyMediaSelect\b"));
                range.SetStyle(StyleInput, new Regex(@"\bKeyboard2KeyUnknown\b"));
                range.SetStyle(StyleInput, new Regex(@"\bPS5Controller1LeftStickX\b"));
                range.SetStyle(StyleInput, new Regex(@"\bPS5Controller1LeftStickY\b"));
                range.SetStyle(StyleInput, new Regex(@"\bPS5Controller1RightStickX\b"));
                range.SetStyle(StyleInput, new Regex(@"\bPS5Controller1RightStickY\b"));
                range.SetStyle(StyleInput, new Regex(@"\bPS5Controller1LeftTriggerPosition\b"));
                range.SetStyle(StyleInput, new Regex(@"\bPS5Controller1RightTriggerPosition\b"));
                range.SetStyle(StyleInput, new Regex(@"\bPS5Controller1TouchX\b"));
                range.SetStyle(StyleInput, new Regex(@"\bPS5Controller1TouchY\b"));
                range.SetStyle(StyleInput, new Regex(@"\bPS5Controller1TouchOn\b"));
                range.SetStyle(StyleInput, new Regex(@"\bPS5Controller1GyroX\b"));
                range.SetStyle(StyleInput, new Regex(@"\bPS5Controller1GyroY\b"));
                range.SetStyle(StyleInput, new Regex(@"\bPS5Controller1AccelX\b"));
                range.SetStyle(StyleInput, new Regex(@"\bPS5Controller1AccelY\b"));
                range.SetStyle(StyleInput, new Regex(@"\bPS5Controller1ButtonCrossPressed\b"));
                range.SetStyle(StyleInput, new Regex(@"\bPS5Controller1ButtonCirclePressed\b"));
                range.SetStyle(StyleInput, new Regex(@"\bPS5Controller1ButtonSquarePressed\b"));
                range.SetStyle(StyleInput, new Regex(@"\bPS5Controller1ButtonTrianglePressed\b"));
                range.SetStyle(StyleInput, new Regex(@"\bPS5Controller1ButtonDPadUpPressed\b"));
                range.SetStyle(StyleInput, new Regex(@"\bPS5Controller1ButtonDPadRightPressed\b"));
                range.SetStyle(StyleInput, new Regex(@"\bPS5Controller1ButtonDPadDownPressed\b"));
                range.SetStyle(StyleInput, new Regex(@"\bPS5Controller1ButtonDPadLeftPressed\b"));
                range.SetStyle(StyleInput, new Regex(@"\bPS5Controller1ButtonL1Pressed\b"));
                range.SetStyle(StyleInput, new Regex(@"\bPS5Controller1ButtonR1Pressed\b"));
                range.SetStyle(StyleInput, new Regex(@"\bPS5Controller1ButtonL2Pressed\b"));
                range.SetStyle(StyleInput, new Regex(@"\bPS5Controller1ButtonR2Pressed\b"));
                range.SetStyle(StyleInput, new Regex(@"\bPS5Controller1ButtonL3Pressed\b"));
                range.SetStyle(StyleInput, new Regex(@"\bPS5Controller1ButtonR3Pressed\b"));
                range.SetStyle(StyleInput, new Regex(@"\bPS5Controller1ButtonCreatePressed\b"));
                range.SetStyle(StyleInput, new Regex(@"\bPS5Controller1ButtonMenuPressed\b"));
                range.SetStyle(StyleInput, new Regex(@"\bPS5Controller1ButtonLogoPressed\b"));
                range.SetStyle(StyleInput, new Regex(@"\bPS5Controller1ButtonTouchpadPressed\b"));
                range.SetStyle(StyleInput, new Regex(@"\bPS5Controller1ButtonFnLPressed\b"));
                range.SetStyle(StyleInput, new Regex(@"\bPS5Controller1ButtonFnRPressed\b"));
                range.SetStyle(StyleInput, new Regex(@"\bPS5Controller1ButtonBLPPressed\b"));
                range.SetStyle(StyleInput, new Regex(@"\bPS5Controller1ButtonBRPPressed\b"));
                range.SetStyle(StyleInput, new Regex(@"\bPS5Controller1ButtonMicPressed\b"));
                range.SetStyle(StyleInput, new Regex(@"\bPS4Controller1LeftStickX\b"));
                range.SetStyle(StyleInput, new Regex(@"\bPS4Controller1LeftStickY\b"));
                range.SetStyle(StyleInput, new Regex(@"\bPS4Controller1RightStickX\b"));
                range.SetStyle(StyleInput, new Regex(@"\bPS4Controller1RightStickY\b"));
                range.SetStyle(StyleInput, new Regex(@"\bPS4Controller1LeftTriggerPosition\b"));
                range.SetStyle(StyleInput, new Regex(@"\bPS4Controller1RightTriggerPosition\b"));
                range.SetStyle(StyleInput, new Regex(@"\bPS4Controller1TouchX\b"));
                range.SetStyle(StyleInput, new Regex(@"\bPS4Controller1TouchY\b"));
                range.SetStyle(StyleInput, new Regex(@"\bPS4Controller1TouchOn\b"));
                range.SetStyle(StyleInput, new Regex(@"\bPS4Controller1GyroX\b"));
                range.SetStyle(StyleInput, new Regex(@"\bPS4Controller1GyroY\b"));
                range.SetStyle(StyleInput, new Regex(@"\bPS4Controller1AccelX\b"));
                range.SetStyle(StyleInput, new Regex(@"\bPS4Controller1AccelY\b"));
                range.SetStyle(StyleInput, new Regex(@"\bPS4Controller1ButtonCrossPressed\b"));
                range.SetStyle(StyleInput, new Regex(@"\bPS4Controller1ButtonCirclePressed\b"));
                range.SetStyle(StyleInput, new Regex(@"\bPS4Controller1ButtonSquarePressed\b"));
                range.SetStyle(StyleInput, new Regex(@"\bPS4Controller1ButtonTrianglePressed\b"));
                range.SetStyle(StyleInput, new Regex(@"\bPS4Controller1ButtonDPadUpPressed\b"));
                range.SetStyle(StyleInput, new Regex(@"\bPS4Controller1ButtonDPadRightPressed\b"));
                range.SetStyle(StyleInput, new Regex(@"\bPS4Controller1ButtonDPadDownPressed\b"));
                range.SetStyle(StyleInput, new Regex(@"\bPS4Controller1ButtonDPadLeftPressed\b"));
                range.SetStyle(StyleInput, new Regex(@"\bPS4Controller1ButtonL1Pressed\b"));
                range.SetStyle(StyleInput, new Regex(@"\bPS4Controller1ButtonR1Pressed\b"));
                range.SetStyle(StyleInput, new Regex(@"\bPS4Controller1ButtonL2Pressed\b"));
                range.SetStyle(StyleInput, new Regex(@"\bPS4Controller1ButtonR2Pressed\b"));
                range.SetStyle(StyleInput, new Regex(@"\bPS4Controller1ButtonL3Pressed\b"));
                range.SetStyle(StyleInput, new Regex(@"\bPS4Controller1ButtonR3Pressed\b"));
                range.SetStyle(StyleInput, new Regex(@"\bPS4Controller1ButtonCreatePressed\b"));
                range.SetStyle(StyleInput, new Regex(@"\bPS4Controller1ButtonMenuPressed\b"));
                range.SetStyle(StyleInput, new Regex(@"\bPS4Controller1ButtonLogoPressed\b"));
                range.SetStyle(StyleInput, new Regex(@"\bPS4Controller1ButtonTouchpadPressed\b"));
                range.SetStyle(StyleInput, new Regex(@"\bPS4Controller1ButtonMicPressed\b"));
                range.SetStyle(StyleInput, new Regex(@"\bPS5Controller2LeftStickX\b"));
                range.SetStyle(StyleInput, new Regex(@"\bPS5Controller2LeftStickY\b"));
                range.SetStyle(StyleInput, new Regex(@"\bPS5Controller2RightStickX\b"));
                range.SetStyle(StyleInput, new Regex(@"\bPS5Controller2RightStickY\b"));
                range.SetStyle(StyleInput, new Regex(@"\bPS5Controller2LeftTriggerPosition\b"));
                range.SetStyle(StyleInput, new Regex(@"\bPS5Controller2RightTriggerPosition\b"));
                range.SetStyle(StyleInput, new Regex(@"\bPS5Controller2TouchX\b"));
                range.SetStyle(StyleInput, new Regex(@"\bPS5Controller2TouchY\b"));
                range.SetStyle(StyleInput, new Regex(@"\bPS5Controller2TouchOn\b"));
                range.SetStyle(StyleInput, new Regex(@"\bPS5Controller2GyroX\b"));
                range.SetStyle(StyleInput, new Regex(@"\bPS5Controller2GyroY\b"));
                range.SetStyle(StyleInput, new Regex(@"\bPS5Controller2AccelX\b"));
                range.SetStyle(StyleInput, new Regex(@"\bPS5Controller2AccelY\b"));
                range.SetStyle(StyleInput, new Regex(@"\bPS5Controller2ButtonCrossPressed\b"));
                range.SetStyle(StyleInput, new Regex(@"\bPS5Controller2ButtonCirclePressed\b"));
                range.SetStyle(StyleInput, new Regex(@"\bPS5Controller2ButtonSquarePressed\b"));
                range.SetStyle(StyleInput, new Regex(@"\bPS5Controller2ButtonTrianglePressed\b"));
                range.SetStyle(StyleInput, new Regex(@"\bPS5Controller2ButtonDPadUpPressed\b"));
                range.SetStyle(StyleInput, new Regex(@"\bPS5Controller2ButtonDPadRightPressed\b"));
                range.SetStyle(StyleInput, new Regex(@"\bPS5Controller2ButtonDPadDownPressed\b"));
                range.SetStyle(StyleInput, new Regex(@"\bPS5Controller2ButtonDPadLeftPressed\b"));
                range.SetStyle(StyleInput, new Regex(@"\bPS5Controller2ButtonL1Pressed\b"));
                range.SetStyle(StyleInput, new Regex(@"\bPS5Controller2ButtonR1Pressed\b"));
                range.SetStyle(StyleInput, new Regex(@"\bPS5Controller2ButtonL2Pressed\b"));
                range.SetStyle(StyleInput, new Regex(@"\bPS5Controller2ButtonR2Pressed\b"));
                range.SetStyle(StyleInput, new Regex(@"\bPS5Controller2ButtonL3Pressed\b"));
                range.SetStyle(StyleInput, new Regex(@"\bPS5Controller2ButtonR3Pressed\b"));
                range.SetStyle(StyleInput, new Regex(@"\bPS5Controller2ButtonCreatePressed\b"));
                range.SetStyle(StyleInput, new Regex(@"\bPS5Controller2ButtonMenuPressed\b"));
                range.SetStyle(StyleInput, new Regex(@"\bPS5Controller2ButtonLogoPressed\b"));
                range.SetStyle(StyleInput, new Regex(@"\bPS5Controller2ButtonTouchpadPressed\b"));
                range.SetStyle(StyleInput, new Regex(@"\bPS5Controller2ButtonFnLPressed\b"));
                range.SetStyle(StyleInput, new Regex(@"\bPS5Controller2ButtonFnRPressed\b"));
                range.SetStyle(StyleInput, new Regex(@"\bPS5Controller2ButtonBLPPressed\b"));
                range.SetStyle(StyleInput, new Regex(@"\bPS5Controller2ButtonBRPPressed\b"));
                range.SetStyle(StyleInput, new Regex(@"\bPS5Controller2ButtonMicPressed\b"));
                range.SetStyle(StyleInput, new Regex(@"\bPS4Controller2LeftStickX\b"));
                range.SetStyle(StyleInput, new Regex(@"\bPS4Controller2LeftStickY\b"));
                range.SetStyle(StyleInput, new Regex(@"\bPS4Controller2RightStickX\b"));
                range.SetStyle(StyleInput, new Regex(@"\bPS4Controller2RightStickY\b"));
                range.SetStyle(StyleInput, new Regex(@"\bPS4Controller2LeftTriggerPosition\b"));
                range.SetStyle(StyleInput, new Regex(@"\bPS4Controller2RightTriggerPosition\b"));
                range.SetStyle(StyleInput, new Regex(@"\bPS4Controller2TouchX\b"));
                range.SetStyle(StyleInput, new Regex(@"\bPS4Controller2TouchY\b"));
                range.SetStyle(StyleInput, new Regex(@"\bPS4Controller2TouchOn\b"));
                range.SetStyle(StyleInput, new Regex(@"\bPS4Controller2GyroX\b"));
                range.SetStyle(StyleInput, new Regex(@"\bPS4Controller2GyroY\b"));
                range.SetStyle(StyleInput, new Regex(@"\bPS4Controller2AccelX\b"));
                range.SetStyle(StyleInput, new Regex(@"\bPS4Controller2AccelY\b"));
                range.SetStyle(StyleInput, new Regex(@"\bPS4Controller2ButtonCrossPressed\b"));
                range.SetStyle(StyleInput, new Regex(@"\bPS4Controller2ButtonCirclePressed\b"));
                range.SetStyle(StyleInput, new Regex(@"\bPS4Controller2ButtonSquarePressed\b"));
                range.SetStyle(StyleInput, new Regex(@"\bPS4Controller2ButtonTrianglePressed\b"));
                range.SetStyle(StyleInput, new Regex(@"\bPS4Controller2ButtonDPadUpPressed\b"));
                range.SetStyle(StyleInput, new Regex(@"\bPS4Controller2ButtonDPadRightPressed\b"));
                range.SetStyle(StyleInput, new Regex(@"\bPS4Controller2ButtonDPadDownPressed\b"));
                range.SetStyle(StyleInput, new Regex(@"\bPS4Controller2ButtonDPadLeftPressed\b"));
                range.SetStyle(StyleInput, new Regex(@"\bPS4Controller2ButtonL1Pressed\b"));
                range.SetStyle(StyleInput, new Regex(@"\bPS4Controller2ButtonR1Pressed\b"));
                range.SetStyle(StyleInput, new Regex(@"\bPS4Controller2ButtonL2Pressed\b"));
                range.SetStyle(StyleInput, new Regex(@"\bPS4Controller2ButtonR2Pressed\b"));
                range.SetStyle(StyleInput, new Regex(@"\bPS4Controller2ButtonL3Pressed\b"));
                range.SetStyle(StyleInput, new Regex(@"\bPS4Controller2ButtonR3Pressed\b"));
                range.SetStyle(StyleInput, new Regex(@"\bPS4Controller2ButtonCreatePressed\b"));
                range.SetStyle(StyleInput, new Regex(@"\bPS4Controller2ButtonMenuPressed\b"));
                range.SetStyle(StyleInput, new Regex(@"\bPS4Controller2ButtonLogoPressed\b"));
                range.SetStyle(StyleInput, new Regex(@"\bPS4Controller2ButtonTouchpadPressed\b"));
                range.SetStyle(StyleInput, new Regex(@"\bPS4Controller2ButtonMicPressed\b"));
                range.SetStyle(StyleNone, new Regex(@"\w", RegexOptions.Singleline));
            }
            catch { }
        }
        private void FillAutocompletion()
        {
            this.autocompleteMenu1.Items = new string[] {
                "TimeBeginPeriod",
                "TimeEndPeriod",
                "NtSetTimerResolution",
                "irmode",
                "centery",
                "vendor_ds_id",
                "product_ds_id",
                "product_ds_label",
                "vendor_ds4_id",
                "product_ds4_id",
                "product_ds4_label",
                "List",
                "ScanGrip",
                "ScanLeftJoycon",
                "ScanRightJoycon",
                "ScanPro",
                "ScanWiimote",
                "CurrentResolution",
                "running",
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
                "sleeptime",
                "task",
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
                "valListX.Count",
                "valListX.Clear",
                "valListX.RemoveAt",
                "valListX.Add",
                "valListX.Average",
                "valListY.Count",
                "valListY.Clear",
                "valListY.RemoveAt",
                "valListY.Add",
                "valListY.Average",
                "ViewData",
                "Close",
                "BeginPolling",
                "Connect",
                "Disconnect",
                "SetController",
                "ScanDualsense",
                "ScanDualshock4",
                "SetKM",
                "Valuechanges",
                "Valuechange",
                "ValueChange",
                "_ValueChange",
                "controllers",
                "controller",
                "XBoxController",
                "XBC",
                "XInputAPI",
                "XInputsAPI",
                "XInput",
                "xi",
                "ScanXInput",
                "controllersds4",
                "controllerds4",
                "DS4Controller",
                "DS4",
                "controllersvjoy",
                "controllervjoy",
                "VJoyController",
                "VJoy",
                "DirectInputAPI",
                "DirectInputsAPI",
                "DirectInput",
                "di",
                "ScanDirectInput",
                "DualSenseAPI",
                "DualSensesAPI",
                "DualSense",
                "DualSenses",
                "ds",
                "InitDualSenseAccel",
                "DualShock4API",
                "DualShocks4API",
                "DualShock4",
                "ds4",
                "InitDualShock4Accel",
                "Interceptions",
                "SendInterception",
                "Input",
                "si",
                "Load",
                "WiiMoteAPI",
                "WiiMote",
                "wm",
                "InitWiimote",
                "SwitchProControllerAPI",
                "SwitchProController",
                "spc",
                "InitProController",
                "InitProControllerAccel",
                "InitProControllerStick",
                "JoyconChargingGripAPI",
                "JoyconChargingGrip",
                "jcg",
                "InitJoyconChargingGrip",
                "InitJoyconChargingGripAccel",
                "InitJoyconChargingGripStick",
                "JoyconLeftAPI",
                "JoyconLeft",
                "jl",
                "InitLeftJoycon",
                "InitLeftJoyconAccel",
                "InitLeftJoyconStick",
                "JoyconRightAPI",
                "JoyconRight",
                "jr",
                "InitRightJoycon",
                "InitRightJoyconAccel",
                "InitRightJoyconStick",
                "KeyboardMouseInputAPI",
                "KeyboardsMousesInputAPI",
                "KeyboardMouseInput",
                "kmi",
                "BeginPollingMouse",
                "BeginPollingKeyboard",
                "ScanMouse",
                "ScanKeyboard",
                "keyboards",
                "SendKeyboard",
                "sk",
                "mouses",
                "SendMouse",
                "sm",
                "getstate",
                "System.Windows.Forms.Screen.PrimaryScreen.Bounds.Width",
                "System.Windows.Forms.Screen.PrimaryScreen.Bounds.Height",
                "Math.Abs",
                "Math.Sign",
                "Math.Round",
                "Math.Pow",
                "Math.Sqrt",
                "Math.Log",
                "Math.Exp",
                "Math.Min",
                "Math.Max",
                "Math.Floor",
                "Math.Truncate",
                "wd",
                "wu",
                "valchanged",
                "Scale",
                "width",
                "height",
                "Controller1ButtonAPressed",
                "Controller1ButtonBPressed",
                "Controller1ButtonXPressed",
                "Controller1ButtonYPressed",
                "Controller1ButtonStartPressed",
                "Controller1ButtonBackPressed",
                "Controller1ButtonDownPressed",
                "Controller1ButtonUpPressed",
                "Controller1ButtonLeftPressed",
                "Controller1ButtonRightPressed",
                "Controller1ButtonShoulderLeftPressed",
                "Controller1ButtonShoulderRightPressed",
                "Controller1ThumbpadLeftPressed",
                "Controller1ThumbpadRightPressed",
                "Controller1TriggerLeftPosition",
                "Controller1TriggerRightPosition",
                "Controller1ThumbLeftX",
                "Controller1ThumbLeftY",
                "Controller1ThumbRightX",
                "Controller1ThumbRightY",
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
                "Controller1DS4_Send_Option",
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
                "Joystick1AxisX",
                "Joystick1AxisY",
                "Joystick1AxisZ",
                "Joystick1RotationX",
                "Joystick1RotationY",
                "Joystick1RotationZ",
                "Joystick1Sliders0",
                "Joystick1Sliders1",
                "Joystick1PointOfViewControllers0",
                "Joystick1PointOfViewControllers1",
                "Joystick1PointOfViewControllers2",
                "Joystick1PointOfViewControllers3",
                "Joystick1VelocityX",
                "Joystick1VelocityY",
                "Joystick1VelocityZ",
                "Joystick1AngularVelocityX",
                "Joystick1AngularVelocityY",
                "Joystick1AngularVelocityZ",
                "Joystick1VelocitySliders0",
                "Joystick1VelocitySliders1",
                "Joystick1AccelerationX",
                "Joystick1AccelerationY",
                "Joystick1AccelerationZ",
                "Joystick1AngularAccelerationX",
                "Joystick1AngularAccelerationY",
                "Joystick1AngularAccelerationZ",
                "Joystick1AccelerationSliders0",
                "Joystick1AccelerationSliders1",
                "Joystick1ForceX",
                "Joystick1ForceY",
                "Joystick1ForceZ",
                "Joystick1TorqueX",
                "Joystick1TorqueY",
                "Joystick1TorqueZ",
                "Joystick1ForceSliders0",
                "Joystick1ForceSliders1",
                "Joystick1Buttons0",
                "Joystick1Buttons1",
                "Joystick1Buttons2",
                "Joystick1Buttons3",
                "Joystick1Buttons4",
                "Joystick1Buttons5",
                "Joystick1Buttons6",
                "Joystick1Buttons7",
                "Joystick1Buttons8",
                "Joystick1Buttons9",
                "Joystick1Buttons10",
                "Joystick1Buttons11",
                "Joystick1Buttons12",
                "Joystick1Buttons13",
                "Joystick1Buttons14",
                "Joystick1Buttons15",
                "Joystick1Buttons16",
                "Joystick1Buttons17",
                "Joystick1Buttons18",
                "Joystick1Buttons19",
                "Joystick1Buttons20",
                "Joystick1Buttons21",
                "Joystick1Buttons22",
                "Joystick1Buttons23",
                "Joystick1Buttons24",
                "Joystick1Buttons25",
                "Joystick1Buttons26",
                "Joystick1Buttons27",
                "Joystick1Buttons28",
                "Joystick1Buttons29",
                "Joystick1Buttons30",
                "Joystick1Buttons31",
                "Joystick1Buttons32",
                "Joystick1Buttons33",
                "Joystick1Buttons34",
                "Joystick1Buttons35",
                "Joystick1Buttons36",
                "Joystick1Buttons37",
                "Joystick1Buttons38",
                "Joystick1Buttons39",
                "Joystick1Buttons40",
                "Joystick1Buttons41",
                "Joystick1Buttons42",
                "Joystick1Buttons43",
                "Joystick1Buttons44",
                "Joystick1Buttons45",
                "Joystick1Buttons46",
                "Joystick1Buttons47",
                "Joystick1Buttons48",
                "Joystick1Buttons49",
                "Joystick1Buttons50",
                "Joystick1Buttons51",
                "Joystick1Buttons52",
                "Joystick1Buttons53",
                "Joystick1Buttons54",
                "Joystick1Buttons55",
                "Joystick1Buttons56",
                "Joystick1Buttons57",
                "Joystick1Buttons58",
                "Joystick1Buttons59",
                "Joystick1Buttons60",
                "Joystick1Buttons61",
                "Joystick1Buttons62",
                "Joystick1Buttons63",
                "Joystick1Buttons64",
                "Joystick1Buttons65",
                "Joystick1Buttons66",
                "Joystick1Buttons67",
                "Joystick1Buttons68",
                "Joystick1Buttons69",
                "Joystick1Buttons70",
                "Joystick1Buttons71",
                "Joystick1Buttons72",
                "Joystick1Buttons73",
                "Joystick1Buttons74",
                "Joystick1Buttons75",
                "Joystick1Buttons76",
                "Joystick1Buttons77",
                "Joystick1Buttons78",
                "Joystick1Buttons79",
                "Joystick1Buttons80",
                "Joystick1Buttons81",
                "Joystick1Buttons82",
                "Joystick1Buttons83",
                "Joystick1Buttons84",
                "Joystick1Buttons85",
                "Joystick1Buttons86",
                "Joystick1Buttons87",
                "Joystick1Buttons88",
                "Joystick1Buttons89",
                "Joystick1Buttons90",
                "Joystick1Buttons91",
                "Joystick1Buttons92",
                "Joystick1Buttons93",
                "Joystick1Buttons94",
                "Joystick1Buttons95",
                "Joystick1Buttons96",
                "Joystick1Buttons97",
                "Joystick1Buttons98",
                "Joystick1Buttons99",
                "Joystick1Buttons100",
                "Joystick1Buttons101",
                "Joystick1Buttons102",
                "Joystick1Buttons103",
                "Joystick1Buttons104",
                "Joystick1Buttons105",
                "Joystick1Buttons106",
                "Joystick1Buttons107",
                "Joystick1Buttons108",
                "Joystick1Buttons109",
                "Joystick1Buttons110",
                "Joystick1Buttons111",
                "Joystick1Buttons112",
                "Joystick1Buttons113",
                "Joystick1Buttons114",
                "Joystick1Buttons115",
                "Joystick1Buttons116",
                "Joystick1Buttons117",
                "Joystick1Buttons118",
                "Joystick1Buttons119",
                "Joystick1Buttons120",
                "Joystick1Buttons121",
                "Joystick1Buttons122",
                "Joystick1Buttons123",
                "Joystick1Buttons124",
                "Joystick1Buttons125",
                "Joystick1Buttons126",
                "Joystick1Buttons127",
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
                "Mouse1Buttons0",
                "Mouse1Buttons1",
                "Mouse1Buttons2",
                "Mouse1Buttons3",
                "Mouse1Buttons4",
                "Mouse1Buttons5",
                "Mouse1Buttons6",
                "Mouse1Buttons7",
                "Mouse1AxisX",
                "Mouse1AxisY",
                "Mouse1AxisZ",
                "Keyboard1KeyEscape",
                "Keyboard1KeyD1",
                "Keyboard1KeyD2",
                "Keyboard1KeyD3",
                "Keyboard1KeyD4",
                "Keyboard1KeyD5",
                "Keyboard1KeyD6",
                "Keyboard1KeyD7",
                "Keyboard1KeyD8",
                "Keyboard1KeyD9",
                "Keyboard1KeyD0",
                "Keyboard1KeyMinus",
                "Keyboard1KeyEquals",
                "Keyboard1KeyBack",
                "Keyboard1KeyTab",
                "Keyboard1KeyQ",
                "Keyboard1KeyW",
                "Keyboard1KeyE",
                "Keyboard1KeyR",
                "Keyboard1KeyT",
                "Keyboard1KeyY",
                "Keyboard1KeyU",
                "Keyboard1KeyI",
                "Keyboard1KeyO",
                "Keyboard1KeyP",
                "Keyboard1KeyLeftBracket",
                "Keyboard1KeyRightBracket",
                "Keyboard1KeyReturn",
                "Keyboard1KeyLeftControl",
                "Keyboard1KeyA",
                "Keyboard1KeyS",
                "Keyboard1KeyD",
                "Keyboard1KeyF",
                "Keyboard1KeyG",
                "Keyboard1KeyH",
                "Keyboard1KeyJ",
                "Keyboard1KeyK",
                "Keyboard1KeyL",
                "Keyboard1KeySemicolon",
                "Keyboard1KeyApostrophe",
                "Keyboard1KeyGrave",
                "Keyboard1KeyLeftShift",
                "Keyboard1KeyBackslash",
                "Keyboard1KeyZ",
                "Keyboard1KeyX",
                "Keyboard1KeyC",
                "Keyboard1KeyV",
                "Keyboard1KeyB",
                "Keyboard1KeyN",
                "Keyboard1KeyM",
                "Keyboard1KeyComma",
                "Keyboard1KeyPeriod",
                "Keyboard1KeySlash",
                "Keyboard1KeyRightShift",
                "Keyboard1KeyMultiply",
                "Keyboard1KeyLeftAlt",
                "Keyboard1KeySpace",
                "Keyboard1KeyCapital",
                "Keyboard1KeyF1",
                "Keyboard1KeyF2",
                "Keyboard1KeyF3",
                "Keyboard1KeyF4",
                "Keyboard1KeyF5",
                "Keyboard1KeyF6",
                "Keyboard1KeyF7",
                "Keyboard1KeyF8",
                "Keyboard1KeyF9",
                "Keyboard1KeyF10",
                "Keyboard1KeyNumberLock",
                "Keyboard1KeyScrollLock",
                "Keyboard1KeyNumberPad7",
                "Keyboard1KeyNumberPad8",
                "Keyboard1KeyNumberPad9",
                "Keyboard1KeySubtract",
                "Keyboard1KeyNumberPad4",
                "Keyboard1KeyNumberPad5",
                "Keyboard1KeyNumberPad6",
                "Keyboard1KeyAdd",
                "Keyboard1KeyNumberPad1",
                "Keyboard1KeyNumberPad2",
                "Keyboard1KeyNumberPad3",
                "Keyboard1KeyNumberPad0",
                "Keyboard1KeyDecimal",
                "Keyboard1KeyOem102",
                "Keyboard1KeyF11",
                "Keyboard1KeyF12",
                "Keyboard1KeyF13",
                "Keyboard1KeyF14",
                "Keyboard1KeyF15",
                "Keyboard1KeyKana",
                "Keyboard1KeyAbntC1",
                "Keyboard1KeyConvert",
                "Keyboard1KeyNoConvert",
                "Keyboard1KeyYen",
                "Keyboard1KeyAbntC2",
                "Keyboard1KeyNumberPadEquals",
                "Keyboard1KeyPreviousTrack",
                "Keyboard1KeyAT",
                "Keyboard1KeyColon",
                "Keyboard1KeyUnderline",
                "Keyboard1KeyKanji",
                "Keyboard1KeyStop",
                "Keyboard1KeyAX",
                "Keyboard1KeyUnlabeled",
                "Keyboard1KeyNextTrack",
                "Keyboard1KeyNumberPadEnter",
                "Keyboard1KeyRightControl",
                "Keyboard1KeyMute",
                "Keyboard1KeyCalculator",
                "Keyboard1KeyPlayPause",
                "Keyboard1KeyMediaStop",
                "Keyboard1KeyVolumeDown",
                "Keyboard1KeyVolumeUp",
                "Keyboard1KeyWebHome",
                "Keyboard1KeyNumberPadComma",
                "Keyboard1KeyDivide",
                "Keyboard1KeyPrintScreen",
                "Keyboard1KeyRightAlt",
                "Keyboard1KeyPause",
                "Keyboard1KeyHome",
                "Keyboard1KeyUp",
                "Keyboard1KeyPageUp",
                "Keyboard1KeyLeft",
                "Keyboard1KeyRight",
                "Keyboard1KeyEnd",
                "Keyboard1KeyDown",
                "Keyboard1KeyPageDown",
                "Keyboard1KeyInsert",
                "Keyboard1KeyDelete",
                "Keyboard1KeyLeftWindowsKey",
                "Keyboard1KeyRightWindowsKey",
                "Keyboard1KeyApplications",
                "Keyboard1KeyPower",
                "Keyboard1KeySleep",
                "Keyboard1KeyWake",
                "Keyboard1KeyWebSearch",
                "Keyboard1KeyWebFavorites",
                "Keyboard1KeyWebRefresh",
                "Keyboard1KeyWebStop",
                "Keyboard1KeyWebForward",
                "Keyboard1KeyWebBack",
                "Keyboard1KeyMyComputer",
                "Keyboard1KeyMail",
                "Keyboard1KeyMediaSelect",
                "Keyboard1KeyUnknown",
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
                ",Controller2ButtonAPressed",
                ",Controller2ButtonBPressed",
                ",Controller2ButtonXPressed",
                ",Controller2ButtonYPressed",
                ",Controller2ButtonStartPressed",
                ",Controller2ButtonBackPressed",
                ",Controller2ButtonDownPressed",
                ",Controller2ButtonUpPressed",
                ",Controller2ButtonLeftPressed",
                ",Controller2ButtonRightPressed",
                ",Controller2ButtonShoulderLeftPressed",
                ",Controller2ButtonShoulderRightPressed",
                ",Controller2ThumbpadLeftPressed",
                ",Controller2ThumbpadRightPressed",
                ",Controller2TriggerLeftPosition",
                ",Controller2TriggerRightPosition",
                ",Controller2ThumbLeftX",
                ",Controller2ThumbLeftY",
                ",Controller2ThumbRightX",
                ",Controller2ThumbRightY",
                ",Controller2_send_xbox",
                ",Controller2_send_back",
                ",Controller2_send_start",
                ",Controller2_send_A",
                ",Controller2_send_B",
                ",Controller2_send_X",
                ",Controller2_send_Y",
                ",Controller2_send_up",
                ",Controller2_send_left",
                ",Controller2_send_down",
                ",Controller2_send_right",
                ",Controller2_send_leftstick",
                ",Controller2_send_rightstick",
                ",Controller2_send_leftbumper",
                ",Controller2_send_rightbumper",
                ",Controller2_send_lefttriggerposition",
                ",Controller2_send_righttriggerposition",
                ",Controller2_send_leftstickx",
                ",Controller2_send_leftsticky",
                ",Controller2_send_rightstickx",
                ",Controller2_send_rightsticky",
                ",Controller2DS4_Send_Options",
                ",Controller2DS4_Send_Option",
                ",Controller2DS4_Send_ThumbLeft",
                ",Controller2DS4_Send_ThumbRight",
                ",Controller2DS4_Send_ShoulderLeft",
                ",Controller2DS4_Send_ShoulderRight",
                ",Controller2DS4_Send_Cross",
                ",Controller2DS4_Send_Circle",
                ",Controller2DS4_Send_Square",
                ",Controller2DS4_Send_Triangle",
                ",Controller2DS4_Send_Ps",
                ",Controller2DS4_Send_Touchpad",
                ",Controller2DS4_Send_Share",
                ",Controller2DS4_Send_DPadUp",
                ",Controller2DS4_Send_DPadDown",
                ",Controller2DS4_Send_DPadLeft",
                ",Controller2DS4_Send_DPadRight",
                ",Controller2DS4_Send_LeftTrigger",
                ",Controller2DS4_Send_RightTrigger",
                ",Controller2DS4_Send_LeftTriggerPosition",
                ",Controller2DS4_Send_RightTriggerPosition",
                ",Controller2DS4_Send_LeftThumbX",
                ",Controller2DS4_Send_RightThumbX",
                ",Controller2DS4_Send_LeftThumbY",
                ",Controller2DS4_Send_RightThumbY",
                ",Controller2VJoy_Send_1",
                ",Controller2VJoy_Send_2",
                ",Controller2VJoy_Send_3",
                ",Controller2VJoy_Send_4",
                ",Controller2VJoy_Send_5",
                ",Controller2VJoy_Send_6",
                ",Controller2VJoy_Send_7",
                ",Controller2VJoy_Send_8",
                ",Controller2VJoy_Send_X",
                ",Controller2VJoy_Send_Y",
                ",Controller2VJoy_Send_Z",
                ",Controller2VJoy_Send_WHL",
                ",Controller2VJoy_Send_SL0",
                ",Controller2VJoy_Send_SL1",
                ",Controller2VJoy_Send_RX",
                ",Controller2VJoy_Send_RY",
                ",Controller2VJoy_Send_RZ",
                ",Controller2VJoy_Send_POV",
                ",Controller2VJoy_Send_Hat",
                ",Controller2VJoy_Send_HatExt1",
                ",Controller2VJoy_Send_HatExt2",
                ",Controller2VJoy_Send_HatExt3",
                "Joystick2AxisX",
                "Joystick2AxisY",
                "Joystick2AxisZ",
                "Joystick2RotationX",
                "Joystick2RotationY",
                "Joystick2RotationZ",
                "Joystick2Sliders0",
                "Joystick2Sliders1",
                "Joystick2PointOfViewControllers0",
                "Joystick2PointOfViewControllers1",
                "Joystick2PointOfViewControllers2",
                "Joystick2PointOfViewControllers3",
                "Joystick2VelocityX",
                "Joystick2VelocityY",
                "Joystick2VelocityZ",
                "Joystick2AngularVelocityX",
                "Joystick2AngularVelocityY",
                "Joystick2AngularVelocityZ",
                "Joystick2VelocitySliders0",
                "Joystick2VelocitySliders1",
                "Joystick2AccelerationX",
                "Joystick2AccelerationY",
                "Joystick2AccelerationZ",
                "Joystick2AngularAccelerationX",
                "Joystick2AngularAccelerationY",
                "Joystick2AngularAccelerationZ",
                "Joystick2AccelerationSliders0",
                "Joystick2AccelerationSliders1",
                "Joystick2ForceX",
                "Joystick2ForceY",
                "Joystick2ForceZ",
                "Joystick2TorqueX",
                "Joystick2TorqueY",
                "Joystick2TorqueZ",
                "Joystick2ForceSliders0",
                "Joystick2ForceSliders1",
                "Joystick2Buttons0",
                "Joystick2Buttons1",
                "Joystick2Buttons2",
                "Joystick2Buttons3",
                "Joystick2Buttons4",
                "Joystick2Buttons5",
                "Joystick2Buttons6",
                "Joystick2Buttons7",
                "Joystick2Buttons8",
                "Joystick2Buttons9",
                "Joystick2Buttons10",
                "Joystick2Buttons11",
                "Joystick2Buttons12",
                "Joystick2Buttons13",
                "Joystick2Buttons14",
                "Joystick2Buttons15",
                "Joystick2Buttons16",
                "Joystick2Buttons17",
                "Joystick2Buttons18",
                "Joystick2Buttons19",
                "Joystick2Buttons20",
                "Joystick2Buttons21",
                "Joystick2Buttons22",
                "Joystick2Buttons23",
                "Joystick2Buttons24",
                "Joystick2Buttons25",
                "Joystick2Buttons26",
                "Joystick2Buttons27",
                "Joystick2Buttons28",
                "Joystick2Buttons29",
                "Joystick2Buttons30",
                "Joystick2Buttons31",
                "Joystick2Buttons32",
                "Joystick2Buttons33",
                "Joystick2Buttons34",
                "Joystick2Buttons35",
                "Joystick2Buttons36",
                "Joystick2Buttons37",
                "Joystick2Buttons38",
                "Joystick2Buttons39",
                "Joystick2Buttons40",
                "Joystick2Buttons41",
                "Joystick2Buttons42",
                "Joystick2Buttons43",
                "Joystick2Buttons44",
                "Joystick2Buttons45",
                "Joystick2Buttons46",
                "Joystick2Buttons47",
                "Joystick2Buttons48",
                "Joystick2Buttons49",
                "Joystick2Buttons50",
                "Joystick2Buttons51",
                "Joystick2Buttons52",
                "Joystick2Buttons53",
                "Joystick2Buttons54",
                "Joystick2Buttons55",
                "Joystick2Buttons56",
                "Joystick2Buttons57",
                "Joystick2Buttons58",
                "Joystick2Buttons59",
                "Joystick2Buttons60",
                "Joystick2Buttons61",
                "Joystick2Buttons62",
                "Joystick2Buttons63",
                "Joystick2Buttons64",
                "Joystick2Buttons65",
                "Joystick2Buttons66",
                "Joystick2Buttons67",
                "Joystick2Buttons68",
                "Joystick2Buttons69",
                "Joystick2Buttons70",
                "Joystick2Buttons71",
                "Joystick2Buttons72",
                "Joystick2Buttons73",
                "Joystick2Buttons74",
                "Joystick2Buttons75",
                "Joystick2Buttons76",
                "Joystick2Buttons77",
                "Joystick2Buttons78",
                "Joystick2Buttons79",
                "Joystick2Buttons80",
                "Joystick2Buttons81",
                "Joystick2Buttons82",
                "Joystick2Buttons83",
                "Joystick2Buttons84",
                "Joystick2Buttons85",
                "Joystick2Buttons86",
                "Joystick2Buttons87",
                "Joystick2Buttons88",
                "Joystick2Buttons89",
                "Joystick2Buttons90",
                "Joystick2Buttons91",
                "Joystick2Buttons92",
                "Joystick2Buttons93",
                "Joystick2Buttons94",
                "Joystick2Buttons95",
                "Joystick2Buttons96",
                "Joystick2Buttons97",
                "Joystick2Buttons98",
                "Joystick2Buttons99",
                "Joystick2Buttons100",
                "Joystick2Buttons101",
                "Joystick2Buttons102",
                "Joystick2Buttons103",
                "Joystick2Buttons104",
                "Joystick2Buttons105",
                "Joystick2Buttons106",
                "Joystick2Buttons107",
                "Joystick2Buttons108",
                "Joystick2Buttons109",
                "Joystick2Buttons110",
                "Joystick2Buttons111",
                "Joystick2Buttons112",
                "Joystick2Buttons113",
                "Joystick2Buttons114",
                "Joystick2Buttons115",
                "Joystick2Buttons116",
                "Joystick2Buttons117",
                "Joystick2Buttons118",
                "Joystick2Buttons119",
                "Joystick2Buttons120",
                "Joystick2Buttons121",
                "Joystick2Buttons122",
                "Joystick2Buttons123",
                "Joystick2Buttons124",
                "Joystick2Buttons125",
                "Joystick2Buttons126",
                "Joystick2Buttons127",
                "Mouse2Buttons0",
                "Mouse2Buttons1",
                "Mouse2Buttons2",
                "Mouse2Buttons3",
                "Mouse2Buttons4",
                "Mouse2Buttons5",
                "Mouse2Buttons6",
                "Mouse2Buttons7",
                "Mouse2AxisX",
                "Mouse2AxisY",
                "Mouse2AxisZ",
                "Keyboard2KeyEscape",
                "Keyboard2KeyD1",
                "Keyboard2KeyD2",
                "Keyboard2KeyD3",
                "Keyboard2KeyD4",
                "Keyboard2KeyD5",
                "Keyboard2KeyD6",
                "Keyboard2KeyD7",
                "Keyboard2KeyD8",
                "Keyboard2KeyD9",
                "Keyboard2KeyD0",
                "Keyboard2KeyMinus",
                "Keyboard2KeyEquals",
                "Keyboard2KeyBack",
                "Keyboard2KeyTab",
                "Keyboard2KeyQ",
                "Keyboard2KeyW",
                "Keyboard2KeyE",
                "Keyboard2KeyR",
                "Keyboard2KeyT",
                "Keyboard2KeyY",
                "Keyboard2KeyU",
                "Keyboard2KeyI",
                "Keyboard2KeyO",
                "Keyboard2KeyP",
                "Keyboard2KeyLeftBracket",
                "Keyboard2KeyRightBracket",
                "Keyboard2KeyReturn",
                "Keyboard2KeyLeftControl",
                "Keyboard2KeyA",
                "Keyboard2KeyS",
                "Keyboard2KeyD",
                "Keyboard2KeyF",
                "Keyboard2KeyG",
                "Keyboard2KeyH",
                "Keyboard2KeyJ",
                "Keyboard2KeyK",
                "Keyboard2KeyL",
                "Keyboard2KeySemicolon",
                "Keyboard2KeyApostrophe",
                "Keyboard2KeyGrave",
                "Keyboard2KeyLeftShift",
                "Keyboard2KeyBackslash",
                "Keyboard2KeyZ",
                "Keyboard2KeyX",
                "Keyboard2KeyC",
                "Keyboard2KeyV",
                "Keyboard2KeyB",
                "Keyboard2KeyN",
                "Keyboard2KeyM",
                "Keyboard2KeyComma",
                "Keyboard2KeyPeriod",
                "Keyboard2KeySlash",
                "Keyboard2KeyRightShift",
                "Keyboard2KeyMultiply",
                "Keyboard2KeyLeftAlt",
                "Keyboard2KeySpace",
                "Keyboard2KeyCapital",
                "Keyboard2KeyF1",
                "Keyboard2KeyF2",
                "Keyboard2KeyF3",
                "Keyboard2KeyF4",
                "Keyboard2KeyF5",
                "Keyboard2KeyF6",
                "Keyboard2KeyF7",
                "Keyboard2KeyF8",
                "Keyboard2KeyF9",
                "Keyboard2KeyF10",
                "Keyboard2KeyNumberLock",
                "Keyboard2KeyScrollLock",
                "Keyboard2KeyNumberPad7",
                "Keyboard2KeyNumberPad8",
                "Keyboard2KeyNumberPad9",
                "Keyboard2KeySubtract",
                "Keyboard2KeyNumberPad4",
                "Keyboard2KeyNumberPad5",
                "Keyboard2KeyNumberPad6",
                "Keyboard2KeyAdd",
                "Keyboard2KeyNumberPad1",
                "Keyboard2KeyNumberPad2",
                "Keyboard2KeyNumberPad3",
                "Keyboard2KeyNumberPad0",
                "Keyboard2KeyDecimal",
                "Keyboard2KeyOem102",
                "Keyboard2KeyF11",
                "Keyboard2KeyF12",
                "Keyboard2KeyF13",
                "Keyboard2KeyF14",
                "Keyboard2KeyF15",
                "Keyboard2KeyKana",
                "Keyboard2KeyAbntC1",
                "Keyboard2KeyConvert",
                "Keyboard2KeyNoConvert",
                "Keyboard2KeyYen",
                "Keyboard2KeyAbntC2",
                "Keyboard2KeyNumberPadEquals",
                "Keyboard2KeyPreviousTrack",
                "Keyboard2KeyAT",
                "Keyboard2KeyColon",
                "Keyboard2KeyUnderline",
                "Keyboard2KeyKanji",
                "Keyboard2KeyStop",
                "Keyboard2KeyAX",
                "Keyboard2KeyUnlabeled",
                "Keyboard2KeyNextTrack",
                "Keyboard2KeyNumberPadEnter",
                "Keyboard2KeyRightControl",
                "Keyboard2KeyMute",
                "Keyboard2KeyCalculator",
                "Keyboard2KeyPlayPause",
                "Keyboard2KeyMediaStop",
                "Keyboard2KeyVolumeDown",
                "Keyboard2KeyVolumeUp",
                "Keyboard2KeyWebHome",
                "Keyboard2KeyNumberPadComma",
                "Keyboard2KeyDivide",
                "Keyboard2KeyPrintScreen",
                "Keyboard2KeyRightAlt",
                "Keyboard2KeyPause",
                "Keyboard2KeyHome",
                "Keyboard2KeyUp",
                "Keyboard2KeyPageUp",
                "Keyboard2KeyLeft",
                "Keyboard2KeyRight",
                "Keyboard2KeyEnd",
                "Keyboard2KeyDown",
                "Keyboard2KeyPageDown",
                "Keyboard2KeyInsert",
                "Keyboard2KeyDelete",
                "Keyboard2KeyLeftWindowsKey",
                "Keyboard2KeyRightWindowsKey",
                "Keyboard2KeyApplications",
                "Keyboard2KeyPower",
                "Keyboard2KeySleep",
                "Keyboard2KeyWake",
                "Keyboard2KeyWebSearch",
                "Keyboard2KeyWebFavorites",
                "Keyboard2KeyWebRefresh",
                "Keyboard2KeyWebStop",
                "Keyboard2KeyWebForward",
                "Keyboard2KeyWebBack",
                "Keyboard2KeyMyComputer",
                "Keyboard2KeyMail",
                "Keyboard2KeyMediaSelect",
                "Keyboard2KeyUnknown",
                "PS5Controller1LeftStickX",
                "PS5Controller1LeftStickY",
                "PS5Controller1RightStickX",
                "PS5Controller1RightStickY",
                "PS5Controller1LeftTriggerPosition",
                "PS5Controller1RightTriggerPosition",
                "PS5Controller1TouchX",
                "PS5Controller1TouchY",
                "PS5Controller1TouchOn",
                "PS5Controller1GyroX",
                "PS5Controller1GyroY",
                "PS5Controller1AccelX",
                "PS5Controller1AccelY",
                "PS5Controller1ButtonCrossPressed",
                "PS5Controller1ButtonCirclePressed",
                "PS5Controller1ButtonSquarePressed",
                "PS5Controller1ButtonTrianglePressed",
                "PS5Controller1ButtonDPadUpPressed",
                "PS5Controller1ButtonDPadRightPressed",
                "PS5Controller1ButtonDPadDownPressed",
                "PS5Controller1ButtonDPadLeftPressed",
                "PS5Controller1ButtonL1Pressed",
                "PS5Controller1ButtonR1Pressed",
                "PS5Controller1ButtonL2Pressed",
                "PS5Controller1ButtonR2Pressed",
                "PS5Controller1ButtonL3Pressed",
                "PS5Controller1ButtonR3Pressed",
                "PS5Controller1ButtonCreatePressed",
                "PS5Controller1ButtonMenuPressed",
                "PS5Controller1ButtonLogoPressed",
                "PS5Controller1ButtonTouchpadPressed",
                "PS5Controller1ButtonFnLPressed",
                "PS5Controller1ButtonFnRPressed",
                "PS5Controller1ButtonBLPPressed",
                "PS5Controller1ButtonBRPPressed",
                "PS5Controller1ButtonMicPressed",
                "PS4Controller1LeftStickX",
                "PS4Controller1LeftStickY",
                "PS4Controller1RightStickX",
                "PS4Controller1RightStickY",
                "PS4Controller1LeftTriggerPosition",
                "PS4Controller1RightTriggerPosition",
                "PS4Controller1TouchX",
                "PS4Controller1TouchY",
                "PS4Controller1TouchOn",
                "PS4Controller1GyroX",
                "PS4Controller1GyroY",
                "PS4Controller1AccelX",
                "PS4Controller1AccelY",
                "PS4Controller1ButtonCrossPressed",
                "PS4Controller1ButtonCirclePressed",
                "PS4Controller1ButtonSquarePressed",
                "PS4Controller1ButtonTrianglePressed",
                "PS4Controller1ButtonDPadUpPressed",
                "PS4Controller1ButtonDPadRightPressed",
                "PS4Controller1ButtonDPadDownPressed",
                "PS4Controller1ButtonDPadLeftPressed",
                "PS4Controller1ButtonL1Pressed",
                "PS4Controller1ButtonR1Pressed",
                "PS4Controller1ButtonL2Pressed",
                "PS4Controller1ButtonR2Pressed",
                "PS4Controller1ButtonL3Pressed",
                "PS4Controller1ButtonR3Pressed",
                "PS4Controller1ButtonCreatePressed",
                "PS4Controller1ButtonMenuPressed",
                "PS4Controller1ButtonLogoPressed",
                "PS4Controller1ButtonTouchpadPressed",
                "PS4Controller1ButtonMicPressed",
                "PS5Controller2LeftStickX",
                "PS5Controller2LeftStickY",
                "PS5Controller2RightStickX",
                "PS5Controller2RightStickY",
                "PS5Controller2LeftTriggerPosition",
                "PS5Controller2RightTriggerPosition",
                "PS5Controller2TouchX",
                "PS5Controller2TouchY",
                "PS5Controller2TouchOn",
                "PS5Controller2GyroX",
                "PS5Controller2GyroY",
                "PS5Controller2AccelX",
                "PS5Controller2AccelY",
                "PS5Controller2ButtonCrossPressed",
                "PS5Controller2ButtonCirclePressed",
                "PS5Controller2ButtonSquarePressed",
                "PS5Controller2ButtonTrianglePressed",
                "PS5Controller2ButtonDPadUpPressed",
                "PS5Controller2ButtonDPadRightPressed",
                "PS5Controller2ButtonDPadDownPressed",
                "PS5Controller2ButtonDPadLeftPressed",
                "PS5Controller2ButtonL1Pressed",
                "PS5Controller2ButtonR1Pressed",
                "PS5Controller2ButtonL2Pressed",
                "PS5Controller2ButtonR2Pressed",
                "PS5Controller2ButtonL3Pressed",
                "PS5Controller2ButtonR3Pressed",
                "PS5Controller2ButtonCreatePressed",
                "PS5Controller2ButtonMenuPressed",
                "PS5Controller2ButtonLogoPressed",
                "PS5Controller2ButtonTouchpadPressed",
                "PS5Controller2ButtonFnLPressed",
                "PS5Controller2ButtonFnRPressed",
                "PS5Controller2ButtonBLPPressed",
                "PS5Controller2ButtonBRPPressed",
                "PS5Controller2ButtonMicPressed",
                "PS4Controller2LeftStickX",
                "PS4Controller2LeftStickY",
                "PS4Controller2RightStickX",
                "PS4Controller2RightStickY",
                "PS4Controller2LeftTriggerPosition",
                "PS4Controller2RightTriggerPosition",
                "PS4Controller2TouchX",
                "PS4Controller2TouchY",
                "PS4Controller2TouchOn",
                "PS4Controller2GyroX",
                "PS4Controller2GyroY",
                "PS4Controller2AccelX",
                "PS4Controller2AccelY",
                "PS4Controller2ButtonCrossPressed",
                "PS4Controller2ButtonCirclePressed",
                "PS4Controller2ButtonSquarePressed",
                "PS4Controller2ButtonTrianglePressed",
                "PS4Controller2ButtonDPadUpPressed",
                "PS4Controller2ButtonDPadRightPressed",
                "PS4Controller2ButtonDPadDownPressed",
                "PS4Controller2ButtonDPadLeftPressed",
                "PS4Controller2ButtonL1Pressed",
                "PS4Controller2ButtonR1Pressed",
                "PS4Controller2ButtonL2Pressed",
                "PS4Controller2ButtonR2Pressed",
                "PS4Controller2ButtonL3Pressed",
                "PS4Controller2ButtonR3Pressed",
                "PS4Controller2ButtonCreatePressed",
                "PS4Controller2ButtonMenuPressed",
                "PS4Controller2ButtonLogoPressed",
                "PS4Controller2ButtonTouchpadPressed",
                "PS4Controller2ButtonMicPressed"
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
            const string message = "• Input Devices : Wiimote and Joycon left (WiiJoyL), Wiimote and Joycon right (WiiJoyR), Wiimote and Nunchuck (Wii), Joycons (Joys), Joycon left (JoyL), Joycon right (JoyR), Switch Pro Controller (SPC), Joycon Charging Grip (JCG), DirectInput Controller (DIC), DirectInput Controller and Mouse (DICM), Dualsense (DS), Dualshock4 (DS4), Keyboard and Mouse (KM), Xbox Controller (XC), Xbox Controller and Mouse (XCM), Mouse and Joycon left (MJoyL), Mouse and Joycon right (MJoyR).\n\r\n\r• Output Devices : Xbox Controller (XC), Keyboard and Mouse (KM), Interception (Int), Dualshock4 Controller (DS4), VJoy Controller (VJoy).\n\r\n\r• Pairing Devices : Wiimote and Joycon left or Wiimote and Joycon right or Wiimote or Joycons or Joycon left or Joycon right need to be set in pairing mode after starting the run process, Switch Pro Controller or Joycon Charging Grip or DirectInput Controller or Dualsense or Dualshock4 or Xbox Controller or Keyboard and Mouse need to be USB wired.";
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
                stringscript = fastColoredTextBox1.Text;
                File.WriteAllText(filename, stringscript);
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
                stringscript = fastColoredTextBox1.Text;
                File.WriteAllText(sf.FileName, stringscript);
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
            code = @"funct_driver".Replace("\"", "\"\"");
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
            parameters.ReferencedAssemblies.Add(Application.StartupPath + @"\controller.dll");
            parameters.ReferencedAssemblies.Add(Application.StartupPath + @"\controllerds4.dll");
            parameters.ReferencedAssemblies.Add(Application.StartupPath + @"\controllervjoy.dll");
            parameters.ReferencedAssemblies.Add(Application.StartupPath + @"\controllers.dll");
            parameters.ReferencedAssemblies.Add(Application.StartupPath + @"\controllersds4.dll");
            parameters.ReferencedAssemblies.Add(Application.StartupPath + @"\controllersvjoy.dll");
            parameters.ReferencedAssemblies.Add(Application.StartupPath + @"\keyboards.dll");
            parameters.ReferencedAssemblies.Add(Application.StartupPath + @"\mouses.dll");
            parameters.ReferencedAssemblies.Add(Application.StartupPath + @"\Interceptions.dll");
            parameters.ReferencedAssemblies.Add(Application.StartupPath + @"\Valuechanges.dll");
            parameters.ReferencedAssemblies.Add(Application.StartupPath + @"\KeyboardMouseinput.dll");
            parameters.ReferencedAssemblies.Add(Application.StartupPath + @"\KeyboardsMousesinput.dll");
            parameters.ReferencedAssemblies.Add(Application.StartupPath + @"\Dualsense.dll");
            parameters.ReferencedAssemblies.Add(Application.StartupPath + @"\Dualshock4.dll");
            parameters.ReferencedAssemblies.Add(Application.StartupPath + @"\Dualsenses.dll");
            parameters.ReferencedAssemblies.Add(Application.StartupPath + @"\Dualshocks4.dll");
            parameters.ReferencedAssemblies.Add(Application.StartupPath + @"\Directinput.dll");
            parameters.ReferencedAssemblies.Add(Application.StartupPath + @"\Directinputs.dll");
            parameters.ReferencedAssemblies.Add(Application.StartupPath + @"\Joyconcharginggrip.dll");
            parameters.ReferencedAssemblies.Add(Application.StartupPath + @"\Joyconleft.dll");
            parameters.ReferencedAssemblies.Add(Application.StartupPath + @"\Joyconright.dll");
            parameters.ReferencedAssemblies.Add(Application.StartupPath + @"\Switchprocontroller.dll");
            parameters.ReferencedAssemblies.Add(Application.StartupPath + @"\Wiimote.dll");
            parameters.ReferencedAssemblies.Add(Application.StartupPath + @"\Xinput.dll");
            parameters.ReferencedAssemblies.Add(Application.StartupPath + @"\Xinputs.dll");
        }
        private void testToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FillCode();
            stringscript = fastColoredTextBox1.Text;
            string finalcode = code.Replace("funct_driver", stringscript);
            provider = new Microsoft.CSharp.CSharpCodeProvider();
            results = provider.CompileAssemblyFromSource(parameters, finalcode);
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
            stringscript = fastColoredTextBox1.Text;
            string finalcode = code.Replace("funct_driver", stringscript);
            provider = new Microsoft.CSharp.CSharpCodeProvider();
            results = provider.CompileAssemblyFromSource(parameters, finalcode);
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
    }
}