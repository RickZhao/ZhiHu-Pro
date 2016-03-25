using System;

using Newtonsoft.Json;


namespace Zhihu.Common.Model
{
    public sealed class Education
    {
        [JsonProperty("name")]
        public String Name { get; set; }

        [JsonProperty("url")]
        public String Url { get; set; }

        [JsonProperty("excerpt")]
        public String Excerpt { get; set; }

        [JsonProperty("experience")]
        public String Experience { get; set; }

        [JsonProperty("introduction")]
        public String Introduction { get; set; }

        [JsonProperty("avatar_url")]
        public String AvatarUrl { get; set; }

        [JsonProperty("type")]
        public String Type { get; set; }

        [JsonProperty("id")]
        public Int32? Id { get; set; }
    }
}