using System;

namespace Zhihu.Common.Model.Result
{
    public sealed class AnswerResult
    {
        public Answer Result { get; private set; }
        public Exception Error { get; private set; }

        public AnswerResult(Answer answer)
        {
            Result = answer;
        }

        public AnswerResult(Exception exception)
        {
            Error = exception;
        }
    }

    public sealed class AnswerDetailResult
    {
        public AnswerDetail Result { get; private set; }
        public Exception Error { get; private set; }

        public AnswerDetailResult(AnswerDetail result)
        {
            Result = result;
        }

        public AnswerDetailResult(Exception error)
        {
            Error = error;
        }
    }

    public sealed class AnswerRelationshipResult
    {
        public AnswerRelationship Result { get; private set; }
        public Exception Error { get; private set; }

        public AnswerRelationshipResult(AnswerRelationship result)
        {
            Result = result;
        }

        public AnswerRelationshipResult(Exception error)
        {
            Error = error;
        }
    }

    public sealed class AnswerCommentsResult : ListResultBase
    {
        public AnswerCommentsResult(AnswerComments result)
            : base(result)
        {
        }

        public AnswerCommentsResult(Exception error)
            : base(error)
        {
        }
    }

    public sealed class AnswerNoHelpeResult
    {
        public AnswerNoHelp Result { get; private set; }
        public Exception Error { get; private set; }

        public AnswerNoHelpeResult(AnswerNoHelp result)
        {
            Result = result;
        }

        public AnswerNoHelpeResult(Exception error)
        {
            Error = error;
        }
    }

    public sealed class AnswerHelpedResult
    {
        public AnswerHelped Result { get; private set; }
        public Exception Error { get; private set; }

        public AnswerHelpedResult(AnswerHelped result)
        {
            Result = result;
        }

        public AnswerHelpedResult(Exception error)
        {
            Error = error;
        }
    }
}
