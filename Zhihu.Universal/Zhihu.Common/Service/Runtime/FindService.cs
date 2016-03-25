using System;
using System.Threading.Tasks;

using Zhihu.Common.Model.Result;
using Zhihu.Common.Net;


namespace Zhihu.Common.Service.Runtime
{
// ReSharper disable once ClassNeverInstantiated.Global
    public class FindService : IFind
    {
        public Task<EditorRecommendsResult> GetRecommends(String accessToken, String request,
            Boolean autoCache = false)
        {
            var api = new HotApi();
            var result = api.GetRecommendsAsync(accessToken, request, autoCache);

            return result;
        }

        public Task<BannerResult> GetBanner(String accessToken, String request, Boolean autoCache = false)
        {
            var api = new HotApi();
            var result = api.GetBannerAsync(accessToken, request, autoCache);

            return result;
        }

        public Task<HotCollectionsResult> GetCollections(String accessToken, String request,
            Boolean autoCache = false)
        {
            var api = new HotApi();
            var result = api.GetCollectionsAsync(accessToken, request, autoCache);

            return result;
        }

        public Task<HotAnswersResult> GetTodayHotAnswers(string accessToken, string request, bool autoCache = false)
        {
            var api = new HotApi();
            var result = api.GetTodayHotAnswersAsync(accessToken, request, autoCache);

            return result;
        }





        public Task<HotTopicsCollectionsResult> GetTopicAndCollections(String accessToken, String request,
            Boolean autoCache = false)
        {
            var api = new HotApi();
            var result = api.GetTopicsCollectionsAsync(accessToken, request, autoCache);

            return result;
        }
    }
}
