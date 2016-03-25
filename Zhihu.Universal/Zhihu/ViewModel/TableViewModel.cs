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
    public sealed class TableViewModel : ViewModelBase
    {
        private readonly ITable _table;

        [Data] private String _tableId = String.Empty;
        [Data] private Boolean _detailLoaded = false;

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

        private RoundTable _detail;

        [Data]
        public RoundTable Detail
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

        private Boolean _activitiesLoading = false;

        public Boolean ActivitiesLoading
        {
            get { return _activitiesLoading; }
            private set
            {
                _activitiesLoading = value;
                RaisePropertyChanged(() => ActivitiesLoading);
            }
        }

        private IncrementalLoading<TableActivity> _activities;

        public IncrementalLoading<TableActivity> Activities
        {
            get { return _activities; }
            private set
            {
                if (_activities == value) return;
                _activities = value;
                RaisePropertyChanged(() => Activities);
            }
        }


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

        private IncrementalLoading<TableQuestion> _questions;

        public IncrementalLoading<TableQuestion> Questions
        {
            get { return _questions; }
            private set
            {
                if (_questions == value) return;
                _questions = value;
                RaisePropertyChanged(() => Questions);
            }
        }

        public RelayCommand GetDetail { get; private set; }

        public RelayCommand FollowUnfollowQuestion { get; private set; }


        private TableViewModel()
        {
            GetDetail = new RelayCommand(GetDetailMethod);
            FollowUnfollowQuestion = new RelayCommand(FollowUnfollowMethod);
        }

        public TableViewModel(ITable tableSerivce) : this()
        {
            this._table = tableSerivce;
        }

        public void Setup(String tableId)
        {
            //if (false == String.IsNullOrEmpty(this._tableId))
            //{
            //    VmHelper.SaveStates(this, _tableId.ToString());
            //}

            _tableId = tableId;

            //VmHelper.ResumeStates(this, _tableId.ToString());

            Loading = ActivitiesLoading = CommentsLoading = QuestionsLoading = false;

            Activities = new IncrementalLoading<TableActivity>(GetMoreActivitiesMethod,
                String.Format("/roundtables/{0}/activities", _tableId), "limit=20&offset=0", false);
            Comments = new IncrementalLoading<Comment>(GetMoreCommentsMethod,
                String.Format("/roundtables/{0}/comments", _tableId), "limit=20&offset=0", false);
            Questions = new IncrementalLoading<TableQuestion>(GetMoreQuestionsMethod,
                String.Format("/roundtables/{0}/questions", _tableId), "limit=20&offset=0", false);
        }
        
        private async void GetDetailMethod()
        {
            if (null == _table) return;

            if (_detailLoaded) return;

            Loading = true;

            var result = await _table.GetDetailAsync(LoginUser.Current.Token, _tableId, true);

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

        private async void FollowUnfollowMethod()
        {
            #region Checking Network

            if (false == Utility.Instance.IsNetworkAvailable)
            {
                ToasteIndicator.Instance.Show(String.Empty, "网络连接已中断", null, 3);

                return;
            }

            #endregion

            if (null == _table || Detail == null) return;

            var questionId = Detail.Id;

            var result = Detail.IsFollowing == false
                ? await _table.FollowAsync(LoginUser.Current.Token, questionId)
                : await _table.UnFollowAsync(LoginUser.Current.Token, questionId, LoginUser.Current.Profile.Id);

            if (null != result.Error)
            {
                ToasteIndicator.Instance.Show(String.Empty, result.Error.Message, null, 3);

                Debug.WriteLine(Regex.Unescape(result.Error.Message));

                return;
            }

            if (result.Result.IsFollowing)
            {
                Detail.Followers++;
            }
            else
            {
                Detail.Followers--;
            }

            Detail.IsFollowing = !Detail.IsFollowing;
        }

        private async Task<ListResultBase> GetMoreCommentsMethod(String requestUri)
        {
            if (null == _table || CommentsLoading) return null;

            CommentsLoading = true;

            var result = await _table.GetCommentsAsync(LoginUser.Current.Token, requestUri, true);

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

        private async Task<ListResultBase> GetMoreQuestionsMethod(String requestUri)
        {
            if (null == _table || QuestionsLoading) return null;

            QuestionsLoading = true;

            var result = await _table.GetQuestionsAsync(LoginUser.Current.Token, requestUri, true);

            QuestionsLoading = false;

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

        private async Task<ListResultBase> GetMoreActivitiesMethod(String requestUri)
        {
            if (null == _table || ActivitiesLoading) return null;

            ActivitiesLoading = true;

            var result = await _table.GetActivtiesAsync(LoginUser.Current.Token, requestUri, true);

            ActivitiesLoading = false;

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
    }
}
