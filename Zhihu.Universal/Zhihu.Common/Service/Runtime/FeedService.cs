using System;
using System.Threading.Tasks;

using Zhihu.Common.Model.Result;
using Zhihu.Common.Net;

namespace Zhihu.Common.Service.Runtime
{
// ReSharper disable once ClassNeverInstantiated.Global
    public sealed class FeedService : IFeed
    {
        public async Task<FeedsResult> GetFeedsAsync(String accessToken, String requestUri,
            Boolean autoCache = false)
        {
            var api = new UserApi();
            var result = await api.GetFeedsAsync(accessToken, requestUri, autoCache);

            return result;
        }

        //public async Task<QuestionsResult> SearchQuestionAsync(String accessToken, String keyword, Int32 offset,
        //    Int32 limit)
        //{
        //    var api = new QuestionApi();
        //    var result = await api.SearchAsync(accessToken, keyword, offset, limit);

        //    return result;
        //}

        //public async Task<CreateQuesResult> PutQuestionAsync(String accessToken, bool isAnonymous, String topicIds,
        //    String title, String detail)
        //{
        //    var api = new QuestionApi();
        //    var result = await api.CreateAsync(accessToken, isAnonymous, topicIds, title, detail);

        //    return result;
        //}

        //public async Task<PeopleFollowingResult> IsPeopleFollowingAsync(String accessToken, String userId, Int32 offset,
        //    Int32 limit)
        //{
        //    var api = new AnswerApi();
        //    var result = await api.IsPeopleFollowingAsync(accessToken, userId, offset, limit);

        //    return result;
        //}
    }
}
