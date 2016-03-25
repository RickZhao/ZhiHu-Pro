using System;

using Windows.UI.Xaml.Data;


namespace Zhihu.Converter
{
    internal sealed class SubStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            var formalString = value as String;

            if (String.IsNullOrEmpty(formalString)) return String.Empty;

            var iSubLength = -1;

            Int32.TryParse(parameter.ToString(), out iSubLength);

            if (iSubLength <= 0) return formalString;

            return formalString.Substring(0, iSubLength) + "...";
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
