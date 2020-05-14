using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace Common
{
    /// <summary>
    /// 加密类
    /// </summary>
    public class Cryptogram
    {
        public static string GetMD5(string str)
        {
            MD5 md5 = MD5.Create();
            byte[] s = md5.ComputeHash(Encoding.UTF8.GetBytes(str));
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < s.Length; i++)
            {
                sb.Append(s[i].ToString("x2"));//ToString("x");
            }
            return sb.ToString();
        }


        /// <summary>
        /// 根据Hex字符串，DES解密出参数
        /// </summary>
        /// <param name="hexString"></param>
        /// <param name="desKey"></param>
        /// <returns></returns>
        public static string DesDecrypt(string hexString, string desKey = "", string desIV = "")
        {
            byte[] inputArray = Convert.FromBase64String(hexString);
            var tripleDES = TripleDES.Create();
            tripleDES.Key = Encoding.UTF8.GetBytes(desKey);
            tripleDES.IV = Encoding.UTF8.GetBytes(desIV);
            tripleDES.Mode = CipherMode.ECB;
            tripleDES.Padding = PaddingMode.PKCS7;
            ICryptoTransform cTransform = tripleDES.CreateDecryptor();
            byte[] resultArray = cTransform.TransformFinalBlock(inputArray, 0, inputArray.Length);
            return Encoding.UTF8.GetString(resultArray);
        }

        /// <summary>
        /// 加密
        /// </summary>
        /// <param name="param"></param>
        /// <param name="desKey"></param>
        /// <param name="desIV"></param>
        /// <returns></returns>
        public static string DesEncrypt(string param, string desKey = "", string desIV = "")
        {
            byte[] inputArray = Encoding.UTF8.GetBytes(param);
            var tripleDES = TripleDES.Create();
            tripleDES.Key = Encoding.UTF8.GetBytes(desKey);
            tripleDES.IV = Encoding.UTF8.GetBytes(desIV);
            tripleDES.Mode = CipherMode.ECB;
            tripleDES.Padding = PaddingMode.PKCS7;
            ICryptoTransform cTransform = tripleDES.CreateEncryptor();
            byte[] resultArray = cTransform.TransformFinalBlock(inputArray, 0, inputArray.Length);
            return Convert.ToBase64String(resultArray, 0, resultArray.Length);
        }
    }
}
