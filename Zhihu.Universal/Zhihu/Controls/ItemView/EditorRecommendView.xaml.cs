using System;

using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Shapes;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Media;

using GalaSoft.MvvmLight.Command;

using Zhihu.Common.Model;
using Zhihu.Common.Helper;
using Windows.UI.Xaml.Input;

namespace Zhihu.Controls.ItemView
{
    public sealed partial class EditorRecommendView : UserControl
    {
        private EditorRecommend _item;
        private RelayCommand<EditorRecommend> _titleTapped;
        private RelayCommand<EditorRecommend> _summaryTapped;
        private RelayCommand<EditorRecommend> _authorTapped;

        public EditorRecommendView()
        {
            this.InitializeComponent();
            this.Title.Tapped += Title_Tapped;
            this.Summary.Tapped += Summary_Tapped;
            this.AuthorContainer.Tapped += AuthorContainer_Tapped;
        }

        private void Summary_Tapped(object sender, TappedRoutedEventArgs e)
        {
            if (_summaryTapped != null)
                _summaryTapped.Execute(_item);
        }

        private void Title_Tapped(object sender, TappedRoutedEventArgs e)
        {
            if (_titleTapped != null)
                _titleTapped.Execute(_item);
        }

        private void AuthorContainer_Tapped(object sender, TappedRoutedEventArgs e)
        {
            if (_authorTapped != null)
                _authorTapped.Execute(_item);
        }

        internal void ShowPlaceHolder(EditorRecommend recommend)
        {
            _item = recommend;

            Title.Opacity = 0;
            Avator.Opacity = 0;
            VoteBorder.Opacity = 0;
            VoteCount.Opacity = 0;
            Summary.Opacity = 0;
        }

        internal void ShowTitle()
        {
            if (_item.Type == "answer")
            {
                Title.Text = _item.Question.Title;
            }
            else if (_item.Type == "column")
            {
                Title.Text = _item.Column.Title;
            }
            else if (_item.Type == "article")
            {
                Title.Text = _item.Title;
            }
            else
            {
            }
            Title.Opacity = 1;
        }

        internal void ShowSummary()
        {
            Summary.Text = _item.Excerpt;
            Summary.Opacity = 1;
            Summary.Visibility = Visibility.Visible;
        }

        internal void ShowAvatar()
        {
            Avator.Fill = new ImageBrush()
            {
                ImageSource = new BitmapImage(new Uri(AvartarHelper.GetLarge(_item.Author.AvatarUrl))),
                Stretch = Stretch.Fill,
            };
            Avator.Opacity = 1;
            Avator.Visibility = Visibility.Visible;
        }

        internal void ShowVoteCount()
        {
            VoteCount.Text = Utility.Instance.GetEasyInt32(_item.VoteupCount);

            VoteBorder.Opacity = VoteCount.Opacity = 1;
            VoteBorder.Visibility = VoteCount.Visibility = Visibility.Visible;
        }

        internal void Clear()
        {
            _item = null;

            Title.ClearValue(TextBlock.TextProperty);
            Avator.ClearValue(Ellipse.FillProperty);
            VoteCount.ClearValue(TextBlock.TextProperty);
            Summary.ClearValue(TextBlock.TextProperty);
        }

        internal void RegisteEventHandler(RelayCommand<EditorRecommend> titleEventHandler,
            RelayCommand<EditorRecommend> summaryEventHandler, RelayCommand<EditorRecommend> authorEventHandler)
        {
            _authorTapped = authorEventHandler;

            _titleTapped = titleEventHandler;

            _summaryTapped = summaryEventHandler;
        }
    }
}
