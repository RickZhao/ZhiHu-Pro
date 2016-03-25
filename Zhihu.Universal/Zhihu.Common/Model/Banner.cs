using System;

using Newtonsoft.Json;


namespace Zhihu.Common.Model
{
    public sealed class Banner
    {
        [JsonProperty("default")]
        public BannerItem[] Default { get; set; }

        [JsonProperty("normal")]
        public BannerItem[] Normal { get; set; }
    }

    public sealed class BannerItem
    {
        [JsonProperty("url")]
        public String Url { get; set; }

        [JsonProperty("image_url")]
        public String ImageUrl { get; set; }

        [JsonProperty("id")]
        public Int32 Id { get; set; }

        [JsonProperty("title")]
        public String Title { get; set; }
    }
}
