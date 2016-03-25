using System;


namespace Zhihu.Common.Model.Result
{
    public sealed class FollowingResult
    {
        public Following Result { get; private set; }
        public Exception Error { get; private set; }

        public FollowingResult(Exception exception)
        {
            Error = exception;
        }

        public FollowingResult(Following questionFllow)
        {
            Result = questionFllow;
        }
    }

}
