using System;
using System.Runtime.Serialization;

namespace Middleman.Exceptions
{
    /// <summary>
    /// Base exception class for all dispatcher exceptions.
    /// </summary>
    public class DispatcherException : Exception
    {
        public DispatcherException()
        {
        }

        public DispatcherException(string? message) : base(message)
        {
        }

        public DispatcherException(string? message, Exception? innerException) : base(message, innerException)
        {
        }

        protected DispatcherException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
