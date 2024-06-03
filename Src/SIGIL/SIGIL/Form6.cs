using System;
using WebSocketSharp;
using System.IO;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using NAudio.CoreAudioApi;
using NAudio.Wave;
using SharpDX.Direct2D1;
using System.Drawing;
using SharpDX;
using SharpDX.DXGI;
using SharpDX.WIC;

namespace SIGIL
{
    public partial class Form6 : Form
    {
        public Form6()
        {
            InitializeComponent();
        }
        [DllImport("winmm.dll", EntryPoint = "timeBeginPeriod")]
        public static extern uint TimeBeginPeriod(uint ms);
        [DllImport("winmm.dll", EntryPoint = "timeEndPeriod")]
        public static extern uint TimeEndPeriod(uint ms);
        [DllImport("ntdll.dll", EntryPoint = "NtSetTimerResolution")]
        public static extern void NtSetTimerResolution(uint DesiredResolution, bool SetResolution, ref uint CurrentResolution);
        public static uint CurrentResolution = 0;
        private string ip, displayport, audioport;
        private WebSocket wsc1display;
        private static int width, height;
        private System.Drawing.Bitmap texture1 = null, texturetemp = null;
        private WebSocket wscaudio;
        private BufferedWaveProvider src;
        private WasapiOut soundOut;
        private bool closed = false;
        private static WindowRenderTarget target;
        private static SharpDX.Direct2D1.Factory1 fact = new SharpDX.Direct2D1.Factory1();
        private static RenderTargetProperties renderProp;
        private static HwndRenderTargetProperties winProp;
        private static int imgheight, imgwidth;
        private void Form6_Shown(object sender, EventArgs e)
        {
            TimeBeginPeriod(1);
            NtSetTimerResolution(1, true, ref CurrentResolution);
            using (StreamReader file = new StreamReader(Application.StartupPath + @"\params.txt"))
            {
                file.ReadLine();
                file.ReadLine();
                file.ReadLine();
                file.ReadLine();
                file.ReadLine();
                file.ReadLine();
                file.ReadLine();
                file.ReadLine();
                file.ReadLine();
                ip = file.ReadLine();
                file.ReadLine();
                displayport = file.ReadLine();
                file.ReadLine();
                audioport = file.ReadLine();
            }
            width = Screen.PrimaryScreen.Bounds.Width;
            height = Screen.PrimaryScreen.Bounds.Height;
            Resizing();
            imgwidth = width;
            imgheight = height;
            pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
            InitDisplayCapture(this.pictureBox1.Handle);
            Connect1Display();
            ConnectAudio();
        }
        private void Form6_FormClosed(object sender, FormClosedEventArgs e)
        {
            Disconnect1Display();
            DisconnectAudio();
        }
        private void Form6_KeyDown(object sender, KeyEventArgs e)
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
        public void Connect1Display()
        {
            String connectionString = "ws://" + ip + ":" + displayport + "/1Display";
            wsc1display = new WebSocket(connectionString);
            wsc1display.OnMessage += Ws_OnMessage1Display;
            while (!wsc1display.IsAlive & !closed)
            {
                try
                {
                    wsc1display.Connect();
                    wsc1display.Send("Hello from client");
                }
                catch { }
                System.Threading.Thread.Sleep(1);
            }
        }
        private void Ws_OnMessage1Display(object sender, MessageEventArgs e)
        {
            try
            {
                texture1 = byteArrayToTexture(e.RawData);
                if (texture1 != null)
                {
                    texturetemp = texture1;
                }
                DisplayCapture(texturetemp);
            }
            catch { }
        }
        public void Disconnect1Display()
        {
            closed = true;
            wsc1display.Close();
        }
        private System.Drawing.Bitmap byteArrayToTexture(byte[] imageBytes)
        {
            try
            {
                if (imageBytes.Length > 300)
                {
                    using (MemoryStream stream = new MemoryStream(imageBytes))
                    {
                        System.Drawing.Bitmap bitmap = System.Drawing.Image.FromStream(stream) as System.Drawing.Bitmap;
                        bitmap = new System.Drawing.Bitmap(bitmap, new Size(width, height));
                        return bitmap;
                    }
                }
                else
                    return null;
            }
            catch
            {
                return null;
            }
        }
        public void ConnectAudio()
        {
            String connectionString = "ws://" + ip + ":" + audioport + "/Audio";
            wscaudio = new WebSocket(connectionString);
            wscaudio.OnMessage += Ws_OnMessageAudio;
            while (!wscaudio.IsAlive & !closed)
            {
                try
                {
                    wscaudio.Connect();
                    wscaudio.Send("Hello from client");
                }
                catch { }
                System.Threading.Thread.Sleep(1);
            }
            var enumerator = new MMDeviceEnumerator();
            MMDevice wasapi = null;
            foreach (var mmdevice in enumerator.EnumerateAudioEndPoints(DataFlow.Render, NAudio.CoreAudioApi.DeviceState.Active))
            {
                wasapi = mmdevice;
                break;
            }
            WaveFormat waveformat = (new WasapiLoopbackCapture()).WaveFormat;
            soundOut = new WasapiOut(wasapi, AudioClientShareMode.Shared, false, 2);
            src = new BufferedWaveProvider(WaveFormat.CreateCustomFormat(waveformat.Encoding, waveformat.SampleRate, waveformat.Channels, waveformat.AverageBytesPerSecond, waveformat.BlockAlign, waveformat.BitsPerSample));
            src.DiscardOnBufferOverflow = true;
            src.BufferDuration = TimeSpan.FromMilliseconds(80);
            src.BufferLength = waveformat.AverageBytesPerSecond * 80 / 1000;
            soundOut.Init(src);
            soundOut.Play();
        }
        private void Ws_OnMessageAudio(object sender, MessageEventArgs e)
        {
            try
            {
                src.AddSamples(e.RawData, 0, e.RawData.Length);
            }
            catch { }
        }
        public void DisconnectAudio()
        {
            closed = true;
            wscaudio.Close();
            soundOut.Stop();
        }
        private static void InitDisplayCapture(IntPtr handle)
        {
            renderProp = new RenderTargetProperties()
            {
                DpiX = 0,
                DpiY = 0,
                MinLevel = SharpDX.Direct2D1.FeatureLevel.Level_DEFAULT,
                PixelFormat = new SharpDX.Direct2D1.PixelFormat(Format.B8G8R8A8_UNorm, SharpDX.Direct2D1.AlphaMode.Premultiplied),
                Type = RenderTargetType.Hardware,
                Usage = RenderTargetUsage.None
            };
            winProp = new HwndRenderTargetProperties()
            {
                Hwnd = handle,
                PixelSize = new Size2(imgwidth, imgheight),
                PresentOptions = PresentOptions.Immediately
            };
            target = new WindowRenderTarget(fact, renderProp, winProp);
        }
        private static void DisplayCapture(System.Drawing.Bitmap bmp)
        {
            System.Drawing.Imaging.BitmapData bmpData = bmp.LockBits(new System.Drawing.Rectangle(0, 0, bmp.Width, bmp.Height), System.Drawing.Imaging.ImageLockMode.ReadOnly, System.Drawing.Imaging.PixelFormat.Format32bppPArgb);
            SharpDX.DataStream stream = new SharpDX.DataStream(bmpData.Scan0, bmpData.Stride * bmpData.Height, true, false);
            SharpDX.Direct2D1.PixelFormat pFormat = new SharpDX.Direct2D1.PixelFormat(SharpDX.DXGI.Format.B8G8R8A8_UNorm, SharpDX.Direct2D1.AlphaMode.Premultiplied);
            SharpDX.Direct2D1.BitmapProperties bmpProps = new SharpDX.Direct2D1.BitmapProperties(pFormat);
            SharpDX.Direct2D1.Bitmap result = new SharpDX.Direct2D1.Bitmap(target, new SharpDX.Size2(imgwidth, imgheight), stream, bmpData.Stride, bmpProps);
            bmp.UnlockBits(bmpData);
            stream.Dispose();
            bmp.Dispose();
            target.BeginDraw();
            target.Clear(new SharpDX.Mathematics.Interop.RawColor4(0, 0, 0, 1f));
            target.DrawBitmap(result, 1.0f, SharpDX.Direct2D1.BitmapInterpolationMode.NearestNeighbor);
            target.EndDraw();
        }
        private void Form6_SizeChanged(object sender, EventArgs e)
        {
            Resizing();
        }
        private void Resizing()
        {
            this.Location = new System.Drawing.Point(0, 0);
            this.Size = new System.Drawing.Size(width, height);
        }
    }
}