using System;
using System.Threading.Tasks;

using Zhihu.Common.Model;
using Zhihu.Common.Net;


namespace Zhihu.Common.Service.Runtime
{
// ReSharper disable once ClassNeverInstantiated.Global
    public sealed class SettingService : ISetting
    {
        public async Task<UseTopStoryResult> CheckUseStory(String access, Boolean autoCache = false)
        {
            var api = new SettingApi();
            var result = await api.CheckUseStory(access, autoCache);

            return result;
        }

        public async Task<UseTopStoryResult> SwitchTopStory(String access, Boolean isOn)
        {
            var api = new SettingApi();
            var result = await api.SwitchTopStory(access, isOn);

            return result;
        }
    }
}
