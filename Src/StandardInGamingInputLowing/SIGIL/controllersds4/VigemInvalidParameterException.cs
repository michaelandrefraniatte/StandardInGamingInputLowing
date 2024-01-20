﻿using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.Serialization;

namespace controllersds4
{

    [SuppressMessage("ReSharper", "UnusedMember.Global")]
    public class VigemInvalidParameterException : Exception
    {
        public VigemInvalidParameterException()
        {
        }

        public VigemInvalidParameterException(string message)
            : base(message)
        {
        }

        public VigemInvalidParameterException(string format, params object[] args)
            : base(string.Format(format, args))
        {
        }

        public VigemInvalidParameterException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        public VigemInvalidParameterException(string format, Exception innerException, params object[] args)
            : base(string.Format(format, args), innerException)
        {
        }

        protected VigemInvalidParameterException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}