using Com.Ctrip.Framework.Apollo;
using Com.Ctrip.Framework.Apollo.Core;
using Com.Ctrip.Framework.Apollo.Enums;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Linq;
using Talk.Extensions;
using Talk.Talk.Apollo;
using Xunit;

namespace Talk.Tests
{
    public class Temp_Tests
    {
        [Fact]
        public void test()
        {
            var options = new ConfigurationBuilder()
                //.AddInMemoryCollection(new Dictionary<string, string>
                //{
                //    {"Apollo:AppId", "apollo-client" },
                //    {"Apollo:Env", "Pro" },
                //})
                .Build()
                .GetSection("Apollo")
                .Get<ApolloOptions>();
            //string someKey = "someKeyFromDefaultNamespace";
            //string someDefaultValue = "someDefaultValueForTheKey";
            //string value = config.GetProperty(someKey, someDefaultValue);
            Dictionary<string, string> Meta = new Dictionary<string, string>();
            Meta.Add("DEV", "url");

            var builder = new ConfigurationBuilder().AddApollo(new ApolloOptions
            {
                AppId = "apollo-client",
                MetaServer = "url",
                Env = Env.Dev,
                Meta = Meta,
            });
            builder.AddDefault();
            builder.AddNamespace("TEST1.Benny");
            builder.AddNamespace(ConfigConsts.NamespaceApplication);
            var bu = builder.Build();

            for (int i = 0; i < 500; i++)
            {
                var aa = bu.GetValue<string>("test");
            }

            var sources = builder.Sources.OfType<ApolloConfigurationProvider>().ToArray();

            var obj = builder.ConfigRepositoryFactory.GetConfigRepository(ConfigConsts.NamespaceApplication);
            var config = obj.GetConfig();
            var tem = config.GetProperty("test");
        }

        [Fact]
        public void test2()
        {
            var test1 = ConfigurationManager.GetConfig("test2");
            var test2 = Talk.Apollo.ApolloConfigurationManager.GetApolloConfig("test", "aa");
            var aaa = ConfigurationManager.GetSection<aa>("apollo:Meta");
        }

        public class aa
        {
            public string DEV { get; set; }
        }
    }
}
