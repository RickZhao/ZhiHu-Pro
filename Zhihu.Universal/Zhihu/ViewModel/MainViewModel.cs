using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Input;

using Windows.ApplicationModel.Store;
using Windows.System;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Microsoft.Practices.ServiceLocation;

using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;

using Zhihu.Common.Model;
using Zhihu.Common.Service;
using Zhihu.Common.Helper;
using Zhihu.Controls;
using Zhihu.Helper;
using Zhihu.View;
using Zhihu.View.Main;


namespace Zhihu.ViewModel
{
    public sealed class MainViewModel : ViewModelBase
    {
        private readonly IPerson _people;

        public FeedsViewModel Feeds
        {
            get { return ServiceLocator.Current.GetInstance<FeedsViewModel>(); }
        }

        public FindViewModel Find
        {
            get { return ServiceLocator.Current.GetInstance<FindViewModel>(); }
        }

        public NotifyViewModel Notify
        {
            get { return ServiceLocator.Current.GetInstance<NotifyViewModel>(); }
        }

        #region SplitView Menu

        private ObservableCollection<MenuItem> _menuItems = new ObservableCollection<MenuItem>();

        public ObservableCollection<MenuItem> MenuItems
        {
            get { return this._menuItems; }
        }

        public RelayCommand ToggleSplitViewPaneCommand { get; private set; }

        private bool isSplitViewPaneOpen =false;
        public bool IsSplitViewPaneOpen
        {
            get { return this.isSplitViewPaneOpen; }
            set { Set(ref this.isSplitViewPaneOpen, value); }
        }

        private MenuItem selectedMenuItem;
        public MenuItem SelectedMenuItem
        {
            get { return this.selectedMenuItem; }
            set
            {
                if (Set(ref this.selectedMenuItem, value))
                {
                    RaisePropertyChanged("SelectedPageType");

                    // auto-close split view pane
                    this.IsSplitViewPaneOpen = false;
                }
            }
        }

        public Type SelectedPageType
        {
            get
            {
                if (this.selectedMenuItem != null)
                {
                    return this.selectedMenuItem.PageType;
                }
                return null;
            }
            set
            {
                // select associated menu item
                this.SelectedMenuItem = this._menuItems.FirstOrDefault(m => m.PageType == value);
                RaisePropertyChanged();
            }
        }

        public RelayCommand FeedsTappd { get; private set; }
        public RelayCommand FindTapped { get; private set; }
        public RelayCommand NoteTapped { get; private set; }
        public RelayCommand PersonalTapped { get; private set; }

        #endregion

        private Boolean _hasPayed = true;

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

        public RelayCommand GetProfile { get; private set; }

        private Visibility _exploreVisible = Visibility.Collapsed;

        public Visibility ExploreVisible
        {
            get { return _exploreVisible; }
            private set
            {
                _exploreVisible = value;
                RaisePropertyChanged(() => ExploreVisible);
            }
        }

        private Boolean _nightModeOn;
        public Boolean NightModeOn
        {
            get { return _nightModeOn; }
            private set
            {
                _nightModeOn = value;
                RaisePropertyChanged(() => NightModeOn);
            }
        }
        
        private Visibility _remindReviewVisible = Visibility.Collapsed;

        public Visibility RemindReviewVisible
        {
            get { return _remindReviewVisible; }
            private set
            {
                _remindReviewVisible = value;
                RaisePropertyChanged(() => RemindReviewVisible);
            }
        }

        private Notifications Notifications { get; set; }

        private Visibility _notifyVisible = Visibility.Collapsed;

        public Visibility NotifyVisible
        {
            get { return _notifyVisible; }
            private set
            {
                _notifyVisible = value;
                RaisePropertyChanged(() => NotifyVisible);
            }
        }

        private Visibility _contributeVisible = Visibility.Collapsed;
        public Visibility ContributeVisible
        {
            get { return _contributeVisible; }
            private set
            {
                _contributeVisible = value;
                RaisePropertyChanged();
            }
        }

