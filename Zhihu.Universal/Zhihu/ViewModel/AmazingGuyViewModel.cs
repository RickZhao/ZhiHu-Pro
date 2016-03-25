using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text.RegularExpressions;

using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;

using Zhihu.Common.Helper;
using Zhihu.Common.Model;
using Zhihu.Common.Model.Result;
using Zhihu.Common.Service;
using Zhihu.Controls;

namespace Zhihu.ViewModel
{
    public sealed class AmazingGuyViewModel : ViewModelBase
    {
        private readonly IPerson _service;
        private readonly INavigate _navigate;

        #region Interestring Guys

        private Boolean _amazingGuysLoading = false;

        public Boolean AmazingGuysLoading
        {
            get { return _amazingGuysLoading; }
            set
            {
                _amazingGuysLoading = value;
                RaisePropertyChanged(() => AmazingGuysLoading);
            }
        }

        public ObservableCollection<AmazingGuy> AmazingGuys { get; private set; }

        #endregion

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

        private Boolean _nextEnable = false;

        public Boolean NextEnable
        {
            get { return _nextEnable; }
            set
            {
                _nextEnable = value;
                RaisePropertyChanged(() => NextEnable);
            }
        }

        public RelayCommand GetAmazingGuys { get; private set; }
        public RelayCommand<AmazingGuy> FollowUnfollow { get; private set; }
        public RelayCommand NextCommand { get; private set; }

        private AmazingGuyViewModel()
        {
            AmazingGuys = new ObservableCollection<AmazingGuy>();
        }

        public AmazingGuyViewModel(IPerson service, INavigate navigation) : this()
        {
            this._service = service;
            this._navigate = navigation;

            NextCommand = new RelayCommand(NextMethod);

            GetAmazingGuys = new RelayCommand(GetAmazingGuysMethod);
            FollowUnfollow = new RelayCommand<AmazingGuy>(FollowUnfollowMethod);
        }


        private async void GetAmazingGuysMethod()
        {
            if (null == _service) return;

            #region Checking Network

            if (false == Utility.Instance.IsNetworkAvailable)
            {
                ToasteIndicator.Instance.Show(String.Empty, "网络连接已中断", null, 3);
             
                return;
            }

            #endregion

            AmazingGuysLoading = true;
            
            var result = await _service.GetAmazingGuysAsync(LoginUser.Current.Token);

            AmazingGuysLoading = false;

            if (null != result.Error)
            {
                ToasteIndicator.Instance.Show(String.Empty, result.Error.Message, null, 3);

                Debug.WriteLine(Regex.Unescape(result.Error.Message));
                return;
            }

            foreach (var guy in result.Result.GetItems())
            {
                AmazingGuys.Add(new AmazingGuy()
                {
                    Guy = guy as Author,
                    IsFollowing = false,
                });
            }
        }

        private void NextMethod()
        {
            //this._navigate.NavigateTo(typeof (MainPage));
        }

        private async void FollowUnfollowMethod(AmazingGuy guy)
        {
            if (null == _service || String.IsNullOrEmpty(LoginUser.Current.Token)) return;

            #region Checking Network

            if (false == Utility.Instance.IsNetworkAvailable)
            {
                ToasteIndicator.Instance.Show(String.Empty, "网络连接已中断", null, 3);
               
                return;
            }

            #endregion

            Loading = true;

            FollowingResult result;

            if (guy.IsFollowing == false)
                result = await _service.FollowAsync(LoginUser.Current.Token, guy.Guy.Id);
            else
                result = await _service.UnFollowAsync(LoginUser.Current.Token, guy.Guy.Id, LoginUser.Current.Profile.Id);

            Loading = false;

            if (null != result.Error)
            {
                Debug.WriteLine(Regex.Unescape(result.Error.Message));
                return;
            }

            guy.IsFollowing = !guy.IsFollowing;

            NextEnable = AmazingGuys.Any(item => item.IsFollowing);
        }
    }
}
