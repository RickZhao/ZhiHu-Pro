using System;
using System.Collections.Generic;
using System.Text;

namespace Zhihu.Common.Model.Result
{

    public sealed class ProfileResult
    {
        public Profile Result { get; private set; }
        public Exception Error { get; private set; }

        public ProfileResult(Exception exception)
        {
            Error = exception;
        }

        public ProfileResult(Profile profile)
        {
            Result = profile;
        }
    }

}
