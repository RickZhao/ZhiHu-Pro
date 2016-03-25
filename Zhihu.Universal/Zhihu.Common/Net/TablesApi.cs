using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

using Newtonsoft.Json;

using Zhihu.Common.Helper;
using Zhihu.Common.Model;
using Zhihu.Common.Model.Result;


namespace Zhihu.Common.Net
{
    public sealed class TablesApi
    {
        internal async Task<TableResult> GetDetailAsync(String access, String tableId, Boolean autoCache = false)
        {
            var http = new HttpUtility();

            var response =
                await
                    http.GetAsync(Utility.Instance.BaseUri, String.Format("roundtables/{0}", tableId), access, autoCache);

            if (false == String.IsNullOrEmpty(response.Json))
            {
                var json = response.Json;
                //json = Regex.Unescape(json);

                var obj = JsonConvert.DeserializeObject<RoundTable>(json);

                return new TableResult(obj);
            }
            else
            {
                var json = response.Error;
                //json = Regex.Unescape(json);

                return new TableResult(new Exception(json));
            }
        }

        internal async Task<CommentsResult> GetCommentsAsync(String access, String requestUri, Boolean autoCache = false)
        {
            var client = new HttpUtility();

            var response = await client.GetAsync(Utility.Instance.BaseUri, requestUri, access, autoCache);

            if (false == String.IsNullOrEmpty(response.Json))
            {
                var json = response.Json;
                //json = Regex.Unescape(json);

                var obj = JsonConvert.DeserializeObject<Comments>(json);

                return new CommentsResult(obj);
            }
            else
            {
                var json = response.Error;
                //json = Regex.Unescape(json);

                return new CommentsResult(new Exception(json));
            }
        }

        internal async Task<TableActivitiesResult> GetActivtiesAsync(String access, String request,
            Boolean autoCache = false)
        {
            var client = new HttpUtility();

            var response = await client.GetAsync(Utility.Instance.BaseUri, request, access, autoCache);

            if (false == String.IsNullOrEmpty(response.Json))
            {
                var json = response.Json;
                //json = Regex.Unescape(json);

                var obj = JsonConvert.DeserializeObject<TableActivities>(json);

                return new TableActivitiesResult(obj);
            }
            else
            {
                var json = response.Error;
                //json = Regex.Unescape(json);

                return new TableActivitiesResult(new Exception(json));
            }
        }

        internal async Task<TableQuestionsResult> GetQuestionsAsync(String access, String request,
            Boolean autoCache = false)
        {
            var client = new HttpUtility();

            var response = await client.GetAsync(Utility.Instance.BaseUri, request, access, autoCache);

            if (false == String.IsNullOrEmpty(response.Json))
            {
                var json = response.Json;
                //json = Regex.Unescape(json);

                var obj = JsonConvert.DeserializeObject<TableQuestions>(json);

                return new TableQuestionsResult(obj);
            }
            else
            {
                var json = response.Error;
                //json = Regex.Unescape(json);

                return new TableQuestionsResult(new Exception(json));
            }
        }

        internal async Task<FollowingResult> FollowAsync(String accessToken, String tableId)
        {
            var client = new HttpUtility();

            var body = new StringContent(String.Empty);
            body.Headers.ContentType = new MediaTypeHeaderValue("application/x-www-form-urlencoded");

            var response =
                await client.PostAsync(Utility.Instance.BaseUri, String.Format("roundtables/{0}/followers",
                    tableId), body, accessToken);

            if (false == String.IsNullOrEmpty(response.Json))
            {
                var json = response.Json;
                //json = Regex.Unescape(json);

                var obj = JsonConvert.DeserializeObject<Following>(json);

                return new FollowingResult(obj);
            }
            else
            {
                var json = response.Error;
                //json = Regex.Unescape(json);

                return new FollowingResult(new Exception(json));
            }
        }

        internal async Task<FollowingResult> UnFollowAsync(String accessToken, String tableId, String userId)
        {
            var client = new HttpUtility();

            var response =
                await
                    client.DeleteAsync(Utility.Instance.BaseUri,
                        String.Format("roundtables/{0}/followers/{1}", tableId, userId), accessToken);

            if (false == String.IsNullOrEmpty(response.Json))
            {
                var json = response.Json;
                //json = Regex.Unescape(json);

                var obj = JsonConvert.DeserializeObject<Following>(json);

                return new FollowingResult(obj);
            }
            else
            {
                var json = response.Error;
                //json = Regex.Unescape(json);

                return new FollowingResult(new Exception(json));
            }
        }
    }
}
