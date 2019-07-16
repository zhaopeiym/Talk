using System;
using System.Collections.Generic;

namespace Talk.Contract
{
    public class ResultBase : ResultBase<string>
    {

    }

    public class ResultBase<T>
    {
        /// <summary>
        /// 请求是否成功
        /// </summary>
        public bool IsSuccess
        {
            get
            {
                return Code == HttpCodeEnum.C200;
            }
        }
        /// <summary>
        /// 请求结果code
        /// </summary>
        public HttpCodeEnum Code { get; set; } = HttpCodeEnum.C200;
        /// <summary>
        /// 是否为用户友好异常
        /// </summary>
        public bool? IsUserErr { get; set; } = true;
        /// <summary>
        /// 异常消息
        /// </summary>
        public string ErrorMsg { get; set; }
        /// <summary>
        /// 重定向地址
        /// </summary>
        public string ResultUrl { get; set; }

        public List<string> ErrorList { get; set; }

        /// <summary>
        /// 请求结果
        /// </summary>
        public T Data { get; set; }
        /// <summary>
        /// 请求跟踪id
        /// </summary>
        public Guid TrackId { get; set; }
    }
}
