using System;
using System.Diagnostics;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;

using Zhihu.Common.Model;
using Zhihu.Common.Helper;
using Zhihu.Common.Service;
using Zhihu.Controls;
using Zhihu.View;


namespace Zhihu.ViewModel
{
    public sealed class LoginViewModel : ViewModelBase
    {
        private readonly ILogin _login;

        #region Log on and off

        private Boolean _loginLoading = false;

        public Boolean LoginLoading
        {
            get { return _loginLoading; }
            set
            {
                _loginLoading = value;
                RaisePropertyChanged(() => LoginLoading);
            }
        }

#if DEBUG
        private String _email = "zhihu20145@163.com";
#else
        private String _email = String.Empty;
#endif

        public String Email
        {
            get { return _email; }
            set
            {
                if (value == _email) return;

                _email = value;
                RaisePropertyChanged(() => Email);
            }
        }

#if DEBUG
        private String _password = "1qaz@WSX";
#else
        private String _password = String.Empty;
#endif

        public String Password
        {
            get { return _password; }
            set
            {
                if (_password == value) return;

                _password = value;
                RaisePropertyChanged(() => Password);
            }
        }

        #endregion

        #region Register

        private String _firstName = String.Empty;

        public String FirstName
        {
            get { return _firstName; }
            set
            {
                if (_firstName == value) return;
                _firstName = value;
                RaisePropertyChanged(() => FirstName);
            }
        }

        private String _lastName = String.Empty;

        public String LastName
        {
            get { return _lastName; }
            set
            {
                if (_lastName == value) return;
                _lastName = value;
                RaisePropertyChanged(() => LastName);
            }
        }

        private String _regEmail = String.Empty;

        public String RegEmail
        {
            get { return _regEmail; }
            set
            {
                if (value == _regEmail) return;

                _regEmail = value;
                RaisePropertyChanged(() => RegEmail);
            }
        }

        private String _regPassword = String.Empty;

        public String RegPassword
        {
            get { return _regPassword; }
            set
            {
                if (_regPassword == value) return;

                _regPassword = value;
                RaisePropertyChanged(() => RegPassword);
            }
        }

        #endregion

        public RelayCommand Login { get; private set; }
        public RelayCommand QqLogin { get; private set; }
        public RelayCommand WeiboLogin { get; private set; }
        public RelayCommand Register { get; private set; }
        public RelayCommand NavToFaq { get; private set; }

        private LoginViewModel()
        {
        }

        public LoginViewModel(ILogin login) : this()
        {
            _login = login;

            Login = new RelayCommand(LoginMethod);
            QqLogin = new RelayCommand(QqLoginMethod);
            WeiboLogin = new RelayCommand(WeiboLoginMethod);

            Register = new RelayCommand(RegisterMethod);

            NavToFaq = new RelayCommand(NavToFaqMethod);
        }

        private async void LoginMethod()
        {
            var result = CheckEmailAndPassword();

            if (true == result)
            {
                var loginRuslt = await LoginMethod(Email, Password);

                if (loginRuslt)
                {
                    AppShellPage.AppFrame?.Navigate(typeof(MainPage));
                    AppShellPage.AppFrame?.BackStack.Clear();
                }
            }
        }

        private async void QqLoginMethod()
        {
            var decompressHandler = new HttpClientHandler();

            if (decompressHandler.SupportsAutomaticDecompression)
            {
                decompressHandler.AutomaticDecompression = DecompressionMethods.GZip;
            }

            var client = new HttpClient(decompressHandler, true) {BaseAddress = new Uri("https://api.zhihu.com")};

            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            client.DefaultRequestHeaders.AcceptEncoding.Add(new StringWithQualityHeaderValue("gzip"));
            client.DefaultRequestHeaders.AcceptEncoding.Add(new StringWithQualityHeaderValue("deflate"));

            client.DefaultRequestHeaders.AcceptLanguage.Add(new StringWithQualityHeaderValue("zh-Hans", 1));
            client.DefaultRequestHeaders.AcceptLanguage.Add(new StringWithQualityHeaderValue("en", 0.9));

            client.DefaultRequestHeaders.UserAgent.Clear();
            client.DefaultRequestHeaders.UserAgent.Add(new ProductInfoHeaderValue("osee2unifiedRelease",
                "332"));
            client.DefaultRequestHeaders.UserAgent.Add(new ProductInfoHeaderValue("(iPhone; iOS 8.1.3; Scale/2.00)"));

            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic",
                "NTc3NGIzMDVkMmFlNDQ2OWEyYzkyNTg5NTZlYTQ5OjNjOTIyMTBhMzIxMjQ2N2U5NDc0ZTAwNmI2MjZhYQ==");

            client.DefaultRequestHeaders.ExpectContinue = true;

            var timestamp = Utility.Instance.Timestamp;
            var signature = "3b0ceb254e72a064edc9d48635a88ce71e407653";

            var body = new StringContent(String.Format(
                "client_id=5774b305d2ae4469a2c9258956ea49&grant_type=password&password=xiaoq931018&signature={0}&source=com.zhihu.ios&timestamp={1}&username=xiaoq416%40163.com",
                signature, timestamp), Encoding.UTF8, "application/x-www-form-urlencoded");

