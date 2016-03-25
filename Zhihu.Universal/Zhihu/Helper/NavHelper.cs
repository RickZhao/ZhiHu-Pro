using System;
using System.Diagnostics;

using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

using Microsoft.Practices.ServiceLocation;

using GalaSoft.MvvmLight.Messaging;

using Zhihu.View;
using Zhihu.View.Answer;
using Zhihu.View.Article;
using Zhihu.View.Profile;
using Zhihu.View.Question;
using Zhihu.View.Table;
using Zhihu.View.Topic;
using Zhihu.ViewModel;


namespace Zhihu.Helper
{
    public sealed class NavHelper
    {
        private const String TableTag = "www.zhihu.com/roundtable/";
        private const String ColumnTag = "zhuanlan.zhihu.com/";
        private const String PeopleTag = "www.zhihu.com/people/";
        private const String QuestionTag = "www.zhihu.com/question/";

        public static void HyperLinkClicked(String hyperlink, Frame detailFrame)
        {
            Debug.WriteLine(hyperlink);

            ProcessInnerHtml(hyperlink, detailFrame);
        }

        public static void NavToWebViewPage(String webUri, Frame navigate)
        {
            var vm = ServiceLocator.Current.GetInstance<WebViewModel>();
            vm?.Setup(webUri);

            navigate?.Navigate(typeof(WebBrowserPage), webUri);
        }

        public static void NavToSearchAndInvitePage(int id, Frame navigate)
        {
            if (id <= 0) return;

            var vm = ServiceLocator.Current.GetInstance<SearchViewModel>();
            vm?.Setup(id);

            //navigate?.NavigateTo(typeof (SearchAndInvitePage));
        }

        public static void NavToQuestionPage(Int32 id, Frame navigate)
        {
            //var vm = ServiceLocator.Current.GetInstance<QuestionViewModel>();
            //vm?.Setup(id);

            navigate?.Navigate(typeof(QuestionPage), id);
        }

        public static void NavToAnswerPage(Int32 id, Frame navigate)
        {
            //var vm = ServiceLocator.Current.GetInstance<AnswerViewModel>();
            //vm?.Setup(id);

            navigate?.Navigate(typeof(AnswerPage), id);
        }

        public static void NavToArticlePage(Int32 id, Frame navigate)
        {
            //var vm = ServiceLocator.Current.GetInstance<ArticleViewModel>();
            //vm?.Setup(id);

            navigate?.Navigate(typeof(ArticlePage), id);
        }
        
        public static void NavToCollectionPage(Int32 id, Frame navigate)
        {
            if (id <= 0) return;

            var vm = ServiceLocator.Current.GetInstance<CollectionViewModel>();
            vm?.Setup(id);

            navigate?.Navigate(typeof(CollectionPage));
        }

        public static void NavToTopicPage(Int32 id, Frame navigate)
        {
            if (id <= 0) return;

            var vm = ServiceLocator.Current.GetInstance<TopicViewModel>();
            vm?.Setup(id);

            navigate?.Navigate(typeof(TopicPage));
        }

        public static void NavToProfilePage(String userId, Frame navigate)
        {
            if (String.IsNullOrEmpty(userId) || userId == "0") return;
            
            navigate?.Navigate(typeof(ProfilePage), userId);
        }

        public static void NavToTablePage(String tableId, Frame navigate)
        {
            if (String.IsNullOrEmpty(tableId)) return;

            var vm = ServiceLocator.Current.GetInstance<TableViewModel>();
            vm?.Setup(tableId);

            navigate?.Navigate(typeof(TablePage));
        }

        public static void NavToProfileCollectionsPage(String userId, Frame navigate)
        {           
            navigate?.Navigate(typeof(CollectionsPage), userId);
        }
        
        public static void NavToProfileFollowingPage(String userId, Frame navigate)
        {
            navigate?.Navigate(typeof(FollowingPage), userId);
        }

        public static void NavToColumnPage(String columnId, Frame navigate)
        {
            var vm = ServiceLocator.Current.GetInstance<ColumnViewModel>();
            vm?.Setup(columnId);

            navigate?.Navigate(typeof(ColumnPage));
        }

