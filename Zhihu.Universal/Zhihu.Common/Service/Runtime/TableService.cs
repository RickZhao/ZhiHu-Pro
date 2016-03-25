using System.Threading.Tasks;

using Zhihu.Common.Model.Result;
using Zhihu.Common.Net;


namespace Zhihu.Common.Service.Runtime
{
    public sealed class TableService : ITable
    {
        public async Task<TableResult> GetDetailAsync(string access, string tableId, bool autoCache = false)
        {
            var api = new TablesApi();
            var result = await api.GetDetailAsync(access, tableId, autoCache);

            return result;
        }

        public async Task<CommentsResult> GetCommentsAsync(string access, string requestUri, bool autoCache = false)
        {
            var api = new TablesApi();
            var result = await api.GetCommentsAsync(access, requestUri, autoCache);

            return result;
        }

        public async Task<TableActivitiesResult> GetActivtiesAsync(string access, string request, bool autoCache = false)
        {
            var api = new TablesApi();
            var result = await api.GetActivtiesAsync(access, request, autoCache);

            return result;
        }

        public async Task<TableQuestionsResult> GetQuestionsAsync(string access, string request, bool autoCache = false)
        {
            var api = new TablesApi();
            var result = await api.GetQuestionsAsync(access, request, autoCache);

            return result;
        }

        public async Task<FollowingResult> FollowAsync(string access, string tableId)
        {
            var api = new TablesApi();
            var result = await api.FollowAsync(access, tableId);

            return result;
        }

        public async Task<FollowingResult> UnFollowAsync(string access, string tableId, string userId)
        {
            var api = new TablesApi();
            var result = await api.UnFollowAsync(access, tableId, userId);

            return result;
        }
    }
}
