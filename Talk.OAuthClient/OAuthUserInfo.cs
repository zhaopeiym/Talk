using System;
using System.Collections.Generic;
using System.Text;

namespace Talk.OAuthClient
{
    public class OAuthUserInfo
    {
        /// <summary>
        /// 可将此ID进行存储便于用户下次登录时辨识其身份
        /// </summary>
        public string Id { get; set; }//唯一标识
        public string Name { get; set; }//昵称
        /// <summary>
        /// 性别 
        /// true 男
        /// </summary>
        public bool? Gender { get; set; }
        public string ImgUrl { get; set; }//头像
    }
}