        public static void NavToSearchPage(Frame navigate)
        {
            var vm = ServiceLocator.Current.GetInstance<SearchViewModel>();
            vm?.Setup(0);

            if (navigate != null) navigate.Navigate(typeof(SearchPage));
        }

        public static void NavToMakeQuestionPage(Frame navigate)
        {
            var vm = ServiceLocator.Current.GetInstance<AskQuestionViewModel>();
            vm?.Setup(String.Empty);

            //if (navigate != null) navigate.NavigateTo(typeof(MakeQuestionPage));
        }

        public static void NavToMessagePage(String receiver, Frame navigate)
        {
            if (String.IsNullOrEmpty(receiver)) return;

            var vm = ServiceLocator.Current.GetInstance<MessageViewModel>();
            vm?.Setup(receiver);

            navigate?.Navigate(typeof(MessagePage));
        }

        private static void ProcessInnerHtml(String hyperLinkUrl, Frame detailFrame)
        {
            if (hyperLinkUrl.Contains("zhihu.com") == false)
            {
                NavToWebViewPage(hyperLinkUrl, AppShellPage.AppFrame);

                return;
            }

            if (hyperLinkUrl.Contains(PeopleTag))
            {
                #region 联系人

                var personId =
                    hyperLinkUrl.Substring(hyperLinkUrl.IndexOf(PeopleTag, StringComparison.Ordinal) + PeopleTag.Length);

                NavToProfilePage(personId, detailFrame);

                return;

                #endregion
            }

            if (hyperLinkUrl.Contains(QuestionTag) && hyperLinkUrl.Contains("/answer/"))
            {
                #region 答案

                Int32 answerId;

                var answerUri = new Uri(hyperLinkUrl, UriKind.RelativeOrAbsolute);

                if (Int32.TryParse(
                    answerUri.LocalPath.Substring(answerUri.LocalPath.LastIndexOf("/answer/", StringComparison.Ordinal) +
                                           "/answer/".Length), out answerId))
                {
                    NavToAnswerPage(answerId, detailFrame);
                }
                else if (hyperLinkUrl.Contains("?group_id="))
                {
                    var startIndex = hyperLinkUrl.LastIndexOf("/answer/", StringComparison.Ordinal) + "/answer/".Length;
                    var endIndex = hyperLinkUrl.LastIndexOf("?group_id=", StringComparison.Ordinal);

                    if (Int32.TryParse(
                        hyperLinkUrl.Substring(startIndex, endIndex - startIndex), out answerId))
                    {
                        NavToAnswerPage(answerId, detailFrame);
                    }
                }

                return;

                #endregion
            }

            if (hyperLinkUrl.Contains(QuestionTag))
            {
                #region 问题

                Int32 questionId;

                if (Int32.TryParse(
                    hyperLinkUrl.Substring(hyperLinkUrl.IndexOf(QuestionTag, StringComparison.Ordinal) +
                                           QuestionTag.Length), out questionId))
                {
                    NavToQuestionPage(questionId, AppShellPage.AppFrame);
                }

                return;

                #endregion
            }

            if (hyperLinkUrl.Contains(ColumnTag))
            {
                #region 专栏

                var columnRequest = hyperLinkUrl.Substring(hyperLinkUrl.IndexOf(ColumnTag, StringComparison.Ordinal) +
                                                           ColumnTag.Length);

                if (columnRequest.Contains("/"))
                {
                    var ids = columnRequest.Split('/');
                    if (ids.Length == 2)
                    {
                        Int32 articleId;

                        if (false == Int32.TryParse(ids[1], out articleId)) return;

                        NavToArticlePage(articleId, AppShellPage.AppFrame);
                    }
                }
                else
                {
                    NavToColumnPage(columnRequest, AppShellPage.AppFrame);
                }

                return;

                #endregion
            }

            if (hyperLinkUrl.Contains(TableTag))
            {
                #region 圆桌

                var tableRequest = hyperLinkUrl.Substring(hyperLinkUrl.IndexOf(TableTag, StringComparison.Ordinal) +
                                                           TableTag.Length);

                NavToTablePage(tableRequest, AppShellPage.AppFrame);

                return;

                #endregion
            }
            else
            {
                NavToWebViewPage(hyperLinkUrl, AppShellPage.AppFrame);
            }
        }
    }
}
