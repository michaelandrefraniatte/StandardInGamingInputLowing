using System;
using System.Collections.Generic;

namespace HidHandle
{
    public delegate IEnumerable<ConnectedDeviceDefinition> GetConnectedDeviceDefinitions();
    public delegate ConnectedDeviceDefinition GetDeviceDefinition(string deviceId, Guid classGuid);
    public delegate HidDevice GetDevice(ConnectedDeviceDefinition deviceId);
}