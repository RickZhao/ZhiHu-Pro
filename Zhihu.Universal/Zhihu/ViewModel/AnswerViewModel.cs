using System;
using System.Diagnostics;
using System.Linq;
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
using Zhihu.Helper;

namespace Zhihu.ViewModel
{
    public sealed class AnswerViewModel : ViewModelBase
    {
        private readonly IAnswer _answerService;
        private readonly IComment _commentService;
        private readonly ICollection _collection;
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

        [Data]
        private Int32 AnswerId { get; set; }

        [Data] private Boolean _isDetailLoaded = false;

        private AnswerDetail _detail;

        [Data]
        public AnswerDetail Detail
        {
            get { return _detail; }
            private set
            {
                if (_detail == value) return;

                _detail = value;
                RaisePropertyChanged(() => Detail);
            }
        }

        private AnswerRelationship _relationship = new AnswerRelationship();

        [Data]
        public AnswerRelationship Relationship
        {
            get { return _relationship; }
            private set
            {
                if (_relationship == value) return;

                _relationship = value;
                RaisePropertyChanged(() => Relationship);
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

        private Comment _currentComment;

        [Data]
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
                _comments = value;
                RaisePropertyChanged(() => Comments);
            }
        }

        private Boolean _collLoading = false;

        public Boolean CollLoading
        {
            get { return _collLoading; }
            private set
            {
                _collLoading = value;
                RaisePropertyChanged(() => CollLoading);
            }
        }

        private IncrementalLoading<Collection> _collections;

        [Data]
        public IncrementalLoading<Collection> Collections
        {
            get { return _collections; }
            private set
            {
                _collections = value;
                RaisePropertyChanged(() => Collections);
            }
        }

        private String _colleTitle = String.Empty;

        [Data]
        public String ColleTitle
        {
            get { return _colleTitle; }
            set
            {
                _colleTitle = value;
                RaisePropertyChanged(() => ColleTitle);
            }
        }

        private String _colleDes = String.Empty;

        [Data]
        public String ColleDes
        {
            get { return _colleDes; }
            set
            {
                _colleDes = value;
                RaisePropertyChanged(() => ColleDes);
            }
        }

        private Boolean _isCollePrivate = false;

        [Data]
        public Boolean IsCollePrivate
        {
            get { return _isCollePrivate; }
            set
            {
                _isCollePrivate = value;
                RaisePropertyChanged(() => IsCollePrivate);
            }
        }
        
        private Boolean _shareTemplateLoading = false;

