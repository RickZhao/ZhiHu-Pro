
using System;
using Newtonsoft.Json;


// ReSharper disable once CheckNamespace
namespace Zhihu.Common.Model
{
    public sealed class Notifies : IPaging
    {
        [JsonProperty("paging")]
        public Paging Paging { get; set; }

        [JsonProperty("new_count")]
        public Int32 NewCount { get; set; }

        [JsonProperty("data")]
        public NotifyItem[] Items { get; set; }

        public object[] GetItems()
        {

            return Items;
        }
    }
}
