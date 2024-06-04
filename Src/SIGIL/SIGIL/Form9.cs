using System;
using System.Drawing;
using System.Windows.Forms;
using Microsoft.Web.WebView2.Core;
using WebView2 = Microsoft.Web.WebView2.WinForms.WebView2;

namespace SIGIL
{
    public partial class Form9 : Form
    {
        public Form9()
        {
            InitializeComponent();
        }
        public WebView2 webView21 = new WebView2();
        private static int width = Screen.PrimaryScreen.Bounds.Width;
        private static int height = Screen.PrimaryScreen.Bounds.Height;
        private static int picwidth = Screen.PrimaryScreen.Bounds.Width;
        private static int picheight = Screen.PrimaryScreen.Bounds.Height;
        private void Form9_Load(object sender, EventArgs e)
        {
        }
        private async void Form9_Shown(object sender, EventArgs e)
        {
            picwidth = 128;
            picheight = 128;
            this.Size = new Size(picwidth, picheight);
            this.Location = new Point(width / 2 - picwidth / 2, height / 2 - picheight / 2);
            this.pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
            CoreWebView2EnvironmentOptions options = new CoreWebView2EnvironmentOptions("--disable-web-security --allow-file-access-from-files --allow-file-access", "en");
            CoreWebView2Environment environment = await CoreWebView2Environment.CreateAsync(null, null, options);
            await webView21.EnsureCoreWebView2Async(environment);
            webView21.CoreWebView2.SetVirtualHostNameToFolderMapping("appassets", "assets", CoreWebView2HostResourceAccessKind.DenyCors);
            webView21.CoreWebView2.Settings.AreDevToolsEnabled = false;
            webView21.Source = new Uri("https://appassets/crossair/index.html");
            webView21.Dock = DockStyle.Fill;
            webView21.DefaultBackgroundColor = Color.Transparent;
            webView21.KeyDown += WebView21_KeyDown;
            this.pictureBox1.Controls.Add(webView21);
        }
        private void Form9_KeyDown(object sender, KeyEventArgs e)
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
        private void Form9_FormClosing(object sender, FormClosingEventArgs e)
        {
        }
    }
}