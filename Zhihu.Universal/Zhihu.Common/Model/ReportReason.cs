using System;


namespace Zhihu.Common.Model
{
// ReSharper disable once ClassNeverInstantiated.Global
    public sealed class ReportReason
    {
        public static ReasonMode AdJunk
        {
            get
            {
                return new ReasonMode()
                {
                    Reason = "广告等垃圾信息",
                    Type = 10001,
                };
            }
        }

        public static ReasonMode UnFridendly
        {
            get
            {
                return new ReasonMode()
                {
                    Reason = "不友善内容",
                    Type = 10002,
                };
            }
        }

        public static ReasonMode Illegal
        {
            get
            {
                return new ReasonMode()
                {
                    Reason = "违反法律法规的内容",
                    Type = 10003,
                };
            }
        }

        public static ReasonMode Political
        {
            get
            {
                return new ReasonMode()
                {
                    Reason = "不宜公开讨论的政治内容",
                    Type = 10004,
                };
            }
        }

        public static ReasonMode Custom
        {
            get
            {
                return new ReasonMode()
                {
                    Reason = "",
                    Type = Int32.MaxValue,
                };
            }
        }
    }

    public struct ReasonMode
    {
        public Int32 Type;
        public String Reason;
    }
}