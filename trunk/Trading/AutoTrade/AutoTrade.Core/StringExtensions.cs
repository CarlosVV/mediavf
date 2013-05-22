using System;
using System.Linq;

namespace AutoTrade.Core
{
    public static class StringExtensions
    {

        /// <summary>
        /// Converts the text value of a setting to a type
        /// </summary>
        /// <typeparam name="T">The type to convert the text value to</typeparam>
        /// <param name="value">The text value</param>
        /// <returns>The value as the given type</returns>
        public static T ConvertTo<T>(this string value)
        {
            object returnValue = null;

            Type t = typeof(T);
            if (t.IsGenericType && t.GetGenericTypeDefinition() == typeof(Nullable<>))
            {
                if (string.IsNullOrEmpty(value))
                    return default(T);

                t = t.GetGenericArguments().First();
            }

            if (t == typeof(string))
            {
                returnValue = value;
            }
            else if (t == typeof(int))
            {
                int x;
                if (int.TryParse(value, out x))
                    returnValue = x;
            }
            else if (t == typeof(long))
            {
                long x;
                if (long.TryParse(value, out x))
                    returnValue = x;
            }
            else if (t == typeof(decimal))
            {
                decimal x;
                if (decimal.TryParse(value, out x))
                    returnValue = x;
            }
            else if (t == typeof(double))
            {
                double x;
                if (double.TryParse(value, out x))
                    returnValue = x;
            }
            else if (t == typeof(float))
            {
                float x;
                if (float.TryParse(value, out x))
                    returnValue = x;
            }
            else if (t == typeof(bool))
            {
                bool x;
                if (bool.TryParse(value, out x))
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

            return returnValue != null ? (T)returnValue : default(T);
        }
    }
}
