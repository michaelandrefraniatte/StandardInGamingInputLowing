using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
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
                        Network.form1.SetLabel1("rawdataavailable : " + Network.rawdataavailable);
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