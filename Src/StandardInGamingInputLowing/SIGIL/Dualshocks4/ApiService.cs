using Microsoft.Win32.SafeHandles;
using System;
using System.IO;

namespace HidHandle
{
    internal class ApiService
    {
        private const uint FILE_FLAG_OVERLAPPED = 0x40000000;
        public static SafeFileHandle CreateReadConnection(string deviceId, FileAccess desiredAccess) => CreateConnection(deviceId, desiredAccess, FileShare.Read | FileShare.Write, FileMode.Open);
        private static SafeFileHandle CreateConnection(string deviceId, FileAccess desiredAccess, FileShare shareMode, FileMode creationDisposition)
        {
            return WindowsDeviceEnumerator.CreateFile(deviceId, desiredAccess, shareMode, IntPtr.Zero, creationDisposition, FILE_FLAG_OVERLAPPED, IntPtr.Zero);
        }
    }
}