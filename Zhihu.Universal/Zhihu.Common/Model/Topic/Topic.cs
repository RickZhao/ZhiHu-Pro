using System;

using Newtonsoft.Json;


// ReSharper disable once CheckNamespace
namespace Zhihu.Common.Model
{
    public sealed class Topic
    {
        [JsonProperty("id")]
        public Int32 Id { get; set; }

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

        [JsonProperty("questions_count")]
        public int QuestionsCount { get; set; }

        [JsonProperty("unanswered_count")]
        public int UnansweredCount { get; set; }

        [JsonProperty("best_answerers_count")]
        public int BestAnswerersCount { get; set; }

        [JsonProperty("best_answers_count")]
        public int BestAnswersCount { get; set; }

        [JsonProperty("related_count")]
        public int RelatedCount { get; set; }

        [JsonProperty("father_count")]
        public int FatherCount { get; set; }

        [JsonProperty("followers_count")]
        public Int32 FollowersCount { get; set; }

        [JsonProperty("type")]
        public String Type { get; set; }
    }
}
