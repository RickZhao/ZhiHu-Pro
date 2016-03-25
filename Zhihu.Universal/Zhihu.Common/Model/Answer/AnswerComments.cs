using System;
using System.Collections.Generic;
using System.Linq;

using Newtonsoft.Json;


// ReSharper disable once CheckNamespace
namespace Zhihu.Common.Model
{
    public sealed class AnswerComments : IPaging
    {
        [JsonProperty("paging")]
        public Paging Paging { get; set; }

        [JsonProperty("data")]
        public IList<Comment> Items { get; set; }

        Object[] IPaging.GetItems()
        {
            return Items.ToArray();
        }
    }
}
