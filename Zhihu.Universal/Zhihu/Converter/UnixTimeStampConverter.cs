using System;

using Windows.UI.Xaml.Data;


namespace Zhihu.Converter
{
    internal sealed class UnixTimeStampConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, String language)
        {
            var timeStamp = (Int32)value;

            var unixStart = new DateTime(1970, 1, 1, 0, 0, 0);
            unixStart = unixStart.AddSeconds(timeStamp);

            return unixStart.ToLocalTime();
        }

        public object ConvertBack(object value, Type targetType, object parameter, String language)
        {
            throw new NotImplementedException();
        }
    }
}
