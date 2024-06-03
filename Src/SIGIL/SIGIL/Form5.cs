﻿using NAudio.Wave;
using System;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using System.Windows.Forms;
using WebSocketSharp.Server;
using WebSocketSharp;
using System.IO;
using System.Threading;
using System.Drawing.Drawing2D;
using System.Drawing;

namespace SIGIL
{
    public partial class Form5 : Form
    {
        public Form5()
        {
            InitializeComponent();
        }
        [DllImport("winmm.dll", EntryPoint = "timeBeginPeriod")]
        private static extern uint TimeBeginPeriod(uint ms);
        [DllImport("winmm.dll", EntryPoint = "timeEndPeriod")]
        private static extern uint TimeEndPeriod(uint ms);
        [DllImport("ntdll.dll", EntryPoint = "NtSetTimerResolution")]
        private static extern void NtSetTimerResolution(uint DesiredResolution, bool SetResolution, ref uint CurrentResolution);
        private static uint CurrentResolution = 0;
        private static bool running = false, closed = false;
        public static string displayport, audioport, localip;
        private static int width = Screen.PrimaryScreen.Bounds.Width, height = Screen.PrimaryScreen.Bounds.Height;
        public static byte[] rawdataavailable;
        public static Action<bool> actionaudio = null, actiondisplay = null;
        private void Form5_Shown(object sender, EventArgs e)
        {
            TimeBeginPeriod(1);
            NtSetTimerResolution(1, true, ref CurrentResolution);
            if (File.Exists(Application.StartupPath + @"\tempremote"))
            {
                using (StreamReader file = new StreamReader(Application.StartupPath + @"\tempremote"))
                {
                    textBox1.Text = file.ReadLine();
                    textBox2.Text = file.ReadLine();
                    textBox3.Text = file.ReadLine();
                }
            }
            Task.Run(() => CopyScreen());
        }
        private void button1_Click(object sender, EventArgs e)
        {
            if (!running)
            {
                button1.Text = "Stop";
                running = true;
                localip = textBox1.Text;
                displayport = textBox2.Text;
                audioport = textBox3.Text;
                Task.Run(() => LSP1Display.Connect());
                Task.Run(() => LSPAudio.Connect());
            }
            else
            {
                button1.Text = "Start";
                running = false;
                Thread.Sleep(100);
                Task.Run(() => LSP1Display.Disconnect());
                Task.Run(() => LSPAudio.Disconnect());
            }
        }
        private void Form5_FormClosed(object sender, FormClosedEventArgs e)
        {
            running = false;
            Thread.Sleep(300);
            using (StreamWriter createdfile = new StreamWriter(Application.StartupPath + @"\tempremote"))
            {
                createdfile.WriteLine(textBox1.Text);
                createdfile.WriteLine(textBox2.Text);
                createdfile.WriteLine(textBox3.Text);
            }
            closed = true;
        }
        private void Form5_KeyDown(object sender, KeyEventArgs e)
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
        private void CopyScreen()
        {
            while (!closed)
            {
                CaptureScreen();
                Thread.Sleep(1);
            }
        }
        private void CaptureScreen()
        {
            MemoryStream file = new MemoryStream();
            Bitmap bitmap = new Bitmap(width, height);
            Graphics graphics = Graphics.FromImage(bitmap);
            graphics.PixelOffsetMode = PixelOffsetMode.HighSpeed;
            graphics.SmoothingMode = SmoothingMode.HighSpeed;
            graphics.InterpolationMode = InterpolationMode.Low;
            graphics.CompositingMode = CompositingMode.SourceCopy;
            graphics.CompositingQuality = CompositingQuality.HighSpeed;
            graphics.CopyFromScreen(0, 0, 0, 0, bitmap.Size);
            bitmap.Save(file, System.Drawing.Imaging.ImageFormat.Jpeg);
            rawdataavailable = file.ToArray();
        }
        public class LSPAudio
        {
            private static string localip;
            private static string port;
            private static Audio audio = new Audio();
            private static WebSocketServer wss;
            private static WasapiLoopbackCapture waveIn = null;
            public static void Connect()
            {
                try
                {
                    localip = Form5.localip;
                    port = Form5.audioport;
                    String connectionString = "ws://" + localip + ":" + port;
                    wss = new WebSocketServer(connectionString);
                    wss.AddWebSocketService<Audio>("/Audio");
                    wss.Start();
                    GetAudioByteArray();
                }
                catch { }
            }
            private static void GetAudioByteArray()
            {
                waveIn = new WasapiLoopbackCapture();
                waveIn.DataAvailable += audio.waveIn_DataAvailable;
                waveIn.StartRecording();
            }
            public static void Disconnect()
            {
                wss.RemoveWebSocketService("/Audio");
                wss.Stop();
                waveIn.Dispose();
            }
        }
        public class Audio : WebSocketBehavior
        {
            private static byte[] rawdataavailable = null, raw = null;
            protected override void OnMessage(MessageEventArgs e)
            {
                base.OnMessage(e);
                while (Form5.running)
                {
                    if (rawdataavailable != null)
                        SendAsync(rawdataavailable, Form5.actionaudio);
                    rawdataavailable = null;
                    Thread.Sleep(1);
                }
            }
            public void waveIn_DataAvailable(object sender, WaveInEventArgs e)
            {
                raw = e.Buffer;
                Array.Resize(ref raw, e.BytesRecorded);
                rawdataavailable = raw;
            }
        }
        public class LSP1Display
        {
            private static string localip;
            private static string port;
            private static WebSocketServer wss;
            public static void Connect()
            {
                try
                {
                    localip = Form5.localip;
                    port = Form5.displayport;
                    String connectionString = "ws://" + localip + ":" + port;
                    wss = new WebSocketServer(connectionString);
                    wss.AddWebSocketService<Display1>("/1Display");
                    wss.Start();
                }
                catch { }
            }
            public static void Disconnect()
            {
                wss.RemoveWebSocketService("/1Display");
                wss.Stop();
            }
        }
        public class Display1 : WebSocketBehavior
        {
            protected override void OnMessage(MessageEventArgs e)
            {
                base.OnMessage(e);
                while (Form5.running)
                {
                    try
                    {
                        SendAsync(Form5.rawdataavailable, actiondisplay);
                    }
                    catch { }
                    Thread.Sleep(50);
                }
            }
        }
    }
}