using System;

using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Media.Imaging;


namespace Zhihu.Converter
{
    internal sealed class VotingToImageConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, String language)
        {
            if (value == null) return null;

            var voting = -1;
            Int32.TryParse(value.ToString(), out voting);

            var mode = parameter.ToString();

            if (voting == 0)
            {
                return mode == "down"
                    ? new BitmapImage(new Uri("ms-appx:///Resource/Images/Public/vote_down_normal.png"))
                    : new BitmapImage(new Uri("ms-appx:///Resource/Images/Public/vote_up_normal.png"));
            }
            if (voting == -1)
            {
                return mode == "down"
                    ? new BitmapImage(new Uri("ms-appx:///Resource/Images/Public/vote_down_checked.png"))
                    : new BitmapImage(new Uri("ms-appx:///Resource/Images/Public/vote_up_normal.png"));
            }
            if (voting == 1)
            {
                return mode == "down"
                    ? new BitmapImage(new Uri("ms-appx:///Resource/Images/Public/vote_down_normal.png"))
                    : new BitmapImage(new Uri("ms-appx:///Resource/Images/Public/vote_up_checked.png"));
            }

            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, String language)
        {
            throw new NotImplementedException();
        }
    }
}
