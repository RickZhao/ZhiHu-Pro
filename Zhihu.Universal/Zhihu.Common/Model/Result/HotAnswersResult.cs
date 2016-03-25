using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zhihu.Common.Model.Result
{
    public sealed class HotAnswersResult : ListResultBase
    {
        public HotAnswersResult(HotAnswers result) : base(result)
        {
        }

        public HotAnswersResult(Exception error) : base(error)
        {
        }
    }
}
