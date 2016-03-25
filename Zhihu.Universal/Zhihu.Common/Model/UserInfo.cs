using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace Zhihu.Common.Model
{
    public sealed class UserInfo
    {
        [JsonProperty("token_type")]
        public String TokenType { get; set; }

        [JsonProperty("access_token")]
        public String AccessToken { get; set; }

        [JsonProperty("cookie")]
        public UserCookie Cookie { get; set; }

        [JsonProperty("expires_in")]
        public Int32 ExpiresIn { get; set; }

        [JsonProperty("refresh_token")]
        public String RefreshToken { get; set; }
    }


    public sealed class UserCookie
    {
        [JsonProperty("q_c0")]
        public String Qc0 { get; set; }
    }
}
