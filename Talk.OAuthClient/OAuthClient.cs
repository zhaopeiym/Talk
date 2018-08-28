using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Talk.OAuthClient
{
    public abstract class OAuthClient : IOAuthClient
    {
        public string ClientId { get; set; }
        public string ClientSecret { get; set; }
        public string CallbackUrl { get; set; }

        public OAuthClient(string clientId, string clientSecret, string callbackUrl)
        {
            ClientId = clientId;
            ClientSecret = clientSecret;
            CallbackUrl = HttpUtility.UrlEncode(callbackUrl);
        }

        public virtual string GetAuthUrl()
        {
            throw new NotImplementedException();
        }

        public virtual Task<OAuthUserInfo> GetUserInfo(AccessTokenObject code)
        {
            throw new NotImplementedException();
        }

        public virtual Task<AccessTokenObject> GetAccessToken(string code)
        {
            throw new NotImplementedException();
        }
    }
}
