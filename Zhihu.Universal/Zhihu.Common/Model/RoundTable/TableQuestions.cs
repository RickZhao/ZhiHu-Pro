using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

// ReSharper disable once CheckNamespace
namespace Zhihu.Common.Model
{
    public sealed class TableQuestions : IPaging
    {
        [JsonProperty("paging")]
        public Paging Paging { get; set; }

        [JsonProperty("data")]
        public TableQuestion[] Items { get; set; }

        public object[] GetItems()
        {
            return Items;
        }
    }

    public sealed class TableQuestion
    {
        [JsonProperty("follower_count")]
        public int FollowerCount { get; set; }

        [JsonProperty("title")]
        public string Title { get; set; }

        [JsonProperty("url")]
        public string Url { get; set; }

        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("answer_count")]
        public int AnswerCount { get; set; }
    }
}
