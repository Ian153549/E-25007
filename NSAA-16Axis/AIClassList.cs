using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NSAA_16Axis
{
    public  class AIClassList
    {
        public static Dictionary<string, Int16> ClassList = new Dictionary<string, Int16>();
        private static readonly string LabelClassNameJsonPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "ClassName.json");
        public AIClassList()
        {
        }

        public void initialize()
        {
            var classNames = GetClassNamesAsync().GetAwaiter().GetResult();
            foreach(var className in classNames)
            {
                if (!ClassList.ContainsKey(className))
                {
                    ClassList.Add(className, (Int16)ClassList.Count);
                }
            }
        }

        public async Task<List<string>> GetClassNamesAsync()
        {
            var psi = new System.Diagnostics.ProcessStartInfo
            {
                FileName = "curl.exe",
                Arguments = "http://127.0.0.1:9300/class_names",
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false,
                CreateNoWindow = true
            };
            var process = System.Diagnostics.Process.Start(psi);
            string output = process.StandardOutput.ReadToEnd();
            process.WaitForExit();
            var dict = JObject.Parse(output).ToObject<Dictionary<string, string>>();
            var names = new List<string>(dict.Values);
            return names;
        }

        public Dictionary<string, Int16> GetClassList()
        {
            return ClassList;
        }
        public void AddClass(string className)
        {
            if (!ClassList.ContainsKey(className))
            {
                ClassList.Add(className, (Int16)ClassList.Count)
;            }
        }

        public static void SaveLabelClassNameToJson(Dictionary<string , Int16> labelClasses)
        {
            try
            {
                var orderedList = labelClasses.OrderBy(kvp => kvp.Value).ToDictionary(kvp => kvp.Key, kvp => kvp.Value);
                string json = JsonConvert.SerializeObject(orderedList, Formatting.Indented);
                File.WriteAllText(LabelClassNameJsonPath, json);
            }
            catch
            {

            }
        }
        public static Dictionary<string, Int16> LoadLabelClassNameFromJson()
        {
            try
            {
                if (File.Exists(LabelClassNameJsonPath))
                {
                    string json = File.ReadAllText(LabelClassNameJsonPath);
                    var loadedClassList = JsonConvert.DeserializeObject<Dictionary<string, Int16>>(json);
                    if(loadedClassList!=null && loadedClassList.Count > 0)
                    {
                        return loadedClassList; 
                    }
                }
            }
            catch
            {

            }
            return new Dictionary<string, Int16>();
        }
    }
}
