﻿// ReSharper disable UnusedMember.Global
// ReSharper disable InconsistentNaming

using Device.Net.Windows;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Win32.SafeHandles;
using System;
using System.Runtime.InteropServices;
using System.Text;

#pragma warning disable CA1707 // Identifiers should not contain underscores
#pragma warning disable CA1021 // Avoid out parameters
#pragma warning disable CA1401 // P/Invokes should not be visible
#pragma warning disable CA5392 // Use DefaultDllImportSearchPaths attribute for P/Invokes
#pragma warning disable CA1045 // Do not pass types by reference
#pragma warning disable CA1060 // Move pinvokes to native methods class

namespace Usb.Net.Windows
{
    internal static partial class WinUsbApiCalls
    {
        #region Constants
        internal const int EnglishLanguageID = 1033;
        internal const uint DEVICE_SPEED = 1;
        internal const byte USB_ENDPOINT_DIRECTION_MASK = 0X80;
        internal const byte WritePipeId = 0x80;

        /// <summary>
        /// Not sure where this constant is defined...
        /// </summary>
        internal const int DEFAULT_DESCRIPTOR_TYPE = 0x01;
        internal const int USB_STRING_DESCRIPTOR_TYPE = 0x03;
        #endregion

        #region API Calls
        [DllImport("winusb.dll", SetLastError = true)]
        internal static extern bool WinUsb_ControlTransfer(IntPtr InterfaceHandle, WINUSB_SETUP_PACKET SetupPacket, byte[] Buffer, uint BufferLength, ref uint LengthTransferred, IntPtr Overlapped);

        [DllImport("winusb.dll", SetLastError = true, CharSet = CharSet.Auto)]
        internal static extern bool WinUsb_GetAssociatedInterface(SafeFileHandle InterfaceHandle, byte AssociatedInterfaceIndex, out SafeFileHandle AssociatedInterfaceHandle);

        [DllImport("winusb.dll", SetLastError = true)]
        internal static extern bool WinUsb_GetDescriptor(SafeFileHandle InterfaceHandle, byte DescriptorType, byte Index, ushort LanguageID, out USB_DEVICE_DESCRIPTOR deviceDesc, uint BufferLength, out uint LengthTransfered);

        [DllImport("winusb.dll", SetLastError = true)]
        internal static extern bool WinUsb_GetDescriptor(SafeFileHandle InterfaceHandle, byte DescriptorType, byte Index, ushort LanguageID, byte[] Buffer, uint BufferLength, out uint LengthTransfered);

        [DllImport("winusb.dll", SetLastError = true)]
        internal static extern bool WinUsb_Free(SafeFileHandle InterfaceHandle);

        [DllImport("winusb.dll", SetLastError = true)]
        internal static extern bool WinUsb_Initialize(SafeFileHandle DeviceHandle, out IntPtr InterfaceHandle);

        [DllImport("winusb.dll", SetLastError = true)]
        internal static extern bool WinUsb_QueryDeviceInformation(IntPtr InterfaceHandle, uint InformationType, ref uint BufferLength, ref byte Buffer);

        [DllImport("winusb.dll", SetLastError = true)]
        internal static extern bool WinUsb_QueryInterfaceSettings(SafeFileHandle InterfaceHandle, byte AlternateInterfaceNumber, out USB_INTERFACE_DESCRIPTOR UsbAltInterfaceDescriptor);

        [DllImport("winusb.dll", SetLastError = true)]
        internal static extern bool WinUsb_QueryPipe(SafeFileHandle InterfaceHandle, byte AlternateInterfaceNumber, byte PipeIndex, out WINUSB_PIPE_INFORMATION PipeInformation);

        [DllImport("winusb.dll", SetLastError = true)]
        internal static extern bool WinUsb_ReadPipe(SafeFileHandle InterfaceHandle, byte PipeID, byte[] Buffer, uint BufferLength, out uint LengthTransferred, IntPtr Overlapped);

        [DllImport("winusb.dll", SetLastError = true)]
        internal static extern bool WinUsb_SetPipePolicy(SafeFileHandle InterfaceHandle, byte PipeID, uint PolicyType, uint ValueLength, ref uint Value);

        [DllImport("winusb.dll", SetLastError = true)]
        internal static extern bool WinUsb_WritePipe(SafeFileHandle InterfaceHandle, byte PipeID, byte[] Buffer, uint BufferLength, out uint LengthTransferred, IntPtr Overlapped);
        #endregion

        #region internal Methods
        internal static string GetDescriptor(SafeFileHandle defaultInterfaceHandle, byte index, string errorMessage, ILogger logger)
        {
            logger ??= NullLogger.Instance;

            var buffer = new byte[256];
            var isSuccess = WinUsb_GetDescriptor(defaultInterfaceHandle, USB_STRING_DESCRIPTOR_TYPE, index, EnglishLanguageID, buffer, (uint)buffer.Length, out var transfered);
            if (WindowsHelpers.HandleError(isSuccess, errorMessage, logger, false) != 0)
            {
                logger.LogWarning(errorMessage);
                return null;
            }

            var descriptor = new string(Encoding.Unicode.GetChars(buffer, 2, (int)transfered));
            return descriptor.Substring(0, descriptor.Length - 1);
        }
        #endregion
    }
}