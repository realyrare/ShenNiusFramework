using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace ShenNiusSystem.Common
{
    public class Md5Crypt
    {
        #region MD5加密字符串处理
        /// <summary>
        /// MD5加密字符串处理
        /// </summary>
        /// <param name="half">加密是16位还是32位；如果为true为16位</param>
        /// <param name="input">待加密码字符串</param>
        /// <returns></returns>
        public static string Encrypt(string input, bool half)
        {
            using (var md5 = MD5.Create())
            {
                var result = md5.ComputeHash(Encoding.UTF8.GetBytes(input));
                var strResult = BitConverter.ToString(result);
                strResult = strResult.Replace("-", "");
                if (half)//16位MD5加密（取32位加密的9~25字符）
                {
                    strResult = strResult?.Substring(8, 16);
                }
                return strResult;
            }            
        }
        #endregion

        /// <summary>
        /// MD5加密
        /// </summary>
        /// <param name="strPwd">加密的字符串</param>
        /// <returns></returns>
        public static string Encrypt(string strPwd)
        {
            MD5 md5 = new MD5CryptoServiceProvider();
            byte[] data = System.Text.Encoding.Default.GetBytes(strPwd);
            byte[] result = md5.ComputeHash(data);
            string ret = "";
            for (int i = 0; i < result.Length; i++)
                ret += result[i].ToString("x").PadLeft(2, '0');
            return ret;
        }
        #region 计算文件的MD5值
        /// <summary>
        /// 计算文件的MD5值
        /// </summary>
        /// <param name="filepath"></param>
        /// <returns></returns>
        public static string GetStreamMd5(Stream stream)
        {
            var oMd5Hasher = new MD5CryptoServiceProvider();
            byte[] arrbytHashValue = oMd5Hasher.ComputeHash(stream);
            //由以连字符分隔的十六进制对构成的String，其中每一对表示value 中对应的元素；例如“F-2C-4A”
            string strHashData = BitConverter.ToString(arrbytHashValue);
            //替换-
            strHashData = strHashData.Replace("-", "");

            return strHashData;
        }
        #endregion
    }

}
