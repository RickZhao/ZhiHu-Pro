using System;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

using Windows.UI.Xaml;

using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;

using Zhihu.Common.Model;
using Zhihu.Common.Helper;
using Zhihu.Common.Model.Result;
using Zhihu.Common.Service;
using Zhihu.Controls;

namespace Zhihu.ViewModel
{
    public sealed class OfflineViewModel : ViewModelBase
    {
        private readonly IAnswer _answer;
        private readonly IArticle _article;
        private readonly IColumn _column;
        private readonly IFeed _feed;
        private readonly IQuestion _question;

        private Int32 _cacheAnswersCount = 0;

        public Int32 CacheAnswersCount
        {
            get { return _cacheAnswersCount; }
            set
            {
                _cacheAnswersCount = value;
                RaisePropertyChanged(() => CacheAnswersCount);
            }
        }

        private Int32 _cacheArticlesCount = 0;

        public Int32 CacheArticlesCount
        {
            get { return _cacheArticlesCount; }
            set
            {
                _cacheArticlesCount = value;
                RaisePropertyChanged(() => CacheArticlesCount);
            }
        }

        private Int32 _cacheCommentsCount = 0;

        public Int32 CacheCommentsCount
        {
            get { return _cacheCommentsCount; }
            set
            {
                _cacheCommentsCount = value;
                RaisePropertyChanged(() => CacheCommentsCount);
            }
        }

        private Int32 _cacheFeedsCount = 0;

        public Int32 CacheFeedsCount
        {
            get { return _cacheFeedsCount; }
            set
            {
                _cacheFeedsCount = value;
                RaisePropertyChanged(() => CacheFeedsCount);
            }
        }

        private readonly object _cachePrecentSync = new object();
        private Int32 _cachePercent = 0;

        public Int32 CachePercent
        {
            get { return _cachePercent; }
            set
            {
                _cachePercent = value;
                RaisePropertyChanged(() => CachePercent);
            }
        }

        private Visibility _finishMarkVisible = Visibility.Collapsed;

        public Visibility FinishMarkVisible
        {
            get { return _finishMarkVisible; }
            private set
            {
                _finishMarkVisible = value;
                RaisePropertyChanged(() => FinishMarkVisible);
            }
        }

        private readonly object _questionsSync = new object();
        private Int32 _questionsCount = 0;

        public Int32 QuestionsCount
        {
            get { return _questionsCount; }
            private set
            {
                _questionsCount = value;
                RaisePropertyChanged(() => QuestionsCount);
            }
        }

        private readonly object _answersSync = new object();
        private Int32 _answersCount = 0;

        public Int32 AnswersCount
        {
            get { return _answersCount; }
            private set
            {
                _answersCount = value;
                RaisePropertyChanged(() => AnswersCount);
            }
        }

        private readonly object _commentsSync = new object();
        private Int32 _commentsCount = 0;

        public Int32 CommentsCount
        {
            get { return _commentsCount; }
            private set
            {
                _commentsCount = value;
                RaisePropertyChanged(() => CommentsCount);
            }
        }

        private readonly object _articlesSync = new object();
        private Int32 _articlesCount = 0;

        public Int32 ArticlesCount
        {
            get { return _articlesCount; }
            private set
            {
                _articlesCount = value;
                RaisePropertyChanged(() => ArticlesCount);
            }
        }

        private Boolean _loading = false;

        public Boolean Loading
        {
            get { return _loading; }
            private set
            {
                _loading = value;
                RaisePropertyChanged(() => Loading);
            }
        }

        private Boolean _customOfflineEnable = true;

        public Boolean CustomOfflineEnable
        {
            get { return _customOfflineEnable; }
            private set
            {
                _customOfflineEnable = value;
                RaisePropertyChanged(() => CustomOfflineEnable);
            }
        }

        public RelayCommand StartOffline { get; private set; }

        private OfflineViewModel()
        {
            CacheFeedsCount = 40;
            CacheAnswersCount = 20;
            CacheCommentsCount = 20;
            CacheArticlesCount = 20;

            StartOffline = new RelayCommand(StartOfflineMethod);
        }

