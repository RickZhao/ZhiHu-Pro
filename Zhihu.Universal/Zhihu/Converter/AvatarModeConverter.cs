using System;
using System.Diagnostics;

using Windows.UI.Xaml.Data;


namespace Zhihu.Converter
{
    internal sealed class AvatarModeConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, String language)
        {
            var avatar = value as String;

            if (String.IsNullOrEmpty(avatar))
            {
                Debugger.Break();
                return value;
            }

            var mode = parameter as String;

            if (String.IsNullOrEmpty(mode))
            {
                Debugger.Break();
                return value;
            }

            return avatar.Replace("_s", "_" + mode);
        }

        public object ConvertBack(object value, Type targetType, object parameter, String language)
        {
            throw new NotImplementedException();
        }
    }
}
