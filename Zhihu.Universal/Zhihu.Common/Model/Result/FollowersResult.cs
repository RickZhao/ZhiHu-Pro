using System;


namespace Zhihu.Common.Model.Result
{
    public sealed class FollowersResult : ListResultBase
    {
        public FollowersResult(Exception exception)
            : base(exception)
        {
        }

        public FollowersResult(Followers feeds)
            : base(feeds)
        {
        }
    }
}
