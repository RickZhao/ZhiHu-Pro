using System;
using System.Diagnostics;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

using Newtonsoft.Json;

using Zhihu.Common.Cache;
using Zhihu.Common.Model;


namespace Zhihu.Common.Helper
{
    public sealed class HttpResponse
    {
        public String Json { get; set; }
        public String Error { get; set; }
    }

    public sealed class HttpUtility
    {
        public async Task<HttpResponse> GetAsync(String baseUri, String requestUri, String accessToken,
            Boolean autoCache = false)
        {
            Debug.WriteLine("request Url: ========================================{0}", requestUri);

            if (Utility.Instance.IsNetworkAvailable == false)
            {
                #region 无网使用缓存

                var cacheFileName = Utility.Instance.GetPlaintText(requestUri);

                var cachedRecord = await DbContext.Instance.CheckCachedJson(cacheFileName);

                if (cachedRecord != null)
                {
                    var cachedJson = await LocalStoreHelper.Instance.GetCachedJson(cachedRecord.StoreFile);

                    //Debug.WriteLine("Use Cached Json for request: {0}", requestUri);

                    return new HttpResponse() {Json = cachedJson};
                }
                else
                {
                    return new HttpResponse() {Error = "网络连接已中断"};
                }

                #endregion
            }
        
            var decompressHandler = new HttpClientHandler();

            if (decompressHandler.SupportsAutomaticDecompression)
            {
                decompressHandler.AutomaticDecompression = DecompressionMethods.GZip;
            }

            using (var client = new HttpClient(decompressHandler))
            {
                client.BaseAddress = new Uri(baseUri);

                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.AcceptEncoding.Add(new StringWithQualityHeaderValue("gzip"));
                client.DefaultRequestHeaders.AcceptEncoding.Add(new StringWithQualityHeaderValue("deflate"));

                client.DefaultRequestHeaders.AcceptLanguage.Add(new StringWithQualityHeaderValue("zh-cn"));

                client.DefaultRequestHeaders.UserAgent.Clear();
                client.DefaultRequestHeaders.UserAgent.Add(new ProductInfoHeaderValue("osee2unifiedRelease",
                    "358"));
                client.DefaultRequestHeaders.UserAgent.Add(new ProductInfoHeaderValue("CFNetwork",
                    "758.0.2"));
                client.DefaultRequestHeaders.UserAgent.Add(new ProductInfoHeaderValue("Darwin",
                    "15.0.0"));

                client.DefaultRequestHeaders.Add("X-APP-Build", "release");
                client.DefaultRequestHeaders.Add("X-APP-VERSION", "3.3");
                client.DefaultRequestHeaders.Add("x-app-za",
                    "OS=iOS&Release=9.0.2&Model=iPhone6,2&VersionName=3.3&VersionCode=358&Width=640&Height=1136");
             
                client.DefaultRequestHeaders.CacheControl = new CacheControlHeaderValue()
                {
                    NoCache = true,
                };

                client.DefaultRequestHeaders.ExpectContinue = true;

                if (false == String.IsNullOrEmpty(accessToken))
                {
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", accessToken);
                }

                try
                {
                    var response = await client.GetAsync(requestUri);

                    if (response.IsSuccessStatusCode)
                    {
                        var responseContent = await response.Content.ReadAsStringAsync();

                        if (autoCache)
                        {
                            #region 有网时缓存Json

                            try
                            {
                                var cacheFileName = Utility.Instance.GetPlaintText(requestUri);

                                // Check if there were any old cached Json of this request.
                                //var cachedJson = await CacheContext.Instance.CheckCachedJson(cacheFileName);
                                // If there were, delete the file and update the database to the new file.
                                //if (cachedJson != null)
                                //{
                                //    var removed = await LocalStoreHelper.Instance.RemoveCachedJson(cachedJson.StoreFile);
                                //}

                                var cacheJsonFile = Guid.NewGuid() + ".json";
                                var cached = await LocalStoreHelper.Instance.CacheJson(cacheJsonFile, responseContent);

                                if (cached)
                                {
                                    //Debug.WriteLine("Cached Json for request: {0} {1} {2}", requestUri, Environment.NewLine,
                                    //    responseContent.Length >= 50
                                    //        ? responseContent.Substring(0, 50) + "..."
                                    //        : responseContent);

                                    var result =
                                        await
                                            DbContext.Instance.StoreCachedJson(cacheFileName, cacheJsonFile);
                                }
                            }
                            catch (Exception exception)
                            {
                                throw exception;
                            }

                            #endregion
                        }

                        return new HttpResponse() { Json = responseContent };
                    }

                    var errorJson = await response.Content.ReadAsStringAsync();

                    if (String.IsNullOrEmpty(errorJson))
                    {
                        return new HttpResponse()
                        {
                            Error = response.ReasonPhrase
                        };
                    }
                    else
                    {
                        var errorObj = JsonConvert.DeserializeObject<Response>(errorJson);
                        return new HttpResponse()
                        {
                            Error = errorObj.Error.Message
                        };
                    }
                }
                catch (Exception exception)
                {
                    return new HttpResponse()
                    {
                        Error = exception.Message
                    };
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="baseUri"></param>
        /// <param name="requestUri"></param>
        /// <param name="content"></param>
        /// <param name="token">accessToken 或 ClientId, 可传String.Empty</param>
        /// <param name="isAccessToken">默认True, 表示为Bearer认证模式，False则为Oauth认证模式</param>
        /// <returns></returns>
        public async Task<HttpResponse> PostAsync(String baseUri, String requestUri, HttpContent content,
            String token, Boolean isAccessToken = true)
        {
            var decompressHandler = new HttpClientHandler();

            if (decompressHandler.SupportsAutomaticDecompression)
            {
                decompressHandler.AutomaticDecompression = DecompressionMethods.GZip;
            }

            using (var client = new HttpClient(decompressHandler))
            {
                client.BaseAddress = new Uri(baseUri);

                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.AcceptEncoding.Add(new StringWithQualityHeaderValue("gzip"));
                client.DefaultRequestHeaders.AcceptEncoding.Add(new StringWithQualityHeaderValue("deflate"));

                client.DefaultRequestHeaders.AcceptLanguage.Add(new StringWithQualityHeaderValue("zh-Hans", 1));
                client.DefaultRequestHeaders.AcceptLanguage.Add(new StringWithQualityHeaderValue("en", 0.9));

                client.DefaultRequestHeaders.UserAgent.Clear();
                client.DefaultRequestHeaders.UserAgent.Add(new ProductInfoHeaderValue("osee2unifiedRelease",
                    "358"));
                client.DefaultRequestHeaders.UserAgent.Add(new ProductInfoHeaderValue("CFNetwork",
                    "758.0.2"));
                client.DefaultRequestHeaders.UserAgent.Add(new ProductInfoHeaderValue("Darwin",
                    "15.0.0"));

                client.DefaultRequestHeaders.Add("X-APP-Build", "release");
                client.DefaultRequestHeaders.Add("X-APP-VERSION", "3.3");
                client.DefaultRequestHeaders.Add("x-app-za",
                    "OS=iOS&Release=9.0.2&Model=iPhone6,2&VersionName=3.3&VersionCode=358&Width=640&Height=1136");

                client.DefaultRequestHeaders.CacheControl = new CacheControlHeaderValue()
                {
                    NoCache = true,
                };

                client.DefaultRequestHeaders.ExpectContinue = true;

                if (false == String.IsNullOrEmpty(token))
                {
                    if (isAccessToken)
                        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer",
                            token);
                    else
                        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("oauth",
                            token);
                }

                try
                {
                    var response = await client.PostAsync(requestUri, content);

                    if (response.IsSuccessStatusCode)
                    {
                        return new HttpResponse() {Json = await response.Content.ReadAsStringAsync()};
                    }

                    var errorJson = await response.Content.ReadAsStringAsync();

                    if (String.IsNullOrEmpty(errorJson))
                    {
                        return new HttpResponse()
                        {
                            Error = response.ReasonPhrase
                        };
                    }
                    else
                    {
                        var errorObj = JsonConvert.DeserializeObject<Response>(errorJson);
                        return new HttpResponse()
                        {
                            Error = errorObj.Error.Message
                        };
                    }
                }
                catch (Exception exception)
                {
                    return new HttpResponse()
                    {
                        Error = exception.Message
                    };
                }
            }
        }

        public async Task<HttpResponse> DeleteAsync(String baseUri, String requestUri, String accessToken)
        {
            var decompressHandler = new HttpClientHandler();

            if (decompressHandler.SupportsAutomaticDecompression)
            {
                decompressHandler.AutomaticDecompression = DecompressionMethods.GZip;
            }

            using (var client = new HttpClient(decompressHandler))
            {
                client.BaseAddress = new Uri(baseUri);

                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.AcceptEncoding.Add(new StringWithQualityHeaderValue("gzip"));
                client.DefaultRequestHeaders.AcceptEncoding.Add(new StringWithQualityHeaderValue("deflate"));

                client.DefaultRequestHeaders.AcceptLanguage.Add(new StringWithQualityHeaderValue("zh-Hans", 1));
                client.DefaultRequestHeaders.AcceptLanguage.Add(new StringWithQualityHeaderValue("en", 0.9));

                client.DefaultRequestHeaders.UserAgent.Clear();
                client.DefaultRequestHeaders.UserAgent.Add(new ProductInfoHeaderValue("osee2unifiedRelease",
                    "358"));
                client.DefaultRequestHeaders.UserAgent.Add(new ProductInfoHeaderValue("CFNetwork",
                    "758.0.2"));
                client.DefaultRequestHeaders.UserAgent.Add(new ProductInfoHeaderValue("Darwin",
                    "15.0.0"));

                client.DefaultRequestHeaders.Add("X-APP-Build", "release");
                client.DefaultRequestHeaders.Add("X-APP-VERSION", "3.3");
                client.DefaultRequestHeaders.Add("x-app-za",
                    "OS=iOS&Release=9.0.2&Model=iPhone6,2&VersionName=3.3&VersionCode=358&Width=640&Height=1136");

                client.DefaultRequestHeaders.CacheControl = new CacheControlHeaderValue()
                {
                    NoCache = true,
                };

                client.DefaultRequestHeaders.ExpectContinue = true;

                if (false == String.IsNullOrEmpty(accessToken))
                {
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", accessToken);
                }

                try
                {
                    var response = await client.DeleteAsync(requestUri);

                    if (response.IsSuccessStatusCode)
                    {
                        return new HttpResponse() {Json = await response.Content.ReadAsStringAsync()};
                    }

                    var errorJson = await response.Content.ReadAsStringAsync();

                    if (String.IsNullOrEmpty(errorJson))
                    {
                        return new HttpResponse()
                        {
                            Error = response.ReasonPhrase
                        };
                    }
                    else
                    {
                        var errorObj = JsonConvert.DeserializeObject<Response>(errorJson);
                        return new HttpResponse()
                        {
                            Error = errorObj.Error.Message
                        };
                    }
                }
                catch (Exception exception)
                {
                    return new HttpResponse()
                    {
                        Error = exception.Message
                    };
                }
            }
        }

        public async Task<HttpResponse> PutAsync(String baseUri, String requestUri, String accessToken,
            HttpContent content)
        {
            var decompressHandler = new HttpClientHandler();

            if (decompressHandler.SupportsAutomaticDecompression)
            {
                decompressHandler.AutomaticDecompression = DecompressionMethods.GZip;
            }

            using (var client = new HttpClient(decompressHandler))
            {
                client.BaseAddress = new Uri(baseUri);

                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.AcceptEncoding.Add(new StringWithQualityHeaderValue("gzip"));
                client.DefaultRequestHeaders.AcceptEncoding.Add(new StringWithQualityHeaderValue("deflate"));

                client.DefaultRequestHeaders.AcceptLanguage.Add(new StringWithQualityHeaderValue("zh-Hans", 1));
                client.DefaultRequestHeaders.AcceptLanguage.Add(new StringWithQualityHeaderValue("en", 0.9));

                client.DefaultRequestHeaders.UserAgent.Clear();
                client.DefaultRequestHeaders.UserAgent.Add(new ProductInfoHeaderValue("osee2unifiedRelease",
                    "358"));
                client.DefaultRequestHeaders.UserAgent.Add(new ProductInfoHeaderValue("CFNetwork",
                    "758.0.2"));
                client.DefaultRequestHeaders.UserAgent.Add(new ProductInfoHeaderValue("Darwin",
                    "15.0.0")); 

                client.DefaultRequestHeaders.Add("X-APP-Build", "release");
                client.DefaultRequestHeaders.Add("X-APP-VERSION", "3.3");
                client.DefaultRequestHeaders.Add("x-app-za",
                    "OS=iOS&Release=9.0.2&Model=iPhone6,2&VersionName=3.3&VersionCode=358&Width=640&Height=1136");

                client.DefaultRequestHeaders.CacheControl = new CacheControlHeaderValue()
                {
                    NoCache = true,
                };

                client.DefaultRequestHeaders.ExpectContinue = true;

                if (false == String.IsNullOrEmpty(accessToken))
                {
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", accessToken);
                }

                try
                {
                    var response = await client.PutAsync(requestUri, content);

                    if (response.IsSuccessStatusCode)
                    {
                        return new HttpResponse() {Json = await response.Content.ReadAsStringAsync()};
                    }

                    var errorJson = await response.Content.ReadAsStringAsync();

                    if (String.IsNullOrEmpty(errorJson))
                    {
                        return new HttpResponse()
                        {
                            Error = response.ReasonPhrase
                        };
                    }
                    else
                    {
                        var errorObj = JsonConvert.DeserializeObject<Response>(errorJson);
                        return new HttpResponse()
                        {
                            Error = errorObj.Error.Message
                        };
                    }
                }
                catch (Exception exception)
                {
                    return new HttpResponse()
                    {
                        Error = exception.Message
                    };
                }
            }
        }
    }
}