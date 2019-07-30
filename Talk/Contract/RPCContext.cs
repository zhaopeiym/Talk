using System;
using Talk.Interface;

namespace Talk.Contract
{
    /// <summary>
    /// 远程调用上下文
    /// </summary>
    public class RPCContext : IRPCContext
    {
        /// <summary>
        /// 调用者邮箱
        /// </summary>
        public string Email { get; set; }
        /// <summary>
        /// 租户id
        /// </summary>
        public long TenantId { get; set; }
        /// <summary>
        /// 租户名称
        /// </summary>
        public string TenantName { get; set; }
        /// <summary>
        /// 用户id
        /// </summary>
        public long UserId { get; set; }
        /// <summary>
        /// 用户名称
        /// </summary>
        public string UserName { get; set; }
        /// <summary>
        /// 请求源跟踪ID
        /// </summary>
        public Guid TrackId { get; set; }
        /// <summary>
        /// 请求源URL
        /// </summary>
        public string RequestUrl { get; set; }
        /// <summary>
        /// 请求源IP
        /// </summary>
        public string RequestIP { get; set; }
        /// <summary>
        /// 请求code，请求源应用编号，一般使用应用端口号。
        /// </summary>
        public string RequestCode { get; set; }
        /// <summary>
        /// 方法描述
        /// </summary>
        public string MethodDescription { get; set; }
        /// <summary>
        /// Action
        /// </summary>
        public string ActionName { get; set; }
        /// <summary>
        /// 控制器
        /// </summary>
        public string ControllerName { get; set; }
    }
}
