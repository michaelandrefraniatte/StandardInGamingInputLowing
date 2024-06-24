using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using Valuechanges;
using WebSocketSharp;
using WebSocketSharp.Server;

namespace Networks
{
    public class Network : IDisposable
    {
        [DllImport("winmm.dll", EntryPoint = "timeBeginPeriod")]
        private static extern uint TimeBeginPeriod(uint ms);
        [DllImport("winmm.dll", EntryPoint = "timeEndPeriod")]
        private static extern uint TimeEndPeriod(uint ms);
        [DllImport("ntdll.dll", EntryPoint = "NtSetTimerResolution")]
        private static extern void NtSetTimerResolution(uint DesiredResolution, bool SetResolution, ref uint CurrentResolution);
        private static uint CurrentResolution = 0;
        public WebSocketServer wss;
        public string rawdataavailable = "";
        public bool running = true, formvisible;
        public Form1 form1;
        public Stopwatch PollingRate;
        public double pollingrateperm = 0, pollingratetemp = 0, pollingratedisplay = 0, pollingrate;
        public string inputdelaybutton = "", inputdelay = "", inputdelaytemp = "";
        public Valuechange ValueChange;
        public double delay, elapseddown, elapsedup, elapsed;
        public bool getstate = false;
        public bool[] wd = { false };
        public bool[] wu = { false };
        private bool[] ws = { false };
        public void valchanged(int n, bool val)
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
        public void Connect(string localip, string port, int number = 0)
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
        public void Disconnect()
        {
            if (formvisible)
                if (form1.Visible)
                    form1.Close();
            running = false;
            wss.RemoveWebSocketService("/Control");
            wss.Stop();
        }
        public void ViewData(string inputdelaybutton = "")
        {
            if (!formvisible)
            {
                form1 = new Form1();
                PollingRate = new Stopwatch();
                PollingRate.Start();
                ValueChange = new Valuechange();
                this.inputdelaybutton = inputdelaybutton;
                formvisible = true;
                Task.Run(() => form1.SetVisible());
            }
        }
        public void Dispose()
        {
            GC.Collect();
            GC.WaitForPendingFinalizers();
            GC.SuppressFinalize(this);
        }
        private class Control : WebSocketBehavior
        {
            private Network network = new Network();
            protected override void OnMessage(MessageEventArgs e)
            {
                base.OnMessage(e);
                while (network.running)
                {
                    try
                    {
                        Send(network.rawdataavailable);
                        if (network.formvisible)
                        {
                            network.pollingratedisplay++;
                            network.pollingratetemp = network.pollingrateperm;
                            network.pollingrateperm = (double)network.PollingRate.ElapsedTicks / (Stopwatch.Frequency / 1000L);
                            if (network.pollingratedisplay > 300)
                            {
                                network.pollingrate = network.pollingrateperm - network.pollingratetemp;
                                network.pollingratedisplay = 0;
                            }
                            string str = "rawdataavailable : " + network.rawdataavailable + Environment.NewLine;
                            str += "PollingRate : " + network.pollingrate + " ms" + Environment.NewLine;
                            string txt = str;
                            string[] lines = txt.Split(new string[] { Environment.NewLine }, StringSplitOptions.None);
                            foreach (string line in lines)
                                if (line.Contains(network.inputdelaybutton + " : "))
                                {
                                    network.inputdelaytemp = network.inputdelay;
                                    network.inputdelay = line;
                                }
                            network.valchanged(0, network.inputdelay != network.inputdelaytemp);
                            if (network.wd[0])
                            {
                                network.getstate = true;
                            }
                            if (network.inputdelay == network.inputdelaytemp)
                                network.getstate = false;
                            if (network.getstate)
                            {
                                network.elapseddown = (double)network.PollingRate.ElapsedTicks / (Stopwatch.Frequency / 1000L);
                                network.elapsed = 0;
                            }
                            if (network.wu[0])
                            {
                                network.elapsedup = (double)network.PollingRate.ElapsedTicks / (Stopwatch.Frequency / 1000L);
                                network.elapsed = network.elapsedup - network.elapseddown;
                            }
                            network.ValueChange[0] = network.inputdelay == network.inputdelaytemp ? network.elapsed : 0;
                            if (network.ValueChange._ValueChange[0] > 0)
                            {
                                network.delay = network.ValueChange._ValueChange[0];
                            }
                            str += "InputDelay : " + network.delay + " ms" + Environment.NewLine;
                            str += Environment.NewLine;
                            network.form1.SetLabel1(str);
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
}