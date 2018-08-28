using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Talk.OAuthClient
{
    public interface IOAuthClient
    {
        string ClientId { get; set; }
        string ClientSecret { get; set; }
        string CallbackUrl { get; set; }

        /// <summary>
        /// 获取验证地址
        /// </summary>
        /// <returns></returns>
        string GetAuthUrl();

        /// <summary>
        /// 获取票据信息
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        Task<AccessTokenObject> GetAccessToken(string code);

        /// <summary>
        /// 获取用户信息
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        Task<OAuthUserInfo> GetUserInfo(AccessTokenObject code);
    }
}
