using System.Reflection;
using Talk.Redis.Tests;

namespace Talk.Tests
{
    [DependsOn(typeof(RedisMoule))]
    public class TalkTestMoule : AppModule
    {
        public override void Initialize()
        {
            ModuleAssembly = Assembly.GetExecutingAssembly();
        }
    }
}
