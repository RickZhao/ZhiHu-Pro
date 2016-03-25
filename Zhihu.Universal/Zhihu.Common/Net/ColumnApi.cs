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
    public sealed class ColumnApi
    {
        internal async Task<ColumnResult> GetDetailAsync(String access, String columnId, Boolean autoCache = false)
        {
            var http = new HttpUtility();

            var response = await http.GetAsync(Utility.Instance.BaseUri, String.Format("columns/{0}", columnId), access, autoCache);

            if (false == String.IsNullOrEmpty(response.Json))
            {
                var json = response.Json;
                //json = Regex.Unescape(json);

                var obj = JsonConvert.DeserializeObject<Column>(json);

                return new ColumnResult(obj);
            }
            else
            {
                var json = response.Error;
                //json = Regex.Unescape(json);

                return new ColumnResult(new Exception(json));
            }
        }

        internal async Task<FollowingResult> CheckFollowingAsync(String access, String columnId, Boolean autoCache = false)
        {
            var client = new HttpUtility();

            var response =
                await
                    client.GetAsync(Utility.Instance.BaseUri,
                        String.Format("columns/{0}/is_following", columnId),
                        access, autoCache);

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

        internal async Task<ArticlesResult> GetArticles(String access, String request, Boolean autoCache = false)
        {
            var client = new HttpUtility();

            var response = await client.GetAsync(Utility.Instance.BaseUri, request, access, autoCache);

            if (false == String.IsNullOrEmpty(response.Json))
            {
                var json = response.Json;
                //json = Regex.Unescape(json);

                var obj = JsonConvert.DeserializeObject<Articles>(json);

                return new ArticlesResult(obj);
            }
            else
            {
                var json = response.Error;
                //json = Regex.Unescape(json);

                return new ArticlesResult(new Exception(json));
            }
        }

        internal async Task<FollowResult> FollowAsync(String access, String columnId)
        {
            var client = new HttpUtility();

            var body = new StringContent(String.Empty);
            body.Headers.ContentType = new MediaTypeHeaderValue("application/x-www-form-urlencoded");

            var response =
                await
                    client.PostAsync(Utility.Instance.BaseUri,
                        String.Format("columns/{0}/followers", columnId), body,
                        access);

            if (false == String.IsNullOrEmpty(response.Json))
            {
                var json = response.Json;
                //json = Regex.Unescape(json);

                var obj = JsonConvert.DeserializeObject<Operation>(json);

                return new FollowResult(obj);
            }
            else
            {
                var json = response.Error;
                //json = Regex.Unescape(json);

                return new FollowResult(new Exception(json));
            }
        }

        internal async Task<UnFollowResult> UnFollowAsync(String accessToken, String columnId, String loginId)
        {
            var client = new HttpUtility();

            var response =
                await
                    client.DeleteAsync(Utility.Instance.BaseUri,
                        String.Format("columns/{0}/followers/{1}", columnId, loginId),
                        accessToken);

            if (false == String.IsNullOrEmpty(response.Json))
            {
                var json = response.Json;
                //json = Regex.Unescape(json);

                var obj = JsonConvert.DeserializeObject<Operation>(json);

                return new UnFollowResult(obj);
            }
            else
            {
                var json = response.Error;
                //json = Regex.Unescape(json);

                return new UnFollowResult(new Exception(json));
            }
        }
    }
}