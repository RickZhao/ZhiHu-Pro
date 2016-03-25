using System;

using Newtonsoft.Json;

using GalaSoft.MvvmLight;


// ReSharper disable once CheckNamespace
namespace Zhihu.Common.Model
{
    public sealed class NotifyItem : ObservableObject
    {
        [JsonProperty("count")]
        public Int32 Count { get; set; }

        [JsonProperty("updated_time")]
        public Int32 UpdatedTime { get; set; }

        [JsonProperty("target")]
        public Target Target { get; set; }

        [JsonProperty("operators")]
        public Operator[] Operators { get; set; }

        [JsonProperty("action_count")]
        public int ActionCount { get; set; }

        [JsonProperty("action_name")]
        public String ActionName { get; set; }

        [JsonProperty("thank_count")]
        public int ThankCount { get; set; }
        
        [JsonProperty("vote_count")]
        public int VoteCount { get; set; }

        private Boolean _isRead = false;
        [JsonProperty("is_read")]
        public Boolean IsRead
        {
            get { return _isRead; }
            set
            {
                _isRead = value;
                RaisePropertyChanged();
            }
        }

        [JsonProperty("thread_id")]
        public String ThreadId { get; set; }

        [JsonProperty("group_name")]
        public String GroupName { get; set; }

        [JsonProperty("type")]
        public String Type { get; set; }

        [JsonProperty("id")]
        public String Id { get; set; }

        [JsonProperty("answer")]
        public Answer Answer { get; set; }
    }
}
