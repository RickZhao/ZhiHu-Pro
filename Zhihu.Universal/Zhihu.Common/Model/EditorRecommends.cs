using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace Zhihu.Common.Model
{
    public sealed class EditorRecommends : IPaging
    {
        [JsonProperty("paging")]
        public Paging Paging { get; set; }

        [JsonProperty("data")]
        public EditorRecommend[] Items { get; set; }

        public object[] GetItems()
        {
            return Items;
        }
    }

    public sealed class EditorRecommend
    {
        [JsonProperty("author")]
        public Author Author { get; set; }

        [JsonProperty("url")]
        public String Url { get; set; }

        [JsonProperty("question")]
        public Question Question { get; set; }

        [JsonProperty("excerpt")]
        public String Excerpt { get; set; }

        [JsonProperty("updated_time")]
        public Int32 UpdatedTime { get; set; }

        [JsonProperty("comment_count")]
        public Int32 CommentCount { get; set; }

        [JsonProperty("created_time")]
        public Int32 CreatedTime { get; set; }

        [JsonProperty("voteup_count")]
        public Int32 VoteupCount { get; set; }

        [JsonProperty("type")]
        public String Type { get; set; }

        [JsonProperty("id")]
        public Int32 Id { get; set; }

        [JsonProperty("thanks_count")]
        public Int32 ThanksCount { get; set; }

        [JsonProperty("updated")]
        public Int32? Updated { get; set; }

        [JsonProperty("title")]
        public String Title { get; set; }

        [JsonProperty("column")]
        public Column Column { get; set; }

        [JsonProperty("image_url")]
        public String ImageUrl { get; set; }
    }
}
