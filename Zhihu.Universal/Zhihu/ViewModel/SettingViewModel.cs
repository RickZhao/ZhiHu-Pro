using System;
using System.Collections.ObjectModel;

using Windows.Foundation.Metadata;
using Windows.UI.ViewManagement;

using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;

using Newtonsoft.Json;

using Zhihu.Common.Model;
using Zhihu.Common.Cache;
using Zhihu.Common.Helper;
using Zhihu.Common.Service;
using Zhihu.Controls;
using Zhihu.Helper;


namespace Zhihu.ViewModel
{
    public sealed class SettingViewModel : ViewModelBase
    {
        private readonly ISetting _setting;
        private Boolean _topStoryEnable = false;

        public Boolean TopStoryEnable
        {
            get { return _topStoryEnable; }
            private set
            {
                _topStoryEnable = value;
                RaisePropertyChanged(() => TopStoryEnable);
            }
        }

        public Boolean OpenLinkWithEdge
        {
            get { return NavHelper.OpenLinkWithEdge; }
            private set
            {
                NavHelper.OpenLinkWithEdge = value;
                RaisePropertyChanged(() => OpenLinkWithEdge);
            }
        }

        public ObservableCollection<Feed> DemoFeeds { get; private set; }
        
        public RelayCommand ClearCache { get; private set; }
        public RelayCommand CheckTopStory { get; private set; }
        public RelayCommand SwitchTopStory { get; private set; }
        public RelayCommand SwitchStatusBar { get; private set; }
        public RelayCommand NavToCustomFontSize { get; private set; }

        public SettingViewModel()
        {
            var feeds = JsonConvert.DeserializeObject<Feeds>(Utility.Instance.DemoFeeds);
            DemoFeeds = new ObservableCollection<Feed>(feeds.Items);
            
            ClearCache = new RelayCommand(ClearCacheMethod);

            CheckTopStory = new RelayCommand(CheckTopStoryMethod);
            
            SwitchTopStory = new RelayCommand(SwitchTopStoryMethod);

            SwitchStatusBar = new RelayCommand(SwitchStatusBarOpen);

            NavToCustomFontSize = new RelayCommand(NavToCustomFontSizeMethod);
        }

        public SettingViewModel(ISetting setting) : this()
        {
            this._setting = setting;
        }

        private async void ClearCacheMethod()
        {
            var clearDbResult = await DbContext.Instance.ClearAll();

            var clearStoredCache = await LocalStoreHelper.Instance.ClearCache();

            if (clearDbResult && clearStoredCache)
            {
                ToasteIndicator.Instance.Show(String.Empty, "缓存已清理", null, 3);
            }
        }

        private async void CheckTopStoryMethod()
        {
            if (_setting == null) return;

            var result = await _setting.CheckUseStory(LoginUser.Current.Token, true);

            if (result.Result == null)
            {
                ToasteIndicator.Instance.Show(String.Empty, result.Error.Message, null, 3);
                return;
            }

            _blockTopStoryToggle = true;

            TopStoryEnable = result.Result.Status.Enable;

            _blockTopStoryToggle = false;
        }

        private Boolean _blockTopStoryToggle = false;

        private async void SwitchTopStoryMethod()
        {
            if (_setting == null || _blockTopStoryToggle) return;

            #region Checking Network

            if (false == Utility.Instance.IsNetworkAvailable)
            {
                ToasteIndicator.Instance.Show(String.Empty, "网络连接已中断", null, 3);

                return;
            }

            #endregion

            if (TopStoryEnable)
            {
                #region Disable TopStory

                var result = await _setting.SwitchTopStory(LoginUser.Current.Token, false);

                if (result.Result == null)
                {
                    ToasteIndicator.Instance.Show(String.Empty, result.Error.Message, null, 3);
                    return;
                }

                _blockTopStoryToggle = true;

                TopStoryEnable = result.Result.Status.Enable;

                _blockTopStoryToggle = false;

                ToasteIndicator.Instance.Show(String.Empty, "重启后生效", null, 3);

                #endregion
            }
            else
            {
                #region Enable TopStory

                var result = await _setting.SwitchTopStory(LoginUser.Current.Token, true);

                if (result.Result == null)
                {
                    ToasteIndicator.Instance.Show(String.Empty, result.Error.Message, null, 3);
                    return;
                }

                _blockTopStoryToggle = true;

                TopStoryEnable = result.Result.Status.Enable;

                _blockTopStoryToggle = false;

                ToasteIndicator.Instance.Show(String.Empty, "重启后生效", null, 3);

                #endregion
            }
        }
        
        private async void SwitchStatusBarOpen()
        {
            // hide status bar - do this only once (mobile device only)
            if (ApiInformation.IsTypePresent("Windows.UI.ViewManagement.StatusBar"))
            {
                var statusBar = StatusBar.GetForCurrentView();

                statusBar.ForegroundColor = Theme.Instance.FeedTitleColor;
                statusBar.BackgroundColor = Theme.Instance.PageBackColor;

                if (Theme.Instance.StatusBarIsOpen)
                    await statusBar.ShowAsync();
                else
                    await statusBar.HideAsync();
            }
        }

        private void SwitchOpenLinkWithEdge()
        {
            OpenLinkWithEdge = !OpenLinkWithEdge;
        }

        private void NavToCustomFontSizeMethod()
        {
            //AppShellPage.AppFrame.Navigate(typeof(FontSizePage));
        }
    }
}
