using System;

using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Animation;
using Windows.UI.Xaml.Navigation;

using Zhihu.Helper;


namespace Zhihu.View
{
    public sealed partial class WebBrowserPage : Page
    {
        public WebBrowserPage()
        {
            this.InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            this.Loaded += Page_Loaded;

            SystemNavigationManager.GetForCurrentView().BackRequested += Page_BackRequested;
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

        private void UpdateBackButton()
        {
            SystemNavigationManager.GetForCurrentView().AppViewBackButtonVisibility = AppViewBackButtonVisibility.Visible;
        }

        private void Page_BackRequested(object sender, BackRequestedEventArgs e)
        {
            e.Handled = true;
            OnBackRequested();
        }
        
        private void OnBackRequested()
        {
            if (this.Frame != null && this.Frame.CanGoBack) this.Frame.GoBack(new DrillInNavigationTransitionInfo());

            UpdateBackButton();
        }

        private void WebBrowserPage_OnLoaded(object sender, RoutedEventArgs e)
        {
            this.ProgressRing.Visibility = Visibility.Visible;

            this.WebView.Navigate(new Uri(WebView.Tag.ToString(), UriKind.RelativeOrAbsolute));
        }

        private void WebView_OnNavigationCompleted(WebView sender, WebViewNavigationCompletedEventArgs args)
        {
            this.ProgressRing.Visibility = Visibility.Collapsed;
        }
    }
}
