using System;
using System.Threading.Tasks;

using Zhihu.Common.Model.Result;
using Zhihu.Common.Net;


namespace Zhihu.Common.Service.Runtime
{
// ReSharper disable once ClassNeverInstantiated.Global
    public sealed class ArticleService : IArticle
    {
        public async Task<ArticleResult> GetDetailAsync(String accessToken, Int32 articleId,
            Boolean autoCache = false)
        {
            var api = new ArticleApi();
            var result = await api.GetDetailAsync(accessToken, articleId, autoCache);

            return result;
        }


        public async Task<CommentsResult> GetCommentsAsync(String accessToken, Int32 articleId, String request,
            Boolean autoCache = false)
        {
            var api = new ArticleApi();
            var result = await api.GetCommentsAsync(accessToken, articleId, request, autoCache);

            return result;
        }

        public async Task<VoteResult> VoteUpAsync(String access, Int32 articleId)
        {
            var api = new ArticleApi();
            var result = await api.VoteUpAsync(access, articleId);

            return result;
        }

        public async Task<VoteResult> CancelVoteUpAsync(String access, Int32 articleId)
        {
            var api = new ArticleApi();
            var result = await api.CancelVoteUpAsync(access, articleId);

            return result;
        }

        public async Task<VoteResult> VoteDownAsync(String access, Int32 articleId, String loginId)
        {
            var api = new ArticleApi();
            var result = await api.VoteDownAsync(access, articleId, loginId);

            return result;
        }

        public async Task<VoteResult> CancelVoteDownAsync(String access, Int32 articleId, String loginId)
        {
            var api = new ArticleApi();
            var result = await api.CancelVoteDownAsync(access, articleId, loginId);

            return result;
        }
    }
}