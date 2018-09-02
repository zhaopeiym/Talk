using System;
using System.Collections.Generic;
using System.Text;

namespace Talk
{
    /// <summary>
    ///  瞬时（每次都重新实例）
    /// </summary>
    public interface ITransientDependency { }
    /// <summary>
    /// 一个请求内唯一（本质上是生命周期范围内的单例）
    /// </summary>
    public interface IScopedDependency { }
    /// <summary>
    /// 单例（全局唯一）
    /// </summary>
    public interface ISingletonDependency { }

    /// <summary>
    /// 自动释放
    /// </summary>
    public interface IRelease : IDisposable { }
    /// <summary>
    /// 拦截器标识
    /// </summary>
    public interface IAutoInterceptor { }
}
