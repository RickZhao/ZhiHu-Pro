using System;


namespace Zhihu.Common.Model.Result
{
    public sealed class TableActivitiesResult : ListResultBase
    {
        public TableActivitiesResult(TableActivities activities) : base(activities)
        {

        }

        public TableActivitiesResult(Exception exception) : base(exception)
        {

        }
    }
}
