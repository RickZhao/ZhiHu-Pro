using System;
using System.Threading.Tasks;

using Zhihu.Common.Model.Result;


namespace Zhihu.Common.Service.Design
{
    public sealed class DesignTableService : ITable
    {
        public Task<TableResult> GetDetailAsync(string access, string tableId, bool autoCache = false)
        {
            throw new NotImplementedException();
        }

        public Task<CommentsResult> GetCommentsAsync(string access, string requestUri, bool autoCache = false)
        {
            throw new NotImplementedException();
        }

        public Task<TableActivitiesResult> GetActivtiesAsync(string access, string request, bool autoCache = false)
        {
            throw new NotImplementedException();
        }

        public Task<TableQuestionsResult> GetQuestionsAsync(string access, string request, bool autoCache = false)
        {
            throw new NotImplementedException();
        }

        public Task<FollowingResult> FollowAsync(string accessToken, string tableId)
        {
            throw new NotImplementedException();
        }

        public Task<FollowingResult> UnFollowAsync(string accessToken, string tableId, string userId)
        {
            throw new NotImplementedException();
        }
    }
}
