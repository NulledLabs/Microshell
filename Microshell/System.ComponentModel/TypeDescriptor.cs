using System;
using Microsoft.SPOT;
using System.Collections;

namespace System.ComponentModel
{
    public class TypeDescriptor
    {
        static Hashtable TypeConverters = new Hashtable()
        {
            { "bool", new BooleanConverter() },
            { "string", new StringConverter() },
            { "datetime", new DateTimeConverter() }
        };

        public static TypeConverter GetConverter(Type type)
        {
            return (TypeConverter)TypeConverters[type.ToString().ToLower()];
        }

        public static TypeConverter GetConverter(object component)
        {
            //TODO: Implement
            return new TypeConverter();
        }
    }
}
