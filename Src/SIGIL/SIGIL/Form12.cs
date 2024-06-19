﻿using System;
using System.Drawing;
using System.Threading.Tasks;
using System.Windows.Forms;
using SharpDX.XInput;
using System.Runtime.InteropServices;
using Microsoft.Web.WebView2.Core;
using WebView2 = Microsoft.Web.WebView2.WinForms.WebView2;
using System.Collections.Generic;
using System.Linq;

namespace SIGIL
{
    public partial class Form12 : Form
    {
        public Form12()
        {
            InitializeComponent();
        }
        [DllImport("user32.dll")]
        public static extern bool GetAsyncKeyState(System.Windows.Forms.Keys vKey);
        [DllImport("winmm.dll", EntryPoint = "timeBeginPeriod")]
        public static extern uint TimeBeginPeriod(uint ms);
        [DllImport("winmm.dll", EntryPoint = "timeEndPeriod")]
        public static extern uint TimeEndPeriod(uint ms);
        [DllImport("ntdll.dll", EntryPoint = "NtSetTimerResolution")]
        public static extern void NtSetTimerResolution(uint DesiredResolution, bool SetResolution, ref uint CurrentResolution);
        public static uint CurrentResolution = 0;
        public bool closed = false;
        public int x, y, Width, Height;
        public WebView2 webView21 = new WebView2();
        public List<double> List_A = new List<double>(), List_B = new List<double>(), List_X = new List<double>(), List_Y = new List<double>(), List_LB = new List<double>(), List_RB = new List<double>(), List_LT = new List<double>(), List_RT = new List<double>(), List_MAP = new List<double>(), List_MENU = new List<double>(), List_LSTICK = new List<double>(), List_RSTICK = new List<double>(), List_DU = new List<double>(), List_DD = new List<double>(), List_DL = new List<double>(), List_DR = new List<double>(), List_XBOX = new List<double>();
        public bool Controller_A, Controller_B, Controller_X, Controller_Y, Controller_LB, Controller_RB, Controller_MAP, Controller_MENU, Controller_LSTICK, Controller_RSTICK, Controller_DU, Controller_DD, Controller_DL, Controller_DR, Controller_XBOX;
        public double Controller_LT, Controller_RT, Controller_LX, Controller_LY, Controller_RX, Controller_RY;
        private Controller[] controller = new Controller[] { null };
        public int xnum;
        private State state;
        public bool Controller1ButtonAPressed;
        public bool Controller1ButtonBPressed;
        public bool Controller1ButtonXPressed;
        public bool Controller1ButtonYPressed;
        public bool Controller1ButtonStartPressed;
        public bool Controller1ButtonBackPressed;
        public bool Controller1ButtonDownPressed;
        public bool Controller1ButtonUpPressed;
        public bool Controller1ButtonLeftPressed;
        public bool Controller1ButtonRightPressed;
        public bool Controller1ButtonShoulderLeftPressed;
        public bool Controller1ButtonShoulderRightPressed;
        public bool Controller1ThumbpadLeftPressed;
        public bool Controller1ThumbpadRightPressed;
        public double Controller1TriggerLeftPosition;
        public double Controller1TriggerRightPosition;
        public double Controller1ThumbLeftX;
        public double Controller1ThumbLeftY;
        public double Controller1ThumbRightX;
        public double Controller1ThumbRightY;
        private int[] wd = { 2, 2, 2, 2, 2, 2, 2, 2 };
        private int[] wu = { 2, 2, 2, 2, 2, 2, 2, 2 };
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
        private async void Form12_Shown(object sender, EventArgs e)
        {
            TimeBeginPeriod(1);
            NtSetTimerResolution(1, true, ref CurrentResolution);
            Width = Screen.PrimaryScreen.Bounds.Width;
            Height = Screen.PrimaryScreen.Bounds.Height;
            CoreWebView2EnvironmentOptions options = new CoreWebView2EnvironmentOptions("--disable-web-security --allow-file-access-from-files --allow-file-access", "en");
            CoreWebView2Environment environment = await CoreWebView2Environment.CreateAsync(null, null, options);
            await webView21.EnsureCoreWebView2Async(environment);
            webView21.CoreWebView2.SetVirtualHostNameToFolderMapping("appassets", "assets", CoreWebView2HostResourceAccessKind.DenyCors);
            webView21.CoreWebView2.Settings.AreDevToolsEnabled = true;
            webView21.KeyDown += WebView21_KeyDown;
            webView21.Source = new Uri("https://appassets/xcoverlay/index.html");
            webView21.Dock = DockStyle.Fill;
            webView21.DefaultBackgroundColor = Color.Transparent;
            this.Controls.Add(webView21);
            this.Location = new Point(Width - this.Size.Width, Height - this.Size.Height);
            try
            {
                var controllers = new[] { new Controller(UserIndex.One) };
                xnum = 0;
                foreach (var selectControler in controllers)
                {
                    if (selectControler.IsConnected)
                    {
                        controller[xnum] = selectControler;
                        xnum++;
                        if (xnum > 0)
                        {
                            break;
                        }
                    }
                }
            }
            catch { }
        }
        private void Form12_KeyDown(object sender, KeyEventArgs e)
        {
            OnKeyDown(e.KeyData);
        }
        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            OnKeyDown(keyData);
            return true;
        }
        private void WebView21_KeyDown(object sender, KeyEventArgs e)
        {
            OnKeyDown(e.KeyData);
        }
        private void OnKeyDown(Keys keyData)
        {
            if (keyData == Keys.F1)
            {
                const string message = "• Author: Michaël André Franiatte.\n\r\n\r• Contact: michael.franiatte@gmail.com.\n\r\n\r• Publisher: https://github.com/michaelandrefraniatte.\n\r\n\r• Copyrights: All rights reserved, no permissions granted.\n\r\n\r• License: Not open source, not free of charge to use.";
                const string caption = "About";
                MessageBox.Show(message, caption, MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
        private void timer1_Tick(object sender, EventArgs e)
        {
            valchanged(0, GetAsyncKeyState(Keys.NumPad1));
            if (wu[0] == 1)
            {
                this.Location = new Point(0, Height - this.Size.Height);
            }
            valchanged(1, GetAsyncKeyState(Keys.NumPad4));
            if (wu[1] == 1)
            {
                this.Location = new Point(0, (Height - this.Size.Height) / 2);
            }
            valchanged(2, GetAsyncKeyState(Keys.NumPad7));
            if (wu[2] == 1)
            {
                this.Location = new Point(0, 0);
            }
            valchanged(3, GetAsyncKeyState(Keys.NumPad8));
            if (wu[3] == 1)
            {
                this.Location = new Point((Width - this.Size.Width) / 2, 0);
            }
            valchanged(4, GetAsyncKeyState(Keys.NumPad9));
            if (wu[4] == 1)
            {
                this.Location = new Point(Width - this.Size.Width, 0);
            }
            valchanged(5, GetAsyncKeyState(Keys.NumPad6));
            if (wu[5] == 1)
            {
                this.Location = new Point(Width - this.Size.Width, (Height - this.Size.Height) / 2);
            }
            valchanged(6, GetAsyncKeyState(Keys.NumPad3));
            if (wu[6] == 1)
            {
                this.Location = new Point(Width - this.Size.Width, Height - this.Size.Height);
            }
            valchanged(7, GetAsyncKeyState(Keys.NumPad2));
            if (wu[7] == 1)
            {
                this.Location = new Point((Width - this.Size.Width) / 2, Height - this.Size.Height);
            }
            taskEmulate();
        }
        private double Scale(double value, double min, double max, double minScale, double maxScale)
        {
            double scaled = minScale + (double)(value - min) / (max - min) * (maxScale - minScale);
            return scaled;
        }
        public async void SetController(bool Controller1ButtonAPressed, bool Controller1ButtonBPressed, bool Controller1ButtonXPressed, bool Controller1ButtonYPressed, bool Controller1ButtonStartPressed, bool Controller1ButtonBackPressed, bool Controller1ButtonDownPressed, bool Controller1ButtonUpPressed, bool Controller1ButtonLeftPressed, bool Controller1ButtonRightPressed, bool Controller1ButtonShoulderLeftPressed, bool Controller1ButtonShoulderRightPressed, bool Controller1ThumbpadLeftPressed, bool Controller1ThumbpadRightPressed, double Controller1TriggerLeftPosition, double Controller1TriggerRightPosition, double Controller1ThumbLeftX, double Controller1ThumbLeftY, double Controller1ThumbRightX, double Controller1ThumbRightY)
        {
            try
            {
                Controller_RX = Controller1ThumbRightX;
                Controller_RY = Controller1ThumbRightY;
                Controller_LX = Controller1ThumbLeftX;
                Controller_LY = Controller1ThumbLeftY;
                Controller_RT = Controller1TriggerRightPosition;
                Controller_LT = Controller1TriggerLeftPosition;
                if (List_A.Count >= 5)
                {
                    List_A.RemoveAt(0);
                    List_A.Add(Controller1ButtonAPressed ? 1 : 0);
                    Controller_A = List_A.Average() > 0 ? true : false;
                }
                else
                {
                    List_A.Add(0);
                }
                if (List_B.Count >= 5)
                {
                    List_B.RemoveAt(0);
                    List_B.Add(Controller1ButtonBPressed ? 1 : 0);
                    Controller_B = List_B.Average() > 0 ? true : false;
                }
                else
                {
                    List_B.Add(0);
                }
                if (List_X.Count >= 5)
                {
                    List_X.RemoveAt(0);
                    List_X.Add(Controller1ButtonXPressed ? 1 : 0);
                    Controller_X = List_X.Average() > 0 ? true : false;
                }
                else
                {
                    List_X.Add(0);
                }
                if (List_Y.Count >= 5)
                {
                    List_Y.RemoveAt(0);
                    List_Y.Add(Controller1ButtonYPressed ? 1 : 0);
                    Controller_Y = List_Y.Average() > 0 ? true : false;
                }
                else
                {
                    List_Y.Add(0);
                }
                if (List_LB.Count >= 5)
                {
                    List_LB.RemoveAt(0);
                    List_LB.Add(Controller1ButtonShoulderLeftPressed ? 1 : 0);
                    Controller_LB = List_LB.Average() > 0 ? true : false;
                }
                else
                {
                    List_LB.Add(0);
                }
                if (List_RB.Count >= 5)
                {
                    List_RB.RemoveAt(0);
                    List_RB.Add(Controller1ButtonShoulderRightPressed ? 1 : 0);
                    Controller_RB = List_RB.Average() > 0 ? true : false;
                }
                else
                {
                    List_RB.Add(0);
                }
                if (List_MAP.Count >= 5)
                {
                    List_MAP.RemoveAt(0);
                    List_MAP.Add(Controller1ButtonBackPressed ? 1 : 0);
                    Controller_MAP = List_MAP.Average() > 0 ? true : false;
                }
                else
                {
                    List_MAP.Add(0);
                }
                if (List_MENU.Count >= 5)
                {
                    List_MENU.RemoveAt(0);
                    List_MENU.Add(Controller1ButtonStartPressed ? 1 : 0);
                    Controller_MENU = List_MENU.Average() > 0 ? true : false;
                }
                else
                {
                    List_MENU.Add(0);
                }
                if (List_LSTICK.Count >= 5)
                {
                    List_LSTICK.RemoveAt(0);
                    List_LSTICK.Add(Controller1ThumbpadLeftPressed ? 1 : 0);
                    Controller_LSTICK = List_LSTICK.Average() > 0 ? true : false;
                }
                else
                {
                    List_LSTICK.Add(0);
                }
                if (List_RSTICK.Count >= 5)
                {
                    List_RSTICK.RemoveAt(0);
                    List_RSTICK.Add(Controller1ThumbpadRightPressed ? 1 : 0);
                    Controller_RSTICK = List_RSTICK.Average() > 0 ? true : false;
                }
                else
                {
                    List_RSTICK.Add(0);
                }
                if (List_DU.Count >= 5)
                {
                    List_DU.RemoveAt(0);
                    List_DU.Add(Controller1ButtonUpPressed ? 1 : 0);
                    Controller_DU = List_DU.Average() > 0 ? true : false;
                }
                else
                {
                    List_DU.Add(0);
                }
                if (List_DD.Count >= 5)
                {
                    List_DD.RemoveAt(0);
                    List_DD.Add(Controller1ButtonDownPressed ? 1 : 0);
                    Controller_DD = List_DD.Average() > 0 ? true : false;
                }
                else
                {
                    List_DD.Add(0);
                }
                if (List_DL.Count >= 5)
                {
                    List_DL.RemoveAt(0);
                    List_DL.Add(Controller1ButtonLeftPressed ? 1 : 0);
                    Controller_DL = List_DL.Average() > 0 ? true : false;
                }
                else
                {
                    List_DL.Add(0);
                }
                if (List_DR.Count >= 5)
                {
                    List_DR.RemoveAt(0);
                    List_DR.Add(Controller1ButtonRightPressed ? 1 : 0);
                    Controller_DR = List_DR.Average() > 0 ? true : false;
                }
                else
                {
                    List_DR.Add(0);
                }
                if (List_XBOX.Count >= 5)
                {
                    List_XBOX.RemoveAt(0);
                    List_XBOX.Add(false ? 1 : 0);
                    Controller_XBOX = List_XBOX.Average() > 0 ? true : false;
                }
                else
                {
                    List_XBOX.Add(0);
                }
                await execScriptHelper($"setController('{Controller_A.ToString()}', '{Controller_B.ToString()}', '{Controller_X.ToString()}', '{Controller_Y.ToString()}', '{Controller_MAP.ToString()}', '{Controller_MENU.ToString()}', '{Controller_DD.ToString()}', '{Controller_DU.ToString()}', '{Controller_DL.ToString()}', '{Controller_DR.ToString()}', '{Controller_LB.ToString()}', '{Controller_RB.ToString()}', '{Controller_LSTICK.ToString()}', '{Controller_RSTICK.ToString()}', '{Controller_LT.ToString()}', '{Controller_RT.ToString()}', '{Controller_XBOX.ToString()}', '{Controller_LX.ToString()}', '{Controller_LY.ToString()}', '{Controller_RX.ToString()}', '{Controller_RY.ToString()}');");
            }
            catch { }
        }
        private async Task<String> execScriptHelper(String script)
        {
            var x = await webView21.ExecuteScriptAsync(script).ConfigureAwait(false);
            return x;
        }
        private async void taskEmulate()
        {
            try
            {
                for (int inc = 0; inc < xnum; inc++)
                {
                    state = controller[inc].GetState();
                    if (inc == 0)
                    {
                        if (state.Gamepad.Buttons.ToString().Contains("A"))
                            Controller1ButtonAPressed = true;
                        else
                            Controller1ButtonAPressed = false;
                        if (state.Gamepad.Buttons.ToString().EndsWith("B") | state.Gamepad.Buttons.ToString().Contains("B, "))
                            Controller1ButtonBPressed = true;
                        else
                            Controller1ButtonBPressed = false;
                        if (state.Gamepad.Buttons.ToString().Contains("X"))
                            Controller1ButtonXPressed = true;
                        else
                            Controller1ButtonXPressed = false;
                        if (state.Gamepad.Buttons.ToString().Contains("Y"))
                            Controller1ButtonYPressed = true;
                        else
                            Controller1ButtonYPressed = false;
                        if (state.Gamepad.Buttons.ToString().Contains("Start"))
                            Controller1ButtonStartPressed = true;
                        else
                            Controller1ButtonStartPressed = false;
                        if (state.Gamepad.Buttons.ToString().Contains("Back"))
                            Controller1ButtonBackPressed = true;
                        else
                            Controller1ButtonBackPressed = false;
                        if (state.Gamepad.Buttons.ToString().Contains("DPadDown"))
                            Controller1ButtonDownPressed = true;
                        else
                            Controller1ButtonDownPressed = false;
                        if (state.Gamepad.Buttons.ToString().Contains("DPadUp"))
                            Controller1ButtonUpPressed = true;
                        else
                            Controller1ButtonUpPressed = false;
                        if (state.Gamepad.Buttons.ToString().Contains("DPadLeft"))
                            Controller1ButtonLeftPressed = true;
                        else
                            Controller1ButtonLeftPressed = false;
                        if (state.Gamepad.Buttons.ToString().Contains("DPadRight"))
                            Controller1ButtonRightPressed = true;
                        else
                            Controller1ButtonRightPressed = false;
                        if (state.Gamepad.Buttons.ToString().Contains("LeftShoulder"))
                            Controller1ButtonShoulderLeftPressed = true;
                        else
                            Controller1ButtonShoulderLeftPressed = false;
                        if (state.Gamepad.Buttons.ToString().Contains("RightShoulder"))
                            Controller1ButtonShoulderRightPressed = true;
                        else
                            Controller1ButtonShoulderRightPressed = false;
                        if (state.Gamepad.Buttons.ToString().Contains("LeftThumb"))
                            Controller1ThumbpadLeftPressed = true;
                        else
                            Controller1ThumbpadLeftPressed = false;
                        if (state.Gamepad.Buttons.ToString().Contains("RightThumb"))
                            Controller1ThumbpadRightPressed = true;
                        else
                            Controller1ThumbpadRightPressed = false;
                        Controller1TriggerLeftPosition = state.Gamepad.LeftTrigger;
                        Controller1TriggerRightPosition = state.Gamepad.RightTrigger;
                        Controller1ThumbLeftX = state.Gamepad.LeftThumbX;
                        Controller1ThumbLeftY = state.Gamepad.LeftThumbY;
                        Controller1ThumbRightX = state.Gamepad.RightThumbX;
                        Controller1ThumbRightY = state.Gamepad.RightThumbY;
                        SetController(Controller1ButtonAPressed, Controller1ButtonBPressed, Controller1ButtonXPressed, Controller1ButtonYPressed, Controller1ButtonStartPressed, Controller1ButtonBackPressed, Controller1ButtonDownPressed, Controller1ButtonUpPressed, Controller1ButtonLeftPressed, Controller1ButtonRightPressed, Controller1ButtonShoulderLeftPressed, Controller1ButtonShoulderRightPressed, Controller1ThumbpadLeftPressed, Controller1ThumbpadRightPressed, Controller1TriggerLeftPosition, Controller1TriggerRightPosition, Controller1ThumbLeftX, Controller1ThumbLeftY, Controller1ThumbRightX, Controller1ThumbRightY);
                    }
                }
            }
            catch { }
        }
        private void Form12_FormClosed(object sender, FormClosedEventArgs e)
        {
            closed = true;
        }
    }
}