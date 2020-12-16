using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Runtime.CompilerServices;
using System.Text;
using MgTube.Annotations;
using Newtonsoft.Json;

namespace MgTube
{
    public static class AppSettings
    {
        public static Properties Properties { get; private set; } = new Properties();

        private static readonly string JsonDirectory = $@"{Directory.GetCurrentDirectory()}\AppData";
        private static readonly string JsonFile = $@"{JsonDirectory}\settings.json";

        public static void Save()
        {
            using var fs = new StreamWriter(JsonFile, false, Encoding.UTF8);
            var json = JsonConvert.SerializeObject(Properties);
            fs.Write(json);
        }

        public static void Load()
        {
            if (!Directory.Exists(JsonDirectory))
            {
                Directory.CreateDirectory(JsonDirectory);
            }
            if (!File.Exists(JsonFile))
            {
                File.Create(JsonFile).Close();
            }
            using var fs = new StreamReader(JsonFile, Encoding.UTF8);
            var json = fs.ReadToEnd();
            Properties = JsonConvert.DeserializeObject<Properties>(json) ?? new Properties();
        }

        static AppSettings()
        {
            Load();
        }
    }

    public class Properties
    {
        public string YtKey { get; set; }
    }
}
