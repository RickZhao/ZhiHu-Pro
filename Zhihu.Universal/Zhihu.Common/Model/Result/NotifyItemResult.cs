using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zhihu.Common.Model.Result
{
    public sealed class NotifyItemResult
    {
        public NotifyItem Result { get; private set; }
        public Exception Error { get; private set; }

        public NotifyItemResult(NotifyItem answer)
        {
            Result = answer;
        }

        public NotifyItemResult(Exception exception)
        {
            Error = exception;
        }
    }
}
