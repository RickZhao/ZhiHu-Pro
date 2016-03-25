using System;
using System.Threading.Tasks;

using Zhihu.Common.Model.Result;


namespace Zhihu.Common.Service.Design
{
// ReSharper disable once ClassNeverInstantiated.Global
    public sealed class DesignArticleService : IArticle
    {
        public Task<ArticleResult> GetDetailAsync(String accessToken, Int32 articleId, Boolean autoCache = false)
        {
            throw new NotImplementedException();
        }

        public Task<CommentsResult> GetCommentsAsync(String accessToken, Int32 articleId, String request, Boolean autoCache = false)
        {
            throw new NotImplementedException();
        }

        public Task<VoteResult> VoteUpAsync(String access, Int32 articleId)
        {
            throw new NotImplementedException();
        }

        public Task<VoteResult> CancelVoteUpAsync(String access, Int32 articleId)
        {
            throw new NotImplementedException();
        }

        public Task<VoteResult> VoteDownAsync(String access, Int32 articleId, String loginId)
        {
            throw new NotImplementedException();
        }

        public Task<VoteResult> CancelVoteDownAsync(String access, Int32 articleId, String loginId)
        {
            throw new NotImplementedException();
        }
    }
}
