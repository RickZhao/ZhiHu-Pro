
using System;
using System.Linq;

using Windows.Foundation;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Navigation;

using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;

using Zhihu.Common.Model;
using Zhihu.Controls.ItemView;
using Zhihu.Helper;


namespace Zhihu.View.Main
{
    public sealed partial class FindPage : Page
    {
        private MainStatus MainStatus;

        private readonly RelayCommand<EditorRecommend> _recommendTitleTapped;
        private readonly RelayCommand<EditorRecommend> _recommendSummaryTapped;
        private readonly RelayCommand<EditorRecommend> _recommendAuthorTapped;

        private readonly RelayCommand<Common.Model.Answer> _answerAuthorTapped;
        private readonly RelayCommand<Common.Model.Answer> _answerQuestionTapped;
        private readonly RelayCommand<Common.Model.Answer> _answerSummaryTapped;

        private readonly RelayCommand<HotCollection> _hotCollectionTapped;

        public FindPage()
        {
            this.InitializeComponent();

            _recommendAuthorTapped = new RelayCommand<EditorRecommend>(RecommendAuthorTappedMethod);
            _recommendTitleTapped = new RelayCommand<EditorRecommend>(RecommendTitleTappedMethod);
            _recommendSummaryTapped = new RelayCommand<EditorRecommend>(RecommendSummaryTappedMethod);

            _answerAuthorTapped = new RelayCommand<Common.Model.Answer>(AnswerAuthorTappedMethod);
            _answerQuestionTapped = new RelayCommand<Common.Model.Answer>(AnswerQuestionTappedMethod);
            _answerSummaryTapped = new RelayCommand<Common.Model.Answer>(AnswerSummaryTappedMethod);

            _hotCollectionTapped = new RelayCommand<HotCollection>(HotCollectionTappedMethod);

            Messenger.Default.Register<MainStatus>(this, OnMainPageStatusChanged);
        }

        private void OnMainPageStatusChanged(MainStatus mainStatus)
        {
            MainStatus = mainStatus;
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            this.Loaded += FindPage_Loaded;
            SystemNavigationManager.GetForCurrentView().BackRequested += FindPage_BackRequested;
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            base.OnNavigatedFrom(e);

            this.Loaded -= FindPage_Loaded;
            SystemNavigationManager.GetForCurrentView().BackRequested -= FindPage_BackRequested;
        }

        private void FindPage_BackRequested(object sender, BackRequestedEventArgs e)
        {
            UpdateBackButton();
            Theme.Instance.UpdateRequestedTheme(this);
        }

        private void UpdateBackButton()
        {
            SystemNavigationManager.GetForCurrentView().AppViewBackButtonVisibility = AppViewBackButtonVisibility.Visible;
        }

        private void FindPage_Loaded(object sender, RoutedEventArgs e)
        {
            UpdateBackButton();
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
            if (bannerItem == null || MainStatus == null) return;

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

                NavHelper.NavToCollectionPage(collectionId, AppShellPage.AppFrame);

                SystemNavigationManager.GetForCurrentView().BackRequested -= FindPage_BackRequested;
            }
            else if (bannerItem.Url.Contains(roundTableTag))
            {
                var tableIdString =
                   bannerItem.Url.Substring(bannerItem.Url.IndexOf(roundTableTag, StringComparison.Ordinal) +
                                            roundTableTag.Length + 1);

                NavHelper.NavToTablePage(tableIdString, AppShellPage.AppFrame);

                SystemNavigationManager.GetForCurrentView().BackRequested -= FindPage_BackRequested;
            }
            else if (bannerItem.Url.Contains(articleTag))
            {
                var articleIdString =
                    bannerItem.Url.Substring(bannerItem.Url.IndexOf(articleTag, StringComparison.Ordinal) +
                                             articleTag.Length + 1);
                var articleId = 0;

                Int32.TryParse(articleIdString, out articleId);

                if (MainStatus.IsWide)
                {
                    SystemNavigationManager.GetForCurrentView().BackRequested -= FindPage_BackRequested;
                }
                else
                {
                    if (this.MainStatus.NavFrame.CanGoBack) this.MainStatus.NavFrame.GoBack();
                }

                NavHelper.NavToArticlePage(articleId, MainStatus.NavFrame);
            }
            else
            {
                if (MainStatus.IsWide)
                {
                    SystemNavigationManager.GetForCurrentView().BackRequested -= FindPage_BackRequested;
                }
                else
                {
                    if (this.MainStatus.NavFrame.CanGoBack) this.MainStatus.NavFrame.GoBack();
                }
                //NavHelper.NavToTablePage("rationalize", AppShellPage.AppFrame);
                NavHelper.HyperLinkClicked(bannerItem.Url, MainStatus.NavFrame);
            }
        }

