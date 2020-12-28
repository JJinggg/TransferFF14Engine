using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DaSuKeTeMoChi
{
    internal class SendDataRequest
    {
        public string deviceId { get; set; }

        public string locale { get; set; }

        public bool dict { get; set; }

        public long dictDisplay { get; set; }

        public bool honorific { get; set; }

        public bool instant { get; set; }

        public bool paging { get; set; }

        public string source { get; set; }

        public string target { get; set; }

        public string text { get; set; }
    }
}
