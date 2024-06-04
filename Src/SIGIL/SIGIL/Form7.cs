using System;
using System.Windows.Forms;
using System.Threading;
using System.Threading.Tasks;
using System.Runtime.InteropServices;

namespace SIGIL
{
    public partial class Form7 : Form
    {
        public Form7()
        {
            InitializeComponent();
        }
        [DllImport("user32.dll")]
        public static extern bool GetAsyncKeyState(Keys vKey);
        [DllImport("winmm.dll", EntryPoint = "timeBeginPeriod")]
        public static extern uint TimeBeginPeriod(uint ms);
        [DllImport("winmm.dll", EntryPoint = "timeEndPeriod")]
        public static extern uint TimeEndPeriod(uint ms);
        [DllImport("ntdll.dll", EntryPoint = "NtSetTimerResolution")]
        public static extern void NtSetTimerResolution(uint DesiredResolution, bool SetResolution, ref uint CurrentResolution);
        [DllImport("user32.dll")]
        public static extern void SetPhysicalCursorPos(int X, int Y);
        [DllImport("user32.dll")]
        public static extern void SetCaretPos(int X, int Y);
        [DllImport("user32.dll")]
        public static extern void SetCursorPos(int X, int Y);
        [DllImport("SendInputLibrary.dll", EntryPoint = "MouseMW3", CallingConvention = CallingConvention.Cdecl)]
        public static extern void MouseMW3(int x, int y);
        [DllImport("SendInputLibrary.dll", EntryPoint = "MouseBrink", CallingConvention = CallingConvention.Cdecl)]
        public static extern void MouseBrink(int x, int y);
        public static bool[] _Valuechanged = new bool[36], _valuechanged = new bool[36];
        public static uint hDevInfo, CurrentResolution = 0;
        private static bool endinvoke;
        public static double WidthS, HeightS, mousex, mousey, mousexp, mouseyp, slowing, slowinglimit = 20, dividing = 1.5f, adding = 0.15f;
        public static bool up, down, left, right;
        public bool this[int i]
        {
            get { return _valuechanged[i]; }
            set
            {
                if (_valuechanged[i] != value)
                    _Valuechanged[i] = true;
                else
                    _Valuechanged[i] = false;
                _valuechanged[i] = value;
            }
        }
        private void Form7_KeyDown(object sender, KeyEventArgs e)
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
        public void Selection()
        {
            for (; ; )
            {
                if (endinvoke)
                    return;
                this[0] = GetAsyncKeyState(Keys.Decimal);
                if (_Valuechanged[0] & this[0])
                {
                    chbcursor.Checked = false;
                    chbbrink.Checked = false;
                    chbtitanfall.Checked = false;
                    chbbo3.Checked = false;
                    chbfake.Checked = false;
                    chbmetro.Checked = false;
                    chbmw3.Checked = false;
                    chbxaim.Checked = false;
                    chbwarface.Checked = false;
                    chbfortnite.Checked = false;
                    chbmc.Checked = false;
                }
                this[1] = GetAsyncKeyState(Keys.F1);
                if (_Valuechanged[1] & this[1])
                {
                    if (!chbcursor.Checked)
                        chbcursor.Checked = true;
                    else
                        chbcursor.Checked = false;
                }
                this[2] = GetAsyncKeyState(Keys.F2);
                if (_Valuechanged[2] & this[2])
                {
                    if (!chbbrink.Checked)
                        chbbrink.Checked = true;
                    else
                        chbbrink.Checked = false;
                }
                this[3] = GetAsyncKeyState(Keys.F3);
                if (_Valuechanged[3] & this[3])
                {
                    if (!chbtitanfall.Checked)
                        chbtitanfall.Checked = true;
                    else
                        chbtitanfall.Checked = false;
                }
                this[4] = GetAsyncKeyState(Keys.F4);
                if (_Valuechanged[4] & this[4])
                {
                    if (!chbbo3.Checked)
                        chbbo3.Checked = true;
                    else
                        chbbo3.Checked = false;
                }
                this[5] = GetAsyncKeyState(Keys.F5);
                if (_Valuechanged[5] & this[5])
                {
                    if (!chbfake.Checked)
                        chbfake.Checked = true;
                    else
                        chbfake.Checked = false;
                }
                this[6] = GetAsyncKeyState(Keys.F6);
                if (_Valuechanged[6] & this[6])
                {
                    if (!chbmetro.Checked)
                        chbmetro.Checked = true;
                    else
                        chbmetro.Checked = false;
                }
                this[7] = GetAsyncKeyState(Keys.F7);
                if (_Valuechanged[7] & this[7])
                {
                    if (!chbmw3.Checked)
                        chbmw3.Checked = true;
                    else
                        chbmw3.Checked = false;
                }
                this[8] = GetAsyncKeyState(Keys.F8);
                if (_Valuechanged[8] & this[8])
                {
                    if (!chbxaim.Checked)
                        chbxaim.Checked = true;
                    else
                        chbxaim.Checked = false;
                }
                this[9] = GetAsyncKeyState(Keys.F9);
                if (_Valuechanged[9] & this[9])
                {
                    if (!chbwarface.Checked)
                        chbwarface.Checked = true;
                    else
                        chbwarface.Checked = false;
                }
                this[10] = GetAsyncKeyState(Keys.F10);
                if (_Valuechanged[10] & this[10])
                {
                    if (!chbfortnite.Checked)
                        chbfortnite.Checked = true;
                    else
                        chbfortnite.Checked = false;
                }
                this[11] = GetAsyncKeyState(Keys.F11);
                if (_Valuechanged[11] & this[11])
                {
                    if (!chbmc.Checked)
                        chbmc.Checked = true;
                    else
                        chbmc.Checked = false;
                }
                right = GetAsyncKeyState(Keys.Right);
                left = GetAsyncKeyState(Keys.Left);
                up = GetAsyncKeyState(Keys.Up);
                down = GetAsyncKeyState(Keys.Down);
                if (mousex > 1200)
                    mousex = 1200;
                if (mousex < -1200)
                    mousex = -1200;
                if (mousey > 1200)
                    mousey = 1200;
                if (mousey < -1200)
                    mousey = -1200;
                if (GetAsyncKeyState(Keys.NumPad0))
                {
                    mousex = 0;
                    mousey = 0;
                }
                if (GetAsyncKeyState(Keys.NumPad1))
                    slowinglimit--;
                if (GetAsyncKeyState(Keys.NumPad2))
                    slowinglimit++;
                if (slowinglimit < 1)
                    slowinglimit = 0;
                if (slowinglimit >= 100)
                    slowinglimit = 100;
                if (GetAsyncKeyState(Keys.NumPad4))
                    dividing -= 0.5;
                if (GetAsyncKeyState(Keys.NumPad5))
                    dividing += 0.5;
                if (dividing < 1)
                    dividing = 0.5;
                if (dividing >= 50)
                    dividing = 50;
                if (GetAsyncKeyState(Keys.NumPad7))
                    adding -= 0.05;
                if (GetAsyncKeyState(Keys.NumPad8))
                    adding += 0.05;
                if (adding < 0.05)
                    adding = 0;
                if (adding >= 5)
                    adding = 5;
                textBox1.Text = slowinglimit.ToString();
                textBox2.Text = dividing.ToString();
                textBox3.Text = adding.ToString();
                Thread.Sleep(100);
            }
        }
        public void Mouse()
        {
            for (; ; )
            {
                if (endinvoke)
                    return;
                if (right)
                    mousex -= 1;
                if (left)
                    mousex += 1;
                if (up)
                    mousey -= 1;
                if (down)
                    mousey += 1;
                if (chbcursor.Checked)
                {
                    WidthS = Screen.PrimaryScreen.Bounds.Width / 2;
                    HeightS = Screen.PrimaryScreen.Bounds.Height / 2;
                    desktopcursorposition((int)(WidthS - mousex * WidthS / 1024f), (int)(HeightS + mousey * HeightS / 1024f));
                }
                if (chbbrink.Checked)
                {
                    slowing++;
                    if (slowing >= slowinglimit)
                    {
                        MouseBrink((int)(-mousex / dividing - (mousex > 0 ? 1 : -1) * adding), (int)(mousey / dividing + (mousey > 0 ? 1 : -1) * adding));
                        slowing = 0;
                    }
                }
                if (chbtitanfall.Checked)
                {
                    MouseMW3((int)(-mousex / dividing - (mousex > 0 ? 1 : -1) * adding), (int)(mousey / dividing + (mousey > 0 ? 1 : -1) * adding));
                }
                if (chbbo3.Checked)
                {
                    slowing++;
                    if (slowing >= slowinglimit)
                    {
                        MouseMW3((int)(-mousex / dividing - (mousex > 0 ? 1 : -1) * adding), (int)(mousey / dividing + (mousey > 0 ? 1 : -1) * adding));
                        slowing = 0;
                    }
                }
                if (chbfake.Checked)
                {
                    MouseMW3((int)(32767.5f - mousex / dividing - (mousex > 0 ? 1 : -1) * adding), (int)(mousey / dividing + (mousey > 0 ? 1 : -1) * adding + 32767.5f));
                }
                if (chbmetro.Checked)
                {
                    mousexp += mousex;
                    mouseyp += mousey;
                    MouseMW3((int)(32767.5f - mousex / dividing - (mousex > 0 ? 1 : -1) * adding - mousexp), (int)(mousey / dividing + (mousey > 0 ? 1 : -1) * adding + mouseyp + 32767.5f));
                }
                if (chbmw3.Checked)
                {
                    MouseMW3((int)(32767.5f - mousex * 32f - (mousex > 0 ? 1 : -1) * adding), (int)(mousey * 32f + (mousey > 0 ? 1 : -1) * adding + 32767.5f));
                    WidthS = Screen.PrimaryScreen.Bounds.Width / 2;
                    HeightS = Screen.PrimaryScreen.Bounds.Height / 2;
                    desktopcursorposition((int)(WidthS - mousex * WidthS / 1024f), (int)(HeightS + mousey * HeightS / 1024f));
                }
                if (chbxaim.Checked)
                {
                    if (mousex > 1000 | mousex < -1000)
                        mousexp += mousex;
                    if (mousey > 1000 | mousey < -1000)
                        mouseyp += mousey;
                    MouseMW3((int)(32767.5f - mousex / dividing - (mousex > 0 ? 1 : -1) * adding - mousexp), (int)(mousey / dividing + (mousey > 0 ? 1 : -1) * adding + mouseyp + 32767.5f));
                }
                if (chbwarface.Checked)
                {
                    WidthS = Screen.PrimaryScreen.Bounds.Width / 2;
                    HeightS = Screen.PrimaryScreen.Bounds.Height / 2;
                    desktopcursorposition((int)(WidthS + mousex * WidthS / 1024f), (int)(HeightS - mousey * HeightS / 1024f));
                    MouseMW3((int)(32767.5 - mousex * 32f), (int)(mousey * 32f + 32767.5));
                }
                if (chbfortnite.Checked)
                {
                    slowing++;
                    if (slowing >= slowinglimit)
                    {
                        MouseBrink((int)(-mousex / dividing - (mousex > 0 ? 1 : -1) * adding), (int)(mousey / dividing + (mousey > 0 ? 1 : -1) * adding));
                        slowing = 0;
                    }
                    MouseMW3((int)(32767.5 - mousex * 32f - (mousex > 0 ? 1 : -1) * adding), (int)(mousey * 32f + (mousey > 0 ? 1 : -1) * adding + 32767.5));
                    WidthS = Screen.PrimaryScreen.Bounds.Width / 2;
                    HeightS = Screen.PrimaryScreen.Bounds.Height / 2;
                    desktopcursorposition((int)(WidthS - mousex * WidthS / 1024f), (int)(HeightS + mousey * HeightS / 1024f));
                }
                if (chbmc.Checked)
                {
                    WidthS = Screen.PrimaryScreen.Bounds.Width / 2;
                    HeightS = Screen.PrimaryScreen.Bounds.Height / 2;
                    slowing++;
                    if (slowing >= slowinglimit)
                    {
                        MouseBrink((int)(-mousex / dividing - (mousex > 0 ? 1 : -1) * adding), (int)(mousey / dividing + (mousey > 0 ? 1 : -1) * adding));
                        slowing = 0;
                    }
                    desktopcursorposition((int)(WidthS - mousex * WidthS / 1024f), (int)(HeightS + mousey * HeightS / 1024f));
                }
                Thread.Sleep(1);
            }
        }
        private void button1_Click(object sender, EventArgs e)
        {
            string message = @"public double WidthS = Screen.PrimaryScreen.Bounds.Width / 2f;
	        public double HeightS = Screen.PrimaryScreen.Bounds.Height / 2f;
            desktopcursorposition((int)(WidthS - mousex * WidthS / 1024f), (int)(HeightS + mousey * HeightS / 1024f));
            public static void desktopcursorposition(int X, int Y)
            {
                Cursor.Position = new System.Drawing.Point(X, Y);
                SetCursorPos(X, Y);
                SetPhysicalCursorPos(X, Y);
                SetCaretPos(X, Y);
            }";
            MessageBox.Show(message);
            Clipboard.SetDataObject(message, true);
        }
        private void button2_Click(object sender, EventArgs e)
        {
            string message = @"slowing++;
            if (slowing >= "
            + textBox1.Text + @")
            {
                MouseBrink((int)(-mousex / "
            + textBox2.Text + @" - (mousex > 0 ? 1 : -1) * " + textBox3.Text + @"), (int)(mousey / " + textBox2.Text + @" + (mousey > 0 ? 1 : -1) * " + textBox3.Text + @"));
                slowing = 0;
            }";
            MessageBox.Show(message);
            Clipboard.SetDataObject(message, true);
        }
        private void button3_Click(object sender, EventArgs e)
        {
            string message = @"MouseMW3((int)(-mousex / " + textBox2.Text + @" - (mousex > 0 ? 1 : -1) * " + textBox3.Text + @"), (int)(mousey / " + textBox2.Text + @" + (mousey > 0 ? 1 : -1) * " + textBox3.Text + @"));";
            MessageBox.Show(message);
            Clipboard.SetDataObject(message, true);
        }
        private void button4_Click(object sender, EventArgs e)
        {
            string message = @"slowing++;
            if (slowing >= "
            + textBox1.Text + @")
            {
                MouseMW3((int)(-mousex / "
            + textBox2.Text + @" - (mousex > 0 ? 1 : -1) * " + textBox3.Text + @"), (int)(mousey / " + textBox2.Text + @" + (mousey > 0 ? 1 : -1) * " + textBox3.Text + @"));
                slowing = 0;
            }";
            MessageBox.Show(message);
            Clipboard.SetDataObject(message, true);
        }
        private void button5_Click(object sender, EventArgs e)
        {
            string message = @"MouseMW3((int)(32767.5f - mousex / " + textBox2.Text + @" - (mousex > 0 ? 1 : -1) * " + textBox3.Text + @"), (int)(mousey / " + textBox2.Text + @" + (mousey > 0 ? 1 : -1) * " + textBox3.Text + @" + 32767.5f));";
            MessageBox.Show(message);
            Clipboard.SetDataObject(message, true);
        }
        private void button6_Click(object sender, EventArgs e)
        {
            string message = @"mousexp += mousex;
            mouseyp += mousey;
            MouseMW3((int)(32767.5f - mousex / " + textBox2.Text + @" - (mousex > 0 ? 1 : -1) * " + textBox3.Text + @" - mousexp), (int)(mousey / " + textBox2.Text + @" + (mousey > 0 ? 1 : -1) * " + textBox3.Text + @" + mouseyp + 32767.5f));";
            MessageBox.Show(message);
            Clipboard.SetDataObject(message, true);
        }
        private void button7_Click(object sender, EventArgs e)
        {
            string message = @"MouseMW3((int)(32767.5f - mousex * 32f - (mousex > 0 ? 1 : -1) * " + textBox3.Text + @"), (int)(mousey * 32f + (mousey > 0 ? 1 : -1) * " + textBox3.Text + @" + 32767.5f));
            public double WidthS = Screen.PrimaryScreen.Bounds.Width / 2f;
	        public double HeightS = Screen.PrimaryScreen.Bounds.Height / 2f;
            desktopcursorposition((int)(WidthS - mousex * WidthS / 1024f), (int)(HeightS + mousey * HeightS / 1024f));
            public static void desktopcursorposition(int X, int Y)
            {
                Cursor.Position = new System.Drawing.Point(X, Y);
                SetCursorPos(X, Y);
                SetPhysicalCursorPos(X, Y);
                SetCaretPos(X, Y);
            }";
            MessageBox.Show(message);
            Clipboard.SetDataObject(message, true);
        }
        private void button8_Click(object sender, EventArgs e)
        {
            string message = @"if (mousex > 1000 | mousex < -1000)
                 mousexp += mousex;
            if (mousey > 1000 | mousey < -1000)
                 mouseyp += mousey;
            MouseMW3((int)(32767.5f - mousex / " + textBox2.Text + @" - (mousex > 0 ? 1 : -1) * " + textBox3.Text + @" - mousexp), (int)(mousey / " + textBox2.Text + @" + (mousey > 0 ? 1 : -1) * " + textBox3.Text + @" + mouseyp + 32767.5f));";
            MessageBox.Show(message);
            Clipboard.SetDataObject(message, true);
        }
        private void button9_Click(object sender, EventArgs e)
        {
            string message = @"public double WidthS = Screen.PrimaryScreen.Bounds.Width / 2f;
	        public double HeightS = Screen.PrimaryScreen.Bounds.Height / 2f;
            desktopcursorposition((int)(WidthS + mousex * WidthS / 1024f), (int)(HeightS - mousey * HeightS / 1024f));
            MouseMW3((int)(32767.5 - mousex * 32f), (int)(mousey * 32f + 32767.5));
            public static void desktopcursorposition(int X, int Y)
            {
                Cursor.Position = new System.Drawing.Point(X, Y);
                SetCursorPos(X, Y);
                SetPhysicalCursorPos(X, Y);
                SetCaretPos(X, Y);
            }";
            MessageBox.Show(message);
            Clipboard.SetDataObject(message, true);
        }
        private void button10_Click(object sender, EventArgs e)
        {
            string message = @"slowing++;
            if (slowing >= "
            + textBox1.Text + @")
            {
                MouseBrink((int)(-mousex / "
            + textBox2.Text + @" - (mousex > 0 ? 1 : -1) * " + textBox3.Text + @"), (int)(mousey / " + textBox2.Text + @" + (mousey > 0 ? 1 : -1) * " + textBox3.Text + @"));
                slowing = 0;
            }
            MouseMW3((int)(32767.5 - mousex * 32f - (mousex > 0 ? 1 : -1) * " + textBox3.Text + @"), (int)(mousey * 32f + (mousey > 0 ? 1 : -1) * " + textBox3.Text + @" + 32767.5));
            public double WidthS = Screen.PrimaryScreen.Bounds.Width / 2f;
	        public double HeightS = Screen.PrimaryScreen.Bounds.Height / 2f;
            desktopcursorposition((int)(WidthS - mousex * WidthS / 1024f), (int)(HeightS + mousey * HeightS / 1024f));
            public static void desktopcursorposition(int X, int Y)
            {
                Cursor.Position = new System.Drawing.Point(X, Y);
                SetCursorPos(X, Y);
                SetPhysicalCursorPos(X, Y);
                SetCaretPos(X, Y);
            }";
            MessageBox.Show(message);
            Clipboard.SetDataObject(message, true);
        }
        private void button11_Click(object sender, EventArgs e)
        {
            const string message = "• Use F1 to F10 keys to enable/disable mouse functions.\n\r• Use decimal key to disable all mouse functions.\n\r• Increase or decrease mouse values with arrow keys.\n\r• Set to 0 mouse values with numpad0 key.\n\r• Increase or decrease slowing with numpad1 and numpad2 keys.\n\r• Increase or decrease dividing with numpad4 and numpad5 keys.\n\r• Increase or decrease adding with numpad7 and numpad8 keys.";
            const string caption = "MouseControlTester Legend";
            MessageBox.Show(message, caption, MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        private void button12_Click(object sender, EventArgs e)
        {
            string message = @"WidthS = Screen.PrimaryScreen.Bounds.Width / 2;
            HeightS = Screen.PrimaryScreen.Bounds.Height / 2;
            slowing++;
            if (slowing >= "
            + textBox1.Text + @")
            {
                MouseBrink((int)(-mousex / "
            + textBox2.Text + @" - (mousex > 0 ? 1 : -1) * " + textBox3.Text + @"), (int)(mousey / " + textBox2.Text + @" + (mousey > 0 ? 1 : -1) * " + textBox3.Text + @"));
                slowing = 0;
            }
            desktopcursorposition((int)(WidthS - mousex * WidthS / 1024f), (int)(HeightS + mousey * HeightS / 1024f));
            public static void desktopcursorposition(int X, int Y)
            {
                Cursor.Position = new System.Drawing.Point(X, Y);
                SetCursorPos(X, Y);
                SetPhysicalCursorPos(X, Y);
                SetCaretPos(X, Y);
            }";
            MessageBox.Show(message);
            Clipboard.SetDataObject(message, true);
        }
        public static void desktopcursorposition(int X, int Y)
        {
            Cursor.Position = new System.Drawing.Point(X, Y);
            SetCursorPos(X, Y);
            SetPhysicalCursorPos(X, Y);
            SetCaretPos(X, Y);
        }
        private void Form7_Shown(object sender, EventArgs e)
        {
            TimeBeginPeriod(1);
            NtSetTimerResolution(1, true, ref CurrentResolution);
            Task.Run(() => Selection());
            Task.Run(() => Mouse());
        }
        private void Form7_FormClosed(object sender, FormClosedEventArgs e)
        {
            endinvoke = true;
            Thread.Sleep(100);
            TimeEndPeriod(1);
        }
    }
}