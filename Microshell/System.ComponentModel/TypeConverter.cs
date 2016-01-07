using System;
using System.Globalization;

namespace System.ComponentModel
{
    public class TypeConverter
    {
        public virtual bool CanConvertFrom(Type type)
        {
            throw new NotImplementedException();
        }

        public virtual object ConvertFrom(object p, CultureInfo invariantCulture, object value)
        {
            throw new NotImplementedException();
        }

        public virtual bool CanConvertTo(Type targetType)
        {
            throw new NotImplementedException();
        }

        public virtual object ConvertTo(object p, CultureInfo invariantCulture, object value, Type targetType)
        {
            throw new NotImplementedException();
        }
    }
}
