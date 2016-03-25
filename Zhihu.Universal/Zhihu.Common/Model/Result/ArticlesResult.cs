using System;


namespace Zhihu.Common.Model.Result
{
    public class ArticlesResult : ListResultBase
    {
        public ArticlesResult(Exception exception)
            : base(exception)
        {
        }

        public ArticlesResult(Articles articles)
            : base(articles)
        {
        }
    }
}
