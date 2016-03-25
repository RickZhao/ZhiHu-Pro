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
    public sealed class ArticleApi
    {
        internal async Task<ArticleResult> GetDetailAsync(String accessToken, Int32 articleId, Boolean autoCache = false)
        {
            var http = new HttpUtility();

            var response = await http.GetAsync(Utility.Instance.BaseUri, String.Format("articles/{0}", articleId), accessToken, autoCache);

            if (false == String.IsNullOrEmpty(response.Json))
            {
                var json = response.Json;
                //json = Regex.Unescape(json);

                var obj = JsonConvert.DeserializeObject<Article>(json);

                return new ArticleResult(obj);
            }
            else
            {
                var json = response.Error;
                //json = Regex.Unescape(json);

                return new ArticleResult(new Exception(json));
            }
        }

        internal async Task<CommentsResult> GetCommentsAsync(String accessToken, Int32 articleId, String request, Boolean autoCache = false)
        {
            var http = new HttpUtility();

            var response =
                await
                    http.GetAsync(Utility.Instance.BaseUri, String.Format(request, articleId),
                        accessToken, autoCache);

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

        internal async Task<VoteResult> VoteUpAsync(String access, Int32 articleId)
        {
            var http = new HttpUtility();

            var body = new StringContent("voting=1");
            body.Headers.ContentType = new MediaTypeHeaderValue("application/x-www-form-urlencoded");

            var response =
                await
                    http.PostAsync(Utility.Instance.BaseUri, String.Format("articles/{0}/voters", articleId),
                        body, access);

            if (false == String.IsNullOrEmpty(response.Json))
            {
                var json = response.Json;
                //json = Regex.Unescape(json);

                var obj = JsonConvert.DeserializeObject<Vote>(json);

                return new VoteResult(obj);
            }
            else
            {
                var json = response.Error;
                //json = Regex.Unescape(json);

                return new VoteResult(new Exception(json));
            } 
        }

        internal async Task<VoteResult> CancelVoteUpAsync(String access, Int32 articleId)
        {
            var http = new HttpUtility();

            var body = new StringContent("voting=0");
            body.Headers.ContentType = new MediaTypeHeaderValue("application/x-www-form-urlencoded");

            var response =
                await
                    http.PostAsync(Utility.Instance.BaseUri, String.Format("articles/{0}/voters", articleId),
                        body, access);

            if (false == String.IsNullOrEmpty(response.Json))
            {
                var json = response.Json;
                //json = Regex.Unescape(json);

                var obj = JsonConvert.DeserializeObject<Vote>(json);

                return new VoteResult(obj);
            }
            else
            {
                var json = response.Error;
                //json = Regex.Unescape(json);

                return new VoteResult(new Exception(json));
            } 
        }

        internal async Task<VoteResult> VoteDownAsync(String access, Int32 articleId, String loginId)
        {
            var http = new HttpUtility();
         
            var body = new StringContent("voting=-1");
            body.Headers.ContentType = new MediaTypeHeaderValue("application/x-www-form-urlencoded");

            var response =
                await
                    http.PostAsync(Utility.Instance.BaseUri,
                        String.Format("articles/{0}/voters", articleId),
                        body, access);

            if (false == String.IsNullOrEmpty(response.Json))
            {
                var json = response.Json;
                //json = Regex.Unescape(json);

                var obj = JsonConvert.DeserializeObject<Vote>(json);

                return new VoteResult(obj);
            }
            else
            {
                var json = response.Error;
                //json = Regex.Unescape(json);

                return new VoteResult(new Exception(json));
            } 
        }

        internal async Task<VoteResult> CancelVoteDownAsync(String access, Int32 articleId, String loginId)
        {
            var http = new HttpUtility();

            var body = new StringContent("voting=0");
            body.Headers.ContentType = new MediaTypeHeaderValue("application/x-www-form-urlencoded");

            var response =
                await
                    http.PostAsync(Utility.Instance.BaseUri,
                        String.Format("articles/{0}/voters", articleId),
                        body, access);

            if (false == String.IsNullOrEmpty(response.Json))
            {
                var json = response.Json;
                //json = Regex.Unescape(json);

                var obj = JsonConvert.DeserializeObject<Vote>(json);

                return new VoteResult(obj);
            }
            else
            {
                var json = response.Error;
                //json = Regex.Unescape(json);

                return new VoteResult(new Exception(json));
            } 
        }
    }
}