using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Runtime.InteropServices;
using System.Threading;
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

        public Task<IEnumerable<ConnectedDeviceDefinition>> GetConnectedDeviceDefinitionsAsync(CancellationToken cancellationToken = default)
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

                    const int flags = APICalls.DigcfDeviceinterface | APICalls.DigcfPresent;

                    var copyOfClassGuid = new Guid(_classGuid.ToString());

                    var devicesHandle =
                        APICalls.SetupDiGetClassDevs(ref copyOfClassGuid, IntPtr.Zero, IntPtr.Zero, flags);

                    spDeviceInterfaceDetailData.CbSize = IntPtr.Size == 8 ? 8 : 4 + Marshal.SystemDefaultCharSize;

                    var i = -1;

                    while (true)
                    {
                        try
                        {
                            i++;

                            var isSuccess = APICalls.SetupDiEnumDeviceInterfaces(devicesHandle, IntPtr.Zero,
                                ref copyOfClassGuid, (uint)i, ref spDeviceInterfaceData);
                            if (!isSuccess)
                            {
                                var errorCode = Marshal.GetLastWin32Error();

                                if (errorCode == APICalls.ERROR_NO_MORE_ITEMS)
                                {
                                    break;
                                }
                            }

                            isSuccess = APICalls.SetupDiGetDeviceInterfaceDetail(devicesHandle,
                                ref spDeviceInterfaceData, ref spDeviceInterfaceDetailData, 256, out _,
                                ref spDeviceInfoData);
                            if (!isSuccess)
                            {
                                var errorCode = Marshal.GetLastWin32Error();

                                if (errorCode == APICalls.ERROR_NO_MORE_ITEMS)
                                {
                                    break;
                                }
                            }

                            var connectedDeviceDefinition =
                                _getDeviceDefinition(spDeviceInterfaceDetailData.DevicePath, copyOfClassGuid);

                            if (connectedDeviceDefinition == null)
                            {
                                continue;
                            }

                            if (!await _isMatch(connectedDeviceDefinition).ConfigureAwait(false)) continue;

                            deviceDefinitions.Add(connectedDeviceDefinition);
                        }
                        catch { }
                    }

                    _ = APICalls.SetupDiDestroyDeviceInfoList(devicesHandle);

                    return new ReadOnlyCollection<ConnectedDeviceDefinition>(deviceDefinitions);
                }
                catch
                {
                    throw;
                }
            }, cancellationToken);

    }
}