using System;
using System.Threading.Tasks;

using Zhihu.Common.Model.Result;
using Zhihu.Common.Net;


namespace Zhihu.Common.Service.Runtime
{
// ReSharper disable once ClassNeverInstantiated.Global
    public sealed class PersonService : IPerson
    {
        public async Task<UnReadResult> CheckUnReadAsync(String access)
        {
            var api = new UserApi();
            var result = await api.CheckUnReadAsync(access);

            return result;
        }

        public async Task<ExplorePeopleResult> GetAmazingGuysAsync(String accessToken)
        {
            var api = new UserApi();
            var result = await api.ExplorePeopleAsync(accessToken);

            return result;
        }

        public async Task<FollowingResult> FollowAsync(String accessToken, String person)
        {
            var api = new UserApi();
            var result = await api.FollowAsync(accessToken, person);

            return result;
        }

        public async Task<FollowingResult> UnFollowAsync(String accessToken, String person, String loginUser)
        {
            var api = new UserApi();
            var result = await api.UnfollowAsync(accessToken, person, loginUser);

            return result;
        }

        public async Task<ProfileResult> GetProfileAsync(String accessToken, String userId = "self",
            Boolean autoCache = false)
        {
            var api = new UserApi();
            var result = await api.GetProfileAsync(accessToken, userId, autoCache);

            return result;
        }

        public async Task<NotificationsResult> CheckNotificationsAsync(String accessToken)
        {
            var api = new UserApi();
            var result = await api.CheckNotificationsAsync(accessToken);

            return result;
        }

        public async Task<ActivitiesResult> GetActivitiesAsync(String access, String request,
            Boolean autoCache = false)
        {
            var api = new UserApi();
            var result = await api.GetActivtiesAsync(access, request, autoCache);

            return result;
        }

        public async Task<PeopleFollowingResult> CheckFollowingAsync(String accessToken, String userId,
            Boolean autoCache = false)
        {
            var api = new UserApi();
            var result = await api.CheckFollowingAsync(accessToken, userId, autoCache);

            return result;
        }


        public async Task<AnswersResult> GetAnswersAsync(String access, String request, String orderBy,
            Boolean autoCache = false)
        {
            var api = new UserApi();
            var result = await api.GetAnswersAsync(access, request, orderBy, autoCache);

            return result;
        }


        public async Task<QuestionsResult> GetQuestionsAsync(String access, String request,
            Boolean autoCache = false)
        {
            var api = new UserApi();
            var result = await api.GetQuestionsAsync(access, request, autoCache);

            return result;
        }


        public async Task<CollectionsResult> GetCollectionsAsync(string access, string request,
            Boolean autoCache = false)
        {
            var api = new UserApi();
            var result = await api.GetCollectionsAsync(access, request, autoCache);

            return result;
        }


        public async Task<ColumnsResult> GetColumnsAsync(String access, String request,
            Boolean autoCache = false)
        {
            var api = new UserApi();
            var result = await api.GetColumnsAsync(access, request, autoCache);

            return result;
        }

        public async Task<ListResultBase> GetFollowingTopicsAsync(String access, String request,
            Boolean autoCache = false)
        {
            var api = new UserApi();
            var result = await api.GetFollowingTopicsAsync(access, request, autoCache);

            return result;
        }

        public async Task<ListResultBase> GetFollowersAsync(String access, String request,
            Boolean autoCache = false)
        {
            var api = new UserApi();
            var result = await api.GetFollowersAsync(access, request, autoCache);

            return result;
        }

        public async Task<ListResultBase> GetFolloweesAsync(String access, String request,
            Boolean autoCache = false)
        {
            var api = new UserApi();
            var result = await api.GetFolloweesAsync(access, request, autoCache);

            return result;
        }
    }
}
