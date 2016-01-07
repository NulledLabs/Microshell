using System;
using Microsoft.SPOT;
using System.Collections.Generic;
using System.Collections;

namespace Microshell.Expression.Parser
{
    public class ConstantTable
    {
        public ConstantTable()
        {


#if NETMF
            this.Add(new Hashtable(12)
#else
            this.Add(new Dictionary<string, object>()
#endif
            {
                //{"euler", Math.E},
                //{"pi", Math.PI},
                //{"nan", double.NaN},
                //{"infinity", double.PositiveInfinity},
                {"$true", true},
                {"$false", false},
                {"$null", new object() }
            });
        }
#if NETMF
        private readonly Hashtable _Constants;
        public static void Add(IEnumerable operators)
        {
            foreach (string key in variables)
            {
#else
        private readonly IDictionary<string, object> _Constants = new Dictionary<string, object>();
        public void Add(IDictionary<string, object> constants)
        {
            foreach (KeyValuePair<string, object> var in constants)
            {
                string key = var.Key;
#endif
                this.Add(key, constants[key]);
            }
        }

        internal void Add(string name, object value)
        {
            this._Constants[name] = value;
        }

        internal void Add(Hashtable constant)
        {
            foreach (DictionaryEntry var in constant)
            {
                this.Add((string)var.Key, var.Value);
            }
        }

        public object this[string index]    // Indexer declaration
        {
            get
            {
                return _Constants[index];
            }
            set
            {
                _Constants[index] = value;
            }
        }

        internal bool ContainsKey(string key)
        {
            return _Constants.ContainsKey(key);
        }

    }
}
