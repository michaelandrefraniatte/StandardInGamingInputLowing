using System;
using System.Windows.Forms;
using System.Text.RegularExpressions;
using System.Collections.Generic;
using FastColoredTextBoxNS;
using Range = FastColoredTextBoxNS.Range;
using System.Runtime.InteropServices;

namespace SIGIL
{
    public partial class ReplaceForm : Form
    {
        [DllImport("winmm.dll", EntryPoint = "timeBeginPeriod")]
        private static extern uint TimeBeginPeriod(uint ms);
        [DllImport("winmm.dll", EntryPoint = "timeEndPeriod")]
        private static extern uint TimeEndPeriod(uint ms);
        [DllImport("ntdll.dll", EntryPoint = "NtSetTimerResolution")]
        private static extern void NtSetTimerResolution(uint DesiredResolution, bool SetResolution, ref uint CurrentResolution);
        private static uint CurrentResolution = 0;
        private FastColoredTextBox tb;
        private bool firstSearch = true;
        private Place startPlace;
        public ReplaceForm(FastColoredTextBox tb)
        {
            InitializeComponent();
            this.tb = tb;
        }
        private void btClose_Click(object sender, EventArgs e)
        {
            Close();
        }
        private void btFindNext_Click(object sender, EventArgs e)
        {
            try
            {
                if (!Find(tbFind.Text))
                {
                    MessageBox.Show("Not found");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private List<Range> FindAll(string pattern)
        {
            var opt = cbMatchCase.Checked ? RegexOptions.None : RegexOptions.IgnoreCase;
            if (!cbRegex.Checked)
                pattern = Regex.Escape(pattern);
            if (cbWholeWord.Checked)
                pattern = "\\b" + pattern + "\\b";
            var range = tb.Range.Clone();
            var list = new List<Range>();
            foreach (var r in range.GetRangesByLines(pattern, opt))
                list.Add(r);
            return list;
        }
        private bool Find(string pattern)
        {
            RegexOptions opt = cbMatchCase.Checked ? RegexOptions.None : RegexOptions.IgnoreCase;
            if (!cbRegex.Checked)
                pattern = Regex.Escape(pattern);
            if (cbWholeWord.Checked)
                pattern = "\\b" + pattern + "\\b";
            Range range = tb.Selection.Clone();
            range.Normalize();
            if (firstSearch)
            {
                startPlace = range.Start;
                firstSearch = false;
            }
            range.Start = range.End;
            if (range.Start >= startPlace)
                range.End = new Place(tb.GetLineLength(tb.LinesCount - 1), tb.LinesCount - 1);
            else
                range.End = startPlace;
            foreach (var r in range.GetRangesByLines(pattern, opt))
            {
                tb.Selection.Start = r.Start;
                tb.Selection.End = r.End;
                tb.DoSelectionVisible();
                tb.Invalidate();
                return true;
            }
            if (range.Start >= startPlace && startPlace > Place.Empty)
            {
                tb.Selection.Start = new Place(0, 0);
                return Find(pattern);
            }
            return false;
        }
        private void tbFind_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == '\r')
                btFindNext_Click(sender, null);
            if (e.KeyChar == '\x1b')
                Hide();
        }
        private void ReplaceForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (e.CloseReason == CloseReason.UserClosing)
            {
                Form1.replaceformvisible = false;
                this.Hide();
                e.Cancel = true;
            }
            this.tb.Focus();
        }
        private void btReplace_Click(object sender, EventArgs e)
        {
            try
            {
                if (tb.SelectionLength != 0 & !tb.Selection.ReadOnly)
                    tb.InsertText(tbReplace.Text);
                btFindNext_Click(sender, null);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private void btReplaceAll_Click(object sender, EventArgs e)
        {
            try
            {
                tb.Selection.BeginUpdate();
                var ranges = FindAll(tbFind.Text);
                var ro = false;
                foreach (var r in ranges)
                {
                    if (r.ReadOnly)
                    {
                        ro = true;
                        break;
                    }
                }
                if (!ro & ranges.Count > 0)
                {
                    tb.TextSource.Manager.ExecuteCommand(new ReplaceTextCommand(tb.TextSource, ranges, tbReplace.Text));
                    tb.Selection.Start = new Place(0, 0);
                }
                tb.Invalidate();
                MessageBox.Show(ranges.Count + " occurrence(s) replaced");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            tb.Selection.EndUpdate();
        }
        protected override void OnActivated(EventArgs e)
        {
            tbFind.Focus();
            firstSearch = true;
        }
        private void cbMatchCase_CheckedChanged(object sender, EventArgs e)
        {
            firstSearch = true;
        }
        private void ReplaceForm_Load(object sender, EventArgs e)
        {
            TimeBeginPeriod(1);
            NtSetTimerResolution(1, true, ref CurrentResolution);
        }
    }
}