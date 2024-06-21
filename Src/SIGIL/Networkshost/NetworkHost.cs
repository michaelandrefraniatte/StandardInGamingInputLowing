using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using Valuechanges;
using WebSocketSharp;

namespace Networkshost
{
    public class NetworkHost : IDisposable
    {
        [DllImport("winmm.dll", EntryPoint = "timeBeginPeriod")]
        private static extern uint TimeBeginPeriod(uint ms);
        [DllImport("winmm.dll", EntryPoint = "timeEndPeriod")]
        private static extern uint TimeEndPeriod(uint ms);
        [DllImport("ntdll.dll", EntryPoint = "NtSetTimerResolution")]
        private static extern void NtSetTimerResolution(uint DesiredResolution, bool SetResolution, ref uint CurrentResolution);
        private static uint CurrentResolution = 0;
        public static WebSocket wsc;
        public static string rawdataavailable = "";
        private static bool running = true, formvisible;
        private static Form1 form1;
        private static Stopwatch PollingRate;
        private static double pollingrateperm = 0, pollingratetemp = 0, pollingratedisplay = 0, pollingrate;
        private static string inputdelaybutton = "", inputdelay = "", inputdelaytemp = "";
        private static Valuechange ValueChange;
        private static double delay, elapseddown, elapsedup, elapsed;
        private static bool getstate = false;
        private static bool[] wd = { false };
        private static bool[] wu = { false };
        private static bool[] ws = { false };
        private static void valchanged(int n, bool val)
        {
            if (val)
            {
                if (!wd[n] & !ws[n])
                {
                    wd[n] = true;
                    ws[n] = true;
                    return;
                }
                if (wd[n] & ws[n])
                {
                    wd[n] = false;
                }
                ws[n] = true;
                wu[n] = false;
            }
            if (!val)
            {
                if (!wu[n] & ws[n])
                {
                    wu[n] = true;
                    ws[n] = false;
                    return;
                }
                if (wu[n] & !ws[n])
                {
                    wu[n] = false;
                }
                ws[n] = false;
                wd[n] = false;
            }
        }
        public static void Connect(string localip, string port, int number = 0)
        {
            TimeBeginPeriod(1);
            NtSetTimerResolution(1, true, ref CurrentResolution);
            Task.Run(() => taskN(localip, port, number));
        }
        private static void taskN(string localip, string port, int number = 0)
        {
            string connectionString = "ws://" + localip + ":" + port + "/Control";
            wsc = new WebSocket(connectionString);
            wsc.OnMessage += Ws_OnMessage;
            while (!wsc.IsAlive & running)
            {
                try
                {
                    wsc.Connect();
                    wsc.Send("Hello from client");
                }
                catch { }
                System.Threading.Thread.Sleep(1);
            }
            while (wsc.IsAlive & running)
            {
                System.Threading.Thread.Sleep(1);
            }
            try
            {
                System.Threading.Thread.Sleep(2000);
                if (running)
                {
                    try
                    {
                        wsc.OnMessage -= Ws_OnMessage;
                        wsc.Close();
                    }
                    catch { }
                    Task.Run(() => Connect(localip, port, number));
                }
            }
            catch { }
        }
        public static void Disconnect()
        {
            running = false;
            try
            {
                wsc.OnMessage -= Ws_OnMessage;
                wsc.Close();
            }
            catch { }
        }
        public static void ViewData(string inputdelaybutton = "")
        {
            if (!formvisible)
            {
                form1 = new Form1();
                PollingRate = new Stopwatch();
                PollingRate.Start();
                ValueChange = new Valuechange();
                NetworkHost.inputdelaybutton = inputdelaybutton;
                formvisible = true;
                Task.Run(() => form1.SetVisible());
            }
        }
        private static void Ws_OnMessage(object sender, MessageEventArgs e)
        {
            rawdataavailable = e.Data;
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
                string str = "rawdataavailable : " + rawdataavailable + Environment.NewLine;
                str += "PollingRate : " + pollingrate + " ms" + Environment.NewLine;
                string txt = str;
                string[] lines = txt.Split(new string[] { Environment.NewLine }, StringSplitOptions.None);
                foreach (string line in lines)
                    if (line.Contains(inputdelaybutton + " : "))
                    {
                        inputdelaytemp = inputdelay;
                        inputdelay = line;
                    }
                valchanged(0, inputdelay != inputdelaytemp);
                if (wd[0])
                {
                    getstate = true;
                }
                if (inputdelay == inputdelaytemp)
                    getstate = false;
                if (getstate)
                {
                    elapseddown = (double)PollingRate.ElapsedTicks / (Stopwatch.Frequency / 1000L);
                    elapsed = 0;
                }
                if (wu[0])
                {
                    elapsedup = (double)PollingRate.ElapsedTicks / (Stopwatch.Frequency / 1000L);
                    elapsed = elapsedup - elapseddown;
                }
                ValueChange[0] = inputdelay == inputdelaytemp ? elapsed : 0;
                if (ValueChange._ValueChange[0] > 0)
                {
                    delay = ValueChange._ValueChange[0];
                }
                str += "InputDelay : " + delay + " ms" + Environment.NewLine;
                str += Environment.NewLine;
                form1.SetLabel1(str);
            }
        }
        public void Dispose()
        {
            GC.Collect();
            GC.SuppressFinalize(this);
        }
    }
}