        #region Recommend Changing

        private void RecommendAuthorTappedMethod(EditorRecommend recommend)
        {
        }

        private void RecommendTitleTappedMethod(EditorRecommend recommend)
        {
            if (recommend == null || MainStatus == null) return;

            if (recommend.Type == "question")
            {
                NavHelper.NavToQuestionPage(recommend.Question.Id, AppShellPage.AppFrame);
                SystemNavigationManager.GetForCurrentView().BackRequested -= FindPage_BackRequested;
            }
            if (recommend.Type == "answer")
            {
                NavHelper.NavToQuestionPage(recommend.Question.Id, AppShellPage.AppFrame);
                SystemNavigationManager.GetForCurrentView().BackRequested -= FindPage_BackRequested;
            }
            if (recommend.Type == "article")
            {
                if (MainStatus.IsWide)
                {
                    SystemNavigationManager.GetForCurrentView().BackRequested -= FindPage_BackRequested;
                }
                else
                {
                    if (this.MainStatus.NavFrame.CanGoBack) this.MainStatus.NavFrame.GoBack();
                }

                NavHelper.NavToArticlePage(recommend.Id, MainStatus.NavFrame);
            }
        }

        private void RecommendSummaryTappedMethod(EditorRecommend recommend)
        {
            if (recommend == null || MainStatus == null) return;

            if (recommend.Type == "question")
            {
                NavHelper.NavToAnswerPage(recommend.Question.Id, AppShellPage.AppFrame);
                SystemNavigationManager.GetForCurrentView().BackRequested -= FindPage_BackRequested;
            }
            if (recommend.Type == "answer")
            {
                if (MainStatus.IsWide)
                {
                    SystemNavigationManager.GetForCurrentView().BackRequested -= FindPage_BackRequested;
                }
                else
                {
                    if (this.MainStatus.NavFrame.CanGoBack) this.MainStatus.NavFrame.GoBack();
                }

                NavHelper.NavToAnswerPage(recommend.Id, MainStatus.NavFrame);
            }
            if (recommend.Type == "article")
            {
                if (MainStatus.IsWide)
                {
                    SystemNavigationManager.GetForCurrentView().BackRequested -= FindPage_BackRequested;
                }
                else
                {
                    if (this.MainStatus.NavFrame.CanGoBack) this.MainStatus.NavFrame.GoBack();
                }

                NavHelper.NavToArticlePage(recommend.Id, MainStatus.NavFrame);
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
            NavHelper.NavToCollectionPage(collection.Id, AppShellPage.AppFrame);

            SystemNavigationManager.GetForCurrentView().BackRequested -= FindPage_BackRequested;
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
            NavHelper.NavToProfilePage(answer.Author.Id, AppShellPage.AppFrame);
            SystemNavigationManager.GetForCurrentView().BackRequested -= FindPage_BackRequested;
        }

        private void AnswerQuestionTappedMethod(Common.Model.Answer answer)
        {
            NavHelper.NavToQuestionPage(answer.Question.Id, AppShellPage.AppFrame);
            SystemNavigationManager.GetForCurrentView().BackRequested -= FindPage_BackRequested;
        }

        private void AnswerSummaryTappedMethod(Common.Model.Answer answer)
        {
            if (answer == null || MainStatus == null) return;

            if (MainStatus.IsWide)
            {
                SystemNavigationManager.GetForCurrentView().BackRequested -= FindPage_BackRequested;
            }
            else
            {
                if (this.MainStatus.NavFrame.CanGoBack) this.MainStatus.NavFrame.GoBack();
            }

            NavHelper.NavToAnswerPage(answer.Id, MainStatus.NavFrame);
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
    }
}
