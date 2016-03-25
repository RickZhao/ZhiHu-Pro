using System;

using Windows.UI.Xaml.Data;

using Zhihu.Helper;


namespace Zhihu.Converter
{
    public sealed class VotingToHighlightConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (value == null || parameter == null) return Theme.Instance.AppBarForeColor;

            return value.ToString() == parameter.ToString() ? Theme.Instance.AppBarHighlightForeColor : Theme.Instance.AppBarForeColor;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
