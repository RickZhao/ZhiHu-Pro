using System;

using Windows.UI.Xaml.Data;


namespace Zhihu.Converter
{
    internal sealed class NullableInt32Converter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            var nullableInt32 = value as Int32?;

            return nullableInt32.HasValue ? nullableInt32.Value : -1;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
