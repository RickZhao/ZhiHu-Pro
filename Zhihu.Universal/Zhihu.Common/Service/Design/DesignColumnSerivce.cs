using System;
using System.Threading.Tasks;

using Zhihu.Common.Model.Result;


namespace Zhihu.Common.Service.Design
{
// ReSharper disable once ClassNeverInstantiated.Global
    public class DesignColumnSerivce : IColumn
    {
        public Task<ColumnResult> GetDetailAsync(string access, string columnId, Boolean autoCache = false)
        {
            throw new NotImplementedException();
        }

        public Task<FollowingResult> CheckFollowingAsync(string access, string columnId, Boolean autoCache = false)
        {
            throw new NotImplementedException();
        }

        public Task<ArticlesResult> GetArticles(string access, string request, Boolean autoCache = false)
        {
            throw new NotImplementedException();
        }

        public Task<FollowResult> FollowAsync(string access, string columnId)
        {
            throw new NotImplementedException();
        }

        public Task<UnFollowResult> UnFollowAsync(string accessToken, string columnId, string loginId)
        {
            throw new NotImplementedException();
        }
    }
}