        private Boolean _isFormalVip = false;
        public Boolean IsFormalVip
        {
            get { return _isFormalVip; }
            private set
            {
                _isFormalVip = value;
                RaisePropertyChanged();
            }
        }

        public RelayCommand CheckNotify { get; private set; }
        public RelayCommand CheckTheme { get; private set; }
        public RelayCommand SwitchTheme { get; private set; }

        public RelayCommand PromptPay { get; private set; }

        public RelayCommand CheckReview { get; private set; }
        public RelayCommand ReviewNow { get; private set; }
        public RelayCommand RemindLater { get; private set; }

        public RelayCommand Logout { get; private set; }
        
        public RelayCommand PromptContribute { get; private set; }
        public RelayCommand ShowContribute { get; private set; }
        public RelayCommand CloseContribute { get; private set; }

        private MainViewModel() { }

        public MainViewModel(IPerson people) : this()
        {
            _people = people;

            MenuItems.Add(new MenuItem { Icon = Symbol.Home, Title = "动态", PageType = typeof(FeedsPage) });
            MenuItems.Add(new MenuItem { Icon = Symbol.Globe, Title = "发现", PageType = typeof(FindPage) });
            MenuItems.Add(new MenuItem { Icon = Symbol.Message, Title = "消息", PageType = typeof(NotePage) });
            MenuItems.Add(new MenuItem { Icon = Symbol.Contact, Title = "个人", PageType = typeof(PersonalPage) });

            this.ToggleSplitViewPaneCommand = new RelayCommand(() => this.IsSplitViewPaneOpen = !this.IsSplitViewPaneOpen);

            FeedsTappd = new RelayCommand(() => { SelectedPageType = typeof(FeedsPage); });
            FindTapped = new RelayCommand(() => { SelectedPageType = typeof(FindPage); });
            NoteTapped = new RelayCommand(() => { SelectedPageType = typeof(NotePage); });
            PersonalTapped = new RelayCommand(() => { SelectedPageType = typeof(PersonalPage); });

            Logout = new RelayCommand(LogoutMethod);

            GetProfile = new RelayCommand(GetProfileMethod);

            CheckNotify = new RelayCommand(CheckNotifyMethod);

            CheckTheme = new RelayCommand(CheckThemeMethod);
            SwitchTheme = new RelayCommand(SwitchThemeMethod);

            PromptPay = new RelayCommand(PromptPayMethod);

            ReviewNow = new RelayCommand(ReviewNowMethod);
            RemindLater = new RelayCommand(RemindLaterMethod);

            CheckReview = new RelayCommand(CheckReviewMethod);

            PromptContribute = new RelayCommand(PromptContributeMethod);
            ShowContribute = new RelayCommand(ShowContributeMethod);
            CloseContribute = new RelayCommand(CloseContributeMethod);
        }
        
        private async void GetProfileMethod()
        {
            CheckCredentialAvailable();

            if(LoginUser.Current.Profile!=null) CheckLicenseMethod();

            if (null == _people || String.IsNullOrEmpty(LoginUser.Current.Token)) return;

            var result = await _people.GetProfileAsync(LoginUser.Current.Token, String.Empty, true);

            if (result == null) return;

            if (null != result.Error)
            {
                Debug.WriteLine(Regex.Unescape(result.Error.Message));

                return;
            }

            LoginUser.Current.Profile = Profile = result.Result;

            if (Profile.FollowingCount == 0 && Profile.FollowingQuestionCount == 0 && Profile.FollowingTopicCount == 0 &&
                Profile.FollowingColumnsCount == 0 && Profile.FollowingCollectionCount == 0)
            {
                ExploreVisible = Visibility.Visible;
            }
            else
            {
                ExploreVisible = Visibility.Collapsed;
            }

            CheckLicenseMethod();
        }

