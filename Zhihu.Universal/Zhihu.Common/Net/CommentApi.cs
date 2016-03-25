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
    public sealed class CommentApi
    {
        #region 添加评论

        public async Task<CommentResult> CommentArticleAsync(String access, Int32 articleId, String content)
        {
            var http = new HttpUtility();

            var body = new StringContent(String.Format("content={0}&resource_id={1}&type=article", WebUtility.UrlEncode(content), articleId));
            body.Headers.ContentType = new MediaTypeHeaderValue("application/x-www-form-urlencoded");

            var response =
                await
                    http.PostAsync(Utility.Instance.BaseUri, "comments", body, access);

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

        public async Task<CommentResult> CommentAnswerAsync(String access, Int32 answerId, String content)
        {
            var http = new HttpUtility();

            var body = new StringContent(String.Format("content={0}&resource_id={1}&type=answer", WebUtility.UrlEncode(content), answerId));
            body.Headers.ContentType = new MediaTypeHeaderValue("application/x-www-form-urlencoded");

            var response =
                await
                    http.PostAsync(Utility.Instance.BaseUri, "comments", body, access);

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

        public async Task<CommentResult> CommentQuestionAsync(String access, Int32 questionId, String content)
        {
            var http = new HttpUtility();

            var body = new StringContent(String.Format("content={0}&resource_id={1}&type=question", WebUtility.UrlEncode(content), questionId));
            body.Headers.ContentType = new MediaTypeHeaderValue("application/x-www-form-urlencoded");

            var response =
                await
                    http.PostAsync(Utility.Instance.BaseUri, "comments", body, access);

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

        #endregion

        #region 回复评论
        
        public async Task<CommentResult> ReplyArticleComment(String access, Int32 answerId, Int32 commentId,
            String content)
        {
            var http = new HttpUtility();

            var body =
                new StringContent(
                    String.Format("comment_id={0}&content={1}&resource_id={2}&type=article",
                    commentId,
                    WebUtility.UrlEncode(content),
                    answerId));

            body.Headers.ContentType = new MediaTypeHeaderValue("application/x-www-form-urlencoded");

            var response =
                await
                    http.PostAsync(Utility.Instance.BaseUri, "comments", body, access);

            if (false == String.IsNullOrEmpty(response.Json))
            {
                var json = response.Json;

                var obj = JsonConvert.DeserializeObject<Comment>(json);

                return new CommentResult(obj);
            }
            else
            {
                var json = response.Error;

                return new CommentResult(new Exception(json));
            }
        }

        public async Task<CommentResult> ReplyAnswerComment(String access, Int32 answerId, Int32 commentId,
            String content)
        {
            var http = new HttpUtility();

            var body =
                new StringContent(String.Format("comment_id={0}&content={1}&resource_id={2}&type=answer", commentId,
                   WebUtility.UrlEncode(content), answerId));

            body.Headers.ContentType = new MediaTypeHeaderValue("application/x-www-form-urlencoded");

            var response =
                await
                    http.PostAsync(Utility.Instance.BaseUri, "comments", body, access);

            if (false == String.IsNullOrEmpty(response.Json))
            {
                var json = response.Json;

                var obj = JsonConvert.DeserializeObject<Comment>(json);

                return new CommentResult(obj);
            }
            else
            {
                var json = response.Error;

                return new CommentResult(new Exception(json));
            }
        }

        public async Task<CommentResult> ReplyQuestionComment(String access, Int32 questionId, Int32 commentId,
            String content)
        {
            var http = new HttpUtility();

            var body =
                new StringContent(String.Format("comment_id={0}&content={1}&resource_id={2}&type=question", commentId,
                    WebUtility.UrlEncode(content), questionId));

            body.Headers.ContentType = new MediaTypeHeaderValue("application/x-www-form-urlencoded");

            var response =
                await
                    http.PostAsync(Utility.Instance.BaseUri, "comments", body, access);

            if (false == String.IsNullOrEmpty(response.Json))
            {
                var json = response.Json;

                var obj = JsonConvert.DeserializeObject<Comment>(json);

                return new CommentResult(obj);
            }
            else
            {
                var json = response.Error;

                return new CommentResult(new Exception(json));
            }
        }
      
        #endregion

        #region 投票

        public async Task<VotedResult> VoteUp(String access, Int32 commentId)
        {
            var http = new HttpUtility();

            var body = new StringContent(String.Empty);
            body.Headers.ContentType = new MediaTypeHeaderValue("application/x-www-form-urlencoded");

            var response =
                await
                    http.PostAsync(Utility.Instance.BaseUri, String.Format("comments/{0}/voters", commentId), body,
                        access);

            if (false == String.IsNullOrEmpty(response.Json))
            {
                var json = response.Json;
                //json = Regex.Unescape(json);

                var obj = JsonConvert.DeserializeObject<Voted>(json);

                return new VotedResult(obj);
            }
            else
            {
                var json = response.Error;
                //json = Regex.Unescape(json);

                return new VotedResult(new Exception(json));
            }
        }

        public async Task<VotedResult> CancelVoteUp(String access, Int32 commentId, String loginId)
        {
            var http = new HttpUtility();

            var response =
                await
                    http.DeleteAsync(Utility.Instance.BaseUri,
                        String.Format("comments/{0}/voters/{1}", commentId, loginId), access);

            if (false == String.IsNullOrEmpty(response.Json))
            {
                var json = response.Json;
                //json = Regex.Unescape(json);

                var obj = JsonConvert.DeserializeObject<Voted>(json);

                return new VotedResult(obj);
            }
            else
            {
                var json = response.Error;
                //json = Regex.Unescape(json);

                return new VotedResult(new Exception(json));
            }
        }

        #endregion

        public async Task<OperationResult> ReportComment(String access, Int32 commentId, ReasonMode reason)
        {
            var http = new HttpUtility();

            var body = new StringContent(String.Format("reason={0}&reason_type={1}&resource_id={2}&type=comment",reason.Reason, reason.Type, commentId));
            body.Headers.ContentType = new MediaTypeHeaderValue("application/x-www-form-urlencoded");

            var response =
                await
                    http.PostAsync(Utility.Instance.BaseUri, String.Format("comments/{0}/voters", commentId), body,
                        access);

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
