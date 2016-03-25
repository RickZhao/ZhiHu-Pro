using System;
using System.Diagnostics;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

using Windows.UI.Xaml;
using Windows.System;

using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;

using Zhihu.Common.DataSource;
using Zhihu.Common.Helper;
using Zhihu.Common.Model;
using Zhihu.Common.Model.Result;
using Zhihu.Common.Service;
using Zhihu.Controls;


namespace Zhihu.ViewModel
{
    public sealed class QuestionViewModel : ViewModelBase
    {
        private readonly IQuestion _questionService;
        private readonly IComment _commentService;
        private readonly ISocial _socialService;

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

        private Boolean _relationLoading = false;

        public Boolean RelationLoading
        {
            get { return _relationLoading; }
            private set
            {
                _relationLoading = value;
                RaisePropertyChanged(() => RelationLoading);
            }
        }

        [Data]
        public Int32 Id { get; private set; }

        [Data] private Boolean _detailLoaded = false;

        private Question _detail;

        [Data]
        public Question Detail
        {
            get { return _detail; }
            private set
            {
                if (_detail == value) return;

                _detail = value;
                RaisePropertyChanged(() => Detail);
            }
        }

        [Data] private Boolean _relationLoaded = false;

        private QuesRelationShip _relationship;

        [Data]
        public QuesRelationShip Relationship
        {
            get { return _relationship; }
            private set
            {
                if (_relationship == value) return;

                _relationship = value;
                RaisePropertyChanged(() => Relationship);
            }
        }

        private Boolean _answersLoading = false;

        public Boolean AnswersLoading
        {
            get { return _answersLoading; }
            private set
            {
                _answersLoading = value;
                RaisePropertyChanged(() => AnswersLoading);
            }
        }

        private IncrementalLoading<Answer> _answers;

