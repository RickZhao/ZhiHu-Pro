using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using Zhihu.Common.Model.Result;


namespace Zhihu.Common.Service
{
    public interface ICollection
    {
        Task<CreateCollectionResult> CreateAsync(String accessToken, Boolean isPublic, String title, String description);

        Task<CollectionResult> GetDetail(String accessToken, Int32 collectionId, Boolean autoCache = false);
        Task<CollectionAnswersResult> GetAnswers(String accessToken, String request, Boolean autoCache = false);
        Task<FollowingResult> GetFollowing(String accessToken, Int32 collectionId, Boolean autoCache = false);

        Task<FollowingResult> Follow(String accessToken, Int32 collectionId);
        Task<UnFollowResult> UnFollow(String accessToken, Int32 collectionId, String userId);

        Task<CollectionsResult> GetAnswerCollections(String access, Int32 answerId, Int32 limit, Boolean autoCache = false);
        Task<CollectionsResult> GetPersonCollections(String access, String request, Boolean autoCache = false);
        Task<OperationResult> CollectionAnswer(String access, Int32 answerId, List<Int32> collectionIds);
    }
}
