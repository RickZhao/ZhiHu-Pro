using System;
using System.Collections.Generic;
using System.Text;

namespace Zhihu.Common.Helper
{
    public sealed class AvartarHelper
    {
        public static String GetMedium(String avatarUrl)
        {
            return avatarUrl.Replace("_s", "_m");
        }

        public static String GetLarge(String avatarUrl)
        {
            return avatarUrl.Replace("_s", "_l");
        }

        public static String GetExtramelyLarge(String avatarUrl)
        {
            return avatarUrl.Replace("_s", "_xl");
        }
    }
}
