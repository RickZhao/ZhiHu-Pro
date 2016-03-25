using System;


namespace Zhihu.Common.Model.Result
{
    public sealed class VoteResult
    {
        public Vote Result { get; private set; }

        public Exception Error { get; private set; }

        private VoteResult()
        {

        }

        public VoteResult(Vote result)
        {
            this.Result = result;
        }

        public VoteResult(Exception error)
        {
            this.Error = error;
        }
    }

    public sealed class VotedResult
    {
        public Voted Result { get; private set; }

        public Exception Error { get; private set; }

        private VotedResult()
        {

        }

        public VotedResult(Voted result)
        {
            this.Result = result;
        }

        public VotedResult(Exception error)
        {
            this.Error = error;
        }
    }
}
