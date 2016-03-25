
using System;
using System.Threading.Tasks;

using Zhihu.Common.Model.Result;
using Zhihu.Common.Net;


namespace Zhihu.Common.Service.Runtime
{
// ReSharper disable once ClassNeverInstantiated.Global
    public sealed class MessageService : IMessage
    {
        public async Task<ChatsResult> GetChats(string access, string request, Boolean autoCache = false)
        {
            var api = new MessageApi();
            var result = await api.GetChats(access, request, autoCache);

            return result;
        }

        public async Task<MessagesResult> GetMessages(string access, string request,
            Boolean autoCache = false)
        {
            var api = new MessageApi();
            var result = await api.GetMessages(access, request, autoCache);

            return result;
        }

        public async Task<MessageResult> SendMessage(String access, String receiver, String content)
        {
            var api = new MessageApi();
            var result = await api.SendMessage(access, receiver, content);

            return result;
        }
    }
}
