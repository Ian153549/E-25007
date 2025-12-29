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

        public static void SaveLabelClassNameToJson(Dictionary<string , Int16> labelClasses, int recipeNumber)
        {
            string trainPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Python", "Train");
            string recipePath = Path.Combine(trainPath, $"{recipeNumber}");
            //try
            //{
            //    var orderedList = labelClasses.OrderBy(kvp => kvp.Value).ToDictionary(kvp => kvp.Key, kvp => kvp.Value);
            //    string json = JsonConvert.SerializeObject(orderedList, Formatting.Indented);
            //    File.WriteAllText(LabelClassNameJsonPath, json);
            //}
            //catch
            //{

            //}
            if (!Directory.Exists(recipePath))
            {
                Directory.CreateDirectory(recipePath);
            }
            string jsonPath = Path.Combine(recipePath, "ClassName.json");
            string json = Newtonsoft.Json.JsonConvert.SerializeObject(labelClasses, Newtonsoft.Json.Formatting.Indented);
            File.WriteAllText(jsonPath, json);
        }
        public static Dictionary<string, Int16> LoadLabelClassNameFromJson(int recipeNumber)
        {
            string trainPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Python", "Train");
            string recipePath = Path.Combine(trainPath, $"{recipeNumber}");
            string jsonPath= Path.Combine(recipePath, "ClassName.json");
            if (!Directory.Exists(recipePath))
            {
                Directory.CreateDirectory(recipePath);
            }
            
            if (!File.Exists(jsonPath))
            {
                var emptyClassList = new Dictionary<string, Int16>();
                string emptyJson = JsonConvert.SerializeObject(emptyClassList, Formatting.Indented);
                File.WriteAllText(jsonPath, emptyJson);
                return emptyClassList;
            }

            try
            {
                string json = File.ReadAllText(jsonPath);
                var loadedClassList = JsonConvert.DeserializeObject<Dictionary<string, Int16>>(json);

                
                if (loadedClassList == null || loadedClassList.Count == 0)
                {
                    return new Dictionary<string, Int16>();
                }

                return loadedClassList;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"載入 ClassName.json 失敗: {ex.Message}");
                return new Dictionary<string, Int16>();
            }
            //try
            //{
            //    if (File.Exists(LabelClassNameJsonPath))
            //    {
            //        string json = File.ReadAllText(LabelClassNameJsonPath);
            //        var loadedClassList = JsonConvert.DeserializeObject<Dictionary<string, Int16>>(json);
            //        if(loadedClassList!=null && loadedClassList.Count > 0)
            //        {
            //            return loadedClassList;
            //        }
            //    }
            //}
            //catch
            //{

            //}
            //return new Dictionary<string, Int16>();
        }
        
    }
}
