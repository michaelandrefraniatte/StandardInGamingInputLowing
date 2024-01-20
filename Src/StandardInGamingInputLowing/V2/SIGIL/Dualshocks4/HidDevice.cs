using System;
using System.IO;
using System.Threading.Tasks;

namespace HidHandle
{
    ///<inheritdoc cref="IHidDevice"/>
    public sealed class HidDevice : IHidDevice
    {

        private readonly IHidDeviceHandler _hidDeviceHandler;
        private bool _IsClosing;
        private bool disposed;

        public HidDevice(
            IHidDeviceHandler hidDeviceHandler
            )
        {
            _hidDeviceHandler = hidDeviceHandler;
        }

        public ConnectedDeviceDefinition ConnectedDeviceDefinition => _hidDeviceHandler.ConnectedDeviceDefinition;
        public bool IsInitialized => _hidDeviceHandler.IsInitialized;

        public string DeviceId => throw new NotImplementedException();

        public void Close()
        {
            if (_IsClosing) return;

            _IsClosing = true;

            try
            {
                _hidDeviceHandler.Close();
            }
            catch { }

            _IsClosing = false;
        }

        public void Dispose()
        {
            if (disposed)
            {
                return;
            }

            disposed = true;

            GC.SuppressFinalize(this);

            Close();
        }

        public async Task InitializeAsync()
        {
            await _hidDeviceHandler.InitializeAsync();
        }

        public Stream GetFileStream()
        {
            return _hidDeviceHandler.GetFileStream();
        }

        public async Task<uint> WriteReportAsync(byte[] data, byte reportId)
        {
            uint bytesWritten = 0;

            try
            {
                bytesWritten = await _hidDeviceHandler.WriteReportAsync(data, reportId);
            }
            catch { }

            return bytesWritten;
        }

        public Task Flush()
        {
            throw new NotImplementedException();
        }

        public Task<uint> WriteAsync(byte[] data)
        {
            throw new NotImplementedException();
        }

    }
}