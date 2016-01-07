using System;
using Microsoft.SPOT;
using System.Collections;

namespace Microshell
{
    public class Alias
    {
        private Hashtable aliases = new Hashtable()
        {
            { "cat", typeof(GetContent) },
            { "echo", typeof(WriteOutput) },
            { "help", typeof(GetHelp) },
            { "kill", typeof(StopProcess) },
            { "ls", typeof(GetChildItemCommand) },
            { "ps", typeof(GetProcessCommand) },
            { "sleep", typeof(StartSleep) },

        };
    }
}
