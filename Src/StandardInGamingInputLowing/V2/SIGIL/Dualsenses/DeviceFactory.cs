using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HidHandle
{
    public sealed class DeviceFactory : IDeviceFactory, IDisposable
    {
        public void Dispose()
        {
        }

        private readonly GetConnectedDeviceDefinitionsAsync _getConnectedDevicesAsync;
        private readonly GetDeviceAsync _getDevice;
        private readonly Func<ConnectedDeviceDefinition, Task<bool>> _supportsDevice;
        
        /// <summary>
        /// Constructs a DeviceFactory
        /// </summary>
        /// <param name="loggerFactory">The factory for creating new loggers for each device</param>
        /// <param name="getConnectedDevicesAsync">A delegate that returns matching connected device definitions</param>
        /// <param name="getDevice">A delegate to construct the device based on the specified connected device definition</param>
        /// <param name="supportsDevice">A delegate that returns whether or not this factory supports the connected device</param>
        public DeviceFactory(
            GetConnectedDeviceDefinitionsAsync getConnectedDevicesAsync,
            GetDeviceAsync getDevice,
            Func<ConnectedDeviceDefinition, Task<bool>> supportsDevice
            )
        {
            _getConnectedDevicesAsync = getConnectedDevicesAsync ?? throw new ArgumentNullException(nameof(getConnectedDevicesAsync));
            _getDevice = getDevice;
            _supportsDevice = supportsDevice ?? throw new ArgumentNullException(nameof(supportsDevice));
        }

        public Task<bool> SupportsDeviceAsync(ConnectedDeviceDefinition connectedDeviceDefinition)
        {
            return _supportsDevice(connectedDeviceDefinition);
        }

        public Task<IEnumerable<ConnectedDeviceDefinition>> GetConnectedDeviceDefinitionsAsync()
        {
            return _getConnectedDevicesAsync();
        }

        public Task<IDevice> GetDeviceAsync(ConnectedDeviceDefinition connectedDeviceDefinition)
        {
            return connectedDeviceDefinition == null ?
                throw new ArgumentNullException(nameof(connectedDeviceDefinition)) :
                _getDevice(connectedDeviceDefinition);
        }

    }
}