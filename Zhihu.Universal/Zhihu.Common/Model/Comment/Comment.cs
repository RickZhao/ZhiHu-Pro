using System;

using GalaSoft.MvvmLight;

using Newtonsoft.Json;

using Zhihu.Common.Helper;


// ReSharper disable once CheckNamespace
namespace Zhihu.Common.Model
{
    public sealed class Comment : ObservableObject
    {
        [JsonProperty("id")]
        public Int32 Id { get; set; }

        [JsonProperty("type")]
        public String Type { get; set; }

        [JsonProperty("url")]
        public String Url { get; set; }

        [JsonProperty("content")]
        public String Content { get; set; }

        [JsonProperty("author")]
        public Author Author { get; set; }

        [JsonProperty("allow_reply")]
        public Boolean AllowReply { get; set; }

        [JsonProperty("is_author")]
        public Boolean IsAuthor { get; set; }

        [JsonProperty("is_parent_author")]
        public Boolean IsParentAuthor { get; set; }

        [JsonProperty("allow_delete")]
        public Boolean AllowDelete { get; set; }

        [JsonProperty("allow_vote")]
        public bool AllowVote { get; set; }

        [JsonProperty("is_delete")]
        public bool IsDelete { get; set; }

        [JsonProperty("ancestor")]
        public Boolean Ancestor { get; set; }

        private Int32 _voteCount = -1;

        [JsonProperty("vote_count")]
        public Int32 VoteCount
        {
            get { return _voteCount; }
            set
            {
                _voteCount = value;
                RaisePropertyChanged(() => VoteCount);
            }
        }

        public DateTime Created => Utility.Instance.GetFromeTimestamp(CreatedTime);

        [JsonProperty("created_time")]
        public Int32 CreatedTime { get; set; }

        private Boolean _voting;

        [JsonProperty("voting")]
        public Boolean Voting
        {
            get { return _voting; }
            set
            {
                _voting = value;
                RaisePropertyChanged(() => Voting);
            }
        }
    }
}