        public OfflineViewModel(IAnswer answer, IArticle article, IColumn column, IFeed feed, IQuestion question)
            : this()
        {
            this._answer = answer;
            this._article = article;
            this._column = column;
            this._feed = feed;
            this._question = question;
        }

        private async void StartOfflineMethod()
        {
            Loading = true;
            CustomOfflineEnable = false;
            FinishMarkVisible = Visibility.Collapsed;

            var iCachePageCount = 0;
            Paging nextPaging = null;

            var cachePagesCount = CacheFeedsCount/20;
            while (iCachePageCount < cachePagesCount)
            {
                #region 先完成整体Feeds下载

                await Task.Delay(10);

                var request = String.Empty;
                if (nextPaging == null)
                {
                    request = "feeds";
                }
                else
                {
                    var url = new Uri(nextPaging.Next);
                    request = url.PathAndQuery.Substring(1);
                }

                Debug.WriteLine(request);

                var result = await OfflinePage(request);

                if (result.Result != null)
                {
                    await OfflineFeeds(result);

                    iCachePageCount++;

                    nextPaging = result.Result.Paging;

                    var feedsCount = result.Result.GetItems().Length;
                    if (feedsCount < 20)
                    {
                        break;
                    }
                }
                else
                {
                    break;
                }

                #endregion
            }

            ToasteIndicator.Instance.Show(String.Empty, "离线缓存下载完成", null, 3);

            FinishMarkVisible = Visibility.Visible;
            CustomOfflineEnable = true;
            Loading = false;
        }

        private async Task<FeedsResult> OfflinePage(String request)
        {
            #region Checking Network

            if (false == Utility.Instance.IsNetworkAvailable)
            {
                ToasteIndicator.Instance.Show(String.Empty, "网络连接已中断", null, 3);
                await Task.Delay(3000);

                return null;
            }

            #endregion

            if (String.IsNullOrEmpty(LoginUser.Current.Token))
            {
                return null;
            }

            var result = await _feed.GetFeedsAsync(LoginUser.Current.Token, request, true);

            if (result == null) return null;

            if (result.Error != null)
            {
                ToasteIndicator.Instance.Show(String.Empty, result.Error.Message, null, 3);
            }

            return result;
        }

        private async Task OfflineFeeds(FeedsResult feeds)
        {
            if (feeds == null || feeds.Error != null || feeds.Result == null) return;

            var childTasks =
                feeds.Result.GetItems()
                    .Select(item => item as Feed)
                    .Select(OfflineFeed);

            await Task.Delay(10);

            await Task.WhenAll(childTasks);

            //foreach (var feed in feeds.Result.GetItems().Select(item => item as Feed))
            //{
            //    await OfflineFeed(feed);
            //}
        }

        private async Task OfflineFeed(Feed feed)
        {
            if (feed == null) return;

            Debug.WriteLine("Totoal Feeds Needs To Offline: {0} , Current Progress: {1}", CacheFeedsCount,
                CachePercent);

            await Task.Delay(10);

            switch (feed.Target.Type)
            {
                case "question":
                    Debug.WriteLine("Offline Feed: {0}", feed.Target.Title);
                    await OfflineQuestion(feed.Target.GetId());
                    break;

                case "answer":
                    Debug.WriteLine("Offline Feed: {0}", feed.Target.Question.Title);
                    await OfflineQuestion(feed.Target.Question.Id);
                    await OfflineAnswer(feed.Target.GetId());
                    break;

                case "article":
                    Debug.WriteLine("Offline Feed: {0}", feed.Target.Title);
                    await OfflineArticle(feed.Target.GetId());
                    break;

                case "column":
                    Debug.WriteLine("Offline Feed: {0}", feed.Target.Title);
                    await OfflineColumn(feed.Target.Id);
                    break;

                default:
                    Debug.WriteLine("===========================Feed Type: {0} 没有处理===========================",
                        feed.Target.Type);
                    break;
            }

            lock (_cachePrecentSync)
            {
                CachePercent++;
            }
        }

