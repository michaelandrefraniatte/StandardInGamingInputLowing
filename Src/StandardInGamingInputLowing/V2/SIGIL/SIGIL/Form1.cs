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
        private static Style StyleInput = new TextStyle(Brushes.Blue, null, System.Drawing.FontStyle.Regular), StyleOutput = new TextStyle(Brushes.Orange, null, System.Drawing.FontStyle.Regular), StyleLibrary = new TextStyle(Brushes.BlueViolet, null, System.Drawing.FontStyle.Regular), StyleClass = new TextStyle(Brushes.DodgerBlue, null, System.Drawing.FontStyle.Regular), StyleMethod = new TextStyle(Brushes.Magenta, null, System.Drawing.FontStyle.Regular), StyleObject = new TextStyle(Brushes.DarkOrange, null, System.Drawing.FontStyle.Regular), StyleExtra = new TextStyle(Brushes.Red, null, System.Drawing.FontStyle.Regular), StyleSpecial = new TextStyle(Brushes.DarkCyan, null, System.Drawing.FontStyle.Regular);
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
                range.SetStyle(StyleExtra, new Regex(@"CurrentResolution"));
                range.SetStyle(StyleExtra, new Regex(@"running"));
                range.SetStyle(StyleExtra, new Regex(@"statex"));
                range.SetStyle(StyleExtra, new Regex(@"statey"));
                range.SetStyle(StyleExtra, new Regex(@"mousex"));
                range.SetStyle(StyleExtra, new Regex(@"mousey"));
                range.SetStyle(StyleExtra, new Regex(@"mousexp"));
                range.SetStyle(StyleExtra, new Regex(@"mouseyp"));
                range.SetStyle(StyleExtra, new Regex(@"mousestatex"));
                range.SetStyle(StyleExtra, new Regex(@"mousestatey"));
                range.SetStyle(StyleExtra, new Regex(@"dzx"));
                range.SetStyle(StyleExtra, new Regex(@"dzy"));
                range.SetStyle(StyleExtra, new Regex(@"viewpower1x"));
                range.SetStyle(StyleExtra, new Regex(@"viewpower2x"));
                range.SetStyle(StyleExtra, new Regex(@"viewpower3x"));
                range.SetStyle(StyleExtra, new Regex(@"viewpower1y"));
                range.SetStyle(StyleExtra, new Regex(@"viewpower2y"));
                range.SetStyle(StyleExtra, new Regex(@"viewpower3y"));
                range.SetStyle(StyleExtra, new Regex(@"viewpower05x"));
                range.SetStyle(StyleExtra, new Regex(@"viewpower05y"));
                range.SetStyle(StyleExtra, new Regex(@"task"));
                range.SetStyle(StyleExtra, new Regex(@"sleeptime"));
                range.SetStyle(StyleSpecial, new Regex(@"Task"));
                range.SetStyle(StyleSpecial, new Regex(@"Thread"));
                range.SetStyle(StyleSpecial, new Regex(@"Sleep"));
                range.SetStyle(StyleExtra, new Regex(@"StringToCode"));
                range.SetStyle(StyleSpecial, new Regex(@"FooClass"));
                range.SetStyle(StyleSpecial, new Regex(@"DllImport"));
                range.SetStyle(StyleLibrary, new Regex(@"Globalization"));
                range.SetStyle(StyleLibrary, new Regex(@"IO"));
                range.SetStyle(StyleLibrary, new Regex(@"Numerics"));
                range.SetStyle(StyleLibrary, new Regex(@"Runtime"));
                range.SetStyle(StyleLibrary, new Regex(@"InteropServices"));
                range.SetStyle(StyleLibrary, new Regex(@"Threading"));
                range.SetStyle(StyleLibrary, new Regex(@"Tasks"));
                range.SetStyle(StyleLibrary, new Regex(@"Windows"));
                range.SetStyle(StyleLibrary, new Regex(@"Forms"));
                range.SetStyle(StyleLibrary, new Regex(@"Reflection"));
                range.SetStyle(StyleLibrary, new Regex(@"Diagnostics"));
                range.SetStyle(StyleLibrary, new Regex(@"Collections"));
                range.SetStyle(StyleLibrary, new Regex(@"Generic"));
                range.SetStyle(StyleLibrary, new Regex(@"Linq"));
                range.SetStyle(StyleInput, new Regex(@"valListX.Count"));
                range.SetStyle(StyleInput, new Regex(@"valListX.Clear"));
                range.SetStyle(StyleInput, new Regex(@"valListX.RemoveAt"));
                range.SetStyle(StyleInput, new Regex(@"valListX.Add"));
                range.SetStyle(StyleInput, new Regex(@"valListX.Average"));
                range.SetStyle(StyleInput, new Regex(@"valListY.Count"));
                range.SetStyle(StyleInput, new Regex(@"valListY.Clear"));
                range.SetStyle(StyleInput, new Regex(@"valListY.RemoveAt"));
                range.SetStyle(StyleInput, new Regex(@"valListY.Add"));
                range.SetStyle(StyleInput, new Regex(@"valListY.Average"));
                range.SetStyle(StyleMethod, new Regex(@"ViewData"));
                range.SetStyle(StyleMethod, new Regex(@"Close"));
                range.SetStyle(StyleMethod, new Regex(@"BeginPolling"));
                range.SetStyle(StyleMethod, new Regex(@"BeginAsyncPolling"));
                range.SetStyle(StyleMethod, new Regex(@"Connect"));
                range.SetStyle(StyleMethod, new Regex(@"Disconnect"));
                range.SetStyle(StyleMethod, new Regex(@"SubmitReport1"));
                range.SetStyle(StyleMethod, new Regex(@"EnumerateControllers"));
                range.SetStyle(StyleMethod, new Regex(@"UnLoadKM"));
                range.SetStyle(StyleMethod, new Regex(@"SetKM"));
                range.SetStyle(StyleLibrary, new Regex(@"Valuechanges"));
                range.SetStyle(StyleClass, new Regex(@"Valuechange"));
                range.SetStyle(StyleObject, new Regex(@"ValueChange"));
                range.SetStyle(StyleMethod, new Regex(@"_ValueChange"));
                range.SetStyle(StyleLibrary, new Regex(@"controllers"));
                range.SetStyle(StyleClass, new Regex(@"ScpBus"));
                range.SetStyle(StyleObject, new Regex(@"scp"));
                range.SetStyle(StyleMethod, new Regex(@"LoadController"));
                range.SetStyle(StyleMethod, new Regex(@"UnLoadController"));
                range.SetStyle(StyleMethod, new Regex(@"SetController"));
                range.SetStyle(StyleLibrary, new Regex(@"XInputAPI"));
                range.SetStyle(StyleClass, new Regex(@"XInput"));
                range.SetStyle(StyleObject, new Regex(@"xi"));
                range.SetStyle(StyleMethod, new Regex(@"XInputHookConnect"));
                range.SetStyle(StyleLibrary, new Regex(@"controllersds4"));
                range.SetStyle(StyleClass, new Regex(@"DS4Controller"));
                range.SetStyle(StyleObject, new Regex(@"DS4"));
                range.SetStyle(StyleLibrary, new Regex(@"controllersvjoy"));
                range.SetStyle(StyleClass, new Regex(@"VJoyController"));
                range.SetStyle(StyleObject, new Regex(@"VJoy"));
                range.SetStyle(StyleLibrary, new Regex(@"DirectInputAPI"));
                range.SetStyle(StyleClass, new Regex(@"DirectInput"));
                range.SetStyle(StyleObject, new Regex(@"di"));
                range.SetStyle(StyleMethod, new Regex(@"DirectInputHookConnect"));
                range.SetStyle(StyleLibrary, new Regex(@"DualSenseAPI"));
                range.SetStyle(StyleClass, new Regex(@"DualSense"));
                range.SetStyle(StyleObject, new Regex(@"ds"));
                range.SetStyle(StyleMethod, new Regex(@"InitDualSenseAccel"));
                range.SetStyle(StyleLibrary, new Regex(@"DualShock4API"));
                range.SetStyle(StyleClass, new Regex(@"DualShock4"));
                range.SetStyle(StyleObject, new Regex(@"ds4"));
                range.SetStyle(StyleMethod, new Regex(@"InitDualShock4Accel"));
                range.SetStyle(StyleLibrary, new Regex(@"Interceptions"));
                range.SetStyle(StyleClass, new Regex(@"SendInterception"));
                range.SetStyle(StyleClass, new Regex(@"Input"));
                range.SetStyle(StyleObject, new Regex(@"si"));
                range.SetStyle(StyleObject, new Regex(@"input"));
                range.SetStyle(StyleMethod, new Regex(@"KeyboardFilterMode"));
                range.SetStyle(StyleMethod, new Regex(@"KeyboardFilterMode.All"));
                range.SetStyle(StyleMethod, new Regex(@"MouseFilterMode"));
                range.SetStyle(StyleMethod, new Regex(@"MouseFilterMode.All"));
                range.SetStyle(StyleMethod, new Regex(@"Load"));
                range.SetStyle(StyleLibrary, new Regex(@"WiiMoteAPI"));
                range.SetStyle(StyleClass, new Regex(@"WiiMote"));
                range.SetStyle(StyleObject, new Regex(@"wm"));
                range.SetStyle(StyleMethod, new Regex(@"Init"));
                range.SetStyle(StyleLibrary, new Regex(@"SwitchProControllerAPI"));
                range.SetStyle(StyleClass, new Regex(@"SwitchProController"));
                range.SetStyle(StyleObject, new Regex(@"spc"));
                range.SetStyle(StyleMethod, new Regex(@"InitProController"));
                range.SetStyle(StyleMethod, new Regex(@"InitProControllerAccel"));
                range.SetStyle(StyleMethod, new Regex(@"InitProControllerStick"));
                range.SetStyle(StyleLibrary, new Regex(@"JoyconChargingGripAPI"));
                range.SetStyle(StyleClass, new Regex(@"JoyconChargingGrip"));
                range.SetStyle(StyleObject, new Regex(@"jcg"));
                range.SetStyle(StyleMethod, new Regex(@"BeginAsyncPollingLeft"));
                range.SetStyle(StyleMethod, new Regex(@"BeginAsyncPollingRight"));
                range.SetStyle(StyleMethod, new Regex(@"InitLeftJoycon"));
                range.SetStyle(StyleMethod, new Regex(@"InitRightJoycon"));
                range.SetStyle(StyleMethod, new Regex(@"InitJoyconChargingGripAccel"));
                range.SetStyle(StyleMethod, new Regex(@"InitJoyconChargingGripStick"));
                range.SetStyle(StyleLibrary, new Regex(@"JoyconLeftAPI"));
                range.SetStyle(StyleClass, new Regex(@"JoyconLeft"));
                range.SetStyle(StyleObject, new Regex(@"jl"));
                range.SetStyle(StyleMethod, new Regex(@"InitLeftJoycon"));
                range.SetStyle(StyleMethod, new Regex(@"InitLeftJoyconAccel"));
                range.SetStyle(StyleMethod, new Regex(@"InitLeftJoyconStick"));
                range.SetStyle(StyleLibrary, new Regex(@"JoyconRightAPI"));
                range.SetStyle(StyleClass, new Regex(@"JoyconRight"));
                range.SetStyle(StyleObject, new Regex(@"jr"));
                range.SetStyle(StyleMethod, new Regex(@"InitRightJoycon"));
                range.SetStyle(StyleMethod, new Regex(@"InitRightJoyconAccel"));
                range.SetStyle(StyleMethod, new Regex(@"InitRightJoyconStick"));
                range.SetStyle(StyleLibrary, new Regex(@"KeyboardMouseInputAPI"));
                range.SetStyle(StyleClass, new Regex(@"KeyboardMouseInput"));
                range.SetStyle(StyleObject, new Regex(@"kmi"));
                range.SetStyle(StyleMethod, new Regex(@"BeginPollingMouse"));
                range.SetStyle(StyleMethod, new Regex(@"BeginPollingKeyboard"));
                range.SetStyle(StyleMethod, new Regex(@"MouseInputHookConnect"));
                range.SetStyle(StyleMethod, new Regex(@"KeyboardInputHookConnect"));
                range.SetStyle(StyleLibrary, new Regex(@"keyboards"));
                range.SetStyle(StyleClass, new Regex(@"SendKeyboard"));
                range.SetStyle(StyleObject, new Regex(@"sk"));
                range.SetStyle(StyleLibrary, new Regex(@"mouses"));
                range.SetStyle(StyleClass, new Regex(@"SendMouse"));
                range.SetStyle(StyleObject, new Regex(@"sm"));
                range.SetStyle(StyleInput, new Regex(@"getstate"));
                range.SetStyle(StyleInput, new Regex(@"System.Windows.Forms.Screen.PrimaryScreen.Bounds.Width"));
                range.SetStyle(StyleInput, new Regex(@"System.Windows.Forms.Screen.PrimaryScreen.Bounds.Height"));
                range.SetStyle(StyleInput, new Regex(@"Math.Abs"));
                range.SetStyle(StyleInput, new Regex(@"Math.Sign"));
                range.SetStyle(StyleInput, new Regex(@"Math.Round"));
                range.SetStyle(StyleInput, new Regex(@"Math.Pow"));
                range.SetStyle(StyleInput, new Regex(@"Math.Sqrt"));
                range.SetStyle(StyleInput, new Regex(@"Math.Log"));
                range.SetStyle(StyleInput, new Regex(@"Math.Exp"));
                range.SetStyle(StyleInput, new Regex(@"Math.Min"));
                range.SetStyle(StyleInput, new Regex(@"Math.Max"));
                range.SetStyle(StyleInput, new Regex(@"Math.Floor"));
                range.SetStyle(StyleInput, new Regex(@"Math.Truncate"));
                range.SetStyle(StyleInput, new Regex(@"wd"));
                range.SetStyle(StyleInput, new Regex(@"wu"));
                range.SetStyle(StyleInput, new Regex(@"valchanged"));
                range.SetStyle(StyleInput, new Regex(@"Scale"));
                range.SetStyle(StyleInput, new Regex(@"width"));
                range.SetStyle(StyleInput, new Regex(@"height"));
                range.SetStyle(StyleInput, new Regex(@"Controller1ButtonAPressed"));
                range.SetStyle(StyleInput, new Regex(@"Controller1ButtonBPressed"));
                range.SetStyle(StyleInput, new Regex(@"Controller1ButtonXPressed"));
                range.SetStyle(StyleInput, new Regex(@"Controller1ButtonYPressed"));
                range.SetStyle(StyleInput, new Regex(@"Controller1ButtonStartPressed"));
                range.SetStyle(StyleInput, new Regex(@"Controller1ButtonBackPressed"));
                range.SetStyle(StyleInput, new Regex(@"Controller1ButtonDownPressed"));
                range.SetStyle(StyleInput, new Regex(@"Controller1ButtonUpPresse"));
                range.SetStyle(StyleInput, new Regex(@"Controller1ButtonLeftPressed"));
                range.SetStyle(StyleInput, new Regex(@"Controller1ButtonRightPressed"));
                range.SetStyle(StyleInput, new Regex(@"Controller1ButtonShoulderLeftPressed"));
                range.SetStyle(StyleInput, new Regex(@"Controller1ButtonShoulderRightPressed"));
                range.SetStyle(StyleInput, new Regex(@"Controller1ThumbpadLeftPressed"));
                range.SetStyle(StyleInput, new Regex(@"Controller1ThumbpadRightPressed"));
                range.SetStyle(StyleInput, new Regex(@"Controller1TriggerLeftPosition"));
                range.SetStyle(StyleInput, new Regex(@"Controller1TriggerRightPosition"));
                range.SetStyle(StyleInput, new Regex(@"Controller1ThumbLeftX"));
                range.SetStyle(StyleInput, new Regex(@"Controller1ThumbLeftY"));
                range.SetStyle(StyleInput, new Regex(@"Controller1ThumbRightX"));
                range.SetStyle(StyleInput, new Regex(@"Controller1ThumbRightY"));
                range.SetStyle(StyleOutput, new Regex(@"controller1_send_xbox"));
                range.SetStyle(StyleOutput, new Regex(@"controller1_send_back"));
                range.SetStyle(StyleOutput, new Regex(@"controller1_send_start"));
                range.SetStyle(StyleOutput, new Regex(@"controller1_send_A"));
                range.SetStyle(StyleOutput, new Regex(@"controller1_send_B"));
                range.SetStyle(StyleOutput, new Regex(@"controller1_send_X"));
                range.SetStyle(StyleOutput, new Regex(@"controller1_send_Y"));
                range.SetStyle(StyleOutput, new Regex(@"controller1_send_up"));
                range.SetStyle(StyleOutput, new Regex(@"controller1_send_left"));
                range.SetStyle(StyleOutput, new Regex(@"controller1_send_down"));
                range.SetStyle(StyleOutput, new Regex(@"controller1_send_right"));
                range.SetStyle(StyleOutput, new Regex(@"controller1_send_leftstick"));
                range.SetStyle(StyleOutput, new Regex(@"controller1_send_rightstick"));
                range.SetStyle(StyleOutput, new Regex(@"controller1_send_leftbumper"));
                range.SetStyle(StyleOutput, new Regex(@"controller1_send_rightbumper"));
                range.SetStyle(StyleOutput, new Regex(@"controller1_send_lefttriggerposition"));
                range.SetStyle(StyleOutput, new Regex(@"controller1_send_righttriggerposition"));
                range.SetStyle(StyleOutput, new Regex(@"controller1_send_leftstickx"));
                range.SetStyle(StyleOutput, new Regex(@"controller1_send_leftsticky"));
                range.SetStyle(StyleOutput, new Regex(@"controller1_send_rightstickx"));
                range.SetStyle(StyleOutput, new Regex(@"controller1_send_rightsticky"));
                range.SetStyle(StyleOutput, new Regex(@"Controller1DS4_Send_Options"));
                range.SetStyle(StyleOutput, new Regex(@"Controller1DS4_Send_Option"));
                range.SetStyle(StyleOutput, new Regex(@"Controller1DS4_Send_ThumbLeft"));
                range.SetStyle(StyleOutput, new Regex(@"Controller1DS4_Send_ThumbRight"));
                range.SetStyle(StyleOutput, new Regex(@"Controller1DS4_Send_ShoulderLeft"));
                range.SetStyle(StyleOutput, new Regex(@"Controller1DS4_Send_ShoulderRight"));
                range.SetStyle(StyleOutput, new Regex(@"Controller1DS4_Send_Cross"));
                range.SetStyle(StyleOutput, new Regex(@"Controller1DS4_Send_Circle"));
                range.SetStyle(StyleOutput, new Regex(@"Controller1DS4_Send_Square"));
                range.SetStyle(StyleOutput, new Regex(@"Controller1DS4_Send_Triangle"));
                range.SetStyle(StyleOutput, new Regex(@"Controller1DS4_Send_Ps"));
                range.SetStyle(StyleOutput, new Regex(@"Controller1DS4_Send_Touchpad"));
                range.SetStyle(StyleOutput, new Regex(@"Controller1DS4_Send_Share"));
                range.SetStyle(StyleOutput, new Regex(@"Controller1DS4_Send_DPadUp"));
                range.SetStyle(StyleOutput, new Regex(@"Controller1DS4_Send_DPadDown"));
                range.SetStyle(StyleOutput, new Regex(@"Controller1DS4_Send_DPadLeft"));
                range.SetStyle(StyleOutput, new Regex(@"Controller1DS4_Send_DPadRight"));
                range.SetStyle(StyleOutput, new Regex(@"Controller1DS4_Send_LeftTrigger"));
                range.SetStyle(StyleOutput, new Regex(@"Controller1DS4_Send_RightTrigger"));
                range.SetStyle(StyleOutput, new Regex(@"Controller1DS4_Send_LeftTriggerPosition"));
                range.SetStyle(StyleOutput, new Regex(@"Controller1DS4_Send_RightTriggerPosition"));
                range.SetStyle(StyleOutput, new Regex(@"Controller1DS4_Send_LeftThumbX"));
                range.SetStyle(StyleOutput, new Regex(@"Controller1DS4_Send_RightThumbX"));
                range.SetStyle(StyleOutput, new Regex(@"Controller1DS4_Send_LeftThumbY"));
                range.SetStyle(StyleOutput, new Regex(@"Controller1DS4_Send_RightThumbY"));
                range.SetStyle(StyleOutput, new Regex(@"Controller1VJoy_Send_1"));
                range.SetStyle(StyleOutput, new Regex(@"Controller1VJoy_Send_2"));
                range.SetStyle(StyleOutput, new Regex(@"Controller1VJoy_Send_3"));
                range.SetStyle(StyleOutput, new Regex(@"Controller1VJoy_Send_4"));
                range.SetStyle(StyleOutput, new Regex(@"Controller1VJoy_Send_5"));
                range.SetStyle(StyleOutput, new Regex(@"Controller1VJoy_Send_6"));
                range.SetStyle(StyleOutput, new Regex(@"Controller1VJoy_Send_7"));
                range.SetStyle(StyleOutput, new Regex(@"Controller1VJoy_Send_8"));
                range.SetStyle(StyleOutput, new Regex(@"Controller1VJoy_Send_X"));
                range.SetStyle(StyleOutput, new Regex(@"Controller1VJoy_Send_Y"));
                range.SetStyle(StyleOutput, new Regex(@"Controller1VJoy_Send_Z"));
                range.SetStyle(StyleOutput, new Regex(@"Controller1VJoy_Send_WHL"));
                range.SetStyle(StyleOutput, new Regex(@"Controller1VJoy_Send_SL0"));
                range.SetStyle(StyleOutput, new Regex(@"Controller1VJoy_Send_SL1"));
                range.SetStyle(StyleOutput, new Regex(@"Controller1VJoy_Send_RX"));
                range.SetStyle(StyleOutput, new Regex(@"Controller1VJoy_Send_RY"));
                range.SetStyle(StyleOutput, new Regex(@"Controller1VJoy_Send_RZ"));
                range.SetStyle(StyleOutput, new Regex(@"Controller1VJoy_Send_POV"));
                range.SetStyle(StyleOutput, new Regex(@"Controller1VJoy_Send_Hat"));
                range.SetStyle(StyleOutput, new Regex(@"Controller1VJoy_Send_HatExt1"));
                range.SetStyle(StyleOutput, new Regex(@"Controller1VJoy_Send_HatExt2"));
                range.SetStyle(StyleOutput, new Regex(@"Controller1VJoy_Send_HatExt3"));
                range.SetStyle(StyleInput, new Regex(@"Joystick1AxisX"));
                range.SetStyle(StyleInput, new Regex(@"Joystick1AxisY"));
                range.SetStyle(StyleInput, new Regex(@"Joystick1AxisZ"));
                range.SetStyle(StyleInput, new Regex(@"Joystick1RotationX"));
                range.SetStyle(StyleInput, new Regex(@"Joystick1RotationY"));
                range.SetStyle(StyleInput, new Regex(@"Joystick1RotationZ"));
                range.SetStyle(StyleInput, new Regex(@"Joystick1Sliders0"));
                range.SetStyle(StyleInput, new Regex(@"Joystick1Sliders1"));
                range.SetStyle(StyleInput, new Regex(@"Joystick1PointOfViewControllers0"));
                range.SetStyle(StyleInput, new Regex(@"Joystick1PointOfViewControllers1"));
                range.SetStyle(StyleInput, new Regex(@"Joystick1PointOfViewControllers2"));
                range.SetStyle(StyleInput, new Regex(@"Joystick1PointOfViewControllers3"));
                range.SetStyle(StyleInput, new Regex(@"Joystick1VelocityX"));
                range.SetStyle(StyleInput, new Regex(@"Joystick1VelocityY"));
                range.SetStyle(StyleInput, new Regex(@"Joystick1VelocityZ"));
                range.SetStyle(StyleInput, new Regex(@"Joystick1AngularVelocityX"));
                range.SetStyle(StyleInput, new Regex(@"Joystick1AngularVelocityY"));
                range.SetStyle(StyleInput, new Regex(@"Joystick1AngularVelocityZ"));
                range.SetStyle(StyleInput, new Regex(@"Joystick1VelocitySliders0"));
                range.SetStyle(StyleInput, new Regex(@"Joystick1VelocitySliders1"));
                range.SetStyle(StyleInput, new Regex(@"Joystick1AccelerationX"));
                range.SetStyle(StyleInput, new Regex(@"Joystick1AccelerationY"));
                range.SetStyle(StyleInput, new Regex(@"Joystick1AccelerationZ"));
                range.SetStyle(StyleInput, new Regex(@"Joystick1AngularAccelerationX"));
                range.SetStyle(StyleInput, new Regex(@"Joystick1AngularAccelerationY"));
                range.SetStyle(StyleInput, new Regex(@"Joystick1AngularAccelerationZ"));
                range.SetStyle(StyleInput, new Regex(@"Joystick1AccelerationSliders0"));
                range.SetStyle(StyleInput, new Regex(@"Joystick1AccelerationSliders1"));
                range.SetStyle(StyleInput, new Regex(@"Joystick1ForceX"));
                range.SetStyle(StyleInput, new Regex(@"Joystick1ForceY"));
                range.SetStyle(StyleInput, new Regex(@"Joystick1ForceZ"));
                range.SetStyle(StyleInput, new Regex(@"Joystick1TorqueX"));
                range.SetStyle(StyleInput, new Regex(@"Joystick1TorqueY"));
                range.SetStyle(StyleInput, new Regex(@"Joystick1TorqueZ"));
                range.SetStyle(StyleInput, new Regex(@"Joystick1ForceSliders0"));
                range.SetStyle(StyleInput, new Regex(@"Joystick1ForceSliders1"));
                range.SetStyle(StyleInput, new Regex(@"Joystick1Buttons0"));
                range.SetStyle(StyleInput, new Regex(@"Joystick1Buttons1"));
                range.SetStyle(StyleInput, new Regex(@"Joystick1Buttons2"));
                range.SetStyle(StyleInput, new Regex(@"Joystick1Buttons3"));
                range.SetStyle(StyleInput, new Regex(@"Joystick1Buttons4"));
                range.SetStyle(StyleInput, new Regex(@"Joystick1Buttons5"));
                range.SetStyle(StyleInput, new Regex(@"Joystick1Buttons6"));
                range.SetStyle(StyleInput, new Regex(@"Joystick1Buttons7"));
                range.SetStyle(StyleInput, new Regex(@"Joystick1Buttons8"));
                range.SetStyle(StyleInput, new Regex(@"Joystick1Buttons9"));
                range.SetStyle(StyleInput, new Regex(@"Joystick1Buttons10"));
                range.SetStyle(StyleInput, new Regex(@"Joystick1Buttons11"));
                range.SetStyle(StyleInput, new Regex(@"Joystick1Buttons12"));
                range.SetStyle(StyleInput, new Regex(@"Joystick1Buttons13"));
                range.SetStyle(StyleInput, new Regex(@"Joystick1Buttons14"));
                range.SetStyle(StyleInput, new Regex(@"Joystick1Buttons15"));
                range.SetStyle(StyleInput, new Regex(@"Joystick1Buttons16"));
                range.SetStyle(StyleInput, new Regex(@"Joystick1Buttons17"));
                range.SetStyle(StyleInput, new Regex(@"Joystick1Buttons18"));
                range.SetStyle(StyleInput, new Regex(@"Joystick1Buttons19"));
                range.SetStyle(StyleInput, new Regex(@"Joystick1Buttons20"));
                range.SetStyle(StyleInput, new Regex(@"Joystick1Buttons21"));
                range.SetStyle(StyleInput, new Regex(@"Joystick1Buttons22"));
                range.SetStyle(StyleInput, new Regex(@"Joystick1Buttons23"));
                range.SetStyle(StyleInput, new Regex(@"Joystick1Buttons24"));
                range.SetStyle(StyleInput, new Regex(@"Joystick1Buttons25"));
                range.SetStyle(StyleInput, new Regex(@"Joystick1Buttons26"));
                range.SetStyle(StyleInput, new Regex(@"Joystick1Buttons27"));
                range.SetStyle(StyleInput, new Regex(@"Joystick1Buttons28"));
                range.SetStyle(StyleInput, new Regex(@"Joystick1Buttons29"));
                range.SetStyle(StyleInput, new Regex(@"Joystick1Buttons30"));
                range.SetStyle(StyleInput, new Regex(@"Joystick1Buttons31"));
                range.SetStyle(StyleInput, new Regex(@"Joystick1Buttons32"));
                range.SetStyle(StyleInput, new Regex(@"Joystick1Buttons33"));
                range.SetStyle(StyleInput, new Regex(@"Joystick1Buttons34"));
                range.SetStyle(StyleInput, new Regex(@"Joystick1Buttons35"));
                range.SetStyle(StyleInput, new Regex(@"Joystick1Buttons36"));
                range.SetStyle(StyleInput, new Regex(@"Joystick1Buttons37"));
                range.SetStyle(StyleInput, new Regex(@"Joystick1Buttons38"));
                range.SetStyle(StyleInput, new Regex(@"Joystick1Buttons39"));
                range.SetStyle(StyleInput, new Regex(@"Joystick1Buttons40"));
                range.SetStyle(StyleInput, new Regex(@"Joystick1Buttons41"));
                range.SetStyle(StyleInput, new Regex(@"Joystick1Buttons42"));
                range.SetStyle(StyleInput, new Regex(@"Joystick1Buttons43"));
                range.SetStyle(StyleInput, new Regex(@"Joystick1Buttons44"));
                range.SetStyle(StyleInput, new Regex(@"Joystick1Buttons45"));
                range.SetStyle(StyleInput, new Regex(@"Joystick1Buttons46"));
                range.SetStyle(StyleInput, new Regex(@"Joystick1Buttons47"));
                range.SetStyle(StyleInput, new Regex(@"Joystick1Buttons48"));
                range.SetStyle(StyleInput, new Regex(@"Joystick1Buttons49"));
                range.SetStyle(StyleInput, new Regex(@"Joystick1Buttons50"));
                range.SetStyle(StyleInput, new Regex(@"Joystick1Buttons51"));
                range.SetStyle(StyleInput, new Regex(@"Joystick1Buttons52"));
                range.SetStyle(StyleInput, new Regex(@"Joystick1Buttons53"));
                range.SetStyle(StyleInput, new Regex(@"Joystick1Buttons54"));
                range.SetStyle(StyleInput, new Regex(@"Joystick1Buttons55"));
                range.SetStyle(StyleInput, new Regex(@"Joystick1Buttons56"));
                range.SetStyle(StyleInput, new Regex(@"Joystick1Buttons57"));
                range.SetStyle(StyleInput, new Regex(@"Joystick1Buttons58"));
                range.SetStyle(StyleInput, new Regex(@"Joystick1Buttons59"));
                range.SetStyle(StyleInput, new Regex(@"Joystick1Buttons60"));
                range.SetStyle(StyleInput, new Regex(@"Joystick1Buttons61"));
                range.SetStyle(StyleInput, new Regex(@"Joystick1Buttons62"));
                range.SetStyle(StyleInput, new Regex(@"Joystick1Buttons63"));
                range.SetStyle(StyleInput, new Regex(@"Joystick1Buttons64"));
                range.SetStyle(StyleInput, new Regex(@"Joystick1Buttons65"));
                range.SetStyle(StyleInput, new Regex(@"Joystick1Buttons66"));
                range.SetStyle(StyleInput, new Regex(@"Joystick1Buttons67"));
                range.SetStyle(StyleInput, new Regex(@"Joystick1Buttons68"));
                range.SetStyle(StyleInput, new Regex(@"Joystick1Buttons69"));
                range.SetStyle(StyleInput, new Regex(@"Joystick1Buttons70"));
                range.SetStyle(StyleInput, new Regex(@"Joystick1Buttons71"));
                range.SetStyle(StyleInput, new Regex(@"Joystick1Buttons72"));
                range.SetStyle(StyleInput, new Regex(@"Joystick1Buttons73"));
                range.SetStyle(StyleInput, new Regex(@"Joystick1Buttons74"));
                range.SetStyle(StyleInput, new Regex(@"Joystick1Buttons75"));
                range.SetStyle(StyleInput, new Regex(@"Joystick1Buttons76"));
                range.SetStyle(StyleInput, new Regex(@"Joystick1Buttons77"));
                range.SetStyle(StyleInput, new Regex(@"Joystick1Buttons78"));
                range.SetStyle(StyleInput, new Regex(@"Joystick1Buttons79"));
                range.SetStyle(StyleInput, new Regex(@"Joystick1Buttons80"));
                range.SetStyle(StyleInput, new Regex(@"Joystick1Buttons81"));
                range.SetStyle(StyleInput, new Regex(@"Joystick1Buttons82"));
                range.SetStyle(StyleInput, new Regex(@"Joystick1Buttons83"));
                range.SetStyle(StyleInput, new Regex(@"Joystick1Buttons84"));
                range.SetStyle(StyleInput, new Regex(@"Joystick1Buttons85"));
                range.SetStyle(StyleInput, new Regex(@"Joystick1Buttons86"));
                range.SetStyle(StyleInput, new Regex(@"Joystick1Buttons87"));
                range.SetStyle(StyleInput, new Regex(@"Joystick1Buttons88"));
                range.SetStyle(StyleInput, new Regex(@"Joystick1Buttons89"));
                range.SetStyle(StyleInput, new Regex(@"Joystick1Buttons90"));
                range.SetStyle(StyleInput, new Regex(@"Joystick1Buttons91"));
                range.SetStyle(StyleInput, new Regex(@"Joystick1Buttons92"));
                range.SetStyle(StyleInput, new Regex(@"Joystick1Buttons93"));
                range.SetStyle(StyleInput, new Regex(@"Joystick1Buttons94"));
                range.SetStyle(StyleInput, new Regex(@"Joystick1Buttons95"));
                range.SetStyle(StyleInput, new Regex(@"Joystick1Buttons96"));
                range.SetStyle(StyleInput, new Regex(@"Joystick1Buttons97"));
                range.SetStyle(StyleInput, new Regex(@"Joystick1Buttons98"));
                range.SetStyle(StyleInput, new Regex(@"Joystick1Buttons99"));
                range.SetStyle(StyleInput, new Regex(@"Joystick1Buttons100"));
                range.SetStyle(StyleInput, new Regex(@"Joystick1Buttons101"));
                range.SetStyle(StyleInput, new Regex(@"Joystick1Buttons102"));
                range.SetStyle(StyleInput, new Regex(@"Joystick1Buttons103"));
                range.SetStyle(StyleInput, new Regex(@"Joystick1Buttons104"));
                range.SetStyle(StyleInput, new Regex(@"Joystick1Buttons105"));
                range.SetStyle(StyleInput, new Regex(@"Joystick1Buttons106"));
                range.SetStyle(StyleInput, new Regex(@"Joystick1Buttons107"));
                range.SetStyle(StyleInput, new Regex(@"Joystick1Buttons108"));
                range.SetStyle(StyleInput, new Regex(@"Joystick1Buttons109"));
                range.SetStyle(StyleInput, new Regex(@"Joystick1Buttons110"));
                range.SetStyle(StyleInput, new Regex(@"Joystick1Buttons111"));
                range.SetStyle(StyleInput, new Regex(@"Joystick1Buttons112"));
                range.SetStyle(StyleInput, new Regex(@"Joystick1Buttons113"));
                range.SetStyle(StyleInput, new Regex(@"Joystick1Buttons114"));
                range.SetStyle(StyleInput, new Regex(@"Joystick1Buttons115"));
                range.SetStyle(StyleInput, new Regex(@"Joystick1Buttons116"));
                range.SetStyle(StyleInput, new Regex(@"Joystick1Buttons117"));
                range.SetStyle(StyleInput, new Regex(@"Joystick1Buttons118"));
                range.SetStyle(StyleInput, new Regex(@"Joystick1Buttons119"));
                range.SetStyle(StyleInput, new Regex(@"Joystick1Buttons120"));
                range.SetStyle(StyleInput, new Regex(@"Joystick1Buttons121"));
                range.SetStyle(StyleInput, new Regex(@"Joystick1Buttons122"));
                range.SetStyle(StyleInput, new Regex(@"Joystick1Buttons123"));
                range.SetStyle(StyleInput, new Regex(@"Joystick1Buttons124"));
                range.SetStyle(StyleInput, new Regex(@"Joystick1Buttons125"));
                range.SetStyle(StyleInput, new Regex(@"Joystick1Buttons126"));
                range.SetStyle(StyleInput, new Regex(@"Joystick1Buttons127"));
                range.SetStyle(StyleInput, new Regex(@"PS5ControllerLeftStickX"));
                range.SetStyle(StyleInput, new Regex(@"PS5ControllerLeftStickY"));
                range.SetStyle(StyleInput, new Regex(@"PS5ControllerRightStickX"));
                range.SetStyle(StyleInput, new Regex(@"PS5ControllerRightStickY"));
                range.SetStyle(StyleInput, new Regex(@"PS5ControllerLeftTriggerPosition"));
                range.SetStyle(StyleInput, new Regex(@"PS5ControllerRightTriggerPosition"));
                range.SetStyle(StyleInput, new Regex(@"PS5ControllerTouchX"));
                range.SetStyle(StyleInput, new Regex(@"PS5ControllerTouchY"));
                range.SetStyle(StyleInput, new Regex(@"PS5ControllerTouchOn"));
                range.SetStyle(StyleInput, new Regex(@"PS5ControllerGyroX"));
                range.SetStyle(StyleInput, new Regex(@"PS5ControllerGyroY"));
                range.SetStyle(StyleInput, new Regex(@"PS5ControllerAccelX"));
                range.SetStyle(StyleInput, new Regex(@"PS5ControllerAccelY"));
                range.SetStyle(StyleInput, new Regex(@"PS5ControllerButtonCrossPressed"));
                range.SetStyle(StyleInput, new Regex(@"PS5ControllerButtonCirclePressed"));
                range.SetStyle(StyleInput, new Regex(@"PS5ControllerButtonSquarePressed"));
                range.SetStyle(StyleInput, new Regex(@"PS5ControllerButtonTrianglePressed"));
                range.SetStyle(StyleInput, new Regex(@"PS5ControllerButtonDPadUpPressed"));
                range.SetStyle(StyleInput, new Regex(@"PS5ControllerButtonDPadRightPressed"));
                range.SetStyle(StyleInput, new Regex(@"PS5ControllerButtonDPadDownPressed"));
                range.SetStyle(StyleInput, new Regex(@"PS5ControllerButtonDPadLeftPressed"));
                range.SetStyle(StyleInput, new Regex(@"PS5ControllerButtonL1Pressed"));
                range.SetStyle(StyleInput, new Regex(@"PS5ControllerButtonR1Pressed"));
                range.SetStyle(StyleInput, new Regex(@"PS5ControllerButtonL2Pressed"));
                range.SetStyle(StyleInput, new Regex(@"PS5ControllerButtonR2Pressed"));
                range.SetStyle(StyleInput, new Regex(@"PS5ControllerButtonL3Pressed"));
                range.SetStyle(StyleInput, new Regex(@"PS5ControllerButtonR3Pressed"));
                range.SetStyle(StyleInput, new Regex(@"PS5ControllerButtonCreatePressed"));
                range.SetStyle(StyleInput, new Regex(@"PS5ControllerButtonMenuPressed"));
                range.SetStyle(StyleInput, new Regex(@"PS5ControllerButtonLogoPressed"));
                range.SetStyle(StyleInput, new Regex(@"PS5ControllerButtonTouchpadPressed"));
                range.SetStyle(StyleInput, new Regex(@"PS5ControllerButtonFnLPressed"));
                range.SetStyle(StyleInput, new Regex(@"PS5ControllerButtonFnRPressed"));
                range.SetStyle(StyleInput, new Regex(@"PS5ControllerButtonBLPPressed"));
                range.SetStyle(StyleInput, new Regex(@"PS5ControllerButtonBRPPressed"));
                range.SetStyle(StyleInput, new Regex(@"PS5ControllerButtonMicPressed"));
                range.SetStyle(StyleInput, new Regex(@"PS4ControllerLeftStickX"));
                range.SetStyle(StyleInput, new Regex(@"PS4ControllerLeftStickY"));
                range.SetStyle(StyleInput, new Regex(@"PS4ControllerRightStickX"));
                range.SetStyle(StyleInput, new Regex(@"PS4ControllerRightStickY"));
                range.SetStyle(StyleInput, new Regex(@"PS4ControllerLeftTriggerPosition"));
                range.SetStyle(StyleInput, new Regex(@"PS4ControllerRightTriggerPosition"));
                range.SetStyle(StyleInput, new Regex(@"PS4ControllerTouchX"));
                range.SetStyle(StyleInput, new Regex(@"PS4ControllerTouchY"));
                range.SetStyle(StyleInput, new Regex(@"PS4ControllerTouchOn"));
                range.SetStyle(StyleInput, new Regex(@"PS4ControllerGyroX"));
                range.SetStyle(StyleInput, new Regex(@"PS4ControllerGyroY"));
                range.SetStyle(StyleInput, new Regex(@"PS4ControllerAccelX"));
                range.SetStyle(StyleInput, new Regex(@"PS4ControllerAccelY"));
                range.SetStyle(StyleInput, new Regex(@"PS4ControllerButtonCrossPressed"));
                range.SetStyle(StyleInput, new Regex(@"PS4ControllerButtonCirclePressed"));
                range.SetStyle(StyleInput, new Regex(@"PS4ControllerButtonSquarePressed"));
                range.SetStyle(StyleInput, new Regex(@"PS4ControllerButtonTrianglePressed"));
                range.SetStyle(StyleInput, new Regex(@"PS4ControllerButtonDPadUpPressed"));
                range.SetStyle(StyleInput, new Regex(@"PS4ControllerButtonDPadRightPressed"));
                range.SetStyle(StyleInput, new Regex(@"PS4ControllerButtonDPadDownPressed"));
                range.SetStyle(StyleInput, new Regex(@"PS4ControllerButtonDPadLeftPressed"));
                range.SetStyle(StyleInput, new Regex(@"PS4ControllerButtonL1Pressed"));
                range.SetStyle(StyleInput, new Regex(@"PS4ControllerButtonR1Pressed"));
                range.SetStyle(StyleInput, new Regex(@"PS4ControllerButtonL2Pressed"));
                range.SetStyle(StyleInput, new Regex(@"PS4ControllerButtonR2Pressed"));
                range.SetStyle(StyleInput, new Regex(@"PS4ControllerButtonL3Pressed"));
                range.SetStyle(StyleInput, new Regex(@"PS4ControllerButtonR3Pressed"));
                range.SetStyle(StyleInput, new Regex(@"PS4ControllerButtonCreatePressed"));
                range.SetStyle(StyleInput, new Regex(@"PS4ControllerButtonMenuPressed"));
                range.SetStyle(StyleInput, new Regex(@"PS4ControllerButtonLogoPressed"));
                range.SetStyle(StyleInput, new Regex(@"PS4ControllerButtonTouchpadPressed"));
                range.SetStyle(StyleInput, new Regex(@"PS4ControllerButtonMicPressed"));
                range.SetStyle(StyleOutput, new Regex(@"keyboard_1_id"));
                range.SetStyle(StyleOutput, new Regex(@"mouse_1_id"));
                range.SetStyle(StyleOutput, new Regex(@"int_1_deltaX"));
                range.SetStyle(StyleOutput, new Regex(@"int_1_deltaY"));
                range.SetStyle(StyleOutput, new Regex(@"int_1_x"));
                range.SetStyle(StyleOutput, new Regex(@"int_1_y"));
                range.SetStyle(StyleOutput, new Regex(@"int_1_SendLeftClick"));
                range.SetStyle(StyleOutput, new Regex(@"int_1_SendRightClick"));
                range.SetStyle(StyleOutput, new Regex(@"int_1_SendMiddleClick"));
                range.SetStyle(StyleOutput, new Regex(@"int_1_SendWheelUp"));
                range.SetStyle(StyleOutput, new Regex(@"int_1_SendWheelDown"));
                range.SetStyle(StyleOutput, new Regex(@"int_1_SendCANCEL"));
                range.SetStyle(StyleOutput, new Regex(@"int_1_SendBACK"));
                range.SetStyle(StyleOutput, new Regex(@"int_1_SendTAB"));
                range.SetStyle(StyleOutput, new Regex(@"int_1_SendCLEAR"));
                range.SetStyle(StyleOutput, new Regex(@"int_1_SendRETURN"));
                range.SetStyle(StyleOutput, new Regex(@"int_1_SendSHIFT"));
                range.SetStyle(StyleOutput, new Regex(@"int_1_SendCONTROL"));
                range.SetStyle(StyleOutput, new Regex(@"int_1_SendMENU"));
                range.SetStyle(StyleOutput, new Regex(@"int_1_SendCAPITAL"));
                range.SetStyle(StyleOutput, new Regex(@"int_1_SendESCAPE"));
                range.SetStyle(StyleOutput, new Regex(@"int_1_SendSPACE"));
                range.SetStyle(StyleOutput, new Regex(@"int_1_SendPRIOR"));
                range.SetStyle(StyleOutput, new Regex(@"int_1_SendNEXT"));
                range.SetStyle(StyleOutput, new Regex(@"int_1_SendEND"));
                range.SetStyle(StyleOutput, new Regex(@"int_1_SendHOME"));
                range.SetStyle(StyleOutput, new Regex(@"int_1_SendLEFT"));
                range.SetStyle(StyleOutput, new Regex(@"int_1_SendUP"));
                range.SetStyle(StyleOutput, new Regex(@"int_1_SendRIGHT"));
                range.SetStyle(StyleOutput, new Regex(@"int_1_SendDOWN"));
                range.SetStyle(StyleOutput, new Regex(@"int_1_SendSNAPSHOT"));
                range.SetStyle(StyleOutput, new Regex(@"int_1_SendINSERT"));
                range.SetStyle(StyleOutput, new Regex(@"int_1_SendNUMPADDEL"));
                range.SetStyle(StyleOutput, new Regex(@"int_1_SendNUMPADINSERT"));
                range.SetStyle(StyleOutput, new Regex(@"int_1_SendHELP"));
                range.SetStyle(StyleOutput, new Regex(@"int_1_SendAPOSTROPHE"));
                range.SetStyle(StyleOutput, new Regex(@"int_1_SendBACKSPACE"));
                range.SetStyle(StyleOutput, new Regex(@"int_1_SendPAGEDOWN"));
                range.SetStyle(StyleOutput, new Regex(@"int_1_SendPAGEUP"));
                range.SetStyle(StyleOutput, new Regex(@"int_1_SendFIN"));
                range.SetStyle(StyleOutput, new Regex(@"int_1_SendMOUSE"));
                range.SetStyle(StyleOutput, new Regex(@"int_1_SendA"));
                range.SetStyle(StyleOutput, new Regex(@"int_1_SendB"));
                range.SetStyle(StyleOutput, new Regex(@"int_1_SendC"));
                range.SetStyle(StyleOutput, new Regex(@"int_1_SendD"));
                range.SetStyle(StyleOutput, new Regex(@"int_1_SendE"));
                range.SetStyle(StyleOutput, new Regex(@"int_1_SendF"));
                range.SetStyle(StyleOutput, new Regex(@"int_1_SendG"));
                range.SetStyle(StyleOutput, new Regex(@"int_1_SendH"));
                range.SetStyle(StyleOutput, new Regex(@"int_1_SendI"));
                range.SetStyle(StyleOutput, new Regex(@"int_1_SendJ"));
                range.SetStyle(StyleOutput, new Regex(@"int_1_SendK"));
                range.SetStyle(StyleOutput, new Regex(@"int_1_SendL"));
                range.SetStyle(StyleOutput, new Regex(@"int_1_SendM"));
                range.SetStyle(StyleOutput, new Regex(@"int_1_SendN"));
                range.SetStyle(StyleOutput, new Regex(@"int_1_SendO"));
                range.SetStyle(StyleOutput, new Regex(@"int_1_SendP"));
                range.SetStyle(StyleOutput, new Regex(@"int_1_SendQ"));
                range.SetStyle(StyleOutput, new Regex(@"int_1_SendR"));
                range.SetStyle(StyleOutput, new Regex(@"int_1_SendS"));
                range.SetStyle(StyleOutput, new Regex(@"int_1_SendT"));
                range.SetStyle(StyleOutput, new Regex(@"int_1_SendU"));
                range.SetStyle(StyleOutput, new Regex(@"int_1_SendV"));
                range.SetStyle(StyleOutput, new Regex(@"int_1_SendW"));
                range.SetStyle(StyleOutput, new Regex(@"int_1_SendX"));
                range.SetStyle(StyleOutput, new Regex(@"int_1_SendY"));
                range.SetStyle(StyleOutput, new Regex(@"int_1_SendZ"));
                range.SetStyle(StyleOutput, new Regex(@"int_1_SendLWIN"));
                range.SetStyle(StyleOutput, new Regex(@"int_1_SendRWIN"));
                range.SetStyle(StyleOutput, new Regex(@"int_1_SendAPPS"));
                range.SetStyle(StyleOutput, new Regex(@"int_1_SendDELETE"));
                range.SetStyle(StyleOutput, new Regex(@"int_1_SendNUMPAD0"));
                range.SetStyle(StyleOutput, new Regex(@"int_1_SendNUMPAD1"));
                range.SetStyle(StyleOutput, new Regex(@"int_1_SendNUMPAD2"));
                range.SetStyle(StyleOutput, new Regex(@"int_1_SendNUMPAD3"));
                range.SetStyle(StyleOutput, new Regex(@"int_1_SendNUMPAD4"));
                range.SetStyle(StyleOutput, new Regex(@"int_1_SendNUMPAD5"));
                range.SetStyle(StyleOutput, new Regex(@"int_1_SendNUMPAD6"));
                range.SetStyle(StyleOutput, new Regex(@"int_1_SendNUMPAD7"));
                range.SetStyle(StyleOutput, new Regex(@"int_1_SendNUMPAD8"));
                range.SetStyle(StyleOutput, new Regex(@"int_1_SendNUMPAD9"));
                range.SetStyle(StyleOutput, new Regex(@"int_1_SendMULTIPLY"));
                range.SetStyle(StyleOutput, new Regex(@"int_1_SendADD"));
                range.SetStyle(StyleOutput, new Regex(@"int_1_SendSUBTRACT"));
                range.SetStyle(StyleOutput, new Regex(@"int_1_SendDECIMAL"));
                range.SetStyle(StyleOutput, new Regex(@"int_1_SendPRINTSCREEN"));
                range.SetStyle(StyleOutput, new Regex(@"int_1_SendDIVIDE"));
                range.SetStyle(StyleOutput, new Regex(@"int_1_SendF1"));
                range.SetStyle(StyleOutput, new Regex(@"int_1_SendF2"));
                range.SetStyle(StyleOutput, new Regex(@"int_1_SendF3"));
                range.SetStyle(StyleOutput, new Regex(@"int_1_SendF4"));
                range.SetStyle(StyleOutput, new Regex(@"int_1_SendF5"));
                range.SetStyle(StyleOutput, new Regex(@"int_1_SendF6"));
                range.SetStyle(StyleOutput, new Regex(@"int_1_SendF7"));
                range.SetStyle(StyleOutput, new Regex(@"int_1_SendF8"));
                range.SetStyle(StyleOutput, new Regex(@"int_1_SendF9"));
                range.SetStyle(StyleOutput, new Regex(@"int_1_SendF10"));
                range.SetStyle(StyleOutput, new Regex(@"int_1_SendF11"));
                range.SetStyle(StyleOutput, new Regex(@"int_1_SendF12"));
                range.SetStyle(StyleOutput, new Regex(@"int_1_SendNUMLOCK"));
                range.SetStyle(StyleOutput, new Regex(@"int_1_SendSCROLLLOCK"));
                range.SetStyle(StyleOutput, new Regex(@"int_1_SendLEFTSHIFT"));
                range.SetStyle(StyleOutput, new Regex(@"int_1_SendRIGHTSHIFT"));
                range.SetStyle(StyleOutput, new Regex(@"int_1_SendLEFTCONTROL"));
                range.SetStyle(StyleOutput, new Regex(@"int_1_SendRIGHTCONTROL"));
                range.SetStyle(StyleOutput, new Regex(@"int_1_SendLEFTALT"));
                range.SetStyle(StyleOutput, new Regex(@"int_1_SendRIGHTALT"));
                range.SetStyle(StyleOutput, new Regex(@"int_1_SendBROWSER_BACK"));
                range.SetStyle(StyleOutput, new Regex(@"int_1_SendBROWSER_FORWARD"));
                range.SetStyle(StyleOutput, new Regex(@"int_1_SendBROWSER_REFRESH"));
                range.SetStyle(StyleOutput, new Regex(@"int_1_SendBROWSER_STOP"));
                range.SetStyle(StyleOutput, new Regex(@"int_1_SendBROWSER_SEARCH"));
                range.SetStyle(StyleOutput, new Regex(@"int_1_SendBROWSER_FAVORITES"));
                range.SetStyle(StyleOutput, new Regex(@"int_1_SendBROWSER_HOME"));
                range.SetStyle(StyleOutput, new Regex(@"int_1_SendVOLUME_MUTE"));
                range.SetStyle(StyleOutput, new Regex(@"int_1_SendVOLUME_DOWN"));
                range.SetStyle(StyleOutput, new Regex(@"int_1_SendVOLUME_UP"));
                range.SetStyle(StyleOutput, new Regex(@"int_1_SendMEDIA_NEXT_TRACK"));
                range.SetStyle(StyleOutput, new Regex(@"int_1_SendMEDIA_PREV_TRACK"));
                range.SetStyle(StyleOutput, new Regex(@"int_1_SendMEDIA_STOP"));
                range.SetStyle(StyleOutput, new Regex(@"int_1_SendMEDIA_PLAY_PAUSE"));
                range.SetStyle(StyleOutput, new Regex(@"int_1_SendLAUNCH_MAIL"));
                range.SetStyle(StyleOutput, new Regex(@"int_1_SendLAUNCH_MEDIA_SELECT"));
                range.SetStyle(StyleOutput, new Regex(@"int_1_SendLAUNCH_APP1"));
                range.SetStyle(StyleOutput, new Regex(@"int_1_SendLAUNCH_APP2"));
                range.SetStyle(StyleOutput, new Regex(@"int_1_SendOEM_1"));
                range.SetStyle(StyleOutput, new Regex(@"int_1_SendOEM_PLUS"));
                range.SetStyle(StyleOutput, new Regex(@"int_1_SendOEM_COMMA"));
                range.SetStyle(StyleOutput, new Regex(@"int_1_SendOEM_MINUS"));
                range.SetStyle(StyleOutput, new Regex(@"int_1_SendOEM_PERIOD"));
                range.SetStyle(StyleOutput, new Regex(@"int_1_SendOEM_2"));
                range.SetStyle(StyleOutput, new Regex(@"int_1_SendOEM_3"));
                range.SetStyle(StyleOutput, new Regex(@"int_1_SendOEM_4"));
                range.SetStyle(StyleOutput, new Regex(@"int_1_SendOEM_5"));
                range.SetStyle(StyleOutput, new Regex(@"int_1_SendOEM_6"));
                range.SetStyle(StyleOutput, new Regex(@"int_1_SendOEM_7"));
                range.SetStyle(StyleOutput, new Regex(@"int_1_SendOEM_8"));
                range.SetStyle(StyleOutput, new Regex(@"int_1_SendOEM_102"));
                range.SetStyle(StyleOutput, new Regex(@"int_1_SendEREOF"));
                range.SetStyle(StyleOutput, new Regex(@"int_1_SendZOOM"));
                range.SetStyle(StyleOutput, new Regex(@"int_1_SendEscape"));
                range.SetStyle(StyleOutput, new Regex(@"int_1_SendOne"));
                range.SetStyle(StyleOutput, new Regex(@"int_1_SendTwo"));
                range.SetStyle(StyleOutput, new Regex(@"int_1_SendThree"));
                range.SetStyle(StyleOutput, new Regex(@"int_1_SendFour"));
                range.SetStyle(StyleOutput, new Regex(@"int_1_SendFive"));
                range.SetStyle(StyleOutput, new Regex(@"int_1_SendSix"));
                range.SetStyle(StyleOutput, new Regex(@"int_1_SendSeven"));
                range.SetStyle(StyleOutput, new Regex(@"int_1_SendEight"));
                range.SetStyle(StyleOutput, new Regex(@"int_1_SendNine"));
                range.SetStyle(StyleOutput, new Regex(@"int_1_SendZero"));
                range.SetStyle(StyleOutput, new Regex(@"int_1_SendDashUnderscore"));
                range.SetStyle(StyleOutput, new Regex(@"int_1_SendPlusEquals"));
                range.SetStyle(StyleOutput, new Regex(@"int_1_SendBackspace"));
                range.SetStyle(StyleOutput, new Regex(@"int_1_SendTab"));
                range.SetStyle(StyleOutput, new Regex(@"int_1_SendOpenBracketBrace"));
                range.SetStyle(StyleOutput, new Regex(@"int_1_SendCloseBracketBrace"));
                range.SetStyle(StyleOutput, new Regex(@"int_1_SendEnter"));
                range.SetStyle(StyleOutput, new Regex(@"int_1_SendControl"));
                range.SetStyle(StyleOutput, new Regex(@"int_1_SendSemicolonColon"));
                range.SetStyle(StyleOutput, new Regex(@"int_1_SendSingleDoubleQuote"));
                range.SetStyle(StyleOutput, new Regex(@"int_1_SendTilde"));
                range.SetStyle(StyleOutput, new Regex(@"int_1_SendLeftShift"));
                range.SetStyle(StyleOutput, new Regex(@"int_1_SendBackslashPipe"));
                range.SetStyle(StyleOutput, new Regex(@"int_1_SendCommaLeftArrow"));
                range.SetStyle(StyleOutput, new Regex(@"int_1_SendPeriodRightArrow"));
                range.SetStyle(StyleOutput, new Regex(@"int_1_SendForwardSlashQuestionMark"));
                range.SetStyle(StyleOutput, new Regex(@"int_1_SendRightShift"));
                range.SetStyle(StyleOutput, new Regex(@"int_1_SendRightAlt"));
                range.SetStyle(StyleOutput, new Regex(@"int_1_SendSpace"));
                range.SetStyle(StyleOutput, new Regex(@"int_1_SendCapsLock"));
                range.SetStyle(StyleOutput, new Regex(@"int_1_SendUp"));
                range.SetStyle(StyleOutput, new Regex(@"int_1_SendDown"));
                range.SetStyle(StyleOutput, new Regex(@"int_1_SendRight"));
                range.SetStyle(StyleOutput, new Regex(@"int_1_SendLeft"));
                range.SetStyle(StyleOutput, new Regex(@"int_1_SendHome"));
                range.SetStyle(StyleOutput, new Regex(@"int_1_SendEnd"));
                range.SetStyle(StyleOutput, new Regex(@"int_1_SendDelete"));
                range.SetStyle(StyleOutput, new Regex(@"int_1_SendPageUp"));
                range.SetStyle(StyleOutput, new Regex(@"int_1_SendPageDown"));
                range.SetStyle(StyleOutput, new Regex(@"int_1_SendInsert"));
                range.SetStyle(StyleOutput, new Regex(@"int_1_SendPrintScreen"));
                range.SetStyle(StyleOutput, new Regex(@"int_1_SendNumLock"));
                range.SetStyle(StyleOutput, new Regex(@"int_1_SendScrollLock"));
                range.SetStyle(StyleOutput, new Regex(@"int_1_SendMenu"));
                range.SetStyle(StyleOutput, new Regex(@"int_1_SendWindowsKey"));
                range.SetStyle(StyleOutput, new Regex(@"int_1_SendNumpadDivide"));
                range.SetStyle(StyleOutput, new Regex(@"int_1_SendNumpadAsterisk"));
                range.SetStyle(StyleOutput, new Regex(@"int_1_SendNumpad7"));
                range.SetStyle(StyleOutput, new Regex(@"int_1_SendNumpad8"));
                range.SetStyle(StyleOutput, new Regex(@"int_1_SendNumpad9"));
                range.SetStyle(StyleOutput, new Regex(@"int_1_SendNumpad4"));
                range.SetStyle(StyleOutput, new Regex(@"int_1_SendNumpad5"));
                range.SetStyle(StyleOutput, new Regex(@"int_1_SendNumpad6"));
                range.SetStyle(StyleOutput, new Regex(@"int_1_SendNumpad1"));
                range.SetStyle(StyleOutput, new Regex(@"int_1_SendNumpad2"));
                range.SetStyle(StyleOutput, new Regex(@"int_1_SendNumpad3"));
                range.SetStyle(StyleOutput, new Regex(@"int_1_SendNumpad0"));
                range.SetStyle(StyleOutput, new Regex(@"int_1_SendNumpadDelete"));
                range.SetStyle(StyleOutput, new Regex(@"int_1_SendNumpadEnter"));
                range.SetStyle(StyleOutput, new Regex(@"int_1_SendNumpadPlus"));
                range.SetStyle(StyleOutput, new Regex(@"int_1_SendNumpadMinus"));
                range.SetStyle(StyleInput, new Regex(@"irx"));
                range.SetStyle(StyleInput, new Regex(@"iry"));
                range.SetStyle(StyleInput, new Regex(@"WiimoteButtonStateA"));
                range.SetStyle(StyleInput, new Regex(@"WiimoteButtonStateB"));
                range.SetStyle(StyleInput, new Regex(@"WiimoteButtonStateMinus"));
                range.SetStyle(StyleInput, new Regex(@"WiimoteButtonStateHome"));
                range.SetStyle(StyleInput, new Regex(@"WiimoteButtonStatePlus"));
                range.SetStyle(StyleInput, new Regex(@"WiimoteButtonStateOne"));
                range.SetStyle(StyleInput, new Regex(@"WiimoteButtonStateTwo"));
                range.SetStyle(StyleInput, new Regex(@"WiimoteButtonStateUp"));
                range.SetStyle(StyleInput, new Regex(@"WiimoteButtonStateDown"));
                range.SetStyle(StyleInput, new Regex(@"WiimoteButtonStateLeft"));
                range.SetStyle(StyleInput, new Regex(@"WiimoteButtonStateRight"));
                range.SetStyle(StyleInput, new Regex(@"WiimoteRawValuesX"));
                range.SetStyle(StyleInput, new Regex(@"WiimoteRawValuesY"));
                range.SetStyle(StyleInput, new Regex(@"WiimoteRawValuesZ"));
                range.SetStyle(StyleInput, new Regex(@"WiimoteNunchuckStateRawJoystickX"));
                range.SetStyle(StyleInput, new Regex(@"WiimoteNunchuckStateRawJoystickY"));
                range.SetStyle(StyleInput, new Regex(@"WiimoteNunchuckStateRawValuesX"));
                range.SetStyle(StyleInput, new Regex(@"WiimoteNunchuckStateRawValuesY"));
                range.SetStyle(StyleInput, new Regex(@"WiimoteNunchuckStateRawValuesZ"));
                range.SetStyle(StyleInput, new Regex(@"WiimoteNunchuckStateC"));
                range.SetStyle(StyleInput, new Regex(@"WiimoteNunchuckStateZ"));
                range.SetStyle(StyleInput, new Regex(@"JoyconRightStickX"));
                range.SetStyle(StyleInput, new Regex(@"JoyconRightStickY"));
                range.SetStyle(StyleInput, new Regex(@"JoyconRightButtonSHOULDER_1"));
                range.SetStyle(StyleInput, new Regex(@"JoyconRightButtonSHOULDER_2"));
                range.SetStyle(StyleInput, new Regex(@"JoyconRightButtonSR"));
                range.SetStyle(StyleInput, new Regex(@"JoyconRightButtonSL"));
                range.SetStyle(StyleInput, new Regex(@"JoyconRightButtonDPAD_DOWN"));
                range.SetStyle(StyleInput, new Regex(@"JoyconRightButtonDPAD_RIGHT"));
                range.SetStyle(StyleInput, new Regex(@"JoyconRightButtonDPAD_UP"));
                range.SetStyle(StyleInput, new Regex(@"JoyconRightButtonDPAD_LEFT"));
                range.SetStyle(StyleInput, new Regex(@"JoyconRightButtonPLUS"));
                range.SetStyle(StyleInput, new Regex(@"JoyconRightButtonHOME"));
                range.SetStyle(StyleInput, new Regex(@"JoyconRightButtonSTICK"));
                range.SetStyle(StyleInput, new Regex(@"JoyconRightButtonACC"));
                range.SetStyle(StyleInput, new Regex(@"JoyconRightButtonSPA"));
                range.SetStyle(StyleInput, new Regex(@"JoyconRightRollLeft"));
                range.SetStyle(StyleInput, new Regex(@"JoyconRightRollRight"));
                range.SetStyle(StyleInput, new Regex(@"JoyconRightAccelX"));
                range.SetStyle(StyleInput, new Regex(@"JoyconRightAccelY"));
                range.SetStyle(StyleInput, new Regex(@"JoyconRightGyroX"));
                range.SetStyle(StyleInput, new Regex(@"JoyconRightGyroY"));
                range.SetStyle(StyleInput, new Regex(@"JoyconLeftStickX"));
                range.SetStyle(StyleInput, new Regex(@"JoyconLeftStickY"));
                range.SetStyle(StyleInput, new Regex(@"JoyconLeftButtonSHOULDER_1"));
                range.SetStyle(StyleInput, new Regex(@"JoyconLeftButtonSHOULDER_2"));
                range.SetStyle(StyleInput, new Regex(@"JoyconLeftButtonSR"));
                range.SetStyle(StyleInput, new Regex(@"JoyconLeftButtonSL"));
                range.SetStyle(StyleInput, new Regex(@"JoyconLeftButtonDPAD_DOWN"));
                range.SetStyle(StyleInput, new Regex(@"JoyconLeftButtonDPAD_RIGHT"));
                range.SetStyle(StyleInput, new Regex(@"JoyconLeftButtonDPAD_UP"));
                range.SetStyle(StyleInput, new Regex(@"JoyconLeftButtonDPAD_LEFT"));
                range.SetStyle(StyleInput, new Regex(@"JoyconLeftButtonMINUS"));
                range.SetStyle(StyleInput, new Regex(@"JoyconLeftButtonCAPTURE"));
                range.SetStyle(StyleInput, new Regex(@"JoyconLeftButtonSTICK"));
                range.SetStyle(StyleInput, new Regex(@"JoyconLeftButtonACC"));
                range.SetStyle(StyleInput, new Regex(@"JoyconLeftButtonSMA"));
                range.SetStyle(StyleInput, new Regex(@"JoyconLeftRollLeft"));
                range.SetStyle(StyleInput, new Regex(@"JoyconLeftRollRight"));
                range.SetStyle(StyleInput, new Regex(@"JoyconLeftAccelX"));
                range.SetStyle(StyleInput, new Regex(@"JoyconLeftAccelY"));
                range.SetStyle(StyleInput, new Regex(@"JoyconLeftGyroX"));
                range.SetStyle(StyleInput, new Regex(@"JoyconLeftGyroY"));
                range.SetStyle(StyleInput, new Regex(@"ProControllerLeftStickX"));
                range.SetStyle(StyleInput, new Regex(@"ProControllerLeftStickY"));
                range.SetStyle(StyleInput, new Regex(@"ProControllerRightStickX"));
                range.SetStyle(StyleInput, new Regex(@"ProControllerRightStickY"));
                range.SetStyle(StyleInput, new Regex(@"ProControllerButtonSHOULDER_Left_1"));
                range.SetStyle(StyleInput, new Regex(@"ProControllerButtonSHOULDER_Left_2"));
                range.SetStyle(StyleInput, new Regex(@"ProControllerButtonDPAD_DOWN"));
                range.SetStyle(StyleInput, new Regex(@"ProControllerButtonDPAD_RIGHT"));
                range.SetStyle(StyleInput, new Regex(@"ProControllerButtonDPAD_UP"));
                range.SetStyle(StyleInput, new Regex(@"ProControllerButtonDPAD_LEFT"));
                range.SetStyle(StyleInput, new Regex(@"ProControllerButtonMINUS"));
                range.SetStyle(StyleInput, new Regex(@"ProControllerButtonCAPTURE"));
                range.SetStyle(StyleInput, new Regex(@"ProControllerButtonSTICK_Left"));
                range.SetStyle(StyleInput, new Regex(@"ProControllerButtonSHOULDER_Right_1"));
                range.SetStyle(StyleInput, new Regex(@"ProControllerButtonSHOULDER_Right_2"));
                range.SetStyle(StyleInput, new Regex(@"ProControllerButtonA"));
                range.SetStyle(StyleInput, new Regex(@"ProControllerButtonB"));
                range.SetStyle(StyleInput, new Regex(@"ProControllerButtonX"));
                range.SetStyle(StyleInput, new Regex(@"ProControllerButtonY"));
                range.SetStyle(StyleInput, new Regex(@"ProControllerButtonPLUS"));
                range.SetStyle(StyleInput, new Regex(@"ProControllerButtonHOME"));
                range.SetStyle(StyleInput, new Regex(@"ProControllerButtonSTICK_Right"));
                range.SetStyle(StyleInput, new Regex(@"ProControllerAccelX"));
                range.SetStyle(StyleInput, new Regex(@"ProControllerAccelY"));
                range.SetStyle(StyleInput, new Regex(@"ProControllerGyroX"));
                range.SetStyle(StyleInput, new Regex(@"ProControllerGyroY"));
                range.SetStyle(StyleInput, new Regex(@"Mouse1Buttons0"));
                range.SetStyle(StyleInput, new Regex(@"Mouse1Buttons1"));
                range.SetStyle(StyleInput, new Regex(@"Mouse1Buttons2"));
                range.SetStyle(StyleInput, new Regex(@"Mouse1Buttons3"));
                range.SetStyle(StyleInput, new Regex(@"Mouse1Buttons4"));
                range.SetStyle(StyleInput, new Regex(@"Mouse1Buttons5"));
                range.SetStyle(StyleInput, new Regex(@"Mouse1Buttons6"));
                range.SetStyle(StyleInput, new Regex(@"Mouse1Buttons7"));
                range.SetStyle(StyleInput, new Regex(@"Mouse1AxisX"));
                range.SetStyle(StyleInput, new Regex(@"Mouse1AxisY"));
                range.SetStyle(StyleInput, new Regex(@"Mouse1AxisZ"));
                range.SetStyle(StyleInput, new Regex(@"Keyboard1KeyEscape"));
                range.SetStyle(StyleInput, new Regex(@"Keyboard1KeyD1"));
                range.SetStyle(StyleInput, new Regex(@"Keyboard1KeyD2"));
                range.SetStyle(StyleInput, new Regex(@"Keyboard1KeyD3"));
                range.SetStyle(StyleInput, new Regex(@"Keyboard1KeyD4"));
                range.SetStyle(StyleInput, new Regex(@"Keyboard1KeyD5"));
                range.SetStyle(StyleInput, new Regex(@"Keyboard1KeyD6"));
                range.SetStyle(StyleInput, new Regex(@"Keyboard1KeyD7"));
                range.SetStyle(StyleInput, new Regex(@"Keyboard1KeyD8"));
                range.SetStyle(StyleInput, new Regex(@"Keyboard1KeyD9"));
                range.SetStyle(StyleInput, new Regex(@"Keyboard1KeyD0"));
                range.SetStyle(StyleInput, new Regex(@"Keyboard1KeyMinus"));
                range.SetStyle(StyleInput, new Regex(@"Keyboard1KeyEquals"));
                range.SetStyle(StyleInput, new Regex(@"Keyboard1KeyBack"));
                range.SetStyle(StyleInput, new Regex(@"Keyboard1KeyTab"));
                range.SetStyle(StyleInput, new Regex(@"Keyboard1KeyQ"));
                range.SetStyle(StyleInput, new Regex(@"Keyboard1KeyW"));
                range.SetStyle(StyleInput, new Regex(@"Keyboard1KeyE"));
                range.SetStyle(StyleInput, new Regex(@"Keyboard1KeyR"));
                range.SetStyle(StyleInput, new Regex(@"Keyboard1KeyT"));
                range.SetStyle(StyleInput, new Regex(@"Keyboard1KeyY"));
                range.SetStyle(StyleInput, new Regex(@"Keyboard1KeyU"));
                range.SetStyle(StyleInput, new Regex(@"Keyboard1KeyI"));
                range.SetStyle(StyleInput, new Regex(@"Keyboard1KeyO"));
                range.SetStyle(StyleInput, new Regex(@"Keyboard1KeyP"));
                range.SetStyle(StyleInput, new Regex(@"Keyboard1KeyLeftBracket"));
                range.SetStyle(StyleInput, new Regex(@"Keyboard1KeyRightBracket"));
                range.SetStyle(StyleInput, new Regex(@"Keyboard1KeyReturn"));
                range.SetStyle(StyleInput, new Regex(@"Keyboard1KeyLeftControl"));
                range.SetStyle(StyleInput, new Regex(@"Keyboard1KeyA"));
                range.SetStyle(StyleInput, new Regex(@"Keyboard1KeyS"));
                range.SetStyle(StyleInput, new Regex(@"Keyboard1KeyD"));
                range.SetStyle(StyleInput, new Regex(@"Keyboard1KeyF"));
                range.SetStyle(StyleInput, new Regex(@"Keyboard1KeyG"));
                range.SetStyle(StyleInput, new Regex(@"Keyboard1KeyH"));
                range.SetStyle(StyleInput, new Regex(@"Keyboard1KeyJ"));
                range.SetStyle(StyleInput, new Regex(@"Keyboard1KeyK"));
                range.SetStyle(StyleInput, new Regex(@"Keyboard1KeyL"));
                range.SetStyle(StyleInput, new Regex(@"Keyboard1KeySemicolon"));
                range.SetStyle(StyleInput, new Regex(@"Keyboard1KeyApostrophe"));
                range.SetStyle(StyleInput, new Regex(@"Keyboard1KeyGrave"));
                range.SetStyle(StyleInput, new Regex(@"Keyboard1KeyLeftShift"));
                range.SetStyle(StyleInput, new Regex(@"Keyboard1KeyBackslash"));
                range.SetStyle(StyleInput, new Regex(@"Keyboard1KeyZ"));
                range.SetStyle(StyleInput, new Regex(@"Keyboard1KeyX"));
                range.SetStyle(StyleInput, new Regex(@"Keyboard1KeyC"));
                range.SetStyle(StyleInput, new Regex(@"Keyboard1KeyV"));
                range.SetStyle(StyleInput, new Regex(@"Keyboard1KeyB"));
                range.SetStyle(StyleInput, new Regex(@"Keyboard1KeyN"));
                range.SetStyle(StyleInput, new Regex(@"Keyboard1KeyM"));
                range.SetStyle(StyleInput, new Regex(@"Keyboard1KeyComma"));
                range.SetStyle(StyleInput, new Regex(@"Keyboard1KeyPeriod"));
                range.SetStyle(StyleInput, new Regex(@"Keyboard1KeySlash"));
                range.SetStyle(StyleInput, new Regex(@"Keyboard1KeyRightShift"));
                range.SetStyle(StyleInput, new Regex(@"Keyboard1KeyMultiply"));
                range.SetStyle(StyleInput, new Regex(@"Keyboard1KeyLeftAlt"));
                range.SetStyle(StyleInput, new Regex(@"Keyboard1KeySpace"));
                range.SetStyle(StyleInput, new Regex(@"Keyboard1KeyCapital"));
                range.SetStyle(StyleInput, new Regex(@"Keyboard1KeyF1"));
                range.SetStyle(StyleInput, new Regex(@"Keyboard1KeyF2"));
                range.SetStyle(StyleInput, new Regex(@"Keyboard1KeyF3"));
                range.SetStyle(StyleInput, new Regex(@"Keyboard1KeyF4"));
                range.SetStyle(StyleInput, new Regex(@"Keyboard1KeyF5"));
                range.SetStyle(StyleInput, new Regex(@"Keyboard1KeyF6"));
                range.SetStyle(StyleInput, new Regex(@"Keyboard1KeyF7"));
                range.SetStyle(StyleInput, new Regex(@"Keyboard1KeyF8"));
                range.SetStyle(StyleInput, new Regex(@"Keyboard1KeyF9"));
                range.SetStyle(StyleInput, new Regex(@"Keyboard1KeyF10"));
                range.SetStyle(StyleInput, new Regex(@"Keyboard1KeyNumberLock"));
                range.SetStyle(StyleInput, new Regex(@"Keyboard1KeyScrollLock"));
                range.SetStyle(StyleInput, new Regex(@"Keyboard1KeyNumberPad7"));
                range.SetStyle(StyleInput, new Regex(@"Keyboard1KeyNumberPad8"));
                range.SetStyle(StyleInput, new Regex(@"Keyboard1KeyNumberPad9"));
                range.SetStyle(StyleInput, new Regex(@"Keyboard1KeySubtract"));
                range.SetStyle(StyleInput, new Regex(@"Keyboard1KeyNumberPad4"));
                range.SetStyle(StyleInput, new Regex(@"Keyboard1KeyNumberPad5"));
                range.SetStyle(StyleInput, new Regex(@"Keyboard1KeyNumberPad6"));
                range.SetStyle(StyleInput, new Regex(@"Keyboard1KeyAdd"));
                range.SetStyle(StyleInput, new Regex(@"Keyboard1KeyNumberPad1"));
                range.SetStyle(StyleInput, new Regex(@"Keyboard1KeyNumberPad2"));
                range.SetStyle(StyleInput, new Regex(@"Keyboard1KeyNumberPad3"));
                range.SetStyle(StyleInput, new Regex(@"Keyboard1KeyNumberPad0"));
                range.SetStyle(StyleInput, new Regex(@"Keyboard1KeyDecimal"));
                range.SetStyle(StyleInput, new Regex(@"Keyboard1KeyOem102"));
                range.SetStyle(StyleInput, new Regex(@"Keyboard1KeyF11"));
                range.SetStyle(StyleInput, new Regex(@"Keyboard1KeyF12"));
                range.SetStyle(StyleInput, new Regex(@"Keyboard1KeyF13"));
                range.SetStyle(StyleInput, new Regex(@"Keyboard1KeyF14"));
                range.SetStyle(StyleInput, new Regex(@"Keyboard1KeyF15"));
                range.SetStyle(StyleInput, new Regex(@"Keyboard1KeyKana"));
                range.SetStyle(StyleInput, new Regex(@"Keyboard1KeyAbntC1"));
                range.SetStyle(StyleInput, new Regex(@"Keyboard1KeyConvert"));
                range.SetStyle(StyleInput, new Regex(@"Keyboard1KeyNoConvert"));
                range.SetStyle(StyleInput, new Regex(@"Keyboard1KeyYen"));
                range.SetStyle(StyleInput, new Regex(@"Keyboard1KeyAbntC2"));
                range.SetStyle(StyleInput, new Regex(@"Keyboard1KeyNumberPadEquals"));
                range.SetStyle(StyleInput, new Regex(@"Keyboard1KeyPreviousTrack"));
                range.SetStyle(StyleInput, new Regex(@"Keyboard1KeyAT"));
                range.SetStyle(StyleInput, new Regex(@"Keyboard1KeyColon"));
                range.SetStyle(StyleInput, new Regex(@"Keyboard1KeyUnderline"));
                range.SetStyle(StyleInput, new Regex(@"Keyboard1KeyKanji"));
                range.SetStyle(StyleInput, new Regex(@"Keyboard1KeyStop"));
                range.SetStyle(StyleInput, new Regex(@"Keyboard1KeyAX"));
                range.SetStyle(StyleInput, new Regex(@"Keyboard1KeyUnlabeled"));
                range.SetStyle(StyleInput, new Regex(@"Keyboard1KeyNextTrack"));
                range.SetStyle(StyleInput, new Regex(@"Keyboard1KeyNumberPadEnter"));
                range.SetStyle(StyleInput, new Regex(@"Keyboard1KeyRightControl"));
                range.SetStyle(StyleInput, new Regex(@"Keyboard1KeyMute"));
                range.SetStyle(StyleInput, new Regex(@"Keyboard1KeyCalculator"));
                range.SetStyle(StyleInput, new Regex(@"Keyboard1KeyPlayPause"));
                range.SetStyle(StyleInput, new Regex(@"Keyboard1KeyMediaStop"));
                range.SetStyle(StyleInput, new Regex(@"Keyboard1KeyVolumeDown"));
                range.SetStyle(StyleInput, new Regex(@"Keyboard1KeyVolumeUp"));
                range.SetStyle(StyleInput, new Regex(@"Keyboard1KeyWebHome"));
                range.SetStyle(StyleInput, new Regex(@"Keyboard1KeyNumberPadComma"));
                range.SetStyle(StyleInput, new Regex(@"Keyboard1KeyDivide"));
                range.SetStyle(StyleInput, new Regex(@"Keyboard1KeyPrintScreen"));
                range.SetStyle(StyleInput, new Regex(@"Keyboard1KeyRightAlt"));
                range.SetStyle(StyleInput, new Regex(@"Keyboard1KeyPause"));
                range.SetStyle(StyleInput, new Regex(@"Keyboard1KeyHome"));
                range.SetStyle(StyleInput, new Regex(@"Keyboard1KeyUp"));
                range.SetStyle(StyleInput, new Regex(@"Keyboard1KeyPageUp"));
                range.SetStyle(StyleInput, new Regex(@"Keyboard1KeyLeft"));
                range.SetStyle(StyleInput, new Regex(@"Keyboard1KeyRight"));
                range.SetStyle(StyleInput, new Regex(@"Keyboard1KeyEnd"));
                range.SetStyle(StyleInput, new Regex(@"Keyboard1KeyDown"));
                range.SetStyle(StyleInput, new Regex(@"Keyboard1KeyPageDown"));
                range.SetStyle(StyleInput, new Regex(@"Keyboard1KeyInsert"));
                range.SetStyle(StyleInput, new Regex(@"Keyboard1KeyDelete"));
                range.SetStyle(StyleInput, new Regex(@"Keyboard1KeyLeftWindowsKey"));
                range.SetStyle(StyleInput, new Regex(@"Keyboard1KeyRightWindowsKey"));
                range.SetStyle(StyleInput, new Regex(@"Keyboard1KeyApplications"));
                range.SetStyle(StyleInput, new Regex(@"Keyboard1KeyPower"));
                range.SetStyle(StyleInput, new Regex(@"Keyboard1KeySleep"));
                range.SetStyle(StyleInput, new Regex(@"Keyboard1KeyWake"));
                range.SetStyle(StyleInput, new Regex(@"Keyboard1KeyWebSearch"));
                range.SetStyle(StyleInput, new Regex(@"Keyboard1KeyWebFavorites"));
                range.SetStyle(StyleInput, new Regex(@"Keyboard1KeyWebRefresh"));
                range.SetStyle(StyleInput, new Regex(@"Keyboard1KeyWebStop"));
                range.SetStyle(StyleInput, new Regex(@"Keyboard1KeyWebForward"));
                range.SetStyle(StyleInput, new Regex(@"Keyboard1KeyWebBack"));
                range.SetStyle(StyleInput, new Regex(@"Keyboard1KeyMyComputer"));
                range.SetStyle(StyleInput, new Regex(@"Keyboard1KeyMail"));
                range.SetStyle(StyleInput, new Regex(@"Keyboard1KeyMediaSelect"));
                range.SetStyle(StyleInput, new Regex(@"Keyboard1KeyUnknown"));
                range.SetStyle(StyleOutput, new Regex(@"KeyboardMouseDriverType"));
                range.SetStyle(StyleOutput, new Regex(@"MouseMoveX"));
                range.SetStyle(StyleOutput, new Regex(@"MouseMoveY"));
                range.SetStyle(StyleOutput, new Regex(@"MouseAbsX"));
                range.SetStyle(StyleOutput, new Regex(@"MouseAbsY"));
                range.SetStyle(StyleOutput, new Regex(@"MouseDesktopX"));
                range.SetStyle(StyleOutput, new Regex(@"MouseDesktopY"));
                range.SetStyle(StyleOutput, new Regex(@"SendLeftClick"));
                range.SetStyle(StyleOutput, new Regex(@"SendRightClick"));
                range.SetStyle(StyleOutput, new Regex(@"SendMiddleClick"));
                range.SetStyle(StyleOutput, new Regex(@"SendWheelUp"));
                range.SetStyle(StyleOutput, new Regex(@"SendWheelDown"));
                range.SetStyle(StyleOutput, new Regex(@"SendLeft"));
                range.SetStyle(StyleOutput, new Regex(@"SendRight"));
                range.SetStyle(StyleOutput, new Regex(@"SendUp"));
                range.SetStyle(StyleOutput, new Regex(@"SendDown"));
                range.SetStyle(StyleOutput, new Regex(@"SendLButton"));
                range.SetStyle(StyleOutput, new Regex(@"SendRButton"));
                range.SetStyle(StyleOutput, new Regex(@"SendCancel"));
                range.SetStyle(StyleOutput, new Regex(@"SendMBUTTON"));
                range.SetStyle(StyleOutput, new Regex(@"SendXBUTTON1"));
                range.SetStyle(StyleOutput, new Regex(@"SendXBUTTON2"));
                range.SetStyle(StyleOutput, new Regex(@"SendBack"));
                range.SetStyle(StyleOutput, new Regex(@"SendTab"));
                range.SetStyle(StyleOutput, new Regex(@"SendClear"));
                range.SetStyle(StyleOutput, new Regex(@"SendReturn"));
                range.SetStyle(StyleOutput, new Regex(@"SendSHIFT"));
                range.SetStyle(StyleOutput, new Regex(@"SendCONTROL"));
                range.SetStyle(StyleOutput, new Regex(@"SendMENU"));
                range.SetStyle(StyleOutput, new Regex(@"SendPAUSE"));
                range.SetStyle(StyleOutput, new Regex(@"SendCAPITAL"));
                range.SetStyle(StyleOutput, new Regex(@"SendKANA"));
                range.SetStyle(StyleOutput, new Regex(@"SendHANGEUL"));
                range.SetStyle(StyleOutput, new Regex(@"SendHANGUL"));
                range.SetStyle(StyleOutput, new Regex(@"SendJUNJA"));
                range.SetStyle(StyleOutput, new Regex(@"SendFINAL"));
                range.SetStyle(StyleOutput, new Regex(@"SendHANJA"));
                range.SetStyle(StyleOutput, new Regex(@"SendKANJI"));
                range.SetStyle(StyleOutput, new Regex(@"SendEscape"));
                range.SetStyle(StyleOutput, new Regex(@"SendCONVERT"));
                range.SetStyle(StyleOutput, new Regex(@"SendNONCONVERT"));
                range.SetStyle(StyleOutput, new Regex(@"SendACCEPT"));
                range.SetStyle(StyleOutput, new Regex(@"SendMODECHANGE"));
                range.SetStyle(StyleOutput, new Regex(@"SendSpace"));
                range.SetStyle(StyleOutput, new Regex(@"SendPRIOR"));
                range.SetStyle(StyleOutput, new Regex(@"SendNEXT"));
                range.SetStyle(StyleOutput, new Regex(@"SendEND"));
                range.SetStyle(StyleOutput, new Regex(@"SendHOME"));
                range.SetStyle(StyleOutput, new Regex(@"SendLEFT"));
                range.SetStyle(StyleOutput, new Regex(@"SendUP"));
                range.SetStyle(StyleOutput, new Regex(@"SendRIGHT"));
                range.SetStyle(StyleOutput, new Regex(@"SendDOWN"));
                range.SetStyle(StyleOutput, new Regex(@"SendSELECT"));
                range.SetStyle(StyleOutput, new Regex(@"SendPRINT"));
                range.SetStyle(StyleOutput, new Regex(@"SendEXECUTE"));
                range.SetStyle(StyleOutput, new Regex(@"SendSNAPSHOT"));
                range.SetStyle(StyleOutput, new Regex(@"SendINSERT"));
                range.SetStyle(StyleOutput, new Regex(@"SendDELETE"));
                range.SetStyle(StyleOutput, new Regex(@"SendHELP"));
                range.SetStyle(StyleOutput, new Regex(@"SendAPOSTROPHE"));
                range.SetStyle(StyleOutput, new Regex(@"Send0"));
                range.SetStyle(StyleOutput, new Regex(@"Send1"));
                range.SetStyle(StyleOutput, new Regex(@"Send2"));
                range.SetStyle(StyleOutput, new Regex(@"Send3"));
                range.SetStyle(StyleOutput, new Regex(@"Send4"));
                range.SetStyle(StyleOutput, new Regex(@"Send5"));
                range.SetStyle(StyleOutput, new Regex(@"Send6"));
                range.SetStyle(StyleOutput, new Regex(@"Send7"));
                range.SetStyle(StyleOutput, new Regex(@"Send8"));
                range.SetStyle(StyleOutput, new Regex(@"Send9"));
                range.SetStyle(StyleOutput, new Regex(@"SendA"));
                range.SetStyle(StyleOutput, new Regex(@"SendB"));
                range.SetStyle(StyleOutput, new Regex(@"SendC"));
                range.SetStyle(StyleOutput, new Regex(@"SendD"));
                range.SetStyle(StyleOutput, new Regex(@"SendE"));
                range.SetStyle(StyleOutput, new Regex(@"SendF"));
                range.SetStyle(StyleOutput, new Regex(@"SendG"));
                range.SetStyle(StyleOutput, new Regex(@"SendH"));
                range.SetStyle(StyleOutput, new Regex(@"SendI"));
                range.SetStyle(StyleOutput, new Regex(@"SendJ"));
                range.SetStyle(StyleOutput, new Regex(@"SendK"));
                range.SetStyle(StyleOutput, new Regex(@"SendL"));
                range.SetStyle(StyleOutput, new Regex(@"SendM"));
                range.SetStyle(StyleOutput, new Regex(@"SendN"));
                range.SetStyle(StyleOutput, new Regex(@"SendO"));
                range.SetStyle(StyleOutput, new Regex(@"SendP"));
                range.SetStyle(StyleOutput, new Regex(@"SendQ"));
                range.SetStyle(StyleOutput, new Regex(@"SendR"));
                range.SetStyle(StyleOutput, new Regex(@"SendS"));
                range.SetStyle(StyleOutput, new Regex(@"SendT"));
                range.SetStyle(StyleOutput, new Regex(@"SendU"));
                range.SetStyle(StyleOutput, new Regex(@"SendV"));
                range.SetStyle(StyleOutput, new Regex(@"SendW"));
                range.SetStyle(StyleOutput, new Regex(@"SendX"));
                range.SetStyle(StyleOutput, new Regex(@"SendY"));
                range.SetStyle(StyleOutput, new Regex(@"SendZ"));
                range.SetStyle(StyleOutput, new Regex(@"SendLWIN"));
                range.SetStyle(StyleOutput, new Regex(@"SendRWIN"));
                range.SetStyle(StyleOutput, new Regex(@"SendAPPS"));
                range.SetStyle(StyleOutput, new Regex(@"SendSLEEP"));
                range.SetStyle(StyleOutput, new Regex(@"SendNUMPAD0"));
                range.SetStyle(StyleOutput, new Regex(@"SendNUMPAD1"));
                range.SetStyle(StyleOutput, new Regex(@"SendNUMPAD2"));
                range.SetStyle(StyleOutput, new Regex(@"SendNUMPAD3"));
                range.SetStyle(StyleOutput, new Regex(@"SendNUMPAD4"));
                range.SetStyle(StyleOutput, new Regex(@"SendNUMPAD5"));
                range.SetStyle(StyleOutput, new Regex(@"SendNUMPAD6"));
                range.SetStyle(StyleOutput, new Regex(@"SendNUMPAD7"));
                range.SetStyle(StyleOutput, new Regex(@"SendNUMPAD8"));
                range.SetStyle(StyleOutput, new Regex(@"SendNUMPAD9"));
                range.SetStyle(StyleOutput, new Regex(@"SendMULTIPLY"));
                range.SetStyle(StyleOutput, new Regex(@"SendADD"));
                range.SetStyle(StyleOutput, new Regex(@"SendSEPARATOR"));
                range.SetStyle(StyleOutput, new Regex(@"SendSUBTRACT"));
                range.SetStyle(StyleOutput, new Regex(@"SendDECIMAL"));
                range.SetStyle(StyleOutput, new Regex(@"SendDIVIDE"));
                range.SetStyle(StyleOutput, new Regex(@"SendF1"));
                range.SetStyle(StyleOutput, new Regex(@"SendF2"));
                range.SetStyle(StyleOutput, new Regex(@"SendF3"));
                range.SetStyle(StyleOutput, new Regex(@"SendF4"));
                range.SetStyle(StyleOutput, new Regex(@"SendF5"));
                range.SetStyle(StyleOutput, new Regex(@"SendF6"));
                range.SetStyle(StyleOutput, new Regex(@"SendF7"));
                range.SetStyle(StyleOutput, new Regex(@"SendF8"));
                range.SetStyle(StyleOutput, new Regex(@"SendF9"));
                range.SetStyle(StyleOutput, new Regex(@"SendF10"));
                range.SetStyle(StyleOutput, new Regex(@"SendF11"));
                range.SetStyle(StyleOutput, new Regex(@"SendF12"));
                range.SetStyle(StyleOutput, new Regex(@"SendF13"));
                range.SetStyle(StyleOutput, new Regex(@"SendF14"));
                range.SetStyle(StyleOutput, new Regex(@"SendF15"));
                range.SetStyle(StyleOutput, new Regex(@"SendF16"));
                range.SetStyle(StyleOutput, new Regex(@"SendF17"));
                range.SetStyle(StyleOutput, new Regex(@"SendF18"));
                range.SetStyle(StyleOutput, new Regex(@"SendF19"));
                range.SetStyle(StyleOutput, new Regex(@"SendF20"));
                range.SetStyle(StyleOutput, new Regex(@"SendF21"));
                range.SetStyle(StyleOutput, new Regex(@"SendF22"));
                range.SetStyle(StyleOutput, new Regex(@"SendF23"));
                range.SetStyle(StyleOutput, new Regex(@"SendF24"));
                range.SetStyle(StyleOutput, new Regex(@"SendNUMLOCK"));
                range.SetStyle(StyleOutput, new Regex(@"SendSCROLL"));
                range.SetStyle(StyleOutput, new Regex(@"SendLeftShift"));
                range.SetStyle(StyleOutput, new Regex(@"SendRightShift"));
                range.SetStyle(StyleOutput, new Regex(@"SendLeftControl"));
                range.SetStyle(StyleOutput, new Regex(@"SendRightControl"));
                range.SetStyle(StyleOutput, new Regex(@"SendLMENU"));
                range.SetStyle(StyleOutput, new Regex(@"SendRMENU"));
            }
            catch { }
        }
        private void FillAutocompletion()
        {
            this.autocompleteMenu1.Items = new string[] {
                "CurrentResolution",
                "running",
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
                "BeginAsyncPolling",
                "Connect",
                "Disconnect",
                "SubmitReport1",
                "EnumerateControllers",
                "UnLoadKM",
                "SetKM",
                "Valuechanges",
                "Valuechange",
                "ValueChange",
                "_ValueChange",
                "controllers",
                "ScpBus",
                "scp",
                "LoadController",
                "UnLoadController",
                "SetController",
                "XInputAPI",
                "XInput",
                "xi",
                "XInputHookConnect",
                "controllersds4",
                "DS4Controller",
                "DS4",
                "controllersvjoy",
                "VJoyController",
                "VJoy",
                "DirectInputAPI",
                "DirectInput",
                "di",
                "DirectInputHookConnect",
                "DualSenseAPI",
                "DualSense",
                "ds",
                "InitDualSenseAccel",
                "DualShock4API",
                "DualShock4",
                "ds4",
                "InitDualShock4Accel",
                "Interceptions",
                "SendInterception",
                "Input",
                "si",
                "input",
                "KeyboardFilterMode",
                "KeyboardFilterMode.All",
                "MouseFilterMode",
                "MouseFilterMode.All",
                "Load",
                "WiiMoteAPI",
                "WiiMote",
                "wm",
                "Init",
                "SwitchProControllerAPI",
                "SwitchProController",
                "spc",
                "InitProController",
                "InitProControllerAccel",
                "InitProControllerStick",
                "JoyconChargingGripAPI",
                "JoyconChargingGrip",
                "jcg",
                "BeginAsyncPollingLeft",
                "BeginAsyncPollingRight",
                "InitLeftJoycon",
                "InitRightJoycon",
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
                "KeyboardMouseInput",
                "kmi",
                "BeginPollingMouse",
                "BeginPollingKeyboard",
                "MouseInputHookConnect",
                "KeyboardInputHookConnect",
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
                "Controller1ButtonUpPresse",
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
                "SendRMENU"
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
            parameters.ReferencedAssemblies.Add(Application.StartupPath + @"\controllers.dll");
            parameters.ReferencedAssemblies.Add(Application.StartupPath + @"\controllersds4.dll");
            parameters.ReferencedAssemblies.Add(Application.StartupPath + @"\controllersvjoy.dll");
            parameters.ReferencedAssemblies.Add(Application.StartupPath + @"\keyboards.dll");
            parameters.ReferencedAssemblies.Add(Application.StartupPath + @"\mouses.dll");
            parameters.ReferencedAssemblies.Add(Application.StartupPath + @"\Interceptions.dll");
            parameters.ReferencedAssemblies.Add(Application.StartupPath + @"\Valuechanges.dll");
            parameters.ReferencedAssemblies.Add(Application.StartupPath + @"\KeyboardMouseinput.dll");
            parameters.ReferencedAssemblies.Add(Application.StartupPath + @"\Dualsense.dll");
            parameters.ReferencedAssemblies.Add(Application.StartupPath + @"\Dualshock4.dll");
            parameters.ReferencedAssemblies.Add(Application.StartupPath + @"\Directinput.dll");
            parameters.ReferencedAssemblies.Add(Application.StartupPath + @"\Joyconcharginggrip.dll");
            parameters.ReferencedAssemblies.Add(Application.StartupPath + @"\Joyconleft.dll");
            parameters.ReferencedAssemblies.Add(Application.StartupPath + @"\Joyconright.dll");
            parameters.ReferencedAssemblies.Add(Application.StartupPath + @"\Switchprocontroller.dll");
            parameters.ReferencedAssemblies.Add(Application.StartupPath + @"\Wiimote.dll");
            parameters.ReferencedAssemblies.Add(Application.StartupPath + @"\Xinput.dll");
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