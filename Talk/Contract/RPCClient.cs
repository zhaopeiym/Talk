using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Talk.Attributes;
using Talk.Extensions;
using Talk.Extensions.Helper;

namespace Talk.Contract
{
    public static class RpcClient
    {
        /// <summary>
        /// post请求
        /// </summary>
        /// <typeparam name="TResponse">返回类型</typeparam>
        /// <param name="return">请求参数</param>
        /// <param name="headers">headers可做认证信息</param>
        /// <returns></returns>
        public static async Task<ResultBase<TResponse>> PostAsync<TResponse>(this IReturn<TResponse> @return, Dictionary<string, string> headers = null)
        {
            var url = string.Empty;
            try
            {
                url = GetUrl(@return);
                var httpResponseMessage = await HttpHelper.Instance.PostAsync(url, JsonConvert.SerializeObject(@return), headers);
                var result = await httpResponseMessage.Content.ReadAsStringAsync();
                var resultObj = JsonConvert.DeserializeObject<ResultBase<TResponse>>(result);
                resultObj.RequestUrl = url;
                return resultObj;
            }
            catch (System.Exception ex)
            {
                return new ResultBase<TResponse>()
                {
                    Code = HttpCodeEnum.C500,
                    IsUserErr = false,
                    ErrorMsg = $"{ex.Message} { ex.StackTrace}",
                    RequestUrl = url,
                };
            }
        }

        /// <summary>
        /// post请求
        /// </summary>
        /// <param name="return">返回类型</param>
        /// <param name="headers">headers可做认证信息</param>
        /// <returns></returns>
        public static async Task<ResultBase<object>> PostAsync(this IReturn @return, Dictionary<string, string> headers = null)
        {
            var url = string.Empty;
            try
            {
                url = GetUrl(@return);
                var httpResponseMessage = await HttpHelper.Instance.PostAsync(url, JsonConvert.SerializeObject(@return), headers);
                var result = await httpResponseMessage.Content.ReadAsStringAsync();
                var resultObj = JsonConvert.DeserializeObject<ResultBase<object>>(result);
                resultObj.RequestUrl = url;
                return resultObj;
            }
            catch (System.Exception ex)
            {
                return new ResultBase<object>()
                {
                    Code = HttpCodeEnum.C500,
                    IsUserErr = false,
                    ErrorMsg = $"{ex.Message} { ex.StackTrace}",
                    RequestUrl = url,
                };
            }
        }

        /// <summary>
        /// post请求
        /// </summary>
        /// <param name="return">返回类型</param>
        /// <param name="jsonString">请求参数Json字符串</param>
        /// <param name="headers">headers可做认证信息</param>
        /// <returns></returns>
        public static async Task<ResultBase<object>> PostAsync(this IReturn @return, string jsonString, Dictionary<string, string> headers = null)
        {
            var url = string.Empty;
            try
            {
                url = GetUrl(@return);
                var httpResponseMessage = await HttpHelper.Instance.PostAsync(url, jsonString, headers);
                var result = await httpResponseMessage.Content.ReadAsStringAsync();
                var resultObj = JsonConvert.DeserializeObject<ResultBase<object>>(result);
                resultObj.RequestUrl = url;
                return resultObj;
            }
            catch (System.Exception ex)
            {
                return new ResultBase<object>()
                {
                    Code = HttpCodeEnum.C500,
                    IsUserErr = false,
                    ErrorMsg = $"{ex.Message} { ex.StackTrace}",
                    RequestUrl = url,
                };
            }
        }

        /// <summary>
        /// 获取请求url
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        private static string GetUrl(object request)
        {
            var assemblyName = request.GetType().Assembly.GetName().Name;
            //这里约定配置BaseUrl
            var baseUrl = ConfigurationManager.GetConfig($"ApiHost.{assemblyName}");
            return baseUrl + request.GetType()
                .GetCustomAttributes(false)
                .OfType<RpcRouteAttribute>()
                .Select(attr => attr).FirstOrDefault()?.Path ?? string.Empty;
        }
    }
}
