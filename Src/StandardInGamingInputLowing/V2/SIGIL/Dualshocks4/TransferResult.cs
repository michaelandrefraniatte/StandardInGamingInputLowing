namespace DeviceHandle
{
    /// <summary>
    /// Represents the result of a read or write transfer
    /// </summary>
    public readonly struct TransferResult
    {
        /// <summary>
        /// The data that was transferred
        /// </summary>
        public byte[] Data { get; }

        /// <summary>
        /// The number of bytes transferred
        /// </summary>
        public uint BytesTransferred { get; }
        
        public static implicit operator byte[](TransferResult TransferResult) => TransferResult.Data;

        /// <summary>
        /// This automatically converts an array of bytes to <see cref="TransferResult"/>. TODO: Remove this because it is too easy to swallow up the information of how many bytes were actually read
        /// </summary>
        /// <param name="data"></param>
        public static implicit operator TransferResult(byte[] data) =>
            new TransferResult(data, data != null ? (uint)data.Length : 0);
        
        public TransferResult(byte[] data, uint bytesRead)
        {
            Data = data;
            BytesTransferred = bytesRead;
        }
        
        public override string ToString() => $"Bytes transferred: {BytesTransferred}\r\n{string.Join(", ", Data)}";
    }
}