using System;
using System.Collections.Generic;
using System.Text;

namespace Zhihu.Common.Model.Result
{
    public sealed class UnReadResult
    {
        public UnRead Result { get; private set; }
        public Exception Error { get; private set; }

        public UnReadResult(UnRead result)
        {
            Result = result;
        }

        public UnReadResult(Exception exception)
        {
            Error = exception;
        }
    }
}
