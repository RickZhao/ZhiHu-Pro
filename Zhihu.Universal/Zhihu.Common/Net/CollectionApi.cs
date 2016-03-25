using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

using Newtonsoft.Json;

using Zhihu.Common.Helper;
using Zhihu.Common.Model;
using Zhihu.Common.Model.Result;


namespace Zhihu.Common.Net
{
    public sealed class CollectionApi
    {
        internal async Task<CollectionResult> GetDetailAsync(String accessToken, Int32 collectionId, Boolean autoCache = false)
        {
            var client = new HttpUtility();

            var response =
                await
                    client.GetAsync(Utility.Instance.BaseUri, String.Format("collections/{0}", collectionId),
                        accessToken, autoCache);

            if (false == String.IsNullOrEmpty(response.Json))
            {
                var json = response.Json;
                //json = Regex.Unescape(json);

                var obj = JsonConvert.DeserializeObject<Collection>(json);

                return new CollectionResult(obj);
            }
            else
            {
                var json = response.Error;
                //json = Regex.Unescape(json);

                return new CollectionResult(new Exception(json));
            }
        }

        internal async Task<CollectionAnswersResult> GetAnswersAsync(String accessToken, String request, Boolean autoCache = false)
        {
            var client = new HttpUtility();

            var response = await client.GetAsync(Utility.Instance.BaseUri, request, accessToken, autoCache);

            if (false == String.IsNullOrEmpty(response.Json))
            {
                var json = response.Json;
                //json = Regex.Unescape(json);

                var obj = JsonConvert.DeserializeObject<CollectionAnswers>(json);

                return new CollectionAnswersResult(obj);
            }
            else
            {
                var json = response.Error;
                //json = Regex.Unescape(json);

                return new CollectionAnswersResult(new Exception(json));
            }
        }

        internal async Task<FollowingResult> GetFollowingAsync(String accessToken, Int32 collectionId, Boolean autoCache = false)
        {
            var client = new HttpUtility();

            var response =
                await
                    client.GetAsync(Utility.Instance.BaseUri,
                        String.Format("collections/{0}/is_following", collectionId),
                        accessToken, autoCache);

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

        internal async Task<FollowingResult> FollowAsync(String accessToken, Int32 collectionId)
        {
            var client = new HttpUtility();

            var body = new StringContent(String.Empty);
            body.Headers.ContentType = new MediaTypeHeaderValue("application/x-www-form-urlencoded");

            var response =
                await
                    client.PostAsync(Utility.Instance.BaseUri,
                        String.Format("collections/{0}/followers", collectionId), body,
                        accessToken);

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

        internal async Task<UnFollowResult> UnFollowAsync(String accessToken, Int32 collectionId, String userId)
        {
            var client = new HttpUtility();

            var response =
                await
                    client.DeleteAsync(Utility.Instance.BaseUri,
                        String.Format("collections/{0}/followers/{1}", collectionId, userId),
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

        internal async Task<CreateCollectionResult> CreateAsync(String accessToken, Boolean isPublic, String title,
            String description)
        {
            var client = new HttpUtility();

            var body =
                new StringContent(String.Format("is_public={0}&title={1}&description={2}",
                    isPublic == true ? "true" : "false",
                    WebUtility.UrlEncode(title),
                    WebUtility.UrlEncode(description)));

            body.Headers.ContentType = new MediaTypeHeaderValue("application/x-www-form-urlencoded");

            var response =
                await
                    client.PostAsync(Utility.Instance.BaseUri, "collections", body,
                           accessToken);

            if (false == String.IsNullOrEmpty(response.Json))
            {
                var json = response.Json;
                //json = Regex.Unescape(json);

                var obj = JsonConvert.DeserializeObject<Collection>(json);

                return new CreateCollectionResult(obj);
            }
            else
            {
                var json = response.Error;
                //json = Regex.Unescape(json);

                return new CreateCollectionResult(new Exception(json));
            }
        }

        internal async Task<CollectionsResult> QueryAsync(String accessToken, Int32 questionId, Int32 offset, Int32 limit, Boolean autoCache = false)
        {
            var client = new HttpUtility();

            var response =
                await
                    client.GetAsync(Utility.Instance.BaseUri,
                        String.Format("collections/{0}/collections_v2?offset={1}&limit={2}", questionId, offset, limit, autoCache),
                        accessToken);

            if (false == String.IsNullOrEmpty(response.Json))
            {
                var json = response.Json;
                //json = Regex.Unescape(json);

                var obj = JsonConvert.DeserializeObject<Collections>(json);

                return new CollectionsResult(obj);
            }
            else
            {
                var json = response.Error;
                //json = Regex.Unescape(json);

                return new CollectionsResult(new Exception(json));
            }
        }

        internal async Task<CollectionsResult> GetMyCollectionsAsync(String accessToken, String request, Boolean autoCache = false)
        {
            var client = new HttpUtility();

            var response = await client.GetAsync(Utility.Instance.BaseUri, request, accessToken, autoCache);

            if (false == String.IsNullOrEmpty(response.Json))
            {
                var json = response.Json;
                //json = Regex.Unescape(json);

                var obj = JsonConvert.DeserializeObject<Collections>(json);

                return new CollectionsResult(obj);
            }
            else
            {
                var json = response.Error;
                //json = Regex.Unescape(json);

                return new CollectionsResult(new Exception(json));
            }
        }

        internal async Task<CollectionsResult> GetAnswerCollectionsAsync(String access, Int32 answerId, Int32 limit, Boolean autoCache = false)
        {
            var client = new HttpUtility();

            var response =
                await
                    client.GetAsync(Utility.Instance.BaseUri,
                        String.Format("answers/{0}/collections_v2?limit={1}", answerId, limit), access, autoCache);


            if (false == String.IsNullOrEmpty(response.Json))
            {
                var json = response.Json;
                //json = Regex.Unescape(json);

                var obj = JsonConvert.DeserializeObject<Collections>(json);

                return new CollectionsResult(obj);
            }
            else
            {
                var json = response.Error;
                //json = Regex.Unescape(json);

                return new CollectionsResult(new Exception(json));
            }
        }

        internal async Task<OperationResult> CollectAnswerAsync(String access, Int32 answerId,
            List<Int32> collectionIds)
        {
            var client = new HttpUtility();

            var ids = collectionIds.Aggregate(String.Empty, (current, id) => current + String.Format("{0},", id));
            
            if (ids.LastOrDefault() == ',')
            {
                ids = ids.Substring(0, ids.Length - 1);
            }

            var body =
                new StringContent(String.Format("collection_ids={0}", ids));

            body.Headers.ContentType = new MediaTypeHeaderValue("application/x-www-form-urlencoded");

            var response =
                await
                    client.PutAsync(Utility.Instance.BaseUri, String.Format("answers/{0}/collections", answerId), access,
                        body);

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
    }
}