using System;
using System.Linq;
using System.Reflection;
using Xunit;

namespace Talk.Tests
{
    public class Module_tests
    {
        [Fact]
        public void Test1()
        {
            var obj = ModuleManager.Create<TalkTestMoule>();
            obj.Initialize();            
            //   var obj = typeof(TalkTestMoule).GetCustomAttributes<DependsOnAttribute>();
            //var attributes = obj.SelectMany(t => t.DependedModuleTypes).ToList();
            //foreach (var attr in attributes)
            //{
            //    var module = (AppModule)Activator.CreateInstance(attr);
            //    module.Initialize();
            //    //module.assembly
            //}
        }
    }
}
