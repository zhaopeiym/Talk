using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;

namespace Talk.Extensions.Helper
{
    /// <summary>
    /// Base64图片相互转换
    /// http://m.tnblog.net/aojiancc2/article/details/2622
    /// </summary>
    public class Base64Convert
    {
        /// <summary>
        /// 文件转换成Base64字符串
        /// </summary>
        /// <param name="fileName">文件绝对路径</param>
        /// <returns></returns>
        public string FileToBase64(string fileName)
        {
            using (FileStream fs = new FileStream(fileName, FileMode.Open))
            {
                byte[] bt = new byte[fs.Length];
                fs.Read(bt, 0, bt.Length);
                return Convert.ToBase64String(bt);
            }
        }

        /// <summary>
        /// Base64字符串转换成文件
        /// </summary>
        /// <param name="strInput">base64字符串</param>
        /// <param name="fileName">保存文件的绝对路径</param>
        /// <returns></returns>
        public void Base64ToFileAndSave(string strInput, string fileName)
        {
            //base64需要把前面的申明去掉
            string newbase = Regex.Replace(strInput, "data:image/.*;base64,", "");
            byte[] buffer = Convert.FromBase64String(newbase);
            using (FileStream fs = new FileStream(fileName, FileMode.CreateNew))
            {
                fs.Write(buffer, 0, buffer.Length);
            }
        }
    }
}
