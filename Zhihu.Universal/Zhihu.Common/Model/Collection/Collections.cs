
using Newtonsoft.Json;

// ReSharper disable once CheckNamespace
namespace Zhihu.Common.Model
{
    public sealed class Collections : IPaging
    {
        [JsonProperty("paging")]
        public Paging Paging { get; set; }

        [JsonProperty("data")]
        public Collection[] Items { get; set; }

        public object[] GetItems()
        {
            return Items;
        }
    }
}
