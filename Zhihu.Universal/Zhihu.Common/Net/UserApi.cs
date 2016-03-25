using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

using Newtonsoft.Json;

using Zhihu.Common.Helper;
using Zhihu.Common.Model;
using Zhihu.Common.Model.Result;


namespace Zhihu.Common.Net
{
    public sealed class UserApi
    {
        internal async Task<OperationResult> RegisterAsync(String email, String password, String lastName,
            String firstName, Int32 gender, String clientId)
        {
            var postBody = String.Format("email={0}&password={1}&lastname={2}&firstname={3}&gender={4}",
                WebUtility.UrlEncode(email),
                WebUtility.UrlEncode(password),
                WebUtility.UrlEncode(lastName),
                WebUtility.UrlEncode(firstName),
                gender);

            var body = new StringContent(postBody);
            body.Headers.ContentType = new MediaTypeHeaderValue("application/x-www-form-urlencoded");

            var http = new HttpUtility();

            var response = await http.PostAsync(Utility.Instance.BaseUri, "register", body, clientId, false);

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

        internal async Task<LoginResult> LoginAsync(String email, String password)
        {
            var postBody = String.Format("email={0}&password={1}&grant_type=password&client_id={2}&client_secret={3}",
                    WebUtility.UrlEncode(email),
                    WebUtility.UrlEncode(password),
                    Utility.Instance.ClientId,
                    Utility.Instance.ClientSecret);

            var body = new StringContent(postBody, Encoding.UTF8, "application/x-www-form-urlencoded");
            body.Headers.ContentType = new MediaTypeHeaderValue("application/x-www-form-urlencoded");

            var http = new HttpUtility();

            var response = await http.PostAsync(Utility.Instance.BaseUri, "token", body, String.Empty);

            if (false == String.IsNullOrEmpty(response.Json))
            {
                var json = response.Json;
                //json = Regex.Unescape(json);

                var obj = JsonConvert.DeserializeObject<UserInfo>(json);

                return new LoginResult(obj);
            }
            else
            {
                var json = response.Error;
                //json = Regex.Unescape(json);

                return new LoginResult(new Exception(json));
            }
        }

        internal async Task<ProfileResult> GetProfileAsync(String accessToken, String userId = "", Boolean autoCache = false)
        {
            var http = new HttpUtility();

            var response =
                await
                    http.GetAsync(Utility.Instance.BaseUri,
                        String.Format("people/{0}", String.IsNullOrEmpty(userId) ? "self" : userId), accessToken, autoCache);

            if (false == String.IsNullOrEmpty(response.Json))
            {
                var json = response.Json;
                //json = Regex.Unescape(json);

                var userInfo = JsonConvert.DeserializeObject<Profile>(json);

                return new ProfileResult(userInfo);
            }
            else
            {
                var json = response.Error;
                //json = Regex.Unescape(json);

                return new ProfileResult(new Exception(json));
            }
        }

        internal async Task<FeedsResult> GetFeedsAsync(String accessToken, String request, Boolean autoCache = false)
        {
            var client = new HttpUtility();

            var response = await client.GetAsync(Utility.Instance.BaseUri, request, accessToken, autoCache);

            if (false == String.IsNullOrEmpty(response.Json))
            {
                var json = response.Json;

                var obj = JsonConvert.DeserializeObject<Feeds>(json);

                return new FeedsResult(obj);
            }
            else
            {
                var json = response.Error;

                return new FeedsResult(new Exception(json));
            }
        }

        internal async Task<ActivitiesResult> GetActivtiesAsync(String access, String request, Boolean autoCache = false)
        {
            var client = new HttpUtility();

            var response = await client.GetAsync(Utility.Instance.BaseUri, request, access, autoCache);

            if (false == String.IsNullOrEmpty(response.Json))
            {
                var json = response.Json;
                //json = Regex.Unescape(json);

                var obj = JsonConvert.DeserializeObject<Activities>(json);

                return new ActivitiesResult(obj);
            }
            else
            {
                var json = response.Error;
                //json = Regex.Unescape(json);

                return new ActivitiesResult(new Exception(json));
            }
        }

        internal async Task<AnswersResult> GetAnswersAsync(String access, String request, String orderby, Boolean autoCache = false)
        {
            var client = new HttpUtility();

            var response =
                await
                    client.GetAsync(Utility.Instance.BaseUri,
                        String.Format("{0}{1}order_by={2}", request, request.Contains("?") ? "&" : "?", orderby), access, autoCache);

            if (false == String.IsNullOrEmpty(response.Json))
            {
                var json = response.Json;
                //json = Regex.Unescape(json);

                var obj = JsonConvert.DeserializeObject<Answers>(json);

                return new AnswersResult(obj);
            }
            else
            {
                var json = response.Error;
                //json = Regex.Unescape(json);

                return new AnswersResult(new Exception(json));
            }
        }

        internal async Task<QuestionsResult> GetQuestionsAsync(String access, String request, Boolean autoCache = false)
        {
            var client = new HttpUtility();

            var response = await client.GetAsync(Utility.Instance.BaseUri, request, access, autoCache);

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

        internal async Task<CollectionsResult> GetCollectionsAsync(String access, String request, Boolean autoCache = false)
        {
            var client = new HttpUtility();

            var response = await client.GetAsync(Utility.Instance.BaseUri, request, access, autoCache);

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

        internal async Task<UnReadResult> CheckUnReadAsync(String access, Boolean autoCache = false)
        {
            var client = new HttpUtility();

            var response = await client.GetAsync(Utility.Instance.BaseUri, "unread_count", access, autoCache);

            if (false == String.IsNullOrEmpty(response.Json))
            {
                var json = response.Json;
                //json = Regex.Unescape(json);

                var obj = JsonConvert.DeserializeObject<UnRead>(json);

                return new UnReadResult(obj);
            }
            else
            {
                var json = response.Error;
                //json = Regex.Unescape(json);

                return new UnReadResult(new Exception(json));
            }
        }

        internal async Task<NotificationsResult> CheckNotificationsAsync(String accessToken, Boolean autoCache = false)
        {
            var client = new HttpUtility();

            var response = await client.GetAsync(Utility.Instance.BaseUri, "notifications/count", accessToken, autoCache);

            if (false == String.IsNullOrEmpty(response.Json))
            {
                var json = response.Json;
                json = Regex.Unescape(json);

                var obj = JsonConvert.DeserializeObject<Notifications>(json);

                return new NotificationsResult(obj);
            }
            else
            {
                var json = response.Error;
                //json = Regex.Unescape(json);

                return new NotificationsResult(new Exception(json));
            }
        }

        internal async Task<ExplorePeopleResult> ExplorePeopleAsync(String accessToken, Boolean autoCache = false)
        {
            var http = new HttpUtility();

            var response = await http.GetAsync(Utility.Instance.BaseUri,
                String.Format("explore/people?offset=0&limit=20"), accessToken, autoCache);

            if (false == String.IsNullOrEmpty(response.Json))
            {
                var json = response.Json;
                //json = Regex.Unescape(json);

                var userInfo = JsonConvert.DeserializeObject<ExplorePeople>(json);

                return new ExplorePeopleResult(userInfo);
            }
            else
            {
                var json = response.Error;
                //json = Regex.Unescape(json);

                return new ExplorePeopleResult(new Exception(json));
            }
        }

        internal async Task<PeopleFollowingResult> CheckFollowingAsync(String accessToken, String userId, Boolean autoCache = false)
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

                var userInfo = JsonConvert.DeserializeObject<PeopleFollowing>(json);

                return new PeopleFollowingResult(userInfo);
            }
            else
            {
                var json = response.Error;
                //json = Regex.Unescape(json);

                return new PeopleFollowingResult(new Exception(json));
            }
        }

        internal async Task<FollowingResult> FollowAsync(String accessToken, String person)
        {
            var client = new HttpUtility();

            var body = new StringContent(String.Empty);
            body.Headers.ContentType = new MediaTypeHeaderValue("application/x-www-form-urlencoded");

            var response =
                await client.PostAsync(Utility.Instance.BaseUri, String.Format("people/{0}/followers",
                    person), body, accessToken);

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

        internal async Task<FollowingResult> UnfollowAsync(String accessToken, String person, String loginUser)
        {
            var client = new HttpUtility();

            var response =
                await
                    client.DeleteAsync(Utility.Instance.BaseUri,
                        String.Format("people/{0}/followers/{1}", person, loginUser), accessToken);

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

        internal async Task<ColumnsResult> GetColumnsAsync(String access, String request, Boolean autoCache = false)
        {
            var client = new HttpUtility();

            var response =
                await
                    client.GetAsync(Utility.Instance.BaseUri, request, access, autoCache);

            if (false == String.IsNullOrEmpty(response.Json))
            {
                var json = response.Json;
                //json = Regex.Unescape(json);

                var obj = JsonConvert.DeserializeObject<Columns>(json);

                return new ColumnsResult(obj);
            }
            else
            {
                var json = response.Error;
                //json = Regex.Unescape(json);

                return new ColumnsResult(new Exception(json));
            }
        }

        internal async Task<ListResultBase> GetFollowingTopicsAsync(String access, String request, Boolean autoCache = false)
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

        internal async Task<ListResultBase> GetFollowersAsync(String access, String request, Boolean autoCache = false)
        {
            var client = new HttpUtility();

            var response =
                await
                    client.GetAsync(Utility.Instance.BaseUri, request, access, autoCache);

            if (false == String.IsNullOrEmpty(response.Json))
            {
                var json = response.Json;
                //json = Regex.Unescape(json);

                var obj = JsonConvert.DeserializeObject<Followers>(json);

                return new FollowersResult(obj);
            }
            else
            {
                var json = response.Error;
                //json = Regex.Unescape(json);

                return new FollowersResult(new Exception(json));
            }
        }

        internal async Task<ListResultBase> GetFolloweesAsync(String access, String request, Boolean autoCache = false)
        {
            var client = new HttpUtility();

            var response =
                await
                    client.GetAsync(Utility.Instance.BaseUri, request, access, autoCache);

            if (false == String.IsNullOrEmpty(response.Json))
            {
                var json = response.Json;
                //json = Regex.Unescape(json);

                var obj = JsonConvert.DeserializeObject<Followees>(json);

                return new FolloweesResult(obj);
            }
            else
            {
                var json = response.Error;
                //json = Regex.Unescape(json);

                return new FolloweesResult(new Exception(json));
            }
        }
    }
}
