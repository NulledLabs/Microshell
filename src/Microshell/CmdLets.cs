using System;
using System.Collections;

namespace MicroShell
{
    internal class CmdLetTable : Hashtable
    {
        public void Add(string index, object o)
        {
            base.Add(index, o);
        }

        public object this[string index]
        {
            get
            {
                return base[index];
            }
            set
            {
                base[index] = value;
            }
        }
    }
}
