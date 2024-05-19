using System.Runtime.InteropServices;
using System.Threading.Tasks;
using WebSocketSharp;

namespace Networkshost
{
    public class NetworkHost
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
        public static bool running = true, formvisible;
        private static Form1 form1 = new Form1();
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
                wsc.OnMessage -= Ws_OnMessage;
                wsc.Close();
                if (running)
                {
                    System.Threading.Thread.Sleep(2000);
                    Task.Run(() => Connect(localip, port, number));
                }
            }
            catch { }
        }
        public static void Disconnect()
        {
            running = false;
            wsc.OnMessage -= Ws_OnMessage;
            wsc.Close();
        }
        public static void ViewData(string inputdelaybutton = "")
        {
            if (!formvisible)
            {
                formvisible = true;
                Task.Run(() => form1.SetVisible());
            }
        }
        private static void Ws_OnMessage(object sender, MessageEventArgs e)
        {
            rawdataavailable = e.Data;
            if (formvisible)
            {
                form1.SetLabel1("rawdataavailable : " + rawdataavailable);
            }
        }
    }
}