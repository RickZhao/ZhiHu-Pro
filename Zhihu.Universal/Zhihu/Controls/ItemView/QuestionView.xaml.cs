using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;

using GalaSoft.MvvmLight.Command;

using Zhihu.Common.Model;


namespace Zhihu.Controls.ItemView
{
    public sealed partial class QuestionView : UserControl
    {
        private Question _item;
        private RelayCommand<Question> _itemTapped;

        public QuestionView()
        {
            this.InitializeComponent();
            this.Tapped += Item_OnTapped;
        }

        private void Item_OnTapped(object sender, TappedRoutedEventArgs tappedRoutedEventArgs)
        {
            _itemTapped?.Execute(_item);
        }

        internal void ShowPlaceHolder(Question item)
        {
            _item = item;

            Title.Opacity = 0;
            Followers.Opacity = 0;
            this.Answers.Opacity = 0;
        }

        internal void RegistEventHandler(RelayCommand<Question> itemTapped)
        {
            this._itemTapped = itemTapped;
        }

        internal void ShowTitle()
        {
            this.Title.Text = _item.Title;
            this.Title.Opacity = 1;
        }

        internal void ShowFollowers()
        {
            Followers.Text = _item.FollowerCount.ToString();
            Followers.Opacity = 1;
        }

        internal void ShowAnswers()
        {
            Answers.Text = _item.AnswerCount.ToString();
            Answers.Opacity = 1;
        }

        public void Clear()
        {
            _item = null;
            _itemTapped = null;

            this.Title.ClearValue(TextBlock.TextProperty);
            this.Followers.ClearValue(TextBlock.TextProperty);
            this.Answers.ClearValue(TextBlock.TextProperty);
        }
    }
}
