using System;
using System.Globalization;
using System.Windows.Data;
using AutoTrade.Core.UI.Properties;

namespace AutoTrade.Core.UI.Converters
{
    public class SecondsToTimeSpanConverter : IValueConverter
    {
        /// <summary>
        /// Converts a number of seconds to a <see cref="TimeSpan"/>
        /// </summary>
        /// <param name="value"></param>
        /// <param name="targetType"></param>
        /// <param name="parameter"></param>
        /// <param name="culture"></param>
        /// <returns></returns>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null || !(value is DateTime))
                throw new InvalidOperationException(string.Format(Resources.ExpectedTypeExceptionMessage,
                    typeof(DateTime),
                    value != null ? value.GetType().FullName : "null"));

            return ((DateTime)value).TimeOfDay.TotalSeconds;
        }

        /// <summary>
        /// Converts a <see cref="TimeSpan"/> to a number of seconds
        /// </summary>
        /// <param name="value"></param>
        /// <param name="targetType"></param>
        /// <param name="parameter"></param>
        /// <param name="culture"></param>
        /// <returns></returns>
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null || !(value is double))
                throw new InvalidOperationException(string.Format(Resources.ExpectedTypeExceptionMessage,
                    typeof(double),
                    value != null ? value.GetType().FullName : "null"));

            var timeSpan = TimeSpan.FromSeconds((double)value);

            return new DateTime(1900, 1, 1, timeSpan.Hours, timeSpan.Minutes, timeSpan.Seconds, timeSpan.Milliseconds);
        }
    }
}
