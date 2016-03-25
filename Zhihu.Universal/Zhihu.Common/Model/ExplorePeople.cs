using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace Zhihu.Common.Model
{
    public sealed class ExplorePeople : IPaging
    {
        [JsonProperty("paging")]
        public Paging Paging { get; set; }

        [JsonProperty("data")]
        public Author[] Items { get; set; }

        public object[] GetItems()
        {
            return Items;
        }
    }
}
