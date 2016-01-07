using System;
using System.Runtime.Serialization;

namespace Microshell.Expression.Parser
{
    [Serializable]
    internal class SessionStateUnauthorizedAccessException : Exception
    {
        public SessionStateUnauthorizedAccessException()
        {
        }

        public SessionStateUnauthorizedAccessException(string message) : base(message)
        {
        }

        public SessionStateUnauthorizedAccessException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected SessionStateUnauthorizedAccessException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}