using System;

using SQLite;


namespace Zhihu.Common.Cache
{
    [Table("UploadedImage")]
    public sealed class UploadedImage
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        
        public String FileName { get; set; }
        public String Json { get; set; }
    }
}
