using System;
using System.Collections.Generic;
using System.Text;

namespace Zhihu.Common.Model.Result
{
    public sealed class AnonymousResult
    {
        public Anonymous Result { get; private set; }
        public Exception Error { get; private set; }

        public AnonymousResult(Anonymous result)
        {
            Result = result;
        }

        public AnonymousResult(Exception error)
        {
            Error = error;
        }
    }
}
