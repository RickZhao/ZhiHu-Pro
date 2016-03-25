using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

using Newtonsoft.Json;

using Zhihu.Common.Helper;
using Zhihu.Common.Model;
using Zhihu.Common.Model.Result;
using Zhihu.Common.Model.Share;


namespace Zhihu.Common.Net
{
    public sealed class ShareApi
    {
        public async Task<ShareResult> GetQuestionShareTemplate(String access, Int32 questionId, Boolean autoCache = false)
        {
            var client = new HttpUtility();

            var response = await client.GetAsync(Utility.Instance.BaseUri, String.Format("questions/{0}/share", questionId), access, autoCache);

            if (false == String.IsNullOrEmpty(response.Json))
            {
                var json = response.Json;
                //json = Regex.Unescape(json);

                var obj = JsonConvert.DeserializeObject<ShareTemplates>(json);

                return new ShareResult(obj);
            }
            else
            {
                var json = response.Error;
                //json = Regex.Unescape(json);

                return new ShareResult(new Exception(json));
            }
        }

        public async Task<ShareResult> GetAnswerShareTemplate(String access, Int32 answerId, Boolean autoCache = false)
        {
            var client = new HttpUtility();

            var response =
                await client.GetAsync(Utility.Instance.BaseUri, String.Format("answers/{0}/share", answerId), access, autoCache);

            if (false == String.IsNullOrEmpty(response.Json))
            {
                var json = response.Json;
                //json = Regex.Unescape(json);

                var obj = JsonConvert.DeserializeObject<ShareTemplates>(json);

                return new ShareResult(obj);
            }
            else
            {
                var json = response.Error;
                //json = Regex.Unescape(json);

                return new ShareResult(new Exception(json));
            }
        }

        public async Task<OperationResult> ShareQuestionViaSina(String access, Int32 questionId, String content)
        {
            var client = new HttpUtility();

            var body = new StringContent(String.Format("content={0}&short_share=true&via=sina", WebUtility.UrlEncode(content)));
            body.Headers.ContentType = new MediaTypeHeaderValue("application/x-www-form-urlencoded");

            var response =
                await client.PostAsync(Utility.Instance.BaseUri, String.Format("questions/{0}/share", questionId),
                    body, access);

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

        public async Task<OperationResult> ShareAnswerViaSina(String access, Int32 answerId, String content)
        {
            var client = new HttpUtility();

            var body = new StringContent(String.Format("content={0}&short_share=true&via=sina", WebUtility.UrlEncode(content)));
            body.Headers.ContentType = new MediaTypeHeaderValue("application/x-www-form-urlencoded");

            var response =
                await client.PostAsync(Utility.Instance.BaseUri, String.Format("answers/{0}/share", answerId),
                    body, access);

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
