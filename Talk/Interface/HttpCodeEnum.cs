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
        /// 内部错误
        /// </summary>
        [Description("内部错误")]
        C500 = 500,
        /// <summary>
        /// 服务不可用
        /// </summary>
        [Description("服务不可用")]
        C503 = 503,
    }
}
