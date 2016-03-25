using System;

using Newtonsoft.Json;
using Windows.UI.Xaml;


// ReSharper disable once CheckNamespace
namespace Zhihu.Common.Model
{
    public sealed class Author
    {
        [JsonProperty("headline")]
        public String Headline { get; set; }

        [JsonProperty("avatar_url")]
        public String AvatarUrl { get; set; }

        [JsonProperty("name")]
        public String Name { get; set; }

        [JsonProperty("url")]
        public String Url { get; set; }

        [JsonProperty("gender")]
        public Int32 Gender { get; set; }

        [JsonProperty("type")]
        public String Type { get; set; }

        [JsonProperty("id")]
        public String Id { get; set; }

        public static void ClearValue(DependencyProperty textProperty)
        {
            throw new NotImplementedException();
        }
    }
}
