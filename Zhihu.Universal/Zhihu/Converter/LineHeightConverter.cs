using System;

using Windows.UI.Xaml.Data;


namespace Zhihu.Converter
{
    internal sealed class LineHeightConverter:IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            var fontSize = (Double) value;

            if (fontSize <= 10.0) return 20;

            return 1.6*fontSize;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
