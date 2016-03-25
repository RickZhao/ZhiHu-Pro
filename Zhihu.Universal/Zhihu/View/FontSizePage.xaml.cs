
using Windows.Foundation;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media.Animation;
using Windows.UI.Xaml.Navigation;

using Zhihu.Common.Model;
using Zhihu.Controls.ItemView;
using Zhihu.Helper;


namespace Zhihu.View
{
    public sealed partial class FontSizePage : Page
    {
        public FontSizePage()
        {
            this.InitializeComponent();
        }

        private void FontSizePage_Loaded(object sender, RoutedEventArgs e)
        {
            Theme.Instance.UpdateRequestedTheme(this);
            UpdateBackButton();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            this.Loaded += FontSizePage_Loaded;

            SystemNavigationManager.GetForCurrentView().BackRequested += QuestionPage_BackRequested;
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            base.OnNavigatedFrom(e);

            this.Loaded -= FontSizePage_Loaded;

            SystemNavigationManager.GetForCurrentView().BackRequested -= QuestionPage_BackRequested;
        }
        
        private void QuestionPage_BackRequested(object sender, BackRequestedEventArgs e)
        {
            e.Handled = true;
            OnBackRequested();
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

        private TypedEventHandler<ListViewBase, ContainerContentChangingEventArgs> _feedChanging;

        private TypedEventHandler<ListViewBase, ContainerContentChangingEventArgs> FeedChanging
        {
            get
            {
                return _feedChanging ??
                       (_feedChanging =
                           new TypedEventHandler<ListViewBase, ContainerContentChangingEventArgs>(
                               FeedsView_OnContainerContentChanging));
            }
        }

        private void FeedsView_OnContainerContentChanging(ListViewBase sender, ContainerContentChangingEventArgs args)
        {
            var feedView = args.ItemContainer.ContentTemplateRoot as FeedView;

            if (feedView == null) return;

            if (args.InRecycleQueue == true)
            {
                feedView.Clear();
            }
            else if (args.Phase == 0)
            {
                feedView.ShowPlaceholder(args.Item as Feed);

                args.RegisterUpdateCallback(FeedChanging);
            }
            else if (args.Phase == 1)
            {
                feedView.ShowTitle();

                args.RegisterUpdateCallback(FeedChanging);
            }
            else if (args.Phase == 2)
            {
                feedView.ShowSummary();

                args.RegisterUpdateCallback(FeedChanging);
            }
            else if (args.Phase == 3)
            {
                feedView.ShowAuthor();

                args.RegisterUpdateCallback(FeedChanging);
            }
            else if (args.Phase == 4)
            {
                feedView.ShowVerb();

                args.RegisterUpdateCallback(FeedChanging);
            }
            else if (args.Phase == 5)
            {
                feedView.ShowVoteup();

                args.RegisterUpdateCallback(FeedChanging);
            }
            else if (args.Phase == 6)
            {
                feedView.ShowAvatar();

                feedView.RegistEventHandler(null, null, null);
            }
            args.Handled = true;
        }

        private void FontSizeReset_OnTapped(object sender, TappedRoutedEventArgs e)
        {
            Theme.Instance.ResetFontSize();
        }
    }
}
