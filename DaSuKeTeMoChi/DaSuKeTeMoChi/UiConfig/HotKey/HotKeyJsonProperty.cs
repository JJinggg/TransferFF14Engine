using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DaSuKeTeMoChi.UiConfig.HotKey
{
    class HotKeyJsonProperty
    {
        internal class HotKeys
        {
            [JsonProperty("inputSelectKey")]
            public string InputSelectKey { get; set; }


            [JsonProperty("outputCopyKey")]
            public string OutputCopyKey { get; set; }
        }

    }
}
