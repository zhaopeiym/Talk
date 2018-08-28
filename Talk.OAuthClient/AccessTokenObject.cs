using System;
using System.Collections.Generic;
using System.Text;

namespace Talk.OAuthClient
{
    public class AccessTokenObject
    {
        /// <summary>
        /// 可将此ID进行存储便于用户下次登录时辨识其身份
        /// </summary>
        public string UserId { get; set; }
        /// <summary>
        /// 票据
        /// </summary>
        public string AccessToken { get; set; }
    }
}
