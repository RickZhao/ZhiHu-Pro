using System;
using System.Collections.Generic;
using System.Text;
using GalaSoft.MvvmLight;
using Newtonsoft.Json;

// ReSharper disable once CheckNamespace
namespace Zhihu.Common.Model
{
    public sealed class Article:ObservableObject
    {
        [JsonProperty("updated")]
        public Int32 Updated { get; set; }

        [JsonProperty("id")]
        public Int32 Id { get; set; }

        [JsonProperty("can_comment")]
        public AnswerCanComment CanComment { get; set; }

        [JsonProperty("title")]
        public String Title { get; set; }

        [JsonProperty("url")]
        public String Url { get; set; }

        [JsonProperty("comment_permission")]
        public String CommentPermission { get; set; }

        [JsonProperty("author")]
        public Author Author { get; set; }

        [JsonProperty("excerpt")]
        public String Excerpt { get; set; }

        [JsonProperty("content")]
        public String Content { get; set; }

        [JsonProperty("column")]
        public Column Column { get; set; }

        [JsonProperty("comment_count")]
        public Int32 CommentCount { get; set; }

        [JsonProperty("image_url")]
        public String ImageUrl { get; set; }

        private Int32 _voting;

        [JsonProperty("voting")]
        public Int32 Voting
        {
            get { return _voting; }
            set
            {
                _voting = value;
                RaisePropertyChanged(() => Voting);
            }
        }

        private Int32 _voteupCount;

        [JsonProperty("voteup_count")]
        public Int32 VoteupCount
        {
            get { return _voteupCount; }
            set
            {
                _voteupCount = value;
                RaisePropertyChanged(() => VoteupCount);
            }
        }

        [JsonProperty("type")]
        public String Type { get; set; }

        [JsonProperty("suggest_edit")]
        public SuggestEdit SuggestEdit { get; set; }
    }
}
