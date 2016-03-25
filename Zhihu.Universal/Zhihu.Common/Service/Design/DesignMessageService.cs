using System;
using System.Threading.Tasks;

using Zhihu.Common.Model.Result;


namespace Zhihu.Common.Service.Design
{
// ReSharper disable once ClassNeverInstantiated.Global
    public sealed class DesignMessageService : IMessage
    {
        public Task<ChatsResult> GetChats(string access, string request, Boolean autoCache = false)
        {
            throw new NotImplementedException();
        }

        public Task<MessagesResult> GetMessages(string access, string request, Boolean autoCache = false)
        {
            throw new NotImplementedException();
        }

        public Task<MessageResult> SendMessage(String access, String receiver, String content)
        {
            throw new NotImplementedException();
        }
    }
}
