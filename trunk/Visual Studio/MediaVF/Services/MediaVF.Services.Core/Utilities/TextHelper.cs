using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MediaVF.Services.Core.Utilities
{
    public static class TextHelper
    {
        public static object ConvertToType(this string text, Type type)
        {
            object convertedObj = null;

            if (type == typeof(string))
                convertedObj = text;
            else if (type == typeof(int))
            {
                convertedObj = default(int);

                int converted;
                if (int.TryParse(text, out converted))
                    convertedObj = converted;
            }
            else if (type == typeof(long))
            {
                convertedObj = default(long);

                long converted;
                if (long.TryParse(text, out converted))
                    convertedObj = converted;
            }
            else if (type == typeof(decimal))
            {
                convertedObj = default(decimal);

                decimal converted;
                if (decimal.TryParse(text, out converted))
                    convertedObj = converted;
            }
            else if (type == typeof(double))
            {
                convertedObj = default(double);

                double converted;
                if (double.TryParse(text, out converted))
                    convertedObj = converted;
            }
            else if (type == typeof(float))
            {
                convertedObj = default(float);

                float converted;
                if (float.TryParse(text, out converted))
                    convertedObj = converted;
            }
            else if (type == typeof(DateTime))
            {
                convertedObj = default(DateTime);

                DateTime converted;
                if (DateTime.TryParse(text, out converted))
                    convertedObj = converted;
            }
            else if (type == typeof(TimeSpan))
            {
                convertedObj = default(TimeSpan);

                TimeSpan converted;
                if (TimeSpan.TryParse(text, out converted))
                    convertedObj = converted;
            }
            else if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>))
            {
                if (!string.IsNullOrEmpty(text))
                    convertedObj = ConvertToType(text, type.GetGenericArguments().First());
                else
                    convertedObj = null;
            }
            // else if (type.GetInterface(typeof(ITextConvertible).Name))

            return convertedObj;
        }
    }
}