        private void CheckCredentialAvailable()
        {
            var expiresDateString = LocalSettingUtility.Instance.Read<String>(Utility.Instance.ExpiresDateKey);

            if (String.IsNullOrEmpty(expiresDateString))
            {
                LogoutMethod();

                ToasteIndicator.Instance.Show(String.Empty, "请重新登录，访问令牌已过期", null, 3);
            }
            else
            {
                var expiresDate = DateTime.Parse(expiresDateString);
                if (DateTime.Now >= expiresDate.AddDays(-1))
                {
                    LogoutMethod();

                    ToasteIndicator.Instance.Show(String.Empty, "请重新登录，访问令牌已过期", null, 3);
                }
            }
        }

        private void LogoutMethod()
        {
            LoginUser.Current.Token = String.Empty;

            var userTokenKey = Utility.Instance.UserTokenKey;
            LocalSettingUtility.Instance.Remove(userTokenKey);

            AppShellPage.AppFrame.Navigate(typeof(LoginPage));
            AppShellPage.AppFrame.BackStack.Clear();
        }

        private void CheckThemeMethod()
        {
            var currentTheme = LocalSettingUtility.Instance.Read<String>(Utility.Instance.CurrentThemeKey);

            if (String.IsNullOrEmpty(currentTheme) || currentTheme == "Light")
            {
                NightModeOn = false;
                Theme.Instance.TurnLight();
            }
            else
            {
                NightModeOn = true;
                Theme.Instance.TurnDark();
            }
        }

        private void PromptPayMethod()
        {
            if (_hasPayed == true) return;

            if (DateTime.Now.Hour < 20 || DateTime.Now.Hour > 6) return;

            var lastPromptedPayDateString = LocalSettingUtility.Instance.Read<String>("LastPromptedPayDate");
            var lastPromptedPayDate = DateTime.MinValue;

            if (false == DateTime.TryParse(lastPromptedPayDateString, out lastPromptedPayDate))
            {
                LocalSettingUtility.Instance.Add(String.Format("LastPromptedPayDate"), DateTime.Now.ToString());
            }
            else if (DateTime.Now.DayOfYear > lastPromptedPayDate.DayOfYear)
            {
                SwitchThemeMethod();
                LocalSettingUtility.Instance.Add(String.Format("LastPromptedPayDate"), DateTime.Now.ToString());
            }
        }

        private async void SwitchThemeMethod()
        {
            if (_hasPayed == false)
            {
                var dialog = new MessageDialog("夜晚刷知乎会损害您的视力健康，请付费使用黑暗模式。");
                dialog.Title = "ZhiHu Pro 温馨提示";
                dialog.Commands.Add(new UICommand() { Label = "现在购买", Id = 0 });
                dialog.Commands.Add(new UICommand() { Label = "暂不购买", Id = 1 });

                var dialogRslt = await dialog.ShowAsync();
                if ((Int32)dialogRslt.Id == 0)
                {
                    try
                    {
#if DEBUG
                        await CurrentAppSimulator.RequestProductPurchaseAsync("NightModeIAP");
#else
                        await CurrentApp.RequestProductPurchaseAsync("NightModeIAP");
#endif
                        CheckLicenseMethod();

                        if (_hasPayed)
                        {
                            ToasteIndicator.Instance.Show("提示", "购买已成功，黑暗模式已解锁!", null, 5);
                        }
                        else
                        {
                            ToasteIndicator.Instance.Show("提示", "购买出现问题，黑暗模式无法解锁!", null, 5);
                        }
                    }
                    catch (Exception)
                    {
                        // The in-app puchase was not completed because an error occurred.
                    }
                }

                return;
            }

            if (NightModeOn == false)
            {
                NightModeOn = true;
                Theme.Instance.TurnDark();
            }
            else
            {
                NightModeOn = false;
                Theme.Instance.TurnLight();
            }
        }

