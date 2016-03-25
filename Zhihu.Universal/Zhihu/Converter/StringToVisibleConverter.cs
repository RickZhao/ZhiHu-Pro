using System;

using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;


namespace Zhihu.Converter
{
    internal sealed class StringToVisibleConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            var asString = value as String;

            return String.IsNullOrEmpty(asString) ? Visibility.Collapsed : Visibility.Visible;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
