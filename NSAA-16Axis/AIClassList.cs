using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NSAA_16Axis
{
    public  class AIClassList
    {
        public static Dictionary<string, Int16> ClassList = new Dictionary<string, Int16>();
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
    }
}
