using System;

using Windows.Foundation.Metadata;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

using Zhihu.Common.Helper;
using Zhihu.Common.Model;
using Zhihu.Helper;
using Zhihu.View;


namespace Zhihu
{
    public sealed partial class AppShellPage : Page
    {
        private static Frame _appFrame;

        public static Frame AppFrame
        {
            get
            {
                if (_appFrame == null)
                {
                    _appFrame = new Frame() { CacheSize = 100 };
                }
                return _appFrame;
            }
        }

        public AppShellPage()
        {
            this.InitializeComponent();
            
            MainFrameContainer.Children.Add(AppFrame);
        }

        private Boolean _loaded = false;

        private async void Page_Loaded(object sender, RoutedEventArgs e)
        {
            if (_loaded) return;

            _loaded = true;

            // hide status bar - do this only once (mobile device only)
            if (ApiInformation.IsTypePresent("Windows.UI.ViewManagement.StatusBar"))
            {
                var statusBar = StatusBar.GetForCurrentView();

                statusBar.ForegroundColor = Theme.Instance.FeedTitleColor;
                statusBar.BackgroundColor = Theme.Instance.PageBackColor;

                if (Theme.Instance.StatusBarIsOpen)
                    await statusBar.ShowAsync();
                else
                    await statusBar.HideAsync();
            }

            if (String.IsNullOrEmpty(LoginUser.Current.Token))
            {
                AppFrame.Navigate(typeof(LoginPage));
            }
            else
            {
                AppFrame.Navigate(typeof(MainPage));
            }

            Utility.Instance.HyperlinkTapped -= Instance_HyperlinkTapped;
            Utility.Instance.HyperlinkTapped += Instance_HyperlinkTapped;
        }

        private void Instance_HyperlinkTapped(String hyperlink)
        {
            NavHelper.HyperLinkClicked(hyperlink, AppShellPage.AppFrame);
        }
    }
}
