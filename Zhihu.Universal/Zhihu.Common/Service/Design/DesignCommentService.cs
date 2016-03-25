using System;
using System.Threading.Tasks;

using Zhihu.Common.Model;
using Zhihu.Common.Model.Result;


namespace Zhihu.Common.Service.Design
{
// ReSharper disable once ClassNeverInstantiated.Global
    public sealed class DesignCommentService : IComment
    {
        public Task<CommentResult> CommentAnswerAsync(string access, int answerId, string content)
        {
            throw new NotImplementedException();
        }

        public Task<CommentResult> CommentQuestionAsync(string access, int questionId, string content)
        {
            throw new NotImplementedException();
        }

        public Task<VotedResult> VoteUp(string access, int commentId)
        {
            throw new NotImplementedException();
        }

        public Task<VotedResult> CancelVoteUp(string access, int commentId, string loginId)
        {
            throw new NotImplementedException();
        }

        public Task<CommentResult> ReplyAnswerComment(String access, Int32 answerId, Int32 commentId, String content)
        {
            throw new NotImplementedException();
        }

        public Task<CommentResult> CommentArticleAsync(string access, int articleId, string content)
        {
            throw new NotImplementedException();
        }

        public Task<CommentResult> ReplyArticleComment(String access, Int32 answerId, Int32 commentId,
            String content)
        {
            throw new NotImplementedException();
        }

        public Task<CommentResult> ReplyQuestionComment(String access, Int32 questionId, Int32 commentId,
            String content)
        {
            throw new NotImplementedException();
        }

        public Task<OperationResult> ReportComment(String access, Int32 commentId, ReasonMode reason)
        {
            throw new NotImplementedException();
        }
    }
}