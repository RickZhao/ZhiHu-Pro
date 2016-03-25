using System;
using System.IO;
using System.Threading.Tasks;

using Windows.Storage;
using SQLite;


namespace Zhihu.Common.Cache
{
    public class DbContext
    {
        private readonly object _sync = new object();
        private const String DbName = "Cache.db";

        #region Singleton

        private static DbContext _instance;

        private DbContext()
        {
            InitDb();
        }

        /// <summary>
        /// LazyLoad的单例模式
        /// </summary>
        public static DbContext Instance
        {
            get { return _instance ?? (_instance = new DbContext()); }
        }

        #endregion

        private async void InitDb()
        {
            var dbExist = true;
            try
            {
                var dbFile = await ApplicationData.Current.LocalFolder.GetFileAsync(DbName);
            }
            catch (FileNotFoundException)
            {
                dbExist = false;
            }

            var db = new SQLiteAsyncConnection(DbName);

            await db.CreateTableAsync<CachedJson>();
            await db.CreateTableAsync<CachedImage>();
            await db.CreateTableAsync<UploadedImage>();

            if (dbExist) return;
        }

        #region Image

        public async Task<CachedImage> CheckCachedImage(String imageUrl)
        {
            var conn = new SQLiteAsyncConnection(DbName);
            var query = conn.Table<CachedImage>().Where(row => row.ImageUrl == imageUrl);
           
            return await query.FirstOrDefaultAsync();
        }

        public async Task<Boolean> StoreCachedImage(String imageUrl)
        {
            var conn = new SQLiteAsyncConnection(DbName);

            var result = await conn.InsertAsync(new CachedImage()
            {
                ImageUrl = imageUrl,
                CachedDate = DateTime.Now,
            });

            return result > 0;
        }

        public async Task<Boolean> RemoveCachedImage(String imageUrl)
        {
            var conn = new SQLiteAsyncConnection(DbName);
            var query = await conn.Table<CachedImage>().Where(row => row.ImageUrl == imageUrl).FirstOrDefaultAsync();

            if (query == null) return false;

            var result = await conn.DeleteAsync(query);

            return result > 0;
        }

        #endregion

        #region Json

        public async Task<CachedJson> CheckCachedJson(String requestUrl)
        {
            var conn = new SQLiteAsyncConnection(DbName);
            var query = conn.Table<CachedJson>().Where(row => row.RequestUrl == requestUrl);

            return await query.FirstOrDefaultAsync();
        }

        public async Task<Boolean> StoreCachedJson(String requestUrl, String storeFile)
        {
            var conn = new SQLiteAsyncConnection(DbName);
            var query = conn.Table<CachedJson>().Where(row => row.RequestUrl == requestUrl);
           
            var cachedRecord = await query.FirstOrDefaultAsync();
          
            if (cachedRecord == null)
            {
                var result = await conn.InsertAsync(new CachedJson()
                {
                    RequestUrl = requestUrl,
                    StoreFile = storeFile,
                    CachedDate = DateTime.Now,
                });

                return result > 0;
            }
            else
            {
                cachedRecord.StoreFile = storeFile;
                cachedRecord.CachedDate = DateTime.Now;

                var result = await conn.UpdateAsync(cachedRecord);

                return result > 0;
            }
        }

        public async Task<Boolean> RemoveCachedJson(String requestUrl)
        {
            var conn = new SQLiteAsyncConnection(DbName);
            var query = conn.Table<CachedJson>().Where(row => row.RequestUrl == requestUrl).FirstOrDefaultAsync();

            if (query == null) return false;

            var result = await conn.DeleteAsync(query);

            return result > 0;
        }

        #endregion

        #region Uploaded image

        public async Task<UploadedImage> CheckUploadedImage(String fileName)
        {
            var conn = new SQLiteAsyncConnection(DbName);
            var query = conn.Table<UploadedImage>().Where(row => row.FileName == fileName);

            var first = await query.FirstOrDefaultAsync();

            return first;
        }

        public async Task<Boolean> StoreUploadedImage(String fileName, String jsonRslt)
        {
            var conn = new SQLiteAsyncConnection(DbName);
            var query = conn.Table<UploadedImage>().Where(row => row.FileName == fileName);

            var cachedRecord = await query.FirstOrDefaultAsync();

            if (cachedRecord == null)
            {
                var result = await conn.InsertAsync(new UploadedImage()
                {
                    FileName = fileName,
                    Json = jsonRslt,
                });

                return result > 0;
            }
            else
            {
                cachedRecord.FileName = fileName;
                cachedRecord.Json = jsonRslt;

                var result = await conn.UpdateAsync(cachedRecord);

                return result > 0;
            }
        }

        public async Task<Boolean> RemoveUploadedImage(String fileName)
        {
            var conn = new SQLiteAsyncConnection(DbName);
            var query = conn.Table<UploadedImage>().Where(row => row.FileName == fileName).FirstOrDefaultAsync();

            if (query == null) return false;

            var result = await conn.DeleteAsync(query);

            return result > 0;
        }

        #endregion

        public async Task<Boolean> ClearAll()
        {
            try
            {
                var conn = new SQLiteAsyncConnection(DbName);
                await conn.DropTableAsync<CachedJson>();
                await conn.DropTableAsync<CachedImage>();
                await conn.DropTableAsync<UploadedImage>();

                await conn.CreateTableAsync<CachedJson>();
                await conn.CreateTableAsync<CachedImage>();
                await conn.CreateTableAsync<UploadedImage>();

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}