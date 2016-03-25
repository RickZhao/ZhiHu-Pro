using System;


namespace Zhihu.Common.Model.Result
{
    public sealed class InviteResult
    {
        public Invite Result { get; private set; }
        public Exception Error { get; private set; }

        public InviteResult(Invite result)
        {
            Result = result;
        }

        public InviteResult(Exception error)
        {
            Error = error;
        }
    }
}
