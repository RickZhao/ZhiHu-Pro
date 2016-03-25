using System;

using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Shapes;

using GalaSoft.MvvmLight.Command;

using Zhihu.Common.Helper;
using Zhihu.Common.Model;


namespace Zhihu.Controls.ItemView
{
    public sealed partial class AnswerActivityView : UserControl
    {
        private Answer _item;

        private RelayCommand<Answer> _titleTapped;
        private RelayCommand<Answer> _authorTapped;
        private RelayCommand<Answer> _summaryTapped;

        public AnswerActivityView()
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

        internal void ShowPlaceholder(Answer activity)
        {
            _item = activity;

            Author.Opacity = 0;
            Verb.Opacity = 0;
            Avatar.Opacity = 0;

            Title.Opacity = 0;

            VoteCount.Opacity = 0;
            Summary.Opacity = 0;
        }

        internal void RegistEventHandler(RelayCommand<Answer> authorEventHandler,
            RelayCommand<Answer> titleEventHandler,
            RelayCommand<Answer> summaryEventHandler)
        {
            _authorTapped = authorEventHandler;

            _titleTapped = titleEventHandler;

            _summaryTapped = summaryEventHandler;
        }


        internal void ShowTitle()
        {
            Title.Text = _item.Question.Title;

            Title.Opacity = 1;
        }

        internal void ShowSummary()
        {
            Summary.Text = _item.Excerpt;

            Summary.Opacity = 1;
        }

        internal void ShowVoteUp()
        {
            VoteCount.Text = _item.VoteupCount > 1000
                ? (_item.VoteupCount / 1000.0).ToString("F1") + "k"
                : _item.VoteupCount.ToString();

            VoteCount.Opacity = 1;
        }

        internal void ShowAuthor()
        {
            Author.Text = _item.Author.Name;

            Author.Opacity = 1;
        }

        internal void ShowAvatar()
        {
            Avatar.Fill = new ImageBrush()
            {
                ImageSource = new BitmapImage(new Uri(AvartarHelper.GetLarge(_item.Author.AvatarUrl))),
                Stretch = Stretch.Fill
            };
            Avatar.Opacity = 1;
        }

        internal void ShowVerb()
        {
            Verb.Text = "回答了该问题";
            Verb.Opacity = 1;
        }

        internal void Clear()
        {
            _item = null;

            Author.ClearValue(TextBlock.TextProperty);
            Verb.ClearValue(TextBlock.TextProperty);
            Avatar.ClearValue(Ellipse.FillProperty);

            Title.ClearValue(TextBlock.TextProperty);

            VoteCount.ClearValue(TextBlock.TextProperty);
            Summary.ClearValue(TextBlock.TextProperty);


            _authorTapped = null;
            _titleTapped = null;
            _summaryTapped = null;
        }
    }
}
