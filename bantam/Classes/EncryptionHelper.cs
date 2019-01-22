using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Windows.Forms;

namespace bantam.Classes
{
    class EncryptionHelper
    {
        /// <summary>
        /// 
        /// </summary>
        public enum RESPONSE_ENCRYPTION_TYPES
        {
            OPENSSL = 0,
            MCRYPT
        }

        /// <summary>
        /// 
        /// </summary>
        public static string GetRandomEncryptionKey()
        {
            int ivLength = 32;
            return Helper.RandomString(ivLength, true, true, true);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static string GetRandomEncryptionIV()
        {
            int keyLength = 16;
            return Helper.RandomString(keyLength, true, true, true);
        }

        /// <summary>
        /// Double base64 encoded because encrpytion or strlen was having issues encrypting non base64 encrypted data
        /// </summary>
        /// <param name="response"></param>
        /// <returns></returns>
        static public string DecryptShellResponse(string response, string encryptionKey, string encryptionIV, int encryptResponseMode)
        {
            if (string.IsNullOrEmpty(response)) {
                return string.Empty;
            }

            var encryptedResult = Helper.DecodeBase64(response);

            if (encryptedResult == null) {
                return string.Empty;
            }

            string decryptedResult = string.Empty;

            decryptedResult = DecryptRJ256(encryptedResult, encryptionKey, encryptionIV);

            //switch (encryptResponseMode) {
            //    case (int)RESPONSE_ENCRYPTION_TYPES.OPENSSL:
            //    decryptedResult = DecryptRJ256(encryptedResult, encryptionKey, encryptionIV);
            //    break;
            //    case (int)RESPONSE_ENCRYPTION_TYPES.MCRYPT:
            //    decryptedResult = DecryptRJ256(encryptedResult, encryptionKey, encryptionIV);
            //    break;
            //    default:
            //    decryptedResult = DecryptRJ256(encryptedResult, encryptionKey, encryptionIV);
            //    break;
            //}

            if (string.IsNullOrEmpty(decryptedResult)) {
                return string.Empty;
            }

            var finalResult = Helper.DecodeBase64ToString(decryptedResult);

            return finalResult;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="cipherText"></param>
        /// <param name="encryptionKey"></param>
        /// <param name="encryptionIV"></param>
        /// <returns></returns>
        static public String DecryptRJ256(byte[] cipherText, string encryptionKey, string encryptionIV, PaddingMode padding = PaddingMode.PKCS7)
        {
            var result = string.Empty;
            var Key = Encoding.UTF8.GetBytes(encryptionKey);
            var IV = Encoding.UTF8.GetBytes(encryptionIV);

            using (var aes = new RijndaelManaged()) {
                    aes.Padding = padding;
                    aes.Mode = CipherMode.CBC;
                    aes.KeySize = 256;
                    aes.BlockSize = 128;
                    aes.Key = Key;
                    aes.IV = IV;

                try {
                    using (var memoryStream = new MemoryStream(cipherText))
                    using (var cryptoStream = new CryptoStream(memoryStream, aes.CreateDecryptor(Key, IV), CryptoStreamMode.Read))
                    using (var streamReader = new StreamReader(cryptoStream)) {
                        result = streamReader.ReadToEnd();
                    }
                } catch (Exception e) {
                    //todo logging
                    MessageBox.Show(e.Message, "Failed to decrypt response");
                    return string.Empty;
                } finally {
                    aes.Clear();
                }
            }
            return result;
        }

        /// <summary>
        /// Returns a base64 encoded AES256 encrypted string
        /// </summary>
        /// <param name="plainText"></param>
        /// <param name="encryptionKey"></param>
        /// <param name="encryptionIV"></param>
        /// <param name="padding"></param>
        /// <returns></returns>
        static public String EncryptRJ256(string plainText, string encryptionKey, string encryptionIV, PaddingMode padding = PaddingMode.PKCS7)
        {
            var result = string.Empty;
            var Key = Encoding.UTF8.GetBytes(encryptionKey);
            var IV = Encoding.UTF8.GetBytes(encryptionIV);

            using (var aes = new RijndaelManaged()) {
                aes.Padding = padding;
                aes.Mode = CipherMode.CBC;
                aes.KeySize = 256;
                aes.BlockSize = 128;
                aes.Key = Key;
                aes.IV = IV;

                try {
                    using (var memoryStream = new MemoryStream())
                    using (var cryptoStream = new CryptoStream(memoryStream, aes.CreateEncryptor(Key, IV), CryptoStreamMode.Write)) { 
                        result = Convert.ToBase64String(memoryStream.ToArray());
                    }
                } catch (Exception e) {
                    //todo logging
                    //MessageBox.Show(e.Message, "Failed to encrypt string");
                    return string.Empty;
                } finally {
                    aes.Clear();
                }
            }
            return result;
        }
    }
}
