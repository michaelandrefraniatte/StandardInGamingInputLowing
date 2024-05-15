using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using Speech;
using Valuechanges;

namespace SpeechAPI
{
    public class SpeechToText
    {
        [DllImport("winmm.dll", EntryPoint = "timeBeginPeriod")]
        private static extern uint TimeBeginPeriod(uint ms);
        [DllImport("winmm.dll", EntryPoint = "timeEndPeriod")]
        private static extern uint TimeEndPeriod(uint ms);
        [DllImport("ntdll.dll", EntryPoint = "NtSetTimerResolution")]
        private static extern void NtSetTimerResolution(uint DesiredResolution, bool SetResolution, ref uint CurrentResolution);
        private static uint CurrentResolution = 0;
        private bool running, formvisible;
        private static Ozeki.Media.Microphone microphone;
        private static Ozeki.Media.MediaConnector connector;
        private static Ozeki.Media.SpeechToText speechToText;
        private static string TextFromSpeech;
        public string speechtext;
        private int number;
        private Form1 form1 = new Form1();
        private Stopwatch PollingRate;
        private double pollingrateperm = 0, pollingratetemp = 0, pollingratedisplay = 0, pollingrate;
        private string inputdelaybutton = "", inputdelay = "";
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
        public SpeechToText()
        {
            TimeBeginPeriod(1);
            NtSetTimerResolution(1, true, ref CurrentResolution);
            running = true;
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
        public void Close()
        {
            running = false;
            Thread.Sleep(100); 
            StopSpeechToText();
        }
        public void BeginPolling()
        {
            Task.Run(() => taskD());
        }
        private void taskD()
        {
            for (; ; )
            {
                if (!running)
                    break;
                speechtext = TextFromSpeech;
                Thread.Sleep(10);
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
                    string str = "speechtext : " + speechtext + Environment.NewLine;
                    str += "PollingRate : " + pollingrate + " ms" + Environment.NewLine;
                    string txt = str;
                    string[] lines = txt.Split(new string[] { Environment.NewLine }, StringSplitOptions.None);
                    foreach (string line in lines)
                        if (line.Contains(inputdelaybutton + " : "))
                            inputdelay = line;
                    valchanged(0, inputdelay.Contains("True"));
                    if (wd[0] == 1)
                    {
                        getstate = true;
                    }
                    if (inputdelay.Contains("False"))
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
                    ValueChange[0] = inputdelay.Contains("False") ? elapsed : 0;
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
        public void Init()
        {
        }
        public void Scan(string[] SpeechToText, int number = 0)
        {
            this.number = number;
            if (SpeechToText.Length != 0)
            {
                microphone = Ozeki.Media.Microphone.GetDefaultDevice();
                connector = new Ozeki.Media.MediaConnector();
                SetupSpeechToText(SpeechToText);
            }
        }
        private static void SetupSpeechToText(string[] speechwords)
        {
            speechToText = Ozeki.Media.SpeechToText.CreateInstance(speechwords);
            speechToText.WordRecognized += SpeechToText_WordsRecognized;
            connector.Connect(microphone, speechToText);
            microphone.Start();
        }
        private static void SpeechToText_WordsRecognized(object sender, Ozeki.Media.SpeechDetectionEventArgs e)
        {
            TextFromSpeech = e.Word;
            Thread.Sleep(100);
            TextFromSpeech = "";
        }
        private static void StopSpeechToText()
        {
            try
            {
                speechToText.WordRecognized -= SpeechToText_WordsRecognized;
            }
            catch { }
            try
            {
                connector.Disconnect(microphone, speechToText);
            }
            catch { }
            try
            {
                speechToText.Dispose();
            }
            catch { }
            try
            {
                microphone.Stop();
            }
            catch { }
        }
    }
}