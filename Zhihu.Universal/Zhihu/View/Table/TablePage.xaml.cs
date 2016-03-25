using Windows.Foundation;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Animation;
using Windows.UI.Xaml.Navigation;

using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Ioc;

using Zhihu.Common.Model;
using Zhihu.Controls.ItemView;
using Zhihu.Helper;
using Zhihu.ViewModel;
using System;

namespace Zhihu.View.Table
{
    public sealed partial class TablePage : Page
    {
        private readonly RelayCommand<TableActivity> _titleTapped;
        private readonly RelayCommand<TableActivity> _answerTapped;

        private readonly RelayCommand<Comment> _commentAuthorTapped;

        private readonly RelayCommand<TableQuestion> _questionTapped;


        public TablePage()
        {
            this.InitializeComponent();
            
            _titleTapped = new RelayCommand<TableActivity>(TitleTappedMethod);
            _answerTapped = new RelayCommand<TableActivity>(AnswerTappedMethod);

            _commentAuthorTapped = new RelayCommand<Comment>(CommentAuthorTappedMethod);
            _questionTapped = new RelayCommand<TableQuestion>(QuestionActivityTappedMethod);

            this.PreviewFrame.Navigate(typeof(WaterMarkPage));
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            this.Loaded += Page_Loaded;

            SystemNavigationManager.GetForCurrentView().BackRequested += QuestionPage_BackRequested;
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            base.OnNavigatedFrom(e);

            this.Loaded -= Page_Loaded;

            SystemNavigationManager.GetForCurrentView().BackRequested -= QuestionPage_BackRequested;
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            this.Pivot.Height = this.ActualHeight;

            Theme.Instance.UpdateRequestedTheme(this);
            UpdateBackButton();
        }

        private void QuestionPage_BackRequested(object sender, BackRequestedEventArgs e)
        {
            e.Handled = true;
            OnBackRequested();
        }

        private void UpdateBackButton()
        {
            SystemNavigationManager.GetForCurrentView().AppViewBackButtonVisibility = AppViewBackButtonVisibility.Visible;
        }
        
        private void OnBackRequested()
        {
            if (this.PreviewFrame.BackStack.Count > 0) return;

            this.Frame.GoBack(new DrillInNavigationTransitionInfo());

            UpdateBackButton();
        }

        private void VisualStateGroup_CurrentStateChanged(object sender, VisualStateChangedEventArgs e)
        {
        }

        private Frame GetNavFrame()
        {
            Frame navFrame = null;

            if (AdaptiveStates.CurrentState == DefaultState)
            {
                navFrame = this.PreviewFrame;
            }
            else
            {
                navFrame = this.Frame;
            }
            return navFrame;
        }

        private void TitleTappedMethod(TableActivity activity)
        {
            if (activity == null) return;

            NavHelper.NavToQuestionPage(activity.Question.Id, this.Frame);
            SystemNavigationManager.GetForCurrentView().BackRequested -= QuestionPage_BackRequested;
        }

        private void AnswerTappedMethod(TableActivity activity)
        {
            if (activity == null) return;

            Int32 answerId;

            if (false == Int32.TryParse(activity.Url.Substring(activity.Url.IndexOf("answers/") + "answers/".Length), out answerId)) return;

            if (AdaptiveStates.CurrentState != DefaultState)
            {
                SystemNavigationManager.GetForCurrentView().BackRequested -= QuestionPage_BackRequested;
            }
            else
            {
                if (this.PreviewFrame.CanGoBack) this.PreviewFrame.GoBack();
            }

            NavHelper.NavToAnswerPage(answerId, GetNavFrame());
        }

        private void CommentAuthorTappedMethod(Comment comment)
        {
            NavHelper.NavToProfilePage(comment.Author.Id, this.Frame);
            SystemNavigationManager.GetForCurrentView().BackRequested -= QuestionPage_BackRequested;
        }

        private void QuestionActivityTappedMethod(TableQuestion question)
        {
            NavHelper.NavToQuestionPage(question.Id, this.Frame);
            SystemNavigationManager.GetForCurrentView().BackRequested -= QuestionPage_BackRequested;
        }

        #region Activity Content Changing

        private TypedEventHandler<ListViewBase, ContainerContentChangingEventArgs> ActivityChanging
        {
            get
            {
                return _activityChanging ??
                       (_activityChanging =
                           new TypedEventHandler<ListViewBase, ContainerContentChangingEventArgs>(
                               Activity_OnContainerContentChanging));
            }
        }

