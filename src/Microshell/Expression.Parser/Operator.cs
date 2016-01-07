using System;
using System.Collections;

namespace Microshell.Expression.Parser
{
    /// <summary>
    /// Class Operator, represents an Operator by holding information about it's symbol
    /// the number of arguments it takes and the operator precedence.
    /// </summary>
    public class Operator
    {
        private readonly String _op;
        private readonly int _prec;
        private bool _is2ArgOp;
        private bool v;

        /// <summary>
        /// Creates an Operator with the specified String name, arguments and precedence
        /// </summary>
        internal Operator(String _operator, int precedence)
        {
            _op = _operator;
            _prec = precedence;

            Functions = new Hashtable();
        }

        public Operator(string _operator, int precedence, bool is2ArgOp) : this(_operator, precedence)
        {
            this._is2ArgOp = is2ArgOp;
        }

        /// <summary>
        /// Returns the precedence for this Operator.
        /// </summary>
        internal int Precedence
        {
            get { return _prec; }
        }

        /// <summary>
        /// Returns the String name of this Operator.
        /// </summary>
        internal string Symbol
        {
            get { return _op; }
        }

        internal bool IsOperator(string symbol)
        {
#if NETMF
         return _op.ToLower().Equals(symbol.ToLower());
#else
            return _op.Equals(symbol, StringComparison.OrdinalIgnoreCase);
#endif
        }

        internal bool Is2ArgOp
        {
            get { return _is2ArgOp; }
        }

        internal Hashtable Functions { get; private set; }

        internal Operator AddFunction(ValueType type1, ValueType type2, ValueType returnType, OperatorDelegate fkt)
        {
            _is2ArgOp = true;
            Functions.Add((uint)((ushort)type1 + ((ushort)type2 << 16)),
               new OperatorProc()
               {
                   ReturnType = returnType,
                   Function = fkt
               });
            return this;
        }

        internal Operator AddFunction(ValueType type1, ValueType returnType, OperatorDelegate fkt)
        {
            Functions.Add((uint)type1,
               new OperatorProc()
               {
                   ReturnType = returnType,
                   Function = fkt
               });
            return this;
        }

        internal Type GetReturnType(Type argType1, Type argType2)
        {
            var type1 = CheckType(argType1);
            var type2 = CheckType(argType2);

            var fkt = Functions[(uint)((ushort)type1 + ((ushort)type2 << 16))];
            if (fkt != null)
            {
                switch (((OperatorProc)fkt).ReturnType)
                {
                    case ValueType.String:
                        return typeof(string);

                    case ValueType.Bool:
                        return typeof(bool);

                    case ValueType.Integer:
                        return typeof(int);

                    case ValueType.Double:
                        return typeof(double);
                }
            }
            if (argType2 != null)
            {
                throw new ParserException(String.Concat("Operator ", Symbol, " not allowed for ", argType1.Name, " and ", argType2.Name));
            }
            throw new ParserException(String.Concat("Operator ", Symbol, " not allowed for type ", argType1.Name));
        }

        internal object Calculate(object arg1, object arg2)
        {
            var type1 = CheckType(ref arg1);
            var type2 = CheckType(ref arg2);

            var fkt = Functions[(uint)((ushort)type1 + ((ushort)type2 << 16))];
            if (fkt != null)
            {
                return ((OperatorProc)fkt).Function(arg1, arg2);
            }
            if (arg2 != null)
            {
                throw new ParserException(String.Concat("Operator ", Symbol, " not allowed for ", arg1.GetType().Name, " and ", arg2.GetType().Name));
            }
            throw new ParserException(String.Concat("Operator ", Symbol, " not allowed for type ", arg1.GetType().Name));
        }

        public static double ToDouble(object arg)
        {
            if (arg is int)
            {
                return (double)((int)arg);
            }
            return (double)arg;
        }

        public static int ToInt(object arg)
        {
            if (arg is double)
            {
                return (int)((double)arg);
            }
            return (int)arg;
        }

        internal static ValueType CheckType(ref object arg)
        {
            if (arg == null)
            {
                return ValueType.None;
            }

            var t = arg.GetType();
            if (t == typeof(string))
            {
                return ValueType.String;
            }
            if (t == typeof(bool))
            {
                return ValueType.Bool;
            }
            if (t == typeof(int))
            {
                return ValueType.Integer;
            }
            if (t == typeof(double))
            {
                return ValueType.Double;
            }
            if (t == typeof(byte))
            {
                arg = (int)((byte)arg);
                return ValueType.Integer;
            }
            if (t == typeof(sbyte))
            {
                arg = (int)((sbyte)arg);
                return ValueType.Integer;
            }
            if (t == typeof(short))
            {
                arg = (int)((short)arg);
                return ValueType.Integer;
            }
            if (t == typeof(ushort))
            {
                arg = (int)((ushort)arg);
                return ValueType.Integer;
            }
            if (t == typeof(uint))
            {
                arg = (double)((uint)arg);
                return ValueType.Double;
            }
            if (t == typeof(long))
            {
                arg = (double)((long)arg);
                return ValueType.Double;
            }
            if (t == typeof(ulong))
            {
                arg = (double)((ulong)arg);
                return ValueType.Double;
            }
            if (t == typeof(float))
            {
                arg = (double)((float)arg);
                return ValueType.Double;
            }
            throw new ParserException(String.Concat("The type ", t.Name, " is not supported"));
        }

        internal static ValueType CheckType(Type t)
        {
            if (t == null)
            {
                return ValueType.None;
            }
            if (t == typeof(string))
            {
                return ValueType.String;
            }
            if (t == typeof(bool))
            {
                return ValueType.Bool;
            }
            if (t == typeof(byte) || t == typeof(sbyte) || t == typeof(short) || t == typeof(ushort) || t == typeof(int))
            {
                return ValueType.Integer;
            }
            if (t == typeof(uint) || t == typeof(long) || t == typeof(ulong) || t == typeof(float) || t == typeof(double))
            {
                return ValueType.Double;
            }
            throw new ParserException(String.Concat("The type ", t.Name, " is not supported"));
        }
    }

    internal struct OperatorProc
    {
        internal ValueType ReturnType;

        internal OperatorDelegate Function;
    }

    internal delegate object OperatorDelegate(object arg1, object arg2);

    internal enum ValueType : ushort
    {
        None = 0,
        String = 1,
        Bool = 2,
        Integer = 3,
        Double = 4
    }
}
