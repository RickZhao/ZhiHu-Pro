using System;
using System.Threading.Tasks;

using Zhihu.Common.Model.Result;


namespace Zhihu.Common.Service
{
    public interface IFind
    {
        Task<BannerResult> GetBanner(String accessToken, String request, Boolean autoCache = false);

        Task<EditorRecommendsResult> GetRecommends(String accessToken, String request, Boolean autoCache = false);

        Task<HotCollectionsResult> GetCollections(String accessToken, String request, Boolean autoCache = false);

        Task<HotAnswersResult> GetTodayHotAnswers(String accessToken, String request, Boolean autoCache = false);




        Task<HotTopicsCollectionsResult> GetTopicAndCollections(String accessToken, String request, Boolean autoCache = false);
    }
}