        public Boolean ShareTemplateLoading
        {
            get { return _shareTemplateLoading; }
            private set
            {
                _shareTemplateLoading = value;
                RaisePropertyChanged(() => ShareTemplateLoading);
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
        public RelayCommand CheckFavorite { get; private set; }
        
        public RelayCommand AnswerNoHelpTapped { get; private set; }
        public RelayCommand AnswerThankTapped { get; private set; }
        public RelayCommand AnswerCollectTapped { get; private set; }

        public RelayCommand VoteUpTapped { get; private set; }
        public RelayCommand VoteDownTapped { get; private set; }

        public RelayCommand CreateCollection { get; private set; }

        public RelayCommand NavToShareCommand { get; private set; }
        public RelayCommand GetShareTemplate { get; private set; }
        public RelayCommand ShareViaQq { get; private set; }
        public RelayCommand ShareViaWeibo { get; private set; }
        public RelayCommand OpenWithEdge { get; private set; }
        
        public RelayCommand<Comment> ReportJunkComment { get; private set; }
        public RelayCommand<Comment> ReportUnFridenlyComment { get; private set; }
        public RelayCommand<Comment> ReportIllegalComment { get; private set; }
        public RelayCommand<Comment> ReportPoliticalComment { get; private set; }

        public RelayCommand<String> CommentAnswer { get; private set; }
        public RelayCommand<Comment> VoteUpComment { get; private set; }

        public RelayCommand<String> ReplyComment { get; private set; }
        public RelayCommand RepotCommand { get; private set; }

        public RelayCommand NavToQuestion { get; private set; }
        public RelayCommand NavToComment { get; private set; }
        public RelayCommand NavToCreateCollection { get; private set; }

        public RelayCommand ShareViaWeChat { get; private set; }
        
        private AnswerViewModel()
        {            
            GetDetail = new RelayCommand(GetAnswerDetail);
            GetRelation = new RelayCommand(GetAnswerRelation);
            CheckFavorite = new RelayCommand(CheckFavoriteMethod);

            AnswerNoHelpTapped = new RelayCommand(AnswerNoHelpMethod);
            AnswerThankTapped = new RelayCommand(AnswerThankMethod);
            AnswerCollectTapped = new RelayCommand(AnswerCollectMethod);

            NavToShareCommand = new RelayCommand(NavToShareCommandMethod);
            GetShareTemplate = new RelayCommand(GetShareTemplateMethod);
            ShareViaQq = new RelayCommand(ShareViaQqMethod);
            ShareViaWeibo = new RelayCommand(ShareViaWeiboMethod);
            OpenWithEdge = new RelayCommand(OpenWithEdgeMethod);

            VoteUpTapped = new RelayCommand(VoteUpTappedMethod);
            VoteDownTapped = new RelayCommand(VoteDownTappedMethod);

            CommentAnswer = new RelayCommand<String>(CreateCommentMethod);

            CreateCollection = new RelayCommand(CreateCollectionMethod);
            
            ReportJunkComment = new RelayCommand<Comment>(ReportJunkCommentMethod);
            ReportUnFridenlyComment = new RelayCommand<Comment>(ReportUnFridenlyCommentMethod);
            ReportIllegalComment = new RelayCommand<Comment>(ReportIllegalCommentMethod);
            ReportPoliticalComment = new RelayCommand<Comment>(ReportPoliticalCommentMethod);

            VoteUpComment = new RelayCommand<Comment>(VoteUpCommentMethod);
            ReplyComment = new RelayCommand<String>(ReplyCommentMethod);

            NavToQuestion = new RelayCommand(NavToQuestionMethod);
            NavToComment = new RelayCommand(NavToCommentMethod);
            NavToCreateCollection = new RelayCommand(NavToCreateCollectionMethod);

            ShareViaWeChat = new RelayCommand(ShareViaWeChatMethod);
        }

        public AnswerViewModel(IAnswer answerService, IComment commentService, ICollection collection,
            ISocial socialService) : this()
        {
            _answerService = answerService;
            _commentService = commentService;
            _collection = collection;
            _social = socialService;
        }

        public void Setup(Int32 answerId)
        {
            //if (AnswerId > 0)
            //{
            //    VmHelper.SaveStates(this, AnswerId.ToString());
            //}

            AnswerId = answerId;

            //VmHelper.ResumeStates(this, AnswerId.ToString());

            if (LoginUser.Current.Profile == null) return;

            _isDetailLoaded = false;
            AnswerId = answerId;
            Detail = null;
            Relationship = new AnswerRelationship();

            Comments = new IncrementalLoading<Comment>(GetAnswerCommentsMethod,
                String.Format("/answers/{0}/comments", AnswerId), "offset=0&limit=20", false);

            Collections = new IncrementalLoading<Collection>(GetPersonCollectionsMethod,
                String.Format("/people/{0}/collections", LoginUser.Current.Profile.Id), "limit=20&offset=0", false);
        }
        
        private async void GetAnswerDetail()
        {
            if (null == _answerService || _isDetailLoaded) return;

            Loading = true;

            var result = await _answerService.GetAnswerDetailAsync(LoginUser.Current.Token, AnswerId, true);

            Loading = false;

            if (null != result.Error)
            {
                ToasteIndicator.Instance.Show(String.Empty, result.Error.Message, null, 3);

                Debug.WriteLine(Regex.Unescape(result.Error.Message));
                return;
            }

            _isDetailLoaded = true;

            if (result.Result.SuggestEdit != null && result.Result.SuggestEdit.Status &&
                String.IsNullOrEmpty(result.Result.Content))
            {
                result.Result.Content = result.Result.SuggestEdit.Reason;
            }

            Detail = result.Result;
        }

        private async void GetAnswerRelation()
        {
            if (null == _answerService) return;

            RelationLoading = true;

            var result =
                await _answerService.GetAnswerRelationshipAsync(LoginUser.Current.Token, AnswerId, true);

            RelationLoading = false;

            if (null != result.Error)
            {
                ToasteIndicator.Instance.Show(String.Empty, result.Error.Message, null, 3);

                Debug.WriteLine(Regex.Unescape(result.Error.Message));
                return;
            }

            Relationship.Voting = result.Result.Voting;

            Relationship.IsNotHelp = result.Result.IsNotHelp;
            Relationship.IsThanked = result.Result.IsThanked;
            Relationship.IsFavorited = result.Result.IsFavorited;
        }

        private async void CheckFavoriteMethod()
        {
            if (null == _answerService) return;

            RelationLoading = true;

            var result =
                await _answerService.CheckFavoriteAsync(LoginUser.Current.Token, AnswerId);

            RelationLoading = false;

            if (null != result.Error)
            {
                ToasteIndicator.Instance.Show(String.Empty, result.Error.Message, null, 3);

                Debug.WriteLine(Regex.Unescape(result.Error.Message));
                return;
            }

            Relationship.IsFavorited = result.Result.IsFavorited;
        }

        private async Task<ListResultBase> GetAnswerCommentsMethod(String requestUri)
        {
            if (CommentsLoading) return null;

            if (null == _answerService) return null;

            CommentsLoading = true;

            var result = await _answerService.GetCommentsAsync(LoginUser.Current.Token, requestUri, true);

            CommentsLoading = false;

            if (null != result.Error)
            {
                ToasteIndicator.Instance.Show(String.Empty, result.Error.Message, null, 3);

                Debug.WriteLine(Regex.Unescape(result.Error.Message));
                return null;
            }

            return result;
        }

        private async Task<ListResultBase> GetPersonCollectionsMethod(String request)
        {
            if (CollLoading) return null;

            if (null == _answerService) return null;

            CollLoading = true;

            var result = await _collection.GetPersonCollections(LoginUser.Current.Token, request, true);

            CollLoading = false;

            if (null != result.Error)
            {
                ToasteIndicator.Instance.Show(String.Empty, result.Error.Message, null, 3);

                Debug.WriteLine(Regex.Unescape(result.Error.Message));
                return null;
            }

            var result1 =
                await _collection.GetAnswerCollections(LoginUser.Current.Token, AnswerId, Int32.MaxValue, true);

            if (null != result1.Error)
            {
                ToasteIndicator.Instance.Show(String.Empty, result1.Error.Message, null, 3);

                Debug.WriteLine(Regex.Unescape(result1.Error.Message));

                return null;
            }

            foreach (var item in result1.Result.GetItems())
            {
                foreach (object coll in Collections)
                {
                    if (coll is Collection && item is Collection && (coll as Collection).Id == (item as Collection).Id)
                    {
                        (coll as Collection).IsFavorited = (item as Collection).IsFavorited;
                    }
                }
            }

            return result;
        }


        private void NavToShareCommandMethod()
        {
#if WINDOWS_PHONE_APP
            if (_navigation != null) _navigation.NavigateTo(typeof (AnswerSharePage));
#endif
        }

        private async void GetShareTemplateMethod()
        {
            if (null == _social) return;

            ShareTemplateLoading = true;

            var result = await _social.GetAnswerShareTemplate(LoginUser.Current.Token, AnswerId, true);

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
                ShareTemplate = String.Format("{0}{1} (分享自@知乎)", Detail.Excerpt, result.Result.Templates.ShortUrl);
            }
        }

        private void ShareViaQqMethod()
        {

        }

        private async void ShareViaWeiboMethod()
        {
            if (null == _social) return;

            #region Checking Network

            if (false == Utility.Instance.IsNetworkAvailable)
            {
                ToasteIndicator.Instance.Show(String.Empty, "网络连接已中断", null, 3);

                return;
            }

            #endregion

            ShareTemplateLoading = true;

            var result = await _social.ShareAnswerViaSina(LoginUser.Current.Token, AnswerId, ShareTemplate);

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
            if (null == _social || this.Detail == null || this.Detail.Question == null) return;

            #region Checking Network

            if (false == Utility.Instance.IsNetworkAvailable)
            {
                ToasteIndicator.Instance.Show(String.Empty, "网络连接已中断", null, 3);

                return;
            }

            #endregion

            var result = await _social.ShareViaWeChat(Utility.Instance.WeChatAppId,
                this.Detail.Question.Title,
                this.Detail.Excerpt,
                String.Format("http://www.zhihu.com/question/{0}/answer/{1}", this.Detail.Question.Id, this.Detail.Id));
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

            if (Relationship.Voting == 1)
            {
                #region Cancel Voteup

                Loading = true;

                var result =
                    await
                        _answerService.CancelVoteAsync(LoginUser.Current.Token, AnswerId, LoginUser.Current.Profile.Id);

                Loading = false;

                if (null != result.Error)
                {
                    ToasteIndicator.Instance.Show(String.Empty, result.Error.Message, null, 3);

                    Debug.WriteLine(Regex.Unescape(result.Error.Message));
                    return;
                }

                Relationship.Voting = 0;
                Detail.VoteupCount = result.Result.VoteupCount;

                #endregion
            }
            else
            {
                #region Voteup

                Loading = true;

                var result = await _answerService.VoteUpAsync(LoginUser.Current.Token, AnswerId);

                Loading = false;

                if (null != result.Error)
                {
                    ToasteIndicator.Instance.Show(String.Empty, result.Error.Message, null, 3);

                    Debug.WriteLine(Regex.Unescape(result.Error.Message));
                    return;
                }

                Relationship.Voting = 1;
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

            if (Relationship.Voting == -1)
            {
                #region Cancel Voteup

                Loading = true;

                var result =
                    await
                        _answerService.CancelVoteAsync(LoginUser.Current.Token, AnswerId, LoginUser.Current.Profile.Id);

                Loading = false;

                if (null != result.Error)
                {
                    ToasteIndicator.Instance.Show(String.Empty, result.Error.Message, null, 3);

                    Debug.WriteLine(Regex.Unescape(result.Error.Message));
                    return;
                }

                Relationship.Voting = 0;
                Detail.VoteupCount = result.Result.VoteupCount;

                #endregion
            }
            else
            {
                #region Votedown

                Loading = true;

                var result = await _answerService.VoteDownAsync(LoginUser.Current.Token, AnswerId);

                Loading = false;

                if (null != result.Error)
                {
                    ToasteIndicator.Instance.Show(String.Empty, result.Error.Message, null, 3);

                    Debug.WriteLine(Regex.Unescape(result.Error.Message));
                    return;
                }

                Relationship.Voting = -1;
                Detail.VoteupCount = result.Result.VoteupCount;

                #endregion
            }
        }

        private async void AnswerNoHelpMethod()
        {
            if (null == _answerService || Detail == null || Relationship == null)
                return;

            #region Checking Network

            if (false == Utility.Instance.IsNetworkAvailable)
            {
                ToasteIndicator.Instance.Show(String.Empty, "网络连接已中断", null, 3);

                return;
            }

            #endregion

            var answerId = Detail.Id;

            var result = Relationship.IsNotHelp == false
                ? await _answerService.NoHelpAsync(LoginUser.Current.Token, answerId)
                : await _answerService.UndoNoHelpAsync(LoginUser.Current.Token, answerId, LoginUser.Current.Profile.Id);

            if (null != result.Error)
            {
                ToasteIndicator.Instance.Show(String.Empty, result.Error.Message, null, 3);

                Debug.WriteLine(Regex.Unescape(result.Error.Message));
                return;
            }

            Relationship.IsNotHelp = result.Result.IsNotHelp;
        }

        private async void AnswerThankMethod()
        {
            if (null == _answerService || Detail == null || Relationship == null)
                return;

            #region Checking Network

            if (false == Utility.Instance.IsNetworkAvailable)
            {
                ToasteIndicator.Instance.Show(String.Empty, "网络连接已中断", null, 3);

                return;
            }

            #endregion

            var answerId = Detail.Id;

            var result = Relationship.IsThanked == false
                ? await _answerService.ThankAsync(LoginUser.Current.Token, answerId)
                : await _answerService.CancelThankAsync(LoginUser.Current.Token, answerId, LoginUser.Current.Profile.Id);

            if (result == null) return;

            if (null != result.Error)
            {
                ToasteIndicator.Instance.Show(String.Empty, result.Error.Message, null, 3);

                Debug.WriteLine(Regex.Unescape(result.Error.Message));
                return;
            }

            Relationship.IsThanked = result.Result.IsThanked;
        }

        private async void AnswerCollectMethod()
        {
            #region Checking Network

            if (false == Utility.Instance.IsNetworkAvailable)
            {
                ToasteIndicator.Instance.Show(String.Empty, "网络连接已中断", null, 3);

                return;
            }

            #endregion

            RelationLoading = true;

            var ids = (from object coll in Collections
                where coll is Collection && (coll as Collection).IsFavorited
                select (coll as Collection).Id).ToList();

            var result = await _collection.CollectionAnswer(LoginUser.Current.Token, AnswerId, ids);

            if (null != result.Error)
            {
                ToasteIndicator.Instance.Show(String.Empty, result.Error.Message, null, 3);

                Debug.WriteLine(Regex.Unescape(result.Error.Message));

                RelationLoading = false;

                return;
            }

            ToasteIndicator.Instance.Show(String.Empty, ids.Count > 0 ? "已加入收藏" : "已取消收藏", null, 3);
            //GetAnswerRelation();
            CheckFavoriteMethod();

            RelationLoading = false;
        }

        private async void CreateCommentMethod(String comment)
        {
            #region Checking Network

            if (false == Utility.Instance.IsNetworkAvailable)
            {
                ToasteIndicator.Instance.Show(String.Empty, "网络连接已中断", null, 3);

                return;
            }

            #endregion

            CommentSending = true;

            var result = await _commentService.CommentAnswerAsync(LoginUser.Current.Token, AnswerId, comment);

            CommentSending = false;

            if (null != result.Error)
            {
                ToasteIndicator.Instance.Show(String.Empty, result.Error.Message, null, 3);

                Debug.WriteLine(Regex.Unescape(result.Error.Message));
                return;
            }
            
            Detail.CommentCount += 1;

            ToasteIndicator.Instance.Show(String.Empty, "评论已发出", null, 3);

            Comments = new IncrementalLoading<Comment>(GetAnswerCommentsMethod,
                String.Format("/answers/{0}/comments", AnswerId), "offset=0&limit=20", false);
            
#if Windows_App
            if (_navigation != null) _navigation.GoBack();
#endif
        }

        private async void CreateCollectionMethod()
        {
            #region Checking Network

            if (false == Utility.Instance.IsNetworkAvailable)
            {
                ToasteIndicator.Instance.Show(String.Empty, "网络连接已中断", null, 3);

                return;
            }

            #endregion

            var result = await _collection.CreateAsync(LoginUser.Current.Token, !IsCollePrivate, ColleTitle, ColleDes);

            if (null != result.Error)
            {
                ToasteIndicator.Instance.Show(String.Empty, result.Error.Message, null, 3);

                Debug.WriteLine(Regex.Unescape(result.Error.Message));
                return;
            }

            IsCollePrivate = false;
            ColleTitle = ColleDes = String.Empty;

            Collections = new IncrementalLoading<Collection>(GetPersonCollectionsMethod,
                String.Format("/people/{0}/collections", LoginUser.Current.Profile.Id), "limit=20&offset=0", false);
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

                var result =
                    await
                        _commentService.CancelVoteUp(LoginUser.Current.Token, comment.Id, LoginUser.Current.Profile.Id);

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
                    _commentService.ReplyAnswerComment(LoginUser.Current.Token, this.Detail.Id, CurrentComment.Id,
                        comment);

            if (null != result.Error)
            {
                ToasteIndicator.Instance.Show(String.Empty, result.Error.Message, null, 3);

                Debug.WriteLine(Regex.Unescape(result.Error.Message));
                return;
            }

            ToasteIndicator.Instance.Show(String.Empty, "回复已发出", null, 3);

            Detail.CommentCount += 1;
            
            Comments = new IncrementalLoading<Comment>(GetAnswerCommentsMethod,
                String.Format("/answers/{0}/comments", AnswerId), "offset=0&limit=20", false);
        }

        private void NavToQuestionMethod()
        {
            if (Detail == null || Detail.Question == null) return;

            NavHelper.NavToQuestionPage(Detail.Question.Id, AppShellPage.AppFrame);
        }

        private void NavToCommentMethod()
        {
            //if (_navigation != null) _navigation.NavigateTo(typeof(CommentAnswerPage));
        }

        private void NavToCreateCollectionMethod()
        {
            //if (_navigation != null) _navigation.NavigateTo(typeof(CreateCollectionPage));
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

            var result =
                await _commentService.ReportComment(LoginUser.Current.Token, comment.Id, ReportReason.UnFridendly);

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

            var result =
                await _commentService.ReportComment(LoginUser.Current.Token, comment.Id, ReportReason.Political);

            if (null != result.Error)
            {
                ToasteIndicator.Instance.Show(String.Empty, result.Error.Message, null, 3);

                Debug.WriteLine(Regex.Unescape(result.Error.Message));
                return;
            }
            ToasteIndicator.Instance.Show(String.Empty, "已举报", null, 3);
        }

        private async void OpenWithEdgeMethod()
        {
            var uri = new Uri(String.Format("http://www.zhihu.com/question/{0}/answer/{1}", this.Detail.Question.Id, this.Detail.Id));
            await Launcher.LaunchUriAsync(uri);
        }
    }
}