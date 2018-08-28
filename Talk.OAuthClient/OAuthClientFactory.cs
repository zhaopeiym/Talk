using System;
using System.Collections.Generic;
using System.Text;

namespace Talk.OAuthClient
{
    public class OAuthClientFactory
    {
        public static IOAuthClient GetOAuthClient(string clientId, string clientSecret, string callbackUrl, AuthType oAuthClientType)
        {
            IOAuthClient authToken = null;
            switch (oAuthClientType)
            {
                case AuthType.QQ:
                    authToken = new OAuthClientQQ(clientId, clientSecret, callbackUrl);
                    break;
                case AuthType.Sina:
                    authToken = new OAuthClientSina(clientId, clientSecret, callbackUrl);
                    break;
            }
            return authToken;
        }
    }
}
