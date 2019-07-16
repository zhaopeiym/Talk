using System.ComponentModel;

namespace Talk
{
    public enum HttpCodeEnum
    {
        /// <summary>
        /// 成功
        /// </summary>
        [Description("成功")]
        C200 = 200,
        /// <summary>
        /// 错误请求
        /// </summary>
        [Description("错误请求")]
        C400 = 400,
        /// <summary>
        /// 未找到
        /// </summary>
        [Description("未找到")]
        C404 = 404,
        /// <summary>
        /// 未登录
        /// </summary>
        [Description("未登录")]
        C401 = 401,
        /// <summary>
        /// 没权限
        /// </summary>
        [Description("没权限")]
        C407 = 407,
        /// <summary>
        /// 参数验证失败
        /// </summary>
        [Description("参数验证失败")]
        C412 = 412,
        /// <summary>
        /// 内部错误
        /// </summary>
        [Description("内部错误")]
        C500 = 500,
        /// <summary>
        /// 服务不可用
        /// </summary>
        [Description("服务不可用")]
        C503 = 503,
        /// <summary>
        /// 请求超时
        /// </summary>
        [Description("请求超时")]
        C504 = 504,
    }
}
