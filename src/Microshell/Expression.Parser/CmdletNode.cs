using System;
using Microsoft.SPOT;

namespace Microshell.Expression.Parser
{
    internal class CmdletNode : Node
    {
        internal override object Evaluate(object context)
        {
            throw new NotImplementedException();
        }

        internal override Type GetReturnType()
        {
            throw new NotImplementedException();
        }
    }
}
