using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using Speech;

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
        public SpeechToText()
        {
            TimeBeginPeriod(1);
            NtSetTimerResolution(1, true, ref CurrentResolution);
            running = true;
        }
        public void ViewData()
        {
            if (!form1.Visible)
            {
                PollingRate = new Stopwatch();
                PollingRate.Start();
                formvisible = true;
                form1.SetVisible();
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
                    pollingrateperm = PollingRate.ElapsedMilliseconds;
                    if (pollingratedisplay > 300)
                    {
                        pollingrate = pollingrateperm - pollingratetemp;
                        pollingratedisplay = 0;
                    }
                    string str = "speechtext : " + speechtext + Environment.NewLine;
                    str += "PollingRate : " + pollingrate + " ms" + Environment.NewLine;
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