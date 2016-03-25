using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace Zhihu.Common.Model
{
    public sealed class HotTopicsCollections
    {
        [JsonProperty("topics")]
        public Topic[] Topics { get; set; }

        [JsonProperty("collections")]
        public Collection[] Collections { get; set; }
    }
}
