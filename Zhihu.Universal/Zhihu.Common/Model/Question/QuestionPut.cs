using System;

using Newtonsoft.Json;


// ReSharper disable once CheckNamespace
namespace Zhihu.Common.Model
{
    public sealed class QuestionPut
    {
        [JsonProperty("status")]
        public Status Status { get; set; }

        [JsonProperty("title")]
        public String Title { get; set; }

        [JsonProperty("url")]
        public String Url { get; set; }

        [JsonProperty("topics")]
        public Topic[] Topics { get; set; }

        [JsonProperty("author")]
        public Author Author { get; set; }

        [JsonProperty("excerpt")]
        public String Excerpt { get; set; }

        [JsonProperty("detail")]
        public String Detail { get; set; }

        [JsonProperty("answer_count")]
        public Int32 AnswerCount { get; set; }

        [JsonProperty("updated_time")]
        public Int32 UpdatedTime { get; set; }

        [JsonProperty("to")]
        public Object[] To { get; set; }

        [JsonProperty("comment_count")]
        public Int32 CommentCount { get; set; }

        [JsonProperty("draft")]
        public Draft Draft { get; set; }

        [JsonProperty("redirection")]
        public Redirection Redirection { get; set; }

        [JsonProperty("follower_count")]
        public Int32 FollowerCount { get; set; }

        [JsonProperty("type")]
        public String Type { get; set; }

        [JsonProperty("id")]
        public Int32 Id { get; set; }
    }
}
