using System;

using Windows.Foundation;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Animation;
using Windows.UI.Xaml.Navigation;

using GalaSoft.MvvmLight.Command;

using Zhihu.Common.Model;
using Zhihu.Controls.ItemView;
using Zhihu.Helper;


namespace Zhihu.View.Profile
{
    public sealed partial class CollectionsPage : Page
    {
        private String _profileId = String.Empty;
        public RelayCommand<Collection> CollectionTapped { get; private set; }

        public CollectionsPage()
        {
            this.InitializeComponent();

            CollectionTapped = new RelayCommand<Collection>(CollectionTappedMethod);

            this.PreviewFrame.Navigate(typeof(WaterMarkPage));
        }

        private void CollectionTappedMethod(Collection collection)
        {
            NavHelper.NavToCollectionPage(collection.Id, AppShellPage.AppFrame);
            SystemNavigationManager.GetForCurrentView().BackRequested -= ProfileCollectionsPage_BackRequested;
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            this.Loaded -= ProfileCollectionsPage_Loaded;
            this.Loaded += ProfileCollectionsPage_Loaded;
            SystemNavigationManager.GetForCurrentView().BackRequested -= ProfileCollectionsPage_BackRequested;
            SystemNavigationManager.GetForCurrentView().BackRequested += ProfileCollectionsPage_BackRequested;

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
            
            this.Loaded -= ProfileCollectionsPage_Loaded;

            SystemNavigationManager.GetForCurrentView().BackRequested -= ProfileCollectionsPage_BackRequested;
        }

        private void ProfileCollectionsPage_BackRequested(object sender, BackRequestedEventArgs e)
        {
            e.Handled = true;
            OnBackRequested();
        }

        private void ProfileCollectionsPage_Loaded(object sender, RoutedEventArgs e)
        {
            Theme.Instance.UpdateRequestedTheme(this);
            UpdateBackButton();
        }

        private void UpdateBackButton()
        {
            SystemNavigationManager.GetForCurrentView().AppViewBackButtonVisibility = AppViewBackButtonVisibility.Visible;
        }
        
        private void OnBackRequested()
        {
            if (this.PreviewFrame.BackStack.Count > 0) return;

            this.Frame.GoBack(new DrillInNavigationTransitionInfo());

            UpdateBackButton();
        }
        
        #region Collection Changing


        private TypedEventHandler<ListViewBase, ContainerContentChangingEventArgs> CollectionChanging
        {
            get
            {
                return _collectionChanging ??
                       (_collectionChanging = new TypedEventHandler<ListViewBase, ContainerContentChangingEventArgs>(
                           Collection_OnContainerContentChanging));
            }
        }

        private TypedEventHandler<ListViewBase, ContainerContentChangingEventArgs> _collectionChanging;


        private void Collection_OnContainerContentChanging(ListViewBase sender, ContainerContentChangingEventArgs args)
        {
            var view = args.ItemContainer.ContentTemplateRoot as CollectionView;
            if (view == null) return;

            if (args.InRecycleQueue == true)
            {
                view.Clear();
                return;
            }
            switch (args.Phase)
            {
                case 0:
                    view.ShowPlaceHolder(args.Item as Collection);

                    args.RegisterUpdateCallback(CollectionChanging);
                    break;
                case 1:
                    view.ShowTitle();

                    args.RegisterUpdateCallback(CollectionChanging);
                    break;
                case 2:
                    view.ShowDescription();

                    args.RegisterUpdateCallback(CollectionChanging);
                    break;
                case 3:
                    view.ShowAvator();
                    view.ShowAuthor();

                    args.RegisterUpdateCallback(CollectionChanging);
                    break;
                case 4:
                    view.ShowCount();

                    view.RegistEventHandler(this.CollectionTapped);
                    break;
            }
            args.Handled = true;
        }

        #endregion
    }
}
