using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

// ReSharper disable once CheckNamespace
namespace Zhihu.Common.Model
{
    public sealed class AnswerCanComment
    {
        [JsonProperty("status")]
        public Boolean Status { get; set; }

        [JsonProperty("reason")]
        public String Reason { get; set; }
    }
}
