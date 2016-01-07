using System;

namespace Microshell.Expression.Parser
{
    /// <summary>
    /// Exception class for parser related exceptions
    /// </summary>
    public class ParserException : Exception
    {
        public ParserException()
        { }

        public ParserException(string message)
           : base(message)
        { }

        public ParserException(string message, Exception inner)
           : base(message, inner)
        { }
    }
}
