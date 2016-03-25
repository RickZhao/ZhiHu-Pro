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
        Task<OperationResult> HasReadContentsAsync(String access);
        Task<OperationResult> HasReadFollowsAsync(String access);
        Task<NotifyItemResult> HasReadContentAsync(String access, String contentId);
    }
}
