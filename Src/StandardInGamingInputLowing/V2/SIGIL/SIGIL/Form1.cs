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
        private static Style StyleInput = new TextStyle(Brushes.Blue, null, System.Drawing.FontStyle.Regular), StyleOutput = new TextStyle(Brushes.Orange, null, System.Drawing.FontStyle.Regular), StyleLibrary = new TextStyle(Brushes.BlueViolet, null, System.Drawing.FontStyle.Regular), StyleClass = new TextStyle(Brushes.DodgerBlue, null, System.Drawing.FontStyle.Regular), StyleMethod = new TextStyle(Brushes.Magenta, null, System.Drawing.FontStyle.Regular), StyleObject = new TextStyle(Brushes.DarkOrange, null, System.Drawing.FontStyle.Regular);
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
                range.SetStyle(StyleMethod, new Regex(@"ViewData"));
                range.SetStyle(StyleMethod, new Regex(@"Close"));
                range.SetStyle(StyleMethod, new Regex(@"BeginPolling"));
                range.SetStyle(StyleMethod, new Regex(@"BeginAsyncPolling"));
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
                range.SetStyle(StyleOutput, new Regex(@"controller1_send_lefttrigger"));
                range.SetStyle(StyleOutput, new Regex(@"controller1_send_righttrigger"));
                range.SetStyle(StyleOutput, new Regex(@"controller1_send_leftstickx"));
                range.SetStyle(StyleOutput, new Regex(@"controller1_send_leftsticky"));
                range.SetStyle(StyleOutput, new Regex(@"controller1_send_rightstickx"));
                range.SetStyle(StyleOutput, new Regex(@"controller1_send_rightsticky"));
            }
            catch { }
        }
        private void FillAutocompletion()
        {
            this.autocompleteMenu1.Items = new string[] {
                "ViewData",
                "Close",
                "BeginPolling",
                "BeginAsyncPolling",
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
                "controller1_send_lefttrigger",
                "controller1_send_righttrigger",
                "controller1_send_leftstickx",
                "controller1_send_leftsticky",
                "controller1_send_rightstickx",
                "controller1_send_rightsticky"
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