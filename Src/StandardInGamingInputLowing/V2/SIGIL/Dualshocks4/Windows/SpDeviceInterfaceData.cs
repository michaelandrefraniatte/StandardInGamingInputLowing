using System;
using System.Runtime.InteropServices;

namespace DeviceHandle.Windows
{
    [StructLayout(LayoutKind.Sequential)]
#pragma warning disable CA1815 // Override equals and operator equals on value types
    internal struct SpDeviceInterfaceData
#pragma warning restore CA1815 // Override equals and operator equals on value types
    {
        public uint CbSize;
        public Guid InterfaceClassGuid;
        public uint Flags;
        public IntPtr Reserved;
    }
}