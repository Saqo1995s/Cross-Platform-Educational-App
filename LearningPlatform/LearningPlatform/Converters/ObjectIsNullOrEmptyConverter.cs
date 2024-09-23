using System;
using System.Globalization;
using Xamarin.Forms;

namespace LearningPlatform.Converters
{
    public class ObjectIsNullOrEmptyConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            bool result = false;
            var param = bool.Parse(parameter?.ToString() ?? true.ToString());
            try
            {
                result = param 
                    ? value == null
                    : value != null; 
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }

            return result;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            //TODO: Not Implemented
            return default;
        }
    }
}