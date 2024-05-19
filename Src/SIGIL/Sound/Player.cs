using NAudio.Wave;
using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using Valuechanges;

namespace Sound
{
    public class Valuechanges
    {
        [DllImport("winmm.dll", EntryPoint = "timeBeginPeriod")]
        private static extern uint TimeBeginPeriod(uint ms);
        [DllImport("winmm.dll", EntryPoint = "timeEndPeriod")]
        private static extern uint TimeEndPeriod(uint ms);
        [DllImport("ntdll.dll", EntryPoint = "NtSetTimerResolution")]
        private static extern void NtSetTimerResolution(uint DesiredResolution, bool SetResolution, ref uint CurrentResolution);
        private static uint CurrentResolution = 0;
        public double[] _valuechange = { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
        public double[] _ValueChange = { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
        public Valuechanges()
        {
            TimeBeginPeriod(1);
            NtSetTimerResolution(1, true, ref CurrentResolution);
        }
        public double this[int index]
        {
            get { return _ValueChange[index]; }
            set
            {
                if (_valuechange[index] != value)
                    _ValueChange[index] = value - _valuechange[index];
                else
                    _ValueChange[index] = 0;
                _valuechange[index] = value;
            }
        }
    }
    public class Player
    {
        [DllImport("winmm.dll", EntryPoint = "timeBeginPeriod")]
        private static extern uint TimeBeginPeriod(uint ms);
        [DllImport("winmm.dll", EntryPoint = "timeEndPeriod")]
        private static extern uint TimeEndPeriod(uint ms);
        [DllImport("ntdll.dll", EntryPoint = "NtSetTimerResolution")]
        private static extern void NtSetTimerResolution(uint DesiredResolution, bool SetResolution, ref uint CurrentResolution);
        private static uint CurrentResolution = 0;
        private int number;
        private string pathsound1 = "", pathsound2 = "", pathsound3 = "", pathsound4 = "", pathsound5 = "", pathsound6 = "", pathsound7 = "", pathsound8 = "", pathsound9 = "", pathsound10 = "", pathsound11 = "", pathsound12 = "", pathtempsound1 = "", pathtempsound2 = "", pathtempsound3 = "", pathtempsound4 = "", pathtempsound5 = "", pathtempsound6 = "", pathtempsound7 = "", pathtempsound8 = "", pathtempsound9 = "", pathtempsound10 = "", pathtempsound11 = "", pathtempsound12 = "";
        private Valuechanges ValueChanges = new Valuechanges();
        private MediaFoundationReader[] audioFileReader = { null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null };
        private IWavePlayer[] waveOutDevice = { null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null };
        private bool formvisible;
        private Form1 form1 = new Form1();
        private Stopwatch PollingRate;
        private double pollingrateperm = 0, pollingratetemp = 0, pollingratedisplay = 0, pollingrate;
        private string inputdelaybutton = "", inputdelay = "", inputdelaytemp = "";
        public Valuechange ValueChange;
        private double delay, elapseddown, elapsedup, elapsed;
        private bool getstate = false;
        private int[] wd = { 2 };
        private int[] wu = { 2 };
        public void valchanged(int n, bool val)
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
        public Player()
        {
            TimeBeginPeriod(1);
            NtSetTimerResolution(1, true, ref CurrentResolution);
        }
        public void ViewData(string inputdelaybutton = "")
        {
            if (!formvisible)
            {
                PollingRate = new Stopwatch();
                PollingRate.Start();
                ValueChange = new Valuechange();
                this.inputdelaybutton = inputdelaybutton;
                formvisible = true;
                Task.Run(() => form1.SetVisible());
            }
        }
        public void Connect(string pathsound1 = "", string pathsound2 = "", string pathsound3 = "", string pathsound4 = "", string pathsound5 = "", string pathsound6 = "", string pathsound7 = "", string pathsound8 = "", string pathsound9 = "", string pathsound10 = "", string pathsound11 = "", string pathsound12 = "", string pathtempsound1 = "", string pathtempsound2 = "", string pathtempsound3 = "", string pathtempsound4 = "", string pathtempsound5 = "", string pathtempsound6 = "", string pathtempsound7 = "", string pathtempsound8 = "", string pathtempsound9 = "", string pathtempsound10 = "", string pathtempsound11 = "", string pathtempsound12 = "", int number = 0)
        {
            this.number = number;
            this.pathsound1 = pathsound1;
            this.pathsound2 = pathsound2;
            this.pathsound3 = pathsound3;
            this.pathsound4 = pathsound4;
            this.pathsound5 = pathsound5;
            this.pathsound6 = pathsound6;
            this.pathsound7 = pathsound7;
            this.pathsound8 = pathsound8;
            this.pathsound9 = pathsound9;
            this.pathsound10 = pathsound10;
            this.pathsound11 = pathsound11;
            this.pathsound12 = pathsound12;
            this.pathtempsound1 = pathtempsound1;
            this.pathtempsound2 = pathtempsound2;
            this.pathtempsound3 = pathtempsound3;
            this.pathtempsound4 = pathtempsound4;
            this.pathtempsound5 = pathtempsound5;
            this.pathtempsound6 = pathtempsound6;
            this.pathtempsound7 = pathtempsound7;
            this.pathtempsound8 = pathtempsound8;
            this.pathtempsound9 = pathtempsound9;
            this.pathtempsound10 = pathtempsound10;
            this.pathtempsound11 = pathtempsound11;
            this.pathtempsound12 = pathtempsound12;
            if (pathsound1 != "")
            {
                audioFileReader[0] = new MediaFoundationReader(pathsound1);
                waveOutDevice[0] = new WaveOutEvent();
                waveOutDevice[0].Init(audioFileReader[0]);
            }
            if (pathsound2 != "")
            {
                audioFileReader[1] = new MediaFoundationReader(pathsound2);
                waveOutDevice[1] = new WaveOutEvent();
                waveOutDevice[1].Init(audioFileReader[1]);
            }
            if (pathsound3 != "")
            {
                audioFileReader[2] = new MediaFoundationReader(pathsound3);
                waveOutDevice[2] = new WaveOutEvent();
                waveOutDevice[2].Init(audioFileReader[2]);
            }
            if (pathsound4 != "")
            {
                audioFileReader[3] = new MediaFoundationReader(pathsound4);
                waveOutDevice[3] = new WaveOutEvent();
                waveOutDevice[3].Init(audioFileReader[3]);
            }
            if (pathsound5 != "")
            {
                audioFileReader[4] = new MediaFoundationReader(pathsound5);
                waveOutDevice[4] = new WaveOutEvent();
                waveOutDevice[4].Init(audioFileReader[4]);
            }
            if (pathsound6 != "")
            {
                audioFileReader[5] = new MediaFoundationReader(pathsound6);
                waveOutDevice[5] = new WaveOutEvent();
                waveOutDevice[5].Init(audioFileReader[5]);
            }
            if (pathsound7 != "")
            {
                audioFileReader[6] = new MediaFoundationReader(pathsound7);
                waveOutDevice[6] = new WaveOutEvent();
                waveOutDevice[6].Init(audioFileReader[6]);
            }
            if (pathsound8 != "")
            {
                audioFileReader[7] = new MediaFoundationReader(pathsound8);
                waveOutDevice[7] = new WaveOutEvent();
                waveOutDevice[7].Init(audioFileReader[7]);
            }
            if (pathsound9 != "")
            {
                audioFileReader[8] = new MediaFoundationReader(pathsound9);
                waveOutDevice[8] = new WaveOutEvent();
                waveOutDevice[8].Init(audioFileReader[8]);
            }
            if (pathsound10 != "")
            {
                audioFileReader[9] = new MediaFoundationReader(pathsound10);
                waveOutDevice[9] = new WaveOutEvent();
                waveOutDevice[9].Init(audioFileReader[9]);
            }
            if (pathsound11 != "")
            {
                audioFileReader[10] = new MediaFoundationReader(pathsound11);
                waveOutDevice[10] = new WaveOutEvent();
                waveOutDevice[10].Init(audioFileReader[10]);
            }
            if (pathsound12 != "")
            {
                audioFileReader[11] = new MediaFoundationReader(pathsound12);
                waveOutDevice[11] = new WaveOutEvent();
                waveOutDevice[11].Init(audioFileReader[11]);
            }
            if (pathtempsound1 != "")
            {
                audioFileReader[12] = new MediaFoundationReader(pathtempsound1);
                waveOutDevice[12] = new WaveOutEvent();
                waveOutDevice[12].Init(audioFileReader[12]);
            }
            if (pathtempsound2 != "")
            {
                audioFileReader[13] = new MediaFoundationReader(pathtempsound2);
                waveOutDevice[13] = new WaveOutEvent();
                waveOutDevice[13].Init(audioFileReader[13]);
            }
            if (pathtempsound3 != "")
            {
                audioFileReader[14] = new MediaFoundationReader(pathtempsound3);
                waveOutDevice[14] = new WaveOutEvent();
                waveOutDevice[14].Init(audioFileReader[14]);
            }
            if (pathtempsound4 != "")
            {
                audioFileReader[15] = new MediaFoundationReader(pathtempsound4);
                waveOutDevice[15] = new WaveOutEvent();
                waveOutDevice[15].Init(audioFileReader[15]);
            }
            if (pathtempsound5 != "")
            {
                audioFileReader[16] = new MediaFoundationReader(pathtempsound5);
                waveOutDevice[16] = new WaveOutEvent();
                waveOutDevice[16].Init(audioFileReader[16]);
            }
            if (pathtempsound6 != "")
            {
                audioFileReader[17] = new MediaFoundationReader(pathtempsound6);
                waveOutDevice[17] = new WaveOutEvent();
                waveOutDevice[17].Init(audioFileReader[17]);
            }
            if (pathtempsound7 != "")
            {
                audioFileReader[18] = new MediaFoundationReader(pathtempsound7);
                waveOutDevice[18] = new WaveOutEvent();
                waveOutDevice[18].Init(audioFileReader[18]);
            }
            if (pathtempsound8 != "")
            {
                audioFileReader[19] = new MediaFoundationReader(pathtempsound8);
                waveOutDevice[19] = new WaveOutEvent();
                waveOutDevice[19].Init(audioFileReader[19]);
            }
            if (pathtempsound9 != "")
            {
                audioFileReader[20] = new MediaFoundationReader(pathtempsound9);
                waveOutDevice[20] = new WaveOutEvent();
                waveOutDevice[20].Init(audioFileReader[20]);
            }
            if (pathtempsound10 != "")
            {
                audioFileReader[21] = new MediaFoundationReader(pathtempsound10);
                waveOutDevice[21] = new WaveOutEvent();
                waveOutDevice[21].Init(audioFileReader[21]);
            }
            if (pathtempsound11 != "")
            {
                audioFileReader[22] = new MediaFoundationReader(pathtempsound11);
                waveOutDevice[22] = new WaveOutEvent();
                waveOutDevice[22].Init(audioFileReader[22]);
            }
            if (pathtempsound12 != "")
            {
                audioFileReader[23] = new MediaFoundationReader(pathtempsound12);
                waveOutDevice[23] = new WaveOutEvent();
                waveOutDevice[23].Init(audioFileReader[23]);
            }
        }
        public void Disconnect()
        {
            if (pathsound1 != "")
                try
                {
                    waveOutDevice[0].Stop();
                    audioFileReader[0].Close();
                    waveOutDevice[0].Dispose();
                    audioFileReader[0].Dispose();
                }
                catch { }
            if (pathsound2 != "")
                try
                {
                    waveOutDevice[1].Stop();
                    audioFileReader[1].Close();
                    waveOutDevice[1].Dispose();
                    audioFileReader[1].Dispose();
                }
                catch { }
            if (pathsound3 != "")
                try
                {
                    waveOutDevice[2].Stop();
                    audioFileReader[2].Close();
                    waveOutDevice[2].Dispose();
                    audioFileReader[2].Dispose();
                }
                catch { }
            if (pathsound4 != "")
                try
                {
                    waveOutDevice[3].Stop();
                    audioFileReader[3].Close();
                    waveOutDevice[3].Dispose();
                    audioFileReader[3].Dispose();
                }
                catch { }
            if (pathsound5 != "")
                try
                {
                    waveOutDevice[4].Stop();
                    audioFileReader[4].Close();
                    waveOutDevice[4].Dispose();
                    audioFileReader[4].Dispose();
                }
                catch { }
            if (pathsound6 != "")
                try
                {
                    waveOutDevice[5].Stop();
                    audioFileReader[5].Close();
                    waveOutDevice[5].Dispose();
                    audioFileReader[5].Dispose();
                }
                catch { }
            if (pathsound7 != "")
                try
                {
                    waveOutDevice[6].Stop();
                    audioFileReader[6].Close();
                    waveOutDevice[6].Dispose();
                    audioFileReader[6].Dispose();
                }
                catch { }
            if (pathsound8 != "")
                try
                {
                    waveOutDevice[7].Stop();
                    audioFileReader[7].Close();
                    waveOutDevice[7].Dispose();
                    audioFileReader[7].Dispose();
                }
                catch { }
            if (pathsound9 != "")
                try
                {
                    waveOutDevice[8].Stop();
                    audioFileReader[8].Close();
                    waveOutDevice[8].Dispose();
                    audioFileReader[8].Dispose();
                }
                catch { }
            if (pathsound10 != "")
                try
                {
                    waveOutDevice[9].Stop();
                    audioFileReader[9].Close();
                    waveOutDevice[9].Dispose();
                    audioFileReader[9].Dispose();
                }
                catch { }
            if (pathsound11 != "")
                try
                {
                    waveOutDevice[10].Stop();
                    audioFileReader[10].Close();
                    waveOutDevice[10].Dispose();
                    audioFileReader[10].Dispose();
                }
                catch { }
            if (pathsound12 != "")
                try
                {
                    waveOutDevice[11].Stop();
                    audioFileReader[11].Close();
                    waveOutDevice[11].Dispose();
                    audioFileReader[11].Dispose();
                }
                catch { }
            if (pathtempsound1 != "")
                try
                {
                    waveOutDevice[12].Stop();
                    audioFileReader[12].Close();
                    waveOutDevice[12].Dispose();
                    audioFileReader[12].Dispose();
                }
                catch { }
            if (pathtempsound2 != "")
                try
                {
                    waveOutDevice[13].Stop();
                    audioFileReader[13].Close();
                    waveOutDevice[13].Dispose();
                    audioFileReader[13].Dispose();
                }
                catch { }
            if (pathtempsound3 != "")
                try
                {
                    waveOutDevice[14].Stop();
                    audioFileReader[14].Close();
                    waveOutDevice[14].Dispose();
                    audioFileReader[14].Dispose();
                }
                catch { }
            if (pathtempsound4 != "")
                try
                {
                    waveOutDevice[15].Stop();
                    audioFileReader[15].Close();
                    waveOutDevice[15].Dispose();
                    audioFileReader[15].Dispose();
                }
                catch { }
            if (pathtempsound5 != "")
                try
                {
                    waveOutDevice[16].Stop();
                    audioFileReader[16].Close();
                    waveOutDevice[16].Dispose();
                    audioFileReader[16].Dispose();
                }
                catch { }
            if (pathtempsound6 != "")
                try
                {
                    waveOutDevice[17].Stop();
                    audioFileReader[17].Close();
                    waveOutDevice[17].Dispose();
                    audioFileReader[17].Dispose();
                }
                catch { }
            if (pathtempsound7 != "")
                try
                {
                    waveOutDevice[18].Stop();
                    audioFileReader[18].Close();
                    waveOutDevice[18].Dispose();
                    audioFileReader[18].Dispose();
                }
                catch { }
            if (pathtempsound8 != "")
                try
                {
                    waveOutDevice[19].Stop();
                    audioFileReader[19].Close();
                    waveOutDevice[19].Dispose();
                    audioFileReader[19].Dispose();
                }
                catch { }
            if (pathtempsound9 != "")
                try
                {
                    waveOutDevice[20].Stop();
                    audioFileReader[20].Close();
                    waveOutDevice[20].Dispose();
                    audioFileReader[20].Dispose();
                }
                catch { }
            if (pathtempsound10 != "")
                try
                {
                    waveOutDevice[21].Stop();
                    audioFileReader[21].Close();
                    waveOutDevice[21].Dispose();
                    audioFileReader[21].Dispose();
                }
                catch { }
            if (pathtempsound11 != "")
                try
                {
                    waveOutDevice[22].Stop();
                    audioFileReader[22].Close();
                    waveOutDevice[22].Dispose();
                    audioFileReader[22].Dispose();
                }
                catch { }
            if (pathtempsound12 != "")
                try
                {
                    waveOutDevice[23].Stop();
                    audioFileReader[23].Close();
                    waveOutDevice[23].Dispose();
                    audioFileReader[23].Dispose();
                }
                catch { }
        }
        public void Set(bool sound1, bool sound2, bool sound3, bool sound4, bool sound5, bool sound6, bool sound7, bool sound8, bool sound9, bool sound10, bool sound11, bool sound12, bool tempsound1, bool tempsound2, bool tempsound3, bool tempsound4, bool tempsound5, bool tempsound6, bool tempsound7, bool tempsound8, bool tempsound9, bool tempsound10, bool tempsound11, bool tempsound12)
        {
            ValueChanges[0] = sound1 ? 1 : 0;
            if (ValueChanges._ValueChange[0] > 0f)
            {
                audioFileReader[0].Position = 0;
                audioFileReader[0].CurrentTime = TimeSpan.Zero;
                waveOutDevice[0].Play();
            }
            ValueChanges[1] = sound2 ? 1 : 0;
            if (ValueChanges._ValueChange[1] > 0f)
            {
                audioFileReader[1].Position = 0;
                audioFileReader[1].CurrentTime = TimeSpan.Zero;
                waveOutDevice[1].Play();
            }
            ValueChanges[2] = sound3 ? 1 : 0;
            if (ValueChanges._ValueChange[2] > 0f)
            {
                audioFileReader[2].Position = 0;
                audioFileReader[2].CurrentTime = TimeSpan.Zero;
                waveOutDevice[2].Play();
            }
            ValueChanges[3] = sound4 ? 1 : 0;
            if (ValueChanges._ValueChange[3] > 0f)
            {
                audioFileReader[3].Position = 0;
                audioFileReader[3].CurrentTime = TimeSpan.Zero;
                waveOutDevice[3].Play();
            }
            ValueChanges[4] = sound5 ? 1 : 0;
            if (ValueChanges._ValueChange[4] > 0f)
            {
                audioFileReader[4].Position = 0;
                audioFileReader[4].CurrentTime = TimeSpan.Zero;
                waveOutDevice[4].Play();
            }
            ValueChanges[5] = sound6 ? 1 : 0;
            if (ValueChanges._ValueChange[5] > 0f)
            {
                audioFileReader[5].Position = 0;
                audioFileReader[5].CurrentTime = TimeSpan.Zero;
                waveOutDevice[5].Play();
            }
            ValueChanges[6] = sound7 ? 1 : 0;
            if (ValueChanges._ValueChange[6] > 0f)
            {
                audioFileReader[6].Position = 0;
                audioFileReader[6].CurrentTime = TimeSpan.Zero;
                waveOutDevice[6].Play();
            }
            ValueChanges[7] = sound8 ? 1 : 0;
            if (ValueChanges._ValueChange[7] > 0f)
            {
                audioFileReader[7].Position = 0;
                audioFileReader[7].CurrentTime = TimeSpan.Zero;
                waveOutDevice[7].Play();
            }
            ValueChanges[8] = sound9 ? 1 : 0;
            if (ValueChanges._ValueChange[8] > 0f)
            {
                audioFileReader[8].Position = 0;
                audioFileReader[8].CurrentTime = TimeSpan.Zero;
                waveOutDevice[8].Play();
            }
            ValueChanges[9] = sound10 ? 1 : 0;
            if (ValueChanges._ValueChange[9] > 0f)
            {
                audioFileReader[9].Position = 0;
                audioFileReader[9].CurrentTime = TimeSpan.Zero;
                waveOutDevice[9].Play();
            }
            ValueChanges[10] = sound11 ? 1 : 0;
            if (ValueChanges._ValueChange[10] > 0f)
            {
                audioFileReader[10].Position = 0;
                audioFileReader[10].CurrentTime = TimeSpan.Zero;
                waveOutDevice[10].Play();
            }
            ValueChanges[11] = sound12 ? 1 : 0;
            if (ValueChanges._ValueChange[11] > 0f)
            {
                audioFileReader[11].Position = 0;
                audioFileReader[11].CurrentTime = TimeSpan.Zero;
                waveOutDevice[11].Play();
            }
            if (tempsound1)
                waveOutDevice[12].Play();
            else if (pathtempsound1 != "")
            {
                waveOutDevice[12].Stop();
                audioFileReader[12].Position = 0;
                audioFileReader[12].CurrentTime = TimeSpan.Zero;
            }
            if (tempsound2)
                waveOutDevice[13].Play();
            else if (pathtempsound2 != "")
            {
                waveOutDevice[13].Stop();
                audioFileReader[13].Position = 0;
                audioFileReader[13].CurrentTime = TimeSpan.Zero;
            }
            if (tempsound3)
                waveOutDevice[14].Play();
            else if (pathtempsound3 != "")
            {
                waveOutDevice[14].Stop();
                audioFileReader[14].Position = 0;
                audioFileReader[14].CurrentTime = TimeSpan.Zero;
            }
            if (tempsound4)
                waveOutDevice[15].Play();
            else if (pathtempsound4 != "")
            {
                waveOutDevice[15].Stop();
                audioFileReader[15].Position = 0;
                audioFileReader[15].CurrentTime = TimeSpan.Zero;
            }
            if (tempsound5)
                waveOutDevice[16].Play();
            else if (pathtempsound5 != "")
            {
                waveOutDevice[16].Stop();
                audioFileReader[16].Position = 0;
                audioFileReader[16].CurrentTime = TimeSpan.Zero;
            }
            if (tempsound6)
                waveOutDevice[17].Play();
            else if (pathtempsound6 != "")
            {
                waveOutDevice[17].Stop();
                audioFileReader[17].Position = 0;
                audioFileReader[17].CurrentTime = TimeSpan.Zero;
            }
            if (tempsound7)
                waveOutDevice[18].Play();
            else if (pathtempsound7 != "")
            {
                waveOutDevice[18].Stop();
                audioFileReader[18].Position = 0;
                audioFileReader[18].CurrentTime = TimeSpan.Zero;
            }
            if (tempsound8)
                waveOutDevice[19].Play();
            else if (pathtempsound8 != "")
            {
                waveOutDevice[19].Stop();
                audioFileReader[19].Position = 0;
                audioFileReader[19].CurrentTime = TimeSpan.Zero;
            }
            if (tempsound9)
                waveOutDevice[20].Play();
            else if (pathtempsound9 != "")
            {
                waveOutDevice[20].Stop();
                audioFileReader[20].Position = 0;
                audioFileReader[20].CurrentTime = TimeSpan.Zero;
            }
            if (tempsound10)
                waveOutDevice[21].Play();
            else if (pathtempsound10 != "")
            {
                waveOutDevice[21].Stop();
                audioFileReader[21].Position = 0;
                audioFileReader[21].CurrentTime = TimeSpan.Zero;
            }
            if (tempsound11)
                waveOutDevice[22].Play();
            else if (pathtempsound11 != "")
            {
                waveOutDevice[22].Stop();
                audioFileReader[22].Position = 0;
                audioFileReader[22].CurrentTime = TimeSpan.Zero;
            }
            if (tempsound12)
                waveOutDevice[23].Play();
            else if (pathtempsound12 != "")
            {
                waveOutDevice[23].Stop();
                audioFileReader[23].Position = 0;
                audioFileReader[23].CurrentTime = TimeSpan.Zero;
            }
            if (formvisible)
            {
                pollingratedisplay++;
                pollingratetemp = pollingrateperm;
                pollingrateperm = (double)PollingRate.ElapsedTicks / (Stopwatch.Frequency / 1000L);
                if (pollingratedisplay > 300)
                {
                    pollingrate = pollingrateperm - pollingratetemp;
                    pollingratedisplay = 0;
                }
                string str = "sound1 : " + sound1 + Environment.NewLine;
                str += "sound2 : " + sound2 + Environment.NewLine;
                str += "sound3 : " + sound3 + Environment.NewLine;
                str += "sound4 : " + sound4 + Environment.NewLine;
                str += "sound5 : " + sound5 + Environment.NewLine;
                str += "sound6 : " + sound6 + Environment.NewLine;
                str += "sound7 : " + sound7 + Environment.NewLine;
                str += "sound8 : " + sound8 + Environment.NewLine;
                str += "sound9 : " + sound9 + Environment.NewLine;
                str += "sound10 : " + sound10 + Environment.NewLine;
                str += "sound11 : " + sound11 + Environment.NewLine;
                str += "sound12 : " + sound12 + Environment.NewLine;
                str += "tempsound1 : " + tempsound1 + Environment.NewLine;
                str += "tempsound2 : " + tempsound2 + Environment.NewLine;
                str += "tempsound3 : " + tempsound3 + Environment.NewLine;
                str += "tempsound4 : " + tempsound4 + Environment.NewLine;
                str += "tempsound5 : " + tempsound5 + Environment.NewLine;
                str += "tempsound6 : " + tempsound6 + Environment.NewLine;
                str += "tempsound7 : " + tempsound7 + Environment.NewLine;
                str += "tempsound8 : " + tempsound8 + Environment.NewLine;
                str += "tempsound9 : " + tempsound9 + Environment.NewLine;
                str += "tempsound10 : " + tempsound10 + Environment.NewLine;
                str += "tempsound11 : " + tempsound11 + Environment.NewLine;
                str += "tempsound12 : " + tempsound12 + Environment.NewLine;
                str += "PollingRate : " + pollingrate + " ms" + Environment.NewLine;
                string txt = str;
                string[] lines = txt.Split(new string[] { Environment.NewLine }, StringSplitOptions.None);
                foreach (string line in lines)
                    if (line.Contains(inputdelaybutton + " : "))
                    {
                        inputdelaytemp = inputdelay;
                        inputdelay = line;
                    }
                valchanged(0, inputdelay.Contains("True") | (!inputdelay.Contains("True") & !inputdelay.Contains("False") & inputdelay != inputdelaytemp));
                if (wd[0] == 1)
                {
                    getstate = true;
                }
                if (inputdelay.Contains("False") | (!inputdelay.Contains("True") & !inputdelay.Contains("False") & inputdelay == inputdelaytemp))
                    getstate = false;
                if (getstate)
                {
                    elapseddown = (double)PollingRate.ElapsedTicks / (Stopwatch.Frequency / 1000L);
                    elapsed = 0;
                }
                if (wu[0] == 1)
                {
                    elapsedup = (double)PollingRate.ElapsedTicks / (Stopwatch.Frequency / 1000L);
                    elapsed = elapsedup - elapseddown;
                }
                ValueChange[0] = inputdelay.Contains("False") | (!inputdelay.Contains("True") & !inputdelay.Contains("False") & inputdelay == inputdelaytemp) ? elapsed : 0;
                if (ValueChange._ValueChange[0] > 0)
                {
                    delay = ValueChange._ValueChange[0];
                }
                str += "InputDelay : " + delay + " ms" + Environment.NewLine;
                str += Environment.NewLine;
                form1.SetLabel1(str);
            }
        }
    }
}