using System;

using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;


namespace Zhihu.Converter
{
    public sealed class BoolToVisiableConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            var visiable = (Boolean)value;

            if (parameter != null) visiable = !visiable;

            return visiable ? Visibility.Visible : Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
