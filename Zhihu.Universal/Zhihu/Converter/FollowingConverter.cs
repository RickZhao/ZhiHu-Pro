using System;

using Windows.UI.Xaml.Data;

using Zhihu.Common.Model;


namespace Zhihu.Converter
{
    internal sealed class FollowingConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, String language)
        {
            if (value == null) return String.Empty;

            if (value is Boolean)
            {
                var follow = (Boolean) value;

                return follow ? "取关" : "关注";
            }

            var following = value as Following;

            if (following == null) return String.Empty;

            return following.IsFollowing ? "取关" : "关注";
        }

        public object ConvertBack(object value, Type targetType, object parameter, String language)
        {
            throw new NotImplementedException();
        }
    }
}
