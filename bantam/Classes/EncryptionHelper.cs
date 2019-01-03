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
        public static string GetRandomEncryptionKey()
        {
            int keyLength = 16;
            bool capitals = false;
            return Helper.RandomString(keyLength, capitals);
        }

        /// <summary>
        /// todo dynamic and move to helpers
        /// </summary>
        /// <returns></returns>
        public static string GetRandomEncryptionIV()
        {
            int ivLength = 32;
            return Helper.RandomNumberString(ivLength);
        }

        /// <summary>
        /// Double base64 encoded because encrpytion or strlen was having issues encrypting non base64 encrypted data
        /// </summary>
        /// <param name="response"></param>
        /// <returns></returns>
        static public string DecryptShellResponse(string response, string encryptionKey, string encryptionIV)
        {
            if (string.IsNullOrEmpty(response)) {
                return string.Empty;
            }

            var encryptedResult = Helper.DecodeBase64(response);

            if (encryptedResult == null) {
                return string.Empty;
            }

            var decryptedResult = DecryptRJ256(encryptedResult, encryptionKey, encryptionIV);

            if (string.IsNullOrEmpty(decryptedResult)) {
                return string.Empty;
            }

            var finalResult = Helper.DecodeBase64ToString(decryptedResult);

            return finalResult;
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
