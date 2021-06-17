﻿using Newtonsoft.Json.Linq;
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

        public bool CheckSettingsConfig()
        {
            try
            {
                DirectoryCheck();
                bool ExitsTxtFile = File.Exists(@"Config\Settings.txt");

                if (ExitsTxtFile)
                {
                    return true;
                }
                else
                {

                    using (System.IO.StreamWriter SaveFile = new System.IO.StreamWriter(@"Config\Settings.txt"))
                    {

                        SaveFile.WriteLine("TextColor=#000000");
                        SaveFile.WriteLine("FieldColor=#000000");
                        SaveFile.WriteLine("FieldColor=#000000");
                        SaveFile.WriteLine("SliderValue=5");
                        SaveFile.Close();
                        return true;
                    }
                }
            }
            catch(Exception e)
            {

                DebugLog.m(e);
                return false;
            }


        }

        public bool FileCheckConfig(string Filename)
        {
            return File.Exists($@"{Filename}");
        }
        public void DirectoryCheck()
        {
            string Path = @"Config";
            DirectoryInfo directory = new DirectoryInfo(Path);

            if (!directory.Exists)
            {
                directory.Create();
            }
        }

        public bool CheckHotKeyConfig()
        {
            try
            {
                DirectoryCheck();

                bool ExitsTxtFile = File.Exists(@"Config\HotKeys.json");

                if (ExitsTxtFile)
                {
                    return true;
                }
                else
                {
                     
                    JObject job = new JObject();
                    job.Add("inputSelectKey", "");
                    job.Add("outputCopyKey", "");

                    File.WriteAllText(@"Config\HotKeys.json", job.ToString());
                    return true; 
                }
            }
            catch
            {
                return false;
            }
        }
    }
}
