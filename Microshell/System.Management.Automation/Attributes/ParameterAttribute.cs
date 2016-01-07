using System;

namespace Microshell.Attributes
{
    //TODO: Not complete
    //https://technet.microsoft.com/en-us/library/ms714348(v=vs.85).aspx
    public class ParameterAttribute : Attribute
    {
        public bool Mandatory;

        public string ParameterSetName;

        public int Position;
    }
}
