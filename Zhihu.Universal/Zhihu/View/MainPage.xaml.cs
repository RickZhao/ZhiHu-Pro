using System;
using System.Linq;
using System.Threading.Tasks;

using Windows.ApplicationModel.Core;
using Windows.Foundation;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Navigation;
using Windows.UI.Xaml.Media.Animation;
using Windows.UI.Xaml.Shapes;

using GalaSoft.MvvmLight.Command;

using Zhihu.Common.Model;
using Zhihu.Controls.ItemView;
using Zhihu.Helper;
using Zhihu.ViewModel;


namespace Zhihu.View
{
    public sealed partial class MainPage : Page
    {
        private Feed _current;

        private readonly RelayCommand<Common.Model.Answer> _answerAuthorTapped;
        private readonly RelayCommand<Common.Model.Answer> _answerQuestionTapped;
        private readonly RelayCommand<Common.Model.Answer> _answerSummaryTapped;

        private readonly RelayCommand<Feed> _feedTitleTapped;
        private readonly RelayCommand<Feed> _feedSummaryTapped;
        private readonly RelayCommand<Feed> _feedAuthorTapped;

        private readonly RelayCommand<EditorRecommend> _recommendTitleTapped;
        private readonly RelayCommand<EditorRecommend> _recommendSummaryTapped;
        private readonly RelayCommand<EditorRecommend> _recommendAuthorTapped;

        private readonly RelayCommand<HotCollection> _hotCollectionTapped;

        private readonly RelayCommand<NotifyItem> _notifyAuthorTapped;
        private readonly RelayCommand<NotifyItem> _notifyTitleTapped;
        private readonly RelayCommand<NotifyItem> _notifySummaryTapped;

        private readonly RelayCommand<Chat> _chatTapped;
        

        public MainPage()
        {
            this.InitializeComponent();

            _answerAuthorTapped = new RelayCommand<Common.Model.Answer>(AnswerAuthorTappedMethod);
            _answerQuestionTapped = new RelayCommand<Common.Model.Answer>(AnswerQuestionTappedMethod);
            _answerSummaryTapped = new RelayCommand<Common.Model.Answer>(AnswerSummaryTappedMethod);

            _feedTitleTapped = new RelayCommand<Feed>(FeedTitleTappedMethod);
            _feedAuthorTapped = new RelayCommand<Feed>(FeedAuthorTappedMethod);
            _feedSummaryTapped = new RelayCommand<Feed>(FeedSummaryTappedMethod);

            _recommendAuthorTapped = new RelayCommand<EditorRecommend>(RecommendAuthorTappedMethod);
            _recommendTitleTapped = new RelayCommand<EditorRecommend>(RecommendTitleTappedMethod);
            _recommendSummaryTapped = new RelayCommand<EditorRecommend>(RecommendSummaryTappedMethod);

            _hotCollectionTapped = new RelayCommand<HotCollection>(HotCollectionTappedMethod);


            _notifyAuthorTapped = new RelayCommand<NotifyItem>(NotifyAuthorTappedMethod);
            _notifyTitleTapped = new RelayCommand<NotifyItem>(NotifyTitleTappedMethod);
            _notifySummaryTapped = new RelayCommand<NotifyItem>(NotifySummaryTappedMethod);

            _chatTapped = new RelayCommand<Chat>(ChatTappedMethod);

            this.PreviewFrame.Navigate(typeof(WaterMarkPage));

            this.Loaded += Page_Loaded;
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            UpdateBackButton();
            SystemNavigationManager.GetForCurrentView().BackRequested += MainPage_BackRequested;
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            base.OnNavigatedFrom(e);

            SystemNavigationManager.GetForCurrentView().BackRequested -= MainPage_BackRequested;
        }

