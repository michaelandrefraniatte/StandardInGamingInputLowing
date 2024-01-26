using Microsoft.Win32.SafeHandles;
using System;
using System.IO;
using System.Runtime.InteropServices;

namespace HidHandle
{
    internal class ApiService
    {
        private const uint FILE_FLAG_OVERLAPPED = 0x40000000;

        public static SafeFileHandle CreateWriteConnection(string deviceId) => CreateConnection(deviceId, FileAccess.Read | FileAccess.Write, FileShare.Read | FileShare.Write, FileMode.Open);

        public static SafeFileHandle CreateReadConnection(string deviceId, FileAccess desiredAccess) => CreateConnection(deviceId, desiredAccess, FileShare.Read | FileShare.Write, FileMode.Open);

        public bool AGetCommState(SafeFileHandle hFile, ref Dcb lpDCB) => GetCommState(hFile, ref lpDCB);
        public bool APurgeComm(SafeFileHandle hFile, int dwFlags) => PurgeComm(hFile, dwFlags);
        public bool ASetCommTimeouts(SafeFileHandle hFile, ref CommTimeouts lpCommTimeouts) => SetCommTimeouts(hFile, ref lpCommTimeouts);
        public bool AWriteFile(SafeFileHandle hFile, byte[] lpBuffer, int nNumberOfBytesToWrite, out int lpNumberOfBytesWritten, int lpOverlapped) => WriteFile(hFile, lpBuffer, nNumberOfBytesToWrite, out lpNumberOfBytesWritten, lpOverlapped);
        public bool AReadFile(SafeFileHandle hFile, byte[] lpBuffer, int nNumberOfBytesToRead, out uint lpNumberOfBytesRead, int lpOverlapped) => ReadFile(hFile, lpBuffer, nNumberOfBytesToRead, out lpNumberOfBytesRead, lpOverlapped);
        public bool ASetCommState(SafeFileHandle hFile, [In] ref Dcb lpDCB) => SetCommState(hFile, ref lpDCB);
        
        private static SafeFileHandle CreateConnection(string deviceId, FileAccess desiredAccess, FileShare shareMode, FileMode creationDisposition)
        {
            return WindowsDeviceEnumerator.CreateFile(deviceId, desiredAccess, shareMode, IntPtr.Zero, creationDisposition, FILE_FLAG_OVERLAPPED, IntPtr.Zero);
        }

        [DllImport("kernel32.dll", SetLastError = true)]
        private static extern bool PurgeComm(SafeFileHandle hFile, int dwFlags);
        [DllImport("kernel32.dll")]
        private static extern bool GetCommState(SafeFileHandle hFile, ref Dcb lpDCB);
        [DllImport("kernel32.dll", SetLastError = true)]
        private static extern bool SetCommTimeouts(SafeFileHandle hFile, ref CommTimeouts lpCommTimeouts);
        [DllImport("kernel32.dll")]
        private static extern bool SetCommState(SafeFileHandle hFile, [In] ref Dcb lpDCB);
        [DllImport("kernel32.dll", SetLastError = true)]
        private static extern bool WriteFile(SafeFileHandle hFile, byte[] lpBuffer, int nNumberOfBytesToWrite, out int lpNumberOfBytesWritten, int lpOverlapped);
        [DllImport("kernel32.dll")]
        private static extern bool ReadFile(SafeFileHandle hFile, byte[] lpBuffer, int nNumberOfBytesToRead, out uint lpNumberOfBytesRead, int lpOverlapped);

        /// <summary>
        /// Defines the control setting for a serial communications device.
        /// </summary>
        [StructLayout(LayoutKind.Sequential)]
        public struct Dcb
        {
            public int DCBlength;
            public uint BaudRate;
            public uint Flags;
            public ushort wReserved;
            public ushort XonLim;
            public ushort XoffLim;
            public byte ByteSize;
            public byte Parity;
            public byte StopBits;
            public sbyte XonChar;
            public sbyte XoffChar;
            public sbyte ErrorChar;
            public sbyte EofChar;
            public sbyte EvtChar;
            public ushort wReserved1;
            public uint fBinary;
            public uint fParity;
            public uint fOutxCtsFlow;
            public uint fOutxDsrFlow;
            public uint fDtrControl;
            public uint fDsrSensitivity;
            public uint fTXContinueOnXoff;
            public uint fOutX;
            public uint fInX;
            public uint fErrorChar;
            public uint fNull;
            public uint fRtsControl;
            public uint fAbortOnError;
        }
    }
}