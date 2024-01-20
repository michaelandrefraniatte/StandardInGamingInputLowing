using System.Threading;
using System.Threading.Tasks;

namespace HidHandle
{
    ///<inheritdoc cref="IDevice"/>
    public interface IHidDevice : IDevice
    {
        /// <summary>
        /// Writes data and allows you to specify the report id
        /// </summary>
        /// <param name="data"></param>
        /// <param name="reportId"></param>
        /// <param name="cancellationToken">Allows you to cancel the operation</param>
        /// <returns></returns>
        Task<uint> WriteReportAsync(byte[] data, byte reportId, CancellationToken cancellationToken = default);
    }
}