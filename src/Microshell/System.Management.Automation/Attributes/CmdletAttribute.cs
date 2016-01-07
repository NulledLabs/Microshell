using System;
using Microsoft.SPOT;

namespace Microshell.Attributes
{
    public class CmdletAttribute : Attribute
    {
        private VerbsCommon start;
        private string v;

        public CmdletAttribute(VerbsCommon start, string v)
        {
            this.start = start;
            this.v = v;
        }

        public CmdletAttribute(string verbName, string nounName)
        {

        }
    }
}
