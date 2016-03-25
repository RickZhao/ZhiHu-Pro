
using System;


// ReSharper disable once CheckNamespace
namespace Zhihu.Common.Model
{
    public sealed class UseTopStoryResult
    {
        public UseTopStory Result { get; private set; }

        public Exception Error { get; private set; }

        public UseTopStoryResult(UseTopStory answer)
        {
            Result = answer;
        }

        public UseTopStoryResult(Exception exception)
        {
            Error = exception;
        }
    }
}
