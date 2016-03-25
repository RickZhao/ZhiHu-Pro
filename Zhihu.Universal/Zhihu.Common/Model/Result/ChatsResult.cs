using System;


namespace Zhihu.Common.Model.Result
{
    public sealed class ChatsResult : ListResultBase
    {
        public ChatsResult(Exception exception)
            : base(exception)
        {
        }

        public ChatsResult(Chats chats)
            : base(chats)
        {
        }
    }
}
