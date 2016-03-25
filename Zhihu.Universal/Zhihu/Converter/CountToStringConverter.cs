using System;

using Windows.UI.Xaml.Data;


namespace Zhihu.Converter
{
    public sealed class CountToStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, String language)
        {
            var count = (Int32) value;

            return count > 1000
                ? (count/1000.0).ToString("F1") + "k"
                : count.ToString();
        }

        public object ConvertBack(object value, Type targetType, object parameter, String language)
        {
            throw new NotImplementedException();
        }
    }
}
