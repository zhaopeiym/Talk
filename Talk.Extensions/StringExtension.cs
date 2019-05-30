﻿using Microsoft.International.Converters.PinYinConverter;
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
        /// 添加判断添加
        /// </summary>
        /// <param name="str"></param>
        /// <param name="condition"></param>
        /// <returns></returns>
        public static string If(this string str, bool condition)
        {
            return condition ? str : string.Empty;
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
        /// 从指定位置截取字符串，如果小于指定位置，则返回空字符串
        /// </summary>
        /// <param name="str"></param>
        /// <param name="len"></param>
        /// <returns></returns>
        public static string Sub(this string str, int len)
        {
            if (str == null)
                throw new ArgumentNullException("str");

            if (str.Length < len)
                return string.Empty;

            return str.Substring(len);
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
        /// 是否是数字
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static bool IsDouble(this string str)
        {
            if (str == null)
                throw new ArgumentNullException(nameof(str));
            return double.TryParse(str, out double number);
        }

        /// <summary> 
        /// 汉字转化为拼音
        /// https://www.cnblogs.com/lwc1st/archive/2018/05/16/9045382.html
        /// https://gitee.com/hyjiacan/Pinyin4Net
        /// </summary> 
        /// <param name="str">汉字</param> 
        /// <returns>全拼</returns> 
        public static string GetPinyin(this string str)
        {
            string r = string.Empty;
            foreach (char obj in str)
            {
                try
                {
                    ChineseChar chineseChar = new ChineseChar(obj);
                    string t = chineseChar.Pinyins[0].ToString();
                    r += t.Substring(0, t.Length - 1);
                }
                catch
                {
                    r += obj.ToString();
                }
            }
            return r;
        }

        /// <summary> 
        /// 汉字转化为拼音首字母
        /// </summary> 
        /// <param name="str">汉字</param> 
        /// <returns>首字母</returns> 
        public static string GetFirstPinyin(this string str)
        {
            string r = string.Empty;
            foreach (char obj in str)
            {
                try
                {
                    ChineseChar chineseChar = new ChineseChar(obj);
                    string t = chineseChar.Pinyins[0].ToString();
                    r += t.Substring(0, 1);
                }
                catch
                {
                    r += obj.ToString();
                }
            }
            return r;
        }

        #region 类型转换
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

        public static float ToFloat(this string str)
        {
            //https://www.cnblogs.com/cjm123/p/8619910.html
            if (str == null)
                throw new ArgumentNullException(nameof(str));
            return Convert.ToSingle(str);
        }

        /// <summary>
        /// 转出对应数据类型
        /// </summary>
        /// <param name="str"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public static object ToDataFormType(this string str, Type type)
        {
            str = str?.Trim();
            try
            {
                switch (Type.GetTypeCode(type))
                {
                    case TypeCode.Boolean:
                        return bool.Parse(str);
                    case TypeCode.Byte:
                        return byte.Parse(str);
                    case TypeCode.Char:
                        return char.Parse(str);
                    case TypeCode.DateTime:
                        return DateTime.Parse(str);
                    case TypeCode.Decimal:
                        return decimal.Parse(str);
                    case TypeCode.Double:
                        return double.Parse(str);
                    case TypeCode.Int16:
                        return short.Parse(str);
                    case TypeCode.Int32:
                        return int.Parse(str);
                    case TypeCode.Int64:
                        return long.Parse(str);
                    case TypeCode.SByte:
                        return sbyte.Parse(str);
                    case TypeCode.Single:
                        return float.Parse(str);
                    case TypeCode.UInt16:
                        return ushort.Parse(str);
                    case TypeCode.UInt32:
                        return uint.Parse(str);
                    case TypeCode.UInt64:
                        return ulong.Parse(str);
                }
            }
            catch
            { }
            return str;
        }

        /// <summary>
        /// 如果不是数值 则返回默认值
        /// </summary>
        /// <param name="str"></param>
        /// <param name="defaultNumber"></param>
        /// <returns></returns>
        public static float? ToFloatOrDefault(this string str, float? defaultNumber = 0)
        {
            if (float.TryParse(str?.Trim(), out float number))
            {
                return number;
            }
            return defaultNumber;
        }

        /// <summary>
        /// Bytes转String
        /// </summary>
        /// <param name="byteArray"></param>
        /// <returns></returns>
        public static string BytesToString(this byte[] byteArray)
        {
            if (byteArray == null)
                throw new ArgumentNullException(nameof(byteArray));
            return Encoding.Default.GetString(byteArray);
        }

        /// <summary>
        /// String转Bytes
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static byte[] ToBytes(this string str)
        {
            if (str == null)
                throw new ArgumentNullException(nameof(str));
            return Encoding.Default.GetBytes(str);
        }
        #endregion 
    }
}
