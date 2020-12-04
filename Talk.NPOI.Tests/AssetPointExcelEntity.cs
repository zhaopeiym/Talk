using System;
using System.Collections.Generic;
using System.Text;

namespace Talk.NPOI.Tests
{
    public class AssetPointExcelEntity
    {
        [Alias("ID")]
        public string Id { get; set; }
        [Alias("*项目名称")]
        public string ProjectName { get; set; }

        /// <summary>
        /// 设备连接地址
        /// </summary>
        [Alias("设备连接地址")]
        public string IpAddress { get; set; }

        [Alias("系统参数")]
        public string SystemParamete { get; set; }
        /// <summary>
        /// 设备名称
        /// </summary>
        [Alias("设备名称")]
        public string ProductName { get; set; }

        /// <summary>
        /// 设备编号
        /// </summary>
        [Alias("设备编号")]
        public string AssetCode { get; set; }

        /// <summary>
        /// 点位地址
        /// </summary>
        [Alias("*地址")]
        public string Address { get; set; }

        /// <summary>
        /// 点位地址名称
        /// </summary>
        [Alias("*名称")]
        public string AddressName { get; set; }

        /// <summary>
        /// 通讯点位数据类型
        /// </summary>
        [Alias("*数据类型")]
        public string DataType { get; set; }

        [Alias("系数")]
        public string Coefficient { get; set; }

        [Alias("修正值")]
        public string Correction { get; set; }

        [Alias("错误信息")]
        public string ErrMsg
        {
            get
            {
                return "";
            }
            set { }
        }

        public List<string> ErrMsgs { get; set; } = new List<string>();
    }
}
