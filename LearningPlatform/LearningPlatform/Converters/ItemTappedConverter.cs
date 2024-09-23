using System;
using System.Globalization;
using Xamarin.Forms;

namespace LearningPlatform.Converters
{
    public class ItemTappedConverter: IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            try
            {
                if (value != null && value is ItemTappedEventArgs eventArgs)
                    return eventArgs.Item;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
            
            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            //TODO: Not Implemented
            return default;
        }
    }
}
