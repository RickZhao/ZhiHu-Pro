using System;
using System.Collections.Generic;
using System.Text;

namespace Zhihu.Common.Model.Result
{
    public sealed class MessagesResult : ListResultBase
    {
        public MessagesResult(Exception exception)
            : base(exception)
        {
        }

        public MessagesResult(Messages messages)
            : base(messages)
        {
        }
    }
}
