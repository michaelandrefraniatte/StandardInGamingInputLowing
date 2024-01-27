using System;
using System.Collections.Generic;

namespace HidHandle
{
    public delegate IEnumerable<ConnectedDeviceDefinition> GetConnectedDeviceDefinitionsAsync();
    public delegate ConnectedDeviceDefinition GetDeviceDefinition(string deviceId, Guid classGuid);
    public delegate HidDevice GetDeviceAsync(ConnectedDeviceDefinition deviceId);
}