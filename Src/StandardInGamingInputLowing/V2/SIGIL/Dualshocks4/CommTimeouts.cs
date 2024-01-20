using System.Runtime.InteropServices;

namespace HidHandle
{
    [StructLayout(LayoutKind.Sequential)]
    public struct CommTimeouts
    {
        public uint ReadIntervalTimeout;
        public uint ReadTotalTimeoutMultiplier;
        public uint ReadTotalTimeoutConstant;
        public uint WriteTotalTimeoutMultiplier;
        public uint WriteTotalTimeoutConstant;
    }
}