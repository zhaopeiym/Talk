using System.Reflection;

namespace Talk.Redis.Tests
{
    public class RedisMoule: AppModule
    {
        public override void Initialize()
        {
             ModuleAssembly = Assembly.GetExecutingAssembly();
        }
    }
}
