using System;
using System.Diagnostics;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

using Windows.UI.Xaml;

using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;

using Zhihu.Common.DataSource;
using Zhihu.Common.Helper;
using Zhihu.Common.Model;
using Zhihu.Common.Model.Result;
using Zhihu.Common.Service;

using Zhihu.Helper;
using Zhihu.Controls;

namespace Zhihu.ViewModel
{
    public sealed class ProfileViewModel : ViewModelBase
    {
        private readonly IPerson _people;

        private const String FirstActivitiesOffset = "limit=20&after_id=";
        private const String FirstOffset = "limit=20&offset=0";

        [Data] private String _userId = String.Empty;

        [Data] private Boolean _isLoaded = false;

        #region Profile

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

        private Profile _profile;

        [Data]
        public Profile Profile
        {
            get { return _profile; }
            private set
            {
                _profile = value;
                RaisePropertyChanged(() => Profile);
            }
        }

        private PeopleFollowing _following;

        [Data]
        public PeopleFollowing Following
        {
            get { return _following; }
            private set
            {
                _following = value;
                RaisePropertyChanged(() => Following);
            }
        }

        #endregion

        #region Acitivies

        private Boolean _activitiesLoading = false;

        [Data]
        public Boolean ActivitiesLoading
        {
            get { return _activitiesLoading; }
            private set
            {
                _activitiesLoading = value;
                RaisePropertyChanged(() => ActivitiesLoading);
            }
        }

        private IncrementalLoading<Activity> _activities;

        [Data]
        public IncrementalLoading<Activity> Activities
        {
            get { return _activities; }
            private set
            {
                if (_activities == value) return;
                _activities = value;

                RaisePropertyChanged(() => Activities);
            }
        }

        #endregion

        #region Answers By Created

        private Boolean _byCreatedLoading = false;

        public Boolean ByCreatedLoading
        {
            get { return _byCreatedLoading; }
            private set
            {
                _byCreatedLoading = value;
                RaisePropertyChanged(() => ByCreatedLoading);
            }
        }

        private IncrementalLoading<Answer> _answersByCreated;

        [Data]
        public IncrementalLoading<Answer> AnswersByCreated
        {
            get { return _answersByCreated; }
            private set
            {
                _answersByCreated = value;
                RaisePropertyChanged(() => AnswersByCreated);
            }
        }

        #endregion

        #region Answers By Vote

        private Boolean _byVoteLoading = false;

        public Boolean ByVoteLoading
        {
            get { return _byVoteLoading; }
            private set
            {
                _byVoteLoading = value;
                RaisePropertyChanged(() => ByVoteLoading);
            }
        }

        private IncrementalLoading<Answer> _answersByVoteNum;

        [Data]
        public IncrementalLoading<Answer> AnswersByVoteNum
        {
            get { return _answersByVoteNum; }
            private set
            {
                _answersByVoteNum = value;
                RaisePropertyChanged(() => AnswersByVoteNum);
            }
        }

        #endregion

        #region Questions

        private Boolean _questionsLoading = false;

        public Boolean QuestionsLoading
        {
            get { return _questionsLoading; }
            private set
            {
                _questionsLoading = value;
                RaisePropertyChanged(() => QuestionsLoading);
            }
        }

        private IncrementalLoading<Question> _questions;

        [Data]
        public IncrementalLoading<Question> Questions
        {
            get { return _questions; }
            private set
            {
                _questions = value;
                RaisePropertyChanged(() => Questions);
            }
        }

        #endregion

        #region Collections

        private Boolean _collectionsLoading = false;

