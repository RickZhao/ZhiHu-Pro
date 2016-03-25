using System;

using Windows.Foundation;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using Windows.UI.Xaml.Media.Animation;

using GalaSoft.MvvmLight.Command;

using Zhihu.Controls.ItemView;
using Zhihu.ViewModel;
using Zhihu.Helper;


namespace Zhihu.View.Profile
{
    public sealed partial class FollowingTopicsPage : Page
    {
        private String _profileId = String.Empty;
        private RelayCommand<Common.Model.Topic> TopicTapped;

        public FollowingTopicsPage()
        {
            this.InitializeComponent();

            this.TopicTapped = new RelayCommand<Common.Model.Topic>(TopicTappedMethod);
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            this.Loaded -= FollowingTopicsPage_Loaded;
            this.Loaded += FollowingTopicsPage_Loaded;
            SystemNavigationManager.GetForCurrentView().BackRequested -= FollowingTopicsPage_BackRequested;
            SystemNavigationManager.GetForCurrentView().BackRequested += FollowingTopicsPage_BackRequested;

            if (e.NavigationMode == NavigationMode.New || e.NavigationMode == NavigationMode.Back)
            {
                _profileId = e.Parameter == null ? String.Empty : e.Parameter.ToString();
                var vm = ViewModelHelper.Instance.GetProfile(_profileId);
                this.DataContext = vm;
            }
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            this.Loaded -= FollowingTopicsPage_Loaded;
            SystemNavigationManager.GetForCurrentView().BackRequested -= FollowingTopicsPage_BackRequested;

            base.OnNavigatedFrom(e);
        }

        private void FollowingTopicsPage_BackRequested(object sender, BackRequestedEventArgs e)
        {
            e.Handled = true;
            OnBackRequested();
        }

        private void FollowingTopicsPage_Loaded(object sender, RoutedEventArgs e)
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
            if (this.Frame != null && this.Frame.CanGoBack) this.Frame.GoBack(new DrillInNavigationTransitionInfo());

            UpdateBackButton();
        }

        private void TopicTappedMethod(Common.Model.Topic topic)
        {
            NavHelper.NavToTopicPage(topic.Id, this.Frame);

            SystemNavigationManager.GetForCurrentView().BackRequested -= FollowingTopicsPage_BackRequested;
        }

        #region Topic Changing

        private TypedEventHandler<ListViewBase, ContainerContentChangingEventArgs> TopicChanging
        {
            get
            {
                return _topicChanging ??
                       (_topicChanging = new TypedEventHandler<ListViewBase, ContainerContentChangingEventArgs>(
                           Topic_OnContainerContentChanging));
            }
        }

        private TypedEventHandler<ListViewBase, ContainerContentChangingEventArgs> _topicChanging;

        private void Topic_OnContainerContentChanging(ListViewBase sender, ContainerContentChangingEventArgs args)
        {
            var view = args.ItemContainer.ContentTemplateRoot as TopicView;
            if (view == null) return;

            if (args.InRecycleQueue == true)
            {
                view.Clear();
            }
            else if (args.Phase == 0)
            {
                view.ShowPlaceHolder(args.Item as Common.Model.Topic);

                args.RegisterUpdateCallback(TopicChanging);
            }
            else if (args.Phase == 1)
            {
                view.ShowTitle();

                args.RegisterUpdateCallback(TopicChanging);
            }
            else if (args.Phase == 2)
            {
                view.ShowDescription();

                args.RegisterUpdateCallback(TopicChanging);
            }
            else if (args.Phase == 3)
            {
                view.ShowAvator();

                args.RegisterUpdateCallback(TopicChanging);
            }
            else if (args.Phase == 4)
            {
                view.RegistEventHandler(this.TopicTapped);
            }
            args.Handled = true;
        }

        #endregion
    }
}
