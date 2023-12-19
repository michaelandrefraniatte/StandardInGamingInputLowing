using System.Windows.Forms;

namespace Xinputs
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        public void SetLabel1(string str)
        {
            this.label1.Text = str;
        }
        public void SetVisible()
        {
            this.ShowDialog();
        }
        public void SetUnvisible()
        {
            this.Hide();
        }
        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.Hide();
            e.Cancel = true;
        }
    }
}