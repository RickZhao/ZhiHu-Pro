using System;
using System.Collections.Generic;
using System.Linq;

using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Input;
using Zhihu.Common.Cache;
using Zhihu.Common.Helper;
using Zhihu.Common.Model.Html;


namespace Zhihu.Controls
{
    public class ImageViewerHelper : Control
    {
        public static readonly DependencyProperty ImageRunsProperty = DependencyProperty.Register(
            "ImageRuns", typeof(List<ImageRun>), typeof(ImageViewerHelper), new PropertyMetadata(null, OnContentChangedCallback));

        public List<ImageRun> ImageRuns
        {
            get { return (List<ImageRun>)GetValue(ImageRunsProperty); }
            set { SetValue(ImageRunsProperty, value); }
        }

        private static void OnContentChangedCallback(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs eventArgs)
        {
            var imgViewer = dependencyObject as ImageViewerHelper;

            if (imgViewer == null) return;

            var imgRuns = eventArgs.NewValue as IList<ImageRun>;
            if (imgRuns == null) return;

            imgViewer.PushImages(imgRuns);
        }

        private void PushImages(IList<ImageRun> images)
        {
            ImageViewerPopup.Instance.Push(images);
        }
    }

    public sealed class ImageViewerPopup
    {
        private Popup _popup;
        private readonly ImageViewer _imageViewer;

        private Dictionary<List<ImageRun>, Boolean> _imageSets = new Dictionary<List<ImageRun>, Boolean>();

        #region Singleton

        private static ImageViewerPopup _instance;
        public static ImageViewerPopup Instance
        {
            get
            {
                if (_instance == null) _instance = new ImageViewerPopup();

                return _instance;
            }
        }

        #endregion

        private ImageViewerPopup()
        {
            _imageViewer = new ImageViewer();
            _popup = new Popup { Child = _imageViewer };
        }

        public void Push(IList<ImageRun> imageSet)
        {
            if (imageSet == null || imageSet.Count == 0) return;

            _imageSets.Add(new List<ImageRun>(imageSet), false);
        }

        public void Clear()
        {
            _imageSets.Clear();
        }

        public Boolean IsOpen
        {
            get
            {
                if (_popup == null || _popup.IsOpen == false) return false;

                return true;
            }
        }

        public void TogglePopup(ImageRun imgRun)
        {
            if (imgRun == null || _imageSets.Count == 0) return;

            var imgSet = (from imgset in _imageSets
                          where imgset.Key.FirstOrDefault(item => item.Image == imgRun.Image) != null
                          select imgset).FirstOrDefault();

            if (imgSet.Key == null) return;

            if (imgSet.Value)
            {
                _popup.IsOpen = false;
                _imageSets[imgSet.Key] = false;
            }
            else
            {
                var selected = imgSet.Key.FirstOrDefault(img => img.Image == imgRun.Image);

                _imageViewer.SetDatasource(imgSet.Key, selected);

                _imageSets[imgSet.Key] = true;

                _popup.IsOpen = true;
            }
        }
    }

    public sealed partial class ImageViewer : UserControl
    {
        public ImageViewer()
        {
            this.InitializeComponent();

            if (this.Root != null)
            {
                this.Root.Tapped += ImageOuter_Tapped;
            }

            if (this.SaveButton != null)
            {
                this.SaveButton.Tapped += _saveButton_Tapped;
            }
        }

        private ImageRun _firstImageRun = null;
        public void SetDatasource(IList<ImageRun> images, ImageRun selected)
        {
            this.FlipView.ItemsSource = images;

            if (selected != null)
            {
                _firstImageRun = selected;
                this.FlipView.SelectedItem = selected;
            }
        }

        private bool _saving = false;

        private async void _saveButton_Tapped(object sender, TappedRoutedEventArgs e)
        {
            e.Handled = true;

            if (_saving == true) return;

            _saving = true;

            var selected = this.FlipView.SelectedItem as ImageRun;

            if (selected == null) return;

            var imgHost = Utility.Instance.GetImageHost(selected.Image);
            var imgRequest = Utility.Instance.GetImageRequest(selected.Image);

            var cacheFileName = Utility.Instance.GetPlaintText(imgRequest);

            var cached = await DbContext.Instance.CheckCachedImage(cacheFileName);

            if (cached == null) return;
            
            var imgBytes = await CacheHelper.Instance.GetCachedImageBytes(cacheFileName);

            var hasSaved = await LocalStoreHelper.Instance.SaveIntoPictureLib(imgBytes);

            ToasteIndicator.Instance.Show(string.Empty, hasSaved ? "图片已保存至手机" : "图片保存失败", null, 2);

            _saving = false;
        }

        private void ImageOuter_Tapped(object sender, TappedRoutedEventArgs e)
        {
            ImageViewerPopup.Instance.TogglePopup(_firstImageRun);
        }
    }
}
