using System;
using System.IO;

namespace HidHandle
{
    public sealed class HidDevice
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

        public Stream GetFileStream()
        {
            return _hidDeviceHandler.GetFileStream();
        }

    }
}