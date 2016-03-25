using System;

using Newtonsoft.Json;


// ReSharper disable once CheckNamespace
namespace Zhihu.Common.Model
{
    public sealed class Chat
    {
        [JsonProperty("allow_reply")]
        public Boolean AllowReply { get; set; }

        [JsonProperty("participant")]
        public Participant Participant { get; set; }

        [JsonProperty("is_replied")]
        public Boolean IsReplied { get; set; }

        [JsonProperty("updated_time")]
        public Int32 UpdatedTime { get; set; }

        [JsonProperty("message_count")]
        public Int32 MessageCount { get; set; }

        [JsonProperty("unread_count")]
        public Int32 UnreadCount { get; set; }

        [JsonProperty("snippet")]
        public String Snippet { get; set; }

        [JsonProperty("url")]
        public String Url { get; set; }

        [JsonProperty("type")]
        public String Type { get; set; }

        [JsonProperty("id")]
        public String Id { get; set; }
    }
}
