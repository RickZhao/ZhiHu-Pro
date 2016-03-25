using System;


namespace Zhihu.Common.Model.Result
{
    public sealed class AuthorsResult : ListResultBase
    {
        public AuthorsResult(Exception exception)
            : base(exception)
        {
        }

        public AuthorsResult(Authors authors)
            : base(authors)
        {
        }
    }
}
