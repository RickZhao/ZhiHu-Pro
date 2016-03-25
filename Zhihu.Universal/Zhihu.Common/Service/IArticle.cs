using System;
using System.Threading.Tasks;

using Zhihu.Common.Model.Result;


namespace Zhihu.Common.Service
{
    public interface IArticle
    {
        Task<ArticleResult> GetDetailAsync(String accessToken, Int32 articleId, Boolean autoCache = false);
        Task<CommentsResult> GetCommentsAsync(String accessToken, Int32 articleId, String request, Boolean autoCache = false);

        Task<VoteResult> VoteUpAsync(String access, Int32 articleId);
        Task<VoteResult> CancelVoteUpAsync(String access, Int32 articleId);
        Task<VoteResult> VoteDownAsync(String access, Int32 articleId, String loginId);
        Task<VoteResult> CancelVoteDownAsync(String access, Int32 articleId, String loginId);
    }
}
