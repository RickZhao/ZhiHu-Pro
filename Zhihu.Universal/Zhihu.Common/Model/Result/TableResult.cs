using System;


namespace Zhihu.Common.Model.Result
{
    public class TableResult
    {
        public RoundTable Result { get; private set; }
        public Exception Error { get; private set; }

        private TableResult()
        {

        }

        public TableResult(RoundTable result)
        {
            Result = result;
        }

        public TableResult(Exception error)
        {
            Error = error;
        }
    }
}
