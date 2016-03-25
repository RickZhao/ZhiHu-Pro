using System;
using System.Diagnostics;

using Windows.Foundation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;

using Zhihu.Common.Cache;
using Zhihu.Common.Helper;
using Zhihu.Common.Model.Html;
using Zhihu.Helper;


namespace Zhihu.Controls
{
    [TemplatePart(Name = HolderName, Type = typeof(Border))]
    [TemplatePart(Name = LoadingName, Type = typeof(Border))]
    [TemplatePart(Name = ContentName, Type = typeof(Image))]
    [TemplateVisualState(GroupName = "States", Name = "Normal")]
    [TemplateVisualState(GroupName = "States", Name = "Loading")]
    [TemplateVisualState(GroupName = "States", Name = "Image")]
    [TemplateVisualState(GroupName = "States", Name = "Failed")]
    public class PlaceholderImage : Control
    {
        private const String HolderName = "HolderBorder";
        private const String LoadingName = "LoadingBorder";
        private const String FailedBorderName = "FailedBorder";
        private const String ContentName = "ContentImage";

        private Border _holderPart;
        private Border _loadingPart;
        private Border _failedPart;
        private Image _content;

        #region Zoom Dependency Properties

        public ZoomMode ZoomMode
        {
            get { return (ZoomMode)GetValue(ZoomModeProperty); }
            set { SetValue(ZoomModeProperty, value); }
        }

        public static readonly DependencyProperty ZoomModeProperty = DependencyProperty.Register("ZoomMode", typeof(ZoomMode), typeof(PlaceholderImage), new PropertyMetadata(default(ZoomMode)));

        public ScrollBarVisibility HorizontalScrollBarVisibility
        {
            get { return (ScrollBarVisibility)GetValue(HorizontalScrollBarVisibilityProperty); }
            set { SetValue(HorizontalScrollBarVisibilityProperty, value); }
        }

        public static readonly DependencyProperty HorizontalScrollBarVisibilityProperty = DependencyProperty.Register("HorizontalScrollBarVisibility", typeof(ZoomMode), typeof(PlaceholderImage), new PropertyMetadata(default(ScrollBarVisibility)));

        public ScrollBarVisibility VerticalScrollBarVisibility
        {
            get { return (ScrollBarVisibility)GetValue(ZoomModeProperty); }
            set { SetValue(ZoomModeProperty, value); }
        }

        public static readonly DependencyProperty VerticalScrollBarVisibilityProperty = DependencyProperty.Register("VerticalScrollBarVisibility", typeof(ScrollBarVisibility), typeof(PlaceholderImage), new PropertyMetadata(default(ScrollBarVisibility)));

        #endregion

        internal ImageRun ImageContent
        {
            get { return (ImageRun)GetValue(ImageContentProperty); }
            set { SetValue(ImageContentProperty, value); }
        }

        public static readonly DependencyProperty ImageContentProperty = DependencyProperty.Register("ImageContent", typeof(ImageRun), typeof(PlaceholderImage), new PropertyMetadata(default(ImageRun), OnContentChanged_Callback));

        private static void OnContentChanged_Callback(DependencyObject dependencyObject,
            DependencyPropertyChangedEventArgs dependencyPropertyChangedEventArgs)
        {
            var holderImage = dependencyObject as PlaceholderImage;

            if (holderImage == null) return;

            var imgRun = dependencyPropertyChangedEventArgs.NewValue as ImageRun;
            if (imgRun == null) return;

            holderImage.OnContentChanged();
        }

        private void OnContentChanged()
        {
            LoadImage();
        }

        private Double _maxWidth;
        private Double _maxHeight;

        protected override Size MeasureOverride(Size availableSize)
        {
            Debug.WriteLine("AvailableSize: Width {0} Height {1}", availableSize.Width, availableSize.Height);

            this._maxWidth = availableSize.Width;
            this._maxHeight = availableSize.Height;

            return base.MeasureOverride(availableSize);
        }

        protected override Size ArrangeOverride(Size finalSize)
        {
            return base.ArrangeOverride(finalSize);
        }

        private Size ReCalSize(Double width, double height)
        {
            var avilableSize = new Size(this._maxWidth, this._maxHeight);

            if (width > avilableSize.Width || height > avilableSize.Height)
            {
                var heightScale = avilableSize.Height / height;
                var widthScale = avilableSize.Width / width;

                if (widthScale > heightScale)
                {
                    return new Size(width * heightScale * 0.96, height * heightScale * 0.96);
                }
                else
                {
                    return new Size(width * widthScale * 0.96, height * widthScale * 0.96);
                }
            }
            else
            {
                return new Size(width, height);
            }
        }

