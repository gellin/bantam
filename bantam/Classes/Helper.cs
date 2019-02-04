using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace bantam.Classes
{
    static class Helper
    {
        /// <summary>
        /// Single shared random object for rng generation, for multi-threading / concurrent calls
        /// </summary>
        private static Random rdm = new Random();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="maxNumber"></param>
        /// <returns></returns>
        public static int RandomNumber(int maxNumber)
        {
            return rdm.Next(1, maxNumber);
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="length"></param>
        /// <returns></returns>
        public static string RandomNumberStringSetLength(int length)
        {
            StringBuilder result = new StringBuilder();

            for (int i = 0; i < length; i++) {
                result.Append(rdm.Next(10).ToString());
            }

            return result.ToString();
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="maxLength"></param>
        /// <returns></returns>
        public static string RandomNumberStringMaxLength(int maxLength)
        {
            StringBuilder result = new StringBuilder();
            int length = rdm.Next(1, maxLength);

            for (int i = 0; i < length; i++) {
                result.Append(rdm.Next(10).ToString());
            }

            return result.ToString();
        }

        /// <summary>
        /// Build a randon string of a charectors at a fixed length, with or without numbers and a few special chars
        /// </summary>
        /// <param name="length"></param>
        /// <param name="capitals"></param>
        /// <param name="numbers"></param>
        /// <param name="special"></param>
        /// <returns></returns>
        public static string RandomString(int length, bool capitals = true, bool numbers = false, bool special = false)
        {
            var charSet = "abcdefghijklmnopqrstuvwxyz";

            if (capitals) {
                charSet += "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            }

            if (numbers) {
                charSet += "0123456789";
            }
            
            if (special) {
                charSet += "!#$%&()*+,-.";
            }

            StringBuilder stringResult = new StringBuilder();

            for (int i = 0; i < length; i++) {
                stringResult.Append(charSet[rdm.Next(charSet.Length)]);
            }
            return stringResult.ToString();
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string EncodeBase64ToString(string str)
        {
            if (string.IsNullOrEmpty(str)) {
                return String.Empty;
            }

            string b64Code = Convert.ToBase64String(Encoding.UTF8.GetBytes(str));
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
                //todo global level 2 or 3 logging cfg
                cleanB64 = Regex.Replace(str, "[^a-zA-Z0-9+=/]", string.Empty);
            }

            try {
                return Encoding.UTF8.GetString(Convert.FromBase64String(cleanB64));
            } catch (Exception) {
                //todo
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
                //todo global slevel 2 or level 3 logging cfg
                cleanB64 = Regex.Replace(str, "[^a-zA-Z0-9+=/]", string.Empty);
            }

            try {
                var decbuff = Convert.FromBase64String(cleanB64);
                return decbuff;
            } catch (Exception) {
                //todo logging check
                MessageBox.Show(str, "Unable to decode base64!");
                return null;
            }
        }

        /// <summary>
        /// Removes tabs, new lines, dirty charectors and whitespace from a string
        /// </summary>
        /// <param name="code"></param>
        /// <returns>string clean and minified</returns>
        public static string MinifyCode(string code)
        {
            string result = string.Empty;
            result = Regex.Replace(code, @"\t|\n|\r", string.Empty);
            result = Regex.Replace(result, @"[^\u0000-\u007F]+", string.Empty);
            return Regex.Replace(result, @"\s+", " ");
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
        public static string FormatBytes(double bytesIn)
        {
            int i = 0;
            double bytesOut = bytesIn;
            string[] suffixes = { "B", "KB", "MB", "GB", "TB" };

            for (; i < suffixes.Length && bytesOut >= 1024; i++) {
                bytesOut /= 1024;
            }

            if (i < suffixes.Length) {
                return String.Format("{0:0.##} {1}", bytesOut, suffixes[i]);
            }
            return "0";
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
