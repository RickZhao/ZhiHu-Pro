
using Windows.UI.Core;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

using Zhihu.Helper;


namespace Zhihu.View
{
    public sealed partial class LoginPage : Page
    {
        public LoginPage()
        {
            this.InitializeComponent();

            this.Loaded += LoginPage_Loaded;
        }

        private void LoginPage_Loaded(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            UpdateBackButton();
            Theme.Instance.UpdateRequestedTheme(this);
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            base.OnNavigatedFrom(e);

            SystemNavigationManager.GetForCurrentView().BackRequested -= LoginPage_BackRequested;
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            SystemNavigationManager.GetForCurrentView().BackRequested += LoginPage_BackRequested;
        }

        private void LoginPage_BackRequested(object sender, BackRequestedEventArgs e)
        {
            e.Handled = true;
            OnBackRequested();
        }

        private void OnBackRequested()
        {
            if (this.Frame.CanGoBack) this.Frame.GoBack();

            UpdateBackButton();
        }

        private void UpdateBackButton()
        {
            SystemNavigationManager.GetForCurrentView().AppViewBackButtonVisibility = AppViewBackButtonVisibility.Visible;
        }
    }
}
