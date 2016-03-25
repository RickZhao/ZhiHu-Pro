using System;
using System.Collections.Generic;
using System.Text;

namespace Zhihu.Common.Model.Result
{
    public sealed class QuestionResult
    {
        public Question Result { get; private set; }
        public Exception Error { get; private set; }

        public QuestionResult(Exception exception)
        {
            Error = exception;
        }

        public QuestionResult(Question question)
        {
            Result = question;
        }
    }

    public sealed class QuestionsResult : ListResultBase
    {
        public QuestionsResult(Questions result)
            : base(result)
        {
        }

        public QuestionsResult(Exception exception)
            : base(exception)
        {
        }
    }

    public sealed class QuesRelaResult
    {
        public QuesRelationShip Result { get; private set; }
        public Exception Error { get; private set; }

        public QuesRelaResult(Exception exception)
        {
            Error = exception;
        }

        public QuesRelaResult(QuesRelationShip relationship)
        {
            Result = relationship;
        }
    }

    public sealed class AnswersResult : ListResultBase
    {
        public AnswersResult(Exception exception)
            : base(exception)
        {
        }

        public AnswersResult(Answers answers)
            : base(answers)
        {
        }
    }

}
