using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;
using timer = System.Timers.Timer;

namespace Device.Net
{
    /// <summary>
    /// Handles connection of devices. This is not the recommended approach. Please use DeviceManager where possible. Documentation href="https://docs.microsoft.com/en-us/dotnet/api/microsoft.extensions.logging.iloggerfactory" 
    /// </summary>
    public sealed class DeviceListener : IDeviceListener
    {
        #region Fields
        private bool _IsDisposed;
        private readonly timer _PollTimer;
        private readonly SemaphoreSlim _ListenSemaphoreSlim = new SemaphoreSlim(1, 1);
        

        /// <summary>
        /// This is the list of Devices by their filter definition. Note this is not actually keyed by the connected definition.
        /// </summary>
        private readonly Dictionary<string, IDevice> _CreatedDevicesByDefinition = new Dictionary<string, IDevice>();
        #endregion

        #region Public Properties
        public IDeviceFactory DeviceFactory { get; }
        #endregion

        #region Events
        public event EventHandler<DeviceEventArgs> DeviceInitialized;
        public event EventHandler<DeviceEventArgs> DeviceDisconnected;
        #endregion

        #region Constructor
        /// <summary>
        /// Handles connecting to and disconnecting from a set of potential devices by their definition
        /// </summary>
        public DeviceListener(
            IDeviceFactory deviceFactory,
            int? pollMilliseconds = 1000,
            ILoggerFactory loggerFactory = null)
        {

            DeviceFactory = deviceFactory ?? throw new ArgumentNullException(nameof(deviceFactory));

            _PollTimer = new timer(pollMilliseconds.Value);
            _PollTimer.Elapsed += PollTimer_Elapsed;
        }
        #endregion

        #region Event Handlers
        private async void PollTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            if (_IsDisposed)
                return;
            await CheckForDevicesAsync().ConfigureAwait(false);
        }
        #endregion

        #region Public Methods
        /// <summary>
        /// Starts the polling for devices if polling is being used.
        /// </summary>
        public void Start()
        {
            _PollTimer.Start();
        }

        public async Task CheckForDevicesAsync(CancellationToken cancellationToken = default)
        {
            try
            {
                if (_IsDisposed) return;
                await _ListenSemaphoreSlim.WaitAsync(cancellationToken).ConfigureAwait(false);

                var connectedDeviceDefinitions = (await DeviceFactory.GetConnectedDeviceDefinitionsAsync(cancellationToken).ConfigureAwait(false)).ToList();

                //Iterate through connected devices
                foreach (var connectedDeviceDefinition in connectedDeviceDefinitions)
                {
                    //TODO: What to do if there are multiple?

                    IDevice device = null;
                    if (_CreatedDevicesByDefinition.ContainsKey(connectedDeviceDefinition.DeviceId))
                    {
                        device = _CreatedDevicesByDefinition[connectedDeviceDefinition.DeviceId];
                    }

                    if (device == null)
                    {
                        //Need to use the connected device def here instead of the filter version because the filter version won't have the id or any details
                        device = await DeviceFactory.GetDeviceAsync(connectedDeviceDefinition, cancellationToken).ConfigureAwait(false);
                        
                        if (device != null)
                        {
                            _CreatedDevicesByDefinition.Add(connectedDeviceDefinition.DeviceId, device);
                        }
                    }
                    else
                    {
                        if (device.IsInitialized) continue;
                    }

                    //The device is not initialized so initialize it
                    await device.InitializeAsync(cancellationToken).ConfigureAwait(false);

                    //Let listeners know a registered device was initialized
                    DeviceInitialized?.Invoke(this, new DeviceEventArgs(device));
                }

                var removeDeviceIds = new List<string>();

                //Iterate through registered devices
                foreach (var deviceId in _CreatedDevicesByDefinition.Keys)
                {
                    var device = _CreatedDevicesByDefinition[deviceId];

                    if (connectedDeviceDefinitions.Any(cdd => cdd.DeviceId == deviceId)) continue;

                    if (!device.IsInitialized) continue;

                    //Let listeners know a registered device was disconnected
                    //NOTE: let the rest of the app know before disposal so that the app can stop doing whatever it's doing.
                    DeviceDisconnected?.Invoke(this, new DeviceEventArgs(device));

                    //The device is no longer connected so close it
                    device.Close();

                    removeDeviceIds.Add(deviceId);
                }

                foreach (var deviceId in removeDeviceIds)
                {
                    _ = _CreatedDevicesByDefinition.Remove(deviceId);
                }

            }
#pragma warning disable CA1031 // Do not catch general exception types
            catch { }
            finally
            {
                if (!_IsDisposed)
                    _ = _ListenSemaphoreSlim.Release();
            }
        }

        public void Stop() => _PollTimer.Stop();

        public void Dispose()
        {
            if (_IsDisposed)
            {
                return;
            }

            _IsDisposed = true;

            Stop();

            _PollTimer?.Dispose();

            foreach (var key in _CreatedDevicesByDefinition.Keys)
            {
                _CreatedDevicesByDefinition[key].Dispose();
            }

            _CreatedDevicesByDefinition.Clear();

            _ListenSemaphoreSlim.Dispose();

            DeviceInitialized = null;
            DeviceDisconnected = null;

            GC.SuppressFinalize(this);
        }
        #endregion
    }
}

