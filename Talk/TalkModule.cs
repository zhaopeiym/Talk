using System.Reflection;

namespace Talk
{
    public class TalkModule : AppModule
    {
        public override void Initialize()
        {
            ModuleAssembly = Assembly.GetExecutingAssembly();
        }
    }
}