            var response = await client.PostAsync("client_login", body);

            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
            }
            else
            {
                var errorJson = await response.Content.ReadAsStringAsync();
            }
        }

        private void WeiboLoginMethod()
        {
#if WINDOWS_PHONE_APP
    //var umengClient = new SinaWeiboClient(Utility.Instance.UmengAppKey);
    //var loginResp = await umengClient.LoginAsync();
#endif
        }

        private Boolean CheckEmailAndPassword()
        {
            if (String.IsNullOrEmpty(Email) || String.IsNullOrEmpty(Password))
            {
                ToasteIndicator.Instance.Show(String.Empty, "请输入注册邮箱与密码", null, 3);

                return false;
            }

            //var emailRegex = Utility.Instance.EmailRegex;
            //var passwordRegex = Utility.Instance.PasswordRegex;

            //var regexEmail = new Regex(emailRegex);
            //var regexPassword = new Regex(passwordRegex);

            //if (false == regexEmail.IsMatch(Email))
            //{
            //    ToasteIndicator.Instance.Show(String.Empty, "邮箱格式有错误", null, 3);
            //    return false;
            //}
            //if (false == regexPassword.IsMatch(Password))
            //{
            //    ToasteIndicator.Instance.Show(String.Empty, "密码格式有错误", null, 3);
            //    return false;
            //}

            return true;
        }

        private async Task<Boolean> LoginMethod(String email, String password)
        {
            #region Checking Network

            if (false == Utility.Instance.IsNetworkAvailable)
            {
                ToasteIndicator.Instance.Show(String.Empty, "网络连接已中断", null, 3);

                return false;
            }

            #endregion

            if (null == _login) return false;

            LoginLoading = true;

            var result = await _login.LoginAsync(email, password);

            LoginLoading = false;

            if (result == null) return false;

            if (null != result.Error)
            {
                ToasteIndicator.Instance.Show(String.Empty, result.Error.Message, null, 3);

                Debug.WriteLine(Regex.Unescape(result.Error.Message));

                return false;
            }

            if (result.Result == null) return false;

            var loginResult = result.Result;

            LoginUser.Current.Token = loginResult.AccessToken;

            var expiresDate = Utility.Instance.GetExpiresInDate(result.Result.ExpiresIn);

            LocalSettingUtility.Instance.Add(Utility.Instance.UserTokenKey, LoginUser.Current.Token);
            LocalSettingUtility.Instance.Add(Utility.Instance.ExpiresDateKey, expiresDate.ToString());

            return true;
        }

        private async void RegisterMethod()
        {
            if (false == CheckRegisterInformation()) return;

            #region Checking Network

            if (false == Utility.Instance.IsNetworkAvailable)
            {
                ToasteIndicator.Instance.Show(String.Empty, "网络连接已中断", null, 3);

                return;
            }

            #endregion

            LoginLoading = true;

            var result =
                await _login.RegisterAsync(RegEmail, RegPassword, LastName, FirstName, 0, Utility.Instance.ClientId);

            LoginLoading = false;

            if (result == null) return;

            if (null != result.Error)
            {
                if (result.Error.Message == "Forbidden")
                {
                    ToasteIndicator.Instance.Show(String.Empty, "已经注册过了，请直接登录", null, 3);

                    return;
                }

                ToasteIndicator.Instance.Show(String.Empty, result.Error.Message, null, 3);

                Debug.WriteLine(Regex.Unescape(result.Error.Message));

                return;
            }

            if (result.Result.Succeed == true)
            {
                var loginSucceed = await LoginMethod(RegEmail, RegPassword);

                if (loginSucceed)
                {
                    //_navigate.NavigateTo(typeof(AmazingGuysPage));
                }
                ToasteIndicator.Instance.Show(String.Empty, "注册成功", null, 3);
            }
        }

        private bool CheckRegisterInformation()
        {
            if (String.IsNullOrEmpty(RegEmail) || String.IsNullOrEmpty(RegPassword) || String.IsNullOrEmpty(FirstName) ||
                String.IsNullOrEmpty(LastName))
            {
                ToasteIndicator.Instance.Show(String.Empty, "请认真填写注册资料", null, 3);

                return false;
            }

            var emailRegex = Utility.Instance.EmailRegex;
            var passwordRegex = Utility.Instance.PasswordRegex;

            var regexEmail = new Regex(emailRegex);
            var regexPassword = new Regex(passwordRegex);

            if (false == regexEmail.IsMatch(RegEmail) || false == regexPassword.IsMatch(RegPassword))
            {
                ToasteIndicator.Instance.Show(String.Empty, "注册邮箱或密码格式不正确", null, 3);

                return false;
            }

            return true;
        }


        private void NavToFaqMethod()
        {
#if WINDOWS_PHONE_APP

            _navigate.NavigateTo(typeof(FaqPage));

#endif

#if WINDOWS_APP
        
#endif
        }
    }
}
