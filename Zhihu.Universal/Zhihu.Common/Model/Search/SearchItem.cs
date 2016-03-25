
using System;
using Newtonsoft.Json;

// ReSharper disable once CheckNamespace
namespace Zhihu.Common.Model
{
    public sealed class SearchItem
    {
        [JsonProperty("name")]
        public String Name { get; set; }

        [JsonProperty("url")]
        public String Url { get; set; }

        [JsonProperty("excerpt")]
        public String Excerpt { get; set; }

        [JsonProperty("introduction")]
        public String Introduction { get; set; }

        [JsonProperty("avatar_url")]
        public String AvatarUrl { get; set; }

        [JsonProperty("type")]
        public String Type { get; set; }

        [JsonProperty("id")]
        public String Id { get; set; }

        [JsonProperty("title")]
        public String Title { get; set; }

        [JsonProperty("answer_count")]
        public int? AnswerCount { get; set; }

        [JsonProperty("follower_count")]
        public int? FollowerCount { get; set; }
    }
}
