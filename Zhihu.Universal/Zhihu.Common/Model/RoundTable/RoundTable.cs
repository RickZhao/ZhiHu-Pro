using System;
using GalaSoft.MvvmLight;
using Newtonsoft.Json;

using Zhihu.Common.Helper;


// ReSharper disable once CheckNamespace
namespace Zhihu.Common.Model
{
    public sealed class RoundTable : ObservableObject
    {
        private Boolean _isFollowing = false;

        [JsonProperty("is_following")]
        public Boolean IsFollowing
        {
            get { return _isFollowing; }
            set
            {
                _isFollowing = value;
                RaisePropertyChanged(() => IsFollowing);
            }
        }

        [JsonProperty("name")]
        public String Name { get; set; }

        [JsonProperty("url")]
        public String Url { get; set; }

        [JsonProperty("topics")]
        public Topic[] Topics { get; set; }

        [JsonProperty("state")]
        public String State { get; set; }
        
        [JsonProperty("count_down")]
        public String CountDown { get; set; }

        [JsonProperty("followers")]
        public Int32 Followers { get; set; }

        public DateTime Start => Utility.Instance.GetFromeTimestamp(StartTime);

        [JsonProperty("start_time")]
        public Int32 StartTime { get; set; }

        public DateTime Stop => Utility.Instance.GetFromeTimestamp(StopTime);

        [JsonProperty("stop_time")]
        public Int32 StopTime { get; set; }

        [JsonProperty("visits")]
        public Int32 Visits { get; set; }

        [JsonProperty("logo")]
        public String Logo { get; set; }

        [JsonProperty("type")]
        public String Type { get; set; }

        [JsonProperty("id")]
        public String Id { get; set; }

        [JsonProperty("description")]
        public String Description { get; set; }
    }
}
