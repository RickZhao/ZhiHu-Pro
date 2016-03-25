using System;

using Windows.UI.Xaml.Data;


namespace Zhihu.Converter
{
    internal sealed class GenderToImageConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, String language)
        {
            var gender = (Int32) value;

            return gender > 0
                ? "/Resource/Images/Public/Gender/ic_gender_m_g.png"
                : "/Resource/Images/Public/Gender/ic_gender_f_g.png";
        }

        public object ConvertBack(object value, Type targetType, object parameter, String language)
        {
            throw new NotImplementedException();
        }
    }
}
