using System;
using System.Threading;
using System.Threading.Tasks;

namespace Device.Net
{
    /// <summary>
    /// Base class for all devices
    /// </summary>
    public abstract class DeviceBase : IDisposable
    {
        #region Fields
        private readonly SemaphoreSlim _WriteAndReadLock = new SemaphoreSlim(1, 1);
        private bool disposed;
        #endregion

        #region Public Properties
        public string DeviceId { get; }
        #endregion

        #region Constructor
        protected DeviceBase(
            string deviceId
            )
        {
            DeviceId = deviceId ?? throw new ArgumentNullException(nameof(deviceId));
        }
        #endregion

        #region Public Abstract Methods
        //TODO: Why are these here?

        public abstract Task<uint> WriteAsync(byte[] data, CancellationToken cancellationToken = default);
        #endregion

        #region Public Methods
        public virtual Task Flush(CancellationToken cancellationToken = default) => throw new NotImplementedException();

        public virtual void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool dispose)
        {
            if (disposed)
            {
                return;
            }

            disposed = true;

            _WriteAndReadLock.Dispose();

            GC.SuppressFinalize(this);
        }
        #endregion

        #region Public Static Methods
        public static ConnectedDeviceDefinition GetDeviceDefinitionFromWindowsDeviceId(
            string deviceId,
            DeviceType deviceType,
            Guid? classGuid = null)
        {
            uint? vid = null;
            uint? pid = null;
            try
            {
                vid = GetNumberFromDeviceId(deviceId, "vid_");
                pid = GetNumberFromDeviceId(deviceId, "pid_");
            }
#pragma warning disable CA1031 
            catch { }

            return new ConnectedDeviceDefinition(deviceId, deviceType, vid, pid, classGuid: classGuid);
        }
        #endregion

        #region Private Static Methods
        private static uint GetNumberFromDeviceId(string deviceId, string searchString)
        {
            if (deviceId == null) throw new ArgumentNullException(nameof(deviceId));

            var indexOfSearchString = deviceId.IndexOf(searchString, StringComparison.OrdinalIgnoreCase);
            string hexString = null;
            if (indexOfSearchString > -1)
            {
                hexString = deviceId.Substring(indexOfSearchString + searchString.Length, 4);
            }
#pragma warning disable CA1305 // Specify IFormatProvider
            var numberAsInteger = uint.Parse(hexString, System.Globalization.NumberStyles.HexNumber);
#pragma warning restore CA1305 // Specify IFormatProvider
            return numberAsInteger;
        }
        #endregion
    }
}
