using System;

using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media.Imaging;

using GalaSoft.MvvmLight.Command;

using Zhihu.Common.Helper;
using Zhihu.Common.Model;
using Zhihu.Converter;


namespace Zhihu.Controls.ItemView
{
    public sealed partial class ArticleView : UserControl
    {
        private readonly RelativeTimeConverter _timeConverter = new RelativeTimeConverter();
        private readonly CountToStringConverter _countConverter = new CountToStringConverter();

        private Article _item;
        private RelayCommand<Article> _articleTapped;

        public ArticleView()
        {
            this.InitializeComponent();

            this.Tapped += ArticleView_OnTapped;
        }

        private void ArticleView_OnTapped(object sender, TappedRoutedEventArgs tappedRoutedEventArgs)
        {
            if (_articleTapped != null)
            {
                _articleTapped.Execute(_item);
            }
        }

        internal void ShowPlaceHolder(Article item)
        {
            this._item = item;

            this.Avatar.Opacity = 0;
            this.Title.Opacity = 0;
            this.Excerpt.Opacity = 0;
            this.Author.Opacity = 0;
            this.CreatedTime.Opacity = 0;
            this.Comments.Opacity = 0;
            this.Voteup.Opacity = 0;
        }

        internal void RegistEventHandler(RelayCommand<Article> authorTapped)
        {
            this._articleTapped = authorTapped;
        }

        internal void ShowAvatar()
        {
            if (String.IsNullOrEmpty(_item.ImageUrl))
            {
                this.Avatar.Visibility = Visibility.Collapsed;
                return;
            }

            this.Avatar.Visibility = Visibility.Visible;

            this.Avatar.Source = new BitmapImage
            {
                UriSource = new Uri(AvartarHelper.GetLarge(_item.ImageUrl))
            };

            this.Avatar.Opacity = 1;
        }

        internal void ShowTitle()
        {
            this.Title.Text = _item.Title;
            this.Title.Opacity = 1;
        }

        internal void ShowExcerpt()
        {
            this.Excerpt.Text = _item.Excerpt;
            this.Excerpt.Opacity = 1;

            this.Excerpt.Visibility = String.IsNullOrEmpty(_item.ImageUrl) ? Visibility.Visible : Visibility.Collapsed;
        }

        internal void ShowAuthor()
        {
            this.Author.Text = _item.Author.Name;
            this.Author.Opacity = 1;
        }

        internal void ShowCreatedTime()
        {
            this.CreatedTime.Text = _timeConverter.Convert(_item.Updated, null, null, null).ToString();
            this.CreatedTime.Opacity = 1;
        }

        internal void ShowComments()
        {
            this.Comments.Text = _countConverter.Convert(_item.CommentCount, null, null, null).ToString();
            this.Comments.Opacity = 1;
        }

        internal void ShowVoteUp()
        {
            this.Voteup.Text = _countConverter.Convert(_item.VoteupCount, null, null, null).ToString();
            this.Voteup.Opacity = 1;
        }

        internal void Clear()
        {
            _item = null;

            this.Avatar.ClearValue(Image.SourceProperty);
            this.Title.ClearValue(TextBlock.TextProperty);
            this.Excerpt.ClearValue(TextBlock.TextProperty);
            this.Author.ClearValue(TextBlock.TextProperty);
            this.CreatedTime.ClearValue(TextBlock.TextProperty);
            this.Comments.ClearValue(TextBlock.TextProperty);
            this.Voteup.ClearValue(TextBlock.TextProperty);
        }
    }
}
