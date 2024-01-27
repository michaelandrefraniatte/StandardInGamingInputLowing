using System;
using System.Collections.Generic;

namespace HidHandle
{
    public sealed class DeviceFactory : IDeviceFactory, IDisposable
    {
        public void Dispose()
        {
        }

        private readonly GetConnectedDeviceDefinitionsAsync _getConnectedDevicesAsync;
        private readonly GetDeviceAsync _getDevice;
        private readonly Func<ConnectedDeviceDefinition, bool> _supportsDevice;
        
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
            Func<ConnectedDeviceDefinition, bool> supportsDevice
            )
        {
            _getConnectedDevicesAsync = getConnectedDevicesAsync ?? throw new ArgumentNullException(nameof(getConnectedDevicesAsync));
            _getDevice = getDevice;
            _supportsDevice = supportsDevice ?? throw new ArgumentNullException(nameof(supportsDevice));
        }

        public bool SupportsDeviceAsync(ConnectedDeviceDefinition connectedDeviceDefinition)
        {
            return _supportsDevice(connectedDeviceDefinition);
        }

        public IEnumerable<ConnectedDeviceDefinition> GetConnectedDeviceDefinitionsAsync()
        {
            return _getConnectedDevicesAsync();
        }

        public HidDevice GetDeviceAsync(ConnectedDeviceDefinition connectedDeviceDefinition)
        {
            return connectedDeviceDefinition == null ?
                throw new ArgumentNullException(nameof(connectedDeviceDefinition)) :
                _getDevice(connectedDeviceDefinition);
        }

    }
}