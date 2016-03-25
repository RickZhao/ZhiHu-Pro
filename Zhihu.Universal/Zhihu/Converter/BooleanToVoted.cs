using System;

using Windows.UI.Xaml.Data;


namespace Zhihu.Converter
{
    internal sealed class BooleanToVoted : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            var boolean = (Boolean) value;
            
            return boolean ? "取消赞" : parameter.ToString();
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
