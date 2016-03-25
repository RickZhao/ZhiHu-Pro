using System;
using System.Threading.Tasks;

using Newtonsoft.Json;

using Zhihu.Common.Helper;
using Zhihu.Common.Model;
using Zhihu.Common.Model.Result;


namespace Zhihu.Common.Net
{
    public class HotApi
    {
        internal async Task<EditorRecommendsResult> GetRecommendsAsync(String accessToken, String request, Boolean autoCache = false)
        {
            var client = new HttpUtility();

            var response = await client.GetAsync(Utility.Instance.BaseUri, request, accessToken, autoCache);

            if (false == String.IsNullOrEmpty(response.Json))
            {
                var json = response.Json;
                //json = Regex.Unescape(json);

                var obj = JsonConvert.DeserializeObject<EditorRecommends>(json);

                return new EditorRecommendsResult(obj);
            }
            else
            {
                var json = response.Error;
                //json = Regex.Unescape(json);

                return new EditorRecommendsResult(new Exception(json));
            }
        }

        internal async Task<BannerResult> GetBannerAsync(String accessToken, String request, Boolean autoCache = false)
        {
            var client = new HttpUtility();

            var response = await client.GetAsync(Utility.Instance.BaseUri, request, accessToken, autoCache);

            if (false == String.IsNullOrEmpty(response.Json))
            {
                var json = response.Json;
                //json = Regex.Unescape(json);

                var obj = JsonConvert.DeserializeObject<Banner>(json);

                return new BannerResult(obj);
            }
            else
            {
                var json = response.Error;
                //json = Regex.Unescape(json);

                return new BannerResult(new Exception(json));
            }
        }

        internal async Task<HotCollectionsResult> GetCollectionsAsync(String accessToken, String request, Boolean autoCache = false)
        {
            var client = new HttpUtility();

            var response = await client.GetAsync(Utility.Instance.BaseUri, request, accessToken, autoCache);

            if (false == String.IsNullOrEmpty(response.Json))
            {
                var json = response.Json;
                //json = Regex.Unescape(json);

                var obj = JsonConvert.DeserializeObject<HotCollections>(json);

                return new HotCollectionsResult(obj);
            }
            else
            {
                var json = response.Error;
                //json = Regex.Unescape(json);

                return new HotCollectionsResult(new Exception(json));
            }
        }

        internal async Task<HotAnswersResult> GetTodayHotAnswersAsync(String accessToken, String request,
            Boolean autoCache = false)
        {
            var client = new HttpUtility();

            var response = await client.GetAsync(Utility.Instance.BaseUri, request, accessToken, autoCache);

            if (false == String.IsNullOrEmpty(response.Json))
            {
                var json = response.Json;
                //json = Regex.Unescape(json);

                var obj = JsonConvert.DeserializeObject<HotAnswers>(json);

                return new HotAnswersResult(obj);
            }
            else
            {
                var json = response.Error;
                //json = Regex.Unescape(json);

                return new HotAnswersResult(new Exception(json));
            }
        }

        internal async Task<HotTopicsCollectionsResult> GetTopicsCollectionsAsync(String accessToken, String request, Boolean autoCache = false)
        {
            var client = new HttpUtility();

            var response = await client.GetAsync(Utility.Instance.BaseUri, request, accessToken, autoCache);

            if (false == String.IsNullOrEmpty(response.Json))
            {
                var json = response.Json;
                //json = Regex.Unescape(json);

                var obj = JsonConvert.DeserializeObject<HotTopicsCollections>(json);

                return new HotTopicsCollectionsResult(obj);
            }
            else
            {
                var json = response.Error;
                //json = Regex.Unescape(json);

                return new HotTopicsCollectionsResult(new Exception(json));
            }
        }
    }
}