        private async Task OfflineAnswer(Int32 answerid)
        {
            if (null == _answer) return;

            var detailTask = _answer.GetAnswerDetailAsync(LoginUser.Current.Token, answerid, true);
            var relationTask = _answer.GetAnswerRelationshipAsync(LoginUser.Current.Token, answerid, true);
            var favoriteTask = _answer.CheckFavoriteAsync(LoginUser.Current.Token, answerid, true);

            var commentsTask = OfflineAnswerComments(answerid);

            await detailTask;
            await relationTask;
            await favoriteTask;
            await commentsTask;

            lock (_answersSync)
            {
                AnswersCount++;
            }
        }

        private async Task OfflineAnswerComments(Int32 answerId)
        {
            var iCachePageCount = 0;
            Paging nextPaging = null;

            var cachePagesCount = CacheCommentsCount/20;
            while (iCachePageCount < cachePagesCount)
            {
                var request = String.Empty;
                if (nextPaging == null)
                {
                    request = String.Format("answers/{0}/comments", answerId);
                }
                else
                {
                    var url = new Uri(nextPaging.Next);
                    request = url.PathAndQuery.Substring(1);
                }

                Debug.WriteLine(request);

                await Task.Delay(10);

                var comments = await _answer.GetCommentsAsync(LoginUser.Current.Token, request, true);

                await Task.Delay(10);

                if (comments.Result != null)
                {
                    iCachePageCount++;

                    nextPaging = comments.Result.Paging;

                    var commentsCount = comments.Result.GetItems().Length;

                    lock (_commentsSync)
                    {
                        CommentsCount += commentsCount;
                    }

                    if (commentsCount < 20)
                    {
                        break;
                    }
                }
                else
                {
                    break;
                }
            }
        }

        private async Task OfflineQuestion(Int32 questionId)
        {
            if (null == _question) return;

            var detailTask = _question.GetDetailAsync(LoginUser.Current.Token, questionId, true);

            var relationTask = _question.GetRelationshipAsync(LoginUser.Current.Token, questionId, true);

            await detailTask;
            await relationTask;

            await OfflineQuestionAnswers(questionId);

            await OfflineQuestionComments(questionId);

            lock (_questionsSync)
            {
                QuestionsCount++;
            }
        }

        private async Task OfflineArticle(Int32 articleId)
        {
            await Task.Delay(10);

            var detail = await _article.GetDetailAsync(LoginUser.Current.Token, articleId, true);

            await Task.Delay(10);

            await OfflineArticleComments(articleId);

            lock (_articlesSync)
            {
                ArticlesCount++;
            }
        }

        private async Task OfflineColumn(String columnId)
        {
            var detailTask = _column.GetDetailAsync(LoginUser.Current.Token, columnId, true);

            var followingTask = _column.CheckFollowingAsync(LoginUser.Current.Token, columnId, true);

            await detailTask;
            await followingTask;

            await OfflineColumnArticles(columnId);
        }

        private async Task OfflineQuestionAnswers(Int32 questionId)
        {
            var iCachePageCount = 0;
            Paging nextPaging = null;

            var cachePagesCount = CacheAnswersCount/20;
            while (iCachePageCount < cachePagesCount)
            {
                var request = String.Empty;
                if (nextPaging == null)
                {
                    request = String.Format("questions/{0}/answers", questionId);
                }
                else
                {
                    var url = new Uri(nextPaging.Next);
                    request = url.PathAndQuery.Substring(1);
                }

                Debug.WriteLine(request);

                await Task.Delay(10);

                var answers = await _question.GetAnswersAsync(LoginUser.Current.Token, request, true);

                await Task.Delay(10);

                if (answers.Result != null)
                {
                    //var childTasks =
                    //    answers.Result.GetItems()
                    //        .Select(item => item as Answer)
                    //        .Select(answer => OfflineAnswer(answer.Id));

                    //await Task.WhenAll(childTasks);

                    foreach (var answer in answers.Result.GetItems().Select(item => item as Answer))
                    {
                        await OfflineAnswer(answer.Id);
                    }

                    iCachePageCount++;

                    nextPaging = answers.Result.Paging;
                    var answersCount = answers.Result.GetItems().Length;

                    lock (_answersSync)
                    {
                        AnswersCount += answersCount;
                    }

                    if (answersCount < 20)
                    {
                        break;
                    }
                }
                else
                {
                    break;
                }
            }
        }

