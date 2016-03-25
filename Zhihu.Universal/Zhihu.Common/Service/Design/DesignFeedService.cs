using System;
using System.Threading.Tasks;

using Newtonsoft.Json;

using Zhihu.Common.Helper;
using Zhihu.Common.Model;
using Zhihu.Common.Model.Result;


namespace Zhihu.Common.Service.Design
{
// ReSharper disable once ClassNeverInstantiated.Global
    public sealed class DesignFeedService : IFeed
    {
        public async Task<FeedsResult> GetFeedsAsync(String accessToken, String uri, Boolean autoCache = false)
        {
            var json = Utility.Instance.DemoFeeds;
            var obj = JsonConvert.DeserializeObject<Feeds>(json);

            await Task.Delay(100);

            return new FeedsResult(obj);
        }

        //public Task<QuestionsResult> SearchQuestionAsync(String accessToken, String keyword, Int32 offset, Int32 limit)
        //{
        //    throw new NotImplementedException();
        //}

        //public Task<CreateQuesResult> PutQuestionAsync(String accessToken, bool isAnonymous, String topicIds, String title, String detail)
        //{
        //    throw new NotImplementedException();
        //}

        //public Task<PeopleFollowingResult> IsPeopleFollowingAsync(String accessToken, String userId, Int32 offset, Int32 limit)
        //{
        //    throw new NotImplementedException();
        //}

    }
}
