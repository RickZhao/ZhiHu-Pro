using System;

using Newtonsoft.Json;


namespace Zhihu.Common.Model
{
    public sealed class Profile
    {
        [JsonProperty("id")]
        public String Id { get; set; }

        [JsonProperty("is_active")]
        public bool IsActive { get; set; }

        [JsonProperty("name")]
        public String Name { get; set; }

        [JsonProperty("url")]
        public String Url { get; set; }

        [JsonProperty("gender")]
        public Int32 Gender { get; set; }

        [JsonProperty("sina_weibo_name")]
        public String SinaWeiboName { get; set; }

        [JsonProperty("avatar_url")]
        public String AvatarUrl { get; set; }

        [JsonProperty("type")]
        public String Type { get; set; }

        [JsonProperty("headline")]
        public String Headline { get; set; }

        [JsonProperty("email")]
        public String Email { get; set; }

        [JsonProperty("description")]
        public String Description { get; set; }

        [JsonProperty("qq_weibo_name")]
        public String QqWeiboName { get; set; }

        [JsonProperty("sina_weibo_url")]
        public String SinaWeiboUrl { get; set; }

        [JsonProperty("qq_weibo_url")]
        public String QqWeiboUrl { get; set; }

        [JsonProperty("favorite_count")]
        public Int32 FavoriteCount { get; set; }

        [JsonProperty("voteup_count")]
        public Int32 VoteupCount { get; set; }

        [JsonProperty("shared_count")]
        public Int32 SharedCount { get; set; }

        [JsonProperty("ask_about_count")]
        public Int32 AskAboutCount { get; set; }

        [JsonProperty("following_topic_count")]
        public Int32 FollowingTopicCount { get; set; }

        [JsonProperty("following_question_count")]
        public Int32 FollowingQuestionCount { get; set; }

        [JsonProperty("following_collection_count")]
        public Int32 FollowingCollectionCount { get; set; }

        [JsonProperty("following_columns_count")]
        public Int32 FollowingColumnsCount { get; set; }

        [JsonProperty("answer_count")]
        public Int32 AnswerCount { get; set; }

        [JsonProperty("question_count")]
        public Int32 QuestionCount { get; set; }

        [JsonProperty("thanked_count")]
        public Int32 ThankedCount { get; set; }

        [JsonProperty("columns_count")]
        public Int32 ColumnsCount { get; set; }

        [JsonProperty("following_count")]
        public Int32 FollowingCount { get; set; }

        [JsonProperty("favorited_count")]
        public Int32 FavoritedCount { get; set; }

        [JsonProperty("follower_count")]
        public Int32 FollowerCount { get; set; }

        [JsonProperty("draft_count")]
        public Int32 DraftCount { get; set; }

        [JsonProperty("business")]
        public Business Business { get; set; }

        [JsonProperty("location")]
        public Location[] Locations { get; set; }

        [JsonProperty("employment")]
        public Employment[][] Employments { get; set; }

        [JsonProperty("education")]
        public Education[] Education { get; set; }
    }
}
