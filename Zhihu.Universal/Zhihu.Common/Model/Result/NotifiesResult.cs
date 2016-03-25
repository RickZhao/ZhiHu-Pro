using System;


namespace Zhihu.Common.Model.Result
{
    public sealed class NotifiesResult : ListResultBase
    {
        public NotifiesResult(Exception exception)
            : base(exception)
        {
        }

        public NotifiesResult(Notifies feeds)
            : base(feeds)
        {
        }
    }
}
