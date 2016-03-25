using System;

using Newtonsoft.Json;


namespace Zhihu.Common.Model
{
    public sealed class Anonymous
    {
        [JsonProperty("is_anonymous")]
        public Boolean IsAnonymous { get; set; }
    }
}
