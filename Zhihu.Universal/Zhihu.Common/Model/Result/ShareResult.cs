using System;

using Zhihu.Common.Model.Share;


namespace Zhihu.Common.Model.Result
{
    public sealed class ShareResult
    {
        public ShareTemplates Result { get; private set; }
        public Exception Error { get; private set; }

        private ShareResult()
        {
        }

        public ShareResult(ShareTemplates result)
        {
            Result = result;
        }

        public ShareResult(Exception error)
        {
            Error = error;
        }
    }
}
