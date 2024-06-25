using Microsoft.Win32.SafeHandles;
using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Diagnostics;
using Valuechanges;

namespace Readhids
{
    public class ReadHid : IDisposable
    {
        [DllImport("hidread.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "hid_read_timeout")]
        private static extern int hid_read_timeout(SafeFileHandle dev, byte[] data, UIntPtr length);
        [DllImport("hidread.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "hid_write")]
        private static extern int hid_write(SafeFileHandle device, byte[] data, UIntPtr length);
        [DllImport("hidread.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "hid_open_path")]
        private static extern SafeFileHandle hid_open_path(IntPtr handle);
        [DllImport("hidread.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "hid_close")]
        private static extern void hid_close(SafeFileHandle device);
        [DllImport("hid.dll")]
        private static extern void HidD_GetHidGuid(out Guid gHid);
        [DllImport("hid.dll")]
        private extern static bool HidD_SetOutputReport(IntPtr HidDeviceObject, byte[] lpReportBuffer, uint ReportBufferLength);
        [DllImport("setupapi.dll")]
        private static extern IntPtr SetupDiGetClassDevs(ref Guid ClassGuid, string Enumerator, IntPtr hwndParent, UInt32 Flags);
        [DllImport("setupapi.dll")]
        private static extern Boolean SetupDiEnumDeviceInterfaces(IntPtr hDevInfo, IntPtr devInvo, ref Guid interfaceClassGuid, Int32 memberIndex, ref SP_DEVICE_INTERFACE_DATA deviceInterfaceData);
        [DllImport("setupapi.dll")]
        private static extern Boolean SetupDiGetDeviceInterfaceDetail(IntPtr hDevInfo, ref SP_DEVICE_INTERFACE_DATA deviceInterfaceData, IntPtr deviceInterfaceDetailData, UInt32 deviceInterfaceDetailDataSize, out UInt32 requiredSize, IntPtr deviceInfoData);
        [DllImport("setupapi.dll")]
        private static extern Boolean SetupDiGetDeviceInterfaceDetail(IntPtr hDevInfo, ref SP_DEVICE_INTERFACE_DATA deviceInterfaceData, ref SP_DEVICE_INTERFACE_DETAIL_DATA deviceInterfaceDetailData, UInt32 deviceInterfaceDetailDataSize, out UInt32 requiredSize, IntPtr deviceInfoData);
        [DllImport("Kernel32.dll")]
        private static extern SafeFileHandle CreateFile(string fileName, [MarshalAs(UnmanagedType.U4)] FileAccess fileAccess, [MarshalAs(UnmanagedType.U4)] FileShare fileShare, IntPtr securityAttributes, [MarshalAs(UnmanagedType.U4)] FileMode creationDisposition, [MarshalAs(UnmanagedType.U4)] uint flags, IntPtr template);
        [DllImport("Kernel32.dll")]
        private static extern IntPtr CreateFile(string fileName, System.IO.FileAccess fileAccess, System.IO.FileShare fileShare, IntPtr securityAttributes, System.IO.FileMode creationDisposition, EFileAttributes flags, IntPtr template);
        [DllImport("winmm.dll", EntryPoint = "timeBeginPeriod")]
        private static extern uint TimeBeginPeriod(uint ms);
        [DllImport("winmm.dll", EntryPoint = "timeEndPeriod")]
        private static extern uint TimeEndPeriod(uint ms);
        [DllImport("ntdll.dll", EntryPoint = "NtSetTimerResolution")]
        private static extern void NtSetTimerResolution(uint DesiredResolution, bool SetResolution, ref uint CurrentResolution);
        private static uint CurrentResolution = 0;
        private uint report_len;
        private byte[] report_buf;
        private SafeFileHandle handle;
        private IntPtr handleptr, handleptrunshared;
        private bool running, formvisible;
        private bool isvalidhandle = false;
        private int number;
        private bool reconnectingbool;
        private double reconnectingcount;
        private string path;
        private static List<string> paths = new List<string>();
        private static List<SafeFileHandle> handles = new List<SafeFileHandle>();
        private Form1 form1;
        private Stopwatch PollingRate;
        private double pollingrateperm = 0, pollingratetemp = 0, pollingratedisplay = 0, pollingrate;
        private string inputdelaybutton = "", inputdelay = "", inputdelaytemp = "";
        public Valuechange ValueChange;
        private double delay, elapseddown, elapsedup, elapsed;
        private bool getstate = false;
        private bool[] wd = { false };
        private bool[] wu = { false };
        private bool[] ws = { false };
        private void valchanged(int n, bool val)
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
        public ReadHid()
        {
            TimeBeginPeriod(1);
            NtSetTimerResolution(1, true, ref CurrentResolution);
            running = true;
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
        public void Close()
        {
            if (formvisible)
                if (form1.Visible)
                    form1.Close();
            running = false;
            Thread.Sleep(100);
            hid_close(handle);
            handle.Close();
            handle.Dispose();
        }
        private void taskDLeft()
        {
            for (; ; )
            {
                if (!running)
                    break;
                try
                {
                    hid_read_timeout(handle, report_buf, (UIntPtr)report_len);
                    reconnectingbool = false;
                }
                catch { Thread.Sleep(10); }
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
                    string str = "";
                    for (int i = 0; i < report_len; i++)
                    {
                        str += "line " + i + 1 + " : " + report_buf[i] + Environment.NewLine;
                    }
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
                    str += "InputDelay : " + delay / 2f + " ms" + Environment.NewLine;
                    str += Environment.NewLine;
                    form1.SetLabel1(str);
                }
            }
        }
        private void taskPLeft()
        {
            for (; ; )
            {
                if (!running)
                    break;
                Reconnection();
                Thread.Sleep(1);
            }
        }
        public void BeginPolling()
        {
            Task.Run(() => taskDLeft());
            Task.Run(() => taskPLeft());
        }
        private void Reconnection()
        {
            if (reconnectingcount == 0)
                reconnectingbool = true;
            reconnectingcount++;
            if (reconnectingcount >= 150f)
            {
                if (reconnectingbool)
                {
                    AttachJoyLeft(path);
                    reconnectingcount = -150f;
                }
                else
                    reconnectingcount = 0;
            }
        }
        private enum EFileAttributes : uint
        {
            Overlapped = 0x40000000,
            Normal = 0x80
        };
        private struct SP_DEVICE_INTERFACE_DATA
        {
            public int cbSize;
            public Guid InterfaceClassGuid;
            public int Flags;
            public IntPtr RESERVED;
        }
        private struct SP_DEVICE_INTERFACE_DETAIL_DATA
        {
            public UInt32 cbSize;
            [System.Runtime.InteropServices.MarshalAs(System.Runtime.InteropServices.UnmanagedType.ByValTStr, SizeConst = 256)]
            public string DevicePath;
        }
        public void Scan(string vendor_id, string product_id, uint length, int number = 0)
        {
            this.number = number;
            this.report_len = length;
            this.report_buf = new byte[length];
            if (number <= 1)
            {
                int index = 0;
                System.Guid guid;
                HidD_GetHidGuid(out guid);
                System.IntPtr hDevInfo = SetupDiGetClassDevs(ref guid, null, new System.IntPtr(), 0x00000010);
                SP_DEVICE_INTERFACE_DATA diData = new SP_DEVICE_INTERFACE_DATA();
                diData.cbSize = System.Runtime.InteropServices.Marshal.SizeOf(diData);
                while (SetupDiEnumDeviceInterfaces(hDevInfo, new System.IntPtr(), ref guid, index, ref diData))
                {
                    System.UInt32 size;
                    SetupDiGetDeviceInterfaceDetail(hDevInfo, ref diData, new System.IntPtr(), 0, out size, new System.IntPtr());
                    SP_DEVICE_INTERFACE_DETAIL_DATA diDetail = new SP_DEVICE_INTERFACE_DETAIL_DATA();
                    diDetail.cbSize = 5;
                    if (SetupDiGetDeviceInterfaceDetail(hDevInfo, ref diData, ref diDetail, size, out size, new System.IntPtr()))
                    {
                        if (diDetail.DevicePath.Contains(vendor_id) & diDetail.DevicePath.Contains(product_id))
                        {
                            path = diDetail.DevicePath;
                            isvalidhandle = AttachJoyLeft(diDetail.DevicePath);
                            handleptrunshared = CreateFile(path, System.IO.FileAccess.ReadWrite, System.IO.FileShare.None, new System.IntPtr(), System.IO.FileMode.Open, EFileAttributes.Normal, new System.IntPtr());
                            if (isvalidhandle)
                            {
                                paths.Add(path);
                                handles.Add(handle);
                            }
                        }
                    }
                    index++;
                }
            }
            path = paths[number < 2 ? 0 : number - 1];
            handle = handles[number < 2 ? 0 : number - 1];
        }
        private bool AttachJoyLeft(string path)
        {
            try
            {
                handleptr = CreateFile(path, System.IO.FileAccess.ReadWrite, System.IO.FileShare.ReadWrite, new System.IntPtr(), System.IO.FileMode.Open, EFileAttributes.Normal, new System.IntPtr());
                handle = hid_open_path(handleptr);
                return true;
            }
            catch { return false; }
        }
        public void Dispose()
        {
            GC.Collect();
            GC.WaitForPendingFinalizers();
            GC.SuppressFinalize(this);
        }
    }
}