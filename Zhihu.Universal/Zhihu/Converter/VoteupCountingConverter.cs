using System;

using Windows.UI.Xaml.Data;


namespace Zhihu.Converter
{
    public sealed class VoteupCountingConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            var count = (Int32) value;

            var countStr = count > 1000
                ? (count/1000.0).ToString("F1") + "k"
                : count.ToString();

            return String.Format("赞({0})", countStr);
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
