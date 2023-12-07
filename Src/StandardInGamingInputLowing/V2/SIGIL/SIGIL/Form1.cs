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
        private static Style InputStyle = new TextStyle(Brushes.Blue, null, System.Drawing.FontStyle.Regular), OutputStyle = new TextStyle(Brushes.Orange, null, System.Drawing.FontStyle.Regular);
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
                range.SetStyle(InputStyle, new Regex(@"getstate"));
                range.SetStyle(InputStyle, new Regex(@"System.Windows.Forms.Screen.PrimaryScreen.Bounds.Width"));
                range.SetStyle(InputStyle, new Regex(@"System.Windows.Forms.Screen.PrimaryScreen.Bounds.Height"));
                range.SetStyle(InputStyle, new Regex(@"Math.Abs"));
                range.SetStyle(InputStyle, new Regex(@"Math.Sign"));
                range.SetStyle(InputStyle, new Regex(@"Math.Round"));
                range.SetStyle(InputStyle, new Regex(@"Math.Pow"));
                range.SetStyle(InputStyle, new Regex(@"Math.Sqrt"));
                range.SetStyle(InputStyle, new Regex(@"Math.Log"));
                range.SetStyle(InputStyle, new Regex(@"Math.Exp"));
                range.SetStyle(InputStyle, new Regex(@"Math.Min"));
                range.SetStyle(InputStyle, new Regex(@"Math.Max"));
                range.SetStyle(InputStyle, new Regex(@"Math.Floor"));
                range.SetStyle(InputStyle, new Regex(@"Math.Truncate"));
                range.SetStyle(InputStyle, new Regex(@"wd"));
                range.SetStyle(InputStyle, new Regex(@"wu"));
                range.SetStyle(InputStyle, new Regex(@"valchanged"));
                range.SetStyle(InputStyle, new Regex(@"Scale"));
                range.SetStyle(InputStyle, new Regex(@"width"));
                range.SetStyle(InputStyle, new Regex(@"height"));
                range.SetStyle(InputStyle, new Regex(@"Key_LBUTTON"));
                range.SetStyle(InputStyle, new Regex(@"Key_RBUTTON"));
                range.SetStyle(InputStyle, new Regex(@"Key_CANCEL"));
                range.SetStyle(InputStyle, new Regex(@"Key_MBUTTON"));
                range.SetStyle(InputStyle, new Regex(@"Key_XBUTTON1"));
                range.SetStyle(InputStyle, new Regex(@"Key_XBUTTON2"));
                range.SetStyle(InputStyle, new Regex(@"Key_BACK"));
                range.SetStyle(InputStyle, new Regex(@"Key_Tab"));
                range.SetStyle(InputStyle, new Regex(@"Key_CLEAR"));
                range.SetStyle(InputStyle, new Regex(@"Key_Return"));
                range.SetStyle(InputStyle, new Regex(@"Key_SHIFT"));
                range.SetStyle(InputStyle, new Regex(@"Key_CONTROL"));
                range.SetStyle(InputStyle, new Regex(@"Key_MENU"));
                range.SetStyle(InputStyle, new Regex(@"Key_PAUSE"));
                range.SetStyle(InputStyle, new Regex(@"Key_CAPITAL"));
                range.SetStyle(InputStyle, new Regex(@"Key_KANA"));
                range.SetStyle(InputStyle, new Regex(@"Key_HANGEUL"));
                range.SetStyle(InputStyle, new Regex(@"Key_HANGUL"));
                range.SetStyle(InputStyle, new Regex(@"Key_JUNJA"));
                range.SetStyle(InputStyle, new Regex(@"Key_FINAL"));
                range.SetStyle(InputStyle, new Regex(@"Key_HANJA"));
                range.SetStyle(InputStyle, new Regex(@"Key_KANJI"));
                range.SetStyle(InputStyle, new Regex(@"Key_Escape"));
                range.SetStyle(InputStyle, new Regex(@"Key_CONVERT"));
                range.SetStyle(InputStyle, new Regex(@"Key_NONCONVERT"));
                range.SetStyle(InputStyle, new Regex(@"Key_ACCEPT"));
                range.SetStyle(InputStyle, new Regex(@"Key_MODECHANGE"));
                range.SetStyle(InputStyle, new Regex(@"Key_Space"));
                range.SetStyle(InputStyle, new Regex(@"Key_PRIOR"));
                range.SetStyle(InputStyle, new Regex(@"Key_NEXT"));
                range.SetStyle(InputStyle, new Regex(@"Key_END"));
                range.SetStyle(InputStyle, new Regex(@"Key_HOME"));
                range.SetStyle(InputStyle, new Regex(@"Key_LEFT"));
                range.SetStyle(InputStyle, new Regex(@"Key_UP"));
                range.SetStyle(InputStyle, new Regex(@"Key_RIGHT"));
                range.SetStyle(InputStyle, new Regex(@"Key_DOWN"));
                range.SetStyle(InputStyle, new Regex(@"Key_SELECT"));
                range.SetStyle(InputStyle, new Regex(@"Key_PRINT"));
                range.SetStyle(InputStyle, new Regex(@"Key_EXECUTE"));
                range.SetStyle(InputStyle, new Regex(@"Key_SNAPSHOT"));
                range.SetStyle(InputStyle, new Regex(@"Key_INSERT"));
                range.SetStyle(InputStyle, new Regex(@"Key_DELETE"));
                range.SetStyle(InputStyle, new Regex(@"Key_HELP"));
                range.SetStyle(InputStyle, new Regex(@"Key_APOSTROPHE"));
                range.SetStyle(InputStyle, new Regex(@"Key_0"));
                range.SetStyle(InputStyle, new Regex(@"Key_1"));
                range.SetStyle(InputStyle, new Regex(@"Key_2"));
                range.SetStyle(InputStyle, new Regex(@"Key_3"));
                range.SetStyle(InputStyle, new Regex(@"Key_4"));
                range.SetStyle(InputStyle, new Regex(@"Key_5"));
                range.SetStyle(InputStyle, new Regex(@"Key_6"));
                range.SetStyle(InputStyle, new Regex(@"Key_7"));
                range.SetStyle(InputStyle, new Regex(@"Key_8"));
                range.SetStyle(InputStyle, new Regex(@"Key_9"));
                range.SetStyle(InputStyle, new Regex(@"Key_A"));
                range.SetStyle(InputStyle, new Regex(@"Key_B"));
                range.SetStyle(InputStyle, new Regex(@"Key_C"));
                range.SetStyle(InputStyle, new Regex(@"Key_D"));
                range.SetStyle(InputStyle, new Regex(@"Key_E"));
                range.SetStyle(InputStyle, new Regex(@"Key_F"));
                range.SetStyle(InputStyle, new Regex(@"Key_G"));
                range.SetStyle(InputStyle, new Regex(@"Key_H"));
                range.SetStyle(InputStyle, new Regex(@"Key_I"));
                range.SetStyle(InputStyle, new Regex(@"Key_J"));
                range.SetStyle(InputStyle, new Regex(@"Key_K"));
                range.SetStyle(InputStyle, new Regex(@"Key_L"));
                range.SetStyle(InputStyle, new Regex(@"Key_M"));
                range.SetStyle(InputStyle, new Regex(@"Key_N"));
                range.SetStyle(InputStyle, new Regex(@"Key_O"));
                range.SetStyle(InputStyle, new Regex(@"Key_P"));
                range.SetStyle(InputStyle, new Regex(@"Key_Q"));
                range.SetStyle(InputStyle, new Regex(@"Key_R"));
                range.SetStyle(InputStyle, new Regex(@"Key_S"));
                range.SetStyle(InputStyle, new Regex(@"Key_T"));
                range.SetStyle(InputStyle, new Regex(@"Key_U"));
                range.SetStyle(InputStyle, new Regex(@"Key_V"));
                range.SetStyle(InputStyle, new Regex(@"Key_W"));
                range.SetStyle(InputStyle, new Regex(@"Key_X"));
                range.SetStyle(InputStyle, new Regex(@"Key_Y"));
                range.SetStyle(InputStyle, new Regex(@"Key_Z"));
                range.SetStyle(InputStyle, new Regex(@"Key_LWIN"));
                range.SetStyle(InputStyle, new Regex(@"Key_RWIN"));
                range.SetStyle(InputStyle, new Regex(@"Key_APPS"));
                range.SetStyle(InputStyle, new Regex(@"Key_SLEEP"));
                range.SetStyle(InputStyle, new Regex(@"Key_NUMPAD0"));
                range.SetStyle(InputStyle, new Regex(@"Key_NUMPAD1"));
                range.SetStyle(InputStyle, new Regex(@"Key_NUMPAD2"));
                range.SetStyle(InputStyle, new Regex(@"Key_NUMPAD3"));
                range.SetStyle(InputStyle, new Regex(@"Key_NUMPAD4"));
                range.SetStyle(InputStyle, new Regex(@"Key_NUMPAD5"));
                range.SetStyle(InputStyle, new Regex(@"Key_NUMPAD6"));
                range.SetStyle(InputStyle, new Regex(@"Key_NUMPAD7"));
                range.SetStyle(InputStyle, new Regex(@"Key_NUMPAD8"));
                range.SetStyle(InputStyle, new Regex(@"Key_NUMPAD9"));
                range.SetStyle(InputStyle, new Regex(@"Key_MULTIPLY"));
                range.SetStyle(InputStyle, new Regex(@"Key_ADD"));
                range.SetStyle(InputStyle, new Regex(@"Key_SEPARATOR"));
                range.SetStyle(InputStyle, new Regex(@"Key_SUBTRACT"));
                range.SetStyle(InputStyle, new Regex(@"Key_DECIMAL"));
                range.SetStyle(InputStyle, new Regex(@"Key_DIVIDE"));
                range.SetStyle(InputStyle, new Regex(@"Key_F1"));
                range.SetStyle(InputStyle, new Regex(@"Key_F2"));
                range.SetStyle(InputStyle, new Regex(@"Key_F3"));
                range.SetStyle(InputStyle, new Regex(@"Key_F4"));
                range.SetStyle(InputStyle, new Regex(@"Key_F5"));
                range.SetStyle(InputStyle, new Regex(@"Key_F6"));
                range.SetStyle(InputStyle, new Regex(@"Key_F7"));
                range.SetStyle(InputStyle, new Regex(@"Key_F8"));
                range.SetStyle(InputStyle, new Regex(@"Key_F9"));
                range.SetStyle(InputStyle, new Regex(@"Key_F10"));
                range.SetStyle(InputStyle, new Regex(@"Key_F11"));
                range.SetStyle(InputStyle, new Regex(@"Key_F12"));
                range.SetStyle(InputStyle, new Regex(@"Key_F13"));
                range.SetStyle(InputStyle, new Regex(@"Key_F14"));
                range.SetStyle(InputStyle, new Regex(@"Key_F15"));
                range.SetStyle(InputStyle, new Regex(@"Key_F16"));
                range.SetStyle(InputStyle, new Regex(@"Key_F17"));
                range.SetStyle(InputStyle, new Regex(@"Key_F18"));
                range.SetStyle(InputStyle, new Regex(@"Key_F19"));
                range.SetStyle(InputStyle, new Regex(@"Key_F20"));
                range.SetStyle(InputStyle, new Regex(@"Key_F21"));
                range.SetStyle(InputStyle, new Regex(@"Key_F22"));
                range.SetStyle(InputStyle, new Regex(@"Key_F23"));
                range.SetStyle(InputStyle, new Regex(@"Key_F24"));
                range.SetStyle(InputStyle, new Regex(@"Key_NUMLOCK"));
                range.SetStyle(InputStyle, new Regex(@"Key_SCROLL"));
                range.SetStyle(InputStyle, new Regex(@"Key_LeftShift"));
                range.SetStyle(InputStyle, new Regex(@"Key_RightShift"));
                range.SetStyle(InputStyle, new Regex(@"Key_LeftControl"));
                range.SetStyle(InputStyle, new Regex(@"Key_RightControl"));
                range.SetStyle(InputStyle, new Regex(@"Key_LMENU"));
                range.SetStyle(InputStyle, new Regex(@"Key_RMENU"));
                range.SetStyle(InputStyle, new Regex(@"Key_BROWSER_BACK"));
                range.SetStyle(InputStyle, new Regex(@"Key_BROWSER_FORWARD"));
                range.SetStyle(InputStyle, new Regex(@"Key_BROWSER_REFRESH"));
                range.SetStyle(InputStyle, new Regex(@"Key_BROWSER_STOP"));
                range.SetStyle(InputStyle, new Regex(@"Key_BROWSER_SEARCH"));
                range.SetStyle(InputStyle, new Regex(@"Key_BROWSER_FAVORITES"));
                range.SetStyle(InputStyle, new Regex(@"Key_BROWSER_HOME"));
                range.SetStyle(InputStyle, new Regex(@"Key_VOLUME_MUTE"));
                range.SetStyle(InputStyle, new Regex(@"Key_VOLUME_DOWN"));
                range.SetStyle(InputStyle, new Regex(@"Key_VOLUME_UP"));
                range.SetStyle(InputStyle, new Regex(@"Key_MEDIA_NEXT_TRACK"));
                range.SetStyle(InputStyle, new Regex(@"Key_MEDIA_PREV_TRACK"));
                range.SetStyle(InputStyle, new Regex(@"Key_MEDIA_STOP"));
                range.SetStyle(InputStyle, new Regex(@"Key_MEDIA_PLAY_PAUSE"));
                range.SetStyle(InputStyle, new Regex(@"Key_LAUNCH_MAIL"));
                range.SetStyle(InputStyle, new Regex(@"Key_LAUNCH_MEDIA_SELECT"));
                range.SetStyle(InputStyle, new Regex(@"Key_LAUNCH_APP1"));
                range.SetStyle(InputStyle, new Regex(@"Key_LAUNCH_APP2"));
                range.SetStyle(InputStyle, new Regex(@"Key_OEM_1"));
                range.SetStyle(InputStyle, new Regex(@"Key_OEM_PLUS"));
                range.SetStyle(InputStyle, new Regex(@"Key_OEM_COMMA"));
                range.SetStyle(InputStyle, new Regex(@"Key_OEM_MINUS"));
                range.SetStyle(InputStyle, new Regex(@"Key_OEM_PERIOD"));
                range.SetStyle(InputStyle, new Regex(@"Key_OEM_2"));
                range.SetStyle(InputStyle, new Regex(@"Key_OEM_3"));
                range.SetStyle(InputStyle, new Regex(@"Key_OEM_4"));
                range.SetStyle(InputStyle, new Regex(@"Key_OEM_5"));
                range.SetStyle(InputStyle, new Regex(@"Key_OEM_6"));
                range.SetStyle(InputStyle, new Regex(@"Key_OEM_7"));
                range.SetStyle(InputStyle, new Regex(@"Key_OEM_8"));
                range.SetStyle(InputStyle, new Regex(@"Key_OEM_102"));
                range.SetStyle(InputStyle, new Regex(@"Key_PROCESSKEY"));
                range.SetStyle(InputStyle, new Regex(@"Key_PACKET"));
                range.SetStyle(InputStyle, new Regex(@"Key_ATTN"));
                range.SetStyle(InputStyle, new Regex(@"Key_CRSEL"));
                range.SetStyle(InputStyle, new Regex(@"Key_EXSEL"));
                range.SetStyle(InputStyle, new Regex(@"Key_EREOF"));
                range.SetStyle(InputStyle, new Regex(@"Key_PLAY"));
                range.SetStyle(InputStyle, new Regex(@"Key_ZOOM"));
                range.SetStyle(InputStyle, new Regex(@"Key_NONAME"));
                range.SetStyle(InputStyle, new Regex(@"Key_PA1"));
                range.SetStyle(InputStyle, new Regex(@"Key_OEM_CLEAR"));
                range.SetStyle(InputStyle, new Regex(@"irx"));
                range.SetStyle(InputStyle, new Regex(@"iry"));
                range.SetStyle(InputStyle, new Regex(@"WiimoteButtonStateA"));
                range.SetStyle(InputStyle, new Regex(@"WiimoteButtonStateB"));
                range.SetStyle(InputStyle, new Regex(@"WiimoteButtonStateMinus"));
                range.SetStyle(InputStyle, new Regex(@"WiimoteButtonStateHome"));
                range.SetStyle(InputStyle, new Regex(@"WiimoteButtonStatePlus"));
                range.SetStyle(InputStyle, new Regex(@"WiimoteButtonStateOne"));
                range.SetStyle(InputStyle, new Regex(@"WiimoteButtonStateTwo"));
                range.SetStyle(InputStyle, new Regex(@"WiimoteButtonStateUp"));
                range.SetStyle(InputStyle, new Regex(@"WiimoteButtonStateDown"));
                range.SetStyle(InputStyle, new Regex(@"WiimoteButtonStateLeft"));
                range.SetStyle(InputStyle, new Regex(@"WiimoteButtonStateRight"));
                range.SetStyle(InputStyle, new Regex(@"WiimoteRawValuesX"));
                range.SetStyle(InputStyle, new Regex(@"WiimoteRawValuesY"));
                range.SetStyle(InputStyle, new Regex(@"WiimoteRawValuesZ"));
                range.SetStyle(InputStyle, new Regex(@"WiimoteNunchuckStateRawJoystickX"));
                range.SetStyle(InputStyle, new Regex(@"WiimoteNunchuckStateRawJoystickY"));
                range.SetStyle(InputStyle, new Regex(@"WiimoteNunchuckStateRawValuesX"));
                range.SetStyle(InputStyle, new Regex(@"WiimoteNunchuckStateRawValuesY"));
                range.SetStyle(InputStyle, new Regex(@"WiimoteNunchuckStateRawValuesZ"));
                range.SetStyle(InputStyle, new Regex(@"WiimoteNunchuckStateC"));
                range.SetStyle(InputStyle, new Regex(@"WiimoteNunchuckStateZ"));
                range.SetStyle(InputStyle, new Regex(@"JoyconRightStickX"));
                range.SetStyle(InputStyle, new Regex(@"JoyconRightStickY"));
                range.SetStyle(InputStyle, new Regex(@"JoyconRightButtonSHOULDER_1"));
                range.SetStyle(InputStyle, new Regex(@"JoyconRightButtonSHOULDER_2"));
                range.SetStyle(InputStyle, new Regex(@"JoyconRightButtonSR"));
                range.SetStyle(InputStyle, new Regex(@"JoyconRightButtonSL"));
                range.SetStyle(InputStyle, new Regex(@"JoyconRightButtonDPAD_DOWN"));
                range.SetStyle(InputStyle, new Regex(@"JoyconRightButtonDPAD_RIGHT"));
                range.SetStyle(InputStyle, new Regex(@"JoyconRightButtonDPAD_UP"));
                range.SetStyle(InputStyle, new Regex(@"JoyconRightButtonDPAD_LEFT"));
                range.SetStyle(InputStyle, new Regex(@"JoyconRightButtonPLUS"));
                range.SetStyle(InputStyle, new Regex(@"JoyconRightButtonHOME"));
                range.SetStyle(InputStyle, new Regex(@"JoyconRightButtonSTICK"));
                range.SetStyle(InputStyle, new Regex(@"JoyconRightButtonACC"));
                range.SetStyle(InputStyle, new Regex(@"JoyconRightButtonSPA"));
                range.SetStyle(InputStyle, new Regex(@"JoyconRightRollLeft"));
                range.SetStyle(InputStyle, new Regex(@"JoyconRightRollRight"));
                range.SetStyle(InputStyle, new Regex(@"JoyconRightAccelX"));
                range.SetStyle(InputStyle, new Regex(@"JoyconRightAccelY"));
                range.SetStyle(InputStyle, new Regex(@"JoyconRightGyroX"));
                range.SetStyle(InputStyle, new Regex(@"JoyconRightGyroY"));
                range.SetStyle(InputStyle, new Regex(@"JoyconLeftStickX"));
                range.SetStyle(InputStyle, new Regex(@"JoyconLeftStickY"));
                range.SetStyle(InputStyle, new Regex(@"JoyconLeftButtonSHOULDER_1"));
                range.SetStyle(InputStyle, new Regex(@"JoyconLeftButtonSHOULDER_2"));
                range.SetStyle(InputStyle, new Regex(@"JoyconLeftButtonSR"));
                range.SetStyle(InputStyle, new Regex(@"JoyconLeftButtonSL"));
                range.SetStyle(InputStyle, new Regex(@"JoyconLeftButtonDPAD_DOWN"));
                range.SetStyle(InputStyle, new Regex(@"JoyconLeftButtonDPAD_RIGHT"));
                range.SetStyle(InputStyle, new Regex(@"JoyconLeftButtonDPAD_UP"));
                range.SetStyle(InputStyle, new Regex(@"JoyconLeftButtonDPAD_LEFT"));
                range.SetStyle(InputStyle, new Regex(@"JoyconLeftButtonMINUS"));
                range.SetStyle(InputStyle, new Regex(@"JoyconLeftButtonCAPTURE"));
                range.SetStyle(InputStyle, new Regex(@"JoyconLeftButtonSTICK"));
                range.SetStyle(InputStyle, new Regex(@"JoyconLeftButtonACC"));
                range.SetStyle(InputStyle, new Regex(@"JoyconLeftButtonSMA"));
                range.SetStyle(InputStyle, new Regex(@"JoyconLeftRollLeft"));
                range.SetStyle(InputStyle, new Regex(@"JoyconLeftRollRight"));
                range.SetStyle(InputStyle, new Regex(@"JoyconLeftAccelX"));
                range.SetStyle(InputStyle, new Regex(@"JoyconLeftAccelY"));
                range.SetStyle(InputStyle, new Regex(@"JoyconLeftGyroX"));
                range.SetStyle(InputStyle, new Regex(@"JoyconLeftGyroY"));
                range.SetStyle(InputStyle, new Regex(@"ProControllerLeftStickX"));
                range.SetStyle(InputStyle, new Regex(@"ProControllerLeftStickY"));
                range.SetStyle(InputStyle, new Regex(@"ProControllerRightStickX"));
                range.SetStyle(InputStyle, new Regex(@"ProControllerRightStickY"));
                range.SetStyle(InputStyle, new Regex(@"ProControllerButtonSHOULDER_Left_1"));
                range.SetStyle(InputStyle, new Regex(@"ProControllerButtonSHOULDER_Left_2"));
                range.SetStyle(InputStyle, new Regex(@"ProControllerButtonDPAD_DOWN"));
                range.SetStyle(InputStyle, new Regex(@"ProControllerButtonDPAD_RIGHT"));
                range.SetStyle(InputStyle, new Regex(@"ProControllerButtonDPAD_UP"));
                range.SetStyle(InputStyle, new Regex(@"ProControllerButtonDPAD_LEFT"));
                range.SetStyle(InputStyle, new Regex(@"ProControllerButtonMINUS"));
                range.SetStyle(InputStyle, new Regex(@"ProControllerButtonCAPTURE"));
                range.SetStyle(InputStyle, new Regex(@"ProControllerButtonSTICK_Left"));
                range.SetStyle(InputStyle, new Regex(@"ProControllerButtonSHOULDER_Right_1"));
                range.SetStyle(InputStyle, new Regex(@"ProControllerButtonSHOULDER_Right_2"));
                range.SetStyle(InputStyle, new Regex(@"ProControllerButtonA"));
                range.SetStyle(InputStyle, new Regex(@"ProControllerButtonB"));
                range.SetStyle(InputStyle, new Regex(@"ProControllerButtonX"));
                range.SetStyle(InputStyle, new Regex(@"ProControllerButtonY"));
                range.SetStyle(InputStyle, new Regex(@"ProControllerButtonPLUS"));
                range.SetStyle(InputStyle, new Regex(@"ProControllerButtonHOME"));
                range.SetStyle(InputStyle, new Regex(@"ProControllerButtonSTICK_Right"));
                range.SetStyle(InputStyle, new Regex(@"ProControllerAccelX"));
                range.SetStyle(InputStyle, new Regex(@"ProControllerAccelY"));
                range.SetStyle(InputStyle, new Regex(@"ProControllerGyroX"));
                range.SetStyle(InputStyle, new Regex(@"ProControllerGyroY"));
                range.SetStyle(InputStyle, new Regex(@"camx"));
                range.SetStyle(InputStyle, new Regex(@"camy"));
                range.SetStyle(InputStyle, new Regex(@"Controller1ButtonAPressed"));
                range.SetStyle(InputStyle, new Regex(@"Controller2ButtonAPressed"));
                range.SetStyle(InputStyle, new Regex(@"Controller1ButtonBPressed"));
                range.SetStyle(InputStyle, new Regex(@"Controller2ButtonBPressed"));
                range.SetStyle(InputStyle, new Regex(@"Controller1ButtonXPressed"));
                range.SetStyle(InputStyle, new Regex(@"Controller2ButtonXPressed"));
                range.SetStyle(InputStyle, new Regex(@"Controller1ButtonYPressed"));
                range.SetStyle(InputStyle, new Regex(@"Controller2ButtonYPressed"));
                range.SetStyle(InputStyle, new Regex(@"Controller1ButtonStartPressed"));
                range.SetStyle(InputStyle, new Regex(@"Controller2ButtonStartPressed"));
                range.SetStyle(InputStyle, new Regex(@"Controller1ButtonBackPressed"));
                range.SetStyle(InputStyle, new Regex(@"Controller2ButtonBackPressed"));
                range.SetStyle(InputStyle, new Regex(@"Controller1ButtonDownPressed"));
                range.SetStyle(InputStyle, new Regex(@"Controller2ButtonDownPressed"));
                range.SetStyle(InputStyle, new Regex(@"Controller1ButtonUpPressed"));
                range.SetStyle(InputStyle, new Regex(@"Controller2ButtonUpPressed"));
                range.SetStyle(InputStyle, new Regex(@"Controller1ButtonLeftPressed"));
                range.SetStyle(InputStyle, new Regex(@"Controller2ButtonLeftPressed"));
                range.SetStyle(InputStyle, new Regex(@"Controller1ButtonRightPressed"));
                range.SetStyle(InputStyle, new Regex(@"Controller2ButtonRightPressed"));
                range.SetStyle(InputStyle, new Regex(@"Controller1ButtonShoulderLeftPressed"));
                range.SetStyle(InputStyle, new Regex(@"Controller2ButtonShoulderLeftPressed"));
                range.SetStyle(InputStyle, new Regex(@"Controller1ButtonShoulderRightPressed"));
                range.SetStyle(InputStyle, new Regex(@"Controller2ButtonShoulderRightPressed"));
                range.SetStyle(InputStyle, new Regex(@"Controller1ThumbpadLeftPressed"));
                range.SetStyle(InputStyle, new Regex(@"Controller2ThumbpadLeftPressed"));
                range.SetStyle(InputStyle, new Regex(@"Controller1ThumbpadRightPressed"));
                range.SetStyle(InputStyle, new Regex(@"Controller2ThumbpadRightPressed"));
                range.SetStyle(InputStyle, new Regex(@"Controller1TriggerLeftPosition"));
                range.SetStyle(InputStyle, new Regex(@"Controller2TriggerLeftPosition"));
                range.SetStyle(InputStyle, new Regex(@"Controller1TriggerRightPosition"));
                range.SetStyle(InputStyle, new Regex(@"Controller2TriggerRightPosition"));
                range.SetStyle(InputStyle, new Regex(@"Controller1ThumbLeftX"));
                range.SetStyle(InputStyle, new Regex(@"Controller2ThumbLeftX"));
                range.SetStyle(InputStyle, new Regex(@"Controller1ThumbLeftY"));
                range.SetStyle(InputStyle, new Regex(@"Controller2ThumbLeftY"));
                range.SetStyle(InputStyle, new Regex(@"Controller1ThumbRightX"));
                range.SetStyle(InputStyle, new Regex(@"Controller2ThumbRightX"));
                range.SetStyle(InputStyle, new Regex(@"Controller1ThumbRightY"));
                range.SetStyle(InputStyle, new Regex(@"Controller2ThumbRightY"));
                range.SetStyle(InputStyle, new Regex(@"Controller1GyroX"));
                range.SetStyle(InputStyle, new Regex(@"Controller1GyroY"));
                range.SetStyle(InputStyle, new Regex(@"Controller2GyroX"));
                range.SetStyle(InputStyle, new Regex(@"Controller2GyroY"));
                range.SetStyle(InputStyle, new Regex(@"Joystick1AxisX"));
                range.SetStyle(InputStyle, new Regex(@"Joystick1AxisY"));
                range.SetStyle(InputStyle, new Regex(@"Joystick1AxisZ"));
                range.SetStyle(InputStyle, new Regex(@"Joystick1RotationX"));
                range.SetStyle(InputStyle, new Regex(@"Joystick1RotationY"));
                range.SetStyle(InputStyle, new Regex(@"Joystick1RotationZ"));
                range.SetStyle(InputStyle, new Regex(@"Joystick1Sliders0"));
                range.SetStyle(InputStyle, new Regex(@"Joystick1Sliders1"));
                range.SetStyle(InputStyle, new Regex(@"Joystick1PointOfViewControllers0"));
                range.SetStyle(InputStyle, new Regex(@"Joystick1PointOfViewControllers1"));
                range.SetStyle(InputStyle, new Regex(@"Joystick1PointOfViewControllers2"));
                range.SetStyle(InputStyle, new Regex(@"Joystick1PointOfViewControllers3"));
                range.SetStyle(InputStyle, new Regex(@"Joystick1VelocityX"));
                range.SetStyle(InputStyle, new Regex(@"Joystick1VelocityY"));
                range.SetStyle(InputStyle, new Regex(@"Joystick1VelocityZ"));
                range.SetStyle(InputStyle, new Regex(@"Joystick1AngularVelocityX"));
                range.SetStyle(InputStyle, new Regex(@"Joystick1AngularVelocityY"));
                range.SetStyle(InputStyle, new Regex(@"Joystick1AngularVelocityZ"));
                range.SetStyle(InputStyle, new Regex(@"Joystick1VelocitySliders0"));
                range.SetStyle(InputStyle, new Regex(@"Joystick1VelocitySliders1"));
                range.SetStyle(InputStyle, new Regex(@"Joystick1AccelerationX"));
                range.SetStyle(InputStyle, new Regex(@"Joystick1AccelerationY"));
                range.SetStyle(InputStyle, new Regex(@"Joystick1AccelerationZ"));
                range.SetStyle(InputStyle, new Regex(@"Joystick1AngularAccelerationX"));
                range.SetStyle(InputStyle, new Regex(@"Joystick1AngularAccelerationY"));
                range.SetStyle(InputStyle, new Regex(@"Joystick1AngularAccelerationZ"));
                range.SetStyle(InputStyle, new Regex(@"Joystick1AccelerationSliders0"));
                range.SetStyle(InputStyle, new Regex(@"Joystick1AccelerationSliders1"));
                range.SetStyle(InputStyle, new Regex(@"Joystick1ForceX"));
                range.SetStyle(InputStyle, new Regex(@"Joystick1ForceY"));
                range.SetStyle(InputStyle, new Regex(@"Joystick1ForceZ"));
                range.SetStyle(InputStyle, new Regex(@"Joystick1TorqueX"));
                range.SetStyle(InputStyle, new Regex(@"Joystick1TorqueY"));
                range.SetStyle(InputStyle, new Regex(@"Joystick1TorqueZ"));
                range.SetStyle(InputStyle, new Regex(@"Joystick1ForceSliders0"));
                range.SetStyle(InputStyle, new Regex(@"Joystick1ForceSliders1"));
                range.SetStyle(InputStyle, new Regex(@"Joystick1Buttons0"));
                range.SetStyle(InputStyle, new Regex(@"Joystick1Buttons1"));
                range.SetStyle(InputStyle, new Regex(@"Joystick1Buttons2"));
                range.SetStyle(InputStyle, new Regex(@"Joystick1Buttons3"));
                range.SetStyle(InputStyle, new Regex(@"Joystick1Buttons4"));
                range.SetStyle(InputStyle, new Regex(@"Joystick1Buttons5"));
                range.SetStyle(InputStyle, new Regex(@"Joystick1Buttons6"));
                range.SetStyle(InputStyle, new Regex(@"Joystick1Buttons7"));
                range.SetStyle(InputStyle, new Regex(@"Joystick1Buttons8"));
                range.SetStyle(InputStyle, new Regex(@"Joystick1Buttons9"));
                range.SetStyle(InputStyle, new Regex(@"Joystick1Buttons10"));
                range.SetStyle(InputStyle, new Regex(@"Joystick1Buttons11"));
                range.SetStyle(InputStyle, new Regex(@"Joystick1Buttons12"));
                range.SetStyle(InputStyle, new Regex(@"Joystick1Buttons13"));
                range.SetStyle(InputStyle, new Regex(@"Joystick1Buttons14"));
                range.SetStyle(InputStyle, new Regex(@"Joystick1Buttons15"));
                range.SetStyle(InputStyle, new Regex(@"Joystick1Buttons16"));
                range.SetStyle(InputStyle, new Regex(@"Joystick1Buttons17"));
                range.SetStyle(InputStyle, new Regex(@"Joystick1Buttons18"));
                range.SetStyle(InputStyle, new Regex(@"Joystick1Buttons19"));
                range.SetStyle(InputStyle, new Regex(@"Joystick1Buttons20"));
                range.SetStyle(InputStyle, new Regex(@"Joystick1Buttons21"));
                range.SetStyle(InputStyle, new Regex(@"Joystick1Buttons22"));
                range.SetStyle(InputStyle, new Regex(@"Joystick1Buttons23"));
                range.SetStyle(InputStyle, new Regex(@"Joystick1Buttons24"));
                range.SetStyle(InputStyle, new Regex(@"Joystick1Buttons25"));
                range.SetStyle(InputStyle, new Regex(@"Joystick1Buttons26"));
                range.SetStyle(InputStyle, new Regex(@"Joystick1Buttons27"));
                range.SetStyle(InputStyle, new Regex(@"Joystick1Buttons28"));
                range.SetStyle(InputStyle, new Regex(@"Joystick1Buttons29"));
                range.SetStyle(InputStyle, new Regex(@"Joystick1Buttons30"));
                range.SetStyle(InputStyle, new Regex(@"Joystick1Buttons31"));
                range.SetStyle(InputStyle, new Regex(@"Joystick1Buttons32"));
                range.SetStyle(InputStyle, new Regex(@"Joystick1Buttons33"));
                range.SetStyle(InputStyle, new Regex(@"Joystick1Buttons34"));
                range.SetStyle(InputStyle, new Regex(@"Joystick1Buttons35"));
                range.SetStyle(InputStyle, new Regex(@"Joystick1Buttons36"));
                range.SetStyle(InputStyle, new Regex(@"Joystick1Buttons37"));
                range.SetStyle(InputStyle, new Regex(@"Joystick1Buttons38"));
                range.SetStyle(InputStyle, new Regex(@"Joystick1Buttons39"));
                range.SetStyle(InputStyle, new Regex(@"Joystick1Buttons40"));
                range.SetStyle(InputStyle, new Regex(@"Joystick1Buttons41"));
                range.SetStyle(InputStyle, new Regex(@"Joystick1Buttons42"));
                range.SetStyle(InputStyle, new Regex(@"Joystick1Buttons43"));
                range.SetStyle(InputStyle, new Regex(@"Joystick1Buttons44"));
                range.SetStyle(InputStyle, new Regex(@"Joystick1Buttons45"));
                range.SetStyle(InputStyle, new Regex(@"Joystick1Buttons46"));
                range.SetStyle(InputStyle, new Regex(@"Joystick1Buttons47"));
                range.SetStyle(InputStyle, new Regex(@"Joystick1Buttons48"));
                range.SetStyle(InputStyle, new Regex(@"Joystick1Buttons49"));
                range.SetStyle(InputStyle, new Regex(@"Joystick1Buttons50"));
                range.SetStyle(InputStyle, new Regex(@"Joystick1Buttons51"));
                range.SetStyle(InputStyle, new Regex(@"Joystick1Buttons52"));
                range.SetStyle(InputStyle, new Regex(@"Joystick1Buttons53"));
                range.SetStyle(InputStyle, new Regex(@"Joystick1Buttons54"));
                range.SetStyle(InputStyle, new Regex(@"Joystick1Buttons55"));
                range.SetStyle(InputStyle, new Regex(@"Joystick1Buttons56"));
                range.SetStyle(InputStyle, new Regex(@"Joystick1Buttons57"));
                range.SetStyle(InputStyle, new Regex(@"Joystick1Buttons58"));
                range.SetStyle(InputStyle, new Regex(@"Joystick1Buttons59"));
                range.SetStyle(InputStyle, new Regex(@"Joystick1Buttons60"));
                range.SetStyle(InputStyle, new Regex(@"Joystick1Buttons61"));
                range.SetStyle(InputStyle, new Regex(@"Joystick1Buttons62"));
                range.SetStyle(InputStyle, new Regex(@"Joystick1Buttons63"));
                range.SetStyle(InputStyle, new Regex(@"Joystick1Buttons64"));
                range.SetStyle(InputStyle, new Regex(@"Joystick1Buttons65"));
                range.SetStyle(InputStyle, new Regex(@"Joystick1Buttons66"));
                range.SetStyle(InputStyle, new Regex(@"Joystick1Buttons67"));
                range.SetStyle(InputStyle, new Regex(@"Joystick1Buttons68"));
                range.SetStyle(InputStyle, new Regex(@"Joystick1Buttons69"));
                range.SetStyle(InputStyle, new Regex(@"Joystick1Buttons70"));
                range.SetStyle(InputStyle, new Regex(@"Joystick1Buttons71"));
                range.SetStyle(InputStyle, new Regex(@"Joystick1Buttons72"));
                range.SetStyle(InputStyle, new Regex(@"Joystick1Buttons73"));
                range.SetStyle(InputStyle, new Regex(@"Joystick1Buttons74"));
                range.SetStyle(InputStyle, new Regex(@"Joystick1Buttons75"));
                range.SetStyle(InputStyle, new Regex(@"Joystick1Buttons76"));
                range.SetStyle(InputStyle, new Regex(@"Joystick1Buttons77"));
                range.SetStyle(InputStyle, new Regex(@"Joystick1Buttons78"));
                range.SetStyle(InputStyle, new Regex(@"Joystick1Buttons79"));
                range.SetStyle(InputStyle, new Regex(@"Joystick1Buttons80"));
                range.SetStyle(InputStyle, new Regex(@"Joystick1Buttons81"));
                range.SetStyle(InputStyle, new Regex(@"Joystick1Buttons82"));
                range.SetStyle(InputStyle, new Regex(@"Joystick1Buttons83"));
                range.SetStyle(InputStyle, new Regex(@"Joystick1Buttons84"));
                range.SetStyle(InputStyle, new Regex(@"Joystick1Buttons85"));
                range.SetStyle(InputStyle, new Regex(@"Joystick1Buttons86"));
                range.SetStyle(InputStyle, new Regex(@"Joystick1Buttons87"));
                range.SetStyle(InputStyle, new Regex(@"Joystick1Buttons88"));
                range.SetStyle(InputStyle, new Regex(@"Joystick1Buttons89"));
                range.SetStyle(InputStyle, new Regex(@"Joystick1Buttons90"));
                range.SetStyle(InputStyle, new Regex(@"Joystick1Buttons91"));
                range.SetStyle(InputStyle, new Regex(@"Joystick1Buttons92"));
                range.SetStyle(InputStyle, new Regex(@"Joystick1Buttons93"));
                range.SetStyle(InputStyle, new Regex(@"Joystick1Buttons94"));
                range.SetStyle(InputStyle, new Regex(@"Joystick1Buttons95"));
                range.SetStyle(InputStyle, new Regex(@"Joystick1Buttons96"));
                range.SetStyle(InputStyle, new Regex(@"Joystick1Buttons97"));
                range.SetStyle(InputStyle, new Regex(@"Joystick1Buttons98"));
                range.SetStyle(InputStyle, new Regex(@"Joystick1Buttons99"));
                range.SetStyle(InputStyle, new Regex(@"Joystick1Buttons100"));
                range.SetStyle(InputStyle, new Regex(@"Joystick1Buttons101"));
                range.SetStyle(InputStyle, new Regex(@"Joystick1Buttons102"));
                range.SetStyle(InputStyle, new Regex(@"Joystick1Buttons103"));
                range.SetStyle(InputStyle, new Regex(@"Joystick1Buttons104"));
                range.SetStyle(InputStyle, new Regex(@"Joystick1Buttons105"));
                range.SetStyle(InputStyle, new Regex(@"Joystick1Buttons106"));
                range.SetStyle(InputStyle, new Regex(@"Joystick1Buttons107"));
                range.SetStyle(InputStyle, new Regex(@"Joystick1Buttons108"));
                range.SetStyle(InputStyle, new Regex(@"Joystick1Buttons109"));
                range.SetStyle(InputStyle, new Regex(@"Joystick1Buttons110"));
                range.SetStyle(InputStyle, new Regex(@"Joystick1Buttons111"));
                range.SetStyle(InputStyle, new Regex(@"Joystick1Buttons112"));
                range.SetStyle(InputStyle, new Regex(@"Joystick1Buttons113"));
                range.SetStyle(InputStyle, new Regex(@"Joystick1Buttons114"));
                range.SetStyle(InputStyle, new Regex(@"Joystick1Buttons115"));
                range.SetStyle(InputStyle, new Regex(@"Joystick1Buttons116"));
                range.SetStyle(InputStyle, new Regex(@"Joystick1Buttons117"));
                range.SetStyle(InputStyle, new Regex(@"Joystick1Buttons118"));
                range.SetStyle(InputStyle, new Regex(@"Joystick1Buttons119"));
                range.SetStyle(InputStyle, new Regex(@"Joystick1Buttons120"));
                range.SetStyle(InputStyle, new Regex(@"Joystick1Buttons121"));
                range.SetStyle(InputStyle, new Regex(@"Joystick1Buttons122"));
                range.SetStyle(InputStyle, new Regex(@"Joystick1Buttons123"));
                range.SetStyle(InputStyle, new Regex(@"Joystick1Buttons124"));
                range.SetStyle(InputStyle, new Regex(@"Joystick1Buttons125"));
                range.SetStyle(InputStyle, new Regex(@"Joystick1Buttons126"));
                range.SetStyle(InputStyle, new Regex(@"Joystick1Buttons127"));
                range.SetStyle(InputStyle, new Regex(@"Joystick2AxisX"));
                range.SetStyle(InputStyle, new Regex(@"Joystick2AxisY"));
                range.SetStyle(InputStyle, new Regex(@"Joystick2AxisZ"));
                range.SetStyle(InputStyle, new Regex(@"Joystick2RotationX"));
                range.SetStyle(InputStyle, new Regex(@"Joystick2RotationY"));
                range.SetStyle(InputStyle, new Regex(@"Joystick2RotationZ"));
                range.SetStyle(InputStyle, new Regex(@"Joystick2Sliders0"));
                range.SetStyle(InputStyle, new Regex(@"Joystick2Sliders1"));
                range.SetStyle(InputStyle, new Regex(@"Joystick2PointOfViewControllers0"));
                range.SetStyle(InputStyle, new Regex(@"Joystick2PointOfViewControllers1"));
                range.SetStyle(InputStyle, new Regex(@"Joystick2PointOfViewControllers2"));
                range.SetStyle(InputStyle, new Regex(@"Joystick2PointOfViewControllers3"));
                range.SetStyle(InputStyle, new Regex(@"Joystick2VelocityX"));
                range.SetStyle(InputStyle, new Regex(@"Joystick2VelocityY"));
                range.SetStyle(InputStyle, new Regex(@"Joystick2VelocityZ"));
                range.SetStyle(InputStyle, new Regex(@"Joystick2AngularVelocityX"));
                range.SetStyle(InputStyle, new Regex(@"Joystick2AngularVelocityY"));
                range.SetStyle(InputStyle, new Regex(@"Joystick2AngularVelocityZ"));
                range.SetStyle(InputStyle, new Regex(@"Joystick2VelocitySliders0"));
                range.SetStyle(InputStyle, new Regex(@"Joystick2VelocitySliders1"));
                range.SetStyle(InputStyle, new Regex(@"Joystick2AccelerationX"));
                range.SetStyle(InputStyle, new Regex(@"Joystick2AccelerationY"));
                range.SetStyle(InputStyle, new Regex(@"Joystick2AccelerationZ"));
                range.SetStyle(InputStyle, new Regex(@"Joystick2AngularAccelerationX"));
                range.SetStyle(InputStyle, new Regex(@"Joystick2AngularAccelerationY"));
                range.SetStyle(InputStyle, new Regex(@"Joystick2AngularAccelerationZ"));
                range.SetStyle(InputStyle, new Regex(@"Joystick2AccelerationSliders0"));
                range.SetStyle(InputStyle, new Regex(@"Joystick2AccelerationSliders1"));
                range.SetStyle(InputStyle, new Regex(@"Joystick2ForceX"));
                range.SetStyle(InputStyle, new Regex(@"Joystick2ForceY"));
                range.SetStyle(InputStyle, new Regex(@"Joystick2ForceZ"));
                range.SetStyle(InputStyle, new Regex(@"Joystick2TorqueX"));
                range.SetStyle(InputStyle, new Regex(@"Joystick2TorqueY"));
                range.SetStyle(InputStyle, new Regex(@"Joystick2TorqueZ"));
                range.SetStyle(InputStyle, new Regex(@"Joystick2ForceSliders0"));
                range.SetStyle(InputStyle, new Regex(@"Joystick2ForceSliders1"));
                range.SetStyle(InputStyle, new Regex(@"Joystick2Buttons0"));
                range.SetStyle(InputStyle, new Regex(@"Joystick2Buttons1"));
                range.SetStyle(InputStyle, new Regex(@"Joystick2Buttons2"));
                range.SetStyle(InputStyle, new Regex(@"Joystick2Buttons3"));
                range.SetStyle(InputStyle, new Regex(@"Joystick2Buttons4"));
                range.SetStyle(InputStyle, new Regex(@"Joystick2Buttons5"));
                range.SetStyle(InputStyle, new Regex(@"Joystick2Buttons6"));
                range.SetStyle(InputStyle, new Regex(@"Joystick2Buttons7"));
                range.SetStyle(InputStyle, new Regex(@"Joystick2Buttons8"));
                range.SetStyle(InputStyle, new Regex(@"Joystick2Buttons9"));
                range.SetStyle(InputStyle, new Regex(@"Joystick2Buttons10"));
                range.SetStyle(InputStyle, new Regex(@"Joystick2Buttons11"));
                range.SetStyle(InputStyle, new Regex(@"Joystick2Buttons12"));
                range.SetStyle(InputStyle, new Regex(@"Joystick2Buttons13"));
                range.SetStyle(InputStyle, new Regex(@"Joystick2Buttons14"));
                range.SetStyle(InputStyle, new Regex(@"Joystick2Buttons15"));
                range.SetStyle(InputStyle, new Regex(@"Joystick2Buttons16"));
                range.SetStyle(InputStyle, new Regex(@"Joystick2Buttons17"));
                range.SetStyle(InputStyle, new Regex(@"Joystick2Buttons18"));
                range.SetStyle(InputStyle, new Regex(@"Joystick2Buttons19"));
                range.SetStyle(InputStyle, new Regex(@"Joystick2Buttons20"));
                range.SetStyle(InputStyle, new Regex(@"Joystick2Buttons21"));
                range.SetStyle(InputStyle, new Regex(@"Joystick2Buttons22"));
                range.SetStyle(InputStyle, new Regex(@"Joystick2Buttons23"));
                range.SetStyle(InputStyle, new Regex(@"Joystick2Buttons24"));
                range.SetStyle(InputStyle, new Regex(@"Joystick2Buttons25"));
                range.SetStyle(InputStyle, new Regex(@"Joystick2Buttons26"));
                range.SetStyle(InputStyle, new Regex(@"Joystick2Buttons27"));
                range.SetStyle(InputStyle, new Regex(@"Joystick2Buttons28"));
                range.SetStyle(InputStyle, new Regex(@"Joystick2Buttons29"));
                range.SetStyle(InputStyle, new Regex(@"Joystick2Buttons30"));
                range.SetStyle(InputStyle, new Regex(@"Joystick2Buttons31"));
                range.SetStyle(InputStyle, new Regex(@"Joystick2Buttons32"));
                range.SetStyle(InputStyle, new Regex(@"Joystick2Buttons33"));
                range.SetStyle(InputStyle, new Regex(@"Joystick2Buttons34"));
                range.SetStyle(InputStyle, new Regex(@"Joystick2Buttons35"));
                range.SetStyle(InputStyle, new Regex(@"Joystick2Buttons36"));
                range.SetStyle(InputStyle, new Regex(@"Joystick2Buttons37"));
                range.SetStyle(InputStyle, new Regex(@"Joystick2Buttons38"));
                range.SetStyle(InputStyle, new Regex(@"Joystick2Buttons39"));
                range.SetStyle(InputStyle, new Regex(@"Joystick2Buttons40"));
                range.SetStyle(InputStyle, new Regex(@"Joystick2Buttons41"));
                range.SetStyle(InputStyle, new Regex(@"Joystick2Buttons42"));
                range.SetStyle(InputStyle, new Regex(@"Joystick2Buttons43"));
                range.SetStyle(InputStyle, new Regex(@"Joystick2Buttons44"));
                range.SetStyle(InputStyle, new Regex(@"Joystick2Buttons45"));
                range.SetStyle(InputStyle, new Regex(@"Joystick2Buttons46"));
                range.SetStyle(InputStyle, new Regex(@"Joystick2Buttons47"));
                range.SetStyle(InputStyle, new Regex(@"Joystick2Buttons48"));
                range.SetStyle(InputStyle, new Regex(@"Joystick2Buttons49"));
                range.SetStyle(InputStyle, new Regex(@"Joystick2Buttons50"));
                range.SetStyle(InputStyle, new Regex(@"Joystick2Buttons51"));
                range.SetStyle(InputStyle, new Regex(@"Joystick2Buttons52"));
                range.SetStyle(InputStyle, new Regex(@"Joystick2Buttons53"));
                range.SetStyle(InputStyle, new Regex(@"Joystick2Buttons54"));
                range.SetStyle(InputStyle, new Regex(@"Joystick2Buttons55"));
                range.SetStyle(InputStyle, new Regex(@"Joystick2Buttons56"));
                range.SetStyle(InputStyle, new Regex(@"Joystick2Buttons57"));
                range.SetStyle(InputStyle, new Regex(@"Joystick2Buttons58"));
                range.SetStyle(InputStyle, new Regex(@"Joystick2Buttons59"));
                range.SetStyle(InputStyle, new Regex(@"Joystick2Buttons60"));
                range.SetStyle(InputStyle, new Regex(@"Joystick2Buttons61"));
                range.SetStyle(InputStyle, new Regex(@"Joystick2Buttons62"));
                range.SetStyle(InputStyle, new Regex(@"Joystick2Buttons63"));
                range.SetStyle(InputStyle, new Regex(@"Joystick2Buttons64"));
                range.SetStyle(InputStyle, new Regex(@"Joystick2Buttons65"));
                range.SetStyle(InputStyle, new Regex(@"Joystick2Buttons66"));
                range.SetStyle(InputStyle, new Regex(@"Joystick2Buttons67"));
                range.SetStyle(InputStyle, new Regex(@"Joystick2Buttons68"));
                range.SetStyle(InputStyle, new Regex(@"Joystick2Buttons69"));
                range.SetStyle(InputStyle, new Regex(@"Joystick2Buttons70"));
                range.SetStyle(InputStyle, new Regex(@"Joystick2Buttons71"));
                range.SetStyle(InputStyle, new Regex(@"Joystick2Buttons72"));
                range.SetStyle(InputStyle, new Regex(@"Joystick2Buttons73"));
                range.SetStyle(InputStyle, new Regex(@"Joystick2Buttons74"));
                range.SetStyle(InputStyle, new Regex(@"Joystick2Buttons75"));
                range.SetStyle(InputStyle, new Regex(@"Joystick2Buttons76"));
                range.SetStyle(InputStyle, new Regex(@"Joystick2Buttons77"));
                range.SetStyle(InputStyle, new Regex(@"Joystick2Buttons78"));
                range.SetStyle(InputStyle, new Regex(@"Joystick2Buttons79"));
                range.SetStyle(InputStyle, new Regex(@"Joystick2Buttons80"));
                range.SetStyle(InputStyle, new Regex(@"Joystick2Buttons81"));
                range.SetStyle(InputStyle, new Regex(@"Joystick2Buttons82"));
                range.SetStyle(InputStyle, new Regex(@"Joystick2Buttons83"));
                range.SetStyle(InputStyle, new Regex(@"Joystick2Buttons84"));
                range.SetStyle(InputStyle, new Regex(@"Joystick2Buttons85"));
                range.SetStyle(InputStyle, new Regex(@"Joystick2Buttons86"));
                range.SetStyle(InputStyle, new Regex(@"Joystick2Buttons87"));
                range.SetStyle(InputStyle, new Regex(@"Joystick2Buttons88"));
                range.SetStyle(InputStyle, new Regex(@"Joystick2Buttons89"));
                range.SetStyle(InputStyle, new Regex(@"Joystick2Buttons90"));
                range.SetStyle(InputStyle, new Regex(@"Joystick2Buttons91"));
                range.SetStyle(InputStyle, new Regex(@"Joystick2Buttons92"));
                range.SetStyle(InputStyle, new Regex(@"Joystick2Buttons93"));
                range.SetStyle(InputStyle, new Regex(@"Joystick2Buttons94"));
                range.SetStyle(InputStyle, new Regex(@"Joystick2Buttons95"));
                range.SetStyle(InputStyle, new Regex(@"Joystick2Buttons96"));
                range.SetStyle(InputStyle, new Regex(@"Joystick2Buttons97"));
                range.SetStyle(InputStyle, new Regex(@"Joystick2Buttons98"));
                range.SetStyle(InputStyle, new Regex(@"Joystick2Buttons99"));
                range.SetStyle(InputStyle, new Regex(@"Joystick2Buttons100"));
                range.SetStyle(InputStyle, new Regex(@"Joystick2Buttons101"));
                range.SetStyle(InputStyle, new Regex(@"Joystick2Buttons102"));
                range.SetStyle(InputStyle, new Regex(@"Joystick2Buttons103"));
                range.SetStyle(InputStyle, new Regex(@"Joystick2Buttons104"));
                range.SetStyle(InputStyle, new Regex(@"Joystick2Buttons105"));
                range.SetStyle(InputStyle, new Regex(@"Joystick2Buttons106"));
                range.SetStyle(InputStyle, new Regex(@"Joystick2Buttons107"));
                range.SetStyle(InputStyle, new Regex(@"Joystick2Buttons108"));
                range.SetStyle(InputStyle, new Regex(@"Joystick2Buttons109"));
                range.SetStyle(InputStyle, new Regex(@"Joystick2Buttons110"));
                range.SetStyle(InputStyle, new Regex(@"Joystick2Buttons111"));
                range.SetStyle(InputStyle, new Regex(@"Joystick2Buttons112"));
                range.SetStyle(InputStyle, new Regex(@"Joystick2Buttons113"));
                range.SetStyle(InputStyle, new Regex(@"Joystick2Buttons114"));
                range.SetStyle(InputStyle, new Regex(@"Joystick2Buttons115"));
                range.SetStyle(InputStyle, new Regex(@"Joystick2Buttons116"));
                range.SetStyle(InputStyle, new Regex(@"Joystick2Buttons117"));
                range.SetStyle(InputStyle, new Regex(@"Joystick2Buttons118"));
                range.SetStyle(InputStyle, new Regex(@"Joystick2Buttons119"));
                range.SetStyle(InputStyle, new Regex(@"Joystick2Buttons120"));
                range.SetStyle(InputStyle, new Regex(@"Joystick2Buttons121"));
                range.SetStyle(InputStyle, new Regex(@"Joystick2Buttons122"));
                range.SetStyle(InputStyle, new Regex(@"Joystick2Buttons123"));
                range.SetStyle(InputStyle, new Regex(@"Joystick2Buttons124"));
                range.SetStyle(InputStyle, new Regex(@"Joystick2Buttons125"));
                range.SetStyle(InputStyle, new Regex(@"Joystick2Buttons126"));
                range.SetStyle(InputStyle, new Regex(@"Joystick2Buttons127"));
                range.SetStyle(InputStyle, new Regex(@"Mouse1Buttons0"));
                range.SetStyle(InputStyle, new Regex(@"Mouse1Buttons1"));
                range.SetStyle(InputStyle, new Regex(@"Mouse1Buttons2"));
                range.SetStyle(InputStyle, new Regex(@"Mouse1Buttons3"));
                range.SetStyle(InputStyle, new Regex(@"Mouse1Buttons4"));
                range.SetStyle(InputStyle, new Regex(@"Mouse1Buttons5"));
                range.SetStyle(InputStyle, new Regex(@"Mouse1Buttons6"));
                range.SetStyle(InputStyle, new Regex(@"Mouse1Buttons7"));
                range.SetStyle(InputStyle, new Regex(@"Mouse1AxisX"));
                range.SetStyle(InputStyle, new Regex(@"Mouse1AxisY"));
                range.SetStyle(InputStyle, new Regex(@"Mouse1AxisZ"));
                range.SetStyle(InputStyle, new Regex(@"MouseHookX"));
                range.SetStyle(InputStyle, new Regex(@"MouseHookY"));
                range.SetStyle(InputStyle, new Regex(@"Mouse2Buttons0"));
                range.SetStyle(InputStyle, new Regex(@"Mouse2Buttons1"));
                range.SetStyle(InputStyle, new Regex(@"Mouse2Buttons2"));
                range.SetStyle(InputStyle, new Regex(@"Mouse2Buttons3"));
                range.SetStyle(InputStyle, new Regex(@"Mouse2Buttons4"));
                range.SetStyle(InputStyle, new Regex(@"Mouse2Buttons5"));
                range.SetStyle(InputStyle, new Regex(@"Mouse2Buttons6"));
                range.SetStyle(InputStyle, new Regex(@"Mouse2Buttons7"));
                range.SetStyle(InputStyle, new Regex(@"Mouse2AxisX"));
                range.SetStyle(InputStyle, new Regex(@"Mouse2AxisY"));
                range.SetStyle(InputStyle, new Regex(@"Mouse2AxisZ"));
                range.SetStyle(InputStyle, new Regex(@"Keyboard1KeyEscape"));
                range.SetStyle(InputStyle, new Regex(@"Keyboard1KeyD1"));
                range.SetStyle(InputStyle, new Regex(@"Keyboard1KeyD2"));
                range.SetStyle(InputStyle, new Regex(@"Keyboard1KeyD3"));
                range.SetStyle(InputStyle, new Regex(@"Keyboard1KeyD4"));
                range.SetStyle(InputStyle, new Regex(@"Keyboard1KeyD5"));
                range.SetStyle(InputStyle, new Regex(@"Keyboard1KeyD6"));
                range.SetStyle(InputStyle, new Regex(@"Keyboard1KeyD7"));
                range.SetStyle(InputStyle, new Regex(@"Keyboard1KeyD8"));
                range.SetStyle(InputStyle, new Regex(@"Keyboard1KeyD9"));
                range.SetStyle(InputStyle, new Regex(@"Keyboard1KeyD0"));
                range.SetStyle(InputStyle, new Regex(@"Keyboard1KeyMinus"));
                range.SetStyle(InputStyle, new Regex(@"Keyboard1KeyEquals"));
                range.SetStyle(InputStyle, new Regex(@"Keyboard1KeyBack"));
                range.SetStyle(InputStyle, new Regex(@"Keyboard1KeyTab"));
                range.SetStyle(InputStyle, new Regex(@"Keyboard1KeyQ"));
                range.SetStyle(InputStyle, new Regex(@"Keyboard1KeyW"));
                range.SetStyle(InputStyle, new Regex(@"Keyboard1KeyE"));
                range.SetStyle(InputStyle, new Regex(@"Keyboard1KeyR"));
                range.SetStyle(InputStyle, new Regex(@"Keyboard1KeyT"));
                range.SetStyle(InputStyle, new Regex(@"Keyboard1KeyY"));
                range.SetStyle(InputStyle, new Regex(@"Keyboard1KeyU"));
                range.SetStyle(InputStyle, new Regex(@"Keyboard1KeyI"));
                range.SetStyle(InputStyle, new Regex(@"Keyboard1KeyO"));
                range.SetStyle(InputStyle, new Regex(@"Keyboard1KeyP"));
                range.SetStyle(InputStyle, new Regex(@"Keyboard1KeyLeftBracket"));
                range.SetStyle(InputStyle, new Regex(@"Keyboard1KeyRightBracket"));
                range.SetStyle(InputStyle, new Regex(@"Keyboard1KeyReturn"));
                range.SetStyle(InputStyle, new Regex(@"Keyboard1KeyLeftControl"));
                range.SetStyle(InputStyle, new Regex(@"Keyboard1KeyA"));
                range.SetStyle(InputStyle, new Regex(@"Keyboard1KeyS"));
                range.SetStyle(InputStyle, new Regex(@"Keyboard1KeyD"));
                range.SetStyle(InputStyle, new Regex(@"Keyboard1KeyF"));
                range.SetStyle(InputStyle, new Regex(@"Keyboard1KeyG"));
                range.SetStyle(InputStyle, new Regex(@"Keyboard1KeyH"));
                range.SetStyle(InputStyle, new Regex(@"Keyboard1KeyJ"));
                range.SetStyle(InputStyle, new Regex(@"Keyboard1KeyK"));
                range.SetStyle(InputStyle, new Regex(@"Keyboard1KeyL"));
                range.SetStyle(InputStyle, new Regex(@"Keyboard1KeySemicolon"));
                range.SetStyle(InputStyle, new Regex(@"Keyboard1KeyApostrophe"));
                range.SetStyle(InputStyle, new Regex(@"Keyboard1KeyGrave"));
                range.SetStyle(InputStyle, new Regex(@"Keyboard1KeyLeftShift"));
                range.SetStyle(InputStyle, new Regex(@"Keyboard1KeyBackslash"));
                range.SetStyle(InputStyle, new Regex(@"Keyboard1KeyZ"));
                range.SetStyle(InputStyle, new Regex(@"Keyboard1KeyX"));
                range.SetStyle(InputStyle, new Regex(@"Keyboard1KeyC"));
                range.SetStyle(InputStyle, new Regex(@"Keyboard1KeyV"));
                range.SetStyle(InputStyle, new Regex(@"Keyboard1KeyB"));
                range.SetStyle(InputStyle, new Regex(@"Keyboard1KeyN"));
                range.SetStyle(InputStyle, new Regex(@"Keyboard1KeyM"));
                range.SetStyle(InputStyle, new Regex(@"Keyboard1KeyComma"));
                range.SetStyle(InputStyle, new Regex(@"Keyboard1KeyPeriod"));
                range.SetStyle(InputStyle, new Regex(@"Keyboard1KeySlash"));
                range.SetStyle(InputStyle, new Regex(@"Keyboard1KeyRightShift"));
                range.SetStyle(InputStyle, new Regex(@"Keyboard1KeyMultiply"));
                range.SetStyle(InputStyle, new Regex(@"Keyboard1KeyLeftAlt"));
                range.SetStyle(InputStyle, new Regex(@"Keyboard1KeySpace"));
                range.SetStyle(InputStyle, new Regex(@"Keyboard1KeyCapital"));
                range.SetStyle(InputStyle, new Regex(@"Keyboard1KeyF1"));
                range.SetStyle(InputStyle, new Regex(@"Keyboard1KeyF2"));
                range.SetStyle(InputStyle, new Regex(@"Keyboard1KeyF3"));
                range.SetStyle(InputStyle, new Regex(@"Keyboard1KeyF4"));
                range.SetStyle(InputStyle, new Regex(@"Keyboard1KeyF5"));
                range.SetStyle(InputStyle, new Regex(@"Keyboard1KeyF6"));
                range.SetStyle(InputStyle, new Regex(@"Keyboard1KeyF7"));
                range.SetStyle(InputStyle, new Regex(@"Keyboard1KeyF8"));
                range.SetStyle(InputStyle, new Regex(@"Keyboard1KeyF9"));
                range.SetStyle(InputStyle, new Regex(@"Keyboard1KeyF10"));
                range.SetStyle(InputStyle, new Regex(@"Keyboard1KeyNumberLock"));
                range.SetStyle(InputStyle, new Regex(@"Keyboard1KeyScrollLock"));
                range.SetStyle(InputStyle, new Regex(@"Keyboard1KeyNumberPad7"));
                range.SetStyle(InputStyle, new Regex(@"Keyboard1KeyNumberPad8"));
                range.SetStyle(InputStyle, new Regex(@"Keyboard1KeyNumberPad9"));
                range.SetStyle(InputStyle, new Regex(@"Keyboard1KeySubtract"));
                range.SetStyle(InputStyle, new Regex(@"Keyboard1KeyNumberPad4"));
                range.SetStyle(InputStyle, new Regex(@"Keyboard1KeyNumberPad5"));
                range.SetStyle(InputStyle, new Regex(@"Keyboard1KeyNumberPad6"));
                range.SetStyle(InputStyle, new Regex(@"Keyboard1KeyAdd"));
                range.SetStyle(InputStyle, new Regex(@"Keyboard1KeyNumberPad1"));
                range.SetStyle(InputStyle, new Regex(@"Keyboard1KeyNumberPad2"));
                range.SetStyle(InputStyle, new Regex(@"Keyboard1KeyNumberPad3"));
                range.SetStyle(InputStyle, new Regex(@"Keyboard1KeyNumberPad0"));
                range.SetStyle(InputStyle, new Regex(@"Keyboard1KeyDecimal"));
                range.SetStyle(InputStyle, new Regex(@"Keyboard1KeyOem102"));
                range.SetStyle(InputStyle, new Regex(@"Keyboard1KeyF11"));
                range.SetStyle(InputStyle, new Regex(@"Keyboard1KeyF12"));
                range.SetStyle(InputStyle, new Regex(@"Keyboard1KeyF13"));
                range.SetStyle(InputStyle, new Regex(@"Keyboard1KeyF14"));
                range.SetStyle(InputStyle, new Regex(@"Keyboard1KeyF15"));
                range.SetStyle(InputStyle, new Regex(@"Keyboard1KeyKana"));
                range.SetStyle(InputStyle, new Regex(@"Keyboard1KeyAbntC1"));
                range.SetStyle(InputStyle, new Regex(@"Keyboard1KeyConvert"));
                range.SetStyle(InputStyle, new Regex(@"Keyboard1KeyNoConvert"));
                range.SetStyle(InputStyle, new Regex(@"Keyboard1KeyYen"));
                range.SetStyle(InputStyle, new Regex(@"Keyboard1KeyAbntC2"));
                range.SetStyle(InputStyle, new Regex(@"Keyboard1KeyNumberPadEquals"));
                range.SetStyle(InputStyle, new Regex(@"Keyboard1KeyPreviousTrack"));
                range.SetStyle(InputStyle, new Regex(@"Keyboard1KeyAT"));
                range.SetStyle(InputStyle, new Regex(@"Keyboard1KeyColon"));
                range.SetStyle(InputStyle, new Regex(@"Keyboard1KeyUnderline"));
                range.SetStyle(InputStyle, new Regex(@"Keyboard1KeyKanji"));
                range.SetStyle(InputStyle, new Regex(@"Keyboard1KeyStop"));
                range.SetStyle(InputStyle, new Regex(@"Keyboard1KeyAX"));
                range.SetStyle(InputStyle, new Regex(@"Keyboard1KeyUnlabeled"));
                range.SetStyle(InputStyle, new Regex(@"Keyboard1KeyNextTrack"));
                range.SetStyle(InputStyle, new Regex(@"Keyboard1KeyNumberPadEnter"));
                range.SetStyle(InputStyle, new Regex(@"Keyboard1KeyRightControl"));
                range.SetStyle(InputStyle, new Regex(@"Keyboard1KeyMute"));
                range.SetStyle(InputStyle, new Regex(@"Keyboard1KeyCalculator"));
                range.SetStyle(InputStyle, new Regex(@"Keyboard1KeyPlayPause"));
                range.SetStyle(InputStyle, new Regex(@"Keyboard1KeyMediaStop"));
                range.SetStyle(InputStyle, new Regex(@"Keyboard1KeyVolumeDown"));
                range.SetStyle(InputStyle, new Regex(@"Keyboard1KeyVolumeUp"));
                range.SetStyle(InputStyle, new Regex(@"Keyboard1KeyWebHome"));
                range.SetStyle(InputStyle, new Regex(@"Keyboard1KeyNumberPadComma"));
                range.SetStyle(InputStyle, new Regex(@"Keyboard1KeyDivide"));
                range.SetStyle(InputStyle, new Regex(@"Keyboard1KeyPrintScreen"));
                range.SetStyle(InputStyle, new Regex(@"Keyboard1KeyRightAlt"));
                range.SetStyle(InputStyle, new Regex(@"Keyboard1KeyPause"));
                range.SetStyle(InputStyle, new Regex(@"Keyboard1KeyHome"));
                range.SetStyle(InputStyle, new Regex(@"Keyboard1KeyUp"));
                range.SetStyle(InputStyle, new Regex(@"Keyboard1KeyPageUp"));
                range.SetStyle(InputStyle, new Regex(@"Keyboard1KeyLeft"));
                range.SetStyle(InputStyle, new Regex(@"Keyboard1KeyRight"));
                range.SetStyle(InputStyle, new Regex(@"Keyboard1KeyEnd"));
                range.SetStyle(InputStyle, new Regex(@"Keyboard1KeyDown"));
                range.SetStyle(InputStyle, new Regex(@"Keyboard1KeyPageDown"));
                range.SetStyle(InputStyle, new Regex(@"Keyboard1KeyInsert"));
                range.SetStyle(InputStyle, new Regex(@"Keyboard1KeyDelete"));
                range.SetStyle(InputStyle, new Regex(@"Keyboard1KeyLeftWindowsKey"));
                range.SetStyle(InputStyle, new Regex(@"Keyboard1KeyRightWindowsKey"));
                range.SetStyle(InputStyle, new Regex(@"Keyboard1KeyApplications"));
                range.SetStyle(InputStyle, new Regex(@"Keyboard1KeyPower"));
                range.SetStyle(InputStyle, new Regex(@"Keyboard1KeySleep"));
                range.SetStyle(InputStyle, new Regex(@"Keyboard1KeyWake"));
                range.SetStyle(InputStyle, new Regex(@"Keyboard1KeyWebSearch"));
                range.SetStyle(InputStyle, new Regex(@"Keyboard1KeyWebFavorites"));
                range.SetStyle(InputStyle, new Regex(@"Keyboard1KeyWebRefresh"));
                range.SetStyle(InputStyle, new Regex(@"Keyboard1KeyWebStop"));
                range.SetStyle(InputStyle, new Regex(@"Keyboard1KeyWebForward"));
                range.SetStyle(InputStyle, new Regex(@"Keyboard1KeyWebBack"));
                range.SetStyle(InputStyle, new Regex(@"Keyboard1KeyMyComputer"));
                range.SetStyle(InputStyle, new Regex(@"Keyboard1KeyMail"));
                range.SetStyle(InputStyle, new Regex(@"Keyboard1KeyMediaSelect"));
                range.SetStyle(InputStyle, new Regex(@"Keyboard1KeyUnknown"));
                range.SetStyle(InputStyle, new Regex(@"Keyboard2KeyEscape"));
                range.SetStyle(InputStyle, new Regex(@"Keyboard2KeyD1"));
                range.SetStyle(InputStyle, new Regex(@"Keyboard2KeyD2"));
                range.SetStyle(InputStyle, new Regex(@"Keyboard2KeyD3"));
                range.SetStyle(InputStyle, new Regex(@"Keyboard2KeyD4"));
                range.SetStyle(InputStyle, new Regex(@"Keyboard2KeyD5"));
                range.SetStyle(InputStyle, new Regex(@"Keyboard2KeyD6"));
                range.SetStyle(InputStyle, new Regex(@"Keyboard2KeyD7"));
                range.SetStyle(InputStyle, new Regex(@"Keyboard2KeyD8"));
                range.SetStyle(InputStyle, new Regex(@"Keyboard2KeyD9"));
                range.SetStyle(InputStyle, new Regex(@"Keyboard2KeyD0"));
                range.SetStyle(InputStyle, new Regex(@"Keyboard2KeyMinus"));
                range.SetStyle(InputStyle, new Regex(@"Keyboard2KeyEquals"));
                range.SetStyle(InputStyle, new Regex(@"Keyboard2KeyBack"));
                range.SetStyle(InputStyle, new Regex(@"Keyboard2KeyTab"));
                range.SetStyle(InputStyle, new Regex(@"Keyboard2KeyQ"));
                range.SetStyle(InputStyle, new Regex(@"Keyboard2KeyW"));
                range.SetStyle(InputStyle, new Regex(@"Keyboard2KeyE"));
                range.SetStyle(InputStyle, new Regex(@"Keyboard2KeyR"));
                range.SetStyle(InputStyle, new Regex(@"Keyboard2KeyT"));
                range.SetStyle(InputStyle, new Regex(@"Keyboard2KeyY"));
                range.SetStyle(InputStyle, new Regex(@"Keyboard2KeyU"));
                range.SetStyle(InputStyle, new Regex(@"Keyboard2KeyI"));
                range.SetStyle(InputStyle, new Regex(@"Keyboard2KeyO"));
                range.SetStyle(InputStyle, new Regex(@"Keyboard2KeyP"));
                range.SetStyle(InputStyle, new Regex(@"Keyboard2KeyLeftBracket"));
                range.SetStyle(InputStyle, new Regex(@"Keyboard2KeyRightBracket"));
                range.SetStyle(InputStyle, new Regex(@"Keyboard2KeyReturn"));
                range.SetStyle(InputStyle, new Regex(@"Keyboard2KeyLeftControl"));
                range.SetStyle(InputStyle, new Regex(@"Keyboard2KeyA"));
                range.SetStyle(InputStyle, new Regex(@"Keyboard2KeyS"));
                range.SetStyle(InputStyle, new Regex(@"Keyboard2KeyD"));
                range.SetStyle(InputStyle, new Regex(@"Keyboard2KeyF"));
                range.SetStyle(InputStyle, new Regex(@"Keyboard2KeyG"));
                range.SetStyle(InputStyle, new Regex(@"Keyboard2KeyH"));
                range.SetStyle(InputStyle, new Regex(@"Keyboard2KeyJ"));
                range.SetStyle(InputStyle, new Regex(@"Keyboard2KeyK"));
                range.SetStyle(InputStyle, new Regex(@"Keyboard2KeyL"));
                range.SetStyle(InputStyle, new Regex(@"Keyboard2KeySemicolon"));
                range.SetStyle(InputStyle, new Regex(@"Keyboard2KeyApostrophe"));
                range.SetStyle(InputStyle, new Regex(@"Keyboard2KeyGrave"));
                range.SetStyle(InputStyle, new Regex(@"Keyboard2KeyLeftShift"));
                range.SetStyle(InputStyle, new Regex(@"Keyboard2KeyBackslash"));
                range.SetStyle(InputStyle, new Regex(@"Keyboard2KeyZ"));
                range.SetStyle(InputStyle, new Regex(@"Keyboard2KeyX"));
                range.SetStyle(InputStyle, new Regex(@"Keyboard2KeyC"));
                range.SetStyle(InputStyle, new Regex(@"Keyboard2KeyV"));
                range.SetStyle(InputStyle, new Regex(@"Keyboard2KeyB"));
                range.SetStyle(InputStyle, new Regex(@"Keyboard2KeyN"));
                range.SetStyle(InputStyle, new Regex(@"Keyboard2KeyM"));
                range.SetStyle(InputStyle, new Regex(@"Keyboard2KeyComma"));
                range.SetStyle(InputStyle, new Regex(@"Keyboard2KeyPeriod"));
                range.SetStyle(InputStyle, new Regex(@"Keyboard2KeySlash"));
                range.SetStyle(InputStyle, new Regex(@"Keyboard2KeyRightShift"));
                range.SetStyle(InputStyle, new Regex(@"Keyboard2KeyMultiply"));
                range.SetStyle(InputStyle, new Regex(@"Keyboard2KeyLeftAlt"));
                range.SetStyle(InputStyle, new Regex(@"Keyboard2KeySpace"));
                range.SetStyle(InputStyle, new Regex(@"Keyboard2KeyCapital"));
                range.SetStyle(InputStyle, new Regex(@"Keyboard2KeyF1"));
                range.SetStyle(InputStyle, new Regex(@"Keyboard2KeyF2"));
                range.SetStyle(InputStyle, new Regex(@"Keyboard2KeyF3"));
                range.SetStyle(InputStyle, new Regex(@"Keyboard2KeyF4"));
                range.SetStyle(InputStyle, new Regex(@"Keyboard2KeyF5"));
                range.SetStyle(InputStyle, new Regex(@"Keyboard2KeyF6"));
                range.SetStyle(InputStyle, new Regex(@"Keyboard2KeyF7"));
                range.SetStyle(InputStyle, new Regex(@"Keyboard2KeyF8"));
                range.SetStyle(InputStyle, new Regex(@"Keyboard2KeyF9"));
                range.SetStyle(InputStyle, new Regex(@"Keyboard2KeyF10"));
                range.SetStyle(InputStyle, new Regex(@"Keyboard2KeyNumberLock"));
                range.SetStyle(InputStyle, new Regex(@"Keyboard2KeyScrollLock"));
                range.SetStyle(InputStyle, new Regex(@"Keyboard2KeyNumberPad7"));
                range.SetStyle(InputStyle, new Regex(@"Keyboard2KeyNumberPad8"));
                range.SetStyle(InputStyle, new Regex(@"Keyboard2KeyNumberPad9"));
                range.SetStyle(InputStyle, new Regex(@"Keyboard2KeySubtract"));
                range.SetStyle(InputStyle, new Regex(@"Keyboard2KeyNumberPad4"));
                range.SetStyle(InputStyle, new Regex(@"Keyboard2KeyNumberPad5"));
                range.SetStyle(InputStyle, new Regex(@"Keyboard2KeyNumberPad6"));
                range.SetStyle(InputStyle, new Regex(@"Keyboard2KeyAdd"));
                range.SetStyle(InputStyle, new Regex(@"Keyboard2KeyNumberPad1"));
                range.SetStyle(InputStyle, new Regex(@"Keyboard2KeyNumberPad2"));
                range.SetStyle(InputStyle, new Regex(@"Keyboard2KeyNumberPad3"));
                range.SetStyle(InputStyle, new Regex(@"Keyboard2KeyNumberPad0"));
                range.SetStyle(InputStyle, new Regex(@"Keyboard2KeyDecimal"));
                range.SetStyle(InputStyle, new Regex(@"Keyboard2KeyOem102"));
                range.SetStyle(InputStyle, new Regex(@"Keyboard2KeyF11"));
                range.SetStyle(InputStyle, new Regex(@"Keyboard2KeyF12"));
                range.SetStyle(InputStyle, new Regex(@"Keyboard2KeyF13"));
                range.SetStyle(InputStyle, new Regex(@"Keyboard2KeyF14"));
                range.SetStyle(InputStyle, new Regex(@"Keyboard2KeyF15"));
                range.SetStyle(InputStyle, new Regex(@"Keyboard2KeyKana"));
                range.SetStyle(InputStyle, new Regex(@"Keyboard2KeyAbntC1"));
                range.SetStyle(InputStyle, new Regex(@"Keyboard2KeyConvert"));
                range.SetStyle(InputStyle, new Regex(@"Keyboard2KeyNoConvert"));
                range.SetStyle(InputStyle, new Regex(@"Keyboard2KeyYen"));
                range.SetStyle(InputStyle, new Regex(@"Keyboard2KeyAbntC2"));
                range.SetStyle(InputStyle, new Regex(@"Keyboard2KeyNumberPadEquals"));
                range.SetStyle(InputStyle, new Regex(@"Keyboard2KeyPreviousTrack"));
                range.SetStyle(InputStyle, new Regex(@"Keyboard2KeyAT"));
                range.SetStyle(InputStyle, new Regex(@"Keyboard2KeyColon"));
                range.SetStyle(InputStyle, new Regex(@"Keyboard2KeyUnderline"));
                range.SetStyle(InputStyle, new Regex(@"Keyboard2KeyKanji"));
                range.SetStyle(InputStyle, new Regex(@"Keyboard2KeyStop"));
                range.SetStyle(InputStyle, new Regex(@"Keyboard2KeyAX"));
                range.SetStyle(InputStyle, new Regex(@"Keyboard2KeyUnlabeled"));
                range.SetStyle(InputStyle, new Regex(@"Keyboard2KeyNextTrack"));
                range.SetStyle(InputStyle, new Regex(@"Keyboard2KeyNumberPadEnter"));
                range.SetStyle(InputStyle, new Regex(@"Keyboard2KeyRightControl"));
                range.SetStyle(InputStyle, new Regex(@"Keyboard2KeyMute"));
                range.SetStyle(InputStyle, new Regex(@"Keyboard2KeyCalculator"));
                range.SetStyle(InputStyle, new Regex(@"Keyboard2KeyPlayPause"));
                range.SetStyle(InputStyle, new Regex(@"Keyboard2KeyMediaStop"));
                range.SetStyle(InputStyle, new Regex(@"Keyboard2KeyVolumeDown"));
                range.SetStyle(InputStyle, new Regex(@"Keyboard2KeyVolumeUp"));
                range.SetStyle(InputStyle, new Regex(@"Keyboard2KeyWebHome"));
                range.SetStyle(InputStyle, new Regex(@"Keyboard2KeyNumberPadComma"));
                range.SetStyle(InputStyle, new Regex(@"Keyboard2KeyDivide"));
                range.SetStyle(InputStyle, new Regex(@"Keyboard2KeyPrintScreen"));
                range.SetStyle(InputStyle, new Regex(@"Keyboard2KeyRightAlt"));
                range.SetStyle(InputStyle, new Regex(@"Keyboard2KeyPause"));
                range.SetStyle(InputStyle, new Regex(@"Keyboard2KeyHome"));
                range.SetStyle(InputStyle, new Regex(@"Keyboard2KeyUp"));
                range.SetStyle(InputStyle, new Regex(@"Keyboard2KeyPageUp"));
                range.SetStyle(InputStyle, new Regex(@"Keyboard2KeyLeft"));
                range.SetStyle(InputStyle, new Regex(@"Keyboard2KeyRight"));
                range.SetStyle(InputStyle, new Regex(@"Keyboard2KeyEnd"));
                range.SetStyle(InputStyle, new Regex(@"Keyboard2KeyDown"));
                range.SetStyle(InputStyle, new Regex(@"Keyboard2KeyPageDown"));
                range.SetStyle(InputStyle, new Regex(@"Keyboard2KeyInsert"));
                range.SetStyle(InputStyle, new Regex(@"Keyboard2KeyDelete"));
                range.SetStyle(InputStyle, new Regex(@"Keyboard2KeyLeftWindowsKey"));
                range.SetStyle(InputStyle, new Regex(@"Keyboard2KeyRightWindowsKey"));
                range.SetStyle(InputStyle, new Regex(@"Keyboard2KeyApplications"));
                range.SetStyle(InputStyle, new Regex(@"Keyboard2KeyPower"));
                range.SetStyle(InputStyle, new Regex(@"Keyboard2KeySleep"));
                range.SetStyle(InputStyle, new Regex(@"Keyboard2KeyWake"));
                range.SetStyle(InputStyle, new Regex(@"Keyboard2KeyWebSearch"));
                range.SetStyle(InputStyle, new Regex(@"Keyboard2KeyWebFavorites"));
                range.SetStyle(InputStyle, new Regex(@"Keyboard2KeyWebRefresh"));
                range.SetStyle(InputStyle, new Regex(@"Keyboard2KeyWebStop"));
                range.SetStyle(InputStyle, new Regex(@"Keyboard2KeyWebForward"));
                range.SetStyle(InputStyle, new Regex(@"Keyboard2KeyWebBack"));
                range.SetStyle(InputStyle, new Regex(@"Keyboard2KeyMyComputer"));
                range.SetStyle(InputStyle, new Regex(@"Keyboard2KeyMail"));
                range.SetStyle(InputStyle, new Regex(@"Keyboard2KeyMediaSelect"));
                range.SetStyle(InputStyle, new Regex(@"Keyboard2KeyUnknown"));
                range.SetStyle(InputStyle, new Regex(@"TextFromSpeech"));
                range.SetStyle(InputStyle, new Regex(@"PS5ControllerLeftStickX"));
                range.SetStyle(InputStyle, new Regex(@"PS5ControllerLeftStickY"));
                range.SetStyle(InputStyle, new Regex(@"PS5ControllerRightStickX"));
                range.SetStyle(InputStyle, new Regex(@"PS5ControllerRightStickY"));
                range.SetStyle(InputStyle, new Regex(@"PS5ControllerLeftTriggerPosition"));
                range.SetStyle(InputStyle, new Regex(@"PS5ControllerRightTriggerPosition"));
                range.SetStyle(InputStyle, new Regex(@"PS5ControllerTouchX"));
                range.SetStyle(InputStyle, new Regex(@"PS5ControllerTouchY"));
                range.SetStyle(InputStyle, new Regex(@"PS5ControllerTouchOn"));
                range.SetStyle(InputStyle, new Regex(@"PS5ControllerGyroX"));
                range.SetStyle(InputStyle, new Regex(@"PS5ControllerGyroY"));
                range.SetStyle(InputStyle, new Regex(@"PS5ControllerAccelX"));
                range.SetStyle(InputStyle, new Regex(@"PS5ControllerAccelY"));
                range.SetStyle(InputStyle, new Regex(@"PS5ControllerButtonCrossPressed"));
                range.SetStyle(InputStyle, new Regex(@"PS5ControllerButtonCirclePressed"));
                range.SetStyle(InputStyle, new Regex(@"PS5ControllerButtonSquarePressed"));
                range.SetStyle(InputStyle, new Regex(@"PS5ControllerButtonTrianglePressed"));
                range.SetStyle(InputStyle, new Regex(@"PS5ControllerButtonDPadUpPressed"));
                range.SetStyle(InputStyle, new Regex(@"PS5ControllerButtonDPadRightPressed"));
                range.SetStyle(InputStyle, new Regex(@"PS5ControllerButtonDPadDownPressed"));
                range.SetStyle(InputStyle, new Regex(@"PS5ControllerButtonDPadLeftPressed"));
                range.SetStyle(InputStyle, new Regex(@"PS5ControllerButtonL1Pressed"));
                range.SetStyle(InputStyle, new Regex(@"PS5ControllerButtonR1Pressed"));
                range.SetStyle(InputStyle, new Regex(@"PS5ControllerButtonL2Pressed"));
                range.SetStyle(InputStyle, new Regex(@"PS5ControllerButtonR2Pressed"));
                range.SetStyle(InputStyle, new Regex(@"PS5ControllerButtonL3Pressed"));
                range.SetStyle(InputStyle, new Regex(@"PS5ControllerButtonR3Pressed"));
                range.SetStyle(InputStyle, new Regex(@"PS5ControllerButtonCreatePressed"));
                range.SetStyle(InputStyle, new Regex(@"PS5ControllerButtonMenuPressed"));
                range.SetStyle(InputStyle, new Regex(@"PS5ControllerButtonLogoPressed"));
                range.SetStyle(InputStyle, new Regex(@"PS5ControllerButtonTouchpadPressed"));
                range.SetStyle(InputStyle, new Regex(@"PS5ControllerButtonFnLPressed"));
                range.SetStyle(InputStyle, new Regex(@"PS5ControllerButtonFnRPressed"));
                range.SetStyle(InputStyle, new Regex(@"PS5ControllerButtonBLPPressed"));
                range.SetStyle(InputStyle, new Regex(@"PS5ControllerButtonBRPPressed"));
                range.SetStyle(InputStyle, new Regex(@"PS5ControllerButtonMicPressed"));
                range.SetStyle(InputStyle, new Regex(@"PS4ControllerLeftStickX"));
                range.SetStyle(InputStyle, new Regex(@"PS4ControllerLeftStickY"));
                range.SetStyle(InputStyle, new Regex(@"PS4ControllerRightStickX"));
                range.SetStyle(InputStyle, new Regex(@"PS4ControllerRightStickY"));
                range.SetStyle(InputStyle, new Regex(@"PS4ControllerLeftTriggerPosition"));
                range.SetStyle(InputStyle, new Regex(@"PS4ControllerRightTriggerPosition"));
                range.SetStyle(InputStyle, new Regex(@"PS4ControllerTouchX"));
                range.SetStyle(InputStyle, new Regex(@"PS4ControllerTouchY"));
                range.SetStyle(InputStyle, new Regex(@"PS4ControllerTouchOn"));
                range.SetStyle(InputStyle, new Regex(@"PS4ControllerGyroX"));
                range.SetStyle(InputStyle, new Regex(@"PS4ControllerGyroY"));
                range.SetStyle(InputStyle, new Regex(@"PS4ControllerAccelX"));
                range.SetStyle(InputStyle, new Regex(@"PS4ControllerAccelY"));
                range.SetStyle(InputStyle, new Regex(@"PS4ControllerButtonCrossPressed"));
                range.SetStyle(InputStyle, new Regex(@"PS4ControllerButtonCirclePressed"));
                range.SetStyle(InputStyle, new Regex(@"PS4ControllerButtonSquarePressed"));
                range.SetStyle(InputStyle, new Regex(@"PS4ControllerButtonTrianglePressed"));
                range.SetStyle(InputStyle, new Regex(@"PS4ControllerButtonDPadUpPressed"));
                range.SetStyle(InputStyle, new Regex(@"PS4ControllerButtonDPadRightPressed"));
                range.SetStyle(InputStyle, new Regex(@"PS4ControllerButtonDPadDownPressed"));
                range.SetStyle(InputStyle, new Regex(@"PS4ControllerButtonDPadLeftPressed"));
                range.SetStyle(InputStyle, new Regex(@"PS4ControllerButtonL1Pressed"));
                range.SetStyle(InputStyle, new Regex(@"PS4ControllerButtonR1Pressed"));
                range.SetStyle(InputStyle, new Regex(@"PS4ControllerButtonL2Pressed"));
                range.SetStyle(InputStyle, new Regex(@"PS4ControllerButtonR2Pressed"));
                range.SetStyle(InputStyle, new Regex(@"PS4ControllerButtonL3Pressed"));
                range.SetStyle(InputStyle, new Regex(@"PS4ControllerButtonR3Pressed"));
                range.SetStyle(InputStyle, new Regex(@"PS4ControllerButtonCreatePressed"));
                range.SetStyle(InputStyle, new Regex(@"PS4ControllerButtonMenuPressed"));
                range.SetStyle(InputStyle, new Regex(@"PS4ControllerButtonLogoPressed"));
                range.SetStyle(InputStyle, new Regex(@"PS4ControllerButtonTouchpadPressed"));
                range.SetStyle(InputStyle, new Regex(@"PS4ControllerButtonMicPressed"));
                range.SetStyle(InputStyle, new Regex(@"PS5Controller1LeftStickX"));
                range.SetStyle(InputStyle, new Regex(@"PS5Controller1LeftStickY"));
                range.SetStyle(InputStyle, new Regex(@"PS5Controller1RightStickX"));
                range.SetStyle(InputStyle, new Regex(@"PS5Controller1RightStickY"));
                range.SetStyle(InputStyle, new Regex(@"PS5Controller1LeftTriggerPosition"));
                range.SetStyle(InputStyle, new Regex(@"PS5Controller1RightTriggerPosition"));
                range.SetStyle(InputStyle, new Regex(@"PS5Controller1TouchX"));
                range.SetStyle(InputStyle, new Regex(@"PS5Controller1TouchY"));
                range.SetStyle(InputStyle, new Regex(@"PS5Controller1TouchOn"));
                range.SetStyle(InputStyle, new Regex(@"PS5Controller1GyroX"));
                range.SetStyle(InputStyle, new Regex(@"PS5Controller1GyroY"));
                range.SetStyle(InputStyle, new Regex(@"PS5Controller1AccelX"));
                range.SetStyle(InputStyle, new Regex(@"PS5Controller1AccelY"));
                range.SetStyle(InputStyle, new Regex(@"PS5Controller1ButtonCrossPressed"));
                range.SetStyle(InputStyle, new Regex(@"PS5Controller1ButtonCirclePressed"));
                range.SetStyle(InputStyle, new Regex(@"PS5Controller1ButtonSquarePressed"));
                range.SetStyle(InputStyle, new Regex(@"PS5Controller1ButtonTrianglePressed"));
                range.SetStyle(InputStyle, new Regex(@"PS5Controller1ButtonDPadUpPressed"));
                range.SetStyle(InputStyle, new Regex(@"PS5Controller1ButtonDPadRightPressed"));
                range.SetStyle(InputStyle, new Regex(@"PS5Controller1ButtonDPadDownPressed"));
                range.SetStyle(InputStyle, new Regex(@"PS5Controller1ButtonDPadLeftPressed"));
                range.SetStyle(InputStyle, new Regex(@"PS5Controller1ButtonL1Pressed"));
                range.SetStyle(InputStyle, new Regex(@"PS5Controller1ButtonR1Pressed"));
                range.SetStyle(InputStyle, new Regex(@"PS5Controller1ButtonL2Pressed"));
                range.SetStyle(InputStyle, new Regex(@"PS5Controller1ButtonR2Pressed"));
                range.SetStyle(InputStyle, new Regex(@"PS5Controller1ButtonL3Pressed"));
                range.SetStyle(InputStyle, new Regex(@"PS5Controller1ButtonR3Pressed"));
                range.SetStyle(InputStyle, new Regex(@"PS5Controller1ButtonCreatePressed"));
                range.SetStyle(InputStyle, new Regex(@"PS5Controller1ButtonMenuPressed"));
                range.SetStyle(InputStyle, new Regex(@"PS5Controller1ButtonLogoPressed"));
                range.SetStyle(InputStyle, new Regex(@"PS5Controller1ButtonTouchpadPressed"));
                range.SetStyle(InputStyle, new Regex(@"PS5Controller1ButtonFnLPressed"));
                range.SetStyle(InputStyle, new Regex(@"PS5Controller1ButtonFnRPressed"));
                range.SetStyle(InputStyle, new Regex(@"PS5Controller1ButtonBLPPressed"));
                range.SetStyle(InputStyle, new Regex(@"PS5Controller1ButtonBRPPressed"));
                range.SetStyle(InputStyle, new Regex(@"PS5Controller1ButtonMicPressed"));
                range.SetStyle(InputStyle, new Regex(@"PS4Controller1LeftStickX"));
                range.SetStyle(InputStyle, new Regex(@"PS4Controller1LeftStickY"));
                range.SetStyle(InputStyle, new Regex(@"PS4Controller1RightStickX"));
                range.SetStyle(InputStyle, new Regex(@"PS4Controller1RightStickY"));
                range.SetStyle(InputStyle, new Regex(@"PS4Controller1LeftTriggerPosition"));
                range.SetStyle(InputStyle, new Regex(@"PS4Controller1RightTriggerPosition"));
                range.SetStyle(InputStyle, new Regex(@"PS4Controller1TouchX"));
                range.SetStyle(InputStyle, new Regex(@"PS4Controller1TouchY"));
                range.SetStyle(InputStyle, new Regex(@"PS4Controller1TouchOn"));
                range.SetStyle(InputStyle, new Regex(@"PS4Controller1GyroX"));
                range.SetStyle(InputStyle, new Regex(@"PS4Controller1GyroY"));
                range.SetStyle(InputStyle, new Regex(@"PS4Controller1AccelX"));
                range.SetStyle(InputStyle, new Regex(@"PS4Controller1AccelY"));
                range.SetStyle(InputStyle, new Regex(@"PS4Controller1ButtonCrossPressed"));
                range.SetStyle(InputStyle, new Regex(@"PS4Controller1ButtonCirclePressed"));
                range.SetStyle(InputStyle, new Regex(@"PS4Controller1ButtonSquarePressed"));
                range.SetStyle(InputStyle, new Regex(@"PS4Controller1ButtonTrianglePressed"));
                range.SetStyle(InputStyle, new Regex(@"PS4Controller1ButtonDPadUpPressed"));
                range.SetStyle(InputStyle, new Regex(@"PS4Controller1ButtonDPadRightPressed"));
                range.SetStyle(InputStyle, new Regex(@"PS4Controller1ButtonDPadDownPressed"));
                range.SetStyle(InputStyle, new Regex(@"PS4Controller1ButtonDPadLeftPressed"));
                range.SetStyle(InputStyle, new Regex(@"PS4Controller1ButtonL1Pressed"));
                range.SetStyle(InputStyle, new Regex(@"PS4Controller1ButtonR1Pressed"));
                range.SetStyle(InputStyle, new Regex(@"PS4Controller1ButtonL2Pressed"));
                range.SetStyle(InputStyle, new Regex(@"PS4Controller1ButtonR2Pressed"));
                range.SetStyle(InputStyle, new Regex(@"PS4Controller1ButtonL3Pressed"));
                range.SetStyle(InputStyle, new Regex(@"PS4Controller1ButtonR3Pressed"));
                range.SetStyle(InputStyle, new Regex(@"PS4Controller1ButtonCreatePressed"));
                range.SetStyle(InputStyle, new Regex(@"PS4Controller1ButtonMenuPressed"));
                range.SetStyle(InputStyle, new Regex(@"PS4Controller1ButtonLogoPressed"));
                range.SetStyle(InputStyle, new Regex(@"PS4Controller1ButtonTouchpadPressed"));
                range.SetStyle(InputStyle, new Regex(@"PS4Controller1ButtonMicPressed"));
                range.SetStyle(InputStyle, new Regex(@"PS5Controller2LeftStickX"));
                range.SetStyle(InputStyle, new Regex(@"PS5Controller2LeftStickY"));
                range.SetStyle(InputStyle, new Regex(@"PS5Controller2RightStickX"));
                range.SetStyle(InputStyle, new Regex(@"PS5Controller2RightStickY"));
                range.SetStyle(InputStyle, new Regex(@"PS5Controller2LeftTriggerPosition"));
                range.SetStyle(InputStyle, new Regex(@"PS5Controller2RightTriggerPosition"));
                range.SetStyle(InputStyle, new Regex(@"PS5Controller2TouchX"));
                range.SetStyle(InputStyle, new Regex(@"PS5Controller2TouchY"));
                range.SetStyle(InputStyle, new Regex(@"PS5Controller2TouchOn"));
                range.SetStyle(InputStyle, new Regex(@"PS5Controller2GyroX"));
                range.SetStyle(InputStyle, new Regex(@"PS5Controller2GyroY"));
                range.SetStyle(InputStyle, new Regex(@"PS5Controller2AccelX"));
                range.SetStyle(InputStyle, new Regex(@"PS5Controller2AccelY"));
                range.SetStyle(InputStyle, new Regex(@"PS5Controller2ButtonCrossPressed"));
                range.SetStyle(InputStyle, new Regex(@"PS5Controller2ButtonCirclePressed"));
                range.SetStyle(InputStyle, new Regex(@"PS5Controller2ButtonSquarePressed"));
                range.SetStyle(InputStyle, new Regex(@"PS5Controller2ButtonTrianglePressed"));
                range.SetStyle(InputStyle, new Regex(@"PS5Controller2ButtonDPadUpPressed"));
                range.SetStyle(InputStyle, new Regex(@"PS5Controller2ButtonDPadRightPressed"));
                range.SetStyle(InputStyle, new Regex(@"PS5Controller2ButtonDPadDownPressed"));
                range.SetStyle(InputStyle, new Regex(@"PS5Controller2ButtonDPadLeftPressed"));
                range.SetStyle(InputStyle, new Regex(@"PS5Controller2ButtonL1Pressed"));
                range.SetStyle(InputStyle, new Regex(@"PS5Controller2ButtonR1Pressed"));
                range.SetStyle(InputStyle, new Regex(@"PS5Controller2ButtonL2Pressed"));
                range.SetStyle(InputStyle, new Regex(@"PS5Controller2ButtonR2Pressed"));
                range.SetStyle(InputStyle, new Regex(@"PS5Controller2ButtonL3Pressed"));
                range.SetStyle(InputStyle, new Regex(@"PS5Controller2ButtonR3Pressed"));
                range.SetStyle(InputStyle, new Regex(@"PS5Controller2ButtonCreatePressed"));
                range.SetStyle(InputStyle, new Regex(@"PS5Controller2ButtonMenuPressed"));
                range.SetStyle(InputStyle, new Regex(@"PS5Controller2ButtonLogoPressed"));
                range.SetStyle(InputStyle, new Regex(@"PS5Controller2ButtonTouchpadPressed"));
                range.SetStyle(InputStyle, new Regex(@"PS5Controller2ButtonFnLPressed"));
                range.SetStyle(InputStyle, new Regex(@"PS5Controller2ButtonFnRPressed"));
                range.SetStyle(InputStyle, new Regex(@"PS5Controller2ButtonBLPPressed"));
                range.SetStyle(InputStyle, new Regex(@"PS5Controller2ButtonBRPPressed"));
                range.SetStyle(InputStyle, new Regex(@"PS5Controller2ButtonMicPressed"));
                range.SetStyle(InputStyle, new Regex(@"PS4Controller2LeftStickX"));
                range.SetStyle(InputStyle, new Regex(@"PS4Controller2LeftStickY"));
                range.SetStyle(InputStyle, new Regex(@"PS4Controller2RightStickX"));
                range.SetStyle(InputStyle, new Regex(@"PS4Controller2RightStickY"));
                range.SetStyle(InputStyle, new Regex(@"PS4Controller2LeftTriggerPosition"));
                range.SetStyle(InputStyle, new Regex(@"PS4Controller2RightTriggerPosition"));
                range.SetStyle(InputStyle, new Regex(@"PS4Controller2TouchX"));
                range.SetStyle(InputStyle, new Regex(@"PS4Controller2TouchY"));
                range.SetStyle(InputStyle, new Regex(@"PS4Controller2TouchOn"));
                range.SetStyle(InputStyle, new Regex(@"PS4Controller2GyroX"));
                range.SetStyle(InputStyle, new Regex(@"PS4Controller2GyroY"));
                range.SetStyle(InputStyle, new Regex(@"PS4Controller2AccelX"));
                range.SetStyle(InputStyle, new Regex(@"PS4Controller2AccelY"));
                range.SetStyle(InputStyle, new Regex(@"PS4Controller2ButtonCrossPressed"));
                range.SetStyle(InputStyle, new Regex(@"PS4Controller2ButtonCirclePressed"));
                range.SetStyle(InputStyle, new Regex(@"PS4Controller2ButtonSquarePressed"));
                range.SetStyle(InputStyle, new Regex(@"PS4Controller2ButtonTrianglePressed"));
                range.SetStyle(InputStyle, new Regex(@"PS4Controller2ButtonDPadUpPressed"));
                range.SetStyle(InputStyle, new Regex(@"PS4Controller2ButtonDPadRightPressed"));
                range.SetStyle(InputStyle, new Regex(@"PS4Controller2ButtonDPadDownPressed"));
                range.SetStyle(InputStyle, new Regex(@"PS4Controller2ButtonDPadLeftPressed"));
                range.SetStyle(InputStyle, new Regex(@"PS4Controller2ButtonL1Pressed"));
                range.SetStyle(InputStyle, new Regex(@"PS4Controller2ButtonR1Pressed"));
                range.SetStyle(InputStyle, new Regex(@"PS4Controller2ButtonL2Pressed"));
                range.SetStyle(InputStyle, new Regex(@"PS4Controller2ButtonR2Pressed"));
                range.SetStyle(InputStyle, new Regex(@"PS4Controller2ButtonL3Pressed"));
                range.SetStyle(InputStyle, new Regex(@"PS4Controller2ButtonR3Pressed"));
                range.SetStyle(InputStyle, new Regex(@"PS4Controller2ButtonCreatePressed"));
                range.SetStyle(InputStyle, new Regex(@"PS4Controller2ButtonMenuPressed"));
                range.SetStyle(InputStyle, new Regex(@"PS4Controller2ButtonLogoPressed"));
                range.SetStyle(InputStyle, new Regex(@"PS4Controller2ButtonTouchpadPressed"));
                range.SetStyle(InputStyle, new Regex(@"PS4Controller2ButtonMicPressed"));
                range.SetStyle(InputStyle, new Regex(@"_ValueChange"));
                range.SetStyle(OutputStyle, new Regex(@"UsersAllowedList"));
                range.SetStyle(OutputStyle, new Regex(@"PS5ControllerDescriptor"));
                range.SetStyle(OutputStyle, new Regex(@"PS4ControllerDescriptor"));
                range.SetStyle(OutputStyle, new Regex(@"NetworkDescriptor"));
                range.SetStyle(OutputStyle, new Regex(@"sleeptime"));
                range.SetStyle(OutputStyle, new Regex(@"KeyboardMouseDriverType"));
                range.SetStyle(OutputStyle, new Regex(@"MouseMoveX"));
                range.SetStyle(OutputStyle, new Regex(@"MouseMoveY"));
                range.SetStyle(OutputStyle, new Regex(@"MouseAbsX"));
                range.SetStyle(OutputStyle, new Regex(@"MouseAbsY"));
                range.SetStyle(OutputStyle, new Regex(@"MouseDesktopX"));
                range.SetStyle(OutputStyle, new Regex(@"MouseDesktopY"));
                range.SetStyle(OutputStyle, new Regex(@"SendLeftClick"));
                range.SetStyle(OutputStyle, new Regex(@"SendRightClick"));
                range.SetStyle(OutputStyle, new Regex(@"SendMiddleClick"));
                range.SetStyle(OutputStyle, new Regex(@"SendWheelUp"));
                range.SetStyle(OutputStyle, new Regex(@"SendWheelDown"));
                range.SetStyle(OutputStyle, new Regex(@"SendLeft"));
                range.SetStyle(OutputStyle, new Regex(@"SendRight"));
                range.SetStyle(OutputStyle, new Regex(@"SendUp"));
                range.SetStyle(OutputStyle, new Regex(@"SendDown"));
                range.SetStyle(OutputStyle, new Regex(@"SendLButton"));
                range.SetStyle(OutputStyle, new Regex(@"SendRButton"));
                range.SetStyle(OutputStyle, new Regex(@"SendCancel"));
                range.SetStyle(OutputStyle, new Regex(@"SendMBUTTON"));
                range.SetStyle(OutputStyle, new Regex(@"SendXBUTTON1"));
                range.SetStyle(OutputStyle, new Regex(@"SendXBUTTON2"));
                range.SetStyle(OutputStyle, new Regex(@"SendBack"));
                range.SetStyle(OutputStyle, new Regex(@"SendTab"));
                range.SetStyle(OutputStyle, new Regex(@"SendClear"));
                range.SetStyle(OutputStyle, new Regex(@"SendReturn"));
                range.SetStyle(OutputStyle, new Regex(@"SendSHIFT"));
                range.SetStyle(OutputStyle, new Regex(@"SendCONTROL"));
                range.SetStyle(OutputStyle, new Regex(@"SendMENU"));
                range.SetStyle(OutputStyle, new Regex(@"SendPAUSE"));
                range.SetStyle(OutputStyle, new Regex(@"SendCAPITAL"));
                range.SetStyle(OutputStyle, new Regex(@"SendKANA"));
                range.SetStyle(OutputStyle, new Regex(@"SendHANGEUL"));
                range.SetStyle(OutputStyle, new Regex(@"SendHANGUL"));
                range.SetStyle(OutputStyle, new Regex(@"SendJUNJA"));
                range.SetStyle(OutputStyle, new Regex(@"SendFINAL"));
                range.SetStyle(OutputStyle, new Regex(@"SendHANJA"));
                range.SetStyle(OutputStyle, new Regex(@"SendKANJI"));
                range.SetStyle(OutputStyle, new Regex(@"SendEscape"));
                range.SetStyle(OutputStyle, new Regex(@"SendCONVERT"));
                range.SetStyle(OutputStyle, new Regex(@"SendNONCONVERT"));
                range.SetStyle(OutputStyle, new Regex(@"SendACCEPT"));
                range.SetStyle(OutputStyle, new Regex(@"SendMODECHANGE"));
                range.SetStyle(OutputStyle, new Regex(@"SendSpace"));
                range.SetStyle(OutputStyle, new Regex(@"SendPRIOR"));
                range.SetStyle(OutputStyle, new Regex(@"SendNEXT"));
                range.SetStyle(OutputStyle, new Regex(@"SendEND"));
                range.SetStyle(OutputStyle, new Regex(@"SendHOME"));
                range.SetStyle(OutputStyle, new Regex(@"SendLEFT"));
                range.SetStyle(OutputStyle, new Regex(@"SendUP"));
                range.SetStyle(OutputStyle, new Regex(@"SendRIGHT"));
                range.SetStyle(OutputStyle, new Regex(@"SendDOWN"));
                range.SetStyle(OutputStyle, new Regex(@"SendSELECT"));
                range.SetStyle(OutputStyle, new Regex(@"SendPRINT"));
                range.SetStyle(OutputStyle, new Regex(@"SendEXECUTE"));
                range.SetStyle(OutputStyle, new Regex(@"SendSNAPSHOT"));
                range.SetStyle(OutputStyle, new Regex(@"SendINSERT"));
                range.SetStyle(OutputStyle, new Regex(@"SendDELETE"));
                range.SetStyle(OutputStyle, new Regex(@"SendHELP"));
                range.SetStyle(OutputStyle, new Regex(@"SendAPOSTROPHE"));
                range.SetStyle(OutputStyle, new Regex(@"Send0"));
                range.SetStyle(OutputStyle, new Regex(@"Send1"));
                range.SetStyle(OutputStyle, new Regex(@"Send2"));
                range.SetStyle(OutputStyle, new Regex(@"Send3"));
                range.SetStyle(OutputStyle, new Regex(@"Send4"));
                range.SetStyle(OutputStyle, new Regex(@"Send5"));
                range.SetStyle(OutputStyle, new Regex(@"Send6"));
                range.SetStyle(OutputStyle, new Regex(@"Send7"));
                range.SetStyle(OutputStyle, new Regex(@"Send8"));
                range.SetStyle(OutputStyle, new Regex(@"Send9"));
                range.SetStyle(OutputStyle, new Regex(@"SendA"));
                range.SetStyle(OutputStyle, new Regex(@"SendB"));
                range.SetStyle(OutputStyle, new Regex(@"SendC"));
                range.SetStyle(OutputStyle, new Regex(@"SendD"));
                range.SetStyle(OutputStyle, new Regex(@"SendE"));
                range.SetStyle(OutputStyle, new Regex(@"SendF"));
                range.SetStyle(OutputStyle, new Regex(@"SendG"));
                range.SetStyle(OutputStyle, new Regex(@"SendH"));
                range.SetStyle(OutputStyle, new Regex(@"SendI"));
                range.SetStyle(OutputStyle, new Regex(@"SendJ"));
                range.SetStyle(OutputStyle, new Regex(@"SendK"));
                range.SetStyle(OutputStyle, new Regex(@"SendL"));
                range.SetStyle(OutputStyle, new Regex(@"SendM"));
                range.SetStyle(OutputStyle, new Regex(@"SendN"));
                range.SetStyle(OutputStyle, new Regex(@"SendO"));
                range.SetStyle(OutputStyle, new Regex(@"SendP"));
                range.SetStyle(OutputStyle, new Regex(@"SendQ"));
                range.SetStyle(OutputStyle, new Regex(@"SendR"));
                range.SetStyle(OutputStyle, new Regex(@"SendS"));
                range.SetStyle(OutputStyle, new Regex(@"SendT"));
                range.SetStyle(OutputStyle, new Regex(@"SendU"));
                range.SetStyle(OutputStyle, new Regex(@"SendV"));
                range.SetStyle(OutputStyle, new Regex(@"SendW"));
                range.SetStyle(OutputStyle, new Regex(@"SendX"));
                range.SetStyle(OutputStyle, new Regex(@"SendY"));
                range.SetStyle(OutputStyle, new Regex(@"SendZ"));
                range.SetStyle(OutputStyle, new Regex(@"SendLWIN"));
                range.SetStyle(OutputStyle, new Regex(@"SendRWIN"));
                range.SetStyle(OutputStyle, new Regex(@"SendAPPS"));
                range.SetStyle(OutputStyle, new Regex(@"SendSLEEP"));
                range.SetStyle(OutputStyle, new Regex(@"SendNUMPAD0"));
                range.SetStyle(OutputStyle, new Regex(@"SendNUMPAD1"));
                range.SetStyle(OutputStyle, new Regex(@"SendNUMPAD2"));
                range.SetStyle(OutputStyle, new Regex(@"SendNUMPAD3"));
                range.SetStyle(OutputStyle, new Regex(@"SendNUMPAD4"));
                range.SetStyle(OutputStyle, new Regex(@"SendNUMPAD5"));
                range.SetStyle(OutputStyle, new Regex(@"SendNUMPAD6"));
                range.SetStyle(OutputStyle, new Regex(@"SendNUMPAD7"));
                range.SetStyle(OutputStyle, new Regex(@"SendNUMPAD8"));
                range.SetStyle(OutputStyle, new Regex(@"SendNUMPAD9"));
                range.SetStyle(OutputStyle, new Regex(@"SendMULTIPLY"));
                range.SetStyle(OutputStyle, new Regex(@"SendADD"));
                range.SetStyle(OutputStyle, new Regex(@"SendSEPARATOR"));
                range.SetStyle(OutputStyle, new Regex(@"SendSUBTRACT"));
                range.SetStyle(OutputStyle, new Regex(@"SendDECIMAL"));
                range.SetStyle(OutputStyle, new Regex(@"SendDIVIDE"));
                range.SetStyle(OutputStyle, new Regex(@"SendF1"));
                range.SetStyle(OutputStyle, new Regex(@"SendF2"));
                range.SetStyle(OutputStyle, new Regex(@"SendF3"));
                range.SetStyle(OutputStyle, new Regex(@"SendF4"));
                range.SetStyle(OutputStyle, new Regex(@"SendF5"));
                range.SetStyle(OutputStyle, new Regex(@"SendF6"));
                range.SetStyle(OutputStyle, new Regex(@"SendF7"));
                range.SetStyle(OutputStyle, new Regex(@"SendF8"));
                range.SetStyle(OutputStyle, new Regex(@"SendF9"));
                range.SetStyle(OutputStyle, new Regex(@"SendF10"));
                range.SetStyle(OutputStyle, new Regex(@"SendF11"));
                range.SetStyle(OutputStyle, new Regex(@"SendF12"));
                range.SetStyle(OutputStyle, new Regex(@"SendF13"));
                range.SetStyle(OutputStyle, new Regex(@"SendF14"));
                range.SetStyle(OutputStyle, new Regex(@"SendF15"));
                range.SetStyle(OutputStyle, new Regex(@"SendF16"));
                range.SetStyle(OutputStyle, new Regex(@"SendF17"));
                range.SetStyle(OutputStyle, new Regex(@"SendF18"));
                range.SetStyle(OutputStyle, new Regex(@"SendF19"));
                range.SetStyle(OutputStyle, new Regex(@"SendF20"));
                range.SetStyle(OutputStyle, new Regex(@"SendF21"));
                range.SetStyle(OutputStyle, new Regex(@"SendF22"));
                range.SetStyle(OutputStyle, new Regex(@"SendF23"));
                range.SetStyle(OutputStyle, new Regex(@"SendF24"));
                range.SetStyle(OutputStyle, new Regex(@"SendNUMLOCK"));
                range.SetStyle(OutputStyle, new Regex(@"SendSCROLL"));
                range.SetStyle(OutputStyle, new Regex(@"SendLeftShift"));
                range.SetStyle(OutputStyle, new Regex(@"SendRightShift"));
                range.SetStyle(OutputStyle, new Regex(@"SendLeftControl"));
                range.SetStyle(OutputStyle, new Regex(@"SendRightControl"));
                range.SetStyle(OutputStyle, new Regex(@"SendLMENU"));
                range.SetStyle(OutputStyle, new Regex(@"SendRMENU"));
                range.SetStyle(OutputStyle, new Regex(@"centery"));
                range.SetStyle(OutputStyle, new Regex(@"irmode"));
                range.SetStyle(OutputStyle, new Regex(@"SpeechToText"));
                range.SetStyle(OutputStyle, new Regex(@"controller1_send_xbox"));
                range.SetStyle(OutputStyle, new Regex(@"controller1_send_back"));
                range.SetStyle(OutputStyle, new Regex(@"controller1_send_start"));
                range.SetStyle(OutputStyle, new Regex(@"controller1_send_A"));
                range.SetStyle(OutputStyle, new Regex(@"controller1_send_B"));
                range.SetStyle(OutputStyle, new Regex(@"controller1_send_X"));
                range.SetStyle(OutputStyle, new Regex(@"controller1_send_Y"));
                range.SetStyle(OutputStyle, new Regex(@"controller1_send_up"));
                range.SetStyle(OutputStyle, new Regex(@"controller1_send_left"));
                range.SetStyle(OutputStyle, new Regex(@"controller1_send_down"));
                range.SetStyle(OutputStyle, new Regex(@"controller1_send_right"));
                range.SetStyle(OutputStyle, new Regex(@"controller1_send_leftstick"));
                range.SetStyle(OutputStyle, new Regex(@"controller1_send_rightstick"));
                range.SetStyle(OutputStyle, new Regex(@"controller1_send_leftbumper"));
                range.SetStyle(OutputStyle, new Regex(@"controller1_send_rightbumper"));
                range.SetStyle(OutputStyle, new Regex(@"controller1_send_lefttrigger"));
                range.SetStyle(OutputStyle, new Regex(@"controller1_send_righttrigger"));
                range.SetStyle(OutputStyle, new Regex(@"controller1_send_leftstickx"));
                range.SetStyle(OutputStyle, new Regex(@"controller1_send_leftsticky"));
                range.SetStyle(OutputStyle, new Regex(@"controller1_send_rightstickx"));
                range.SetStyle(OutputStyle, new Regex(@"controller1_send_rightsticky"));
                range.SetStyle(OutputStyle, new Regex(@"controller2_send_xbox"));
                range.SetStyle(OutputStyle, new Regex(@"controller2_send_back"));
                range.SetStyle(OutputStyle, new Regex(@"controller2_send_start"));
                range.SetStyle(OutputStyle, new Regex(@"controller2_send_A"));
                range.SetStyle(OutputStyle, new Regex(@"controller2_send_B"));
                range.SetStyle(OutputStyle, new Regex(@"controller2_send_X"));
                range.SetStyle(OutputStyle, new Regex(@"controller2_send_Y"));
                range.SetStyle(OutputStyle, new Regex(@"controller2_send_up"));
                range.SetStyle(OutputStyle, new Regex(@"controller2_send_left"));
                range.SetStyle(OutputStyle, new Regex(@"controller2_send_down"));
                range.SetStyle(OutputStyle, new Regex(@"controller2_send_right"));
                range.SetStyle(OutputStyle, new Regex(@"controller2_send_leftstick"));
                range.SetStyle(OutputStyle, new Regex(@"controller2_send_rightstick"));
                range.SetStyle(OutputStyle, new Regex(@"controller2_send_leftbumper"));
                range.SetStyle(OutputStyle, new Regex(@"controller2_send_rightbumper"));
                range.SetStyle(OutputStyle, new Regex(@"controller2_send_lefttrigger"));
                range.SetStyle(OutputStyle, new Regex(@"controller2_send_righttrigger"));
                range.SetStyle(OutputStyle, new Regex(@"controller2_send_leftstickx"));
                range.SetStyle(OutputStyle, new Regex(@"controller2_send_leftsticky"));
                range.SetStyle(OutputStyle, new Regex(@"controller2_send_rightstickx"));
                range.SetStyle(OutputStyle, new Regex(@"controller2_send_rightsticky"));
                range.SetStyle(OutputStyle, new Regex(@"controller1_send_lefttriggerposition"));
                range.SetStyle(OutputStyle, new Regex(@"controller1_send_righttriggerposition"));
                range.SetStyle(OutputStyle, new Regex(@"controller2_send_lefttriggerposition"));
                range.SetStyle(OutputStyle, new Regex(@"controller2_send_righttriggerposition"));
                range.SetStyle(OutputStyle, new Regex(@"Controller1DS4_Send_Options"));
                range.SetStyle(OutputStyle, new Regex(@"Controller1DS4_Send_Option"));
                range.SetStyle(OutputStyle, new Regex(@"Controller1DS4_Send_ThumbLeft"));
                range.SetStyle(OutputStyle, new Regex(@"Controller1DS4_Send_ThumbRight"));
                range.SetStyle(OutputStyle, new Regex(@"Controller1DS4_Send_ShoulderLeft"));
                range.SetStyle(OutputStyle, new Regex(@"Controller1DS4_Send_ShoulderRight"));
                range.SetStyle(OutputStyle, new Regex(@"Controller1DS4_Send_Cross"));
                range.SetStyle(OutputStyle, new Regex(@"Controller1DS4_Send_Circle"));
                range.SetStyle(OutputStyle, new Regex(@"Controller1DS4_Send_Square"));
                range.SetStyle(OutputStyle, new Regex(@"Controller1DS4_Send_Triangle"));
                range.SetStyle(OutputStyle, new Regex(@"Controller1DS4_Send_Ps"));
                range.SetStyle(OutputStyle, new Regex(@"Controller1DS4_Send_Touchpad"));
                range.SetStyle(OutputStyle, new Regex(@"Controller1DS4_Send_Share"));
                range.SetStyle(OutputStyle, new Regex(@"Controller1DS4_Send_DPadUp"));
                range.SetStyle(OutputStyle, new Regex(@"Controller1DS4_Send_DPadDown"));
                range.SetStyle(OutputStyle, new Regex(@"Controller1DS4_Send_DPadLeft"));
                range.SetStyle(OutputStyle, new Regex(@"Controller1DS4_Send_DPadRight"));
                range.SetStyle(OutputStyle, new Regex(@"Controller1DS4_Send_LeftTrigger"));
                range.SetStyle(OutputStyle, new Regex(@"Controller1DS4_Send_RightTrigger"));
                range.SetStyle(OutputStyle, new Regex(@"Controller1DS4_Send_LeftTriggerPosition"));
                range.SetStyle(OutputStyle, new Regex(@"Controller1DS4_Send_RightTriggerPosition"));
                range.SetStyle(OutputStyle, new Regex(@"Controller1DS4_Send_LeftThumbX"));
                range.SetStyle(OutputStyle, new Regex(@"Controller1DS4_Send_RightThumbX"));
                range.SetStyle(OutputStyle, new Regex(@"Controller1DS4_Send_LeftThumbY"));
                range.SetStyle(OutputStyle, new Regex(@"Controller1DS4_Send_RightThumbY"));
                range.SetStyle(OutputStyle, new Regex(@"Controller2DS4_Send_Options"));
                range.SetStyle(OutputStyle, new Regex(@"Controller2DS4_Send_Option"));
                range.SetStyle(OutputStyle, new Regex(@"Controller2DS4_Send_ThumbLeft"));
                range.SetStyle(OutputStyle, new Regex(@"Controller2DS4_Send_ThumbRight"));
                range.SetStyle(OutputStyle, new Regex(@"Controller2DS4_Send_ShoulderLeft"));
                range.SetStyle(OutputStyle, new Regex(@"Controller2DS4_Send_ShoulderRight"));
                range.SetStyle(OutputStyle, new Regex(@"Controller2DS4_Send_Cross"));
                range.SetStyle(OutputStyle, new Regex(@"Controller2DS4_Send_Circle"));
                range.SetStyle(OutputStyle, new Regex(@"Controller2DS4_Send_Square"));
                range.SetStyle(OutputStyle, new Regex(@"Controller2DS4_Send_Triangle"));
                range.SetStyle(OutputStyle, new Regex(@"Controller2DS4_Send_Ps"));
                range.SetStyle(OutputStyle, new Regex(@"Controller2DS4_Send_Touchpad"));
                range.SetStyle(OutputStyle, new Regex(@"Controller2DS4_Send_Share"));
                range.SetStyle(OutputStyle, new Regex(@"Controller2DS4_Send_DPadUp"));
                range.SetStyle(OutputStyle, new Regex(@"Controller2DS4_Send_DPadDown"));
                range.SetStyle(OutputStyle, new Regex(@"Controller2DS4_Send_DPadLeft"));
                range.SetStyle(OutputStyle, new Regex(@"Controller2DS4_Send_DPadRight"));
                range.SetStyle(OutputStyle, new Regex(@"Controller2DS4_Send_LeftTrigger"));
                range.SetStyle(OutputStyle, new Regex(@"Controller2DS4_Send_RightTrigger"));
                range.SetStyle(OutputStyle, new Regex(@"Controller2DS4_Send_LeftTriggerPosition"));
                range.SetStyle(OutputStyle, new Regex(@"Controller2DS4_Send_RightTriggerPosition"));
                range.SetStyle(OutputStyle, new Regex(@"Controller2DS4_Send_LeftThumbX"));
                range.SetStyle(OutputStyle, new Regex(@"Controller2DS4_Send_RightThumbX"));
                range.SetStyle(OutputStyle, new Regex(@"Controller2DS4_Send_LeftThumbY"));
                range.SetStyle(OutputStyle, new Regex(@"Controller2DS4_Send_RightThumbY"));
                range.SetStyle(OutputStyle, new Regex(@"Controller1VJoy_Send_1"));
                range.SetStyle(OutputStyle, new Regex(@"Controller1VJoy_Send_2"));
                range.SetStyle(OutputStyle, new Regex(@"Controller1VJoy_Send_3"));
                range.SetStyle(OutputStyle, new Regex(@"Controller1VJoy_Send_4"));
                range.SetStyle(OutputStyle, new Regex(@"Controller1VJoy_Send_5"));
                range.SetStyle(OutputStyle, new Regex(@"Controller1VJoy_Send_6"));
                range.SetStyle(OutputStyle, new Regex(@"Controller1VJoy_Send_7"));
                range.SetStyle(OutputStyle, new Regex(@"Controller1VJoy_Send_8"));
                range.SetStyle(OutputStyle, new Regex(@"Controller1VJoy_Send_X"));
                range.SetStyle(OutputStyle, new Regex(@"Controller1VJoy_Send_Y"));
                range.SetStyle(OutputStyle, new Regex(@"Controller1VJoy_Send_Z"));
                range.SetStyle(OutputStyle, new Regex(@"Controller1VJoy_Send_WHL"));
                range.SetStyle(OutputStyle, new Regex(@"Controller1VJoy_Send_SL0"));
                range.SetStyle(OutputStyle, new Regex(@"Controller1VJoy_Send_SL1"));
                range.SetStyle(OutputStyle, new Regex(@"Controller1VJoy_Send_RX"));
                range.SetStyle(OutputStyle, new Regex(@"Controller1VJoy_Send_RY"));
                range.SetStyle(OutputStyle, new Regex(@"Controller1VJoy_Send_RZ"));
                range.SetStyle(OutputStyle, new Regex(@"Controller1VJoy_Send_POV"));
                range.SetStyle(OutputStyle, new Regex(@"Controller1VJoy_Send_Hat"));
                range.SetStyle(OutputStyle, new Regex(@"Controller1VJoy_Send_HatExt1"));
                range.SetStyle(OutputStyle, new Regex(@"Controller1VJoy_Send_HatExt2"));
                range.SetStyle(OutputStyle, new Regex(@"Controller1VJoy_Send_HatExt3"));
                range.SetStyle(OutputStyle, new Regex(@"Controller2VJoy_Send_1"));
                range.SetStyle(OutputStyle, new Regex(@"Controller2VJoy_Send_2"));
                range.SetStyle(OutputStyle, new Regex(@"Controller2VJoy_Send_3"));
                range.SetStyle(OutputStyle, new Regex(@"Controller2VJoy_Send_4"));
                range.SetStyle(OutputStyle, new Regex(@"Controller2VJoy_Send_5"));
                range.SetStyle(OutputStyle, new Regex(@"Controller2VJoy_Send_6"));
                range.SetStyle(OutputStyle, new Regex(@"Controller2VJoy_Send_7"));
                range.SetStyle(OutputStyle, new Regex(@"Controller2VJoy_Send_8"));
                range.SetStyle(OutputStyle, new Regex(@"Controller2VJoy_Send_X"));
                range.SetStyle(OutputStyle, new Regex(@"Controller2VJoy_Send_Y"));
                range.SetStyle(OutputStyle, new Regex(@"Controller2VJoy_Send_Z"));
                range.SetStyle(OutputStyle, new Regex(@"Controller2VJoy_Send_WHL"));
                range.SetStyle(OutputStyle, new Regex(@"Controller2VJoy_Send_SL0"));
                range.SetStyle(OutputStyle, new Regex(@"Controller2VJoy_Send_SL1"));
                range.SetStyle(OutputStyle, new Regex(@"Controller2VJoy_Send_RX"));
                range.SetStyle(OutputStyle, new Regex(@"Controller2VJoy_Send_RY"));
                range.SetStyle(OutputStyle, new Regex(@"Controller2VJoy_Send_RZ"));
                range.SetStyle(OutputStyle, new Regex(@"Controller2VJoy_Send_POV"));
                range.SetStyle(OutputStyle, new Regex(@"Controller2VJoy_Send_Hat"));
                range.SetStyle(OutputStyle, new Regex(@"Controller2VJoy_Send_HatExt1"));
                range.SetStyle(OutputStyle, new Regex(@"Controller2VJoy_Send_HatExt2"));
                range.SetStyle(OutputStyle, new Regex(@"Controller2VJoy_Send_HatExt3"));
                range.SetStyle(OutputStyle, new Regex(@"EnableKM"));
                range.SetStyle(OutputStyle, new Regex(@"EnableXC"));
                range.SetStyle(OutputStyle, new Regex(@"EnableDS4"));
                range.SetStyle(OutputStyle, new Regex(@"EnableVJoy"));
                range.SetStyle(OutputStyle, new Regex(@"EnableRI"));
                range.SetStyle(OutputStyle, new Regex(@"EnableCI"));
                range.SetStyle(OutputStyle, new Regex(@"EnableXI"));
                range.SetStyle(OutputStyle, new Regex(@"EnableDI"));
                range.SetStyle(OutputStyle, new Regex(@"EnableJI"));
                range.SetStyle(OutputStyle, new Regex(@"EnablePI"));
                range.SetStyle(OutputStyle, new Regex(@"EnableWI"));
                range.SetStyle(OutputStyle, new Regex(@"EnableGI"));
                range.SetStyle(OutputStyle, new Regex(@"pollcount"));
                range.SetStyle(OutputStyle, new Regex(@"keys12345"));
                range.SetStyle(OutputStyle, new Regex(@"keys54321"));
                range.SetStyle(OutputStyle, new Regex(@"mousexp"));
                range.SetStyle(OutputStyle, new Regex(@"mouseyp"));
                range.SetStyle(OutputStyle, new Regex(@"statex"));
                range.SetStyle(OutputStyle, new Regex(@"statey"));
                range.SetStyle(OutputStyle, new Regex(@"mousex"));
                range.SetStyle(OutputStyle, new Regex(@"mousey"));
                range.SetStyle(OutputStyle, new Regex(@"mousestatex"));
                range.SetStyle(OutputStyle, new Regex(@"mousestatey"));
                range.SetStyle(OutputStyle, new Regex(@"viewpower1x"));
                range.SetStyle(OutputStyle, new Regex(@"viewpower2x"));
                range.SetStyle(OutputStyle, new Regex(@"viewpower3x"));
                range.SetStyle(OutputStyle, new Regex(@"viewpower1y"));
                range.SetStyle(OutputStyle, new Regex(@"viewpower2y"));
                range.SetStyle(OutputStyle, new Regex(@"viewpower3y"));
                range.SetStyle(OutputStyle, new Regex(@"dzx"));
                range.SetStyle(OutputStyle, new Regex(@"dzy"));
                range.SetStyle(OutputStyle, new Regex(@"viewpower05x"));
                range.SetStyle(OutputStyle, new Regex(@"viewpower05y"));
                range.SetStyle(OutputStyle, new Regex(@"state1x"));
                range.SetStyle(OutputStyle, new Regex(@"state1y"));
                range.SetStyle(OutputStyle, new Regex(@"mouse1x"));
                range.SetStyle(OutputStyle, new Regex(@"mouse1y"));
                range.SetStyle(OutputStyle, new Regex(@"mousestate1x"));
                range.SetStyle(OutputStyle, new Regex(@"mousestate1y"));
                range.SetStyle(OutputStyle, new Regex(@"state2x"));
                range.SetStyle(OutputStyle, new Regex(@"state2y"));
                range.SetStyle(OutputStyle, new Regex(@"mouse2x"));
                range.SetStyle(OutputStyle, new Regex(@"mouse2y"));
                range.SetStyle(OutputStyle, new Regex(@"mousestate2x"));
                range.SetStyle(OutputStyle, new Regex(@"mousestate2y"));
                range.SetStyle(OutputStyle, new Regex(@"statestickx"));
                range.SetStyle(OutputStyle, new Regex(@"statesticky"));
                range.SetStyle(OutputStyle, new Regex(@"mousestickx"));
                range.SetStyle(OutputStyle, new Regex(@"mousesticky"));
                range.SetStyle(OutputStyle, new Regex(@"mousestatestickx"));
                range.SetStyle(OutputStyle, new Regex(@"mousestatesticky"));
                range.SetStyle(OutputStyle, new Regex(@"testdouble"));
                range.SetStyle(OutputStyle, new Regex(@"testbool"));
                range.SetStyle(OutputStyle, new Regex(@"JoyconLeftAccelCenter"));
                range.SetStyle(OutputStyle, new Regex(@"JoyconRightAccelCenter"));
                range.SetStyle(OutputStyle, new Regex(@"ProControllerAccelCenter"));
                range.SetStyle(OutputStyle, new Regex(@"PS5ControllerAccelCenter"));
                range.SetStyle(OutputStyle, new Regex(@"PS4ControllerAccelCenter"));
                range.SetStyle(OutputStyle, new Regex(@"PS5Controller1AccelCenter"));
                range.SetStyle(OutputStyle, new Regex(@"PS4Controller1AccelCenter"));
                range.SetStyle(OutputStyle, new Regex(@"PS5Controller2AccelCenter"));
                range.SetStyle(OutputStyle, new Regex(@"PS4Controller2AccelCenter"));
                range.SetStyle(OutputStyle, new Regex(@"JoyconLeftStickCenter"));
                range.SetStyle(OutputStyle, new Regex(@"JoyconRightStickCenter"));
                range.SetStyle(OutputStyle, new Regex(@"ProControllerStickCenter"));
                range.SetStyle(OutputStyle, new Regex(@"xcnumber"));
                range.SetStyle(OutputStyle, new Regex(@"ds4number"));
                range.SetStyle(OutputStyle, new Regex(@"vjoynumber"));
                range.SetStyle(OutputStyle, new Regex(@"ValueChange"));
                range.SetStyle(InputStyle, new Regex(@"ir1x"));
                range.SetStyle(InputStyle, new Regex(@"ir1y"));
                range.SetStyle(InputStyle, new Regex(@"Wiimote1ButtonStateA"));
                range.SetStyle(InputStyle, new Regex(@"Wiimote1ButtonStateB"));
                range.SetStyle(InputStyle, new Regex(@"Wiimote1ButtonStateMinus"));
                range.SetStyle(InputStyle, new Regex(@"Wiimote1ButtonStateHome"));
                range.SetStyle(InputStyle, new Regex(@"Wiimote1ButtonStatePlus"));
                range.SetStyle(InputStyle, new Regex(@"Wiimote1ButtonStateOne"));
                range.SetStyle(InputStyle, new Regex(@"Wiimote1ButtonStateTwo"));
                range.SetStyle(InputStyle, new Regex(@"Wiimote1ButtonStateUp"));
                range.SetStyle(InputStyle, new Regex(@"Wiimote1ButtonStateDown"));
                range.SetStyle(InputStyle, new Regex(@"Wiimote1ButtonStateLeft"));
                range.SetStyle(InputStyle, new Regex(@"Wiimote1ButtonStateRight"));
                range.SetStyle(InputStyle, new Regex(@"Wiimote1RawValuesX"));
                range.SetStyle(InputStyle, new Regex(@"Wiimote1RawValuesY"));
                range.SetStyle(InputStyle, new Regex(@"Wiimote1RawValuesZ"));
                range.SetStyle(InputStyle, new Regex(@"Wiimote1NunchuckStateRawJoystickX"));
                range.SetStyle(InputStyle, new Regex(@"Wiimote1NunchuckStateRawJoystickY"));
                range.SetStyle(InputStyle, new Regex(@"Wiimote1NunchuckStateRawValuesX"));
                range.SetStyle(InputStyle, new Regex(@"Wiimote1NunchuckStateRawValuesY"));
                range.SetStyle(InputStyle, new Regex(@"Wiimote1NunchuckStateRawValuesZ"));
                range.SetStyle(InputStyle, new Regex(@"Wiimote1NunchuckStateC"));
                range.SetStyle(InputStyle, new Regex(@"Wiimote1NunchuckStateZ"));
                range.SetStyle(InputStyle, new Regex(@"ir2x"));
                range.SetStyle(InputStyle, new Regex(@"ir2y"));
                range.SetStyle(InputStyle, new Regex(@"Wiimote2ButtonStateA"));
                range.SetStyle(InputStyle, new Regex(@"Wiimote2ButtonStateB"));
                range.SetStyle(InputStyle, new Regex(@"Wiimote2ButtonStateMinus"));
                range.SetStyle(InputStyle, new Regex(@"Wiimote2ButtonStateHome"));
                range.SetStyle(InputStyle, new Regex(@"Wiimote2ButtonStatePlus"));
                range.SetStyle(InputStyle, new Regex(@"Wiimote2ButtonStateOne"));
                range.SetStyle(InputStyle, new Regex(@"Wiimote2ButtonStateTwo"));
                range.SetStyle(InputStyle, new Regex(@"Wiimote2ButtonStateUp"));
                range.SetStyle(InputStyle, new Regex(@"Wiimote2ButtonStateDown"));
                range.SetStyle(InputStyle, new Regex(@"Wiimote2ButtonStateLeft"));
                range.SetStyle(InputStyle, new Regex(@"Wiimote2ButtonStateRight"));
                range.SetStyle(InputStyle, new Regex(@"Wiimote2RawValuesX"));
                range.SetStyle(InputStyle, new Regex(@"Wiimote2RawValuesY"));
                range.SetStyle(InputStyle, new Regex(@"Wiimote2RawValuesZ"));
                range.SetStyle(InputStyle, new Regex(@"Wiimote2NunchuckStateRawJoystickX"));
                range.SetStyle(InputStyle, new Regex(@"Wiimote2NunchuckStateRawJoystickY"));
                range.SetStyle(InputStyle, new Regex(@"Wiimote2NunchuckStateRawValuesX"));
                range.SetStyle(InputStyle, new Regex(@"Wiimote2NunchuckStateRawValuesY"));
                range.SetStyle(InputStyle, new Regex(@"Wiimote2NunchuckStateRawValuesZ"));
                range.SetStyle(InputStyle, new Regex(@"Wiimote2NunchuckStateC"));
                range.SetStyle(InputStyle, new Regex(@"Wiimote2NunchuckStateZ"));
                range.SetStyle(OutputStyle, new Regex(@"keyboard_1_id"));
                range.SetStyle(OutputStyle, new Regex(@"mouse_1_id"));
                range.SetStyle(OutputStyle, new Regex(@"int_1_deltaX"));
                range.SetStyle(OutputStyle, new Regex(@"int_1_deltaY"));
                range.SetStyle(OutputStyle, new Regex(@"int_1_x"));
                range.SetStyle(OutputStyle, new Regex(@"int_1_y"));
                range.SetStyle(OutputStyle, new Regex(@"int_1_SendLeftClick"));
                range.SetStyle(OutputStyle, new Regex(@"int_1_SendRightClick"));
                range.SetStyle(OutputStyle, new Regex(@"int_1_SendMiddleClick"));
                range.SetStyle(OutputStyle, new Regex(@"int_1_SendWheelUp"));
                range.SetStyle(OutputStyle, new Regex(@"int_1_SendWheelDown"));
                range.SetStyle(OutputStyle, new Regex(@"int_1_SendCANCEL"));
                range.SetStyle(OutputStyle, new Regex(@"int_1_SendBACK"));
                range.SetStyle(OutputStyle, new Regex(@"int_1_SendTAB"));
                range.SetStyle(OutputStyle, new Regex(@"int_1_SendCLEAR"));
                range.SetStyle(OutputStyle, new Regex(@"int_1_SendRETURN"));
                range.SetStyle(OutputStyle, new Regex(@"int_1_SendSHIFT"));
                range.SetStyle(OutputStyle, new Regex(@"int_1_SendCONTROL"));
                range.SetStyle(OutputStyle, new Regex(@"int_1_SendMENU"));
                range.SetStyle(OutputStyle, new Regex(@"int_1_SendCAPITAL"));
                range.SetStyle(OutputStyle, new Regex(@"int_1_SendESCAPE"));
                range.SetStyle(OutputStyle, new Regex(@"int_1_SendSPACE"));
                range.SetStyle(OutputStyle, new Regex(@"int_1_SendPRIOR"));
                range.SetStyle(OutputStyle, new Regex(@"int_1_SendNEXT"));
                range.SetStyle(OutputStyle, new Regex(@"int_1_SendEND"));
                range.SetStyle(OutputStyle, new Regex(@"int_1_SendHOME"));
                range.SetStyle(OutputStyle, new Regex(@"int_1_SendLEFT"));
                range.SetStyle(OutputStyle, new Regex(@"int_1_SendUP"));
                range.SetStyle(OutputStyle, new Regex(@"int_1_SendRIGHT"));
                range.SetStyle(OutputStyle, new Regex(@"int_1_SendDOWN"));
                range.SetStyle(OutputStyle, new Regex(@"int_1_SendSNAPSHOT"));
                range.SetStyle(OutputStyle, new Regex(@"int_1_SendINSERT"));
                range.SetStyle(OutputStyle, new Regex(@"int_1_SendNUMPADDEL"));
                range.SetStyle(OutputStyle, new Regex(@"int_1_SendNUMPADINSERT"));
                range.SetStyle(OutputStyle, new Regex(@"int_1_SendHELP"));
                range.SetStyle(OutputStyle, new Regex(@"int_1_SendAPOSTROPHE"));
                range.SetStyle(OutputStyle, new Regex(@"int_1_SendBACKSPACE"));
                range.SetStyle(OutputStyle, new Regex(@"int_1_SendPAGEDOWN"));
                range.SetStyle(OutputStyle, new Regex(@"int_1_SendPAGEUP"));
                range.SetStyle(OutputStyle, new Regex(@"int_1_SendFIN"));
                range.SetStyle(OutputStyle, new Regex(@"int_1_SendMOUSE"));
                range.SetStyle(OutputStyle, new Regex(@"int_1_SendA"));
                range.SetStyle(OutputStyle, new Regex(@"int_1_SendB"));
                range.SetStyle(OutputStyle, new Regex(@"int_1_SendC"));
                range.SetStyle(OutputStyle, new Regex(@"int_1_SendD"));
                range.SetStyle(OutputStyle, new Regex(@"int_1_SendE"));
                range.SetStyle(OutputStyle, new Regex(@"int_1_SendF"));
                range.SetStyle(OutputStyle, new Regex(@"int_1_SendG"));
                range.SetStyle(OutputStyle, new Regex(@"int_1_SendH"));
                range.SetStyle(OutputStyle, new Regex(@"int_1_SendI"));
                range.SetStyle(OutputStyle, new Regex(@"int_1_SendJ"));
                range.SetStyle(OutputStyle, new Regex(@"int_1_SendK"));
                range.SetStyle(OutputStyle, new Regex(@"int_1_SendL"));
                range.SetStyle(OutputStyle, new Regex(@"int_1_SendM"));
                range.SetStyle(OutputStyle, new Regex(@"int_1_SendN"));
                range.SetStyle(OutputStyle, new Regex(@"int_1_SendO"));
                range.SetStyle(OutputStyle, new Regex(@"int_1_SendP"));
                range.SetStyle(OutputStyle, new Regex(@"int_1_SendQ"));
                range.SetStyle(OutputStyle, new Regex(@"int_1_SendR"));
                range.SetStyle(OutputStyle, new Regex(@"int_1_SendS"));
                range.SetStyle(OutputStyle, new Regex(@"int_1_SendT"));
                range.SetStyle(OutputStyle, new Regex(@"int_1_SendU"));
                range.SetStyle(OutputStyle, new Regex(@"int_1_SendV"));
                range.SetStyle(OutputStyle, new Regex(@"int_1_SendW"));
                range.SetStyle(OutputStyle, new Regex(@"int_1_SendX"));
                range.SetStyle(OutputStyle, new Regex(@"int_1_SendY"));
                range.SetStyle(OutputStyle, new Regex(@"int_1_SendZ"));
                range.SetStyle(OutputStyle, new Regex(@"int_1_SendLWIN"));
                range.SetStyle(OutputStyle, new Regex(@"int_1_SendRWIN"));
                range.SetStyle(OutputStyle, new Regex(@"int_1_SendAPPS"));
                range.SetStyle(OutputStyle, new Regex(@"int_1_SendDELETE"));
                range.SetStyle(OutputStyle, new Regex(@"int_1_SendNUMPAD0"));
                range.SetStyle(OutputStyle, new Regex(@"int_1_SendNUMPAD1"));
                range.SetStyle(OutputStyle, new Regex(@"int_1_SendNUMPAD2"));
                range.SetStyle(OutputStyle, new Regex(@"int_1_SendNUMPAD3"));
                range.SetStyle(OutputStyle, new Regex(@"int_1_SendNUMPAD4"));
                range.SetStyle(OutputStyle, new Regex(@"int_1_SendNUMPAD5"));
                range.SetStyle(OutputStyle, new Regex(@"int_1_SendNUMPAD6"));
                range.SetStyle(OutputStyle, new Regex(@"int_1_SendNUMPAD7"));
                range.SetStyle(OutputStyle, new Regex(@"int_1_SendNUMPAD8"));
                range.SetStyle(OutputStyle, new Regex(@"int_1_SendNUMPAD9"));
                range.SetStyle(OutputStyle, new Regex(@"int_1_SendMULTIPLY"));
                range.SetStyle(OutputStyle, new Regex(@"int_1_SendADD"));
                range.SetStyle(OutputStyle, new Regex(@"int_1_SendSUBTRACT"));
                range.SetStyle(OutputStyle, new Regex(@"int_1_SendDECIMAL"));
                range.SetStyle(OutputStyle, new Regex(@"int_1_SendPRINTSCREEN"));
                range.SetStyle(OutputStyle, new Regex(@"int_1_SendDIVIDE"));
                range.SetStyle(OutputStyle, new Regex(@"int_1_SendF1"));
                range.SetStyle(OutputStyle, new Regex(@"int_1_SendF2"));
                range.SetStyle(OutputStyle, new Regex(@"int_1_SendF3"));
                range.SetStyle(OutputStyle, new Regex(@"int_1_SendF4"));
                range.SetStyle(OutputStyle, new Regex(@"int_1_SendF5"));
                range.SetStyle(OutputStyle, new Regex(@"int_1_SendF6"));
                range.SetStyle(OutputStyle, new Regex(@"int_1_SendF7"));
                range.SetStyle(OutputStyle, new Regex(@"int_1_SendF8"));
                range.SetStyle(OutputStyle, new Regex(@"int_1_SendF9"));
                range.SetStyle(OutputStyle, new Regex(@"int_1_SendF10"));
                range.SetStyle(OutputStyle, new Regex(@"int_1_SendF11"));
                range.SetStyle(OutputStyle, new Regex(@"int_1_SendF12"));
                range.SetStyle(OutputStyle, new Regex(@"int_1_SendNUMLOCK"));
                range.SetStyle(OutputStyle, new Regex(@"int_1_SendSCROLLLOCK"));
                range.SetStyle(OutputStyle, new Regex(@"int_1_SendLEFTSHIFT"));
                range.SetStyle(OutputStyle, new Regex(@"int_1_SendRIGHTSHIFT"));
                range.SetStyle(OutputStyle, new Regex(@"int_1_SendLEFTCONTROL"));
                range.SetStyle(OutputStyle, new Regex(@"int_1_SendRIGHTCONTROL"));
                range.SetStyle(OutputStyle, new Regex(@"int_1_SendLEFTALT"));
                range.SetStyle(OutputStyle, new Regex(@"int_1_SendRIGHTALT"));
                range.SetStyle(OutputStyle, new Regex(@"int_1_SendBROWSER_BACK"));
                range.SetStyle(OutputStyle, new Regex(@"int_1_SendBROWSER_FORWARD"));
                range.SetStyle(OutputStyle, new Regex(@"int_1_SendBROWSER_REFRESH"));
                range.SetStyle(OutputStyle, new Regex(@"int_1_SendBROWSER_STOP"));
                range.SetStyle(OutputStyle, new Regex(@"int_1_SendBROWSER_SEARCH"));
                range.SetStyle(OutputStyle, new Regex(@"int_1_SendBROWSER_FAVORITES"));
                range.SetStyle(OutputStyle, new Regex(@"int_1_SendBROWSER_HOME"));
                range.SetStyle(OutputStyle, new Regex(@"int_1_SendVOLUME_MUTE"));
                range.SetStyle(OutputStyle, new Regex(@"int_1_SendVOLUME_DOWN"));
                range.SetStyle(OutputStyle, new Regex(@"int_1_SendVOLUME_UP"));
                range.SetStyle(OutputStyle, new Regex(@"int_1_SendMEDIA_NEXT_TRACK"));
                range.SetStyle(OutputStyle, new Regex(@"int_1_SendMEDIA_PREV_TRACK"));
                range.SetStyle(OutputStyle, new Regex(@"int_1_SendMEDIA_STOP"));
                range.SetStyle(OutputStyle, new Regex(@"int_1_SendMEDIA_PLAY_PAUSE"));
                range.SetStyle(OutputStyle, new Regex(@"int_1_SendLAUNCH_MAIL"));
                range.SetStyle(OutputStyle, new Regex(@"int_1_SendLAUNCH_MEDIA_SELECT"));
                range.SetStyle(OutputStyle, new Regex(@"int_1_SendLAUNCH_APP1"));
                range.SetStyle(OutputStyle, new Regex(@"int_1_SendLAUNCH_APP2"));
                range.SetStyle(OutputStyle, new Regex(@"int_1_SendOEM_1"));
                range.SetStyle(OutputStyle, new Regex(@"int_1_SendOEM_PLUS"));
                range.SetStyle(OutputStyle, new Regex(@"int_1_SendOEM_COMMA"));
                range.SetStyle(OutputStyle, new Regex(@"int_1_SendOEM_MINUS"));
                range.SetStyle(OutputStyle, new Regex(@"int_1_SendOEM_PERIOD"));
                range.SetStyle(OutputStyle, new Regex(@"int_1_SendOEM_2"));
                range.SetStyle(OutputStyle, new Regex(@"int_1_SendOEM_3"));
                range.SetStyle(OutputStyle, new Regex(@"int_1_SendOEM_4"));
                range.SetStyle(OutputStyle, new Regex(@"int_1_SendOEM_5"));
                range.SetStyle(OutputStyle, new Regex(@"int_1_SendOEM_6"));
                range.SetStyle(OutputStyle, new Regex(@"int_1_SendOEM_7"));
                range.SetStyle(OutputStyle, new Regex(@"int_1_SendOEM_8"));
                range.SetStyle(OutputStyle, new Regex(@"int_1_SendOEM_102"));
                range.SetStyle(OutputStyle, new Regex(@"int_1_SendEREOF"));
                range.SetStyle(OutputStyle, new Regex(@"int_1_SendZOOM"));
                range.SetStyle(OutputStyle, new Regex(@"int_1_SendEscape"));
                range.SetStyle(OutputStyle, new Regex(@"int_1_SendOne"));
                range.SetStyle(OutputStyle, new Regex(@"int_1_SendTwo"));
                range.SetStyle(OutputStyle, new Regex(@"int_1_SendThree"));
                range.SetStyle(OutputStyle, new Regex(@"int_1_SendFour"));
                range.SetStyle(OutputStyle, new Regex(@"int_1_SendFive"));
                range.SetStyle(OutputStyle, new Regex(@"int_1_SendSix"));
                range.SetStyle(OutputStyle, new Regex(@"int_1_SendSeven"));
                range.SetStyle(OutputStyle, new Regex(@"int_1_SendEight"));
                range.SetStyle(OutputStyle, new Regex(@"int_1_SendNine"));
                range.SetStyle(OutputStyle, new Regex(@"int_1_SendZero"));
                range.SetStyle(OutputStyle, new Regex(@"int_1_SendDashUnderscore"));
                range.SetStyle(OutputStyle, new Regex(@"int_1_SendPlusEquals"));
                range.SetStyle(OutputStyle, new Regex(@"int_1_SendBackspace"));
                range.SetStyle(OutputStyle, new Regex(@"int_1_SendTab"));
                range.SetStyle(OutputStyle, new Regex(@"int_1_SendOpenBracketBrace"));
                range.SetStyle(OutputStyle, new Regex(@"int_1_SendCloseBracketBrace"));
                range.SetStyle(OutputStyle, new Regex(@"int_1_SendEnter"));
                range.SetStyle(OutputStyle, new Regex(@"int_1_SendControl"));
                range.SetStyle(OutputStyle, new Regex(@"int_1_SendSemicolonColon"));
                range.SetStyle(OutputStyle, new Regex(@"int_1_SendSingleDoubleQuote"));
                range.SetStyle(OutputStyle, new Regex(@"int_1_SendTilde"));
                range.SetStyle(OutputStyle, new Regex(@"int_1_SendLeftShift"));
                range.SetStyle(OutputStyle, new Regex(@"int_1_SendBackslashPipe"));
                range.SetStyle(OutputStyle, new Regex(@"int_1_SendCommaLeftArrow"));
                range.SetStyle(OutputStyle, new Regex(@"int_1_SendPeriodRightArrow"));
                range.SetStyle(OutputStyle, new Regex(@"int_1_SendForwardSlashQuestionMark"));
                range.SetStyle(OutputStyle, new Regex(@"int_1_SendRightShift"));
                range.SetStyle(OutputStyle, new Regex(@"int_1_SendRightAlt"));
                range.SetStyle(OutputStyle, new Regex(@"int_1_SendSpace"));
                range.SetStyle(OutputStyle, new Regex(@"int_1_SendCapsLock"));
                range.SetStyle(OutputStyle, new Regex(@"int_1_SendUp"));
                range.SetStyle(OutputStyle, new Regex(@"int_1_SendDown"));
                range.SetStyle(OutputStyle, new Regex(@"int_1_SendRight"));
                range.SetStyle(OutputStyle, new Regex(@"int_1_SendLeft"));
                range.SetStyle(OutputStyle, new Regex(@"int_1_SendHome"));
                range.SetStyle(OutputStyle, new Regex(@"int_1_SendEnd"));
                range.SetStyle(OutputStyle, new Regex(@"int_1_SendDelete"));
                range.SetStyle(OutputStyle, new Regex(@"int_1_SendPageUp"));
                range.SetStyle(OutputStyle, new Regex(@"int_1_SendPageDown"));
                range.SetStyle(OutputStyle, new Regex(@"int_1_SendInsert"));
                range.SetStyle(OutputStyle, new Regex(@"int_1_SendPrintScreen"));
                range.SetStyle(OutputStyle, new Regex(@"int_1_SendNumLock"));
                range.SetStyle(OutputStyle, new Regex(@"int_1_SendScrollLock"));
                range.SetStyle(OutputStyle, new Regex(@"int_1_SendMenu"));
                range.SetStyle(OutputStyle, new Regex(@"int_1_SendWindowsKey"));
                range.SetStyle(OutputStyle, new Regex(@"int_1_SendNumpadDivide"));
                range.SetStyle(OutputStyle, new Regex(@"int_1_SendNumpadAsterisk"));
                range.SetStyle(OutputStyle, new Regex(@"int_1_SendNumpad7"));
                range.SetStyle(OutputStyle, new Regex(@"int_1_SendNumpad8"));
                range.SetStyle(OutputStyle, new Regex(@"int_1_SendNumpad9"));
                range.SetStyle(OutputStyle, new Regex(@"int_1_SendNumpad4"));
                range.SetStyle(OutputStyle, new Regex(@"int_1_SendNumpad5"));
                range.SetStyle(OutputStyle, new Regex(@"int_1_SendNumpad6"));
                range.SetStyle(OutputStyle, new Regex(@"int_1_SendNumpad1"));
                range.SetStyle(OutputStyle, new Regex(@"int_1_SendNumpad2"));
                range.SetStyle(OutputStyle, new Regex(@"int_1_SendNumpad3"));
                range.SetStyle(OutputStyle, new Regex(@"int_1_SendNumpad0"));
                range.SetStyle(OutputStyle, new Regex(@"int_1_SendNumpadDelete"));
                range.SetStyle(OutputStyle, new Regex(@"int_1_SendNumpadEnter"));
                range.SetStyle(OutputStyle, new Regex(@"int_1_SendNumpadPlus"));
                range.SetStyle(OutputStyle, new Regex(@"int_1_SendNumpadMinus"));
                range.SetStyle(OutputStyle, new Regex(@"keyboard_2_id"));
                range.SetStyle(OutputStyle, new Regex(@"mouse_2_id"));
                range.SetStyle(OutputStyle, new Regex(@"int_2_deltaX"));
                range.SetStyle(OutputStyle, new Regex(@"int_2_deltaY"));
                range.SetStyle(OutputStyle, new Regex(@"int_2_x"));
                range.SetStyle(OutputStyle, new Regex(@"int_2_y"));
                range.SetStyle(OutputStyle, new Regex(@"int_2_SendLeftClick"));
                range.SetStyle(OutputStyle, new Regex(@"int_2_SendRightClick"));
                range.SetStyle(OutputStyle, new Regex(@"int_2_SendMiddleClick"));
                range.SetStyle(OutputStyle, new Regex(@"int_2_SendWheelUp"));
                range.SetStyle(OutputStyle, new Regex(@"int_2_SendWheelDown"));
                range.SetStyle(OutputStyle, new Regex(@"int_2_SendCANCEL"));
                range.SetStyle(OutputStyle, new Regex(@"int_2_SendBACK"));
                range.SetStyle(OutputStyle, new Regex(@"int_2_SendTAB"));
                range.SetStyle(OutputStyle, new Regex(@"int_2_SendCLEAR"));
                range.SetStyle(OutputStyle, new Regex(@"int_2_SendRETURN"));
                range.SetStyle(OutputStyle, new Regex(@"int_2_SendSHIFT"));
                range.SetStyle(OutputStyle, new Regex(@"int_2_SendCONTROL"));
                range.SetStyle(OutputStyle, new Regex(@"int_2_SendMENU"));
                range.SetStyle(OutputStyle, new Regex(@"int_2_SendCAPITAL"));
                range.SetStyle(OutputStyle, new Regex(@"int_2_SendESCAPE"));
                range.SetStyle(OutputStyle, new Regex(@"int_2_SendSPACE"));
                range.SetStyle(OutputStyle, new Regex(@"int_2_SendPRIOR"));
                range.SetStyle(OutputStyle, new Regex(@"int_2_SendNEXT"));
                range.SetStyle(OutputStyle, new Regex(@"int_2_SendEND"));
                range.SetStyle(OutputStyle, new Regex(@"int_2_SendHOME"));
                range.SetStyle(OutputStyle, new Regex(@"int_2_SendLEFT"));
                range.SetStyle(OutputStyle, new Regex(@"int_2_SendUP"));
                range.SetStyle(OutputStyle, new Regex(@"int_2_SendRIGHT"));
                range.SetStyle(OutputStyle, new Regex(@"int_2_SendDOWN"));
                range.SetStyle(OutputStyle, new Regex(@"int_2_SendSNAPSHOT"));
                range.SetStyle(OutputStyle, new Regex(@"int_2_SendINSERT"));
                range.SetStyle(OutputStyle, new Regex(@"int_2_SendNUMPADDEL"));
                range.SetStyle(OutputStyle, new Regex(@"int_2_SendNUMPADINSERT"));
                range.SetStyle(OutputStyle, new Regex(@"int_2_SendHELP"));
                range.SetStyle(OutputStyle, new Regex(@"int_2_SendAPOSTROPHE"));
                range.SetStyle(OutputStyle, new Regex(@"int_2_SendBACKSPACE"));
                range.SetStyle(OutputStyle, new Regex(@"int_2_SendPAGEDOWN"));
                range.SetStyle(OutputStyle, new Regex(@"int_2_SendPAGEUP"));
                range.SetStyle(OutputStyle, new Regex(@"int_2_SendFIN"));
                range.SetStyle(OutputStyle, new Regex(@"int_2_SendMOUSE"));
                range.SetStyle(OutputStyle, new Regex(@"int_2_SendA"));
                range.SetStyle(OutputStyle, new Regex(@"int_2_SendB"));
                range.SetStyle(OutputStyle, new Regex(@"int_2_SendC"));
                range.SetStyle(OutputStyle, new Regex(@"int_2_SendD"));
                range.SetStyle(OutputStyle, new Regex(@"int_2_SendE"));
                range.SetStyle(OutputStyle, new Regex(@"int_2_SendF"));
                range.SetStyle(OutputStyle, new Regex(@"int_2_SendG"));
                range.SetStyle(OutputStyle, new Regex(@"int_2_SendH"));
                range.SetStyle(OutputStyle, new Regex(@"int_2_SendI"));
                range.SetStyle(OutputStyle, new Regex(@"int_2_SendJ"));
                range.SetStyle(OutputStyle, new Regex(@"int_2_SendK"));
                range.SetStyle(OutputStyle, new Regex(@"int_2_SendL"));
                range.SetStyle(OutputStyle, new Regex(@"int_2_SendM"));
                range.SetStyle(OutputStyle, new Regex(@"int_2_SendN"));
                range.SetStyle(OutputStyle, new Regex(@"int_2_SendO"));
                range.SetStyle(OutputStyle, new Regex(@"int_2_SendP"));
                range.SetStyle(OutputStyle, new Regex(@"int_2_SendQ"));
                range.SetStyle(OutputStyle, new Regex(@"int_2_SendR"));
                range.SetStyle(OutputStyle, new Regex(@"int_2_SendS"));
                range.SetStyle(OutputStyle, new Regex(@"int_2_SendT"));
                range.SetStyle(OutputStyle, new Regex(@"int_2_SendU"));
                range.SetStyle(OutputStyle, new Regex(@"int_2_SendV"));
                range.SetStyle(OutputStyle, new Regex(@"int_2_SendW"));
                range.SetStyle(OutputStyle, new Regex(@"int_2_SendX"));
                range.SetStyle(OutputStyle, new Regex(@"int_2_SendY"));
                range.SetStyle(OutputStyle, new Regex(@"int_2_SendZ"));
                range.SetStyle(OutputStyle, new Regex(@"int_2_SendLWIN"));
                range.SetStyle(OutputStyle, new Regex(@"int_2_SendRWIN"));
                range.SetStyle(OutputStyle, new Regex(@"int_2_SendAPPS"));
                range.SetStyle(OutputStyle, new Regex(@"int_2_SendDELETE"));
                range.SetStyle(OutputStyle, new Regex(@"int_2_SendNUMPAD0"));
                range.SetStyle(OutputStyle, new Regex(@"int_2_SendNUMPAD1"));
                range.SetStyle(OutputStyle, new Regex(@"int_2_SendNUMPAD2"));
                range.SetStyle(OutputStyle, new Regex(@"int_2_SendNUMPAD3"));
                range.SetStyle(OutputStyle, new Regex(@"int_2_SendNUMPAD4"));
                range.SetStyle(OutputStyle, new Regex(@"int_2_SendNUMPAD5"));
                range.SetStyle(OutputStyle, new Regex(@"int_2_SendNUMPAD6"));
                range.SetStyle(OutputStyle, new Regex(@"int_2_SendNUMPAD7"));
                range.SetStyle(OutputStyle, new Regex(@"int_2_SendNUMPAD8"));
                range.SetStyle(OutputStyle, new Regex(@"int_2_SendNUMPAD9"));
                range.SetStyle(OutputStyle, new Regex(@"int_2_SendMULTIPLY"));
                range.SetStyle(OutputStyle, new Regex(@"int_2_SendADD"));
                range.SetStyle(OutputStyle, new Regex(@"int_2_SendSUBTRACT"));
                range.SetStyle(OutputStyle, new Regex(@"int_2_SendDECIMAL"));
                range.SetStyle(OutputStyle, new Regex(@"int_2_SendPRINTSCREEN"));
                range.SetStyle(OutputStyle, new Regex(@"int_2_SendDIVIDE"));
                range.SetStyle(OutputStyle, new Regex(@"int_2_SendF1"));
                range.SetStyle(OutputStyle, new Regex(@"int_2_SendF2"));
                range.SetStyle(OutputStyle, new Regex(@"int_2_SendF3"));
                range.SetStyle(OutputStyle, new Regex(@"int_2_SendF4"));
                range.SetStyle(OutputStyle, new Regex(@"int_2_SendF5"));
                range.SetStyle(OutputStyle, new Regex(@"int_2_SendF6"));
                range.SetStyle(OutputStyle, new Regex(@"int_2_SendF7"));
                range.SetStyle(OutputStyle, new Regex(@"int_2_SendF8"));
                range.SetStyle(OutputStyle, new Regex(@"int_2_SendF9"));
                range.SetStyle(OutputStyle, new Regex(@"int_2_SendF10"));
                range.SetStyle(OutputStyle, new Regex(@"int_2_SendF11"));
                range.SetStyle(OutputStyle, new Regex(@"int_2_SendF12"));
                range.SetStyle(OutputStyle, new Regex(@"int_2_SendNUMLOCK"));
                range.SetStyle(OutputStyle, new Regex(@"int_2_SendSCROLLLOCK"));
                range.SetStyle(OutputStyle, new Regex(@"int_2_SendLEFTSHIFT"));
                range.SetStyle(OutputStyle, new Regex(@"int_2_SendRIGHTSHIFT"));
                range.SetStyle(OutputStyle, new Regex(@"int_2_SendLEFTCONTROL"));
                range.SetStyle(OutputStyle, new Regex(@"int_2_SendRIGHTCONTROL"));
                range.SetStyle(OutputStyle, new Regex(@"int_2_SendLEFTALT"));
                range.SetStyle(OutputStyle, new Regex(@"int_2_SendRIGHTALT"));
                range.SetStyle(OutputStyle, new Regex(@"int_2_SendBROWSER_BACK"));
                range.SetStyle(OutputStyle, new Regex(@"int_2_SendBROWSER_FORWARD"));
                range.SetStyle(OutputStyle, new Regex(@"int_2_SendBROWSER_REFRESH"));
                range.SetStyle(OutputStyle, new Regex(@"int_2_SendBROWSER_STOP"));
                range.SetStyle(OutputStyle, new Regex(@"int_2_SendBROWSER_SEARCH"));
                range.SetStyle(OutputStyle, new Regex(@"int_2_SendBROWSER_FAVORITES"));
                range.SetStyle(OutputStyle, new Regex(@"int_2_SendBROWSER_HOME"));
                range.SetStyle(OutputStyle, new Regex(@"int_2_SendVOLUME_MUTE"));
                range.SetStyle(OutputStyle, new Regex(@"int_2_SendVOLUME_DOWN"));
                range.SetStyle(OutputStyle, new Regex(@"int_2_SendVOLUME_UP"));
                range.SetStyle(OutputStyle, new Regex(@"int_2_SendMEDIA_NEXT_TRACK"));
                range.SetStyle(OutputStyle, new Regex(@"int_2_SendMEDIA_PREV_TRACK"));
                range.SetStyle(OutputStyle, new Regex(@"int_2_SendMEDIA_STOP"));
                range.SetStyle(OutputStyle, new Regex(@"int_2_SendMEDIA_PLAY_PAUSE"));
                range.SetStyle(OutputStyle, new Regex(@"int_2_SendLAUNCH_MAIL"));
                range.SetStyle(OutputStyle, new Regex(@"int_2_SendLAUNCH_MEDIA_SELECT"));
                range.SetStyle(OutputStyle, new Regex(@"int_2_SendLAUNCH_APP1"));
                range.SetStyle(OutputStyle, new Regex(@"int_2_SendLAUNCH_APP2"));
                range.SetStyle(OutputStyle, new Regex(@"int_2_SendOEM_1"));
                range.SetStyle(OutputStyle, new Regex(@"int_2_SendOEM_PLUS"));
                range.SetStyle(OutputStyle, new Regex(@"int_2_SendOEM_COMMA"));
                range.SetStyle(OutputStyle, new Regex(@"int_2_SendOEM_MINUS"));
                range.SetStyle(OutputStyle, new Regex(@"int_2_SendOEM_PERIOD"));
                range.SetStyle(OutputStyle, new Regex(@"int_2_SendOEM_2"));
                range.SetStyle(OutputStyle, new Regex(@"int_2_SendOEM_3"));
                range.SetStyle(OutputStyle, new Regex(@"int_2_SendOEM_4"));
                range.SetStyle(OutputStyle, new Regex(@"int_2_SendOEM_5"));
                range.SetStyle(OutputStyle, new Regex(@"int_2_SendOEM_6"));
                range.SetStyle(OutputStyle, new Regex(@"int_2_SendOEM_7"));
                range.SetStyle(OutputStyle, new Regex(@"int_2_SendOEM_8"));
                range.SetStyle(OutputStyle, new Regex(@"int_2_SendOEM_102"));
                range.SetStyle(OutputStyle, new Regex(@"int_2_SendEREOF"));
                range.SetStyle(OutputStyle, new Regex(@"int_2_SendZOOM"));
                range.SetStyle(OutputStyle, new Regex(@"int_2_SendEscape"));
                range.SetStyle(OutputStyle, new Regex(@"int_2_SendOne"));
                range.SetStyle(OutputStyle, new Regex(@"int_2_SendTwo"));
                range.SetStyle(OutputStyle, new Regex(@"int_2_SendThree"));
                range.SetStyle(OutputStyle, new Regex(@"int_2_SendFour"));
                range.SetStyle(OutputStyle, new Regex(@"int_2_SendFive"));
                range.SetStyle(OutputStyle, new Regex(@"int_2_SendSix"));
                range.SetStyle(OutputStyle, new Regex(@"int_2_SendSeven"));
                range.SetStyle(OutputStyle, new Regex(@"int_2_SendEight"));
                range.SetStyle(OutputStyle, new Regex(@"int_2_SendNine"));
                range.SetStyle(OutputStyle, new Regex(@"int_2_SendZero"));
                range.SetStyle(OutputStyle, new Regex(@"int_2_SendDashUnderscore"));
                range.SetStyle(OutputStyle, new Regex(@"int_2_SendPlusEquals"));
                range.SetStyle(OutputStyle, new Regex(@"int_2_SendBackspace"));
                range.SetStyle(OutputStyle, new Regex(@"int_2_SendTab"));
                range.SetStyle(OutputStyle, new Regex(@"int_2_SendOpenBracketBrace"));
                range.SetStyle(OutputStyle, new Regex(@"int_2_SendCloseBracketBrace"));
                range.SetStyle(OutputStyle, new Regex(@"int_2_SendEnter"));
                range.SetStyle(OutputStyle, new Regex(@"int_2_SendControl"));
                range.SetStyle(OutputStyle, new Regex(@"int_2_SendSemicolonColon"));
                range.SetStyle(OutputStyle, new Regex(@"int_2_SendSingleDoubleQuote"));
                range.SetStyle(OutputStyle, new Regex(@"int_2_SendTilde"));
                range.SetStyle(OutputStyle, new Regex(@"int_2_SendLeftShift"));
                range.SetStyle(OutputStyle, new Regex(@"int_2_SendBackslashPipe"));
                range.SetStyle(OutputStyle, new Regex(@"int_2_SendCommaLeftArrow"));
                range.SetStyle(OutputStyle, new Regex(@"int_2_SendPeriodRightArrow"));
                range.SetStyle(OutputStyle, new Regex(@"int_2_SendForwardSlashQuestionMark"));
                range.SetStyle(OutputStyle, new Regex(@"int_2_SendRightShift"));
                range.SetStyle(OutputStyle, new Regex(@"int_2_SendRightAlt"));
                range.SetStyle(OutputStyle, new Regex(@"int_2_SendSpace"));
                range.SetStyle(OutputStyle, new Regex(@"int_2_SendCapsLock"));
                range.SetStyle(OutputStyle, new Regex(@"int_2_SendUp"));
                range.SetStyle(OutputStyle, new Regex(@"int_2_SendDown"));
                range.SetStyle(OutputStyle, new Regex(@"int_2_SendRight"));
                range.SetStyle(OutputStyle, new Regex(@"int_2_SendLeft"));
                range.SetStyle(OutputStyle, new Regex(@"int_2_SendHome"));
                range.SetStyle(OutputStyle, new Regex(@"int_2_SendEnd"));
                range.SetStyle(OutputStyle, new Regex(@"int_2_SendDelete"));
                range.SetStyle(OutputStyle, new Regex(@"int_2_SendPageUp"));
                range.SetStyle(OutputStyle, new Regex(@"int_2_SendPageDown"));
                range.SetStyle(OutputStyle, new Regex(@"int_2_SendInsert"));
                range.SetStyle(OutputStyle, new Regex(@"int_2_SendPrintScreen"));
                range.SetStyle(OutputStyle, new Regex(@"int_2_SendNumLock"));
                range.SetStyle(OutputStyle, new Regex(@"int_2_SendScrollLock"));
                range.SetStyle(OutputStyle, new Regex(@"int_2_SendMenu"));
                range.SetStyle(OutputStyle, new Regex(@"int_2_SendWindowsKey"));
                range.SetStyle(OutputStyle, new Regex(@"int_2_SendNumpadDivide"));
                range.SetStyle(OutputStyle, new Regex(@"int_2_SendNumpadAsterisk"));
                range.SetStyle(OutputStyle, new Regex(@"int_2_SendNumpad7"));
                range.SetStyle(OutputStyle, new Regex(@"int_2_SendNumpad8"));
                range.SetStyle(OutputStyle, new Regex(@"int_2_SendNumpad9"));
                range.SetStyle(OutputStyle, new Regex(@"int_2_SendNumpad4"));
                range.SetStyle(OutputStyle, new Regex(@"int_2_SendNumpad5"));
                range.SetStyle(OutputStyle, new Regex(@"int_2_SendNumpad6"));
                range.SetStyle(OutputStyle, new Regex(@"int_2_SendNumpad1"));
                range.SetStyle(OutputStyle, new Regex(@"int_2_SendNumpad2"));
                range.SetStyle(OutputStyle, new Regex(@"int_2_SendNumpad3"));
                range.SetStyle(OutputStyle, new Regex(@"int_2_SendNumpad0"));
                range.SetStyle(OutputStyle, new Regex(@"int_2_SendNumpadDelete"));
                range.SetStyle(OutputStyle, new Regex(@"int_2_SendNumpadEnter"));
                range.SetStyle(OutputStyle, new Regex(@"int_2_SendNumpadPlus"));
                range.SetStyle(OutputStyle, new Regex(@"int_2_SendNumpadMinus"));
            }
            catch { }
        }
        private void FillAutocompletion()
        {
            this.autocompleteMenu1.Items = new string[] {
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
                "Key_PROCESSKEY",
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
                "camx",
                "camy",
                "Controller1ButtonAPressed",
                "Controller2ButtonAPressed",
                "Controller1ButtonBPressed",
                "Controller2ButtonBPressed",
                "Controller1ButtonXPressed",
                "Controller2ButtonXPressed",
                "Controller1ButtonYPressed",
                "Controller2ButtonYPressed",
                "Controller1ButtonStartPressed",
                "Controller2ButtonStartPressed",
                "Controller1ButtonBackPressed",
                "Controller2ButtonBackPressed",
                "Controller1ButtonDownPressed",
                "Controller2ButtonDownPressed",
                "Controller1ButtonUpPressed",
                "Controller2ButtonUpPressed",
                "Controller1ButtonLeftPressed",
                "Controller2ButtonLeftPressed",
                "Controller1ButtonRightPressed",
                "Controller2ButtonRightPressed",
                "Controller1ButtonShoulderLeftPressed",
                "Controller2ButtonShoulderLeftPressed",
                "Controller1ButtonShoulderRightPressed",
                "Controller2ButtonShoulderRightPressed",
                "Controller1ThumbpadLeftPressed",
                "Controller2ThumbpadLeftPressed",
                "Controller1ThumbpadRightPressed",
                "Controller2ThumbpadRightPressed",
                "Controller1TriggerLeftPosition",
                "Controller2TriggerLeftPosition",
                "Controller1TriggerRightPosition",
                "Controller2TriggerRightPosition",
                "Controller1ThumbLeftX",
                "Controller2ThumbLeftX",
                "Controller1ThumbLeftY",
                "Controller2ThumbLeftY",
                "Controller1ThumbRightX",
                "Controller2ThumbRightX",
                "Controller1ThumbRightY",
                "Controller2ThumbRightY",
                "Controller1GyroX",
                "Controller1GyroY",
                "Controller2GyroX",
                "Controller2GyroY",
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
                "MouseHookX",
                "MouseHookY",
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
                "TextFromSpeech",
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
                "PS4Controller2ButtonMicPressed",
                "_ValueChange",
                "UsersAllowedList",
                "PS5ControllerDescriptor",
                "PS4ControllerDescriptor",
                "NetworkDescriptor",
                "sleeptime",
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
                "centery",
                "irmode",
                "SpeechToText",
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
                "controller1_send_lefttrigger",
                "controller1_send_righttrigger",
                "controller1_send_leftstickx",
                "controller1_send_leftsticky",
                "controller1_send_rightstickx",
                "controller1_send_rightsticky",
                "controller2_send_xbox",
                "controller2_send_back",
                "controller2_send_start",
                "controller2_send_A",
                "controller2_send_B",
                "controller2_send_X",
                "controller2_send_Y",
                "controller2_send_up",
                "controller2_send_left",
                "controller2_send_down",
                "controller2_send_right",
                "controller2_send_leftstick",
                "controller2_send_rightstick",
                "controller2_send_leftbumper",
                "controller2_send_rightbumper",
                "controller2_send_lefttrigger",
                "controller2_send_righttrigger",
                "controller2_send_leftstickx",
                "controller2_send_leftsticky",
                "controller2_send_rightstickx",
                "controller2_send_rightsticky",
                "controller1_send_lefttriggerposition",
                "controller1_send_righttriggerposition",
                "controller2_send_lefttriggerposition",
                "controller2_send_righttriggerposition",
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
                "Controller2DS4_Send_Options",
                "Controller2DS4_Send_Option",
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
                "Controller2VJoy_Send_HatExt3",
                "EnableKM",
                "EnableXC",
                "EnableDS4",
                "EnableVJoy",
                "EnableRI",
                "EnableCI",
                "EnableXI",
                "EnableDI",
                "EnableJI",
                "EnablePI",
                "EnableWI",
                "EnableGI",
                "pollcount",
                "keys12345",
                "keys54321",
                "mousexp",
                "mouseyp",
                "statex",
                "statey",
                "mousex",
                "mousey",
                "mousestatex",
                "mousestatey",
                "viewpower1x",
                "viewpower2x",
                "viewpower3x",
                "viewpower1y",
                "viewpower2y",
                "viewpower3y",
                "dzx",
                "dzy",
                "viewpower05x",
                "viewpower05y",
                "state1x",
                "state1y",
                "mouse1x",
                "mouse1y",
                "mousestate1x",
                "mousestate1y",
                "state2x",
                "state2y",
                "mouse2x",
                "mouse2y",
                "mousestate2x",
                "mousestate2y",
                "statestickx",
                "statesticky",
                "mousestickx",
                "mousesticky",
                "mousestatestickx",
                "mousestatesticky",
                "testdouble",
                "testbool",
                "JoyconLeftAccelCenter",
                "JoyconRightAccelCenter",
                "ProControllerAccelCenter",
                "PS5ControllerAccelCenter",
                "PS4ControllerAccelCenter",
                "PS5Controller1AccelCenter",
                "PS4Controller1AccelCenter",
                "PS5Controller2AccelCenter",
                "PS4Controller2AccelCenter",
                "JoyconLeftStickCenter",
                "JoyconRightStickCenter",
                "ProControllerStickCenter",
                "xcnumber",
                "ds4number",
                "vjoynumber",
                "ValueChange",
                "ir1x",
                "ir1y",
                "Wiimote1ButtonStateA",
                "Wiimote1ButtonStateB",
                "Wiimote1ButtonStateMinus",
                "Wiimote1ButtonStateHome",
                "Wiimote1ButtonStatePlus",
                "Wiimote1ButtonStateOne",
                "Wiimote1ButtonStateTwo",
                "Wiimote1ButtonStateUp",
                "Wiimote1ButtonStateDown",
                "Wiimote1ButtonStateLeft",
                "Wiimote1ButtonStateRight",
                "Wiimote1RawValuesX",
                "Wiimote1RawValuesY",
                "Wiimote1RawValuesZ",
                "Wiimote1NunchuckStateRawJoystickX",
                "Wiimote1NunchuckStateRawJoystickY",
                "Wiimote1NunchuckStateRawValuesX",
                "Wiimote1NunchuckStateRawValuesY",
                "Wiimote1NunchuckStateRawValuesZ",
                "Wiimote1NunchuckStateC",
                "Wiimote1NunchuckStateZ",
                "ir2x",
                "ir2y",
                "Wiimote2ButtonStateA",
                "Wiimote2ButtonStateB",
                "Wiimote2ButtonStateMinus",
                "Wiimote2ButtonStateHome",
                "Wiimote2ButtonStatePlus",
                "Wiimote2ButtonStateOne",
                "Wiimote2ButtonStateTwo",
                "Wiimote2ButtonStateUp",
                "Wiimote2ButtonStateDown",
                "Wiimote2ButtonStateLeft",
                "Wiimote2ButtonStateRight",
                "Wiimote2RawValuesX",
                "Wiimote2RawValuesY",
                "Wiimote2RawValuesZ",
                "Wiimote2NunchuckStateRawJoystickX",
                "Wiimote2NunchuckStateRawJoystickY",
                "Wiimote2NunchuckStateRawValuesX",
                "Wiimote2NunchuckStateRawValuesY",
                "Wiimote2NunchuckStateRawValuesZ",
                "Wiimote2NunchuckStateC",
                "Wiimote2NunchuckStateZ",
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
                "int_2_SendNumpadMinus"
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