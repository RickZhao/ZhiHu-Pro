using System;
using System.Collections.Generic;
using System.Text;

namespace Zhihu.Common.Model.Result
{
    public sealed class TopicResult
    {
        public Topic Result { get; private set; }
        public Exception Error { get; private set; }

        public TopicResult(Topic answer)
        {
            Result = answer;
        }

        public TopicResult(Exception exception)
        {
            Error = exception;
        }
    }
    public sealed class TopicsResult : ListResultBase
    {
        public TopicsResult(Topics result)
            : base(result)
        {
        }

        public TopicsResult(Exception error)
            : base(error)
        {
        }
    }

    public sealed class TopicBestAnswersResult : ListResultBase
    {
        public TopicBestAnswersResult(TopicBestAnswers answer) : base(answer)
        {
        }

        public TopicBestAnswersResult(Exception exception) : base(exception)
        {
        }
    }
}
