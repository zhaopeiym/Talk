using System;
using System.Runtime.InteropServices;

namespace Talk.Extensions.Helper.SystemInfo
{
    public class OSInfo
    {
        /// <summary>
        /// 系统架构
        /// </summary>
        public Architecture OSArchitecture { get; set; }
        /// <summary>
        /// 系统名称
        /// </summary>
        public string OSDescription { get; set; }
        /// <summary>
        /// 进程架构
        /// </summary>
        public Architecture ProcessArchitecture { get; set; }
        /// <summary>
        /// 是否64位操作系统
        /// </summary>
        public bool Is64BitOperatingSystem { get; set; }
        /// <summary>
        /// CPU CORE
        /// </summary>
        public int ProcessorCount { get; set; }
        /// <summary>
        /// HostName
        /// </summary>
        public string MachineName { get; set; }
        /// <summary>
        /// Version
        /// </summary>
        public OperatingSystem OSVersion { get; set; }
        /// <summary>
        /// IPV4地址
        /// </summary>
        public string FirstIpV4Address { get; set; }
        /// <summary>
        /// MAC地址
        /// </summary>
        public string MacAddress { get; set; }
        /// <summary>
        /// 自定义序列号
        /// </summary>
        public string SerialNumber { get; set; }
    }
}
