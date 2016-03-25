using System;
using System.Linq;

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
    internal sealed partial class FeedView : UserControl
    {
        private Feed _item;

        private RelayCommand<Feed> _titleTapped;
        private RelayCommand<Feed> _authorTapped;
        private RelayCommand<Feed> _summaryTapped;

        public FeedView()
        {
            this.InitializeComponent();

            this.Author.Tapped += Author_Tapped;
            this.Avatar.Tapped += Author_Tapped;

            this.Title.Tapped += Title_Tapped;
            this.Summary.Tapped += Summary_Tapped;
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

        private void Author_Tapped(object sender, TappedRoutedEventArgs e)
        {
            if (_authorTapped != null)
                _authorTapped.Execute(_item);
        }

        internal void ShowPlaceholder(Feed feed)
        {
            _item = feed;

            this.From.Opacity = 0;
            this.From.Visibility = Visibility.Collapsed;

            this.Author.Opacity = 0;
            this.Author.Visibility = Visibility.Collapsed;

            this.Verb.Opacity = 0;
            this.Verb.Visibility = Visibility.Collapsed;

            this.Avatar.Opacity = 0;

            this.Title.Opacity = 0;

            this.VoteCount.Opacity = 0;
            this.Summary.Opacity = 0;
        }

        internal void RegistEventHandler(RelayCommand<Feed> authorEventHandler,
            RelayCommand<Feed> titleEventHandler,
            RelayCommand<Feed> summaryEventHandler)
        {
            _authorTapped = authorEventHandler;

            _titleTapped = titleEventHandler;

            _summaryTapped = summaryEventHandler;
        }

        internal void ShowAuthor()
        {
            if (this._item.Verb == "PROMOTION_ANSWER")
            {
                return;
            }

            if (this._item.Verb == "PROMOTION_ARTICLE")
            {
                return;
            }

            if (this._item.Verb == "COLUMN_POPULAR_ARTICLE" || this._item.Verb == "COLUMN_NEW_ARTICLE")
            {
                this.Author.Opacity = 1;
                this.Author.Text = _item.Target.Column.Title;
                this.Author.Visibility = Visibility.Visible;

                return;
            }

            this.Author.Opacity = 1;
            this.Author.Text = _item.Actors.Length == 1
                ? _item.Actors[0].Name
                : String.Format("{0} 等", _item.Actors[0].Name);
            this.Author.Visibility = Visibility.Visible;
        }

        internal void ShowVerb()
        {
            if (this._item.Verb == "TOPIC_POPULAR_QUESTION" || _item.Verb == "TOPIC_ACKNOWLEDGED_ANSWER" ||
                _item.Verb == "ROUNDTABLE_FOLLOW" || _item.Verb == "MEMBER_FOLLOW_ROUNDTABLE")
            {
                this.From.Opacity = 1;
                this.From.Visibility = Visibility.Visible;
            }

            var verbModel = VerbModels.ForFeed.FirstOrDefault(item => item.Verbs.Contains(this._item.Verb));

            if (verbModel != null)
            {
                this.Verb.Opacity = 1;
                this.Verb.Text = _item.Verb == "ARTICLE_CREATE"
                    ? String.Format(verbModel.Display, _item.Target.Column.Title)
                    : verbModel.Display;
                this.Verb.Visibility = Visibility.Visible;
            }
            else
            {

            }
        }


        internal void ShowTitle()
        {
            if (_item.Target.Type == "answer")
            {
                this.Title.Text = _item.Target.Question.Title;
            }
            else if (_item.Target.Type == "question" ||
                     _item.Target.Type == "article" ||
                     _item.Target.Type == "column")
            {
                this.Title.Text = _item.Target.Title;
            }
            else if (_item.Target.Type == "roundtable")
            {
                this.Title.Text = _item.Target.Name;
            }
            else
            {

            }

            this.Title.Opacity = 1;
        }

        internal void ShowSummary()
        {
            if (_item.Target.Type == "roundtable" || _item.Target.Type == "question" ||
                _item.Target.Type == "column")
            {
                this.SummaryContainer.Visibility = Visibility.Collapsed;
                return;
            }

            if (this._item.Verb == "TOPIC_ACKNOWLEDGED_ANSWER")
            {
                this.SummaryContainer.Visibility = Visibility.Visible;

                this.Summary.Text = _item.Target.Excerpt;
                this.Summary.Visibility = Visibility.Visible;
                this.Summary.Opacity = 1;

                return;
            }

            var isFeedQuestionRelative = _item.Verb.Contains("QUESTION_");

            this.Summary.Text = isFeedQuestionRelative
                ? String.Empty
                : String.IsNullOrEmpty(_item.Target.Excerpt) ? String.Empty : _item.Target.Excerpt;

            this.SummaryContainer.Visibility =
                Summary.Visibility = isFeedQuestionRelative ? Visibility.Collapsed : Visibility.Visible;

            this.Summary.Opacity = isFeedQuestionRelative ? 0 : 1;
        }

        internal void ShowVoteup()
        {
            if (_item.Target.Type == "roundtable" || _item.Target.Type == "question" ||
                _item.Target.Type == "column")
            {
                this.SummaryContainer.Visibility = Visibility.Collapsed;
                return;
            }

            if (this._item.Verb == "TOPIC_ACKNOWLEDGED_ANSWER")
            {
                this.SummaryContainer.Visibility = Visibility.Visible;

                this.VoteCount.Text = _item.Target.VoteupCount.HasValue
                    ? Utility.Instance.GetEasyInt32(_item.Target.VoteupCount.Value)
                    : String.Empty;

                this.VoteCount.Opacity = 1;

                return;
            }

            var isFeedQuestionRelative = _item.Verb.Contains("QUESTION_");

            this.VoteCount.Text = isFeedQuestionRelative
                ? String.Empty
                : _item.Target.VoteupCount.HasValue
                    ? Utility.Instance.GetEasyInt32(_item.Target.VoteupCount.Value)
                    : "0";

            this.VoteCount.Opacity = isFeedQuestionRelative ? 0 : 1;
        }

        internal void ShowAvatar()
        {
            if (_item.Actors == null || _item.Actors.Length == 0) return;

            Avatar.Fill = new ImageBrush()
            {
                ImageSource = new BitmapImage(new Uri(AvartarHelper.GetLarge(_item.Actors[0].AvatarUrl))),
                Stretch = Stretch.Fill
            };
            Avatar.Opacity = 1;
        }

        internal void Clear()
        {
            this._item = null;

            this.Author.ClearValue(TextBlock.TextProperty);
            this.Verb.ClearValue(TextBlock.TextProperty);
            this.Avatar.ClearValue(Ellipse.FillProperty);

            this.Title.ClearValue(TextBlock.TextProperty);

            this.VoteCount.ClearValue(TextBlock.TextProperty);
            this.Summary.ClearValue(TextBlock.TextProperty);

            this._authorTapped = null;
            this._titleTapped = null;
            this._summaryTapped = null;
        }
    }
}
