using System;

namespace Device.Net
{
    /// <summary>
    /// Receives data from some source
    /// </summary>
    public interface IDataReceiver : IDisposable
    {
        /// <summary>
        /// Manually set the data that appears at the source
        /// </summary>
        /// <param name="bytes"></param>
        void DataReceived(TransferResult bytes);

        /// <summary>
        /// Whether or not data has already been received that has not yet been read
        /// </summary>
        bool HasData { get; }
    }
}