using System;

namespace Microshell.Expression.Parser
{
    /// <summary>
    /// Class Node, represents a Node in a tree data structure representation of a mathematical expression.
    /// </summary>
    internal abstract class Node
    {
        public Token _token;

        internal abstract object Evaluate(object context);

        internal abstract Type GetReturnType();
    }
}

