using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace WrackEngine
{
    public static class Settings
    {
        public const string DEFAULT_SETTINGS_FILE = "settings.txt";
        public static string SettingsFile = DEFAULT_SETTINGS_FILE;

        public static Dictionary<string, string> SettingsList = new Dictionary<string, string>();

        public static void Initialize()
        {
        }

        public static void Load(string file)
        {
            SettingsFile = file;

            if (!File.Exists(file))
            {
                Save(file);
                return;
            }

            Dictionary<string, string> fileSettings = new Dictionary<string, string>();

            FileStream fs = new FileStream(file, FileMode.Open, FileAccess.Read);
            StreamReader sr = new StreamReader(fs);

            string str = sr.ReadToEnd();
            string[] lines = str.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries);
            for (int i = 0; i < lines.Length; i++)
            {
                string[] split = lines[i].Split(new char[] { '=' });
                if (split.Length > 1)
                {
                    string key = split[0];
                    string val = split[1];
                    for (int j = 2; j < split.Length; j++)
                    {
                        val += "=" + split[j];
                    }
                    fileSettings.Add(key.Trim(), val.Trim());
                }
            }

            foreach (KeyValuePair<string, string> kvp in fileSettings)
            {
                SetSetting(kvp.Key, kvp.Value);
            }

            sr.Close();
            fs.Close();
        }

        public static void Save(string file)
        {
            SettingsFile = file;

            FileStream fs = new FileStream(file, FileMode.Create, FileAccess.Write);
            StreamWriter sw = new StreamWriter(fs);
            string appName = System.Windows.Forms.Application.ProductName;
            sw.WriteLine("# " + appName + " Settings");
            sw.WriteLine("# " + DateTime.Now.ToString());
            foreach (KeyValuePair<string, string> kvp in SettingsList)
            {
                sw.WriteLine(kvp.Key + " = " + kvp.Value + ";");
            }
            sw.Flush();
            sw.Close();
            fs.Close();
        }

        public static void Load() { Load(SettingsFile); }
        public static void Save() { Save(SettingsFile); }

        public static void SetSetting(string key, string value)
        {
            if (SettingsList.ContainsKey(key))
            {
                SettingsList[key] = value;
            }
            else
            {
                SettingsList.Add(key, value);
            }
        }

        public static string GetSetting(string key)
        {
            if (SettingsList.ContainsKey(key))
            {
                return SettingsList[key];
            }
            else
            {
                return "";
            }
        }

        public static int GetIntSetting(string key)
        {
            int i = 0;
            int.TryParse(GetSetting(key), out i);
            return i;
        }

        public static double GetDoubleSetting(string key)
        {
            double i = 0;
            double.TryParse(GetSetting(key), out i);
            return i;
        }
    }
}
