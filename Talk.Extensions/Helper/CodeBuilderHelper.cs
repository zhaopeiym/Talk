using System;
using System.Linq;

namespace Talk.Extensions.Helper
{
    /// <summary>
    /// code生成器
    /// </summary>
    public class CodeBuilderHelper
    {
        /// <summary>
        /// 字典
        /// 警告：不可随意修改字典
        /// </summary>
        private static string[] Dictionaries => new string[]
        {
            "5064271938",
            "0918724365",
            "6547081923",
            "9018274356",
            "9081723654",
            "0912348576",
            "8945670123"
        };

        /// <summary>
        /// Code转换器
        /// </summary>
        /// <param name="numberChar">数字</param>
        /// <param name="position">位数</param>
        /// <returns></returns>
        private static char ConvertToCode(char numberChar, int position)
        {
            var number = Convert.ToInt32(numberChar.ToString());
            if (number < 0 || number > 9)
                throw new System.Exception("number范围有误");

            var index = position % Dictionaries.Length;//规则
            return Dictionaries[index][number];
        }

        /// <summary>
        /// 数字转成混淆后的Code
        /// </summary>
        /// <param name="number">数字</param>
        /// <param name="position">位数</param>
        /// <returns></returns>
        public static string NumberConvertCode(int number, int position = 2)
        {
            //数值混淆后的Code
            var code = string.Join("", number.ToString().PadLeft(position, '0').ToCharArray().Select((t, i) => (ConvertToCode(t, i))).ToList());
            return code;
        }

        /// <summary>
        /// 混淆后的Code解析成自增数字
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public static int CodeConvertNumber(string code)
        {
            var length = code.Length;
            var number = code.ToCharArray().Select((n, i) =>
            {
                var index = i % Dictionaries.Length;//规则

                var multiple = 1;//倍数
                for (int j = 0; j < length - (i + 1); j++)
                {
                    multiple *= 10;
                }
                var newNum = Dictionaries[index].IndexOf(n);//对应的坐标（也就是实际对应的数字）
                return newNum * multiple;//真正的数值
            }).Sum();
            return number;
        }
    }
}
