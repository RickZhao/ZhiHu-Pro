
using System;

namespace Zhihu.Common.HtmlAgilityPack
{
    internal partial class Trace
    {
        internal static Trace _current;
        internal static Trace Current
        {
            get
            {
                if (_current == null)
                    _current = new Trace();
                return _current;
            }
        }
        partial void WriteLineIntern(String message, String category);
        public static void WriteLine(String message, String category)
        {
            Current.WriteLineIntern(message, category);
        }
    }
}
