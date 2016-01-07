using System;
using MicroShell;
using Microsoft.SPOT;

namespace Microshell.Demo
{
    public class Program
    {
        public static void Main()
        {
            Debug.Print(Resources.GetString(Resources.StringResources.String1));

            MicroShell.Shell shell = new Shell();
        }
    }
}
