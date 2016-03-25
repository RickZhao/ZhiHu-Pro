using System;

using Newtonsoft.Json;


namespace Zhihu.Common.Model
{
    public struct PeopleFollowing
    {
        [JsonProperty("is_followed")]
        public Boolean Followed { get; set; }

        [JsonProperty("is_following")]
        public Boolean Following { get; set; }
    }
}
