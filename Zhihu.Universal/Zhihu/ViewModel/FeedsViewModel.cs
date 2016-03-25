using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

using Windows.UI.Xaml;

using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;

using Zhihu.Common.Model;
using Zhihu.Common.DataSource;
using Zhihu.Common.Helper;
using Zhihu.Common.Model.Result;
using Zhihu.Common.Service;

using Zhihu.Helper;
using Zhihu.View;
using Zhihu.Controls;


namespace Zhihu.ViewModel
{
    public sealed class FeedsViewModel : ViewModelBase
    {
        private readonly IFeed _feed;
        private readonly IPerson _people;
        private readonly INavigate _navigate;

        [Data] private Boolean _isLoaded = false;
        
        #region Properties

        #region User feeds

        private Boolean _feedsLoading = false;

        public Boolean FeedsLoading
        {
            get { return _feedsLoading; }
            private set
            {
                _feedsLoading = value;
                RaisePropertyChanged(() => FeedsLoading);
            }
        }
        
        private const String Offset = "limit=20&after_id=";

        private IncrementalLoading<Feed> _feeds;

        [Data]
        public IncrementalLoading<Feed> Feeds
        {
            get { return _feeds; }
            private set
            {
                _feeds = value;

                RaisePropertyChanged(() => Feeds);
            }
        }

        #endregion

        private Visibility _refreshVisible = Visibility.Collapsed;

        public Visibility RefreshVisible
        {
            get { return _refreshVisible; }
            private set
            {
                _refreshVisible = value;
                RaisePropertyChanged(() => RefreshVisible);
            }
        }

        private Visibility _turnDarkVisible = Visibility.Collapsed;

        public Visibility TurnDarkVisible
        {
            get { return _turnDarkVisible; }
            private set
            {
                _turnDarkVisible = value;
                RaisePropertyChanged(() => TurnDarkVisible);
            }
        }

        #endregion

        public RelayCommand Load { get; private set; }

        public RelayCommand Refresh { get; private set; }
        public RelayCommand TurnDark { get; private set; }


        public RelayCommand NavToNotificationCommand { get; private set; }
        public RelayCommand NavToExploreCommand { get; private set; }
        public RelayCommand NavToSearchCommand { get; private set; }
        public RelayCommand NavToPopularCommand { get; private set; }
        public RelayCommand NavToMakeQuesCommand { get; private set; }
        public RelayCommand NavToOfflineCommand { get; set; }


        private FeedsViewModel()
        {
        }

        public FeedsViewModel(IFeed feed, IPerson people, INavigate navigate) : this()
        {
            _feed = feed;
            _people = people;
            _navigate = navigate;

            Refresh = new RelayCommand(RefreshMethod);
            TurnDark = new RelayCommand(TurnDarkMethod);

            Load = new RelayCommand(LoadMethod);
            
            NavToNotificationCommand = new RelayCommand(NavToNotificationMethod);
            NavToExploreCommand = new RelayCommand(NavigateToExploreMethod);
            NavToSearchCommand = new RelayCommand(NavToSearchMethod);
            NavToPopularCommand = new RelayCommand(NavToPopularPopularMethod);
            NavToMakeQuesCommand = new RelayCommand(NavToMakeQuesMethod);
            NavToOfflineCommand = new RelayCommand(NavToOfflineMethod);
        }
        
        private void LoadMethod()
        {
            if (_isLoaded)
            {
                return;
            }

            #region 检测当前系统主题，若为浅色主题且当前时间大于等于22：00，提示开启暗夜模式

            var currentTheme = LocalSettingUtility.Instance.Read<String>(Utility.Instance.CurrentThemeKey);

            if (currentTheme != "Dark" && DateTime.Now.Hour >= 22)
            {
                TurnDarkVisible = Visibility.Visible;
            }
            else
            {
                TurnDarkVisible = Visibility.Collapsed;
            }

            #endregion

            //VmHelper.Save(this);
            
            _isLoaded = true;
            FeedsLoading = false;
            
            Feeds = new IncrementalLoading<Feed>(GetMoreFeeds, "/feeds?excerpt_len=75", Offset, false);
        }
        
