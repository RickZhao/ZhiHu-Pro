using System;

using Newtonsoft.Json;


namespace Zhihu.Common.Model
{
    public struct Operation
    {
        [JsonProperty("success")]
        public Boolean Succeed { get; set; }
    }
}
