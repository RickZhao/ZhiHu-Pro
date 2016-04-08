using System;
using System.Threading.Tasks;

using Zhihu.Common.Model.Result;


namespace Zhihu.Common.Service.Design
{
    public sealed class DesignNotifyService : INotify
    {
        public Task<NotifiesResult> CheckNotifiesAsync(string access, string request, bool autoCache = false)
        {
            throw new NotImplementedException();
        }

        public Task<NotifiesResult> CheckLikesAync(string access, string request, bool autoCache = false)
        {
            throw new NotImplementedException();
        }

        public Task<OperationResult> HasReadContentsAsync(String access)
        {
            throw new NotImplementedException();
        }

        public Task<NotifiesResult> CheckFollowsAsync(string access, string request, bool autoCache = false)
        {
            throw new NotImplementedException();
        }

        public Task<OperationResult> HasReadFollowsAsync(string access)
        {
            throw new NotImplementedException();
        }

        public Task<OperationResult> HasReadLikeAsync(string access)
        {
            throw new NotImplementedException();
        }

        public Task<NotifyItemResult> HasReadContentAsync(String access, String contentId)
        {
            throw new NotImplementedException();
        }

    }
}
