using System;

using Windows.Foundation;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Animation;
using Windows.UI.Xaml.Navigation;

using GalaSoft.MvvmLight.Command;

using Zhihu.Controls.ItemView;
using Zhihu.Helper;


namespace Zhihu.View.Profile
{
    public sealed partial class FollowersPage : Page
    {
        private String _profileId = String.Empty;
        private RelayCommand<Common.Model.Profile> ProfileTapped;

        public FollowersPage()
        {
            this.InitializeComponent();

            this.ProfileTapped = new RelayCommand<Common.Model.Profile>(ProfileTappedMethod);
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
        
        private void ProfileTappedMethod(Common.Model.Profile profile)
        {
            NavHelper.NavToProfilePage(profile.Id, this.Frame);
        }

        #region Author Changing

        private TypedEventHandler<ListViewBase, ContainerContentChangingEventArgs> AuthorChanging
        {
            get
            {
                return _authorChanging ??
                       (_authorChanging =
                           new TypedEventHandler<ListViewBase, ContainerContentChangingEventArgs>(
                               Author_OnContainerContentChanging));
            }
        }

        private TypedEventHandler<ListViewBase, ContainerContentChangingEventArgs> _authorChanging;

        private void Author_OnContainerContentChanging(ListViewBase sender, ContainerContentChangingEventArgs args)
        {
            var view = args.ItemContainer.ContentTemplateRoot as ProfileView;

            if (view == null) return;

            if (args.InRecycleQueue == true)
            {
                view.Clear();
            }
            else if (args.Phase == 0)
            {
                view.ShowPlaceHolder(args.Item as Common.Model.Profile);

                args.RegisterUpdateCallback(AuthorChanging);
            }
            else if (args.Phase == 1)
            {
                view.ShowAuthor();

                args.RegisterUpdateCallback(AuthorChanging);
            }
            else if (args.Phase == 2)
            {
                view.ShowHeadline();

                args.RegisterUpdateCallback(AuthorChanging);
            }
            else if (args.Phase == 3)
            {
                view.ShowAvatar();

                args.RegisterUpdateCallback(AuthorChanging);
            }
            else if (args.Phase == 4)
            {
                view.RegistEventHandler(this.ProfileTapped);
            }
            args.Handled = true;
        }

        #endregion
    }
}
