using System;
using System.Collections.Generic;
using System.Text;

namespace Talk.Application.Models
{
    /// <summary>
    /// 远程认证
    /// </summary>
    public class RemoteAttestationModel
    {
        /// <summary>
        /// 边缘设备表示
        /// </summary>
        public string Code { get; set; }
        /// <summary>
        /// 租户Id
        /// </summary>
        public long TenantId { get; set; }
        /// <summary>
        /// 项目Id
        /// </summary>
        public long ProjectId { get; set; }
        /// <summary>
        /// 认证加密键
        /// </summary>
        public string Key { get; set; }
    }
}
