using DeviceHandle;
using DeviceHandle.Windows;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace HidHandle.Windows
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
        /// <param name="loggerFactory"><see href="https://docs.microsoft.com/en-us/dotnet/api/microsoft.extensions.logging.iloggerfactory"/></param>
        /// <param name="hidApiService">Abstraction for Hid interaction</param>
        /// <param name="classGuid">Filters by specified class guid</param>
        /// <param name="readBufferSize">Override the input report size</param>
        /// <param name="writeBufferSize">Override the output report size</param>
        /// <param name="writeReportTransform">Given the data supplied, allow you to divide the data in to a <see cref="Report"/></param>
        /// <param name="getConnectedDeviceDefinitionsAsync">Override the default call for getting definitions</param>
        /// <param name="readReportTransform">Allows you to manually convert the <see cref="Report"/> in to a <see cref="TransferResult"/> so that the Report Id is not discarded on ReadAsync. By default, this inserts the Report Id at index zero of the array.</param>
        /// <param name="readTransferTransform">Exposes the raw data from the device (including Report Id) on reads and allows you to format the returned <see cref="TransferResult"/></param>
        /// <param name="writeTransferTransform">Given the Report Id and data supplied for the write, allow you to format the raw data that is sent to the device</param>
        /// <returns>A factory which enumerates and instantiates devices</returns>
        public static IDeviceFactory CreateWindowsHidDeviceFactory(
        this FilterDeviceDefinition filterDeviceDefinition,
        IHidApiService hidApiService = null,
        Guid? classGuid = null,
        ushort? readBufferSize = null,
        ushort? writeBufferSize = null,
        GetConnectedDeviceDefinitionsAsync getConnectedDeviceDefinitionsAsync = null,
        Func<byte[], byte, byte[]> writeTransferTransform = null
            )
        {
            return CreateWindowsHidDeviceFactory(
                new ReadOnlyCollection<FilterDeviceDefinition>(new List<FilterDeviceDefinition> { filterDeviceDefinition }),
                hidApiService,
                classGuid,
                readBufferSize,
                writeBufferSize,
                getConnectedDeviceDefinitionsAsync,
                writeTransferTransform
                );
        }

        /// <summary>
        /// Creates a factory Hid devices
        /// </summary>
        /// <param name="filterDeviceDefinitions">Devices must match these</param>
        /// <param name="loggerFactory"><see href="https://docs.microsoft.com/en-us/dotnet/api/microsoft.extensions.logging.iloggerfactory"/></param>
        /// <param name="hidApiService">Abstraction for Hid interaction</param>
        /// <param name="classGuid">Filters by specified class guid</param>
        /// <param name="readBufferSize">Override the input report size</param>
        /// <param name="writeBufferSize">Override the output report size</param>
        /// <param name="getConnectedDeviceDefinitionsAsync">Override the default call for getting definitions</param>
        /// <param name="readReportTransform">Allows you to manually convert the <see cref="Report"/> in to a <see cref="TransferResult"/> so that the Report Id is not discarded on ReadAsync. By default, this inserts the Report Id at index zero of the array.</param>
        /// <param name="readTransferTransform">Exposes the raw data from the device (including Report Id) on reads and allows you to format the returned <see cref="TransferResult"/></param>
        /// <param name="writeTransferTransform">Given the Report Id and data supplied for the write, allow you to format the raw data that is sent to the device</param>
        /// <param name="writeReportTransform">Given the data supplied, allow you to divide the data in to a <see cref="Report"/></param>
        /// <returns>A factory which enumerates and instantiates devices</returns>
        public static IDeviceFactory CreateWindowsHidDeviceFactory(
            this IEnumerable<FilterDeviceDefinition> filterDeviceDefinitions,
            IHidApiService hidApiService = null,
            Guid? classGuid = null,
            ushort? readBufferSize = null,
            ushort? writeBufferSize = null,
            GetConnectedDeviceDefinitionsAsync getConnectedDeviceDefinitionsAsync = null,
            Func<byte[], byte, byte[]> writeTransferTransform = null
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
                (c, cancellationToken) => Task.FromResult<IDevice>(new HidDevice
                (
                    new WindowsHidHandler(
                        c.DeviceId,
                        writeBufferSize,
                        readBufferSize,
                        hidApiService,
                        writeTransferTransform)
                )),
                (c, cancellationToken) => Task.FromResult(c.DeviceType == DeviceType.Hid));
        }

        private static ConnectedDeviceDefinition GetDeviceDefinition(string deviceId, IHidApiService HidService)
        {
            try
            {
                var safeFileHandle = HidService.CreateReadConnection(deviceId, FileAccessRights.None);

                return HidService.GetDeviceDefinition(deviceId, safeFileHandle);
            }
            catch 
            {
                return null;
            }
        }
    }

}