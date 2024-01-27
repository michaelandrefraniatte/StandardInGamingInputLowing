using System.IO;

namespace HidHandle
{
    public interface IHidDeviceHandler
    {
        ConnectedDeviceDefinition ConnectedDeviceDefinition { get; }
        bool? IsReadOnly { get; }
        ushort? ReadBufferSize { get; }
        string DeviceId { get; }
        bool IsInitialized { get; }
        void Close();
        Stream GetFileStream();
    }
}