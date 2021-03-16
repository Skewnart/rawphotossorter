using System;
using System.Collections.Generic;
using System.IO;

namespace RAWPhotosSorter.Global
{
    public static class Config
    {
        private static string RAWPHOTOSORTERFOLER = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "RAWPhotosSorter");
        private static string CONFIGFILE = Path.Combine(RAWPHOTOSORTERFOLER, "config.txt");

        public static string JpgExtension = "";
        public static string RawExtension = "";
        public static string DestinationPath = "";

        public static bool Load()
        {
            try {
                if (Directory.Exists(RAWPHOTOSORTERFOLER) && File.Exists(CONFIGFILE)) {
                    var content = ReadTextFile(CONFIGFILE);
                    if (content.ContainsKey("jpgext")) JpgExtension = content["jpgext"];
                    if (content.ContainsKey("rawext")) RawExtension = content["rawext"];
                    if (content.ContainsKey("destpath")) DestinationPath = content["destpath"];
                }

                if (String.IsNullOrEmpty(JpgExtension)) JpgExtension = "jpg";
                if (String.IsNullOrEmpty(RawExtension)) RawExtension = "raf";

                return true;
            }
            catch (Exception) {
                return false;
            }
        }

        public static bool Save()
        {
            try {
                Directory.CreateDirectory(RAWPHOTOSORTERFOLER);
                File.WriteAllLines(CONFIGFILE, new string[]
                {
                    $"jpgext = {JpgExtension}",
                    $"rawext = {RawExtension}",
                    $"destpath = {DestinationPath}"
                });

                return true;
            }
            catch(Exception) {
                return false;
            }
        }

        private static Dictionary<string, string> ReadTextFile(string pathfile)
        {
            Dictionary<string, string> content = new Dictionary<string, string>();

            foreach (string line in File.ReadAllLines(pathfile)) {
                if (!line.StartsWith("#") && line.Contains("=") && line.IndexOf("=") < line.Length - 1) {
                    content.Add(line.Substring(0, line.IndexOf("=")).Trim(),
                        line.Substring(line.IndexOf("=") + 1).Trim());
                }
            }

            return content;
        }
    }
}
