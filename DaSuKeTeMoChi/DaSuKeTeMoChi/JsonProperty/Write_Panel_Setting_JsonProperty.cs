using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DaSuKeTeMoChi.JsonProperty
{
    internal class Write_Panel_Setting_JsonProperty
    {
        [JsonProperty("textColor")]
        public string TextColor { get; set; }

        [JsonProperty("fieldColor")]
        public string FieldColor { get; set; } 

        [JsonProperty("sliderValue")]
        public string SliderValue { get; set; } 
    }
}
