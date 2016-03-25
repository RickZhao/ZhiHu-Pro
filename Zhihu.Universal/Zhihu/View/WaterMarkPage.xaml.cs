using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;


namespace Zhihu.View
{
    public sealed partial class WaterMarkPage : Page
    {
        public WaterMarkPage()
        {
            this.InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            //SystemNavigationManager.GetForCurrentView().BackRequested += LogoholderPage_BackRequested;
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            base.OnNavigatedFrom(e);

            //SystemNavigationManager.GetForCurrentView().BackRequested -= LogoholderPage_BackRequested;
        }

        private void LogoholderPage_BackRequested(object sender, BackRequestedEventArgs e)
        {
            e.Handled = true;
        }
    }
}
