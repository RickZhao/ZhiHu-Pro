using System;
using System.Threading.Tasks;

using Zhihu.Common.Model;


namespace Zhihu.Common.Service
{
    public interface ISetting
    {
        Task<UseTopStoryResult> CheckUseStory(String access, Boolean autoCache = false);
        Task<UseTopStoryResult> SwitchTopStory(String access, Boolean isOn);
    }
}
