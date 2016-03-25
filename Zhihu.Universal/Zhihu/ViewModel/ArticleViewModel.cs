using System;
using System.Diagnostics;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

using Windows.UI.Xaml;
using Windows.System;

using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;

using Zhihu.Common.Model;
using Zhihu.Common.DataSource;
using Zhihu.Common.Helper;
using Zhihu.Common.Model.Result;
using Zhihu.Common.Service;
using Zhihu.Controls;


namespace Zhihu.ViewModel
{
    public sealed class ArticleViewModel : ViewModelBase
    {
        private readonly IArticle _article;
        private readonly IComment _commentService;
        private readonly ISocial _social;

        #region Properties

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

        [Data] private Int32 _articleId;


        [Data] private Boolean _detailLoaded = false;

        private Article _detail;

        [Data]
        public Article Detail
        {
            get { return _detail; }
            private set
            {
                if (_detail == value) return;
                _detail = value;
                RaisePropertyChanged(() => Detail);
            }
        }

        private Boolean _commentsLoading = false;

        public Boolean CommentsLoading
        {
            get { return _commentsLoading; }
            private set
            {
                _commentsLoading = value;
                RaisePropertyChanged(() => CommentsLoading);
            }
        }

        private const String FirstCommentsUri = "limit=10&offset=0";

        private IncrementalLoading<Comment> _comments;

        [Data]
        public IncrementalLoading<Comment> Comments
        {
            get { return _comments; }
            private set
            {
                if (_comments == value) return;
                _comments = value;
                RaisePropertyChanged(() => Comments);
            }
        }

        private Boolean _commentSending = false;

        public Boolean CommentSending
        {
            get { return _commentSending; }
            private set
            {
                _commentSending = value;
                RaisePropertyChanged(() => CommentSending);
            }
        }
        
        private Comment _currentComment;

        public Comment CurrentComment
        {
            get { return _currentComment; }
            set
            {
                _currentComment = value;
                RaisePropertyChanged(() => CurrentComment);
            }
        }

        #endregion

        public RelayCommand Load { get; private set; }
        
        public RelayCommand VoteUpTapped { get; private set; }
        public RelayCommand VoteDownTapped { get; private set; }
        
        public RelayCommand<Comment> VoteUpComment { get; private set; }
        public RelayCommand<String> CommentArticle { get; private set; }
        public RelayCommand<String> ReplyComment { get; private set; }

        public RelayCommand<Comment> ReportJunkComment { get; private set; }
        public RelayCommand<Comment> ReportUnFridenlyComment { get; private set; }
        public RelayCommand<Comment> ReportIllegalComment { get; private set; }
        public RelayCommand<Comment> ReportPoliticalComment { get; private set; }

        public RelayCommand ShareViaWeChat { get; private set;}
        public RelayCommand OpenWithEdge { get; private set; }

        private ArticleViewModel()
        {
            Load = new RelayCommand(LoadMethod);

            VoteUpTapped = new RelayCommand(VoteUpTappedMethod);
            VoteDownTapped = new RelayCommand(VoteDownTappedMethod);
            
            CommentArticle = new RelayCommand<String>(CommentArticleMethod);

            VoteUpComment = new RelayCommand<Comment>(VoteUpCommentMethod);
            ReplyComment = new RelayCommand<String>(ReplyCommentMethod);

            ReportJunkComment = new RelayCommand<Comment>(ReportJunkCommentMethod);
            ReportUnFridenlyComment = new RelayCommand<Comment>(ReportUnFridenlyCommentMethod);
            ReportIllegalComment = new RelayCommand<Comment>(ReportIllegalCommentMethod);
            ReportPoliticalComment = new RelayCommand<Comment>(ReportPoliticalCommentMethod);

            ShareViaWeChat = new RelayCommand(ShareToWeChatTimelineMethod);
            OpenWithEdge = new RelayCommand(OpenWithEdgeMethod);
        }

