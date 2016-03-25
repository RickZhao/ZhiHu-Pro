using System;
using System.Collections.Generic;
using System.Text;
using GalaSoft.MvvmLight;
using Newtonsoft.Json;


// ReSharper disable once CheckNamespace
namespace Zhihu.Common.Model
{
    public sealed class Question : ObservableObject
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

        private Int32 _commentCount;

        [JsonProperty("comment_count")]
        public Int32 CommentCount
        {
            get { return _commentCount; }
            set
            {
                _commentCount = value;
                RaisePropertyChanged(() => CommentCount);
            }
        }

        [JsonProperty("draft")]
        public Draft Draft { get; set; }

        [JsonProperty("redirection")]
        public Redirection Redirection { get; set; }

        private Int32 _followerCount = 0;

        [JsonProperty("follower_count")]
        public Int32 FollowerCount
        {
            get { return _followerCount; }
            set
            {
                _followerCount = value;
                RaisePropertyChanged(() => FollowerCount);
            }
        }

        [JsonProperty("type")]
        public String Type { get; set; }

        [JsonProperty("id")]
        public Int32 Id { get; set; }
    }

    public sealed class Draft
    {
    }

    public sealed class Redirection
    {
        [JsonProperty("to")]
        public To To { get; set; }

        [JsonProperty("from")]
        public object[] From { get; set; }
    }

    public sealed class Status
    {
    }

    public sealed class To
    {
    }

}
