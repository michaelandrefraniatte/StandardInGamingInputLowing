using Microsoft.Win32.SafeHandles;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Runtime.InteropServices;
using System.Threading.Tasks;

namespace HidHandle
{
    public delegate Task<bool> IsMatch(ConnectedDeviceDefinition connectedDeviceDefinition);

    public class WindowsDeviceEnumerator
    {
        private readonly Guid _classGuid;
        private readonly GetDeviceDefinition _getDeviceDefinition;
        private readonly IsMatch _isMatch;

        //TODO: Inject a windows API abstraction here for unit testing purposes

        public WindowsDeviceEnumerator(
            Guid classGuid,
            GetDeviceDefinition getDeviceDefinition,
            IsMatch isMatch
            )
        {
            _classGuid = classGuid;
            _getDeviceDefinition = getDeviceDefinition;
            _isMatch = isMatch;
        }

        public Task<IEnumerable<ConnectedDeviceDefinition>> GetConnectedDeviceDefinitionsAsync()
            => Task.Run<IEnumerable<ConnectedDeviceDefinition>>(async () =>
            {
                try
                {

                    var deviceDefinitions = new List<ConnectedDeviceDefinition>();
                    var spDeviceInterfaceData = new SpDeviceInterfaceData();
                    var spDeviceInfoData = new SpDeviceInfoData();
                    var spDeviceInterfaceDetailData = new SpDeviceInterfaceDetailData();
                    spDeviceInterfaceData.CbSize = (uint)Marshal.SizeOf(spDeviceInterfaceData);
                    spDeviceInfoData.CbSize = (uint)Marshal.SizeOf(spDeviceInfoData);

                    const int flags = DigcfDeviceinterface | DigcfPresent;

                    var copyOfClassGuid = new Guid(_classGuid.ToString());

                    var devicesHandle = SetupDiGetClassDevs(ref copyOfClassGuid, IntPtr.Zero, IntPtr.Zero, flags);

                    spDeviceInterfaceDetailData.CbSize = IntPtr.Size == 8 ? 8 : 4 + Marshal.SystemDefaultCharSize;

                    var i = -1;

                    while (true)
                    {
                        try
                        {
                            i++;

                            var isSuccess = SetupDiEnumDeviceInterfaces(devicesHandle, IntPtr.Zero,
                                ref copyOfClassGuid, (uint)i, ref spDeviceInterfaceData);
                            if (!isSuccess)
                            {
                                var errorCode = Marshal.GetLastWin32Error();

                                if (errorCode == ERROR_NO_MORE_ITEMS)
                                {
                                    break;
                                }
                            }

                            isSuccess = SetupDiGetDeviceInterfaceDetail(devicesHandle,
                                ref spDeviceInterfaceData, ref spDeviceInterfaceDetailData, 256, out _,
                                ref spDeviceInfoData);
                            if (!isSuccess)
                            {
                                var errorCode = Marshal.GetLastWin32Error();

                                if (errorCode == ERROR_NO_MORE_ITEMS)
                                {
                                    break;
                                }
                            }

                            var connectedDeviceDefinition = _getDeviceDefinition(spDeviceInterfaceDetailData.DevicePath, copyOfClassGuid);

                            if (connectedDeviceDefinition == null)
                            {
                                continue;
                            }

                            if (!await _isMatch(connectedDeviceDefinition)) continue;

                            deviceDefinitions.Add(connectedDeviceDefinition);
                        }
                        catch { }
                    }

                    _ = SetupDiDestroyDeviceInfoList(devicesHandle);

                    return new ReadOnlyCollection<ConnectedDeviceDefinition>(deviceDefinitions);
                }
                catch
                {
                    throw;
                }
            });

        public const int DigcfDeviceinterface = 16;
        public const int DigcfPresent = 2;

        public const int FileAttributeNormal = 128;
        public const int FileFlagOverlapped = 1073741824;

        public const int ERROR_NO_MORE_ITEMS = 259;

        public const int PURGE_TXCLEAR = 0x0004;
        public const int PURGE_RXCLEAR = 0x0008;

        [DllImport("kernel32.dll", SetLastError = true, CharSet = CharSet.Unicode)]
        public static extern SafeFileHandle CreateFile(string lpFileName, FileAccess dwDesiredAccess, FileShare dwShareMode, IntPtr lpSecurityAttributes, FileMode dwCreationDisposition, uint dwFlagsAndAttributes, IntPtr hTemplateFile);

        [DllImport("setupapi.dll", CharSet = CharSet.Unicode, SetLastError = true)]
        public static extern bool SetupDiDestroyDeviceInfoList(IntPtr deviceInfoSet);

        [DllImport(@"setupapi.dll", CharSet = CharSet.Unicode, SetLastError = true)]
        public static extern bool SetupDiEnumDeviceInterfaces(IntPtr hDevInfo, IntPtr devInfo, ref Guid interfaceClassGuid, uint memberIndex, ref SpDeviceInterfaceData deviceInterfaceData);

        [DllImport("setupapi.dll", CharSet = CharSet.Unicode, SetLastError = true)]
        public static extern IntPtr SetupDiGetClassDevs(ref Guid classGuid, IntPtr enumerator, IntPtr hwndParent, uint flags);

        [DllImport(@"setupapi.dll", CharSet = CharSet.Unicode, SetLastError = true)]
        public static extern bool SetupDiGetDeviceInterfaceDetail(IntPtr hDevInfo, ref SpDeviceInterfaceData deviceInterfaceData, ref SpDeviceInterfaceDetailData deviceInterfaceDetailData, uint deviceInterfaceDetailDataSize, out uint requiredSize, ref SpDeviceInfoData deviceInfoData);

        [StructLayout(LayoutKind.Sequential)]
        public struct SpDeviceInterfaceData
        {
            public uint CbSize;
            public Guid InterfaceClassGuid;
            public uint Flags;
            public IntPtr Reserved;
        }

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
        public struct SpDeviceInterfaceDetailData
        {
            public int CbSize;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 256)]
            public string DevicePath;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct SpDeviceInfoData
        {
            public uint CbSize;
            public Guid ClassGuid;
            public uint DevInst;
            public IntPtr Reserved;
        }
    }
}