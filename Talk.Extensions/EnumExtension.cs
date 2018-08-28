using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Talk.Extensions
{
    /// <summary>
    /// 枚举 扩展类
    /// </summary>
    public static class EnumExtension
    {
        /// <summary>
        /// 获取DisplayAttribute上指定的Name
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string ToDisplay(this Enum value)
        {
            var info = value.GetType().GetField(value.ToString());
            var attribute = (DisplayAttribute)info.GetCustomAttributes(typeof(DisplayAttribute), true).FirstOrDefault();
            return attribute == null ? value.ToString() : attribute.Name;
        }

        /// <summary>
        ///  获取DescriptionAttribute上指定的Description
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string ToDescription(this Enum value)
        {
            return EnumHelper.GetEnumDescription(value);
        }

        /// <summary>
        ///  转化为枚举类型，转化不成功为默认值
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static T ToEnum<T>(this int value, T defVal = default(T)) where T : struct
        {
            return EnumHelper.ConvertToEnum<T>(value.ToString(), defVal);
        }

        /// <summary>
        /// 获取DisplayAttribute上指定的Name
        /// </summary>
        /// <param name="value"></param>
        /// <param name="defVal">defVal</param>
        /// <returns></returns>
        public static string GetDisplayName(this Enum value, string defVal = "")
        {
            var attribute = EnumHelper.GetEnumDisplayAttributs(value).FirstOrDefault();
            return attribute == null ? defVal : attribute.GetName();
        }

        /// <summary>
        /// 获取DisplayAttribute上指定的ShortName
        /// </summary>
        /// <param name="value"></param>
        /// <param name="defVal">defVal</param>
        /// <returns></returns>
        public static string GetDisplayShortName(this Enum value, string defVal = "")
        {
            var attribute = EnumHelper.GetEnumDisplayAttributs(value).FirstOrDefault();
            return attribute == null ? defVal : attribute.GetShortName();
        }

        /// <summary>
        /// 获取DisplayAttribute上指定的Desc
        /// </summary>
        /// <param name="value"></param>
        /// <param name="defVal">defVal</param>
        /// <returns></returns>
        public static string GetDisplayDesc(this Enum value, string defVal = "")
        {
            var attribute = EnumHelper.GetEnumDisplayAttributs(value).FirstOrDefault();
            return attribute == null ? defVal : attribute.GetDescription();
        }
    }

    #region EnumHelper
    /// <summary>
    /// 枚举Entry
    /// </summary>
    /// <typeparam name="TEnum">枚举类型</typeparam>
    /// <typeparam name="TRaw">枚举类型的基础类型</typeparam>
    public class EnumEntry<TEnum, TRaw>
    {
        public EnumEntry()
        {

        }
        public EnumEntry(TEnum value, TRaw raw, string strEnum)
        {
            EnumValue = value;
            RawValue = raw;
            EnumString = strEnum;
        }
        /// <summary>
        /// 基础数值类型
        /// </summary>
        public TRaw RawValue { get; set; }
        /// <summary>
        /// 原始枚举类型
        /// </summary>
        public TEnum EnumValue { get; set; }
        /// <summary>
        /// 枚举类型字符串表示
        /// </summary>
        public string EnumString { get; set; }
        /// <summary>
        /// 枚举类型的文字描述(DescriptionAttribute)
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// 枚举类型的本地字符串名称(DisplayAttribute.Name)
        /// </summary>
        public string DisplayName { get; set; }

        /// <summary>
        /// 枚举类型的本地字符串简称(DisplayAttribute.ShortName)
        /// </summary>
        public string DisplayShortName { get; set; }
        /// <summary>
        /// 枚举类型的Order(DisplayAttribute.Order)
        /// </summary>
        public int DisplayOrder { get; set; }

        /// <summary>
        /// 枚举类型的本地字符串说明(DisplayAttribute.Description)
        /// </summary>
        public string DisplayDesc { get; set; }

        /// <summary>
        /// 自定义名称
        /// </summary>
        public string CustomName { get; set; }
    }

    /// <summary>
    /// 枚举类型的帮助类
    /// </summary>
    public static class EnumHelper
    {
        /// <summary>
        /// 将指定枚举类型转换成List，用来绑定DropDownList
        /// </summary>
        /// <typeparam name="T">枚举类型的基础型</typeparam>
        /// <param name="enumType">枚举类型</param>
        /// <param name="excludes">排除的枚举类型集合（基础型集合）</param>
        /// <param name="func">枚举类型对应的文字描述</param>
        /// <returns></returns>
        public static Dictionary<string, string> ConvertEnumToList<T>(Type enumType, IEnumerable<T> excludes = null, Func<T, string, string> func = null)
        {
            if (enumType.IsEnum == false) { return null; }
            var list = new Dictionary<string, string>();
            FieldInfo[] fields = enumType.GetFields();
            string strText = string.Empty;
            string strValue = string.Empty;
            T rawValue = default(T);
            foreach (FieldInfo field in fields)
            {
                if (!field.FieldType.IsEnum) continue;
                //strValue = Convert.ChangeType(enumType.InvokeMember(field.Name, BindingFlags.GetField, null, null, null), typeof(T)).ToString();
                strValue = field.GetRawConstantValue().ToString();
                rawValue = (T)field.GetRawConstantValue();
                if (excludes != null && excludes.Any(o => o.Equals(rawValue)))
                {
                    continue;
                }
                if (func != null)
                {
                    strText = func(rawValue, "");
                }
                else
                {
                    Enum objEnum = Enum.Parse(enumType, field.Name) as Enum;
                    strText = GetEnumDescription(objEnum);
                    if (strText == field.Name)
                    {
                        strText = GetEnumDisplay(objEnum);
                    }
                }
                list.Add(strValue, strText);
            }
            return list;
        }

        public static List<EnumEntry<TEnum, TRaw>> ConvertEnumToEntryList<TEnum, TRaw>(IEnumerable<TEnum> excludes = null, Func<TRaw, string, string> func = null)
        {
            var enumType = typeof(TEnum);
            var list = new List<EnumEntry<TEnum, TRaw>>();
            if (enumType.IsEnum == false) return list;

            //
            TEnum[] aryEnum = Enum.GetValues(enumType) as TEnum[];
            if (aryEnum == null) return list;

            var lstEnums = aryEnum;
            if (excludes != null)
            {
                lstEnums = aryEnum.Except(excludes).ToArray();
            }
            //
            foreach (var item in lstEnums)
            {
                string strEnum = item.ToString();
                TRaw rawVal = (TRaw)Convert.ChangeType(item, typeof(TRaw));
                Enum objEnum = Enum.Parse(enumType, strEnum) as Enum;
                var entry = new EnumEntry<TEnum, TRaw>(item, rawVal, strEnum)
                {
                    Description = GetEnumDescription(objEnum)
                };
                //entry.DisplayName = GetEnumDisplay(objEnum);
                //entry.DisplayDesc = GetEnumDisplay(objEnum, 1);
                var disObj = GetEnumDisplayAttributs(objEnum);
                DisplayAttribute displayAttr = null;
                if (disObj != null)
                {
                    displayAttr = disObj.FirstOrDefault();
                }
                if (displayAttr != null)
                {
                    entry.DisplayName = displayAttr.GetName();
                    entry.DisplayShortName = displayAttr.GetShortName();
                    entry.DisplayDesc = displayAttr.GetDescription();
                    entry.DisplayOrder = displayAttr.GetOrder() ?? 0;
                }
                if (func != null)
                {
                    entry.CustomName = func(rawVal, "");
                }

                list.Add(entry);
            }

            list = list.OrderBy(l => l.RawValue).ToList();
            return list;
        }
        /// <summary>
        /// 获取EnumEntry元素简称
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="T1"></typeparam>
        /// <param name="enums"></param>
        /// <param name="code">编码</param>
        /// <returns></returns>
        public static string GetEnumShortNameByCode<T, T1>(List<EnumEntry<T, T1>> enums, string code, string spliteSChar = "[", string spliteEChar = "]")
        {
            var value = string.Empty;
            if (enums != null)
            {
                var selectedValue = enums.FirstOrDefault(o => o.DisplayName == code);
                if (selectedValue != null)
                    value = spliteSChar + selectedValue.DisplayShortName + spliteEChar;
            }
            return value;
        }
        /// <summary>
        /// 取得枚举类型的说明文字
        /// </summary>
        /// <param name="objEnum"></param>
        /// <returns></returns>
        public static string GetEnumDescription(Enum objEnum)
        {
            Type typeDescription = typeof(DescriptionAttribute);
            Type typeField = objEnum.GetType();
            string strDesc = string.Empty;
            try
            {
                FieldInfo field = typeField.GetField(objEnum.ToString());
                var arr = field.GetCustomAttributes(typeDescription, true);
                if (arr.Length > 0)
                {
                    strDesc = (arr[0] as DescriptionAttribute).Description;
                }
                else
                {
                    strDesc = field.Name;
                }
            }
            catch
            {
                strDesc = string.Empty;
            }

            return strDesc;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="objEnum"></param>
        /// <param name="flag">默认值为0.获取Display属性Name值，否则获取Descriptiom</param>
        /// <returns></returns>
        public static string GetEnumDisplay(Enum objEnum, int flag = 0)
        {
            var typeDisplayName = typeof(DisplayAttribute);
            var typeField = objEnum.GetType();
            string strDesc = string.Empty;
            try
            {
                FieldInfo field = typeField.GetField(objEnum.ToString());
                var arr = field.GetCustomAttributes(typeDisplayName, true);
                if (arr.Length > 0)
                {
                    var displayAttribute = arr[0] as DisplayAttribute;
                    if (displayAttribute != null)
                        strDesc = flag == 0 ? displayAttribute.Name : displayAttribute.Description;
                }
                else
                {

                    strDesc = field.Name;
                }
            }
            catch
            {
                strDesc = String.Empty;
            }

            return strDesc;
        }
        /// <summary>
        /// 取得枚举类型的Display属性
        /// </summary>
        /// <param name="objEnum"></param>
        /// <returns></returns>
        public static DisplayAttribute[] GetEnumDisplayAttributs(Enum objEnum)
        {
            var typeDisplayName = typeof(DisplayAttribute);
            var typeField = objEnum.GetType();
            try
            {
                var field = typeField.GetField(objEnum.ToString());
                var arr = field.GetCustomAttributes(typeDisplayName, true);
                return arr.OfType<DisplayAttribute>().ToArray();
            }
            catch
            {
                return new DisplayAttribute[0];
            }
        }
        /// <summary>
        /// 把值转换为相应的枚举类型
        /// </summary>
        /// <typeparam name="T">枚举类型</typeparam>
        /// <param name="rawVal">值</param>
        /// <param name="defVal">默认值</param>
        /// <returns></returns>
        public static T ConvertToEnum<T>(string rawVal, T defVal = default(T)) where T : struct
        {
            T objEnum;
            Type typeEnum = typeof(T);
            if (String.IsNullOrEmpty(rawVal)) return defVal;
            //objEnum = (T)Enum.Parse(typeEnum, rawVal.ToString());
            if (!Enum.TryParse<T>(rawVal, out objEnum) || !Enum.IsDefined(typeEnum, objEnum))
            {
                objEnum = defVal;
            }
            return objEnum;
        }
    }
    #endregion
}

