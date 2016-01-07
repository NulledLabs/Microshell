using System;
using System.Globalization;

namespace System.ComponentModel
{
    public class IntConverter : TypeConverter
    {
        public override object ConvertFrom(object p, CultureInfo invariantCulture, object value)
        {
            if (value is string)
            {
                value = Convert.ToInt32((string)value);
            }
            else
            {
                value = (int)value;
            }

            return value;
        }
    }
}
