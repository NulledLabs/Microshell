using System;
using Microsoft.SPOT;
using System.Collections.Generic;
using System.Collections;

namespace Microshell.Expression.Parser
{
    public class VariableTable
    {

#if NETMF
        private readonly Hashtable _Variables;
        public static void Add(IEnumerable operators)
        {
            foreach (string key in variables)
            {
#else
        private readonly IDictionary<string, object> _Variables = new Dictionary<string, object>();
        public void Add(IDictionary<string, object> variables)
        {
            foreach (KeyValuePair<string, object> var in variables)
            {
                string key = var.Key;
#endif
                this.Add(key, variables[key]);
            }
        }

        internal void Add(string name, object value)
        {
            this._Variables[name] = value;
        }

        internal void Add(Hashtable variables)
        {
            foreach (DictionaryEntry var in variables)
            {
                this.Add((string)var.Key, var.Value);
            }
        }

        public object this[string index]    // Indexer declaration
        {
            get
            {
                return _Variables[index];
            }
            set
            {
                _Variables[index] = value;
            }
        }

        internal bool ContainsKey(string key)
        {
            return _Variables.ContainsKey(key);
        }
    }
}
