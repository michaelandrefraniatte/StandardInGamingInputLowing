using System.IO;
using System.Threading.Tasks;

namespace HidHandle
{
    public interface IHidDeviceHandler
    {
        ConnectedDeviceDefinition ConnectedDeviceDefinition { get; }
        bool? IsReadOnly { get; }
        ushort? ReadBufferSize { get; }
        ushort? WriteBufferSize { get; }
        string DeviceId { get; }

        Task InitializeAsync();

        /// <summary>
        /// Writes data and allows you to specify the report id
        /// </summary>
        /// <param name="data"></param>
        /// <param name="reportId"></param>
        /// <returns></returns>
        Task<uint> WriteReportAsync(byte[] data, byte reportId);

        bool IsInitialized { get; }
        void Close();
        Stream GetFileStream();
    }
}