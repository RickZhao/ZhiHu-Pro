using System;
using System.Threading.Tasks;

using Windows.ApplicationModel;
using Windows.ApplicationModel.Store;
using Windows.System;

using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;

using Zhihu.Common.Cache;
using Zhihu.Common.Helper;
using Zhihu.Common.Model;
using Zhihu.Common.Service;
using Zhihu.Controls;
using Zhihu.Helper;
using Zhihu.View;
using Windows.System.Profile;

namespace Zhihu.ViewModel
{
    public sealed class AboutViewModel : ViewModelBase
    {
        private readonly ISetting _setting;

        public String AppVersion
        {
            get
            {
                var ver = Package.Current.Id.Version;

                return ver.Major.ToString() + "." + ver.Minor.ToString() + "." + ver.Build.ToString() + "." +
                       ver.Revision.ToString();
            }
        }

        private String _feedBackTitle = String.Empty;

        [Data]
        public String FeedBackTitle
        {
            get { return _feedBackTitle; }
            set
            {
                _feedBackTitle = value;
                RaisePropertyChanged(() => FeedBackTitle);
            }
        }

        private String _feedBackContent = String.Empty;

        [Data]
        public String FeedBackContent
        {
            get { return _feedBackContent; }
            set
            {
                _feedBackContent = value;
                RaisePropertyChanged(() => FeedBackContent);
            }
        }
        
        private Boolean _isLight = true;

        public Boolean IsLight
        {
            get { return _isLight; }
            private set
            {
                _isLight = value;
                RaisePropertyChanged(() => IsLight);
            }
        }

        private Boolean _isDark = false;

        public Boolean IsDark
        {
            get { return _isDark; }
            private set
            {
                _isDark = value;
                RaisePropertyChanged(() => IsDark);
            }
        }

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
        
        public RelayCommand NavToAppAuthor { get; private set; }
        public RelayCommand NavToFaq { get; private set; }

        public RelayCommand ReviewApp { get; private set; }

        public RelayCommand NavToFeedback { get; private set; }
        public RelayCommand Feedback { get; private set; }

        public RelayCommand CheckTheme { get; private set; }
        public RelayCommand TurnDark { get; private set; }
        public RelayCommand TurnLight { get; private set; }

        public RelayCommand ClearCache { get; private set; }

        public RelayCommand CheckTopStory { get; private set; }
        public RelayCommand SwitchTopStory { get; private set; }
        public RelayCommand NavToCustomFontSize { get; private set; }

        private AboutViewModel()
        {
            ReviewApp = new RelayCommand(ReviewAppMethod);
            NavToAppAuthor = new RelayCommand(NavToAppAuthorMethod);
            NavToFaq = new RelayCommand(NavToFaqMethod);

            Feedback = new RelayCommand(FeedbackMethod);
            NavToFeedback = new RelayCommand(NavToFeedbackMethod);

            CheckTheme = new RelayCommand(CheckThemeMethod);
            TurnDark = new RelayCommand(TurnDarkMethod);
            TurnLight = new RelayCommand(TurnLightMethod);

            CheckTopStory = new RelayCommand(CheckTopStoryMethod);

            SwitchTopStory = new RelayCommand(SwitchTopStoryMethod);

            NavToCustomFontSize = new RelayCommand(NavToCustomFontSizeMethod);


            ClearCache = new RelayCommand(ClearCacheMethod);
        }

        public AboutViewModel(ISetting setting) : this()
        {
            this._setting = setting;
        }

        private void Remember(String webUri)
        {
        }

        private void NavToAppAuthorMethod()
        {
            NavHelper.NavToProfilePage("3f1a81e62c454dc5278c688a9662f709", AppShellPage.AppFrame);
        }

        private void NavToFaqMethod()
        {
        }

        private async void ReviewAppMethod()
        {
            await Launcher.LaunchUriAsync(
                new Uri("ms-windows-store:reviewapp?appid=" + CurrentApp.AppId));
        }

        private async void FeedbackMethod()
        {
            var result = await Launcher.LaunchUriAsync(
                new Uri(
                    String.Format("mailto:zhihupro@outlook.com?subject={0}&body={1}", FeedBackTitle,
                        FeedBackContent.Replace(Environment.NewLine, "%0d%0a"))
                    ));

            FeedBackTitle = FeedBackContent = String.Empty;

            await Task.Delay(300);

            if (AppShellPage.AppFrame.CanGoBack) AppShellPage.AppFrame.GoBack();
        }

        private void NavToFeedbackMethod()
        {
            // get the system family name
            var ai = AnalyticsInfo.VersionInfo;
            var systemFamily = ai.DeviceFamily;

            // get the system version number
            string sv = AnalyticsInfo.VersionInfo.DeviceFamilyVersion;
            ulong v = ulong.Parse(sv);
            ulong v1 = (v & 0xFFFF000000000000L) >> 48;
            ulong v2 = (v & 0x0000FFFF00000000L) >> 32;
            ulong v3 = (v & 0x00000000FFFF0000L) >> 16;
            ulong v4 = (v & 0x000000000000FFFFL);
            var systemVersion = $"{v1}.{v2}.{v3}.{v4}";

            // get the package architecure
            var package = Package.Current;
            var systemArchitecture = package.Id.Architecture.ToString();


            FeedBackContent = String.Format("{0}{1}{2}{3}From: {4} {5} {6} {7}", Environment.NewLine, Environment.NewLine, Environment.NewLine, Environment.NewLine,
                systemFamily, systemVersion, systemArchitecture,
                AppVersion);

            AppShellPage.AppFrame.Navigate(typeof(FeedbackPage));
        }

        private void NavToCustomFontSizeMethod()
        {

#if WINDOWS_PHONE_APP

            if (_navigate == null) return;

            _navigate.NavigateTo(typeof (CustomFontSizePage));

#endif
        }

        private void TurnDarkMethod()
        {
            IsDark = true;
            IsLight = false;
            Theme.Instance.TurnDark();
        }

        private void TurnLightMethod()
        {
            IsDark = false;
            IsLight = true;
            Theme.Instance.TurnLight();
        }

        private void CheckThemeMethod()
        {
            var currentTheme = LocalSettingUtility.Instance.Read<String>(Utility.Instance.CurrentThemeKey);

            if (String.IsNullOrEmpty(currentTheme) || currentTheme == "Light")
            {
                IsDark = false;
                IsLight = true;
            }
            else
            {
                IsDark = true;
                IsLight = false;
            }
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
    }
}
