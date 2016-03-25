using System;
using System.Threading.Tasks;

using Zhihu.Common.Model;


namespace Zhihu.Common.Service.Design
{
// ReSharper disable once ClassNeverInstantiated.Global
    public sealed class DesignSettingService : ISetting
    {
        public Task<UseTopStoryResult> CheckUseStory(string access, Boolean autoCache = false)
        {
            throw new NotImplementedException();
        }

        public Task<UseTopStoryResult> SwitchTopStory(string access, bool isOn)
        {
            throw new NotImplementedException();
        }
    }
}
