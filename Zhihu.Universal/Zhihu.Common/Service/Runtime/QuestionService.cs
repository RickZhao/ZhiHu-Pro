using System;
using System.Threading.Tasks;

using Zhihu.Common.Model.Result;
using Zhihu.Common.Net;


namespace Zhihu.Common.Service.Runtime
{
// ReSharper disable once ClassNeverInstantiated.Global
    public class QuestionService : IQuestion
    {
        public async Task<InviteResult> Invite(String access, Int32 questionId, String personId)
        {
            var api = new QuestionApi();
            var result = await api.Invite(access, questionId, personId);

            return result;
        }

        public async Task<CreateQuesResult> CreateAsync(String accessToken, Boolean isAnonymous, String topicIds,
            String title, String detail)
        {
            var api = new QuestionApi();
            var result = await api.CreateAsync(accessToken, isAnonymous, topicIds, title, detail);

            return result;
        }

        public async Task<QuestionResult> GetDetailAsync(String accessToken, Int32 questionId,
            Boolean autoCache = false)
        {
            var api = new QuestionApi();
            var result = await api.GetDetailAsync(accessToken, questionId, autoCache);

            return result;
        }

        public async Task<QuesRelaResult> GetRelationshipAsync(String accessToken, Int32 questionId,
            Boolean autoCache = false)
        {
            var api = new QuestionApi();
            var result = await api.GetRelationshipAsync(accessToken, questionId, autoCache);

            return result;
        }

        public async Task<TopicsResult> GetTopicsAsync(String accessToken, Int32 questionId,
            Boolean autoCache = false)
        {
            var api = new QuestionApi();
            var result = await api.GetTopicsAsync(accessToken, questionId, autoCache);

            return result;
        }

        public async Task<AnswersResult> GetAnswersAsync(String accessToken, String requestUri,
            Boolean autoCache = false)
        {
            var api = new QuestionApi();
            var result = await api.GetAnswersAsync(accessToken, requestUri, autoCache);

            return result;
        }

        public async Task<CommentsResult> GetCommentsAsync(String accessToken, String requestUri,
            Boolean autoCache = false)
        {
            var api = new QuestionApi();
            var result = await api.GetCommentsAsync(accessToken, requestUri, autoCache);

            return result;
        }


        public async Task<FollowingResult> FollowAsync(String accessToken, Int32 questionId)
        {
            var api = new QuestionApi();
            var result = await api.FollowAsync(accessToken, questionId);

            return result;
        }

        public async Task<FollowingResult> UnFollowAsync(String accessToken, Int32 questionId, String userId)
        {
            var api = new QuestionApi();
            var result = await api.UnFollowAsync(accessToken, questionId, userId);

            return result;
        }

        public async Task<AnswerResult> AnswerAsync(String accessToken, Int32 questionId, String answer)
        {
            var api = new QuestionApi();
            var result = await api.AnswerAsync(accessToken, questionId, answer);

            return result;
        }


        public async Task<AnonymousResult> AnonymousAsync(String access, Int32 questionId)
        {
            var api = new QuestionApi();
            var result = await api.AnonymousAsync(access, questionId);

            return result;
        }

        public async Task<AnonymousResult> CancelAnonymousAsync(String access, Int32 questionId)
        {
            var api = new QuestionApi();
            var result = await api.CancelAnonymousAsync(access, questionId);

            return result;
        }
    }
}
