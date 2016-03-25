using System;

using Windows.Foundation;
using Windows.UI.Core;
using Windows.UI.Xaml.Media.Animation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

using GalaSoft.MvvmLight.Command;

using Zhihu.Common.Model;
using Zhihu.Controls.ItemView;
using Zhihu.Helper;


namespace Zhihu.View.Profile
{
    public sealed partial class ColumnsPage : Page
    {
        private String _profileId = String.Empty;
        public RelayCommand<Column> ColumnTapped { get; private set; }

        public ColumnsPage()
        {
            this.InitializeComponent();

            this.ColumnTapped = new RelayCommand<Column>(ColumnTappedMethod);
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            this.Loaded -= ColumnsPage_Loaded;
            this.Loaded += ColumnsPage_Loaded;
            SystemNavigationManager.GetForCurrentView().BackRequested -= ColumnsPage_BackRequested;
            SystemNavigationManager.GetForCurrentView().BackRequested += ColumnsPage_BackRequested;

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
            
            this.Loaded -= ColumnsPage_Loaded;
            SystemNavigationManager.GetForCurrentView().BackRequested -= ColumnsPage_BackRequested;
        }

        private void ColumnsPage_Loaded(object sender, RoutedEventArgs e)
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

        private void ColumnsPage_BackRequested(object sender, BackRequestedEventArgs e)
        {
            e.Handled = true;
            OnBackRequested();
        }

        private void ColumnTappedMethod(Column column)
        {
            NavHelper.NavToColumnPage(column.Id, AppShellPage.AppFrame);

            SystemNavigationManager.GetForCurrentView().BackRequested -= ColumnsPage_BackRequested;
        }

        #region Column Changing

        private TypedEventHandler<ListViewBase, ContainerContentChangingEventArgs> ColumnChanging
        {
            get
            {
                return _columnChanging ??
                       (_columnChanging =
                           new TypedEventHandler<ListViewBase, ContainerContentChangingEventArgs>(
                               Column_OnContainerContentChanging));
            }
        }
        private TypedEventHandler<ListViewBase, ContainerContentChangingEventArgs> _columnChanging;

        private void Column_OnContainerContentChanging(ListViewBase sender, ContainerContentChangingEventArgs args)
        {
            var view = args.ItemContainer.ContentTemplateRoot as ColumnView;

            if (view == null) return;

            if (args.InRecycleQueue == true)
            {
                view.Clear();
            }
            else if (args.Phase == 0)
            {
                view.ShowPlaceHolder(args.Item as Column);

                args.RegisterUpdateCallback(ColumnChanging);
            }
            else if (args.Phase == 1)
            {
                view.ShowTitle();

                args.RegisterUpdateCallback(ColumnChanging);
            }
            else if (args.Phase == 2)
            {
                view.ShowHeadline();

                args.RegisterUpdateCallback(ColumnChanging);
            }
            else if (args.Phase == 3)
            {
                view.ShowArticlsCount();

                args.RegisterUpdateCallback(ColumnChanging);
            }
            else if (args.Phase == 4)
            {
                view.ShowAvatar();

                args.RegisterUpdateCallback(ColumnChanging);
            }
            else if (args.Phase == 5)
            {
                view.RegistEventHandler(this.ColumnTapped);
            }
            args.Handled = true;
        }

        #endregion
    }
}
