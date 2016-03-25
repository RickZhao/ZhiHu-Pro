using System;

using Windows.Foundation;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Animation;
using Windows.UI.Xaml.Navigation;

using GalaSoft.MvvmLight.Command;

using Zhihu.Common.Model;
using Zhihu.Controls.ItemView;
using Zhihu.Helper;


namespace Zhihu.View.Profile
{
    public sealed partial class AnswersPage : Page
    {
        private String _profileId = String.Empty;
        private Common.Model.Answer _current;
        private readonly RelayCommand<Common.Model.Answer> _answerAuthorTapped;
        private readonly RelayCommand<Common.Model.Answer> _answerQuestionTapped;
        private readonly RelayCommand<Common.Model.Answer> _answerSummaryTapped;

        public AnswersPage()
        {
            this.InitializeComponent();

            _answerAuthorTapped = new RelayCommand<Common.Model.Answer>(AnswerAuthorTappedMethod);
            _answerQuestionTapped = new RelayCommand<Common.Model.Answer>(AnswerQuestionTappedMethod);
            _answerSummaryTapped = new RelayCommand<Common.Model.Answer>(AnswerSummaryTappedMethod);

            this.PreviewFrame.Navigate(typeof(WaterMarkPage));
        }

        #region Answer Tapped Methods

        private void AnswerAuthorTappedMethod(Common.Model.Answer answer)
        {
            _current = null;
            NavHelper.NavToProfilePage(answer.Author.Id, this.Frame);
            SystemNavigationManager.GetForCurrentView().BackRequested -= ProfileAnswersPage_BackRequested;
        }

        private void AnswerQuestionTappedMethod(Common.Model.Answer answer)
        {
            _current = null;
            NavHelper.NavToQuestionPage(answer.Question.Id, this.Frame);

            SystemNavigationManager.GetForCurrentView().BackRequested -= ProfileAnswersPage_BackRequested;
        }

        private void AnswerSummaryTappedMethod(Common.Model.Answer answer)
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
                SystemNavigationManager.GetForCurrentView().BackRequested -= ProfileAnswersPage_BackRequested;
            }
            else
            {
                if (this.PreviewFrame.CanGoBack) this.PreviewFrame.GoBack();
            }

            NavHelper.NavToAnswerPage(answer.Id, navFrame);
        }

        #endregion

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            this.Loaded -= ProfileAnswersPage_Loaded;
            this.Loaded += ProfileAnswersPage_Loaded;
            SystemNavigationManager.GetForCurrentView().BackRequested -= ProfileAnswersPage_BackRequested;
            SystemNavigationManager.GetForCurrentView().BackRequested += ProfileAnswersPage_BackRequested;
            
            if (e.NavigationMode == NavigationMode.New || e.NavigationMode == NavigationMode.Back)
            {
                _profileId = e.Parameter == null ? String.Empty : e.Parameter.ToString();
                var vm = ViewModelHelper.Instance.GetProfile(_profileId);
                this.DataContext = vm;
            }
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            this.Loaded -= ProfileAnswersPage_Loaded;

            SystemNavigationManager.GetForCurrentView().BackRequested -= ProfileAnswersPage_BackRequested;

            base.OnNavigatedFrom(e);
        }

        private void ProfileAnswersPage_Loaded(object sender, RoutedEventArgs e)
        {
            Theme.Instance.UpdateRequestedTheme(this);
            UpdateBackButton();
        }

        private void ProfileAnswersPage_BackRequested(object sender, BackRequestedEventArgs e)
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
            UpdateForVisualState(e.NewState, e.OldState);
        }

        private void UpdateForVisualState(VisualState newState, VisualState oldState)
        {
            if (oldState == DefaultState && newState == NarrowState)
            {
                AnswerSummaryTappedMethod(this._current);
            }
            if (oldState == NarrowState && newState == DefaultState)
            {
                AnswerSummaryTappedMethod(this._current);
            }
        }

        #region Answers By Created

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

                view.RegistEventHandler(this._answerAuthorTapped, this._answerQuestionTapped, this._answerSummaryTapped);
            }
            args.Handled = true;
        }

        #endregion

        #region Answers By VoteNum

        private TypedEventHandler<ListViewBase, ContainerContentChangingEventArgs> ByVoteChanging
        {
            get
            {
                return _byVoteChanging ??
                       (_byVoteChanging =
                           new TypedEventHandler<ListViewBase, ContainerContentChangingEventArgs>(
                               AnswersByVoteNum_OnContainerContentChanging));
            }
        }

        private TypedEventHandler<ListViewBase, ContainerContentChangingEventArgs> _byVoteChanging;

        private void AnswersByVoteNum_OnContainerContentChanging(ListViewBase sender,
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

                args.RegisterUpdateCallback(ByVoteChanging);
            }
            else if (args.Phase == 1)
            {
                view.ShowTitle();

                args.RegisterUpdateCallback(ByVoteChanging);
            }

            else if (args.Phase == 2)
            {
                view.ShowSummary();

                args.RegisterUpdateCallback(ByVoteChanging);
            }
            else if (args.Phase == 3)
            {
                view.ShowVerb();

                args.RegisterUpdateCallback(ByVoteChanging);
            }
            else if (args.Phase == 4)
            {
                view.ShowAuthor();

                args.RegisterUpdateCallback(ByVoteChanging);
            }
            else if (args.Phase == 5)
            {
                view.ShowVoteUp();

                args.RegisterUpdateCallback(ByVoteChanging);
            }
            else if (args.Phase == 6)
            {
                view.ShowAvatar();

                view.RegistEventHandler(this._answerAuthorTapped, this._answerQuestionTapped, this._answerSummaryTapped);
            }
            args.Handled = true;
        }

        #endregion
    }
}
