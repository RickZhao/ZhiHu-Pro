using System;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Runtime.InteropServices.WindowsRuntime;

using Windows.Storage;
using Windows.UI.Text;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media.Imaging;

using Newtonsoft.Json;

using Zhihu.Common.Cache;
using Zhihu.Common.Helper;
using Zhihu.Common.Model;
using Zhihu.Common.Rtf;

using Itenso.Rtf;
using Itenso.Rtf.Converter.Image;
using Itenso.Rtf.Interpreter;
using Itenso.Rtf.Parser;
using Itenso.Rtf.Support;


namespace Zhihu.Controls
{
    public sealed partial class UgcPad : UserControl
    {
        public UgcPad()
        {
            this.InitializeComponent();
        }

        public static readonly DependencyProperty AnonymousVisiableProperty = DependencyProperty.Register("AnonymousVisiable", typeof(Visibility), typeof(UgcPad), new PropertyMetadata(Visibility.Visible));

        public Visibility AnonymousVisiable
        {
            get { return (Visibility)GetValue(AnonymousVisiableProperty); }
            set { SetValue(AnonymousVisiableProperty, value); }
        }

        public static readonly DependencyProperty AnonymousProperty = DependencyProperty.Register("Anonymous", typeof(Boolean), typeof(UgcPad), new PropertyMetadata(false));

        public Boolean Anonymous
        {
            get { return (Boolean)GetValue(AnonymousProperty); }
            set { SetValue(AnonymousProperty, value); }
        }

        public static readonly DependencyProperty TextProperty = DependencyProperty.Register("Text", typeof(String), typeof(UgcPad), new PropertyMetadata(String.Empty));

        public String Text
        {
            get { return (String)GetValue(TextProperty); }
            private set { SetValue(TextProperty, value); }
        }

        private Int32 GetNewSize(double pixelWidth, double original)
        {
            var i = 0.9 * this.RichBox.ActualWidth / pixelWidth;

            return (Int32)(original * i);
        }

        private async void PostImageButton_Tapped(object sender, TappedRoutedEventArgs e)
        {
            var file = await LocalStoreHelper.Instance.PickImage();

            if (file == null) return;

            var fileName = String.Format("ZhihuPro_Uploaded_{0}", file.Name);

            #region Insert image

            using (var readStream = await file.OpenAsync(FileAccessMode.Read))
            using (var randomStream = readStream.AsStream().AsRandomAccessStream())
            {
                var image = new BitmapImage();

                await image.SetSourceAsync(randomStream);

                var iWidth = GetNewSize(image.PixelWidth, image.PixelWidth);
                var iHeight = GetNewSize(image.PixelWidth, image.PixelHeight);

                this.RichBox.Document.Selection.InsertImage(iWidth, iHeight, 0, VerticalCharacterAlignment.Baseline, fileName, randomStream);
            }

            #endregion

            var uploaded = await DbContext.Instance.CheckUploadedImage(fileName);

            if (uploaded == null)
            {
                #region Upload to server

                using (var readStream = await file.OpenAsync(FileAccessMode.Read))
                using (var randomStream = readStream.AsStream().AsRandomAccessStream())
                using (var memoryStream = readStream.AsStreamForRead())
                {
                    var streamContent = new StreamContent(memoryStream);
                    streamContent.Headers.ContentLength = memoryStream.Length;
                    streamContent.Headers.ContentType = new MediaTypeHeaderValue("image/*");
                    streamContent.Headers.ContentDisposition = new ContentDispositionHeaderValue("form-data")
                    {
                        DispositionType = "form-data",
                        Name = "picture",
                        FileName = fileName,
                    };

                    this.RichBox.IsReadOnly = true;
                    this.ProgressRing.IsActive = true;

                    var uploadRsult = await CacheHelper.Instance.UploadImageAsync(Utility.Instance.BaseUri, "upload_image", streamContent, LoginUser.Current.Token);

                    this.RichBox.IsReadOnly = false;
                    this.ProgressRing.IsActive = false;

                    if (false == String.IsNullOrEmpty(uploadRsult.Json))
                    {
                        var json = uploadRsult.Json;
                        var imgUploadRslt = JsonConvert.DeserializeObject<ImageUploadResult>(json);

                        await DbContext.Instance.StoreUploadedImage(fileName, json);
                    }
                    else
                    {

                        this.RichBox.Document.Selection.StartPosition--;
                        this.RichBox.Document.Selection.Delete(Windows.UI.Text.TextRangeUnit.Object, 1);

                        this.RichBox.Document.Selection.Text = "[图片上传失败]";
                    }
                }

                #endregion
            }
        }

        private void AtPersonButton_Tapped(object sender, TappedRoutedEventArgs e)
        {

        }

        private async Task<String> GetUgcPadContent()
        {
            String richText;

            #region Get RichEdit Raw Data

            this.RichBox.Document.GetText(TextGetOptions.FormatRtf, out richText);

            #endregion

            var structureBuilder = new RtfParserListenerStructureBuilder();
            var parser = new RtfParser(structureBuilder);
            parser.IgnoreContentAfterRootGroup = true; // support WordPad documents

            parser.Parse(new RtfSource(richText));

            var rtfStructure = structureBuilder.StructureRoot;

            // image converter
            var imageAdapter = new RtfVisualImageAdapter(ImageFormat.Jpeg);
            var imageConvertSettings = new RtfImageConvertSettings(imageAdapter);
            imageConvertSettings.ScaleImage = true; // scale images
            var imageConverter = new RtfImageConverter(imageConvertSettings);

            // rtf interpreter
            var interpreterSettings = new RtfInterpreterSettings();

            var rtfDocument = RtfInterpreterTool.BuildDoc(rtfStructure, interpreterSettings, imageConverter);

            var htmlSegments = await RtfToHtmlSegments(rtfDocument.VisualContent);

            return htmlSegments;
        }

        private async Task<String> RtfToHtmlSegments(IRtfVisualCollection rtfVisualContent)
        {
            var htmlSegments = new StringBuilder();

            foreach (var visualChild in rtfVisualContent)
            {
                if (visualChild is IRtfVisualImage)
                {
                    var uploaded = await DbContext.Instance.CheckUploadedImage((visualChild as IRtfVisualImage).ImportFileName);

                    if (uploaded == null) Debugger.Break();
                    var json = uploaded.Json;
                    var imgUploadRslt = JsonConvert.DeserializeObject<ImageUploadResult>(json);

                    var imgHtml = String.Format("<img data-rawwidth=\"{0}\" data-rawheight = \"{1}\" class=\"content_image\" src=\"{2}\" style=\"margin:auto auto;width:100%\">",
                        imgUploadRslt.RawWidth, imgUploadRslt.RawHeight, imgUploadRslt.Source);

                    htmlSegments.Append(imgHtml);
                }
                else if (visualChild is IRtfVisualBreak)
                {
                    htmlSegments.Append("<br>");
                }
                else if (visualChild is IRtfVisualText)
                {
                    htmlSegments.Append((visualChild as IRtfVisualText).Text);
                }
            }
            return htmlSegments.ToString();
        }

        private async void ButtonOk_Tapped(object sender, TappedRoutedEventArgs e)
        {
            Text = await GetUgcPadContent();
            Anonymous = this.Anonymity.IsChecked.HasValue && this.Anonymity.IsChecked.Value == true;
        }

        private void ButtonCancel_Tapped(object sender, TappedRoutedEventArgs e)
        {
            Text = String.Empty;
            Anonymous = this.Anonymity.IsChecked.HasValue && this.Anonymity.IsChecked.Value == true;
        }
    }
}
