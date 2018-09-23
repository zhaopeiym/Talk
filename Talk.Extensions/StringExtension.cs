using System;
using System.Text;

namespace Talk.Extensions
{
    public static class StringExtension
    {
        /// <summary>
        /// 字符串是否为Null、空字符串组成。
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static bool IsNullOrEmpty(this string str)
        {
            return string.IsNullOrEmpty(str);
        }

        /// <summary>
        /// 字符串是否为Null、空字符串或仅由空白字符组成。
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static bool IsNullOrWhiteSpace(this string str)
        {
            return string.IsNullOrWhiteSpace(str);
        }

        /// <summary>
        /// 从字符串的开头得到一个字符串的子串
        /// len参数不能大于给定字符串的长度
        /// </summary>
        /// <param name="str"></param>
        /// <param name="len"></param>
        /// <returns></returns>
        public static string Left(this string str, int len)
        {
            if (str == null) 
                throw new ArgumentNullException("str"); 

            if (str.Length < len) 
                throw new ArgumentException("len参数不能大于给定字符串的长度"); 

            return str.Substring(0, len);
        }

        /// <summary>
        /// 从字符串的末尾得到一个字符串的子串
        /// len参数不能大于给定字符串的长度
        /// </summary>
        /// <param name="str"></param>
        /// <param name="len"></param>
        /// <returns></returns>
        public static string Right(this string str, int len)
        {
            if (str == null) 
                throw new ArgumentNullException("str"); 

            if (str.Length < len) 
                throw new ArgumentException("len参数不能大于给定字符串的长度"); 

            return str.Substring(str.Length - len, len);
        }

        /// <summary>
        /// 
        /// len参数大于给定字符串是返回原字符串
        /// </summary>
        /// <param name="str"></param>
        /// <param name="len"></param>
        /// <returns></returns>
        public static string MaxLeft(this string str, int len)
        {
            if (str == null)
                throw new ArgumentNullException("str");

            if (str.Length < len)
                return str;

            return str.Substring(0, len);
        }

        /// <summary>
        /// 从字符串的末尾得到一个字符串的子串
        /// </summary>
        /// <param name="str"></param>
        /// <param name="len"></param>
        /// <returns></returns>
        public static string MaxRight(this string str, int len)
        {
            if (str == null) 
                throw new ArgumentNullException("str"); 

            if (str.Length < len)
                return str;

            return str.Substring(str.Length - len, len);
        }

        /// <summary>
        /// 字符串转枚举
        /// </summary>
        /// <typeparam name="T">类型的枚举</typeparam>
        /// <param name="value">字符串值转换</param>
        /// <returns></returns>
        public static T ToEnum<T>(this string value)
           where T : struct
        {
            if (value == null) 
                throw new ArgumentNullException(nameof(value));  
            return (T)Enum.Parse(typeof(T), value);
        }

        /// <summary>
        /// 字符串转枚举
        /// </summary>
        /// <typeparam name="T">类型的枚举</typeparam>
        /// <param name="value">字符串值转换</param>
        /// <param name="ignoreCase">忽略大小写</param>
        /// <returns>Returns enum object</returns>
        public static T ToEnum<T>(this string value, bool ignoreCase)
            where T : struct
        {
            if (value == null)
                throw new ArgumentNullException(nameof(value)); 
            return (T)Enum.Parse(typeof(T), value, ignoreCase);
        }

        public static int ToInt32(this string str)
        {
            if (str == null)
                throw new ArgumentNullException(nameof(str));
            return Convert.ToInt32(str);
        }

        public static bool ToBoolean(this string str)
        {
            if (str == null)
                throw new ArgumentNullException(nameof(str));
            return Convert.ToBoolean(str);
        }

        public static DateTime ToDateTime(this string str)
        {
            if (str == null)
                throw new ArgumentNullException(nameof(str));
            return Convert.ToDateTime(str);
        }

        public static decimal ToDecimal(this string str)
        {
            if (str == null)
                throw new ArgumentNullException(nameof(str));
            return Convert.ToDecimal(str);
        }

        public static double ToDouble(this string str)
        {
            if (str == null)
                throw new ArgumentNullException(nameof(str));
            return Convert.ToDouble(str);
        }

        public static string BytesToString(this byte[] byteArray)
        {
            if (byteArray == null)
                throw new ArgumentNullException(nameof(byteArray));
            return Encoding.Default.GetString(byteArray);
        }

        public static byte[] ToBytes(this string str)
        {
            if (str == null)
                throw new ArgumentNullException(nameof(str));
            return Encoding.Default.GetBytes(str); ;
        }
    }
}
