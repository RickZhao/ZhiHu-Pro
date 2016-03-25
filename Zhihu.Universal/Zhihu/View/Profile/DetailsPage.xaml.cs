using System;

using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media.Animation;
using Windows.UI.Xaml.Navigation;

using Zhihu.Common.Model;
using Zhihu.Helper;
using Zhihu.ViewModel;

namespace Zhihu.View.Profile
{
    public sealed partial class DetailsPage : Page
    {
        private String _profileId = String.Empty;

        public DetailsPage()
        {
            this.InitializeComponent();
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
            if (this.Frame != null && this.Frame.CanGoBack) this.Frame.GoBack(new DrillInNavigationTransitionInfo());

            UpdateBackButton();
        }

        private void Business_Tapped(object sender, TappedRoutedEventArgs e)
        {
            var grid = sender as Grid;
            var vm = grid?.DataContext as ProfileViewModel;
            if (vm == null) return;

            if (vm.Profile.Business == null || vm.Profile.Business.Id.HasValue == false) return;

            NavHelper.NavToTopicPage(vm.Profile.Business.Id.Value, this.Frame);

            SystemNavigationManager.GetForCurrentView().BackRequested -= Page_BackRequested;
        }

        private void Item_Tapped(object sender, TappedRoutedEventArgs e)
        {
            var grid = sender as Grid;
            if (grid == null) return;

            int? topicId = 0;

            if (grid.DataContext is Location)
            {
                topicId = (grid?.DataContext as Location)?.Id;
            }
            else if (grid.DataContext is Employment)
            {
                topicId = (grid?.DataContext as Employment)?.Id;
            }
            else if (grid.DataContext is Education)
            {
                topicId = (grid?.DataContext as Education)?.Id;
            }

            if (topicId.HasValue == false) return;

            NavHelper.NavToTopicPage(topicId.Value, this.Frame);

            SystemNavigationManager.GetForCurrentView().BackRequested -= Page_BackRequested;
        }
    }
}
