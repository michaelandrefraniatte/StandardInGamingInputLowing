using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.Diagnostics;
using System.Drawing.Imaging;
using System.IO;

namespace SIGIL
{
    public partial class Form17 : Form
    {
        public Form17()
        {
            InitializeComponent();
        }
        [DllImport("advapi32.dll")]
        private static extern bool LogonUser(string lpszUsername, string lpszDomain, string lpszPassword, int dwLogonType, int dwLogonProvider, out IntPtr phToken);
        [DllImport("user32.dll")]
        public static extern bool GetAsyncKeyState(System.Windows.Forms.Keys vKey);
        [DllImport("winmm.dll", EntryPoint = "timeBeginPeriod")]
        public static extern uint TimeBeginPeriod(uint ms);
        [DllImport("winmm.dll", EntryPoint = "timeEndPeriod")]
        public static extern uint TimeEndPeriod(uint ms);
        [DllImport("ntdll.dll", EntryPoint = "NtSetTimerResolution")]
        public static extern void NtSetTimerResolution(uint DesiredResolution, bool SetResolution, ref uint CurrentResolution);
        public static uint CurrentResolution = 0;
        private int height, width, heightstart, widthstart, itemIndex = 0, pollingcount, fpscount = 1, fpscountmin = 300, fpscountmax = 1;
        private double watchM = 50, watchM1 = 2, watchM2 = 0;
        private string text = "", lastkey = "F2";
        public ohmDataTree ohmdt = new ohmDataTree();
        public const string noSenseText = "No sensors";
        public List<string> lvg = new List<string>();
        private Stopwatch diffM = new Stopwatch();
        private Bitmap screenshotstart = null, screenshot = null;
        private int[] wd = { 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2 };
        private int[] wu = { 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2 };
        private void valchanged(int n, bool val)
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
        private void Form17_Shown(object sender, EventArgs e)
        {
            TimeBeginPeriod(1);
            NtSetTimerResolution(1, true, ref CurrentResolution);
            this.TopMost = true;
            Application.ApplicationExit += new EventHandler(this.OnApplicationExit);
            heightstart = Screen.PrimaryScreen.Bounds.Height;
            widthstart = 400;
            this.Size = new Size(width, height);
            this.ClientSize = new Size(width, height);
            this.Location = new System.Drawing.Point(Screen.PrimaryScreen.Bounds.Width - width, 0);
            ohmdt.MainboardEnabled = true;
            ohmdt.FanControllerEnabled = true;
            ohmdt.CPUEnabled = true;
            ohmdt.GPUEnabled = true;
            ohmdt.RAMEnabled = true;
            ohmdt.HDDEnabled = true;
            diffM.Start();
        }
        private void timer2_Tick(object sender, EventArgs e)
        {
            try
            {
                watchM2 = (double)diffM.ElapsedTicks / (Stopwatch.Frequency / (1000L * 1000L));
                watchM += (watchM2 - watchM1) / 1000f;
                watchM1 = watchM2;
                screenshotstart = screenshot;
                Bitmap bitmap = new Bitmap(Screen.PrimaryScreen.Bounds.Width, Screen.PrimaryScreen.Bounds.Height);
                using (Graphics g = Graphics.FromImage(bitmap))
                {
                    g.CopyFromScreen(new Point(Screen.PrimaryScreen.Bounds.Left, Screen.PrimaryScreen.Bounds.Top), Point.Empty, Screen.PrimaryScreen.Bounds.Size);
                }
                screenshot = bitmap;
                if (!SameImage((Image)screenshot, (Image)screenshotstart))
                {
                    pollingcount++;
                }
                if (watchM < 1000f & watchM >= 900f)
                {
                    fpscount = (int)(pollingcount / watchM * 1000f * timer2.Interval);
                    if (fpscount > 1)
                    {
                        fpscountmin = Math.Min(fpscountmin, fpscount);
                        fpscountmax = Math.Max(fpscountmax, fpscount);
                    }
                }
                if (watchM >= 1000f)
                {
                    pollingcount = 1;
                    watchM = timer2.Interval;
                }
            }
            catch { }
        }
        private void Form17_Activated(object sender, EventArgs e)
        {
            this.FormBorderStyle = FormBorderStyle.FixedToolWindow;
        }
        private void Form17_Deactivate(object sender, EventArgs e)
        {
            this.FormBorderStyle = FormBorderStyle.None;
        }
        private string getItems(ohmHwNode[] items, ref int itemIndex)
        {
            text = "";
            try
            {
                text += "FPS: " + fpscount + ", " + fpscountmin + ", " + fpscountmax + Environment.NewLine + Environment.NewLine;
                for (int i = 0; i < items.Length; i++)
                {
                    string header = "Name: " + items[i].name + " Type: " + items[i].type.ToString();
                    if (items[i].ohmSensors == null)
                    {
                        itemIndex++;
                    }
                    else
                    {
                        text += header + Environment.NewLine + Environment.NewLine;
                        for (int j = 0; j < items[i].ohmSensors.Length; j++)
                        {
                            text += items[i].ohmSensors[j].sType.ToString() + ", ";
                            text += items[i].ohmSensors[j].Index.ToString() + ", ";
                            text += items[i].ohmSensors[j].Name + ", ";
                            text += items[i].ohmSensors[j].stValue + ", ";
                            text += items[i].ohmSensors[j].stMin + ", ";
                            text += items[i].ohmSensors[j].stMax + Environment.NewLine;
                            itemIndex++;
                        }
                        text += Environment.NewLine;
                    }
                    text += Environment.NewLine;
                }
            }
            catch { }
            return text;
        }
        private void timer1_Tick(object sender, EventArgs e)
        {
            try
            {
                itemIndex = 0;
                label1.Text = getItems(ohmdt.hWareList, ref itemIndex);
                width = this.label1.Width + 10;
                height = this.label1.Height + 10;
                if (width != widthstart | height != heightstart)
                {
                    widthstart = width;
                    heightstart = height;
                    if (lastkey == "F1")
                    {
                        this.Size = new Size(width, height);
                        this.ClientSize = new Size(width, height);
                        this.Location = new System.Drawing.Point(0, 0);
                    }
                    if (lastkey == "F2")
                    {
                        this.Size = new Size(width, height);
                        this.ClientSize = new Size(width, height);
                        this.Location = new System.Drawing.Point(Screen.PrimaryScreen.Bounds.Width - width, 0);
                    }
                    if (lastkey == "F3")
                    {
                        this.Size = new Size(0, 0);
                        this.ClientSize = new Size(0, 0);
                        this.Location = new System.Drawing.Point(0, 0);
                    }
                }
                valchanged(1, GetAsyncKeyState(Keys.F1));
                if (wd[1] == 1)
                {
                    this.Size = new Size(width, height);
                    this.ClientSize = new Size(width, height);
                    this.Location = new System.Drawing.Point(0, 0);
                    lastkey = "F1";
                }
                valchanged(2, GetAsyncKeyState(Keys.F2));
                if (wd[2] == 1)
                {
                    this.Size = new Size(width, height);
                    this.ClientSize = new Size(width, height);
                    this.Location = new System.Drawing.Point(Screen.PrimaryScreen.Bounds.Width - width, 0);
                    lastkey = "F2";
                }
                valchanged(3, GetAsyncKeyState(Keys.F3));
                if (wd[3] == 1)
                {
                    this.Size = new Size(0, 0);
                    this.ClientSize = new Size(0, 0);
                    this.Location = new System.Drawing.Point(0, 0);
                    lastkey = "F3";
                }
                valchanged(4, GetAsyncKeyState(Keys.F4));
                if (wd[4] == 1)
                {
                    fpscountmin = fpscount;
                    fpscountmax = fpscount;
                }
            }
            catch { }
        }
        private bool SameImage(Image image1, Image image2)
        {
            byte[] image1Bytes;
            byte[] image2Bytes;
            using (var mstream = new MemoryStream())
            {
                image1.Save(mstream, ImageFormat.Bmp);
                image1Bytes = mstream.ToArray();
            }
            using (var mstream = new MemoryStream())
            {
                image2.Save(mstream, ImageFormat.Bmp);
                image2Bytes = mstream.ToArray();
            }
            var image164 = Convert.ToBase64String(image1Bytes);
            var image264 = Convert.ToBase64String(image2Bytes);
            return string.Equals(image164, image264);
        }
        private void Form17_FormClosed(object sender, FormClosedEventArgs e)
        {
            FormClose();
        }
        private void OnApplicationExit(object sender, EventArgs e)
        {
            FormClose();
        }
        private void FormClose()
        {
            TimeEndPeriod(1);
        }
        private void Form17_KeyDown(object sender, KeyEventArgs e)
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
    }
}