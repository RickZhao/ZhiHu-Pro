using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

using Windows.UI.Xaml.Media.Imaging;
using Windows.Storage;
using Windows.Storage.Streams;
using Windows.Storage.Pickers;

using UmengSDK;


namespace Zhihu.Common.Helper
{
    public sealed class LocalStoreHelper
    {
        #region Singleton

        private static LocalStoreHelper _instance;

        private LocalStoreHelper()
        {
        }

        /// <summary>
        /// LazyLoad的单例模式
        /// </summary>
        public static LocalStoreHelper Instance
        {
            get { return _instance ?? (_instance = new LocalStoreHelper()); }
        }

        #endregion

        public async Task<StorageFile> PickImage()
        {
            var filePicker = new FileOpenPicker()
            {
                ViewMode = PickerViewMode.Thumbnail,
                SuggestedStartLocation = PickerLocationId.PicturesLibrary,
            };
            filePicker.FileTypeFilter.Add(".png");
            filePicker.FileTypeFilter.Add(".jpg");
            filePicker.FileTypeFilter.Add(".jpeg");

            var file = await filePicker.PickSingleFileAsync();
            return file;
        }

        #region Image Cache

        public async Task<Boolean> HasCachedImage(String cacheFileName)
        {
            var localFolder = ApplicationData.Current.LocalCacheFolder;

            var imgCacheFolder =
                await localFolder.CreateFolderAsync("Image_Cache", CreationCollisionOption.OpenIfExists);

            try
            {
                var imgFile = await imgCacheFolder.GetFileAsync(cacheFileName);
                return true;
            }
            catch (FileNotFoundException exception)
            {
                await UmengAnalytics.TrackException(exception, "HasCachedImage: " + exception.Message);
                return false;
            }
        }

        public async Task<Boolean> SaveIntoPictureLib(byte[] imgBytes)
        {
            if (imgBytes == null || imgBytes.Length == 0) return false;

            var imgFileName = String.Format("ZhiHu_Pro_{0}.jpg", DateTime.Now.ToString("yyyyMMdd_HHmmss"));

            var savedPicturesFolder = KnownFolders.SavedPictures;

            var imgCacheFile =
                await savedPicturesFolder.CreateFileAsync(imgFileName, CreationCollisionOption.ReplaceExisting);

            try
            {
                using (var fileStream = await imgCacheFile.OpenAsync(FileAccessMode.ReadWrite))
                using (var fileWriter = new DataWriter(fileStream))
                {
                    fileWriter.WriteBytes(imgBytes);
                    await fileWriter.StoreAsync();
                }
            }
            catch (Exception exception)
            {
                await UmengAnalytics.TrackException(exception, "CacheImage: " + exception.Message);
                return false;
            }
            return true;
        }

        public async Task<Boolean> SaveImage(String cacheFileName, byte[] imgBytes)
        {
            var localFolder = ApplicationData.Current.LocalCacheFolder;

            var imgCacheFolder =
                await localFolder.CreateFolderAsync("Image_Cache", CreationCollisionOption.OpenIfExists);

            var imgCacheFile =
                await imgCacheFolder.CreateFileAsync(cacheFileName, CreationCollisionOption.ReplaceExisting);

            try
            {
                using (var fileStream = await imgCacheFile.OpenAsync(FileAccessMode.ReadWrite))
                using (var fileWriter = new DataWriter(fileStream))
                {
                    fileWriter.WriteBytes(imgBytes);
                    await fileWriter.StoreAsync();
                }
            }
            catch (Exception exception)
            {
                await UmengAnalytics.TrackException(exception, "CacheImage: " + exception.Message);
                return false;
            }
            return true;
        }

        #endregion

        #region Json Cache

        public async Task<Boolean> HasCachedJson(String cacheFileName)
        {
            var localFolder = ApplicationData.Current.LocalCacheFolder;

            var imgCacheFolder =
                await localFolder.CreateFolderAsync("Json_Cache", CreationCollisionOption.OpenIfExists);

            try
            {
                var jsonFile = await imgCacheFolder.GetFileAsync(cacheFileName);
                return true;
            }
            catch (FileNotFoundException e)
            {
                await UmengAnalytics.TrackException(e, e.Message);
                return false;
            }
        }

        public async Task<Boolean> CacheJson(String cacheFileName, String fileContent)
        {
            var localFolder = ApplicationData.Current.LocalCacheFolder;

            var jsonCacheFolder =
                await localFolder.CreateFolderAsync("Json_Cache", CreationCollisionOption.OpenIfExists);

            var jsonCacheFile =
                await jsonCacheFolder.CreateFileAsync(cacheFileName, CreationCollisionOption.ReplaceExisting);

            using (var targetStream = await jsonCacheFile.OpenStreamForWriteAsync())
            {
                var bytes = Encoding.UTF8.GetBytes(fileContent);
                await targetStream.WriteAsync(bytes, 0, bytes.Length);
            }

            return true;
        }

        public async Task<String> GetCachedJson(String cacheFileName)
        {
            var localFolder = ApplicationData.Current.LocalCacheFolder;

            var jsonCacheFolder =
                await localFolder.CreateFolderAsync("Json_Cache", CreationCollisionOption.OpenIfExists);

            try
            {
                var jsonCacheFile = await jsonCacheFolder.GetFileAsync(cacheFileName);

                using (var readStream = await jsonCacheFile.OpenStreamForReadAsync())
                using (var streamReader = new StreamReader(readStream))
                {
                    var fileContent = await streamReader.ReadToEndAsync();
                    return fileContent;
                }
            }
            catch (FileNotFoundException)
            {
                return null;
            }
            catch (Exception ex)
            {
                await UmengAnalytics.TrackException(ex, ex.Message);
                return null;
            }

            //if (bitmapImage == null)
            //{
            //    var imgCacheFile = await imgCacheFolder.GetFileAsync(cacheFileName);
            //    await imgCacheFile.DeleteAsync(StorageDeleteOption.PermanentDelete);
            //}

            //return bitmapImage;
        }

        public async Task<Boolean> RemoveCachedJson(String cacheFileName)
        {
            var localFolder = ApplicationData.Current.LocalCacheFolder;

            var jsonCacheFolder =
                await localFolder.CreateFolderAsync("Json_Cache", CreationCollisionOption.OpenIfExists);

            try
            {
                var jsonCacheFile = await jsonCacheFolder.GetFileAsync(cacheFileName);
                await jsonCacheFile.DeleteAsync(StorageDeleteOption.PermanentDelete);
            }
            catch (FileNotFoundException)
            {
                return false;
            }
            catch (Exception ex)
            {
                await UmengAnalytics.TrackException(ex, ex.Message);
                return false;
            }

            return true;
        }

        public async Task<Boolean> ClearCache()
        {
            try
            {
                var localFolder = ApplicationData.Current.LocalCacheFolder;

                var imgCacheFolder =
                    await localFolder.CreateFolderAsync("Image_Cache", CreationCollisionOption.OpenIfExists);

                await imgCacheFolder.DeleteAsync(StorageDeleteOption.PermanentDelete);

                var jsonCacheFolder =
                    await localFolder.CreateFolderAsync("Json_Cache", CreationCollisionOption.OpenIfExists);

                await jsonCacheFolder.DeleteAsync(StorageDeleteOption.PermanentDelete);

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        #endregion
    }
}
