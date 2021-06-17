using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UtilityClassDLL;

namespace DaSuKeTeMoChi.UiConfig
{
    public class CheckConfigInfo
    {
        public bool isDirectoryCheck = false;
  
        public void DirectoryCheck()
        {

            if (isDirectoryCheck) return;

            string Path = @"Config";
            DirectoryInfo directory = new DirectoryInfo(Path);

            if (!directory.Exists)
            {
                directory.Create();
            }

            isDirectoryCheck = true;
        }
         
        public bool CheckController(string Sector)
        {
            try
            {
                DirectoryCheck();

                bool ExitsTxtFile = File.Exists(Sector);

                JObject job = new JObject();

                if (ExitsTxtFile) return true;

                if (Sector.Equals(ResourceDex.PathResource.HotKey))
                {
                    job.Add("inputSelectKey", "");
                    job.Add("outputCopyKey", "");
                }
                else if (Sector.Equals(ResourceDex.PathResource.WritePannel))
                {
                    job.Add("textColor", "#FF000000");
                    job.Add("fieldColor", "#FF000000");
                    job.Add("sliderValue", "5");
                }
                else if (Sector.Equals(ResourceDex.PathResource.Location))
                {
                    job.Add("x", 0);
                    job.Add("y", 0);
                }

                File.WriteAllText(Sector, job.ToString());
                return true;
            }
            catch (Exception)
            {

                return false;
            } 
        } 
    }
}
