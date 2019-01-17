using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace bantam.Classes
{
    class Helper
    {
        /// <summary>
        /// 
        /// </summary>
        private static Random rdm = new Random();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="maxLength"></param>
        /// <param name="matchLength"></param>
        /// <returns></returns>
        public static int RandomNumber(int maxNumber)
        {
            return rdm.Next(1, maxNumber);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="maxLength"></param>
        /// <param name="matchLength"></param>
        /// <returns></returns>
        public static string RandomNumberString(int maxLength, bool matchLength = true)
        {
            string s = string.Empty;

            if (!matchLength) {
                maxLength = rdm.Next(1, maxLength);
            }

            for (int i = 0; i < maxLength; i++) {
                s += rdm.Next(10).ToString();
            }

            return s;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="length"></param>
        /// <returns></returns>
        public static string RandomString(int length, bool capitals = true, bool numbers = false)
        {
            var charSet = "abcdefghijklmnopqrstuvwxyz";

            if (capitals) {
                charSet += "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            }

            if (numbers) {
                charSet += "0123456789";
            }

            string stringResult = string.Empty;

            for (int i = 0; i < length; i++) {
               stringResult += charSet[rdm.Next(charSet.Length)];
            }
            return stringResult;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="base64"></param>
        /// <returns></returns>
        public static string EncodeBase64ToString(string str)
        {
            if (string.IsNullOrEmpty(str)) {
                return String.Empty;
            }

            var plainTextBytes = Encoding.UTF8.GetBytes(str);
            string b64Code = Convert.ToBase64String(plainTextBytes);
            return b64Code;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string DecodeBase64ToString(string str)
        {
            if (string.IsNullOrEmpty(str)) {
                return string.Empty;
            }

            string cleanB64 = str;

            if (!Regex.IsMatch(str, @"^[a-zA-Z0-9\+/]*={0,2}$")) {
                //todo level 2 or 3 logging cfg
                //MessageBox.Show(str, "Unable to decode base64! - cleaning it and trying again");
                cleanB64 = Regex.Replace(str, "[^a-zA-Z0-9+=/]", string.Empty);
            }

            try {
                return Encoding.UTF8.GetString(Convert.FromBase64String(cleanB64));
            }
            catch(Exception) {
                //todo level 1 logging
                MessageBox.Show(str, "Unable to decode base64!");
                return string.Empty;
            }
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

            string cleanB64 = str;

            if (!Regex.IsMatch(str, @"^[a-zA-Z0-9\+/]*={0,2}$")) {
                //todo level 2 or level 3 logging cfg
                //MessageBox.Show(str, "Unable to decode base64! - cleaning it and trying again");
                cleanB64 = Regex.Replace(str, "[^a-zA-Z0-9+=/]", string.Empty);
            }

            try {
                var decbuff = Convert.FromBase64String(cleanB64);
                return decbuff;
            } catch (Exception) {
                //todo level 1 logging
                MessageBox.Show(str, "Unable to decode base64!");
                return null;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public static string MinifyCode(string code)
        {
            string clean = Regex.Replace(code, @"\t|\n|\r", string.Empty);
            string clean2 = Regex.Replace(clean, @"[^\u0000-\u007F]+", string.Empty);
            return Regex.Replace(clean2, @"\s+", " ");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TKey"></typeparam>
        /// <typeparam name="TValue"></typeparam>
        /// <param name="dict"></param>
        /// <returns></returns>
        public static TKey RandomDicionaryValue<TKey, TValue>(Dictionary<TKey, TValue> dict)
        {
            List<TKey> keyList = new List<TKey>(dict.Keys);
      
            return keyList[rdm.Next(keyList.Count)];
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="bytes"></param>
        /// <returns></returns>
        public static string FormatBytes(double bytes)
        {
            int i = 0;
            string[] suffixes = { "B", "KB", "MB", "GB", "TB" };
            for (; i < suffixes.Length && bytes >= 1024; i++, bytes /= 1024) { }

            return String.Format("{0:0.##} {1}", bytes, suffixes[i]);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        public static void ShuffleList<T>(IList<T> list)
        {
            int count = list.Count;
            for (int i = count - 1; i > 1; i--) {
                int rnd = rdm.Next(i + 1);

                T val = list[rnd];
                list[rnd] = list[i];
                list[i] = val;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="uri"></param>
        /// <returns></returns>
        public static bool IsValidUri(string uri)
        {
            bool uriResult = Uri.TryCreate(uri, UriKind.Absolute, out Uri tempUri);
            return uriResult && (tempUri.Scheme == Uri.UriSchemeHttp || tempUri.Scheme == Uri.UriSchemeHttps);
        }
    }
}
