using System;

namespace HidHandle
{
    internal static class HidExtensions
    {
        /// <summary>
        /// Shifts the array to the right and inserts the report id at index zero
        /// </summary>
        public static byte[] InsertReportIdAtIndexZero(this byte[] data, byte reportId)
        {
            if (data == null) throw new ArgumentNullException(nameof(data));

            var transformedData = InsertZeroAtIndexZero(data);

            //Set the report id at index 0
            transformedData[0] = reportId;

            return transformedData;
        }

        /// <summary>
        /// Removes the first byte of the array and shifts other elements to the left
        /// </summary>
        public static byte[] TrimFirstByte(this byte[] inputData)
        {
            var length = inputData.Length - 1;
            var data = new byte[length];
            Array.Copy(inputData, 1, data, 0, length);

            return data;
        }

        private static byte[] InsertZeroAtIndexZero(this byte[] data)
        {
            if (data == null) throw new ArgumentNullException(nameof(data));

            //Create a new array which is one byte larger 
            var transformedData = new byte[data.Length + 1];

            //copy the data to it without the report id at index 1
            Array.Copy(data, 0, transformedData, 1, data.Length);

            return transformedData;
        }

    }
}