using System;
using System.Collections.Generic;
using System.Text;

namespace Zhihu.Common.Model.Result
{
    public class ColumnResult
    {
        public Column Result { get; private set; }
        public Exception Error { get; private set; }

        private ColumnResult()
        {
        }

        public ColumnResult(Column result)
        {
            Result = result;
        }

        public ColumnResult(Exception error)
        {
            Error = error;
        }
    }
}
