using System;
using GalaSoft.MvvmLight;
using Newtonsoft.Json;


// ReSharper disable once CheckNamespace
namespace Zhihu.Common.Model
{
    public sealed class AnswerDetail : ObservableObject
    {
        [JsonProperty("suggest_edit")]
        public SuggestEdit SuggestEdit { get; set; }

        [JsonProperty("can_comment")]
        public AnswerCanComment CanComment { get; set; }

        [JsonProperty("is_mine")]
        public Boolean IsMine { get; set; }

        [JsonProperty("author")]
        public Author Author { get; set; }

        [JsonProperty("url")]
        public String Url { get; set; }

        private Question _question;

        [JsonProperty("question")]
        public Question Question
        {
            get { return _question; }
            set
            {
                _question = value;
                RaisePropertyChanged();
            }
        }

        [JsonProperty("excerpt")]
        public String Excerpt { get; set; }

        [JsonProperty("updated_time")]
        public Int32 UpdatedTime { get; set; }

        [JsonProperty("content")]
        public String Content { get; set; }

        private Int32 _commentCount;

        [JsonProperty("comment_count")]
        public Int32 CommentCount
        {
            get { return _commentCount; }
            set
            {
                _commentCount = value;
                RaisePropertyChanged();
            }
        }

        [JsonProperty("comment_permission")]
        public String CommentPermission { get; set; }

        private Int32 _voteupCount = 0;

        [JsonProperty("voteup_count")]
        public Int32 VoteupCount
        {
            get { return _voteupCount; }
            set
            {
                _voteupCount = value;
                RaisePropertyChanged();
            }
        }

        [JsonProperty("type")]
        public String Type { get; set; }

        [JsonProperty("id")]
        public Int32 Id { get; set; }

        [JsonProperty("thanks_count")]
        public Int32 ThanksCount { get; set; }
    }
}
