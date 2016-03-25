using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

// ReSharper disable once CheckNamespace
namespace Zhihu.Common.Model
{
    public sealed class Collection
    {
        [JsonProperty("questions")]
        public Question[] Questions { get; set; }

        [JsonProperty("id")]
        public Int32 Id { get; set; }

        [JsonProperty("title")]
        public String Title { get; set; }

        [JsonProperty("description")]
        public String Description { get; set; }

        [JsonProperty("creator")]
        public CollectionCreator Creator { get; set; }

        [JsonProperty("is_favorited")]
        public Boolean IsFavorited { get; set; }

        [JsonProperty("url")]
        public String Url { get; set; }

        [JsonProperty("type")]
        public String Type { get; set; }

        [JsonProperty("is_public")]
        public Boolean IsPublic { get; set; }

        [JsonProperty("created_time")]
        public Int32 CreatedTime { get; set; }

        [JsonProperty("updated_time")]
        public Int32 UpdatedTime { get; set; }

        [JsonProperty("follower_count")]
        public Int32 FollowerCount { get; set; }

        [JsonProperty("answer_count")]
        public Int32 AnswerCount { get; set; }

        [JsonProperty("comment_count")]
        public Int32 CommentCount { get; set; }
    }
}
