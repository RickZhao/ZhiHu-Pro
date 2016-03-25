using System;

using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Shapes;

using GalaSoft.MvvmLight.Command;

using Zhihu.Common.Model;
using Zhihu.Common.Helper;


namespace Zhihu.Controls.ItemView
{
    public sealed partial class SearchItemView : UserControl
    {
        private SearchItem _item;
        private RelayCommand<SearchItem> _itemTapped;

        public SearchItemView()
        {
            this.InitializeComponent();

            this.Tapped += Item_OnTapped;
        }

        private void Item_OnTapped(object sender, TappedRoutedEventArgs tappedRoutedEventArgs)
        {
            if (_itemTapped != null)
                _itemTapped.Execute(_item);
        }

        internal void ShowPlaceHolder(SearchItem item)
        {
            _item = item;

            Avatar.Opacity = 0;
            Title.Opacity = 0;
            Headline.Opacity = 0;
        }

        internal void RegistEventHandler(RelayCommand<SearchItem> itemTapped)
        {
            this._itemTapped = itemTapped;
        }

        internal void ShowTitle()
        {
            this.Title.Text = _item.Type == "topic" ? _item.Name : _item.Title;
            this.Title.Opacity = 1;
        }

        internal void ShowHeadline()
        {
            this.Headline.Text = String.IsNullOrEmpty(_item.Excerpt) ? String.Empty : _item.Excerpt;

            this.Headline.Opacity = 1;

            if (String.IsNullOrEmpty(Headline.Text))
            {
                Headline.Visibility = Visibility.Collapsed;
            }
        }

        internal void ShowFollowers()
        {
            if (this._item.Type == "topic")
            {
                OverView.Visibility = Visibility.Collapsed;
                return;
            }

            Followers.Text = _item.FollowerCount.ToString();
            Followers.Opacity = 1;
        }

        internal void ShowAnswers()
        {
            if (this._item.Type == "topic")
            {
                OverView.Visibility = Visibility.Collapsed;
                return;
            }

            Answers.Text = _item.AnswerCount.ToString();
            Answers.Opacity = 1;
        }

        internal void ShowAvatar()
        {
            if (_item.Type == "topic")
            {
                Avatar.Visibility = Visibility.Visible;

                SetAvatar();
            }
            else if (_item.Type == "question")
            {
                Avatar.Visibility = Visibility.Collapsed;
            }
        }

        private void SetAvatar()
        {
            Avatar.Fill = new ImageBrush()
            {
                ImageSource = new BitmapImage(new Uri(AvartarHelper.GetLarge(_item.AvatarUrl))),
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

            this.Avatar.ClearValue(Ellipse.FillProperty);
        }
    }
}
