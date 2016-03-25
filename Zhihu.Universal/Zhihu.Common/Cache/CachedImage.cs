using System;
using SQLite;


namespace Zhihu.Common.Cache
{
    [Table("CachedImage")]
    public sealed class CachedImage
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        
        public String ImageUrl { get; set; }
      
        public DateTime CachedDate { get; set; }
    }
}
