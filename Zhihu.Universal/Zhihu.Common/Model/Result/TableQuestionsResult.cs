using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zhihu.Common.Model.Result
{
    public sealed class TableQuestionsResult : ListResultBase
    {
        public TableQuestionsResult(Exception exception)
            : base(exception)
        {
        }

        public TableQuestionsResult(TableQuestions comments)
            : base(comments)
        {
        }
    }
}
