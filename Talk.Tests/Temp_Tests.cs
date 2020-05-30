using Com.Ctrip.Framework.Apollo;
using Com.Ctrip.Framework.Apollo.Core;
using Com.Ctrip.Framework.Apollo.Enums;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Linq;
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
            var str = "[{\"id\":2,\"name\":\"天下无双\",\"damage\":123},{\"id\":3,\"name\":\"天下无贼\",\"damage\":21},{\"id\":4,\"name\":\"咫尺天涯\",\"damage\":900}]";
            var obj = JsonConvert.DeserializeObject(str);

            var p = (obj as JToken).FirstOrDefault()?.Select(t => (t as JProperty).Name).ToList();

            foreach (var item1 in ((JToken)obj))
            {
                foreach (JProperty item in item1)
                {   //((Newtonsoft.Json.Linq.JProperty)item).Name
                    //((Newtonsoft.Json.Linq.JValue)item1["id"]).Value

                    var value = (item1[item.Name] as JValue).Value;
                }
            }
            //foreach (var item in obj)
            //{

            //}
            //var jsonObj = JsonMapper.ToObject(str);
            //foreach (JsonData item in jsonObj)
            //{
            //    foreach (var key in item.Keys)
            //    {
            //        var aa = item[key];
            //    }
            //}

        }

        public class aa
        {
            public string DEV { get; set; }
        }
    }
}
