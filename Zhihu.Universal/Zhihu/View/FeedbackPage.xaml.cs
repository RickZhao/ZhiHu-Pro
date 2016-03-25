
using Windows.UI.Core;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;


namespace Zhihu.View
{
    public sealed partial class FeedbackPage : Page
    {
        public FeedbackPage()
        {
            this.InitializeComponent();
            this.Loaded += FeedbackPage_Loaded;
        }

        private void FeedbackPage_Loaded(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            UpdateBackButton();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            SystemNavigationManager.GetForCurrentView().BackRequested += FeedbackPage_BackRequested;
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            base.OnNavigatedFrom(e);

            SystemNavigationManager.GetForCurrentView().BackRequested -= FeedbackPage_BackRequested;
        }

        private void FeedbackPage_BackRequested(object sender, BackRequestedEventArgs e)
        {
            e.Handled = true;
            OnBackRequested();
        }

        private void OnBackRequested()
        {
            this.Frame.GoBack();

            UpdateBackButton();
        }

        private void UpdateBackButton()
        {
            SystemNavigationManager.GetForCurrentView().AppViewBackButtonVisibility = AppViewBackButtonVisibility.Visible;
        }
    }
}
