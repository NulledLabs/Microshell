using System;

namespace Microshell.Expression.Parser
{
    internal class OperatorNode : Node
    {
        private readonly Operator _operator;
        private readonly Node _arg1;
        private readonly Node _arg2;

        /// <summary>
        /// Creates a Node containing the specified Operator and arguments.
        /// This will automatically mark this Node as a TYPE_EXPRESSION
        /// </summary>
        /// <param name="parser"></param>
        /// <param name="op">the string representing an operator</param>
        /// <param name="arg1">the first argument to the specified operator</param>
        /// <param name="arg2">the second argument to the specified operator</param>
        internal OperatorNode(Operator op, Node arg1, Node arg2)
        {
            _arg1 = arg1;
            _arg2 = arg2;
            _operator = op;
        }

        /// <summary>
        /// Creates a Node containing the specified Operator and argument.
        /// This will automatically mark this Node as a TYPE_EXPRESSION
        /// </summary>
        /// <param name="parser"></param>
        /// <param name="op">the string representing an operator</param>
        /// <param name="arg1">the argument to the specified operator</param>
        internal OperatorNode(Operator op, Node arg1)
        {
            _arg1 = arg1;
            _operator = op;
        }

        /// <summary>
        /// Returns the String operator of this Node 
        /// </summary>
        internal Operator Operator
        {
            get { return _operator; }
        }

        /// <summary>
        /// Returns the first argument of this Node
        /// </summary>
        internal Node Arg1
        {
            get { return _arg1; }
        }

        /// <summary>
        /// Returns the second argument of this Node
        /// </summary>
        internal Node Arg2
        {
            get { return _arg2; }
        }

        internal override Type GetReturnType()
        {
            if (Arg2 != null)
            {
                return Operator.GetReturnType(Arg1.GetReturnType(), Arg2.GetReturnType());
            }
            return Operator.GetReturnType(Arg1.GetReturnType(), null);
        }

        internal override object Evaluate(object context)
        {
            if (Arg2 != null)
            {
                return Operator.Calculate(Arg1.Evaluate(context), Arg2.Evaluate(context));
            }
            return Operator.Calculate(Arg1.Evaluate(context), null);
        }
    }
}
