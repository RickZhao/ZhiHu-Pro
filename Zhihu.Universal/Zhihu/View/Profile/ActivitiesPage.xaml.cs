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
    public sealed partial class ActivitiesPage : Page
    {
        private String _profileId = String.Empty;

        private Activity _current;
        private readonly RelayCommand<Activity> _authorTapped;
        private readonly RelayCommand<Activity> _titleTapped;
        private readonly RelayCommand<Activity> _summaryTapped;

        public ActivitiesPage()
        {
            this.InitializeComponent();

            _authorTapped = new RelayCommand<Activity>(AuthorTappedMethod);
            _titleTapped = new RelayCommand<Activity>(TitleTappedMethod);
            _summaryTapped = new RelayCommand<Activity>(SummaryTappedMethod);

            this.PreviewFrame.Navigate(typeof(WaterMarkPage));
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            this.Loaded -= Page_Loaded;
            this.Loaded += Page_Loaded;
            SystemNavigationManager.GetForCurrentView().BackRequested -= Page_BackRequested;
            SystemNavigationManager.GetForCurrentView().BackRequested += Page_BackRequested;

            if (e.NavigationMode == NavigationMode.New || e.NavigationMode == NavigationMode.Back)
            {
                _profileId = e.Parameter == null ? String.Empty : e.Parameter.ToString();
                var vm = ViewModelHelper.Instance.GetProfile(_profileId);
                this.DataContext = vm;
            }
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            base.OnNavigatedFrom(e);
            
            this.Loaded -= Page_Loaded;
            SystemNavigationManager.GetForCurrentView().BackRequested -= Page_BackRequested;
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            Theme.Instance.UpdateRequestedTheme(this);
            UpdateBackButton();
        }

        private void Page_BackRequested(object sender, BackRequestedEventArgs e)
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
            SummaryTappedMethod(this._current);
        }

        private void AuthorTappedMethod(Activity activity)
        {
            NavHelper.NavToProfilePage(activity.Actor.Id, this.Frame);
            SystemNavigationManager.GetForCurrentView().BackRequested -= Page_BackRequested;
        }

        private void TitleTappedMethod(Activity activity)
        {
            if (activity == null) return;

            if (AdaptiveStates.CurrentState != DefaultState)
            {
                SystemNavigationManager.GetForCurrentView().BackRequested -= Page_BackRequested;
            }
            else
            {
                if (this.PreviewFrame.CanGoBack) this.PreviewFrame.GoBack();
            }

            if (activity.Verb.Contains("COLUMN"))
            {
                NavHelper.NavToColumnPage(activity.Target.Id, this.Frame);
            }
            else if (activity.Verb.Contains("COLLECTION"))
            {
                NavHelper.NavToCollectionPage(activity.Target.GetId(), this.Frame);
            }
            else if (activity.Verb.Contains("ARTICLE"))
            {
                NavHelper.NavToArticlePage(activity.Target.GetId(), this.Frame);
            }
            else if (activity.Verb.Contains("TOPIC"))
            {
                NavHelper.NavToTopicPage(activity.Target.GetId(), this.Frame);
            }
            else if (activity.Verb.Contains("ANSWER"))
            {
                NavHelper.NavToQuestionPage(activity.Target.Question.Id, this.Frame);
            }
            else if (activity.Verb.Contains("ROUNDTABLE"))
            {
                NavHelper.NavToQuestionPage(activity.Target.Question.Id, this.Frame);
            }
            else
            {
                NavHelper.NavToQuestionPage(activity.Target.GetId(), this.Frame);
            }
        }

        private void SummaryTappedMethod(Activity activity)
        {
            if (activity == null) return;

            this._current = activity;

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
                SystemNavigationManager.GetForCurrentView().BackRequested -= Page_BackRequested;
            }
            else
            {
                if (this.PreviewFrame.CanGoBack) this.PreviewFrame.GoBack();
            }

            var id = activity.Target.GetId();

            if (activity.Verb.Contains("ARTICLE"))
            {
                NavHelper.NavToArticlePage(id, navFrame);
            }
            else
            {
                NavHelper.NavToAnswerPage(id, navFrame);
            }
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
            var view = args.ItemContainer.ContentTemplateRoot as ActivityView;

            if (view == null) return;

            if (args.InRecycleQueue == true)
            {
                view.Clear();
            }
            else if (args.Phase == 0)
            {
                view.ShowPlaceholder(args.Item as Activity);

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
                view.ShowAuthor();

                args.RegisterUpdateCallback(ActivityChanging);
            }
            else if (args.Phase == 4)
            {
                view.ShowVerb();

                args.RegisterUpdateCallback(ActivityChanging);
            }
            else if (args.Phase == 5)
            {
                view.ShowAvatar();

                view.RegistEventHandler(this._authorTapped, this._titleTapped, this._summaryTapped);
            }
            args.Handled = true;
        }

        #endregion
    }
}
