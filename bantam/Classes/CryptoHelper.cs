using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Windows.Forms;

namespace bantam.Classes
{
    static class CryptoHelper
    {
        /// <summary>
        /// 
        /// </summary>
        public const int IV_Length = 16;

        /// <summary>
        /// 
        /// </summary>
        public const int KEY_Length = 32;

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
            return Helper.RandomString(KEY_Length, true, true, true);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static string GetRandomEncryptionIV()
        {
            return Helper.RandomString(IV_Length, true, true, true);
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
        public static RijndaelManaged BuildAesMode(byte[] encryptionKey, byte[] encryptionIV)
        {
            var aes = new RijndaelManaged {
                Padding = PaddingMode.PKCS7,
                Mode = CipherMode.CBC,
                KeySize = 256,
                BlockSize = 128,
                Key = encryptionKey,
                IV = encryptionIV
            };

            return aes;
        }

        /// <summary>
        /// Decrypts an encrypted array of bytes to a string, using AES-256 / "Rijndael"-256
        /// </summary>
        /// <param name="cipherText"></param>
        /// <param name="encryptionKey"></param>
        /// <param name="encryptionIV"></param>
        /// <returns></returns>
        public static string DecryptRJ256(byte[] cipherText, string encryptionKey, string encryptionIV)
        {
            var result = string.Empty;
            var Key = Encoding.UTF8.GetBytes(encryptionKey);
            var IV = Encoding.UTF8.GetBytes(encryptionIV);

            using (var aes = BuildAesMode(Key, IV)) {
                try {
                    using (var memoryStream = new MemoryStream(cipherText))
                    using (var cryptoStream = new CryptoStream(memoryStream, aes.CreateDecryptor(Key, IV), CryptoStreamMode.Read))
                    using (var streamReader = new StreamReader(cryptoStream)) {
                        result = streamReader.ReadToEnd();
                    }
                } catch (Exception e) {
                    LogHelper.AddGlobalLog("Failed to decrypt cipherText - ( " + e.Message + " )", "Decryption routine failure", LogHelper.LOG_LEVEL.WARNING);
                } finally {
                    aes.Clear();
                }
            }
            return result;
        }

        /// <summary>
        /// Encrypts an array of plaintext bytes using AES-256 / "Rijndael"-256
        /// </summary>
        /// <param name="plainText"></param>
        /// <param name="encryptionKey"></param>
        /// <param name="encryptionIV"></param>
        /// <param name="padding"></param>
        /// <returns>Base64 encoded string, of encrypted bytes</returns>
        public static string EncryptBytesToRJ256ToBase64(byte[] plainText, string encryptionKey, string encryptionIV)
        {
            var result = string.Empty;
            var IV = Encoding.UTF8.GetBytes(encryptionIV);
            var Key = Encoding.UTF8.GetBytes(encryptionKey);

            using (var aes = BuildAesMode(Key, IV)) {
                try {
                    using (var memoryStream = new MemoryStream()) {
                        using (var cryptoStream = new CryptoStream(memoryStream, aes.CreateEncryptor(Key, IV), CryptoStreamMode.Write)) {
                            cryptoStream.Write(plainText, 0, plainText.Length);
                            cryptoStream.Close();
                        }
                        result = Convert.ToBase64String(memoryStream.ToArray());
                    }
                } catch (Exception e) {
                    LogHelper.AddGlobalLog("Failed to encrypt string - ( " + e.Message + " )", "Encryption routine failure", LogHelper.LOG_LEVEL.WARNING);
                } finally {
                    aes.Clear();
                }
            }
            return result;
        }

        /// <summary>
        /// Overloaded to accept a string as plaintext
        /// </summary>
        /// <param name="plainText"></param>
        /// <param name="encryptionKey"></param>
        /// <param name="encryptionIV"></param>
        /// <param name="padding"></param>
        /// <returns>Base64 encoded string, of encrypted bytes</returns>
        public static string EncryptBytesToRJ256ToBase64(string plainText, string encryptionKey, string encryptionIV)
        {
            return EncryptBytesToRJ256ToBase64(Encoding.UTF8.GetBytes(plainText), encryptionKey, encryptionIV);
        }
    }
}
