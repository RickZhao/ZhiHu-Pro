using System;
using System.Threading.Tasks;

using Zhihu.Common.Model.Result;


namespace Zhihu.Common.Service.Design
{
// ReSharper disable once ClassNeverInstantiated.Global
    public sealed class DesignTopicService : ITopic
    {
        public Task<TopicResult> GetDetailAsync(String access, int topicId, Boolean autoCache = false)
        {
            throw new NotImplementedException();
        }

        public Task<FollowingResult> CheckFollowingAsync(String access, int topicId,
            Boolean autoCache = false)
        {
            throw new NotImplementedException();
        }

        public Task<FollowingResult> FollowAsync(String access, Int32 topicId)
        {
            throw new NotImplementedException();
        }

        public Task<FollowingResult> UnFollowAsync(String access, Int32 topicId, String loginId)
        {
            throw new NotImplementedException();
        }

        public Task<TopicActivitiesResult> GetActivitiesAsync(String access, String request,
            Boolean autoCache = false)
        {
            throw new NotImplementedException();
        }

        public Task<TopicBestAnswersResult> GetBestAnswersAsync(String access, String request,
            Boolean autoCache = false)
        {
            throw new NotImplementedException();
        }

        public Task<AuthorsResult> GetBestAnswerersAsync(String access, String request,
            Boolean autoCache = false)
        {
            throw new NotImplementedException();
        }

        public Task<QuestionsResult> GetQuestionsAsync(String access, String request,
            Boolean autoCache = false)
        {
            throw new NotImplementedException();
        }

        public Task<TopicsResult> GetFatherTopicsAsync(String access, String request,
            Boolean autoCache = false)
        {
            throw new NotImplementedException();
        }
    }
}