        public PlaceholderImage()
        {
            this.DefaultStyleKey = typeof(PlaceholderImage);

            this.Loaded -= PlaceholderImage_Loaded;
            this.Loaded += PlaceholderImage_Loaded;

            this.Tapped -= TapToLoadPart_Tapped;
            this.Tapped += TapToLoadPart_Tapped;
        }

        protected override void OnApplyTemplate()
        {
            _holderPart = GetTemplateChild(HolderName) as Border;
            _loadingPart = GetTemplateChild(LoadingName) as Border;
            _failedPart = GetTemplateChild(FailedBorderName) as Border;
            _content = GetTemplateChild(ContentName) as Image;

            this.Loaded -= PlaceholderImage_Loaded;
            this.Loaded += PlaceholderImage_Loaded;

            this.Tapped -= TapToLoadPart_Tapped;
            this.Tapped += TapToLoadPart_Tapped;

            base.OnApplyTemplate();
        }
        
        private async void TapToLoadPart_Tapped(object sender, TappedRoutedEventArgs e)
        {
            e.Handled = true;

            // avoid TapToLoad & TapToReload 
            if (this._holderPart.Visibility == Visibility.Collapsed && this._failedPart.Visibility == Visibility.Collapsed)
            {
                ImageViewerPopup.Instance.TogglePopup(ImageContent);
                return;
            }
            
            VisualStateManager.GoToState(this, "Loading", true);

            var imgHost = Utility.Instance.GetImageHost(ImageContent.Image);
            var imgRequest = Utility.Instance.GetImageRequest(ImageContent.Image);

            var cacheFileName = Utility.Instance.GetPlaintText(imgRequest);

            var img = await
                CacheHelper.Instance.LoadAndCacheImageAsync(String.Format("http://{0}", imgHost), imgRequest,
                    cacheFileName);

            var newSize = ReCalSize(this.ImageContent.Width, this.ImageContent.Height);

            /*this.Width =*/
            this._content.Width = newSize.Width;
            /*this.Height =*/
            this._content.Height = newSize.Height;

            this._content.Source = img;

            VisualStateManager.GoToState(this, this._content.Source == null ? "Failed" : "Image", true);

            if (img != null)
            {
                await DbContext.Instance.StoreCachedImage(cacheFileName);
            }
        }

        private Boolean _loaded = false;

        private void PlaceholderImage_Loaded(object sender, RoutedEventArgs e)
        {
            if (_loaded) return;

            _loaded = true;

            LoadImage();
        }

        private async void LoadImage()
        {
            if (ImageContent == null || _content == null) return;

            var imgHost = Utility.Instance.GetImageHost(ImageContent.Image);
            var imgRequest = Utility.Instance.GetImageRequest(ImageContent.Image);

            if (String.IsNullOrEmpty(imgRequest)) return;

            var cacheFileName = Utility.Instance.GetPlaintText(imgRequest);

            var cached = await DbContext.Instance.CheckCachedImage(cacheFileName);

            if (cached == null)
            {
                if (Theme.Instance.NoImage && Utility.Instance.IsUsingWifi == false) return;

                VisualStateManager.GoToState(this, "Loading", true);

                #region 图片未缓存

                var img =
                    await
                        CacheHelper.Instance.LoadAndCacheImageAsync(String.Format("http://{0}", imgHost), imgRequest, cacheFileName);

                if (img == null)
                {
                    VisualStateManager.GoToState(this, "Failed", true);
                    return;
                }

                var newSize = ReCalSize(ImageContent.Width, ImageContent.Height);

                /*this.Width =*/
                this._content.Width = newSize.Width;
                /*this.Height =*/
                this._content.Height = newSize.Height;

                await DbContext.Instance.StoreCachedImage(cacheFileName);

                this._content.Source = img;

                VisualStateManager.GoToState(this, "Image", true);

                #endregion
            }
            else
            {
                VisualStateManager.GoToState(this, "Loading", true);

                #region 图片已缓存直接读取缓存图片

                var newSize = ReCalSize(ImageContent.Width, ImageContent.Height);

                /*this.Width =*/
                this._content.Width = newSize.Width;
                /*this.Height =*/
                this._content.Height = newSize.Height;

                var cachedBitmap = await CacheHelper.Instance.GetCachedImage(cacheFileName);

                Debug.WriteLine("Read cached image for: {0} , cached file name: {1}", ImageContent.Image, cacheFileName);

                if (cachedBitmap != null)
                {
                    #region 成功读取缓存图片

                    this._content.Source = cachedBitmap;

                    #endregion

                    VisualStateManager.GoToState(this, "Image", true);
                }
                else
                {
                    #region 缓存图片读取失败

                    await DbContext.Instance.RemoveCachedImage(cacheFileName);

                    #endregion

                    VisualStateManager.GoToState(this, "Normal", true);
                }

                #endregion
            }
        }

    }
}
