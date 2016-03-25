using System;

using Windows.UI;
using Windows.UI.Xaml.Data;


namespace Zhihu.Converter
{
    internal sealed class RandomColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, String language)
        {
            var bytes = new byte[3];
            var random = new Random();
            random.NextBytes(bytes);

            return Color.FromArgb(0xff, bytes[0], bytes[1], bytes[2]);
        }

        public object ConvertBack(object value, Type targetType, object parameter, String language)
        {
            throw new NotImplementedException();
        }
    }
}
