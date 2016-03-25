using System;

using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Shapes;

using GalaSoft.MvvmLight.Command;

using Zhihu.Common.Helper;
using Zhihu.Common.Model;


namespace Zhihu.Controls.ItemView
{
    public sealed partial class ColumnView : UserControl
    {
        private Column _item;
        private RelayCommand<Column> _itemTapped;

        public ColumnView()
        {
            this.InitializeComponent();
            this.Tapped += Column_OnTapped;
        }

        private void Column_OnTapped(object sender, TappedRoutedEventArgs tappedRoutedEventArgs)
        {
            if (_itemTapped != null)
                _itemTapped.Execute(_item);
        }


        internal void ShowPlaceHolder(Column item)
        {
            _item = item;

            Avatar.Opacity = 0;
            Title.Opacity = 0;
            ArticlesCount.Opacity = 0;
        }

        internal void RegistEventHandler(RelayCommand<Column> itemTapped)
        {
            this._itemTapped = itemTapped;
        }

        internal void ShowTitle()
        {
            this.Title.Text = _item.Title;
            this.Title.Opacity = 1;
        }

        internal void ShowHeadline()
        {
            this.Headline.Text = String.IsNullOrEmpty(_item.Description) ? String.Empty : _item.Description;
            this.Headline.Visibility = String.IsNullOrEmpty(_item.Description) ? Visibility.Visible : Visibility.Collapsed;
        }

        internal void ShowArticlsCount()
        {
            ArticlesCount.Text = _item.ArticlesCount.ToString();
            ArticlesCount.Opacity = 1;
        }

        internal void ShowAvatar()
        {
            Avatar.Fill = new ImageBrush()
            {
                ImageSource = new BitmapImage(new Uri(AvartarHelper.GetLarge(_item.ImageUrl))),
                Stretch = Stretch.Fill
            };
            Avatar.Opacity = 1;
        }

        public void Clear()
        {
            _item = null;
            _itemTapped = null;

            this.Title.ClearValue(TextBlock.TextProperty);
            this.Headline.ClearValue(TextBlock.TextProperty);
            this.ArticlesCount.ClearValue(TextBlock.TextProperty);

            this.Avatar.ClearValue(Ellipse.FillProperty);
        }
    }
}
