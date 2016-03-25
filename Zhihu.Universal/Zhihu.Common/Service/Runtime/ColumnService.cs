using System;
using System.Threading.Tasks;

using Zhihu.Common.Model.Result;
using Zhihu.Common.Net;


namespace Zhihu.Common.Service.Runtime
{
// ReSharper disable once ClassNeverInstantiated.Global
    public sealed class ColumnService : IColumn
    {
        public async Task<ColumnResult> GetDetailAsync(String access, String columnId,
            Boolean autoCache = false)
        {
            var api = new ColumnApi();
            var result = await api.GetDetailAsync(access, columnId, autoCache);

            return result;
        }

        public async Task<FollowingResult> CheckFollowingAsync(string access, string columnId,
            Boolean autoCache = false)
        {
            var api = new ColumnApi();
            var result = await api.CheckFollowingAsync(access, columnId, autoCache);

            return result;
        }

        public async Task<ArticlesResult> GetArticles(string access, string request,
            Boolean autoCache = false)
        {
            var api = new ColumnApi();
            var result = await api.GetArticles(access, request, autoCache);

            return result;
        }

        public async Task<FollowResult> FollowAsync(String access, String columnId)
        {
            var api = new ColumnApi();
            var result = await api.FollowAsync(access, columnId);

            return result;
        }

        public async Task<UnFollowResult> UnFollowAsync(String access, String columnId, String loginId)
        {
            var api = new ColumnApi();
            var result = await api.UnFollowAsync(access, columnId, loginId);

            return result;
        }
    }
}
