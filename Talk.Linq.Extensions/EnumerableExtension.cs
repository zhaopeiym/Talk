using System;
using System.Collections.Generic;
using System.Linq;

namespace Talk.Linq.Extensions
{
    /// <summary>
    /// 
    /// </summary>
    public static class EnumerableExtension
    {
        /// <summary>
        /// 确定Enumerable是否包含任何元素。
        /// </summary>
        /// <typeparam name="T">Enumerable对象类型</typeparam>
        /// <param name="t">Enumerable对象</param>
        /// <returns>如果源Enumerable包含任何元素，则为 true；否则为 false。</returns>
        public static bool IsAny<T>(this IEnumerable<T> t)
        {          
            if (t == null) return false;
            return t.Any();
        }

        /// <summary>
        /// 将Enumerable对象转换成字符串，多个时按分隔符分隔
        /// </summary>
        /// <typeparam name="T">Enumerable对象的类型</typeparam>
        /// <param name="source">Enumerable对象</param>
        /// <param name="separator">分隔符（默认,）</param>
        /// <returns></returns>
        public static String StringJoin<T>(this IEnumerable<T> source, string separator = ",")
        {
            if (!source.IsAny())
            {
                return "";
            }
            return String.Join(separator, source);
        }

        /// <summary>
        /// 去重
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <typeparam name="TKey"></typeparam>
        /// <param name="source"></param>
        /// <param name="keySelector"></param>
        /// <returns></returns>
        public static IEnumerable<TSource> DistinctBy<TSource, TKey>(this IEnumerable<TSource> source, Func<TSource, TKey> keySelector)
        {
            //return collection.GroupBy(groupFunc).Select(group => group.First());
            HashSet<TKey> seenKeys = new HashSet<TKey>();
            foreach (TSource element in source)
            {
                if (seenKeys.Add(keySelector(element)))
                {
                    yield return element;
                }
            }
        }

        /// <summary>
        /// 比较两个集合中的每个元素是否都一样
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <typeparam name="TKey"></typeparam>
        /// <param name="source1"></param>
        /// <param name="source2"></param>
        /// <returns></returns>
        public static bool ItmesEquals<TSource>(this IEnumerable<TSource> source1, IEnumerable<TSource> source2)
        {
            return (source1.Count() == source2.Count() && !source1.Any(t => !source2.Contains(t)));
        }

        /// <summary>
        /// 将Enumerable对象转换成列表对象
        /// </summary>
        /// <param name="list">Enumerable对象</param>
        /// <returns></returns>
        public static List<int> Each2Int(this IEnumerable<string> list)
        {
            var lstVal = new List<int>();
            foreach (var item in list)
            {
                var value = -1;
                if (!int.TryParse(item, out value)) continue;
                lstVal.Add(value);
            }
            return lstVal;
        }

        /// <summary>
        /// 枚举迭代Enumerable对象，按照传入方法操作
        /// </summary>
        /// <typeparam name="T">Enumerable对象类型</typeparam>
        /// <param name="list">Enumerable对象</param>
        /// <param name="action">操作方法</param>
        /// <returns></returns>
        public static IEnumerable<T> ForEach<T>(this IEnumerable<T> list, Action<T> action)
        {
            var each = list as IList<T> ?? list.ToList();
            foreach (T t in each)
            {
                action.Invoke(t);
            }
            return each;
        }    

        /// <summary>
        /// 自定义排序，根据条件的先后将原List重新排序
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list">需要排序的List</param>
        /// <param name="predicates">表达式条件</param>
        /// <returns></returns>
        public static List<T> CustomOrderBy<T>(this IEnumerable<T> list, params Func<T, bool>[] predicates)
        {
            List<T> items = new List<T>();
            foreach (var predicate in predicates)
            {
                var datas = list.Where(predicate).ToList();
                if (datas != null && datas.Any())
                {
                    var expect = items.Except(datas);
                    items.AddRange(expect);
                }
            }
            var diff = list.Except(items);
            items.AddRange(diff);
            return items;
        }  

        /// <summary>
        /// 是否包含
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TKey"></typeparam>
        /// <param name="collection"></param>
        /// <param name="selectFunc"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool Contains<T, TKey>(this IEnumerable<T> collection, Func<T, TKey> selectFunc, TKey value)
        {
            return collection.Select(selectFunc).Contains(value);
        } 

        #region MyRegion
        ///// <summary>
        ///// 随机取 Enumerable 中的一个值
        ///// </summary>
        ///// <typeparam name="T"></typeparam>
        ///// <param name="source"></param>
        ///// <param name="random"></param>
        ///// <returns></returns>
        //public static T RandomEnumerableValue<T>(this IEnumerable<T> source, Random random)
        //{
        //    if (source == null)
        //        throw new ArgumentNullException("source");
        //    if (random == null)
        //        throw new ArgumentNullException("random");

        //    if (source is ICollection)//如果实现了ICollection接口
        //    {
        //        ICollection collection = source as ICollection;
        //        int count = collection.Count;//获取总数，用来取合理随机数
        //        if (count == 0)
        //        {
        //            throw new Exception("IEnumerable没有数据");
        //        }
        //        int index = random.Next(count);
        //        return source.ElementAt(index);//直接取Enumerable中的值
        //    }
        //    using (IEnumerator<T> iterator = source.GetEnumerator())
        //    {
        //        if (!iterator.MoveNext())
        //        {
        //            throw new Exception("IEnumerable没有数据");
        //        }
        //        int count = 1;
        //        T current = iterator.Current;
        //        while (iterator.MoveNext())
        //        {
        //            count++;
        //            if (random.Next(count) == 0) //看似不够随机，其实每个取值的概率都是一样的。（第n个值取值概率的1/n）
        //                current = iterator.Current;
        //        }
        //        return current;
        //    }
        //} 
        #endregion
    }
}