        public void Setup(Int32 articleId)
        {
            //if (_articleId > 0)
            //{
            //    VmHelper.SaveStates(this, articleId.ToString());
            //}
            
            _articleId = articleId;
            //VmHelper.ResumeStates(this, _articleId.ToString());

            Detail = null;
            _detailLoaded = false;

            Comments = new IncrementalLoading<Comment>(GetMoreCommentsMethod,
                String.Format("/articles/{0}/comments", _articleId), FirstCommentsUri, false);
        }
        
        public ArticleViewModel(IArticle articleService, IComment commentService, ISocial socialService) : this()
        {
            _article = articleService;
            _commentService = commentService;
            _social = socialService;
        }

        private async void LoadMethod()
        {
            if (_detailLoaded) return;

            Loading = true;

            var result = await _article.GetDetailAsync(LoginUser.Current.Token, _articleId, true);

            Loading = false;

            if (null != result.Error)
            {
                ToasteIndicator.Instance.Show(String.Empty, result.Error.Message, null, 3);

                Debug.WriteLine(Regex.Unescape(result.Error.Message));
                return;
            }

            _detailLoaded = true;

            Detail = result.Result;
        }

        private async Task<ListResultBase> GetMoreCommentsMethod(String requestUri)
        {
            if (null == _article || null == Detail || CommentsLoading) return null;

            CommentsLoading = true;

            var result = await _article.GetCommentsAsync(LoginUser.Current.Token, _articleId, requestUri, true);

            if (result.Result == null)
            {
                ToasteIndicator.Instance.Show(String.Empty, result.Error.Message, null, 3);
            }

            CommentsLoading = false;

            return result;
        }

        private async void VoteUpTappedMethod()
        {
            #region Checking Network

            if (false == Utility.Instance.IsNetworkAvailable)
            {
                ToasteIndicator.Instance.Show(String.Empty, "网络连接已中断", null, 3);

                return;
            }

            #endregion

            if (Detail.Voting == 1)
            {
                #region Cancel Voteup

                Loading = true;

                var result = await _article.CancelVoteUpAsync(LoginUser.Current.Token, _articleId);

                Loading = false;

                if (null != result.Error)
                {
                    ToasteIndicator.Instance.Show(String.Empty, result.Error.Message, null, 3);

                    Debug.WriteLine(Regex.Unescape(result.Error.Message));
                    return;
                }

                Detail.Voting = result.Result.Voting;
                Detail.VoteupCount = result.Result.VoteupCount;

                #endregion
            }
            else
            {
                #region Voteup

                Loading = true;

                var result = await _article.VoteUpAsync(LoginUser.Current.Token, _articleId);

                Loading = false;

                if (null != result.Error)
                {
                    ToasteIndicator.Instance.Show(String.Empty, result.Error.Message, null, 3);

                    Debug.WriteLine(Regex.Unescape(result.Error.Message));
                    return;
                }

                Detail.Voting = result.Result.Voting;
                Detail.VoteupCount = result.Result.VoteupCount;

                #endregion
            }
        }

        private async void VoteDownTappedMethod()
        {
            #region Checking Network

            if (false == Utility.Instance.IsNetworkAvailable)
            {
                ToasteIndicator.Instance.Show(String.Empty, "网络连接已中断", null, 3);
                
                return;
            }

            #endregion

            if (Detail.Voting == -1)
            {
                #region Cancel Votedown
                
                Loading = true;

                var result = await _article.CancelVoteDownAsync(LoginUser.Current.Token, _articleId, LoginUser.Current.Profile.Id);

                Loading = false;

                if (null != result.Error)
                {
                    ToasteIndicator.Instance.Show(String.Empty, result.Error.Message, null, 3);

                    Debug.WriteLine(Regex.Unescape(result.Error.Message));
                    return;
                }

                Detail.Voting = result.Result.Voting;
                Detail.VoteupCount = result.Result.VoteupCount;

                #endregion
            }
            else
            {
                #region Votedown
                
                Loading = true;

                var result = await _article.VoteDownAsync(LoginUser.Current.Token, _articleId, LoginUser.Current.Profile.Id);

                Loading = false;

                if (null != result.Error)
                {
                    ToasteIndicator.Instance.Show(String.Empty, result.Error.Message, null, 3);

                    Debug.WriteLine(Regex.Unescape(result.Error.Message));
                    return;
                }

                Detail.Voting = result.Result.Voting;
                Detail.VoteupCount = result.Result.VoteupCount;

                #endregion
            }
        }

