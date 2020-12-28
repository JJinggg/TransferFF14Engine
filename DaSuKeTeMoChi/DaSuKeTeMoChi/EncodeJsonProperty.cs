using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DaSuKeTeMoChi
{
    internal class EncodeJsonProperty
    {
        [JsonProperty("stringReqv")]
        public string EncodedTranslationRequest { get; set; }

        [JsonProperty("hmacKey")]
        public string HmacKey { get; set; }

        [JsonProperty("hmacInput")]
        public string HmacInput { get; set; }

        [JsonProperty("guid")]
        public string Guid { get; set; }

        [JsonProperty("guidTime")]
        public string GuidTime { get; set; }
    }
}
