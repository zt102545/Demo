using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZZTCoreAPI.Common
{
    public class Appsettings
    {
        private static IConfiguration Configuration { get; set; }

        static Appsettings()
        {
            //ReloadOnChange = true 当appsettings.json被修改时重新加载
            Configuration = new ConfigurationBuilder()
            .Add(new JsonConfigurationSource { Path = "appsettings.json", ReloadOnChange = true })//请注意要把当前appsetting.json 文件->右键->属性->复制到输出目录->始终复制
            .Build();
        }
        public static string GetConfigure(string key)
        {
            //取配置根下的 name 部分,多层用":"分隔
            return Configuration.GetSection(key).Value; ;
        }

        public static void SetConfigure(string key, string value)
        {
            string jsonfile = Directory.GetCurrentDirectory() + "/appsettings.json";
            if (File.Exists(jsonfile))
            {
                string jsonString = File.ReadAllText(jsonfile, Encoding.Default);//读取文件
                JObject jobject = JObject.Parse(jsonString);//解析成json
                jobject[key] = value;//替换需要的文件
                string convertString = Convert.ToString(jobject);//将json装换为string
                File.WriteAllText(jsonfile, convertString);//将内容写进jon文件中
            }
        }
    }
}
