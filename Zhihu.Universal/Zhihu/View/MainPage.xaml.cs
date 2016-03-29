using System;
using System.Linq;
using System.Threading.Tasks;

using Windows.ApplicationModel.Core;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Navigation;
using Windows.UI.Xaml.Media.Animation;
using Windows.UI.Xaml.Shapes;

using GalaSoft.MvvmLight.Messaging;

using Zhihu.Common.Model;
using Zhihu.Helper;
using Zhihu.View.Main;


namespace Zhihu.View
{
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();

            this.PreviewFrame.Navigate(typeof(WaterMarkPage));

            this.Loaded += Page_Loaded;
        }
        
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            this.Loaded += MainPage_Loaded;
            SystemNavigationManager.GetForCurrentView().BackRequested += MainPage_BackRequested;
        }
        
        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            base.OnNavigatedFrom(e);

            this.Loaded -= MainPage_Loaded;
            SystemNavigationManager.GetForCurrentView().BackRequested -= MainPage_BackRequested;
        }

        private void MainPage_BackRequested(object sender, BackRequestedEventArgs e)
        {
            e.Handled = true;

            OnBackRequested();

            UpdateBackButton();
        }
        private void MainPage_Loaded(object sender, RoutedEventArgs e)
        {
            BroadcastPageStatus();
            UpdateBackButton();
        }

        private void OnBackRequested()
        {
            if (this.PreviewFrame.BackStack.Count > 0) return;

            if (this.Frame.CanGoBack)
            {
                this.Frame.GoBack(new DrillInNavigationTransitionInfo());
            }
            else
            {
                if (LeaveTip.Visibility == Visibility.Collapsed)
                {
                    LeaveTipEaseOut.Begin();

                    return;
                }
                if (LeaveTip.Opacity > 0.2)
                {
                    CoreApplication.Exit();
                }
                else
                {
                    LeaveTipEaseOut.Begin();
                }
            }
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            Theme.Instance.UpdateRequestedTheme(this);
        }
     
        private Frame GetNavFrame()
        {
            var navFrame = AdaptiveStates.CurrentState == DefaultState ? this.PreviewFrame : this.Frame;

            return navFrame;
        }

        private async void ToggleSwitch_OnToggled(object sender, RoutedEventArgs arg)
        {
            await Task.Delay(50);

            Theme.Instance.UpdateRequestedTheme(this);
        }

        private void FeedsGoToTop_Tapped(object sender, TappedRoutedEventArgs e)
        {
            //var firstFeed = FeedsView.Items.FirstOrDefault();

            //if (firstFeed == null) return;

            //FeedsView.ScrollIntoView(firstFeed);
        }

        private async void SwitchThemeButton_Tapped(object sender, TappedRoutedEventArgs e)
        {
            await Task.Delay(50);
            Theme.Instance.UpdateRequestedTheme(this);
        }

        private void AppBarButton_Home_Tapped(object sender, TappedRoutedEventArgs e)
        {
            BroadcastPageStatus();
            //FeedsGrid.Visibility = FindGrid.Visibility = NoteGrid.Visibility = AboutGrid.Visibility = Visibility.Collapsed;

            //FeedsGrid.Visibility = Visibility.Visible;
        }

        private void AppBarButton_Find_Tapped(object sender, TappedRoutedEventArgs e)
        {
            BroadcastPageStatus();
            //FeedsGrid.Visibility = FindGrid.Visibility = NoteGrid.Visibility = AboutGrid.Visibility = Visibility.Collapsed;

            //FindGrid.Visibility = Visibility.Visible;
        }

        private void AppBarButton_Message_Tapped(object sender, TappedRoutedEventArgs e)
        {
            BroadcastPageStatus();
            //FeedsGrid.Visibility = FindGrid.Visibility = NoteGrid.Visibility = AboutGrid.Visibility = Visibility.Collapsed;

            //NoteGrid.Visibility = Visibility.Visible;
        }

        private void AppBarButton_Me_Tapped(object sender, TappedRoutedEventArgs e)
        {
            BroadcastPageStatus();
            //FeedsGrid.Visibility = FindGrid.Visibility = NoteGrid.Visibility = AboutGrid.Visibility = Visibility.Collapsed;

            //AboutGrid.Visibility = Visibility.Visible;
        }

        private async void UpdateBackButton()
        {
            await Task.Delay(50);

            var canGoBack = this.PreviewFrame.BackStackDepth > 0;

            SystemNavigationManager.GetForCurrentView().AppViewBackButtonVisibility = canGoBack ? AppViewBackButtonVisibility.Visible : AppViewBackButtonVisibility.Collapsed;
        }

        private void NotifyView_Tapped(object sender, TappedRoutedEventArgs e)
        {

        }
        
        private void AppAbout_Tapped(object sender, TappedRoutedEventArgs e)
        {
            GetNavFrame().Navigate(typeof(AboutPage));
        }

        private void PrivacyPolicy_Tapped(object sender, TappedRoutedEventArgs e)
        {
            var frame = GetNavFrame();
            NavHelper.NavToWebViewPage("http://zhihu.azurewebsites.net/privacy.html", frame);
        }

        private void AdaptiveStates_CurrentStateChanged(object sender, VisualStateChangedEventArgs e)
        {
            BroadcastPageStatus();
        }

        private async void BroadcastPageStatus()
        {
            var mainStatus = new MainStatus(AdaptiveStates.CurrentState != DefaultState, GetNavFrame());

            await Task.Delay(1000);

            Messenger.Default.Send(mainStatus);
        }

        private void ListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            BroadcastPageStatus();
        }
    }
}
