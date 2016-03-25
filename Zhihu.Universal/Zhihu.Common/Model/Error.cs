using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace Zhihu.Common.Model
{
    public sealed class Error
    {
        [JsonProperty("message")]
        public String Message { get; set; }

        [JsonProperty("code")]
        public Int32 Code { get; set; }
    }

    public sealed class Response
    {
        [JsonProperty("error")]
        public Error Error { get; set; }
    }
}
