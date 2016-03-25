using System;

using Newtonsoft.Json;


namespace Zhihu.Common.Model
{
    public sealed class Invite
    {
        [JsonProperty("success")]
        public Boolean IsSccess { get; set; }
    }
}
