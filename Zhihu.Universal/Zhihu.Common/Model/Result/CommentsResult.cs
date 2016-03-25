using System;


namespace Zhihu.Common.Model.Result
{
    public sealed class CommentResult
    {
        public Comment Result { get; private set; }
        public Exception Error { get; private set; }

        public CommentResult(Comment comment)
        {
            Result = comment;
        }

        public CommentResult(Exception exception)
        {
            Error = exception;
        }
    }

    public sealed class CommentsResult : ListResultBase
    {
        public CommentsResult(Exception exception)
            : base(exception)
        {
        }

        public CommentsResult(Comments comments)
            : base(comments)
        {
        }
    }
}
