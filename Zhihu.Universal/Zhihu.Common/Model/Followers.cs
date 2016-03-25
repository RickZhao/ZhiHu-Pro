using System;
using System.Collections.Generic;
using System.Text;

using Newtonsoft.Json;


namespace Zhihu.Common.Model
{
    public sealed class Followers : IPaging
    {
        [JsonProperty("paging")]
        public Paging Paging { get; set; }

        [JsonProperty("data")]
        public Profile[] Data { get; set; }

        public object[] GetItems()
        {
            return Data;
        }
    }
}
