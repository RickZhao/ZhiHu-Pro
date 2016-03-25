using System;

using Newtonsoft.Json;


namespace Zhihu.Common.Model
{
    public sealed class Operator
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
}
