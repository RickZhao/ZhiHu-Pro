using System;

using Windows.Foundation;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;

using Zhihu.Common.Model;
using Zhihu.Controls.ItemView;
using Zhihu.Helper;


namespace Zhihu.View.Main
{
    public sealed partial class FeedsPage : Page
    {
        private MainStatus MainStatus;
        private Feed _current;
        private readonly RelayCommand<Feed> _feedTitleTapped;
        private readonly RelayCommand<Feed> _feedSummaryTapped;
        private readonly RelayCommand<Feed> _feedAuthorTapped;


        public FeedsPage()
        {
            this.InitializeComponent();

            _feedTitleTapped = new RelayCommand<Feed>(FeedTitleTappedMethod);
            _feedAuthorTapped = new RelayCommand<Feed>(FeedAuthorTappedMethod);
            _feedSummaryTapped = new RelayCommand<Feed>(FeedSummaryTappedMethod);

            Messenger.Default.Register<MainStatus>(this, OnMainPageStatusChanged);
        }

        private void OnMainPageStatusChanged(MainStatus mainStatus)
        {
            MainStatus = mainStatus;
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            this.Loaded += FeedsPage_Loaded;
            SystemNavigationManager.GetForCurrentView().BackRequested += FeedsPage_BackRequested;
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            base.OnNavigatedFrom(e);

            this.Loaded -= FeedsPage_Loaded;
            SystemNavigationManager.GetForCurrentView().BackRequested -= FeedsPage_BackRequested;
        }

        private void FeedsPage_BackRequested(object sender, BackRequestedEventArgs e)
        {
            UpdateBackButton();
            Theme.Instance.UpdateRequestedTheme(this);
        }

        private void UpdateBackButton()
        {
            SystemNavigationManager.GetForCurrentView().AppViewBackButtonVisibility = AppViewBackButtonVisibility.Visible;
        }

        private void FeedsPage_Loaded(object sender, RoutedEventArgs e)
        {
            UpdateBackButton();
            Theme.Instance.UpdateRequestedTheme(this);
        }

        private void FeedTitleTappedMethod(Feed feed)
        {
            if (feed == null) return;

            _current = feed;

            if (feed.Target.Type == "answer")
            {
                NavHelper.NavToQuestionPage(feed.Target.Question.Id, AppShellPage.AppFrame);
                SystemNavigationManager.GetForCurrentView().BackRequested -= FeedsPage_BackRequested;
            }
            else if (feed.Target.Type == "question")
            {
                NavHelper.NavToQuestionPage(feed.Target.GetId(), AppShellPage.AppFrame);
                SystemNavigationManager.GetForCurrentView().BackRequested -= FeedsPage_BackRequested;
            }
            else if (feed.Target.Type == "column")
            {
                NavHelper.NavToColumnPage(feed.Target.Id, AppShellPage.AppFrame);
                SystemNavigationManager.GetForCurrentView().BackRequested -= FeedsPage_BackRequested;
            }
            else if (feed.Target.Type == "article")
            {
                if (MainStatus.IsWide)
                {
                    SystemNavigationManager.GetForCurrentView().BackRequested -= FeedsPage_BackRequested;
                }
                else
                {
                    if (MainStatus.NavFrame.CanGoBack) this.MainStatus.NavFrame.GoBack();
                }

                NavHelper.NavToArticlePage(Int32.Parse(feed.Target.Id), MainStatus.NavFrame);
            }
            else if (feed.Target.Type == "roundtable")
            {
                if (MainStatus.IsWide)
                {
                    SystemNavigationManager.GetForCurrentView().BackRequested -= FeedsPage_BackRequested;
                }
                else
                {
                    if (this.MainStatus.NavFrame.CanGoBack) this.MainStatus.NavFrame.GoBack();
                }

                NavHelper.NavToTablePage(feed.Target.Id, MainStatus.NavFrame);
            }
        }

        private void FeedSummaryTappedMethod(Feed feed)
        {
            _current = feed;

            var id = feed.Target.GetId();

            if (MainStatus.IsWide)
            {
                SystemNavigationManager.GetForCurrentView().BackRequested -= FeedsPage_BackRequested;
            }
            else
            {
                if (this.MainStatus.NavFrame.CanGoBack) this.MainStatus.NavFrame.GoBack();
            }

            if (true == feed.Verb.Contains("ARTICLE"))
            {
                NavHelper.NavToArticlePage(id, MainStatus.NavFrame);
            }
            else
            {
                NavHelper.NavToAnswerPage(id, MainStatus.NavFrame);
            }
        }

        private void FeedAuthorTappedMethod(Feed feed)
        {
            if (MainStatus.IsWide)
            {
                SystemNavigationManager.GetForCurrentView().BackRequested -= FeedsPage_BackRequested;
            }
            else
            {
                if (this.MainStatus.NavFrame.CanGoBack) this.MainStatus.NavFrame.GoBack();
            }

            if (feed.Target.Type == "article")
            {
                NavHelper.NavToProfilePage(feed.Target.Author.Id, MainStatus.NavFrame);
            }
            else if (feed.Actors != null && feed.Actors.Length > 0)
            {
                if (feed.Actors[0].Type == "topic")
                {
                    NavHelper.NavToTopicPage(Int32.Parse(feed.Actors[0].Id), MainStatus.NavFrame);
                }
                else if (feed.Actors[0].Type == "people")
                {
                    NavHelper.NavToProfilePage(feed.Actors[0].Id, MainStatus.NavFrame);
                }
            }
            else
            {

            }
        }

        #region Feed Changing

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

        private TypedEventHandler<ListViewBase, ContainerContentChangingEventArgs> _feedChanging;

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

                feedView.RegistEventHandler(this._feedAuthorTapped, this._feedTitleTapped, this._feedSummaryTapped);
            }
            args.Handled = true;
        }

        #endregion
    }
}
