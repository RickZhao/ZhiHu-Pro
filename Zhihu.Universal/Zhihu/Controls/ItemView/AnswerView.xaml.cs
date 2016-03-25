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
    public sealed partial class AnswerView : UserControl
    {
        private readonly CountToStringConverter _countConverter = new CountToStringConverter();

        private Answer _item;

        private RelayCommand<Answer> _answerTapped;
        private RelayCommand<Answer> _authorTapped;

        public AnswerView()
        {
            this.InitializeComponent();

            this.Avatar.Tapped += Author_Tapped;
            this.Author.Tapped += Author_Tapped;
            this.VoteupContainer.Tapped += Author_Tapped;

            this.Summary.Tapped += Answer_Tapped;
        }

        private void Author_Tapped(object sender, TappedRoutedEventArgs tappedRoutedEventArgs)
        {
            if (_authorTapped != null)
                _authorTapped.Execute(_item);
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
            this.Author.Opacity = 0;
            this.VoteupCount.Opacity = 0;
            this.Summary.Opacity = 0;
        }

        internal void RegistEventHandler(RelayCommand<Answer> authorTapped, RelayCommand<Answer> answerTapped)
        {
            this._answerTapped = answerTapped;
            this._authorTapped = authorTapped;
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

        internal void ShowAuthor()
        {
            this.Author.Text = _item.Author.Name;
            this.Author.Opacity = 1;
        }

        internal void ShowUpdatedTime()
        {
            //this.UpdatedTime.Text = _timeConverter.Convert(_item.UpdatedTime, null, null, null).ToString();
            //this.UpdatedTime.Opacity = 1;
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

            this.Avatar.ClearValue(Ellipse.FillProperty);
            this.Author.ClearValue(TextBlock.TextProperty);
            //this.UpdatedTime.ClearValue(TextBlock.TextProperty);
            this.VoteupCount.ClearValue(TextBlock.TextProperty);
            this.Summary.ClearValue(TextBlock.TextProperty);

            this._answerTapped = null;
            this._authorTapped = null;
        }
    }
}
