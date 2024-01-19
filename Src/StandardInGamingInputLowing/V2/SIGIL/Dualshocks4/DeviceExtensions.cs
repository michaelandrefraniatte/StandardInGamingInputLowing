using System;

namespace DeviceHandle
{
    public static class DeviceExtensions
    {
        /// <summary>
        /// Compares a <see cref="ConnectedDeviceDefinition"/> with a <see cref="FilterDeviceDefinition"/>
        /// </summary>
        /// <param name="filterDevice"></param>
        /// <param name="actualDevice"></param>
        /// <param name="deviceType"></param>
        /// <returns>True if the filterDevice matches the actualDevice</returns>
        public static bool IsDefinitionMatch(this FilterDeviceDefinition filterDevice, ConnectedDeviceDefinition actualDevice, DeviceType deviceType)
        {
            if (actualDevice == null) throw new ArgumentNullException(nameof(actualDevice));

            if (filterDevice == null) return true;

            var vendorIdPasses = !filterDevice.VendorId.HasValue || filterDevice.VendorId == actualDevice.VendorId;
            var productIdPasses = !filterDevice.ProductId.HasValue || filterDevice.ProductId == actualDevice.ProductId;
            var deviceTypePasses = actualDevice.DeviceType == deviceType;
            var usagePagePasses = !filterDevice.UsagePage.HasValue || filterDevice.UsagePage == actualDevice.UsagePage;
            var classGuidPasses = !filterDevice.ClassGuid.HasValue || filterDevice.ClassGuid == actualDevice.ClassGuid;

            var returnValue =
                vendorIdPasses &&
                productIdPasses &&
                deviceTypePasses &&
                usagePagePasses &&
                classGuidPasses;

            return returnValue;
        }

    }
}