
using Newtonsoft.Json;


namespace Zhihu.Common.Model
{
    public sealed class TableActivities : IPaging
    {
        [JsonProperty("paging")]
        public Paging Paging { get; set; }

        [JsonProperty("data")]
        public TableActivity[] Data { get; set; }

        public object[] GetItems()
        {
            return Data;
        }
    }

    public sealed class TableActivity
    {
        [JsonProperty("is_copyable")]
        public bool IsCopyable { get; set; }

        [JsonProperty("author")]
        public Author Author { get; set; }

        [JsonProperty("url")]
        public string Url { get; set; }

        [JsonProperty("question")]
        public Question Question { get; set; }

        [JsonProperty("excerpt")]
        public string Excerpt { get; set; }

        [JsonProperty("updated_time")]
        public int UpdatedTime { get; set; }

        [JsonProperty("comment_count")]
        public int CommentCount { get; set; }

        [JsonProperty("created_time")]
        public int CreatedTime { get; set; }

        [JsonProperty("voteup_count")]
        public int VoteupCount { get; set; }

        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("thanks_count")]
        public int ThanksCount { get; set; }

        [JsonProperty("follower_count")]
        public int? FollowerCount { get; set; }

        [JsonProperty("title")]
        public string Title { get; set; }

        [JsonProperty("answer_count")]
        public int? AnswerCount { get; set; }
    }
}
