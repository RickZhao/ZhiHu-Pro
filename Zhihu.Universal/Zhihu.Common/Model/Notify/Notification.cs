using System;

using Newtonsoft.Json;


// ReSharper disable once CheckNamespace
namespace Zhihu.Common.Model
{
    public sealed class Notifications
    {
        [JsonProperty("message_count")]
        public Int32 Message { get; set; }

        [JsonProperty("new_content_count")]
        public Int32 NewContent { get; set; }

        [JsonProperty("new_follow_count")]
        public Int32 NewFollow { get; set; }

        [JsonProperty("new_love_count")]
        public Int32 NewLove { get; set; }
    }
}
