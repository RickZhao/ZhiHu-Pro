using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;

using GalaSoft.MvvmLight.Command;

using Zhihu.Common.Model;
using Zhihu.Common.Helper;

namespace Zhihu.Controls.ItemView
{
    public sealed partial class QuestionActivityView : UserControl
    {
        private Question _item;

        private RelayCommand<Question> _tapped;

        public QuestionActivityView()
        {
            this.InitializeComponent();

            this.Tapped += QuestionActivityView_Tapped;
        }

        void QuestionActivityView_Tapped(object sender, TappedRoutedEventArgs e)
        {
            if (_tapped != null)
                _tapped.Execute(_item);
        }

        internal void ShowPlaceHolder(Question question)
        {
            _item = question;

            this.Title.Opacity = 0;
            this.Followers.Opacity = 0;
            this.Answers.Opacity = 0;
            this.OverView.Opacity = 0;
        }

        internal void RegistEventHandler(RelayCommand<Question> questionTapped)
        {
            _tapped = questionTapped;
        }

        internal void ShowTitle()
        {
            Title.Text = _item.Title;

            Title.Opacity = 1;
        }

        internal void ShowFollowers()
        {
            Followers.Text = Utility.Instance.GetEasyInt32(_item.FollowerCount);

            Followers.Opacity = 1;
        }
        internal void ShowAnswers()
        {
            Answers.Text = Utility.Instance.GetEasyInt32(_item.AnswerCount);

            Answers.Opacity = 1;
        }

        internal void ShowOverview()
        {
            OverView.Opacity = 1;
        }

        internal void Clear()
        {
            _item = null;

            Title.ClearValue(TextBlock.TextProperty);

            Answers.ClearValue(TextBlock.TextProperty);
            Followers.ClearValue(TextBlock.TextProperty);

            _tapped = null;
        }
    }
}
