using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Device.Net
{

    internal class AggregateDeviceFactory
    {
        //TODO: Put logging in here

        #region Fields
        private readonly ILoggerFactory _loggerFactory;
        private readonly ILogger _logger;
        #endregion

        #region Public Properties
        public const string ObsoleteMessage = "This method will soon be removed. Create an instance of DeviceManager and register factories there";
        public IReadOnlyCollection<IDeviceFactory> DeviceFactories { get; }
        #endregion

        #region Constructor
        public AggregateDeviceFactory(
            IReadOnlyCollection<IDeviceFactory> deviceFactories,
            ILoggerFactory loggerFactory = null)
        {
            _loggerFactory = loggerFactory ?? new NullLoggerFactory();
            _logger = _loggerFactory.CreateLogger<AggregateDeviceFactory>();
            DeviceFactories = deviceFactories ?? throw new ArgumentNullException(nameof(deviceFactories));

            if (deviceFactories.Count == 0)
            {
                throw new InvalidOperationException("You must specify at least one Device Factory");
            }

        }
        #endregion

        #region Public Methods

        public async Task<IEnumerable<ConnectedDeviceDefinition>> GetConnectedDeviceDefinitionsAsync(CancellationToken cancellationToken = default)
        {
            var retVal = new List<ConnectedDeviceDefinition>();

            foreach (var deviceFactory in DeviceFactories)
            {
                try
                {
                    //TODO: Do this in parallel?
                    var factoryResults = await deviceFactory.GetConnectedDeviceDefinitionsAsync(cancellationToken).ConfigureAwait(false);
                    retVal.AddRange(factoryResults);

                    _logger.LogDebug("Called " + nameof(GetConnectedDeviceDefinitionsAsync) + " on " + deviceFactory.GetType().Name);
                }
                catch (Exception ex)
                {
                    //TODO: We probably want to remove this. If a factory crashes, we probably don't want to swallow the error

                    _logger.LogError(ex, "Error calling " + nameof(GetConnectedDeviceDefinitionsAsync));
                }
            }

            return retVal;
        }

        #endregion
    }
}
