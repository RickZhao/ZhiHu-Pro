using System;

using Windows.Foundation;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Animation;
using Windows.UI.Xaml.Navigation;

using GalaSoft.MvvmLight.Command;

using Zhihu.Common.Helper;
using Zhihu.Controls.ItemView;
using Zhihu.Helper;


namespace Zhihu.View.Topic
{
    public sealed partial class AnswersPage : Page
    {
        private Common.Model.Answer _current;
        private RelayCommand<Common.Model.Answer> BestAnswerAuthorTapped;
        private RelayCommand<Common.Model.Answer> BestAnswerQuestionTapped;
        private RelayCommand<Common.Model.Answer> BestAnswerSummaryTapped;

        public AnswersPage()
        {
            this.InitializeComponent();

            BestAnswerAuthorTapped = new RelayCommand<Common.Model.Answer>(BestAnswerAuthorTappedMethod);
            BestAnswerQuestionTapped = new RelayCommand<Common.Model.Answer>(BestAnswerQuestionTappedMethod);
            BestAnswerSummaryTapped = new RelayCommand<Common.Model.Answer>(BestAnswerSummaryTappedMethod);

            this.PreviewFrame.Navigate(typeof(WaterMarkPage));
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            this.Loaded += AnswersPage_Loaded;
            SystemNavigationManager.GetForCurrentView().BackRequested += AnswersPage_BackRequested;
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            base.OnNavigatedFrom(e);

            this.Loaded -= AnswersPage_Loaded;
            SystemNavigationManager.GetForCurrentView().BackRequested -= AnswersPage_BackRequested;
        }

        private void AnswersPage_BackRequested(object sender, BackRequestedEventArgs e)
        {
            e.Handled = true;
            OnBackRequested();
        }

        private void AnswersPage_Loaded(object sender, RoutedEventArgs e)
        {
            Theme.Instance.UpdateRequestedTheme(this);
            UpdateBackButton();
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
            UpdateForVisualState(e.NewState, e.OldState);
        }

        private void UpdateForVisualState(VisualState newState, VisualState oldState)
        {
            BestAnswerSummaryTappedMethod(this._current);
        }

        #region Best Answers

        private TypedEventHandler<ListViewBase, ContainerContentChangingEventArgs> ByCreatedChanging
        {
            get
            {
                return _byCreatedChanging ??
                       (_byCreatedChanging =
                           new TypedEventHandler<ListViewBase, ContainerContentChangingEventArgs>(
                               AnswersByCreated_OnContainerContentChanging));
            }
        }

        private TypedEventHandler<ListViewBase, ContainerContentChangingEventArgs> _byCreatedChanging;


        private void AnswersByCreated_OnContainerContentChanging(ListViewBase sender,
            ContainerContentChangingEventArgs args)
        {
            var view = args.ItemContainer.ContentTemplateRoot as AnswerActivityView;

            if (view == null) return;

            if (args.InRecycleQueue == true)
            {
                view.Clear();
            }
            else if (args.Phase == 0)
            {
                view.ShowPlaceholder(args.Item as Common.Model.Answer);

                args.RegisterUpdateCallback(ByCreatedChanging);
            }
            else if (args.Phase == 1)
            {
                view.ShowTitle();

                args.RegisterUpdateCallback(ByCreatedChanging);
            }

            else if (args.Phase == 2)
            {
                view.ShowSummary();

                args.RegisterUpdateCallback(ByCreatedChanging);
            }
            else if (args.Phase == 3)
            {
                view.ShowVerb();

                args.RegisterUpdateCallback(ByCreatedChanging);
            }
            else if (args.Phase == 4)
            {
                view.ShowAuthor();

                args.RegisterUpdateCallback(ByCreatedChanging);
            }
            else if (args.Phase == 5)
            {
                view.ShowVoteUp();

                args.RegisterUpdateCallback(ByCreatedChanging);
            }
            else if (args.Phase == 6)
            {
                view.ShowAvatar();

                view.RegistEventHandler(BestAnswerAuthorTapped, BestAnswerQuestionTapped, BestAnswerSummaryTapped);
            }
            args.Handled = true;
        }

        #endregion

        #region Best Answers Tapped Methods

        private void BestAnswerSummaryTappedMethod(Common.Model.Answer answer)
        {
            if (answer == null) return;

            this._current = answer;

            Frame navFrame = null;

            if (AdaptiveStates.CurrentState == DefaultState)
            {
                navFrame = this.PreviewFrame;
            }
            else
            {
                navFrame = this.Frame;
            }

            if (AdaptiveStates.CurrentState != DefaultState)
            {
                SystemNavigationManager.GetForCurrentView().BackRequested -= AnswersPage_BackRequested;
            }
            else
            {
                if (this.PreviewFrame.CanGoBack) this.PreviewFrame.GoBack();
            }

            NavHelper.NavToAnswerPage(answer.Id, navFrame);
        }

        private void BestAnswerQuestionTappedMethod(Common.Model.Answer answer)
        {
            _current = answer;
            NavHelper.NavToQuestionPage(answer.Question.Id, this.Frame);
            SystemNavigationManager.GetForCurrentView().BackRequested -= AnswersPage_BackRequested;
        }

        private void BestAnswerAuthorTappedMethod(Common.Model.Answer answer)
        {
            _current = answer;
            NavHelper.NavToProfilePage(answer.Author.Id, this.Frame);
            SystemNavigationManager.GetForCurrentView().BackRequested -= AnswersPage_BackRequested;
        }

        #endregion
    }
}
