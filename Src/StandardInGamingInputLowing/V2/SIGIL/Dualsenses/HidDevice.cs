using DeviceHandle;
using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace HidHandle
{
    ///<inheritdoc cref="IHidDevice"/>
    public sealed class HidDevice : DeviceBase, IHidDevice
    {
        #region Private Fields

        private readonly IHidDeviceHandler _hidDeviceHandler;
        private bool _IsClosing;
        private bool disposed;
        private readonly WriteReportTransform _writeReportTransform;

        #endregion Private Fields

        #region Public Constructors

        public HidDevice(
            IHidDeviceHandler hidDeviceHandler,
            WriteReportTransform writeReportTransform = null
            ) :
            base(
                hidDeviceHandler != null ? hidDeviceHandler.DeviceId : throw new ArgumentNullException(nameof(hidDeviceHandler))
                )
        {
            _hidDeviceHandler = hidDeviceHandler;

            _writeReportTransform = writeReportTransform ?? new WriteReportTransform((data)
                => new Report(data[0], data.TrimFirstByte()));
        }

        #endregion Public Constructors

        #region Public Properties

        public ConnectedDeviceDefinition ConnectedDeviceDefinition => _hidDeviceHandler.ConnectedDeviceDefinition;
        public bool IsInitialized => _hidDeviceHandler.IsInitialized;
        public bool? IsReadOnly => _hidDeviceHandler.IsReadOnly;
        public ushort ReadBufferSize => _hidDeviceHandler.ReadBufferSize ?? throw new InvalidOperationException("Read buffer size unknown");
        public ushort WriteBufferSize => _hidDeviceHandler.WriteBufferSize ?? throw new InvalidOperationException("Write buffer size unknown");

        #endregion Public Properties

        #region Public Methods

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

        public sealed override void Dispose()
        {
            if (disposed)
            {
                return;
            }

            disposed = true;

            GC.SuppressFinalize(this);

            Close();

            base.Dispose();
        }

        public async Task InitializeAsync(CancellationToken cancellationToken = default)
        {
            await _hidDeviceHandler.InitializeAsync(cancellationToken).ConfigureAwait(false);
        }

        public Stream GetFileStream()
        {
            return _hidDeviceHandler.GetFileStream();
        }

        /// <summary>
        /// Write a report. The report Id comes from DefaultReportId, or the first byte in the array if the DefaultReportId is null
        /// </summary>
        /// <param name="data"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public override Task<uint> WriteAsync(byte[] data, CancellationToken cancellationToken = default)
        {
            var hidReport = _writeReportTransform(data);

            //Write a report based on the default report id or the first byte in the array
            return WriteReportAsync(hidReport.TransferResult.Data, hidReport.ReportId, cancellationToken);
        }


        public async Task<uint> WriteReportAsync(byte[] data, byte reportId, CancellationToken cancellationToken = default)
        {
            try
            {
                uint bytesWritten = 0;

                if (data == null) throw new ArgumentNullException(nameof(data));

                try
                {
                    bytesWritten = await _hidDeviceHandler.WriteReportAsync(data, reportId, cancellationToken).ConfigureAwait(false);
                }
                catch { }

                return bytesWritten;
            }
            catch
            {
                throw;
            }
        }

        #endregion Public Methods

    }
}