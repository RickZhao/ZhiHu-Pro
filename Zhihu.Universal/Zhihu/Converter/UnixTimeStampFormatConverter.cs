using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Data;

namespace Zhihu.Converter
{
    internal sealed class UnixTimeStampFormatConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            var timeStamp = (Int32)value;

            var unixStart = new DateTime(1970, 1, 1, 0, 0, 0);
            unixStart = unixStart.AddSeconds(timeStamp);

            return unixStart.ToLocalTime().ToString("yyyy-MM-dd");
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
