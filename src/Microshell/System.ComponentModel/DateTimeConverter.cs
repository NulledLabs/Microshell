using System;
using Microsoft.SPOT;

namespace System.ComponentModel
{
    public class DateTimeConverter : TypeConverter
    {
        public DateTime ConvertFrom(Object obj)
        {
            if (obj is string)
            {
                return this.ConvertFromString((string)obj);
            }
            //TODO: Fix
            return new DateTime();
        }

        public DateTime ConvertFromString(string value)
        {
            return DateTime.Parse(value);
        }
    }
}
