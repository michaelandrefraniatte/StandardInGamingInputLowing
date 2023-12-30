﻿using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.Serialization;

namespace controllersds4
{

    [SuppressMessage("ReSharper", "UnusedMember.Global")]
    public class VigemBusInvalidHandleException : Exception
    {
        public VigemBusInvalidHandleException()
        {
        }

        public VigemBusInvalidHandleException(string message)
            : base(message)
        {
        }

        public VigemBusInvalidHandleException(string format, params object[] args)
            : base(string.Format(format, args))
        {
        }

        public VigemBusInvalidHandleException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        public VigemBusInvalidHandleException(string format, Exception innerException, params object[] args)
            : base(string.Format(format, args), innerException)
        {
        }

        protected VigemBusInvalidHandleException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}