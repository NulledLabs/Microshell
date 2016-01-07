using System;
using Microsoft.SPOT;
using System.Globalization;

namespace System.ComponentModel
{
    public class BooleanConverter : TypeConverter
    {
        public override object ConvertFrom(object p, CultureInfo invariantCulture, object value)
        {
            return new object();
        }
    }
}
