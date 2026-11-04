using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrainModel;

namespace NSAA_16Axis
{
    class GV 
    {
        public static AIClassList AIClassList = new AIClassList();
        public static AppSettingParm AppSettingParm = new AppSettingParm();
        public static DisplayLanguage Dlang = new DisplayLanguage();
        public static AIConfig AIConfig = new AIConfig();
        public static LanguageType CurrentLanguage = LanguageType.English;
        public static void SetLanguage(LanguageType language)
        {
            CurrentLanguage = language;
            AppSettingParm.Language = language.ToString();
        }

        public static void LoadLanguageFromConfig()
        {
            if (Enum.TryParse(AppSettingParm.Language, out LanguageType lang))
            {
                CurrentLanguage = lang;
            }
        }
    }
    class YoloBox
    {
        public int ClassId;
        public float X, Y, W, H;
        public string ClassName { get; set; }
    }

    //public class AppSettingParm
    //{
    //    public string Language = "default";
    //}

    public class DisplayLanguage
    {

    }
    public class AppSettingParm
    {
        public string Language = "English";

        public static AppSettingParm LoadFromJson()
        {
            string configPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "AppSettings.json");
            if (File.Exists(configPath))
            {
                string json = File.ReadAllText(configPath);
                return JsonConvert.DeserializeObject<AppSettingParm>(json);
            }
            return new AppSettingParm();
        }

        public void SaveToJson()
        {
            string configPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "AppSettings.json");
            string json = JsonConvert.SerializeObject(this, Formatting.Indented);
            File.WriteAllText(configPath, json);
        }
    }
    public class AIConfig
    {
        public string TrainPath { get; set; }
        public string ModelPath { get; set; }
        public string ModelBackupPath { get; set; }
        public int LabeledRecipeNumber { get; set; }

        public string trainBasePath { get; set; }

        public string labelImage { get; set; }
        public string serviceExePath { get; set; }
        public string trainModel { get; set; }
        public int lastClassCount { get; set; }

        public List<string> lastClassNames { get; set; } = new List<string>();
        public static AIConfig LoadFromJson()
        {
            string configPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "AIConfig.json");
            if (File.Exists(configPath))
            {
                string json =File.ReadAllText(configPath);
                return Newtonsoft.Json.JsonConvert.DeserializeObject<AIConfig>(json);
            }
            else
            {
                return new AIConfig();
            }
        }
        public void SaveToJson()
        {
            string configPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "AIConfig.json");
            string json = Newtonsoft.Json.JsonConvert.SerializeObject(this, Newtonsoft.Json.Formatting.Indented);
            File.WriteAllText(configPath, json);
        }

    } 
}
