using System.Collections.Generic;
using System.Linq;

namespace Talk.Extensions
{
    public static class EnumerableExtensions
    {
        /// <summary>
        /// 确定序列是否包含任何元素。
        /// </summary>
        /// <typeparam name="T">序列对象类型</typeparam>
        /// <param name="t">序列对象</param>
        /// <returns>如果源序列包含任何元素，则为 true；否则为 false。</returns>
        public static bool IsAny<T>(this IEnumerable<T> t)
        {
            if (t == null) return false;

            return t.Any();
        }

        /// <summary>
        /// 将序列对象转换成字符串，多个时按分隔符分隔
        /// </summary>
        /// <typeparam name="T">序列对象的类型</typeparam>
        /// <param name="source">序列对象</param>
        /// <param name="separator">分隔符（默认,）</param>
        /// <returns></returns>
        public static string StringJoin<T>(this IEnumerable<T> source, string separator = ",")
        {
            if (!source.IsAny()) return "";

            return string.Join(separator, source);
        }
    }
}
