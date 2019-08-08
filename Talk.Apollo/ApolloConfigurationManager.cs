using Com.Ctrip.Framework.Apollo;
using Microsoft.Extensions.Configuration;
using System.IO;

namespace Talk.Talk.Apollo
{
    /// <summary>
    /// 阿波罗配置中心客户端
    /// </summary>
    public static class ApolloConfigurationManager
    {
        private readonly static IConfiguration Configuration;

        static ApolloConfigurationManager()
        {
            var TempConfiguration = new ConfigurationBuilder()
                  .SetBasePath(Directory.GetCurrentDirectory())
                  .AddJsonFile("appsettings.json", optional: true)
                  .Build();
            Configuration = new ConfigurationBuilder()
                .AddApollo(TempConfiguration.GetSection("apollo"))
                .AddDefault()
                .Build();
        }

        /// <summary>
        /// 获取Apollo配置
        /// </summary>
        /// <param name="key"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public static string GetApolloConfig(this string key, string defaultValue = "")
        {
            var value = Configuration.GetValue(key, defaultValue);
            if (string.IsNullOrWhiteSpace(value))
            {
                if (!string.IsNullOrWhiteSpace(defaultValue))
                    return defaultValue;
                throw new System.Exception($"获取配置{key}异常");
            }
            return value;
        }
    }
}
