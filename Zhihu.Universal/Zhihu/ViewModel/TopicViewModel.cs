using System;
using System.Diagnostics;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;

using Zhihu.Common.DataSource;
using Zhihu.Common.Helper;
using Zhihu.Common.Model;
using Zhihu.Common.Model.Result;
using Zhihu.Common.Service;
using Zhihu.Controls;

namespace Zhihu.ViewModel
{
    public sealed class TopicViewModel : ViewModelBase
    {
        private readonly INavigate _navigation;
        private readonly ITopic _topic;

        [Data] private Int32 _topicId = 0;

        [Data] private Boolean _isLoaded = false;

        private Topic _detail;

        [Data]
        public Topic Detail
        {
            get { return _detail; }
            private set
            {
                _detail = value;
                RaisePropertyChanged(() => Detail);
            }
        }

        private Following _following;

        [Data]
        public Following Following
        {
            get { return _following; }
            set
            {
                _following = value;
                RaisePropertyChanged(() => Following);
            }
        }

        private const String FirstOffset = "limit=20&offset=0";


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

        private Boolean _questionsloading = false;

        public Boolean QuestionsLoading
        {
            get { return _questionsloading; }
            private set
            {
                _questionsloading = value;
                RaisePropertyChanged(() => QuestionsLoading);
            }
        }


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

        private IncrementalLoading<TopicActivity> _activities;

        [Data]
        public IncrementalLoading<TopicActivity> Activities
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

        #region Best Answers

        private Boolean _answersloading = false;

        public Boolean AnswersLoading
        {
            get { return _answersloading; }
            private set
            {
                _answersloading = value;
                RaisePropertyChanged(() => AnswersLoading);
            }
        }

        private IncrementalLoading<Answer> _bestAnswers;

        [Data]
        public IncrementalLoading<Answer> BestAnswers
        {
            get { return _bestAnswers; }
            private set
            {
                _bestAnswers = value;
                RaisePropertyChanged(() => BestAnswers);
            }
        }

        #endregion

        #region Questions

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

        public RelayCommand GetDetail { get; private set; }
        public RelayCommand GetFollowing { get; private set; }
        public RelayCommand FollowUnFollow { get; private set; }

        #region Activity Tapped
                
        #endregion
        
        public RelayCommand<Answer> NavToAnswerCommand { get; private set; }

        private TopicViewModel()
        {
        }

        public TopicViewModel(ITopic topic, INavigate naviagation)
            : this()
        {
            this._topic = topic;
            this._navigation = naviagation;

            GetDetail = new RelayCommand(GetDetailMethod);
            GetFollowing = new RelayCommand(GetFollowingMethod);
            FollowUnFollow = new RelayCommand(FollowUnFollowMethod);
       
            NavToAnswerCommand = new RelayCommand<Answer>(NavToAnswerMethod);
        }

        public void Setup(Int32 topicId)
        {
            //if (_topicId < 0)
            //{
            //    VmHelper.SaveStates(this, _topicId.ToString());
            //}

            _topicId = topicId;

            //VmHelper.ResumeStates(this, _topicId.ToString());

            _isLoaded = false;
            Detail = null;
            BestAnswers = null;
            Following = new Following() {IsFollowing = false};

            Questions = new IncrementalLoading<Question>(GetMoreQuestions,
                String.Format("topics/{0}/unanswered_questions", _topicId), FirstOffset, false);
            
            BestAnswers = new IncrementalLoading<Answer>(GetMoreBestAnswers,
                String.Format("topics/{0}/best_answers", _topicId), FirstOffset, false);

            Activities = new IncrementalLoading<TopicActivity>(GetMoreActivities,
                String.Format("topics/{0}/activities_new", topicId), "limit=20&after_id=", false);
        }
        
        private async void GetDetailMethod()
        {
            if (_isLoaded || null == _topic) return;

            Loading = true;

            var result = await _topic.GetDetailAsync(LoginUser.Current.Token, _topicId, true);

            Loading = false;

            if (null != result.Error)
            {
                ToasteIndicator.Instance.Show(String.Empty, result.Error.Message, null, 3);

                Debug.WriteLine(Regex.Unescape(result.Error.Message));
                return;
            }

            _isLoaded = true;
            Detail = result.Result;
        }

        private async void GetFollowingMethod()
        {
            if (null == _topic) return;

            var result = await _topic.CheckFollowingAsync(LoginUser.Current.Token, _topicId);

            if (null != result.Error)
            {
                ToasteIndicator.Instance.Show(String.Empty, result.Error.Message, null, 3);

                Debug.WriteLine(Regex.Unescape(result.Error.Message));
                return;
            }

            Following = result.Result;
        }

        private void FollowUnFollowMethod()
        {
            if (Following.IsFollowing)
            {
                UnFollowMethod();
            }
            else
            {
                FollowMethod();
            }
        }

        private async void FollowMethod()
        {
            #region Checking Network

            if (false == Utility.Instance.IsNetworkAvailable)
            {
                ToasteIndicator.Instance.Show(String.Empty, "网络连接已中断", null, 3);
              
                return;
            }

            #endregion

            if (null == _topic) return;

            var result = await _topic.FollowAsync(LoginUser.Current.Token, _topicId);

            if (null != result.Error)
            {
                ToasteIndicator.Instance.Show(String.Empty, result.Error.Message, null, 3);

                Debug.WriteLine(Regex.Unescape(result.Error.Message));
                return;
            }

            Following = result.Result;
        }

        private async void UnFollowMethod()
        {
            #region Checking Network

            if (false == Utility.Instance.IsNetworkAvailable)
            {
                ToasteIndicator.Instance.Show(String.Empty, "网络连接已中断", null, 3);
               
                return;
            }

            #endregion

            if (null == _topic) return;

            var result = await _topic.UnFollowAsync(LoginUser.Current.Token, _topicId, LoginUser.Current.Profile.Id);

            if (null != result.Error)
            {
                ToasteIndicator.Instance.Show(String.Empty, result.Error.Message, null, 3);

                Debug.WriteLine(Regex.Unescape(result.Error.Message));
                return;
            }

            if (result.Result.IsFollowing == false)
            {
                Following = new Following() {IsFollowing = false};
            }
        }

        private async Task<ListResultBase> GetMoreQuestions(String request)
        {
            if (QuestionsLoading) return null;

            QuestionsLoading = true;

            var result = await _topic.GetQuestionsAsync(LoginUser.Current.Token, request, true);

            if (result.Result == null)
            {
                ToasteIndicator.Instance.Show(String.Empty, result.Error.Message, null, 3);
            }

            QuestionsLoading = false;

            return result;
        }

        private async Task<ListResultBase> GetMoreBestAnswers(String request)
        {
            if (AnswersLoading) return null;

            AnswersLoading = true;

            var result = await _topic.GetBestAnswersAsync(LoginUser.Current.Token, request, true);

            if (result.Result == null)
            {
                ToasteIndicator.Instance.Show(String.Empty, result.Error.Message, null, 3);
            }

            AnswersLoading = false;

            return result;
        }

        private async Task<ListResultBase> GetMoreActivities(String request)
        {
            if (ActivitiesLoading) return null;

            ActivitiesLoading = true;

            var result = await _topic.GetActivitiesAsync(LoginUser.Current.Token, request, true);

            ActivitiesLoading = false;

            if (result.Result == null)
            {
                ToasteIndicator.Instance.Show(String.Empty, result.Error.Message, null, 3);

                return null;
            }

            return result;
        }

        private void NavToAnswerMethod(Answer answer)
        {
            //VmNavHelper.NavToAnswerPage(answer.Id, _navigation);
        }
    }
}
