using System;
using System.Threading.Tasks;

using Zhihu.Common.Model;
using Zhihu.Common.Model.Result;


namespace Zhihu.Common.Service
{
    public interface IComment
    {
        Task<CommentResult> CommentArticleAsync(String access, Int32 articleId, String content);
        Task<CommentResult> CommentAnswerAsync(String access, Int32 answerId, String content);
        Task<CommentResult> CommentQuestionAsync(String access, Int32 questionId, String content);

        Task<VotedResult> VoteUp(String access, Int32 commentId);
        Task<VotedResult> CancelVoteUp(String access, Int32 commentId, String loginId);

        Task<CommentResult> ReplyAnswerComment(String access, Int32 answerId, Int32 commentId, String content);
        Task<CommentResult> ReplyArticleComment(String access, Int32 answerId, Int32 commentId, String content);
        Task<CommentResult> ReplyQuestionComment(String access, Int32 questionId, Int32 commentId, String content);

        Task<OperationResult> ReportComment(String access, Int32 commentId, ReasonMode reason);
    }
}
