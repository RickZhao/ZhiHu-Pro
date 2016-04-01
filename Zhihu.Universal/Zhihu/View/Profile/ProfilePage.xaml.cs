using System;

using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media.Animation;
using Windows.UI.Xaml.Navigation;

using GalaSoft.MvvmLight.Ioc;

using Zhihu.ViewModel;
using Zhihu.Helper;


namespace Zhihu.View.Profile
{
    public sealed partial class ProfilePage : Page
    {
        private String _profileId = String.Empty;

        public ProfilePage()
        {
            this.InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            this.Loaded -= ProfilePage_Loaded;
            this.Loaded += ProfilePage_Loaded;
            SystemNavigationManager.GetForCurrentView().BackRequested -= ProfilePage_BackRequested;
            SystemNavigationManager.GetForCurrentView().BackRequested += ProfilePage_BackRequested;

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

            this.Loaded -= ProfilePage_Loaded;
            SystemNavigationManager.GetForCurrentView().BackRequested -= ProfilePage_BackRequested;

            if(e.NavigationMode == NavigationMode.Back)
            {
                ViewModelHelper.Instance.RemoveProfile(_profileId);
            }
        }

        private void ProfilePage_Loaded(object sender, RoutedEventArgs e)
        {
            Theme.Instance.UpdateRequestedTheme(this);
            UpdateBackButton();
        }

        private void ProfilePage_BackRequested(object sender, BackRequestedEventArgs e)
        {
            e.Handled = true;
            OnBackRequested();
        }
        
        private void OnBackRequested()
        {
            if (this.Frame != null && this.Frame.CanGoBack) this.Frame.GoBack(new DrillInNavigationTransitionInfo());

            UpdateBackButton();
        }

        private void UpdateBackButton()
        {
            SystemNavigationManager.GetForCurrentView().AppViewBackButtonVisibility = AppViewBackButtonVisibility.Visible;
        }

        private void NavToFollowingTopics_Tapped(object sender, TappedRoutedEventArgs e)
        {
            AppShellPage.AppFrame.Navigate(typeof(FollowingTopicsPage), _profileId);
            SystemNavigationManager.GetForCurrentView().BackRequested -= ProfilePage_BackRequested;
        }

        private void NavToFollowers_Tapped(object sender, TappedRoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(FolloweesPage), _profileId);
            SystemNavigationManager.GetForCurrentView().BackRequested -= ProfilePage_BackRequested;
        }

        private void NavToFollowees_Tapped(object sender, TappedRoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(FollowersPage), _profileId);
            SystemNavigationManager.GetForCurrentView().BackRequested -= ProfilePage_BackRequested;
        }

        private void NavToDetail_Tapped(object sender, TappedRoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(DetailsPage), _profileId);
            SystemNavigationManager.GetForCurrentView().BackRequested -= ProfilePage_BackRequested;
        }

        private void NavToActivities_Tapped(object sender, TappedRoutedEventArgs e)
        {
            AppShellPage.AppFrame.Navigate(typeof(ActivitiesPage), _profileId);
            SystemNavigationManager.GetForCurrentView().BackRequested -= ProfilePage_BackRequested;
        }

        private void NavToAnswers_Tapped(object sender, TappedRoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(AnswersPage), _profileId);
            SystemNavigationManager.GetForCurrentView().BackRequested -= ProfilePage_BackRequested;
        }

        private void NavToQuestions_Tapped(object sender, TappedRoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(QuestionsPage), _profileId);
            SystemNavigationManager.GetForCurrentView().BackRequested -= ProfilePage_BackRequested;
        }

        private void NavToFollowingColumns_Tapped(object sender, TappedRoutedEventArgs e)
        {
            this.Frame.Navigate(typeof (FollowingColumnsPage), _profileId);
            SystemNavigationManager.GetForCurrentView().BackRequested -= ProfilePage_BackRequested;
        }

        private void NavToColumns_Tapped(object sender, TappedRoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(ColumnsPage), _profileId);
            SystemNavigationManager.GetForCurrentView().BackRequested -= ProfilePage_BackRequested;
        }

        private void NavToCollections_Tapped(object sender, TappedRoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(CollectionsPage), _profileId);
            SystemNavigationManager.GetForCurrentView().BackRequested -= ProfilePage_BackRequested;
        }

        private void NavToSinaWeibo_Tapped(object sender, TappedRoutedEventArgs e)
        {
            //NavHelper.ProcessInnerHtml(SimpleIoc.Default.GetInstance<ProfileViewModel>().Profile.SinaWeiboName);
        }

        private void NavToMessage_Tapped(object sender, TappedRoutedEventArgs e)
        {
            NavHelper.NavToMessagePage(_profileId, this.Frame);
        }
    }
}
