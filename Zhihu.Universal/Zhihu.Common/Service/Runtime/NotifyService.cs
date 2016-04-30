using System;
using System.Threading.Tasks;

using Zhihu.Common.Model.Result;
using Zhihu.Common.Net;


namespace Zhihu.Common.Service.Runtime
{
    public sealed class NotifyService : INotify
    {
        public async Task<NotifiesResult> CheckFollowsAsync(String access, String request, Boolean autoCache = false)
        {
            var api = new NotifyApi();
            var result = await api.CheckFollowsAsync(access, request, autoCache);

            return result;
        }

        public async Task<NotifiesResult> CheckNotifiesAsync(String access, String request, Boolean autoCache = false)
        {
            var api = new NotifyApi();
            var result = await api.GetContentsAsync(access, request, autoCache);

            return result;
        }

        public async Task<NotifiesResult> CheckLikesAync(String access, String request, Boolean autoCache = false)
        {
            var api = new NotifyApi();
            var result = await api.GetLikesAync(access, request, autoCache);

            return result;
        }

        public async Task<OperationResult> DismissContentsNotifyAsync(String access)
        {
            var api = new NotifyApi();
            var result = await api.DismissContentsNotifyAsync(access);

            return result;
        }


        public async Task<OperationResult> DismissFollowsNotifyAsync(String access)
        {
            var api = new NotifyApi();
            var result = await api.DismissFollowsNotifyAsync(access);

            return result;
        }

        public async Task<OperationResult> DismissLikeNotifyAsync(string access)
        {
            var api = new NotifyApi();
            var result = await api.DismissLikesNotifyAsync(access);

            return result;
        }

        public async Task<NotifyItemResult> DismissContentNotifyAsync(String access, String contentId)
        {
            var api = new NotifyApi();
            var result = await api.DismissContentNotifyAsync(access, contentId);

            return result;
        }
    }
}