        public Boolean CollectionsLoading
        {
            get { return _collectionsLoading; }
            private set
            {
                _collectionsLoading = value;
                RaisePropertyChanged(() => CollectionsLoading);
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

        #endregion

        #region Following Questions

        private IncrementalLoading<Question> _followingQues;

        [Data]
        public IncrementalLoading<Question> FollowingQues
        {
            get { return _followingQues; }
            private set
            {
                _followingQues = value;
                RaisePropertyChanged(() => FollowingQues);
            }
        }

        #endregion

        #region Following Collections

        private IncrementalLoading<Collection> _followingColles;

        [Data]
        public IncrementalLoading<Collection> FollowingColles
        {
            get { return _followingColles; }
            private set
            {
                _followingColles = value;
                RaisePropertyChanged(() => FollowingColles);
            }
        }

        #endregion

        #region Columns Following

        private Boolean _columnsLoading = false;

        public Boolean ColumnsLoading
        {
            get { return _columnsLoading; }
            private set
            {
                _columnsLoading = value;
                RaisePropertyChanged(() => ColumnsLoading);
            }
        }

        private IncrementalLoading<Column> _columnsFollowing;

        [Data]
        public IncrementalLoading<Column> ColumnsFollowing
        {
            get { return _columnsFollowing; }
            private set
            {
                _columnsFollowing = value;
                RaisePropertyChanged(() => ColumnsFollowing);
            }
        }

        #endregion

        #region Columns

        private IncrementalLoading<Column> _columns;

        [Data]
        public IncrementalLoading<Column> Columns
        {
            get { return _columns; }
            private set
            {
                _columns = value;
                RaisePropertyChanged(() => Columns);
            }
        }

        #endregion

        #region Following Topics

        private Boolean _topicsLoading = false;

        public Boolean TopicsLoading
        {
            get { return _topicsLoading; }
            private set
            {
                _topicsLoading = value;
                RaisePropertyChanged(() => TopicsLoading);
            }
        }


        private IncrementalLoading<Topic> _followingTopics;

        [Data]
        public IncrementalLoading<Topic> FollowingTopics
        {
            get { return _followingTopics; }
            private set
            {
                _followingTopics = value;
                RaisePropertyChanged(() => FollowingTopics);
            }
        }

        #endregion

        #region Followees
       
        private Boolean _followeesLoading = false;

        public Boolean FolloweesLoading
        {
            get { return _followeesLoading; }
            private set
            {
                _followeesLoading = value;
                RaisePropertyChanged(() => FolloweesLoading);
            }
        }


        private IncrementalLoading<Profile> _followees;

        [Data]
        public IncrementalLoading<Profile> Followees
        {
            get { return _followees; }
            private set
            {
                _followees = value;
                RaisePropertyChanged(() => Followees);
            }
        }

        #endregion

        #region Followers

        private Boolean _followersLoading = false;

        public Boolean FollowersLoading
        {
            get { return _followersLoading; }
            private set
            {
                _followersLoading = value;
                RaisePropertyChanged(() => FollowersLoading);
            }
        }


        private IncrementalLoading<Profile> _followers;
        [Data]
        public IncrementalLoading<Profile> Followers
        {
            get { return _followers; }
            private set
            {
                _followers = value;
                RaisePropertyChanged(() => Followers);
            }
        }

        #endregion

        #region Visibilities

        private Visibility _selfVisibility = Visibility.Collapsed;

        [Data]
        public Visibility SelfVisibility
        {
            get { return _selfVisibility; }
            private set
            {
                _selfVisibility = value;
                RaisePropertyChanged(() => SelfVisibility);
            }
        }

        private Visibility _businessVisibility = Visibility.Collapsed;

        [Data]
        public Visibility BusinessVisibility
        {
            get { return _businessVisibility; }
            private set
            {
                _businessVisibility = value;
                RaisePropertyChanged(() => BusinessVisibility);
            }
        }

        private Visibility _locationVisibility = Visibility.Collapsed;

        [Data]
        public Visibility LocationVisibility
        {
            get { return _locationVisibility; }
            private set
            {
                _locationVisibility = value;
                RaisePropertyChanged(() => LocationVisibility);
            }
        }

        private Visibility _employmentVisibility = Visibility.Collapsed;

        [Data]
        public Visibility EmploymentVisibility
        {
            get { return _employmentVisibility; }
            private set
            {
                _employmentVisibility = value;
                RaisePropertyChanged(() => EmploymentVisibility);
            }
        }

        private Visibility _educationVisibility = Visibility.Collapsed;

        [Data]
        public Visibility EducationVisibility
        {
            get { return _educationVisibility; }
            private set
            {
                _educationVisibility = value;
                RaisePropertyChanged(() => EducationVisibility);
            }
        }

        #endregion

        #region 文案提示

        public String PageTitle
        {
            get { return SelfVisibility == Visibility.Visible ? "用户资料" : "我的资料"; }
        }

        public String QuestionTitle
        {
            get { return String.Format("{0}的提问", Profile == null ? String.Empty : Profile.Name); }
        }

        public String VoteupTip
        {
            get { return SelfVisibility == Visibility.Visible ? "他获得的赞同" : "我获得的赞同"; }
        }

        public String ThankedCountTip
        {
            get { return SelfVisibility == Visibility.Visible ? "他获得的感谢" : "我获得的感谢"; }
        }

        public String AnswerCollectedTip
        {
            get { return "答案被收藏"; }
        }

        public String AnswerSharedTip
        {
            get { return "答案被分享"; }
        }

        public String TopicTip
        {
            get { return SelfVisibility == Visibility.Visible ? "他的话题" : "我的话题"; }
        }

        public String FollowingTip
        {
            get { return SelfVisibility == Visibility.Visible ? "他关注的人" : "我关注的人"; }
        }

        public String FollowerTip
        {
            get { return SelfVisibility == Visibility.Visible ? "关注他的人" : "关注我的人"; }
        }

        public String ColumnsTip
        {
            get { return SelfVisibility == Visibility.Visible ? "他的专栏" : "我的专栏"; }
        }

        public String ColumnsFollowingTip
        {
            get { return SelfVisibility == Visibility.Visible ? "他关注的专栏" : "我关注的专栏"; }
        }

        public String CollectionsTip
        {
            get { return SelfVisibility == Visibility.Visible ? "他的收藏" : "我的收藏"; }
        }

        #endregion

        public RelayCommand GetProfile { get; private set; }
        public RelayCommand CheckFollowing { get; private set; }
        public RelayCommand FollowUnFollow { get; private set; }
        
        public RelayCommand<Int32> NavToTopic { get; private set; }
        public RelayCommand<String> NavToQqWeibo { get; private set; }
        

        private ProfileViewModel()
        {
        }

        public ProfileViewModel(IPerson people)
            : this()
        {
            _people = people;

            GetProfile = new RelayCommand(GetProfileMethod);
            CheckFollowing = new RelayCommand(CheckFollowingMethod);
            FollowUnFollow = new RelayCommand(FollowUnFollowMethod);

            NavToTopic = new RelayCommand<Int32>(NavToTopicMethod);

            NavToQqWeibo = new RelayCommand<String>(NavToQqWeiboMethod);
        }

        public void Setup(String userId)
        {
            //if (false == String.IsNullOrEmpty(_userId))
            //{
            //    VmHelper.SaveStates(this, _userId);
            //}

            if (LoginUser.Current.Profile == null) return;

            _userId = userId;

            //VmHelper.ResumeStates(this, _userId);

            if (String.IsNullOrEmpty(_userId) || "self" == _userId || _userId == LoginUser.Current.Profile.Id)
            {
                SelfVisibility = Visibility.Collapsed;
            }
            else
            {
                SelfVisibility = Visibility.Visible;
            }

            _isLoaded = false;

            Profile = null;

            Activities = new IncrementalLoading<Activity>(GetMoreActivities,
                String.Format("people/{0}/activities", "self" == _userId ? LoginUser.Current.Profile.Id : _userId),
                FirstActivitiesOffset, false);

            AnswersByCreated = new IncrementalLoading<Answer>(GetMoreAnswersByCreated,
                String.Format("people/{0}/answers", "self" == _userId ? LoginUser.Current.Profile.Id : _userId),
                FirstOffset, false);

            AnswersByVoteNum = new IncrementalLoading<Answer>(GetMoreAnswersByVoteNum,
                String.Format("people/{0}/answers", "self" == _userId ? LoginUser.Current.Profile.Id : _userId),
                FirstOffset,
                false);

            Questions = new IncrementalLoading<Question>(GetMoreQuestions,
                String.Format("people/{0}/questions", "self" == _userId ? LoginUser.Current.Profile.Id : _userId),
                FirstOffset, false);

            Collections = new IncrementalLoading<Collection>(GetMoreCollections,
                String.Format("people/{0}/collections", "self" == _userId ? LoginUser.Current.Profile.Id : _userId),
                FirstOffset, false);

            FollowingQues = new IncrementalLoading<Question>(GetMoreQuestions,
                String.Format("people/{0}/following_questions", "self" == _userId ? LoginUser.Current.Profile.Id : _userId),
                FirstOffset, false);

            FollowingColles = new IncrementalLoading<Collection>(GetMoreCollections,
                String.Format("people/{0}/following_collections", "self" == _userId ? LoginUser.Current.Profile.Id : _userId),
                FirstOffset, false);

            Columns = new IncrementalLoading<Column>(GetMoreColumns,
                String.Format("people/{0}/columns", "self" == _userId ? LoginUser.Current.Profile.Id : _userId),
                FirstOffset, false);

            ColumnsFollowing = new IncrementalLoading<Column>(GetMoreColumns,
                String.Format("people/{0}/following_columns", "self" == _userId ? LoginUser.Current.Profile.Id : _userId),
                FirstOffset, false);

            FollowingTopics = new IncrementalLoading<Topic>(GetMoreFollowingTopics,
                String.Format("people/{0}/following_topics", "self" == _userId ? LoginUser.Current.Profile.Id : _userId),
                FirstOffset, false);

            Followees = new IncrementalLoading<Profile>(GetMoreFollowees,
                String.Format("people/{0}/followees", "self" == _userId ? LoginUser.Current.Profile.Id : _userId),
                FirstOffset, false);

            Followers = new IncrementalLoading<Profile>(GetMoreFollowers,
                String.Format("people/{0}/followers", "self" == _userId ? LoginUser.Current.Profile.Id : _userId),
                FirstOffset, false);
        }
        
        private async void GetProfileMethod()
        { 
            if (_isLoaded) return;

            if (null == _people || String.IsNullOrEmpty(LoginUser.Current.Token)) return;

            Loading = true;

            var result = await _people.GetProfileAsync(LoginUser.Current.Token, _userId, true);

            Loading = false;

            if (result == null) return;

            if (null != result.Error)
            {
                Debug.WriteLine(Regex.Unescape(result.Error.Message));
                return;
            }

            if (result.Result == null) return;

            Profile = result.Result;

            RaisePropertyChanged(() => QuestionTitle);

            _isLoaded = true;

            BusinessVisibility = Profile.Business != null && false == String.IsNullOrEmpty(Profile.Business.Name)
                ? Visibility.Visible
                : Visibility.Collapsed;

            LocationVisibility = Profile.Locations != null && Profile.Locations.Any()
                ? Visibility.Visible
                : Visibility.Collapsed;

            EmploymentVisibility = Profile.Employments != null && Profile.Employments.Any()
                ? Visibility.Visible
                : Visibility.Collapsed;

            EducationVisibility = Profile.Education != null && Profile.Education.Any()
                ? Visibility.Visible
                : Visibility.Collapsed;
        }

        private async Task<ListResultBase> GetMoreActivities(String request)
        {
            if (ActivitiesLoading) return null;

            ActivitiesLoading = true;

            var result = await _people.GetActivitiesAsync(LoginUser.Current.Token, request, true);

            ActivitiesLoading = false;
           
            if (result.Result == null)
            {
                ToasteIndicator.Instance.Show(String.Empty, result.Error.Message, null, 3);

                return null;
            }

            return result;
        }

        private async Task<ListResultBase> GetMoreAnswersByCreated(String request)
        {
            if (ByCreatedLoading) return null;

            ByCreatedLoading = true;

            var result = await _people.GetAnswersAsync(LoginUser.Current.Token, request, "created", true);

            ByCreatedLoading = false;

            if (result.Result != null) return result;

            ToasteIndicator.Instance.Show(String.Empty, result.Error.Message, null, 3);

            return null;
        }

        private async Task<ListResultBase> GetMoreAnswersByVoteNum(String request)
        {
            if (ByVoteLoading) return null;

            ByVoteLoading = true;

            var result = await _people.GetAnswersAsync(LoginUser.Current.Token, request, "votenum", true);

            ByVoteLoading = false;

            if (result.Result != null) return result;

            ToasteIndicator.Instance.Show(String.Empty, result.Error.Message, null, 3);

            return null;
        }

        private async Task<ListResultBase> GetMoreQuestions(String request)
        {
            if (QuestionsLoading) return null;

            QuestionsLoading = true;

            var result = await _people.GetQuestionsAsync(LoginUser.Current.Token, request, true);

            QuestionsLoading = false;

            if (result.Result != null) return result;

            ToasteIndicator.Instance.Show(String.Empty, result.Error.Message, null, 3);

            return null;
        }

        private async Task<ListResultBase> GetMoreCollections(String request)
        {
            if (CollectionsLoading) return null;

            CollectionsLoading = true;

            var result = await _people.GetCollectionsAsync(LoginUser.Current.Token, request, true);

            CollectionsLoading = false;

            if (result.Result != null) return result;

            ToasteIndicator.Instance.Show(String.Empty, result.Error.Message, null, 3);

            return null;
        }

        private async Task<ListResultBase> GetMoreColumns(String request)
        {
            if (ColumnsLoading) return null;

            ColumnsLoading = true;

            var result = await _people.GetColumnsAsync(LoginUser.Current.Token, request, true);

            ColumnsLoading = false;

            if (result.Result != null) return result;

            ToasteIndicator.Instance.Show(String.Empty, result.Error.Message, null, 3);

            return null;
        }

        private async Task<ListResultBase> GetMoreFollowingTopics(String request)
        {
            if (TopicsLoading) return null;

            TopicsLoading = true;

            var result = await _people.GetFollowingTopicsAsync(LoginUser.Current.Token, request, true);

            TopicsLoading = false;

            if (result.Result != null) return result;

            ToasteIndicator.Instance.Show(String.Empty, result.Error.Message, null, 3);

            return null;
        }

        private async Task<ListResultBase> GetMoreFollowees(String request)
        {
            if (FolloweesLoading) return null;

            FolloweesLoading = true;

            var result = await _people.GetFolloweesAsync(LoginUser.Current.Token, request, true);

            FolloweesLoading = false;

            if (result.Result != null) return result;

            ToasteIndicator.Instance.Show(String.Empty, result.Error.Message, null, 3);

            return null;
        }

        private async Task<ListResultBase> GetMoreFollowers(String request)
        {
            if (FollowersLoading) return null;

            FollowersLoading = true;

            var result = await _people.GetFollowersAsync(LoginUser.Current.Token, request, true);

            FollowersLoading = false;

            if (result.Result != null) return result;

            ToasteIndicator.Instance.Show(String.Empty, result.Error.Message, null, 3);

            return null;
        }

        private async void CheckFollowingMethod()
        {
            if (String.IsNullOrEmpty(_userId) || "self" == _userId) return;

            var result = await _people.CheckFollowingAsync(LoginUser.Current.Token, _userId, true);

            Loading = false;
           
            if (result == null) return;

            if (null != result.Error)
            {
                Debug.WriteLine(Regex.Unescape(result.Error.Message));
                return;
            }

            Following = result.Result;
        }

        private async void FollowUnFollowMethod()
        {
            #region Checking Network

            if (false == Utility.Instance.IsNetworkAvailable)
            {
                ToasteIndicator.Instance.Show(String.Empty, "网络连接已中断", null, 3);
                
                return;
            }

            #endregion

            if (String.IsNullOrEmpty(_userId) || "self" == _userId) return;

            if (Following.Following)
            {
                var result = await _people.UnFollowAsync(LoginUser.Current.Token, _userId, LoginUser.Current.Profile.Id);

                if (result.Error == null)
                {
                    CheckFollowingMethod();
                }
                return;
            }
            else
            {
                var result = await _people.FollowAsync(LoginUser.Current.Token, _userId);

                if (result.Error == null)
                {
                    CheckFollowingMethod();
                }
                return;
            }
        }
        
        private void NavToTopicMethod(Int32 topicId)
        {
            //VmNavHelper.NavToTopicPage(topicId, _navigate);
        }
                
        
        private void NavToQqWeiboMethod(String qqWeibo)
        {
            //VmNavHelper.ProcessInnerHtml(qqWeibo, _navigate);
        }
                
        private void TopicTappedMethod(Topic topic)
        {
            //VmNavHelper.NavToTopicPage(topic.Id, _navigate);
        }

    }
}