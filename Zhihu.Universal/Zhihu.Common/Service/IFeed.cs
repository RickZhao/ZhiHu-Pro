using System;
using System.Threading.Tasks;

using Zhihu.Common.Model.Result;


namespace Zhihu.Common.Service
{
    public interface IFeed
    {
        Task<FeedsResult> GetFeedsAsync(String accessToken, String uri, Boolean autoCache = false);

        //Task<QuestionsResult> SearchQuestionAsync(String accessToken, String keyword, Int32 offset,
        //    Int32 limit);

        //Task<CreateQuesResult> PutQuestionAsync(String accessToken, Boolean isAnonymous, String topicIds,
        //    String title, String detail);

        //Task<PeopleFollowingResult> IsPeopleFollowingAsync(String accessToken, String userId, Int32 offset, Int32 limit);

    }
}
