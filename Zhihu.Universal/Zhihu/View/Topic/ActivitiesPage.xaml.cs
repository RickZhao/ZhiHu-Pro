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


namespace Zhihu.View.Topic
{
    public sealed partial class ActivitiesPage : Page
    {
        private Common.Model.Answer _current;

        private RelayCommand<TopicActivity> QuestionTapped;
        private RelayCommand<Common.Model.Answer> AnswerTapped;

        public ActivitiesPage()
        {
            this.InitializeComponent();

            this.QuestionTapped = new RelayCommand<TopicActivity>(QuestionTappedMethod);
            this.AnswerTapped = new RelayCommand<Common.Model.Answer>(AnswerTappedMethod);

            this.PreviewFrame.Navigate(typeof(WaterMarkPage));
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            this.Loaded += ActivitiesPage_Loaded;
            SystemNavigationManager.GetForCurrentView().BackRequested += ActivitiesPage_BackRequested;
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            base.OnNavigatedFrom(e);
            
            this.Loaded -= ActivitiesPage_Loaded;
            SystemNavigationManager.GetForCurrentView().BackRequested -= ActivitiesPage_BackRequested;
        }

        private void ActivitiesPage_BackRequested(object sender, BackRequestedEventArgs e)
        {
            e.Handled = true;
            OnBackRequested();
        }
        
        private void OnBackRequested()
        {
            if (this.PreviewFrame.BackStack.Count > 0) return;

            this.Frame.GoBack(new DrillInNavigationTransitionInfo());

            UpdateBackButton();
        }

        private void ActivitiesPage_Loaded(object sender, RoutedEventArgs e)
        {
            Theme.Instance.UpdateRequestedTheme(this);
            UpdateBackButton();
        }

        private void UpdateBackButton()
        {
            SystemNavigationManager.GetForCurrentView().AppViewBackButtonVisibility = AppViewBackButtonVisibility.Visible;
        }

        private void VisualStateGroup_CurrentStateChanged(object sender, VisualStateChangedEventArgs e)
        {
            UpdateForVisualState(e.NewState, e.OldState);
        }

        private void UpdateForVisualState(VisualState newState, VisualState oldState)
        {
            AnswerTappedMethod(this._current);
        }

        #region Activity Tapped Methods

        private void QuestionTappedMethod(TopicActivity activity)
        {
            _current = null;
            NavHelper.NavToQuestionPage(activity.Id, this.Frame);
            SystemNavigationManager.GetForCurrentView().BackRequested -= ActivitiesPage_BackRequested;
        }

        private void AnswerTappedMethod(Common.Model.Answer answer)
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
                SystemNavigationManager.GetForCurrentView().BackRequested -= ActivitiesPage_BackRequested;
            }
            else
            {
                if (this.PreviewFrame.CanGoBack) this.PreviewFrame.GoBack();
            }

            NavHelper.NavToAnswerPage(answer.Id, navFrame);
        }

        #endregion

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
            var view = args.ItemContainer.ContentTemplateRoot as TopicActivityView;

            if (view == null) return;

            if (args.InRecycleQueue == true)
            {
                view.Clear();
            }
            else if (args.Phase == 0)
            {
                view.ShowPlaceholder(args.Item as TopicActivity);

                args.RegisterUpdateCallback(ActivityChanging);
            }
            else if (args.Phase == 1)
            {
                view.ShowTitle();

                args.RegisterUpdateCallback(ActivityChanging);
            }
            else if (args.Phase == 2)
            {
                view.ShowAnswer1();

                args.RegisterUpdateCallback(ActivityChanging);
            }
            else if (args.Phase == 3)
            {
                view.ShowAnswer2();

                args.RegisterUpdateCallback(ActivityChanging);
            }
            else if (args.Phase == 4)
            {
                view.ShowAnswer3();

                args.RegisterUpdateCallback(ActivityChanging);
            }
            else if (args.Phase == 5)
            {
                view.RegistEventHandler(this.QuestionTapped, this.AnswerTapped);
            }
            args.Handled = true;
        }

        #endregion
    }
}
