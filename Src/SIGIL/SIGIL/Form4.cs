using System;
using System.Drawing;
using System.Windows.Forms;
using AForge.Video;
using AForge.Video.DirectShow;
using System.Runtime.InteropServices;
using Bitmap = System.Drawing.Bitmap;
using Point = System.Drawing.Point;
using System.Drawing.Drawing2D;
using WebView2 = Microsoft.Web.WebView2.WinForms.WebView2;
using Microsoft.Web.WebView2.Core;
using System.Threading.Tasks;

namespace SIGIL
{
    public partial class Form4 : Form
    {
        public Form4()
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
        public static Bitmap img;
        private FilterInfoCollection CaptureDevice;
        private VideoCaptureDevice FinalFrame;
        private VideoCapabilities[] videoCapabilities;
        private static int height = 300, width = 300, border = 0, d = 66;
        private static double initratio, ratio = 1f;
        private GraphicsPath gp;
        private Rectangle rectangle;
        private Bitmap image, shadowrounded, shadowcircle;
        private static bool getstateminus, getstateplus;
        private WebView2 webView21 = new WebView2();
        private static int[] wd = { 2, 2 };
        private static int[] wu = { 2, 2 };
        public static void valchanged(int n, bool val)
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
        [Obsolete]
        private async void Form4_Shown(object sender, EventArgs e)
        {
            try
            {
                TimeBeginPeriod(1);
                NtSetTimerResolution(1, true, ref CurrentResolution);
                AppDomain.CurrentDomain.UnhandledException += new System.UnhandledExceptionEventHandler(AppDomain_UnhandledException);
                System.Windows.Forms.Application.ThreadException += new System.Threading.ThreadExceptionEventHandler(Application_ThreadException);
                this.TopMost = true;
                CaptureDevice = new FilterInfoCollection(FilterCategory.VideoInputDevice);
                FinalFrame = new VideoCaptureDevice(CaptureDevice[0].MonikerString);
                FinalFrame.DesiredFrameRate = 10;
                FinalFrame.DesiredFrameSize = new Size(300, 300);
                videoCapabilities = FinalFrame.VideoCapabilities;
                FinalFrame.VideoResolution = videoCapabilities[1];
                initratio = Convert.ToDouble(FinalFrame.VideoResolution.FrameSize.Width) / Convert.ToDouble(FinalFrame.VideoResolution.FrameSize.Height);
                width = (int)(height * ratio);
                this.Size = new Size(width, height);
                this.ClientSize = new Size(width, height);
                this.Location = new Point(Screen.PrimaryScreen.Bounds.Width - width - border, border);
                this.pictureBox1.Size = new Size(width - 30, height - 30);
                this.pictureBox1.Location = new Point(15, 15);
                this.pictureBox2.Size = new Size(width - 10, height - 10);
                this.pictureBox2.Location = new Point(5, 5);
                FinalFrame.DesiredFrameSize = new Size((int)(height * ratio), height);
                FinalFrame.SetCameraProperty(CameraControlProperty.Zoom, 0, CameraControlFlags.Manual);
                FinalFrame.SetCameraProperty(CameraControlProperty.Focus, 0, CameraControlFlags.Manual);
                FinalFrame.SetCameraProperty(CameraControlProperty.Exposure, 0, CameraControlFlags.Manual);
                FinalFrame.SetCameraProperty(CameraControlProperty.Iris, 0, CameraControlFlags.Manual);
                FinalFrame.SetCameraProperty(CameraControlProperty.Pan, 0, CameraControlFlags.Manual);
                FinalFrame.SetCameraProperty(CameraControlProperty.Tilt, 0, CameraControlFlags.Manual);
                FinalFrame.SetCameraProperty(CameraControlProperty.Roll, 0, CameraControlFlags.Manual);
                FinalFrame.NewFrame += FinalFrame_NewFrame;
                FinalFrame.Start();
                pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
                rectangle = new Rectangle(0, 0, pictureBox1.Width, pictureBox1.Height);
                gp = new GraphicsPath();
                gp.AddArc(rectangle.X, rectangle.Y, d, d, 180, 90);
                gp.AddArc(rectangle.X + rectangle.Width - d, rectangle.Y, d, d, 270, 90);
                gp.AddArc(rectangle.X + rectangle.Width - d, rectangle.Y + rectangle.Height - d, d, d, 0, 90);
                gp.AddArc(rectangle.X, rectangle.Y + rectangle.Height - d, d, d, 90, 90);
                pictureBox1.Region = new Region(gp);
                rectangle = new Rectangle(0, 0, pictureBox2.Width, pictureBox2.Height);
                gp = new GraphicsPath();
                gp.AddArc(rectangle.X, rectangle.Y, d, d, 180, 90);
                gp.AddArc(rectangle.X + rectangle.Width - d, rectangle.Y, d, d, 270, 90);
                gp.AddArc(rectangle.X + rectangle.Width - d, rectangle.Y + rectangle.Height - d, d, d, 0, 90);
                gp.AddArc(rectangle.X, rectangle.Y + rectangle.Height - d, d, d, 90, 90);
                pictureBox2.Region = new Region(gp);
                shadowrounded = new Bitmap(Application.StartupPath + @"\shadowrounded.gif");
                shadowcircle = new Bitmap(Application.StartupPath + @"\shadowcircle.gif");
                this.pictureBox2.SizeMode = PictureBoxSizeMode.StretchImage;
                this.pictureBox2.Image = shadowrounded;
                CoreWebView2EnvironmentOptions options = new CoreWebView2EnvironmentOptions("--disable-web-security --allow-file-access-from-files --allow-file-access", "en");
                CoreWebView2Environment environment = await CoreWebView2Environment.CreateAsync(null, null, options);
                await webView21.EnsureCoreWebView2Async(environment);
                webView21.CoreWebView2.SetVirtualHostNameToFolderMapping("appassets", "assets", CoreWebView2HostResourceAccessKind.DenyCors);
                webView21.CoreWebView2.Settings.AreDevToolsEnabled = false;
                webView21.KeyDown += WebView21_KeyDown;
                webView21.Source = new Uri("https://appassets/webcam/index.html");
                webView21.Dock = DockStyle.Fill;
                webView21.DefaultBackgroundColor = Color.Transparent;
                this.Controls.Add(webView21);
            }
            catch
            {
                this.Close();
            }
        }
        private async void timer1_Tick(object sender, EventArgs e)
        {
            valchanged(0, GetAsyncKeyState(Keys.Subtract));
            if (wu[0] == 1 & !getstateminus)
            {
                getstateminus = true;
                this.Opacity = 1;
            }
            else if (wu[0] == 1 & getstateminus)
            {
                getstateminus = false;
                this.Opacity = 0.75D;
            }
            valchanged(1, GetAsyncKeyState(Keys.Add));
            if (wu[1] == 1 & !getstateplus)
            {
                getstateplus = true;
                gp = new GraphicsPath();
                gp.AddEllipse(pictureBox1.DisplayRectangle);
                pictureBox1.Region = new Region(gp);
                gp = new GraphicsPath();
                gp.AddEllipse(pictureBox2.DisplayRectangle);
                pictureBox2.Region = new Region(gp);
                this.pictureBox2.Image = shadowcircle;
                this.Controls.Remove(webView21);
                this.Controls.Add(webView21);
                await execScriptHelper("setShadowCircle()");
            }
            else if (wu[1] == 1 & getstateplus)
            {
                getstateplus = false;
                rectangle = new Rectangle(0, 0, pictureBox1.Width, pictureBox1.Height);
                gp = new GraphicsPath();
                gp.AddArc(rectangle.X, rectangle.Y, d, d, 180, 90);
                gp.AddArc(rectangle.X + rectangle.Width - d, rectangle.Y, d, d, 270, 90);
                gp.AddArc(rectangle.X + rectangle.Width - d, rectangle.Y + rectangle.Height - d, d, d, 0, 90);
                gp.AddArc(rectangle.X, rectangle.Y + rectangle.Height - d, d, d, 90, 90);
                pictureBox1.Region = new Region(gp);
                rectangle = new Rectangle(0, 0, pictureBox2.Width, pictureBox2.Height);
                gp = new GraphicsPath();
                gp.AddArc(rectangle.X, rectangle.Y, d, d, 180, 90);
                gp.AddArc(rectangle.X + rectangle.Width - d, rectangle.Y, d, d, 270, 90);
                gp.AddArc(rectangle.X + rectangle.Width - d, rectangle.Y + rectangle.Height - d, d, d, 0, 90);
                gp.AddArc(rectangle.X, rectangle.Y + rectangle.Height - d, d, d, 90, 90);
                pictureBox2.Region = new Region(gp);
                this.pictureBox2.Image = shadowrounded;
                this.Controls.Remove(webView21);
                this.Controls.Add(webView21);
                await execScriptHelper("setShadowRounded()");
            }
        }
        private async Task<String> execScriptHelper(String script)
        {
            var x = await webView21.ExecuteScriptAsync(script).ConfigureAwait(false);
            return x;
        }
        private void WebView21_KeyDown(object sender, KeyEventArgs e)
        {
            OnKeyDown(e.KeyData);
        }
        private void Form4_KeyDown(object sender, KeyEventArgs e)
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
        public void AppDomain_UnhandledException(object sender, System.UnhandledExceptionEventArgs e)
        {
            FormClose();
        }
        public void Application_ThreadException(object sender, System.Threading.ThreadExceptionEventArgs e)
        {
            FormClose();
        }
        private void Form4_Activated(object sender, EventArgs e)
        {
            if (this.FormBorderStyle == FormBorderStyle.FixedToolWindow)
                return;
            this.FormBorderStyle = FormBorderStyle.FixedToolWindow;
        }
        private void Form4_Deactivate(object sender, EventArgs e)
        {
            this.FormBorderStyle = FormBorderStyle.None;
        }
        private void FinalFrame_NewFrame(object sender, NewFrameEventArgs eventArgs)
        {
            try
            {
                Bitmap capture = eventArgs.Frame.Clone() as Bitmap;
                this.pictureBox1.Image = cropImage(capture);
                capture.Dispose();
            }
            catch { }
        }
        private Bitmap cropImage(Bitmap bmp)
        {
            int oldwidth = (int)(initratio * bmp.Height);
            int oldheight = bmp.Height;
            int newWidth = oldheight;
            int newHeight = oldheight;
            Rectangle CropArea = new Rectangle((oldwidth - oldheight) / 2, 0, newWidth, newHeight);
            Bitmap bmpCrop = bmp.Clone(CropArea, bmp.PixelFormat);
            return bmpCrop;
        }
        private void Form4_FormClosed(object sender, FormClosedEventArgs e)
        {
            FormClose();
        }
        private void FormClose()
        {
            try
            {
                FinalFrame.NewFrame -= FinalFrame_NewFrame;
                if (FinalFrame.IsRunning)
                    FinalFrame.Stop();
            }
            catch { }
        }
    }
}