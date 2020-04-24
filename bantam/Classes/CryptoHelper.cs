using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace bantam.Classes
{
    static class CryptoHelper
    {
        /// <summary>
        /// IV length in bytes, for AES-256 in CBC
        /// </summary>
        public const int IV_Length = 16;

        /// <summary>
        /// Key length in bytes, for AES-256 in CBC
        /// </summary>
        public const int KEY_Length = 32;

        /// <summary>
        /// The different php methods that can be included to do AES-256 crypto operations
        /// </summary>
        public enum RESPONSE_ENCRYPTION_TYPES
        {
            OPENSSL = 0,
            MCRYPT
        }

        /// <summary>
        /// A string representation of "enum RESPONSE_ENCRYPTION_TYPES"
        /// </summary>
        public static readonly ReadOnlyCollection<string> encryptoModeStrings = new List<string> {
             "openssl",
             "mcrypt",
        }.AsReadOnly();

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
        public static string GetRandomEncryptionIV()
        {
            return Helper.RandomString(IV_Length, true, true, true);
        }

        /// <summary>
        /// Generic wrapper/handler for decrypting a response from a shell
        /// </summary>
        /// <param name="response"></param>
        /// <returns></returns>
        public static string DecryptShellResponse(string response, string encryptionKey, string encryptionIV, int encryptResponseMode)
        {
            if (string.IsNullOrEmpty(response)) {
                return string.Empty;
            }

            byte[] encryptedResult = Helper.DecodeBase64(response);

            if (encryptedResult == null) {
                return string.Empty;
            }

            string decryptedResult = string.Empty;

            decryptedResult = DecryptRJ256(encryptedResult, encryptionKey, encryptionIV);

            if (string.IsNullOrEmpty(decryptedResult)) {
                return string.Empty;
            }

            string finalResult = Helper.DecodeBase64ToString(decryptedResult);

            return finalResult;
        }

        /// <summary>
        /// Intializes and returns an "RijndaelManaged" instance that contains our AES crypto settings, currently using AES-256 in CBC mode.
        /// </summary>
        /// <param name="encryptionKey"></param>
        /// <param name="encryptionIV"></param>
        /// <param name="padding"></param>
        /// <returns></returns>
        public static RijndaelManaged BuildAesMode(byte[] encryptionKey, byte[] encryptionIV)
        {
            RijndaelManaged aes = new RijndaelManaged {
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
            string result = string.Empty;
            byte[] Key = Encoding.UTF8.GetBytes(encryptionKey);
            byte[] IV = Encoding.UTF8.GetBytes(encryptionIV);

            using (RijndaelManaged aes = BuildAesMode(Key, IV)) {
                try {
                    using (MemoryStream memoryStream = new MemoryStream(cipherText))
                    using (CryptoStream cryptoStream = new CryptoStream(memoryStream, aes.CreateDecryptor(Key, IV), CryptoStreamMode.Read))
                    using (StreamReader streamReader = new StreamReader(cryptoStream)) {
                        result = streamReader.ReadToEnd();
                    }
                }
                catch (Exception e) {
                    LogHelper.AddGlobalLog("Failed to decrypt cipherText - ( " + e.Message + " )", "Decryption routine failure", LogHelper.LOG_LEVEL.WARNING);
                }
                finally {
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
            string result = string.Empty;
            byte[] IV = Encoding.UTF8.GetBytes(encryptionIV);
            byte[] Key = Encoding.UTF8.GetBytes(encryptionKey);

            using (RijndaelManaged aes = BuildAesMode(Key, IV)) {
                try {
                    using (MemoryStream memoryStream = new MemoryStream()) {
                        using (CryptoStream cryptoStream = new CryptoStream(memoryStream, aes.CreateEncryptor(Key, IV), CryptoStreamMode.Write)) {
                            cryptoStream.Write(plainText, 0, plainText.Length);
                            cryptoStream.Close();
                        }
                        result = Convert.ToBase64String(memoryStream.ToArray());
                    }
                }
                catch (Exception e) {
                    LogHelper.AddGlobalLog("Failed to encrypt string - ( " + e.Message + " )", "Encryption routine failure", LogHelper.LOG_LEVEL.WARNING);
                }
                finally {
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
