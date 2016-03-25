using System;
using Newtonsoft.Json;

// ReSharper disable once CheckNamespace
namespace Zhihu.Common.Model
{
    /// <summary>
    /// 感谢
    /// </summary>
    public sealed class AnswerHelped
    {
        [JsonProperty("is_thanked")]
        public Boolean IsThanked { get; set; }
    }

    public sealed class AnswerNoHelp
    {
        [JsonProperty("is_nothelp")]
        public Boolean IsNotHelp { get; set; }
    }
}
