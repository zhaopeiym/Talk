using System;
using System.Diagnostics;

namespace Talk.OAuthClient.Tests
{
    class Program
    {
        static void Main(string[] args)
        {
            var oAuthClient = OAuthClientFactory.GetOAuthClient("", "", "", AuthType.Sina);
            var url = oAuthClient.GetAuthUrl();//获取验证地址
            Process.Start(@"C:\Program Files\Internet Explorer\IEXPLORE.EXE", url);//打开IE
            Console.Write("code:");
            var code = Console.ReadLine();//输入回调地址Url中code的值
            var accessToken =  oAuthClient.GetAccessToken(code).Result;//获取票据
            var user = oAuthClient.GetUserInfo(accessToken).Result;//获取用户信息
            Console.WriteLine(user.Name);
            Console.ReadKey();
        }
    }
}
