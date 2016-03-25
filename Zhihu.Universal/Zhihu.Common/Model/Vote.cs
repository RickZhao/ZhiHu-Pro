using System;

using Newtonsoft.Json;


namespace Zhihu.Common.Model
{
    public sealed class Vote
    {
        [JsonProperty("voteup_count")]
        public Int32 VoteupCount { get; set; }

        [JsonProperty("voting")]
        public Int32 Voting { get; set; }
    }

    public sealed class Voted
    {
        [JsonProperty("vote_count")]
        public Int32 VoteupCount { get; set; }
        
        [JsonProperty("voting")]
        public Boolean Voting { get; set; }
    }
}
