using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DaSuKeTeMoChi.JsonProperty
{
    internal class Write_Panel_Area_JsonProperty
    {
        [JsonProperty("x")]
        public double XPath{ get; set; }

        [JsonProperty("y")]
        public double YPath{ get; set; }
    }
}
