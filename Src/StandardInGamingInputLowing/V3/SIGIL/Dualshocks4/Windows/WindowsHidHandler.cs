using Device.Net;
using Device.Net.Windows;
using Microsoft.Extensions.Logging;
using Microsoft.Win32.SafeHandles;
using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace Hid.Net.Windows
{
    internal class WindowsHidHandler : IHidDeviceHandler
    {

        #region Private Fields

        private readonly IHidApiService _hidService;
        
        private readonly Func<TransferResult, Report> _readTransferTransform;
        private readonly Func<byte[], byte, byte[]> _writeTransferTransform;
        private Stream _readFileStream;
        private SafeFileHandle _readSafeFileHandle;
        private Stream _writeFileStream;
        private SafeFileHandle _writeSafeFileHandle;

        #endregion Private Fields

        #region Public Constructors

        public WindowsHidHandler(
            string deviceId,
            ushort? writeBufferSize = null,
            ushort? readBufferSize = null,
            IHidApiService hidApiService = null,
            ILoggerFactory loggerFactory = null,
            Func<TransferResult, Report> readTransferTransform = null,
            Func<byte[], byte, byte[]> writeTransferTransform = null)
        {
            DeviceId = deviceId ?? throw new ArgumentNullException(nameof(deviceId));

            _readTransferTransform = readTransferTransform ??
                new Func<TransferResult, Report>((tr) => tr.ToReadReport(null));

            _writeTransferTransform = writeTransferTransform ??
                new Func<byte[], byte, byte[]>(
                (data, reportId) => data.InsertReportIdAtIndexZero(reportId, null));

            _hidService = hidApiService ?? new WindowsHidApiService(loggerFactory);
            WriteBufferSize = writeBufferSize;
            ReadBufferSize = readBufferSize;
        }

        #endregion Public Constructors

        #region Public Properties

        public ConnectedDeviceDefinition ConnectedDeviceDefinition { get; private set; }
        public string DeviceId { get; }
        public bool IsInitialized { get; private set; }
        public bool? IsReadOnly { get; private set; }
        public ushort? ReadBufferSize { get; private set; }
        public ushort? WriteBufferSize { get; private set; }

        #endregion Public Properties

        #region Public Methods

        public void Close()
        {
            _readFileStream?.Dispose();
            _writeFileStream?.Dispose();

            _readFileStream = null;
            _writeFileStream = null;

            if (_readSafeFileHandle != null)
            {
                _readSafeFileHandle.Dispose();
                _readSafeFileHandle = null;
            }

            if (_writeSafeFileHandle != null)
            {
                _writeSafeFileHandle.Dispose();
                _writeSafeFileHandle = null;
            }
        }

        public Task InitializeAsync(CancellationToken cancellationToken = default)
        {
            return Task.Run(() =>
              {

                  _readSafeFileHandle = _hidService.CreateReadConnection(DeviceId, FileAccessRights.GenericRead);
                  _writeSafeFileHandle = _hidService.CreateWriteConnection(DeviceId);

                  IsReadOnly = _writeSafeFileHandle.IsInvalid;

                  ConnectedDeviceDefinition = _hidService.GetDeviceDefinition(DeviceId, _readSafeFileHandle);

                  ReadBufferSize = (ushort?)ConnectedDeviceDefinition.ReadBufferSize;
                  WriteBufferSize = (ushort?)ConnectedDeviceDefinition.WriteBufferSize;

                  _readFileStream = _hidService.OpenRead(_readSafeFileHandle, ReadBufferSize.Value);

                  if (IsReadOnly.Value) return;

                  //Don't open if this is a read only connection
                  _writeFileStream = _hidService.OpenWrite(_writeSafeFileHandle, WriteBufferSize.Value);

                  IsInitialized = true;
              }, cancellationToken);
        }

        public Stream GetFileStream()
        {
            return _readFileStream;
        }

        public async Task<uint> WriteReportAsync(byte[] data, byte reportId, CancellationToken cancellationToken = default)
        {
            if (data == null) throw new ArgumentNullException(nameof(data));

            if (_writeFileStream.CanWrite)
            {
                var transformedData = _writeTransferTransform(data, reportId);
                await _writeFileStream.WriteAsync(transformedData, 0, transformedData.Length, cancellationToken).ConfigureAwait(false);
                return (uint)data.Length;
            }
            else
            {
                throw new IOException("The file stream cannot be written to");
            }
        }

        #endregion Public Methods
    }
}