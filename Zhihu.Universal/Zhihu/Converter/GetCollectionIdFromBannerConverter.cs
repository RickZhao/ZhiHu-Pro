using System;
using System.Linq;

using Windows.UI.Xaml.Data;

using Zhihu.Common.Model;


namespace Zhihu.Converter
{
    internal class GetCollectionIdFromBannerConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, String language)
        {
            var banner = value as Banner;

            if (banner == null) return -1;

            var defaultBanner = banner.Default.FirstOrDefault();

            if (defaultBanner == null) return -1;

            var idStartIndex = defaultBanner.Url.LastIndexOf("collections/", StringComparison.Ordinal) +
                               "collections/".Length;
            var collectionId = -1;
            Int32.TryParse(defaultBanner.Url.Substring(idStartIndex), out collectionId);

            return collectionId;
        }

        public object ConvertBack(object value, Type targetType, object parameter, String language)
        {
            throw new NotImplementedException();
        }
    }
}
