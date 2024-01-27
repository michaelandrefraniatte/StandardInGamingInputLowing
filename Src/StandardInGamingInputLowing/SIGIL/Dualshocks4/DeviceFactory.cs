using System;
using System.Collections.Generic;

namespace HidHandle
{
    public sealed class DeviceFactory : IDeviceFactory, IDisposable
    {
        public void Dispose()
        {
        }

        private readonly GetConnectedDeviceDefinitions _getConnectedDevicesAsync;
        private readonly GetDevice _getDevice;
        private readonly Func<ConnectedDeviceDefinition, bool> _supportsDevice;
        
        /// <summary>
        /// Constructs a DeviceFactory
        /// </summary>
        /// <param name="getConnectedDevicesAsync">A delegate that returns matching connected device definitions</param>
        /// <param name="getDevice">A delegate to construct the device based on the specified connected device definition</param>
        /// <param name="supportsDevice">A delegate that returns whether or not this factory supports the connected device</param>
        public DeviceFactory(
            GetConnectedDeviceDefinitions getConnectedDevicesAsync,
            GetDevice getDevice,
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

        public IEnumerable<ConnectedDeviceDefinition> GetConnectedDeviceDefinitions()
        {
            return _getConnectedDevicesAsync();
        }

        public HidDevice GetDevice(ConnectedDeviceDefinition connectedDeviceDefinition)
        {
            return connectedDeviceDefinition == null ?
                throw new ArgumentNullException(nameof(connectedDeviceDefinition)) :
                _getDevice(connectedDeviceDefinition);
        }

    }
}