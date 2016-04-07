using System;
using System.Threading.Tasks;

using Zhihu.Common.Model.Result;


namespace Zhihu.Common.Service
{
    public interface INotify
    {
        Task<NotifiesResult> CheckFollowsAsync(String access, String request, Boolean autoCache = false);
        Task<NotifiesResult> CheckNotifiesAsync(String access, String request, Boolean autoCache = false);
        Task<NotifiesResult> CheckLikesAync(String access, String request, Boolean autoCache = false);
        Task<OperationResult> DismissContentsNotifyAsync(String access);
        Task<OperationResult> DismissFollowsNotifyAsync(String access);
        Task<OperationResult> DismissLikeNotifyAsync(String access);
        Task<NotifyItemResult> DismissContentNotifyAsync(String access, String contentId);
    }
}
