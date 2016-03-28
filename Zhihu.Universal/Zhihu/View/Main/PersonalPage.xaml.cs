using GalaSoft.MvvmLight.Messaging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Zhihu.Helper;

// “空白页”项模板在 http://go.microsoft.com/fwlink/?LinkId=234238 上提供

namespace Zhihu.View.Main
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class PersonalPage : Page
    {
        private MainStatus MainStatus;

        public PersonalPage()
        {
            this.InitializeComponent();

            Messenger.Default.Register<MainStatus>(this, OnMainPageStatusChanged);
        }

        private void OnMainPageStatusChanged(MainStatus mainStatus)
        {
            MainStatus = mainStatus;
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            this.Loaded += PersonalPage_Loaded;
            SystemNavigationManager.GetForCurrentView().BackRequested += PersonalPage_BackRequested;
        }


        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            base.OnNavigatedFrom(e);

            this.Loaded -= PersonalPage_Loaded;
            SystemNavigationManager.GetForCurrentView().BackRequested -= PersonalPage_BackRequested;
        }

        private void PersonalPage_Loaded(object sender, RoutedEventArgs e)
        {
            UpdateBackButton();
            Theme.Instance.UpdateRequestedTheme(this);
        }

        private void PersonalPage_BackRequested(object sender, BackRequestedEventArgs e)
        {
            UpdateBackButton();
            Theme.Instance.UpdateRequestedTheme(this);
        }

        private void UpdateBackButton()
        {
            SystemNavigationManager.GetForCurrentView().AppViewBackButtonVisibility = AppViewBackButtonVisibility.Visible;
        }

        private void NavigateToMyProfile_Tapped(object sender, TappedRoutedEventArgs e)
        {
            if (MainStatus.IsWide)
            {
                SystemNavigationManager.GetForCurrentView().BackRequested -= PersonalPage_BackRequested;
            }
            else
            {
                if (MainStatus.NavFrame.CanGoBack) MainStatus.NavFrame.GoBack();
            }

            NavHelper.NavToProfilePage("self", MainStatus.NavFrame);
        }

        private void NavToMyFollowing_Tapped(object sender, TappedRoutedEventArgs e)
        {
            if (MainStatus.IsWide)
            {
                SystemNavigationManager.GetForCurrentView().BackRequested -= PersonalPage_BackRequested;
            }
            else
            {
                if (MainStatus.NavFrame.CanGoBack) MainStatus.NavFrame.GoBack();
            }

            NavHelper.NavToProfileFollowingPage("self", MainStatus.NavFrame);
        }

        private void NavToMyCollections_Tapped(object sender, TappedRoutedEventArgs e)
        {
            if (MainStatus.IsWide)
            {
                SystemNavigationManager.GetForCurrentView().BackRequested -= PersonalPage_BackRequested;
            }
            else
            {
                if (MainStatus.NavFrame.CanGoBack) MainStatus.NavFrame.GoBack();
            }

            NavHelper.NavToProfileCollectionsPage("self", MainStatus.NavFrame);
        }

        private void Search_Tapped(object sender, TappedRoutedEventArgs e)
        {
            if (MainStatus.IsWide)
            {
                SystemNavigationManager.GetForCurrentView().BackRequested -= PersonalPage_BackRequested;
            }
            else
            {
                if (MainStatus.NavFrame.CanGoBack) MainStatus.NavFrame.GoBack();
            }

            NavHelper.NavToSearchPage(MainStatus.NavFrame);
        }

        private void SettingNav_OnTapped(object sender, TappedRoutedEventArgs args)
        {
            if (MainStatus.IsWide)
            {
                SystemNavigationManager.GetForCurrentView().BackRequested -= PersonalPage_BackRequested;
            }
            else
            {
                if (MainStatus.NavFrame.CanGoBack) MainStatus.NavFrame.GoBack();
            }

            MainStatus.NavFrame.Navigate(typeof(SettingPage));
        }

        private void MakeQuestion_Tapped(object sender, TappedRoutedEventArgs e)
        {

        }

    }
}