        private TypedEventHandler<ListViewBase, ContainerContentChangingEventArgs> _activityChanging;

        private void Activity_OnContainerContentChanging(ListViewBase sender, ContainerContentChangingEventArgs args)
        {
            var view = args.ItemContainer.ContentTemplateRoot as TableActivityView;

            if (view == null) return;

            if (args.InRecycleQueue == true)
            {
                view.Clear();
            }
            else if (args.Phase == 0)
            {
                view.ShowPlaceholder(args.Item as TableActivity);

                args.RegisterUpdateCallback(ActivityChanging);
            }
            else if (args.Phase == 1)
            {
                view.ShowTitle();

                args.RegisterUpdateCallback(ActivityChanging);
            }
            else if (args.Phase == 2)
            {
                view.ShowSummary();

                args.RegisterUpdateCallback(ActivityChanging);
            }
            else if (args.Phase == 3)
            {
                view.ShowAnswer();

                args.RegisterUpdateCallback(ActivityChanging);
            }
            else if (args.Phase == 4)
            {
                view.RegistEventHandler(this._titleTapped, this._answerTapped);
            }
            args.Handled = true;
        }

        #endregion

        #region Comment Content Changing

        private TypedEventHandler<ListViewBase, ContainerContentChangingEventArgs> CommentChanging
        {
            get
            {
                return _commentChanging ??
                       (_commentChanging =
                           new TypedEventHandler<ListViewBase, ContainerContentChangingEventArgs>(
                               Comment_OnContainerContentChanging));
            }
        }

        private TypedEventHandler<ListViewBase, ContainerContentChangingEventArgs> _commentChanging;

        private void Comment_OnContainerContentChanging(ListViewBase sender, ContainerContentChangingEventArgs args)
        {
            var view = args.ItemContainer.ContentTemplateRoot as CommentView;

            if (view == null) return;

            if (args.InRecycleQueue == true)
            {
                view.Clear();
            }
            else if (args.Phase == 0)
            {
                view.ShowPlaceHolder(args.Item as Comment);

                args.RegisterUpdateCallback(CommentChanging);
            }
            else if (args.Phase == 1)
            {
                view.ShowAuthor();

                args.RegisterUpdateCallback(CommentChanging);
            }
            else if (args.Phase == 2)
            {
                view.ShowSummary();

                args.RegisterUpdateCallback(CommentChanging);
            }
            else if (args.Phase == 3)
            {
                view.ShowUpdatedTime();
                view.ShowVoteup();

                args.RegisterUpdateCallback(CommentChanging);
            }
            else if (args.Phase == 4)
            {
                view.ShowAvatar();

                view.RegistEventHandler(this._commentAuthorTapped);
            }
            args.Handled = true;
        }

        #endregion

        #region Question Changing

        private TypedEventHandler<ListViewBase, ContainerContentChangingEventArgs> QuestionChanging
        {
            get
            {
                return _questionChanging ??
                       (_questionChanging =
                           new TypedEventHandler<ListViewBase, ContainerContentChangingEventArgs>(
                               QuestionAcitivity_OnContainerContentChanging));
            }
        }

        private TypedEventHandler<ListViewBase, ContainerContentChangingEventArgs> _questionChanging;

        private void QuestionAcitivity_OnContainerContentChanging(ListViewBase sender,
            ContainerContentChangingEventArgs args)
        {
            var view = args.ItemContainer.ContentTemplateRoot as TableQuestionView;

            if (view == null) return;

            if (args.InRecycleQueue == true)
            {
                view.Clear();
            }
            else if (args.Phase == 0)
            {
                view.ShowPlaceHolder(args.Item as TableQuestion);

                args.RegisterUpdateCallback(QuestionChanging);
            }
            else if (args.Phase == 1)
            {
                view.ShowTitle();

                args.RegisterUpdateCallback(QuestionChanging);
            }
            else if (args.Phase == 2)
            {
                view.ShowSummary();
                
                args.RegisterUpdateCallback(QuestionChanging);
            }
            else if (args.Phase == 3)
            {
                view.RegistEventHandler(this._questionTapped);
            }
            args.Handled = true;
        }

        #endregion
    }
}
