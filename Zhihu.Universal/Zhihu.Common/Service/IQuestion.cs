using System;
using System.Threading.Tasks;

using Zhihu.Common.Model.Result;


namespace Zhihu.Common.Service
{
    public interface IQuestion
    {
        Task<InviteResult> Invite(String access, Int32 questionId, String personId);

        Task<CreateQuesResult> CreateAsync(String accessToken, Boolean isAnonymous, String topicIds, String title,
            String detail);

        Task<QuestionResult> GetDetailAsync(String accessToken, Int32 questionId, Boolean autoCache = false);

        Task<QuesRelaResult> GetRelationshipAsync(String accessToken, Int32 questionId, Boolean autoCache = false);

        Task<TopicsResult> GetTopicsAsync(String accessToken, Int32 questionId, Boolean autoCache = false);

        Task<AnswersResult> GetAnswersAsync(String accessToken, String requestUri, Boolean autoCache = false);
        Task<CommentsResult> GetCommentsAsync(String accessToken, String requestUri, Boolean autoCache = false);

        Task<AnswerResult> AnswerAsync(String accessToken, Int32 questionId, String answer);

        Task<FollowingResult> FollowAsync(String accessToken, Int32 questionId);
        Task<FollowingResult> UnFollowAsync(String accessToken, Int32 questionId, String userId);
       
        Task<AnonymousResult> AnonymousAsync(String access, Int32 questionId);
        Task<AnonymousResult> CancelAnonymousAsync(String access, Int32 questionId);
    }
}