        private void MainPage_BackRequested(object sender, BackRequestedEventArgs e)
        {
            e.Handled = true;

            OnBackRequested();

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

        #region Feed

        private void FeedTitleTappedMethod(Feed feed)
        {
            if (feed == null) return;

            _current = feed;

            if (feed.Target.Type == "answer")
            {
                NavHelper.NavToQuestionPage(feed.Target.Question.Id, this.Frame);
                SystemNavigationManager.GetForCurrentView().BackRequested -= MainPage_BackRequested;
            }
            else if (feed.Target.Type == "question")
            {
                NavHelper.NavToQuestionPage(feed.Target.GetId(), this.Frame);
                SystemNavigationManager.GetForCurrentView().BackRequested -= MainPage_BackRequested;
            }
            else if (feed.Target.Type == "column")
            {
                NavHelper.NavToColumnPage(feed.Target.Id, this.Frame);
                SystemNavigationManager.GetForCurrentView().BackRequested -= MainPage_BackRequested;
            }
            else if (feed.Target.Type == "article")
            {
                if (AdaptiveStates.CurrentState != DefaultState)
                {
                    SystemNavigationManager.GetForCurrentView().BackRequested -= MainPage_BackRequested;
                }
                else
                {
                    if (this.PreviewFrame.CanGoBack) this.PreviewFrame.GoBack();
                }

                NavHelper.NavToArticlePage(Int32.Parse(feed.Target.Id), GetNavFrame());
            }
            else if (feed.Target.Type == "roundtable")
            {
                if (AdaptiveStates.CurrentState != DefaultState)
                {
                    SystemNavigationManager.GetForCurrentView().BackRequested -= MainPage_BackRequested;
                }
                else
                {
                    if (this.PreviewFrame.CanGoBack) this.PreviewFrame.GoBack();
                }

                NavHelper.NavToTablePage(feed.Target.Id, GetNavFrame());
            }
        }

        private void FeedSummaryTappedMethod(Feed feed)
        {
            _current = feed;

            var id = feed.Target.GetId();

            if (AdaptiveStates.CurrentState != DefaultState)
            {
                SystemNavigationManager.GetForCurrentView().BackRequested -= MainPage_BackRequested;
            }
            else
            {
                if (this.PreviewFrame.CanGoBack) this.PreviewFrame.GoBack();
            }

            if (true == feed.Verb.Contains("ARTICLE"))
            {
                NavHelper.NavToArticlePage(id, GetNavFrame());
            }
            else
            {
                NavHelper.NavToAnswerPage(id, GetNavFrame());
            }
        }

        private void FeedAuthorTappedMethod(Feed feed)
        {
            if (AdaptiveStates.CurrentState != DefaultState)
            {
                SystemNavigationManager.GetForCurrentView().BackRequested -= MainPage_BackRequested;
            }
            else
            {
                if (this.PreviewFrame.CanGoBack) this.PreviewFrame.GoBack();
            }

            if (feed.Target.Type == "article")
            {
                NavHelper.NavToProfilePage(feed.Target.Author.Id, GetNavFrame());
            }
            else if (feed.Actors != null && feed.Actors.Length > 0)
            {
                if (feed.Actors[0].Type == "topic")
                {
                    NavHelper.NavToTopicPage(Int32.Parse(feed.Actors[0].Id), GetNavFrame());
                }
                else if (feed.Actors[0].Type == "people")
                {
                    NavHelper.NavToProfilePage(feed.Actors[0].Id, GetNavFrame());
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

        #endregion

        #region Find

        #region Recommend Changing

        private void RecommendAuthorTappedMethod(EditorRecommend recommend)
        {
        }

        private void RecommendTitleTappedMethod(EditorRecommend recommend)
        {
            if (recommend == null) return;

            if (recommend.Type == "question")
            {
                NavHelper.NavToQuestionPage(recommend.Question.Id, this.Frame);
                SystemNavigationManager.GetForCurrentView().BackRequested -= MainPage_BackRequested;
            }
            if (recommend.Type == "answer")
            {
                NavHelper.NavToQuestionPage(recommend.Question.Id, this.Frame);
                SystemNavigationManager.GetForCurrentView().BackRequested -= MainPage_BackRequested;
            }
            if (recommend.Type == "article")
            {
                if (AdaptiveStates.CurrentState != DefaultState)
                {
                    SystemNavigationManager.GetForCurrentView().BackRequested -= MainPage_BackRequested;
                }
                else
                {
                    if (this.PreviewFrame.CanGoBack) this.PreviewFrame.GoBack();
                }

                NavHelper.NavToArticlePage(recommend.Id, GetNavFrame());
            }
        }

        private void RecommendSummaryTappedMethod(EditorRecommend recommend)
        {
            if (recommend == null) return;

            if (recommend.Type == "question")
            {
                NavHelper.NavToAnswerPage(recommend.Question.Id, this.Frame);
                SystemNavigationManager.GetForCurrentView().BackRequested -= MainPage_BackRequested;
            }
            if (recommend.Type == "answer")
            {
                if (AdaptiveStates.CurrentState != DefaultState)
                {
                    SystemNavigationManager.GetForCurrentView().BackRequested -= MainPage_BackRequested;
                }
                else
                {
                    if (this.PreviewFrame.CanGoBack) this.PreviewFrame.GoBack();
                }

                NavHelper.NavToAnswerPage(recommend.Id, GetNavFrame());
            }
            if (recommend.Type == "article")
            {
                if (AdaptiveStates.CurrentState != DefaultState)
                {
                    SystemNavigationManager.GetForCurrentView().BackRequested -= MainPage_BackRequested;
                }
                else
                {
                    if (this.PreviewFrame.CanGoBack) this.PreviewFrame.GoBack();
                }

                NavHelper.NavToArticlePage(recommend.Id, GetNavFrame());
            }
        }

        private TypedEventHandler<ListViewBase, ContainerContentChangingEventArgs> RecommendChanging
        {
            get
            {
                return _recommendChanging ??
                       (_recommendChanging = new TypedEventHandler<ListViewBase, ContainerContentChangingEventArgs>(
                           Recommend_OnContainerContentChanging));
            }
        }

        private TypedEventHandler<ListViewBase, ContainerContentChangingEventArgs> _recommendChanging;

        private void Recommend_OnContainerContentChanging(ListViewBase sender, ContainerContentChangingEventArgs args)
        {
            var view = args.ItemContainer.ContentTemplateRoot as EditorRecommendView;
            if (view == null) return;

            if (args.InRecycleQueue == true)
            {
                view.Clear();
            }
            else
            {
                switch (args.Phase)
                {
                    case 0:
                        view.ShowPlaceHolder(args.Item as EditorRecommend);

                        args.RegisterUpdateCallback(RecommendChanging);
                        break;
                    case 1:
                        view.ShowTitle();

                        args.RegisterUpdateCallback(RecommendChanging);
                        break;
                    case 2:
                        view.ShowSummary();

                        args.RegisterUpdateCallback(RecommendChanging);
                        break;
                    case 3:
                        view.ShowVoteCount();

                        args.RegisterUpdateCallback(RecommendChanging);
                        break;
                    case 4:
                        view.ShowAvatar();

                        view.RegisteEventHandler(_recommendTitleTapped, _recommendSummaryTapped, _recommendAuthorTapped);
                        break;
                }
                args.Handled = true;
            }
        }

        #endregion

        #region Collection Changing

        private void HotCollectionTappedMethod(HotCollection collection)
        {
            NavHelper.NavToCollectionPage(collection.Id, this.Frame);

            SystemNavigationManager.GetForCurrentView().BackRequested -= MainPage_BackRequested;
        }

        private TypedEventHandler<ListViewBase, ContainerContentChangingEventArgs> CollectionChanging
        {
            get
            {
                return _collectionChanging ??
                       (_collectionChanging = new TypedEventHandler<ListViewBase, ContainerContentChangingEventArgs>(
                           HotCollection_OnContainerContentChanging));
            }
        }

        private TypedEventHandler<ListViewBase, ContainerContentChangingEventArgs> _collectionChanging;

        private void HotCollection_OnContainerContentChanging(ListViewBase sender,
            ContainerContentChangingEventArgs args)
        {
            var view = args.ItemContainer.ContentTemplateRoot as HotCollectionView;
            if (view == null) return;

            if (args.InRecycleQueue == true)
            {
                view.Clear();
            }
            else
            {
                switch (args.Phase)
                {
                    case 0:
                        view.ShowPlaceHolder(args.Item as HotCollection);

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

                        view.RegistEventHandler(_hotCollectionTapped);
                        break;
                }
            }
            args.Handled = true;
        }

        #endregion

        #region Hot answers

        private void AnswerAuthorTappedMethod(Common.Model.Answer answer)
        {
            _current = null;
            NavHelper.NavToProfilePage(answer.Author.Id, this.Frame);
            SystemNavigationManager.GetForCurrentView().BackRequested -= MainPage_BackRequested;
        }

        private void AnswerQuestionTappedMethod(Common.Model.Answer answer)
        {
            _current = null;
            NavHelper.NavToQuestionPage(answer.Question.Id, this.Frame);
            SystemNavigationManager.GetForCurrentView().BackRequested -= MainPage_BackRequested;
        }

        private void AnswerSummaryTappedMethod(Common.Model.Answer answer)
        {
            if (answer == null) return;

            if (AdaptiveStates.CurrentState != DefaultState)
            {
                SystemNavigationManager.GetForCurrentView().BackRequested -= MainPage_BackRequested;
            }
            else
            {
                if (this.PreviewFrame.CanGoBack) this.PreviewFrame.GoBack();
            }

            NavHelper.NavToAnswerPage(answer.Id, GetNavFrame());
        }

        private TypedEventHandler<ListViewBase, ContainerContentChangingEventArgs> AnswerChanging
        {
            get
            {
                return _answerChanging ??
                       (_answerChanging =
                           new TypedEventHandler<ListViewBase, ContainerContentChangingEventArgs>(
                               HotAnswer_OnContainerContentChanging));
            }
        }

        private TypedEventHandler<ListViewBase, ContainerContentChangingEventArgs> _answerChanging;

        private void HotAnswer_OnContainerContentChanging(ListViewBase sender,
            ContainerContentChangingEventArgs args)
        {
            var view = args.ItemContainer.ContentTemplateRoot as AnswerActivityView;

            if (view == null) return;

            if (args.InRecycleQueue == true)
            {
                view.Clear();
            }
            else if (args.Phase == 0)
            {
                var hotAnswer = args.Item as HotAnswer;
                var answer = hotAnswer?.Answers?.FirstOrDefault();

                if (answer == null) return;

                view.ShowPlaceholder(answer);

                args.RegisterUpdateCallback(AnswerChanging);
            }
            else if (args.Phase == 1)
            {
                view.ShowTitle();

                args.RegisterUpdateCallback(AnswerChanging);
            }

            else if (args.Phase == 2)
            {
                view.ShowSummary();

                args.RegisterUpdateCallback(AnswerChanging);
            }
            else if (args.Phase == 3)
            {
                view.ShowVerb();

                args.RegisterUpdateCallback(AnswerChanging);
            }
            else if (args.Phase == 4)
            {
                view.ShowAuthor();

                args.RegisterUpdateCallback(AnswerChanging);
            }
            else if (args.Phase == 5)
            {
                view.ShowVoteUp();

                args.RegisterUpdateCallback(AnswerChanging);
            }
            else if (args.Phase == 6)
            {
                view.ShowAvatar();

                view.RegistEventHandler(this._answerAuthorTapped, this._answerQuestionTapped, this._answerSummaryTapped);
            }
            args.Handled = true;
        }

        #endregion

        #endregion

        #region Note

        private void NotifyAuthorTappedMethod(NotifyItem item)
        {
            if (item != null)
            {
                if (AdaptiveStates.CurrentState != DefaultState)
                {
                    SystemNavigationManager.GetForCurrentView().BackRequested -= MainPage_BackRequested;
                }
                else
                {
                    if (this.PreviewFrame.CanGoBack) this.PreviewFrame.GoBack();
                }

                NavHelper.NavToProfilePage(item.Operators[0].Id, GetNavFrame());
            }
        }

        private void NotifyTitleTappedMethod(NotifyItem item)
        {
            if (item == null) return;

            HasReadNotifyItem(item);

            if ("question" == item.Target.Type)
            {
                NavHelper.NavToQuestionPage(item.Target.GetId(), this.Frame);
                SystemNavigationManager.GetForCurrentView().BackRequested -= MainPage_BackRequested;
            }
            else if ("answer" == item.Target.Type)
            {
                if (AdaptiveStates.CurrentState != DefaultState)
                {
                    SystemNavigationManager.GetForCurrentView().BackRequested -= MainPage_BackRequested;
                }
                else
                {
                    if (this.PreviewFrame.CanGoBack) this.PreviewFrame.GoBack();
                }
                NavHelper.NavToQuestionPage(item.Target.Question.Id, GetNavFrame());
            }
            else if ("article" == item.Target.Type)
            {
                if (AdaptiveStates.CurrentState != DefaultState)
                {
                    SystemNavigationManager.GetForCurrentView().BackRequested -= MainPage_BackRequested;
                }
                else
                {
                    if (this.PreviewFrame.CanGoBack) this.PreviewFrame.GoBack();
                }
                NavHelper.NavToArticlePage(item.Target.GetId(), GetNavFrame());
            }
            else if("column" == item.Target.Type)
            {
                NavHelper.NavToColumnPage(item.Target.Id, this.Frame);
                SystemNavigationManager.GetForCurrentView().BackRequested -= MainPage_BackRequested;
            }
        }

        private void NotifySummaryTappedMethod(NotifyItem item)
        {
            if (item == null) return;

            HasReadNotifyItem(item);

            if (AdaptiveStates.CurrentState != DefaultState)
            {
                SystemNavigationManager.GetForCurrentView().BackRequested -= MainPage_BackRequested;
            }
            else
            {
                if (this.PreviewFrame.CanGoBack) this.PreviewFrame.GoBack();
            }

            if ("question" == item.Target.Type && item.Answer != null)
            {
                NavHelper.NavToAnswerPage(item.Answer.Id, GetNavFrame());
            }
            if ("answer" == item.Target.Type)
            {
                NavHelper.NavToAnswerPage(item.Target.GetId(), GetNavFrame());
            }
        }

        private void HasReadNotifyItem(NotifyItem item)
        {
            var mainVm = this.DataContext as MainViewModel;
            if (mainVm == null) return;

            var notifyVm = mainVm.Notify as NotifyViewModel;
            if (notifyVm == null) return;

            if (notifyVm.HasReadContent.CanExecute(item))
            {
                notifyVm.HasReadContent.Execute(item);
            }
        }

        private void ChatTappedMethod(Chat chat)
        {
            if (chat?.Participant != null)
            {
                if (AdaptiveStates.CurrentState != DefaultState)
                {
                    SystemNavigationManager.GetForCurrentView().BackRequested -= MainPage_BackRequested;
                }
                else
                {
                    if (this.PreviewFrame.CanGoBack) this.PreviewFrame.GoBack();
                }

                NavHelper.NavToMessagePage(chat.Participant.Id, GetNavFrame());
            }
        }

        #region Notifies

        private TypedEventHandler<ListViewBase, ContainerContentChangingEventArgs> NotifyChanging
        {
            get
            {
                return _notifyChanging ??
                       (_notifyChanging =
                           new TypedEventHandler<ListViewBase, ContainerContentChangingEventArgs>(
                               NotifyView_OnContainerContentChanging));
            }
        }

        private TypedEventHandler<ListViewBase, ContainerContentChangingEventArgs> _notifyChanging;


        private void NotifyView_OnContainerContentChanging(ListViewBase sender, ContainerContentChangingEventArgs args)
        {
            var feedView = args.ItemContainer.ContentTemplateRoot as NotifyView;

            if (feedView == null) return;

            if (args.InRecycleQueue == true)
            {
                feedView.Clear();
            }
            else if (args.Phase == 0)
            {
                feedView.ShowPlaceholder(args.Item as NotifyItem);

                args.RegisterUpdateCallback(NotifyChanging);
            }
            else if (args.Phase == 1)
            {
                feedView.ShowTitle();

                args.RegisterUpdateCallback(NotifyChanging);
            }
            else if (args.Phase == 2)
            {
                feedView.ShowAuthor();

                args.RegisterUpdateCallback(NotifyChanging);
            }
            else if (args.Phase == 3)
            {
                feedView.ShowVerb();

                args.RegisterUpdateCallback(NotifyChanging);
            }
            else if (args.Phase == 4)
            {
                feedView.ShowSummary();

                feedView.RegistEventHandler(_notifyAuthorTapped, _notifyTitleTapped, _notifySummaryTapped);
            }
            args.Handled = true;
        }

        #endregion

        #region Likes

        private TypedEventHandler<ListViewBase, ContainerContentChangingEventArgs> LikeChanging
        {
            get
            {
                return _likeChanging ??
                       (_likeChanging =
                           new TypedEventHandler<ListViewBase, ContainerContentChangingEventArgs>(
                               Likes_OnContainerContentChanging));
            }
        }

        private TypedEventHandler<ListViewBase, ContainerContentChangingEventArgs> _likeChanging;

        private void Likes_OnContainerContentChanging(ListViewBase sender, ContainerContentChangingEventArgs args)
        {
            var feedView = args.ItemContainer.ContentTemplateRoot as NotifyView;

            if (feedView == null) return;

            if (args.InRecycleQueue == true)
            {
                feedView.Clear();
            }
            else if (args.Phase == 0)
            {
                feedView.ShowPlaceholder(args.Item as NotifyItem);

                args.RegisterUpdateCallback(LikeChanging);
            }
            else if (args.Phase == 1)
            {
                feedView.ShowTitle();

                args.RegisterUpdateCallback(LikeChanging);
            }
            else if (args.Phase == 2)
            {
                feedView.ShowAuthor();

                args.RegisterUpdateCallback(LikeChanging);
            }
            else if (args.Phase == 3)
            {
                feedView.ShowVerb();

                args.RegisterUpdateCallback(LikeChanging);
            }
            else if (args.Phase == 4)
            {
                feedView.ShowSummary();

                feedView.RegistEventHandler(_notifyAuthorTapped, _notifyTitleTapped, _notifySummaryTapped);
            }
            args.Handled = true;
        }

        #endregion

        #region Chat

        private TypedEventHandler<ListViewBase, ContainerContentChangingEventArgs> ChatChanging
        {
            get
            {
                return _chatChanging ??
                       (_chatChanging =
                           new TypedEventHandler<ListViewBase, ContainerContentChangingEventArgs>(
                               ChatView_OnContainerContentChanging));
            }
        }
        
        private TypedEventHandler<ListViewBase, ContainerContentChangingEventArgs> _chatChanging;

        private void ChatView_OnContainerContentChanging(ListViewBase sender, ContainerContentChangingEventArgs args)
        {
            var feedView = args.ItemContainer.ContentTemplateRoot as ChatView;

            if (feedView == null) return;

            if (args.InRecycleQueue == true)
            {
                feedView.Clear();
            }
            else if (args.Phase == 0)
            {
                feedView.ShowPlaceholder(args.Item as Chat);

                args.RegisterUpdateCallback(ChatChanging);
            }
            else if (args.Phase == 1)
            {
                feedView.ShowTitle();

                args.RegisterUpdateCallback(ChatChanging);
            }
            else if (args.Phase == 2)
            {
                feedView.ShowAvatar();

                args.RegisterUpdateCallback(ChatChanging);
            }
            else if (args.Phase == 3)
            {
                feedView.ShowSummary();

                feedView.RegistEventHandler(_chatTapped);
            }
            args.Handled = true;
        }

        #endregion

        #endregion

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

        private void BannerItem_Tapped(object sender, TappedRoutedEventArgs e)
        {
            var border = sender as Border;

            var bannerItem = border?.DataContext as BannerItem;

            BannerTappedMethod(bannerItem);
        }

        private void BannerTappedMethod(BannerItem bannerItem)
        {
            if (bannerItem == null) return;

            var collectionTag = "collections";
            var roundTableTag = "roundtable";
            var articleTag = "articles";

            if (bannerItem.Url.Contains(collectionTag))
            {
                var collectionIdString =
                    bannerItem.Url.Substring(bannerItem.Url.IndexOf(collectionTag, StringComparison.Ordinal) +
                                             collectionTag.Length + 1);
                var collectionId = 0;

                Int32.TryParse(collectionIdString, out collectionId);

                NavHelper.NavToCollectionPage(collectionId, this.Frame);

                SystemNavigationManager.GetForCurrentView().BackRequested -= MainPage_BackRequested;
            }
            else if (bannerItem.Url.Contains(roundTableTag))
            {
                var tableIdString =
                   bannerItem.Url.Substring(bannerItem.Url.IndexOf(roundTableTag, StringComparison.Ordinal) +
                                            roundTableTag.Length + 1);

                NavHelper.NavToTablePage(tableIdString, this.Frame);

                SystemNavigationManager.GetForCurrentView().BackRequested -= MainPage_BackRequested;
            }
            else if (bannerItem.Url.Contains(articleTag))
            {
                var articleIdString =
                    bannerItem.Url.Substring(bannerItem.Url.IndexOf(articleTag, StringComparison.Ordinal) +
                                             articleTag.Length + 1);
                var articleId = 0;

                Int32.TryParse(articleIdString, out articleId);

                if (AdaptiveStates.CurrentState != DefaultState)
                {
                    SystemNavigationManager.GetForCurrentView().BackRequested -= MainPage_BackRequested;
                }
                else
                {
                    if (this.PreviewFrame.CanGoBack) this.PreviewFrame.GoBack();
                }

                NavHelper.NavToArticlePage(articleId, GetNavFrame());
            }
            else
            {
                if (AdaptiveStates.CurrentState != DefaultState)
                {
                    SystemNavigationManager.GetForCurrentView().BackRequested -= MainPage_BackRequested;
                }
                else
                {
                    if (this.PreviewFrame.CanGoBack) this.PreviewFrame.GoBack();
                }
                //NavHelper.NavToTablePage("rationalize", this.Frame);
                NavHelper.HyperLinkClicked(bannerItem.Url, GetNavFrame());
            }
        }

        private void Search_Tapped(object sender, TappedRoutedEventArgs e)
        {
            if (AdaptiveStates.CurrentState != DefaultState)
            {
                SystemNavigationManager.GetForCurrentView().BackRequested -= MainPage_BackRequested;
            }
            else
            {
                if (this.PreviewFrame.CanGoBack) this.PreviewFrame.GoBack();
            }

            NavHelper.NavToSearchPage(GetNavFrame());
        }

        private void MakeQuestion_Tapped(object sender, TappedRoutedEventArgs e)
        {

        }

        private void FeedsGoToTop_Tapped(object sender, TappedRoutedEventArgs e)
        {
            var firstFeed = FeedsView.Items.FirstOrDefault();

            if (firstFeed == null) return;

            FeedsView.ScrollIntoView(firstFeed);
        }

        private async void SwitchThemeButton_Tapped(object sender, TappedRoutedEventArgs e)
        {
            await Task.Delay(50);
            Theme.Instance.UpdateRequestedTheme(this);
        }

        private void AppBarButton_Home_Tapped(object sender, TappedRoutedEventArgs e)
        {
            FeedsGrid.Visibility = FindGrid.Visibility = NoteGrid.Visibility = AboutGrid.Visibility = Visibility.Collapsed;

            FeedsGrid.Visibility = Visibility.Visible;
        }

        private void AppBarButton_Find_Tapped(object sender, TappedRoutedEventArgs e)
        {
            FeedsGrid.Visibility = FindGrid.Visibility = NoteGrid.Visibility = AboutGrid.Visibility = Visibility.Collapsed;

            FindGrid.Visibility = Visibility.Visible;
        }

        private void AppBarButton_Message_Tapped(object sender, TappedRoutedEventArgs e)
        {
            FeedsGrid.Visibility = FindGrid.Visibility = NoteGrid.Visibility = AboutGrid.Visibility = Visibility.Collapsed;

            NoteGrid.Visibility = Visibility.Visible;
        }

        private void AppBarButton_Me_Tapped(object sender, TappedRoutedEventArgs e)
        {
            FeedsGrid.Visibility = FindGrid.Visibility = NoteGrid.Visibility = AboutGrid.Visibility = Visibility.Collapsed;

            AboutGrid.Visibility = Visibility.Visible;
        }

        private async void UpdateBackButton()
        {
            await Task.Delay(50);

            var canGoBack = this.PreviewFrame.BackStackDepth > 0;

            SystemNavigationManager.GetForCurrentView().AppViewBackButtonVisibility = canGoBack ? AppViewBackButtonVisibility.Visible : AppViewBackButtonVisibility.Collapsed;
        }

        private void NavigateToMyProfile_Tapped(object sender, TappedRoutedEventArgs e)
        {
            if (AdaptiveStates.CurrentState != DefaultState)
            {
                SystemNavigationManager.GetForCurrentView().BackRequested -= MainPage_BackRequested;
            }
            else
            {
                if (this.PreviewFrame.CanGoBack) this.PreviewFrame.GoBack();
            }

            NavHelper.NavToProfilePage("self", GetNavFrame());
        }

        private void NavToMyCollections_Tapped(object sender, TappedRoutedEventArgs e)
        {
            if (AdaptiveStates.CurrentState != DefaultState)
            {
                SystemNavigationManager.GetForCurrentView().BackRequested -= MainPage_BackRequested;
            }
            else
            {
                if (this.PreviewFrame.CanGoBack) this.PreviewFrame.GoBack();
            }

            NavHelper.NavToProfileCollectionsPage("self", GetNavFrame());
        }

        private void NavToMyFollowing_Tapped(object sender, TappedRoutedEventArgs e)
        {
            if (AdaptiveStates.CurrentState != DefaultState)
            {
                SystemNavigationManager.GetForCurrentView().BackRequested -= MainPage_BackRequested;
            }
            else
            {
                if (this.PreviewFrame.CanGoBack) this.PreviewFrame.GoBack();
            }

            NavHelper.NavToProfileFollowingPage("self", GetNavFrame());
        }

        private void SettingNav_OnTapped(object sender, TappedRoutedEventArgs args)
        {
            if (AdaptiveStates.CurrentState != DefaultState)
            {
                SystemNavigationManager.GetForCurrentView().BackRequested -= MainPage_BackRequested;
            }
            else
            {
                if (this.PreviewFrame.CanGoBack) this.PreviewFrame.GoBack();
            }

            this.GetNavFrame().Navigate(typeof(SettingPage));
        }

        private void NotifyFollowerAvatar_Tapped(object sender, TappedRoutedEventArgs e)
        {
            var ellipse = sender as Ellipse;
            if (ellipse == null) return;

            var notifyItem = ellipse.DataContext as NotifyItem;

            if (notifyItem == null) return;

            if (AdaptiveStates.CurrentState != DefaultState)
            {
                SystemNavigationManager.GetForCurrentView().BackRequested -= MainPage_BackRequested;
            }
            else
            {
                if (this.PreviewFrame.CanGoBack) this.PreviewFrame.GoBack();
            }

            NavHelper.NavToProfilePage(notifyItem.Operators[0].Id, GetNavFrame());
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
    }
}
