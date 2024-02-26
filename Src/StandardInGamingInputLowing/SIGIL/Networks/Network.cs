using System;
using System.Runtime.InteropServices;
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
        public static byte[] rawdataavailable;
        public static bool running = false;
        public static void Connect(string localip, string port, int number)
        {
            try
            {
                TimeBeginPeriod(1);
                NtSetTimerResolution(1, true, ref CurrentResolution);
                running = true;
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