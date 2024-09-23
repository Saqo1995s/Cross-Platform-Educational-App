using System;
using System.Globalization;
using Xamarin.Forms;

namespace LearningPlatform.Converters
{
    public class TicksToDateTimeStrConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var ticks = (long) value;
            var format = parameter.ToString();

            try
            {
                var dateTimeStr = new DateTime(ticks).ToString(format);
                return dateTimeStr;
            }
            catch
            {
                return string.Empty;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            //Not need
            return null;
        }
    }
}