using System;
using System.Threading.Tasks;

using Zhihu.Common.Model.Result;


namespace Zhihu.Common.Service
{
    public interface IPerson
    {
        Task<ProfileResult> GetProfileAsync(String accessToken, String userId = "self", Boolean autoCache = false);
        Task<UnReadResult> CheckUnReadAsync(String access);
        Task<NotificationsResult> CheckNotificationsAsync(String accessToken);
        Task<PeopleFollowingResult> CheckFollowingAsync(String accessToken, String userId, Boolean autoCache = false);

        Task<AnswersResult> GetAnswersAsync(String access, String request, String orderBy, Boolean autoCache = false);
        Task<QuestionsResult> GetQuestionsAsync(String access, String request, Boolean autoCache = false);
        Task<ActivitiesResult> GetActivitiesAsync(String access, String request, Boolean autoCache = false);
        Task<CollectionsResult> GetCollectionsAsync(String access, String request, Boolean autoCache = false);
        Task<ColumnsResult> GetColumnsAsync(String access, String request, Boolean autoCache = false);

        Task<FollowingResult> FollowAsync(String accessToken, String person);
        Task<FollowingResult> UnFollowAsync(String accessToken, String person, String loginUser);
        Task<ExplorePeopleResult> GetAmazingGuysAsync(String accessToken);

        Task<ListResultBase> GetFollowingTopicsAsync(String access, String request, Boolean autoCache = false);
        Task<ListResultBase> GetFollowersAsync(String access, String request, Boolean autoCache = false);
        Task<ListResultBase> GetFolloweesAsync(String access, String request, Boolean autoCache = false);
    }
}
