using NAudio.Wave;
using System.Runtime.InteropServices;

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
        private Valuechanges ValueChange = new Valuechanges();
        private MediaFoundationReader[] audioFileReader = { null, null, null, null, null, null, null, null, null, null, null, null };
        private IWavePlayer[] waveOutDevice = { null, null, null, null, null, null, null, null, null, null, null, null };
        public Player()
        {
            TimeBeginPeriod(1);
            NtSetTimerResolution(1, true, ref CurrentResolution);
        }
        public void Connect(string pathsound1 = "", string pathsound2 = "", string pathsound3 = "", string pathsound4 = "", string pathsound5 = "", string pathsound6 = "", string pathsound7 = "", string pathsound8 = "", string pathsound9 = "", string pathsound10 = "", string pathsound11 = "", string pathsound12 = "", int number = 0)
        {
            this.number = number;
            if (pathsound1 != "")
            {
                audioFileReader[0] = new MediaFoundationReader(pathsound1);
                waveOutDevice[0].Init(audioFileReader[0]);
            }
            if (pathsound2 != "")
            {
                audioFileReader[1] = new MediaFoundationReader(pathsound2);
                waveOutDevice[1].Init(audioFileReader[1]);
            }
            if (pathsound3 != "")
            {
                audioFileReader[2] = new MediaFoundationReader(pathsound3);
                waveOutDevice[2].Init(audioFileReader[2]);
            }
            if (pathsound4 != "")
            {
                audioFileReader[3] = new MediaFoundationReader(pathsound4);
                waveOutDevice[3].Init(audioFileReader[3]);
            }
            if (pathsound5 != "")
            {
                audioFileReader[4] = new MediaFoundationReader(pathsound5);
                waveOutDevice[4].Init(audioFileReader[4]);
            }
            if (pathsound6 != "")
            {
                audioFileReader[5] = new MediaFoundationReader(pathsound6);
                waveOutDevice[5].Init(audioFileReader[5]);
            }
            if (pathsound7 != "")
            {
                audioFileReader[6] = new MediaFoundationReader(pathsound7);
                waveOutDevice[6].Init(audioFileReader[6]);
            }
            if (pathsound8 != "")
            {
                audioFileReader[7] = new MediaFoundationReader(pathsound8);
                waveOutDevice[7].Init(audioFileReader[7]);
            }
            if (pathsound9 != "")
            {
                audioFileReader[8] = new MediaFoundationReader(pathsound9);
                waveOutDevice[8].Init(audioFileReader[8]);
            }
            if (pathsound10 != "")
            {
                audioFileReader[9] = new MediaFoundationReader(pathsound10);
                waveOutDevice[9].Init(audioFileReader[9]);
            }
            if (pathsound11 != "")
            {
                audioFileReader[10] = new MediaFoundationReader(pathsound11);
                waveOutDevice[10].Init(audioFileReader[10]);
            }
            if (pathsound12 != "")
            {
                audioFileReader[11] = new MediaFoundationReader(pathsound12);
                waveOutDevice[11].Init(audioFileReader[11]);
            }
        }
        public void Disconnect()
        {
            try
            {
                waveOutDevice[0].Stop();
                audioFileReader[0].Close();
                waveOutDevice[0].Dispose();
                audioFileReader[0].Dispose();
            }
            catch { }
            try
            {
                waveOutDevice[1].Stop();
                audioFileReader[1].Close();
                waveOutDevice[1].Dispose();
                audioFileReader[1].Dispose();
            }
            catch { }
            try
            {
                waveOutDevice[2].Stop();
                audioFileReader[2].Close();
                waveOutDevice[2].Dispose();
                audioFileReader[2].Dispose();
            }
            catch { }
            try
            {
                waveOutDevice[3].Stop();
                audioFileReader[3].Close();
                waveOutDevice[3].Dispose();
                audioFileReader[3].Dispose();
            }
            catch { }
            try
            {
                waveOutDevice[4].Stop();
                audioFileReader[4].Close();
                waveOutDevice[4].Dispose();
                audioFileReader[4].Dispose();
            }
            catch { }
            try
            {
                waveOutDevice[5].Stop();
                audioFileReader[5].Close();
                waveOutDevice[5].Dispose();
                audioFileReader[5].Dispose();
            }
            catch { }
            try
            {
                waveOutDevice[6].Stop();
                audioFileReader[6].Close();
                waveOutDevice[6].Dispose();
                audioFileReader[6].Dispose();
            }
            catch { }
            try
            {
                waveOutDevice[7].Stop();
                audioFileReader[7].Close();
                waveOutDevice[7].Dispose();
                audioFileReader[7].Dispose();
            }
            catch { }
            try
            {
                waveOutDevice[8].Stop();
                audioFileReader[8].Close();
                waveOutDevice[8].Dispose();
                audioFileReader[8].Dispose();
            }
            catch { }
            try
            {
                waveOutDevice[9].Stop();
                audioFileReader[9].Close();
                waveOutDevice[9].Dispose();
                audioFileReader[9].Dispose();
            }
            catch { }
            try
            {
                waveOutDevice[10].Stop();
                audioFileReader[10].Close();
                waveOutDevice[10].Dispose();
                audioFileReader[10].Dispose();
            }
            catch { }
            try
            {
                waveOutDevice[11].Stop();
                audioFileReader[11].Close();
                waveOutDevice[11].Dispose();
                audioFileReader[11].Dispose();
            }
            catch { }
        }
        public void Set(bool sound1, bool sound2, bool sound3, bool sound4, bool sound5, bool sound6, bool sound7, bool sound8, bool sound9, bool sound10, bool sound11, bool sound12)
        {
            ValueChange[0] = sound1 ? 1 : 0;
            if (ValueChange._ValueChange[0] > 0f)
            {
                waveOutDevice[0].Play();
            }
            ValueChange[1] = sound2 ? 1 : 0;
            if (ValueChange._ValueChange[1] > 0f)
            {
                waveOutDevice[1].Play();
            }
            ValueChange[2] = sound3 ? 1 : 0;
            if (ValueChange._ValueChange[2] > 0f)
            {
                waveOutDevice[2].Play();
            }
            ValueChange[3] = sound4 ? 1 : 0;
            if (ValueChange._ValueChange[3] > 0f)
            {
                waveOutDevice[3].Play();
            }
            ValueChange[4] = sound5 ? 1 : 0;
            if (ValueChange._ValueChange[4] > 0f)
            {
                waveOutDevice[4].Play();
            }
            ValueChange[5] = sound6 ? 1 : 0;
            if (ValueChange._ValueChange[5] > 0f)
            {
                waveOutDevice[5].Play();
            }
            ValueChange[6] = sound7 ? 1 : 0;
            if (ValueChange._ValueChange[6] > 0f)
            {
                waveOutDevice[6].Play();
            }
            ValueChange[7] = sound8 ? 1 : 0;
            if (ValueChange._ValueChange[7] > 0f)
            {
                waveOutDevice[7].Play();
            }
            ValueChange[8] = sound9 ? 1 : 0;
            if (ValueChange._ValueChange[8] > 0f)
            {
                waveOutDevice[8].Play();
            }
            ValueChange[9] = sound10 ? 1 : 0;
            if (ValueChange._ValueChange[9] > 0f)
            {
                waveOutDevice[9].Play();
            }
            ValueChange[10] = sound11 ? 1 : 0;
            if (ValueChange._ValueChange[10] > 0f)
            {
                waveOutDevice[10].Play();
            }
            ValueChange[11] = sound12 ? 1 : 0;
            if (ValueChange._ValueChange[11] > 0f)
            {
                waveOutDevice[11].Play();
            }
        }
    }
}