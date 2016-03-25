using System;
using System.Collections.Generic;
using System.Text;

namespace Zhihu.Common.Model.Result
{
    public sealed class FollowResult
    {
        public Operation Result { get; private set; }
        public Exception Error { get; private set; }

        private FollowResult()
        {

        }

        public FollowResult(Operation answer)
        {
            Result = answer;
        }

        public FollowResult(Exception exception)
        {
            Error = exception;
        }
    }
}
