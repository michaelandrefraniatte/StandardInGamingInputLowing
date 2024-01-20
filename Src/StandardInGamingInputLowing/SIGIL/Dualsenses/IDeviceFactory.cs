using System;
using System.Collections.Generic;
using System.Threading.Tasks;

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
        Task<IEnumerable<ConnectedDeviceDefinition>> GetConnectedDeviceDefinitionsAsync();

        /// <summary>
        /// Given a <see cref="ConnectedDeviceDefinition"/> returns a <see cref="IDevice"/>
        /// </summary>
        /// <param name="connectedDeviceDefinition"></param>
        /// <returns></returns>
        Task<IDevice> GetDeviceAsync(ConnectedDeviceDefinition connectedDeviceDefinition);

        /// <summary>
        /// Whether or not the factory supports the given device definition
        /// </summary>
        /// <param name="connectedDeviceDefinition"></param>
        /// <returns></returns>
        Task<bool> SupportsDeviceAsync(ConnectedDeviceDefinition connectedDeviceDefinition);
    }
}