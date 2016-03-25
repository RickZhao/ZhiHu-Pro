using System;


namespace Zhihu.Common.Model.Result
{
    public sealed class TopicActivitiesResult : ListResultBase
    {
        public TopicActivitiesResult(TopicActivities collections)
            : base(collections)
        {
        }

        public TopicActivitiesResult(Exception exception)
            : base(exception)
        {
        }
    }
}
