using System;
using System.Text;
using System.Collections;
#if NETMF
using System.Collections;
#else
using System.Globalization;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
#endif

namespace Microshell.Expression.Parser
{
    public class ExpressionParser
    {
#if NETMF
      public static bool IgnoreCaseStringComparison { get; set; }
#else
        public static StringComparison StringComparisonType { get; set; }
#endif
        private ConstantTable _Constants = new ConstantTable();

        private readonly VariableTable _Variables = new VariableTable();

        private readonly OperatorTable _Operators = new OperatorTable();

        private Hashtable _TreeTable;
        private int nodeIndex = 0;

        private Node _Tree;

        private readonly GetVarTypeDelegate _GetVarTypeDelegate;
        private readonly GetVarValueDelegate _GetVarValueDelegate;
        private string expression;

        internal GetVarTypeDelegate GetVarTypeDelegate
        {
            get { return _GetVarTypeDelegate; }
        }

        internal GetVarValueDelegate GetVarValueDelegate
        {
            get { return _GetVarValueDelegate; }
        }

        public ExpressionParser()
        {
#if NETMF
            IgnoreCaseStringComparison = true;
#else
            StringComparisonType = StringComparison.OrdinalIgnoreCase;
#endif
            _Variables.Add(new Dictionary<string, object>()
                {
                    { "$true", true},
                    { "$false", false}
                }
            );

            _Operators.Add(new[]
            {
               new Operator("..", 4),

               new Operator("+", 6)
                  .AddFunction(ValueType.String, ValueType.String, ValueType.String, OperatorFunctions.PlusTss)
                  .AddFunction(ValueType.Integer, ValueType.Integer, ValueType.Integer, OperatorFunctions.PlusTii)
                  .AddFunction(ValueType.Integer, ValueType.Double, ValueType.Double, OperatorFunctions.PlusTdd)
                  .AddFunction(ValueType.Double, ValueType.Integer, ValueType.Double, OperatorFunctions.PlusTdd)
                  .AddFunction(ValueType.Double, ValueType.Double, ValueType.Double, OperatorFunctions.PlusTdd),
               new Operator("-", 6)
                  .AddFunction(ValueType.Integer, ValueType.Integer, OperatorFunctions.MinusTi)
                  .AddFunction(ValueType.Double, ValueType.Double, OperatorFunctions.MinusTd)
                  .AddFunction(ValueType.Integer, ValueType.Integer, ValueType.Integer, OperatorFunctions.MinusTii)
                  .AddFunction(ValueType.Integer, ValueType.Double, ValueType.Double, OperatorFunctions.MinusTdd)
                  .AddFunction(ValueType.Double, ValueType.Integer, ValueType.Double, OperatorFunctions.MinusTdd)
                  .AddFunction(ValueType.Double, ValueType.Double, ValueType.Double, OperatorFunctions.MinusTdd),
               new Operator("/", 4)
                  .AddFunction(ValueType.Integer, ValueType.Integer, ValueType.Integer, OperatorFunctions.DivTii)
                  .AddFunction(ValueType.Integer, ValueType.Double, ValueType.Double, OperatorFunctions.DivTdd)
                  .AddFunction(ValueType.Double, ValueType.Integer, ValueType.Double, OperatorFunctions.DivTdd)
                  .AddFunction(ValueType.Double, ValueType.Double, ValueType.Double, OperatorFunctions.DivTdd),
               new Operator("*", 4)
                  .AddFunction(ValueType.Integer, ValueType.Integer, ValueType.Integer, OperatorFunctions.MultTii)
                  .AddFunction(ValueType.Integer, ValueType.Double, ValueType.Double, OperatorFunctions.MultTdd)
                  .AddFunction(ValueType.Double, ValueType.Integer, ValueType.Double, OperatorFunctions.MultTdd)
                  .AddFunction(ValueType.Double, ValueType.Double, ValueType.Double, OperatorFunctions.MultTdd),
               new Operator("%", 4)
                  .AddFunction(ValueType.Integer, ValueType.Integer, ValueType.Integer, OperatorFunctions.ModuloTii),
               new Operator("cos", 2)
                  .AddFunction(ValueType.Integer, ValueType.Double, OperatorFunctions.Cos)
                  .AddFunction(ValueType.Double, ValueType.Double, OperatorFunctions.Cos),
               new Operator("sin", 2)
                  .AddFunction(ValueType.Integer, ValueType.Double, OperatorFunctions.Sin)
                  .AddFunction(ValueType.Double, ValueType.Double, OperatorFunctions.Sin),
               new Operator("exp", 2)
                  .AddFunction(ValueType.Integer, ValueType.Double, OperatorFunctions.Exp)
                  .AddFunction(ValueType.Double, ValueType.Double, OperatorFunctions.Exp),
               new Operator("ln", 2)
                  .AddFunction(ValueType.Integer, ValueType.Double, OperatorFunctions.Ln)
                  .AddFunction(ValueType.Double, ValueType.Double, OperatorFunctions.Ln),
               new Operator("tan", 2)
                  .AddFunction(ValueType.Integer, ValueType.Double, OperatorFunctions.Tan)
                  .AddFunction(ValueType.Double, ValueType.Double, OperatorFunctions.Tan),
               new Operator("acos", 2)
                  .AddFunction(ValueType.Integer, ValueType.Double, OperatorFunctions.Acos)
                  .AddFunction(ValueType.Double, ValueType.Double, OperatorFunctions.Acos),
               new Operator("asin", 2)
                  .AddFunction(ValueType.Integer, ValueType.Double, OperatorFunctions.Asin)
                  .AddFunction(ValueType.Double, ValueType.Double, OperatorFunctions.Asin),
               new Operator("atan", 2)
                  .AddFunction(ValueType.Integer, ValueType.Double, OperatorFunctions.Atan)
                  .AddFunction(ValueType.Double, ValueType.Double, OperatorFunctions.Atan),
               new Operator("cosh", 2)
                  .AddFunction(ValueType.Integer, ValueType.Double, OperatorFunctions.Cosh)
                  .AddFunction(ValueType.Double, ValueType.Double, OperatorFunctions.Cosh),
               new Operator("sinh", 2)
                  .AddFunction(ValueType.Integer, ValueType.Double, OperatorFunctions.Sinh)
                  .AddFunction(ValueType.Double, ValueType.Double, OperatorFunctions.Sinh),
               new Operator("tanh", 2)
                  .AddFunction(ValueType.Integer, ValueType.Double, OperatorFunctions.Tanh)
                  .AddFunction(ValueType.Double, ValueType.Double, OperatorFunctions.Tanh),
               new Operator("sqrt", 2)
                  .AddFunction(ValueType.Integer, ValueType.Double, OperatorFunctions.Sqrt)
                  .AddFunction(ValueType.Double, ValueType.Double, OperatorFunctions.Sqrt),
               new Operator("cotan", 2)
                  .AddFunction(ValueType.Integer, ValueType.Double, OperatorFunctions.Cotan)
                  .AddFunction(ValueType.Double, ValueType.Double, OperatorFunctions.Cotan),
               new Operator("fpart", 2)
                  .AddFunction(ValueType.Integer, ValueType.Double, OperatorFunctions.Fpart)
                  .AddFunction(ValueType.Double, ValueType.Double, OperatorFunctions.Fpart),
               new Operator("acotan", 2)
                  .AddFunction(ValueType.Integer, ValueType.Double, OperatorFunctions.Acotan)
                  .AddFunction(ValueType.Double, ValueType.Double, OperatorFunctions.Acotan),
               new Operator("round", 2)
                  .AddFunction(ValueType.Integer, ValueType.Integer, OperatorFunctions.Round)
                  .AddFunction(ValueType.Double, ValueType.Integer, OperatorFunctions.Round),
               new Operator("ceil", 2)
                  .AddFunction(ValueType.Integer, ValueType.Integer, OperatorFunctions.Ceil)
                  .AddFunction(ValueType.Double, ValueType.Integer, OperatorFunctions.Ceil),
               new Operator("floor", 2)
                  .AddFunction(ValueType.Integer, ValueType.Integer, OperatorFunctions.Floor)
                  .AddFunction(ValueType.Double, ValueType.Integer, OperatorFunctions.Floor),
               new Operator("fac", 2)
                  .AddFunction(ValueType.Integer, ValueType.Integer, OperatorFunctions.Fac),
               new Operator("sfac", 2)
                  .AddFunction(ValueType.Integer, ValueType.Integer, OperatorFunctions.Sfac),
               new Operator("abs", 2)
                  .AddFunction(ValueType.Integer, ValueType.Integer, OperatorFunctions.AbsTi)
                  .AddFunction(ValueType.Double, ValueType.Double, OperatorFunctions.AbsTd),
               new Operator("log", 2)
                  .AddFunction(ValueType.Integer, ValueType.Double, OperatorFunctions.Log)
                  .AddFunction(ValueType.Double, ValueType.Double, OperatorFunctions.Log),
               //new Operator(">", 7)
               new Operator("-gt", 7)
                  .AddFunction(ValueType.String, ValueType.String, ValueType.Bool, OperatorFunctions.IsGreaterTss)
                  .AddFunction(ValueType.Integer, ValueType.Integer, ValueType.Bool, OperatorFunctions.IsGreaterTii)
                  .AddFunction(ValueType.Integer, ValueType.Double, ValueType.Bool, OperatorFunctions.IsGreaterTdd)
                  .AddFunction(ValueType.Double, ValueType.Integer, ValueType.Bool, OperatorFunctions.IsGreaterTdd)
                  .AddFunction(ValueType.Double, ValueType.Double, ValueType.Bool, OperatorFunctions.IsGreaterTdd)
                  .AddFunction(ValueType.String, ValueType.Integer, ValueType.String, OperatorFunctions.RightStr),
               //new Operator("<", 7)
               new Operator("-lt", 7)
                  .AddFunction(ValueType.String, ValueType.String, ValueType.Bool, OperatorFunctions.IsLessTss)
                  .AddFunction(ValueType.Integer, ValueType.Integer, ValueType.Bool, OperatorFunctions.IsLessTii)
                  .AddFunction(ValueType.Integer, ValueType.Double, ValueType.Bool, OperatorFunctions.IsLessTdd)
                  .AddFunction(ValueType.Double, ValueType.Integer, ValueType.Bool, OperatorFunctions.IsLessTdd)
                  .AddFunction(ValueType.Double, ValueType.Double, ValueType.Bool, OperatorFunctions.IsLessTdd)
                  .AddFunction(ValueType.String, ValueType.Integer, ValueType.String, OperatorFunctions.LeftStr),
               //new Operator(">=", 7)
               new Operator("-ge", 7)
                  .AddFunction(ValueType.String, ValueType.String, ValueType.Bool, OperatorFunctions.IsGreaterOrEqualTss)
                  .AddFunction(ValueType.Integer, ValueType.Integer, ValueType.Bool, OperatorFunctions.IsGreaterOrEqualTii)
                  .AddFunction(ValueType.Integer, ValueType.Double, ValueType.Bool, OperatorFunctions.IsGreaterOrEqualTdd)
                  .AddFunction(ValueType.Double, ValueType.Integer, ValueType.Bool, OperatorFunctions.IsGreaterOrEqualTdd)
                  .AddFunction(ValueType.Double, ValueType.Double, ValueType.Bool, OperatorFunctions.IsGreaterOrEqualTdd),
               //new Operator("<=", 7)
               new Operator("-le", 7)
                  .AddFunction(ValueType.String, ValueType.String, ValueType.Bool, OperatorFunctions.IsLessOrEqualTss)
                  .AddFunction(ValueType.Integer, ValueType.Integer, ValueType.Bool, OperatorFunctions.IsLessOrEqualTii)
                  .AddFunction(ValueType.Integer, ValueType.Double, ValueType.Bool, OperatorFunctions.IsLessOrEqualTdd)
                  .AddFunction(ValueType.Double, ValueType.Integer, ValueType.Bool, OperatorFunctions.IsLessOrEqualTdd)
                  .AddFunction(ValueType.Double, ValueType.Double, ValueType.Bool, OperatorFunctions.IsLessOrEqualTdd),
               //new Operator("=", 7)
               //     .AddFunction(ValueType.None, ValueType.None, (obj1, obj2) => { return this._Variables[(string)obj1] = obj2; }),
               new Operator("-eq", 7)
                  .AddFunction(ValueType.String, ValueType.String, ValueType.Bool, OperatorFunctions.IsEqualTss)
                  .AddFunction(ValueType.Bool, ValueType.Bool, ValueType.Bool, OperatorFunctions.IsEqualTbb)
                  .AddFunction(ValueType.Integer, ValueType.Integer, ValueType.Bool, OperatorFunctions.IsEqualTii)
                  .AddFunction(ValueType.Integer, ValueType.Double, ValueType.Bool, OperatorFunctions.IsEqualTdd)
                  .AddFunction(ValueType.Double, ValueType.Integer, ValueType.Bool, OperatorFunctions.IsEqualTdd)
                  .AddFunction(ValueType.Double, ValueType.Double, ValueType.Bool, OperatorFunctions.IsEqualTdd),
               //new Operator("!=", 7)
               new Operator("-ne", 7)
                  .AddFunction(ValueType.String, ValueType.String, ValueType.Bool, OperatorFunctions.IsInEqualTss)
                  .AddFunction(ValueType.Bool, ValueType.Bool, ValueType.Bool, OperatorFunctions.IsInEqualTbb)
                  .AddFunction(ValueType.Integer, ValueType.Integer, ValueType.Bool, OperatorFunctions.IsInEqualTii)
                  .AddFunction(ValueType.Integer, ValueType.Double, ValueType.Bool, OperatorFunctions.IsInEqualTdd)
                  .AddFunction(ValueType.Double, ValueType.Integer, ValueType.Bool, OperatorFunctions.IsInEqualTdd)
                  .AddFunction(ValueType.Double, ValueType.Double, ValueType.Bool, OperatorFunctions.IsInEqualTdd),
               new Operator("^", 3)
                  .AddFunction(ValueType.Bool, ValueType.Bool, ValueType.Bool, OperatorFunctions.XorTbb),
               new Operator("&", 8)
                  .AddFunction(ValueType.Bool, ValueType.Bool, ValueType.Bool, OperatorFunctions.AndTbb),
               new Operator("|", 9)
                  .AddFunction(ValueType.Bool, ValueType.Bool, ValueType.Bool, OperatorFunctions.OrTbb),
               new Operator("strlen", 2)
                  .AddFunction(ValueType.String, ValueType.Integer, OperatorFunctions.StrLen),
               new Operator("!", 1)
                  .AddFunction(ValueType.Bool, ValueType.Bool, OperatorFunctions.NotTb)
            });

#if NETMF
            _Constants.Add(new Hashtable(12)
#else
            _Constants.Add(new Dictionary<string, object>()
#endif
            {
                {"euler", Math.E},
                {"pi", Math.PI},
                {"nan", double.NaN},
                {"infinity", double.PositiveInfinity},
                {"$true", true},
                {"$false", false}
            });


            if (_GetVarTypeDelegate == null)
            {
                _GetVarTypeDelegate = GetHashtableVarType;
            }
            if (_GetVarValueDelegate == null)
            {
                _GetVarValueDelegate = GetHashtableVarValue;
            }
        }

        /// <summary>
        /// Default constructor, creates an ExpressionParser object
        /// </summary>
        public ExpressionParser(Hashtable variables) : this()
        {
            if (variables == null)
            {
                throw new ArgumentNullException("variables");
            }

            _Variables.Add(variables);
        }

#if !NETMF
        public ExpressionParser(IDictionary<string, object> variables) : this()
        {
            if (variables == null)
            {
                throw new ArgumentNullException("variables");
            }

            _Variables.Add(variables);
        }
#endif

        //public ExpressionParser(GetVarTypeDelegate getVarTypeDelegate, GetVarValueDelegate getVarValueDelegate) : this()
        //{
        //    _GetVarTypeDelegate = getVarTypeDelegate;
        //    _GetVarValueDelegate = getVarValueDelegate;
        //}

        public void ParseExpression(string expression)
        {
            this.expression = expression;
            _Tree = ParseNodeTree(expression);
        }

        public bool CheckReturnType(Type desiredType)
        {
            var type = GetReturnType();
            if (type == desiredType)
            {
                return true;
            }

            if (type == typeof(int) && desiredType == typeof(double))
            {
                return true;
            }

            return false;
        }

        public Type GetReturnType()
        {
            try
            {
                return _Tree.GetReturnType();
            }
            catch (Exception e)
            {
                throw new ParserException(e.Message);
            }
        }

        public string EvaluateString(object context)
        {
            var value = Evaluate(context);
            if (value is string)
            {
                return (string)value;
            }
            throw new ParserException("Result value is not of type string");
        }

        public bool EvaluateBool(object context)
        {
            var value = Evaluate(context);
            if (value is bool)
            {
                return (bool)value;
            }
            throw new ParserException("Result value is not of type bool");
        }

        public int EvaluateInteger(object context)
        {
            var value = Evaluate(context);
            if (value is int)
            {
                return (int)value;
            }
            throw new ParserException("Result value is not of type integer");
        }

        public double EvaluateDouble(object context)
        {
            var value = Evaluate(context);
            if (value is double)
            {
                return (double)value;
            }
            if (value is int)
            {
                return (int)value;
            }
            throw new ParserException("Result value is not of type double or int");
        }

        public object Evaluate(object context)
        {
            try
            {
                return _Tree.Evaluate(context);
            }
            catch (Exception e)
            {
                throw new ParserException(e.Message);
            }
        }

        private Node ParseNodeTree(string expression)
        {
#if NETMF
         var tokens = new ArrayList();
#else
            var tokens = new List<Token>();
#endif
            int i = 0;
            int len = expression.Length;
            int start;

            int iline = 1;
            int ichar = 1;

            while (i < len)
            {
                tokens.Add(GetNextToken(expression, ref i, out start, ref iline, ref ichar));
            }
            return ParseTokens(tokens);
        }

#if NETMF
      private Node ParseTokens(IEnumerable tokens)
#else
        private Node ParseTokens(IEnumerable<Token> tokens)
#endif
        {
            // transform all but operators to nodes
#if NETMF
         var list = new ArrayList();
#else
            var list = new List<object>();
#endif

            int opCount = 0;


            foreach (Token token in tokens)
            {
                switch (token.TokenType)
                {
                    case TokenTypes.End:
                        //this._Tree = 
                        //this._TreeTable.Add();
                        this.nodeIndex++;
                        break;

                    case TokenTypes.Constant:
                        list.Add(new ConstantNode(token));
                        break;

                    case TokenTypes.Number:
                        list.Add(new ConstantNode(token));
                        break;

                    case TokenTypes.StringConst:
                        list.Add(new ConstantNode(token));
                        break;

                    case TokenTypes.Variable:
                        list.Add(new VariableNode(this, (string)token.Value));
                        break;
                    case TokenTypes.Assignment:
                    case TokenTypes.Operator:
                        // operator stays token
                        ++opCount;
                        list.Add(token);
                        break;

                    case TokenTypes.Term:
                        // a sub expression
                        list.Add(ParseNodeTree((string)token.Value));
                        break;

                    default:
                        throw new NotImplementedException("Interpretation of token type is not implemented");
                }
            }

            // now we have a list of nodes and operator tokens

            // next look for '-' operator at the beginning or after an operator token and transform it into a 1 arg operator
            for (int n = 0; n < list.Count - 1; ++n)
            {
                if (list[n] is Token)
                {
                    var token = (Token)list[n];
                    if (token.Operator.IsOperator("-") && (n == 0 || list[n - 1] is Token))
                    {
                        var nextNode = list[n + 1] as Node;
                        if (nextNode != null)
                        {
                            list[n] = new OperatorNode(token.Operator, nextNode);
                            list.RemoveAt(n + 1);
                            --opCount;
                        }
                    }
                }
            }

            // now transform all operators into nodes starting at the lowest precedence
            while (opCount > 0)
            {
                int prec = Int32.MaxValue;
                int idx = -1;
                for (int n = 0; n < list.Count; n++)
                {
                    if (list[n] is Token)
                    {
                        var t = (Token)list[n];
                        if (idx < 0 || t.Operator.Precedence < prec)
                        {
                            idx = n;
                            prec = t.Operator.Precedence;
                        }
                    }
                }

                if (idx < 0)
                {
                    break;
                }

                if (idx >= list.Count)
                {
                    throw new ParserException("Operator cannot be at end of expression");
                }

                var token = (Token)list[idx];
                var nextNode = list[idx + 1] as Node;

                if (nextNode == null)
                {
                    throw new ParserException("An operator cannot be followed by an operator. Use paranthesis.");
                }

                if (token.Operator.Is2ArgOp)
                {
                    if (idx == 0)
                    {
                        throw new ParserException("Operator cannot be at beginning of expression");
                    }

                    var prevNode = list[idx - 1] as Node;
                    if (prevNode == null)
                    {
                        throw new ParserException("An operator cannot be followed by an operator.");
                    }

                    if (token.TokenType == TokenTypes.Assignment)
                    {
                        if (prevNode is VariableNode)
                        {
                            /*
                            case TokenTypes.Assignment:
                                list.Add(new AssignmentNode(token.Value));
                                break;
                            */
                            if (nextNode != null)
                            {
                                list[idx] = new AssignmentNode(this._Variables, prevNode, nextNode);
                            }
                        }
                        else if (prevNode is ConstantNode)
                        {
                            throw new SessionStateUnauthorizedAccessException(String.Format(
@"Cannot overwrite variable {0} because it is read-only or constant.
At line:{1} char:{2}
+ $true = $false
+ ~~~~~~~~~~~~~~
    + CategoryInfo          : WriteError: ({0}:String) [], SessionStateUnauthorizedAccessException
    + FullyQualifiedErrorId : VariableNotWritable", (((ConstantNode)prevNode).Value), ((ConstantNode)prevNode)._token.lineStart, ((ConstantNode)prevNode)._token.lineCharStart));
                        }

                        else
                        {
                            throw new Exception(String.Format(
@"At line:{0} char:{1}
+ {2}
+ ~
The assignment expression is not valid. The input to an assignment operator must be an object that is able to accept
assignments, such as a variable or a property.
    + CategoryInfo          : ParserError: (:) [], ParentContainsErrorRecordException
    + FullyQualifiedErrorId : InvalidLeftHandSide
", prevNode._token.lineStart, prevNode._token.lineCharStart, this.expression.Substring(prevNode._token.charStart, nextNode._token.charEnd)
)
);
                        }
                    }
                    /*else if (token.TokenType == TokenTypes.Pipe)
                    {

                    }*/
                    else
                    {
                        list[idx] = new OperatorNode(token.Operator, prevNode, nextNode);
                    }
                    list.RemoveAt(idx + 1);
                    list.RemoveAt(idx - 1);
                }
                else
                {
                    list[idx] = new OperatorNode(token.Operator, nextNode);
                    list.RemoveAt(idx + 1);
                }
                --opCount;
            }

            if (list.Count > 1)
            {
                throw new ParserException("Expression cannot be translated into a single node! Missing operators?");
            }
            else if (list.Count == 1)
            {
                return list[0] as Node;
            }
            else
            {
                return new NoOpNode();
            }
        }

        private static string UnescapeString(string exp)
        {
            // exp always starts and ends with quotes
            var sb = new StringBuilder(exp.Length - 2);
            for (int n = 1; n < exp.Length - 1; ++n)
            {
                if (exp[n] == '\\' && n < exp.Length - 2)
                {
                    ++n;
                    switch (exp[n])
                    {
                        case 'R':
                        case 'r':
                            sb.Append('\r');
                            break;

                        case 'N':
                        case 'n':
                            sb.Append('\n');
                            break;

                        case 'T':
                        case 't':
                            sb.Append('\t');
                            break;

                        default:
                            sb.Append(exp[n]);
                            break;
                    }
                }
                else
                {
                    sb.Append(exp[n]);
                }
            }
            return sb.ToString();
        }
        /// <summary>
        /// Checks to see if the string is the name of an assignment operator.
        /// </summary>
        /// <param name="str">The string to check</param>
        /// <returns>true if it is an assignment operator, false otherwise.</returns>
        private static bool IsAssignmentOperator(String str)
        {
#if NETMF
            return (str.ToLower()) == "=");
#else
            return (str.ToLowerInvariant() == "=");
#endif
        }

        /// <summary>
        /// Checks to see if the string is the name of a acceptable operator.
        /// </summary>
        /// <param name="str">The string to check</param>
        /// <returns>true if it is an acceptable operator, false otherwise.</returns>
        private bool IsOperator(String str)
        {
#if NETMF
            return _Operators.Contains(str.ToLower());
#else
            return _Operators.ContainsKey(str.ToLowerInvariant());
#endif
        }

        /// <summary>
        /// Checks to see if the string is the name of a acceptable operator.
        /// </summary>
        /// <param name="str">The string to check</param>
        /// <returns>true if it is an acceptable operator, false otherwise.</returns>
        private bool IsConstant(String str)
        {
#if NETMF
            return _Constants.Contains(str.ToLower());
#else
            return _Constants.ContainsKey(str.ToLowerInvariant());
#endif
        }

        private static bool IsVariableSym(char s)
        {
            return (s == '$');
        }

        private static bool IsOperatorSym(char s)
        {
            return (
                s == '%' ||
                s == '!' ||
                s == '>' ||
                s == '<' ||
                s == '&' ||
                s == '=' ||
                s == '|' ||
                s == '^' ||
                s == '+' ||
                s == '-' ||
                s == '/' ||
                s == '*'
            );
        }

        private static bool IsWhiteSpace(char c)
        {
#if NETMF
         return (c == ' ' || c == '\t' || c == '\r' || c == '\n');
#else
            return Char.IsWhiteSpace(c);
#endif
        }

        private static bool IsLetter(char c)
        {
#if NETMF
         return (c >= 'a' && c <= 'z') || (c >= 'A' && c <= 'Z');
#else
            return Char.IsLetter(c);
#endif
        }

        private static bool IsDigit(char c)
        {
#if NETMF
         return (c >= '0' && c <= '9');
#else
            return Char.IsDigit(c);
#endif
        }

        private static bool IsLetterOrDigit(char c)
        {
#if NETMF
         return IsLetter(c) || IsDigit(c);
#else
            return Char.IsLetterOrDigit(c);
#endif
        }

        //#if !NETMF
        //        public static IEnumerable<ExpressionVariable> EnumerateVariables(string expression)
        //        {
        //            int index = 0;
        //            int start;
        //            int len = expression.Length;
        //            int iline = 0;
        //            int ichar = 0;

        //            while (index < len)
        //            {
        //                var token = GetNextToken(expression, ref index, out start, ref iline, ref ichar);
        //                if (token.TokenType == TokenTypes.Variable)
        //                {
        //                    yield return new ExpressionVariable
        //                    {
        //                        Name = (string)token.Value,
        //                        Start = start,
        //                        Length = index - start
        //                    };
        //                }
        //                else if (token.TokenType == TokenTypes.Term)
        //                {
        //                    foreach (var subVar in EnumerateVariables((string)token.Value))
        //                    {
        //                        yield return new ExpressionVariable
        //                        {
        //                            Name = subVar.Name,
        //                            Start = start + 1 + subVar.Start, // offset of term + 1 for bracket
        //                            Length = subVar.Length
        //                        };
        //                    }
        //                }
        //            }
        //        }
        //#endif

        private Token GetNextToken(string exp, ref int index, out int start, ref int iline, ref int ichar)
        {
            Token token = new InvalidToken();

            while (index < exp.Length && IsWhiteSpace(exp[index]))
            {
                if (exp[index] == '\n')
                {
                    ichar = 1;
                    ++iline;
                }
                ++ichar;

                ++index;
            }

            start = index;

            if (index >= exp.Length || start >= exp.Length || exp[index] == ';')
            {
                token = new Token {
                    TokenType = TokenTypes.End,
                    lineStart = iline,
                    lineCharStart = ichar,
                    charStart = start,
                    charEnd = index - start
                };
            }
            else if (exp[index] == '\n')
            {
                token = new Token {
                    TokenType = TokenTypes.NewLine,
                    lineStart = iline,
                    lineCharStart = ichar,
                    charStart = start,
                    charEnd = index - start
                };
            }
            else if (exp[index] == '(')
            {
                index = MatchCharPair(exp, start, '(', ')') + 1;
                token = new Token
                {
                    TokenType = TokenTypes.Term,
                    Value = exp.Substring(start + 1, (index - start) - 2),
                    lineStart = iline,
                    lineCharStart = ichar,
                    charStart = start,
                    charEnd = index - start
                };
            }
            else if (exp[index] == '{')
            {
                index = MatchCharPair(exp, start, '{', '}') + 1;
                token = new Token
                {
                    TokenType = TokenTypes.Term,
                    Value = exp.Substring(start + 1, (index - start) - 2),
                    lineStart = iline,
                    lineCharStart = ichar,
                    charStart = start,
                    charEnd = index - start
                };
            }
            else if (exp[index] == '[')
            {
                index = MatchCharPair(exp, start, '[', ']') + 1;
                token = new Token
                {
                    TokenType = TokenTypes.Term,
                    Value = exp.Substring(start + 1, (index - start) - 2),
                    lineStart = iline,
                    lineCharStart = ichar,
                    charStart = start,
                    charEnd = index - start
                };
            }
            else if (exp[index] == '\"')
            {
                index = MatchQuote(exp, start) + 1;
                token = new Token
                {
                    TokenType = TokenTypes.StringConst,
                    Value = UnescapeString(exp.Substring(start, index - start)),
                    lineStart = iline,
                    lineCharStart = ichar,
                    charStart = start,
                    charEnd = index - start
                };
            }
            else if (IsLetter(exp[index]) || exp[index] == '_' || exp[index] == ':' || exp[index] == '.' || exp[index] == '$')
            {
                ++ichar;
                ++index;

                while (index < exp.Length &&
                   (IsLetterOrDigit(exp[index]) || exp[index] == '-' || exp[index] == '_' || exp[index] == '.'))
                {
                    ++ichar;
                    if (exp[index] == '\n')
                    {
                        ichar = 1;
                        ++iline;
                    }

                    ++index;
                }

                var name = exp.Substring(start, index - start);
                if (IsCmdlet(name))
                {

                }
                else if (IsOperator(name))
                {
                    token = new Token
                    {
                        TokenType = TokenTypes.Operator,
#if NETMF
                        Operator = _Operators[name.ToLower()] as Operator
#else
                        Operator = _Operators[name.ToLowerInvariant()]
#endif
                        ,
                        lineStart = iline,
                        lineCharStart = ichar,
                        charStart = start,
                        charEnd = index - start
                    };
                }
                else if (IsConstant(name))
                {
                    token = new Token
                    {
                        TokenType = TokenTypes.Constant,
                        Value = _Constants[name.ToLower()],
                        //Value = name,
                        lineStart = iline,
                        lineCharStart = ichar,
                        charStart = start,
                        charEnd = index - start
                    };
                }
                else if (IsVariable(name))
                {
                    if (!this._Variables.ContainsKey(name))
                    {
                        this._Variables.Add(name, new object());
                    }

                    token = new Token
                    {
                        TokenType = TokenTypes.Variable,
                        Value = name,
                        lineStart = iline,
                        lineCharStart = ichar,
                        charStart = start,
                        charEnd = index - start
                    };
                }
                else
                {
                    throw new CommandNotFoundException(String.Format(
@"{0} : The term '{0}' is not recognized as the name of a cmdlet, function, script file, or operable program. Check the spelling of the name, or if a path was included, verify that the path is correct and try again.
At line:{1} char:{2}
+ {0}
+ ~~~~
    +CategoryInfo          : ObjectNotFound: ({0}:String) [], CommandNotFoundException
    + FullyQualifiedErrorId : CommandNotFoundException
.", name, iline, ichar));
                }
            }
            else if (IsDigit(exp[index]))
            {
                ++ichar;
                ++index;
                while (index < exp.Length &&
                   (IsDigit(exp[index]) || exp[index] == '.' || exp[index] == 'e'))
                {
                    ++ichar;
                    if (exp[index] == '\n')
                    {
                        ichar = 1;
                        ++iline;
                    }

                    ++index;
                }

                var no = exp.Substring(start, index - start);

                token = new Token
                {
                    TokenType = TokenTypes.Number,
                    lineStart = iline,
                    lineCharStart = ichar,
                    charStart = start,
                    charEnd = index - start
                };

                if (no.IndexOf('.') >= 0 || no.IndexOf('e') >= 0)
                {
                    token.Value = Double.Parse(no);
                }
                else
                {
                    token.Value = Int32.Parse(no);
                }
            }
            else if (IsOperatorSym(exp[index]))
            {
                ++ichar;
                ++index;
                while (index < exp.Length && IsOperatorSym(exp[index]))
                {
                    if (exp[index] == '\n')
                    {
                        ichar = 1;
                        ++iline;
                    }
                    ++index;
                }

                var ops = exp.Substring(start, index - start);
                if (IsAssignmentOperator(ops))
                {
                    token = new Token
                    {
                        TokenType = TokenTypes.Assignment,
                        Operator = new Operator("=", 0, true),
                        lineStart = iline,
                        lineCharStart = ichar,
                        charStart = start,
                        charEnd = index - start
                    };
                }
                else if (IsOperator(ops))
                {
                    token = new Token
                    {
                        TokenType = TokenTypes.Operator,
#if NETMF
                        Operator = _Operators[ops.ToLower()] as Operator
#else
                        Operator = _Operators[ops.ToLowerInvariant()]
#endif
                        ,
                        lineStart = iline,
                        lineCharStart = ichar,
                        charStart = start,
                        charEnd = index - start
                    };
                }
                else
                {
                    throw new Exception("Operator symbol matched but no Operator found.");
                }
            }

            // TODO: Figure out why this goes crazy!
            if (token != null)
            // But this is fine
            //if (!(token is InvalidToken))
            {
                return token;
            }
            else
            {
                throw new ParserException(String.Concat("Unallowed symbol: '", exp.Substring(start, index - start), "'"));
            }
        }

        private bool IsCmdlet(string name)
        {
            return false;
        }

        private static bool IsVariable(string name)
        {
            return (name[0] == '$');
        }

        private static int MatchQuote(string exp, int index)
        {
            ++index;
            bool escapeChar = false;
            while (index < exp.Length)
            {
                if (escapeChar)
                {
                    escapeChar = false;
                }
                else
                {
                    if (exp[index] == '\"')
                    {
                        return index;
                    }
                    else if (exp[index] == '\\')
                    {
                        escapeChar = true;
                    }
                }
                ++index;
            }
            return index - 1;
        }

        /// <summary>
        /// Matches an opening left paranthesis.
        /// </summary>
        /// <param name="exp">the string to search in</param>
        /// <param name="index">the index of the opening left paranthesis</param>
        /// <returns>the index of the matching closing right paranthesis</returns>
        private static int MatchCharPair(String exp, int index, char beginChar, char endChar)
        {
            int len = exp.Length;
            int i = index;
            int count = 0;

            while (i < len)
            {
                if (exp[i] == beginChar)
                {
                    count++;
                }
                else if (exp[i] == endChar)
                {
                    count--;
                }

                if (count == 0)
                {
                    return i;
                }

                i++;
            }

            return index;
        }

        /// <summary>
        /// Retrieves a value stored in the Hashtable containing all variable = value pairs.
        /// </summary>
        /// <remarks>
        /// The hashtable used in this method is set by the Parse( String, Hashtable ) method so this method retrives
        /// values inserted by the user of this class. Please note that no processing has been made
        /// on these values, they may have incorrect syntax or casing.
        /// </remarks>
        /// <param name="key">the name of the variable we want the value for</param>
        /// <returns>the value stored in the Hashtable or null if none.</returns>
        private object GetHashtableVarValue(string key, object context)
        {
#if NETMF
         var ob = _Variables[key.ToLower()];
#else
            var ob = _Variables[key.ToLowerInvariant()];
#endif

            if (ob == null) throw new ParserException("No value associated with " + key);

            return ob;
        }

        private Type GetHashtableVarType(string key)
        {
#if NETMF
         key = key.ToLower();
         if (_Variables.Contains(key))
#else
            key = key.ToLowerInvariant();
            if (_Variables.ContainsKey(key))
#endif
            {
                var val = _Variables[key];
                if (val == null)
                {
                    return null;
                }
                return val.GetType();
            }
            return null;
        }

        internal static Type ConvertToType(ref object value, Type targetType)
        {
            var tc = TypeDescriptor.GetConverter(targetType);

            if (tc.CanConvertFrom(value.GetType()))
            {
                // ReSharper disable once AssignNullToNotNullAttribute
                value = tc.ConvertFrom(null, CultureInfo.InvariantCulture, value);
            }
            else
            {
                tc = TypeDescriptor.GetConverter(value);
                if (tc.CanConvertTo(targetType))
                {
                    value = tc.ConvertTo(null, CultureInfo.InvariantCulture, value, targetType);
                }
                else
                {
                    throw new ParserException(String.Format("Cannot convert '{0}' to {1}", value, targetType.Name));
                }
            }

            return targetType;
        }
    }

    public delegate Type GetVarTypeDelegate(string varName);

    public delegate object GetVarValueDelegate(string varName, object context);
}