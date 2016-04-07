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

        public Task<OperationResult> DismissContentsNotifyAsync(String access)
        {
            throw new NotImplementedException();
        }

        public Task<NotifiesResult> CheckFollowsAsync(string access, string request, bool autoCache = false)
        {
            throw new NotImplementedException();
        }

        public Task<OperationResult> DismissFollowsNotifyAsync(string access)
        {
            throw new NotImplementedException();
        }

        public Task<OperationResult> DismissLikeNotifyAsync(string access)
        {
            throw new NotImplementedException();
        }

        public Task<NotifyItemResult> DismissContentNotifyAsync(String access, String contentId)
        {
            throw new NotImplementedException();
        }

    }
}
