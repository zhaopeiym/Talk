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

        /// <summary>
        /// 千字节(KB)转可读大小
        /// </summary>
        /// <param name="size"></param>
        /// <returns></returns>
        public static string KBToShowSize(float size)
        {
            if (size / 1024 / 1024 >= 1)
            {
                return $"{ Round(size / 1024 / 1024)}GB";
            }
            else if (size / 1024 >= 1)
            {
                return $"{Round(size / 1024)}MB";
            }
            else
            {
                return $"{size}KB";
            }
        }

        /// <summary>
        /// 字节(B)转可读大小
        /// </summary>
        /// <param name="size"></param>
        /// <returns></returns>
        public static string BToShowSize(float size)
        {
            if (size / 1024 / 1024 / 1024 >= 1)
            {
                return $"{Round(size / 1024 / 1024 / 1024)}GB";
            }
            else if (size / 1024 / 1024 >= 1)
            {
                return $"{Round(size / 1024 / 1024)}MB";
            }
            else
            {
                return $"{Round(size / 1024)}KB";
            }
        }
    }
}
