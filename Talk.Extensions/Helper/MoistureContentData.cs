using System;
using System.Collections.Generic;
using System.Text;

namespace Talk.Extensions.Helper
{
    /// <summary>
    /// 含湿量数据
    /// </summary>
    public class MoistureContentData
    {
        /// <summary>
        /// 饱和水蒸气分压力Pa
        /// </summary>
        public double PartialPressureSaturatedSteam { get; set; }

        /// <summary>
        /// 含湿量g/kg
        /// </summary>
        public double MoistureContent { get; set; }

        /// <summary>
        /// 焓值计算KJ/Kg
        /// </summary>
        public double Enthalpy { get; set; }
    }
}
