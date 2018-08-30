using Autofac;
using System;
using System.Linq;
using System.Reflection;

namespace Talk
{
    public class ModuleManager
    {
        public ContainerBuilder ContainerBuilder { get; private set; }
        public IContainer Container { get; private set; }
        private static Type StartupType { get; set; }
        public ModuleManager()
        {
            ContainerBuilder = new ContainerBuilder();
        }

        public static ModuleManager Create<TModule>() where TModule : AppModule
        {
            StartupType = typeof(TModule);
            return new ModuleManager();
        }
        /// <summary>
        /// 初始化
        /// </summary>
        public void Initialize()
        {
            var obj = StartupType.GetCustomAttributes<DependsOnAttribute>();
            var attributes = obj.SelectMany(t => t.DependedModuleTypes).ToList();
            foreach (var attr in attributes)
            {
                var module = (AppModule)Activator.CreateInstance(attr);
                module.Initialize();
                ContainerBuilder.AutoInjection(module.ModuleAssembly);
            }
            Container = ContainerBuilder.Build();
        }
    }
}
