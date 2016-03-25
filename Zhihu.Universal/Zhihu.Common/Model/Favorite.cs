using System;

using Newtonsoft.Json;

namespace Zhihu.Common.Model
{
    public sealed class Favorite
    {
        [JsonProperty("is_favorited")]
        public Boolean IsFavorited { get; set; }
    }
}
