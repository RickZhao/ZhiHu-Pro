using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using Zhihu.Common.Model.Result;
using Zhihu.Common.Net;


namespace Zhihu.Common.Service.Runtime
{
// ReSharper disable once ClassNeverInstantiated.Global
    public sealed class CollectionService : ICollection
    {
        public async Task<CreateCollectionResult> CreateAsync(String accessToken, Boolean isPublic, String title,
            String description)
        {
            var api = new CollectionApi();
            var result = await api.CreateAsync(accessToken, isPublic, title, description);

            return result;
        }

        public async Task<CollectionResult> GetDetail(String accessToken, Int32 collectionId,
            Boolean autoCache = false)
        {
            var api = new CollectionApi();
            var result = await api.GetDetailAsync(accessToken, collectionId, autoCache);

            return result;
        }

        public async Task<CollectionAnswersResult> GetAnswers(String accessToken, String request,
            Boolean autoCache = false)
        {
            var api = new CollectionApi();
            var result = await api.GetAnswersAsync(accessToken, request, autoCache);

            return result;
        }

        public async Task<FollowingResult> GetFollowing(String accessToken, Int32 collectionId,
            Boolean autoCache = false)
        {
            var api = new CollectionApi();
            var result = await api.GetFollowingAsync(accessToken, collectionId, autoCache);

            return result;
        }

        public async Task<FollowingResult> Follow(String accessToken, Int32 collectionId)
        {
            var api = new CollectionApi();
            var result = await api.FollowAsync(accessToken, collectionId);

            return result;
        }

        public async Task<UnFollowResult> UnFollow(String accessToken, Int32 collectionId, String userId)
        {
            var api = new CollectionApi();
            var result = await api.UnFollowAsync(accessToken, collectionId, userId);

            return result;
        }


        public async Task<CollectionsResult> GetAnswerCollections(String access, Int32 answerId, Int32 limit,
            Boolean autoCache = false)
        {
            var api = new CollectionApi();
            var result = await api.GetAnswerCollectionsAsync(access, answerId, limit, autoCache);

            return result;
        }

        public async Task<CollectionsResult> GetPersonCollections(String access, String request,
            Boolean autoCache = false)
        {
            var api = new CollectionApi();
            var result = await api.GetMyCollectionsAsync(access, request, autoCache);

            return result;
        }

        public async Task<OperationResult> CollectionAnswer(String access, Int32 answerId, List<Int32> collectionIds)
        {
            var api = new CollectionApi();
            var result = await api.CollectAnswerAsync(access, answerId, collectionIds);

            return result;
        }
    }
}