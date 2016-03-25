using System;

using Newtonsoft.Json;


// ReSharper disable once CheckNamespace
namespace Zhihu.Common.Model
{
    public sealed class TopicActivity
    {
        [JsonProperty("title")]
        public String Title { get; set; }

        [JsonProperty("url")]
        public String Url { get; set; }

        [JsonProperty("answers")]
        public Answer[] Answers { get; set; }

        [JsonProperty("topic")]
        public Topic Topic { get; set; }

        [JsonProperty("is_following")]
        public Boolean IsFollowing { get; set; }

        [JsonProperty("answer_count")]
        public Int32 AnswerCount { get; set; }

        [JsonProperty("offset")]
        public Double Offset { get; set; }

        [JsonProperty("type")]
        public String Type { get; set; }

        [JsonProperty("id")]
        public Int32 Id { get; set; }
    }
}
