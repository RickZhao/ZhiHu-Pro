using System;

using Newtonsoft.Json;


namespace Zhihu.Common.Model
{
    public sealed class SuggestEdit
    {
        [JsonProperty("status")]
        public Boolean Status { get; set; }

        [JsonProperty("reason")]
        public String Reason { get; set; }

        [JsonProperty("url")]
        public string Url { get; set; }

        [JsonProperty("tip")]
        public string Tip { get; set; }

        [JsonProperty("title")]
        public string Title { get; set; }
    }
}
