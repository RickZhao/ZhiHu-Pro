using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

// ReSharper disable once CheckNamespace
namespace Zhihu.Common.Model
{
    public sealed class Answer
    {
        [JsonProperty("suggest_edit")]
        public SuggestEdit SuggestEdit { get; set; }

        [JsonProperty("author")]
        public Author Author { get; set; }

        [JsonProperty("url")]
        public String Url { get; set; }

        [JsonProperty("question")]
        public Question Question { get; set; }

        [JsonProperty("excerpt")]
        public String Excerpt { get; set; }

        [JsonProperty("created_time")]
        public Int32 CreatedTime { get; set; }

        [JsonProperty("updated_time")]
        public Int32 UpdatedTime { get; set; }

        [JsonProperty("comment_count")]
        public Int32 CommentCount { get; set; }

        [JsonProperty("voteup_count")]
        public Int32 VoteupCount { get; set; }

        [JsonProperty("type")]
        public String Type { get; set; }

        [JsonProperty("id")]
        public Int32 Id { get; set; }

        [JsonProperty("thanks_count")]
        public Int32 ThanksCount { get; set; }
    }
}
