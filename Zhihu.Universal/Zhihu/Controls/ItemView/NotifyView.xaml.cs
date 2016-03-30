using System;
using System.Linq;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;

using GalaSoft.MvvmLight.Command;

using Zhihu.Common.Helper;
using Zhihu.Common.Model;


namespace Zhihu.Controls.ItemView
{
    public sealed partial class NotifyView : UserControl
    {
        public NotifyItem Notify { get; private set; }

        private RelayCommand<NotifyItem> _authorTapped;
        private RelayCommand<NotifyItem> _titleTapped;
        private RelayCommand<NotifyItem> _summaryTapped;

        public NotifyView()
        {
            this.InitializeComponent();

            this.Author.Tapped += Author_Tapped;
            this.Title.Tapped += Title_Tapped;
            this.Summary.Tapped += Summary_Tapped;
        }

        internal void ShowPlaceholder(NotifyItem notify)
        {
            Notify = notify;
            
            this.Author.Opacity = 0;

            this.Verb.Opacity = 0;

            this.Title.Opacity = 0;

            this.SummaryContainer.Visibility = Visibility.Collapsed;
            //this.BackgroundBorder.Visibility = _notify.IsRead == true ? Visibility.Collapsed : Visibility.Visible;

            this.DataContext = this;
        }


        internal void RegistEventHandler(RelayCommand<NotifyItem> authorEventHandler,
            RelayCommand<NotifyItem> titleEventHandler,
            RelayCommand<NotifyItem> summaryEventHandler)
        {
            _authorTapped = authorEventHandler;

            _titleTapped = titleEventHandler;

            _summaryTapped = summaryEventHandler;
        }

        internal void ShowAuthor()
        {
            this.Author.Text = Notify.Operators.Length == 1
                ? Notify.Operators[0].Name
                : String.Format("{0} 等", Notify.Operators[0].Name);

            this.Author.Opacity = 1;

            if (Notify.Operators.Length > 0)
            {
                this.Author.Visibility = Notify.Operators[0].Id != "-1" ? Visibility.Visible : Visibility.Collapsed;
            }
        }

        internal void ShowVerb()
        {
            var verbModel = VerbModels.ForNotify.FirstOrDefault(item => item.Verbs.Contains(Notify.ActionName));

            if (verbModel != null)
            {
                this.Verb.Opacity = 1;
                this.Verb.Text = verbModel.Display;
            }
            else
            {

            }
        }

        internal void ShowTitle()
        {
            if (this.Notify.Target.Type == "article")
            {
                this.Title.Text = Notify.Target.Title;
                this.Title.Opacity = 1;
            }
            else if (this.Notify.Target.Type == "answer")
            {
                this.Title.Text = Notify.Target.Question.Title;
                this.Title.Opacity = 1;
            }
            else if (this.Notify.Target.Type == "question")
            {
                this.Title.Text = Notify.Target.Title;
                this.Title.Opacity = 1;
            }
            else if (this.Notify.Target.Type == "column")
            {
                this.Title.Text = Notify.Target.Title;
                this.Title.Opacity = 1;
            }
        }

        internal void ShowSummary()
        {
            if (this.Notify.Target.Type == "article")
            {
                this.SummaryContainer.Visibility = Visibility.Collapsed;
            }
            if (this.Notify.Target.Type == "answer")
            {
                this.VoteCount.Text = Notify.Target.VoteupCount.HasValue
                    ? Utility.Instance.GetEasyInt32(Notify.Target.VoteupCount.Value)
                    : String.Empty;
                this.VoteCount.Opacity = 1;

                this.Summary.Text = Notify.Target.Excerpt;
                this.Summary.Opacity = 1;

                this.SummaryContainer.Visibility = Visibility.Visible;
            }
            if (this.Notify.Target.Type == "question" && Notify.Answer != null)
            {
                this.VoteCount.Text = Utility.Instance.GetEasyInt32(Notify.Answer.VoteupCount);
                this.VoteCount.Opacity = 1;

                this.Summary.Text = Notify.Answer.Excerpt;
                this.Summary.Opacity = 1;

                this.SummaryContainer.Visibility = Visibility.Visible;
            }
        }

        internal void Clear()
        {
            this.Notify = null;

            this.Author.ClearValue(TextBlock.TextProperty);
            this.Verb.ClearValue(TextBlock.TextProperty);

            this.Title.ClearValue(TextBlock.TextProperty);

            this.VoteCount.ClearValue(TextBlock.TextProperty);
            this.Summary.ClearValue(TextBlock.TextProperty);

            this._authorTapped = null;
            this._titleTapped = null;
            this._summaryTapped = null;
        }

        private void Summary_Tapped(object sender, TappedRoutedEventArgs e)
        {
            _summaryTapped?.Execute(Notify);
            
            //this.BackgroundBorder.Visibility = _notify.IsRead == true ? Visibility.Collapsed : Visibility.Visible;
        }

        private void Title_Tapped(object sender, TappedRoutedEventArgs e)
        {
            _titleTapped?.Execute(Notify);
            
            //this.BackgroundBorder.Visibility = _notify.IsRead == true ? Visibility.Collapsed : Visibility.Visible;
        }

        private void Author_Tapped(object sender, TappedRoutedEventArgs e)
        {
            _authorTapped?.Execute(Notify);
        }
    }
}
