using System;
using System.Text;
using System.Web;
using System.IO;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using System.Net;

namespace bantam_php
{
    class WebHelper
    {

        /// <summary>
        /// 
        /// </summary>
        public static string g_GlobalDefaultUserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64; rv:64.0) Gecko/20100101 Firefox/64.0";

        private static readonly HttpClient client = new HttpClient(new HttpClientHandler() { UseCookies = false });

        public static async Task<string> WebRequest(string url, string code)
        {
            string requestArgsName = BantamMain.Hosts[url].RequestArgName;
            bool sendViaCookie = BantamMain.Hosts[url].SendDataViaCookie;

            try {
                HttpMethod method;
                if (sendViaCookie) {
                    method = HttpMethod.Get;
                } else {
                    method = HttpMethod.Post;
                }

                var request = new HttpRequestMessage(method, url);
                var httpClient = new HttpClient();

                if (!string.IsNullOrEmpty(code)) {
                    string minifiedCode = PhpHelper.minifyCode(code);
                    string encodedCode = HttpUtility.UrlEncode(minifiedCode);
                    var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(encodedCode);
                    string b64 = System.Convert.ToBase64String(plainTextBytes);

                    if (sendViaCookie) {
                        request.Headers.TryAddWithoutValidation("Cookie", requestArgsName + "=" + b64);
                    } else {
                        var values = new Dictionary<string, string>
                        {
                           { requestArgsName, b64 }
                        };

                        var content = new FormUrlEncodedContent(values);
                        request.Content = content;
                    }
                }

                var response = await client.SendAsync(request);
                var responseString = await response.Content.ReadAsStringAsync();
                return responseString;
            } catch (System.Net.Http.HttpRequestException e) {

            }
            return "";
        }
    }
}
