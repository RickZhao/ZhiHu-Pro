using System;

using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Media.Imaging;

using Zhihu.Common.Model;


namespace Zhihu.Converter
{
    internal sealed class AnsweredToImageConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            var myAnswer = value as MyAnswer;

            if (myAnswer == null || myAnswer.AnswerId <= 0)
                return new BitmapImage(new Uri("ms-appx:///Resource/Images/Public/ic_comment.png"));

            return new BitmapImage(new Uri("ms-appx:///Resource/Images/Public/ic_commented.png"));
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
