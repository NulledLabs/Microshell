using System;

namespace Microshell.Expression.Parser
{
    internal class ConstantNode : Node
    {
        private readonly object _value;

        internal ConstantNode(Token token)
        {
            _token = token;
            _value = token.Value;
        }
        internal ConstantNode(object value)
        {
            _value = value;
        }

        /// <summary>
        /// Returns the value of this Node 
        /// </summary>
        internal object Value
        {
            get { return _value; }
        }

        internal override object Evaluate(object context)
        {
            return _value;
        }

        internal override Type GetReturnType()
        {
            return _value.GetType();
        }
    }
}
