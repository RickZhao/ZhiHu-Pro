using System;

using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Shapes;

using GalaSoft.MvvmLight.Command;

using Zhihu.Common.Model;
using Zhihu.Common.Helper;


namespace Zhihu.Controls.ItemView
{
    public sealed partial class ProfileView : UserControl
    {
        private Profile _item;
        private RelayCommand<Profile> _itemTapped;

        public ProfileView()
        {
            this.InitializeComponent();

            this.Tapped += Item_OnTapped;
        }

        private void Item_OnTapped(object sender, TappedRoutedEventArgs tappedRoutedEventArgs)
        {
            if (_itemTapped != null)
                _itemTapped.Execute(_item);
        }

        internal void ShowPlaceHolder(Profile author)
        {
            _item = author;

            this.Avatar.Opacity = 0;
            this.Author.Opacity = 0;
            this.Headline.Opacity = 0;
        }

        internal void RegistEventHandler(RelayCommand<Profile> itemTapped)
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
            this.Avatar.Fill = new ImageBrush()
            {
                ImageSource = new BitmapImage(new Uri(AvartarHelper.GetLarge(_item.AvatarUrl))),
                Stretch = Stretch.Fill
            };
            this.Avatar.Opacity = 1;
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
