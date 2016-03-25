using System;

using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Shapes;

using GalaSoft.MvvmLight.Command;

using Zhihu.Common.Helper;
using Zhihu.Common.Model;
using Zhihu.Converter;


namespace Zhihu.Controls.ItemView
{
    public sealed partial class CollectedAnswerView : UserControl
    {
        private readonly RelativeTimeConverter _timeConverter = new RelativeTimeConverter();
        private readonly CountToStringConverter _countConverter = new CountToStringConverter();

        private Answer _item;

        private RelayCommand<Question> _titleTapped;
        private RelayCommand<Answer> _answerTapped;
        private RelayCommand<Author> _authorTapped;

        public CollectedAnswerView()
        {
            this.InitializeComponent();

            this.Title.Tapped += Title_Tapped;
            this.Avatar.Tapped += Author_Tapped;
            this.VoteupContainer.Tapped += Author_Tapped;

            this.Summary.Tapped += Answer_Tapped;
        }

        void Title_Tapped(object sender, TappedRoutedEventArgs e)
        {
            if (_titleTapped != null)
            {
                _titleTapped.Execute(_item.Question);
            }
        }

        private void Author_Tapped(object sender, TappedRoutedEventArgs tappedRoutedEventArgs)
        {
            if (_authorTapped != null)
                _authorTapped.Execute(_item.Author);
        }

        private void Answer_Tapped(object sender, TappedRoutedEventArgs tappedRoutedEventArgs)
        {
            if (_answerTapped != null)
                _answerTapped.Execute(_item);
        }

        internal void ShowPlaceHolder(Answer item)
        {
            this._item = item;

            this.Avatar.Opacity = 0;
            this.VoteupCount.Opacity = 0;
            this.Summary.Opacity = 0;
        }

        internal void RegistEventHandler(RelayCommand<Question> titleTapped, RelayCommand<Author> authorTapped, RelayCommand<Answer> answerTapped)
        {
            this._titleTapped = titleTapped;
            this._answerTapped = answerTapped;
            this._authorTapped = authorTapped;
        }

        internal void ShowTitle()
        {
            this.Title.Text = _item.Question.Title;
            this.Title.Opacity = 1;
        }

        internal void ShowAvatar()
        {
            this.Avatar.Fill = new ImageBrush()
            {
                ImageSource = new BitmapImage(new Uri(AvartarHelper.GetLarge(_item.Author.AvatarUrl))),
                Stretch = Stretch.Fill,
            };
            this.Avatar.Opacity = 1;
        }

        internal void ShowVoteup()
        {
            this.VoteupCount.Text = _countConverter.Convert(_item.VoteupCount, null, null, null).ToString();
            this.VoteupCount.Opacity = 1;
        }

        internal void ShowSummary()
        {
            this.Summary.Text = _item.Excerpt;
            this.Summary.Opacity = 1;
        }

        internal void Clear()
        {
            _item = null;

            this.Title.ClearValue(TextBlock.TextProperty);
            this.Avatar.ClearValue(Ellipse.FillProperty);
            this.VoteupCount.ClearValue(TextBlock.TextProperty);
            this.Summary.ClearValue(TextBlock.TextProperty);

            this._answerTapped = null;
            this._authorTapped = null;
        }
    }
}
