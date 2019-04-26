using System;

namespace Talk.Extensions
{
    public class NumberHelper
    {
        /// <summary>
        /// 保留小数位 - 不四舍五入（默认保留2位）
        /// </summary>
        /// <param name="number"></param>
        /// <param name="position"></param>
        /// <returns></returns>
        public static double KeepDigit(double number, int position = 2)
        {
            return (int)(number * Math.Pow(10, position)) / Math.Pow(10, position);
        }

        /// <summary>
        /// 保留小数位 - 四舍五入（默认保留2位）
        /// </summary>
        /// <param name="number"></param>
        /// <param name="position"></param>
        /// <returns></returns>
        public static double Round(double number, int position = 2)
        {
            return Math.Round(number, position);
        }
    }
}
