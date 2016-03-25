using System;
using System.Threading.Tasks;
using Windows.ApplicationModel.Store;
using Windows.UI.Xaml.Data;
using Zhihu.Common.Helper;
using Zhihu.Common.Model;

namespace Zhihu.Converter
{
    public sealed class NightModeConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            var nightModeOn = (Boolean)value;
            var nightModeFormat = String.Empty;

            if (nightModeOn) nightModeFormat = "日间模式";
            else nightModeFormat = "夜间模式";

            if (LoginUser.Current.Profile == null) return nightModeFormat;

            var isVip = LocalSettingUtility.Instance.Read<String>(String.Format("IsVip_{0}", LoginUser.Current.Profile.Id));
            var hasPayed = LocalSettingUtility.Instance.Read<String>("HasPayed");

            if ((String.IsNullOrEmpty(isVip) == false && isVip == Boolean.TrueString)
                || (String.IsNullOrEmpty(hasPayed) == false && hasPayed == Boolean.TrueString))
                return nightModeFormat;

#if DEBUG
            var license = CurrentAppSimulator.LicenseInformation;
#else
            var license = CurrentApp.LicenseInformation;
#endif

            if (license.ProductLicenses["NightModeIAP"].IsActive == false)
            {
                nightModeFormat += "(付费后解锁)";
            }

            return nightModeFormat;
        }
        
        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
