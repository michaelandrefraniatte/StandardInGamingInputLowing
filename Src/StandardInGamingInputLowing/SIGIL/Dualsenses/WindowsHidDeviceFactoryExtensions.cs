using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace HidHandle
{
    /// <summary>
    /// Instantiates Windows Hid Factories. Use these methods as extension methods with <see cref="FilterDeviceDefinition"/> or directly to get all devices
    /// </summary>
    public static class WindowsHidDeviceFactoryExtensions
    {

        /// <summary>
        /// Creates a <see cref="IDeviceFactory"/> for Windows Hid devices
        /// </summary>
        /// <param name="filterDeviceDefinition">Devices must match this</param>
        /// <param name="hidApiService">Abstraction for Hid interaction</param>
        /// <param name="classGuid">Filters by specified class guid</param>
        /// <param name="readBufferSize">Override the input report size</param>
        /// <param name="getConnectedDeviceDefinitionsAsync">Override the default call for getting definitions</param>
        /// <returns>A factory which enumerates and instantiates devices</returns>
        public static IDeviceFactory CreateWindowsHidDeviceFactory(
        this FilterDeviceDefinition filterDeviceDefinition,
        IHidApiService hidApiService = null,
        Guid? classGuid = null,
        ushort? readBufferSize = null,
        GetConnectedDeviceDefinitionsAsync getConnectedDeviceDefinitionsAsync = null
            )
        {
            return CreateWindowsHidDeviceFactory(
                new ReadOnlyCollection<FilterDeviceDefinition>(new List<FilterDeviceDefinition> { filterDeviceDefinition }),
                hidApiService,
                classGuid,
                readBufferSize,
                getConnectedDeviceDefinitionsAsync
                );
        }

        /// <summary>
        /// Creates a factory Hid devices
        /// </summary>
        /// <param name="filterDeviceDefinitions">Devices must match these</param>
        /// <param name="hidApiService">Abstraction for Hid interaction</param>
        /// <param name="classGuid">Filters by specified class guid</param>
        /// <param name="readBufferSize">Override the input report size</param>
        /// <param name="getConnectedDeviceDefinitionsAsync">Override the default call for getting definitions</param>
        /// <returns>A factory which enumerates and instantiates devices</returns>
        public static IDeviceFactory CreateWindowsHidDeviceFactory(
            this IEnumerable<FilterDeviceDefinition> filterDeviceDefinitions,
            IHidApiService hidApiService = null,
            Guid? classGuid = null,
            ushort? readBufferSize = null,
            GetConnectedDeviceDefinitionsAsync getConnectedDeviceDefinitionsAsync = null
            )
        {
            if (filterDeviceDefinitions == null) throw new ArgumentNullException(nameof(filterDeviceDefinitions));

            var selectedHidApiService = hidApiService ?? new WindowsHidApiService();

            classGuid = selectedHidApiService.GetHidGuid();

            if (getConnectedDeviceDefinitionsAsync == null)
            {
                var windowsDeviceEnumerator = new WindowsDeviceEnumerator(
                    classGuid.Value,
                    (d, guid) => GetDeviceDefinition(d, selectedHidApiService),
                    c => Task.FromResult(!filterDeviceDefinitions.Any() || filterDeviceDefinitions.FirstOrDefault(f => f.IsDefinitionMatch(c, DeviceType.Hid)) != null)
                    );

                getConnectedDeviceDefinitionsAsync = windowsDeviceEnumerator.GetConnectedDeviceDefinitionsAsync;
            }

            return new DeviceFactory(
                getConnectedDeviceDefinitionsAsync,
                (c) => new HidDevice
                (
                    new WindowsHidHandler(
                        c.DeviceId,
                        readBufferSize,
                        hidApiService)
                ),
                (c) => c.DeviceType == DeviceType.Hid);
        }

        private static ConnectedDeviceDefinition GetDeviceDefinition(string deviceId, IHidApiService HidService)
        {
            try
            {
                var safeFileHandle = ApiService.CreateReadConnection(deviceId, FileAccess.Read);

                return HidService.GetDeviceDefinition(deviceId, safeFileHandle);
            }
            catch 
            {
                return null;
            }
        }
    }

}