using Microsoft.AspNetCore.Http;
using System;
using System.Linq;
using System.Net;
using Talk.Cache;

namespace Talk.AntiMalice
{
    /// <summary>
    ///反捣乱
    /// </summary>
    public static class AntiMalice
    {
        private readonly static string defaultValue = "defaltValue";
        private readonly static string key = "talkantimaliceXX";

        /// <summary>
        /// 设置Token
        /// </summary>
        /// <param name="httpContext"></param>
        public static void SetToken(this HttpContext httpContext)
        {
            var antimaliceValue = httpContext.Request.Cookies.FirstOrDefault(t => t.Key == key).Value;
            if (string.IsNullOrWhiteSpace(antimaliceValue))//如果没有cookie
            {
                var value = "Talke|" + DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss") + "|" + Guid.NewGuid();
                value = value.DES3Encrypt(key);
                httpContext.Response.Cookies.Append(key, value);
            }
        }

        public class AntiMaliceToken
        {
            public long Num { get; set; }
            public int Type { get; set; }
            public bool IsOk { get; set; }
        }

        /// <summary>
        /// 验证Token
        /// </summary>
        /// <param name="httpContext"></param>
        /// <param name="interval">时间间隔</param>
        /// <param name="nsecond">未标记请求请求可以访问次数</param>
        /// <param name="usecond">标记请求可以访问次数（两分钟后清除标记）</param>
        /// <returns></returns>
        public static AntiMaliceToken ValidateToken(this HttpContext httpContext, int interval = 30, int nsecond = 15, int usecond = 5)
        {
            var antimaliceValue = httpContext.Request.Cookies.FirstOrDefault(t => t.Key == key).Value;
            antimaliceValue = string.IsNullOrWhiteSpace(antimaliceValue) ? defaultValue : antimaliceValue;
            var time = DateTime.Now.AddSeconds(interval) - DateTime.Now;
            ExecuteNum executeNum = new ExecuteNum(antimaliceValue, time);
            var num = executeNum.GetNum();
            var antiMaliceToken = new AntiMaliceToken()
            {
                Num = num,
                IsOk = true
            };
            if (antimaliceValue == defaultValue)//证明是没有标记的请求
            {
                antiMaliceToken.Type = 0;
                httpContext.SetToken();
                if (num > nsecond)//一定时间内不能超过多少次                
                    antiMaliceToken.IsOk = false;
            }
            else
            {
                antiMaliceToken.Type = 1;
                if (num > usecond)//有标记也不能频繁操作                
                    antiMaliceToken.IsOk = false;
                else
                {
                    var valueString = antimaliceValue.DES3Decrypt(key);
                    var values = valueString.Split('|');
                    if (values.Length <= 1 || values[0] != "Talke")
                    {
                        antiMaliceToken.IsOk = false;
                        //httpContext.Response.Cookies.Delete(key);// 兼容 value 的修改
                    }
                    else if (DateTime.Parse(values[1]).AddMinutes(2) <= DateTime.Now)//清除2分钟前的标记                
                        httpContext.Response.Cookies.Delete(key);
                }
            }
            return antiMaliceToken;
        }
    }
}
