using System;
using Microsoft.SPOT;
using System.Globalization;

namespace System.ComponentModel
{
    public class DoubleConverter : TypeConverter
    {
        public override bool CanConvertFrom(Type type)
        {
            if (type == typeof(bool))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public override object ConvertFrom(object p, CultureInfo invariantCulture, object value)
        {
            if (value is bool)
            {
                value = ((bool)value) ? 1.0 : 0.0;
                return typeof(double);
            }
            else if (value is string)
            {
                value = Convert.ToDouble((string)value);
            }
            else
            {
                value = (double)value;
            }

            return value;
        }
    }
}