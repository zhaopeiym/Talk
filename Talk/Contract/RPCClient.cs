using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Talk.Attributes;
using Talk.Extensions;
using Talk.Extensions.Helper;
using Talk.Interface;

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
        public static async Task<ResultBase<TResponse>> PostAsync<TResponse>(this IRPCContext context, IReturn<TResponse> @return, Dictionary<string, string> headers = null)
        {
            var url = string.Empty;
            if (headers == null) headers = new Dictionary<string, string>();
            try
            {
                headers.Add("RPCContext", HttpUtility.UrlEncode(JsonConvert.SerializeObject(context)));
                url = @return.GetUrl();
                var httpResponseMessage = await HttpHelper.Instance.PostAsync(url, JsonConvert.SerializeObject(@return), headers);
                if (httpResponseMessage == null || httpResponseMessage.Content == null)
                {
                    return new ResultBase<TResponse>()
                    {
                        Code = HttpCodeEnum.C500,
                        IsUserErr = false,
                        ErrorMsg = $"请求结果HttpResponseMessage为null。请确认是否存在地址{url},或检查参数是否有误。",
                        RequestUrl = url,
                    };
                }
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
        public static async Task<ResultBase<object>> PostAsync(this IRPCContext context, IReturn @return, Dictionary<string, string> headers = null)
        {
            var url = string.Empty;
            if (headers == null) headers = new Dictionary<string, string>();
            try
            {
                headers.Add("RPCContext", JsonConvert.SerializeObject(context));
                url = @return.GetUrl();
                var httpResponseMessage = await HttpHelper.Instance.PostAsync(url, JsonConvert.SerializeObject(@return), headers);
                if (httpResponseMessage == null || httpResponseMessage.Content == null)
                {
                    return new ResultBase<object>()
                    {
                        Code = HttpCodeEnum.C500,
                        IsUserErr = false,
                        ErrorMsg = $"请求结果HttpResponseMessage为null。请确认是否存在地址{url},或检查参数是否有误。",
                        RequestUrl = url,
                    };
                }
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
        public static async Task<ResultBase<object>> PostAsync(this IRPCContext context, string url, string jsonString, Dictionary<string, string> headers = null)
        {
            if (headers == null) headers = new Dictionary<string, string>();
            try
            {
                headers.Add("RPCContext", JsonConvert.SerializeObject(context));
                var httpResponseMessage = await HttpHelper.Instance.PostAsync(url, jsonString, headers);
                if (httpResponseMessage == null || httpResponseMessage.Content == null)
                {
                    return new ResultBase<object>()
                    {
                        Code = HttpCodeEnum.C500,
                        IsUserErr = false,
                        ErrorMsg = $"请求结果HttpResponseMessage为null。请确认是否存在地址{url},或检查参数是否有误。",
                        RequestUrl = url,
                    };
                }
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
        public static string GetUrl(this IReturn request)
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
