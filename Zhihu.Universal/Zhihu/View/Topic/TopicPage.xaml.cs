using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media.Animation;
using Windows.UI.Xaml.Navigation;

using GalaSoft.MvvmLight.Ioc;

using Zhihu.ViewModel;
using Zhihu.Helper;


namespace Zhihu.View.Topic
{
    public sealed partial class TopicPage : Page
    {
        public TopicPage()
        {
            this.InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            this.Loaded += TopicPage_Loaded;
            SystemNavigationManager.GetForCurrentView().BackRequested += TopicPage_BackRequested;
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            base.OnNavigatedFrom(e);

            this.Loaded -= TopicPage_Loaded;
            SystemNavigationManager.GetForCurrentView().BackRequested -= TopicPage_BackRequested;
        }

        private void TopicPage_BackRequested(object sender, BackRequestedEventArgs e)
        {
            e.Handled = true;
            OnBackRequested();
        }

        private void TopicPage_Loaded(object sender, RoutedEventArgs e)
        {
            Theme.Instance.UpdateRequestedTheme(this);
            UpdateBackButton();
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

        private void NavToActivities_Tapped(object sender, TappedRoutedEventArgs e)
        {
            AppShellPage.AppFrame.Navigate(typeof(ActivitiesPage));
            SystemNavigationManager.GetForCurrentView().BackRequested -= TopicPage_BackRequested;
        }

        private void NavToBestAnswers_Tapped(object sender, TappedRoutedEventArgs e)
        {
            AppShellPage.AppFrame.Navigate(typeof(AnswersPage));
            SystemNavigationManager.GetForCurrentView().BackRequested -= TopicPage_BackRequested;
        }

        private void NavToAllQuestions_Tapped(object sender, TappedRoutedEventArgs e)
        {
            AppShellPage.AppFrame.Navigate(typeof(QuestionsPage));
            SystemNavigationManager.GetForCurrentView().BackRequested -= TopicPage_BackRequested;
        }
    }
}
