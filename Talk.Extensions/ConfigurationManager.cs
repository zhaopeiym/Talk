using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System.IO;

namespace Talk.Extensions
{
    public static class ConfigurationManager
    {
        public readonly static IConfiguration Configuration;

        static ConfigurationManager() =>
            Configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: true)
                .Build();


        public static T GetSection<T>(string key) where T : class, new()
        {
            var obj = new ServiceCollection()
                .AddOptions()
                .Configure<T>(t => Configuration.GetSection(key))
                .BuildServiceProvider()
                .GetService<IOptions<T>>()
                .Value;
            return obj;
        }

        /// <summary>
        /// 获取配置文件
        /// </summary>
        /// <param name="key"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public static string GetConfig(this string key, string defaultValue = "")
        {
            try
            {
                var value = Configuration.GetValue<string>(key);
                if (string.IsNullOrWhiteSpace(value))
                {
                    if (!string.IsNullOrWhiteSpace(defaultValue))
                        return defaultValue;
                    throw new System.Exception($"获取配置{key}异常");
                }
                return value;
            }
            catch (System.Exception)
            {
                if (!string.IsNullOrWhiteSpace(defaultValue))
                    return defaultValue;
                throw new System.Exception($"获取配置{key}异常");
                throw;
            }
        }
    }
}
