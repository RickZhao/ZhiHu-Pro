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

        public async Task<OperationResult> HasReadContentsAsync(String access)
        {
            var api = new NotifyApi();
            var result = await api.HasReadContentsAsync(access);

            return result;
        }


        public async Task<OperationResult> HasReadFollowsAsync(String access)
        {
            var api = new NotifyApi();
            var result = await api.HasReadFollowsAsync(access);

            return result;
        }

        public async Task<OperationResult> HasReadLikeAsync(string access)
        {
            var api = new NotifyApi();
            var result = await api.HasReadLikessAsync(access);

            return result;
        } 

        public async Task<NotifyItemResult> HasReadContentAsync(String access, String contentId)
        {
            var api = new NotifyApi();
            var result = await api.HasReadContentAsync(access, contentId);

            return result;
        }
    }
}
