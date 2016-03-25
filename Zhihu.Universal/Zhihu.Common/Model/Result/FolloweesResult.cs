using System;


namespace Zhihu.Common.Model.Result
{
    public sealed class FolloweesResult : ListResultBase
    {
        public FolloweesResult(Exception exception)
            : base(exception)
        {
        }

        public FolloweesResult(Followees feeds)
            : base(feeds)
        {
        }
    }
}