        private async void RefreshMethod()
        {
            #region Checking Network

            if (false == Utility.Instance.IsNetworkAvailable)
            {
                ToasteIndicator.Instance.Show(String.Empty, "网络连接已中断", null, 3);

                return;
            }

            #endregion

            FeedsLoading = true;

            var result = await _feed.GetFeedsAsync(LoginUser.Current.Token, "/feeds?excerpt_len=75", true);

            if (result.Result == null)
            {
                ToasteIndicator.Instance.Show(String.Empty, result.Error.Message, null, 3);
                return;
            }

            var previousUri = new Uri(result.Result.Paging.Previous);

            var feedsAll = (from object feed in Feeds select feed as Feed).ToList();
            
            var newFeeds = new List<Feed>();

            var query = from i in result.Result.GetItems()
                        let asFeed = i as Feed
                        where feedsAll.All(o => asFeed != null && o.Target.Url != asFeed.Target.Url)
                        select asFeed;

            newFeeds.AddRange(query);
          
            if (newFeeds.Count == 0)
            {
                FeedsLoading = false;

                ToasteIndicator.Instance.Show(String.Empty, "没有更新的内容了...", null, 3);

                return;
            }

            ToasteIndicator.Instance.Show(String.Empty, String.Format("获得了 {0} 条动态...", newFeeds.Count), null, 3);

            FeedsLoading = false;

            if (newFeeds.Count == 20)
            {
                Feeds = new IncrementalLoading<Feed>(GetMoreFeeds, "/feeds?excerpt_len=75", Offset, false);
                return;
            }

            for (var i = 0; i < newFeeds.Count; i++)
            {
                Feeds.Insert(i, newFeeds[i]);
            }
        }

        private void TurnDarkMethod()
        {
            Theme.Instance.TurnDark();
            
            TurnDarkVisible = Visibility.Collapsed;
        }

        private async Task<ListResultBase> GetMoreFeeds(String requestUri)
        {
            if (FeedsLoading) return null;

            Debug.WriteLineIf(FeedsLoading, "Feeds Loading, Please wait");

            FeedsLoading = true;

            if (String.IsNullOrEmpty(LoginUser.Current.Token))
            {
                return null;
            }

            var result = await _feed.GetFeedsAsync(LoginUser.Current.Token, requestUri, true);
        
            FeedsLoading = false;
  
            if (result == null) return null;
            
            if (result.Result != null) return result;

            ToasteIndicator.Instance.Show(String.Empty, result.Error.Message, null, 3);

            if (result.Error.Message.Contains("访问令牌"))
            {
                LogoutMethod();
            }

            return null;
        }

        private void LogoutMethod()
        {
            LoginUser.Current.Token = String.Empty;

            var userTokenKey = Utility.Instance.UserTokenKey;
            LocalSettingUtility.Instance.Remove(userTokenKey);

            AppShellPage.AppFrame.Navigate(typeof(LoginPage));
            AppShellPage.AppFrame.BackStack.Clear();
        }

        private void NavToNotificationMethod()
        {
            //if (_navigate != null) _navigate.NavigateTo(typeof(NotifiesPage));
        }

        private void NavigateToExploreMethod()
        {
            //if (_navigate != null) _navigate.NavigateTo(typeof (AmazingGuysPage));
        }
        
        private void NavToSearchMethod()
        {
            //VmNavHelper.NavToSearchPage(_navigate);
        }

        private void NavToPopularPopularMethod()
        {
            //if (_navigate != null) _navigate.NavigateTo(typeof (PopularPage));
        }

        private void NavToMakeQuesMethod()
        {
            //VmNavHelper.NavToMakeQuestionPage(_navigate);
        }
      
        private void NavToOfflineMethod()
        {
#if WINDOWS_PHONE_APP
            if (_navigate != null) _navigate.NavigateTo(typeof(OfflinePage));
#endif
        }

    }
}