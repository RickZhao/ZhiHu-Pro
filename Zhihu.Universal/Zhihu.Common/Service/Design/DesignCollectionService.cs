using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using Zhihu.Common.Model.Result;


namespace Zhihu.Common.Service.Design
{
// ReSharper disable once ClassNeverInstantiated.Global
    public sealed class DesignCollectionService : ICollection
    {
        public Task<CollectionResult> GetDetail(String accessToken, Int32 collectionId,
            Boolean autoCache = false)
        {
            throw new NotImplementedException();
        }


        public Task<CollectionAnswersResult> GetAnswers(String accessToken, String request, Boolean autoCache = false)
        {
            throw new NotImplementedException();
        }

        public Task<FollowingResult> GetFollowing(String accessToken, int collectionId, Boolean autoCache = false)
        {
            throw new NotImplementedException();
        }

        public Task<FollowingResult> Follow(String accessToken, int collectionId)
        {
            throw new NotImplementedException();
        }

        public Task<UnFollowResult> UnFollow(String accessToken, int collectionId, String userId)
        {
            throw new NotImplementedException();
        }


        public Task<CollectionsResult> GetAnswerCollections(string access, int answerId, int limit, Boolean autoCache = false)
        {
            throw new NotImplementedException();
        }

        public Task<CollectionsResult> GetPersonCollections(string access, string request, Boolean autoCache = false)
        {
            throw new NotImplementedException();
        }


        public Task<OperationResult> CollectionAnswer(string access, int answerId, List<Int32> collectionIds)
        {
            throw new NotImplementedException();
        }

        public Task<CreateCollectionResult> CreateAsync(string accessToken, bool isPublic, string title, string description)
        {
            throw new NotImplementedException();
        }
    }
}
