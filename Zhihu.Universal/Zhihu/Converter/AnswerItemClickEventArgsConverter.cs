using System;

using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Data;

using Zhihu.Common.Model;


namespace Zhihu.Converter
{
    internal sealed class AnswerItemClickEventArgsConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, String language)
        {
            var eventArgs = value as ItemClickEventArgs;

            if (null == eventArgs || null == eventArgs.ClickedItem) return null;

            var feed = eventArgs.ClickedItem as Answer;

            if (null == feed) return null;
            else return feed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, String language)
        {
            throw new NotImplementedException();
        }
    }
}
