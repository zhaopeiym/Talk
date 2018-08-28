using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;

namespace Talk.OAuthClient
{
    //http://open.weibo.com/wiki/API%E6%96%87%E6%A1%A3_V2

    public class OAuthClientSina : OAuthClient
    {
        public OAuthClientSina(string clientId, string clientSecret, string callbackUrl)
            : base(clientId, clientSecret, callbackUrl)
        { }

        /// <summary>
        /// 获取验证地址
        /// </summary>
        /// <returns></returns>
        public override string GetAuthUrl()
        {
            //1、获取Authorization Code
            StringBuilder authorizeUrl = new StringBuilder();
            authorizeUrl.Append("https://api.weibo.com/oauth2/authorize");
            authorizeUrl.Append("?client_id=" + ClientId);//申请应用时分配的AppKey
            authorizeUrl.Append("&redirect_uri=" + CallbackUrl);//授权回调地址，站外应用需与设置的回调地址一致，站内应用需填写canvas page的地址
            authorizeUrl.Append("&state=sina");//用于保持请求和回调的状态
            authorizeUrl.Append("&response_type=code");//授权类型，此值固定为“code”。
            return authorizeUrl.ToString();
        }
        /// <summary>
        /// 获取票据信息
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public override async Task<AccessTokenObject> GetAccessToken(string code)
        {
            using (HttpClient httpClient = new HttpClient())
            {
                //2、通过Authorization Code获取Access Token
                StringBuilder tokenUrl = new StringBuilder();
                tokenUrl.Append("client_id=" + ClientId);//申请应用时分配的AppKey
                tokenUrl.Append("&client_secret=" + ClientSecret);//申请应用时分配的AppSecret
                tokenUrl.Append("&grant_type=authorization_code");//请求的类型，填写authorization_code
                tokenUrl.Append("&code=" + code);//调用authorize获得的code值
                tokenUrl.Append("&redirect_uri=" + CallbackUrl);//回调地址，需需与注册应用里的回调地址一致
                StringContent fromurlcontent = new StringContent(tokenUrl.ToString());
                fromurlcontent.Headers.ContentType = new MediaTypeHeaderValue("application/x-www-form-urlencoded");
                var token_url = "https://api.weibo.com/oauth2/access_token";
                var responseTokenMsg = await httpClient.PostAsync(new Uri(token_url), fromurlcontent);
                var responseToken = await responseTokenMsg.Content.ReadAsStringAsync();
                var dynamicObject = JsonConvert.DeserializeObject<dynamic>(responseToken);
                var access_token = dynamicObject.access_token.ToString();//票据
                var uid = dynamicObject.uid.ToString();

                var accessTokenObject = new AccessTokenObject()
                {
                    UserId = uid,
                    AccessToken = access_token
                };
                return accessTokenObject;
            }
        }

        /// <summary>
        /// 获取用户信息
        /// </summary>
        /// <param name="accessTokenObject"></param>
        /// <returns></returns>
        public override async Task<OAuthUserInfo> GetUserInfo(AccessTokenObject accessTokenObject)
        {
            using (HttpClient httpClient = new HttpClient())
            {
                //3、获取用户信息                 
                var userInfo = await httpClient.GetStringAsync("https://api.weibo.com/2/users/show.json?access_token=" + accessTokenObject.AccessToken + "&uid=" + accessTokenObject.UserId);

                //授权用户的UID，本字段只是为了方便开发者，减少一次user/show接口调用而返回的
                //，第三方应用不能用此字段作为用户登录状态的识别，只有access_token才是用户授权的唯一票据。
                var dynamicObject = JsonConvert.DeserializeObject<dynamic>(userInfo);
                var user = new OAuthUserInfo()
                {
                    Id = accessTokenObject.UserId,
                    Name = dynamicObject.name.ToString(),
                    Gender = dynamicObject.gender.ToString() == "m" ? true : false,
                    ImgUrl = dynamicObject.avatar_hd.ToString()
                };
                return user;
            }
        }
    }
}
