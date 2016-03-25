using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;

using Windows.Storage;
using Windows.Storage.Streams;
using Windows.UI.Xaml.Media.Imaging;

using UmengSDK;
using Newtonsoft.Json;

using Zhihu.Common.Model;


namespace Zhihu.Common.Helper
{
    public sealed class CacheHelper
    {
        #region Singleton

        private static CacheHelper _instance;

        private CacheHelper()
        {
        }

        /// <summary>
        /// LazyLoad的单例模式
        /// </summary>
        public static CacheHelper Instance
        {
            get { return _instance ?? (_instance = new CacheHelper()); }
        }

        #endregion

        private readonly SemaphoreSlim _readSemaphore = new SemaphoreSlim(3, 3);
        private readonly SemaphoreSlim _downloadSemaphore = new SemaphoreSlim(3, 3);
        
        public async Task<byte[]> LoadImageAsync(String imgUri)
        {
            var baseUri = Utility.Instance.GetImageHost(imgUri);
            var requUri = Utility.Instance.GetImageRequest(imgUri);

            return await DownLoadImageAsync(String.Format("http://{0}", baseUri), requUri);
        } 

        public async Task<BitmapImage> LoadAndCacheImageAsync(String baseUri, String requestUri, String cachedFileName)
        {
            try
            {
                var imgBytes = await DownLoadImageAsync(baseUri, requestUri);

                if (imgBytes == null || imgBytes.Length == 0) return null;

                if (await LocalStoreHelper.Instance.SaveImage(cachedFileName, imgBytes))
                {
                    return await GetCachedImage(cachedFileName);
                }
                else
                {
                    return null;
                }
            }
            catch (Exception exception)
            {
                await UmengAnalytics.TrackException(exception, "LoadAndCacheImageAsync: " + exception.Message);

                return null;
            }
        }

        public async Task<BitmapImage> GetCachedImage(String cacheFileName)
        {
            await _readSemaphore.WaitAsync();
            
            var localFolder = ApplicationData.Current.LocalCacheFolder;

            var imgCacheFolder = await localFolder.CreateFolderAsync("Image_Cache", CreationCollisionOption.OpenIfExists);

            BitmapImage bitmapImage;
            try
            {
                var imgCacheFile = await imgCacheFolder.GetFileAsync(cacheFileName);

                var cachedImage = new BitmapImage();
                
                using (var readStream = await imgCacheFile.OpenAsync(FileAccessMode.Read))
                using (var randomStran = readStream.AsStream().AsRandomAccessStream())
                {
                    await cachedImage.SetSourceAsync(randomStran);
                }

                bitmapImage = cachedImage;
            }
            catch (FileNotFoundException notFoundException)
            {
                await UmengAnalytics.TrackException(notFoundException, "GetCachedImage: " + notFoundException.Message);
                return null;
            }
            catch (UnauthorizedAccessException unauthorizedAccessException)
            {
                await UmengAnalytics.TrackException(unauthorizedAccessException, "GetCachedImage: " + unauthorizedAccessException.Message);
                return null;
            }
            catch (Exception ex)
            {
                await UmengAnalytics.TrackException(ex, "GetCachedImage: " + ex.Message);
                return null;
            }

            _readSemaphore.Release();

            return bitmapImage;
        }

        public async Task<Byte[]> GetCachedImageBytes(String cacheFileName)
        {
            var localFolder = ApplicationData.Current.LocalCacheFolder;

            var imgCacheFolder = await localFolder.CreateFolderAsync("Image_Cache", CreationCollisionOption.OpenIfExists);

            try
            {
                var imgCacheFile = await imgCacheFolder.GetFileAsync(cacheFileName);

                using (var readStream = await imgCacheFile.OpenAsync(FileAccessMode.Read))
                using (var dataReader = new DataReader(readStream))
                {
                    var bytes = new Byte[readStream.Size];
                    await dataReader.LoadAsync((uint)readStream.Size);

                    dataReader.ReadBytes(bytes);

                    return bytes;
                }
            }
            catch (FileNotFoundException notFoundException)
            {
                await UmengAnalytics.TrackException(notFoundException, "GetCachedImage: " + notFoundException.Message);
                return null;
            }
            catch (UnauthorizedAccessException unauthorizedAccessException)
            {
                await UmengAnalytics.TrackException(unauthorizedAccessException, "GetCachedImage: " + unauthorizedAccessException.Message);
                return null;
            }
            catch (Exception ex)
            {
                await UmengAnalytics.TrackException(ex, "GetCachedImage: " + ex.Message);
                return null;
            }
        }

        public async Task<HttpResponse> UploadImageAsync(String baseUri, String requestUri, StreamContent content, String token)
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
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", token);
                }

                try
                {
                    using (var contentToSend = new MultipartFormDataContent())
                    {
                        contentToSend.Add(content);

                        var response = await client.PostAsync(requestUri, contentToSend);

                        if (response.IsSuccessStatusCode)
                        {
                            return new HttpResponse() { Json = await response.Content.ReadAsStringAsync() };
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

        private async Task<Byte[]> DownLoadImageAsync(String baseUri, String requestUri)
        {
            await _downloadSemaphore.WaitAsync();

            using (var client = new HttpClient())
            {
                #region Download and cache image in local.

                client.BaseAddress = new Uri(baseUri);

                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("*/*"));
                client.DefaultRequestHeaders.AcceptEncoding.Add(new StringWithQualityHeaderValue("gzip"));
                client.DefaultRequestHeaders.AcceptEncoding.Add(new StringWithQualityHeaderValue("deflate"));

                client.DefaultRequestHeaders.UserAgent.Clear();
                client.DefaultRequestHeaders.UserAgent.Add(new ProductInfoHeaderValue("Mozilla", "5.0"));
                client.DefaultRequestHeaders.UserAgent.Add(
                    new ProductInfoHeaderValue("(iPhone; CPU iPhone OS 8_2 like Mac OS X)"));
                client.DefaultRequestHeaders.UserAgent.Add(new ProductInfoHeaderValue("AppleWebKit", "600.1.4"));
                client.DefaultRequestHeaders.UserAgent.Add(new ProductInfoHeaderValue("(KHTML, like Gecko)"));
                client.DefaultRequestHeaders.UserAgent.Add(new ProductInfoHeaderValue("Mobile", "12D508"));

                try
                {
                    var response = await client.GetAsync(requestUri);

                    _downloadSemaphore.Release();

                    if (!response.IsSuccessStatusCode) return null;

                    var imgBytes = await response.Content.ReadAsByteArrayAsync();

                    if (imgBytes == null || imgBytes.Length == 0)
                    {
                        return null;
                    }
                    return imgBytes;
                }
                catch (Exception exception)
                {
                    await UmengAnalytics.TrackException(exception, "LoadAndCacheImageAsync: " + exception.Message);

                    return null;
                }

                #endregion
            }
        }
    }
}
