using System;

using Newtonsoft.Json;


// ReSharper disable once CheckNamespace
namespace Zhihu.Common.Model
{
    public sealed class Column
    {
        [JsonProperty("id")]
        public String Id { get; set; }

        [JsonProperty("title")]
        public String Title { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("followers")]
        public Int32 FollowersCount { get; set; }

        [JsonProperty("articles_count")]
        public Int32 ArticlesCount { get; set; }

        [JsonProperty("image_url")]
        public String ImageUrl { get; set; }

        [JsonProperty("type")]
        public String Type { get; set; }

        [JsonProperty("updated")]
        public Int32 Updated { get; set; }

        [JsonProperty("author")]
        public Author Author { get; set; }

        [JsonProperty("url")]
        public String Url { get; set; }

        [JsonProperty("comment_permission")]
        public String CommentPermission { get; set; }
    }
}