        private async void CheckNotifyMethod()
        {
            #region Checking Network

            if (false == Utility.Instance.IsNetworkAvailable)
            {
                ToasteIndicator.Instance.Show(String.Empty, "网络连接已中断", null, 3);

                return;
            }
            
            #endregion

            if (null == _people) return;

            var result = await _people.CheckNotificationsAsync(LoginUser.Current.Token);

            if (result == null) return;

            if (null != result.Error)
            {
                ToasteIndicator.Instance.Show(String.Empty, result.Error.Message, null, 3);

                Debug.WriteLine(Regex.Unescape(result.Error.Message));
                return;
            }

            Notifications = result.Result;

            if (result.Result != null)
            {
                var totalCount = result.Result.Message + result.Result.NewContent + result.Result.NewFollow +
                                 result.Result.NewLove;

                NotifyVisible = totalCount > 0 ? Visibility.Visible : Visibility.Collapsed;
            }
            //var unReadResult = await _people.CheckUnReadAsync(LoginUser.Current.Token);

            //if (unReadResult == null) return;

            //if (null != unReadResult.Error)
            //{
            //    ToasteIndicator.Instance.Show(String.Empty, result.Error.Message, null, 3);

            //    Debug.WriteLine(Regex.Unescape(result.Error.Message));
            //    return;
            //}

            //if (unReadResult.Result.NotificationContent > 0)
            //{
            //    NotifyVisible = Visibility.Visible;
            //}
        }

        private Boolean _firstCheck = true;

        private void CheckReviewMethod()
        {
            if (_firstCheck == true)
            {
                _firstCheck = false;
                return;
            }

            var valueString = LocalSettingUtility.Instance.Read<String>(Utility.Instance.HasReviewedKey);

            if (valueString == Boolean.TrueString) return;

            var dateString = LocalSettingUtility.Instance.Read<String>(Utility.Instance.LastRemindKey);

            DateTime lastRemind;

            if (DateTime.TryParse(dateString, out lastRemind) == false)
            {
                LocalSettingUtility.Instance.Add(Utility.Instance.HasReviewedKey, Boolean.FalseString);
                LocalSettingUtility.Instance.Add(Utility.Instance.LastRemindKey, DateTime.Now.ToString());

                RemindReviewVisible = Visibility.Collapsed;
                return;
            }

            if (DateTime.Now.DayOfYear > lastRemind.DayOfYear)
            {
                RemindReviewVisible = Visibility.Visible;
            }
        }

        private void RemindLaterMethod()
        {
            RemindReviewVisible = Visibility.Collapsed;

            LocalSettingUtility.Instance.Add(Utility.Instance.HasReviewedKey, Boolean.FalseString);
            LocalSettingUtility.Instance.Add(Utility.Instance.LastRemindKey, DateTime.Now.ToString());
        }

        private async void ReviewNowMethod()
        {
            var result = await Launcher.LaunchUriAsync(new Uri("ms-windows-store:reviewapp?appid=" + CurrentApp.AppId));

            LocalSettingUtility.Instance.Add(Utility.Instance.HasReviewedKey, Boolean.TrueString);

            RemindReviewVisible = Visibility.Collapsed;
        }

        private async void CheckLicenseMethod()
        {
            IsFormalVip = await LoginUser.Current.IsVip();

            if (IsFormalVip)
            {
                _hasPayed = true;
                return;
            }

#if DEBUG
            var license = CurrentAppSimulator.LicenseInformation;
#else
            var license = CurrentApp.LicenseInformation;
#endif

            _hasPayed = license.ProductLicenses["NightModeIAP"].IsActive;
            LocalSettingUtility.Instance.Add(String.Format("HasPayed"), _hasPayed.ToString());
        }

        private Boolean _hasPomptedContribute = false;

        private void PromptContributeMethod()
        {
            if (_hasPomptedContribute) return;

            var dateString = LocalSettingUtility.Instance.Read<String>(Utility.Instance.LastPromptContributeKey);

            DateTime lastPromptContribute;

            if (DateTime.TryParse(dateString, out lastPromptContribute) == false && lastPromptContribute.DayOfYear < DateTime.Now.DayOfYear)
            {
                ContributeVisible = Visibility.Visible;
                LocalSettingUtility.Instance.Add(Utility.Instance.LastPromptContributeKey, DateTime.Now.ToString());
            }

            _hasPomptedContribute = true;
        }

        private void ShowContributeMethod()
        {
            ContributeVisible = Visibility.Visible;
        }

        private void CloseContributeMethod()
        {
            ContributeVisible = Visibility.Collapsed;
        }
    }
}
