using System;
using System.Diagnostics;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Threading.Tasks;

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

using Itenso.Rtf.Parser;
using Itenso.Rtf.Support;
using Itenso.Rtf.Converter.Image;
using Itenso.Rtf.Interpreter;
using Itenso.Rtf;



namespace Zhihu.View
{
    public class UgcDialogResult
    {
        public Boolean Canceled { get; private set; }
        public Boolean Anonymited { get; private set; }
        public String Content { get; private set; }

        public UgcDialogResult()
        {
            Canceled = true;
            Anonymited = false;
            Content = String.Empty;
        }

        public UgcDialogResult(Boolean anonymited, String content) : this()
        {
            Canceled = false;
            Anonymited = anonymited;
            Content = content;
        }
    }

    public sealed partial class UgcDialog : ContentDialog
    {
        public UgcDialogResult Result { get; private set; }

        private UgcDialog()
        {
            this.InitializeComponent();

            this.Result = new UgcDialogResult();
        }

        public UgcDialog(String title, String leftBtnText, String rightBtnText, Boolean anonymitable = false, Boolean atable = false, Boolean imageable = false) : this()
        {
            this.Title = title;
            this.PrimaryButtonText = leftBtnText;
            this.SecondaryButtonText = rightBtnText;

            this.Anonymity.Visibility = anonymitable ? Visibility.Visible : Visibility.Collapsed;
            this.AtButton.Visibility = atable ? Visibility.Visible : Visibility.Collapsed;
            this.ImgButton.Visibility = imageable ? Visibility.Visible : Visibility.Collapsed;
        }

        private async void ContentDialog_PrimaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
            String richText;

            this.RichBox.Document.GetText(TextGetOptions.FormatRtf, out richText);

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

            var anonymited = this.Anonymity.IsChecked.HasValue && this.Anonymity.IsChecked.Value == true;

            this.Result = new UgcDialogResult(anonymited, htmlSegments);

            //var deferral = args.GetDeferral();

            //deferral.Complete();
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

        private void ContentDialog_SecondaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
            this.Result = new UgcDialogResult();
        }

        private void AtPersonButton_Tapped(object sender, TappedRoutedEventArgs e)
        {

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
                var cachedImage = new BitmapImage();

                await cachedImage.SetSourceAsync(randomStream);

                var iWidth = GetNewSize(cachedImage.PixelWidth, cachedImage.PixelWidth);
                var iHeight = GetNewSize(cachedImage.PixelWidth, cachedImage.PixelHeight);

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
                        this.RichBox.Document.Selection.Delete(TextRangeUnit.Object, 1);

                        this.RichBox.Document.Selection.Text = "[图片上传失败]";
                    }
                }

                #endregion
            }
        }
    }
}
