using System;
using System.Threading.Tasks;

using Zhihu.Common.Model.Result;


namespace Zhihu.Common.Service
{
    public interface ITopic
    {
        Task<TopicResult> GetDetailAsync(String access, Int32 topicId, Boolean autoCache = false);

        Task<FollowingResult> CheckFollowingAsync(String access, Int32 topicId, Boolean autoCache = false);
        Task<FollowingResult> FollowAsync(String access, Int32 topicId);
        Task<FollowingResult> UnFollowAsync(String access, Int32 topicId, String loginId);

        Task<TopicActivitiesResult> GetActivitiesAsync(String access, String request, Boolean autoCache = false);
        Task<TopicBestAnswersResult> GetBestAnswersAsync(String access, String request, Boolean autoCache = false);
        Task<AuthorsResult> GetBestAnswerersAsync(String access, String request, Boolean autoCache = false);

        Task<QuestionsResult> GetQuestionsAsync(String access, String request, Boolean autoCache = false);
        Task<TopicsResult> GetFatherTopicsAsync(String access, String request, Boolean autoCache = false);
    }
}
