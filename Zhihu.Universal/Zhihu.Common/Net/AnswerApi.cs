using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

using Newtonsoft.Json;

using Zhihu.Common.Helper;
using Zhihu.Common.Model;
using Zhihu.Common.Model.Result;


namespace Zhihu.Common.Net
{
    public sealed class AnswerApi
    {
        internal async Task<AnswerDetailResult> GetDetailAsync(String accessToken, Int32 answerId, Boolean autoCache = false)
        {
            var http = new HttpUtility();

            var response =
                await http.GetAsync(Utility.Instance.BaseUri, String.Format("answers/{0}", answerId), accessToken, autoCache);

            if (false == String.IsNullOrEmpty(response.Json))
            {
                var json = response.Json;
                //json = Regex.Unescape(json);

                var obj = JsonConvert.DeserializeObject<AnswerDetail>(json);

                return new AnswerDetailResult(obj);
            }
            else
            {
                var json = response.Error;
                //json = Regex.Unescape(json);

                return new AnswerDetailResult(new Exception(json));
            }
        }

        internal async Task<AnswerRelationshipResult> GetRelationshipAsync(String accessToken, Int32 answerId, Boolean autoCache = false)
        {
            var http = new HttpUtility();

            var response =
                await
                    http.GetAsync(Utility.Instance.BaseUri,
                        String.Format("answers/{0}/relationship", answerId), accessToken, autoCache);

            if (false == String.IsNullOrEmpty(response.Json))
            {
                var json = response.Json;
                //json = Regex.Unescape(json);

                var obj = JsonConvert.DeserializeObject<AnswerRelationship>(json);

                return new AnswerRelationshipResult(obj);
            }
            else
            {
                var json = response.Error;
                //json = Regex.Unescape(json);

                return new AnswerRelationshipResult(new Exception(json));
            }
        }

        internal async Task<FavoriteResult> CheckFavoriteAsync(String access, Int32 answerId, Boolean autoCache = false)
        {
            var http = new HttpUtility();

            var response =
                await
                    http.GetAsync(Utility.Instance.BaseUri,
                        String.Format("answers/{0}/is_favorited", answerId), access, autoCache);

            if (false == String.IsNullOrEmpty(response.Json))
            {
                var json = response.Json;
                //json = Regex.Unescape(json);

                var obj = JsonConvert.DeserializeObject<Favorite>(json);

                return new FavoriteResult(obj);
            }
            else
            {
                var json = response.Error;
                //json = Regex.Unescape(json);

                return new FavoriteResult(new Exception(json));
            }
        }

        internal async Task<AnswerCommentsResult> GetCommentsAsync(String accessToken, String requestUri, Boolean autoCache = false)
        {
            var http = new HttpUtility();

            var response = await http.GetAsync(Utility.Instance.BaseUri, requestUri, accessToken, autoCache);

            if (false == String.IsNullOrEmpty(response.Json))
            {
                var json = response.Json;
                //json = Regex.Unescape(json);

                var obj = JsonConvert.DeserializeObject<AnswerComments>(json);

                return new AnswerCommentsResult(obj);
            }
            else
            {
                var json = response.Error;
                //json = Regex.Unescape(json);

                return new AnswerCommentsResult(new Exception(json));
            }
        }

        internal async Task<AnswerNoHelpeResult> BeNoHelpAsync(String accessToken, Int32 answerId)
        {
            var http = new HttpUtility();

            var body = new StringContent(String.Empty);
            body.Headers.ContentType = new MediaTypeHeaderValue("application/x-www-form-urlencoded");

            var response =
                await
                    http.PostAsync(Utility.Instance.BaseUri, String.Format("answers/{0}/nothelpers", answerId),
                        body, accessToken);

            if (false == String.IsNullOrEmpty(response.Json))
            {
                var json = response.Json;
                //json = Regex.Unescape(json);

                var obj = JsonConvert.DeserializeObject<AnswerNoHelp>(json);

                return new AnswerNoHelpeResult(obj);
            }
            else
            {
                var json = response.Error;
                //json = Regex.Unescape(json);

                return new AnswerNoHelpeResult(new Exception(json));
            }
        }

        internal async Task<AnswerNoHelpeResult> UndoNoHelpAsync(String accessToken, Int32 answerId, String userId)
        {
            var http = new HttpUtility();

            var response =
                await
                    http.DeleteAsync(Utility.Instance.BaseUri,
                        String.Format("answers/{0}/nothelpers/{1}", answerId, userId), accessToken);

            if (false == String.IsNullOrEmpty(response.Json))
            {
                var json = response.Json;
                //json = Regex.Unescape(json);

                var obj = JsonConvert.DeserializeObject<AnswerNoHelp>(json);

                return new AnswerNoHelpeResult(obj);
            }
            else
            {
                var json = response.Error;
                //json = Regex.Unescape(json);

                return new AnswerNoHelpeResult(new Exception(json));
            }
        }

