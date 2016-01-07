using System;

namespace Microshell
{
    public abstract class MsCmdLet
    {
        protected virtual string GetHelp()
        {
            return "Help, I need somebody";
        }

        protected virtual void BeginProcessing()
        {

        }

        protected virtual void ProcessRecord()
        {

        }

        protected virtual void EndProcessing()
        {

        }
    }
}
