
using Newtonsoft.Json;


// ReSharper disable once CheckNamespace

namespace Zhihu.Common.Model
{
    public sealed class TopStoryStatus
    {
        [JsonProperty("enable")]
        public bool Enable { get; set; }
    }

    public sealed class UseTopStory
    {
        [JsonProperty("use_topstory")]
        public TopStoryStatus Status { get; set; }
    }
}
