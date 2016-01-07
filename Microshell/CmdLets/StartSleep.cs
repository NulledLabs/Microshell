using Microshell.Attributes;
using System;
using System.Threading;

namespace Microshell
{
    [Cmdlet(VerbsCommon.Start, "Sleep")]
    public class StartSleep : MsCmdLet
    {
        [Alias("s")]
        [Parameter]
        public int Seconds;

        [Alias("m")]
        [Parameter]
        public int Milliseconds;

        protected override void BeginProcessing()
        {
            throw new NotImplementedException();
        }

        protected override void ProcessRecord()
        {
            //Debug("Sleeping: " + milliseconds);
            Thread.Sleep(Milliseconds);
        }
    }
}
