using System;
using System.Threading.Tasks;

using Newtonsoft.Json;

using Zhihu.Common.Helper;
using Zhihu.Common.Model;
using Zhihu.Common.Model.Result;


namespace Zhihu.Common.Net
{
    public sealed class SearchApi
    {
        internal async Task<AuthorsResult> FindPeopleAsync(String access, String request, Boolean autoCache = false)
        {
            var client = new HttpUtility();

            var response =
                await
                    client.GetAsync(Utility.Instance.BaseUri, request, access, autoCache);

            if (false == String.IsNullOrEmpty(response.Json))
            {
                var json = response.Json;
                //json = Regex.Unescape(json);

                var obj = JsonConvert.DeserializeObject<Authors>(json);

                return new AuthorsResult(obj);
            }
            else
            {
                var json = response.Error;
                //json = Regex.Unescape(json);

                return new AuthorsResult(new Exception(json));
            }
        }

        internal async Task<SearchResult> FindContentsAsync(String access, String request, Boolean autoCache = false)
        {
            var client = new HttpUtility();

            var response =
                await
                    client.GetAsync(Utility.Instance.BaseUri, request, access, autoCache);

            if (false == String.IsNullOrEmpty(response.Json))
            {
                var json = response.Json;
                //json = Regex.Unescape(json);

                var obj = JsonConvert.DeserializeObject<SearchItems>(json);

                return new SearchResult(obj);
            }
            else
            {
                var json = response.Error;
                //json = Regex.Unescape(json);

                return new SearchResult(new Exception(json));
            }
        }

        internal async Task<QuestionsResult> FindQuestionsAsync(String access, String request, Boolean autoCache = false)
        {
            var client = new HttpUtility();

            var response =
                await
                    client.GetAsync(Utility.Instance.BaseUri, request, access, autoCache);

            if (false == String.IsNullOrEmpty(response.Json))
            {
                var json = response.Json;
                //json = Regex.Unescape(json);

                var obj = JsonConvert.DeserializeObject<Questions>(json);

                return new QuestionsResult(obj);
            }
            else
            {
                var json = response.Error;
                //json = Regex.Unescape(json);

                return new QuestionsResult(new Exception(json));
            }
        }

        internal async Task<TopicsResult> FindTopicsAsync(String access, String request, Boolean autoCache = false)
        {
            var client = new HttpUtility();

            var response =
                await
                    client.GetAsync(Utility.Instance.BaseUri, request, access, autoCache);

            if (false == String.IsNullOrEmpty(response.Json))
            {
                var json = response.Json;
                //json = Regex.Unescape(json);

                var obj = JsonConvert.DeserializeObject<Topics>(json);

                return new TopicsResult(obj);
            }
            else
            {
                var json = response.Error;
                //json = Regex.Unescape(json);

                return new TopicsResult(new Exception(json));
            }
        }
    }
}
