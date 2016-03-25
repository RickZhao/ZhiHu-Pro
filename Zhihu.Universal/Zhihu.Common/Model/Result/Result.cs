using System;


namespace Zhihu.Common.Model.Result
{
    public class ListResultBase
    {
        public IPaging Result { get; private set; }
        public Exception Error { get; private set; }

        private ListResultBase()
        {
        }

        public ListResultBase(IPaging result) : this()
        {
            Result = result;
        }

        public ListResultBase(Exception error)
            : this()
        {
            Error = error;
        }
    }

    public sealed class FeedsResult : ListResultBase
    {
        public FeedsResult(Exception exception)
            : base(exception)
        {
        }

        public FeedsResult(Feeds feeds)
            : base(feeds)
        {
        }
    }

    public sealed class NotificationsResult
    {
        public Notifications Result { get; private set; }
        public Exception Error { get; private set; }

        public NotificationsResult(Notifications notifications)
        {
            Result = notifications;
        }

        public NotificationsResult(Exception exception)
        {
            Error = exception;
        }
    }

    public sealed class CreateQuesResult
    {
        public QuestionPut Result { get; private set; }
        public Exception Error { get; private set; }

        public CreateQuesResult(QuestionPut putQue)
        {
            Result = putQue;
        }

        public CreateQuesResult(Exception exception)
        {
            Error = exception;
        }
    }

    public sealed class OperationResult
    {
        public Operation Result { get; private set; }
        public Exception Error { get; private set; }

        public OperationResult(Operation result)
        {
            Result = result;
        }

        public OperationResult(Exception error)
        {
            Error = error;
        }
    }

    public sealed class PeopleFollowingResult
    {
        public PeopleFollowing Result { get; private set; }
        public Exception Error { get; private set; }

        private PeopleFollowingResult()
        {
        }

        public PeopleFollowingResult(PeopleFollowing result)
        {
            Result = result;
        }

        public PeopleFollowingResult(Exception error)
        {
            Error = error;
        }
    }
}