﻿using System;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.IO;
using System.Diagnostics;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Threading;
using NAudio.CoreAudioApi;
using NAudio.Wave;
using NAudio.Lame;

namespace SIGIL
{
    public partial class Form21 : Form
    {
        public Form21()
        {
            InitializeComponent();
        }
        [DllImport("advapi32.dll")]
        private static extern bool LogonUser(string lpszUsername, string lpszDomain, string lpszPassword, int dwLogonType, int dwLogonProvider, out IntPtr phToken);
        [DllImport("winmm.dll", EntryPoint = "timeBeginPeriod")]
        public static extern uint TimeBeginPeriod(uint ms);
        [DllImport("winmm.dll", EntryPoint = "timeEndPeriod")]
        public static extern uint TimeEndPeriod(uint ms);
        [DllImport("ntdll.dll", EntryPoint = "NtSetTimerResolution")]
        public static extern void NtSetTimerResolution(uint DesiredResolution, bool SetResolution, ref uint CurrentResolution);
        [DllImport("user32.dll")]
        public static extern bool GetAsyncKeyState(Keys vKey);
        public static ThreadStart threadstart;
        public static Thread thread;
        public static uint CurrentResolution = 0;
        private static bool closed = false, recording = false;
        private static string audioName;
        private static List<MMDevice> wasapis = new List<MMDevice>();
        private static WasapiOut wasapiOut;
        private static WasapiLoopbackCapture capture;
        private static WaveFileWriter writer;
        public static int[] wd = { 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2 };
        public static int[] wu = { 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2 };
        public static bool[] ws = { false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false };
        static void valchanged(int n, bool val)
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
            ws[n] = val;
        }
        private void Form21_Shown(object sender, EventArgs e)
        {
            TimeBeginPeriod(1);
            NtSetTimerResolution(1, true, ref CurrentResolution);
            var enumerator = new MMDeviceEnumerator();
            MMDevice wasapi = null;
            foreach (var mmdevice in enumerator.EnumerateAudioEndPoints(DataFlow.Render, DeviceState.Active))
            {
                wasapi = mmdevice;
                wasapis.Add(mmdevice);
                comboBox1.Items.Add(mmdevice.DeviceFriendlyName);
            }
            comboBox1.SelectedIndex = 0;
            Task.Run(() => Start());
        }
        private void Form21_KeyDown(object sender, KeyEventArgs e)
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
            if (keyData == Keys.Escape)
            {
                this.Close();
            }
        }
        public void Start()
        {
            while (!closed)
            {
                valchanged(0, GetAsyncKeyState(Keys.NumPad0));
                valchanged(1, GetAsyncKeyState(Keys.Decimal));
                if (wd[1] == 1 & !recording)
                {
                    Record();
                }
                else
                {
                    if (wd[0] == 1 & recording)
                    {
                        Stop();
                    }
                }
                Thread.Sleep(70);
            }
        }
        public void Record()
        {
            button1.Text = "Stop";
            recording = true;
            Task.Factory.StartNew(() => recordingSound());
        }
        public void Stop()
        {
            button1.Text = "Start";
            recording = false;
            Thread.Sleep(5000);
            ConvertWavMP3(audioName);
            this.WindowState = FormWindowState.Minimized;
            Task.Factory.StartNew(() => openDirectory());
            File.Delete(audioName);
        }
        private void recordingSound()
        {
            string localDate = DateTime.Now.ToString();
            audioName = localDate.Replace(" ", "-").Replace("/", "-").Replace(":", "-") + ".wav";
            if (textBox1.Text == audioName)
            {
                audioName = localDate.Replace(" ", "-").Replace("/", "-").Replace(":", "-") + ".wav";
                textBox1.Text = audioName;
            }
            else if (textBox1.Text != "")
            {
                audioName = textBox1.Text;
            }
            else
            {
                textBox1.Text = audioName;
            }
            if (comboBox1.SelectedIndex < 1)
            {
                capture = new NAudio.Wave.WasapiLoopbackCapture();
            }
            else
            {
                capture = new NAudio.Wave.WasapiLoopbackCapture(wasapis[comboBox1.SelectedIndex]);
            }
            writer = new WaveFileWriter(Path.Combine(Application.StartupPath, audioName), capture.WaveFormat);
            capture.DataAvailable += (s, a) =>
            {
                writer.Write(a.Buffer, 0, a.BytesRecorded);
                writer.Flush();
            }; 
            capture.RecordingStopped += (s, a) =>
            {
                writer.Dispose();
                writer = null;
                capture.Dispose();
                wasapiOut.Stop();
            };
            capture.StartRecording();
            var device = new MMDeviceEnumerator().GetDefaultAudioEndpoint(DataFlow.Render, Role.Multimedia);
            var silenceProvider = new SilenceProvider(capture.WaveFormat);
            wasapiOut = new WasapiOut(device, AudioClientShareMode.Shared, false, 250);
            wasapiOut.Init(silenceProvider);
            wasapiOut.Play();
            for (int count = 0; count <= 60 * 60 * 1000; count++)
            {
                if (!recording | count == 60 * 60 * 1000)
                {
                    capture.StopRecording();
                    break;
                }
                Thread.Sleep(1);
            }
        }
        private void ConvertWavMP3(string wavfile)
        {
            using (var reader = new NAudio.Wave.WaveFileReader(wavfile))
            using (var mp3Writer = new LameMP3FileWriter(wavfile.Replace(".wav", ".mp3"), reader.WaveFormat, 128))
            {
                reader.CopyTo(mp3Writer);
            }
        }
        private void openDirectory()
        {
            ProcessStartInfo startInfo = new ProcessStartInfo
            {
                Arguments = AppContext.BaseDirectory,
                FileName = "explorer.exe"
            };
            Process.Start(startInfo);
        }
        private void Form21_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (recording)
            {
                Stop();
            }
            closed = true;
        }
        private void button1_Click(object sender, EventArgs e)
        {
            if (!recording)
            {
                Record();
            }
            else
            {
                Stop();
            }
        }
    }
}