using System;

using Windows.UI.Xaml.Data;

using Zhihu.Helper;


namespace Zhihu.Converter
{
    public sealed class BoolToHighlightConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            var highlighted = (Boolean)value;

            return highlighted ? Theme.Instance.AppBarHighlightForeColor : Theme.Instance.AppBarForeColor;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
