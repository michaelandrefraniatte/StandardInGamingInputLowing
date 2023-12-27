﻿using Device.Net;
using Device.Net.Windows;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Win32.SafeHandles;
using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace SerialPort.Net.Windows
{
    public class WindowsSerialPortDevice : DeviceBase, IDevice
    {
        #region Fields
        private readonly int _BaudRate;
        private readonly byte _ByteSize;
        private bool disposed;
        private readonly Parity _Parity;
        private SafeFileHandle _ReadSafeFileHandle;
        private readonly StopBits _StopBits;
        private ushort ReadBufferSize { get; }
        #endregion

        #region Public Properties
        public bool IsInitialized => _ReadSafeFileHandle != null && !_ReadSafeFileHandle.IsInvalid;
        /// <summary>
        /// TODO: No need to implement this. The property probably shouldn't exist at the base level
        /// </summary>
        public IApiService ApiService { get; }
        public ConnectedDeviceDefinition ConnectedDeviceDefinition { get; private set; }
        #endregion

        #region Constructor
        public WindowsSerialPortDevice(
            string deviceId,
            int baudRate = 9600,
            StopBits stopBits = StopBits.One,
            Parity parity = Parity.None,
            byte byteSize = 8,
            ushort readBufferSize = 1024,
            ILoggerFactory loggerFactory = null,
            IApiService apiService = null) : base(
                deviceId,
                loggerFactory,
                (loggerFactory ?? NullLoggerFactory.Instance).CreateLogger<WindowsSerialPortDevice>())
        {
            ApiService = apiService ?? new ApiService(null);

            ConnectedDeviceDefinition = new ConnectedDeviceDefinition(DeviceId, DeviceType.SerialPort);

            if ((byteSize == 5 && stopBits == StopBits.Two) || (stopBits == StopBits.OnePointFive && byteSize > 5))
                throw new ArgumentException(Messages.ErrorInvalidByteSizeAndStopBitsCombo);

            if (byteSize is < 5 or > 8)
                throw new ArgumentOutOfRangeException(nameof(byteSize), Messages.ErrorByteSizeMustBeFiveToEight);

            if (baudRate is < 110 or > 256000)
                throw new ArgumentOutOfRangeException(nameof(baudRate), Messages.ErrorBaudRateInvalid);

            if (stopBits == StopBits.None)
                throw new ArgumentException(Messages.ErrorMessageStopBitsMustBeSpecified, nameof(stopBits));

            ReadBufferSize = readBufferSize;
            _BaudRate = baudRate;
            _ByteSize = byteSize;
            _StopBits = stopBits;
            _Parity = parity;
        }
        #endregion

        #region Public Methods
        public Task InitializeAsync(CancellationToken cancellationToken = default) => Task.Run(Initialize, cancellationToken);

        private uint Write(byte[] data) => data == null ? 0 : ApiService.AWriteFile(_ReadSafeFileHandle, data, data.Length, out var bytesWritten, 0) ? (uint)bytesWritten : 0;

        public override Task<uint> WriteAsync(byte[] data, CancellationToken cancellationToken = default)
        {
            ValidateConnection();
            return Task.Run(() =>
            {
                var bytesWritten = Write(data);
                Logger.LogDataTransfer(new Trace(false, data));
                return bytesWritten;
            }, cancellationToken);
        }

        public override Task<TransferResult> ReadAsync(CancellationToken cancellationToken = default)
        {
            ValidateConnection();

            return Task.Run(() =>
            {
                var buffer = new byte[ReadBufferSize];
                var bytesRead = Read(buffer);
                var transferResult = new TransferResult(buffer, bytesRead);
                Logger.LogDataTransfer(new Trace(false, transferResult));
                return transferResult;
            }, cancellationToken);
        }

        public override Task Flush(CancellationToken cancellationToken = default)
        {
            ValidateConnection();

            return Task.Run(() => ApiService.APurgeComm(_ReadSafeFileHandle, APICalls.PURGE_RXCLEAR | APICalls.PURGE_TXCLEAR),
                cancellationToken);
        }

        public override void Dispose()
        {
            if (disposed)
            {
                Logger.LogWarning(Messages.WarningMessageAlreadyDisposed, DeviceId);
                return;
            }

            disposed = true;

            Logger.LogInformation(Messages.InformationMessageDisposingDevice, DeviceId);

            if (_ReadSafeFileHandle != null)
            {
                _ReadSafeFileHandle.Dispose();
                _ReadSafeFileHandle = new SafeFileHandle((IntPtr)0, true);
            }

            base.Dispose();
        }

        public void Close() => Dispose();
        #endregion

        #region Private Methods
        private void Initialize()
        {
            _ReadSafeFileHandle = ApiService.CreateReadConnection(DeviceId, FileAccessRights.GenericRead | FileAccessRights.GenericWrite);

            if (_ReadSafeFileHandle.IsInvalid) return;

            var dcb = new Dcb();

            var isSuccess = ApiService.AGetCommState(_ReadSafeFileHandle, ref dcb);

            _ = WindowsHelpers.HandleError(isSuccess, Messages.ErrorCouldNotGetCommState, Logger);

            dcb.ByteSize = _ByteSize;
            dcb.fDtrControl = 1;
            dcb.BaudRate = (uint)_BaudRate;
            dcb.fBinary = 1;
            dcb.fTXContinueOnXoff = 0;
            dcb.fAbortOnError = 0;

            dcb.fParity = 1;
#pragma warning disable IDE0010 // Add missing cases
            dcb.Parity = _Parity switch
            {
                Parity.Even => 2,
                Parity.Mark => 3,
                Parity.Odd => 1,
                Parity.Space => 4,
                Parity.None => 0,
                _ => 0
            };

            dcb.StopBits = _StopBits switch
            {
                StopBits.One => 0,
                StopBits.OnePointFive => 1,
                StopBits.Two => 2,
                StopBits.None => throw new ArgumentException(Messages.ErrorMessageStopBitsMustBeSpecified),
                _ => throw new ArgumentException(Messages.ErrorMessageStopBitsMustBeSpecified),
            };
#pragma warning restore IDE0010 // Add missing cases

            isSuccess = ApiService.ASetCommState(_ReadSafeFileHandle, ref dcb);
            _ = WindowsHelpers.HandleError(isSuccess, Messages.ErrorCouldNotSetCommState, Logger);

            var timeouts = new CommTimeouts
            {
                WriteTotalTimeoutConstant = 0,
                ReadIntervalTimeout = 1,
                WriteTotalTimeoutMultiplier = 0,
                ReadTotalTimeoutMultiplier = 0,
                ReadTotalTimeoutConstant = 0
            };

            isSuccess = ApiService.ASetCommTimeouts(_ReadSafeFileHandle, ref timeouts);
            _ = WindowsHelpers.HandleError(isSuccess, Messages.ErrorCouldNotSetCommTimeout, Logger);

            Logger.LogInformation("Serial Port device initialized successfully. Port: {port}", DeviceId);
        }

        private uint Read(byte[] data)
        =>
             ApiService.AReadFile(_ReadSafeFileHandle, data, data.Length, out var bytesRead, 0)
                ? bytesRead
                : throw new IOException(Messages.ErrorMessageRead);


        private void ValidateConnection()
        {
            if (!IsInitialized)
            {
                throw new InvalidOperationException(Messages.ErrorMessageNotInitialized);
            }
        }
        #endregion
    }
}