using System.Diagnostics;
using System.Runtime.InteropServices;

namespace Valuechangesdelay
{
    public class Valuechangedelay
    {
        [DllImport("winmm.dll", EntryPoint = "timeBeginPeriod")]
        private static extern uint TimeBeginPeriod(uint ms);
        [DllImport("winmm.dll", EntryPoint = "timeEndPeriod")]
        private static extern uint TimeEndPeriod(uint ms);
        [DllImport("ntdll.dll", EntryPoint = "NtSetTimerResolution")]
        private static extern void NtSetTimerResolution(uint DesiredResolution, bool SetResolution, ref uint CurrentResolution);
        private static uint CurrentResolution = 0;
        private static Stopwatch watch = new Stopwatch();
        public double[] _valuechange = { 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2 };
        public double[] _ValueChange = { 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2 };
        public double[] _valuechangedelay = { 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2 };
        public double[] _ValueChangedelay = { 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2 };
        public Valuechangedelay()
        {
            TimeBeginPeriod(1);
            NtSetTimerResolution(1, true, ref CurrentResolution);
            watch = new Stopwatch();
            watch.Start();
        }
        public double this[int index]
        {
            get { return _ValueChange[index]; }
            set
            {
                if (_valuechange[index] != value)
                {
                    _ValueChange[index] = value - _valuechange[index];
                    _ValueChangedelay[index] = (double)watch.ElapsedTicks / (Stopwatch.Frequency / 1000L) - _valuechangedelay[index];
                }
                else
                {
                    _ValueChange[index] = 0;
                    _ValueChangedelay[index] = 0;
                }
                _valuechange[index] = value;
                _valuechangedelay[index] = (double)watch.ElapsedTicks / (Stopwatch.Frequency / 1000L);
            }
        }
    }
}