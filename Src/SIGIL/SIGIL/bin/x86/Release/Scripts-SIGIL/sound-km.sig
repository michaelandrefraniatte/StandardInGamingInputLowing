using System;
using System.Globalization;
using System.IO;
using System.Drawing;
using System.Numerics;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;
using System.Reflection;
using System.Diagnostics;
using Sound;
using MouseInputsAPI;
using KeyboardInputsAPI;
namespace StringToCode
{
    public class FooClass 
    { 
        [DllImport("winmm.dll", EntryPoint = "timeBeginPeriod")]
        private static extern uint TimeBeginPeriod(uint ms);
        [DllImport("winmm.dll", EntryPoint = "timeEndPeriod")]
        private static extern uint TimeEndPeriod(uint ms);
        [DllImport("ntdll.dll", EntryPoint = "NtSetTimerResolution")]
        private static extern void NtSetTimerResolution(uint DesiredResolution, bool SetResolution, ref uint CurrentResolution);
        private static uint CurrentResolution = 0;
        private static bool running;
        private static string pathsound1 = @"sounds\sound1.mp3", pathtempsound1 = @"sounds\tempsound1.mp3";
        private static bool sound1 = false, tempsound1 = false;
        private static int width = System.Windows.Forms.Screen.PrimaryScreen.Bounds.Width, height = System.Windows.Forms.Screen.PrimaryScreen.Bounds.Height;
        private static double statex = 0f, statey = 0f, mousex = 0f, mousey = 0f, mousestatex = 0f, mousestatey = 0f, dzx = 0.0f, dzy = 0.0f, viewpower1x = 0f, viewpower2x = 1f, viewpower3x = 0f, viewpower1y = 0.25f, viewpower2y = 0.75f, viewpower3y = 0f, viewpower05x = 0f, viewpower05y = 0f;
        private static bool[] getstate = new bool[12];
        private static int sleeptime = 1;
        private Player player = new Player();
        private MouseInput mi = new MouseInput();
        private KeyboardInput ki = new KeyboardInput();
        private static int[] wd = { 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2 };
        private static int[] wu = { 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2 };
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
        public void Close()
        {
            try
            {
                running = false;
                Thread.Sleep(100);
                mi.Close();
                ki.Close();
                player.Disconnect();
            }
            catch { }
        }
        public static void Main() {}
        public void Load()
        {
            TimeBeginPeriod(1);
            NtSetTimerResolution(1, true, ref CurrentResolution);
            Task.Run(() => Start());
        }
        private void Start()
        {
            running = true;
            mi.Scan();
            ki.Scan();
            mi.BeginPolling();
            ki.BeginPolling();
            player.Connect(pathsound1, "", "", "", "", "", "", "", "", "", "", "", pathtempsound1, "", "", "", "", "", "", "", "", "", "", "");
            Task.Run(() => task());
        }
        private void task()
        {
            for (; ; )
            {
                if (!running)
                    break;
                sound1     = ki.KeyboardKeyA;
                tempsound1 = ki.KeyboardKeyS;
                player.Set(sound1, false, false, false, false, false, false, false, false, false, false, false, tempsound1, false, false, false, false, false, false, false, false, false, false, false);
                /*player.ViewData();*/
                /*mi.ViewData();*/
                /*ki.ViewData();*/
                Thread.Sleep(sleeptime);
            }
        }
    }
}