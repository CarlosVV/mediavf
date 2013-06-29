using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows;

namespace AutoTrade.Core.UI.Converters 
{
    public class VisibilityConverter : IValueConverter
    {
        /// <summary>
        /// Converts a boolean value to a Visibility value
        /// </summary>
        /// <param name="value"></param>
        /// <param name="targetType"></param>
        /// <param name="parameter"></param>
        /// <param name="culture"></param>
        /// <returns></returns>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var boolValue = (bool?)value;

            // if no value, return hidden
            if (!boolValue.HasValue)
                return Visibility.Hidden;

            // if value is true, return visible
            return boolValue.Value ? Visibility.Visible : Visibility.Collapsed;
        }

        /// <summary>
        /// Converts a Visibility value back to a boolean value
        /// </summary>
        /// <param name="value"></param>
        /// <param name="targetType"></param>
        /// <param name="parameter"></param>
        /// <param name="culture"></param>
        /// <returns></returns>
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            switch ((Visibility)value)
            {
                case Visibility.Visible:
                    return true;
                case Visibility.Hidden:
                    return null;
                default:
                    return false;
            }
        }
    }
}
