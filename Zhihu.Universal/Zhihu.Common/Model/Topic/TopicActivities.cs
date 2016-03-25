
using Newtonsoft.Json;


// ReSharper disable once CheckNamespace
namespace Zhihu.Common.Model
{
    public sealed class TopicActivities : IPaging
    {
        [JsonProperty("paging")]
        public Paging Paging { get; set; }

        [JsonProperty("data")]
        public TopicActivity[] Items { get; set; }

        public object[] GetItems()
        {
            return Items;
        }
    }
}
