using System;

namespace Zhihu.Common.Model.Result
{
    public sealed class ArticleResult
    {
        public Article Result { get; private set; }
        public Exception Error { get; private set; }

        private ArticleResult()
        {
        }

        public ArticleResult(Article result)
        {
            Result = result;
        }

        public ArticleResult(Exception error)
        {
            Error = error;
        }
    }
}
