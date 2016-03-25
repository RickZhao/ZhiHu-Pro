using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zhihu.Common.Model
{
    public sealed class ImageUploadResult
    {
        [JsonProperty("class")]
        public String Class { get; private set; }

        [JsonProperty("data-rawheight")]
        public Int32 RawHeight { get; set; }

        [JsonProperty("data-rawwidth")]
        public Int32 RawWidth { get; set; }

        [JsonProperty("src")]
        public String Source { get; set; }
    }
}
