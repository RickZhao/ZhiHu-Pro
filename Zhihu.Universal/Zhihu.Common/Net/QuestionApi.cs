using System;
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
    public sealed class QuestionApi
    {
        internal async Task<InviteResult> Invite(String access, Int32 questionId, String personId)
        {
            var client = new HttpUtility();

            var body = new StringContent(String.Format("people_ids={0}", personId));
            body.Headers.ContentType = new MediaTypeHeaderValue("application/x-www-form-urlencoded");

            var response =
                await client.PostAsync(Utility.Instance.BaseUri, String.Format("questions/{0}/invitees",
                    questionId), body, access);
            
            if (false == String.IsNullOrEmpty(response.Json))
            {
                var json = response.Json;

                var obj = JsonConvert.DeserializeObject<Invite>(json);

                return new InviteResult(obj);
            }
            else
            {
                var json = response.Error;

                return new InviteResult(new Exception(json));
            }
        }

        internal async Task<QuestionResult> GetDetailAsync(String accessToken, Int32 questionId, Boolean autoCache = false)
        {
            var client = new HttpUtility();

            var response =
                await
                    client.GetAsync(Utility.Instance.BaseUri, String.Format("questions/{0}", questionId), accessToken, autoCache);

            if (false == String.IsNullOrEmpty(response.Json))
            {
                var json = response.Json;
                //json = Regex.Unescape(json);

                var obj = JsonConvert.DeserializeObject<Question>(json);

                return new QuestionResult(obj);
            }
            else
            {
                var json = response.Error;
                //json = Regex.Unescape(json);

                return new QuestionResult(new Exception(json));
            }
        }

        internal async Task<QuesRelaResult> GetRelationshipAsync(String accessToken, Int32 questionId, Boolean autoCache = false)
        {
            var client = new HttpUtility();
            var response =
                await
                    client.GetAsync(Utility.Instance.BaseUri,
                        String.Format("questions/{0}/relationship", questionId), accessToken, autoCache);

            if (false == String.IsNullOrEmpty(response.Json))
            {
                var json = response.Json;
                //json = Regex.Unescape(json);

                var obj = JsonConvert.DeserializeObject<QuesRelationShip>(json);

                return new QuesRelaResult(obj);
            }
            else
            {
                var json = response.Error;
                //json = Regex.Unescape(json);

                return new QuesRelaResult(new Exception(json));
            }
        }

        internal async Task<AnswersResult> GetAnswersAsync(String accessToken, String requestUri, Boolean autoCache = false)
        {
            var client = new HttpUtility();
            var response = await client.GetAsync(Utility.Instance.BaseUri, requestUri, accessToken, autoCache);

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

        internal async Task<TopicsResult> GetTopicsAsync(String accessToken, Int32 questionId, Boolean autoCache = false)
        {
            var client = new HttpUtility();
            var response =
                await
                    client.GetAsync(Utility.Instance.BaseUri, String.Format("questions/{0}/topics", questionId),
                        accessToken, autoCache);

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

        internal async Task<CommentsResult> GetCommentsAsync(String accessToken, String requestUri, Boolean autoCache = false)
        {
            var client = new HttpUtility();

            var response = await client.GetAsync(Utility.Instance.BaseUri, requestUri, accessToken, autoCache);

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

        internal async Task<AnonymousResult> AnonymousAsync(String access, Int32 questionId)
        {
            var client = new HttpUtility();

            var body = new StringContent("is_anonymous=true");
            body.Headers.ContentType = new MediaTypeHeaderValue("application/x-www-form-urlencoded");

            var response =
                await client.PutAsync(Utility.Instance.BaseUri, String.Format("questions/{0}/anonymous",
                    questionId), access, body);

            if (false == String.IsNullOrEmpty(response.Json))
            {
                var json = response.Json;
                //json = Regex.Unescape(json);

                var obj = JsonConvert.DeserializeObject<Anonymous>(json);

                return new AnonymousResult(obj);
            }
            else
            {
                var json = response.Error;
                //json = Regex.Unescape(json);

                return new AnonymousResult(new Exception(json));
            }
        }
    
        internal async Task<AnonymousResult> CancelAnonymousAsync(String access, Int32 questionId)
        {
            var client = new HttpUtility();

            var body = new StringContent("is_anonymous=false");
            body.Headers.ContentType = new MediaTypeHeaderValue("application/x-www-form-urlencoded");

            var response =
                await client.PutAsync(Utility.Instance.BaseUri, String.Format("questions/{0}/anonymous",
                    questionId), access, body);

            if (false == String.IsNullOrEmpty(response.Json))
            {
                var json = response.Json;
                //json = Regex.Unescape(json);

                var obj = JsonConvert.DeserializeObject<Anonymous>(json);

                return new AnonymousResult(obj);
            }
            else
            {
                var json = response.Error;
                //json = Regex.Unescape(json);

                return new AnonymousResult(new Exception(json));
            }
        }

        internal async Task<FollowingResult> FollowAsync(String accessToken, Int32 questionId)
        {
            var client = new HttpUtility();

            var body = new StringContent(String.Empty);
            body.Headers.ContentType = new MediaTypeHeaderValue("application/x-www-form-urlencoded");

            var response =
                await client.PostAsync(Utility.Instance.BaseUri, String.Format("questions/{0}/followers",
                    questionId), body, accessToken);

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
  
        internal async Task<FollowingResult> UnFollowAsync(String accessToken, Int32 questionId, String userId)
        {
            var client = new HttpUtility();
            var response =
                await
                    client.DeleteAsync(Utility.Instance.BaseUri,
                        String.Format("questions/{0}/followers/{1}", questionId, userId), accessToken);

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

        internal async Task<AnswerResult> AnswerAsync(String accessToken, Int32 questionId, String answer)
        {
            var client = new HttpUtility();

            var encodedContent = String.Format("question_id={0}&content={1}", questionId, WebUtility.UrlEncode(answer));

            var body = new StringContent(encodedContent);

            body.Headers.ContentType = new MediaTypeHeaderValue("application/x-www-form-urlencoded");

            var response = await client.PostAsync(Utility.Instance.BaseUri, "answers", body, accessToken);

            if (false == String.IsNullOrEmpty(response.Json))
            {
                var json = response.Json;
                //json = Regex.Unescape(json);

                var obj = JsonConvert.DeserializeObject<Answer>(json);

                return new AnswerResult(obj);
            }
            else
            {
                var json = response.Error;
                //json = Regex.Unescape(json);

                return new AnswerResult(new Exception(json));
            }
        }
     
        internal async Task<CommentResult> CommentAsync(String accessToken, Int32 questionId, String comment)
        {
            var client = new HttpUtility();
            var body =
                new StringContent(String.Format("type=question&content={0}&resource_id={1}", WebUtility.UrlEncode(comment), questionId));

            body.Headers.ContentType = new MediaTypeHeaderValue("application/x-www-form-urlencoded");

            var response = await client.PostAsync(Utility.Instance.BaseUri, "comments", body, accessToken);

            if (false == String.IsNullOrEmpty(response.Json))
            {
                var json = response.Json;
                //json = Regex.Unescape(json);

                var obj = JsonConvert.DeserializeObject<Comment>(json);

                return new CommentResult(obj);
            }
            else
            {
                var json = response.Error;
                //json = Regex.Unescape(json);

                return new CommentResult(new Exception(json));
            }
        }

   
        internal async Task<QuestionsResult> SearchAsync(String accessToken, String keyword, Int32 offset,
            Int32 limit, Boolean autoCache = false)
        {
            var client = new HttpUtility();

            var response =
                await
                    client.GetAsync(Utility.Instance.BaseUri,
                        String.Format("search?q={0}&t=question&offset={1}&limit={2}", keyword, offset, limit),
                        accessToken, autoCache);

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

   
        internal async Task<CreateQuesResult> CreateAsync(String accessToken, Boolean isAnonymous, String topicIds,
            String title, String detail)
        {
            var client = new HttpUtility();

            var body =
                new StringContent(String.Format("is_anonymous={0}&title={1}&detail={2}&topic_ids={3}",
                    isAnonymous == true ? "True" : "False", WebUtility.UrlEncode(title), WebUtility.UrlEncode(detail), topicIds));

            body.Headers.ContentType = new MediaTypeHeaderValue("application/x-www-form-urlencoded");

            var response = await client.PostAsync(Utility.Instance.BaseUri, "questions", body, accessToken);

            if (false == String.IsNullOrEmpty(response.Json))
            {
                var json = response.Json;
                //json = Regex.Unescape(json);

                var obj = JsonConvert.DeserializeObject<QuestionPut>(json);

                return new CreateQuesResult(obj);
            }
            else
            {
                var json = response.Error;
                //json = Regex.Unescape(json);

                return new CreateQuesResult(new Exception(json));
            }
        }
    }
}
