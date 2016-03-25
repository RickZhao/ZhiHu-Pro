using System;

using Windows.UI.Xaml.Data;


namespace Zhihu.Converter
{
    internal sealed class BannerHeightConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            var hosterWidth = (double)value;

            var ratio = 292.0 / 720.0;

            var height = ratio * hosterWidth;

            return height;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
