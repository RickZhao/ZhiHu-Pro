using Newtonsoft.Json;


namespace Zhihu.Common.Model
{
    public sealed class HotAnswers : IPaging
    {
        [JsonProperty("paging")]
        public Paging Paging { get; set; }
        
        [JsonProperty("data")]
        public HotAnswer[] Answers { get; set; }

        public object[] GetItems()
        {
            return Answers;
        }
    }

    public sealed class HotAnswer
    {
        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("answers")]
        public Answer[] Answers { get; set; }
    }
}
