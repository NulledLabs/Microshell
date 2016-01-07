using System;
using System.Collections;
using System.Linq;
using System.Reflection;
using Microshell.Attributes;
using System.Diagnostics;
using Microshell.Expression.Parser;
using System.Text;

namespace MicroShell
{

    public class Shell
    {
        string[] cmds;

        string cmdlet;
        string[] p;

        CmdLetTable cmdlets = new CmdLetTable();

        //var expParser = new ExpressionParser(GetVarTypeCallback, GetVarValueCallback);
        ExpressionParser expParser = new ExpressionParser();

        public Shell()
        {
            Initialize();

            //StartupBullshit();

            Console.WriteLine("Microshell");
            StringBuilder command = new StringBuilder();
            command.Append("");

            ConsoleKeyInfo key;

            Console.Write("PS > ");

            int cursorLine = 0;
            int cursorChar = 0;

            do
            {
                key = Console.ReadKey();

                if (key.Key == ConsoleKey.Enter)
                {
                    Console.WriteLine();
                    if (!key.Modifiers.HasFlag(ConsoleModifiers.Shift))
                    {
                        try
                        {

                            Run(command.ToString());
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message.ToString());
                            //Console.WriteLine(ex.ToString());
                        }
                        command.Clear();

                        //Console.WriteLine();
                        Console.Write("PS > ");
                    }
                    else if (key.Modifiers.HasFlag(ConsoleModifiers.Shift))
                    {
                        command.Append("\n");

                        //Console.WriteLine();
                        Console.Write(">> ");
                    }
                }
                if (key.Key == ConsoleKey.Backspace)
                {
                    command.Length--;

                    if (Console.CursorLeft <= 3)
                    {

                    }
                    else
                    {
                        Console.Write('\0');
                        Console.CursorLeft--;
                    }
                }
                else
                {
                    command.Append(key.KeyChar);
                    //Console.CursorLeft++;
                }
            } while (command.ToString() != "exit");
        }

        void Initialize()
        {
            cmdlets.Add("start-sleep", new Hashtable() {
                { "test", "MicroShell.Sleep.StartSleep" }
            });
        }

        void StartupBullshit()
        {
            debug("Initializing Soft bits");
            debug("Initializing Hard bits");

            debug("Synchonizing LEO Satellite Coordinates");

            debug("Verifying Chinese Panda Population");
            debug("....................");
            debug("Pandas... They do exist.");
            debug("");
            debug("Checksumming CRC's");
            debug("32-bit access acknowledged");
            debug("");
            debug("Cracking 512 bit Vernam encryption firewall.");
            debug("Firewall successfully penetrated!");
            debug("");
            debug("Vroom-vroom! Weeeeeeeeeeee.......");
            debug("");
        }

        public void Run(string expression)
        {
            //cmds = command.Trim().Split(';');

            //Parse();
            //Test();
            //Console.WriteLine("Command: \\" + command + "\\");

            //expression = "$Var1 + 2 * 3";

            expParser.ParseExpression(expression);
            object eval = expParser.Evaluate(null);

            //if (eval is OperatorNode || eval is VariableNode)
            if (eval.GetType().IsPrimitive || eval.Equals(typeof(string)))
            {
                Debug.Print(String.Concat(expression, " = ", eval, "\t(", expParser.GetReturnType(), ")"));
            }
        }

        public void Parse()
        {
            foreach (string command in cmds)
            {
                p = command.Trim().Split(' ');

                cmdlet = p[0].Trim();

                p = p.Skip(1).ToArray();

                ArrayList parms = new ArrayList();

                foreach (string parm in p)
                {
                    int result;
                    bool success = Int32.TryParse(parm, out result);

                    if (success)
                    {
                        parms.Add(result);
                    }
                    else
                    {
                        parms.Add(parm);
                    }
                }
                //Array.Copy(parms, 1, parms, 0, 1);

                debug();

                string cmd = (string)cmdlets[cmdlet];

                if (cmd != null)
                {
                    int ind = cmd.LastIndexOf('.');
                    string obj = cmd.Substring(0, ind);
                    string method = cmd.Substring(ind + 1);

                    Type t = Type.GetType(obj);
                    MethodInfo m = t.GetMethod(method, BindingFlags.Public | BindingFlags.Instance);

                    Type[] parameterTypes = null;
                    object[] parameters = null;

                    //ConstructorInfo constructor = t.GetConstructor(parameterTypes);
                    ConstructorInfo constructor = t.GetConstructor(Type.EmptyTypes);
                    object o = constructor.Invoke(parameters);

                    var properties = t.GetProperties();

                    foreach (var property in properties)
                    {
                        //var attr = property.GetCustomAttribute(typeof(ParameterAttributes));
                        var attrs = property.GetCustomAttributes(true);

                        foreach (var attr in attrs)
                        {
                            //if (attr.GetType() == typeof(Microshell.Attributes.ParameterAttributes))
                            //{
                            //var p = (Microshell.Attributes.ParameterAttributes)attr;
                            //}
                        }
                        //var p = (Microshell.Attributes.ParameterAttributes)attr;
                    }

                    MethodInfo process = t.GetMethod("ProcessRecord");
                    process.Invoke(o, null);

                    m.Invoke(o, parms.ToArray());
                }
            }
        }
        //public void Test()
        //{
        //    Debug.Print("");
        //    Debug.Print("============================================================================");

