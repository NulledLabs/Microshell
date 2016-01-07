using System;
using Microsoft.SPOT;

namespace Microshell.Expression.Parser
{
    internal class NoOpNode : Node
    {
        internal override object Evaluate(object context)
        {
            //throw new NotImplementedException();
            return context;
        }

        internal override Type GetReturnType()
        {
            //throw new NotImplementedException();
            return typeof(NoOpNode);
        }
    }
}
