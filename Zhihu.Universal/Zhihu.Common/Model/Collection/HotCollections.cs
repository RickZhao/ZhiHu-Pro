
using System.Linq;

using Newtonsoft.Json;


// ReSharper disable once CheckNamespace
namespace Zhihu.Common.Model
{
    public sealed class HotCollections : IPaging
    {
        public Paging Paging { get; set; }

        [JsonProperty("data")]
        public HotCollection[] Items { get; set; }

        public object[] GetItems()
        {
            return Items.ToArray();
        }
    }
}
