using Microsoft.Win32.SafeHandles;
using System;
using System.IO;

namespace HidHandle
{
    internal class WindowsHidHandler : IHidDeviceHandler
    {
        private readonly IHidApiService _hidService;
        private Stream _readFileStream;
        private SafeFileHandle _readSafeFileHandle;

        public WindowsHidHandler(
            string deviceId,
            ushort? readBufferSize = null,
            IHidApiService hidApiService = null)
        {
            DeviceId = deviceId ?? throw new ArgumentNullException(nameof(deviceId));
            _hidService = hidApiService ?? new WindowsHidApiService();
            ReadBufferSize = readBufferSize;
        }

        public ConnectedDeviceDefinition ConnectedDeviceDefinition { get; private set; }
        public string DeviceId { get; }
        public bool IsInitialized { get; private set; }
        public bool? IsReadOnly { get; private set; }
        public ushort? ReadBufferSize { get; private set; }

        public void Close()
        {
            _readFileStream?.Dispose();
            _readFileStream = null;
            if (_readSafeFileHandle != null)
            {
                _readSafeFileHandle.Dispose();
                _readSafeFileHandle = null;
            }
        }

        public Stream GetFileStream()
        {
            _readSafeFileHandle = ApiService.CreateReadConnection(DeviceId, FileAccess.Read);
            ConnectedDeviceDefinition = _hidService.GetDeviceDefinition(DeviceId, _readSafeFileHandle);
            ReadBufferSize = (ushort?)ConnectedDeviceDefinition.ReadBufferSize;
            _readFileStream = _hidService.OpenRead(_readSafeFileHandle, ReadBufferSize.Value);
            return _readFileStream;
        }
    }
}