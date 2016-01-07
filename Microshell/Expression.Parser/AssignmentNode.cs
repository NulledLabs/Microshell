using System;
using Microsoft.SPOT;

namespace Microshell.Expression.Parser
{
    internal class AssignmentNode : Node
    {
        private VariableTable variables;

        private Node arg1;
        private Node arg2;

        private object value;

       /* public AssignmentNode(object value)
        {
            this.value = value;
        }
        */

        public AssignmentNode(VariableTable variables, Node arg1, Node arg2)
        {
            this.variables = variables;
            this.arg1 = arg1;
            this.arg2 = arg2;
        }

        internal override object Evaluate(object context)
        {
            if (arg2 != null)
            {
                this.variables[((VariableNode)arg1).Variable] = arg2.Evaluate(context);
            }

            //return this.variables[((VariableNode)arg1).Variable];
            return new object();
        }

        internal override Type GetReturnType()
        {
            //return this.variables[((VariableNode)arg1).Variable].GetType();
            return typeof(object);
        }
    }
}
