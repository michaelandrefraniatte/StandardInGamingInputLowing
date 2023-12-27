﻿using Hid.Net;
using Hid.Net.Windows;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Win32.SafeHandles;
using Moq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Device.Net.UnitTests
{
    [TestClass]
    public class HidTests
    {
        #region Private Fields

        private Mock<IHidDeviceHandler> _trezorDeviceHandler;
        private readonly ILoggerFactory loggerFactory = LoggerFactory.Create(builder => _ = builder.AddDebug().AddConsole().SetMinimumLevel(LogLevel.Trace));

        #endregion Private Fields


        #region Public Methods

        [TestMethod]
        public void TestDeviceIdInvalidException()
        {
            try
            {
                _ = new WindowsHidHandler(null, readTransferTransform: (a) => default, writeTransferTransform: (a, b) => default);
            }
            catch (ArgumentNullException ane)
            {
                Assert.AreEqual("deviceId", ane.ParamName);
                return;
            }

            Assert.Fail();
        }


        [TestMethod]
        public async Task TestInitializeHidDeviceReadOnly()
        {
            var windowsHidDevice = await InitializeWindowsHidDevice(true);
            Assert.AreEqual(true, windowsHidDevice.IsReadOnly);
        }

        [TestMethod]
        public async Task TestInitializeHidDeviceWriteable()
        {
            var windowsHidDevice = await InitializeWindowsHidDevice(false);
            Assert.AreEqual(false, windowsHidDevice.IsReadOnly);
        }

        [TestMethod]
        public async Task TestTrezorHid()
        {
            _ = await IntegrationTests.TestWriteAndReadFromTrezor(
            GetMockTrezorDeviceFactory(loggerFactory, (readReport)
                //We expect to get back 64 bytes but ReadAsync would normally add the Report Id back index 0
                //In the case of Trezor we just take the 64 bytes and don't put the Report Id back at index 0
                => new TransferResult(readReport.TransferResult.Data, readReport.TransferResult.BytesTransferred)),
            64,
            65
            );

            _trezorDeviceHandler.Verify(t => t.InitializeAsync(It.IsAny<CancellationToken>()), Times.Once);

            _trezorDeviceHandler.Verify(t => t.Close(), Times.Once);
        }

        //TODO : Assert the logging here

        [TestMethod]
        public void TestToTransferResult()
        {
            //Arrange
            const byte reportId = 1;
            var report = new Report(reportId, new TransferResult(new byte[1] { 2 }, 1));

            //Act
            var transferResult = report.ToTransferResult(loggerFactory.CreateLogger<HidTests>());

            //Assert

            //Bytes transferred is intact
            Assert.AreEqual(transferResult.BytesTransferred, (uint)1);

            //Data is intact and report id is inserted at index zero
            //Also asserts length of array
            Assert.IsTrue(transferResult.Data.SequenceEqual(new byte[] { reportId, 2 }));
        }

        [TestMethod]
        public void TestInsertReportIdAtIndexZero()
        {
            //Arrange
            const byte reportId = 1;

            //Act
            var data = new byte[0].InsertReportIdAtIndexZero(reportId, loggerFactory.CreateLogger<HidTests>());

            //Assert

            //Data length is correct
            Assert.AreEqual(data.Length, 1);

            //Data is intact and report id is inserted at index zero
            Assert.IsTrue(data.SequenceEqual(new byte[] { reportId }));
        }

        [TestMethod]
        public void TestToReadReport()
        {
            //Arrange
            const byte reportId = 1;
            var transferResult = new TransferResult(new byte[] { reportId, 2 }, 2);

            //Act
            var report = transferResult.ToReadReport(loggerFactory.CreateLogger<HidTests>());

            //Assert

            //Bytes transferred is intact
            Assert.AreEqual(report.TransferResult.BytesTransferred, (uint)2);

            //Data is intact and report was removed from index zero
            //Also asserts length of array
            Assert.IsTrue(report.TransferResult.Data.SequenceEqual(new byte[] { 2 }));

            //Check the report id
            Assert.AreEqual(report.ReportId, reportId);
        }

        [TestMethod]
        public void TestTrimFirstByte()
        {
            //Arrange
            var data = new byte[] { 1, 2 };

            //Act
            var trimmeData = data.TrimFirstByte(loggerFactory.CreateLogger<HidTests>());

            //Assert

            //Data is intact and byte was removed from index zero
            //Also asserts length of array
            Assert.IsTrue(trimmeData.SequenceEqual(new byte[] { 2 }));
        }
        #endregion Public Methods

        #region Private Methods

        private static async Task<WindowsHidHandler> InitializeWindowsHidDevice(bool isReadonly)
        {
            const string deviceId = "test";
            var hidService = new Mock<IHidApiService>();
            var invalidSafeFileHandle = new SafeFileHandle((IntPtr)(-1), true);
            var validSafeFileHandle = new SafeFileHandle((IntPtr)100, true);

            _ = hidService.Setup(s => s.CreateReadConnection(deviceId, Windows.FileAccessRights.GenericRead)).Returns(validSafeFileHandle);
            _ = hidService.Setup(s => s.CreateWriteConnection(deviceId)).Returns(!isReadonly ? validSafeFileHandle : invalidSafeFileHandle);
            _ = hidService.Setup(s => s.GetDeviceDefinition(deviceId, validSafeFileHandle)).Returns(new ConnectedDeviceDefinition(deviceId, DeviceType.Hid, readBufferSize: 64, writeBufferSize: 64));

            var readStream = new Mock<Stream>();
            _ = readStream.Setup(s => s.CanRead).Returns(true);
            _ = hidService.Setup(s => s.OpenRead(It.IsAny<SafeFileHandle>(), It.IsAny<ushort>())).Returns(readStream.Object);

            _ = readStream.Setup(s => s.CanWrite).Returns(!isReadonly);
            _ = hidService.Setup(s => s.OpenWrite(It.IsAny<SafeFileHandle>(), It.IsAny<ushort>())).Returns(readStream.Object);

            var loggerFactory = new Mock<ILoggerFactory>();
            var logger = new Mock<ILogger<HidDevice>>();
            _ = logger.Setup(l => l.BeginScope(It.IsAny<It.IsAnyType>())).Returns(new Mock<IDisposable>().Object);

            _ = loggerFactory.Setup(f => f.CreateLogger(It.IsAny<string>())).Returns(logger.Object);

            var windowsHidDevice = new WindowsHidHandler(deviceId, loggerFactory: loggerFactory.Object, readTransferTransform: (a) => default, writeTransferTransform: (a, b) => default, hidApiService: hidService.Object);
            await windowsHidDevice.InitializeAsync();

            //TODO: Fix this

            if (!isReadonly)
            {
                //UnitTests.CheckLogMessageText(logger, Messages.SuccessMessageReadFileStreamOpened, LogLevel.Information, Times.Once());

                //logger.Received().Log(Messages.SuccessMessageReadFileStreamOpened, nameof(WindowsHidDevice), null, LogLevel.Information);
            }
            else
            {
                //logger.Received().Log(Messages.WarningMessageOpeningInReadonlyMode(deviceId), nameof(WindowsHidDevice), null, LogLevel.Warning);
            }

            hidService.Verify(s => s.OpenRead(It.IsAny<SafeFileHandle>(), It.IsAny<ushort>()));

            if (!isReadonly)
            {
                hidService.Verify(s => s.OpenWrite(It.IsAny<SafeFileHandle>(), It.IsAny<ushort>()));
            }

            return windowsHidDevice;
        }

        private IDeviceFactory GetMockTrezorDeviceFactory(ILoggerFactory loggerFactory, Func<Report, TransferResult> readReportTransform)
        {
            //TODO: Turn this in to a real device factory with a mocked GetConnectedDeviceDefinitions
            var deviceFactory = new Mock<IDeviceFactory>();

            //Mock the handler
            _trezorDeviceHandler = new Mock<IHidDeviceHandler>();

            _ = _trezorDeviceHandler.Setup(dh => dh.DeviceId).Returns("123");

            var inputReport = new Report
            (
                0,
                new TransferResult(new byte[]
                { 
                    //Blank out the Report Id because the Windows / UWP handler would do this for us
                    //0,
                63, 35, 35, 0, 17, 0, 0, 0, 142, 10, 17, 98, 105, 116, 99, 111, 105, 110, 116, 114, 101, 122, 111, 114, 46, 99, 111, 109, 16, 1, 24, 6, 32, 3, 50, 24, 66, 70, 67, 69, 48, 52, 68, 52, 67, 51, 69, 68, 53, 51, 70, 68, 51, 66, 67, 57, 53, 53, 48, 54, 56, 0, 64, 1 },
                65)
            );

            _ = _trezorDeviceHandler.Setup(dh => dh.ReadReportAsync(It.IsAny<CancellationToken>())).ReturnsAsync(inputReport);

            //Create an actual device
            var hidDevice = new HidDevice(_trezorDeviceHandler.Object, loggerFactory, readReportTransform: readReportTransform);

            //Set up the factory calls
            _ = deviceFactory.Setup(df => df.GetConnectedDeviceDefinitionsAsync(It.IsAny<CancellationToken>())).ReturnsAsync(
                new List<ConnectedDeviceDefinition>
                {
                    new ConnectedDeviceDefinition(_trezorDeviceHandler.Object.DeviceId, DeviceType.Hid)
                }); ;

            _ = deviceFactory.Setup(df => df.GetDeviceAsync(It.IsAny<ConnectedDeviceDefinition>(), It.IsAny<CancellationToken>())).ReturnsAsync(hidDevice);

            return deviceFactory.Object;
        }
        #endregion Private Methods

    }
}