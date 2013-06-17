using System;
using System.Linq;
using AutoTrade.Core.Properties;

namespace AutoTrade.Core
{
    public static class StringExtensions
    {
        /// <summary>
        /// Gets a type from a type name string
        /// </summary>
        /// <param name="assemblyQualifiedTypeName"></param>
        /// <returns></returns>
        public static Type ParseType(this string assemblyQualifiedTypeName)
        {
            // get the type
            var type = Type.GetType(assemblyQualifiedTypeName);
            if (type == null)
                throw new TypeLoadException(string.Format(Resources.TypeNotFoundExceptionMessage, assemblyQualifiedTypeName));

            return type;
        }

        /// <summary>
        /// Converts the text value of a setting to a type
        /// </summary>
        /// <typeparam name="T">The type to convert the text value to</typeparam>
        /// <param name="value">The text value</param>
        /// <returns>The value as the given type</returns>
        public static T ConvertTo<T>(this string value)
        {
            var obj = ConvertTo(value, typeof(T));

            return obj != null ? (T)obj : default(T);
        }

        /// <summary>
        /// Converts the text value of a setting to a type
        /// </summary>
        /// <param name="value"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public static object ConvertTo(this string value, Type type)
        {
            object returnValue = null;

            Type t = type;
            if (t.IsGenericType && t.GetGenericTypeDefinition() == typeof(Nullable<>))
            {
                if (String.IsNullOrEmpty(value))
                    return null;

                t = t.GetGenericArguments().First();
            }

            if (t == typeof(string))
            {
                returnValue = value;
            }
            else if (t == typeof(int))
            {
                int x;
                if (Int32.TryParse(value, out x))
                    returnValue = x;
            }
            else if (t == typeof(long))
            {
                long x;
                if (Int64.TryParse(value, out x))
                    returnValue = x;
            }
            else if (t == typeof(decimal))
            {
                decimal x;
                if (Decimal.TryParse(value, out x))
                    returnValue = x;
            }
            else if (t == typeof(double))
            {
                double x;
                if (Double.TryParse(value, out x))
                    returnValue = x;
            }
            else if (t == typeof(float))
            {
                float x;
                if (Single.TryParse(value, out x))
                    returnValue = x;
            }
            else if (t == typeof(bool))
            {
                bool x;
                if (Boolean.TryParse(value, out x))
                    returnValue = x;
            }
            else if (t == typeof(DateTime))
            {
                DateTime x;
                if (DateTime.TryParse(value, out x))
                    returnValue = x;
            }
            else if (t == typeof(TimeSpan))
            {
                TimeSpan x;
                if (TimeSpan.TryParse(value, out x))
                    returnValue = x;
            }
            else
                throw new InvalidCastException("Type conversion not supported for type " + t.FullName);

            return returnValue;
        }
    }
}
