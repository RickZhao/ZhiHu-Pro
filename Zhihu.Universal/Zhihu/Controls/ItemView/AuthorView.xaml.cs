using System;

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
    public sealed partial class AuthorView : UserControl
    {
        private Author _item;
        private RelayCommand<Author> _itemTapped;

        public AuthorView()
        {
            this.InitializeComponent();

            this.Tapped += Item_OnTapped;
        }

        private void Item_OnTapped(object sender, TappedRoutedEventArgs tappedRoutedEventArgs)
        {
            if (_itemTapped != null)
                _itemTapped.Execute(_item);
        }

        internal void ShowPlaceHolder(Author author)
        {
            _item = author;

            Avatar.Opacity = 0;
            Author.Opacity = 0;
            Headline.Opacity = 0;
        }

        internal void RegistEventHandler(RelayCommand<Author> itemTapped)
        {
            this._itemTapped = itemTapped;
        }

        internal void ShowAuthor()
        {
            this.Author.Text = _item.Name;
            this.Author.Opacity = 1;
        }

        internal void ShowHeadline()
        {
            this.Headline.Text = _item.Headline;
            this.Headline.Opacity = 1;
        }

        internal void ShowAvatar()
        {
            Avatar.Fill = new ImageBrush()
            {
                ImageSource = new BitmapImage(new Uri(AvartarHelper.GetLarge(_item.AvatarUrl))),
                Stretch = Stretch.Fill
            };
            Avatar.Opacity = 1;
        }

        internal void Clear()
        {
            _item = null;
            _itemTapped = null;

            this.Author.ClearValue(TextBlock.TextProperty);
            this.Headline.ClearValue(TextBlock.TextProperty);

            this.Avatar.ClearValue(Ellipse.FillProperty);
        }
    }
}
