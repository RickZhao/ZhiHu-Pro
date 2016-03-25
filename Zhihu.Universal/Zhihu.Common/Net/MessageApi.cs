using System;
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
    public sealed class MessageApi
    {
        internal async Task<ChatsResult> GetChats(String access, String request, Boolean autoCache = false)
        {
            var http = new HttpUtility();

            var response =
                await http.GetAsync(Utility.Instance.BaseUri, request, access, autoCache);

            if (false == String.IsNullOrEmpty(response.Json))
            {
                var json = response.Json;

                var obj = JsonConvert.DeserializeObject<Chats>(json);

                return new ChatsResult(obj);
            }
            else
            {
                var json = response.Error;

                return new ChatsResult(new Exception(json));
            }
        }

        internal async Task<MessagesResult> GetMessages(String access, String request, Boolean autoCache = false)
        {
            var http = new HttpUtility();

            var response =
                await http.GetAsync(Utility.Instance.BaseUri, request, access, autoCache);

            if (false == String.IsNullOrEmpty(response.Json))
            {
                var json = response.Json;

                var obj = JsonConvert.DeserializeObject<Messages>(json);
                if (obj != null && obj.Items != null)
                {
                    obj.Items = obj.Items.Reverse().ToArray();
                }
                return new MessagesResult(obj);
            }
            else
            {
                var json = response.Error;

                return new MessagesResult(new Exception(json));
            }
        }

        internal async Task<MessageResult> SendMessage(String access, String receiver, String content)
        {
            var http = new HttpUtility();

            var body = new StringContent(String.Format("content={0}&receiver_id={1}", WebUtility.UrlEncode(content), receiver));
            body.Headers.ContentType = new MediaTypeHeaderValue("application/x-www-form-urlencoded");

            var response =
                await http.PostAsync(Utility.Instance.BaseUri, "messages", body, access);

            if (false == String.IsNullOrEmpty(response.Json))
            {
                var json = response.Json;

                var obj = JsonConvert.DeserializeObject<Message>(json);

                return new MessageResult(obj);
            }
            else
            {
                var json = response.Error;

                return new MessageResult(new Exception(json));
            }
        }
    }
}