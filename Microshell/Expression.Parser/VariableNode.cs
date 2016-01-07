using System;

namespace Microshell.Expression.Parser
{
    internal class VariableNode : Node
    {
        public ExpressionParser Parser { get; private set; }

        private readonly string _variable = String.Empty;

        /// <summary>
        /// Creates a Node containing the specified variable.
        /// This will automatically mark this Node as a TYPE_VARIABLE
        /// </summary>
        /// <param name="parser"></param>
        /// <param name="variable">the string representing a variable</param>
        internal VariableNode(ExpressionParser parser, string variable)
        {
            Parser = parser;
            _variable = variable;
        }

        /// <summary>
        /// Returns the String variable of this Node 
        /// </summary>
        internal string Variable
        {
            get { return _variable; }
        }

        internal override object Evaluate(object context)
        {
            var value = Parser.GetVarValueDelegate(Variable, context);
            
            if (value == null)
            {
                throw new ParserException(String.Concat("The variable '", Variable, "' does not exist"));
            }
            return value;
        }

        internal override Type GetReturnType()
        {
            var type = Parser.GetVarTypeDelegate(Variable);

            if (type == null)
            {
                throw new ParserException(String.Concat("The variable '", Variable, "' does not exist"));
            }
            return type;
        }
    }
}