        private async Task OfflineQuestionComments(Int32 questionId)
        {
            var iCachePageCount = 0;
            Paging nextPaging = null;

            var cachePagesCount = CacheCommentsCount/20;
            while (iCachePageCount < cachePagesCount)
            {
                var request = String.Empty;
                if (nextPaging == null)
                {
                    request = String.Format("questions/{0}/comments", questionId);
                }
                else
                {
                    var url = new Uri(nextPaging.Next);
                    request = url.PathAndQuery.Substring(1);
                }

                Debug.WriteLine(request);

                await Task.Delay(10);

                var comments = await _question.GetCommentsAsync(LoginUser.Current.Token, request, true);

                await Task.Delay(10);

                if (comments.Result != null)
                {
                    iCachePageCount++;

                    nextPaging = comments.Result.Paging;

                    var commentsCount = comments.Result.GetItems().Length;

                    lock (_commentsSync)
                    {
                        CommentsCount += commentsCount;
                    }

                    if (commentsCount < 20)
                    {
                        break;
                    }
                }
                else
                {
                    break;
                }
            }
        }

        private async Task OfflineArticleComments(Int32 articleId)
        {
            var iCachePageCount = 0;
            Paging nextPaging = null;

            var cachePagesCount = CacheCommentsCount/20;
            while (iCachePageCount < cachePagesCount)
            {
                var request = String.Empty;
                if (nextPaging == null)
                {
                    request = String.Format("articles/{0}/comments", articleId);
                }
                else
                {
                    var url = new Uri(nextPaging.Next);
                    request = url.PathAndQuery.Substring(1);
                }

                Debug.WriteLine(request);

                var comments = await _article.GetCommentsAsync(LoginUser.Current.Token, articleId, request, true);

                await Task.Delay(10);

                if (comments.Result != null)
                {
                    iCachePageCount++;

                    nextPaging = comments.Result.Paging;

                    var commentsCount = comments.Result.GetItems().Length;

                    lock (_commentsSync)
                    {
                        CommentsCount += commentsCount;
                    }

                    if (commentsCount < 20)
                    {
                        break;
                    }
                }
                else
                {
                    break;
                }
            }
        }

        private async Task OfflineColumnArticles(String columnId)
        {
            var iCachePageCount = 0;
            Paging nextPaging = null;

            var cachePagesCount = CacheArticlesCount/10;
            while (iCachePageCount < cachePagesCount)
            {
                var request = String.Empty;
                if (nextPaging == null)
                {
                    request = String.Format("columns/{0}/articles", columnId);
                }
                else
                {
                    var url = new Uri(nextPaging.Next);
                    request = url.PathAndQuery.Substring(1);
                }

                Debug.WriteLine(request);

                await Task.Delay(10);

                var articles =
                    await
                        _column.GetArticles(LoginUser.Current.Token, request, true);

                await Task.Delay(10);

                if (articles.Result != null)
                {
                    //var childTasks =
                    //    articles.Result.GetItems()
                    //        .Select(item => item as Article)
                    //        .Select(article => OfflineArticle(article.Id));

                    //await Task.WhenAll(childTasks);

                    foreach (var article in articles.Result.GetItems().Select(item => item as Article))
                    {
                        await OfflineArticle(article.Id);
                    }

                    iCachePageCount++;

                    nextPaging = articles.Result.Paging;

                    var articlesCount = articles.Result.GetItems().Length;
                    lock (_articlesSync)
                    {
                        ArticlesCount += articlesCount;
                    }
                    if (articlesCount < 10)
                    {
                        break;
                    }
                }
                else
                {
                    break;
                }
            }
        }
    }
}