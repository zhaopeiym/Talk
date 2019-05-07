using Talk.Extensions.Helper;

namespace Talk.Extensions
{
    public static class ShortCodeExtension
    {
        public static ShortCodeGenerate shortCode;

        public static void Init(string seqkey)
        {
            shortCode = new ShortCodeGenerate(seqkey);
        }

        /// <summary>
        /// 转换短码
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string EncryptShortCode(this long str)
        {
            return shortCode.ConfusionConvert(str);
        }

        /// <summary>
        /// 解析短码
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static long DecryptShortCode(this string str)
        {
            return shortCode.ConfusionConvert(str);
        }

        /// <summary>
        /// 解析短码
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static long DecryptShortCodeTry(this string str)
        {
            try
            {
                return shortCode.ConfusionConvert(str);
            }
            catch (System.Exception)
            {
                return -1;
            }
        }
    }
}
