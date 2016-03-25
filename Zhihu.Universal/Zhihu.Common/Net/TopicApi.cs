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
    public sealed class TopicApi
    {
        internal async Task<TopicsResult> SearchAsync(String accessToken, String keyword, Int32 offset, Int32 limit, Boolean autoCache = false)
        {
            var client = new HttpUtility();
            var response =
                await
                    client.GetAsync(Utility.Instance.BaseUri,
                        String.Format("search?q={0}&t=topic&offset={1}&limit={2}",
                            keyword, offset, limit), accessToken, autoCache);

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

        public async Task<TopicResult> GetDetailAsync(String accessToken, Int32 topicId, Boolean autoCache = false)
        {
            var client = new HttpUtility();
         
            var response =
                await
                    client.GetAsync(Utility.Instance.BaseUri,
                        String.Format("topics/{0}", topicId), accessToken, autoCache);

            if (false == String.IsNullOrEmpty(response.Json))
            {
                var json = response.Json;
                //json = Regex.Unescape(json);

                var obj = JsonConvert.DeserializeObject<Topic>(json);

                return new TopicResult(obj);
            }
            else
            {
                var json = response.Error;
                //json = Regex.Unescape(json);

                return new TopicResult(new Exception(json));
            }
        }

        internal async Task<FollowingResult> CheckFolloingAsync(String accessToken, Int32 topicId, Boolean autoCache = false)
        {
            var client = new HttpUtility();

            var response =
                await
                    client.GetAsync(Utility.Instance.BaseUri, String.Format("topics/{0}/is_following", topicId),
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

        internal async Task<TopicBestAnswersResult> GetBestAnswersAsync(String accessToken, String request, Boolean autoCache = false)
        {
            var client = new HttpUtility();

            var response = await client.GetAsync(Utility.Instance.BaseUri, request, accessToken, autoCache);

            if (false == String.IsNullOrEmpty(response.Json))
            {
                var json = response.Json;
                //json = Regex.Unescape(json);

                var obj = JsonConvert.DeserializeObject<TopicBestAnswers>(json);

                return new TopicBestAnswersResult(obj);
            }
            else
            {
                var json = response.Error;
                //json = Regex.Unescape(json);

                return new TopicBestAnswersResult(new Exception(json));
            }
        }

        internal async Task<AuthorsResult> GetBestAnswerersAsync(String access, String request, Boolean autoCache = false)
        {
            var client = new HttpUtility();

            var response = await client.GetAsync(Utility.Instance.BaseUri, request, access, autoCache);

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

        internal async Task<QuestionsResult> GetQuestionsAsync(String accessToken, String request, Boolean autoCache = false)
        {
            var client = new HttpUtility();

            var response = await client.GetAsync(Utility.Instance.BaseUri, request, accessToken, autoCache);

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

        internal async Task<TopicsResult> GetFatherTopicsAsync(String accessToken, String request, Boolean autoCache = false)
        {
            var client = new HttpUtility();
        
            var response =
                await
                    client.GetAsync(Utility.Instance.BaseUri, request, accessToken, autoCache);

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

        internal async Task<TopicActivitiesResult> GetActivitiesAsync(String access, String request, Boolean autoCache = false)
        {
            var client = new HttpUtility();

            var response =
                await
                    client.GetAsync(Utility.Instance.BaseUri, request, access, autoCache);

            if (false == String.IsNullOrEmpty(response.Json))
            {
                var json = response.Json;
                //json = Regex.Unescape(json);

                var obj = JsonConvert.DeserializeObject<TopicActivities>(json);

                return new TopicActivitiesResult(obj);
            }
            else
            {
                var json = response.Error;
                //json = Regex.Unescape(json);

                return new TopicActivitiesResult(new Exception(json));
            }
        }
     
        internal async Task<FollowingResult> FollowAsync(String access, Int32 topicId)
        {
            var client = new HttpUtility();

            var body = new StringContent(String.Empty);
            body.Headers.ContentType = new MediaTypeHeaderValue("application/x-www-form-urlencoded");

            var response =
                await
                    client.PostAsync(Utility.Instance.BaseUri,
                        String.Format("topics/{0}/followers", topicId), body,
                        access);

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

        internal async Task<FollowingResult> UnFollowAsync(String access, Int32 topicId, String loginId)
        {
            var client = new HttpUtility();

            var response =
                await
                    client.DeleteAsync(Utility.Instance.BaseUri,
                        String.Format("topics/{0}/followers/{1}", topicId, loginId),
                        access);

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
