using System;

using Newtonsoft.Json;


// ReSharper disable once CheckNamespace
namespace Zhihu.Common.Model
{
    public sealed class HotCollection
    {
        [JsonProperty("pageing_id")]
        public Int32 PageingId { get; set; }

        [JsonProperty("description")]
        public String Description { get; set; }

        [JsonProperty("creator")]
        public CollectionCreator Creator { get; set; }

        [JsonProperty("url")]
        public String Url { get; set; }

        [JsonProperty("title")]
        public String Title { get; set; }

        [JsonProperty("answer_count")]
        public Int32 AnswerCount { get; set; }

        [JsonProperty("updated_time")]
        public Int32 UpdatedTime { get; set; }

        [JsonProperty("created_time")]
        public Int32 CreatedTime { get; set; }

        [JsonProperty("comment_count")]
        public Int32 CommentCount { get; set; }

        [JsonProperty("follower_count")]
        public Int32 FollowerCount { get; set; }

        [JsonProperty("is_public")]
        public bool IsPublic { get; set; }

        [JsonProperty("type")]
        public String Type { get; set; }

        [JsonProperty("id")]
        public Int32 Id { get; set; }
    }
}
