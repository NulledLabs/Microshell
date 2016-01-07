using System;
using Microsoft.SPOT;

namespace Microshell.Expression.Parser
{
#if !NETMF
    public struct ExpressionVariable
    {
        public string Name;
        public int Start;
        public int Length;
    }
#endif
}
