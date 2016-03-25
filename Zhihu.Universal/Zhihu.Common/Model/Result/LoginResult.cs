using System;
using System.Collections.Generic;
using System.Text;

namespace Zhihu.Common.Model.Result
{
    public sealed class LoginResult
    {
        public UserInfo Result { get; private set; }
        public Exception Error { get; private set; }

        public LoginResult(Exception exception)
        {
            Error = exception;
        }

        public LoginResult(UserInfo userInfo)
        {
            Result = userInfo;
        }
    }

}
