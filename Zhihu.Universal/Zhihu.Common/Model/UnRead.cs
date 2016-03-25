using System;

using Newtonsoft.Json;

namespace Zhihu.Common.Model
{
    public sealed class UnRead
    {
        [JsonProperty("message")]
        public Int32 Message { get; set; }

        [JsonProperty("new_friends")]
        public Int32 NewFriends { get; set; }

        [JsonProperty("notification")]
        public Int32 Notification { get; set; }

        [JsonProperty("notification_content")]
        public Int32 NotificationContent { get; set; }

        [JsonProperty("notification_love")]
        public Int32 NotificationLove { get; set; }
    }
}
