using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;

namespace Talk.OAuthClient
{
    //http://wiki.connect.qq.com/%E4%BD%BF%E7%94%A8authorization_code%E8%8E%B7%E5%8F%96access_token

    public class OAuthClientQQ : OAuthClient
    {

        public OAuthClientQQ(string clientId, string clientSecret, string callbackUrl)
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
            authorizeUrl.Append("https://graph.qq.com/oauth2.0/authorize");
            authorizeUrl.Append("?response_type=code");//授权类型，此值固定为“code”。
            authorizeUrl.Append("&client_id=" + ClientId);//申请QQ登录成功后，分配给应用的appid。
            authorizeUrl.Append("&redirect_uri=" + CallbackUrl);//成功授权后的回调地址，必须是注册appid时填写的主域名下的地址，建议设置为网站首页或网站的用户中心。注意需要将url进行URLEncode。
            authorizeUrl.Append("&state=qq");//client端的状态值。用于第三方应用防止CSRF攻击，成功授权后回调时会原样带回。请务必严格按照流程检查用户与state参数状态的绑定。
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
                tokenUrl.Append("https://graph.qq.com/oauth2.0/token");
                tokenUrl.Append("?grant_type=authorization_code");//授权类型，在本步骤中，此值为“authorization_code”。
                tokenUrl.Append("&client_id=" + ClientId);//申请QQ登录成功后，分配给网站的appid。
                tokenUrl.Append("&client_secret=" + ClientSecret);//申请QQ登录成功后，分配给网站的appkey。
                #region 注释
                /*  
                *  上一步返回的authorization code。
                    如果用户成功登录并授权，则会跳转到指定的回调地址，并在URL中带上Authorization Code。
                    例如，回调地址为www.qq.com/my.php，则跳转到：
                    http://www.qq.com/my.php?code=520DD95263C1CFEA087******
                    注意此code会在10分钟内过期。
                */
                #endregion
                tokenUrl.Append("&code=" + code);
                tokenUrl.Append("&redirect_uri=" + CallbackUrl);//与上面一步中传入的redirect_uri保持一致。              
                var responseToken = await httpClient.GetStringAsync(tokenUrl.ToString());

                //3、获取用户OpenID
                var access_token = responseToken.Split('&')[0].Split('=')[1];//票据
                var access_token_url = "https://graph.qq.com/oauth2.0/me?access_token=" + access_token;
                var responseAccess_token = await httpClient.GetStringAsync(access_token_url);
                Regex reg = new Regex("^callback[(](?<data>.*?)[);]+$", RegexOptions.IgnoreCase);
                Match match = reg.Match(responseAccess_token.Trim());
                string jsonText = match.Groups["data"].Value;
                var dynamicObject = JsonConvert.DeserializeObject<dynamic>(jsonText);
                //openid是此网站上唯一对应用户身份的标识，
                //网站可将此ID进行存储便于用户下次登录时辨识其身份，或将其与用户在网站上的原有账号进行绑定。
                var openid = dynamicObject.openid.ToString();

                var accessTokenObject = new AccessTokenObject()
                {
                    AccessToken = access_token,
                    UserId = openid
                };
                return accessTokenObject;
            }
        }

        /// <summary>
        /// 获取用户信息
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public override async Task<OAuthUserInfo> GetUserInfo(AccessTokenObject accessTokenObject)
        {
            using (HttpClient httpClient = new HttpClient())
            {               
                //4、OpenAPI调用，获取用户信息
                StringBuilder getUserInfoUrl = new StringBuilder();
                getUserInfoUrl.Append("https://graph.qq.com/user/get_user_info");
                getUserInfoUrl.Append("?oauth_consumer_key=" + ClientId);
                getUserInfoUrl.Append("&access_token=" + accessTokenObject.AccessToken);
                getUserInfoUrl.Append("&openid=" + accessTokenObject.UserId);
                getUserInfoUrl.Append("&format=json");

                var str = await httpClient.GetStringAsync(getUserInfoUrl.ToString());
                var dynamicObject = JsonConvert.DeserializeObject<dynamic>(str);
                var user = new OAuthUserInfo()
                {
                    Id = accessTokenObject.UserId,
                    Name = dynamicObject.nickname.ToString(),
                    ImgUrl = dynamicObject.figureurl_qq_2.ToString(),
                    Gender = dynamicObject.gender.ToString() == "男" ? true : false
                };
                return user;
            }
        }
    }
}
