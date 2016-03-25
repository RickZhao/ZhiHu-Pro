using System;

using SQLite;


namespace Zhihu.Common.Cache
{
    [Table("CachedJson")]
    public sealed class CachedJson
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        public String RequestUrl { get; set; }
        public String StoreFile { get; set; }
        public DateTime CachedDate { get; set; }
    }
}