        private async void VoteUpCommentMethod(Comment comment)
        {
            if (_commentService == null) return;

            #region Checking Network

            if (false == Utility.Instance.IsNetworkAvailable)
            {
                ToasteIndicator.Instance.Show(String.Empty, "网络连接已中断", null, 3);
               
                return;
            }

            #endregion

            if (comment.Voting == false)
            {
                #region Voteup
                
                var result = await _commentService.VoteUp(LoginUser.Current.Token, comment.Id);

                if (null != result.Error)
                {
                    ToasteIndicator.Instance.Show(String.Empty, result.Error.Message, null, 3);

                    Debug.WriteLine(Regex.Unescape(result.Error.Message));
                    return;
                }
                comment.Voting = result.Result.Voting;
                comment.VoteCount = result.Result.VoteupCount;

                #endregion
            }
            else
            {
                #region Cancel Voteup
                
                var result = await _commentService.CancelVoteUp(LoginUser.Current.Token, comment.Id, LoginUser.Current.Profile.Id);

                if (null != result.Error)
                {
                    ToasteIndicator.Instance.Show(String.Empty, result.Error.Message, null, 3);

                    Debug.WriteLine(Regex.Unescape(result.Error.Message));
                    return;
                }
                comment.Voting = result.Result.Voting;
                comment.VoteCount = result.Result.VoteupCount;

                #endregion
            }
        }

        private async void CommentArticleMethod(String comment)
        {
            #region Checking Network

            if (false == Utility.Instance.IsNetworkAvailable)
            {
                ToasteIndicator.Instance.Show(String.Empty, "网络连接已中断", null, 3);
             
                return;
            }

            #endregion

            CommentSending = true;

            var result = await _commentService.CommentArticleAsync(LoginUser.Current.Token, _articleId, comment);

            CommentSending = false;

            if (null != result.Error)
            {
                ToasteIndicator.Instance.Show(String.Empty, result.Error.Message, null, 3);

                Debug.WriteLine(Regex.Unescape(result.Error.Message));
                return;
            }
            
            Detail.CommentCount += 1;

            Comments = new IncrementalLoading<Comment>(GetMoreCommentsMethod,
                String.Format("/articles/{0}/comments", _articleId), FirstCommentsUri, false);

#if Windows_App
            if (_navigation != null) _navigation.GoBack();
#endif
        }
        private async void ReplyCommentMethod(String comment)
        {
            if (_commentService == null) return;

            #region Checking Network

            if (false == Utility.Instance.IsNetworkAvailable)
            {
                ToasteIndicator.Instance.Show(String.Empty, "网络连接已中断", null, 3);
             
                return;
            }

            #endregion

            var result =
                await
                    _commentService.ReplyArticleComment(LoginUser.Current.Token, this.Detail.Id, CurrentComment.Id, comment);

            if (null != result.Error)
            {
                ToasteIndicator.Instance.Show(String.Empty, result.Error.Message, null, 3);

                Debug.WriteLine(Regex.Unescape(result.Error.Message));
                return;
            }
            
            ToasteIndicator.Instance.Show(String.Empty, String.Format("回复 {0} 的评论已发出", CurrentComment.Author.Name), null, 3);

            Detail.CommentCount += 1;

            Comments = new IncrementalLoading<Comment>(GetMoreCommentsMethod,
                String.Format("/articles/{0}/comments", _articleId), FirstCommentsUri, false);
        }

