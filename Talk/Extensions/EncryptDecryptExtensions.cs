using System;
using System.Security.Cryptography;
using System.Text;

namespace Talk.Extensions
{
    /// <summary>
    /// 3DES 加密、解密
    /// </summary>
    public static class EncryptDecryptExtensions
    {
        /// <summary>
        /// 3DES加密
        /// </summary>
        /// <param name="str"></param>
        /// <param name="key">必须16位</param>
        /// <returns></returns>
        public static string DES3Encrypt(this string str, string key)
        {
            if (key?.Length != 16)
                throw new ArgumentNullException("DES3Encrypt的Key必须为16位");

            byte[] inputArray = Encoding.UTF8.GetBytes(str);
            var tripleDES = TripleDES.Create();
            var byteKey = Encoding.UTF8.GetBytes(key);
            byte[] allKey = new byte[24];
            Buffer.BlockCopy(byteKey, 0, allKey, 0, 16);
            Buffer.BlockCopy(byteKey, 0, allKey, 16, 8);
            tripleDES.Key = allKey;
            tripleDES.Mode = CipherMode.ECB;
            tripleDES.Padding = PaddingMode.PKCS7;
            ICryptoTransform cTransform = tripleDES.CreateEncryptor();
            byte[] resultArray = cTransform.TransformFinalBlock(inputArray, 0, inputArray.Length);
            return Convert.ToBase64String(resultArray, 0, resultArray.Length);
        }

        /// <summary>
        /// 3DES解密
        /// </summary>
        /// <param name="str"></param>
        /// <param name="key">必须16位</param>
        /// <returns></returns>
        public static string DES3Decrypt(this string str, string key)
        {
            if (key?.Length != 16)
                throw new ArgumentNullException("DES3Decrypt的Key必须为16位");

            byte[] inputArray = Convert.FromBase64String(str);
            var tripleDES = TripleDES.Create();
            var byteKey = Encoding.UTF8.GetBytes(key);
            byte[] allKey = new byte[24];
            Buffer.BlockCopy(byteKey, 0, allKey, 0, 16);
            Buffer.BlockCopy(byteKey, 0, allKey, 16, 8);
            tripleDES.Key = allKey;
            tripleDES.Mode = CipherMode.ECB;
            tripleDES.Padding = PaddingMode.PKCS7;
            ICryptoTransform cTransform = tripleDES.CreateDecryptor();
            byte[] resultArray = cTransform.TransformFinalBlock(inputArray, 0, inputArray.Length);
            return Encoding.UTF8.GetString(resultArray);
        }

        /// <summary>
        /// Md5加密
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string ToMd5(this string str)
        {
            using (var md5 = MD5.Create())
            {
                var inputBytes = Encoding.UTF8.GetBytes(str);
                var hashBytes = md5.ComputeHash(inputBytes);

                var sb = new StringBuilder();
                foreach (var hashByte in hashBytes)
                {
                    sb.Append(hashByte.ToString("X2"));
                }

                return sb.ToString();
            }
        }
    }
}
