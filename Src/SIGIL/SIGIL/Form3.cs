using System;
using System.Windows.Forms;
using InterceptionTest;

namespace SIGIL
{
    public partial class Form3 : Form
    {
        public Form3()
        {
            InitializeComponent();
        }
        Input input = new Input();
        private void Form3_Load(object sender, EventArgs e)
        {
            input.KeyboardFilterMode = KeyboardFilterMode.All;
            input.MouseFilterMode = MouseFilterMode.All;
            input.Load();
        }
        private void Form3_FormClosed(object sender, FormClosedEventArgs e)
        {
            input.Unload();
        }
        private void Form3_KeyDown(object sender, KeyEventArgs e)
        {
            OnKeyDown(e.KeyData);
        }
        private void textBox1_KeyDown(object sender, KeyEventArgs e)
        {
            OnKeyDown(e.KeyData);
        }
        private void textBox3_KeyDown(object sender, KeyEventArgs e)
        {
            OnKeyDown(e.KeyData);
        }
        private void textBox2_KeyDown(object sender, KeyEventArgs e)
        {
            OnKeyDown(e.KeyData);
        }
        private void textBox4_KeyDown(object sender, KeyEventArgs e)
        {
            OnKeyDown(e.KeyData);
        }
        private void OnKeyDown(System.Windows.Forms.Keys keyData)
        {
            if (keyData == System.Windows.Forms.Keys.F1)
            {
                const string message = "• Author: Michaël André Franiatte.\n\r\n\r• Contact: michael.franiatte@gmail.com.\n\r\n\r• Publisher: https://github.com/michaelandrefraniatte.\n\r\n\r• Copyrights: All rights reserved, no permissions granted.\n\r\n\r• License: Not open source, not free of charge to use.";
                const string caption = "About";
                MessageBox.Show(message, caption, MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            if (keyData == System.Windows.Forms.Keys.Escape)
            {
                this.Close();
            }
        }
    }
}