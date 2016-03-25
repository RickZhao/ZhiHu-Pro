using System;
using System.Xml;

namespace Zhihu.Common.HtmlAgilityPack
{
    public class HtmlNameTable : XmlNameTable
    {
        #region Fields

        private NameTable _nametable = new NameTable();

        #endregion

        #region Public Methods

        public override String Add(String array)
        {
            return _nametable.Add(array);
        }

        public override String Add(char[] array, Int32 offset, Int32 length)
        {
            return _nametable.Add(array, offset, length);
        }

        public override String Get(String array)
        {
            return _nametable.Get(array);
        }

        public override String Get(char[] array, Int32 offset, Int32 length)
        {
            return _nametable.Get(array, offset, length);
        }

        #endregion

        #region Internal Methods

        internal String GetOrAdd(String array)
        {
            String s = Get(array);
            if (s == null)
            {
                return Add(array);
            }
            return s;
        }

        #endregion
    }
}
