using System;
using System.Diagnostics;

using Windows.UI.Xaml.Data;

using Zhihu.Common.HtmlAgilityPack;
using Zhihu.Helper;


namespace Zhihu.Converter
{
    internal sealed class HtmlImageExtractConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            var htmlContent = value as String;

            if (String.IsNullOrEmpty(htmlContent)) return null;

            var htmlDoc = new HtmlDocument();

            htmlDoc.LoadHtml(htmlContent);

            //Debug.WriteLine("HAP完成解析：{0}", DateTime.Now);

            //Debug.WriteLine("RichText开始构建：{0}", DateTime.Now);

            var spans = HtmlHelper.Instance.GetAllImages(htmlDoc.DocumentNode.ChildNodes);

            return spans;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
