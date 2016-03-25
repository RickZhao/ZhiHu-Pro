using System;

using GalaSoft.MvvmLight;

using Newtonsoft.Json;


namespace Zhihu.Common.Model
{
    public sealed class Feeds : IPaging
    {
        [JsonProperty("paging")]
        public Paging Paging { get; set; }

        [JsonProperty("data")]
        public Feed[] Items { get; set; }

        public object[] GetItems()
        {
            return Items;
        }
    }

    public sealed class Paging
    {
        [JsonProperty("is_end")]
        public Boolean IsEnd { get; set; }

        [JsonProperty("next")]
        public String Next { get; set; }

        [JsonProperty("previous")]
        public String Previous { get; set; }
    }

    public sealed class Feed
    {
        [JsonProperty("target")]
        public Target Target { get; set; }

        [JsonProperty("count")]
        public Int32? Count { get; set; }

        [JsonProperty("updated_time")]
        public Int32? UpdatedTime { get; set; }

        [JsonProperty("verb")]
        public String Verb { get; set; }

        [JsonProperty("actors")]
        public Actor[] Actors { get; set; }

        [JsonProperty("voters")]
        public object[] Voters { get; set; }

        [JsonProperty("topic")]
        public Topic Topic { get; set; }

        [JsonProperty("type")]
        public String Type { get; set; }

        [JsonProperty("id")]
        public String Id { get; set; }

        [JsonProperty("followers")]
        public Author[] Followers { get; set; }
    }

    public sealed class Actor
    {
        [JsonProperty("headline")]
        public String Headline { get; set; }

        [JsonProperty("avatar_url")]
        public String AvatarUrl { get; set; }

        [JsonProperty("name")]
        public String Name { get; set; }

        [JsonProperty("url")]
        public String Url { get; set; }

        [JsonProperty("gender")]
        public Int32 Gender { get; set; }

        [JsonProperty("type")]
        public String Type { get; set; }

        [JsonProperty("id")]
        public String Id { get; set; }
    }

    public sealed class AmazingGuy : ObservableObject
    {
        private Author _guy;

        public Author Guy
        {
            get { return _guy; }
            set
            {
                _guy = value;
                RaisePropertyChanged(() => Guy);
            }
        }

        private Boolean _isFollowing = false;

        public Boolean IsFollowing
        {
            get { return _isFollowing; }
            set
            {
                _isFollowing = value;
                RaisePropertyChanged(() => IsFollowing);
            }
        }
    }

    public sealed class QuestionSummary
    {
        [JsonProperty("url")]
        public String Url { get; set; }

        [JsonProperty("type")]
        public String Type { get; set; }

        [JsonProperty("id")]
        public Int32 Id { get; set; }

        [JsonProperty("title")]
        public String Title { get; set; }
    }

    public sealed class Target
    {
        [JsonProperty("is_public")]
        public bool IsPublic { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("introduction")]
        public string Introduction { get; set; }

        [JsonProperty("avatar_url")]
        public string AvatarUrl { get; set; }

        [JsonProperty("author")]
        public Author Author { get; set; }

        [JsonProperty("url")]
        public String Url { get; set; }

        [JsonProperty("question")]
        public QuestionSummary Question { get; set; }

        [JsonProperty("excerpt")]
        public String Excerpt { get; set; }

        [JsonProperty("updated_time")]
        public Int32? UpdatedTime { get; set; }

        [JsonProperty("comment_count")]
        public Int32? CommentCount { get; set; }

        [JsonProperty("created_time")]
        public Int32? CreatedTime { get; set; }

        [JsonProperty("voteup_count")]
        public Int32? VoteupCount { get; set; }

        [JsonProperty("thanks_count")]
        public Int32? ThanksCount { get; set; }

        [JsonProperty("follower_count")]
        public Int32? FollowerCount { get; set; }

        [JsonProperty("type")]
        public String Type { get; set; }

        public Int32 GetId()
        {
            var id = -1;
          
            var result = Int32.TryParse(Id, out id);
           
            if (result == false)
            {
            }

            return id;
        }

        [JsonProperty("id")]
        public String Id { get; set; }

        [JsonProperty("title")]
        public String Title { get; set; }

        [JsonProperty("answer_count")]
        public Int32? AnswerCount { get; set; }

        [JsonProperty("updated")]
        public Int32? Updated { get; set; }

        [JsonProperty("is_author_follower")]
        public Boolean? IsAuthorFollower { get; set; }

        [JsonProperty("column")]
        public Column Column { get; set; }

        [JsonProperty("image_url")]
        public String ImageUrl { get; set; }
    }
}