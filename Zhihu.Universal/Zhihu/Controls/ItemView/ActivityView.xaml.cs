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
    public sealed partial class ActivityView : UserControl
    {
        private Activity _item;

        private RelayCommand<Activity> _titleTapped;
        private RelayCommand<Activity> _authorTapped;
        private RelayCommand<Activity> _summaryTapped;

        public ActivityView()
        {
            this.InitializeComponent();

            this.Author.Tapped += Author_Tapped;
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

        internal void ShowPlaceholder(Activity activity)
        {
            _item = activity;

            this.Author.Opacity = 0;
            this.Verb.Opacity = 0;
            this.Avatar.Opacity = 0;

            this.Title.Opacity = 0;

            this.VoteCount.Opacity = 0;
            this.Summary.Opacity = 0;
        }

        internal void RegistEventHandler(RelayCommand<Activity> authorEventHandler,
            RelayCommand<Activity> titleEventHandler,
            RelayCommand<Activity> summaryEventHandler)
        {
            _authorTapped = authorEventHandler;

            _titleTapped = titleEventHandler;

            _summaryTapped = summaryEventHandler;
        }


        internal void ShowTitle()
        {
            // roundtable question activity
            if (_item.Verb == null)
            {
                this.Title.Text = String.IsNullOrEmpty(_item.Title) ? _item.Question.Title : _item.Title;

                this.Title.Opacity = 1;
                return;
            }

            // QUESTION_FOLLOW & QUESTION_CREATE
            if (true == _item.Verb.Contains("QUESTION_"))
            {
                this.Title.Text = _item.Target.Title;
            }
            else if (true == _item.Verb.Contains("ANSWER_"))
            {
                this.Title.Text = _item.Target.Question.Title;
            }
            else if (true == _item.Verb.Contains("ARTICLE_"))
            {
                this.Title.Text = _item.Target.Title;
            }
            else if (_item.Verb == "MEMBER_FOLLOW_COLLECTION")
            {
                this.Title.Text = _item.Target.Title;
            }
            else if (_item.Verb == "MEMBER_FOLLOW_COLUMN")
            {
                this.Title.Text = _item.Target.Title;
            }
            else if (_item.Verb == "MEMBER_FOLLOW_ROUNDTABLE")
            {
                this.Title.Text = _item.Target.Name;
            }
            else if (_item.Verb == "MEMBER_VOTEUP_ARTICLE")
            {
                this.Title.Text = _item.Target.Title;
            }
            else if (_item.Verb == "TOPIC_FOLLOW")
            {
                this.Title.Text = _item.Target.Name;
            }
            else
            {
                this.Title.Text = _item.Target.Title;
            }

            this.Title.Opacity = 1;
        }

        internal void ShowSummary()
        {
            // roundtable question activity
            if (_item.Verb == null)
            {
                this.Summary.Text = String.IsNullOrEmpty(_item.Title) ? _item.Question.Title : _item.Title;

                this.Summary.Opacity = 1;
                return;
            }

            var cannotShowSummary = _item.Verb.Contains("QUESTION_") || _item.Verb.Contains("MEMBER_FOLLOW") ||
                                    _item.Verb.Contains("TOPIC_FOLLOW");

            this.Summary.Text = cannotShowSummary
                ? String.Empty
                : String.IsNullOrEmpty(_item.Target.Excerpt) ? String.Empty : _item.Target.Excerpt;

            this.VoteCount.Text = cannotShowSummary
                ? String.Empty
                : _item.Target.VoteupCount.HasValue
                    ? Utility.Instance.GetEasyInt32(_item.Target.VoteupCount.Value)
                    : "0";

            this.VoteBorder.Opacity = this.VoteCount.Opacity = this.Summary.Opacity = cannotShowSummary ? 0 : 1;

            this.SummaryContainer.Visibility =
                this.VoteBorder.Visibility =
                    this.VoteCount.Visibility =
                        this.Summary.Visibility = cannotShowSummary ? Visibility.Collapsed : Visibility.Visible;
        }

        internal void ShowAuthor()
        {
            if (_item.Author != null)
            {
                this.Author.Text = _item.Author.Name;
            }
            if (_item.Actor != null)
            {
                this.Author.Text = _item.Actor.Name;
            }
            this.Author.Opacity = 1;
        }

        internal void ShowAvatar()
        {
            if (_item.Actor == null)
            {
                this.Avatar.Opacity = 0;
                return;
            }

            this.Avatar.Fill = new ImageBrush()
            {
                ImageSource = new BitmapImage(new Uri(AvartarHelper.GetLarge(_item.Actor.AvatarUrl))),
                Stretch = Stretch.Fill
            };
            this.Avatar.Opacity = 1;
        }

        internal void ShowVerb()
        {
            switch (_item.Verb)
            {
                case "ANSWER_CREATE":
                    this.Verb.Text = "回答了问题";
                    break;

                case "ANSWER_VOTE_UP":
                    this.Verb.Text = "赞同了回答";
                    break;

                case "QUESTION_FOLLOW":
                    this.Verb.Text = "关注了问题";
                    break;

                case "QUESTION_CREATE":
                    this.Verb.Text = "提了一个问题";
                    break;

                case "ARTICLE_VOTE_UP":
                    this.Verb.Text = "赞同文章";
                    break;

                case "ARTICLE_CREATE":
                    this.Verb.Text = String.Format("在 {0} 发表了一篇文章", _item.Target.Type);
                    break;
                case "MEMBER_CREATE_ARTICLE":
                    this.Verb.Text = String.Format("发表了一篇文章");
                    break;
                case "MEMBER_FOLLOW_COLLECTION":
                    this.Verb.Text = "关注了收藏夹";
                    break;

                case "MEMBER_FOLLOW_COLUMN":
                    this.Verb.Text = "关注了专栏";
                    break;

                case "MEMBER_VOTEUP_ARTICLE":
                    this.Verb.Text = "赞同文章";
                    break;

                case "TOPIC_FOLLOW":
                    this.Verb.Text = "关注了话题";
                    break;

                case "MEMBER_FOLLOW_ROUNDTABLE":
                    this.Verb.Text = "关注了圆桌";
                    break;
            }
            this.Verb.Opacity = 1;
        }

        internal void Clear()
        {
            _item = null;

            this.Author.ClearValue(TextBlock.TextProperty);
            this.Verb.ClearValue(TextBlock.TextProperty);
            this.Avatar.ClearValue(Ellipse.FillProperty);

            this.Title.ClearValue(TextBlock.TextProperty);

            this.VoteCount.ClearValue(TextBlock.TextProperty);
            this.Summary.ClearValue(TextBlock.TextProperty);


            _authorTapped = null;
            _titleTapped = null;
            _summaryTapped = null;
        }
    }
}
