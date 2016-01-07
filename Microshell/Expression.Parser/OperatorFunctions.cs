using System;

namespace Microshell.Expression.Parser
{
    internal static class OperatorFunctions
    {
        internal static object PlusTss(object arg1, object arg2)
        {
            return (string)arg1 + (string)arg2;
        }

        internal static object PlusTii(object arg1, object arg2)
        {
            return Operator.ToInt(arg1) + Operator.ToInt(arg2);
        }

        internal static object PlusTdd(object arg1, object arg2)
        {
            return Operator.ToDouble(arg1) + Operator.ToDouble(arg2);
        }

        internal static object MinusTii(object arg1, object arg2)
        {
            return Operator.ToInt(arg1) - Operator.ToInt(arg2);
        }

        internal static object MinusTdd(object arg1, object arg2)
        {
            return Operator.ToDouble(arg1) - Operator.ToDouble(arg2);
        }

        internal static object MinusTi(object arg1, object arg2)
        {
            return -Operator.ToInt(arg1);
        }

        internal static object MinusTd(object arg1, object arg2)
        {
            return -Operator.ToDouble(arg1);
        }

        internal static object DivTii(object arg1, object arg2)
        {
            return Operator.ToInt(arg1) / Operator.ToInt(arg2);
        }

        internal static object DivTdd(object arg1, object arg2)
        {
            return Operator.ToDouble(arg1) / Operator.ToDouble(arg2);
        }

        internal static object MultTii(object arg1, object arg2)
        {
            return Operator.ToInt(arg1) * Operator.ToInt(arg2);
        }

        internal static object MultTdd(object arg1, object arg2)
        {
            return Operator.ToDouble(arg1) * Operator.ToDouble(arg2);
        }

        internal static object ModuloTii(object arg1, object arg2)
        {
            return Operator.ToInt(arg1) % Operator.ToInt(arg2);
        }

        internal static object Cos(object arg1, object arg2)
        {
            return Math.Cos(Operator.ToDouble(arg1));
        }

        internal static object Sin(object arg1, object arg2)
        {
            return Math.Sin(Operator.ToDouble(arg1));
        }

        internal static object Exp(object arg1, object arg2)
        {
            return Math.Exp(Operator.ToDouble(arg1));
        }

        internal static object Ln(object arg1, object arg2)
        {
            return Math.Log(Operator.ToDouble(arg1));
        }

        internal static object Tan(object arg1, object arg2)
        {
            return Math.Tan(Operator.ToDouble(arg1));
        }

        internal static object Acos(object arg1, object arg2)
        {
            return Math.Acos(Operator.ToDouble(arg1));
        }

        internal static object Asin(object arg1, object arg2)
        {
            return Math.Asin(Operator.ToDouble(arg1));
        }

        internal static object Atan(object arg1, object arg2)
        {
            return Math.Atan(Operator.ToDouble(arg1));
        }

        internal static object Cosh(object arg1, object arg2)
        {
            return ((Math.Exp(Operator.ToDouble(arg1)) + (1d / Math.Exp(Operator.ToDouble(arg1)))) / 2d);
        }

        internal static object Sinh(object arg1, object arg2)
        {
            return ((Math.Exp(Operator.ToDouble(arg1)) - (1d / Math.Exp(Operator.ToDouble(arg1)))) / 2d);
        }

        internal static object Tanh(object arg1, object arg2)
        {
            return (((Math.Exp(Operator.ToDouble(arg1)) - (1d / Math.Exp(Operator.ToDouble(arg1)))) / 2d) / ((Math.Exp(Operator.ToDouble(arg1)) + (1d / Math.Exp(Operator.ToDouble(arg1)))) / 2d));
        }

        internal static object Sqrt(object arg1, object arg2)
        {
            return Math.Sqrt(Operator.ToDouble(arg1));
        }

        internal static object Cotan(object arg1, object arg2)
        {
            return (1d / Math.Tan(Operator.ToDouble(arg1)));
        }

        internal static object Fpart(object arg1, object arg2)
        {
            var val = Operator.ToDouble(arg1);
            if (val >= 0)
            {
                return (val - Math.Floor(val));
            }
            return (val - Math.Ceiling(val));
        }

        internal static object Acotan(object arg1, object arg2)
        {
            return (Math.PI / 2d - Math.Atan(Operator.ToDouble(arg1)));
        }

        internal static object Round(object arg1, object arg2)
        {
            return (int)Math.Round(Operator.ToDouble(arg1));
        }

        internal static object Ceil(object arg1, object arg2)
        {
            return (int)Math.Ceiling(Operator.ToDouble(arg1));
        }

        internal static object Floor(object arg1, object arg2)
        {
            return (int)Math.Floor(Operator.ToDouble(arg1));
        }

        internal static object Fac(object arg1, object arg2)
        {
            return Fac(Operator.ToInt(arg1));
        }

        internal static int Fac(int val)
        {
            if (val < 0)
            {
                return 0;
            }
            if (val <= 1)
            {
                return 1;
            }
            return (val * Fac(val - 1));
        }

        internal static object Sfac(object arg1, object arg2)
        {
            return Sfac(Operator.ToInt(arg1));
        }

