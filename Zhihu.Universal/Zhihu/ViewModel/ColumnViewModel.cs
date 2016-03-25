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
    public sealed class ColumnViewModel : ViewModelBase
    {
        private readonly INavigate _navigation;
        private readonly IColumn _column;

        [Data] private String _columnId = String.Empty;
        [Data] private Boolean _isLoaded = false;

        private Column _detail;

        [Data]
        public Column Detail
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

        private const String FirstArticlesUri = "limit=10&offset=0";

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

        private IncrementalLoading<Article> _articles;

        [Data]
        public IncrementalLoading<Article> Articles
        {
            get { return _articles; }
            private set
            {
                _articles = value;
                RaisePropertyChanged(() => Articles);
            }
        }

        public RelayCommand GetDetailCommand { get; private set; }
        public RelayCommand GetFollowingCommand { get; private set; }
        public RelayCommand FollowUnFollow { get; private set; }
        

        private ColumnViewModel()
        {
        }

        public ColumnViewModel(IColumn collection, INavigate naviagation)
            : this()
        {
            this._column = collection;
            this._navigation = naviagation;

            GetDetailCommand = new RelayCommand(GetDetailMethod);
            GetFollowingCommand = new RelayCommand(GetFollowingMethod);
            FollowUnFollow = new RelayCommand(FollowUnFollowMethod);
        }

        public void Setup(String columnId)
        {
            //if (String.IsNullOrEmpty(_columnId) == false)
            //{
            //    VmHelper.SaveStates(this, _columnId.ToString());
            //}
            
            _columnId = columnId;

            //VmHelper.ResumeStates(this, _columnId.ToString());

            _isLoaded = false;

            Detail = null;

            Articles = new IncrementalLoading<Article>(GetMoreArticlesMethod,
                String.Format("/columns/{0}/articles", _columnId), FirstArticlesUri, false);
        }
        
        private async void GetDetailMethod()
        {
            if (_isLoaded || null == _column) return;

            var result = await _column.GetDetailAsync(LoginUser.Current.Token, _columnId, true);

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
            if (null == _column) return;

            var result = await _column.CheckFollowingAsync(LoginUser.Current.Token, _columnId, true);

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

            if (null == _column) return;

            var result = await _column.FollowAsync(LoginUser.Current.Token, _columnId);

            if (null != result.Error)
            {
                ToasteIndicator.Instance.Show(String.Empty, result.Error.Message, null, 3);

                Debug.WriteLine(Regex.Unescape(result.Error.Message));
                return;
            }

            if (result.Result.Succeed)
            {
                Following = new Following() { IsFollowing = true };
            }
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

            if (null == _column) return;

            var result = await _column.UnFollowAsync(LoginUser.Current.Token, _columnId, LoginUser.Current.Profile.Id);

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

        private async Task<ListResultBase> GetMoreArticlesMethod(String request)
        {
            if (Loading) return null;

            Loading = true;

            var result = await _column.GetArticles(LoginUser.Current.Token, request, true);

            if (result.Result == null)
            {
                ToasteIndicator.Instance.Show(String.Empty, result.Error.Message, null, 3);
            }

            Loading = false;

            return result;
        }
    }
}
