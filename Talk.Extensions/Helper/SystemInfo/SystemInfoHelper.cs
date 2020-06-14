using Newtonsoft.Json;
using System;
using System.Linq;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Text;

namespace Talk.Extensions.Helper.SystemInfo
{
    /// <summary>
    /// 系统信息
    /// </summary>
    public class SystemInfoHelper
    {
        /// <summary>
        /// 获取系统信息
        /// </summary>
        /// <param name="fuzzyIp">ip模糊匹配，定位到想要的网卡</param>
        /// <returns></returns>
        public static OSInfo GetOSInfo(string fuzzyIp = "192.168.")
        {
            OSInfo osInfo = new OSInfo()
            {
                OSArchitecture = RuntimeInformation.OSArchitecture,
                OSDescription = RuntimeInformation.OSDescription,
                ProcessArchitecture = RuntimeInformation.ProcessArchitecture,
                Is64BitOperatingSystem = Environment.Is64BitOperatingSystem,
                ProcessorCount = Environment.ProcessorCount,
                MachineName = Environment.MachineName,
                OSVersion = Environment.OSVersion
            };
            var firstUpInterface = NetworkInterface.GetAllNetworkInterfaces()
                   .Where(t => t.GetIPProperties().UnicastAddresses.Any(u => u.Address.ToString().Contains(fuzzyIp)))
                   .OrderByDescending(c => c.Speed)
                   .FirstOrDefault(c => c.NetworkInterfaceType != NetworkInterfaceType.Loopback && c.OperationalStatus == OperationalStatus.Up);
            if (firstUpInterface != null)
            {
                var props = firstUpInterface.GetIPProperties();
                // 获取分配给该接口的第一个IPV4地址
                osInfo.FirstIpV4Address = props.UnicastAddresses
                    .Where(c => c.Address.AddressFamily == AddressFamily.InterNetwork)
                    .Select(c => c.Address)
                    .FirstOrDefault().ToString();
                //获取mac地址
                osInfo.MacAddress = string.Join("-", firstUpInterface.GetPhysicalAddress().GetAddressBytes());
            }
            using (var md5 = MD5.Create())
            {
                osInfo.SerialNumber = BitConverter.ToString(md5.ComputeHash(Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(osInfo))))
                    .Replace("-", "")
                    .Substring(8, 16);
            }
            return osInfo;
        }
    }
}
