using System;


namespace Zhihu.Common.Model.Result
{
    public sealed class FavoriteResult
    {
        public Favorite Result { get; private set; }
        public Exception Error { get; private set; }

        public FavoriteResult(Exception exception)
        {
            Error = exception;
        }

        public FavoriteResult(Favorite userInfo)
        {
            Result = userInfo;
        }
    }
}
