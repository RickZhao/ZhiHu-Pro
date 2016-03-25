using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

// ReSharper disable once CheckNamespace
namespace Zhihu.Common.Model
{
    public sealed class Articles : IPaging
    {
        [JsonProperty("paging")]
        public Paging Paging { get; set; }

        [JsonProperty("data")]
        public Article[] Items { get; set; }

        public object[] GetItems()
        {
            return Items;
        }
    }
}
