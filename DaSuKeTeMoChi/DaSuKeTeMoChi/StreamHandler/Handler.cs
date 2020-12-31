using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DaSuKeTeMoChi.StreamHandler
{
    public class Writer
    {
        public void WriteTextToFile(StreamWriter stream, string WriteTextValueName,string Value)
        {
                    stream.WriteLine($"{WriteTextValueName}={Value}");
        }
    }
}
