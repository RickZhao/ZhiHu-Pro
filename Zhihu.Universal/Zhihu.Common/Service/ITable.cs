using System;
using System.Threading.Tasks;

using Zhihu.Common.Model.Result;


namespace Zhihu.Common.Service
{
    public interface ITable
    {
        Task<TableResult> GetDetailAsync(String access, String tableId, Boolean autoCache = false);
        Task<CommentsResult> GetCommentsAsync(String access, String requestUri, Boolean autoCache = false);

        Task<TableActivitiesResult> GetActivtiesAsync(String access, String request,
            Boolean autoCache = false);

        Task<TableQuestionsResult> GetQuestionsAsync(String access, String request,
            Boolean autoCache = false);

        Task<FollowingResult> FollowAsync(String accessToken, String tableId);
        Task<FollowingResult> UnFollowAsync(String accessToken, String tableId, String userId);
    }
}