        internal static int Sfac(int val)
        {
            if (val < 0)
            {
                return 0;
            }
            if (val <= 1)
            {
                return 1;
            }
            return (val * Sfac(val - 2));
        }

        internal static object AbsTi(object arg1, object arg2)
        {
            return Math.Abs(Operator.ToInt(arg1));
        }

        internal static object AbsTd(object arg1, object arg2)
        {
            return Math.Abs(Operator.ToDouble(arg1));
        }

        internal static object Log(object arg1, object arg2)
        {
            return Math.Log10(Operator.ToDouble(arg1));
        }

        internal static object IsGreaterTss(object arg1, object arg2)
        {
            return CompareStrings((string)arg1, (string)arg2) > 0;
        }

        internal static object IsGreaterTii(object arg1, object arg2)
        {
            return Operator.ToInt(arg1) > Operator.ToInt(arg2);
        }

        internal static object IsGreaterTdd(object arg1, object arg2)
        {
            return Operator.ToDouble(arg1) > Operator.ToDouble(arg2);
        }

        internal static object IsLessTss(object arg1, object arg2)
        {
            return CompareStrings((string)arg1, (string)arg2) < 0;
        }

        internal static object IsLessTii(object arg1, object arg2)
        {
            return Operator.ToInt(arg1) < Operator.ToInt(arg2);
        }

        internal static object IsLessTdd(object arg1, object arg2)
        {
            return Operator.ToDouble(arg1) < Operator.ToDouble(arg2);
        }

        internal static object IsGreaterOrEqualTss(object arg1, object arg2)
        {
            return CompareStrings((string)arg1, (string)arg2) >= 0;
        }

        internal static object IsGreaterOrEqualTii(object arg1, object arg2)
        {
            return Operator.ToInt(arg1) >= Operator.ToInt(arg2);
        }

        internal static object IsGreaterOrEqualTdd(object arg1, object arg2)
        {
            return Operator.ToDouble(arg1) >= Operator.ToDouble(arg2);
        }

        internal static object IsLessOrEqualTss(object arg1, object arg2)
        {
            return CompareStrings((string)arg1, (string)arg2) <= 0;
        }

        internal static object IsLessOrEqualTii(object arg1, object arg2)
        {
            return Operator.ToInt(arg1) <= Operator.ToInt(arg2);
        }

        internal static object IsLessOrEqualTdd(object arg1, object arg2)
        {
            return Operator.ToDouble(arg1) <= Operator.ToDouble(arg2);
        }

        internal static object IsEqualTss(object arg1, object arg2)
        {
            return CompareStrings((string)arg1, (string)arg2) == 0;
        }

        internal static object IsEqualTbb(object arg1, object arg2)
        {
            return (bool)arg1 == (bool)arg2;
        }

        internal static object IsEqualTii(object arg1, object arg2)
        {
            return Operator.ToInt(arg1) == Operator.ToInt(arg2);
        }

        internal static object IsEqualTdd(object arg1, object arg2)
        {
            return Math.Abs(Operator.ToDouble(arg1) - Operator.ToDouble(arg2)) <= Double.Epsilon;
        }

        internal static object IsInEqualTss(object arg1, object arg2)
        {
            return CompareStrings((string)arg1, (string)arg2) != 0;
        }

        internal static object IsInEqualTbb(object arg1, object arg2)
        {
            return (bool)arg1 != (bool)arg2;
        }

        internal static object IsInEqualTii(object arg1, object arg2)
        {
            return Operator.ToInt(arg1) != Operator.ToInt(arg2);
        }

        internal static object IsInEqualTdd(object arg1, object arg2)
        {
            return Math.Abs(Operator.ToDouble(arg1) - Operator.ToDouble(arg2)) > Double.Epsilon;
        }

        internal static object XorTbb(object arg1, object arg2)
        {
            return (bool)arg1 ^ (bool)arg2;
        }

        internal static object AndTbb(object arg1, object arg2)
        {
            return (bool)arg1 && (bool)arg2;
        }

        internal static object OrTbb(object arg1, object arg2)
        {
            return (bool)arg1 || (bool)arg2;
        }

        internal static object NotTb(object arg1, object arg2)
        {
            return !((bool)arg1);
        }

        private static int CompareStrings(string a, string b)
        {
#if NETMF
         // ReSharper disable StringCompareIsCultureSpecific.1
         if (ExpressionParser.IgnoreCaseStringComparison)
         {
            return String.Compare(a.ToLower(), b.ToLower());
         }
         return String.Compare(a, b);
         // ReSharper restore StringCompareIsCultureSpecific.1
#else
            return String.Compare(a, b, ExpressionParser.StringComparisonType);
#endif
        }

        public static object LeftStr(object arg1, object arg2)
        {
            return ((string)arg1).Substring(0, Operator.ToInt(arg2));
        }

        public static object RightStr(object arg1, object arg2)
        {
            var str = (string)arg1;
            return str.Substring(str.Length - Operator.ToInt(arg2));
        }

        public static object StrLen(object arg1, object arg2)
        {
            return ((string)arg1).Length;
        }
    }
}