        private async void ReportJunkCommentMethod(Comment comment)
        {
            #region Checking Network

            if (false == Utility.Instance.IsNetworkAvailable)
            {
                ToasteIndicator.Instance.Show(String.Empty, "网络连接已中断", null, 3);
              
                return;
            }

            #endregion

            var result = await _commentService.ReportComment(LoginUser.Current.Token, comment.Id, ReportReason.AdJunk);

            if (null != result.Error)
            {
                ToasteIndicator.Instance.Show(String.Empty, result.Error.Message, null, 3);

                Debug.WriteLine(Regex.Unescape(result.Error.Message));
                return;
            }
            ToasteIndicator.Instance.Show(String.Empty, "已举报", null, 3);
        }

        private async void ReportUnFridenlyCommentMethod(Comment comment)
        {
            #region Checking Network

            if (false == Utility.Instance.IsNetworkAvailable)
            {
                ToasteIndicator.Instance.Show(String.Empty, "网络连接已中断", null, 3);
             
                return;
            }

            #endregion

            var result = await _commentService.ReportComment(LoginUser.Current.Token, comment.Id, ReportReason.UnFridendly);

            if (null != result.Error)
            {
                ToasteIndicator.Instance.Show(String.Empty, result.Error.Message, null, 3);

                Debug.WriteLine(Regex.Unescape(result.Error.Message));
                return;
            }
            ToasteIndicator.Instance.Show(String.Empty, "已举报", null, 3);
        }

        private async void ReportIllegalCommentMethod(Comment comment)
        {
            #region Checking Network

            if (false == Utility.Instance.IsNetworkAvailable)
            {
                ToasteIndicator.Instance.Show(String.Empty, "网络连接已中断", null, 3);
               
                return;
            }

            #endregion

            var result = await _commentService.ReportComment(LoginUser.Current.Token, comment.Id, ReportReason.Illegal);

            if (null != result.Error)
            {
                ToasteIndicator.Instance.Show(String.Empty, result.Error.Message, null, 3);

                Debug.WriteLine(Regex.Unescape(result.Error.Message));
                return;
            }
            ToasteIndicator.Instance.Show(String.Empty, "已举报", null, 3);
        }

        private async void ReportPoliticalCommentMethod(Comment comment)
        {
            #region Checking Network

            if (false == Utility.Instance.IsNetworkAvailable)
            {
                ToasteIndicator.Instance.Show(String.Empty, "网络连接已中断", null, 3);
                
                return;
            }

            #endregion

            var result = await _commentService.ReportComment(LoginUser.Current.Token, comment.Id, ReportReason.Political);

            if (null != result.Error)
            {
                ToasteIndicator.Instance.Show(String.Empty, result.Error.Message, null, 3);

                Debug.WriteLine(Regex.Unescape(result.Error.Message));
                return;
            }
            ToasteIndicator.Instance.Show(String.Empty, "已举报", null, 3);
        }

        private async void ShareToWeChatTimelineMethod()
        {
            if (null == _social || this.Detail == null || this.Detail.Column == null ||
                this.Detail.Id == 0 || String.IsNullOrEmpty(this.Detail.Title) || String.IsNullOrEmpty(this.Detail.Column.Id)) return;

            #region Checking Network

            if (false == Utility.Instance.IsNetworkAvailable)
            {
                ToasteIndicator.Instance.Show(String.Empty, "网络连接已中断", null, 3);

                return;
            }

            #endregion

            var result = await _social.ShareViaWeChat(Utility.Instance.WeChatAppId,
                this.Detail.Title,
                String.Empty,
                String.Format("http://zhuanlan.zhihu.com/{0}/{1}", this.Detail.Column.Id, this.Detail.Id));
        }

        private async void OpenWithEdgeMethod()
        {
            var uri = new Uri(String.Format("http://zhuanlan.zhihu.com/{0}/{1}", this.Detail.Column.Id, this.Detail.Id));
            await Launcher.LaunchUriAsync(uri);
        }
    }
}