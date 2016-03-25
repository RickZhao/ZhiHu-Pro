using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;

using GalaSoft.MvvmLight.Command;

using Zhihu.Common.Model;
using Zhihu.Common.Helper;

namespace Zhihu.Controls.ItemView
{
    public sealed partial class TopicActivityView : UserControl
    {
        private TopicActivity _item;

        private RelayCommand<TopicActivity> _questionTapped;

        private RelayCommand<Answer> _answerTapped;

        public TopicActivityView()
        {
            this.InitializeComponent();
            this.Title.Tapped += Question_Tapped;

            this.Answer1.Tapped += Answer1_Tapped;
            this.Answer2.Tapped += Answer2_Tapped;
            this.Answer3.Tapped += Answer3_Tapped;
        }

        private void Question_Tapped(object sender, TappedRoutedEventArgs e)
        {
            if (_questionTapped != null)
                _questionTapped.Execute(_item);
        }

        private void Answer1_Tapped(object sender, TappedRoutedEventArgs e)
        {
            if (_answerTapped != null)
                _answerTapped.Execute(_item.Answers[0]);
        }

        private void Answer2_Tapped(object sender, TappedRoutedEventArgs e)
        {
            if (_answerTapped != null)
                _answerTapped.Execute(_item.Answers[1]);
        }

        private void Answer3_Tapped(object sender, TappedRoutedEventArgs e)
        {
            if (_answerTapped != null)
                _answerTapped.Execute(_item.Answers[2]);
        }

        internal void ShowPlaceholder(TopicActivity activity)
        {
            _item = activity;

            Title.Opacity = 0;

            Answer1Summary.Opacity = Answer1VoteCount.Opacity = 0;
            Answer2Summary.Opacity = Answer2VoteCount.Opacity = 0;
            Answer3Summary.Opacity = Answer3VoteCount.Opacity = 0;

            Answer1.Visibility = Answer2.Visibility = Answer3.Visibility = Visibility.Collapsed;
        }

        internal void RegistEventHandler(RelayCommand<TopicActivity> questionTapped,
            RelayCommand<Answer> answerTapped)
        {
            _questionTapped = questionTapped;

            _answerTapped = answerTapped;
        }


        internal void ShowTitle()
        {
            Title.Text = _item.Title;

            Title.Opacity = 1;
        }

        internal void ShowAnswer1()
        {
            if (this._item.Answers == null || this._item.Answers.Length == 0)
            {
                return;
            }

            Answer1VoteCount.Text = Utility.Instance.GetEasyInt32(_item.Answers[0].VoteupCount);

            Answer1Summary.Text = _item.Answers[0].Excerpt;

            Answer1.Visibility = Visibility.Visible;

            Answer1VoteCount.Opacity = Answer1Summary.Opacity = 1;
        }

        internal void ShowAnswer2()
        {
            if (this._item.Answers == null || this._item.Answers.Length <= 1)
            {
                return;
            }

            Answer2VoteCount.Text = Utility.Instance.GetEasyInt32(_item.Answers[1].VoteupCount);

            Answer2Summary.Text = _item.Answers[1].Excerpt;

            Answer2.Visibility = Answer1Sperator.Visibility = Visibility.Visible;

            Answer2VoteCount.Opacity = Answer2Summary.Opacity = 1;
        }

        internal void ShowAnswer3()
        {
            if (this._item.Answers == null || this._item.Answers.Length <= 2)
            {
                return;
            }

            Answer3VoteCount.Text = Utility.Instance.GetEasyInt32(_item.Answers[2].VoteupCount);

            Answer3Summary.Text = _item.Answers[2].Excerpt;

            Answer3.Visibility = Answer2Sperator.Visibility = Visibility.Visible;

            Answer3VoteCount.Opacity = Answer3Summary.Opacity = 1;
        }

        internal void Clear()
        {
            _item = null;
            _questionTapped = null;
            _answerTapped = null;

            Title.ClearValue(TextBlock.TextProperty);

            Answer1VoteCount.ClearValue(TextBlock.TextProperty);
            Answer1Summary.ClearValue(TextBlock.TextProperty);

            Answer2VoteCount.ClearValue(TextBlock.TextProperty);
            Answer2Summary.ClearValue(TextBlock.TextProperty);

            Answer3VoteCount.ClearValue(TextBlock.TextProperty);
            Answer3Summary.ClearValue(TextBlock.TextProperty);

            Answer1Summary.Opacity = Answer1VoteCount.Opacity = 0;
            Answer2Summary.Opacity = Answer2VoteCount.Opacity = 0;
            Answer3Summary.Opacity = Answer3VoteCount.Opacity = 0;

            Answer1.Visibility = Answer2.Visibility = Answer3.Visibility = Visibility.Collapsed;
        }
    }
}