        [Data]
        public IncrementalLoading<Answer> Answers
        {
            get { return _answers; }
            private set
            {
                if (_answers == value) return;
                _answers = value;

                RaisePropertyChanged(() => Answers);
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
        
        private Boolean _isAnoymous = false;

        public Boolean IsAnoymous
        {
            get { return _isAnoymous; }
            set
            {
                _isAnoymous = value;
                RaisePropertyChanged(() => IsAnoymous);
            }
        }

        private Boolean _shareTemplateLoading = false;

        public Boolean ShareTemplateLoading
        {
            get { return _shareTemplateLoading; }
            private set
            {
                _shareTemplateLoading = value;
                RaisePropertyChanged(()=>ShareTemplateLoading);
            }
        }

        private String _shareTemplate = String.Empty;
        [Data]
        public String ShareTemplate
        {
            get { return _shareTemplate; }
            private set
            {
                _shareTemplate = value;
                RaisePropertyChanged(() => ShareTemplate);
            }
        }

        #endregion

        public RelayCommand GetDetail { get; private set; }
        public RelayCommand GetRelation { get; private set; }
        public RelayCommand AuthorTapped { get; private set; }

        
        public RelayCommand FollowUnfollowQuestion { get; private set; }

        public RelayCommand AnonymousCommand { get; private set; }
        public RelayCommand CancelAnonymousCommand { get; private set; }

        public RelayCommand<String> CreateAnswer { get; private set; }
        public RelayCommand<String> CommentQuestion { get; private set; }
        
        //public RelayCommand<Comment> ReportCommentPopUp { get; private set; }

        public RelayCommand<Comment> VoteUpComment { get; private set; }

        public RelayCommand<String> ReplyComment { get; private set; }

        public RelayCommand<Comment> ReportJunkComment { get; private set; }
        public RelayCommand<Comment> ReportUnFridenlyComment { get; private set; }
        public RelayCommand<Comment> ReportIllegalComment { get; private set; }
        public RelayCommand<Comment> ReportPoliticalComment { get; private set; }

        public RelayCommand NavToShareCommand { get; private set; }
        public RelayCommand GetShareTemplate { get; private set; }
        public RelayCommand ShareViaQq{ get; private set; }
        public RelayCommand ShareViaWeibo { get; private set; }
        public RelayCommand ShareViaWeChat { get; private set; }

        public RelayCommand NavToAnswerQuestion { get; private set; }

        public RelayCommand NavToInviteCommand { get; private set; }
        public RelayCommand OpenWithEdge { get; private set; }


        private QuestionViewModel()
        {
            GetDetail = new RelayCommand(GetDetailMethod);
            GetRelation = new RelayCommand(GetRelationshipMethod);

            AuthorTapped = new RelayCommand(AuthorTappedMethod);
            
            FollowUnfollowQuestion = new RelayCommand(FollowUnfollowMethod);

            AnonymousCommand = new RelayCommand(AnonymousMethod);
            CancelAnonymousCommand = new RelayCommand(CancelAnonymousMethod);

            CreateAnswer = new RelayCommand<String>(CreateAnswerMethod);
            CommentQuestion = new RelayCommand<String>(CommentQuestionMethod);
            
            VoteUpComment = new RelayCommand<Comment>(VoteUpCommentMethod);
            ReplyComment = new RelayCommand<String>(ReplyCommentMethod);

            NavToShareCommand = new RelayCommand(NavToShareCommandMethod);
            GetShareTemplate = new RelayCommand(GetShareTemplateMethod);
            ShareViaQq = new RelayCommand(ShareViaQqMethod);
            ShareViaWeibo = new RelayCommand(ShareViaWeiboMethod);
            ShareViaWeChat = new RelayCommand(ShareViaWeChatMethod);

            ReportJunkComment = new RelayCommand<Comment>(ReportJunkCommentMethod);
            ReportUnFridenlyComment = new RelayCommand<Comment>(ReportUnFridenlyCommentMethod);
            ReportIllegalComment = new RelayCommand<Comment>(ReportIllegalCommentMethod);
            ReportPoliticalComment = new RelayCommand<Comment>(ReportPoliticalCommentMethod);
            
            NavToAnswerQuestion = new RelayCommand(NavToAnswerQuestionMethod);

            NavToInviteCommand = new RelayCommand(InviteCommandMethod);

            OpenWithEdge = new RelayCommand(OpenWithEdgeMethod);
        }

        public QuestionViewModel(IQuestion questionQuestionService, IComment commentService, ISocial socialService) : this()
        {
            _questionService = questionQuestionService;
            _commentService = commentService;
            _socialService = socialService;
        }

        public void Setup(Int32 questionId)
        {
            //if (Id > 0)
            //{
            //    VmHelper.SaveStates(this, Id.ToString());
            //}

            Id = questionId;

            //if (VmHelper.HasSaved(this, Id.ToString()))
            //{
            //    VmHelper.ResumeStates(this, Id.ToString());
            //    return;
            //}

            Detail = null;
            Relationship = null;

            Answers = null;
            Comments = null;

            _detailLoaded = false;
            _relationLoaded = false;

            Answers = new IncrementalLoading<Answer>(GetMoreAnswersMethod,
                String.Format("/questions/{0}/answers", Id), "offset=0&limit=20", false);

            Comments = new IncrementalLoading<Comment>(GetMoreCommentsMethod,
                String.Format("/questions/{0}/comments", Id), "offset=0&limit=20", false);
        }

        private async void GetDetailMethod()
        {
            if (null == _questionService) return;

            if (_detailLoaded) return;

            Loading = true;

            var result =
                await _questionService.GetDetailAsync(LoginUser.Current.Token, Id, true);

            Loading = false;
      
            if (result == null) return;

            if (null != result.Error)
            {
                ToasteIndicator.Instance.Show(String.Empty, result.Error.Message, null, 3);

                Debug.WriteLine(Regex.Unescape(result.Error.Message));
           
                return;
            }

            _detailLoaded = true;

            Detail = result.Result;
        }

        private async void GetRelationshipMethod()
        {
            if (null == _questionService) return;
            if (_relationLoaded) return;

            RelationLoading = true;

            var result = await _questionService.GetRelationshipAsync(LoginUser.Current.Token, Id, true);

            RelationLoading = false;
          
            if (result == null) return;

            if (null != result.Error)
            {
                ToasteIndicator.Instance.Show(String.Empty, result.Error.Message, null, 3);

                Debug.WriteLine(Regex.Unescape(result.Error.Message));
           
                return;
            }

            _relationLoaded = true;

            Relationship = result.Result;
        }

        private async Task<ListResultBase> GetMoreAnswersMethod(String requestUri)
        {
            if (null == _questionService || AnswersLoading) return null;

            AnswersLoading = true;

            var result = await _questionService.GetAnswersAsync(LoginUser.Current.Token, requestUri, true);
            
            AnswersLoading = false;

            if (result == null) return null;
            
            if (result.Error != null)
            {
                ToasteIndicator.Instance.Show(String.Empty, result.Error.Message, null, 3);

                Debug.WriteLine(Regex.Unescape(result.Error.Message));

                await Task.Delay(3000);

                return null;
            }

            return result;
        }

        private async Task<ListResultBase> GetMoreCommentsMethod(String requestUri)
        {
            if (null == _questionService || CommentsLoading) return null;

            CommentsLoading = true;

            var result = await _questionService.GetCommentsAsync(LoginUser.Current.Token, requestUri, true);

            CommentsLoading = false;
            
            if (result == null) return null;
            
            if (result.Error != null)
            {
                ToasteIndicator.Instance.Show(String.Empty, result.Error.Message, null, 3);

                Debug.WriteLine(Regex.Unescape(result.Error.Message));
             
                await Task.Delay(3000);
             
                return null;
            }

            return result;
        }

        private async void FollowUnfollowMethod()
        {
            #region Checking Network

            if (false == Utility.Instance.IsNetworkAvailable)
            {
                ToasteIndicator.Instance.Show(String.Empty, "网络连接已中断", null, 3);
                
                return;
            }

            #endregion

            if (null == _questionService || Detail == null ||
                Relationship == null) return;

            var questionId = Detail.Id;

            var result = Relationship.IsFollowing == false
                ? await _questionService.FollowAsync(LoginUser.Current.Token, questionId)
                : await _questionService.UnFollowAsync(LoginUser.Current.Token, questionId, LoginUser.Current.Profile.Id);

            if (null != result.Error)
            {
                ToasteIndicator.Instance.Show(String.Empty, result.Error.Message, null, 3);

                Debug.WriteLine(Regex.Unescape(result.Error.Message));
              
                return;
            }

            if (result.Result.IsFollowing)
            {
                Detail.FollowerCount++;
            }
            else
            {
                Detail.FollowerCount--;
            }

            Relationship.IsFollowing = result.Result.IsFollowing;
        }

        private async void AnonymousMethod()
        {
            #region Checking Network

            if (false == Utility.Instance.IsNetworkAvailable)
            {
                ToasteIndicator.Instance.Show(String.Empty, "网络连接已中断", null, 3);
               
                return;
            }

            #endregion

            if (null == _questionService || Relationship == null) return;

            var result = await _questionService.AnonymousAsync(LoginUser.Current.Token, Id);
            
            if (result == null) return;

            if (null != result.Error)
            {
                ToasteIndicator.Instance.Show(String.Empty, result.Error.Message, null, 3);

                Debug.WriteLine(Regex.Unescape(result.Error.Message));
                return;
            }

            Relationship.IsAnonymous = result.Result.IsAnonymous;
        }

        private async void CancelAnonymousMethod()
        {
            #region Checking Network

            if (false == Utility.Instance.IsNetworkAvailable)
            {
                ToasteIndicator.Instance.Show(String.Empty, "网络连接已中断", null, 3);
               
                return;
            }

            #endregion

            if (null == _questionService || Relationship == null) return;

            var result = await _questionService.CancelAnonymousAsync(LoginUser.Current.Token, Id);

            if (result == null) return;

            if (null != result.Error)
            {
                ToasteIndicator.Instance.Show(String.Empty, result.Error.Message, null, 3);

                Debug.WriteLine(Regex.Unescape(result.Error.Message));
               
                return;
            }

            Relationship.IsAnonymous = result.Result.IsAnonymous;
        }

        private async void CreateAnswerMethod(String answerContent)
        {
            #region Checking Network

            if (false == Utility.Instance.IsNetworkAvailable)
            {
                ToasteIndicator.Instance.Show(String.Empty, "网络连接已中断", null, 3);
               
                return;
            }

            #endregion

            if (null == _questionService) return;

            var result = await _questionService.AnswerAsync(LoginUser.Current.Token, Id, answerContent);

            if (result == null) return;

            if (null != result.Error)
            {
                ToasteIndicator.Instance.Show(String.Empty, result.Error.Message, null, 3);

                Debug.WriteLine(Regex.Unescape(result.Error.Message));
                
                return;
            }

            if (Detail == null) return;

            Detail.AnswerCount += 1;

            ToasteIndicator.Instance.Show(String.Empty, "回答已提交", null, 3);
            
            _relationLoaded = false;

            GetRelationshipMethod();
        }

        private void NavToShareCommandMethod()
        {
#if WINDOWS_PHONE_APP
            if (_navigation != null) _navigation.NavigateTo(typeof (QuestionSharePage));
#endif
        }

        private async void GetShareTemplateMethod()
        {
            if (null == _socialService) return;

            ShareTemplateLoading = true;

            var result = await _socialService.GetQuestionShareTemplate(LoginUser.Current.Token, Id, true);

            ShareTemplateLoading = false;

            if (result == null) return;

            if (null != result.Error)
            {
                ToasteIndicator.Instance.Show(String.Empty, result.Error.Message, null, 3);

                Debug.WriteLine(Regex.Unescape(result.Error.Message));

                return;
            }

            ShareTemplate = result.Result.Templates.Sina;

            if (String.IsNullOrEmpty(ShareTemplate))
            {
                ShareTemplate = String.Format("{0}{1} （分享自 @知乎）", Detail.Title, result.Result.Templates.ShortUrl);
            }
        }

        private void ShareViaQqMethod()
        {
           
        }

        private async void ShareViaWeiboMethod()
        {
            #region Checking Network

            if (false == Utility.Instance.IsNetworkAvailable)
            {
                ToasteIndicator.Instance.Show(String.Empty, "网络连接已中断", null, 3);
               
                return;
            }

            #endregion

             if (null == _socialService) return;

             ShareTemplateLoading = true;

             var result = await _socialService.ShareQuestionViaSina(LoginUser.Current.Token, Id, ShareTemplate);

            ShareTemplateLoading = false;

            if (result == null) return;

            if (null != result.Error)
            {
                ToasteIndicator.Instance.Show(String.Empty, result.Error.Message, null, 3);

                Debug.WriteLine(Regex.Unescape(result.Error.Message));
                
                return;
            }

            if (result.Result.Succeed)
            {
                ToasteIndicator.Instance.Show(String.Empty, "已分享至新浪微博", null, 3);

#if WINDOWS_PHONE_APP
                if (_navigation != null) _navigation.GoBack();
#endif
            }
        }

        private async void ShareViaWeChatMethod()
        {
            if (null == _socialService || this.Detail == null ) return;

            #region Checking Network

            if (false == Utility.Instance.IsNetworkAvailable)
            {
                ToasteIndicator.Instance.Show(String.Empty, "网络连接已中断", null, 3);

                return;
            }

            #endregion

            var result = await _socialService.ShareViaWeChat(Utility.Instance.WeChatAppId,
                this.Detail.Title,
                this.Detail.Excerpt,
                String.Format("http://www.zhihu.com/question/{0}", this.Detail.Id));
        }


        private async void VoteUpCommentMethod(Comment comment)
        { 
            #region Checking Network

            if (false == Utility.Instance.IsNetworkAvailable)
            {
                ToasteIndicator.Instance.Show(String.Empty, "网络连接已中断", null, 3);
              
                return;
            }

            #endregion

            if (_commentService == null || comment == null) return;

            if (comment.Voting == false)
            {
                var result = await _commentService.VoteUp(LoginUser.Current.Token, comment.Id);
              
                if (result == null) return;

                if (null != result.Error)
                {
                    ToasteIndicator.Instance.Show(String.Empty, result.Error.Message, null, 3);

                    Debug.WriteLine(Regex.Unescape(result.Error.Message));
                 
                    return;
                }

                comment.Voting = result.Result.Voting;
                comment.VoteCount = result.Result.VoteupCount;
            }
            else
            {
                var result = await _commentService.CancelVoteUp(LoginUser.Current.Token, comment.Id, LoginUser.Current.Profile.Id);
              
                if (result == null) return;

                if (null != result.Error)
                {
                    ToasteIndicator.Instance.Show(String.Empty, result.Error.Message, null, 3);

                    Debug.WriteLine(Regex.Unescape(result.Error.Message));
                 
                    return;
                }

                comment.Voting = result.Result.Voting;
                comment.VoteCount = result.Result.VoteupCount;
            }
        }

        private async void ReplyCommentMethod(String comment)
        {
            #region Checking Network

            if (false == Utility.Instance.IsNetworkAvailable)
            {
                ToasteIndicator.Instance.Show(String.Empty, "网络连接已中断", null, 3);

                return;
            }

            #endregion

            if (_commentService == null) return;

            var result =
                await
                    _commentService.ReplyQuestionComment(LoginUser.Current.Token, this.Detail.Id, CurrentComment.Id, comment);

            if (result == null) return;

            if (null != result.Error)
            {
                ToasteIndicator.Instance.Show(String.Empty, result.Error.Message, null, 3);

                Debug.WriteLine(Regex.Unescape(result.Error.Message));

                return;
            }

            ToasteIndicator.Instance.Show(String.Empty, String.Format("回复 {0} 的评论已发出", CurrentComment.Author.Name), null, 3);

            // Reload Comments
            Comments = new IncrementalLoading<Comment>(GetMoreCommentsMethod,
                String.Format("/questions/{0}/comments", Id), "offset=0&limit=20", false);

            Detail.CommentCount += 1;
        }

        private async void CommentQuestionMethod(String comment)
        {
            #region Checking Network

            if (false == Utility.Instance.IsNetworkAvailable)
            {
                ToasteIndicator.Instance.Show(String.Empty, "网络连接已中断", null, 3);

                return;
            }

            #endregion

            CommentSending = true;

            var result = await _commentService.CommentQuestionAsync(LoginUser.Current.Token, Id, comment);

            CommentSending = false;

            if (result == null) return;

            if (null != result.Error)
            {
                ToasteIndicator.Instance.Show(String.Empty, result.Error.Message, null, 3);

                Debug.WriteLine(Regex.Unescape(result.Error.Message));
                return;
            }

            ToasteIndicator.Instance.Show(String.Empty, "评论已发出", null, 3);

            // Reload Comments
            Comments = new IncrementalLoading<Comment>(GetMoreCommentsMethod,
                String.Format("/questions/{0}/comments", Id), "offset=0&limit=20", false);
            
            Detail.CommentCount += 1;
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
   
            if (result == null) return;

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
         
            if (result == null) return;

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

            if (result == null) return;

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

            if (result == null) return;

            if (null != result.Error)
            {
                ToasteIndicator.Instance.Show(String.Empty, result.Error.Message, null, 3);

                Debug.WriteLine(Regex.Unescape(result.Error.Message));
                return;
            }
            ToasteIndicator.Instance.Show(String.Empty, "已举报", null, 3);
        }

        private void AuthorTappedMethod()
        {
            if (Detail == null || Detail.Author == null) return;

            //VmNavHelper.NavToProfilePage(Detail.Author.Id, _navigation);
        }
        
        private void InviteCommandMethod()
        {
            //VmNavHelper.NavToSearchAndInvitePage(_questionId, _navigation);
        }

        private void NavToAnswerQuestionMethod()
        {
            //if (_navigation != null) _navigation.NavigateTo(typeof(AnswerQuestionPage));
        }

        private async void OpenWithEdgeMethod()
        {
            var uri = new Uri(String.Format("http://www.zhihu.com/question/{0}", this.Detail.Id));
            await Launcher.LaunchUriAsync(uri);
        }
    }
}