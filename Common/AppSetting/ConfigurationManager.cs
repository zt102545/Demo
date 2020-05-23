using Microsoft.Extensions.Configuration;
using System.IO;
using System.Reflection;

namespace Common
{
    public static class ConfigurationManager
    {
        public static IConfiguration GetConfiguration(string jsonFile, bool optional = true, bool reloadOnChange = true)
        {
            string currentDirectory = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);
            //Directory.GetCurrentDirectory()

            IConfigurationBuilder builder = new ConfigurationBuilder().SetBasePath(currentDirectory)
                .AddJsonFile(jsonFile, optional: optional, reloadOnChange: reloadOnChange);
            appsettings = builder.Build();
            return appsettings;
        }

        private static IConfiguration appsettings;
        public static IConfiguration Appsettings
        {
            get
            {
                if (appsettings == null)
                {
                    appsettings = GetConfiguration("appsettings.json");
                    string env = appsettings.GetSection("ASPNETCORE_ENVIRONMENT").Value;
                    if (env == "Development")
                    {
                        appsettings = GetConfiguration($"appsettings.Development.json");
                    }
                }
                return appsettings;
            }
            set
            {
                if (appsettings != value)
                {
                    appsettings = value;
                }
            }
        }

        ///// <summary>
        ///// 获取json配置直接获取值
        ///// </summary>
        ///// <param name="key"></param>
        ///// <returns></returns>
        //public static string GetSectionValue(string key)
        //{
        //    return Appsettings.GetSection(key).Value;
        //}

        /// <summary>
        /// 获取json配置映射到模型
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <returns></returns>
        public static T GetSection<T>(string key) where T : class, new()
        {
            return Appsettings.GetSection(key).Get<T>();
        }
    }
}
