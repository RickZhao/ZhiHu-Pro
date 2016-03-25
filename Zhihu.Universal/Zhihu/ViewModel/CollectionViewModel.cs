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

using Zhihu.Helper;
using Zhihu.Controls;

namespace Zhihu.ViewModel
{
    public sealed class CollectionViewModel : ViewModelBase
    {
        private readonly INavigate _navigation;
        private readonly ICollection _collection;

        [Data] private Int32 _collectionId = 0;
        [Data] private Boolean _isLoaded = false;

        private Collection _detail;

        [Data]
        public Collection Detail
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

        private const String FirstAnswersUri = "limit=20&offset=0";

        private Boolean _loading = false;

        [Data]
        public Boolean Loading
        {
            get { return _loading; }
            private set
            {
                _loading = value;
                RaisePropertyChanged(() => Loading);
            }
        }

        private IncrementalLoading<Answer> _answers;

        [Data]
        public IncrementalLoading<Answer> Answers
        {
            get { return _answers; }
            private set
            {
                _answers = value;
                RaisePropertyChanged(() => Answers);
            }
        }

        public RelayCommand GetDetailCommand { get; private set; }
        public RelayCommand GetFollowingCommand { get; private set; }
        public RelayCommand FollowUnFollow { get; private set; }


        private CollectionViewModel()
        {
        }

        public CollectionViewModel(ICollection collection, INavigate naviagation) : this()
        {
            this._collection = collection;
            this._navigation = naviagation;

            GetDetailCommand = new RelayCommand(GetCollectionDetailMethod);
            GetFollowingCommand = new RelayCommand(GetFollowingMethod);
            FollowUnFollow = new RelayCommand(FollowUnFollowMethod);
        }

        public void Setup(Int32 collectionId)
        {
            //if (_collectionId > 0)
            //{
            //    VmHelper.SaveStates(this, _collectionId.ToString());
            //}

            _collectionId = collectionId;

            //VmHelper.ResumeStates(this, _collectionId.ToString());

            _isLoaded = false;
            Detail = null;

            Answers = new IncrementalLoading<Answer>(GetMoreAnswersMethod, String.Format("/collections/{0}/answers", _collectionId), FirstAnswersUri, false);
        }
        
        private async void GetCollectionDetailMethod()
        {
            if (_isLoaded || null == _collection) return;

            var result = await _collection.GetDetail(LoginUser.Current.Token, _collectionId, true);

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
            if (null == _collection) return;

            var result = await _collection.GetFollowing(LoginUser.Current.Token, _collectionId, true);

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
            if (null == _collection) return;

            #region Checking Network

            if (false == Utility.Instance.IsNetworkAvailable)
            {
                ToasteIndicator.Instance.Show(String.Empty, "网络连接已中断", null, 3);
               
                return;
            }

            #endregion

            var result = await _collection.Follow(LoginUser.Current.Token, _collectionId);

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
            if (null == _collection) return;

            #region Checking Network

            if (false == Utility.Instance.IsNetworkAvailable)
            {
                ToasteIndicator.Instance.Show(String.Empty, "网络连接已中断", null, 3);
               
                return;
            }

            #endregion

            var result = await _collection.UnFollow(LoginUser.Current.Token, _collectionId, LoginUser.Current.Profile.Id);

            if (null != result.Error)
            {
                ToasteIndicator.Instance.Show(String.Empty, result.Error.Message, null, 3);

                Debug.WriteLine(Regex.Unescape(result.Error.Message));
                return;
            }

            if (result.Result.Succeed)
            {
                Following = new Following() {IsFollowing = false};
            }
        }

        private async Task<ListResultBase> GetMoreAnswersMethod(String request)
        {
            if (Loading) return null;

            Loading = true;

            var result = await _collection.GetAnswers(LoginUser.Current.Token, request, true);

            if (result.Result == null)
            {
                ToasteIndicator.Instance.Show(String.Empty, result.Error.Message, null, 3);
            }

            Loading = false;

            return result;
        }

    }
}