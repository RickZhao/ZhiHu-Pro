using System;
using System.Threading.Tasks;

using Zhihu.Common.Model.Result;


namespace Zhihu.Common.Service
{
    public interface IColumn
    {
        Task<ColumnResult> GetDetailAsync(String access, String columnId, Boolean autoCache = false);
        Task<FollowingResult> CheckFollowingAsync(String access, String columnId, Boolean autoCache = false);
        Task<ArticlesResult> GetArticles(String access, String request, Boolean autoCache = false);
        Task<FollowResult> FollowAsync(String access, String columnId);
        Task<UnFollowResult> UnFollowAsync(String accessToken, String columnId, String loginId);
    }
}