        //    // print the values and types of the variables
        //    Debug.Print(String.Concat("$Var1 = ", GetVarValueCallback("$Var1", null), "\t(", GetVarTypeCallback("$Var1"), ")"));
        //    Debug.Print(String.Concat("$VarS = ", GetVarValueCallback("$VarS", null), "\t(", GetVarTypeCallback("$VarS"), ")"));
        //    Debug.Print(String.Concat("$VarB = ", GetVarValueCallback("$VarB", null), "\t(", GetVarTypeCallback("$VarB"), ")"));
        //    Debug.Print("");

        //    string expression = "";
        //    // simple numeric expression
        //    var expresion1 = new ExpressionParser(GetVarTypeCallback, GetVarValueCallback);
        //    expression = "$Var1 + 2 * 3";
        //    expresion1.ParseExpression(expression);
        //    Debug.Print(String.Concat(expression, " = ", expresion1.Evaluate(null), "\t(", expresion1.GetReturnType(), ")"));

        //    // numeric expression with braces
        //    var expresion2 = new ExpressionParser(GetVarTypeCallback, GetVarValueCallback);
        //    expression = "($Var1 + 2) * 3";
        //    expresion2.ParseExpression(expression);
        //    Debug.Print(String.Concat(expression, " = ", expresion2.Evaluate(null), "\t(", expresion2.GetReturnType(), ")"));

        //    // string concatenation
        //    var expresion3 = new ExpressionParser(GetVarTypeCallback, GetVarValueCallback);
        //    expression = "\"Hello \" + $VarS";
        //    expresion3.ParseExpression(expression);
        //    Debug.Print(String.Concat(expression, " = ", expresion3.Evaluate(null), "\t(", expresion3.GetReturnType(), ")"));

        //    // numeric comparission
        //    var expresion4 = new ExpressionParser(GetVarTypeCallback, GetVarValueCallback);
        //    expression = "$Var1 > 0.5";
        //    expresion4.ParseExpression(expression);
        //    Debug.Print(String.Concat(expression, " = ", expresion4.Evaluate(null), "\t(", expresion4.GetReturnType(), ")"));

        //    // boolean operations
        //    var expresion5 = new ExpressionParser(GetVarTypeCallback, GetVarValueCallback);
        //    expression = "$Var1 = 1.0 | $VarB";
        //    expresion5.ParseExpression(expression);
        //    Debug.Print(String.Concat(expression, " = ", expresion5.Evaluate(null), "\t(", expresion5.GetReturnType(), ")"));

        //    // mathematic functions
        //    var expresion6 = new ExpressionParser(GetVarTypeCallback, GetVarValueCallback);
        //    expression = "sin(0.5) + cos(0.5)";
        //    expresion6.ParseExpression(expression);
        //    Debug.Print(String.Concat(expression, " = ", expresion6.Evaluate(null), "\t(", expresion6.GetReturnType(), ")"));

        //    // get string length
        //    var expresion7 = new ExpressionParser(GetVarTypeCallback, GetVarValueCallback);
        //    expression = "strlen($VarS)";
        //    expresion7.ParseExpression(expression);
        //    Debug.Print(String.Concat(expression, " = ", expresion7.Evaluate(null), "\t(", expresion7.GetReturnType(), ")"));

        //    Debug.Print("============================================================================");
        //    Debug.Print("");
        //}

        private static Type GetVarTypeCallback(string varName)
        {
            // return the types of the variables
            if (varName == "$Var1")
                return typeof(double);
            if (varName == "$VarS")
                return typeof(string);
            if (varName == "$VarB")
                return typeof(bool);

            // return null for unknown variables
            return null;
        }

        private static object GetVarValueCallback(string varName, object context)
        {
            // return the values of the variables
            if (varName == "$Var1")
                return 1d;
            if (varName == "$VarS")
                return "World";
            if (varName == "$VarB")
                return false;

            // return null for unknown variables
            return null;
        }


        public void debug()
        {
            Console.WriteLine("Cmdlet: " + cmdlet);

            foreach (string parm in p)
            {
                Console.WriteLine("Parm:" + parm);
            }
        }

        static void debug(string message)
        {
            Console.WriteLine(message);
        }
    }
}
