using Microsoft.Web.WebView2.Core;
using System;
using System.IO;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using WebView2 = Microsoft.Web.WebView2.WinForms.WebView2;
using System.Drawing;
using System.Threading;
using System.Threading.Tasks;
using System.Drawing.Imaging;
using System.Linq;
using System.Collections.Generic;
using System.Diagnostics;

namespace SIGIL
{
    public partial class Form15 : Form
    {
        public Form15()
        {
            InitializeComponent();
        }
        [DllImport("User32", EntryPoint = "FindWindow")]
        private static extern IntPtr FindWindow(string lpClassName, string lpWindowName);
        [DllImport("user32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool PrintWindow(IntPtr hwnd, IntPtr hDC, uint nFlags);
        [DllImport("user32.dll", SetLastError = true)]
        static extern int GetWindowRgn(IntPtr hWnd, IntPtr hRgn);
        [DllImport("gdi32.dll")]
        static extern IntPtr CreateRectRgn(int nLeftRect, int nTopRect, int nRightRect, int nBottomRect);
        [DllImport("user32.dll", SetLastError = true)]
        static extern bool GetWindowRect(IntPtr hwnd, out Rectangle lpRect);
        [DllImport("User32.dll")]
        private static extern bool GetCursorPos(out int x, out int y);
        [DllImport("winmm.dll", EntryPoint = "timeBeginPeriod")]
        private static extern uint TimeBeginPeriod(uint ms);
        [DllImport("winmm.dll", EntryPoint = "timeEndPeriod")]
        private static extern uint TimeEndPeriod(uint ms);
        [DllImport("ntdll.dll", EntryPoint = "NtSetTimerResolution")]
        private static extern void NtSetTimerResolution(uint DesiredResolution, bool SetResolution, ref uint CurrentResolution);
        private static uint CurrentResolution = 0;
        private WebView2 webView21 = new WebView2();
        private static int mousex, mousey;
        private static int width = Screen.PrimaryScreen.Bounds.Width, height = Screen.PrimaryScreen.Bounds.Height;
        private static string windowtitle, base64image;
        private static IntPtr findwindow;
        private static uint PW_CLIENTONLY = 0x1;
        private static uint PW_RENDERFULLCONTENT = 0x2;
        private static uint flags = PW_CLIENTONLY | PW_RENDERFULLCONTENT;
        private Rectangle rc;
        private Bitmap bmp;
        private Graphics gfxBmp;
        private IntPtr hdcBitmap;
        private Bitmap bitmap;
        private ImageCodecInfo jpegEncoder;
        private EncoderParameters encoderParameters;
        private void Form15_Load(object sender, EventArgs e)
        {
            TimeBeginPeriod(1);
            NtSetTimerResolution(1, true, ref CurrentResolution);
        }
        private async void Form15_Shown(object sender, EventArgs e)
        {
            this.Size = new Size(width, height);
            this.Location = new System.Drawing.Point(0, 0);
            CoreWebView2EnvironmentOptions options = new CoreWebView2EnvironmentOptions("--disable-web-security --allow-file-access-from-files --allow-file-access", "en");
            CoreWebView2Environment environment = await CoreWebView2Environment.CreateAsync(null, null, options);
            await webView21.EnsureCoreWebView2Async(environment);
            webView21.CoreWebView2.SetVirtualHostNameToFolderMapping("appassets", "assets", CoreWebView2HostResourceAccessKind.DenyCors);
            webView21.CoreWebView2.Settings.AreDevToolsEnabled = false;
            webView21.KeyDown += WebView21_KeyDown;
            webView21.Source = new Uri("https://appassets/resizedvision/index.html");
            webView21.Dock = DockStyle.Fill;
            webView21.DefaultBackgroundColor = System.Drawing.Color.Transparent;
            this.Controls.Add(webView21);
            using (System.IO.StreamReader file = new System.IO.StreamReader(Application.StartupPath + @"\tempresized"))
            {
                file.ReadLine();
                windowtitle = file.ReadLine();
            }
            List<string> listrecords = new List<string>();
            listrecords = GetWindowTitles();
            string record = windowtitle;
            windowtitle = await PromptHandle.ShowDialog("Window Titles", "What should be the window to handle capture?", record, listrecords);
            jpegEncoder = ImageCodecInfo.GetImageDecoders().First(c => c.FormatID == ImageFormat.Jpeg.Guid);
            encoderParameters = new EncoderParameters(1);
            encoderParameters.Param[0] = new EncoderParameter(System.Drawing.Imaging.Encoder.Compression, 255);
            findwindow = FindWindow(null, windowtitle);
            GetWindowRect(findwindow, out rc);
            bmp = new Bitmap(rc.Width, rc.Height, PixelFormat.Format32bppArgb);
            gfxBmp = Graphics.FromImage(bmp);
        }
        public List<string> GetWindowTitles()
        {
            List<string> titles = new List<string>();
            foreach (Process proc in Process.GetProcesses())
            {
                string title = proc.MainWindowTitle;
                if (title != null & title != "")
                    titles.Add(title);
            }
            return titles;
        }
        private void Form15_KeyDown(object sender, KeyEventArgs e)
        {
            OnKeyDown(e.KeyData);
        }
        protected override bool ProcessCmdKey(ref System.Windows.Forms.Message msg, Keys keyData)
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
        private async void timer1_Tick(object sender, EventArgs e)
        {
            try
            {
                GetCursorPos(out mousex, out mousey);
                bitmap = PrintWindow(findwindow);
                if (mousex < 150)
                {
                    mousex = 150;
                }
                if (mousex > width - 150)
                {
                    mousex = width - 150;
                }
                if (mousey < 150)
                {
                    mousey = 150;
                }
                if (mousey > height - 150)
                {
                    mousey = height - 150;
                }
                Rectangle srcRect = new Rectangle(mousex - 150, mousey - 150, 300, 300);
                bitmap = (Bitmap)bitmap.Clone(srcRect, bitmap.PixelFormat);
                bitmap = new Bitmap(bitmap, new Size(bitmap.Width * 4, bitmap.Height * 4));
                byte[] imageArray = ImageToByteArray(bitmap);
                base64image = Convert.ToBase64String(imageArray);
                await execScriptHelper($"setMouse('{mousex.ToString()}', '{mousey.ToString()}', '{width.ToString()}', '{height.ToString()}', '{base64image.ToString()}');");
            }
            catch { }
        }
        public byte[] ImageToByteArray(Bitmap image)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                image.Save(ms, jpegEncoder, encoderParameters);
                return ms.ToArray();
            }
        }
        public Bitmap PrintWindow(IntPtr hwnd)
        {
            hdcBitmap = gfxBmp.GetHdc();
            PrintWindow(hwnd, hdcBitmap, PW_CLIENTONLY | PW_RENDERFULLCONTENT);
            gfxBmp.ReleaseHdc(hdcBitmap);
            return bmp;
        }
        private async Task<String> execScriptHelper(String script)
        {
            var x = await webView21.ExecuteScriptAsync(script).ConfigureAwait(false);
            return x;
        }
        private void Form15_FormClosed(object sender, FormClosedEventArgs e)
        {
        }
        private void Form15_FormClosing(object sender, FormClosingEventArgs e)
        {
            Thread.Sleep(100);
            webView21.Dispose();
            using (System.IO.StreamWriter file = new System.IO.StreamWriter(Application.StartupPath + @"\tempresized"))
            {
                file.WriteLine("// Window title");
                file.WriteLine(windowtitle);
            }
        }
    }
}