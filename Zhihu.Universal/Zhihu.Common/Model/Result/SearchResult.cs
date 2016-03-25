using System;
using System.Collections.Generic;
using System.Text;

namespace Zhihu.Common.Model.Result
{
    public sealed class SearchResult : ListResultBase
    {
        public SearchResult(Exception exception)
            : base(exception)
        {
        }

        public SearchResult(SearchItems activities)
            : base(activities)
        {
        }
    }
}
