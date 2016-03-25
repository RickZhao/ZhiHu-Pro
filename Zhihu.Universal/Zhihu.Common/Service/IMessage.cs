using System;
using System.Threading.Tasks;

using Zhihu.Common.Model.Result;


namespace Zhihu.Common.Service
{
    public interface IMessage
    {
        Task<ChatsResult> GetChats(String access, String request, Boolean autoCache = false);
        Task<MessagesResult> GetMessages(String access, String request, Boolean autoCache = false);
        Task<MessageResult> SendMessage(String access, String receiver, String content);
    }
}
