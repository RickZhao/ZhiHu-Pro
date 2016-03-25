using System;


namespace Zhihu.Common.Model.Result
{
    public sealed class ColumnsResult : ListResultBase
    {
        public ColumnsResult(Exception exception)
            : base(exception)
        {
        }

        public ColumnsResult(Columns comments)
            : base(comments)
        {
        }
    }
}