        internal async Task<AnswerHelpedResult> ExpressThanksAsync(String accessToken, Int32 answerId)
        {
            var http = new HttpUtility();

            var body = new StringContent(String.Empty);
            body.Headers.ContentType = new MediaTypeHeaderValue("application/x-www-form-urlencoded");

            var response =
                await http.PostAsync(Utility.Instance.BaseUri, String.Format("answers/{0}/thankers",
                    answerId), body, accessToken);

            if (false == String.IsNullOrEmpty(response.Json))
            {
                var json = response.Json;
                //json = Regex.Unescape(json);

                var obj = JsonConvert.DeserializeObject<AnswerHelped>(json);

                return new AnswerHelpedResult(obj);
            }
            else
            {
                var json = response.Error;
                //json = Regex.Unescape(json);

                return new AnswerHelpedResult(new Exception(json));
            }
        }

        internal async Task<AnswerHelpedResult> CancelThanksAsync(String accessToken, Int32 answerId, String loginId)
        {
            var http = new HttpUtility();

            var response =
                await http.DeleteAsync(Utility.Instance.BaseUri, String.Format("answers/{0}/thankers/{1}", answerId, loginId), accessToken);

            if (false == String.IsNullOrEmpty(response.Json))
            {
                var json = response.Json;
                //json = Regex.Unescape(json);

                var obj = JsonConvert.DeserializeObject<AnswerHelped>(json);

                return new AnswerHelpedResult(obj);
            }
            else
            {
                var json = response.Error;
                //json = Regex.Unescape(json);

                return new AnswerHelpedResult(new Exception(json));
            }
        }

        internal async Task<OperationResult> CollectAsync(String accessToken, Int32 answerId,
            IEnumerable<Int32> collectionIds)
        {
            var http = new HttpUtility();

            var ids = collectionIds.Aggregate("collection_ids=", (current, id) => current + String.Format("{0},", id));
            ids = ids.TrimEnd(',');

            var body = new StringContent(ids);
            body.Headers.ContentType = new MediaTypeHeaderValue("application/x-www-form-urlencoded");

            var response =
                await
                    http.PutAsync(Utility.Instance.BaseUri, String.Format("/answers/{0}/collections", answerId),
                        accessToken, body);

            if (false == String.IsNullOrEmpty(response.Json))
            {
                var json = response.Json;
                //json = Regex.Unescape(json);

                var obj = JsonConvert.DeserializeObject<Operation>(json);

                return new OperationResult(obj);
            }
            else
            {
                var json = response.Error;
                //json = Regex.Unescape(json);

                return new OperationResult(new Exception(json));
            }
        }

        internal async Task<PeopleFollowingResult> IsPeopleFollowingAsync(String accessToken, String userId,
            Int32 offset, Int32 limit, Boolean autoCache = false)
        {
            var http = new HttpUtility();

            var response =
                await
                    http.GetAsync(Utility.Instance.BaseUri, String.Format("people/{0}/is_following", userId),
                        accessToken, autoCache);

            if (false == String.IsNullOrEmpty(response.Json))
            {
                var json = response.Json;
                //json = Regex.Unescape(json);

                var obj = JsonConvert.DeserializeObject<PeopleFollowing>(json);

                return new PeopleFollowingResult(obj);
            }
            else
            {
                var json = response.Error;
                //json = Regex.Unescape(json);

                return new PeopleFollowingResult(new Exception(json));
            }
        }

        internal async Task<VoteResult> VoteUpAsync(String access, Int32 answerId)
        {
            var http = new HttpUtility();

            var body = new StringContent("voting=1");
            body.Headers.ContentType = new MediaTypeHeaderValue("application/x-www-form-urlencoded");

            var response =
                await
                    http.PostAsync(Utility.Instance.BaseUri, String.Format("answers/{0}/voters", answerId),
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

        internal async Task<VoteResult> VoteDownAsync(String access, Int32 answerId)
        {
            var http = new HttpUtility();

            var body = new StringContent("voting=-1");
            body.Headers.ContentType = new MediaTypeHeaderValue("application/x-www-form-urlencoded");

            var response =
                await
                    http.PostAsync(Utility.Instance.BaseUri, String.Format("answers/{0}/voters", answerId),
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

        internal async Task<VoteResult> CancelVoteAsync(String access, Int32 answerId, String loginId)
        {
            var http = new HttpUtility();

            var response =
                await
                    http.DeleteAsync(Utility.Instance.BaseUri,
                        String.Format("answers/{0}/voters/{1}", answerId, loginId),
                        access);

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
