using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace Talk.Extensions
{
    /// <summary>
    /// 加密、解密
    /// </summary>
    public static class EncryptDecryptExtension
    {
        #region Base64加密解密
        /// <summary>
        /// Base64加密
        /// </summary>
        /// <param name="input">需要加密的字符串</param>
        /// <returns></returns>
        public static string Base64Encrypt(this string input)
        {
            return Base64Encrypt(input, new UTF8Encoding());
        }

        /// <summary>
        /// Base64加密
        /// </summary>
        /// <param name="input">需要加密的字符串</param>
        /// <param name="encode">字符编码</param>
        /// <returns></returns>
        public static string Base64Encrypt(this string input, Encoding encode)
        {
            return Convert.ToBase64String(encode.GetBytes(input));
        }

        /// <summary>
        /// Base64解密
        /// </summary>
        /// <param name="input">需要解密的字符串</param>
        /// <returns></returns>
        public static string Base64Decrypt(this string input)
        {
            return Base64Decrypt(input, new UTF8Encoding());
        }

        /// <summary>
        /// Base64解密
        /// </summary>
        /// <param name="input">需要解密的字符串</param>
        /// <param name="encode">字符的编码</param>
        /// <returns></returns>
        public static string Base64Decrypt(this string input, Encoding encode)
        {
            return encode.GetString(Convert.FromBase64String(input));
        }
        #endregion

        #region DES加密解密
        /// <summary>
        /// DES加密
        /// </summary>
        /// <param name="data">加密数据</param>
        /// <param name="key">8位字符的密钥字符串</param>
        /// <param name="iv">8位字符的初始化向量字符串</param>
        /// <returns></returns>
        public static string DESEncrypt(this string data, string key, string iv)
        {
            byte[] byKey = Encoding.ASCII.GetBytes(key);
            byte[] byIV = Encoding.ASCII.GetBytes(iv);

            DESCryptoServiceProvider cryptoProvider = new DESCryptoServiceProvider();
            int i = cryptoProvider.KeySize;
            MemoryStream ms = new MemoryStream();
            CryptoStream cst = new CryptoStream(ms, cryptoProvider.CreateEncryptor(byKey, byIV), CryptoStreamMode.Write);

            StreamWriter sw = new StreamWriter(cst);
            sw.Write(data);
            sw.Flush();
            cst.FlushFinalBlock();
            sw.Flush();
            return Convert.ToBase64String(ms.GetBuffer(), 0, (int)ms.Length);
        }

        /// <summary>
        /// DES解密
        /// </summary>
        /// <param name="data">解密数据</param>
        /// <param name="key">8位字符的密钥字符串(需要和加密时相同)</param>
        /// <param name="iv">8位字符的初始化向量字符串(需要和加密时相同)</param>
        /// <returns></returns>
        public static string DESDecrypt(this string data, string key, string iv)
        {
            byte[] byKey = System.Text.ASCIIEncoding.ASCII.GetBytes(key);
            byte[] byIV = System.Text.ASCIIEncoding.ASCII.GetBytes(iv);

            byte[] byEnc;
            try
            {
                byEnc = Convert.FromBase64String(data);
            }
            catch
            {
                return null;
            }

            DESCryptoServiceProvider cryptoProvider = new DESCryptoServiceProvider();
            MemoryStream ms = new MemoryStream(byEnc);
            CryptoStream cst = new CryptoStream(ms, cryptoProvider.CreateDecryptor(byKey, byIV), CryptoStreamMode.Read);
            StreamReader sr = new StreamReader(cst);
            return sr.ReadToEnd();
        }
        #endregion

        #region MD5加密
        /// <summary>
        /// MD5加密
        /// </summary>
        /// <param name="input">需要加密的字符串</param>
        /// <returns></returns>
        public static string Md5Encrypt(this string input)
        {
            return Md5Encrypt(input, new UTF8Encoding());
        }

        /// <summary>
        /// MD5加密
        /// </summary>
        /// <param name="input">需要加密的字符串</param>
        /// <param name="encode">字符的编码</param>
        /// <returns></returns>
        public static string Md5Encrypt(this string input, Encoding encode)
        {
            using (MD5 md5 = new MD5CryptoServiceProvider())
            {
                byte[] t = md5.ComputeHash(encode.GetBytes(input));
                StringBuilder sb = new StringBuilder(32);
                for (int i = 0; i < t.Length; i++)
                    sb.Append(t[i].ToString("x").PadLeft(2, '0'));
                return sb.ToString().ToUpper();
            }
        }

        /// <summary>
        /// MD5对文件流加密
        /// </summary>
        /// <param name="sr"></param>
        /// <returns></returns>
        public static string Md5Encrypt(this Stream stream)
        {
            using (MD5 md5serv = new MD5CryptoServiceProvider())
            {
                byte[] buffer = md5serv.ComputeHash(stream);
                StringBuilder sb = new StringBuilder();
                foreach (byte var in buffer)
                    sb.Append(var.ToString("x2"));
                return sb.ToString().ToUpper();
            }
        }

        /// <summary>
        /// MD5加密(返回16位加密串)
        /// </summary>
        /// <param name="input"></param>
        /// <param name="encode"></param>
        /// <returns></returns>
        public static string ToMD5Encrypt16(this string input, Encoding encode)
        {
            MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider();
            string result = BitConverter.ToString(md5.ComputeHash(encode.GetBytes(input)), 4, 8);
            result = result.Replace("-", "");
            return result;
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
        #endregion

        #region 3DES 加密解密
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
            if (string.IsNullOrWhiteSpace(str))
                return string.Empty;

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
            if (string.IsNullOrWhiteSpace(str))
                return string.Empty;

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

        #endregion

        /// <summary>
        /// RSA加密
        /// </summary>
        /// <param name="str">需要加密的内容</param>
        /// <param name="publicKey">公钥</param>
        /// <param name="type">类型（加密算法类型 RSA SHA1 长度不限制，推荐使用2048位以上;RSA2 SHA256 密钥长度至少为2048）</param>
        /// <returns></returns>
        public static string RSAEncrypt(this string str, string publicKey, RSAType type)
        {
            RSAHelper helper = new RSAHelper(type, Encoding.UTF8, null, publicKey);
            return helper.Encrypt(str);
        }

        /// <summary>
        /// RSA解密
        /// </summary>
        /// <param name="str">需要解密的内容</param>
        /// <param name="privateKey">私钥</param>
        /// <param name="type">类型（加密算法类型 RSA SHA1 长度不限制，推荐使用2048位以上;RSA2 SHA256 密钥长度至少为2048）</param>
        /// <returns></returns>
        public static string RSADecrypt(this string str, string privateKey, RSAType type)
        {
            RSAHelper helper = new RSAHelper(type, Encoding.UTF8, privateKey);
            return helper.Decrypt(str);
        }
    }
}
