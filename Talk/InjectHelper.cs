using Autofac;
using Autofac.Extras.DynamicProxy;
using System;
using System.Linq;
using System.Reflection;

namespace Talk
{
    public static class InjectHelper
    {
        /// <summary>
        /// 自动注入
        /// </summary>
        /// <param name="services"></param>
        /// <param name="assembly"></param>
        /// <param name="interceptedType">拦截器</param>
        public static void AutoInjection(this ContainerBuilder services, Assembly assembly, Type interceptedType = null)
        {
            var types = assembly.GetTypes();

            //标记了 拦截器 的类型
            var autoInterceptorClass = types
                .Where(t => t.GetInterfaces().Contains(typeof(IAutoInterceptor)) && t.IsClass)
                .ToList();

            //标记了 IRelease（自动释放） 的类型
            var releaseClass = types
                .Where(t => t.GetInterfaces().Contains(typeof(IRelease)) && t.IsClass)
                .ToList();

            #region ISingletonDependency （单例）
            //获取标记了ISingletonDependency接口的接口
            var singletonInterfaceDependency = types
                    .Where(t => t.GetInterfaces().Contains(typeof(ISingletonDependency)))
                    .SelectMany(t => t.GetInterfaces().Where(f => f.GetInterfaces().Contains(typeof(ISingletonDependency)) && f.Name != "ISingletonDependency"))
                    .ToList();
            //自动注入标记了 ISingletonDependency接口的 接口
            foreach (var interfaceName in singletonInterfaceDependency)
            {
                var type = types.Where(t => t.GetInterfaces().Contains(interfaceName)).FirstOrDefault();
                //规则：
                //1、接口必须以大写"I"开头
                //2、type必须以接口去掉"I"为命名前缀
                if (type != null && type.Name.StartsWith(interfaceName.Name.Substring(1)))
                {
                    var builder = services.RegisterType(type).As(interfaceName).PropertiesAutowired().SingleInstance();
                    if (autoInterceptorClass.Any(t => t == type) && interceptedType != null)
                        builder = builder.InterceptedBy(interceptedType).EnableInterfaceInterceptors();
                    if (releaseClass.Any(t => t == type))
                    {
                        builder.OnRelease(e =>
                        {
                            (e as IRelease).Dispose();
                        });
                    }
                }
            }


            //获取标记了ISingletonDependency接口的类
            var singletonTypeDependency = types
                    .Where(t => t.GetInterfaces().Contains(typeof(ISingletonDependency)) && t.IsClass)
                    .ToList();
            //自动注入标记了 ISingletonDependency接口的 类
            foreach (var type in singletonTypeDependency)
            {
                var builder = services.RegisterType(type).As(type).PropertiesAutowired().SingleInstance();
                if (autoInterceptorClass.Any(t => t == type) && interceptedType != null)
                    builder = builder.InterceptedBy(interceptedType).EnableClassInterceptors();
                if (releaseClass.Any(t => t == type))
                {
                    builder.OnRelease(e =>
                    {
                        (e as IRelease).Dispose();
                    });
                }
            }
            #endregion

            #region ITransientDependency（每次实例）
            //获取标记了ITransientDependency接口的接口
            var transientInterfaceDependency = types
                   .Where(t => t.GetInterfaces().Contains(typeof(ITransientDependency)))
                   .SelectMany(t => t.GetInterfaces().Where(f => f.GetInterfaces().Contains(typeof(ITransientDependency)) && !f.Name.Equals("ITransientDependency")))
                   .ToList();
            //自动注入标记了 ITransientDependency接口的 接口
            foreach (var interfaceName in transientInterfaceDependency)
            {
                var type = types.Where(t => t.GetInterfaces().Contains(interfaceName)).FirstOrDefault();
                if (type != null && type.Name.StartsWith(interfaceName.Name.Substring(1)))
                {
                    var builder = services.RegisterType(type).As(interfaceName).PropertiesAutowired().InstancePerDependency();
                    if (autoInterceptorClass.Any(t => t == type) && interceptedType != null)
                        builder = builder.InterceptedBy(interceptedType).EnableInterfaceInterceptors();
                    if (releaseClass.Any(t => t == type))
                    {
                        builder.OnRelease(e =>
                        {
                            (e as IRelease).Dispose();
                        });
                    }
                }
            }
            //获取标记了ITransientDependency接口的类
            var transientTypeDependency = types
                    .Where(t => t.GetInterfaces().Contains(typeof(ITransientDependency)) && t.IsClass)
                    .ToList();
            //自动注入标记了 ITransientDependency接口的 类
            foreach (var type in transientTypeDependency)
            {
                var builder = services.RegisterType(type).As(type).PropertiesAutowired().InstancePerDependency();
                if (autoInterceptorClass.Any(t => t == type) && interceptedType != null)
                    builder = builder.InterceptedBy(interceptedType).EnableClassInterceptors();
                if (releaseClass.Any(t => t == type))
                {
                    builder.OnRelease(e =>
                    {
                        (e as IRelease).Dispose();
                    });
                }
            }
            #endregion

            #region IScopedDependency（线程内唯一）
            //获取标记了IScopedDependency接口的接口
            var scopedInterfaceDependency = types
                   .Where(t => t.GetInterfaces().Contains(typeof(IScopedDependency)))
                   .SelectMany(t => t.GetInterfaces().Where(f => f.GetInterfaces().Contains(typeof(IScopedDependency)) && !f.Name.Equals("IScopedDependency")))
                   .ToList();
            //自动注入标记了 IScopedDependency接口的 接口
            foreach (var interfaceName in scopedInterfaceDependency)
            {
                var type = types.Where(t => t.GetInterfaces().Contains(interfaceName)).FirstOrDefault();
                if (type != null && type.Name.StartsWith(interfaceName.Name.Substring(1)))//如：IDBContext 和 DBContext 这种命名才注入
                {
                    var builder = services.RegisterType(type).As(interfaceName).PropertiesAutowired().InstancePerLifetimeScope();
                    if (releaseClass.Any(t => t == type))
                    {
                        builder = builder.OnRelease(e =>
                        {
                            (e as IRelease).Dispose();
                        });
                    }
                    if (autoInterceptorClass.Any(t => t == type) && interceptedType != null)
                        builder = builder.InterceptedBy(interceptedType).EnableInterfaceInterceptors();

                }
            }


            //获取标记了IScopedDependency接口的类
            var scopedTypeDependency = types
                    .Where(t => t.GetInterfaces().Contains(typeof(IScopedDependency)) && t.IsClass)
                    .ToList();
            //自动注入标记了 IScopedDependency接口的 类
            foreach (var type in scopedTypeDependency)
            {
                var builder = services.RegisterType(type).As(type).PropertiesAutowired().InstancePerLifetimeScope();
                if (autoInterceptorClass.Any(t => t == type) && interceptedType != null)
                    builder = builder.InterceptedBy(interceptedType).EnableClassInterceptors();
                if (releaseClass.Any(t => t == type))
                {
                    builder.OnRelease(e =>
                    {
                        (e as IRelease).Dispose();
                    });
                }
            }
            #endregion
        }
    }
}
