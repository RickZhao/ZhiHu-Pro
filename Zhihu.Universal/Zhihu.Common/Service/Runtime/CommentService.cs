using System;
using System.Threading.Tasks;
using Zhihu.Common.Model;
using Zhihu.Common.Model.Result;
using Zhihu.Common.Net;


namespace Zhihu.Common.Service.Runtime
{
// ReSharper disable once ClassNeverInstantiated.Global
    public sealed class CommentService : IComment
    {
        public async Task<CommentResult> CommentArticleAsync(String access, Int32 articleId, String content)
        {
            var api = new CommentApi();
            var result = await api.CommentArticleAsync(access, articleId, content);

            return result;
        }

        public async Task<CommentResult> CommentAnswerAsync(String access, Int32 answerId, String content)
        {
            var api = new CommentApi();
            var result = await api.CommentAnswerAsync(access, answerId, content);

            return result;
        }

        public async Task<CommentResult> CommentQuestionAsync(String access, Int32 questionId, String content)
        {
            var api = new CommentApi();
            var result = await api.CommentQuestionAsync(access, questionId, content);

            return result;
        }

        public async Task<VotedResult> VoteUp(String access, Int32 commentId)
        {
            var api = new CommentApi();
            var result = await api.VoteUp(access, commentId);

            return result;
        }

        public async Task<VotedResult> CancelVoteUp(String access, Int32 commentId, String loginId)
        {
            var api = new CommentApi();
            var result = await api.CancelVoteUp(access, commentId, loginId);

            return result;
        }

        public async Task<CommentResult> ReplyAnswerComment(String access, Int32 answerId, Int32 commentId, String content)
        {
            var api = new CommentApi();
            var result = await api.ReplyAnswerComment(access, answerId, commentId, content);

            return result;
        }

        public async Task<CommentResult> ReplyArticleComment(String access, Int32 answerId, Int32 commentId,
            String content)
        {
            var api = new CommentApi();
            var result = await api.ReplyArticleComment(access, answerId, commentId, content);

            return result;
        }

        public async Task<CommentResult> ReplyQuestionComment(String access, Int32 questionId, Int32 commentId,
            String content)
        {
            var api = new CommentApi();
            var result = await api.ReplyQuestionComment(access, questionId, commentId, content);

            return result;
        }

        public async Task<OperationResult> ReportComment(String access, Int32 commentId, ReasonMode reason)
        {
            var api = new CommentApi();
            var result = await api.ReportComment(access, commentId, reason);

            return result;
        }
    }
}
