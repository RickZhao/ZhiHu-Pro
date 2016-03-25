using System;
using System.Collections.Generic;


namespace Zhihu.Helper
{
    public sealed class ListViewHelper
    {
        #region Singleton

        private static ListViewHelper _instance;

        private ListViewHelper()
        {
        }
        
        public static ListViewHelper Instance
        {
            get { return _instance ?? (_instance = new ListViewHelper()); }
        }

        #endregion

        private Dictionary<String, String> _storage = new Dictionary<String, String>();

        public void PopRelativePos(Type pageType, Int32 pageId, String relativePos)
        {
            var key = String.Format("{0}_{1}", pageType.FullName.ToString(), pageId);

            if (_storage.ContainsKey(key))
            {
                _storage[key] = relativePos;
            }
            else
            {
                _storage.Add(key, relativePos);
            }
        }

        public String PushRelativePos(Type pageType, Int32 pageId)
        {
            var key = String.Format("{0}_{1}", pageType.FullName.ToString(), pageId);

            if (_storage.ContainsKey(key))
            {
                var relativePos = _storage[key];

                _storage.Remove(key);

                return relativePos;
            }
            else
            {
                return null;
            }
        }
    }
}
