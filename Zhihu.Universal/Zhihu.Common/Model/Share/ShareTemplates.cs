using System;
using System.Collections.Generic;
using System.Text;

using Newtonsoft.Json;


namespace Zhihu.Common.Model.Share
{
    public sealed class ShareTemplates
    {
        [JsonProperty("templates")]
        public Templates Templates { get; set; }

        [JsonProperty("status")]
        public Status Status { get; set; }

        [JsonProperty("type")]
        public String Type { get; set; }
    }

    public sealed class Templates
    {
        [JsonProperty("short_url")]
        public String ShortUrl { get; set; }

        [JsonProperty("sina")]
        public String Sina { get; set; }
    }

    public sealed class ShareStatus
    {
        [JsonProperty("qq")]
        public Qq Qq { get; set; }

        [JsonProperty("sina")]
        public Sina Sina { get; set; }
    }

    public sealed class Sina
    {
        [JsonProperty("code")]
        public int Code { get; set; }
    }

    public sealed class Qq
    {

    }
}
