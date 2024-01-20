using System;
using System.IO;
using System.Threading;
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

        public async Task InitializeAsync(CancellationToken cancellationToken = default)
        {
            await _hidDeviceHandler.InitializeAsync(cancellationToken).ConfigureAwait(false);
        }

        public Stream GetFileStream()
        {
            return _hidDeviceHandler.GetFileStream();
        }

        public async Task<uint> WriteReportAsync(byte[] data, byte reportId, CancellationToken cancellationToken = default)
        {
            uint bytesWritten = 0;

            try
            {
                bytesWritten = await _hidDeviceHandler.WriteReportAsync(data, reportId, cancellationToken).ConfigureAwait(false);
            }
            catch { }

            return bytesWritten;
        }

        public Task Flush(CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<uint> WriteAsync(byte[] data, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

    }
}