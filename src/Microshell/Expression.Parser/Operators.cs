using System;
using Microsoft.SPOT;
using System.Collections.Generic;

namespace Microshell.Expression.Parser
{
    public class OperatorTable
    {
#if NETMF
      private static readonly Hashtable _Operators;
#else
        private static Dictionary<string, Operator> _Operators;
#endif

        public OperatorTable()
        {
#if NETMF
            _Operators = new Hashtable(52);
#else
            _Operators = new Dictionary<string, Operator>();
#endif
        }

#if NETMF
        public static void Add(IEnumerable operators)
#else
        public void Add(IEnumerable<Operator> operators)
#endif
        {
            foreach (Operator op in operators)
            {
                _Operators.Add(op.Symbol, op);
            }
        }

        public Operator this[string index]    // Indexer declaration
        {
            get
            {
                return (Operator)_Operators[index];
            }
            set
            {
                _Operators[index] = value;
            }
        }

        internal bool ContainsKey(string key)
        {
            return _Operators.ContainsKey(key);
        }
    }
}