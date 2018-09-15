using System;

namespace Talk.Extensions
{
    public static class ComparableExtension
    {
        /// <summary>
        /// 检查一个值在最小值和最大值之间
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value">要检查的值</param>
        /// <param name="minInclusiveValue">最小(包容)值</param>
        /// <param name="maxInclusiveValue">最大(包容)值</param>
        /// <returns></returns>
        public static bool IsBetween<T>(this T value, T minInclusiveValue, T maxInclusiveValue) where T : IComparable<T>
        {
            return value.CompareTo(minInclusiveValue) >= 0 && value.CompareTo(maxInclusiveValue) <= 0;
        }
    }
}
