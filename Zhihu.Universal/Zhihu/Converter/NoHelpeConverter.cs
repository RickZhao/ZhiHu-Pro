using System;

using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Media.Imaging;


namespace Zhihu.Converter
{
    internal sealed class NoHelpeConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, String language)
        {
            var likeDislike = (Boolean)value;

            return likeDislike ? "撤销没有帮助" : "没有帮助";
        }

        public object ConvertBack(object value, Type targetType, object parameter, String language)
        {
            throw new NotImplementedException();
        }
    }

    internal sealed class ThankedConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, String language)
        {
            var likeDislike = (Boolean)value;

            return likeDislike ? "已感谢" : "感谢作者";
        }

        public object ConvertBack(object value, Type targetType, object parameter, String language)
        {
            throw new NotImplementedException();
        }
    }

    internal sealed class FavoritedConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, String language)
        {
            if (value == null) return new BitmapImage(new Uri("ms-appx:///Resource/Images/Public/ic_collect.png"));

            var noHelp = (Boolean) value;

            var bitmap = noHelp == false
                ? new BitmapImage(new Uri("ms-appx:///Resource/Images/Public/ic_collect.png"))
                : new BitmapImage(new Uri("ms-appx:///Resource/Images/Public/ic_collected.png"));

            return bitmap;
        }

        public object ConvertBack(object value, Type targetType, object parameter, String language)
        {
            throw new NotImplementedException();
        }
    }

    internal sealed class CommentsCountConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, String language)
        {
            var voting = (Int32) value;

            return String.Format("评论 {0}", voting);
        }

        public object ConvertBack(object value, Type targetType, object parameter, String language)
        {
            throw new NotImplementedException();
        }
    }
}