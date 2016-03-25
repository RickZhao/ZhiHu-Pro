using System;
using System.Linq;
using System.Threading.Tasks;

using Windows.Data.Xml.Dom;

using Zhihu.Common.Helper;


namespace Zhihu.Common.Model
{
    public sealed class LoginUser
    {
        #region Singleton

        private static LoginUser _user = null;

        public static LoginUser Current
        {
            get
            {
                if (_user == null)
                {
                    _user = new LoginUser();

                    var userTokenKey = Utility.Instance.UserTokenKey;
                    var userTokenValue = LocalSettingUtility.Instance.Read<String>(userTokenKey);

                    _user.Token = userTokenValue;
                }

                return _user;
            }
        }

        private LoginUser()
        {
        }

        #endregion

        public String Token { get; set; }

        public Profile Profile { get; set; }

        public async Task<Boolean> IsVip()
        {
            if (this.Profile == null) return true;

            var isVip = LocalSettingUtility.Instance.Read<String>(String.Format("IsVip_{0}", Profile.Id));
            if (isVip == Boolean.TrueString) return true;

            var xmlContributors = await XmlDocument.LoadFromUriAsync(new Uri("http://zhihu.azurewebsites.net/contributors.xml", UriKind.Absolute));

            var emailNodes = xmlContributors.SelectNodes("/Contributors/eMail");

            if (emailNodes.Any(
                    node => node.InnerText == this.Profile.Email || node.InnerText == this.Profile.Id))
            {
                LocalSettingUtility.Instance.Add(String.Format("IsVip_{0}", Profile.Id), Boolean.TrueString);
                return true;
            }

            return false;
        }
    }
}
