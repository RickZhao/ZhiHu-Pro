using System;
using System.Threading.Tasks;

using Zhihu.Common.Model.Result;


namespace Zhihu.Common.Service.Design
{
// ReSharper disable once ClassNeverInstantiated.Global
    public class DesignFindService : IFind
    {
        public Task<EditorRecommendsResult> GetRecommends(String accessToken, String request,
            Boolean autoCache = false)
        {
            throw new NotImplementedException();
        }

        public Task<BannerResult> GetBanner(String accessToken, String request, Boolean autoCache = false)
        {
            throw new NotImplementedException();
        }

        public Task<HotCollectionsResult> GetCollections(String accessToken, String request, Boolean autoCache = false)
        {
            throw new NotImplementedException();
        }

        public Task<HotAnswersResult> GetTodayHotAnswers(string accessToken, string request, bool autoCache = false)
        {
            throw new NotImplementedException();
        }

        public Task<HotTopicsCollectionsResult> GetTopicAndCollections(String accessToken, String request, Boolean autoCache = false)
        {
            throw new NotImplementedException();
        }
    }
}
