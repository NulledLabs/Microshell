using System;
using Microsoft.SPOT;

namespace Microshell.Expression.Parser
{
    internal class Token
    {
        public TokenTypes TokenType;
        public object Value;
        public Operator Operator;

        public int lineStart;
        public int lineCharStart;
        public int charStart;
        public int charEnd;

    }

    internal enum TokenTypes
    {
        End,
        Term,
        Operator,
        StringConst,
        Number,
        Variable,
        Assignment,
        NewLine,
        Pipe,
        Constant
    }
}
