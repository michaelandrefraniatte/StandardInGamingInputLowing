using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HidHandle
{
    public delegate Task<IEnumerable<ConnectedDeviceDefinition>> GetConnectedDeviceDefinitionsAsync();
    public delegate ConnectedDeviceDefinition GetDeviceDefinition(string deviceId, Guid classGuid);
    public delegate Task<IDevice> GetDeviceAsync(ConnectedDeviceDefinition deviceId);
}