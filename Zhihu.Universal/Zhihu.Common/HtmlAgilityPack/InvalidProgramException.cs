
using System;
namespace Zhihu.Common.HtmlAgilityPack
{
    public class InvalidProgramException : System.Exception
    {
        public InvalidProgramException(String message)
            : base(message)
        {
        }
    }
}
