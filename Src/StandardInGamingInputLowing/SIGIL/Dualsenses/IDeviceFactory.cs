using System;
using System.Collections.Generic;

namespace HidHandle
{
    /// <summary>
    /// Abstraction for enumerating and constructing <see cref="IDeviceFactory"/>s 
    /// </summary>
    public interface IDeviceFactory : IDisposable
    {
        /// <summary>
        /// Gets the definition of connected devices
        /// </summary>
        /// <returns></returns>
        IEnumerable<ConnectedDeviceDefinition> GetConnectedDeviceDefinitionsAsync();

        /// <summary>
        /// Given a <see cref="ConnectedDeviceDefinition"/> returns a <see cref="HidDevice"/>
        /// </summary>
        /// <param name="connectedDeviceDefinition"></param>
        /// <returns></returns>
        HidDevice GetDeviceAsync(ConnectedDeviceDefinition connectedDeviceDefinition);

        /// <summary>
        /// Whether or not the factory supports the given device definition
        /// </summary>
        /// <param name="connectedDeviceDefinition"></param>
        /// <returns></returns>
        bool SupportsDeviceAsync(ConnectedDeviceDefinition connectedDeviceDefinition);
    }
}