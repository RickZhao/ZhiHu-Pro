using System;
using System.Threading.Tasks;

using Zhihu.Common.Model.Result;
using Zhihu.Common.Net;


namespace Zhihu.Common.Service.Runtime
{
// ReSharper disable once ClassNeverInstantiated.Global
    public class TopicService : ITopic
    {
        public async Task<TopicResult> GetDetailAsync(String access, int topicId, Boolean autoCache = false)
        {
            var api = new TopicApi();
            var result = await api.GetDetailAsync(access, topicId, autoCache);

            return result;
        }

        public async Task<FollowingResult> CheckFollowingAsync(String access, int topicId,
            Boolean autoCache = false)
        {
            var api = new TopicApi();
            var result = await api.CheckFolloingAsync(access, topicId, autoCache);

            return result;
        }

        public async Task<FollowingResult> FollowAsync(String access, Int32 topicId)
        {
            var api = new TopicApi();
            var result = await api.FollowAsync(access, topicId);

            return result;
        }

        public async Task<FollowingResult> UnFollowAsync(String access, Int32 topicId, String loginId)
        {
            var api = new TopicApi();
            var result = await api.UnFollowAsync(access, topicId, loginId);

            return result;
        }

        public async Task<TopicActivitiesResult> GetActivitiesAsync(String access, String request,
            Boolean autoCache = false)
        {
            var api = new TopicApi();
            var result = await api.GetActivitiesAsync(access, request, autoCache);

            return result;
        }

        public async Task<TopicBestAnswersResult> GetBestAnswersAsync(String access, String request,
            Boolean autoCache = false)
        {
            var api = new TopicApi();
            var result = await api.GetBestAnswersAsync(access, request, autoCache);

            return result;
        }

        public async Task<AuthorsResult> GetBestAnswerersAsync(String access, String request,
            Boolean autoCache = false)
        {
            var api = new TopicApi();
            var result = await api.GetBestAnswerersAsync(access, request, autoCache);

            return result;
        }

        public async Task<QuestionsResult> GetQuestionsAsync(String access, String request,
            Boolean autoCache = false)
        {
            var api = new TopicApi();
            var result = await api.GetQuestionsAsync(access, request, autoCache);

            return result;
        }

        public async Task<TopicsResult> GetFatherTopicsAsync(String access, String request,
            Boolean autoCache = false)
        {
            var api = new TopicApi();
            var result = await api.GetFatherTopicsAsync(access, request, autoCache);

            return result;
        }
    }
}