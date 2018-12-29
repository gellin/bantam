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

        private static readonly HttpClient client = new HttpClient(new HttpClientHandler() { UseCookies = false });

        public static async Task<string> Request2(string url, string code)
        {
            string requestArgsName = BantamMain.Hosts[url].RequestArgName;

            //var values = new Dictionary<string, string>
            //{
            //   { requestArgsName, code }
            //};

            //var content = new FormUrlEncodedContent(values);

            //var response = await client.PostAsync(url, content);
            //var responseString = await response.Content.ReadAsStringAsync();
            //return responseString;

            try
            {
                HttpMethod method = HttpMethod.Get /*Put, Post, Delete, etc.*/;
                var request = new HttpRequestMessage(method, url);

                var httpClient = new HttpClient();

                if (!string.IsNullOrEmpty(code))
                {
                    string minifiedCode = PhpHelper.minifyCode(code);
                    string encodedCode = HttpUtility.UrlEncode(minifiedCode);
                    var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(encodedCode);
                    string b64 = System.Convert.ToBase64String(plainTextBytes);

                    request.Headers.TryAddWithoutValidation("Cookie", requestArgsName + "=" + b64);
                }

                //request.Content = content;

                var response = await client.SendAsync(request);
                var responseString = await response.Content.ReadAsStringAsync();
                string data = responseString;
                return data;
            }
            catch(System.Net.Http.HttpRequestException e)
            {

            }
            return "";
        }
    }
}
