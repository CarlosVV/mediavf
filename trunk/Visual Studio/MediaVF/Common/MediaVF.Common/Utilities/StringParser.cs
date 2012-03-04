using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MediaVF.Common.Utilities
{
    public static class StringParser
    {
        /// <summary>
        /// Parses a string to an object
        /// </summary>
        /// <param name="t"></param>
        /// <param name="text"></param>
        /// <param name="objValue"></param>
        /// <returns></returns>
        public static bool TryParse(Type t, string text, out object objValue)
        {
            if (t == typeof(string))
            {
                objValue = text;
                return true;
            }
            else
            {
                // initialize to default value
                bool parsed = false;
                objValue = null;
                if (!string.IsNullOrEmpty(text))
                {
                    if (t == typeof(int))
                    {
                        // parse int
                        int intValue;
                        if (int.TryParse(text, out intValue))
                        {
                            parsed = true;
                            objValue = intValue;
                        }
                    }
                    else if (t == typeof(float))
                    {
                        // parse float
                        float floatValue;
                        if (float.TryParse(text, out floatValue))
                        {
                            parsed = true;
                            objValue = floatValue;
                        }
                    }
                    else if (t == typeof(decimal))
                    {
                        // parse decimal
                        decimal decimalValue;
                        if (decimal.TryParse(text, out decimalValue))
                        {
                            parsed = true;
                            objValue = decimalValue;
                        }
                    }
                    else if (t == typeof(double))
                    {
                        // parse double
                        double doubleValue;
                        if (double.TryParse(text, out doubleValue))
                        {
                            parsed = true;
                            objValue = doubleValue;
                        }
                    }
                    else if (t == typeof(long))
                    {
                        // parse long
                        long longValue;
                        if (long.TryParse(text, out longValue))
                        {
                            parsed = true;
                            objValue = longValue;
                        }
                    }
                    else if (t == typeof(DateTime))
                    {
                        // parse DateTime
                        DateTime dateTimeValue;
                        if (DateTime.TryParse(text, out dateTimeValue))
                        {
                            parsed = true;
                            objValue = dateTimeValue;
                        }
                    }
                    else if (t == typeof(TimeSpan))
                    {
                        // parse TimeSpan
                        TimeSpan timeSpanValue;
                        if (TimeSpan.TryParse(text, out timeSpanValue))
                        {
                            parsed = true;
                            objValue = timeSpanValue;
                        }
                    }
                }

                // return if parsed
                return parsed;
            }
        }

        /// <summary>
        /// Parses a string to an object
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="text"></param>
        /// <returns></returns>
        public static bool TryParse<T>(string text, out T typeValue)
        {
            object objValue;
            if (TryParse(typeof(T), text, out objValue))
            {
                typeValue = (T)objValue;
                return true;
            }
            else
            {
                typeValue = default(T);
                return false;
            }
        }
    }
}
