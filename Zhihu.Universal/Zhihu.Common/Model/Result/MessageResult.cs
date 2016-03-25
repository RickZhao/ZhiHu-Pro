using System;


namespace Zhihu.Common.Model.Result
{
    public sealed class MessageResult
    {
        public Message Result { get; private set; }
        public Exception Error { get; private set; }

        private MessageResult()
        {
        }

        public MessageResult(Message result)
        {
            Result = result;
        }

        public MessageResult(Exception error)
        {
            Error = error;
        }
    }
}
