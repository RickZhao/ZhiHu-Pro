using System;
using Zhihu.Common.Model;
using Zhihu.Common.Model.Result;

namespace Zhihu.Common.Model.Result
{
    public sealed class HotCollectionsResult : ListResultBase
    {
        public HotCollectionsResult(HotCollections collections)
            : base(collections)
        {
        }

        public HotCollectionsResult(Exception exception)
            : base(exception)
        {
        }
    }

    public sealed class CollectionsResult : ListResultBase
    {
        public CollectionsResult(Collections collections)
            : base(collections)
        {
        }

        public CollectionsResult(Exception exception)
            : base(exception)
        {
        }
    }

    public sealed class CollectionAnswersResult : ListResultBase
    {
        public CollectionAnswersResult(CollectionAnswers collections)
            : base(collections)
        {
        }

        public CollectionAnswersResult(Exception exception)
            : base(exception)
        {
        }
    }

    public sealed class UnFollowResult
    {
        public Operation Result { get; private set; }
        public Exception Error { get; private set; }

        public UnFollowResult(Operation answer)
        {
            Result = answer;
        }

        public UnFollowResult(Exception exception)
        {
            Error = exception;
        }
    }

    public sealed class CollectionResult
    {
        public Collection Result { get; private set; }
        public Exception Error { get; private set; }

        public CollectionResult(Collection putColl)
        {
            Result = putColl;
        }

        public CollectionResult(Exception exception)
        {
            Error = exception;
        }
    }

    public sealed class CreateCollectionResult
    {
        public Collection Result { get; private set; }
        public Exception Error { get; private set; }

        public CreateCollectionResult(Collection putColl)
        {
            Result = putColl;
        }

        public CreateCollectionResult(Exception exception)
        {
            Error = exception;
        }
    }
}