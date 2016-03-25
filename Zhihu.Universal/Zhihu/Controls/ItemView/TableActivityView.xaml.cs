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
    public sealed partial class TableActivityView : UserControl
    {
        private TableActivity _item;

        private RelayCommand<TableActivity> _titleTapped;
        private RelayCommand<TableActivity> _answerTapped;

        public TableActivityView()
        {
            this.InitializeComponent();

            this.Title.Tapped += Title_Tapped;
            this.AnswerContainer.Tapped += Answer_Tapped;
        }

        private void Answer_Tapped(object sender, TappedRoutedEventArgs e)
        {
            if (_answerTapped != null)
                _answerTapped.Execute(_item);
        }

        private void Title_Tapped(object sender, TappedRoutedEventArgs e)
        {
            if (_titleTapped != null)
                _titleTapped.Execute(_item);
        }

        internal void ShowPlaceholder(TableActivity activity)
        {
            _item = activity;

            this.Title.Opacity = 0;

            this.AnswerContainer.Visibility = Visibility.Collapsed;
            this.SummaryContainer.Visibility = Visibility.Collapsed;
        }

        internal void RegistEventHandler(RelayCommand<TableActivity> titleEventHandler, RelayCommand<TableActivity> answerEventHandler)
        {
            _titleTapped = titleEventHandler;

            _answerTapped = answerEventHandler;
        }


        internal void ShowTitle()
        {
            this.Title.Text = String.IsNullOrEmpty(_item.Title) ? _item.Question.Title : _item.Title;

            this.Title.Opacity = 1;
        }


        internal void ShowSummary()
        {
            if (String.IsNullOrEmpty(this._item.Excerpt))
            {
                this.AnswersCount.Text = Utility.Instance.GetEasyInt32(this._item.AnswerCount.Value);
                this.CommentsCount.Text = Utility.Instance.GetEasyInt32(this._item.CommentCount);

                this.SummaryContainer.Visibility = Visibility.Visible;
            }
        }

        internal void ShowAnswer()
        {
            if (false == String.IsNullOrEmpty(this._item.Excerpt))
            {
                this.Avatar.Fill = new ImageBrush()
                {
                    ImageSource = new BitmapImage(new Uri(AvartarHelper.GetLarge(this._item.Author.AvatarUrl))),
                    Stretch = Stretch.Fill
                };

                this.VoteCount.Text = Utility.Instance.GetEasyInt32(this._item.VoteupCount);

                this.Excerpt.Text = this._item.Excerpt;

                this.AnswerContainer.Visibility = Visibility.Visible;
            }
        }

        internal void Clear()
        {
            _item = null;

            this.Title.ClearValue(TextBlock.TextProperty);

            this.Avatar.ClearValue(Ellipse.FillProperty);

            this.VoteCount.ClearValue(TextBlock.TextProperty);
            this.Excerpt.ClearValue(TextBlock.TextProperty);

            this.AnswersCount.ClearValue(TextBlock.TextProperty);
            this.CommentsCount.ClearValue(TextBlock.TextProperty);

            _titleTapped = null;
            _answerTapped = null;
        }
    }
}