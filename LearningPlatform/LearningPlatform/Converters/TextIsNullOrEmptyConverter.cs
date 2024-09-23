using System;
using System.Globalization;
using Xamarin.Forms;

namespace LearningPlatform.Converters
{
    class TextIsNullOrEmptyConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            bool result = false;
            try
            {
                result = string.IsNullOrEmpty(value?.ToString() ?? string.Empty);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
            
            return result;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            Console.WriteLine("NullOrEmptyConverter can only be used OneWay.");
            return default;
        }
    }
}
