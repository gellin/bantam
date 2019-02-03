using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Windows.Forms;

namespace bantam.Classes
{
    static class EncryptionHelper
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
        public static string DecryptShellResponse(string response, string encryptionKey, string encryptionIV, int encryptResponseMode)
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

            if (string.IsNullOrEmpty(decryptedResult)) {
                return string.Empty;
            }

            var finalResult = Helper.DecodeBase64ToString(decryptedResult);

            return finalResult;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="encryptionKey"></param>
        /// <param name="encryptionIV"></param>
        /// <param name="padding"></param>
        /// <returns></returns>
        public static RijndaelManaged BuildAesMode(byte[] encryptionKey, byte[] encryptionIV, PaddingMode padding)
        {
            var aes = new RijndaelManaged {
                Padding = padding,
                Mode = CipherMode.CBC,
                KeySize = 256,
                BlockSize = 128,
                Key = encryptionKey,
                IV = encryptionIV
            };

            return aes;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="cipherText"></param>
        /// <param name="encryptionKey"></param>
        /// <param name="encryptionIV"></param>
        /// <returns></returns>
        public static String DecryptRJ256(byte[] cipherText, string encryptionKey, string encryptionIV, PaddingMode padding = PaddingMode.PKCS7)
        {
            var result = string.Empty;
            var Key = Encoding.UTF8.GetBytes(encryptionKey);
            var IV = Encoding.UTF8.GetBytes(encryptionIV);

            using (var aes = BuildAesMode(Key, IV, padding)) {
                try {
                    using (var memoryStream = new MemoryStream(cipherText))
                    using (var cryptoStream = new CryptoStream(memoryStream, aes.CreateDecryptor(Key, IV), CryptoStreamMode.Read))
                    using (var streamReader = new StreamReader(cryptoStream)) {
                        result = streamReader.ReadToEnd();
                    }
                } catch (Exception e) {
                    MessageBox.Show(e.Message, "Failed to decrypt response");
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
        public static string EncryptBytesToRJ256ToBase64(byte[] plainText, string encryptionKey, string encryptionIV, PaddingMode padding = PaddingMode.PKCS7)
        {
            var result = string.Empty;
            var Key = Encoding.UTF8.GetBytes(encryptionKey);
            var IV = Encoding.UTF8.GetBytes(encryptionIV);

            using (var aes = BuildAesMode(Key, IV, padding)) {
                try {
                    using (var memoryStream = new MemoryStream()) {
                        using (var cryptoStream = new CryptoStream(memoryStream, aes.CreateEncryptor(Key, IV), CryptoStreamMode.Write)) {
                            cryptoStream.Write(plainText, 0, plainText.Length);
                            cryptoStream.Close();
                        }
                        result = Convert.ToBase64String(memoryStream.ToArray());
                    }
                } catch (Exception e) {
                    MessageBox.Show(e.Message, "Failed to encrypt string");
                } finally {
                    aes.Clear();
                }
            }
            return result;
        }
    }
}
