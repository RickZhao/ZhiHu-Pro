using System;


namespace Zhihu.Common.Model.Result
{
    public sealed class ActivitiesResult : ListResultBase
    {
        public ActivitiesResult(Exception exception)
            : base(exception)
        {
        }

        public ActivitiesResult(Activities activities)
            : base(activities)
        {
        }
    }
}
