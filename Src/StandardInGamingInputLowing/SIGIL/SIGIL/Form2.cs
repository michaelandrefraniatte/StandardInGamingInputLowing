using System;
using System.Drawing;
using System.Windows.Forms;

namespace SIGIL
{
    public partial class Form2 : Form
    {
        private static int width = Screen.PrimaryScreen.Bounds.Width, height = Screen.PrimaryScreen.Bounds.Height;
        public static Form1 form1 = new Form1(null);
        public Form2()
        {
            InitializeComponent();
        }
        private void Form2_Load(object sender, EventArgs e)
        {
            Resizing();
        }
        private void Form2_SizeChanged(object sender, EventArgs e)
        {
            Resizing();
        }
        private void Resizing()
        {
            this.Location = new Point(0, 0);
            this.Size = new System.Drawing.Size(width, height);
        }
        private void Form2_FormClosing(object sender, FormClosingEventArgs e)
        {
            Form1.form2visible = false;
            form1.SetShowATransparentClickableOverlayToolStripMenuItem();
            this.Hide();
            e.Cancel = true;
        }
    }
}