using System;

using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Media.Imaging;


namespace Zhihu.Converter
{
    internal sealed class FollowToImageConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (value == null) return new BitmapImage(new Uri("ms-appx:///Resource/Images/Public/img_empty_follow.png"));

            var isFollowing = (Boolean) value;

            return isFollowing
                ? new BitmapImage(new Uri("ms-appx:///Resource/Images/Public/ic_drawer_follow_normal.png"))
                : new BitmapImage(new Uri("ms-appx:///Resource/Images/Public/img_empty_follow.png"));
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
