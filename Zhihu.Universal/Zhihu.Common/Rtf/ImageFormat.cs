using System;


namespace Zhihu.Common.Rtf
{
    public sealed class ImageFormat
    {
        private ImageFormat(Guid guid)
        {
            GUID = guid;
        }

        private static ImageFormat _bmp = new ImageFormat(Guid.NewGuid());
        private static ImageFormat _emf = new ImageFormat(Guid.NewGuid());
        private static ImageFormat _exif = new ImageFormat(Guid.NewGuid());
        private static ImageFormat _gif = new ImageFormat(Guid.NewGuid());
        private static ImageFormat _icon = new ImageFormat(Guid.NewGuid());
        private static ImageFormat _jpeg = new ImageFormat(Guid.NewGuid());
        private static ImageFormat _memoryBmp = new ImageFormat(Guid.NewGuid());
        private static ImageFormat _png = new ImageFormat(Guid.NewGuid());
        private static ImageFormat _tiff = new ImageFormat(Guid.NewGuid());
        private static ImageFormat _wmf = new ImageFormat(Guid.NewGuid());

        public static ImageFormat Bmp { get { return _bmp; } }
        public static ImageFormat Emf { get { return _emf; } }
        public static ImageFormat Exif { get { return _exif; } }
        public static ImageFormat Gif { get { return _gif; } }
        public static ImageFormat Icon { get { return _icon; } }
        public static ImageFormat MemoryBmp { get { return _memoryBmp; } }
        public static ImageFormat Jpeg { get { return _jpeg; } }
        public static ImageFormat Png { get { return _png; } }
        public static ImageFormat Tiff { get { return _tiff; } }
        public static ImageFormat Wmf { get { return _wmf; } }

        public Guid GUID { get; private set; }

        public override bool Equals(object o)
        {
            if (o is ImageFormat)
            {
                var imgFormat = o as ImageFormat;

                return imgFormat.GUID == this.GUID;
            }
            return false;
        }

        public override int GetHashCode()
        {
            return this.GUID.GetHashCode();
        }

        public override string ToString()
        {
            if (this.Equals(ImageFormat.Bmp))
            {
                return "Bmp";
            }
            else if (this.Equals(ImageFormat.Emf))
            {
                return "Emf";
            }
            else if (this.Equals(ImageFormat.Exif))
            {
                return "Exif";
            }
            else if (this.Equals(ImageFormat.Gif))
            {
                return "Gif";
            }
            else if (this.Equals(ImageFormat.Icon))
            {
                return "Icon";
            }
            else if (this.Equals(ImageFormat.MemoryBmp))
            {
                return "MemoryBmp";
            }
            else if (this.Equals(ImageFormat.Jpeg))
            {
                return "Jpeg";
            }
            else if (this.Equals(ImageFormat.Png))
            {
                return "Png";
            }
            else if (this.Equals(ImageFormat.Tiff))
            {
                return "Tiff";
            }
            else if (this.Equals(ImageFormat.Wmf))
            {
                return "Wmf";
            }

            return String.Empty;
        }
    }
}
