﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using Talk.Extensions;
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
        /// <summary>
        /// 远程认证
        /// </summary>
        public RemoteAuthentication Authentication { get; set; }
        /// <summary>
        /// rpc baseurl ,如果没有，则会读取配置文件
        /// </summary>
        public string RPCBaseUrl { get; set; }
    }

    /// <summary>
    /// 远程认证
    /// </summary>
    public class RemoteAuthentication
    {
        /// <summary>
        /// 加密后的认证内容（实际上就是 RemoteAttestationModel 序列号后再加密）
        /// 包括：Key（认证加密键）、TenantId [这里的TenantId比RPCContext.TenantId可信度更高]、ProjectId（项目Id）、Code（边缘设备表示）        
        /// 使用被调用方的key加密
        /// </summary>
        public string RemoteAuthorize { get; set; }
        /// <summary>
        /// 加密后的时间（使用上面解密后的Key（认证加密键）来进行加密）
        /// </summary>
        public string RemoteToken { get; set; }

        /// <summary>
        /// 加密键（必须16位）
        /// JsonIgnore 序列化的时候忽略，也就是不会远程传输
        /// </summary>
        [JsonIgnore]
        public string EncryptKey { get; set; }
    }
}
