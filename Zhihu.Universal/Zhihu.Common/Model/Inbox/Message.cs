using System;

using Newtonsoft.Json;


// ReSharper disable once CheckNamespace
namespace Zhihu.Common.Model
{
    public sealed class Message
    {
        [JsonProperty("content")]
        public String Content { get; set; }

        [JsonProperty("created_time")]
        public Int32 CreatedTime { get; set; }

        [JsonProperty("sender")]
        public Sender Sender { get; set; }

        [JsonProperty("receiver")]
        public Receiver Receiver { get; set; }

        [JsonProperty("url")]
        public String Url { get; set; }

        [JsonProperty("type")]
        public String Type { get; set; }

        [JsonProperty("id")]
        public String Id { get; set; }
    }
}
