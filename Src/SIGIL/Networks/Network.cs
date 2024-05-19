using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using Valuechanges;
using WebSocketSharp;
using WebSocketSharp.Server;

namespace Networks
{
    public class Network
    {
        [DllImport("winmm.dll", EntryPoint = "timeBeginPeriod")]
        private static extern uint TimeBeginPeriod(uint ms);
        [DllImport("winmm.dll", EntryPoint = "timeEndPeriod")]
        private static extern uint TimeEndPeriod(uint ms);
        [DllImport("ntdll.dll", EntryPoint = "NtSetTimerResolution")]
        private static extern void NtSetTimerResolution(uint DesiredResolution, bool SetResolution, ref uint CurrentResolution);
        private static uint CurrentResolution = 0;
        public static WebSocketServer wss;
        public static string rawdataavailable = "";
        public static bool running = true, formvisible;
        public static Form1 form1 = new Form1();
        public static Stopwatch PollingRate;
        public static double pollingrateperm = 0, pollingratetemp = 0, pollingratedisplay = 0, pollingrate;
        public static string inputdelaybutton = "", inputdelay = "", inputdelaytemp = "";
        public static Valuechange ValueChange;
        public static double delay, elapseddown, elapsedup, elapsed;
        public static bool getstate = false;
        public static int[] wd = { 2 };
        public static int[] wu = { 2 };
        public static void valchanged(int n, bool val)
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
        public static void Connect(string localip, string port, int number = 0)
        {
            try
            {
                TimeBeginPeriod(1);
                NtSetTimerResolution(1, true, ref CurrentResolution);
                String connectionString = "ws://" + localip + ":" + port;
                wss = new WebSocketServer(connectionString);
                wss.AddWebSocketService<Control>("/Control");
                wss.Start();
            }
            catch { }
        }
        public static void Disconnect()
        {
            running = false;
            wss.RemoveWebSocketService("/Control");
            wss.Stop();
        }
        public static void ViewData(string inputdelaybutton = "")
        {
            if (!formvisible)
            {
                PollingRate = new Stopwatch();
                PollingRate.Start();
                ValueChange = new Valuechange();
                Network.inputdelaybutton = inputdelaybutton;
                formvisible = true;
                Task.Run(() => form1.SetVisible());
            }
        }
    }
    public class Control : WebSocketBehavior
    {
        protected override void OnMessage(MessageEventArgs e)
        {
            base.OnMessage(e);
            while (Network.running)
            {
                try
                {
                    Send(Network.rawdataavailable);
                    if (Network.formvisible)
                    {
                        Network.pollingratedisplay++;
                        Network.pollingratetemp = Network.pollingrateperm;
                        Network.pollingrateperm = (double)Network.PollingRate.ElapsedTicks / (Stopwatch.Frequency / 1000L);
                        if (Network.pollingratedisplay > 300)
                        {
                            Network.pollingrate = Network.pollingrateperm - Network.pollingratetemp;
                            Network.pollingratedisplay = 0;
                        }
                        string str = "rawdataavailable : " + Network.rawdataavailable + Environment.NewLine;
                        str += "PollingRate : " + Network.pollingrate + " ms" + Environment.NewLine;
                        string txt = str;
                        string[] lines = txt.Split(new string[] { Environment.NewLine }, StringSplitOptions.None);
                        foreach (string line in lines)
                            if (line.Contains(Network.inputdelaybutton + " : "))
                            {
                                Network.inputdelaytemp = Network.inputdelay;
                                Network.inputdelay = line;
                            }
                        Network.valchanged(0, Network.inputdelay != Network.inputdelaytemp);
                        if (Network.wd[0] == 1)
                        {
                            Network.getstate = true;
                        }
                        if (Network.inputdelay == Network.inputdelaytemp)
                            Network.getstate = false;
                        if (Network.getstate)
                        {
                            Network.elapseddown = (double)Network.PollingRate.ElapsedTicks / (Stopwatch.Frequency / 1000L);
                            Network.elapsed = 0;
                        }
                        if (Network.wu[0] == 1)
                        {
                            Network.elapsedup = (double)Network.PollingRate.ElapsedTicks / (Stopwatch.Frequency / 1000L);
                            Network.elapsed = Network.elapsedup - Network.elapseddown;
                        }
                        Network.ValueChange[0] = Network.inputdelay == Network.inputdelaytemp ? Network.elapsed : 0;
                        if (Network.ValueChange._ValueChange[0] > 0)
                        {
                            Network.delay = Network.ValueChange._ValueChange[0];
                        }
                        str += "InputDelay : " + Network.delay + " ms" + Environment.NewLine;
                        str += Environment.NewLine;
                        Network.form1.SetLabel1(str);
                    }
                }
                catch
                {
                    System.Threading.Thread.Sleep(1);
                }
                System.Threading.Thread.Sleep(1);
            }
        }
    }
}