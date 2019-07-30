using System;
using System.Collections.Generic;

namespace Talk.Interface
{
    /// <summary>
    /// 远程调用上下文接口
    /// </summary>
    public interface IRPCContext : IScopedDependency
    {
        /// <summary>
        /// 调用者邮箱
        /// </summary>
        string Email { get; set; }
        /// <summary>
        /// 租户id
        /// </summary>
        long TenantId { get; set; }
        /// <summary>
        /// 租户名称
        /// </summary>
        string TenantName { get; set; }
        /// <summary>
        /// 用户id
        /// </summary>
        long UserId { get; set; }
        /// <summary>
        /// 用户名称
        /// </summary>
        string UserName { get; set; }
        /// <summary>
        /// 请求源跟踪ID
        /// </summary>
        Guid TrackId { get; set; }
        /// <summary>
        /// 请求源URL
        /// </summary>
        string RequestUrl { get; set; }
        /// <summary>
        /// 请求了哪些客户端
        /// </summary>
        List<string> ClientUrls { get; set; }
        /// <summary>
        /// 请求源IP
        /// </summary>
        string RequestIP { get; set; }
        /// <summary>
        /// 请求code，请求源应用编号，一般使用应用端口号。
        /// </summary>
        string RequestCode { get; set; }
    }
}
