using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Newtonsoft.Json;

// ReSharper disable once CheckNamespace

namespace Zhihu.Common.Model
{
    public sealed class Questions : IPaging
    {
        [JsonProperty("paging")]
        public Paging Paging { get; set; }

        [JsonProperty("data")]
        public Question[] Items { get; set; }

        public object[] GetItems()
        {
            return Items.ToArray();
        }
    }
}
